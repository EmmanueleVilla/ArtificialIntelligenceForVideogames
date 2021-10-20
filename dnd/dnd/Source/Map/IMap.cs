using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Map
{
    public interface IMap
    {
        CellInfo GetCellInfo(int x, int y);
        void SetCellTerrain(int x, int y, string terrainType);
        void SetCellHeight(int x, int y, int height);
    }
}
