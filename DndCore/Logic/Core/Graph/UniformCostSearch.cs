using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core.Battle;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Graph
{
    public class UniformCostSearch
    {
        private ILogger logger;
        private ISpeedCalculator speedCalculator;

        public UniformCostSearch(ISpeedCalculator speedCalculator = null, ILogger logger = null)
        {
            this.logger = logger ?? DndModule.Get<ILogger>();
            this.speedCalculator = speedCalculator ?? DndModule.Get<ISpeedCalculator>();
        }

        struct Delta
        {
            public int DeltaX;
            public int DeltaY;
        }

        List<Delta> deltas = new List<Delta>()
        {
            new Delta() { DeltaX = -1, DeltaY = 0 },
            new Delta() { DeltaX = 0, DeltaY = -1 },
            new Delta() { DeltaX = 0, DeltaY = 1 },
            new Delta() { DeltaX = 1, DeltaY = 0 },
            new Delta() { DeltaX = -1, DeltaY = -1 },
            new Delta() { DeltaX = -1, DeltaY = 1 },
            new Delta() { DeltaX = 1, DeltaY = -1 },
            new Delta() { DeltaX = 1, DeltaY = 1 }
        };

        public List<MemoryEdge> Search(CellInfo from, IMap map)
        {
            var result = new List<MemoryEdge>();
            var creature = from.Creature;
            if(creature == null || creature.RemainingMovement == null)
            {
                creature.ToString();
            }
            var movements = creature.RemainingMovement;
            if (movements.TrueForAll(x => x.Square <= 0))
            {
                return result;
            }
            var visited = new HashSet<int>();
            var queue = new SortedList<ReachedCell, ReachedCell>(new ReachedCellComparer());
            var startingPoint = new ReachedCell(from);
            queue.Add(startingPoint, startingPoint);
            while (queue.Count > 0)
            {
                var best = queue.First().Value;
                visited.Add((best.Cell.X << 6) + best.Cell.Y);
                queue.RemoveAt(0);
                var remainingMovement = movements.Select(x => new Speed(x.Movement, x.Square - best.UsedMovement)).ToList();
                foreach(var delta in deltas) { 
                    var newX = best.Cell.X + delta.DeltaX;
                    var newY = best.Cell.Y + delta.DeltaY;
                    var key = (newX << 6) + newY;
                    if (visited.Contains(key))
                    {
                        continue;
                    }
                    var to = map.GetCellInfo(newX, newY);
                    if(to.Terrain == ' ')
                    {
                        visited.Add(key);
                        continue;
                    }
                    var edge = speedCalculator.GetNeededSpeed(
                        from.Creature,
                        best.Cell,
                        to,
                        map);
                    if (!edge.Equals(Edge.Empty()))
                    {
                        if (!remainingMovement.Any(x => x.Square - edge.Speed >= 0))
                        {
                            continue;
                        }
                        var path = new List<CellInfo>(best.Path);
                        path.Add(edge.Start);
                        var events = new List<GameEvent>(best.Events);
                        events.AddRange(edge.MovementEvents);
                        var reached = new ReachedCell(to)
                        {
                            UsedMovement = best.UsedMovement + edge.Speed,
                            CanEndMovementHere = edge.CanEndMovementHere,
                            DamageTaken = best.DamageTaken + edge.Damage,
                            Path = path,
                            Events = events
                        };
                        queue.Add(reached, reached);
                        edge.Speed += best.UsedMovement;
                        edge.Damage += best.DamageTaken;
                        if (edge.Damage == 0)
                        {
                            visited.Add(key);
                        }
                        result.Add(new MemoryEdge(path, events, edge.Destination, edge.Speed, edge.Damage, edge.CanEndMovementHere));
                    }
                    else
                    {
                        visited.Add(key);
                    }
                }
            }
            var distinct = result.Distinct();
            var grouped = distinct.GroupBy(x => new { x.Destination.X, x.Destination.Y, x.Damage });
            var filtered = new List<MemoryEdge>();
            foreach(var group in grouped)
            {
                var min = group.Min(x => x.Speed);
                filtered.Add(group.First(e => e.Speed == min));
            }
            return filtered;
        }
    }
}
