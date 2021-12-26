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
        public int creatureId;
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
                creatureId = battleArg.GetCreatureInTurn().Id,
                actions = new List<IAvailableAction>(),
                battle = battleArg
            });
            int loop = 0;
            while(queue.Count > 0)
            {
                loop++;

                var current = queue.Dequeue();
                if (loop % 100 == 0)
                {
                    //Console.WriteLine(loop.ToString());
                    //Console.WriteLine(string.Join("-", current.actions.Select(x => x.GetType().Name)));
                }
                current.battle.BuildAvailableActions();
                var nextActions = current.battle.GetAvailableActions().Where(x => x.ReachableCells.Count > 0);
                if(current.actions.LastOrDefault() is ConfirmMovementAction)
                {
                    nextActions = nextActions.Where(x => !(x is RequestMovementAction));
                }
                //Console.WriteLine("Next actions count: " + nextActions.Count());
                var maxPriority = nextActions.Max(x => x.Priority);
                nextActions = nextActions.Where(x => x.Priority == maxPriority).ToList();
                //Console.WriteLine("After filter count: " + nextActions.Count());
                foreach (var nextAction in nextActions)
                {
                    //Console.WriteLine(nextAction.GetType().Name + ": " + nextAction.Description + ", " + nextAction.ReachableCells.Count() + " targets");

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
                                    creatureId = creature.Id, 
                                    actions = newActions,
                                    battle = newBattle
                                } );
                            }
                        } else  if(nextAction is RequestAttackAction)
                        {
                            if(target.Creature == null)
                            {
                                continue;
                            }
                            var attackAction = nextAction as RequestAttackAction;
                            var newBattle = current.battle.Copy();
                            var attacked = newBattle.Map.GetOccupantCreature(target.X, target.Y);
                            var confirmAttack = new ConfirmAttackAction()
                            {
                                ActionEconomy = nextAction.ActionEconomy,
                                AttackingCreature = newBattle.GetCreatureInTurn().Id,
                                TargetCreature = attacked.Id,
                                Attack = attackAction.Attack
                            };
                            var events = newBattle.Attack(confirmAttack);

                            var updatedActions = new List<IAvailableAction>(current.actions)
                            {
                                confirmAttack
                            };

                            queue.Enqueue(new ActionList() {
                                creatureId = confirmAttack.AttackingCreature,
                                actions = new List<IAvailableAction>(updatedActions),
                                battle = newBattle
                            });
                        }
                        else if (nextAction is EndTurnAction)
                        {
                            var updatedActions = new List<IAvailableAction>(current.actions)
                            {
                                nextAction
                            };
                            result.Add(new ActionList()
                            {
                                creatureId = current.battle.GetCreatureInTurn().Id,
                                actions = new List<IAvailableAction>(updatedActions),
                                battle = current.battle
                            });
                        }
                        else if (nextAction is RequestSpellAction)
                        {
                            if (target.Creature == null)
                            {
                                continue;
                            }
                            var spellAction = nextAction as RequestSpellAction;
                            var newBattle = current.battle.Copy();
                            var attacked = newBattle.Map.GetOccupantCreature(target.X, target.Y);
                            var confirmSpell = new ConfirmSpellAction(newBattle.GetCreatureInTurn().Id, spellAction.Spell)
                            {
                                ActionEconomy = nextAction.ActionEconomy,
                                Target = target
                            };
                            var events = newBattle.Spell(confirmSpell);

                            var updatedActions = new List<IAvailableAction>(current.actions)
                            {
                                confirmSpell
                            };

                            queue.Enqueue(new ActionList()
                            {
                                creatureId = confirmSpell.Caster,
                                actions = new List<IAvailableAction>(updatedActions),
                                battle = newBattle
                            });
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
                                creatureId = newBattle.GetCreatureInTurn().Id,
                                actions = new List<IAvailableAction>(updatedActions),
                                battle = newBattle
                            });
                        }
                    }
                }
            }

            //Console.WriteLine("Completed building actions " + result.Count());

            return result;
        }
    }
}
