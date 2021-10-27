using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Core.Graph
{
    public class SpeedCalculator
    {
        public Edge GetNeededSpeed(ICreature creature, CellInfo from, CellInfo to, IMap map)
        {
            if (to.Terrain == ' ')
            {
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
            
            if(sizeInCells == 1)
            {
                // if size is 1, I don't waste time creating the cell list
                return GetNeedSpeedInternal(creature, from, to, map);
            }

            var edges = new List<Edge>();

            var myCells = new List<CellInfo>();
            for (int x = 0; x < sizeInCells; x++)
            {
                for (int y = 0; y < sizeInCells; y++)
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
                    if(myCells.Contains(newTo))
                    {
                        continue;
                    }
                    var newFrom = map.GetCellInfo(from.X + x, from.Y + y);

                    // if the height of the destination square is different, I can't go there
                    if(Math.Abs(newTo.Height - to.Height) > 1)
                    {
                        return null;
                    }

                    // calculate the cost of a single cell
                    edges.Add(GetNeedSpeedInternal(creature, newFrom, newTo, map));
                }
            }

            // if one edge is null, the path is blocked somewhere
            if(edges.Any(x => x == null))
            {
                return null;
            }

            //return an edge with the worst case of every cell
            var maxMov = edges.Max(x => x.Speed);
            return new Edge(
                CellInfo.Copy(to),
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
                            .OrderByDescending(x => x.Damage.OrderByDescending(xx => xx.AverageDamage).First())
                            .First()
                            .Damage.OrderByDescending(xx => xx.AverageDamage).First().AverageDamage;
                    }
                }
            }

            return new Edge(CellInfo.Copy(to), amount, damage, !occupied);
        }
    }
}
