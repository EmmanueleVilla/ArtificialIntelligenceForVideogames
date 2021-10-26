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
    class OccupiedByEnemyMap : IMap
    {
        Sizes EnemySize;

        public OccupiedByEnemyMap(Sizes enemySize)
        {
            EnemySize = enemySize;
        }

        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public CellInfo GetCellInfo(int x, int y)
        {
            return new CellInfo('G', 0, new SizedCreature(EnemySize, Loyalties.Enemy));
        }
    }
}
