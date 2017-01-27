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
    public class UserManagement 
    {
        private SqlConnection _objSQLCon;
        private SqlCommand _objSqlCmd;
        private SqlClientFactory _factory = null;
        SqlDatabase db;

        public UserManagement()
        {
            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public int insertUserPipeline(string sType, string sUserName, string sEmailAddress, string sDisplayName, string sAuthKey, string sRole, string sClient, string sModifiedBy)
        {
            int iretValue = 0;

            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                // DbTransaction trans = conn.BeginTransaction();
                try
                {
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.usp_EPIC_User_PipelineInsertUpdate))
                    {
                        var sqlParam = new SqlParameter(DBResources.col_C_Type, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sType));

                        sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sUserName));

                        sqlParam = new SqlParameter(DBResources.col_C_sUserRole, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sRole));

                        sqlParam = new SqlParameter(DBResources.col_C_sUserDisplayName, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sDisplayName));

                        sqlParam = new SqlParameter(DBResources.col_C_sAuthenticationKey, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sAuthKey));

                        sqlParam = new SqlParameter(DBResources.col_C_sEmail, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sEmailAddress));

                        sqlParam = new SqlParameter(DBResources.col_C_sClientList, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sClient));

                        sqlParam = new SqlParameter(DBResources.col_C_sModifiedBy, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sModifiedBy));

                        sqlParam = new SqlParameter(DBResources.col_C_sIsExpired, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, 0));

                        db.ExecuteNonQuery(sqlCommand);

                    }

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return iretValue;
        }


        public DataSet getUserInPipeline(string sAuthenticationKey)
        {            

            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                
                try
                {
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.usp_EPIC_User_GetPipelineInfoByKey))
                    {

                        SqlParameter sqlParam;
                        sqlParam = new SqlParameter(DBResources.col_C_sAuthenticationKey, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sAuthenticationKey));

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

        public void updateEPIUserStatus(string sAuthenticationKey, string sModifiedBy)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();

                try
                {
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.usp_EPIC_User_CreateProfile))
                    {

                        SqlParameter sqlParam;
                        sqlParam = new SqlParameter(DBResources.col_C_sAuthenticationKey, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sAuthenticationKey));
                        sqlParam = new SqlParameter(DBResources.col_C_sModifiedBy, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, sModifiedBy));

                        sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                         db.ExecuteDataSet(sqlCommand);
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

        //public int deleteUserPipeline(string sUserName)
        //{
        //    ClearParameter();
        //    AddParameter(new SqlParameter(Resources.DBResources.param_UserName, sUserName));

        //    return ExecuteNonQuery(Resources.DBResources.usp_EPIC_User_DeletePipelineInfo, CommandType.StoredProcedure);
        //}

        //public DataSet getUserInPipeline(string sAuthenticationKey)
        //{
        //    ClearParameter();
        //    AddParameter(new SqlParameter(Resources.DBResources.col_C_sAuthenticationKey, sAuthenticationKey));

        //    return GetDataSet(Resources.DBResources.usp_EPIC_User_GetPipelineInfoByKey, CommandType.StoredProcedure);
        //}

        //public void updateEPIUserStatus(string sAuthenticationKey, string sModifiedBy)
        //{

        //    AddParameter(new SqlParameter(Resources.DBResources.col_C_sAuthenticationKey, sAuthenticationKey));
        //    AddParameter(new SqlParameter(Resources.DBResources.col_C_sModifiedBy, sModifiedBy));

        //    //  ExecuteNonQuery(DBCons.SP.usp_EPIC_User_CreateProfile, CommandType.StoredProcedure);

        //}

        //protected void ClearParameter()
        //{
        //    _objSqlCmd.Parameters.Clear();
        //}
        //protected void AddParameter(SqlParameter objParam)
        //{
        //    _objSqlCmd.Parameters.Add(objParam);
        //}

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        //protected SqlCommand objSqlCmd
        //{
        //    get
        //    {
        //        return _objSqlCmd;
        //    }
        //}
        //protected SqlConnection objSQLCon
        //{
        //    get
        //    {
        //        return _objSQLCon;
        //    }
        //}

        //#region Transactions
        //private void BeginTransaction()
        //{
        //    if (objSQLCon.State == ConnectionState.Closed)
        //    {
        //        objSQLCon.Open();
        //    }
        //    objSqlCmd.Transaction = objSQLCon.BeginTransaction();
        //}

        //private void CommitTransaction()
        //{
        //    objSqlCmd.Transaction.Commit();
        //    objSQLCon.Close();
        //}

        //private void RollbackTransaction()
        //{
        //    objSqlCmd.Transaction.Rollback();
        //    objSQLCon.Close();
        //}
        //#endregion

        //protected int ExecuteNonQuery(string query, CommandType commandtype)
        //{
        //    objSqlCmd.CommandText = query;
        //    objSqlCmd.CommandType = commandtype;
        //    int iRetVal = -1;
        //    try
        //    {
        //        if (objSQLCon.State == ConnectionState.Closed)
        //        {
        //            objSQLCon.Open();
        //        }

        //        BeginTransaction();

        //        iRetVal = objSqlCmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        RollbackTransaction();
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        CommitTransaction();
        //        objSqlCmd.Parameters.Clear();

        //        if (objSQLCon.State == ConnectionState.Open)
        //        {
        //            objSQLCon.Close();
        //            objSQLCon.Dispose();
        //        }
        //    }

        //    return iRetVal;
        //}

        //protected DataSet GetDataSet(string query, CommandType commandtype)
        //{
        //    DbDataAdapter adapter = _factory.CreateDataAdapter();
        //    _objSqlCmd.CommandText = query;
        //    _objSqlCmd.CommandType = commandtype;
        //    adapter.SelectCommand = _objSqlCmd;
        //    DataSet dsResultSet = new DataSet();
        //    try
        //    {
        //        adapter.Fill(dsResultSet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objSqlCmd.Parameters.Clear();

        //        if (objSQLCon.State == ConnectionState.Open)
        //        {
        //            objSQLCon.Close();
        //            objSQLCon.Dispose();
        //            objSqlCmd.Dispose();
        //        }
        //    }
        //    return dsResultSet;
        //}
    }
}
