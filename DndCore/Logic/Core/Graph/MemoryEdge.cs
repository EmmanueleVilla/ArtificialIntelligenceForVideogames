using DndCore.Map;
using Logic.Core.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Graph
{
    public struct MemoryEdge
    {
        //TODO: Remove List<CellInfo> and rely only on the MovementEvents list
        public List<CellInfo> Start;
        public List<GameEvent> Events;
        public CellInfo Destination;
        public int Speed;
        public int Damage;
        public bool CanEndMovementHere;
        
        public MemoryEdge(List<CellInfo> start, List<GameEvent> events, CellInfo destination, int speed, int damage, bool canEndMovementHere)
        {
            Start = start;
            Destination = destination;
            Events = events;
            Speed = speed;
            Damage = damage;
            CanEndMovementHere = canEndMovementHere;
        }

        public static MemoryEdge Empty()
        {
            return new MemoryEdge(new List<CellInfo>(), new List<GameEvent>(), CellInfo.Empty(), 0, 0, false);
        }


        public override string ToString()
        {
            return string.Format("From: {0}, To: {1}, Speed: {2}, Damage: {3}, CanEnd: {4}, Events: {5}",
                Start.Last().X + "," + Start.Last().Y,
                Destination.X + "," + Destination.Y,
                Speed,
                Damage,
                CanEndMovementHere,
                string.Join("-", Events));
        }

        public override bool Equals(object obj)
        {
            if(obj.GetType() != this.GetType())
            {
                return false;
            }
            var other = (MemoryEdge)obj;
            return CanEndMovementHere == other.CanEndMovementHere
                && Speed == other.Speed
                && Damage == other.Damage
                && Destination.X == other.Destination.X
                && Destination.Y == other.Destination.Y;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + CanEndMovementHere.GetHashCode();
            hash = (hash * 7) + Speed.GetHashCode();
            hash = (hash * 7) + Damage.GetHashCode();
            hash = (hash * 7) + Destination.GetHashCode();
            hash = (hash * 7) + Events.GetHashCode();
            return hash;
        }
    }
}
