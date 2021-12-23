using Core.DI;
using Core.Utils.Log;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Movement;
using Logic.Core.Battle.Actions.Spells;
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
        public ILogger Logger;
        public ActionSequenceBuilder(ILogger logger = null)
        {
            Logger = logger ?? DndModule.Get<ILogger>();
        }
        public List<ActionList> GetAvailableActions(IDndBattle battleArg)
        {
            var result = new List<ActionList>();
            var queue = new Queue<ActionList>();
            queue.Enqueue(new ActionList() {
                creature = battleArg.GetCreatureInTurn(),
                actions = new List<IAvailableAction>(),
                battle = battleArg
            });
            int loop = 0;
            while(queue.Count > 0)
            {
                loop++;
                
                //safe threshold in case of bugs
                if(loop > 10000)
                {
                    break;
                }

                var current = queue.Dequeue();
                if (loop % 100 == 0)
                {
                    //Logger.WriteLine(loop.ToString());
                    //Logger.WriteLine(string.Join("-", current.actions.Select(x => x.GetType().Name)));
                }
                current.battle.BuildAvailableActions();
                var nextActions = current.battle.GetAvailableActions().Where(x => x.ReachableCells.Count > 0);
                var maxPriority = nextActions.Max(x => x.Priority);
                nextActions = nextActions.Where(x => x.Priority == maxPriority).ToList();
                foreach (var nextAction in nextActions)
                {
                    if (nextAction is RequestMovementAction && current.actions.LastOrDefault() is ConfirmMovementAction)
                    {
                        continue;
                    }

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

                                var newActions = new List<IAvailableAction>(current.actions);
                                newActions.Add(new ConfirmMovementAction()
                                {
                                    MemoryEdge = memoryEdge,
                                    DestinationX = memoryEdge.Destination.X,
                                    DestinationY = memoryEdge.Destination.Y,
                                    Speed = memoryEdge.Speed,
                                    Damage = memoryEdge.Damage
                                }) ;

                                queue.Enqueue(new ActionList() { 
                                    creature = creature, 
                                    actions = newActions,
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
                                if(attacked == null)
                                {
                                    throw new Exception("WTF");
                                }
                                var confirmAttack = new ConfirmAttackAction()
                                {
                                    ActionEconomy = nextAction.ActionEconomy,
                                    AttackingCreature = newBattle.GetCreatureInTurn(),
                                    TargetCreature = attacked,
                                    Attack = attackAction.Attack
                                };
                                var events = newBattle.Attack(confirmAttack);

                                var updatedActions = new List<IAvailableAction>(current.actions)
                                {
                                    confirmAttack
                                };

                                queue.Enqueue(new ActionList() {
                                    creature = confirmAttack.AttackingCreature,
                                    actions = new List<IAvailableAction>(updatedActions),
                                    battle = newBattle
                                });
                            }
                        }
                        else if (nextAction is EndTurnAction)
                        {
                            var updatedActions = new List<IAvailableAction>(current.actions)
                            {
                                nextAction
                            };
                            result.Add(new ActionList()
                            {
                                creature = current.battle.GetCreatureInTurn(),
                                actions = new List<IAvailableAction>(updatedActions),
                                battle = current.battle
                            });
                        }
                        else if (nextAction is RequestSpellAction)
                        {
                            var spellAction = nextAction as RequestSpellAction;
                            foreach (var targets in nextAction.ReachableCells)
                            {
                                var newBattle = current.battle.Copy();
                                var attacked = newBattle.Map.GetOccupantCreature(targets.X, targets.Y);
                                var confirmSpell = new ConfirmSpellAction(newBattle.GetCreatureInTurn(), spellAction.Spell)
                                {
                                    ActionEconomy = nextAction.ActionEconomy,
                                    Target = targets
                                };
                                var events = newBattle.Spell(confirmSpell);

                                var updatedActions = new List<IAvailableAction>(current.actions)
                                {
                                    confirmSpell
                                };

                                queue.Enqueue(new ActionList()
                                {
                                    creature = confirmSpell.Caster,
                                    actions = new List<IAvailableAction>(updatedActions),
                                    battle = newBattle
                                });
                            }
                        } else
                        {
                            var newBattle = current.battle.Copy();
                            var events = newBattle.UseAbility(nextAction);
                            var updatedActions = new List<IAvailableAction>(current.actions)
                            {
                                nextAction
                            };
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

            Logger.WriteLine("Completed building actions");

            return result;
        }
    }
}
