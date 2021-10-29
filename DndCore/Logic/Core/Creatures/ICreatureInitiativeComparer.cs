using System;
using System.Collections.Generic;

namespace Logic.Core.Creatures
{
    public class CreatureInitiativeComparer : IComparer<ICreature>
    {
        public int Compare(ICreature x, ICreature y)
        {
            if (x.RolledInitiative == y.RolledInitiative && x.AbilityScores.Dexterity != y.AbilityScores.Dexterity)
            {
                return x.AbilityScores.Dexterity.CompareTo(y.AbilityScores.Dexterity);
            }
            var result = x.RolledInitiative.CompareTo(y.RolledInitiative);
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
