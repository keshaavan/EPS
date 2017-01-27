using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Entities;
using EPS.DataLayer;

namespace EPS.BusinessLayer
{
    public partial class ClientProject : IDisposable
    {
        ClientProjectManager manager;

        public ClientProject()
        {
            manager = new ClientProjectManager();
        }

        public IEnumerable<Entities.ClientProject> GetAllClientProjects(Boolean isActive)
        {
            try
            {
                var clientProjects = manager.GetAllClientProjects();

                if (isActive)
                {
                    return (IEnumerable<Entities.ClientProject>)(from cp in clientProjects where cp.isActive == true select cp);
                }

                return clientProjects;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /*---*/
        public IEnumerable<Entities.GetProjects> GetAllProjects(string sClientId)
        {
            try
            {
                var clientProjects = manager.GetAllProjects(sClientId);
                return (IEnumerable<Entities.GetProjects>)(from cp in clientProjects select cp);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.GetALLQueues> GetAllQueues(string sProjectId)
        {
            try
            {
                var clientProjects = manager.GetAllQueues(sProjectId);
                return (IEnumerable<Entities.GetALLQueues>)(from cp in clientProjects select cp);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public System.Data.DataSet GetAllProjects(string sClientId)
        //{
        //    try
        //    {
        //        //var clientProjects = manager.GetAllProjects(sClientId);
        //        //return (IEnumerable<Entities.GetProjects>)(from cp in clientProjects select cp);
        //        return manager.GetAllProjects(sClientId);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public System.Data.DataSet GetAllQueues(string sProjectId)
        //{
        //    try
        //    {
        //        //var clientProjects = manager.GetAllProjects(sClientId);
        //        //return (IEnumerable<Entities.GetProjects>)(from cp in clientProjects select cp);
        //        return manager.GetAllQueues(sProjectId);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


       
        /*---*/
       public IEnumerable<Entities.ClientProject> GetAllClientProjectsdrp()
        {
            try
            {
                var clientProjects = manager.GetAllClientProjectsdrp();

                return clientProjects;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.ClientProject GetClientProjectById(int id)
        {
            try
            {
                return manager.GetClientProjectById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateClientProject(Entities.ClientProject clientProject)
        {
            try
            {
                manager.UpdateClientProject(clientProject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public System.Data.DataSet GetProductionStatistics(string clientProjectId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return manager.GetProductionStatistics(clientProjectId, fromDate, toDate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCompletedChartCount(string username, int clientProjectId, int levelnumber, DateTime sfromDate, DateTime stoDate)
        {
            try
            {
                return manager.GetCompletedChartCount(username, clientProjectId, levelnumber, sfromDate, stoDate);
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
