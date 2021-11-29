using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.Actions.Attacks
{
    public class ConfirmAttackActionn : IAvailableAction
    {
        public ActionsTypes ActionType => ActionsTypes.ConfirmAttack;

        public ICreature creature;

        public string Description => string.Format("Attack {0}", creature.GetType().ToString());
    }
}