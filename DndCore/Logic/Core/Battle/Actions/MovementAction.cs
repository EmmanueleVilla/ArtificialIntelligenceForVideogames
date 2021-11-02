using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions
{
    public class MovementAction : IAvailableAction
    {
        public ActionsTypes ActionType => ActionsTypes.Movement;

        public List<Speed> RemainingMovement = new List<Speed>();
    }
}
