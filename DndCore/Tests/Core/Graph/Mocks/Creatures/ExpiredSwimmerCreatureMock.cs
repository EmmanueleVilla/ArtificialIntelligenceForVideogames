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
    class ExpiredSwimmerCreatureMock : ICreature
    {
        public int Id { get; set; }
        Sizes _size;
        public ExpiredSwimmerCreatureMock(Sizes size = Sizes.Medium)
        {
            _size = size;
        }
        public List<Speed> Movements
        {
            get => new List<Speed>() {
                new Speed(SpeedTypes.Walking, 6),
                new Speed(SpeedTypes.Swimming, 0)
            };
            set => throw new Exception("NYI");
        }

        public Sizes Size => _size;

        public List<Attack> Attacks => new List<Attack>();

        public Loyalties Loyalty => Loyalties.Ally;

        public bool Disangaged => false;

        public int RolledInitiative => throw new NotImplementedException();

        public int RollInitiative()
        {
            throw new NotImplementedException();
        }

        public AbilityScores AbilityScores => throw new NotImplementedException();

        public bool HasAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool HasBonusAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool HasReaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
