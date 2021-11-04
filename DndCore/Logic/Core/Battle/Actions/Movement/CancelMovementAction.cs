using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Movement
{
    public class CancelMovementAction : IAvailableAction
    {
        public ActionsTypes ActionType => ActionsTypes.CancelMovement;

        public string Description => "Cancel Movement";
    }
}
