using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core.Battle;
using Logic.Core.Battle.ActionBuilders;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Movement;
using Logic.Core.Battle.Actions.Spells;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
using Logic.Core.Creatures.Abilities.Spells;
using Logic.Core.Creatures.Classes;
using Logic.Core.Dice;
using Logic.Core.GOAP.Actions;
using Logic.Core.Graph;
using Logic.Core.Movements;
using Newtonsoft.Json;

namespace Logic.Core
{
    public class DndBattle: IDndBattle
    {
        public IMap map;
        public int turnIndex = 0;

        public List<int> initiativeOrder = new List<int>();

        public UniformCostSearch Search;
        public IDiceRoller Roller;
        public ILogger Logger;
        public IActionBuildersWrapper ActionBuildersWrapper;
        public IActionBuildersCleanup ActionBuildersCleanup;
        public IActionSequenceBuilder ActionSequenceBuilder;

        public List<IAvailableAction> _cachedActions = new List<IAvailableAction>();
        public List<MemoryEdge> _reachableCellCache = null;

        public IMap Map => map;

        public IDndBattle Copy()
        {
            var battle = new DndBattle(Roller, Search, ActionBuildersWrapper, ActionSequenceBuilder, ActionBuildersCleanup, Logger);
            battle.initiativeOrder = new List<int>(initiativeOrder);
            battle.turnIndex = turnIndex;
            battle.map = map.Copy();
            battle._reachableCellCache = new List<MemoryEdge>(_reachableCellCache);
            return battle;
        }

        public DndBattle(
            IDiceRoller roller = null,
            UniformCostSearch search = null,
            IActionBuildersWrapper actionBuildersWrapper = null,
            IActionSequenceBuilder actionSequenceBuilder = null,
            IActionBuildersCleanup actionBuildersCleanup = null,
            ILogger logger = null) {
            Search = search ?? DndModule.Get<UniformCostSearch>();
            Roller = roller ?? DndModule.Get<IDiceRoller>();
            Logger = logger ?? DndModule.Get<ILogger>();
            ActionBuildersCleanup = actionBuildersCleanup ?? DndModule.Get<IActionBuildersCleanup>();
            ActionSequenceBuilder = actionSequenceBuilder ?? DndModule.Get<IActionSequenceBuilder>();
            ActionBuildersWrapper = actionBuildersWrapper ?? DndModule.Get<IActionBuildersWrapper>();
        }

        public List<int> Init(IMap map)
        {
            File.WriteAllText("log.txt", "");
            this.map = map;
            var temp = new List<ICreature>();
            foreach (var creature in map.Creatures.Values)
            {
                temp.Add(creature);
            }
            temp.Sort(new CreatureInitiativeComparer());
            var ids = temp.Select(x => x.Id).ToList();
            initiativeOrder = new List<int>(ids);
            return ids;
        }

        public ICreature GetCreatureInTurn()
        {
            try
            {
                return map.Creatures[initiativeOrder[turnIndex]];
            } catch(Exception e)
            {
                return null;
            }
        }

        public void BuildAvailableActions(ICreature creature = null, bool isAI = false)
        {
            creature = creature ?? GetCreatureInTurn();

            _cachedActions = new List<IAvailableAction>();

            ActionBuildersWrapper.ActionBuilders.ForEach(x => _cachedActions.AddRange(x.Build(this, creature)));

            if(isAI)
            {
                _cachedActions = ActionBuildersCleanup.Cleanup(creature, _cachedActions);
            }

            _cachedActions.Add(new EndTurnAction());
        }

        public List<IAvailableAction> GetAvailableActions()
        {
            return _cachedActions;
        }

        public List<GameEvent> Events { get; private set; } = new List<GameEvent>();
        public void PlayTurn()
        {
            Events = new List<GameEvent>();
            var actions = ActionSequenceBuilder.GetBestActions(this);
            PlayTurnInternal(actions);
        }
        private void PlayTurnInternal(ActionList actions)
        {
            foreach (var v in actions.actions)
            {
                DndModule.Get<ILogger>().WriteLine("Executing action " + v.GetType().Name);
                var temp = new List<GameEvent>();
                if (v is ConfirmMovementAction)
                {
                    var moveAction = v as ConfirmMovementAction;
                    temp.AddRange(MoveTo(moveAction.MemoryEdge));
                }
                else if (v is ConfirmAttackAction)
                {
                    var attackAction = v as ConfirmAttackAction;
                    temp.AddRange(Attack(attackAction));
                }
                else if (v is ConfirmSpellAction)
                {
                    var spellAction = v as ConfirmSpellAction;
                    temp.AddRange(Spell(spellAction));
                }
                else
                {
                    temp.AddRange(UseAbility(v));
                }
                DndModule.Get<ILogger>().WriteLine("-- Added events:\n" + string.Join("\n", temp));
                Events.AddRange(temp);
                if(RemoveDeaths())
                {
                    actions = ActionSequenceBuilder.GetBestActions(this);
                    PlayTurnInternal(actions);
                    break;
                }
            }
        }

