using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities.Spells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Spells
{
    public class RequestSpellAction : IAvailableAction
    {
        public ICreature Caster;
        public ISpell Spell;

        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>();
        public RequestSpellAction(ICreature caster, ISpell spell)
        {
            Caster = caster;
            Spell = spell;
            ActionEconomy = spell.CastingTime;
        }

        public BattleActions ActionEconomy { get; set; }

        public ActionsTypes ActionType => ActionsTypes.RequestSpell;

        public string Description => "(" + ActionEconomy + ") " + Spell.Name + " - lvl " + Spell.Level;
        public int Priority => 1000;
    }
}
