using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tests
{
    class ATestClass : ITestClass
    {
        public void Execute()
        {
            var methods = GetType()
                      .GetMethods()
                      .Where(m => m.GetCustomAttributes(typeof(TestAttribute), false).Length > 0)
                      .ToArray();
            foreach(var method in methods)
            {
                method.Invoke(this, new object[] { });
                Debug.Log(string.Format("Test {0} passed.", method.Name));
            }
        }
    }
}
