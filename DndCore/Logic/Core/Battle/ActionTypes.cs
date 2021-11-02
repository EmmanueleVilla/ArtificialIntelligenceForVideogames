using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle
{
    public class AvailableActions
    {
        public List<Speed> Movements { get; protected set; }

        public AvailableActions(List<Speed> movements)
        {
            Movements = movements;
        }
    }
}
