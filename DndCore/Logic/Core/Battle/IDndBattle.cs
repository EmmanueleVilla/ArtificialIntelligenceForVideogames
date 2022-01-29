using System;
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
        List<int> Init(IMap map);
        IMap Map { get; }
        ICreature GetCreatureInTurn();
        ICreature GetCreatureById(int id);
        void NextTurn();
        void ClearCache();
        void BuildAvailableActions(ICreature creature = null, bool isAI = false);
        List<IAvailableAction> GetAvailableActions();
        List<MemoryEdge> GetReachableCells();
        void CalculateReachableCells(ICreature creature = null);
        List<GameEvent> MoveTo(MemoryEdge end);
        List<GameEvent> Attack(ConfirmAttackAction confirmAttackAction, bool forceHit = false);
        List<GameEvent> Spell(ConfirmSpellAction confirmSpellAction);
        List<GameEvent> UseAbility(IAvailableAction availableAction);
        IDndBattle Copy();
        void PlayTurn();
        List<GameEvent> Events { get; }
        CellInfo GetCellInfo(int x, int y);
    }
}
