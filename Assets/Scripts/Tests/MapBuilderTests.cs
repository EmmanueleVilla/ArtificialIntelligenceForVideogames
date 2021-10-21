using Assets.Scripts.Tests;
using dnd.Source.DI;
using dnd.Source.Map;
using UnityEngine.Assertions;

namespace test.Source.Map
{
    class MapBuilderTests: ATestClass
    {
        IMapBuilder builder = DndModule.Get<IMapBuilder>();

        [Test]
        public void CheckBuildMapFromStringAndUsesCorrectDefaultTerrainToSaveSpaceInternal()
        {
            CheckBuildMapFromStringAndUsesCorrectDefaultTerrainToSaveSpaceInternal("" +
            "G,H,I,L,M,N\n" +
            "G,G,G,G,G,G\n" +
            "G,G,G,G,G,G", "G");

            CheckBuildMapFromStringAndUsesCorrectDefaultTerrainToSaveSpaceInternal("" +
            "G,H,I,L,M,N\n" +
            "H,H,H,H,H,H\n" +
            "H,H,H,H,H,H", "H");
        }

        protected void CheckBuildMapFromStringAndUsesCorrectDefaultTerrainToSaveSpaceInternal(string csv, string expectedDefault)
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
