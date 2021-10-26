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

        public CellInfo(char terrain, byte heigth, ICreature creature = null)
        {
            Terrain = terrain;
            Height = heigth;
            Creature = creature;
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
