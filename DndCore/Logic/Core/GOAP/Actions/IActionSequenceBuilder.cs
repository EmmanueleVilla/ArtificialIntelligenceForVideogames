using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using System.Collections.Generic;

namespace Logic.Core.GOAP.Actions
{
    public interface IActionSequenceBuilder
    {
        List<ActionList> GetAvailableActions(IDndBattle battle);
    }
}
