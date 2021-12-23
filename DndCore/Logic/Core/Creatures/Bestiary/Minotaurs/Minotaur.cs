using Logic.Core.Actions;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class LargeMinotaur : AMinotaur
    {
        public override int Size => 2;

        public override ICreature Copy()
        {
            return new LargeMinotaur()
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
    }
}
