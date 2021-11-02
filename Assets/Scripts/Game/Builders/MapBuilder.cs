using Core.DI;
using Core.Map;
using Logic.Core.Creatures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    public MapProvider MapProvider;
    public EncounterProvider EncounterProvider;

    private List<char> validTerrains = new List<char>() { 'G' };

    internal IMap BuildMap()
    {
        var map = MapProvider.BuildMap();
        var creatures = EncounterProvider.BuildEncounter();

        var random = DndModule.Get<System.Random>();
        foreach (var creature in creatures)
        {
            bool fit = false;
            while (!fit)
            {
                var startingX = creature.Loyalty == Loyalties.Ally ? 0 : map.Width / 3 * 2;
                var endingX = creature.Loyalty == Loyalties.Ally ? map.Width / 3 : map.Width;
                var x = random.Next(startingX, endingX);
                var y = random.Next(3, map.Height - 3);
                if (!validTerrains.Contains(map.GetCellInfo(x, y).Terrain))
                {
                    continue;
                }

                fit = map.AddCreature(creature, x, y);
            }
        }
        return map;
    }
}
