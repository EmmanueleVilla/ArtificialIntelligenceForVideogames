using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class BaseActionsBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IDndBattle battle, ICreature creature)
        {
            var actions = new List<IAvailableAction>();

            if (!creature.ActionUsedNotToAttack && !creature.ActionUsedToAttack)
            {
                if (!creature.DashUsed)
                {
                    actions.Add(new DashAction() { ActionEconomy = BattleActions.Action });
                }
                if (!creature.Disangaged)
                {
                    actions.Add(new DisengageAction() { ActionEconomy = BattleActions.Action });
                }
                if (!creature.DodgeUsed)
                {
                    actions.Add(new DodgeAction() { ActionEconomy = BattleActions.Action });
                }
            }

            return actions;
        }
    }
}
