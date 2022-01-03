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


        public Dictionary<int, ICreature> Creatures => new Dictionary<int, ICreature>
        {
            {0, new InitiativeCreatureMock(0, new ZeroRoller(), new Random()) },
            {1, new InitiativeCreatureMock(1, new ZeroRoller(), new Random()) },
            {2, new InitiativeCreatureMock(2, new ZeroRoller(), new Random()) },
            {3, new InitiativeCreatureMock(3, new ZeroRoller(), new Random()) },
            {4, new InitiativeCreatureMock(4, new ZeroRoller(), new Random()) },
            {5, new InitiativeCreatureMock(5, new ZeroRoller(), new Random()) },
            {6, new InitiativeCreatureMock(6, new ZeroRoller(), new Random()) },
            {7, new InitiativeCreatureMock(7, new ZeroRoller(), new Random()) },
            {8, new InitiativeCreatureMock(8, new ZeroRoller(), new Random()) },
            {9, new InitiativeCreatureMock(9, new ZeroRoller(), new Random()) },
        };

        public bool AddCreature(ICreature creature, int x, int y)
        {
            throw new NotImplementedException();
        }

        public IMap Copy()
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

        public void RemoveCreature(ICreature creature)
        {
            throw new NotImplementedException();
        }
    }
}
