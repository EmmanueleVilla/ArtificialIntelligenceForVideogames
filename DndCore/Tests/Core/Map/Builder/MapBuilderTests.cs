using Core.DI;
using Core.Map;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace test.Source.Map
{
    [TestFixture]
    class MapBuilderTests
    {
        CsvDeltaMapBuilder builder = new CsvDeltaMapBuilder();

        [Test]
        [TestCase("" +
            "G,H,I,L,M,N\n" +
            "G,G,G,G,G,G\n" +
            "G,G,G,G,G,G", "G")]
        [TestCase("" +
            "G,H,I,L,M,N\n" +
            "H,H,H,H,H,H\n" +
            "H,H,H,H,H,H", "H")]
        public void CheckBuildMapFromStringAndUsesCorrectDefaultTerrainToSaveSpace(string csv, string expectedDefault)
        {
            var map = builder.FromCsv(csv) as DeltaDndMap;
            Assert.AreEqual(6, map.Width);
            Assert.AreEqual(3, map.Height);
            Assert.AreEqual(expectedDefault, map.DefaultTerrain);
            Assert.AreEqual("G", map.GetCellInfo(0, 0).Terrain);
            Assert.AreEqual("H", map.GetCellInfo(1, 0).Terrain);
            Assert.AreEqual("I", map.GetCellInfo(2, 0).Terrain);
            Assert.AreEqual("L", map.GetCellInfo(3, 0).Terrain);
            Assert.AreEqual("M", map.GetCellInfo(4, 0).Terrain);
            Assert.AreEqual("N", map.GetCellInfo(5, 0).Terrain);
        }

    }
}
