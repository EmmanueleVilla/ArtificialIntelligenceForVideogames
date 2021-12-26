using Core.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    class SecondWindAction : IAvailableAction
    {
        public ActionsTypes ActionType => ActionsTypes.SecondWind;
        public string Description => "(" + ActionEconomy + ") Second Wind";
        public BattleActions ActionEconomy { get; set; } = BattleActions.BonusAction;
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>() { CellInfo.Empty() };
        public int Priority => 3;
    }
}