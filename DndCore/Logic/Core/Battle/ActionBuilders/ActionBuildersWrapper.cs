using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class ActionBuildersWrapper : IActionBuildersWrapper
    {
        public List<IActionsBuilder> ActionBuilders => new List<IActionsBuilder>()
        {
            new MovementActionBuilder(),
            new AttacksActionBuilder(),
            new FlurryOfBlowsActionBuilder(),
            new BaseKiPointsActionBuilder(),
            new MartialArtsActionBuilder(),
            new FightingSpiritActionBuilder(),
            new SecondWindActionBuilder(),
            new SpellCasterActionBuilder(),
            new BaseActionsBuilder(),
        };
    }
}
