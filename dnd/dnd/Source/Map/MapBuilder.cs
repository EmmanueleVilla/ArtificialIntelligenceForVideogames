using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dnd.Source.Map
{
    class MapBuilder : IMapBuilder
    {
        public IMap FromCSV(string content)
        {
            var rows = content.Split('\n');
            var y = 0;
            var x = 0;
            var width = 0;
            var height = 0;
            var map = new Dictionary<Tuple<int, int>, TerrainTypes>();
            foreach(var row in rows)
            {
                var columns = row.Split(',');
                foreach(var column in columns)
                {
                    map.Add(new Tuple<int, int>(y, x), (TerrainTypes)int.Parse(column));
                    y++;
                    width = Math.Max(width, y);
                }
                x++;
                y = 0;
            }
            height = Math.Max(height, x);

            var list = map.GroupBy(c => c.Value).OrderByDescending(c => c.Count());
            var max = list.First().Key;

            var m = new DndMap(width, height, max);

            list.Where(c => c.Key != max).ToList().ForEach(t =>
            {
                t.ToList().ForEach(c => m.SetCell(c.Key.Item1, c.Key.Item2, t.Key));
            });

            return m;
        }
    }
}
