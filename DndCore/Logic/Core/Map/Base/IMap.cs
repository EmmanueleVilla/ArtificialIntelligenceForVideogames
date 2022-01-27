using Logic.Core.Creatures;
using Logic.Core.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Map
{
    public interface IMap
    {
        CellInfo GetCellInfo(int x, int y);
        CellInfo GetCellOccupiedBy(ICreature creature);
        List<CellInfo> GetCellsOccupiedBy(Loyalties loyalty);
        int Width { get; }
        int Height { get; }
        bool AddCreature(ICreature creature, int x, int y);
        ICreature GetOccupantCreature(int x, int y);
        List<ICreature> IsLeavingThreateningArea(ICreature mover, CellInfo start, CellInfo end);
        Dictionary<int, ICreature> Creatures { get; }
        List<CellInfo> GetCellsOccupiedBy(int x, int y);
        void MoveCreatureTo(ICreature creature, MemoryEdge end);
        void RemoveCreature(ICreature creature);
        IMap Copy();
    }
}
