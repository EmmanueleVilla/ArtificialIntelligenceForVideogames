using System;
using System.Collections.Generic;
using System.Linq;
using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core.Battle;
using Logic.Core.Battle.ActionBuilders;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Spells;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
using Logic.Core.Creatures.Abilities.Spells;
using Logic.Core.Creatures.Classes;
using Logic.Core.Dice;
using Logic.Core.Graph;
using Logic.Core.Movements;

namespace Logic.Core
{
    public class DndBattle: IDndBattle
    {
        private IMap map;
        private int turnIndex = 0;

        private List<ICreature> initiativeOrder = new List<ICreature>();

        private UniformCostSearch Search;
        private IDiceRoller Roller;
        private ILogger Logger;
        private IActionBuildersWrapper ActionBuildersWrapper;

        List<IAvailableAction> _cachedActions = new List<IAvailableAction>();
        List<MemoryEdge> _reachableCellCache = new List<MemoryEdge>();

        public IDndBattle Copy()
        {
            var copy = new DndBattle(Roller, Search, ActionBuildersWrapper, Logger);
            copy.turnIndex = turnIndex;
            //copy.initiativeOrder = initiativeOrder.Select(x => x.Copy()).ToList();
            return copy;
        }

        public DndBattle(IDiceRoller roller = null, UniformCostSearch search = null, IActionBuildersWrapper actionBuildersWrapper = null, ILogger logger = null) {
            Search = search ?? DndModule.Get<UniformCostSearch>();
            Roller = roller ?? DndModule.Get<IDiceRoller>();
            Logger = logger ?? DndModule.Get<ILogger>();
            ActionBuildersWrapper = actionBuildersWrapper ?? DndModule.Get<IActionBuildersWrapper>();
        }

        public List<ICreature> Init(IMap map)
        {
            this.map = map;
            foreach (var creature in map.Creatures)
            {
                initiativeOrder.Add(creature);
            }
            initiativeOrder.Sort(new CreatureInitiativeComparer());
            return initiativeOrder;
        }

        public ICreature GetCreatureInTurn()
        {
            return initiativeOrder[turnIndex];
        }

        public void BuildAvailableActions(ICreature creature = null)
        {
            creature = creature ?? GetCreatureInTurn();

            _cachedActions = new List<IAvailableAction>();

            ActionBuildersWrapper.ActionBuilders.ForEach(x => _cachedActions.AddRange(x.Build(this, map, creature)));

            _cachedActions.Add(new EndTurnAction());
        }

        public List<IAvailableAction> GetAvailableActions(ICreature creature = null)
        {
            return _cachedActions;
        }

        public void NextTurn()
        {
            _reachableCellCache.Clear();
            _cachedActions.Clear();

            turnIndex++;
            if(turnIndex >= map.Creatures.Count)
            {
                foreach (var creature in map.Creatures)
                {
                    creature.ResetTurn();
                }
                turnIndex = 0;
            }

            var creatureInTurn = GetCreatureInTurn();
            foreach (var creature in map.Creatures)
            {
                creature.TemporaryEffectsList = creature.TemporaryEffectsList.Select( x =>
                {
                    var remainingTurns = x.Item2;
                    if (x.Item1 == creatureInTurn)
                    {
                        remainingTurns--;
                    }
                    var newTuple = new Tuple<ICreature, int, TemporaryEffects>(x.Item1, remainingTurns, x.Item3);
                    
                    return newTuple;
                }).Where(x => x.Item2 > 0).ToList();
            }
        }
        

        public void CalculateReachableCells(ICreature creature = null)
        {
            creature = creature ?? GetCreatureInTurn();
            _reachableCellCache = Search.Search(map.GetCellOccupiedBy(creature), map).Where(x => x.Speed > 0 && x.CanEndMovementHere).ToList();
        }

        public List<MemoryEdge> GetReachableCells()
        {
            return _reachableCellCache;
        }

