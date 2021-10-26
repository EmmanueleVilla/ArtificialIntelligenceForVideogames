using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures
{
    public interface ICreature
    {
        List<Speed> Movements { get; }
    }
}
