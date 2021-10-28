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
        ILogger logger = DndModule.Get<ILogger>();
        class ReachedCell
        {
            public readonly CellInfo Cell;

            public int UsedMovement;
            public int DamageTaken;
            public bool CanEndMovementHere;
            public ReachedCell(CellInfo cell)
            {
                Cell = cell;
            }

            public override string ToString()
            {
                return string.Format("Used Movement until now:" + UsedMovement);
            }
        }

        class ReachedCellComparer : IComparer<ReachedCell>
        {
            public int Compare(ReachedCell x, ReachedCell y)
            {
                if(x.DamageTaken == y.DamageTaken && x.UsedMovement != y.UsedMovement)
                {
                    return x.UsedMovement.CompareTo(y.UsedMovement);
                }
                var result = x.DamageTaken.CompareTo(y.DamageTaken);
                if (result == 0)
                {
                    // HACK
                    // This breaks get(key), but we won't use it
                    // and it lets us have multiple items with the same key
                    return -1;
                }
                else
                {
                    return result;
                }
            }
        }

        public List<Edge> Search(CellInfo from, IMap map)
        {
            logger?.WriteLine("Starting search");
            var result = new List<Edge>();
            var speedCalculator = new SpeedCalculator();
            var creature = from.Creature;
            var movements = creature.Movements;
            if (movements.TrueForAll(x => x.Item2 <= 0))
            {
                return new List<Edge>();
            }
            var visited = new HashSet<string>();
            var queue = new SortedList<ReachedCell, ReachedCell>(new ReachedCellComparer());
            var startingPoint = new ReachedCell(from);
            queue.Add(startingPoint, startingPoint);
            while (queue.Count > 0)
            {
                var best = queue.First().Value;
                visited.Add(best.Cell.X + "," + best.Cell.Y);
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
                        var key = newX + "," + newY;
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
                        if (edge != null)
                        {
                            if (!remainingMovement.Any(x => x.Item2 - edge.Speed >= 0))
                            {
                                continue;
                            }
                            var reached = new ReachedCell(to)
                            {
                                UsedMovement = best.UsedMovement + edge.Speed,
                                CanEndMovementHere = edge.CanEndMovementHere,
                                DamageTaken = best.DamageTaken + edge.Damage
                            };
                            queue.Add(reached, reached);
                            edge.Speed += best.UsedMovement;
                            edge.Damage += best.DamageTaken;
                            if(edge.Damage == 0)
                            {
                                visited.Add(key);
                            }
                            result.Add(edge);
                        }
                        else
                        {
                            visited.Add(key);
                        }
                    }
                }
            }

            return result;
        }
    }
}
