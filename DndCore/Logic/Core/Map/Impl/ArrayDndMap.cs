﻿using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Graph;
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
        public List<CellInfo> occupiedCells = new List<CellInfo>();
        public Dictionary<int,ICreature> occupiedCellsDictionary = new Dictionary<int, ICreature>();
        public List<Tuple<ICreature, List<CellInfo>>> threateningAreas = new List<Tuple<ICreature, List<CellInfo>>>();

        int IMap.Width => Width;
        int IMap.Height => Height;

        public List<ICreature> Creatures => occupiedCells.Select(x => x.Creature).Distinct().ToList();

        public ArrayDndMap(int width, int height, CellInfo defaultInfo)
        {
            if(width >= 64 || height >= 64)
            {
                throw new Exception("This map can only handle 63x63 size");
            }
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
        
        public bool AddCreature(ICreature creature, int x, int y)
        {
            var cell = GetCellInfo(x, y);
            if (cell.Terrain == ' ' || GetOccupantCreature(x, y) != null) 
            {
                return false;
            }

            var tempOccupiedCells = new List<CellInfo>();
            var fit = true;
            for (int i = x; i < creature.Size + x; i++)
            {
                for (int j = y; j < creature.Size + y; j++)
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
            tempOccupiedCells.ForEach(temp =>
            {
                occupiedCellsDictionary.Add((temp.X << 6) + temp.Y, creature);
            });

            var reach = 0;

            if (creature.Attacks != null && creature.Attacks.Any(a => a.Range == 1))
            {
                reach = 1;
            }

            if (creature.Attacks != null && creature.Attacks.Any(a => a.Range == 2))
            {
                reach = 2;
            }

            if (reach > 0)
            {
                var cells = new List<CellInfo>();
                var startI = x - reach;
                var endI = x + creature.Size + reach;
                var startJ = y - reach;
                var endJ = y + creature.Size + reach;
                for (int i = startI; i < endI; i++)
                {
                    for (int j = startJ; j < endJ; j++)
                    {
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
            ICreature creature;
            occupiedCellsDictionary.TryGetValue((x << 6) + y, out creature);
            return creature;
        }

        public List<ICreature> IsLeavingThreateningArea(ICreature mover, CellInfo start, CellInfo end)
        {
            var creatures = new List<ICreature>();
            foreach (var area in threateningAreas)
            {
                if (area.Item1.Loyalty == mover.Loyalty)
                {
                    continue;
                }
                var areaContainsStart = area.Item2.Any(x => x.X == start.X && x.Y == start.Y);
                if(!areaContainsStart)
                {
                    continue;
                }
                var areaContainsEnd = !area.Item2.Any(x => x.X == end.X && x.Y == end.Y);
                if (areaContainsEnd)
                {
                    creatures.Add(area.Item1);
                }
            }
            return creatures;
        }

        public List<CellInfo> GetCellsOccupiedBy(int x, int y)
        {
            var cell = GetCellInfo(x, y);
            if (cell.Creature == null)
            {
                return new List<CellInfo>();
            }
            var occupied = occupiedCells.Where(c => c.Creature == cell.Creature).ToList();
            if (occupied.Count == 0)
            {
                return new List<CellInfo>() { cell };
            }

            return occupied.Select(c => GetCellInfo(c.X, c.Y)).ToList();
        }

        public CellInfo GetCellOccupiedBy(ICreature creature)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var cell = GetCellInfo(i, j);
                    if (cell.Creature == creature)
                    {
                        return cell;
                    }
                }
            }
            return CellInfo.Empty();
        }

        public void MoveCreatureTo(ICreature creature, MemoryEdge edge)
        {
            var startCoord = edge.Start.First();
            var startCell = GetCellInfo(startCoord.X, startCoord.Y);
            var newCell = CellInfo.Copy(startCell);
            //newCell.Creature = creature;
            startCell.Creature = null;
            SetCell(startCell.X, startCell.Y, startCell);
            occupiedCells.RemoveAll(x => x.Creature == creature);
            threateningAreas.RemoveAll(x => x.Item1 == creature);
            var keys = occupiedCellsDictionary.Where(x => x.Value == creature).Select(x => x.Key).ToList();
            foreach(var key in keys)
            {
                occupiedCellsDictionary.Remove(key);
            }

            if(!AddCreature(creature, edge.Destination.X, edge.Destination.Y))
            {
                throw new Exception("Should never happen");
            }
        }
    }
}
