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

    internal void ConfirmMovement(int destinationX, int destinationY, int damage, int speed)
    {
        //Battle.MoveTo(destinationX, destinationY)
        var end = NextMovementAvailableCells.First(edge =>
            edge.X == destinationX
            && edge.Y == destinationY
            && edge.CanEndMovementHere == true
            && edge.Damage == damage
            && edge.Speed == speed
            );
        var path = Battle.GetPathTo(new Edge(CellInfo.Empty(), map.GetCellInfo(destinationX, destinationY), end.Speed, end.Damage, end.CanEndMovementHere));
        path.Add(map.GetCellInfo(end.X, end.Y));
        UIManager.MoveAlong(path);
    }

    public void ExitMovementMode()
    {
        InMovementMode = false;
        ActionsManager.SetActions(Battle.GetAvailableActions());
        UIManager.ResetMovement();
        NextMovementAvailableCells.Clear();
    }

    List<UnmanagedEdge> NextMovementAvailableCells = new List<UnmanagedEdge>();

    IEnumerator StartMovementMode()
    {

        NativeArray<UnmanagedEdge> result = new NativeArray<UnmanagedEdge>(MovementSearchJob.MAX_EDGES, Allocator.Persistent);

        // Set up the job data
        var jobData = new MovementSearchJob
        {
            result = result
        };

        // Schedule the job
        JobHandle handle = jobData.Schedule();

        while (!handle.IsCompleted)
        {
            yield return null;
        }

        handle.Complete();

        UIManager.HighlightMovement(new List<UnmanagedEdge>(result));

        NextMovementAvailableCells = result.Where(x => x.Speed > 0).ToList();

        result.Dispose();
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
        if(InMovementMode && NextMovementAvailableCells.Any(edge => edge.X == y && edge.Y == x))
        {
            //check if there are multiple paths
            var ends = NextMovementAvailableCells.Where(edge => edge.X == y && edge.Y == x).OrderBy(x => x.Damage).ToList();
            var actions = new List<IAvailableAction>();
            UIManager.ResetMovement();
            UIManager.HighlightMovement(NextMovementAvailableCells);
            int index = 0;
            foreach (var end in ends)
            {
                UIManager.ShowPath(Battle.GetPathTo(new Edge(CellInfo.Empty(), map.GetCellInfo(y, x), end.Speed, end.Damage, end.CanEndMovementHere)), end, colors[index]);
                actions.Add(new ConfirmMovementAction() { Damage = end.Damage, DestinationX = end.X, DestinationY = end.Y, Speed = end.Speed }) ;
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
