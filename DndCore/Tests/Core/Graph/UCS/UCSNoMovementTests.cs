using Core.Map;
using Logic.Core.Graph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks;
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.Graph.UCS
{
    [TestFixture]
    class UCSNoMovementTests
    {
        [Test]
        public void EmptyGraphIfCreatureCantMove()
        {
            Assert.IsEmpty(new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(new CellInfo(' ', 0, StillCreature.Build(1)), new EmptyMap()));
        }
    }
}
