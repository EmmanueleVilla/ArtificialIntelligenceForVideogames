using Logic.Core.Actions;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class Minotaur : ICreature
    {
        public Loyalties Loyalty => Loyalties.Enemy;

        public Sizes Size => Sizes.Large;

        public List<Speed> Movements => throw new NotImplementedException();

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
