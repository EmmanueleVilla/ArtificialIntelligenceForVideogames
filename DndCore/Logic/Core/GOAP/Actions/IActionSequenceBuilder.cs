using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using System.Collections.Generic;

namespace Logic.Core.GOAP.Actions
{
    interface IActionSequenceBuilder
    {
        List<List<IAvailableAction>> GetAvailableActions(IDndBattle battle);
    }
}
