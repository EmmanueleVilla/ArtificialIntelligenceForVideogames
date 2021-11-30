using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
using Logic.Core.Creatures.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class FlurryOfBlowsActionBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IMap map, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            if (creature is IFlurryOfBlows)
            {
                var flurry = creature as IFlurryOfBlows;
                if (!flurry.FlurryOfBlowsUsed && creature.LastAttackUsed != null && (creature as IKiPointsOwner).RemainingKiPoints > 0)
                {
                    actions.Add(new FlurryOfBlowsAction());
                }
            }
            return actions;
        }
    }
}
