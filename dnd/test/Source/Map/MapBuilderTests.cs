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
            "1,2,3,1,1,1\n" +
            "1,1,1,1,1,1\n" +
            "1,1,1,1,1,1", TerrainTypes.Grass)]
        [TestCase("" +
            "1,2,3,2,2,2\n" +
            "2,2,2,2,2,2\n" +
            "2,2,2,2,2,2", TerrainTypes.Water)]
        public void CheckBuildMapFromStringAndUsesCorrectDefaultTerrainToSaveSpace(string csv, TerrainTypes expectedDefault)
        {
            
            var map = builder.FromCSV(csv) as DndMap;
            Assert.AreEqual(6, map.Width);
            Assert.AreEqual(3, map.Height);
            Assert.AreEqual(expectedDefault, map.DefaultTerrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(0, 0));
            Assert.AreEqual(TerrainTypes.Water, map.GetCellType(1, 0));
            Assert.AreEqual(TerrainTypes.Rock, map.GetCellType(2, 0));
        }

        [Test]
        [TestCase("" +
            "1,2,3,1,1,1\n" +
            "1,1,1,1,1,1\n" +
            "1,1,1,1,1,1")]
        [TestCase("" +
            "1,2,3,2,2,2\n" +
            "2,2,2,2,2,2\n" +
            "2,2,2,2,2,2")]
        public void CheckBuildMapFromStringWithCSVRepresentation(string csv)
        {
            var map = builder.FromCSV(csv) as DndMap;
            Assert.AreEqual(csv, map.ToCSV());
        }

    }
}
