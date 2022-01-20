using Core.DI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core
{
    [TestFixture]
    class BaseDndTest
    {
        [SetUp]
        public void Setup()
        {
            DndModule.RegisterRules();
        }
    }
}
