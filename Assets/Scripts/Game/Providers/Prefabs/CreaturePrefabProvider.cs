using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePrefabProvider : MonoBehaviour
{
    public GameObject RatWithBow;
    public GameObject RatWithDagger;
    public GameObject RatWithClaws;
    public GameObject RatWithStaff;
    public GameObject Minotaur;


    public GameObject DwarfMaleWarrior;
    public GameObject ElfFemaleWizard;
    public GameObject HumanFemaleMonk;
    public GameObject HumanMaleRanger;

    public GameObject GetPrefabFor(string name)
    {
        switch (name)
        {
            case "RatmanWithBow":
                return RatWithBow;
            case "RatmanWithClaw":
                return RatWithClaws;
            case "RatmanWithDagger":                
                return RatWithDagger;
            case "RatmanWithStaff":
                return RatWithStaff;
            case "HumanMaleRanger":
                return HumanMaleRanger;
            case "HumanFemaleMonk":
                return HumanFemaleMonk;
            case "DwarfMaleWarrior":
                return DwarfMaleWarrior;
            case "ElfFemaleWizard":
                return ElfFemaleWizard;
            case "LargeMinotaur":
                return Minotaur;
        }
        throw new Exception("No prefab found for creature " + name);
    }
}
