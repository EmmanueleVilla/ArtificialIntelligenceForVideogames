using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Map
{
    public class DndMap : IMap
    {
        private readonly int width;
        private readonly int height;
        private readonly TerrainTypes defaultTerrain;
        private Dictionary<Tuple<int, int>, TerrainTypes> delta = new Dictionary<Tuple<int, int>, TerrainTypes>();

        public DndMap(int width, int height, TerrainTypes defaultTerrain)
        {
            this.width = width;
            this.height = height;
            this.defaultTerrain = defaultTerrain;
        }

        public TerrainTypes GetCellType(int x, int y)
        {
            if (x > 0 && x < width && y > 0 && y < height)
            {
                TerrainTypes terrain;
                delta.TryGetValue(new Tuple<int,int>(x,y), out terrain);
                if(terrain == TerrainTypes.Void)
                {
                    return defaultTerrain;
                }
                return terrain;
            }

            return TerrainTypes.Void;
        }
    }
}
