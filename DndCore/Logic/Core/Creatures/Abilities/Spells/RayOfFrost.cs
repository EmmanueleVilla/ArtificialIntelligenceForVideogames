using Logic.Core.Actions;
using Logic.Core.Battle;
using System;
using System.Collections.Generic;

namespace Logic.Core.Creatures.Abilities.Spells
{
    public class RayOfFrost : ASpell
    {

        public RayOfFrost(int toHit) : base(toHit)
        {
        }
        public override string Name => "Ray of Frost";

        public override int Level => 0;

        public override BattleActions CastingTime => BattleActions.Action;

        public override int Range => 12;

        public override bool CanTargetSelf => false;

        public override List<Damage> Damages => new List<Damage> { new Damage(DamageTypes.Cold, 10, 2, 8, 0) };

        public override List<Tuple<TemporaryEffects,int>> Effects => new List<Tuple<TemporaryEffects, int>>
        { new Tuple<TemporaryEffects, int>(TemporaryEffects.SpeedReducedByTwo, 1) };

        public override int Area => 0;
    }
}
