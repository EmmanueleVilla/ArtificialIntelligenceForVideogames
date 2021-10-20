using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Map
{
    public interface IMapBuilder
    {
        IMapBuilder WithTerrains(string content);

        IMapBuilder WithHeights(string content);

        IMap Build();
    }
}
