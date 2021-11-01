﻿using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;

namespace Tests.Core.DndBattles.Mock
{
    class InitiativeCreatureMock : ACreature
    {
        int _initiative;
        public InitiativeCreatureMock(int initiative, IDiceRoller roller) : base(roller)
        {
            _initiative = initiative;
        }

        public override Loyalties Loyalty => throw new NotImplementedException();

        public override Sizes Size => throw new NotImplementedException();

        public override List<Speed> Movements => throw new NotImplementedException();

        public override List<Attack> Attacks => throw new NotImplementedException();

        public override bool Disangaged => throw new NotImplementedException();

        public override int InitiativeModifier => _initiative;

        public override RollTypes InitiativeRollType => RollTypes.Normal;

        public override bool HasReaction()
        {
            throw new NotImplementedException();
        }
    }
}
