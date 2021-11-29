using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class RatmanWithDagger : ARatman
    {
        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Dagger", AttackTypes.WeaponRanged, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 10, 1, 12, 3)
            }, 5)
        };

        public override int InitiativeModifier => 4;

        public override int HitPoints => 52;

        public override int ArmorClass => 15;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(12, 16, 12, 9, 11, 17);
    }
}
