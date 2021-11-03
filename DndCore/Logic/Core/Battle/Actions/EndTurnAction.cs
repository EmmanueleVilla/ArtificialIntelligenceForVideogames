using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions
{
    public class EndTurnAction : IAvailableAction
    {
        public ActionsTypes ActionType => ActionsTypes.EndTurn;

        public string Description => "End turn";
    }
}
