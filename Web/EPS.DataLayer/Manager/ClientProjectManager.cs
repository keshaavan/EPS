using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Interfaces;
using EPS.Entities;

namespace EPS.DataLayer
{
    public partial class ClientProjectManager : IClientProject, IDisposable
    {
        ClientProject clientProjectObject;

        public ClientProjectManager()
        {
            clientProjectObject = new ClientProject();
        }

        public IEnumerable<Entities.ClientProject> GetAllClientProjects()
        {
            try
            {
                return clientProjectObject.GetClientProjects();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /*----*/
        public IEnumerable<Entities.GetProjects> GetAllProjects(string sClientId)
        {
            try
            {
                return clientProjectObject.GetAllProjects(sClientId);
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
                return clientProjectObject.GetAllQueues(sProjectId);
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
        //        return clientProjectObject.GetAllProjects(sClientId);
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
        //        return clientProjectObject.GetAllQueues(sProjectId);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        
        /*----*/
        public IEnumerable<Entities.ClientProject> GetAllClientProjectsdrp()
        {
            try
            {
                return clientProjectObject.GetClientProjectsdrp();
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
                return clientProjectObject.GetClientProjectById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //June9
        public Entities.ClientProject GetClientProjectByIdList()
        {
            try
            {
                return clientProjectObject.GetClientProjectByIdList();
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
                return clientProjectObject.GetProductionStatistics(clientProjectId, fromDate, toDate);
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
                return clientProjectObject.GetCompletedChartCount(username, clientProjectId, levelnumber, sfromDate, stoDate);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void Dispose()
        {
            clientProjectObject = null;
        }

        public void UpdateClientProject(Entities.ClientProject clientProject)
        {
            try
            {
                clientProjectObject.UpdateClientProject(clientProject);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
