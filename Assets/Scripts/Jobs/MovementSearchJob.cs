using Core.DI;
using Logic.Core.Battle;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Assets.Scripts.Jobs
{
    public struct UnmanagedEdge
    {
        public int X;
        public int Y;
        public int Speed;
        public int Damage;
        public bool CanEndMovementHere;

        public override string ToString()
        {
            return string.Format("[{0},{1}], speed {2}, damage {3}, canEndHere {4}",
                X, Y, Speed, Damage, CanEndMovementHere);
        }
    }

    struct MovementSearchJob : IJob
    {
        public static int MAX_EDGES = 1000;
        public NativeArray<UnmanagedEdge> result;

        public void Execute()
        {
            var battle = DndModule.Get<IDndBattle>();
            var res = battle.GetReachableCells();
            for (int i = 0; i < res.Count && i < MAX_EDGES; i++)
            {
                result[i] = new UnmanagedEdge() {
                    X = res[i].Destination.X,
                    Y = res[i].Destination.Y,
                    Damage = res[i].Damage,
                    CanEndMovementHere = res[i].CanEndMovementHere,
                    Speed = res[i].Speed
                };
            }
        }
    }
}
