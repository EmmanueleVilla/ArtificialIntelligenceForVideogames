using Core.Map;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Core.Map.Impl
{
    public class ArrayDndMap : IMap
    {
        public readonly int Width;
        public readonly int Height;
        private CellInfo[,] cells;

        int IMap.Width => Width;
        int IMap.Height => Height;
        public ArrayDndMap(int width, int height, CellInfo defaultInfo)
        {
            Width = width;
            Height = height;
            cells = new CellInfo[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    SetCell(i, j, CellInfo.Copy(defaultInfo));
                }
            }
        }

        public CellInfo GetCellInfo(int x, int y)
        {
            if(x < 0 || x >= Width || y < 0 || y >= Height) {
                return CellInfo.Empty();
            }
            return cells[x, y];
        }

        public void SetCell(int x, int y, CellInfo info)
        {
            cells[x, y] = info;
        }

        private List<CellInfo> occupiedCells = new List<CellInfo>();

        public bool AddCreature(ICreature creature, int x, int y)
        {
            var cell = GetCellInfo(x, y);
            if (cell.Terrain == ' ' || GetOccupantCreature(x, y) != null) 
            {
                return false;
            }

            int sizeInCells = 1;
            switch (creature.Size)
            {
                case Sizes.Large:
                    sizeInCells = 2;
                    break;
                case Sizes.Huge:
                    sizeInCells = 3;
                    break;
                case Sizes.Gargantuan:
                    sizeInCells = 4;
                    break;
            }

            var tempOccupiedCells = new List<CellInfo>();
            var fit = true;
            for (int i = x; i < sizeInCells + x; i++)
            {
                for (int j = y; j < sizeInCells + y; j++)
                {
                    var occupiedCell = GetCellInfo(i, j);
                    if(Math.Abs(cell.Height - occupiedCell.Height) > 1 || GetOccupantCreature(i, j) != null)
                    {
                        fit = false;
                    }
                    occupiedCell.Creature = creature;
                    tempOccupiedCells.Add(occupiedCell);
                }
            }

            if(!fit)
            {
                return false;
            }


            occupiedCells.AddRange(tempOccupiedCells);

            cell.Creature = creature;

            return true;
        }

        public ICreature GetOccupantCreature(int x, int y)
        {
            return occupiedCells.FirstOrDefault(c => c.X == x && c.Y == y).Creature;
        }
    }
}
