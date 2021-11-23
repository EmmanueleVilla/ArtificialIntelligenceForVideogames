using Core.Map;
using Logic.Core.Actions;

namespace Logic.Core.Battle
{
    public struct MovementEvent
    {
        public enum Types
        {
            Movement,
            Falling,
            Attacks
        }

        public Types type;
        public int FallingHeight;
        public Attack Attack;
        public CellInfo Destination;
    }
}
