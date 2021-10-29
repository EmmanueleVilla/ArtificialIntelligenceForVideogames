using Core.Map;
using Logic.Core.Creatures;

namespace Logic.Core.Graph
{
    public interface ISpeedCalculator
    {
        Edge GetNeededSpeed(ICreature creature, CellInfo from, CellInfo to, IMap map);
    }
}