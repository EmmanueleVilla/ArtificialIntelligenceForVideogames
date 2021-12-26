using Core.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    public class FlurryOfBlowsAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public ActionsTypes ActionType => ActionsTypes.FlurryOfBlows;
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>() { CellInfo.Empty() };
        public string Description => "Flurry of blows";
        public int Priority => 3;
    }
}
