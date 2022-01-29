using Logic.Core.Actions;
using Logic.Core.Battle;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities.Spells
{
    public abstract class ASpell : ISpell
    {
        public int ToHit { get; set; }

        protected ASpell(int toHit)
        {
            ToHit = toHit;
        }
        public abstract string Name { get; }
        public abstract int Level { get; }
        public abstract BattleActions CastingTime { get; }
        public abstract int Range { get; }
        public abstract bool CanTargetSelf { get; }
        public abstract List<Damage> Damages { get; }
        public abstract List<Tuple<TemporaryEffects, int>> Effects { get; }
        public abstract int Area { get; }
    }
}
