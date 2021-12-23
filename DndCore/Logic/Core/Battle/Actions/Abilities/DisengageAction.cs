using Core.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    class DisengageAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Action;
        public ActionsTypes ActionType => ActionsTypes.Disengage;
        public string Description => "(" + ActionEconomy + ") Disengage";
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>() { CellInfo.Empty() };
        public int Priority => 1;
    }
}