        private bool RemoveDeaths()
        {
            var deads = map.Creatures.Where(x => x.Value.CurrentHitPoints <= 0).Select(x => x.Key).ToList();
            foreach (var dead in deads)
            {
                map.RemoveCreature(GetCreatureById(dead));
            }
            return deads.Count > 0;
        }

        public void NextTurn()
        {
            _reachableCellCache = null;
            _cachedActions.Clear();

            turnIndex++;
            if(turnIndex >= initiativeOrder.Count)
            {
                foreach (var creature in map.Creatures)
                {
                    creature.Value.ResetTurn();
                }
                turnIndex = 0;
            }

            var creatureInTurn = GetCreatureInTurn();
            
            if(creatureInTurn == null)
            {
                NextTurn();
                return;
            }

            foreach (var creature in map.Creatures)
            {
                creature.Value.TemporaryEffectsList = creature.Value.TemporaryEffectsList.Select( x =>
                {
                    var remainingTurns = x.Item2;
                    if (x.Item1 == creatureInTurn.Id)
                    {
                        remainingTurns--;
                    }
                    var newTuple = new Tuple<int, int, TemporaryEffects>(x.Item1, remainingTurns, x.Item3);
                    
                    return newTuple;
                }).Where(x => x.Item2 > 0).ToList();
            }
            RemoveDeaths();

            if (!map.Creatures.ContainsKey(creatureInTurn.Id))
            {
                NextTurn();
            }
        }

        public void CalculateReachableCells(ICreature creature = null)
        {
            creature = creature ?? GetCreatureInTurn();
            _reachableCellCache = Search.Search(map.GetCellOccupiedBy(creature), map).Where(x => x.Speed > 0 && x.CanEndMovementHere).ToList();
        }

