using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EPS.Resources;
using EPS.Utilities;

namespace EPS.DataLayer
{
    public class Userpipeline
    {
        private SqlConnection _objSQLCon = new SqlConnection();
        private SqlCommand _objSqlCmd = null;
        SqlDatabase db;
        private SqlClientFactory _factory = null;

        public Userpipeline()
        {

            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public DataSet getUser(string sUserName)
        {

            //DataSet dsUsersProfile = new DataSet();

            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                // DbTransaction trans = conn.BeginTransaction();
                try
                {
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.GetPipeLineUsers))
                    {

                        SqlParameter sqlParam;
                        sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sUserName));

                        sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                        return db.ExecuteDataSet(sqlCommand);
                    }
                    // trans.Commit();                
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }

        }
    }
}
