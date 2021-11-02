using System;
using System.Collections.Generic;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Creatures;

namespace Logic.Core
{
    public class DndBattle: IDndBattle
    {
        private IMap map;
        private int turnIndex = 0;

        private List<ICreature> initiativeOrder = new List<ICreature>();

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

        public AvailableActions GetAvailableActions(ICreature creature)
        {
            //TODO deplete used movement
            return new AvailableActions(creature.Movements);
        }

        public void NextTurn()
        {
            turnIndex++;
            if(turnIndex >= map.Creatures.Count)
            {
                turnIndex = 0;
            }
        }
    }
}
