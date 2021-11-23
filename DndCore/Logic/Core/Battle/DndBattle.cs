using System;
using System.Collections.Generic;
using System.Linq;
using Core.DI;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
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
        private ISpeedCalculator SpeedCalculator;

        private List<Tuple<ICreature, List<Speed>>> remainingSpeeds = new List<Tuple<ICreature, List<Speed>>>();

        public DndBattle(UniformCostSearch search = null, ISpeedCalculator speedCalculator = null) {
            Search = search ?? DndModule.Get<UniformCostSearch>();
            SpeedCalculator = speedCalculator ?? DndModule.Get<ISpeedCalculator>();
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
                Console.WriteLine(string.Format("{0} rolled {1}", creature, creature.RollInitiative()));
                initiativeOrder.Add(creature);
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
            return new List<IAvailableAction>() {
                new RequestMovementAction() { RemainingMovement = remainingSpeeds.First(x => x.Item1 == GetCreatureInTurn()).Item2 },
                new EndTurnAction()
            };
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
            _reachableCellCache = Search.Search(map.GetCellOccupiedBy(GetCreatureInTurn()), map);
            //TODO cache also List<MovementEvent>
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
            map.MoveCreatureTo(GetCreatureInTurn(), end);
            var path = GetPathTo(end);
            path.Add(map.GetCellInfo(end.Destination.X, end.Destination.Y));
            var movementEvents = new List<MovementEvent>();
            for (int i=0; i<path.Count-1; i++)
            {
                var edge = SpeedCalculator.GetNeededSpeed(GetCreatureInTurn(), path[i], path[i + 1], map);
                movementEvents.Add(new MovementEvent() { type = MovementEvent.Types.Movement, Destination = edge.Destination });
                movementEvents.AddRange(edge.MovementEvents);
            }
            return movementEvents;
        }
    }
}
