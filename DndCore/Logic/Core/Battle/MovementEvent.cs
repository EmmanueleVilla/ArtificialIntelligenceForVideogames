using Logic.Core.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle
{
    public class MovementEvent
    {
        public enum Types
        {
            Movement,
            Falling,
            Attacks
        }

        public Types type;
        public int MovementUsed;
        public int FallingHeight;
        public Attack Attack;
    }
}
