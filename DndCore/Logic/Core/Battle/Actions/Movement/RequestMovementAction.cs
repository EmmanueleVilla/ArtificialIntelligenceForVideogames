using Core.Map;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Battle.Actions
{
    public class RequestMovementAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public ActionsTypes ActionType => ActionsTypes.RequestMovement;
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>();
        public string Description => "(M) " + string.Join(",", RemainingMovement.Select(x => string.Format(x.Movement +"="+(x.Square*1.5)+"m")));

        public List<Speed> RemainingMovement = new List<Speed>();
    }
}
