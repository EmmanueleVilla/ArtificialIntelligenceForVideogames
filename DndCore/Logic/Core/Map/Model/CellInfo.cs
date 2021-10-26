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
            return new CellInfo(other.Terrain, other.Height, other.Creature);
        }

        public static CellInfo Empty()
        {
            return new CellInfo(' ', 0);
        }
    }
}
