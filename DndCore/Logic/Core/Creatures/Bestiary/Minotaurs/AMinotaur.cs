using System;
using System.Collections.Generic;
using Logic.Core.Actions;
using Logic.Core.Dice;
using Logic.Core.Movements;

namespace Logic.Core.Creatures.Bestiary
{
    public abstract class AMinotaur : ACreature
    {
        public override Loyalties Loyalty => Loyalties.Enemy;

        public override List<Speed> Movements => new List<Speed>() { new Speed(SpeedTypes.Walking, 6) };

        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Greataxe", AttackTypes.WeaponMelee, new List<Damage>()
            {
                new Damage(DamageTypes.Slashing, 18, 2, 12, 4)
            }),
            new Attack("Greataxe", AttackTypes.WeaponMelee, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 14, 2, 8, 4)
            })
        };

        public override bool Disangaged => false;

        public override int InitiativeModifier => throw new NotImplementedException();

        public override RollTypes InitiativeRollType => throw new NotImplementedException();

        public override bool HasReaction()
        {
            return true;
        }
    }
}
