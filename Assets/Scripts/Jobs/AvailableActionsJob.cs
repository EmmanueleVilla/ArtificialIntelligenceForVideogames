using Core.DI;
using Logic.Core.Battle;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Assets.Scripts.Jobs
{
    
    struct AvailableActionsJob : IJob
    {
        public void Execute()
        {
            var battle = DndModule.Get<IDndBattle>();
            battle.BuildAvailableActions();
        }
    }
}
