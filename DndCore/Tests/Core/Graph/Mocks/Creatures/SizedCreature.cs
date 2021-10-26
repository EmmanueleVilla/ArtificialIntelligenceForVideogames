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
    public class SizedCreature : ICreature
    {
        Sizes _size;
        Loyalties _loyalty;

        public SizedCreature(Sizes size, Loyalties loyalty)
        {
            _size = size;
            _loyalty = loyalty;
        }

        public Loyalties Loyalty => _loyalty;

        public Sizes Size => _size;

        public List<Speed> Movements => throw new NotImplementedException();

        public List<Attack> Attacks => throw new NotImplementedException();
    }
}
