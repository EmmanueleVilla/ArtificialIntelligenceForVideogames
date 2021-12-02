using Logic.Core.Actions;
using Logic.Core.Battle;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Abilities.Spells
{
    public interface ISpell
    {
        string Name { get; }
        int Level { get; }
        BattleActions CastingTime { get; }
        int Range { get; }
        int Area { get; }
        bool CanTargetSelf { get; }
        List<Damage> Damages { get; }
        List<Tuple<TemporaryEffects, int>> Effects { get; }
    }
}
