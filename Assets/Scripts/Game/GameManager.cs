using Assets.Scripts.Jobs;
using Core.DI;
using Core.Map;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IMap map;
    public TextAsset RiverMap;
    public Camera menuCamera;
    public Camera gameCamera;

    public GameObject MapRoot;
    public GameObject DirtPrefab;
    public GameObject GrassPrefab;
    public GameObject StonePrefab;
    public GameObject RiverPrefab;

    void Start()
    {
        Physics.queriesHitTriggers = true;
        menuCamera.gameObject.SetActive(true);
        gameCamera.gameObject.SetActive(false);
        DndModule.RegisterRules();
        this.StartCoroutine(StartJob());
    }

    IEnumerator StartJob()
    {
        NativeArray<int> result = new NativeArray<int>(1, Allocator.Persistent);

        // Set up the job data
        var jobData = new VeryTimeConsumingJob
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

        // All copies of the NativeArray point to the same memory, you can access the result in "your" copy of the NativeArray
        int res = result[0];

        Debug.Log("Result is " + res);

        // Free the memory allocated by the result array
        result.Dispose();
    }
    
    void Update()
    {
        
    }

    public void InitRiverMap()
    {
        map = DndModule.Get<IMapBuilder>().FromCsv(RiverMap.text);
        InitGame();
    }

    private void InitGame()
    {
        menuCamera.gameObject.SetActive(false);
        gameCamera.gameObject.SetActive(true);
        var offsetUp = 0f;
        var offsetRight = 0f;
        for (int x = 0; x < map.Height; x++) 
        {
            for (int y = 0; y < map.Width; y++)
            {
                var cell = map.GetCellInfo(y, x);
                GameObject go = null;
                switch(cell.Terrain)
                {
                    case 'G':
                        go = Instantiate(GrassPrefab);
                        break;
                    case 'S':
                        go = Instantiate(StonePrefab);
                        break;
                    case 'R':
                        go = Instantiate(RiverPrefab);
                        break;
                }
                if(go != null)
                {
                    go.transform.parent = MapRoot.transform;
                    go.transform.localPosition = new Vector3(offsetUp, offsetRight, 0);
                    go.transform.localPosition += Vector3.up * 2.45f * y;
                    go.transform.localPosition += Vector3.right * 4.25f * y;
                    go.transform.localPosition += Vector3.up * cell.Height * 1.5f;
                    if(cell.Height > 0 && cell.Terrain == 'G')
                    {
                        var height = cell.Height - 1;
                        do
                        {
                            var child = Instantiate(DirtPrefab);
                            child.transform.parent = MapRoot.transform;
                            child.transform.localPosition = new Vector3(offsetUp, offsetRight, 0);
                            child.transform.localPosition += Vector3.up * 2.45f * y;
                            child.transform.localPosition += Vector3.right * 4.25f * y;
                            child.GetComponent<SpriteRenderer>().sortingOrder = x * map.Height - y + height;
                            child.transform.localPosition += Vector3.up * height * 1.5f;
                            height--;
                        } while (height >= 0);
                    }
                    go.GetComponent<SpriteRenderer>().sortingOrder = x * map.Height - y + map.Height;
                    var spriteManager = go.GetComponent<SpriteManager>();
                    if (spriteManager != null) {
                        spriteManager.X = x;
                        spriteManager.Y = y;
                    }
                }
            }
            offsetUp += 4.3f;
            offsetRight -= 2.5f;
        }
    }
}
