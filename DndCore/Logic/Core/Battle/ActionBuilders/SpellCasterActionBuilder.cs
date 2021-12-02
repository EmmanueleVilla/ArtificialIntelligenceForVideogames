using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Spells;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class SpellCasterActionBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IMap map, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            var caster = creature as ISpellCaster;
            if (caster == null)
            {
                return actions;
            }

            var position = map.GetCellOccupiedBy(creature);

            foreach (var spell in caster.Spells)
            {
                if(caster.RemainingSpellSlots[spell.Level] > 0)
                {
                    if (!creature.ActionUsedNotToAttack && !creature.ActionUsedToAttack)
                    {
                        var action = new RequestSpellAction(creature, spell);
                        var cells = new List<CellInfo>();
                        var startI = position.X - spell.Range;
                        var endI = position.X + creature.Size + spell.Range;
                        var startJ = position.Y - spell.Range;
                        var endJ = position.Y + creature.Size + spell.Range;
                        for (int i = startI; i < endI; i++)
                        {
                            for (int j = startJ; j < endJ; j++)
                            {
                                var occupant = map.GetOccupantCreature(i, j);
                                if (occupant != null && occupant.Loyalty != creature.Loyalty)
                                {
                                    cells.Add(map.GetCellInfo(i, j));
                                }
                                if(occupant == creature && spell.CanTargetSelf)
                                {
                                    cells.Add(map.GetCellInfo(i, j));
                                }
                            }
                        }
                        action.ReachableCells = cells;
                        actions.Add(action);
                    }
                }
            }

            return actions;
        }
    }
}
