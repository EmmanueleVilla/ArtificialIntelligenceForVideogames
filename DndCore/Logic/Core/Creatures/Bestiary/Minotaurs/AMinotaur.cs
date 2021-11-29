using System;
using System.Collections.Generic;
using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;

namespace Logic.Core.Creatures.Bestiary
{
    public abstract class AMinotaur : ACreature
    {
        public override Loyalties Loyalty => Loyalties.Enemy;

        public override int Size => 2;

        public override int CriticalThreshold => 20;

        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Greataxe", AttackTypes.WeaponMelee, new List<Damage>()
            {
                new Damage(DamageTypes.Slashing, 18, 2, 12, 4)
            }, 6),
            new Attack("Gore", AttackTypes.WeaponMelee, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 14, 2, 8, 4)
            }, 6)
        };

        public override List<Speed> Movements => new List<Speed>()
        {
            new Speed(SpeedTypes.Walking, 8)
        };

        public override int InitiativeModifier => 0;

        public override RollTypes InitiativeRollType => RollTypes.Normal;

        public override int AttacksPerAction => 1;

        public override int HitPoints => 76;

        public override int ArmorClass => 14;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(18, 11, 16, 6, 16, 9);
    }
}
