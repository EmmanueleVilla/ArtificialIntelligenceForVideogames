using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Map
{
    public class CellInfo
    {
        public TerrainTypes Terrain { get; private set; }
        public int Height { get; private set; }

        public CellInfo(TerrainTypes terrain, int heigth)
        {
            Terrain = terrain;
            Height = heigth;
        }
    }
}
