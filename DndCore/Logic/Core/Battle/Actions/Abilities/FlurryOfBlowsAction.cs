using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Abilities
{
    public class FlurryOfBlowsAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public ActionsTypes ActionType => ActionsTypes.FlurryOfBlows;

        public string Description => "Flurry of blows";
    }
}
