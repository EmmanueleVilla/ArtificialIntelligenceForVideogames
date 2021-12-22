using Core.Map;
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
        public List<IAvailableAction> Build(IDndBattle battle, IMap map, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            var position = map.GetCellOccupiedBy(creature);

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
                            var occupant = map.GetOccupantCreature(i, j);
                            if (occupant != null && occupant.Loyalty != creature.Loyalty)
                            {
                                cells.Add(map.GetCellInfo(i, j));
                            }
                        }
                    }

                    actions.Add(new RequestAttackAction() { Attack = attack, ReachableCells = cells });
                }
            }
            return actions;
        }
    }
}
