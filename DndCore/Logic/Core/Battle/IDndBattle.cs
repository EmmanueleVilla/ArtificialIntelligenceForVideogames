using System;
using Core.Map;
using Logic.Core.Creatures;

namespace Logic.Core.Battle
{
    public interface IDndBattle
    {
        void Init(IMap map);

        ICreature GetCreatureInTurn();
    }
}
