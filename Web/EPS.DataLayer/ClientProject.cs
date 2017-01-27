using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using EPS.Entities;
using EPS.Resources;
using EPS.Utilities;

namespace EPS.DataLayer
{
    partial class ClientProject : IDisposable
    {
        SqlDatabase db;

        public ClientProject()
        {
            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public IEnumerable<Entities.ClientProject> GetClientProjects()
        {
            var clientProjects = new List<Entities.ClientProject>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetClientProjects))
            {
                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var clientProject = new Entities.ClientProject();
                        AssignClientProject(reader, clientProject);
                        clientProjects.Add(clientProject);
                    }
                }
            }

            return clientProjects;
        }

        /*---*/
        public IEnumerable<Entities.GetProjects> GetAllProjects(string sClientId)
        {
            var clientProjects = new List<Entities.GetProjects>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetProjectList))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientKey, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sClientId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var clientProject = new Entities.GetProjects();
                        AssignProject(reader, clientProject);
                        clientProjects.Add(clientProject);
                    }
                }
            }
            return clientProjects;
        }

        public IEnumerable<Entities.GetALLQueues> GetAllQueues(string sProjectKey)
        {
            var clientProjects = new List<Entities.GetALLQueues>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetQueueList))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ProjectKey, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sProjectKey));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var clientProject = new Entities.GetALLQueues();
                        AssignQueues(reader, clientProject);
                        clientProjects.Add(clientProject);
                    }
                }
            }
            return clientProjects;
        }


        //public System.Data.DataSet GetAllProjects(string sClientId)
        //{
        //    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetProjectList))
        //    {
        //        SqlParameter sqlParam;
        //        sqlParam = new SqlParameter(DBResources.param_ClientKey, SqlDbType.VarChar, 50);
        //        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sClientId));

        //        sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

        //        return db.ExecuteDataSet(sqlCommand);
        //    }
        //}

        //public System.Data.DataSet GetAllQueues(string sProjectKey)
        //{
        //    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetQueueList))
        //    {
        //        SqlParameter sqlParam;
        //        sqlParam = new SqlParameter(DBResources.param_ProjectKey, SqlDbType.VarChar, 50);
        //        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sProjectKey));

        //        sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

        //        return db.ExecuteDataSet(sqlCommand);
        //    }
        //}

       

        /*---*/

        //public IEnumerable<Entities.ClientProject> GetAllClientNames()
        //{
        //    var clientProjects = new List<Entities.ClientProject>();

        //    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetClientProjects))
        //    {
        //        using (IDataReader reader = db.ExecuteReader(sqlCommand))
        //        {
        //            while (reader.Read())
        //            {
        //                var clientProject = new Entities.ClientProject();
        //                AssignClientProject(reader, clientProject);
        //                clientProjects.Add(clientProject);
        //            }
        //        }
        //    }

        //    return clientProjects;
        //}


        public IEnumerable<Entities.ClientProject> GetClientProjectsdrp()
        {
            var clientProjects = new List<Entities.ClientProject>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetClientProjectsdrp))
            {
                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var clientProject = new Entities.ClientProject();
                        AssignClientProject(reader, clientProject);
                        clientProjects.Add(clientProject);
                    }
                }
            }

            return clientProjects;
        }


        public Entities.ClientProject GetClientProjectById(int id)
        {
            var clientProject = new Entities.ClientProject();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetClientProjectById))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignClientProject(reader, clientProject);
                    }
                }
            }

            return clientProject;
        }
        //June9
        public Entities.ClientProject GetClientProjectByIdList()
        {
            var clientProject = new Entities.ClientProject();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetClientProjectById_List))
            {                

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignClientProject(reader, clientProject);
                    }
                }
            }

            return clientProject;
        }

        public System.Data.DataSet GetProductionStatistics(string clientProjectId, DateTime fromDate, DateTime toDate)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetProductionStats))            
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId,  SqlDbType.VarChar ,50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                return db.ExecuteDataSet(sqlCommand);
            }
        }

     


        //June15
        public Entities.ClientProject GetQueuesList(string projectkey)
        {
           
            var clientProject = new Entities.ClientProject();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetQueueList))
            {
                    SqlParameter sqlParam;
                    sqlParam = new SqlParameter(DBResources.param_ProjectKey, SqlDbType.VarChar, 50);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, projectkey));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignClientProject(reader, clientProject);
                    }
                }
            }
            return clientProject;
        }

        //June15
        public Entities.ClientProject GetProjects(string clientkey)
        {
            

            var clientProject = new Entities.ClientProject();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetProjectList))
            {
                    SqlParameter sqlParam;
                    sqlParam = new SqlParameter(DBResources.param_ClientKey, SqlDbType.VarChar, 50);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientkey));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignClientProject(reader, clientProject);
                    }
                }
            }
            return clientProject;
        }

      



        public int GetCompletedChartCount(string username, int clientProjectId, int levelnumber, DateTime sfromDate, DateTime stoDate)
        {
            int retValue = 0;

            try
            {
                using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartsCompletedCount))
                {
                    SqlParameter sqlParam;
                    sqlParam = new SqlParameter(DBResources.param_EmployeeId, SqlDbType.NVarChar);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, username));

                    sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.BigInt);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                    sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, levelnumber));

                    sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sfromDate ));

                    sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, stoDate));


                    retValue = Convert.ToInt32(db.ExecuteScalar(sqlCommand));
                }

                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public void UpdateClientProject(Entities.ClientProject clientProject)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateClientProject))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProject.Id));

                sqlParam = new SqlParameter(DBResources.param_IsL1Auto, SqlDbType.Bit);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProject.IsL1Auto));

                sqlParam = new SqlParameter(DBResources.param_IsL2Auto, SqlDbType.Bit);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProject.IsL2Auto));

                sqlParam = new SqlParameter(DBResources.param_IsL3Auto, SqlDbType.Bit);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProject.IsL3Auto));

                sqlParam = new SqlParameter(DBResources.param_HomepageText, SqlDbType.NText);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProject.HomepageText));

                sqlParam = new SqlParameter(DBResources.param_UpdatedBy, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProject.UpdatedBy));

                sqlParam = new SqlParameter(DBResources.param_Factorial, SqlDbType.Decimal);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProject.Factorialization));

                db.ExecuteNonQuery(sqlCommand);
            }
        }

        private Entities.ClientProject AssignClientProject(IDataReader reader, Entities.ClientProject clientProject)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                clientProject.Id = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_Client);
            if (!reader.IsDBNull(i))
                clientProject.Client = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_Project);
            if (!reader.IsDBNull(i))
                clientProject.Project = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_ClientReferenceLabel);
            if (!reader.IsDBNull(i))
                clientProject.ClientReferenceLabel = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_ClientKey);
            if (!reader.IsDBNull(i))
                clientProject.ClientKey = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_ProjectKey);
            if (!reader.IsDBNull(i))
                clientProject.ProjectKey = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_Queue);
            if (!reader.IsDBNull(i))
                clientProject.Queue = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_QueueKey);
            if (!reader.IsDBNull(i))
                clientProject.QueueKey = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_IsByDxCode);
            if (!reader.IsDBNull(i))
                clientProject.IsByDxCode = reader.GetBoolean(i);

            i = reader.GetOrdinal(DBResources.col_IsL1Auto);
            if (!reader.IsDBNull(i))
                clientProject.IsL1Auto = reader.GetBoolean(i);

            i = reader.GetOrdinal(DBResources.col_IsL2Auto);
            if (!reader.IsDBNull(i))
                clientProject.IsL2Auto = reader.GetBoolean(i);

            i = reader.GetOrdinal(DBResources.col_IsL3Auto);
            if (!reader.IsDBNull(i))
                clientProject.IsL3Auto = reader.GetBoolean(i);

            i = reader.GetOrdinal(DBResources.col_IsActive);
            if (!reader.IsDBNull(i))
                clientProject.isActive = reader.GetBoolean(i);

            i = reader.GetOrdinal(DBResources.col_HomepageText);
            if (!reader.IsDBNull(i))
                clientProject.HomepageText = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_UpdatedBy);
            if (!reader.IsDBNull(i))
                clientProject.UpdatedBy = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_UpdatedDate);
            if (!reader.IsDBNull(i))
                clientProject.UpdatedDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_Factorialization);
            if (!reader.IsDBNull(i))
                clientProject.Factorialization = reader.GetDecimal(i);

            return clientProject;
        }

        private Entities.GetProjects AssignProject(IDataReader reader, Entities.GetProjects clientProject)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_ProjectKey);
            if (!reader.IsDBNull(i))
                clientProject.ProjectKey = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_Project);
            if (!reader.IsDBNull(i))
                clientProject.Project = reader.GetString(i);
            return clientProject;
        }

        private Entities.GetALLQueues AssignQueues(IDataReader reader, Entities.GetALLQueues clientProject)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                clientProject.Id = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_Queue);
            if (!reader.IsDBNull(i))
                clientProject.Queue = reader.GetString(i);

            return clientProject;
        }

        public void Dispose()
        {
            db = null;
        }
    }
}
