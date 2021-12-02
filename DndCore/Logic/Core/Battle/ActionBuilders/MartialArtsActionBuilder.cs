using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class MartialArtsActionBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IMap map, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            var position = map.GetCellOccupiedBy(creature);

            if (creature is IMartialArts
                && !creature.BonusActionUsedNotToAttack
                && creature.RemainingAttacksPerBonusAction > 0)
            {
                var unarmedStrike = creature.Attacks.FirstOrDefault(a => a.Name.ToLower().Contains("unarmed strike"));
                if (unarmedStrike.Name != null)
                {
                    var cells = new List<CellInfo>();
                    var startI = position.X - unarmedStrike.Range;
                    var endI = position.X + creature.Size + unarmedStrike.Range;
                    var startJ = position.Y - unarmedStrike.Range;
                    var endJ = position.Y + creature.Size + unarmedStrike.Range;
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

                    actions.Add(new RequestAttackAction() { Attack = unarmedStrike, ReachableCells = cells, ActionEconomy = BattleActions.BonusAction });
                }
            }
            return actions;
        }
    }
}
