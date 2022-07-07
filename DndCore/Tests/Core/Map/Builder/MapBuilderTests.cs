using DndCore.DI;
using DndCore.Map;
using Logic.Core.Map;
using Logic.Core.Map.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.Core;

namespace test.Source.Map
{
    [TestFixture]
    class MapBuilderTests: BaseDndTest
    {
        CsvFullMapBuilder builder = new CsvFullMapBuilder();

        [Test]
        public void CheckBuildMapFromString() {
            var csv =
            "G,H,I,L,M,N\n" +
            "G,G,G,G,G,G\n" +
            "G,G,G,G,G,G";
            var map = builder.FromCsv(csv) as ArrayDndMap;
            Assert.AreEqual(6, map.Width);
            Assert.AreEqual(3, map.Height);
            Assert.AreEqual(
                new CellInfo('G', 0, null, 0, 0),
                map.GetCellInfo(0, 0)
                );
            Assert.AreEqual(
                new CellInfo('H', 0, null, 1, 0),
                map.GetCellInfo(1, 0)
                );
            Assert.AreEqual(
                new CellInfo('I', 0, null, 2, 0),
                map.GetCellInfo(2, 0)
                );
            Assert.AreEqual(
                new CellInfo('L', 0, null, 3, 0),
                map.GetCellInfo(3, 0)
                );
            Assert.AreEqual(
                new CellInfo('M', 0, null, 4, 0),
                map.GetCellInfo(4, 0)
                );
            Assert.AreEqual(
                new CellInfo('N', 0, null, 5, 0),
                map.GetCellInfo(5, 0)
                );
        }

    }
}
