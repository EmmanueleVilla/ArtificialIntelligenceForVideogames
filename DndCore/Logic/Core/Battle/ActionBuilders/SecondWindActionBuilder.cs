using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class SecondWindActionBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IMap map, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            var secondWind = creature as ISecondWind;
            if (secondWind != null && secondWind.SecondWindRemaining > 0 && !creature.BonusActionUsedNotToAttack && !creature.BonusActionUsedToAttack)
            {
                actions.Add(new SecondWindAction() { ActionEconomy = "B" });
            }

            return actions;
        }
    }
}
