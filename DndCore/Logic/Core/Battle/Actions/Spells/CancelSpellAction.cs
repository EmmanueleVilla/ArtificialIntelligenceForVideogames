using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Spells
{
    public class CancelSpellAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public ActionsTypes ActionType => ActionsTypes.CancelSpell;
        public string Description => "Cancel Spell";
    }
}
