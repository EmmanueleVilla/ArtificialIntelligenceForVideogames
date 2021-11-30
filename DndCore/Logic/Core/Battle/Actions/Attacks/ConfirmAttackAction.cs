using Logic.Core.Actions;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Battle.Actions.Attacks
{
    public class ConfirmAttackAction : IAvailableAction
    {
        public string ActionEconomy = "";
        public ActionsTypes ActionType => ActionsTypes.ConfirmAttack;

        public ICreature Creature;
        public Attack Attack;

        public string Description => string.Format("Confirm attack to {0}", Creature.GetType().ToString().Split('.').Last());
    }
}