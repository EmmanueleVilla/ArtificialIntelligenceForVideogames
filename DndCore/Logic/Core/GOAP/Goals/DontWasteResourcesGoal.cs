using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    class DontWasteResourcesGoal : AGoal
    {
        public override float EvaluateGoal(ICreature creature, IDndBattle oldState, List<IAvailableAction> actions, IDndBattle newState)
        {
            //use fightning Spirit only before an attack
            var fightningSpirit = actions.FindIndex(x => x is FightingSpiritAction);
            var attack = actions.FindIndex(x => x is ConfirmAttackAction);
            if(fightningSpirit >= 0 && fightningSpirit > attack)
            {
                //File.AppendAllText("log.txt", "Wasted resources: " + float.MinValue + "\n");
                return float.MinValue;
            }

            if(creature.HitPoints - creature.CurrentHitPoints < 15 && actions.Any(x => x is SecondWindAction))
            {
                //File.AppendAllText("log.txt", "Wasted resources: " + float.MinValue + "\n");
                return float.MinValue;
            }
            //File.AppendAllText("log.txt", "Wasted resources: " + 0 + "\n");
            return 0;
        }
    }
}
