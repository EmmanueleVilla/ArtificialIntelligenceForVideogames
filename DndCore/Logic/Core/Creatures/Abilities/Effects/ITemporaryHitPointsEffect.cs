using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities.Spells
{
    interface ITemporaryHitPointsEffect
    {
        int NumberOfDice { get; }
        int DiceFaces { get; }
        int Modifier { get; }
    }
}
