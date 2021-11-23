﻿using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.Graph.Mocks
{
    class EmptyMap : IMap
    {
        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public List<ICreature> Creatures => throw new NotImplementedException();

        public bool AddCreature(ICreature creature, int x, int y)
        {
            throw new NotImplementedException();
        }

        public CellInfo GetCellInfo(int x, int y)
        {
            throw new NotImplementedException();
        }

        public CellInfo GetCellOccupiedBy(ICreature creature)
        {
            throw new NotImplementedException();
        }

        public List<CellInfo> GetCellsOccupiedBy(int x, int y)
        {
            throw new NotImplementedException();
        }

        public ICreature GetOccupantCreature(int x, int y)
        {
            return null;
        }

        public List<ICreature> IsLeavingThreateningArea(ICreature mover, CellInfo start, CellInfo end)
        {
            return new List<ICreature>();
        }

        public void MoveCreatureTo(ICreature creature, MemoryEdge end)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(MemoryEdge edge)
        {
            throw new NotImplementedException();
        }
    }
}
