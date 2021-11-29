using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Actions
{
    public struct Attack
    {
        public readonly int Range;
        public readonly string Name;
        public readonly List<Damage> Damage;
        public readonly int ToHit;
        public readonly bool IsMagical;

        public Attack(
            string name,
            int range,
            List<Damage> damage,
            bool isMagical = false,
            int toHit = 0)
        {
            Name = name;
            Range = range;
            Damage = damage;
            ToHit = toHit;
            IsMagical = isMagical;
        }
    }
}
