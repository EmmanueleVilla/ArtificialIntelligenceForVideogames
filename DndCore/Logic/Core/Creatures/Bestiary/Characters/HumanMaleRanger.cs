﻿using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Logic.Core.Creatures.Bestiary
{
    public class HumanMaleRanger : ARangedCreature
    {

        public override ICreature Copy()
        {
            return new HumanMaleRanger()
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
                RemainingAttacksPerBonusAction = this.RemainingAttacksPerBonusAction,
            };
        }
        public override Loyalties Loyalty => Loyalties.Ally;

        public override int Size => 1;

        public override int CriticalThreshold => 20;

        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Dagger", 1, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 7, 1, 4, 4)
            }, false, 7),
            new Attack("Longbow", 30, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 9, 1, 8, 4)
            }, false, 9)
        };

        public override List<Speed> Movements => new List<Speed>()
        {
            new Speed(SpeedTypes.Walking, 6)
        };

        public override int InitiativeModifier => 4;

        public override RollTypes InitiativeRollType => RollTypes.Normal;

        public override int AttacksPerAction => 2;

        public override int HitPoints => 44;

        public override int ArmorClass => 16;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(14, 18, 15, 9, 13, 11);
    }
}
