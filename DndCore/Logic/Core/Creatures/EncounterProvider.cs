using DndCore.DI;
using Logic.Core.Creatures.Bestiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.Creatures
{
    public class EncounterProvider
    {
        public List<ICreature> BuildEncounter()
        {
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
        }
    }

}
