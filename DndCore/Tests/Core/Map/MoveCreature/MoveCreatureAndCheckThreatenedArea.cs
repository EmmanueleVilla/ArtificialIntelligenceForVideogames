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

namespace Tests.Core.Map.MoveCreature
{
    [TestFixture]
    class MoveCreatureAndCheckThreatenedArea
    {
        [Test]
        public void AddMediumCreatureWithBaseAttack()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new MockedCreature(Sizes.Medium, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", AttackTypes.WeaponMelee, new List<Damage>())
                    });
            map.AddCreature(creature, 0, 0);
            map.MoveCreatureTo(creature, new Logic.Core.Graph.MemoryEdge(
                new List<CellInfo>() { map.GetCellInfo(0, 0) },
                map.GetCellInfo(1, 1), 1, 0, true));

            var expected = new List<CellInfo>() {
                new CellInfo('G', 0, null, 0, 0),
                new CellInfo('G', 0, null, 0, 1),
                new CellInfo('G', 0, null, 0, 2),
                new CellInfo('G', 0, null, 1, 0),
                new CellInfo('G', 0, null, 1, 1),
                new CellInfo('G', 0, null, 1, 2),
                new CellInfo('G', 0, null, 2, 0),
                new CellInfo('G', 0, null, 2, 1),
                new CellInfo('G', 0, null, 2, 2),
            };
            Assert.AreEqual(expected, map.threateningAreas.FirstOrDefault(x => x.Item1 == creature).Item2);
        }

        [Test]
        public void AddMediumCreatureWithReach()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new MockedCreature(Sizes.Medium, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", AttackTypes.WeaponMeleeReach, new List<Damage>())
                    });
            map.AddCreature(creature, 0, 0);
            map.MoveCreatureTo(creature, new Logic.Core.Graph.MemoryEdge(
                new List<CellInfo>() { map.GetCellInfo(0, 0) },
                map.GetCellInfo(1, 1), 1, 0, true));

            var expected = new List<CellInfo>() {
                new CellInfo(' ', 0, null, -1, -1),
                new CellInfo(' ', 0, null, -1, 0),
                new CellInfo(' ', 0, null, -1, 1),
                new CellInfo(' ', 0, null, -1, 2),
                new CellInfo(' ', 0, null, -1, 3),

                new CellInfo(' ', 0, null, 0, -1),
                new CellInfo('G', 0, null, 0, 0),
                new CellInfo('G', 0, null, 0, 1),
                new CellInfo('G', 0, null, 0, 2),
                new CellInfo('G', 0, null, 0, 3),

                new CellInfo(' ', 0, null, 1, -1),
                new CellInfo('G', 0, null, 1, 0),
                new CellInfo('G', 0, null, 1, 1),
                new CellInfo('G', 0, null, 1, 2),
                new CellInfo('G', 0, null, 1, 3),

                new CellInfo(' ', 0, null, 2, -1),
                new CellInfo('G', 0, null, 2, 0),
                new CellInfo('G', 0, null, 2, 1),
                new CellInfo('G', 0, null, 2, 2),
                new CellInfo('G', 0, null, 2, 3),

                new CellInfo(' ', 0, null, 3, -1),
                new CellInfo('G', 0, null, 3, 0),
                new CellInfo('G', 0, null, 3, 1),
                new CellInfo('G', 0, null, 3, 2),
                new CellInfo('G', 0, null, 3, 3),
            };
            Assert.AreEqual(expected, map.threateningAreas.FirstOrDefault(x => x.Item1 == creature).Item2);
        }

        [Test]
        public void AddHugeCreatureWithBaseAttack()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new MockedCreature(Sizes.Huge, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", AttackTypes.WeaponMelee, new List<Damage>())
                    });
            map.AddCreature(creature, 0, 0);
            map.MoveCreatureTo(creature, new Logic.Core.Graph.MemoryEdge(
                new List<CellInfo>() { map.GetCellInfo(0, 0) },
                map.GetCellInfo(1, 1), 1, 0, true));

            var expected = new List<CellInfo>() {
                new CellInfo('G', 0, null, 0, 0),
                new CellInfo('G', 0, null, 0, 1),
                new CellInfo('G', 0, null, 0, 2),
                new CellInfo('G', 0, null, 0, 3),
                new CellInfo('G', 0, null, 0, 4),

                new CellInfo('G', 0, null, 1, 0),
                new CellInfo('G', 0, null, 1, 1),
                new CellInfo('G', 0, null, 1, 2),
                new CellInfo('G', 0, null, 1, 3),
                new CellInfo('G', 0, null, 1, 4),

                new CellInfo('G', 0, null, 2, 0),
                new CellInfo('G', 0, null, 2, 1),
                new CellInfo('G', 0, null, 2, 2),
                new CellInfo('G', 0, null, 2, 3),
                new CellInfo('G', 0, null, 2, 4),

                new CellInfo('G', 0, null, 3, 0),
                new CellInfo('G', 0, null, 3, 1),
                new CellInfo('G', 0, null, 3, 2),
                new CellInfo('G', 0, null, 3, 3),
                new CellInfo('G', 0, null, 3, 4),

                new CellInfo('G', 0, null, 4, 0),
                new CellInfo('G', 0, null, 4, 1),
                new CellInfo('G', 0, null, 4, 2),
                new CellInfo('G', 0, null, 4, 3),
                new CellInfo('G', 0, null, 4, 4),
            };
            var result = map.threateningAreas.FirstOrDefault(x => x.Item1 == creature).Item2;
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void AddHugeCreatureWithReach()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new MockedCreature(Sizes.Huge, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", AttackTypes.WeaponMeleeReach, new List<Damage>())
                    });
            map.AddCreature(creature, 1, 1);
            map.MoveCreatureTo(creature, new Logic.Core.Graph.MemoryEdge(
                new List<CellInfo>() { map.GetCellInfo(1, 1) },
                map.GetCellInfo(0, 0), 1, 0, true));

            var expected = new List<CellInfo>() {
                new CellInfo(' ', 0, null, -2, -2),
                new CellInfo(' ', 0, null, -2, -1),
                new CellInfo(' ', 0, null, -2, 0),
                new CellInfo(' ', 0, null, -2, 1),
                new CellInfo(' ', 0, null, -2, 2),
                new CellInfo(' ', 0, null, -2, 3),
                new CellInfo(' ', 0, null, -2, 4),

                new CellInfo(' ', 0, null, -1, -2),
                new CellInfo(' ', 0, null, -1, -1),
                new CellInfo(' ', 0, null, -1, 0),
                new CellInfo(' ', 0, null, -1, 1),
                new CellInfo(' ', 0, null, -1, 2),
                new CellInfo(' ', 0, null, -1, 3),
                new CellInfo(' ', 0, null, -1, 4),

                new CellInfo(' ', 0, null, 0, -2),
                new CellInfo(' ', 0, null, 0, -1),
                new CellInfo('G', 0, null, 0, 0),
                new CellInfo('G', 0, null, 0, 1),
                new CellInfo('G', 0, null, 0, 2),
                new CellInfo('G', 0, null, 0, 3),
                new CellInfo('G', 0, null, 0, 4),

                new CellInfo(' ', 0, null, 1, -2),
                new CellInfo(' ', 0, null, 1, -1),
                new CellInfo('G', 0, null, 1, 0),
                new CellInfo('G', 0, null, 1, 1),
                new CellInfo('G', 0, null, 1, 2),
                new CellInfo('G', 0, null, 1, 3),
                new CellInfo('G', 0, null, 1, 4),

                new CellInfo(' ', 0, null, 2, -2),
                new CellInfo(' ', 0, null, 2, -1),
                new CellInfo('G', 0, null, 2, 0),
                new CellInfo('G', 0, null, 2, 1),
                new CellInfo('G', 0, null, 2, 2),
                new CellInfo('G', 0, null, 2, 3),
                new CellInfo('G', 0, null, 2, 4),

                new CellInfo(' ', 0, null, 3, -2),
                new CellInfo(' ', 0, null, 3, -1),
                new CellInfo('G', 0, null, 3, 0),
                new CellInfo('G', 0, null, 3, 1),
                new CellInfo('G', 0, null, 3, 2),
                new CellInfo('G', 0, null, 3, 3),
                new CellInfo('G', 0, null, 3, 4),

                new CellInfo(' ', 0, null, 4, -2),
                new CellInfo(' ', 0, null, 4, -1),
                new CellInfo('G', 0, null, 4, 0),
                new CellInfo('G', 0, null, 4, 1),
                new CellInfo('G', 0, null, 4, 2),
                new CellInfo('G', 0, null, 4, 3),
                new CellInfo('G', 0, null, 4, 4),
            };
            Assert.AreEqual(expected, map.threateningAreas.FirstOrDefault(x => x.Item1 == creature).Item2);
        }
    }
}
