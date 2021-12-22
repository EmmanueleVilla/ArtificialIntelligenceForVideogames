using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle
{
    public interface IActionsBuilder
    {
        List<IAvailableAction> Build(IDndBattle dndBattle, IMap map, ICreature creature);
    }
}
