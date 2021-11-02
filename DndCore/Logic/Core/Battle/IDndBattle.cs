using System;
using System.Collections.Generic;
using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using Logic.Core.Graph;

namespace Logic.Core.Battle
{
    public interface IDndBattle
    {
        void Init(IMap map);
        ICreature GetCreatureInTurn();
        List<ICreature> RollInitiative();
        void NextTurn();
        List<IAvailableAction> GetAvailableActions(ICreature creature);
        List<Edge> GetReachableCells(ICreature creature);
    }
}
