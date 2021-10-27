using Logic.Core.Actions;
using Logic.Core.Creatures;
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
        Sizes _size;
        Loyalties _loyalty;
        List<Attack> _attacks;
        bool _disengaged;
        bool _hasReactions;

        public MockedCreature(
            Sizes size,
            Loyalties loyalty,
            List<Attack> attacks = null,
            bool disengaged = false,
            bool hasReactions = true)
        {
            _size = size;
            _loyalty = loyalty;
            _attacks = attacks ?? new List<Attack>();
            _disengaged = disengaged;
            _hasReactions = hasReactions;
        }

        public Loyalties Loyalty => _loyalty;

        public Sizes Size => _size;

        public List<Speed> Movements => new List<Speed>();

        public List<Attack> Attacks => _attacks;

        public bool Disangaged => _disengaged;

        public bool HasReaction()
        {
            return _hasReactions;
        }
    }
}
