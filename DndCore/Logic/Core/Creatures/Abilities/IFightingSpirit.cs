using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    public interface IFightingSpirit
    {
        int FightingSpiritTemporaryHitPoints { get; }
        int FightingSpiritUsages { get; }
        int FightingSpiritRemaining { get; set; }
    }
}
