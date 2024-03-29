﻿using DndCore.Map;
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
        public BattleActions ActionEconomy { get; set; } = BattleActions.Action;
        public ActionsTypes ActionType => ActionsTypes.ConfirmAttack;

        public int AttackingCreature;
        public int TargetCreature;
        public Attack Attack;
        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>();
        public string Description => string.Format("Confirm attack to {0}", TargetCreature.GetType().ToString().Split('.').Last());
        public int Priority => 0;
    }
}