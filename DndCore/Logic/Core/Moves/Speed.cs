using Logic.Core.Movements;
using System;

namespace Logic.Core.Movements
{
    public class Speed
    {
        public SpeedTypes Movement;
        public int Square;
        public Speed(SpeedTypes movement, int square)
        {
            Movement = movement;
            Square = square;
        }
    }
}
