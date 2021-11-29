using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Battle.Actions
{
    public class RequestMovementAction : IAvailableAction
    {
        public ActionsTypes ActionType => ActionsTypes.RequestMovement;

        public string Description => "(M) " + string.Join(",", RemainingMovement.Select(x => string.Format(x.Item1 +"="+(x.Item2*1.5)+"m")));

        public List<Speed> RemainingMovement = new List<Speed>();
    }
}
