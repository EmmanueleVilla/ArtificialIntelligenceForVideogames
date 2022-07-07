
using DndCore.Map;
using System.Collections.Generic;

namespace Logic.Core.Battle.Actions
{
    public interface IAvailableAction
    {
        BattleActions ActionEconomy { get; set; }
        ActionsTypes ActionType { get; }
        List<CellInfo> ReachableCells { get; set; }
        string Description { get; }

        int Priority { get; }
    }
}
