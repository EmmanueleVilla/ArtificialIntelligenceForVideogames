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

        public MockedCreature(Sizes size, Loyalties loyalty, List<Attack> attacks = null)
        {
            _size = size;
            _loyalty = loyalty;
            _attacks = attacks ?? new List<Attack>();
        }

        public Loyalties Loyalty => _loyalty;

        public Sizes Size => _size;

        public List<Speed> Movements => new List<Speed>();

        public List<Attack> Attacks => _attacks;
    }
}
