using System;
using System.Collections.Generic;
using Core.Map;
using Logic.Core.Creatures;

namespace Logic.Core.Battle
{
    public interface IDndBattle
    {
        void Init(IMap map);
        ICreature GetCreatureInTurn();
        List<ICreature> RollInitiative();
        void NextTurn();
    }
}
