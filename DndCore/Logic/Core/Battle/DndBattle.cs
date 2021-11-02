using System;
using System.Collections.Generic;
using Core.DI;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using Logic.Core.Graph;

namespace Logic.Core
{
    public class DndBattle: IDndBattle
    {
        private IMap map;
        private int turnIndex = 0;

        private List<ICreature> initiativeOrder = new List<ICreature>();

        private UniformCostSearch Search;

        public DndBattle(UniformCostSearch search = null) {
            Search = search ?? DndModule.Get<UniformCostSearch>();
        }

        public void Init(IMap map)
        {
            this.map = map;
        }

        public List<ICreature> RollInitiative()
        {
            foreach (var creature in map.Creatures)
            {
                Console.WriteLine(string.Format("{0} rolled {1}", creature, creature.RollInitiative()));
                initiativeOrder.Add(creature);
            }
            initiativeOrder.Sort(new CreatureInitiativeComparer());
            return initiativeOrder;
        }

        public ICreature GetCreatureInTurn()
        {
            if(initiativeOrder.Count == 0)
            {
                RollInitiative();
            }
            return initiativeOrder[turnIndex];
        }

        public List<IAvailableAction> GetAvailableActions(ICreature creature)
        {
            //TODO deplete used movement
            return new List<IAvailableAction>() {
                new MovementAction() { RemainingMovement = creature.Movements }
            };
        }

        public void NextTurn()
        {
            turnIndex++;
            if(turnIndex >= map.Creatures.Count)
            {
                turnIndex = 0;
            }
        }

        public List<Edge> GetReachableCells(ICreature creature)
        {
            return Search.Search(map.GetCellOccupiedBy(creature), map);
        }
    }
}
