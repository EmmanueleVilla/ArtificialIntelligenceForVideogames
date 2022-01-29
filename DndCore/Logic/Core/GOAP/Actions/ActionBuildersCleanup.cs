using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.GOAP.Actions
{
    public class ActionBuildersCleanup : IActionBuildersCleanup
    {
        public List<IAvailableAction> Cleanup(ICreature creature, List<IAvailableAction> cachedActions)
        {
            var newList = cachedActions.AsEnumerable();
            if(cachedActions.Count > 0)
            {
                if (!cachedActions.Any(x => x is RequestAttackAction))
                {
                    newList = newList.Where(x => !(x is FightingSpiritAction));
                } else
                {
                    newList = newList.Where(x => !(x is FlurryOfBlowsAction));
                }

                if (creature.HitPoints - creature.CurrentHitPoints < 15)
                {
                    newList = newList.Where(x => !(x is SecondWindAction));
                }
            }
            return newList.ToList();
        }
    }
}
