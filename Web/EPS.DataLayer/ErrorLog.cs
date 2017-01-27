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
    partial class ErrorLog : IDisposable
    {
        SqlDatabase db;

        public ErrorLog()
        {
            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public void InsertErrorLog(Entities.ErrorLogs errorLog)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_InsertErrorLog))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.VarChar, 256);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, errorLog.UserName));

                sqlParam = new SqlParameter(DBResources.param_Timestamp, SqlDbType.DateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, errorLog.TimeStamp));

                sqlParam = new SqlParameter(DBResources.param_MessageType, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, errorLog.MessageType));

                sqlParam = new SqlParameter(DBResources.param_Message, SqlDbType.VarChar, -1);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, errorLog.Message));

                sqlParam = new SqlParameter(DBResources.param_Module, SqlDbType.VarChar, -1);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, errorLog.Module));

                sqlParam = new SqlParameter(DBResources.param_FromIP, SqlDbType.VarChar, 20);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, errorLog.FromIP));

                sqlParam = new SqlParameter(DBResources.param_LogType, SqlDbType.VarChar, 30);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, errorLog.LogType)); 
                
                db.ExecuteNonQuery(sqlCommand);
            }
        }

        public void Dispose()
        {
            db = null;
        }
    }
}
