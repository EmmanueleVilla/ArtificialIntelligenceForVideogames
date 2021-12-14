using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    abstract class AGoal : IGoal
    {
        public float Value { get; set; }

        public AGoal(float value)
        {
            Value = value;
        }
    }
}
