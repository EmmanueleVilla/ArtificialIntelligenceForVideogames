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

        [Test]
        public void CheckTerramorphSingleCell()
        {
            IMap map = new DndMap(10, 10, TerrainTypes.Grass);
            map.SetCell(5, 5, TerrainTypes.Water);
            Assert.AreEqual(TerrainTypes.Water, map.GetCellType(5, 5));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(5, 6));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(6, 5));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(4, 5));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(5, 4));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(4, 6));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(6, 4));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(6, 6));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(4, 4));
        }

        [Test]
        public void CheckTerramorphMultipleCells()
        {
            IMap map = new DndMap(10, 10, TerrainTypes.Grass);
            map.SetCells(5, 5, 2, 2, TerrainTypes.Water);
            Assert.AreEqual(TerrainTypes.Water, map.GetCellType(5, 5));
            Assert.AreEqual(TerrainTypes.Water, map.GetCellType(5, 6));
            Assert.AreEqual(TerrainTypes.Water, map.GetCellType(6, 5));
            Assert.AreEqual(TerrainTypes.Water, map.GetCellType(6, 6));
            
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(4, 4));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(5, 4));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(6, 4));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(7, 4));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(4, 5));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(5, 7));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(4, 6));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(6, 7));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(4, 7));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(5, 7));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(6, 7));
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellType(7, 7));

        }
    }
}
