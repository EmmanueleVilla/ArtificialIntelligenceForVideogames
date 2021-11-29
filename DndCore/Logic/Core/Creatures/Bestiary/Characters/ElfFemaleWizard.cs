using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Logic.Core.Creatures.Bestiary
{
    public class ElfFemaleWizard : ACreature
    {
        public override Loyalties Loyalty => Loyalties.Ally;

        public override int Size => 1;

        public override int CriticalThreshold => 20;

        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Quarterstaff", 1, new List<Damage>()
            {
                new Damage(DamageTypes.Bludgeoning, 7, 1, 6, -1)
            }, false, 2)
        };

        public override List<Speed> Movements => new List<Speed>()
        {
            new Speed(SpeedTypes.Walking, 6)
        };

        public override int InitiativeModifier => 2;

        public override RollTypes InitiativeRollType => RollTypes.Normal;

        public override int AttacksPerAction => 1;

        public override int HitPoints => 27;

        public override int ArmorClass => 12;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(8, 14, 13, 16, 15, 11);
    }
}