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
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
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

        //TODO Move this to the job
        public List<IAvailableAction> GetAvailableActions()
        {
            var creature = GetCreatureInTurn();
            
            var actions = new List<IAvailableAction>();

            ActionBuildersWrapper.ActionBuilders.ForEach(x => actions.AddRange(x.Build(map, creature)));

            actions.Add(new EndTurnAction());

            return actions;
        }

        public void NextTurn()
        {
            _reachableCellCache.Clear();

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

        List<MemoryEdge> _reachableCellCache = new List<MemoryEdge>();

        public void CalculateReachableCells()
        {
            var creature = GetCreatureInTurn();
            _reachableCellCache = Search.Search(map.GetCellOccupiedBy(creature), map);
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

        public IEnumerable<MovementEvent> MoveTo(MemoryEdge end)
        {
            var creature = GetCreatureInTurn();
            creature.RemainingMovement = creature.RemainingMovement.Select(x =>
           {
                   return new Speed(x.Item1, x.Item2 - end.Speed);
           }).ToList();
            map.MoveCreatureTo(creature, end);
            foreach(var e in end.Events)
            {
                if(e.Type == MovementEvent.Types.Falling)
                {
                    e.Damage = Roller.Roll(RollTypes.Normal, e.FallingHeight, 6, 0);
                    creature.CurrentHitPoints -= e.Damage;
                }
                yield return e;
            }
        }

        public void Attack(ConfirmAttackAction confirmAttackAction)
        {
            var hasAdvantage = false;
            var hasDisadvantage = false;
            if(confirmAttackAction.Creature.TemporaryEffectsList.Any(x => x.Item3 == TemporaryEffects.DisadvantageToSufferedAttacks))
            {
                hasDisadvantage = true;
            }

            if(hasDisadvantage && hasAdvantage)
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


            //TODO: check critical hit
            if(toHit >= confirmAttackAction.Creature.ArmorClass)
            {
                var totalDamage = 0;
                foreach(var damage in confirmAttackAction.Attack.Damage)
                {
                    totalDamage += Roller.Roll(RollTypes.Normal, damage.NumberOfDice, damage.DiceFaces, damage.Modifier);
                }
                //TODO apply other damage effects
                Logger.WriteLine(string.Format("Inflicted {0} damage to {1}", totalDamage, confirmAttackAction.Creature.GetType().ToString().Split('.').Last()));
                confirmAttackAction.Creature.CurrentHitPoints -= totalDamage;
                //TODO kill creature if hp < 0

            } else
            {
                Logger.WriteLine("Not hit");
            }

            if (confirmAttackAction.ActionEconomy.Contains("A")) {
                GetCreatureInTurn().RemainingAttacksPerAction--;
                GetCreatureInTurn().ActionUsedToAttack = true;
            }
            if (confirmAttackAction.ActionEconomy.Contains("B")) {
                GetCreatureInTurn().RemainingAttacksPerBonusAction--;
                GetCreatureInTurn().BonusActionUsedToAttack = true;
            }
            
            GetCreatureInTurn().LastAttackUsed += confirmAttackAction.Attack.Name;
        }

        public void UseAbility(IAvailableAction availableAction)
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
                    if(action.ActionEconomy == "A")
                    {
                        DndModule.Get<ILogger>().WriteLine(string.Format("Used Dash, doubled movement"));
                        creature.ActionUsedNotToAttack = true;
                    }
                    if (action.ActionEconomy == "B")
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
                    if (disAction.ActionEconomy == "A")
                    {
                        DndModule.Get<ILogger>().WriteLine(string.Format("Used Disengage"));
                        creature.ActionUsedNotToAttack = true;
                    }
                    if (disAction.ActionEconomy == "B")
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
            }
        }
    }
}
