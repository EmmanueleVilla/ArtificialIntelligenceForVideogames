using System;
namespace Logic.Core.Dice
{
    public interface IDiceRoller
    {
        int Roll(RollTypes type, int diceAmount, int dieFaces, int modifier);
    }
}
