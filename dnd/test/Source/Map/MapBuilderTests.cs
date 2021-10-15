using dnd.Source.DI;
using dnd.Source.Map;
using NUnit.Framework;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace test.Source.Map
{
    [TestFixture]
    class MapBuilderTests
    {
        IMapBuilder builder;
        [SetUp]
        public void BeforeEachTest()
        {
            new DndModule().RegisterRules();
            builder = Locator.Current.GetService<IMapBuilder>();
        }
    }
}
