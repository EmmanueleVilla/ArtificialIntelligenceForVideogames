using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public class CsvMapBuilder : IMapBuilder
    {
        public IMap FromCsv(string content)
        {
            var rows = content.Split('\n');
            var y = 0;
            var x = 0;
            var width = 0;
            var height = 0;
            var terrains = new Dictionary<Tuple<int, int>, string>();
            var heights = new Dictionary<Tuple<int, int>, int>();
            foreach (var row in rows)
            {
                var columns = row.Split(',');
                foreach (var column in columns)
                {
                    var terrainType = column.Substring(0, 1);
                    var terrainHeight = 0;
                    if (column.Length > 1)
                    {
                        terrainHeight = int.Parse(column.Substring(1, column.Length - 1));
                    }
                    terrains.Add(new Tuple<int, int>(y, x), terrainType);
                    heights.Add(new Tuple<int, int>(y, x), terrainHeight);
                    y++;
                    width = Math.Max(width, y);
                }
                x++;
                y = 0;
            }
            height = Math.Max(height, x);

            var terrainList = terrains.GroupBy(c => c.Value).OrderByDescending(c => c.Count());
            var defaultTerrain = terrainList.First().Key;
            var heightsList = heights.GroupBy(c => c.Value).OrderByDescending(c => c.Count());
            var defaultHeight = heightsList.First().Key;

            var m = new DndMap(width, height, defaultTerrain, defaultHeight);

            terrainList.Where(c => c.Key != defaultTerrain).ToList().ForEach(t =>
            {
                t.ToList().ForEach(c => m.SetCellTerrain(c.Key.Item1, c.Key.Item2, t.Key));
            });
            heightsList.Where(c => c.Key != defaultHeight).ToList().ForEach(t =>
            {
                t.ToList().ForEach(c => m.SetCellHeight(c.Key.Item1, c.Key.Item2, t.Key));
            });

            return m;
        }

    }
}
