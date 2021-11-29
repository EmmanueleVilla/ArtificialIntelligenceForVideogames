using System;
using System.Collections.Generic;
using System.Linq;
using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Creatures;
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

        public DndBattle(IDiceRoller roller = null, UniformCostSearch search = null, ILogger logger = null) {
            Search = search ?? DndModule.Get<UniformCostSearch>();
            Roller = roller ?? DndModule.Get<IDiceRoller>();
            Logger = logger ?? DndModule.Get<ILogger>();
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
            var movementAction = new RequestMovementAction() { RemainingMovement = creature.RemainingMovement };

            if(movementAction.RemainingMovement.Any(x => x.Item2 > 0))
            {
                actions.Add(movementAction);
            }

            if (!creature.ActionUsed && creature.RemainingAttacksPerAction > 0)
            {
                foreach(var attack in creature.Attacks)
                {

                    int sizeInCells = 1;
                    switch (creature.Size)
                    {
                        case Sizes.Large:
                            sizeInCells = 2;
                            break;
                        case Sizes.Huge:
                            sizeInCells = 3;
                            break;
                        case Sizes.Gargantuan:
                            sizeInCells = 4;
                            break;
                    }

                    var position = map.GetCellOccupiedBy(creature);

                    var cells = new List<CellInfo>();
                    var startI = position.X - attack.Range;
                    var endI = position.X + sizeInCells + attack.Range;
                    var startJ = position.Y - attack.Range;
                    var endJ = position.Y + sizeInCells + attack.Range;
                    for (int i = startI; i < endI; i++)
                    {
                        for (int j = startJ; j < endJ; j++)
                        {
                            var occupant = map.GetOccupantCreature(i, j);
                            if (occupant != null && occupant.Loyalty != creature.Loyalty)
                            {
                                cells.Add(map.GetCellInfo(i, j));
                            }
                        }
                    }

                    actions.Add(new RequestAttackAction() { Attack = attack, ReachableCells = cells });
                }
            }

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

        public List<MovementEvent> MoveTo(MemoryEdge end)
        {
            var creature = GetCreatureInTurn();
            creature.RemainingMovement = creature.RemainingMovement.Select(x =>
           {
                   return new Speed(x.Item1, x.Item2 - end.Speed);
           }).ToList();
            map.MoveCreatureTo(creature, end);
            return end.Events;
        }

        public void Attack(ConfirmAttackAction confirmAttackAction)
        {
            //TODO: check advantage and disadvantage
            var toHit = Roller.Roll(RollTypes.Normal, 1, 20, confirmAttackAction.Attack.ToHit);
            Logger.WriteLine(string.Format("Rolled {0} to hit", toHit));

            //TODO: check critical hit
            if(toHit >= confirmAttackAction.Creature.ArmorClass)
            {
                var totalDamage = 0;
                foreach(var damage in confirmAttackAction.Attack.Damage)
                {
                    totalDamage += Roller.Roll(RollTypes.Normal, damage.NumberOfDice, damage.DiceFaces, damage.Modifier);
                }
                //TODO apply other damage effects
                Logger.WriteLine(string.Format("Inflicted {0} damage", totalDamage));
                confirmAttackAction.Creature.CurrentHitPoints -= totalDamage;
                //TODO kill creature if hp < 0

                GetCreatureInTurn().RemainingAttacksPerAction--;

            } else
            {
                Logger.WriteLine("Not hit");
            }
        }
    }
}
