using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Characters
{
    public class Behir : ICharacter
    {
        public List<Speed> Movements =>
            new List<Speed>() {
                new Speed(SpeedTypes.Base, 10),
                new Speed(SpeedTypes.Climbing, 8)
            };
    }
}
