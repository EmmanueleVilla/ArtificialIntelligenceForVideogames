using Logic.Core.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Movement
{
    public class ConfirmMovementAction : IAvailableAction
    {
        public ActionsTypes ActionType => ActionsTypes.ConfirmMovement;
        public BattleActions ActionEconomy { get; set; } = BattleActions.Free;
        public int DestinationX { get; set; }
        public int DestinationY { get; set; }
        public int Speed { get; set; }
        public int Damage { get; set; }
        public string Description => string.Format("Move to ({0}-{1}), distance {2}m, risking {3} damage", DestinationX, DestinationY, Speed * 1.5, Damage);
    }
}