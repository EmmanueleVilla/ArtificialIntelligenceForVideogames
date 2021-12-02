
namespace Logic.Core.Battle.Actions
{
    public interface IAvailableAction
    {
        BattleActions ActionEconomy { get; set; }
        ActionsTypes ActionType { get; }

        string Description { get; }
    }
}
