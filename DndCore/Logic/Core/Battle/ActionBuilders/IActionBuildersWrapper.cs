using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    public interface IActionBuildersWrapper
    {
        List<IActionsBuilder> ActionBuilders { get; }
    }
}
