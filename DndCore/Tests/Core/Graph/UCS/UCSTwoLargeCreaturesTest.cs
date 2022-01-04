using Core.DI;
using Logic.Core.Graph;
using Logic.Core.Map;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph.UCS
{
    [TestFixture]
    class UCSTwoLargeCreaturesTest
    {
        [SetUp]
        public void Setup()
        {
            DndModule.RegisterRulesForTest();
            var mapCsv =
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0";
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var first = WalkerCreatureMock.Build(2);
            map.AddCreature(first, 0, 0);

            var second = WalkerCreatureMock.Build(2);
            map.AddCreature(second, 2, 2);

            var from = map.GetCellInfo(0, 0);

            var path = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map);

            Assert.AreEqual(5, path.Count);
        }
    }
}