        public List<CellInfo> GetPathTo(MemoryEdge edge)
        {
            return _reachableCellCache.FirstOrDefault(e => e.Destination.Equals(edge.Destination) && e.Speed == edge.Speed && e.Damage == edge.Damage && e.CanEndMovementHere == edge.CanEndMovementHere).Start;
        }

        public List<Edge> GetPathsTo(List<Edge> searched, int x, int y)
        {
            return searched.Where(edge => edge.Destination.X == x && edge.Destination.Y == y).ToList();
        }

        public IEnumerable<GameEvent> MoveTo(MemoryEdge end)
        {
            var creature = GetCreatureInTurn();
            creature.RemainingMovement = creature.RemainingMovement.Select(x =>
           {
                   return new Speed(x.Item1, x.Item2 - end.Speed);
           }).ToList();
            map.MoveCreatureTo(creature, end);
            foreach(var e in end.Events)
            {
                if(e.Type == GameEvent.Types.Falling)
                {
                    e.Damage = Roller.Roll(RollTypes.Normal, e.FallingHeight, 6, 0);
                    creature.CurrentHitPoints -= e.Damage;
                }
                yield return e;
            }
        }

        public List<GameEvent> Attack(ConfirmAttackAction confirmAttackAction)
        {
            var list = new List<GameEvent>();
            var hasAdvantage = false;
            var hasDisadvantage = false;
            if (confirmAttackAction.TargetCreature.TemporaryEffectsList.Any(x => x.Item3 == TemporaryEffects.DisadvantageToSufferedAttacks))
            {
                hasDisadvantage = true;
            }
            if (confirmAttackAction.AttackingCreature.TemporaryEffectsList.Any(x => x.Item3 == TemporaryEffects.AdvantageToAttacks))
            {
                hasAdvantage = true;
            }
            
            if (hasDisadvantage && hasAdvantage)
            {
                hasAdvantage = false;
                hasDisadvantage = false;
            }

            var rollType = RollTypes.Normal;
            if(hasDisadvantage)
            {
                rollType = RollTypes.Disadvantage;
            }
            if(hasAdvantage)
            {
                rollType = RollTypes.Advantage;
            }
            var toHit = Roller.Roll(rollType, 1, 20, confirmAttackAction.Attack.ToHit);
            Logger.WriteLine(string.Format("Roll Type {0}, rolled {1} to hit", rollType, toHit));

            var isCritical = toHit >= confirmAttackAction.AttackingCreature.CriticalThreshold;
            if(toHit >= confirmAttackAction.TargetCreature.ArmorClass || isCritical)
            {
                var totalDamage = 0;
                foreach(var damage in confirmAttackAction.Attack.Damage)
                {
                    totalDamage += Roller.Roll(RollTypes.Normal, isCritical ? 2 : 1 * damage.NumberOfDice, damage.DiceFaces, damage.Modifier);
                }
                //TODO apply other damage effects
                Logger.WriteLine(string.Format("Inflicted {0} damage to {1}", totalDamage, confirmAttackAction.TargetCreature.GetType().ToString().Split('.').Last()));

                confirmAttackAction.TargetCreature.TemporaryHitPoints -= totalDamage;

                if (confirmAttackAction.TargetCreature.TemporaryHitPoints < 0)
                {
                    confirmAttackAction.TargetCreature.CurrentHitPoints += confirmAttackAction.TargetCreature.TemporaryHitPoints;
                    confirmAttackAction.TargetCreature.TemporaryHitPoints = 0;
                }

                list.Add(new GameEvent
                {
                    Type = GameEvent.Types.Attacks,
                    Attacker = confirmAttackAction.AttackingCreature,
                    Attacked = confirmAttackAction.TargetCreature,
                    Attack = confirmAttackAction.Attack
                });

                //TODO kill creature if hp < 0

            } else
            {
                Logger.WriteLine("Not hit");
                list.Add(new GameEvent
                {
                    Type = GameEvent.Types.AttackMissed,
                    Attacker = confirmAttackAction.AttackingCreature,
                    Attacked = confirmAttackAction.TargetCreature,
                    Attack = confirmAttackAction.Attack
                });
            }

            if (confirmAttackAction.ActionEconomy == BattleActions.Action) {
                confirmAttackAction.AttackingCreature.RemainingAttacksPerAction--;
                confirmAttackAction.AttackingCreature.ActionUsedToAttack = true;
                if(confirmAttackAction.AttackingCreature is IMartialArts && confirmAttackAction.Attack.Name.ToLower().Contains("quarterstaff"))
                {
                    (confirmAttackAction.AttackingCreature as IMartialArts).BonusAttackTriggered = true;
                }
            }
            if (confirmAttackAction.ActionEconomy == BattleActions.BonusAction) {
                confirmAttackAction.AttackingCreature.RemainingAttacksPerBonusAction--;
                confirmAttackAction.AttackingCreature.BonusActionUsedToAttack = true;
            }

            confirmAttackAction.AttackingCreature.LastAttackUsed += confirmAttackAction.Attack.Name;

            return list;
        }

