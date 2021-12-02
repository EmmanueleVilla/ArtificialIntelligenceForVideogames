using Logic.Core.Creatures.Abilities.Spells;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities
{
    interface ISpellCaster
    {
        Dictionary<int,int> SpellSlots { get; }
        Dictionary<int, int> RemainingSpellSlots { get; set; }

        List<ISpell> Spells { get; }
    }
}
