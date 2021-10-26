using Logic.Core.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Scripts.Jobs
{
    struct VeryTimeConsumingJob : IJob
    {
        public NativeArray<int> result;

        public void Execute()
        {
            var executor = new VeryTimeConsumingClass();
            result[0] = executor.VeryTimeConsumingMethod();
        }
    }
}
