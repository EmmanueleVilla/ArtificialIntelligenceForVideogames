using Assets.Scripts.Jobs;
using Core.DI;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Movement;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Bestiary;
using Logic.Core.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public MapBuilder MapBuilder;
    public ActionsManager ActionsManager;
    public CameraManager CameraManager;
    public SoundManager SoundManager;
    public UIManager UIManager;
    public InitiativeUIManager InitiativeUIManager;

    public event EventHandler<EventArgs> GameStarted;
    public event EventHandler<List<ICreature>> InitiativesRolled;
    public event EventHandler<ICreature> TurnStarted;

    private IMap map;
    private IDndBattle Battle;
    private List<ICreature> Initiatives;

    void Awake()
    {
        Physics.queriesHitTriggers = true;
        DndModule.RegisterRules();
    }

    void Start()
    {
        Battle = DndModule.Get<IDndBattle>();
    }

    bool InMovementMode = false;

    public void EnterMovementMode()
    {
        InMovementMode = true;
        ActionsManager.SetActions(
            new List<IAvailableAction>() {
                new CancelMovementAction()
            });
        this.StartCoroutine(StartMovementMode());
    }

    internal IEnumerator ConfirmMovement(int destinationX, int destinationY, int damage, int speed)
    {
        ActionsManager.SetActions(new List<IAvailableAction>());
        var end = NextMovementAvailableCells.First(edge =>
            edge.Destination.X == destinationX
            && edge.Destination.Y == destinationY
            && edge.CanEndMovementHere == true
            && edge.Damage == damage
            && edge.Speed == speed
            );
        var movementEvents = Battle.MoveTo(end);
        yield return StartCoroutine(UIManager.MoveAlong(movementEvents));
        ExitMovementMode();
    }

    public void ExitMovementMode()
    {
        InMovementMode = false;
        ActionsManager.SetActions(Battle.GetAvailableActions());
        UIManager.ResetMovement();
        NextMovementAvailableCells.Clear();
    }

    List<MemoryEdge> NextMovementAvailableCells = new List<MemoryEdge>();

    IEnumerator StartMovementMode()
    {

        // Set up the job data
        var jobData = new MovementSearchJob();

        // Schedule the job
        JobHandle handle = jobData.Schedule();

        while (!handle.IsCompleted)
        {
            yield return null;
        }

        handle.Complete();

        NextMovementAvailableCells = Battle.GetReachableCells().Where(x => x.Speed > 0).ToList();
        UIManager.HighlightMovement(NextMovementAvailableCells);
    }

    public void InitMap()
    {
        this.StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        map = MapBuilder.BuildMap();
        GameStarted?.Invoke(this, EventArgs.Empty);
        yield return StartCoroutine(UIManager.DrawMap(map));
        Initiatives = Battle.Init(map);
        InitiativesRolled?.Invoke(this, Initiatives);
        TurnStarted?.Invoke(this, Battle.GetCreatureInTurn());
        ActionsManager.SetActions(Battle.GetAvailableActions());
    }

    internal void NextTurn()
    {
        Battle.NextTurn();
        TurnStarted?.Invoke(this, Battle.GetCreatureInTurn());
        ActionsManager.SetActions(Battle.GetAvailableActions());
    }

    List<Color> colors = new List<Color>() { Color.green, Color.yellow, Color.red, Color.magenta, Color.blue, Color.black };

    internal void OnCellClicked(int x, int y)
    {
        if(InMovementMode && NextMovementAvailableCells.Any(edge => edge.Destination.X == y && edge.Destination.Y == x))
        {
            //check if there are multiple paths
            var ends = NextMovementAvailableCells.Where(edge => edge.Destination.X == y && edge.Destination.Y == x).OrderBy(x => x.Damage).ToList();
            var actions = new List<IAvailableAction>();
            UIManager.ResetMovement();
            UIManager.HighlightMovement(NextMovementAvailableCells);
            int index = 0;
            foreach (var end in ends)
            {
                UIManager.ShowPath(Battle.GetPathTo(end), end, colors[index]);
                actions.Add(new ConfirmMovementAction() { Damage = end.Damage, DestinationX = end.Destination.X, DestinationY = end.Destination.Y, Speed = end.Speed }) ;
                index++;
                if(index == colors.Count())
                {
                    index = 0;
                }
            }
            actions.Add(new CancelMovementAction());
            ActionsManager.SetActions(actions);
        }
    }
}
