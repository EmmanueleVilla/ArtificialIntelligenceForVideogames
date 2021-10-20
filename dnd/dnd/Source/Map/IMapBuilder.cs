using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Map
{
    public interface IMapBuilder
    {
        IMap FromCsv(string content);
    }
}
