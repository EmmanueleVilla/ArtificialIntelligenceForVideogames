using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    class SecondWindAction : IAvailableAction
    {
        public string ActionEconomy = "B";
        public ActionsTypes ActionType => ActionsTypes.SecondWind;
        public string Description => "(" + ActionEconomy + ") Second Wind";
    }
}