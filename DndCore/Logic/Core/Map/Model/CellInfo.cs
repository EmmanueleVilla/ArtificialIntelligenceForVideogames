using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Map
{
    public struct CellInfo
    {
        public char Terrain;
        public byte Height;

        public CellInfo(char terrain, byte heigth)
        {
            Terrain = terrain;
            Height = heigth;
        }

        public static CellInfo Copy(CellInfo other)
        {
            return new CellInfo(other.Terrain, other.Height);
        }

        public static CellInfo Empty()
        {
            return new CellInfo(' ', 0);
        }
    }
}
