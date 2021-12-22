using Core.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions
{
    public class EndTurnAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public ActionsTypes ActionType => ActionsTypes.EndTurn;

        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>() { CellInfo.Empty() };
        public string Description => "End turn";
    }
}
