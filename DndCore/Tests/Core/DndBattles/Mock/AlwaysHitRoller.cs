﻿using Logic.Core.Dice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.DndBattles.Mock
{
    public class AlwaysHitRoller : IDiceRoller
    {
        public int Roll(RollTypes type, int diceAmount, int dieFaces, int modifier)
        {
            return 18;
        }
    }
}