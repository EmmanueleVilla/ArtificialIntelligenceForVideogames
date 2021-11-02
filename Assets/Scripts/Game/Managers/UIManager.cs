using Core.Map;
using Logic.Core.Creatures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject MapRoot;

    public TerrainPrefabProvider TerrainPrefabProvider;
    public CreaturePrefabProvider CreaturePrefabProvider;
    List<SpriteManager> tiles = new List<SpriteManager>();

    List<Tuple<ICreature, InitiativeIndicator>> initiativeIndicators = new List<Tuple<ICreature, InitiativeIndicator>>();

    void Start()
    {
        GameManager.TurnStarted += GameManager_TurnStarted;
    }

    private void GameManager_TurnStarted(object sender, ICreature e)
    {
        foreach (var indicator in initiativeIndicators)
        {
            if (indicator.Item1.Id == e.Id)
            {
                indicator.Item2.Show();
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
                    go.GetComponent<SpriteRenderer>().sortingOrder = (x * map.Height - y + map.Height) * 10;
                    var spriteManager = go.GetComponent<SpriteManager>();
                    tiles.Add(spriteManager);
                    spriteManager.X = x;
                    spriteManager.Y = y;
                }
            }
            offsetUp += 4.3f;
            offsetRight -= 2.5f;
        }

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
                    initiativeIndicators.Add(new Tuple<ICreature, InitiativeIndicator>(cell.Creature, indicator));
                    creature.transform.parent = go.transform;
                    creature.transform.localPosition = Vector3.zero;
                    var cells = map.GetCellsOccupiedBy(yPos, xPos);
                    var max = cells.OrderByDescending(c => c.Y).ThenBy(c => c.X).First();
                    var tile = tiles.FirstOrDefault(tile => tile.X == max.Y && tile.Y == max.X);
                    int offset = 1;
                    if (tile != null)
                    {
                        foreach (var renderer in creature.GetComponentsInChildren<SpriteRenderer>())
                        {
                            renderer.sortingOrder =
                                tile.gameObject.GetComponent<SpriteRenderer>().sortingOrder + offset++;
                        }
                    }
                }
            }
        }
        yield return null;
    }
}
