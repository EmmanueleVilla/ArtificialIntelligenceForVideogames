using DndCore.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    public class PatientDefenseAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.BonusAction;
        public ActionsTypes ActionType => ActionsTypes.PatientDefence;
        public string Description => "(" + ActionEconomy + ") Patient Defense";
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>() { CellInfo.Empty() };
        public int Priority => 1;
    }
}