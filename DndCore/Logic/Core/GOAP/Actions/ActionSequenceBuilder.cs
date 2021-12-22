using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Movement;
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
        public IDndBattle battle;

        public override string ToString()
        {
            return String.Join(",", actions);
        }
    }

    public class ActionSequenceBuilder : IActionSequenceBuilder
    {
        public List<ActionList> GetAvailableActions(IDndBattle battleArg)
        {
            var result = new List<ActionList>();
            var queue = new Queue<ActionList>();
            queue.Enqueue(new ActionList() {
                creature = battleArg.GetCreatureInTurn(),
                actions = new List<IAvailableAction>(),
                battle = battleArg
            });

            var loop = 0;
            while(queue.Count > 0)
            {
                loop++;
                var current = queue.Dequeue();
                current.battle.BuildAvailableActions();
                var nextActions = current.battle.GetAvailableActions(current.creature);
                Console.WriteLine(nextActions.Count);
                foreach (var nextAction in nextActions)
                {
                    if(nextAction is RequestMovementAction && current.actions.LastOrDefault() is RequestMovementAction)
                    {
                        continue;
                    }
                    var updatedActions = new List<IAvailableAction>(current.actions);
                    
                    updatedActions.Add(nextAction);
                    foreach (var target in nextAction.ReachableCells)
                    {
                        if(nextAction is RequestMovementAction)
                        {
                            var memoryEdges = current.battle.GetReachableCells().Where(x => x.Destination.X == target.X && x.Destination.Y == target.Y).ToList();
                            foreach(var memoryEdge in memoryEdges)
                            {
                                var newBattle = current.battle.Copy();
                                newBattle.MoveTo(memoryEdge);
                                queue.Enqueue(new ActionList() { 
                                    creature = newBattle.GetCreatureInTurn(), 
                                    actions = new List<IAvailableAction>(updatedActions),
                                    battle = newBattle
                                } );
                            }
                        } else  if(nextAction is RequestAttackAction)
                        {
                            var attackAction = nextAction as RequestAttackAction;
                            foreach (var targets in nextAction.ReachableCells)
                            {
                                var newBattle = current.battle.Copy();
                                var attacked = newBattle.Map.GetOccupantCreature(targets.X, targets.Y);
                                var confirmAttack = new ConfirmAttackAction()
                                {
                                    ActionEconomy = nextAction.ActionEconomy,
                                    AttackingCreature = newBattle.GetCreatureInTurn(),
                                    TargetCreature = attacked,
                                    Attack = attackAction.Attack
                                };
                                newBattle.Attack(confirmAttack);

                                queue.Enqueue(new ActionList() {
                                    creature = confirmAttack.AttackingCreature,
                                    actions = new List<IAvailableAction>(updatedActions),
                                    battle = newBattle
                                });
                            }
                        } else if (nextAction is EndTurnAction)
                        {
                            result.Add(new ActionList() { 
                                creature = current.battle.GetCreatureInTurn(),
                                actions = new List<IAvailableAction>(updatedActions),
                                battle = current.battle
                            });
                        } else
                        {
                            var newBattle = current.battle.Copy();
                            newBattle.UseAbility(nextAction);
                            queue.Enqueue(new ActionList()
                            {
                                creature = newBattle.GetCreatureInTurn(),
                                actions = new List<IAvailableAction>(updatedActions),
                                battle = newBattle
                            });
                        }
                    }
                }
            }
            Console.WriteLine(String.Join("\n", result.Select(x => string.Join("-",  x.actions.Select(a => string.Format("({0}) {1}", a.ActionEconomy, a.GetType().ToString().Split('.').Last()))))));
            return result;
        }
    }
}
