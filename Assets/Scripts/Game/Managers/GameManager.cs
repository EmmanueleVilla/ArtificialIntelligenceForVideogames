using Assets.Scripts.Jobs;
using Core.DI;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Bestiary;
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
    public void TriggerMovementMode()
    {
        if (!InMovementMode)
        {
            this.StartCoroutine(StartMovementMode());
        } else
        {
            UIManager.ResetMovementHighlight();
            NextMovementAvailableCells.Clear();
            InMovementMode = false;
            ActionsManager.SetActions(Battle.GetAvailableActions());
        }
    }


    List<UnmanagedEdge> NextMovementAvailableCells = new List<UnmanagedEdge>();

    IEnumerator StartMovementMode()
    {
        InMovementMode = true;

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

        UIManager.HighlightMovement(result);

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

    internal void OnCellClicked(int x, int y)
    {
        if(InMovementMode && NextMovementAvailableCells.Any(edge => edge.X == y && edge.Y == x))
        {
            //check if there are multiple paths
            var ends = NextMovementAvailableCells.Where(edge => edge.X == y && edge.Y == x).ToList();
            var actions = new List<IAvailableAction>();
            foreach(var end in ends)
            {
                actions.Add(new ConfirmMovementAction() { Edge = end });
            }
            actions.Add(new CancelMovementAction());
            ActionsManager.SetActions(actions);
        }
    }
}
