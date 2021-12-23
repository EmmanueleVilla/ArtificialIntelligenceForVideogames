using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;

namespace Logic.Core.Creatures
{
    public enum TemporaryEffects
    {
        DisadvantageToSufferedAttacks,
        AdvantageToAttacks,
        SpeedReducedByTwo
    }

    public interface ICreature
    {
        // General --------------------------------------------------------------------------

        int Id { get; }

        Loyalties Loyalty { get; }

        int Size { get; }

        List<Tuple<int, int, TemporaryEffects>> TemporaryEffectsList { get; set; }

        // Defense

        int HitPoints { get; }
        int CurrentHitPoints { get; set; }
        int TemporaryHitPoints { get; set; }
        int ArmorClass { get; }

        // Ability scores --------------------------------------------------------------------------

        AbilityScores AbilityScores { get; }

        ICreature Copy();

        // Movement --------------------------------------------------------------------------

        List<Speed> Movements { get; }

        List<Speed> RemainingMovement { get; set; }

        // Attacks --------------------------------------------------------------------------

        List<Attack> Attacks { get; }

        int AttacksPerAction { get; }

        int RemainingAttacksPerAction { get; set; }

        int RemainingAttacksPerBonusAction { get; set; }

        string LastAttackUsed { get; set; }

        int CriticalThreshold { get; }

        // Action economy --------------------------------------------------------------------------

        bool ActionUsedNotToAttack { get; set; }

        bool ActionUsedToAttack { get; set; }

        bool BonusActionUsedToAttack { get; set; }
        bool BonusActionUsedNotToAttack { get; set; }

        bool ReactionUsed { get; set; }

        bool Disangaged { get; set; }

        bool DashUsed { get; set; }

        bool DodgeUsed { get; set; }

        // Initiative --------------------------------------------------------------------------

        int RolledInitiative { get; }
        void ResetTurn();

        // Init ---------------------------------------------------------------------------------
        ICreature Init();
    }
}