        public List<GameEvent> UseAbility(IAvailableAction availableAction)
        {
            var creature = GetCreatureInTurn();
            switch (availableAction.ActionType)
            {
                case ActionsTypes.FlurryOfBlows:
                    creature.RemainingAttacksPerBonusAction++;
                    (creature as IFlurryOfBlows).FlurryOfBlowsUsed = true;
                    (creature as IKiPointsOwner).RemainingKiPoints--;
                    DndModule.Get<ILogger>().WriteLine(string.Format("Used Flurry of Blows, +1 attack in the bonus action, -1 Ki Point"));
                    break;
                case ActionsTypes.Dash:
                    var action = availableAction as DashAction;
                    creature.RemainingMovement = creature.RemainingMovement.Select(mov =>
                    {
                        var baseSpeed = creature.Movements.First(x => x.Item1 == mov.Item1);
                        return new Speed(mov.Item1, mov.Item2 + baseSpeed.Item2);
                    }
                    ).ToList();
                    if(action.ActionEconomy == BattleActions.Action)
                    {
                        DndModule.Get<ILogger>().WriteLine(string.Format("Used Dash, doubled movement"));
                        creature.ActionUsedNotToAttack = true;
                    }
                    if (action.ActionEconomy == BattleActions.BonusAction)
                    {
                        DndModule.Get<ILogger>().WriteLine(string.Format("Used Dash, doubled movement, -1 Ki Point"));
                        creature.BonusActionUsedNotToAttack = true;
                        var monk = creature as IKiPointsOwner;
                        if (monk != null)
                        {
                            monk.RemainingKiPoints--;
                        }
                    }
                    break;
                case ActionsTypes.Disengage:
                    creature.Disangaged = true;
                    var disAction = availableAction as DisengageAction;
                    if (disAction.ActionEconomy == BattleActions.Action)
                    {
                        DndModule.Get<ILogger>().WriteLine(string.Format("Used Disengage"));
                        creature.ActionUsedNotToAttack = true;
                    }
                    if (disAction.ActionEconomy == BattleActions.BonusAction)
                    {
                        DndModule.Get<ILogger>().WriteLine(string.Format("Used Disengage, -1 Ki Point"));
                        creature.BonusActionUsedNotToAttack = true;
                        var monk = creature as IKiPointsOwner;
                        if(monk != null)
                        {
                            monk.RemainingKiPoints--;
                        }
                    }
                    break;
                case ActionsTypes.PatientDefence:
                    creature.TemporaryEffectsList.Add(new Tuple<ICreature, int, TemporaryEffects>(creature, 1, TemporaryEffects.DisadvantageToSufferedAttacks));
                    var defAction = availableAction as PatientDefenseAction;
                    DndModule.Get<ILogger>().WriteLine(string.Format("Used Patient Defense,  Disadvantage to suffered attacks until your next turn, -1 Ki Point"));
                    creature.BonusActionUsedNotToAttack = true;
                    var monkDef = creature as IKiPointsOwner;
                    if (monkDef != null)
                    {
                        monkDef.RemainingKiPoints--;
                    }
                    break;
                case ActionsTypes.Dodge:
                    creature.TemporaryEffectsList.Add(new Tuple<ICreature, int, TemporaryEffects>(creature, 1, TemporaryEffects.DisadvantageToSufferedAttacks));
                    creature.ActionUsedNotToAttack = true;
                    break;
                case ActionsTypes.FightingSpirit:
                    creature.TemporaryEffectsList.Add(new Tuple<ICreature, int, TemporaryEffects>(creature, 1, TemporaryEffects.AdvantageToAttacks));
                    creature.BonusActionUsedNotToAttack = true;
                    var spirit = creature as IFightingSpirit;
                    if(spirit != null)
                    {
                        spirit.FightingSpiritRemaining--;
                        creature.TemporaryHitPoints += spirit.FightingSpiritTemporaryHitPoints;
                    }
                    DndModule.Get<ILogger>().WriteLine(string.Format("Used Fightning Spirit,  Advantage to attacks until your next turn, +{0} temporary hit points", spirit.FightingSpiritTemporaryHitPoints));
                    break;
                case ActionsTypes.SecondWind:
                    creature.BonusActionUsedNotToAttack = true;
                    var secondWind = creature as ISecondWind;
                    if (secondWind != null) {
                        var heal = Roller.Roll(RollTypes.Normal, 1, 10, secondWind.SecondWindTemporaryHitPoints);
                        creature.CurrentHitPoints += heal;
                        creature.CurrentHitPoints = Math.Min(creature.CurrentHitPoints, creature.HitPoints);
                        secondWind.SecondWindRemaining--;
                    }
                    break;
            }

            return new List<GameEvent> { 
                new GameEvent
                {
                    Type = GameEvent.Types.SelfAbility
                }
            };
        }

