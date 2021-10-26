using Logic.Core.Actions;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures
{
    public interface ICreature
    {
        Loyalties Loyalty { get; }
        Sizes Size { get; }
        List<Speed> Movements { get; }
        List<Attack> Attacks { get; }
    }
}
