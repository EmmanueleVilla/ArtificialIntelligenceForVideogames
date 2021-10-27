using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Map
{
    public struct CellInfo
    {
        public char Terrain;
        public byte Height;
        public ICreature Creature;
        public int X;
        public int Y;

        public CellInfo(char terrain, byte heigth, ICreature creature = null, int x = 0, int y = 0)
        {
            Terrain = terrain;
            Height = heigth;
            Creature = creature;
            X = x;
            Y = y;
        }

        public static CellInfo Copy(CellInfo other)
        {
            return new CellInfo(other.Terrain, other.Height, other.Creature, other.X, other.Y);
        }

        public static CellInfo Empty()
        {
            return new CellInfo(' ', 0);
        }

        public override string ToString()
        {
            return string.Format("[CellInfo: Terrain={0}, Height={1}, Creature={2}, X={3}, Y={4}]",
                Terrain.ToString(),
                Height.ToString(),
                Creature?.ToString(),
                X.ToString(),
                Y.ToString());
        }
    }
}
