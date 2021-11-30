using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    class FightingSpiritAction : IAvailableAction
    {
        public string ActionEconomy = "B";
        public ActionsTypes ActionType => ActionsTypes.FightingSpirit;
        public string Description => "(" + ActionEconomy + ") Fighting Spirit";
    }
}