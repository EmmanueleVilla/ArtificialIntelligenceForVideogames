using Core.Map;
using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Map.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.Map.AOP
{
    [TestFixture]
    class AttackOfOpportunityTests
    {
        [Test]
        public void LeavingOneEnemy()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(1, Loyalties.Ally);
            var enemy = MockedCreature.Build(1, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>())
                    });
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var expected = new List<ICreature>() { enemy };
            var result = map.IsLeavingThreateningArea(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3));
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void LeavingOneAlly()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(1, Loyalties.Ally);
            var other = MockedCreature.Build(1, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>())
                    });
            map.AddCreature(other, 1, 1);
            map.AddCreature(creature, 2, 2);
            var expected = new List<ICreature>();
            var result = map.IsLeavingThreateningArea(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3));
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void LeavingTwoEnemies()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(1, Loyalties.Ally);
            var enemyOne = MockedCreature.Build(1, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>())
                    });
            var enemyTwo = MockedCreature.Build(1, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>())
                    });
            map.AddCreature(enemyOne, 1, 1);
            map.AddCreature(enemyTwo, 3, 1);
            map.AddCreature(creature, 2, 2);
            var expected = new List<ICreature>() { enemyOne, enemyTwo };
            var result = map.IsLeavingThreateningArea(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3));
            Assert.AreEqual(expected, result);
        }
    }
}
