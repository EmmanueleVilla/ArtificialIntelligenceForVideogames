using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    class PatientDefenceAction : IAvailableAction
    {
        public string ActionEconomy = "A";
        public ActionsTypes ActionType => ActionsTypes.PatientDefence;
        public string Description => "(" + ActionEconomy + ") Patient Defence";
    }
}