        public List<GameEvent> Spell(ConfirmSpellAction confirmSpellAction)
        {
            var list = new List<GameEvent>();

            if(confirmSpellAction.Spell is FalseLife)
            {
                var temporary = Roller.Roll(RollTypes.Normal, 1, 4, 4);
                var creature = map.GetOccupantCreature(confirmSpellAction.Target.Y, confirmSpellAction.Target.X);
                creature.TemporaryHitPoints += temporary;
                GetCreatureInTurn().ActionUsedNotToAttack = true;
                DndModule.Get<ILogger>().WriteLine(string.Format("Obtained {0} temp hit points", temporary));
            }

            if (confirmSpellAction.Spell is MagicMissile)
            {
                var damage = Roller.Roll(RollTypes.Normal, 1, 4, 1) * 3;
                var creature = map.GetOccupantCreature(confirmSpellAction.Target.Y, confirmSpellAction.Target.X);
                creature.CurrentHitPoints -= damage;
                GetCreatureInTurn().ActionUsedNotToAttack = true;
                DndModule.Get<ILogger>().WriteLine(string.Format("Inflicted {0} damage to {1}", damage, creature.GetType().ToString().Split('.').Last()));
            }

            if (confirmSpellAction.Spell is RayOfFrost)
            {
                var damage = Roller.Roll(RollTypes.Normal, 1, 8, 0);
                var creature = map.GetOccupantCreature(confirmSpellAction.Target.Y, confirmSpellAction.Target.X);
                creature.CurrentHitPoints -= damage;
                creature.TemporaryEffectsList.Add(new Tuple<ICreature, int, TemporaryEffects>(
                    confirmSpellAction.Caster,
                    1,
                    TemporaryEffects.SpeedReducedByTwo
                    ));
                GetCreatureInTurn().ActionUsedNotToAttack = true;
                DndModule.Get<ILogger>().WriteLine(string.Format("Inflicted {0} damage to {1} and reduced their speed by 2 squares", damage, creature.GetType().ToString().Split('.').Last()));
            }

            return list;
        }
    }
}
