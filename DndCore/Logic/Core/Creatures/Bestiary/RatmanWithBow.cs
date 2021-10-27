using Logic.Core.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class RatmanWithBow : ARatman
    {
        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Bow", AttackTypes.WeaponRanged, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 6, 1, 6, 2)
            })
        };
    }
}
