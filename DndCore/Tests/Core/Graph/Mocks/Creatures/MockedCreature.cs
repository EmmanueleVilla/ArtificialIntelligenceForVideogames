using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.Graph.Mocks.Creatures
{
    public class MockedCreature : ICreature
    {
        public int Id { get; set; }
        Sizes _size;
        Loyalties _loyalty;
        List<Attack> _attacks;
        bool _disengaged;
        bool _hasReactions;
        List<Speed> _movements;

        public MockedCreature(
            Sizes size = Sizes.Medium,
            Loyalties loyalty = Loyalties.Ally,
            List<Attack> attacks = null,
            List<Speed> movements = null,
            bool disengaged = false,
            bool hasReactions = true)
        {
            _size = size;
            _loyalty = loyalty;
            _attacks = attacks ?? new List<Attack>();
            _disengaged = disengaged;
            _hasReactions = hasReactions;
            _movements = movements ?? new List<Speed>();
        }

        public Loyalties Loyalty => _loyalty;

        public Sizes Size => _size;

        public List<Speed> Movements { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<Attack> Attacks => _attacks;

        public bool Disangaged { get => _disengaged; set => throw new NotImplementedException(); }

        public int RolledInitiative => throw new System.NotImplementedException();

        public AbilityScores AbilityScores => throw new NotImplementedException();

        public int HitPoints => throw new NotImplementedException();

        public int CurrentHitPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TemporaryHitPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int ArmorClass => throw new NotImplementedException();

        public List<Speed> RemainingMovement { get => _movements; set => throw new NotImplementedException(); }

        public int AttacksPerAction => throw new NotImplementedException();

        public int RemainingAttacksPerAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ActionUsed { get => false; set => throw new NotImplementedException(); }
        public bool BonusActionUsed { get => false; set => throw new NotImplementedException(); }
        public bool ReactionUsed { get => !_hasReactions; set => throw new NotImplementedException(); }

        public void ResetTurn()
        {
            throw new NotImplementedException();
        }
    }
}
