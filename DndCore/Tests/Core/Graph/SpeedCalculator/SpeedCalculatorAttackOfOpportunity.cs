using Core.Map;
using Logic.Core.Actions;
using Logic.Core.Battle;
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
            var creature = MockedCreature.Build(1, Loyalties.Ally);
            var enemy = MockedCreature.Build(1, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0)
                    }, false, 0)
                    });
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(2, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(GameEvent.Types.Attacks, result.MovementEvents[1].Type);
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
            var creature = MockedCreature.Build(1, Loyalties.Ally);
            var enemy = MockedCreature.Build(1, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0),
                        new Damage(DamageTypes.Acid, 5, 0, 0, 0)
                    })
                    });
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(2, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(GameEvent.Types.Attacks, result.MovementEvents[1].Type);
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
            var creature = MockedCreature.Build(1, Loyalties.Ally, disengaged: true);
            var enemy = MockedCreature.Build(1, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0)
                    })
                    });
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
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
            var creature = MockedCreature.Build(1, Loyalties.Ally);
            var enemy = MockedCreature.Build(1, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0)
                    })
                    }, hasReactions: false);
            map.AddCreature(enemy, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(0, result.Damage);
        }

        [Test]
        public void DontTakeDamageBecauseSameAllyTeam()
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
            var ally = MockedCreature.Build(1, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0)
                    })
                    }, hasReactions: false);
            map.AddCreature(ally, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(0, result.Damage);
        }

        [Test]
        public void DontTakeDamageBecauseSameEnemyTeam()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(1, Loyalties.Enemy);
            var ally = MockedCreature.Build(1, Loyalties.Enemy,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>() {
                        new Damage(DamageTypes.Acid, 10, 0, 0, 0)
                    })
                    }, hasReactions: false);
            map.AddCreature(ally, 1, 1);
            map.AddCreature(creature, 2, 2);
            var result = speedCalculator.GetNeededSpeed(creature, map.GetCellInfo(2, 2), map.GetCellInfo(3, 3), map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(0, result.Damage);
        }
    }
}
