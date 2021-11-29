using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
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
                new Damage(DamageTypes.Piercing, 10, 1, 12, 3)
            }, 5)
        };

        public override int InitiativeModifier => 3;

        public override int HitPoints => 45;

        public override int ArmorClass => 13;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(12, 16, 12, 9, 11, 17);
    }
}
