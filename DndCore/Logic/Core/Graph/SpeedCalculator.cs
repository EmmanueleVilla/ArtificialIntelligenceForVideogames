using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Core.Graph
{
    public class SpeedCalculator
    {
        ILogger logger = DndModule.Get<ILogger>();
        public Edge GetNeededSpeed(ICreature creature, CellInfo from, CellInfo to, IMap map)
        {
            logger?.WriteLine("GetNeededSpeed from " + from + " to " + to);
            if (to.Terrain == ' ')
            {
                logger?.WriteLine("Invalid Terrain");
                return null;
            }

            //Console.Writeline(string.Format("Testing path to: {0},{1}", to.X, to.Y));
            // Check the creature size on the grid
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

            if (sizeInCells == 1)
            {
                // if size is 1, I don't waste time creating the cell list
                return GetNeedSpeedInternal(creature, from, to, map);
            }

            var edges = new List<Edge>();

            //TEST WITHOUT LINQ
            var deltaX = to.X - from.X;
            var deltaY = to.Y - from.Y;
            var movingHorizontal = deltaY == 0 && deltaX != 0;
            var movingVertically = deltaX == 0 && deltaY != 0;
            if (movingHorizontal)
            {
                var xPos = from.X + (deltaX > 0 ? sizeInCells - 1 : 0);
                for (int y = from.Y; y < from.Y + sizeInCells; y++)
                {
                    var tempFrom = map.GetCellInfo(xPos, y);
                    var tempTo = map.GetCellInfo(xPos + deltaX, y);
                    if (Math.Abs(tempTo.Height - to.Height) > 1)
                    {
                        return null;
                    }

                    var edge = GetNeedSpeedInternal(creature, tempFrom, tempTo, map);
                    if (edge == null)
                    {
                        return null;
                    }
                    edges.Add(edge);
                }
            }
            if (movingVertically)
            {
                var yPos = from.Y + (deltaY > 0 ? sizeInCells - 1 : 0);
                for (int x = from.X; x < from.X + sizeInCells; x++)
                {
                    var tempFrom = map.GetCellInfo(x, yPos);
                    var tempTo = map.GetCellInfo(x, yPos + deltaY);
                    if (Math.Abs(tempTo.Height - to.Height) > 1)
                    {
                        return null;
                    }

                    var edge = GetNeedSpeedInternal(creature, tempFrom, tempTo, map);
                    if (edge == null)
                    {
                        return null;
                    }
                    edges.Add(edge);
                }
            }

            if (!movingHorizontal && !movingVertically)
            {
                return null;
            }


            /*
            var myCells = new List<CellInfo>();
            var startX = from.X;
            var endX = from.X + sizeInCells;
            var startY = from.Y;
            var endY = from.Y + sizeInCells;
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    var cell = map.GetCellInfo(x, y);
                    myCells.Add(cell);
                }
            }

            // otherwise I check every connection between the touched cells
            for (int x = 0; x < sizeInCells; x++)
            {
                for (int y = 0; y < sizeInCells; y++)
                {
                    var newTo = map.GetCellInfo(to.X + x, to.Y + y);
                    if (myCells.Any(my => my.X == newTo.X && my.Y == newTo.Y))
                    {
                        continue;
                    }
                    var newFrom = map.GetCellInfo(from.X + x, from.Y + y);

                    // if the height of the destination square is different, I can't go there
                    if(Math.Abs(newTo.Height - to.Height) > 1)
                    {
                        return null;
                    }

                    var edge = GetNeedSpeedInternal(creature, newFrom, newTo, map);
                    if(edge == null)
                    {
                        return null;
                    }
                    edges.Add(edge);
                }
            }
            */

            //return an edge with the worst case of every cell
            var maxMov = edges.Max(x => x.Speed);
            return new Edge(
                from,
                to,
                edges.Max(x => x.Speed),
                edges.Max(x => x.Damage),
                edges.All(x => x.CanEndMovementHere)
                );
        }

        Edge GetNeedSpeedInternal(ICreature creature, CellInfo from, CellInfo to, IMap map)
        {
            // check if terrain is outside the map
            if (to.Terrain == ' ')
            {
                return null;
            }

            //Console.Writeline(string.Format("Internal testing path to: {0},{1}", to.X, to.Y));

            // check if there is an enemy creature and if I can pass through it
            var occupant = map.GetOccupantCreature(to.X, to.Y);
            if (occupant != null && occupant.Loyalty == Loyalties.Enemy)
            {
                var fromSize = (int)creature.Size;
                var toSize = (int)occupant.Size;
                var sizeDifference = Math.Abs(fromSize - toSize);
                if (sizeDifference < 2)
                {
                    return null;
                }
            }

            // base amount
            var amount = 1;

            // check height difference
            var heightDiff = to.Height - from.Height;

            // I need to climb
            if (heightDiff > 1)
            {
                var climbingMovement = creature.Movements.FirstOrDefault(x => x.Item1 == SpeedTypes.Climbing);
                if (climbingMovement != null && climbingMovement.Item2 > 0)
                {
                    amount += (heightDiff + 1) / 2 - 1;
                }
                else
                {
                    amount += heightDiff - 1;
                }
            }

            var damage = 0;

            // I need to jump down
            if (heightDiff < 0)
            {
                amount += -heightDiff - 1;
                damage += -(heightDiff / 2) * 4;
            }

            switch (to.Terrain)
            {
                // I need to swim
                case 'R':
                    var swimmingMovement = creature.Movements.FirstOrDefault(x => x.Item1 == SpeedTypes.Swimming);
                    amount += swimmingMovement != null && swimmingMovement.Item2 > 0 ? 0 : 1;
                    break;
            }

            // Cell is occupied, I need 1 more speed and I can't stop here
            var occupied = occupant != null;
            if (occupied)
            {
                amount++;
            }

            if (!creature.Disangaged)
            {
                var enemiesLeft = map.IsLeavingThreateningArea(creature, from, to);
                foreach (var enemy in enemiesLeft)
                {
                    if(enemy.HasReaction())
                    {
                        damage += enemy.Attacks
                            .Where(x => x.Type == Actions.AttackTypes.WeaponMelee || x.Type == Actions.AttackTypes.WeaponMeleeReach)
                            .OrderByDescending(x => x.Damage.Select(xx => xx.AverageDamage).Sum())
                            .First()
                            .Damage.Select(xx => xx.AverageDamage).Sum();
                    }
                }
            }

            return new Edge(from, to, amount, damage, !occupied);
        }
    }
}
