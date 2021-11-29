using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class RatmanWithStaff : ARatman
    {
        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Quarterstaff", AttackTypes.WeaponRanged, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 10, 1, 6, -1)
            }, 5)
        };

        public override int InitiativeModifier => 3;

        public override int HitPoints => 67;

        public override int ArmorClass => 14;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(8, 16, 12, 9, 11, 17);
    }
}
