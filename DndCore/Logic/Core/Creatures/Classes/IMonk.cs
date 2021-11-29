using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Classes
{
    interface IMonk
    {
        int KiPoints { get; }
        int RemainingKiPoints { get; set; }
    }
}
