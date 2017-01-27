using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Entities;
using EPS.DataLayer;

namespace EPS.BusinessLayer
{
    public partial class Lookup : IDisposable
    {
        LookupManager manager;

        public Lookup()
        {
            manager = new LookupManager();
        }

        public IEnumerable<Entities.Lookup> GetLookupByCategory(int? clientProjectId, string category)
        {
            try
            {
                clientProjectId = (category.ToLower() == "location" || category.ToLower() == "l3leveldisagree") ? null : clientProjectId;
                return manager.GetLookupByCategory(clientProjectId, category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.Lookup GetLookupById(int? clientProjectId, Int32 id)
        {
            try
            {
                return manager.GetLookupById(clientProjectId, id);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Boolean ValidateLookupByCategory(int? clientProjectId, string category, string lookupName, int lookupId)
        {
            try
            {
                clientProjectId = (category.ToLower() == "location") ? null : clientProjectId;
                var lookupobj = manager.GetLookupByCategory(clientProjectId, category).Where(a => a.Name.ToLower() == lookupName.ToLower() && a.Id != lookupId).ToList();

                if (lookupobj.Count > 0)
                    return false;
                else
                    return true;

                //charts.Where(a => a.ChartMoreInfo.LevelStatusId == (int)Helper.ChartStatus.Pending || a.ChartMoreInfo.LevelStatusId == (int)Helper.ChartStatus.WorkInProgress).ToList();
                //manager.GetLookupByCategory(client, category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.Lookup> GetLookupByClientProjectIdAndCategory(int clientProjectId, string category)
        {
            try
            {
                return manager.GetLookupByClientProjectIdAndCategory(clientProjectId, category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<string> GetLookupCategories(int? clientProjectId)
        {
            try
            {
                return manager.GetLookups(clientProjectId).Select(lk => lk.Category).Distinct();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertUpdateLookup(Entities.Lookup lookup)
        {
            try
            {
                var clientProjectId = (lookup.Category.ToLower() == "location" || lookup.Category.ToLower() == "l3leveldisagree") ? null : lookup.ClientProjectId;

                if (!lookup.IsActive || ValidateLookupByCategory(clientProjectId, lookup.Category, lookup.Name, lookup.Id))
                {
                    lookup.ClientProjectId = clientProjectId;
                    manager.InsertUpdateLookup(lookup);

                    return "[\" 1 \"]";
                }
                else
                    return "[\" 2 \"]";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.Lookup> GetCommentsCategories(int clientProjectId)
        {
            try
            {
                return manager.GetCommentsCategories(clientProjectId).Where(l => l.IsActive == true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            manager = null;
        }
    }
}
