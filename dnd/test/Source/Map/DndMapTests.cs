using dnd.Source.DI;
using dnd.Source.Map;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace test.Source.Map
{
    [TestFixture]
    class DndMapTests
    {
        [SetUp]
        public void BeforeEachTest()
        {
            new DndModule().RegisterRules();
        }

        [Test]
        [TestCase(-1, 0, TerrainTypes.Void)]
        [TestCase(0, -1, TerrainTypes.Void)]
        [TestCase(10, 0, TerrainTypes.Void)]
        [TestCase(0, 10, TerrainTypes.Void)]
        [TestCase(1, 1, TerrainTypes.Grass)]
        [TestCase(1, 9, TerrainTypes.Grass)]
        [TestCase(9, 1, TerrainTypes.Grass)]
        [TestCase(9, 9, TerrainTypes.Grass)]
        [Parallelizable(ParallelScope.All)]
        public void CheckGetCorrectTerrainType(int i, int j, TerrainTypes expected)
        {
            IMap map = new DndMap(10, 10, TerrainTypes.Grass);
            Assert.AreEqual(expected, map.GetCellType(i, j));
        }
    }
}
