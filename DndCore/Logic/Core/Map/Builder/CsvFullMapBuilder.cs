﻿using Core.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Map
{
    class CsvFullMapBuilder : IMapBuilder
    {
        public IMap FromCsv(string content)
        {
            var rows = content.Split('\n');
            var y = 0;
            var x = 0;
            var width = 0;
            var height = 0;
            var terrains = new Dictionary<Tuple<int, int>, CellInfo>();
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
                    terrains.Add(new Tuple<int, int>(y, x), new CellInfo(terrainType, terrainHeight));
                    y++;
                    width = Math.Max(width, y);
                }
                x++;
                y = 0;
            }
            height = Math.Max(height, x);

            var m = new FullDndMap(width, height);

            terrains.ToList().ForEach(t =>
            {
                m.SetCell(t.Key.Item1, t.Key.Item2, t.Value);
            });

            return m;
        }

    }
}
