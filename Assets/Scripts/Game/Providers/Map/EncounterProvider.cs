using Core.DI;
using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Bestiary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterProvider: MonoBehaviour
{
    private int ENCOUNTERS_SIZE = 2;
    internal List<ICreature> BuildEncounter()
    {
        var encounterIndex = DndModule.Get<System.Random>().Next(0, ENCOUNTERS_SIZE);
        switch(encounterIndex)
        {
            case 0:
                return new List<ICreature>() {
                    new RatmanWithBow(),
                    new RatmanWithClaw(),
                    new RatmanWithDagger(),
                    new RatmanWithStaff(),
                    new LargeMinotaur(),
                    new HumanMaleRanger(),
                    new HumanFemaleMonk(),
                    new DwarfMaleWarrior(),
                    new ElfFemaleWizard()
                };
            case 1:
                return new List<ICreature>() {
                    new RatmanWithBow(),
                    new LargeMinotaur(),
                    new RatmanWithStaff(),
                    new LargeMinotaur(),
                    new HumanMaleRanger(),
                    new HumanFemaleMonk(),
                    new DwarfMaleWarrior(),
                    new ElfFemaleWizard()
                };
        }
        throw new Exception("Invalid encounterIndex " + encounterIndex);
    }
}
