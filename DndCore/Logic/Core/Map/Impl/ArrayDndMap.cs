﻿using Core.Map;
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
                return new CellInfo(' ', 0, null, x, y);
            }
            return cells[x, y];
        }

        public void SetCell(int x, int y, CellInfo info)
        {
            cells[x, y] = info;
        }

        public List<CellInfo> occupiedCells = new List<CellInfo>();
        public List<Tuple<ICreature, List<CellInfo>>> threateningAreas = new List<Tuple<ICreature, List<CellInfo>>>();
        
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

            if (!fit)
            {
                return false;
            }

            occupiedCells.AddRange(tempOccupiedCells);

            var reach = 0;
            if (creature.Attacks.Any(a => a.Type == Actions.AttackTypes.WeaponMelee))
            {
                reach = 1;
            }

            if (creature.Attacks.Any(a => a.Type == Actions.AttackTypes.WeaponMeleeReach))
            {
                reach = 2;
            }


            if (reach > 0)
            {
                var cells = new List<CellInfo>();
                var startI = x - reach;
                var endI = x + sizeInCells + reach;
                var startJ = y - reach;
                var endJ = y + sizeInCells + reach;
                for (int i = startI; i < endI; i++)
                {
                    for (int j = startJ; j < endJ; j++)
                    {
                        Console.WriteLine(String.Format("{0},{1}", i, j));
                        cells.Add(GetCellInfo(i, j));
                    }
                }
                threateningAreas.Add(new Tuple<ICreature, List<CellInfo>>(creature, cells));
            }

            SetCell(cell.X, cell.Y, new CellInfo(cell.Terrain, cell.Height, creature, cell.X, cell.Y));

            return true;
        }

        public ICreature GetOccupantCreature(int x, int y)
        {
            return occupiedCells.FirstOrDefault(c => c.X == x && c.Y == y).Creature;
        }

        public List<CellInfo> GetCellsOccupiedBy(int x, int y)
        {
            var cell = GetCellInfo(x, y);
            if(cell.Creature == null)
            {
                return new List<CellInfo>();
            }
            var occupied = occupiedCells.Where(c => c.Creature == cell.Creature).ToList();
            if(occupied.Count == 0)
            {
                return new List<CellInfo>() { cell };
            }

            return occupied.Select(c => GetCellInfo(c.X, c.Y)).ToList();
        }

        public List<ICreature> IsLeavingThreateningArea(CellInfo start, CellInfo end)
        {
            throw new NotImplementedException();
        }
    }
}
