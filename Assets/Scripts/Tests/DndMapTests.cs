using Assets.Scripts.Tests;
using dnd.Source.DI;
using dnd.Source.Map;
using UnityEngine.Assertions;

namespace test.Source.Map
{
    class DndMapTests: ATestClass
    {
        [Test]
        public void CheckGetCorrectTerrainType()
        {
            CheckGetCorrectTerrainTypeInternal(-1, 0, null);
            CheckGetCorrectTerrainTypeInternal(0, -1, null);
            CheckGetCorrectTerrainTypeInternal(10, 0, null);
            CheckGetCorrectTerrainTypeInternal(0, 10, null);
            CheckGetCorrectTerrainTypeInternal(1, 1, "G");
            CheckGetCorrectTerrainTypeInternal(1, 9, "G");
            CheckGetCorrectTerrainTypeInternal(9, 1, "G");
            CheckGetCorrectTerrainTypeInternal(9, 9, "G");
        }

        protected void CheckGetCorrectTerrainTypeInternal(int i, int j, string expected)
        {
            IMap map = new DndMap(10, 10, "G", 0);
            Assert.AreEqual(expected, map.GetCellInfo(i, j).Terrain);
        }

        [Test]
        public void CheckTerramorphSingleCell()
        {
            IMap map = new DndMap(10, 10, "G", 0);
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
