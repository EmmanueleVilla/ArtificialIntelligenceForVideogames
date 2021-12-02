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
    }
}