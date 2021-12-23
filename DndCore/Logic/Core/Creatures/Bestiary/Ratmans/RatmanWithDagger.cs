using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class RatmanWithDagger : ARatman
    {
        public override ICreature Copy()
        {
            return new RatmanWithDagger()
            {
                Id = this.Id,
                remainingMovement = this.remainingMovement,
                RolledInitiative = this.RolledInitiative,
                Disangaged = this.Disangaged,
                RemainingAttacksPerAction = this.RemainingAttacksPerAction,
                ActionUsedNotToAttack = this.ActionUsedNotToAttack,
                ActionUsedToAttack = this.ActionUsedToAttack,
                BonusActionUsedNotToAttack = this.BonusActionUsedNotToAttack,
                BonusActionUsedToAttack = this.BonusActionUsedToAttack,
                ReactionUsed = this.ReactionUsed,
                CurrentHitPoints = this.CurrentHitPoints,
                TemporaryHitPoints = this.TemporaryHitPoints,
                LastAttackUsed = this.LastAttackUsed,
                DashUsed = this.DashUsed,
                DodgeUsed = this.DodgeUsed,
                RemainingAttacksPerBonusAction = this.RemainingAttacksPerBonusAction
            };
        }
        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Dagger", 1, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 10, 1, 12, 3)
            }, false, 5)
        };

        public override int InitiativeModifier => 4;

        public override int HitPoints => 52;

        public override int ArmorClass => 15;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(12, 16, 12, 9, 11, 17);
    }
}
