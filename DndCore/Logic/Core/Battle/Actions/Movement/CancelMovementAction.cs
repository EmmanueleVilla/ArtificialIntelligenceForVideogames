using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Movement
{
    public class CancelMovementAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public ActionsTypes ActionType => ActionsTypes.CancelMovement;

        public string Description => "Cancel Movement";
    }
}