        public List<MemoryEdge> GetReachableCells()
        {
            if(_reachableCellCache == null)
            {
                CalculateReachableCells();
            }
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

        public List<GameEvent> MoveTo(MemoryEdge end)
        {
            var list = new List<GameEvent>();
            var creature = GetCreatureInTurn();
            creature.RemainingMovement = creature.RemainingMovement.Select(x =>
           {
                   return new Speed(x.Movement, x.Square - end.Speed);
           }).ToList();
            map.MoveCreatureTo(creature, end);
            foreach(var e in end.Events)
            {
                if(e.Type == GameEvent.Types.Falling)
                {
                    e.Damage = Roller.Roll(RollTypes.Normal, e.FallingHeight, 6, 0);
                    creature.CurrentHitPoints -= e.Damage;
                }
                if (e.Type == GameEvent.Types.Attacks)
                {
                    e.Attacked = GetCreatureInTurn().Id;
                    //check advantage
                    var damage = e.Attack.Damage.Sum(x => Roller.Roll(RollTypes.Normal, x.NumberOfDice, x.DiceFaces, x.Modifier));
                    GetCreatureInTurn().CurrentHitPoints -= damage;
                }
                list.Add(e);
            }
            return list;
        }

        public List<GameEvent> Attack(ConfirmAttackAction confirmAttackAction, bool forceHit = false)
        {
            var targetCreature = GetCreatureById(confirmAttackAction.TargetCreature);
            var attackingCreature = GetCreatureById(confirmAttackAction.AttackingCreature);
            var list = new List<GameEvent>();
            var hasAdvantage = false;
            var hasDisadvantage = false;
            if (targetCreature.TemporaryEffectsList.Any(x => x.Item3 == TemporaryEffects.DisadvantageToSufferedAttacks))
            {
                //TODO: also if too near
                hasDisadvantage = true;
            }
            if (targetCreature.TemporaryEffectsList.Any(x => x.Item3 == TemporaryEffects.AdvantageToAttacks))
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

            var isCritical = (toHit - confirmAttackAction.Attack.ToHit) >= attackingCreature.CriticalThreshold;
            if(forceHit || toHit >= targetCreature.ArmorClass || isCritical)
            {
                var totalDamage = 0;
                foreach(var damage in confirmAttackAction.Attack.Damage)
                {
                    if (forceHit)
                    {
                        var multiplier = 1f;
                        if (hasAdvantage)
                        {
                            multiplier = 1.2f;
                        }
                        if (hasDisadvantage)
                        {
                            multiplier = 0.8f;
                        }
                        totalDamage += (int)Math.Round(((damage.NumberOfDice * damage.DiceFaces) + damage.Modifier) * multiplier);
                    }
                    else
                    {
                        totalDamage += Roller.Roll(RollTypes.Normal, isCritical ? 2 : 1 * damage.NumberOfDice, damage.DiceFaces, damage.Modifier);
                    }
                }

                targetCreature.TemporaryHitPoints -= totalDamage;

                if (targetCreature.TemporaryHitPoints < 0)
                {
                    targetCreature.CurrentHitPoints += targetCreature.TemporaryHitPoints;
                    targetCreature.TemporaryHitPoints = 0;
                }

                list.Add(new GameEvent
                {
                    Type = GameEvent.Types.Attacks,
                    Attacker = attackingCreature.Id,
                    Attacked = targetCreature.Id,
                    Attack = confirmAttackAction.Attack,
                    LogDescription = string.Format("\nAttacked {0}\nAttack: {1}\nDamage: {2}", GetCreatureById(confirmAttackAction.TargetCreature).GetType().Name, confirmAttackAction.Attack.Name, totalDamage)
                }); ;

            } else
            {
                Logger.WriteLine("Not hit");
                list.Add(new GameEvent
                {
                    Type = GameEvent.Types.AttackMissed,
                    Attacker = attackingCreature.Id,
                    Attacked = targetCreature.Id,
                    Attack = confirmAttackAction.Attack,
                    LogDescription = string.Format("\nAttacked {0}\nAttack: {1}\nDamage: {2}", GetCreatureById(confirmAttackAction.TargetCreature).GetType().Name, confirmAttackAction.Attack.Name, "Missed")
                });
            }

            if (confirmAttackAction.ActionEconomy == BattleActions.Action) {
                attackingCreature.RemainingAttacksPerAction--;
                attackingCreature.ActionUsedToAttack = true;
                if(attackingCreature is IMartialArts && confirmAttackAction.Attack.Name.ToLower().Contains("quarterstaff"))
                {
                    (attackingCreature as IMartialArts).BonusAttackTriggered = true;
                }
            }
            if (confirmAttackAction.ActionEconomy == BattleActions.BonusAction) {
                attackingCreature.RemainingAttacksPerBonusAction--;
                attackingCreature.BonusActionUsedToAttack = true;
            }

            attackingCreature.LastAttackUsed += confirmAttackAction.Attack.Name;

            if(!forceHit)
            {
                RemoveDeaths();
            }

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
                    _reachableCellCache = null;
                    var action = availableAction as DashAction;
                    creature.RemainingMovement = creature.RemainingMovement.Select(mov =>
                    {
                        var baseSpeed = creature.Movements.First(x => x.Movement == mov.Movement);
                        return new Speed(mov.Movement, mov.Square + baseSpeed.Square);
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
                    creature.DashUsed = true;
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
                    creature.TemporaryEffectsList.Add(new Tuple<int, int, TemporaryEffects>(creature.Id, 1, TemporaryEffects.DisadvantageToSufferedAttacks));
                    var defAction = availableAction as PatientDefenseAction;
                    DndModule.Get<ILogger>().WriteLine(string.Format("Used Patient Defense,  Disadvantage to suffered attacks until your next turn, -1 Ki Point"));
                    creature.BonusActionUsedNotToAttack = true;
                    var monkDef = creature as IKiPointsOwner;
                    if (monkDef != null)
                    {
                        monkDef.RemainingKiPoints--;
                    }
                    creature.DodgeUsed = true;
                    break;
                case ActionsTypes.Dodge:
                    creature.TemporaryEffectsList.Add(new Tuple<int, int, TemporaryEffects>(creature.Id, 1, TemporaryEffects.DisadvantageToSufferedAttacks));
                    creature.ActionUsedNotToAttack = true;
                    creature.DodgeUsed = true;
                    break;
                case ActionsTypes.FightingSpirit:
                    creature.TemporaryEffectsList.Add(new Tuple<int, int, TemporaryEffects>(creature.Id, 1, TemporaryEffects.AdvantageToAttacks));
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
                    Type = GameEvent.Types.SelfAbility,
                    Ability = availableAction.ActionType.ToString()
                }
            };
        }

        public List<GameEvent> Spell(ConfirmSpellAction confirmSpellAction, bool forceHit = false)
        {
            var list = new List<GameEvent>();

            if(confirmSpellAction.Spell is FalseLife)
            {
                var temporary = Roller.Roll(RollTypes.Normal, 1, 4, 4);
                if (forceHit)
                {
                    temporary = 4 + 4;
                }
                var creature = map.GetOccupantCreature(confirmSpellAction.Target.X, confirmSpellAction.Target.Y);
                creature.TemporaryHitPoints += temporary;
                GetCreatureInTurn().ActionUsedNotToAttack = true;
                list.Add(new GameEvent
                {
                    Type = GameEvent.Types.Spell,
                    LogDescription = "False Life (+" + temporary + " temp pf)",
                    Attacker = GetCreatureInTurn().Id,
                    Attacked = creature.Id
                });
            }

            if (confirmSpellAction.Spell is MagicMissile)
            {
                var damage = Roller.Roll(RollTypes.Normal, 1, 4, 1) * 3;
                if (forceHit)
                {
                    damage = 5 * 3;
                }
                var creature = map.GetOccupantCreature(confirmSpellAction.Target.X, confirmSpellAction.Target.Y);
                creature.CurrentHitPoints -= damage;
                GetCreatureInTurn().ActionUsedNotToAttack = true;
                list.Add(new GameEvent
                {
                    Type = GameEvent.Types.Spell,
                    LogDescription = "Magic Missile (" + damage + " dmg) to " + creature.GetType().Name,
                    Attacker = GetCreatureInTurn().Id,
                    Attacked = creature.Id
                });
            }

            if (confirmSpellAction.Spell is RayOfFrost)
            {
                var hasAdvantage = false;
                var hasDisadvantage = false;
                var targetCreature = map.GetOccupantCreature(confirmSpellAction.Target.X, confirmSpellAction.Target.Y);
                var attackingCreature = GetCreatureById(confirmSpellAction.Caster);
                if (targetCreature.TemporaryEffectsList.Any(x => x.Item3 == TemporaryEffects.DisadvantageToSufferedAttacks))
                {
                    hasDisadvantage = true;
                    //TODO: also if too near
                }
                if (targetCreature.TemporaryEffectsList.Any(x => x.Item3 == TemporaryEffects.AdvantageToAttacks))
                {
                    hasAdvantage = true;
                }

                if (hasDisadvantage && hasAdvantage)
                {
                    hasAdvantage = false;
                    hasDisadvantage = false;
                }

                var rollType = RollTypes.Normal;
                if (hasDisadvantage)
                {
                    rollType = RollTypes.Disadvantage;
                }
                if (hasAdvantage)
                {
                    rollType = RollTypes.Advantage;
                }
                var toHit = Roller.Roll(rollType, 1, 20, confirmSpellAction.Spell.ToHit);

                var isCritical = (toHit - confirmSpellAction.Spell.ToHit) >= attackingCreature.CriticalThreshold;
                if (forceHit || toHit >= targetCreature.ArmorClass || isCritical)
                {
                    var totalDamage = 0;
                    if (forceHit)
                    {
                        var multiplier = 1f;
                        if (hasAdvantage)
                        {
                            multiplier = 1.2f;
                        }
                        if (hasDisadvantage)
                        {
                            multiplier = 0.8f;
                        }
                        totalDamage += (int)Math.Round(8 * multiplier);
                    }
                    else
                    {
                        totalDamage += Roller.Roll(RollTypes.Normal, isCritical ? 2 : 1 * 1, 8, 0);
                    }

                    targetCreature.TemporaryHitPoints -= totalDamage;

                    if (targetCreature.TemporaryHitPoints < 0)
                    {
                        targetCreature.CurrentHitPoints += targetCreature.TemporaryHitPoints;
                        targetCreature.TemporaryHitPoints = 0;
                    }
                    list.Add(new GameEvent
                    {
                        Type = GameEvent.Types.Spell,
                        LogDescription = "Ray of frost (" + totalDamage + " dmg) to " + targetCreature.GetType().Name,
                        Attacker = GetCreatureInTurn().Id,
                        Attacked = targetCreature.Id
                    });

                    targetCreature.TemporaryEffectsList.Add(new Tuple<int, int, TemporaryEffects>(
                        confirmSpellAction.Caster,
                        1,
                        TemporaryEffects.SpeedReducedByTwo
                    ));
                }
                else
                {
                    Logger.WriteLine("Not hit");
                    list.Add(new GameEvent
                    {
                        Type = GameEvent.Types.AttackMissed,
                        Attacker = attackingCreature.Id,
                        Attacked = targetCreature.Id,
                        LogDescription = string.Format("\nRay of frost to {0}\nDamage: {1}", GetCreatureById(targetCreature.Id).GetType().Name, "Missed")
                    });
                }
                
                GetCreatureInTurn().ActionUsedNotToAttack = true;
            }

            if (!forceHit)
            {
                RemoveDeaths();
            }

            return list;
        }

        public ICreature GetCreatureById(int id)
        {
            return map.Creatures[id];
        }

        public CellInfo GetCellInfo(int x, int y)
        {
            return Map.GetCellInfo(x, y);
        }

        public void ClearCache()
        {
            _reachableCellCache = null;
            _cachedActions.Clear();
        }
    }
}
