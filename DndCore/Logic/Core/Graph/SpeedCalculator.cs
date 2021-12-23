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

            var movements = creature.RemainingMovement;

            if (creature.Size == 1)
            {
                // if size is 1, I don't waste time creating the cell list
                return GetNeedSpeedSingleStep(creature, from, to, map, movements);
            }

            //TODO: unit test to check the fit
            var fit = true;
            for (int i = to.X; i < creature.Size + to.X; i++)
            {
                for (int j = to.Y; j < creature.Size + to.Y; j++)
                {
                    var occupiedCell = map.GetCellInfo(i, j);
                    var occupantCreature = map.GetOccupantCreature(i, j);
                    if (Math.Abs(from.Height - occupiedCell.Height) > 1 || (occupantCreature != null && occupantCreature != creature))
                    {
                        fit = false;
                    }
                }
            }

            var maxSpeed = 0;
            var maxDamage = 0;
            var canEndMovementHere = true;
            var movementEvents = new List<GameEvent>();

            var deltaX = to.X - from.X;
            var deltaY = to.Y - from.Y;
            var movingHorizontal = deltaX != 0;
            var movingVertically = deltaY != 0;
            if (movingHorizontal)
            {
                var xPos = from.X + (deltaX > 0 ? creature.Size - 1 : 0);
                for (int y = from.Y; y < from.Y + creature.Size; y++)
                {
                    var tempFrom = map.GetCellInfo(xPos, y);
                    var tempTo = map.GetCellInfo(xPos + deltaX, y + deltaY);
                    if (Math.Abs(tempTo.Height - to.Height) > 1)
                    {
                        return Edge.Empty(); ;
                    }

                    var edge = GetNeedSpeedSingleStep(creature, tempFrom, tempTo, map, movements);
                    if (edge.Equals(Edge.Empty()))
                    {
                        return edge;
                    }
                    if (movementEvents.Count == 0)
                    {
                        movementEvents = edge.MovementEvents.Select(@event =>
                       {
                           if(@event.Type == GameEvent.Types.Movement)
                           {
                               @event.Destination = to;
                           }
                           return @event;
                       }).ToList();
                    }
                    maxSpeed = Math.Max(maxSpeed, edge.Speed);
                    if(edge.Damage > maxDamage)
                    {
                        maxDamage = edge.Damage;
                        movementEvents = edge.MovementEvents.Select(@event =>
                        {
                            if (@event.Type == GameEvent.Types.Movement)
                            {
                                @event.Destination = to;
                            }
                            return @event;
                        }).ToList();
                    }
                    canEndMovementHere &= edge.CanEndMovementHere;
                }
            }
            if (movingVertically)
            {
                var yPos = from.Y + (deltaY > 0 ? creature.Size - 1 : 0);
                for (int x = from.X; x < from.X + creature.Size; x++)
                {
                    var tempFrom = map.GetCellInfo(x, yPos);
                    var tempTo = map.GetCellInfo(x + deltaX, yPos + deltaY);
                    if (Math.Abs(tempTo.Height - to.Height) > 1)
                    {
                        return Edge.Empty();
                    }

                    var edge = GetNeedSpeedSingleStep(creature, tempFrom, tempTo, map, movements);
                    if (edge.Equals(Edge.Empty()))
                    {
                        return edge;
                    }
                    if (movementEvents.Count == 0)
                    {
                        movementEvents = edge.MovementEvents.Select(@event =>
                        {
                            if (@event.Type == GameEvent.Types.Movement)
                            {
                                @event.Destination = to;
                            }
                            return @event;
                        }).ToList();
                    }
                    maxSpeed = Math.Max(maxSpeed, edge.Speed);
                    if (edge.Damage > maxDamage)
                    {
                        maxDamage = edge.Damage;
                        movementEvents = edge.MovementEvents.Select(@event =>
                        {
                            if (@event.Type == GameEvent.Types.Movement)
                            {
                                @event.Destination = to;
                            }
                            return @event;
                        }).ToList();
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
                canEndMovementHere && fit
                );
        }

        Edge GetNeedSpeedSingleStep(ICreature creature, CellInfo from, CellInfo to, IMap map, List<Speed> movements)
        {
            // check if terrain is outside the map
            if (to.Terrain == ' ')
            {
                return Edge.Empty();
            }

            var movementEvents = new List<GameEvent>();
            movementEvents.Add(new GameEvent() { Type = GameEvent.Types.Movement, Destination = to });

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
                var climbingMovement = movements.FirstOrDefault(x => x.Movement == SpeedTypes.Climbing);
                if (climbingMovement != null && climbingMovement.Square > 0)
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
                    movementEvents.Add(new GameEvent() { Type = GameEvent.Types.Falling, FallingHeight = Math.Abs(heightDiff / 2) });
                }
            }

            switch (to.Terrain)
            {
                // I need to swim
                case 'R':
                    var swimmingMovement = movements.FirstOrDefault(x => x.Movement == SpeedTypes.Swimming);
                    amount += swimmingMovement != null && swimmingMovement.Square > 0 ? 0 : 1;
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
                    if(!enemy.ReactionUsed)
                    {
                        var attack = enemy.Attacks.FirstOrDefault(x => x.Range <= 2);
                        movementEvents.Add(new GameEvent() { Type = GameEvent.Types.Attacks, Attack = attack } );
                        damage += enemy.Attacks
                            .Where(x => x.Range <= 2)
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
