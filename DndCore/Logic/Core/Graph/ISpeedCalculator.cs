using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Logic.Core.Graph
{
    public interface ISpeedCalculator
    {
        Edge GetNeededSpeed(ICreature creature, CellInfo from, CellInfo to, IMap map, List<Speed> movementsArg = null);
    }
}