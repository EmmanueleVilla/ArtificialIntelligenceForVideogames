using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures
{
    public interface ICreature
    {
        int Id { get; }
        Loyalties Loyalty { get; }
        Sizes Size { get; }
        List<Speed> Movements { get; }
        List<Attack> Attacks { get; }
        bool Disangaged { get; }
        
        int RolledInitiative { get; }
        int RollInitiative();

        //Ability scores
        AbilityScores AbilityScores { get; }

        //Actions values
        bool HasAction { get; set; }
        bool HasBonusAction { get; set; }
        bool HasReaction { get; set; }
    }
}
