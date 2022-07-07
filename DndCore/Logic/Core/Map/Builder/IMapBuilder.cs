using System;
using System.Collections.Generic;
using System.Text;

namespace DndCore.Map
{
    public interface IMapBuilder
    {
        IMap FromCsv(string content);
    }
}
