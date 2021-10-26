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
                Console.WriteLine(fromSize);
                var toSize = (int)to.Creature.Size;
                Console.WriteLine(toSize);
                var sizeDifference = Math.Abs(fromSize - toSize);
                Console.WriteLine(sizeDifference);
                if (sizeDifference < 2)
                {
                    return null;
                }
            }

            if (to.Terrain == ' ')
            {   
                return null;
            }

            var heightDiff = to.Height - from.Height;

            if (heightDiff < 0 && heightDiff < -2)
            {
                return null;
            }

            var amount = 1;

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

            switch (to.Terrain)
            {
                case 'R':
                    amount += creature.Movements.Any(x => x.Item1 == SpeedTypes.Swimming) ? 0 : 1;
                 break;
            }

            //TODO: calculate damage
            //TODO: check if cell is occupied by ally
            return new Edge(CellInfo.Copy(to), amount, 0, true);
        }
    }
}
