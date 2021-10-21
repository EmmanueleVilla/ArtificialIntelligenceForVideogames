using dnd.Source.DI;
using System.Collections.Generic;
using test.Source.Map;

namespace Assets.Scripts.Tests
{
    public static class Tests
    {
        public static void Execute() {
            var tests = new List<ITestClass>() { new DndMapTests(), new MapBuilderTests() };
            foreach(var test in tests)
            {
                test.Execute();
            }
        }
    }
}
