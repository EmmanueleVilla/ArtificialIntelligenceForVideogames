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
        public List<IAvailableAction> Build(IDndBattle battle, ICreature creature)
        {
            var actions = new List<IAvailableAction>();
            var movementAction = new RequestMovementAction() { RemainingMovement = creature.RemainingMovement };

            battle.CalculateReachableCells();
            if (movementAction.RemainingMovement.Any(x => x.Square > 0))
            {
                movementAction.ReachableCells = battle.GetReachableCells().Select(x => x.Destination).ToList();
                actions.Add(movementAction);
            }
            return actions;
        }
    }
}
