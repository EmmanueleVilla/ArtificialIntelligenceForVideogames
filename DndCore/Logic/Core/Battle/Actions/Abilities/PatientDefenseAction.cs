﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    public class PatientDefenseAction : IAvailableAction
    {
        public string ActionEconomy = "B";
        public ActionsTypes ActionType => ActionsTypes.PatientDefence;
        public string Description => "(" + ActionEconomy + ") Patient Defense";
    }
}