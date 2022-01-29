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
                var diffX = Math.Pow(x.X - myOldPos.X, 2);
                var diffy = Math.Pow(x.Y - myOldPos.Y, 2);
                return diffX + diffy;
            }).Min();
            var newNearestEnemy = enemiesPositions.Select(x =>
            {
                var diffX = Math.Pow(x.X - myNewPos.X, 2);
                var diffy = Math.Pow(x.Y - myNewPos.Y, 2);
                return diffX + diffy;
            }).Min();
            var fullfillment = newNearestEnemy - oldNearestEnemy;
            if(oldNearestEnemy > 100 && newNearestEnemy > 100)
            {
                fullfillment = 0;
            }
            //File.AppendAllText("log.txt", "Ranged position: " + fullfillment + "\n");
            return (float)fullfillment * 0.2f;
        }
    }
}
