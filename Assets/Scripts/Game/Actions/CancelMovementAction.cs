using Logic.Core.Battle;
using Logic.Core.Battle.Actions;

public class CancelMovementAction : IAvailableAction
{
    public ActionsTypes ActionType => ActionsTypes.Movement;

    public string Description => "Cancel movement";
}
