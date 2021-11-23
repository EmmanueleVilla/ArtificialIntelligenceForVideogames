using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core.Battle;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Core.Graph
{
    public class SpeedCalculator : ISpeedCalculator
    {
        public Edge GetNeededSpeed(ICreature creature, CellInfo from, CellInfo to, IMap map)
        {
            if (to.Terrain == ' ')
            {
                return Edge.Empty();
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
                return GetNeedSpeedSingleStep(creature, from, to, map);
            }

            var maxSpeed = 0;
            var maxDamage = 0;
            var canEndMovementHere = true;
            var movementEvents = new List<MovementEvent>();

            var deltaX = to.X - from.X;
            var deltaY = to.Y - from.Y;
            var movingHorizontal = deltaX != 0;
            var movingVertically = deltaY != 0;
            if (movingHorizontal)
            {
                var xPos = from.X + (deltaX > 0 ? sizeInCells - 1 : 0);
                for (int y = from.Y; y < from.Y + sizeInCells; y++)
                {
                    var tempFrom = map.GetCellInfo(xPos, y);
                    var tempTo = map.GetCellInfo(xPos + deltaX, y + deltaY);
                    if (Math.Abs(tempTo.Height - to.Height) > 1)
                    {
                        return Edge.Empty(); ;
                    }

                    var edge = GetNeedSpeedSingleStep(creature, tempFrom, tempTo, map);
                    if (edge.Equals(Edge.Empty()))
                    {
                        return edge;
                    }
                    maxSpeed = Math.Max(maxSpeed, edge.Speed);
                    if(edge.Damage > maxDamage)
                    {
                        maxDamage = edge.Damage;
                        movementEvents = edge.MovementEvents;
                    }
                    canEndMovementHere &= edge.CanEndMovementHere;
                }
            }
            if (movingVertically)
            {
                var yPos = from.Y + (deltaY > 0 ? sizeInCells - 1 : 0);
                for (int x = from.X; x < from.X + sizeInCells; x++)
                {
                    var tempFrom = map.GetCellInfo(x, yPos);
                    var tempTo = map.GetCellInfo(x + deltaX, yPos + deltaY);
                    if (Math.Abs(tempTo.Height - to.Height) > 1)
                    {
                        return Edge.Empty();
                    }

                    var edge = GetNeedSpeedSingleStep(creature, tempFrom, tempTo, map);
                    if (edge.Equals(Edge.Empty()))
                    {
                        return edge;
                    }
                    maxSpeed = Math.Max(maxSpeed, edge.Speed);
                    if (edge.Damage > maxDamage)
                    {
                        maxDamage = edge.Damage;
                        movementEvents = edge.MovementEvents;
                    }
                    canEndMovementHere &= edge.CanEndMovementHere;
                }
            }

            if (!movingHorizontal && !movingVertically)
            {
                return Edge.Empty();
            }

            //return an edge with the worst case of every cell
            return new Edge(
                from,
                to,
                maxSpeed,
                maxDamage,
                movementEvents,
                canEndMovementHere
                );
        }

        Edge GetNeedSpeedSingleStep(ICreature creature, CellInfo from, CellInfo to, IMap map)
        {
            // check if terrain is outside the map
            if (to.Terrain == ' ')
            {
                return Edge.Empty();
            }

            var movementEvents = new List<MovementEvent>();
            movementEvents.Add(new MovementEvent() { type = MovementEvent.Types.Movement, Destination = to });

            // check if there is an enemy creature and if I can pass through it
            var occupant = map.GetOccupantCreature(to.X, to.Y);
            if (occupant != null && occupant.Loyalty == Loyalties.Enemy)
            {
                var fromSize = (int)creature.Size;
                var toSize = (int)occupant.Size;
                var sizeDifference = Math.Abs(fromSize - toSize);
                if (sizeDifference < 2)
                {
                    return Edge.Empty();
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
                if (damage > 0)
                {
                    movementEvents.Add(new MovementEvent() { type = MovementEvent.Types.Falling, FallingHeight = Math.Abs(heightDiff / 2) });
                }
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
                    if(enemy.HasReaction)
                    {
                        var attack = enemy.Attacks.FirstOrDefault(x => x.Type == Actions.AttackTypes.WeaponMelee);
                        movementEvents.Add(new MovementEvent() { type = MovementEvent.Types.Attacks, Attack = attack } );
                        damage += enemy.Attacks
                            .Where(x => x.Type == Actions.AttackTypes.WeaponMelee || x.Type == Actions.AttackTypes.WeaponMeleeReach)
                            .OrderByDescending(x => x.Damage.Select(xx => xx.AverageDamage).Sum())
                            .First()
                            .Damage.Select(xx => xx.AverageDamage).Sum();
                    }
                }
            }

            return new Edge(from, to, amount, damage, movementEvents, !occupied);
        }
    }
}
