using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.GOAP.Actions
{
    public interface IActionBuildersCleanup
    {
        List<IAvailableAction> Cleanup(ICreature creature, List<IAvailableAction> cachedActions);
    }
}
