﻿using Logic.Core.Actions;
using Logic.Core.Creatures.Abilities;
using Logic.Core.Creatures.Classes;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;

namespace Logic.Core.Creatures.Bestiary
{
    public class HumanFemaleMonk : ACreature, IKiPointsOwner, IMartialArts, IFlurryOfBlows
    {
        public HumanFemaleMonk(IDiceRoller roller = null, Random random = null): base(roller, random)
        {

        }

        public override ICreature Copy()
        {
            return new HumanFemaleMonk()
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
                RemainingKiPoints = this.RemainingKiPoints,
                FlurryOfBlowsUsed = this.FlurryOfBlowsUsed,
                bonusAttackTriggered = this.bonusAttackTriggered,
            };
        }
        public override void ResetTurn()
        {
            base.ResetTurn();
            FlurryOfBlowsUsed = false;
            BonusAttackTriggered = false;
            RemainingAttacksPerBonusAction = 0;
        }

        public override Loyalties Loyalty => Loyalties.Ally;

        public override int Size => 1;

        public override int CriticalThreshold => 20;

        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Quarterstaff", 1, new List<Damage>()
            {
                new Damage(DamageTypes.Bludgeoning, 7, 1, 6, 3)
            }, false, 6),
            new Attack("Unarmed Strike", 1, new List<Damage>()
            {
                new Damage(DamageTypes.Bludgeoning, 7, 1, 6, 3)
            }, false, 6)
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

        public int KiPoints => 5;

        public int RemainingKiPoints { get; set; } = 5;
        public bool FlurryOfBlowsUsed { get; set; } = false;

        private bool bonusAttackTriggered;
        public bool BonusAttackTriggered { 
            get {
                return bonusAttackTriggered;
            }
            set {
                if (bonusAttackTriggered != value)
                {
                    bonusAttackTriggered = value;
                    if(bonusAttackTriggered)
                    {
                        RemainingAttacksPerBonusAction = 1;
                    }
                }
            } 
        }
    }
}
