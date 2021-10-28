﻿using Core.Map;

namespace Logic.Core.Graph
{
    public class Edge
    {
        public CellInfo Start;
        public CellInfo Destination;
        public int Speed;
        public int Damage;
        public bool CanEndMovementHere;

        public Edge(CellInfo start, CellInfo destination, int speed, int damage, bool canEndMovementHere)
        {
            Start = start;
            Destination = destination;
            Speed = speed;
            Damage = damage;
            CanEndMovementHere = canEndMovementHere;
        }

        public override string ToString()
        {
            return string.Format("From: {0}, To: {1}, Speed: {2}, Damage: {3}, CanEnd: {4}",
                Start.X + "," + Start.Y,
                Destination.X + "," + Destination.Y,
                Speed,
                Damage,
                CanEndMovementHere);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Edge;
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
            return hash;
        }
    }
}
