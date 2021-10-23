using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Map
{
    public class CellInfo
    {
        public string Terrain { get; private set; }
        public int Height { get; private set; }

        public CellInfo(string terrain, int heigth)
        {
            Terrain = terrain;
            Height = heigth;
        }
    }
}
