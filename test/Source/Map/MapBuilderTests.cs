using dnd.Source.DI;
using dnd.Source.Map;
using NUnit.Framework;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace test.Source.Map
{
    [TestFixture]
    class MapBuilderTests
    {
        IMapBuilder builder;
        [SetUp]
        public void BeforeEachTest()
        {
            new DndModule().RegisterRules();
            builder = Locator.Current.GetService<IMapBuilder>();
        }


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
            var map = builder.FromCsv(csv) as DndMap;
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
