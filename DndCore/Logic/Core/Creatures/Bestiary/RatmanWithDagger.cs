using Logic.Core.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class RatmanWithDagger : ARatman
    {
        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Dagger", AttackTypes.WeaponMelee, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 6, 1, 6, 2)
            })
        };
    }
}