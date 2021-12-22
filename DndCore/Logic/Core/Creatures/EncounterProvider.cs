using Core.DI;
using Logic.Core.Creatures.Bestiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Creatures
{
    public class EncounterProvider
    {
        private int ENCOUNTERS_SIZE = 2;
        internal List<ICreature> BuildEncounter()
        {
            var encounterIndex = DndModule.Get<Random>().Next(0, ENCOUNTERS_SIZE);
            switch (encounterIndex)
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
                }.Select(x => x.Init()).ToList();
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
                }.Select(x => x.Init()).ToList();
            }
            throw new Exception("Invalid encounterIndex " + encounterIndex);
        }
    }

}
