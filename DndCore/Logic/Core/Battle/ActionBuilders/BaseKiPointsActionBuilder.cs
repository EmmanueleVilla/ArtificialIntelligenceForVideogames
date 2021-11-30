using Core.Map;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Battle.ActionBuilders
{
    class BaseKiPointsActionBuilder : IActionsBuilder
    {
        public List<IAvailableAction> Build(IMap map, ICreature creature)
        {
            var actions = new List<IAvailableAction>();

            if (creature is IKiPointsOwner && !creature.BonusActionUsedNotToAttack && !creature.BonusActionUsedToAttack && (creature as IKiPointsOwner).RemainingKiPoints > 0)
            {
                actions.Add(new PatientDefenseAction() { ActionEconomy = "B" });
                actions.Add(new DisengageAction() { ActionEconomy = "B" });
                actions.Add(new DashAction() { ActionEconomy = "B" });
            }

            return actions;
        }
    }
}
