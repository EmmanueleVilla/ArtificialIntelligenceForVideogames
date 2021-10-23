using Core.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Map
{
    class FullDndMap : IMap
    {
        public readonly int Width;
        public readonly int Height;
        private Dictionary<int, CellInfo> cells = new Dictionary<int, CellInfo>();

        public FullDndMap(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public CellInfo GetCellInfo(int x, int y)
        {
            CellInfo info;
            cells.TryGetValue(x * Width + y, out info);
            return info;
        }

        public void SetCell(int x, int y, CellInfo info)
        {
            var key = x * Width + y;
            cells.Remove(key);
            cells.Add(key, info);
        }

        public void SetCellTerrain(int x, int y, string terrainType)
        {
            //throw new NotImplementedException();
        }

        public void SetCellHeight(int x, int y, int height)
        {
            //throw new NotImplementedException();
        }
    }
}
