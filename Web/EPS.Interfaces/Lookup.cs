using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Interfaces
{
    public interface ILookup
    {
        IEnumerable<Entities.Lookup> GetLookupByCategory(string client, string category);
        Entities.Lookup GetLookupById(string client, int id);
    }
}
