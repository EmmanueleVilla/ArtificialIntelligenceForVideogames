using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Movement;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.IO;
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
                if (loop % 50 == 0)
                {
                    Console.WriteLine(loop + ") " + string.Join(" - ", current.actions.Select(x => x.GetType().ToString().Split('.').Last())));
                }
                //Console.WriteLine("***** CURRENT " + loop + "*****");
                //Console.WriteLine(String.Join("-", current.actions.Select(x => x.GetType().ToString().Split('.').Last())));
                //Console.WriteLine("***********************");
                current.battle.BuildAvailableActions();
                var nextActions = current.battle.GetAvailableActions(current.creature);
                foreach (var nextAction in nextActions)
                {
                    
                    if(nextAction is RequestMovementAction && current.actions.LastOrDefault() is RequestMovementAction)
                    {
                        continue;
                    }
                    var updatedActions = new List<IAvailableAction>(current.actions)
                    {
                        nextAction
                    };
                    //Console.Write("Can " + nextAction.GetType().ToString().Split('.').Last());
                    //Console.WriteLine(" with " + nextAction.ReachableCells.Count() + " targets");
                    foreach (var target in nextAction.ReachableCells)
                    {
                        if(nextAction is RequestMovementAction)
                        {
                            var memoryEdges = current.battle.GetReachableCells().Where(x => x.Destination.X == target.X && x.Destination.Y == target.Y).ToList();
                            foreach(var memoryEdge in memoryEdges)
                            {
                                var newBattle = current.battle.Copy();
                                var creature = newBattle.GetCreatureInTurn();
                                var events = newBattle.MoveTo(memoryEdge);
                                
                                queue.Enqueue(new ActionList() { 
                                    creature = creature, 
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
                                var events = newBattle.Attack(confirmAttack);

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
                            var events = newBattle.UseAbility(nextAction);
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
            File.WriteAllText("sequence.txt", String.Join("\n", result.Select(x => string.Join("-",  x.actions.Select(a => string.Format("({0}) {1}", a.ActionEconomy, a.GetType().ToString().Split('.').Last()))))));
            return result;
        }
    }
}
