using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    class RangedPositionGoal : AGoal
    {
        public override float EvaluateGoal(ICreature creature, IDndBattle oldState, List<IAvailableAction> actions, IDndBattle newState)
        {
            var team = creature.Loyalty;
            var enemiesPositions = oldState.Map.GetCellsOccupiedBy(oldState.Map.Creatures.FirstOrDefault(x => x.Value.Loyalty != team).Value.Loyalty);
            var myOldPos = oldState.Map.GetCellOccupiedBy(oldState.GetCreatureInTurn());
            var myNewPos = newState.Map.GetCellOccupiedBy(oldState.GetCreatureInTurn());
            var oldNearestEnemy = enemiesPositions.Select(x =>
            {
                var diffX = Math.Abs(x.X - myOldPos.X);
                var diffy = Math.Abs(x.Y - myOldPos.Y);
                return diffX + diffy;
            }).Min();
            var newNearestEnemy = enemiesPositions.Select(x =>
            {
                var diffX = Math.Abs(x.X - myNewPos.X);
                var diffy = Math.Abs(x.Y - myNewPos.Y);
                return diffX + diffy;
            }).Min();
            var fullfillment = newNearestEnemy - oldNearestEnemy;
            //File.AppendAllText("log.txt", "Ranged position: " + fullfillment + "\n");
            return fullfillment;
        }
    }
}
