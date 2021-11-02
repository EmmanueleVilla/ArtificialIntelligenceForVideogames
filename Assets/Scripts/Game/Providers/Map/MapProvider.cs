using Core.DI;
using Core.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProvider : MonoBehaviour
{
    public TextAsset[] Maps;

    public IMap BuildMap()
    {
        var mapIndex = DndModule.Get<System.Random>().Next(0, Maps.Length);
        var map = DndModule.Get<IMapBuilder>().FromCsv(Maps[mapIndex].text);
        return map;
    }
}
