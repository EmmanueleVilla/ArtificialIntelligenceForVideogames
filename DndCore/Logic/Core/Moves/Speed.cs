using Logic.Core.Movements;
using System;

namespace Logic.Core.Movements
{
    public class Speed : Tuple<SpeedTypes, int>
    {
        public Speed(SpeedTypes movement, int square): base(movement, square)
        {

        }
    }
}
