﻿using System;
using System.Collections.Generic;
using DndCore.Map;
using Logic.Core.Battle;

namespace Logic.Core.Graph
{
    class ReachedCell: IComparable
    {
        public readonly CellInfo Cell;

        public List<CellInfo> Path = new List<CellInfo>();
        public List<GameEvent> Events = new List<GameEvent>();
        public int UsedMovement;
        public int DamageTaken;
        public bool CanEndMovementHere;
        public ReachedCell(CellInfo cell)
        {
            Cell = cell;
        }

        public int CompareTo(object obj)
        {
            var other = (ReachedCell)obj;
            if (DamageTaken == other.DamageTaken && UsedMovement != other.UsedMovement)
            {
                return UsedMovement.CompareTo(other.UsedMovement);
            }
            var result = DamageTaken.CompareTo(other.DamageTaken);
            if (result == 0)
            {
                // HACK
                // This breaks get(key), but we won't use it
                // and it lets us have multiple items with the same key
                return -1;
            }
            else
            {
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format("Used Movement until now:" + UsedMovement);
        }
    }

    class ReachedCellComparer : IComparer<ReachedCell>
    {
        public int Compare(ReachedCell x, ReachedCell y)
        {
            if (x.DamageTaken == y.DamageTaken && x.UsedMovement != y.UsedMovement)
            {
                return x.UsedMovement.CompareTo(y.UsedMovement);
            }
            var result = x.DamageTaken.CompareTo(y.DamageTaken);
            if (result == 0)
            {
                // HACK
                // This breaks get(key), but we won't use it
                // and it lets us have multiple items with the same key
                return -1;
            }
            else
            {
                return result;
            }
        }
    }
}
