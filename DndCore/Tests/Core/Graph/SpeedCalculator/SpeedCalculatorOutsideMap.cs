using Core.Map;
using Logic.Core.Graph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorOutsideMap: ASpeedCalculatorBaseTests
    {
        [Test]
        public void NullWhenOutsideMap()
        {
            var to = new CellInfo(' ', 0);
            Assert.AreEqual(Edge.Empty(), speedCalculator.GetNeededSpeed(new WalkerCreatureMock(), CellInfo.Empty(), to, null));
        }
    }
}
