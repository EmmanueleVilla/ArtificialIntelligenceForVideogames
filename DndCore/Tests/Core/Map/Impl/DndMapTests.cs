using Core.DI;
using Logic.Core.Map;
using Logic.Core.Map.Impl;
using NUnit.Framework;
using Tests.Core;

namespace Core.Map
{
    [TestFixture]
    class DndMapTests: BaseDndTest
    {
        [Test]
        [TestCase(-1, 0, ' ')]
        [TestCase(0, -1, ' ')]
        [TestCase(10, 0, ' ')]
        [TestCase(0, 10, ' ')]
        [TestCase(1, 1, 'G')]
        [TestCase(1, 9, 'G')]
        [TestCase(9, 1, 'G')]
        [TestCase(9, 9, 'G')]
        public void CheckGetCorrectTerrainType(int i, int j, char expected)
        {
            var map = new ArrayDndMap(10, 10, new CellInfo('G', 0));
            Assert.AreEqual(expected, map.GetCellInfo(i, j).Terrain);
        }

        [Test]
        public void CheckTerramorphSingleCell()
        {
            var map = new ArrayDndMap(10, 10, new CellInfo('G', 0));
            map.SetCell(5, 5, new CellInfo('W', 0));
            Assert.AreEqual('W', map.GetCellInfo(5, 5).Terrain);
            Assert.AreEqual('G', map.GetCellInfo(5, 6).Terrain);
            Assert.AreEqual('G', map.GetCellInfo(6, 5).Terrain);
            Assert.AreEqual('G', map.GetCellInfo(4, 5).Terrain);
            Assert.AreEqual('G', map.GetCellInfo(5, 4).Terrain);
            Assert.AreEqual('G', map.GetCellInfo(4, 6).Terrain);
            Assert.AreEqual('G', map.GetCellInfo(6, 4).Terrain);
            Assert.AreEqual('G', map.GetCellInfo(6, 6).Terrain);
            Assert.AreEqual('G', map.GetCellInfo(4, 4).Terrain);
        }
    }
}