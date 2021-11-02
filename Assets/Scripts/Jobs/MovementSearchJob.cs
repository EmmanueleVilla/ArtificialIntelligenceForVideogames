using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Jobs
{
    struct MovementSearchJob : IJob
    {
        public NativeArray<int> result;
        public void Execute()
        {
            var executor = new VeryTimeConsumingClass();
            result[0] = executor.VeryTimeConsumingMethod();
        }
    }
}
