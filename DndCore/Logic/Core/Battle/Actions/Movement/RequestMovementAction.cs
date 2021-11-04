using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions
{
    public class RequestMovementAction : IAvailableAction
    {
        public ActionsTypes ActionType => ActionsTypes.RequestMovement;

        public string Description => "Movement";

        public List<Speed> RemainingMovement = new List<Speed>();
    }
}
