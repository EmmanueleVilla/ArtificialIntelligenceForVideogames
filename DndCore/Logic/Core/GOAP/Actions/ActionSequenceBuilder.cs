using DndCore.DI;
using DndCore.Utils.Log;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Movement;
using Logic.Core.Battle.Actions.Spells;
using Logic.Core.Creatures;
using Logic.Core.GOAP.Goals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static DndCore.DI.DndModule;

namespace Logic.Core.GOAP.Actions
{
    public struct ActionList
    {
        public int creatureId;
        public List<IAvailableAction> actions;
        public IDndBattle battle;
        public float fullfillment;
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
        public ActionList GetBestActions(IDndBattle battleArg)
        {
            var writeToFile = DndModule.Get<WriteToFileBool>();
            if (writeToFile.ShouldWrite)
            {
                File.AppendAllText("log.txt", "*********************************************************\n");
                File.AppendAllText("log.txt", "CREATURE " + battleArg.GetCreatureInTurn().GetType().Name + "\n");
            }
            battleArg.ClearCache();
            ActionList result = new ActionList()
            {
                creatureId = battleArg.GetCreatureInTurn().Id,
                actions = new List<IAvailableAction>(),
                battle = battleArg,
                fullfillment = 0
            };
            var queue = new Stack<ActionList>();
            queue.Push(new ActionList() {
                creatureId = battleArg.GetCreatureInTurn().Id,
                actions = new List<IAvailableAction>(),
                battle = battleArg
            });
            int cutout = 0;
            int evaluated = 1;
            float maxFullfillment = float.MinValue;
            while(queue.Count > 0)
            {
                cutout++;
                var current = queue.Pop();
                current.battle.BuildAvailableActions(isAI: true);
                var nextActions = current.battle.GetAvailableActions().Where(x => x.ReachableCells.Count > 0);
                
                if(current.actions.Any(x => x is ConfirmMovementAction))
                {
                    nextActions = nextActions.Where(x => !(x is RequestMovementAction));
                }
                var maxPriority = nextActions.Max(x => x.Priority);
                nextActions = nextActions.Where(x => x.Priority == maxPriority || x is EndTurnAction);
                foreach (var nextAction in nextActions)
                {
                    foreach (var target in nextAction.ReachableCells)
                    {
                        if (nextAction is RequestMovementAction)
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
                                queue.Push(new ActionList()
                                {
                                    creatureId = creature.Id,
                                    actions = newActions,
                                    battle = newBattle
                                });
                            }
                        } else  if(nextAction is RequestAttackAction)
                        {
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
                            var events = newBattle.Attack(confirmAttack, true);

                            var updatedActions = new List<IAvailableAction>(current.actions)
                            {
                                confirmAttack
                            };

                            queue.Push(new ActionList() {
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
                            var actions = string.Join(",", updatedActions.Select(x => x.ActionType));
                            var fullfillment = current.battle.GetCreatureInTurn().EvaluateFullfillment(battleArg, updatedActions, current.battle);
                            if (writeToFile.ShouldWrite)
                            {
                                File.AppendAllText("log.txt", "--- Evaluating " + actions + "\n");
                                File.AppendAllText("log.txt", "Result is " + fullfillment + "\n");
                            }
                            evaluated++;
                            if (fullfillment > maxFullfillment)
                            {
                                cutout = 0;
                                maxFullfillment = fullfillment;
                                result = new ActionList()
                                {
                                    creatureId = current.battle.GetCreatureInTurn().Id,
                                    actions = new List<IAvailableAction>(updatedActions),
                                    battle = current.battle,
                                    fullfillment = fullfillment
                                };
                                if (writeToFile.ShouldWrite)
                                {
                                    File.AppendAllText("log.txt", $"{evaluated}), {fullfillment} points: {actions}\n");
                                }
                            }
                        }
                        else if (nextAction is RequestSpellAction)
                        {
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

                            queue.Push(new ActionList()
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
                            queue.Push(new ActionList()
                            {
                                creatureId = newBattle.GetCreatureInTurn().Id,
                                actions = new List<IAvailableAction>(updatedActions),
                                battle = newBattle
                            });
                        }
                    }
                }
            }
            var a = string.Join(",", result.actions.Select(x => x.ActionType));
            var message = $"{evaluated}), {result.fullfillment} points: {a}\n";
            Console.WriteLine(message);
            Console.WriteLine($"Evaluated: {evaluated}\n");
            if (writeToFile.ShouldWrite)
            {
                File.AppendAllText("log.txt", message);
            }
            return result;
        }
    }
}
