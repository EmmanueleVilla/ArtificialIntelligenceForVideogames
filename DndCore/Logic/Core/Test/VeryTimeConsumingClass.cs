using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Test
{
    public class VeryTimeConsumingClass
    {
        public int VeryTimeConsumingMethod() {
            var sum = 0;
            for(int i=0; i<100000; i++)
            {
                for (int j = 0; j < 100000; j++)
                {
                    sum += i*j;
                }
            }
            return sum;
        }
    }
}
