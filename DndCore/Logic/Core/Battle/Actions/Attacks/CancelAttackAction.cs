using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Attacks
{
    public class CancelAttackAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public ActionsTypes ActionType => ActionsTypes.CancelAttack;

        public string Description => "Cancel Attack";
    }
}
