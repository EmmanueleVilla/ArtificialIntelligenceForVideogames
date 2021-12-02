using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities.Spells
{
    interface ISpell
    {
        string Name { get; }
        int Level { get; }
    }
}
