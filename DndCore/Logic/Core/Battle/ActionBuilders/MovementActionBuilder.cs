using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class MovementActionBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IMap map, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            var movementAction = new RequestMovementAction() { RemainingMovement = creature.RemainingMovement };

            if (movementAction.RemainingMovement.Any(x => x.Item2 > 0))
            {
                actions.Add(movementAction);
            }
            return actions;
        }
    }
}
