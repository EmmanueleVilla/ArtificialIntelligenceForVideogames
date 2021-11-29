using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks
{
    class WalkerCreatureMock : ICreature
    {
        Sizes _size;
        public WalkerCreatureMock(Sizes size = Sizes.Medium)
        {
            _size = size;
        }

        public int Id => throw new NotImplementedException();

        public Loyalties Loyalty => Loyalties.Ally;

        public Sizes Size => _size;

        public int HitPoints => throw new NotImplementedException();

        public int CurrentHitPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TemporaryHitPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int ArmorClass => throw new NotImplementedException();

        public AbilityScores AbilityScores => throw new NotImplementedException();

        public List<Speed> Movements => throw new NotImplementedException();

        public List<Speed> RemainingMovement
        {
            get => new List<Speed>() {
                new Speed(SpeedTypes.Walking, 6)
            };
            set => throw new Exception("MOCK");
        }
        public List<Attack> Attacks => new List<Attack>();

        public int AttacksPerAction => throw new NotImplementedException();

        public int RemainingAttacksPerAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ActionUsed { get => false; set => throw new NotImplementedException(); }
        public bool BonusActionUsed { get => false; set => throw new NotImplementedException(); }
        public bool ReactionUsed { get => false; set => throw new NotImplementedException(); }
        public bool Disangaged { get => false; set => throw new NotImplementedException(); }

        public int RolledInitiative => throw new NotImplementedException();

        public void ResetTurn()
        {
            throw new NotImplementedException();
        }
    }
}
