using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dnd.Source.Map
{
    class CsvMapBuilder : IMapBuilder
    {
        private int width;
        private int height;
        private TerrainTypes defaultTerrain;
        private IOrderedEnumerable<IGrouping<TerrainTypes, KeyValuePair<Tuple<int, int>, TerrainTypes>>> terrainList;
        private int defaultHeight;
        private IOrderedEnumerable<IGrouping<int, KeyValuePair<Tuple<int, int>, int>>> heightsList;

        public IMap Build()
        {
            if(terrainList == null)
            {
                throw new Exception("Error building map, terrain not initialized");
            }

            var m = new DndMap(width, height, defaultTerrain);

            terrainList.Where(c => c.Key != defaultTerrain).ToList().ForEach(t =>
            {
                t.ToList().ForEach(c => m.SetCellTerrain(c.Key.Item1, c.Key.Item2, t.Key));
            });

            return m;
        }


        public IMapBuilder WithHeights(string content)
        {
            var rows = content.Split('\n');
            var map = new Dictionary<Tuple<int, int>, int>();
            var y = 0;
            var x = 0;
            foreach (var row in rows)
            {
                var columns = row.Split(',');
                foreach (var column in columns)
                {
                    map.Add(new Tuple<int, int>(y, x), int.Parse(column));
                    y++;
                }
                x++;
                y = 0;
            }
            heightsList = map.GroupBy(c => c.Value).OrderByDescending(c => c.Count());
            defaultHeight = heightsList.First().Key;
            return this;
        }

        public IMapBuilder WithTerrains(string content)
        {
            var rows = content.Split('\n');
            var y = 0;
            var x = 0;
            width = 0;
            height = 0;
            var map = new Dictionary<Tuple<int, int>, TerrainTypes>();
            foreach (var row in rows)
            {
                var columns = row.Split(',');
                foreach (var column in columns)
                {
                    map.Add(new Tuple<int, int>(y, x), (TerrainTypes)int.Parse(column));
                    y++;
                    width = Math.Max(width, y);
                }
                x++;
                y = 0;
            }
            height = Math.Max(height, x);

            terrainList = map.GroupBy(c => c.Value).OrderByDescending(c => c.Count());
            defaultTerrain = terrainList.First().Key;

            return this;
        }
    }
}
