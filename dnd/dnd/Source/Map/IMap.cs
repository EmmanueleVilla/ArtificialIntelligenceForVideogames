using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Map
{
    public interface IMap
    {
        TerrainTypes GetCellType(int x, int y);
        void SetCells(int x, int y, int width, int height, TerrainTypes terrainType);
        void SetCell(int x, int y, TerrainTypes terrainType);
    }
}
