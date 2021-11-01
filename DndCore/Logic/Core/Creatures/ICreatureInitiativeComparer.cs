using System;
using System.Collections.Generic;

namespace Logic.Core.Creatures
{
    public class CreatureInitiativeComparer : IComparer<ICreature>
    {
        public int Compare(ICreature x, ICreature y)
        {
            if (y.RolledInitiative == x.RolledInitiative && y.AbilityScores.Dexterity != x.AbilityScores.Dexterity)
            {
                return y.AbilityScores.Dexterity.CompareTo(x.AbilityScores.Dexterity);
            }
            var result = y.RolledInitiative.CompareTo(x.RolledInitiative);
            if (result == 0)
            {
                // HACK
                // This breaks get(key), but we won't use it
                // and it lets us have multiple items with the same key
                return -1;
            }
            else
            {
                return result;
            }
        }
    }
}
