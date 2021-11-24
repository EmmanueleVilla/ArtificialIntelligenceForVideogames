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

        public Types Type;
        public int FallingHeight;
        public Attack Attack;
        public CellInfo Destination;

        public override string ToString()
        {
            return string.Format("type:{0}, destination:{1}, attack:{2}, falling:{3}", 
                Type, Destination, Attack, FallingHeight);
        }

        public override bool Equals(object obj)
        {
            var other = (MovementEvent)obj;
            return Type == other.Type
                && Destination.X == other.Destination.X
                && Destination.Y == other.Destination.Y
                && FallingHeight == other.FallingHeight
                && Attack.Name == other.Attack.Name;
        }
    }
}
