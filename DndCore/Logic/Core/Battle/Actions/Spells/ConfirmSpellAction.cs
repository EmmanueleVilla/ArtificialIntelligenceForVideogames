using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities.Spells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Spells
{
    public class ConfirmSpellAction : IAvailableAction
    {
        public ICreature Caster;
        public CellInfo Target;
        public ISpell Spell;
        public ConfirmSpellAction(ICreature caster, ISpell spell)
        {
            Caster = caster;
            Spell = spell;
            ActionEconomy = spell.CastingTime;
        }

        public BattleActions ActionEconomy { get; set; }

        public ActionsTypes ActionType => ActionsTypes.ConfirmSpell;

        public string Description => "Confirm " + Spell.Name + " at " + "(" + Target.X + "," + Target.Y + ")";
    }
}
