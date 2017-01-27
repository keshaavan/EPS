using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Interfaces
{
    public interface ILookup
    {
        IEnumerable<Entities.Lookup> GetLookups(int? clientProjectId);

        IEnumerable<Entities.Lookup> GetLookupByClientProjectIdAndCategory(int clientProjectId, string category);
        IEnumerable<Entities.Lookup> GetLookupByCategory(int? clientProjectId, string category);
        Entities.Lookup GetLookupById(int? clientProjectId, int id);
        IEnumerable<Entities.Lookup> GetCommentsCategories(int clientProjectId);

        void InsertUpdateLookup(Entities.Lookup lookup);
    }
}
