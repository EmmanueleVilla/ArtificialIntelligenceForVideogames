﻿using DndCore.Map;
using Logic.Core.Creatures;
using Logic.Core.Graph;
using System.Collections.Generic;

namespace Logic.Core.Map
{
    public class DictionaryDndMap : IMap
    {
        public readonly int Width;
        public readonly int Height;
        private Dictionary<int, CellInfo> cells = new Dictionary<int, CellInfo>();
        int IMap.Width => Width;
        int IMap.Height => Height;
        public Dictionary<int, ICreature> Creatures => throw new System.NotImplementedException();

        public DictionaryDndMap(int width, int height, CellInfo defaultInfo)
        {
            Width = width;
            Height = height;
            for(int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    SetCell(i, j, CellInfo.Copy(defaultInfo));
                }
            }
        }

        public CellInfo GetCellInfo(int x, int y)
        {
            CellInfo info;
            cells.TryGetValue(x * Width + y, out info);
            return info;
        }

        public void SetCell(int x, int y, CellInfo info)
        {
            var key = x * Width + y;
            cells.Remove(key);
            cells.Add(key, info);
        }

        public bool AddCreature(ICreature creature, int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public ICreature GetOccupantCreature(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public List<ICreature> IsLeavingThreateningArea(ICreature mover, CellInfo start, CellInfo end)
        {
            throw new System.NotImplementedException();
        }

        public List<CellInfo> GetCellsOccupiedBy(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public CellInfo GetCellOccupiedBy(ICreature creature)
        {
            throw new System.NotImplementedException();
        }

        public void MoveTo(MemoryEdge edge)
        {
            throw new System.NotImplementedException();
        }

        public void MoveCreatureTo(ICreature creature, MemoryEdge end)
        {
            throw new System.NotImplementedException();
        }

        public IMap Copy()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveCreature(ICreature creature)
        {
            throw new System.NotImplementedException();
        }

        public List<CellInfo> GetCellsOccupiedBy(Loyalties loyalty)
        {
            throw new System.NotImplementedException();
        }
    }
}
