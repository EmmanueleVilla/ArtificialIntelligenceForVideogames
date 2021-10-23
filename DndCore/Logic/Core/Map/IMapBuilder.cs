using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Map
{
    public interface IMapBuilder
    {
        IMap FromCsv(string content);
    }
}
