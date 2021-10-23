using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Map
{
    public class DeltaDndMap : IMap
    {
        public readonly int Width;
        public readonly int Height;
        public readonly string DefaultTerrain;
        public readonly int DefaultHeight;
        private Dictionary<int, string> terrainDelta = new Dictionary<int, string>();
        private Dictionary<int, int> heightDelta = new Dictionary<int, int>();

        public DeltaDndMap(int width, int height, string defaultTerrain, int defaultHeight)
        {
            Width = width;
            Height = height;
            DefaultTerrain = defaultTerrain;
            DefaultHeight = defaultHeight;
        }

        public CellInfo GetCellInfo(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                string terrain;
                int height;
                terrainDelta.TryGetValue(x * Width + y, out terrain);
                heightDelta.TryGetValue(x * Width + y, out height);
                return new CellInfo(terrain ?? DefaultTerrain, height);
            }
            else
            {
                return new CellInfo(null, int.MaxValue);
            }
        }

        public void SetCellHeight(int x, int y, int height)
        {
            var key = x * Width + y;
            heightDelta.Remove(key);

            if (height == DefaultHeight)
            {
                return;
            }

            heightDelta.Add(key, height);
        }

        public void SetCellTerrain(int x, int y, String terrainType)
        {
            var key = x * Width + y;
            terrainDelta.Remove(key);

            if (terrainType == DefaultTerrain)
            {
                return;
            }

            terrainDelta.Add(key, terrainType);
        }
    }
}
