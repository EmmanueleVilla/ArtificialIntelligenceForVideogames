using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Actions
{
    public struct Damage
    {
        public readonly DamageTypes Type;
        public readonly int AverageDamage;
        public readonly int NumberOfDice;
        public readonly int DiceFaces;
        public readonly int Modifier;

        public Damage(
            DamageTypes type,
            int averageDamage,
            int numberOfDice,
            int diceFaces,
            int modifier
            )
        {
            Type = type;
            AverageDamage = averageDamage;
            NumberOfDice = numberOfDice;
            DiceFaces = diceFaces;
            Modifier = modifier;
        }
    }
}
