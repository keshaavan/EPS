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
    public class SiteMaster : IDisposable
    {


        SqlDatabase db;

        public SiteMaster()
        {
            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public DataSet getQueues(int iClientProjectID,string sUserId)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetQueueByUser))
            {
                SqlParameter sqlParam;               

                sqlParam = new SqlParameter(DBResources.param_iClientProjectID, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, iClientProjectID));

                sqlParam = new SqlParameter(DBResources.param_EmployeeId, SqlDbType.NVarChar);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sUserId));

                return db.ExecuteDataSet(sqlCommand);
            }
        }

        public DataSet getQueueCategory(int iClientProjectID)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetQueueCategoryByUser))
            {
                SqlParameter sqlParam;

                sqlParam = new SqlParameter(DBResources.param_iClientProjectID, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, iClientProjectID));

                return db.ExecuteDataSet(sqlCommand);
            }
        }

        public int insertorupdateQueue(Entities.Queue Queue)
        {
            int retval = -1;
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_InsertUpdateQueue))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, Queue.ClientProjectId));

                sqlParam = new SqlParameter(DBResources.param_QueueId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, Queue.QueueId));

                sqlParam = new SqlParameter(DBResources.param_Queue, SqlDbType.VarChar, 255);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, Queue.QueueName));

                sqlParam = new SqlParameter(DBResources.param_IsActive, SqlDbType.Bit);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ((Queue.isActive == true) ? 1 : 0)));

                sqlParam = new SqlParameter(DBResources.param_Flag, SqlDbType.VarChar, 1);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, Queue.Flag));

                sqlParam = new SqlParameter(DBResources.param_ClientKey, SqlDbType.VarChar, 255);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, Queue.ClientKey));

                sqlParam = new SqlParameter(DBResources.param_ProjectKey, SqlDbType.VarChar, 255);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, Queue.ProjectKey));

                sqlParam = new SqlParameter(DBResources.param_RETVAL, SqlDbType.Int, 1);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Output,0));

                db.ExecuteNonQuery(sqlCommand);

                retval = Convert.ToInt32(sqlCommand.Parameters[DBResources.param_RETVAL].Value);

            }
            return retval;

        }

        public void Dispose()
        {
            db = null;
        }
    }
}
