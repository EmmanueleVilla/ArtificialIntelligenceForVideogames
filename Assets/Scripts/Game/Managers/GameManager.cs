using Assets.Scripts.Jobs;
using Core.DI;
using Core.Map;
using Logic.Core.Actions;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Battle.Actions.Movement;
using Logic.Core.Battle.Actions.Spells;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities.Spells;
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
using ILogger = Core.Utils.Log.ILogger;

public class GameManager : MonoBehaviour
{
    public TextLogger TextLogger;
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
        DndModule.RegisterRules(enableLogs: true, logger: TextLogger);
    }

    void Start()
    {
        Battle = DndModule.Get<IDndBattle>();
        var newBattle = Battle.Copy();
    }

    IEnumerator SetAvailableActions()
    {
        // Set up the job data
        var jobData = new AvailableActionsJob();

        // Schedule the job
        JobHandle handle = jobData.Schedule();

        while (!handle.IsCompleted)
        {
            yield return null;
        }

        handle.Complete();

        ActionsManager.SetActions(Battle.GetAvailableActions());
    }

    bool InMovementMode = false;
    bool InSpellMode = false;
    ISpell spell = null;
    public void EnterSpellMode(RequestSpellAction action)
    {
        Debug.Log("enter spell mode");
        InSpellMode = true;
        spell = action.Spell;
        ActionsManager.SetActions(
            new List<IAvailableAction>() {
                new CancelSpellAction()
            });
        UIManager.HighlightSpell(action.ReachableCells);
    }
    public void EnterMovementMode()
    {
        InMovementMode = true;
        ActionsManager.SetActions(
            new List<IAvailableAction>() {
                new CancelMovementAction()
            });
        StartMovementMode();
    }

    bool InAttackMode = false;
    RequestAttackAction requestedAttack;

    public void EnterAttackMode(RequestAttackAction action)
    {
        InAttackMode = true;
        requestedAttack = action;
        ActionsManager.SetActions(
            new List<IAvailableAction>()
            {
                new CancelAttackAction()
            });
        UIManager.HighlightAttack(action.ReachableCells);
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
        var GameEvents = Battle.MoveTo(end);
        yield return StartCoroutine(UIManager.MoveAlong(GameEvents));
        ExitMovementMode();
    }

    internal void UseAbility(IAvailableAction availableAction)
    {
        Battle.UseAbility(availableAction);
        ActionsManager.SetActions(new List<IAvailableAction>());
        this.StartCoroutine(SetAvailableActions());
    }

    internal IEnumerator ConfirmAttack(ConfirmAttackAction confirmAttackAction)
    {
        ActionsManager.SetActions(new List<IAvailableAction>());
        var events = Battle.Attack(confirmAttackAction);
        yield return this.StartCoroutine(UIManager.ShowGameEvents(events));
        ExitAttackMode();
    }

    internal IEnumerator ConfirmSpell(ConfirmSpellAction confirmSpellAction)
    {
        ActionsManager.SetActions(new List<IAvailableAction>());
        var events = Battle.Spell(confirmSpellAction);
        yield return this.StartCoroutine(UIManager.ShowGameEvents(events));
        ExitAttackMode();
    }

    public void ExitAttackMode()
    {
        InAttackMode = false;
        ActionsManager.SetActions(new List<IAvailableAction>());
        this.StartCoroutine(SetAvailableActions());
        UIManager.ResetAttacks();
    }

    internal void ExitSpellMode()
    {
        InSpellMode = false;
        ActionsManager.SetActions(new List<IAvailableAction>());
        this.StartCoroutine(SetAvailableActions());
        UIManager.ResetAttacks();
    }

    public void ExitMovementMode()
    {
        InMovementMode = false;
        ActionsManager.SetActions(new List<IAvailableAction>());
        this.StartCoroutine(SetAvailableActions());
        UIManager.ResetCellsUI();
        NextMovementAvailableCells.Clear();
    }

    List<MemoryEdge> NextMovementAvailableCells = new List<MemoryEdge>();

    void StartMovementMode()
    {
        NextMovementAvailableCells = Battle.GetReachableCells().Where(x => x.Speed > 0 && x.CanEndMovementHere).ToList();
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
        var creature = Battle.GetCreatureInTurn();
        TurnStarted?.Invoke(this, creature);
        DndModule.Get<ILogger>().WriteLine("\nStart turn of " + creature.GetType().ToString().Split('.').Last());
        ActionsManager.SetActions(new List<IAvailableAction>());
        this.StartCoroutine(SetAvailableActions());
    }

    internal void NextTurn()
    {
        Battle.NextTurn();
        var creature = Battle.GetCreatureInTurn();
        DndModule.Get<ILogger>().WriteLine("\nStart turn of " + creature.GetType().ToString().Split('.').Last());
        TurnStarted?.Invoke(this, creature);
        ActionsManager.SetActions(new List<IAvailableAction>());
        this.StartCoroutine(SetAvailableActions());
    }

    List<Color> colors = new List<Color>() { Color.green, Color.yellow, Color.red, Color.magenta, Color.blue, Color.black };

    internal void OnCellClicked(int x, int y)
    {
        if(InSpellMode)
        {
            var actions = new List<IAvailableAction>();
            actions.Add(new ConfirmSpellAction(Battle.GetCreatureInTurn(), spell)
            {
                Target = map.GetCellInfo(x, y)
            }); ;
            actions.Add(new CancelSpellAction());
            ActionsManager.SetActions(actions);
        }

        if(InAttackMode)
        {
            var creature = map.GetOccupantCreature(y, x);
            if (creature != null)
            {
                var actions = new List<IAvailableAction>();
                actions.Add(new ConfirmAttackAction()
                {
                    TargetCreature = creature,
                    AttackingCreature = Battle.GetCreatureInTurn(),
                    Attack = requestedAttack.Attack,
                    ActionEconomy = requestedAttack.ActionEconomy
                });
                actions.Add(new CancelAttackAction());
                ActionsManager.SetActions(actions);
            }
        }

        if(InMovementMode && NextMovementAvailableCells.Any(edge => edge.Destination.X == y && edge.Destination.Y == x))
        {
            //check if there are multiple paths
            var ends = NextMovementAvailableCells.Where(edge => edge.Destination.X == y && edge.Destination.Y == x).OrderBy(x => x.Damage).ToList();
            var actions = new List<IAvailableAction>();
            UIManager.ResetCellsUI();
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
