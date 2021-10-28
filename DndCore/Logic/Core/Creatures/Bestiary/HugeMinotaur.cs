using Logic.Core.Actions;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class HugeMinotaur : ICreature
    {
        public Loyalties Loyalty => Loyalties.Enemy;

        public Sizes Size => Sizes.Huge;

        public List<Speed> Movements => new List<Speed>() { new Speed(SpeedTypes.Walking, 6) };

        public List<Attack> Attacks => new List<Attack>()
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
        public bool Disangaged => false;

        public bool HasReaction()
        {
            return true;
        }
    }
}
