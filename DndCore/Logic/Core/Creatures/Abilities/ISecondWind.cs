using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities
{
    public interface ISecondWind
    {
        int SecondWindTemporaryHitPoints { get; }
        int SecondWindUsages { get; }
        int SecondWindRemaining { get; set; }
    }
}
