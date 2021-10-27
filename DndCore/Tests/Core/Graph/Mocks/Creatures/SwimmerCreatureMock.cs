using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.Graph.Mocks
{
    class SwimmerCreatureMock : ICreature
    {
        Sizes _size;
        public SwimmerCreatureMock(Sizes size = Sizes.Medium)
        {
            _size = size;
        }
        public List<Speed> Movements =>
            new List<Speed>() {
                new Speed(SpeedTypes.Walking, 6),
                new Speed(SpeedTypes.Swimming, 8)
            };

        public Sizes Size => _size;

        public List<Attack> Attacks => throw new System.NotImplementedException();

        public Loyalties Loyalty => throw new System.NotImplementedException();
    }
}
