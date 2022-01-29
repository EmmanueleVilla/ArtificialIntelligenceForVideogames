using Logic.Core.Actions;
using Logic.Core.Battle;
using Logic.Core.Creatures.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities.Spells
{
    class MagicMissile : ASpell, IAlwaysHit
    {
        public MagicMissile() : base(0)
        {
        }
        public override string Name => "Magic Missile";

        public override int Level => 1;

        public override BattleActions CastingTime => BattleActions.Action;

        public override int Range => 24;

        public override bool CanTargetSelf => false;

        public override List<Damage> Damages => new List<Damage> { new Damage(DamageTypes.Force, 12, 3, 4, 1) };

        public override List<Tuple<TemporaryEffects, int>> Effects => new List<Tuple<TemporaryEffects, int>>();

        public override int Area => 0;
    }
}
