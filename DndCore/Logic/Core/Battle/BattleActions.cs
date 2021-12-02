using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Logic.Core.Battle
{
    public enum BattleActions
    {
        [Description("")]
        Free,
        [Description("A")]
        Action,
        [Description("B")]
        BonusAction
    }
}
