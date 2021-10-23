using Core.Map;
using Logic.Core.Characters;
using Logic.Core.Movements;
using System.Linq;

namespace Logic.Core.Graph
{
    public class SpeedCalculator
    {
        public int GetNeededSpeed(ICharacter character, CellInfo from, CellInfo to)
        {
            
            if(to.Terrain == ' ')
            {
                //we can't go there, that's outside the map
                return -1;
            }

            var heightDiff = to.Height - from.Height;


            if (heightDiff < 0 && heightDiff < -2)
            {
                //we can't go there, we'll need to jump down, otherwise the speed needed is only 1
                return -1;
            }

            var squares = 1;

            if (heightDiff > 1)
            {
                var hasClimb = character.Movements.Any(x => x.Item1 == SpeedTypes.Climbing);
                if(hasClimb)
                {
                    squares = (heightDiff + 1) / 2;
                }
                else
                {
                    squares = heightDiff;
                }
            }

            switch (to.Terrain)
            {
                case 'R':
                    if(!character.Movements.Any(x => x.Item1 == SpeedTypes.Swimming)) {
                        squares++;
                    }
                 break;
            }

            return squares;
        }
    }
}
