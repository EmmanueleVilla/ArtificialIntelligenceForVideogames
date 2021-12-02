using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    class DashAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Action;
        public ActionsTypes ActionType => ActionsTypes.Dash;
        public string Description => "(" + ActionEconomy + ") Dash";
    }
}