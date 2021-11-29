using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks
{
    class ExpiredClimberCreatureMock : ICreature
    {
        public int Id => throw new NotImplementedException();

        public Loyalties Loyalty => throw new NotImplementedException();

        public int Size => 1;

        public int HitPoints => throw new NotImplementedException();

        public int CurrentHitPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TemporaryHitPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int ArmorClass => throw new NotImplementedException();

        public AbilityScores AbilityScores => throw new NotImplementedException();

        public List<Speed> Movements => throw new NotImplementedException();

        public List<Speed> RemainingMovement
        {
            get => new List<Speed>() {
                new Speed(SpeedTypes.Walking, 10),
                new Speed(SpeedTypes.Climbing, 0)
            };
            set => throw new Exception("MOCK");
        }
        public List<Attack> Attacks => throw new NotImplementedException();

        public int AttacksPerAction => throw new NotImplementedException();

        public int RemainingAttacksPerAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ActionUsed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool BonusActionUsed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ReactionUsed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Disangaged { get => false; set => throw new NotImplementedException(); }

        public int RolledInitiative => throw new NotImplementedException();

        public string LastAttackUsed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int RemainingAttacksPerBonusAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ResetTurn()
        {
            throw new NotImplementedException();
        }
    }
}