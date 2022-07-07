using DndCore.Map;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Bestiary;
using Logic.Core.Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Core.Map.Impl
{
    public class ArrayDndMap : IMap
    {
        public int Width;
        public int Height;
        public CellInfo[,] cells;
        public Dictionary<int,int> occupiedCellsDictionary = new Dictionary<int, int>();
        public List<Tuple<int, HashSet<int>>> threateningAreas = new List<Tuple<int, HashSet<int>>>();

        int IMap.Width => Width;
        int IMap.Height => Height;

        public IMap Copy()
        {
            var newMap = new ArrayDndMap();
            newMap.Width = Width;
            newMap.Height = Height;
            newMap.cells = new CellInfo[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var old = GetCellInfo(i, j);
                    newMap.SetCell(i, j, new CellInfo(old.Terrain, old.Height, old.Creature?.Copy(), i, j));
                }
            }
            newMap.occupiedCellsDictionary = new Dictionary<int, int>(occupiedCellsDictionary);
            newMap.threateningAreas = new List<Tuple<int, HashSet<int>>>();
            foreach(var area in threateningAreas)
            {
                newMap.threateningAreas.Add(new Tuple<int, HashSet<int>>(area.Item1, new HashSet<int>(area.Item2)));
            }
            return newMap;
        }

        private Dictionary<int, ICreature> creatures;

        public Dictionary<int, ICreature> Creatures
        { 
            get {
                if (creatures == null)
                {
                    creatures = new Dictionary<int, ICreature>();
                    for (int i = 0; i < Width; i++)
                    {
                        for (int j = 0; j < Height; j++)
                        {
                            var cell = GetCellInfo(i, j);
                            if (cell.Creature != null)
                            {
                                creatures.Add(cell.Creature.Id, cell.Creature);
                            }
                        }
                    }
                }
                return creatures;
            }
        }

        public ArrayDndMap()
        {

        }

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

        private CellInfo GetCellInfo(int key)
        {
            for (int i = -1; i < Width + 1; i++)
            {
                for (int j = -1; j < Height + 1; j++)
                {
                    var k = (i << 6) + j;
                    if(key == k)
                    {
                        return GetCellInfo(i, j);
                    }
                }
            }
            return CellInfo.Empty();
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
            try
            {
                var cell = GetCellInfo(x, y);
                var occ = GetOccupantCreature(x, y);
                if (cell.Terrain == ' ' || (occ != null && occ != creature))
                {
                    return false;
                }

                var tempOccupiedCells = new List<int>();
                var fit = true;
                for (int i = x; i < creature.Size + x; i++)
                {
                    for (int j = y; j < creature.Size + y; j++)
                    {
                        var occupiedCell = GetCellInfo(i, j);
                        var occupantCreature = GetOccupantCreature(i, j);
                        if (Math.Abs(cell.Height - occupiedCell.Height) > 1 || (occupantCreature != null && occupantCreature != creature))
                        {
                            fit = false;
                        }
                        occupiedCell.Creature = creature;
                        tempOccupiedCells.Add((i << 6) + j);
                    }
                }

                if (!fit)
                {
                    return false;
                }

                tempOccupiedCells.ForEach(temp =>
                {
                    occupiedCellsDictionary.Add(temp, creature.Id);
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
                    var cells = new HashSet<int>();
                    var startI = x - reach;
                    var endI = x + creature.Size + reach;
                    var startJ = y - reach;
                    var endJ = y + creature.Size + reach;
                    for (int i = startI; i < endI; i++)
                    {
                        for (int j = startJ; j < endJ; j++)
                        {
                            cells.Add((i << 6) + j);
                        }
                    }
                    threateningAreas.Add(new Tuple<int, HashSet<int>>(creature.Id, cells));
                }

                SetCell(cell.X, cell.Y, new CellInfo(cell.Terrain, cell.Height, creature, cell.X, cell.Y));

                creatures = null;

                return true;
            } catch(Exception e)
            {
                return false;
            }
        }

        public ICreature GetOccupantCreature(int x, int y)
        {
            int creatureId;
            occupiedCellsDictionary.TryGetValue((x << 6) + y, out creatureId);
            ICreature creature;
            Creatures.TryGetValue(creatureId, out creature);
            return creature;
        }

        public List<ICreature> IsLeavingThreateningArea(ICreature mover, CellInfo start, CellInfo end)
        {
            var creatures = new List<ICreature>();
            foreach (var area in threateningAreas)
            {
                var creature = Creatures[area.Item1];
                if (creature.Loyalty == mover.Loyalty)
                {
                    continue;
                }

                var startValue = (start.X << 6) + start.Y;
                var areaContainsStart = area.Item2.Contains(startValue);
                if(!areaContainsStart)
                {
                    continue;
                }
                var endValue = (end.X << 6) + end.Y;
                var areaContainsEnd = !area.Item2.Contains(endValue);
                if (areaContainsEnd)
                {
                    creatures.Add(creature);
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
            var occupied = occupiedCellsDictionary.Where(c => c.Value == cell.Creature.Id).ToList();
            if (occupied.Count == 0)
            {
                return new List<CellInfo>() { cell };
            }

            return occupied.Select(c => GetCellInfo(c.Key)).ToList();
        }

        public List<CellInfo> GetCellsOccupiedBy(Loyalties loyalty)
        {
            var result = new List<CellInfo>();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var cell = GetCellInfo(i, j);
                    if (cell.Creature != null && cell.Creature.Loyalty == loyalty)
                    {
                        result.Add(cell);
                    }
                }
            }
            return result;
        }

        public CellInfo GetCellOccupiedBy(ICreature creature)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var cell = GetCellInfo(i, j);
                    if (cell.Creature != null && cell.Creature.Id == creature.Id)
                    {
                        return cell;
                    }
                }
            }
            return CellInfo.Empty();
        }

        public void MoveCreatureTo(ICreature creature, MemoryEdge edge)
        {
            RemoveCreature(creature);

            if(!AddCreature(creature, edge.Destination.X, edge.Destination.Y))
            {
                throw new Exception("Should never happen");
            }
        }

        public void RemoveCreature(ICreature creature)
        {
            creatures = null;
            var startCoord = GetCellOccupiedBy(creature);
            var startCell = GetCellInfo(startCoord.X, startCoord.Y);
            var newCell = CellInfo.Copy(startCell);
            startCell.Creature = null;
            SetCell(startCell.X, startCell.Y, startCell);
            threateningAreas.RemoveAll(x => x.Item1 == creature.Id);
            var keys = occupiedCellsDictionary.Where(x => x.Value == creature.Id).Select(x => x.Key).ToList();
            foreach (var key in keys)
            {
                occupiedCellsDictionary.Remove(key);
            }
        }
    }
}
