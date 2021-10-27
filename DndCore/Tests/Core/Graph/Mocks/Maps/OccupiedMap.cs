using Core.Map;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.Graph.Mocks
{
    class OccupiedMap : IMap
    {
        Sizes Size;
        Loyalties Loyalty;

        public OccupiedMap(Sizes size, Loyalties loyalty)
        {
            Size = size;
            Loyalty = loyalty;
        }

        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public bool AddCreature(ICreature creature, int x, int y)
        {
            throw new NotImplementedException();
        }

        public CellInfo GetCellInfo(int x, int y)
        {
            return new CellInfo('G', 0, new MockedCreature(Size, Loyalty));
        }

        public List<CellInfo> GetCellsOccupiedBy(int x, int y)
        {
            throw new NotImplementedException();
        }

        public ICreature GetOccupantCreature(int x, int y)
        {
            return new MockedCreature(Size, Loyalty);
        }

        public List<ICreature> IsLeavingThreateningArea(ICreature mover, CellInfo start, CellInfo end)
        {
            throw new NotImplementedException();
        }
    }
}
