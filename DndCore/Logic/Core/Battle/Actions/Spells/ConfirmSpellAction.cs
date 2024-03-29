﻿using DndCore.Map;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities.Spells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Spells
{
    public class ConfirmSpellAction : IAvailableAction
    {
        public int Caster;
        public CellInfo Target;
        public ISpell Spell;
        public ConfirmSpellAction(int caster, ISpell spell)
        {
            Caster = caster;
            Spell = spell;
            ActionEconomy = spell.CastingTime;
        }

        public BattleActions ActionEconomy { get; set; }

        public ActionsTypes ActionType => ActionsTypes.ConfirmSpell;
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>();
        public string Description => "Confirm " + Spell.Name + " at " + "(" + Target.X + "," + Target.Y + ")";
        public int Priority => 0;
    }
}
