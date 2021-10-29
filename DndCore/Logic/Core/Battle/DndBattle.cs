using System;
using System.Collections.Generic;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Creatures;

namespace Logic.Core
{
    public class DndBattle: IDndBattle
    {
        private SortedList<ICreature, ICreature> initiativeOrder = new SortedList<ICreature, ICreature>(new CreatureInitiativeComparer());
        private IMap map;

        public void Init(IMap map)
        {
            this.map = map;
        }

        public ICreature GetCreatureInTurn()
        {
            if(initiativeOrder.Count == 0)
            {
                foreach(var creature in map.Creatures)
                {

                }
            }
            return null;
        }
    }
}
