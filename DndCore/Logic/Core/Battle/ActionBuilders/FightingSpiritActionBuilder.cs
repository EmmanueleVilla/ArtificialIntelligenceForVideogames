using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class FightingSpiritActionBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IMap map, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            var fightningSpirit = creature as IFightingSpirit;
            if (fightningSpirit != null && fightningSpirit.FightingSpiritRemaining > 0 && !creature.BonusActionUsedNotToAttack && !creature.BonusActionUsedToAttack)
            {
                actions.Add(new FightingSpiritAction() { ActionEconomy = BattleActions.BonusAction });
            }

            return actions;
        }
    }
}
