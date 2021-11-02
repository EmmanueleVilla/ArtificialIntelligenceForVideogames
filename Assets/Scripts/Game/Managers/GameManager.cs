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

    internal void SelectAction(IAvailableAction availableAction)
    {
    }

    void Awake()
    {
        Physics.queriesHitTriggers = true;
        DndModule.RegisterRules();
    }

    void Start()
    {
        Battle = DndModule.Get<IDndBattle>();
    }

    public void EnterMovementMode()
    {
        //this.StartCoroutine(StartMovementMode());
    }

    bool InMovementMode = false;
    List<UnmanagedEdge> NextMovementAvailableCells = new List<UnmanagedEdge>();

    /*
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

        foreach(var tile in tiles)
        {
            if(!result.Any(res => res.X == tile.Y && res.Y == tile.X && res.Speed > 0))
            {
                tile.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
        }

        NextMovementAvailableCells = result.Where(x => x.Speed > 0).ToList();

        // Free the memory allocated by the result array
        result.Dispose();
    }
    */

    public void InitMap()
    {
        this.StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        map = MapBuilder.BuildMap();
        GameStarted?.Invoke(this, EventArgs.Empty);
        yield return StartCoroutine(UIManager.DrawMap(map));
        Battle.Init(map);
        Initiatives = Battle.RollInitiative();
        InitiativesRolled?.Invoke(this, Initiatives);
        TurnStarted?.Invoke(this, Battle.GetCreatureInTurn());
        ActionsManager.SetActions(Battle.GetAvailableActions(Battle.GetCreatureInTurn()));
    }

    internal void OnCellClicked(int x, int y)
    {
        if(InMovementMode)
        {
            //Battle.MoveCurrentCreatureTo(x, y);
        }
    }
}
