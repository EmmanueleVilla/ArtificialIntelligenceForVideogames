using Assets.Scripts.Jobs;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;

public class ConfirmMovementAction : IAvailableAction
{
    public ActionsTypes ActionType => ActionsTypes.Movement;

    public UnmanagedEdge Edge { get; set; }

    public string Description => string.Format("To {0}-{1}, {2} m, {3} dmg", Edge.X, Edge.Y, Edge.Speed * 1.5, Edge.Damage);
}
