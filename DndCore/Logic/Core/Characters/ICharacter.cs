using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Characters
{
    public interface ICharacter
    {
        List<Speed> Movements { get; }
    }
}
