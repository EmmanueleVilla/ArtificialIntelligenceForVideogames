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
        private Dictionary<Tuple<int, int>, TerrainTypes> delta = new Dictionary<Tuple<int, int>, TerrainTypes>();

        public DndMap(int width, int height, TerrainTypes defaultTerrain)
        {
            this.Width = width;
            this.Height = height;
            this.DefaultTerrain = defaultTerrain;
        }

        public TerrainTypes GetCellType(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                TerrainTypes terrain;
                delta.TryGetValue(new Tuple<int,int>(x,y), out terrain);
                if(terrain == TerrainTypes.Void)
                {
                    return DefaultTerrain;
                }
                return terrain;
            }

            return TerrainTypes.Void;
        }

        public void SetCell(int x, int y, TerrainTypes terrainType)
        {
            SetCells(x, y, 1, 1, terrainType);
        }

        public void SetCells(int x, int y, int width, int height, TerrainTypes terrainType)
        {
            var keys = new List<Tuple<int, int>>();
            var maxX = x + width;
            var maxY = y + height;
            for (int i = x; i < maxX; i++)  {
                for (int j = y; j < maxY; j++)
                {
                    keys.Add(new Tuple<int,int>(i, j));
                }
            }

            keys.ForEach(k => delta.Remove(k));

            if(terrainType == DefaultTerrain)
            {
                return;
            }

            keys.ForEach(k => delta.Add(k, terrainType));
        }

        public string ToCSV()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                var row = new List<int>();
                for (int j = 0; j < Width; j++)
                {
                    row.Add((int)GetCellType(j, i));
                }
                builder.AppendLine(string.Join(",", row));
            }

            return builder.ToString().Substring(0, builder.Length - 2).Replace("\r", "");
        }
    }
}
