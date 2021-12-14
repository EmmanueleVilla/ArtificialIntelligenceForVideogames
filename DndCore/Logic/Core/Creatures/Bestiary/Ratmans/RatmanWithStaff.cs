using Logic.Core.Actions;
using Logic.Core.Creatures.Abilities;
using Logic.Core.Creatures.Abilities.Spells;
using Logic.Core.Creatures.Scores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class RatmanWithStaff : ARatman, ISpellCaster
    {
        public override List<Attack> Attacks => new List<Attack>()
        {
            new Attack("Quarterstaff", 1, new List<Damage>()
            {
                new Damage(DamageTypes.Piercing, 10, 1, 6, -1)
            }, false, 5)
        };

        public override int InitiativeModifier => 3;

        public override int HitPoints => 67;

        public override int ArmorClass => 14;

        public override AbilityScores AbilityScores { get; } = new AbilityScores(8, 16, 12, 9, 11, 17);

        public Dictionary<int, int> SpellSlots { get; set; } = new Dictionary<int, int>() { { 0, int.MaxValue }, { 1, 4 }, { 2, 3 }, { 3, 2 } };

        public Dictionary<int, int> RemainingSpellSlots { get; set; } = new Dictionary<int, int>() { { 0, int.MaxValue }, { 1, 4 }, { 2, 3 }, { 3, 2 } };

        public List<ISpell> Spells => new List<ISpell> {
            new RayOfFrost(),
            new FalseLife(),
            new MagicMissile()
        };
    }
}
