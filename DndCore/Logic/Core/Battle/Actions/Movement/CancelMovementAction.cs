using Core.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Movement
{
    public class CancelMovementAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public ActionsTypes ActionType => ActionsTypes.CancelMovement;
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>();
        public string Description => "Cancel Movement";
        public int Priority => 0;
    }
}
