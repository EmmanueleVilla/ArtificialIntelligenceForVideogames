using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Characters
{
    public class Orc : ICharacter
    {
        public List<Speed> Movements => 
            new List<Speed>() {
                new Speed(SpeedTypes.Base, 6)
            };
    }
}
