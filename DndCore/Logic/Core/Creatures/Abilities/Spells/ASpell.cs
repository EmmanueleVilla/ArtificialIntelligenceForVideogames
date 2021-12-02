using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities.Spells
{
    abstract class ASpell : ISpell
    {
        public abstract string Name { get; }
        public abstract int Level { get; }
    }
}
