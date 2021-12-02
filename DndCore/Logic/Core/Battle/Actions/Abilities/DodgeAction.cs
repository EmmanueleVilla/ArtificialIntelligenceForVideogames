using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    class DodgeAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Action;
        public ActionsTypes ActionType => ActionsTypes.Dodge;
        public string Description => "(" + ActionEconomy + ") Dodge";
    }
}
