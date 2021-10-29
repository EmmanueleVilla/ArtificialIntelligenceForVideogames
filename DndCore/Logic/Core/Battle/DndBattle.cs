using System;
using System.Collections.Generic;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Creatures;

namespace Logic.Core
{
    public class DndBattle: IDndBattle
    {
        private List<ICreature> initiativeOrder = new List<ICreature>();
        private IMap map;
        private int turnIndex;

        public void Init(IMap map)
        {
            this.map = map;
        }

        public List<ICreature> RollInitiative()
        {
            foreach (var creature in map.Creatures)
            {
                initiativeOrder.Add(creature);
            }
            initiativeOrder.Sort(new CreatureInitiativeComparer());
            return initiativeOrder;
        }

        public ICreature GetCreatureInTurn()
        {
            if(initiativeOrder.Count == 0)
            {
                foreach(var creature in map.Creatures)
                {
                    initiativeOrder.Add(creature);
                }
                initiativeOrder.Sort(new CreatureInitiativeComparer());
            }
            return initiativeOrder[turnIndex];
        }
    }
}
