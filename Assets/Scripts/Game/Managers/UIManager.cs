using Assets.Scripts.Jobs;
using Core.DI;
using Core.Map;
using Logic.Core.Battle;
using Logic.Core.Creatures;
using Logic.Core.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using ILogger = Core.Utils.Log.ILogger;

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject MapRoot;
    public GameObject Miss;

    public TerrainPrefabProvider TerrainPrefabProvider;
    public CreaturePrefabProvider CreaturePrefabProvider;
    List<SpriteManager> tiles = new List<SpriteManager>();

    public static GameObject creatureInTurn;
    List<Tuple<int, InitiativeIndicator>> initiativeIndicators = new List<Tuple<int, InitiativeIndicator>>();

    public GameObject Ball;

    void Start()
    {
        Miss.SetActive(false);
    }

    void Update()
    {
        foreach (var indicator in initiativeIndicators)
        {
            if (!GameManager.Battle.Map.Creatures.ContainsKey(indicator.Item1))
            {
                indicator.Item2.gameObject.SetActive(false);
            }
            if (indicator.Item1 == GameManager.Battle.GetCreatureInTurn().Id)
            {
                indicator.Item2.Show();
                creatureInTurn = indicator.Item2.gameObject;
            }
            else
            {
                indicator.Item2.Hide();
            }
        }
    }

    public IEnumerator DrawMap(IMap map)
    {
        var offsetUp = 0f;
        var offsetRight = 0f;
        for (int x = 0; x < map.Height; x++)
        {
            yield return null;
            for (int y = 0; y < map.Width; y++)
            {
                var cell = map.GetCellInfo(y, x);
                var prefab = TerrainPrefabProvider.GetPrefabFor(cell.Terrain);
                GameObject go = Instantiate(prefab);
                
                if (go != null)
                {
                    go.transform.parent = MapRoot.transform;
                    go.transform.localPosition = new Vector3(offsetUp, offsetRight, 0);
                    go.transform.localPosition += Vector3.up * 2.45f * y;
                    go.transform.localPosition += Vector3.right * 4.25f * y;
                    go.transform.localPosition += Vector3.up * cell.Height * 1.5f;
                    if (cell.Height > 0 && cell.Terrain == 'G')
                    {
                        var height = cell.Height - 1;
                        do
                        {
                            var child = Instantiate(TerrainPrefabProvider.GetFillerPrefabFor(cell.Terrain));
                            if(child == null)
                            {
                                continue;
                            }
                            child.transform.parent = MapRoot.transform;
                            child.transform.localPosition = new Vector3(offsetUp, offsetRight, 0);
                            child.transform.localPosition += Vector3.up * 2.45f * y;
                            child.transform.localPosition += Vector3.right * 4.25f * y;
                            child.GetComponent<SpriteRenderer>().sortingOrder = (x * map.Height - y + height) * 10;
                            child.transform.localPosition += Vector3.up * height * 1.5f;
                            height--;
                        } while (height >= 0);
                    }
                    var sprites = go.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var sprite in sprites)
                    {
                        sprite.sortingOrder += (x * map.Height - y + map.Height) * 10;
                    }
                    var spriteManager = go.GetComponent<SpriteManager>();
                    tiles.Add(spriteManager);
                    spriteManager.X = x;
                    spriteManager.Y = y;
                }
            }
            offsetUp += 4.3f;
            offsetRight -= 2.5f;
        }

        var maxSortingOrder = tiles.Max(x => x.GetComponent<SpriteRenderer>().sortingOrder);

        foreach (var go in tiles)
        {
            var xPos = go.GetComponent<SpriteManager>().X;
            var yPos = go.GetComponent<SpriteManager>().Y;
            var cell = map.GetCellInfo(yPos, xPos);

            if (cell.Creature != null)
            {
                yield return null;

                var creatureName = cell.Creature.GetType().ToString().Split('.').Last();
                GameObject creature = Instantiate(CreaturePrefabProvider.GetPrefabFor(creatureName));
                
                if (creature != null)
                {
                    var indicator = creature.GetComponent<InitiativeIndicator>();
                    indicator.Hide();
                    initiativeIndicators.Add(new Tuple<int, InitiativeIndicator>(cell.Creature.Id, indicator));
                    creature.transform.parent = MapRoot.transform;
                    creature.transform.localPosition = go.transform.localPosition;
                    var cells = map.GetCellsOccupiedBy(yPos, xPos);
                    var max = cells.OrderByDescending(c => c.Y).ThenBy(c => c.X).First();
                    var tile = tiles.FirstOrDefault(tile => tile.X == max.Y && tile.Y == max.X);
                    int offset = 1;
                    if (tile != null)
                    {
                        foreach (var renderer in creature.GetComponentsInChildren<SpriteRenderer>())
                        {
                            renderer.sortingOrder = maxSortingOrder + 1 + offset++;
                        }
                    }
                }
            }
        }
        yield return null;
    }

    internal IEnumerator ShowGameEvents(List<GameEvent> events)
    {
        foreach (var eve in events)
        {
            //DndModule.Get<ILogger>().WriteLine(eve.Type + ", " + eve.Ability);
            if (eve.Type == GameEvent.Types.Movement)
            {
                var tile = tiles.First(tile => tile.X == eve.Destination.Y && tile.Y == eve.Destination.X);
                var target = tile.transform.localPosition;
                DndModule.Get<ILogger>().WriteLine(string.Format("Moving to {0}-{1}", eve.Destination.X, eve.Destination.Y));
                yield return StartCoroutine(MoveToIterator(creatureInTurn,
                    creatureInTurn.transform.localPosition,
                    target,
                    0.25f
                    ));
            }
            if (eve.Type == GameEvent.Types.Falling)
            {
                var renderers = creatureInTurn.GetComponentsInChildren<SpriteRenderer>().ToList();
                DndModule.Get<ILogger>().WriteLine(string.Format("Taking {0} fall damage", eve.Damage));
                yield return StartCoroutine(ColorEffect(renderers, 0.25f, Color.red));
            }

            if (eve.Type == GameEvent.Types.SelfAbility)
            {
                var renderers = creatureInTurn.GetComponentsInChildren<SpriteRenderer>().ToList();
                DndModule.Get<ILogger>().WriteLine("Used " + eve.Ability);
                yield return StartCoroutine(ColorEffect(renderers, 0.25f, Color.green));
            }

            if(eve.Type == GameEvent.Types.AttackMissed)
            {
                DndModule.Get<ILogger>().WriteLine(string.Format(eve.LogDescription));
                GameObject target = null;
                GameObject start = null;
                GameObject ball = Instantiate(Ball);
                foreach (var indicator in initiativeIndicators)
                {
                    if (indicator.Item1 == eve.Attacker)
                    {
                        start = indicator.Item2.gameObject;
                    }
                    if (indicator.Item1 == eve.Attacked)
                    {
                        target = indicator.Item2.gameObject;
                    }
                }
                ball.transform.parent = start.transform.parent;
                Miss.transform.position = target.transform.position;
                Miss.transform.position += Vector3.up * 8;
                yield return StartCoroutine(Move(ball, start.transform.localPosition + Vector3.up * 5, target.transform.localPosition + Vector3.up * 5, 1.0f));
                Destroy(ball);
                Miss.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                Miss.SetActive(false);
            }

            if(eve.Type == GameEvent.Types.Spell)
            {
                DndModule.Get<ILogger>().WriteLine(eve.LogDescription);
                GameObject target = null;
                GameObject start = null;
                GameObject ball = Instantiate(Ball);
                foreach (var indicator in initiativeIndicators)
                {
                    if (indicator.Item1 == eve.Attacker)
                    {
                        start = indicator.Item2.gameObject;
                    }
                    if (indicator.Item1 == eve.Attacked)
                    {
                        target = indicator.Item2.gameObject;
                    }
                }
                if (start != null && target != null)
                {
                    ball.transform.parent = start.transform.parent;
                    yield return StartCoroutine(Move(ball, start.transform.localPosition + Vector3.up * 5, target.transform.localPosition + Vector3.up * 5, 1.0f));
                    var renderers = target.GetComponentsInChildren<SpriteRenderer>().ToList();
                    yield return StartCoroutine(ColorEffect(renderers, 0.25f, Color.red));
                }
                Destroy(ball);
            }

            if (eve.Type == GameEvent.Types.Attacks)
            {
                DndModule.Get<ILogger>().WriteLine(eve.LogDescription);
                GameObject target = null;
                GameObject start = null;
                GameObject ball = Instantiate(Ball);
                foreach (var indicator in initiativeIndicators)
                {
                    if(indicator.Item1 == eve.Attacker)
                    {
                        start = indicator.Item2.gameObject;
                    }
                    if (indicator.Item1 == eve.Attacked)
                    {
                        target = indicator.Item2.gameObject;
                    }
                }
                ball.transform.parent = start.transform.parent;
                yield return StartCoroutine(Move(ball, start.transform.localPosition + Vector3.up * 5, target.transform.localPosition + Vector3.up * 5, 1.0f));
                Destroy(ball);
                var renderers = target.GetComponentsInChildren<SpriteRenderer>().ToList();
                yield return StartCoroutine(ColorEffect(renderers, 0.25f, Color.red));
            }
            yield return null;
        }
    }

    private IEnumerator Move(GameObject go, Vector3 start, Vector3 end, float time)
    {
        var now = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - now < time)
        {
            var newPos = Vector3.Lerp(start, end, (Time.realtimeSinceStartup - now) / time);
            go.transform.localPosition = newPos;
            yield return null;
        }
    }

    private IEnumerator ColorEffect(List<SpriteRenderer> renderers, float time, Color end)
    {
        time *= 2;
        var now = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - now < time)
        {
            var newColor = Color.Lerp(Color.white, end, (Time.realtimeSinceStartup - now) / time);
            foreach(var renderer in renderers)
            {
                renderer.color = newColor;
            }
            yield return null;
        }

        now = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - now < time)
        {
            var newColor = Color.Lerp(end, Color.white, (Time.realtimeSinceStartup - now) / time);
            foreach (var renderer in renderers)
            {
                renderer.color = newColor;
            }
            yield return null;
        }
    }

    private IEnumerator MoveToIterator(GameObject go, Vector3 start, Vector3 end, float time)
    {
        var now = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - now < time)
        {
            var newPos = Vector3.Lerp(start, end, (Time.realtimeSinceStartup - now) / time);
            go.transform.localPosition = newPos;
            yield return null;
        }
    }

    internal void ShowPath(List<CellInfo> cellPath, MemoryEdge end, Color color)
    {
        foreach (var tile in tiles)
        {
            if (cellPath.Any(res => res.X == tile.Y && res.Y == tile.X) || (end.Destination.X == tile.Y && end.Destination.Y == tile.X))
            {
                tile.knob.color = color;
            }
        }
    }

    internal void HighlightSpell(List<CellInfo> reachableCells)
    {
        foreach (var tile in tiles)
        {
            if (!reachableCells.Any(res => res.X == tile.Y && res.Y == tile.X))
            {
                tile.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
            }
        }
    }

    internal void ResetSpells()
    {
        ResetCellsUI();
    }

    internal void HighlightAttack(List<CellInfo> reachableCells)
    {
        foreach (var tile in tiles)
        {
            if (!reachableCells.Any(res => res.X == tile.Y && res.Y == tile.X))
            {
                tile.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
            }
        }
    }

    internal void ResetAttacks()
    {
        ResetCellsUI();
    }

    internal void HighlightMovement(List<MemoryEdge> result)
    {
        foreach (var tile in tiles)
        {
            if (!result.Any(res => res.Destination.X == tile.Y && res.Destination.Y == tile.X && res.Speed > 0))
            {
                tile.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
            }
        }
    }

    internal void ResetCellsUI()
    {
        foreach (var tile in tiles)
        {
            tile.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            tile.knob.color = new Color(1, 1, 1, 0);
        }
    }
}
