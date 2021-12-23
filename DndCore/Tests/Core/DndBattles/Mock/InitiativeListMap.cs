using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.DndBattles.Mock
{
    public class InitiativeListMap : IMap
    {
        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public List<ICreature> Creatures => new List<ICreature>()
        {
            new InitiativeCreatureMock(0, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(1, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(2, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(3, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(4, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(5, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(6, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(7, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(8, new ZeroRoller(), new Random()),
            new InitiativeCreatureMock(9, new ZeroRoller(), new Random()),
        };

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
            throw new NotImplementedException();
        }

        public List<ICreature> IsLeavingThreateningArea(ICreature mover, CellInfo start, CellInfo end)
        {
            throw new NotImplementedException();
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
