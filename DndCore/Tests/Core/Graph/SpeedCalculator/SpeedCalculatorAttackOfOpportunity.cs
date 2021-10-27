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

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorAttackOfOpportunitye : ASpeedCalculatorBaseTests
    {
        [Test]
        public void TakeDamageOne()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new MockedCreature(Sizes.Medium, Loyalties.Ally);
            var enemy = new MockedCreature(Sizes.Medium, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", AttackTypes.WeaponMelee, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0)
                    })
                    });
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(10, result.Damage);
        }

        [Test]
        public void TakeDamageTwo()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new MockedCreature(Sizes.Medium, Loyalties.Ally);
            var enemy = new MockedCreature(Sizes.Medium, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", AttackTypes.WeaponMelee, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0),
                        new Damage(DamageTypes.Acid, 5, 0, 0, 0)
                    })
                    });
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(15, result.Damage);
        }

        [Test]
        public void DontTakeDamageBecauseDisengaged()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new MockedCreature(Sizes.Medium, Loyalties.Ally, disengaged: true);
            var enemy = new MockedCreature(Sizes.Medium, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", AttackTypes.WeaponMelee, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0)
                    })
                    });
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(0, result.Damage);
        }

        [Test]
        public void DontTakeDamageBecauseEnemyDoesntHaveReactions()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new MockedCreature(Sizes.Medium, Loyalties.Ally);
            var enemy = new MockedCreature(Sizes.Medium, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", AttackTypes.WeaponMelee, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0)
                    })
                    }, hasReactions: false);
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(0, result.Damage);
        }
    }
}
