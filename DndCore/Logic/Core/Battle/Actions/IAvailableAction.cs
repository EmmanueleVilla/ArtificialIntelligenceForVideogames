﻿
namespace Logic.Core.Battle.Actions
{
    public interface IAvailableAction
    {
        ActionsTypes ActionType { get; }

        string Description { get; }
    }
}