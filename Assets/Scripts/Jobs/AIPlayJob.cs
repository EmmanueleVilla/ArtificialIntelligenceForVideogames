using Core.DI;
using Logic.Core.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;

namespace Assets.Scripts.Jobs
{
    struct AIPlayJob : IJob
    {
        public void Execute()
        {
            var battle = DndModule.Get<IDndBattle>();
            battle.PlayTurn();
        }
    }
}
