using System;
using System.Collections.Generic;
using System.Linq;
using Core.DI;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Creatures;
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

        private List<Tuple<ICreature, List<Speed>>> remainingSpeeds = new List<Tuple<ICreature, List<Speed>>>();

        public DndBattle(UniformCostSearch search = null) {
            Search = search ?? DndModule.Get<UniformCostSearch>();
        }

        public List<ICreature> Init(IMap map)
        {
            this.map = map;
            foreach(var creature in map.Creatures)
            {
                remainingSpeeds.Add(new Tuple<ICreature, List<Speed>>(creature, creature.Movements));
            }

            foreach (var creature in map.Creatures)
            {
                initiativeOrder.Add(creature);
                creature.RollInitiative();
            }
            initiativeOrder.Sort(new CreatureInitiativeComparer());
            return initiativeOrder;
        }

        public ICreature GetCreatureInTurn()
        {
            return initiativeOrder[turnIndex];
        }

        public List<IAvailableAction> GetAvailableActions()
        {
            var creature = GetCreatureInTurn();
            
            var actions = new List<IAvailableAction>();
            var movementAction = new RequestMovementAction() { RemainingMovement = remainingSpeeds.First(x => x.Item1 == creature).Item2 };

            if(movementAction.RemainingMovement.Any(x => x.Item2 > 0))
            {
                actions.Add(movementAction);
            }

            if (creature.HasAction)
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
                remainingSpeeds.Clear();
                foreach (var creature in map.Creatures)
                {
                    remainingSpeeds.Add(new Tuple<ICreature, List<Speed>>(creature, creature.Movements));
                }
                turnIndex = 0;
            }
        }

        List<MemoryEdge> _reachableCellCache = new List<MemoryEdge>();

        public void CalculateReachableCells()
        {
            var creature = GetCreatureInTurn();
            _reachableCellCache = Search.Search(map.GetCellOccupiedBy(creature), map, remainingSpeeds.First(x => x.Item1 == creature).Item2);
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
            remainingSpeeds = remainingSpeeds.Select(x =>
           {
               if (x.Item1 == creature)
               {
                   var newSpeeds = new List<Speed>();
                   foreach (var speed in x.Item2)
                   {
                       newSpeeds.Add(new Speed(speed.Item1, speed.Item2 - end.Speed));
                   }
                   return new Tuple<ICreature, List<Speed>>(creature, newSpeeds);
               }
               return x;
           }).ToList();
            map.MoveCreatureTo(creature, end);
            return end.Events;
        }
    }
}
