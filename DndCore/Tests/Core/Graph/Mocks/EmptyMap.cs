using Core.Map;
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

        public CellInfo GetCellInfo(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
