using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPrefabProvider : MonoBehaviour
{
    public GameObject DirtPrefab;
    public GameObject GrassPrefab;
    public GameObject StonePrefab;
    public GameObject RiverPrefab;

    internal GameObject GetPrefabFor(char terrain)
    {
        switch (terrain)
        {
            case 'G':
                return GrassPrefab;
            case 'S':
                return StonePrefab;
            case 'R':
                return RiverPrefab;
        }
        throw new Exception("No prefab found for terrain " + terrain);
    }

    internal GameObject GetFillerPrefabFor(char terrain)
    {
        switch (terrain)
        {
            case 'G':
                return DirtPrefab;
        }
        return null;
    }
}
