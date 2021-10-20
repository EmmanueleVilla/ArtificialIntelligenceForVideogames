using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Map
{
    public class DndMap : IMap
    {
        public readonly int Width;
        public readonly int Height;
        public readonly TerrainTypes DefaultTerrain;
        private Dictionary<int, TerrainTypes> terrainDelta = new Dictionary<int, TerrainTypes>();

        public readonly int DefaultHeight;
        private Dictionary<int, int> heightDelta = new Dictionary<int, int>();

        public DndMap(int width, int height, TerrainTypes defaultTerrain)
        {
            this.Width = width;
            this.Height = height;
            this.DefaultTerrain = defaultTerrain;
        }

        public CellInfo GetCellInfo(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                TerrainTypes terrain;
                int height = 0;
                terrainDelta.TryGetValue(x * Width + y, out terrain);
                heightDelta.TryGetValue(x * Width + y, out height);
                if (terrain == TerrainTypes.Void)
                {
                    terrain = DefaultTerrain;
                    height = int.MaxValue;
                }
                return new CellInfo(terrain, height);
            }
            else
            {
                return new CellInfo(TerrainTypes.Void, int.MaxValue);
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

        public void SetCellTerrain(int x, int y, TerrainTypes terrainType)
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
