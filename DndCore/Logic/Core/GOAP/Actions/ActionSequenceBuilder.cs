using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.GOAP.Actions
{
    public struct ActionList
    {
        public ICreature creature;
        public List<IAvailableAction> actions;

        public override string ToString()
        {
            return String.Join(",", actions);
        }
    }

    public class ActionSequenceBuilder : IActionSequenceBuilder
    {
        public List<List<IAvailableAction>> GetAvailableActions(IDndBattle battle)
        {
            var queue = new Queue<ActionList>();
            queue.Enqueue(new ActionList() { creature = battle.GetCreatureInTurn(), actions = new List<IAvailableAction>() });

            while(queue.Count > 0)
            {
                var current = queue.Dequeue();
                battle.BuildAvailableActions();
                var nextActions = battle.GetAvailableActions(current.creature);
                foreach (var nextAction in nextActions)
                {
                    
                    if(nextAction is RequestMovementAction && current.actions.LastOrDefault() is RequestMovementAction)
                    {
                        continue;
                    }
                    Console.WriteLine("Checking action " + nextAction + " with " + nextAction.ReachableCells.Count + " targets");
                    foreach (var target in nextAction.ReachableCells)
                    {
                    }
                }
            }

            return new List<List<IAvailableAction>>();
        }
    }
}
