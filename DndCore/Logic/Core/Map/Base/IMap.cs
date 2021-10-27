using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Map
{
    public interface IMap
    {
        CellInfo GetCellInfo(int x, int y);
        int Width { get; }
        int Height { get; }
        void AddCreature(ICreature creature, int x, int y);
        ICreature GetOccupantCreature(int x, int y);
    }
}
