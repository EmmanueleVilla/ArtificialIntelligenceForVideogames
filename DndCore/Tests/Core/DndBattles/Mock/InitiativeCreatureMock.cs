using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;

namespace Tests.Core.DndBattles.Mock
{
    public class InitiativeCreatureMock : ACreature
    {
        int _initiative;
        public InitiativeCreatureMock(int initiative, IDiceRoller roller, Random random) : base(roller, random, initiative)
        {
            _initiative = initiative;
        }

        public override int RolledInitiative => _initiative;

        public override Loyalties Loyalty => throw new NotImplementedException();

        public override int Size => throw new NotImplementedException();

        public override int CriticalThreshold => throw new NotImplementedException();

        public override List<Attack> Attacks => throw new NotImplementedException();

        public override List<Speed> Movements => new List<Speed>();

        public override int InitiativeModifier => 0;

        public override RollTypes InitiativeRollType => RollTypes.Advantage;

        public override int AttacksPerAction => 0;

        public override int HitPoints => 0;

        public override int ArmorClass => throw new NotImplementedException();

        public override AbilityScores AbilityScores => throw new NotImplementedException();
    }
}
