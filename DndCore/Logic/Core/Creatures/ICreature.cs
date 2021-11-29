using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Logic.Core.Creatures
{
    public interface ICreature
    {
        // General --------------------------------------------------------------------------

        int Id { get; }

        Loyalties Loyalty { get; }

        int Size { get; }


        // Defense

        int HitPoints { get; }
        int CurrentHitPoints { get; set; }
        int TemporaryHitPoints { get; set; }
        int ArmorClass { get; }

        // Ability scores --------------------------------------------------------------------------

        AbilityScores AbilityScores { get; }

        // Movement --------------------------------------------------------------------------

        List<Speed> Movements { get; }

        List<Speed> RemainingMovement { get; set; }

        // Attacks --------------------------------------------------------------------------

        List<Attack> Attacks { get; }

        int AttacksPerAction { get; }

        int RemainingAttacksPerAction { get; set; }

        int RemainingAttacksPerBonusAction { get; set; }

        string LastAttackUsed { get; set; }

        // Action economy --------------------------------------------------------------------------

        bool ActionUsed { get; set; }

        bool BonusActionUsed { get; set; }

        bool ReactionUsed { get; set; }

        bool Disangaged { get; set; }

        // Initiative --------------------------------------------------------------------------

        int RolledInitiative { get; }
        void ResetTurn();
    }
}
