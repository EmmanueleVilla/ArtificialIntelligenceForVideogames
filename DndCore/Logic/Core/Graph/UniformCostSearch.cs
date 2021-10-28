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
            public float DamageTaken;
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

            var visited = new List<int>();
            var queue = new List<ReachedCell>();
            queue.Add(new ReachedCell(from));

            while(queue.Count > 0)
            {
                queue = queue.OrderBy(x => x.DamageTaken).ThenBy(x => x.UsedMovement).ToList();
                var best = queue[0];
                visited.Add(best.Cell.X * map.Width + best.Cell.Y);
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
                        var key = newX * map.Width + newY;
                        if(visited.Contains(key))
                        {
                            continue;
                        }
                        var to = map.GetCellInfo(newX, newY);
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
                            queue.Add(reached);
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
