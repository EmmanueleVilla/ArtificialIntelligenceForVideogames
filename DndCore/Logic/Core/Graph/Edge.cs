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
    }
}
