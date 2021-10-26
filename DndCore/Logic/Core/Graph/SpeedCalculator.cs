using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System;
using System.Linq;

namespace Logic.Core.Graph
{
    public class SpeedCalculator
    {
        public Edge GetNeededSpeed(ICreature creature, CellInfo from, CellInfo to, IMap map)
        {
            if (to.Creature != null && to.Creature.Loyalty == Loyalties.Enemy)
            {
                var fromSize = (int)creature.Size;
                var toSize = (int)to.Creature.Size;
                var sizeDifference = Math.Abs(fromSize - toSize);
                if (sizeDifference < 2)
                {
                    return null;
                }
            }

            if (to.Terrain == ' ')
            {   
                return null;
            }

            var amount = 1;
            var heightDiff = to.Height - from.Height;

            if (heightDiff > 1)
            {
                var hasClimb = creature.Movements.Any(x => x.Item1 == SpeedTypes.Climbing);
                if(hasClimb)
                {
                    amount += (heightDiff + 1) / 2 - 1;
                }
                else
                {
                    amount += heightDiff - 1;
                }
            }

            var damage = 0;

            if(heightDiff < 0)
            {
                amount += -heightDiff - 1;
                damage += -(heightDiff / 2) * 4;
            }

            switch (to.Terrain)
            {
                case 'R':
                    amount += creature.Movements.Any(x => x.Item1 == SpeedTypes.Swimming) ? 0 : 1;
                 break;
            }

            var occupied = to.Creature != null;
            if(occupied)
            {
                amount++;
            }

            return new Edge(CellInfo.Copy(to), amount, damage, !occupied);
        }
    }
}
