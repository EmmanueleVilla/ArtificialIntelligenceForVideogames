using Core.DI;
using Logic.Core.Battle;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.DI
{
    [TestFixture]
    class DnDModuleTests: BaseDndTest
    {
        [Test]
        public void GetBattle()
        {
            DndModule.RegisterRules(false);
            Assert.NotNull(DndModule.Get<IDndBattle>());
        }
    }
}
