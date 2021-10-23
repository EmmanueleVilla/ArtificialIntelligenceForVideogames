using Core.DI;
using NUnit.Framework;

namespace Core.Map
{
    [TestFixture]
    class DndMapTests
    {
        [Test]
        [TestCase(-1, 0, null)]
        [TestCase(0, -1, null)]
        [TestCase(10, 0, null)]
        [TestCase(0, 10, null)]
        [TestCase(1, 1, "G")]
        [TestCase(1, 9, "G")]
        [TestCase(9, 1, "G")]
        [TestCase(9, 9, "G")]
        [Parallelizable(ParallelScope.All)]
        public void CheckGetCorrectTerrainType(int i, int j, string expected)
        {
            var map = new DeltaDndMap(10, 10, "G", 0);
            Assert.AreEqual(expected, map.GetCellInfo(i, j).Terrain);
        }

        [Test]
        public void CheckTerramorphSingleCell()
        {
            var map = new DeltaDndMap(10, 10, "G", 0);
            map.SetCellTerrain(5, 5, "W");
            Assert.AreEqual("W", map.GetCellInfo(5, 5).Terrain);
            Assert.AreEqual("G", map.GetCellInfo(5, 6).Terrain);
            Assert.AreEqual("G", map.GetCellInfo(6, 5).Terrain);
            Assert.AreEqual("G", map.GetCellInfo(4, 5).Terrain);
            Assert.AreEqual("G", map.GetCellInfo(5, 4).Terrain);
            Assert.AreEqual("G", map.GetCellInfo(4, 6).Terrain);
            Assert.AreEqual("G", map.GetCellInfo(6, 4).Terrain);
            Assert.AreEqual("G", map.GetCellInfo(6, 6).Terrain);
            Assert.AreEqual("G", map.GetCellInfo(4, 4).Terrain);
        }
    }
}