using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Logic.Core.Creatures.Bestiary
{
    public class HumanFemaleMonk : ACreature
    {
        public override Loyalties Loyalty => Loyalties.Ally;

        public override Sizes Size => Sizes.Medium;

        public override int CriticalThreshold => 20;

        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Quarterstaff", AttackTypes.WeaponMelee, new List<Damage>()
            {
                new Damage(DamageTypes.Bludgeoning, 7, 1, 6, 3)
            }, 6),
            new Attack("Unarmed Strike", AttackTypes.WeaponMelee, new List<Damage>()
            {
                new Damage(DamageTypes.Bludgeoning, 7, 1, 6, 3)
            }, 6)
        };

        public override List<Speed> Movements => new List<Speed>()
        {
            new Speed(SpeedTypes.Walking, 6)
        };

        public override int InitiativeModifier => 3;

        public override RollTypes InitiativeRollType => RollTypes.Normal;

        public override int AttacksPerAction => 2;

        public override int HitPoints => 38;

        public override int ArmorClass => 16;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(13, 17, 14, 11, 16, 9);
    }
}
