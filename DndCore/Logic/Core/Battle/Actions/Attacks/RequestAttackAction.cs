using Core.Map;
using Logic.Core.Actions;
using System.Collections.Generic;

namespace Logic.Core.Battle.Actions.Attacks
{
    public class RequestAttackAction : IAvailableAction
    {
        public BattleActions ActionEconomy { get; set; } = BattleActions.Action;
        public Attack Attack;

        public ActionsTypes ActionType => ActionsTypes.RequestAttack;

        public List<CellInfo> ReachableCells { get; set; } = new List<CellInfo>();

        public string Description => string.Format("(" + ActionEconomy + ") " + "{0}: {1}m range, {2}d{3}+{4} {5} damage",
            Attack.Name, Attack.Range * 1.5, Attack.Damage[0].NumberOfDice, Attack.Damage[0].DiceFaces, Attack.Damage[0].Modifier, Attack.Damage[0].Type);
        public int Priority => 4;
    }
}
