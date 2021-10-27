﻿using Logic.Core.Creatures;
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
        bool AddCreature(ICreature creature, int x, int y);
        ICreature GetOccupantCreature(int x, int y);
        List<CellInfo> GetCellsOccupiedBy(int x, int y);
        List<ICreature> IsLeavingThreateningArea(ICreature mover, CellInfo start, CellInfo end);
    }
}
