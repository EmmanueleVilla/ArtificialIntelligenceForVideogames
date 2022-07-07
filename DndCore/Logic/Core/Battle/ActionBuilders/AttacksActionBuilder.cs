using DndCore.DI;
using DndCore.Map;
using DndCore.Utils.Log;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle
{
    class AttacksActionBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IDndBattle battle, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            var position = battle.Map.GetCellOccupiedBy(creature);

            if (!creature.ActionUsedNotToAttack && creature.RemainingAttacksPerAction > 0)
            {
                foreach (var attack in creature.Attacks)
                {
                    var cells = new List<CellInfo>();
                    var startI = position.X - attack.Range;
                    var endI = position.X + creature.Size + attack.Range;
                    var startJ = position.Y - attack.Range;
                    var endJ = position.Y + creature.Size + attack.Range;
                    for (int i = startI; i < endI; i++)
                    {
                        for (int j = startJ; j < endJ; j++)
                        {
                            var occupant = battle.Map.GetOccupantCreature(i, j);
                            if (occupant != null && occupant.Loyalty != creature.Loyalty)
                            {
                                cells.Add(battle.Map.GetCellInfo(i, j));
                            }
                        }
                    }
                    if (cells.Count > 0)
                    {
                        actions.Add(new RequestAttackAction() { Attack = attack, ReachableCells = cells });
                    }
                }
            }
            return actions;
        }
    }
}
