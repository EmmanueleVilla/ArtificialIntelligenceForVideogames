using Logic.Core.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Logic.Core.Creatures.Bestiary
{
    public class DwarfMaleWarrior : ACreature, IFightingSpirit
    {
        public override Loyalties Loyalty => Loyalties.Ally;

        public override int Size => 1;

        public override int CriticalThreshold => 19;

        public override void ResetTurn()
        {
            base.ResetTurn();
        }

        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Battleaxe", 1, new List<Damage>()
            {
                new Damage(DamageTypes.Slashing, 10, 1, 8, 5)
            }, false, 6)
        };

        public override List<Speed> Movements => new List<Speed>()
        {
            new Speed(SpeedTypes.Walking, 5)
        };

        public override int InitiativeModifier => 2;

        public override RollTypes InitiativeRollType => RollTypes.Normal;

        public override int AttacksPerAction => 2;

        public override int HitPoints => 54;

        public override int ArmorClass => 18;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(16, 14, 16, 12, 9, 10);

        public int FightingSpiritTemporaryHitPoints => 5;

        public int FightingSpiritUsages => 3;

        public int FightingSpiritRemaining { get; set; } = 3;
    }
}
