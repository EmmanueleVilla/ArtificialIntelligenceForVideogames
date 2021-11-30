﻿using Core.Map;
using Logic.Core.Actions;

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

        public Types Type;
        public int FallingHeight;
        public Attack Attack;
        public CellInfo Destination;
        public int Damage;
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

        public override int GetHashCode()
        {
            int hashCode = 1024929324;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + FallingHeight.GetHashCode();
            hashCode = hashCode * -1521134295 + Attack.GetHashCode();
            hashCode = hashCode * -1521134295 + Destination.GetHashCode();
            return hashCode;
        }
    }
}
