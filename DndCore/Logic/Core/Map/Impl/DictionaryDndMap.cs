using Core.Map;
using Logic.Core.Creatures;
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

        public void AddCreature(ICreature creature, int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public ICreature GetOccupantCreature(int x, int y)
        {
            throw new System.NotImplementedException();
        }
    }
}
