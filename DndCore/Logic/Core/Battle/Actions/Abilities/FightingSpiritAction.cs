using DndCore.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    class FightingSpiritAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.BonusAction;
        public ActionsTypes ActionType => ActionsTypes.FightingSpirit;
        public string Description => "(" + ActionEconomy + ") Fighting Spirit";
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>() { CellInfo.Empty() };
        public int Priority => 3;
    }
}