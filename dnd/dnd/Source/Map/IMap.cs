using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Map
{
    public interface IMap
    {
        TerrainTypes GetCellType(int x, int y);
    }
}
