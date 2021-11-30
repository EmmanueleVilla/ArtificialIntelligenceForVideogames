using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Classes
{
    public interface IKiPointsOwner
    {
        int KiPoints { get; }
        int RemainingKiPoints { get; set; }
    }
}
