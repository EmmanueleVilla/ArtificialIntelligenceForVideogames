﻿using dnd.Source.DI;
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
            Assert.AreEqual(expected, map.GetCellInfo(i, j).Terrain);
        }

        [Test]
        public void CheckTerramorphSingleCell()
        {
            IMap map = new DndMap(10, 10, TerrainTypes.Grass);
            map.SetCellTerrain(5, 5, TerrainTypes.Water);
            Assert.AreEqual(TerrainTypes.Water, map.GetCellInfo(5, 5).Terrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellInfo(5, 6).Terrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellInfo(6, 5).Terrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellInfo(4, 5).Terrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellInfo(5, 4).Terrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellInfo(4, 6).Terrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellInfo(6, 4).Terrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellInfo(6, 6).Terrain);
            Assert.AreEqual(TerrainTypes.Grass, map.GetCellInfo(4, 4).Terrain);
        }
    }
}
