﻿using System;
using System.Collections.Generic;
using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using Logic.Core.Graph;

namespace Logic.Core.Battle
{
    public interface IDndBattle
    {
        List<CellInfo> GetPathTo(MemoryEdge edge);
        List<ICreature> Init(IMap map);
        ICreature GetCreatureInTurn();
        void NextTurn();
        List<IAvailableAction> GetAvailableActions();
        List<MemoryEdge> GetReachableCells();
        void CalculateReachableCells();
        List<MovementEvent> MoveTo(MemoryEdge end);
    }
}
