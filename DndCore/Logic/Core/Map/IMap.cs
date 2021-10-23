using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Map
{
    public interface IMap
    {
        int Height { get; }
        int Width { get; }
        CellInfo GetCellInfo(int x, int y);
        void SetCellTerrain(int x, int y, string terrainType);
        void SetCellHeight(int x, int y, int height);
    }
}
