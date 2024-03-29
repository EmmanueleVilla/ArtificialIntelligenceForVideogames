﻿using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle
{
    public enum ActionsTypes
    {
        RequestMovement,
        CancelMovement,
        ConfirmMovement,
        Action,
        BonusAction,
        EndTurn,
        RequestAttack,
        CancelAttack,
        ConfirmAttack,
        Disengage,
        Dash,
        Dodge,
        FlurryOfBlows,
        PatientDefence,
        FightingSpirit,
        SecondWind,
        RequestSpell,
        CancelSpell,
        ConfirmSpell
    }
}
