using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Interfaces;
using EPS.Entities;

namespace EPS.DataLayer
{
    public partial class LookupManager : ILookup, IDisposable
    {
        Lookup lookupObject;

        public LookupManager()
        {
            lookupObject = new Lookup();
        }

        public IEnumerable<Entities.Lookup> GetLookupByCategory(int? clientProjectId, string category)
        {
            try
            {
                var lookups = lookupObject.GetLookupByCategory(clientProjectId, category);

                if (clientProjectId.HasValue)
                {
                    var clientProject = new ClientProjectManager().GetClientProjectById(clientProjectId.Value);

                    foreach (var lookup in lookups)
                    {
                        lookup.ClientProject = clientProject;
                    }
                }

                return lookups;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.Lookup GetLookupById(int? clientProjectId, int id)
        {
            try
            {
                var lookup = lookupObject.GetLookupById(clientProjectId, id);

                if (clientProjectId.HasValue)
                {
                    var clientProject = new ClientProjectManager().GetClientProjectById(clientProjectId.Value);
                    lookup.ClientProject = clientProject;
                }

                return lookup;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.Lookup GetLookupByIdList()
        {
            try
            {
                var lookup = lookupObject.GetLookupByIdList();

                
                    var clientProject = new ClientProjectManager().GetClientProjectByIdList();
                    lookup.ClientProject = clientProject;
                

                return lookup;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void InsertUpdateLookup(Entities.Lookup lookup)
        {
            try
            {
                lookupObject.InsertUpdateLookup(lookup);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.Lookup> GetLookups(int? clientProjectId)
        {
            try
            {
                var lookups = lookupObject.GetLookups(clientProjectId);
                if (clientProjectId.HasValue)
                {
                    var clientProject = new ClientProjectManager().GetClientProjectById(clientProjectId.Value);

                    foreach (var lookup in lookups)
                    {
                        lookup.ClientProject = clientProject;
                    }
                }

                return lookups;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region ILookup Members


        public IEnumerable<Entities.Lookup> GetLookupByClientProjectIdAndCategory(int clientProjectId, string category)
        {
            try
            {
                var lookups = lookupObject.GetLookupByClientProjectIdAndCategory(clientProjectId, category);
                var clientProject = new ClientProjectManager().GetClientProjectById(clientProjectId);

                foreach (var lookup in lookups)
                {
                    lookup.ClientProject = clientProject;
                }

                return lookups;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public void Dispose()
        {
            lookupObject = null;
        }

        public IEnumerable<Entities.Lookup> GetCommentsCategories(int clientProjectId)
        {
            try
            {
                var lookups = lookupObject.GetCommentsCategories(clientProjectId);
                var clientProject = new ClientProjectManager().GetClientProjectById(clientProjectId);

                foreach (var lookup in lookups)
                {
                    lookup.ClientProject = clientProject;
                }

                return lookups;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
