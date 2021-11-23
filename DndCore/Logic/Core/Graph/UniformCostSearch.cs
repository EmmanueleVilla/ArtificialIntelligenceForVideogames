using Core.DI;
using Core.Map;
using Core.Utils.Log;
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

        public List<MemoryEdge> Search(CellInfo from, IMap map)
        {
            logger?.WriteLine("Starting search");
            var result = new List<MemoryEdge>();
            var creature = from.Creature;
            var movements = creature.Movements;
            if (movements.TrueForAll(x => x.Item2 <= 0))
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
                var remainingMovement = from.Creature.Movements.Select(x => new Speed(x.Item1, x.Item2 - best.UsedMovement)).ToList();
                for (int deltaX = -1; deltaX <= 1; deltaX++)
                {
                    for (int deltaY = -1; deltaY <= 1; deltaY++)
                    {
                        var newX = best.Cell.X + deltaX;
                        var newY = best.Cell.Y + deltaY;
                        if(deltaX == 0 && deltaY == 0)
                        {
                            continue;
                        }
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
                            if (!remainingMovement.Any(x => x.Item2 - edge.Speed >= 0))
                            {
                                continue;
                            }
                            var path = new List<CellInfo>(best.Path);
                            path.Add(edge.Start);
                            var reached = new ReachedCell(to)
                            {
                                UsedMovement = best.UsedMovement + edge.Speed,
                                CanEndMovementHere = edge.CanEndMovementHere,
                                DamageTaken = best.DamageTaken + edge.Damage,
                                Path = path
                            };
                            queue.Add(reached, reached);
                            edge.Speed += best.UsedMovement;
                            edge.Damage += best.DamageTaken;
                            if(edge.Damage == 0)
                            {
                                visited.Add(key);
                            }
                            result.Add(new MemoryEdge(path, edge.Destination, edge.Speed, edge.Damage, edge.CanEndMovementHere));
                        }
                        else
                        {
                            visited.Add(key);
                        }
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
