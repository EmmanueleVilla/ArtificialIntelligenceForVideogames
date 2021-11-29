using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Actions
{
    public struct Attack
    {
        public readonly AttackTypes Type;
        public readonly string Name;
        public readonly List<Damage> Damage;

        public Attack(
            string name,
            AttackTypes type,
            List<Damage> damage)
        {
            Name = name;
            Type = type;
            Damage = damage;
        }

        public int Range
        {
            get
            {
                switch (Type)
                {
                    case AttackTypes.WeaponMelee:
                    case AttackTypes.MagicMelee:
                        return 1;
                    case AttackTypes.WeaponMeleeReach:
                        return 2;
                }
                return 1;
            }
        }
    }
}
