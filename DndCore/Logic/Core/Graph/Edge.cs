using Core.Map;

namespace Logic.Core.Graph
{
    public class Edge
    {
        public CellInfo Destination;
        public int Speed;
        public float Damage;
        public bool CanEndMovementHere;

        public Edge(CellInfo destination, int speed, float damage, bool canEndMovementHere)
        {
            Destination = destination;
            Speed = speed;
            Damage = damage;
            CanEndMovementHere = canEndMovementHere;
        }

        public override string ToString()
        {
            return string.Format("Destination: {0}, Speed Used: {1}, Damage Taken: {2}, CanEndMovementHere: {3}",
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
