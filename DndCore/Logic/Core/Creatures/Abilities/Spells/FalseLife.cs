using Logic.Core.Actions;
using Logic.Core.Battle;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities.Spells
{
    public class FalseLife : ASpell, ITemporaryHitPointsEffect
    {
        public override string Name => "False Life";

        public override int Level => 1;

        public override BattleActions CastingTime => BattleActions.Action;

        public override int Range => 0; // 0 means only self

        public override bool CanTargetSelf => true;

        public override List<Damage> Damages => new List<Damage> {};

        public override List<Tuple<TemporaryEffects, int>> Effects => new List<Tuple<TemporaryEffects, int>> { };

        public int NumberOfDice => 1;

        public int DiceFaces => 4;

        public int Modifier => 4;

        public override int Area => 0;
    }
}
