﻿using System;
using System.Collections.Generic;
using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Spells;
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
        void BuildAvailableActions(ICreature creature = null);
        List<IAvailableAction> GetAvailableActions(ICreature creature = null);
        List<MemoryEdge> GetReachableCells();
        void CalculateReachableCells(ICreature creature = null);
        IEnumerable<GameEvent> MoveTo(MemoryEdge end);
        List<GameEvent> Attack(ConfirmAttackAction confirmAttackAction);
        List<GameEvent> Spell(ConfirmSpellAction confirmSpellAction);
        List<GameEvent> UseAbility(IAvailableAction availableAction);
        IDndBattle Copy();
    }
}
