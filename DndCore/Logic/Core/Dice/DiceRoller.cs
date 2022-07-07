using System;
using DndCore.DI;

namespace Logic.Core.Dice
{
    public class DiceRoller : IDiceRoller
    {
        private readonly Random random;
        public DiceRoller(Random random = null)
        {
            this.random = random ?? DndModule.Get<Random>();
        }

        public int Roll(RollTypes type, int diceAmount, int dieFaces, int modifier)
        {
            switch (type)
            {
                case RollTypes.Advantage:
                    return Math.Max(SingleRoll(diceAmount, dieFaces, modifier), SingleRoll(diceAmount, dieFaces, modifier));
                case RollTypes.Disadvantage:
                    return Math.Min(SingleRoll(diceAmount, dieFaces, modifier), SingleRoll(diceAmount, dieFaces, modifier));
                default:
                    return SingleRoll(diceAmount, dieFaces, modifier);
            }
        }

        int SingleRoll(int diceAmount, int dieFaces, int modifier)
        {
            var total = 0;
            for (int i = 0; i < diceAmount; i++)
            {
                total += random.Next(dieFaces) + 1;
            }
            total += modifier;
            return total;
        }
    }
}
