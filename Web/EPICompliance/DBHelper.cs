//System
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace EPICompliance
{
    public class DBHelper : IDisposable
    {
        #region Members
        private string c_sConStr = "EPIC";        
        private SqlConnection _objSQLCon;
        private SqlCommand _objSqlCmd;
        private SqlClientFactory _factory = null;
        protected string sUserName = string.Empty;
        #endregion

        #region properties

        protected string sConStr
        {
            get
            {
                return c_sConStr;
            }
            set
            {
                if (value != "")
                {
                    //c_sConStr = CryptographicHelper.Decrypt(value, false);
                    c_sConStr = value;
                }
            }
        }

        protected SqlConnection objSQLCon
        {
            get
            {
                return _objSQLCon;
            }
        }

        protected SqlCommand objSqlCmd
        {
            get
            {
                return _objSqlCmd;
            }
        }

        #endregion

        #region Functions

        public DBHelper(string UserName)
        {
            
            _factory = SqlClientFactory.Instance;
            _objSQLCon = new SqlConnection(ConfigurationManager.ConnectionStrings[c_sConStr].ConnectionString);
            _objSqlCmd = new SqlCommand();
            _objSqlCmd.Connection = _objSQLCon;
            
            this.sUserName = UserName;
        }

        #region parameters

        protected void ClearParameter()
        {
            _objSqlCmd.Parameters.Clear();
        }
        protected void AddParameter(SqlParameter objParam)
        {
            _objSqlCmd.Parameters.Add(objParam);
        }
        protected void AddOutputParameter(SqlParameter objParam, System.Data.SqlDbType objType)
        {
            objParam.Direction = ParameterDirection.Output;
            objParam.SqlDbType = objType;
            _objSqlCmd.Parameters.Add(objParam);
        }
        protected object GetOutputParameter(string ParameterName)
        {
            return _objSqlCmd.Parameters[ParameterName].Value;
        }
        #endregion

        #region Transactions
        private void BeginTransaction()
        {
            if (objSQLCon.State == System.Data.ConnectionState.Closed)
            {
                objSQLCon.Open();
            }
            objSqlCmd.Transaction = objSQLCon.BeginTransaction();
        }

        private void CommitTransaction()
        {
            objSqlCmd.Transaction.Commit();
            objSQLCon.Close();
        }

        private void RollbackTransaction()
        {
            objSqlCmd.Transaction.Rollback();
            objSQLCon.Close();
        }
        #endregion

        #region Database functions
        protected int ExecuteNonQuery(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            //enforceMultiTenancy();
            return ExecuteNonQuery(query, commandtype);
        }
       
        private int ExecuteNonQuery(string query, CommandType commandtype)
        {
            objSqlCmd.CommandText = query;
            objSqlCmd.CommandType = commandtype;
            int iRetVal = -1;
            try
            {
                if (objSQLCon.State == System.Data.ConnectionState.Closed)
                {
                    objSQLCon.Open();
                }

                BeginTransaction();

                iRetVal = objSqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw (ex);
            }
            finally
            {
                CommitTransaction();
                objSqlCmd.Parameters.Clear();

                if (objSQLCon.State == System.Data.ConnectionState.Open)
                {
                    objSQLCon.Close();
                    objSQLCon.Dispose();
                }
            }

            return iRetVal;
        }

        protected object ExecuteScaler(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            objSqlCmd.CommandText = query;
            objSqlCmd.CommandType = commandtype;
            object obj = null;
            try
            {
                if (objSQLCon.State == System.Data.ConnectionState.Closed)
                {
                    objSQLCon.Open();
                }

                BeginTransaction();
                obj = objSqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw (ex);
            }
            finally
            {
                CommitTransaction();
                objSqlCmd.Parameters.Clear();

                if (objSQLCon.State == System.Data.ConnectionState.Open)
                {
                    objSQLCon.Close();
                    objSQLCon.Dispose();
                    objSqlCmd.Dispose();
                }
            }

            return obj;
        }

        protected DataSet GetDataSet(string query, CommandType commandtype, ConnectionState connectionstate)
        {
           // enforceMultiTenancy();
            return GetDataSet(query, commandtype);
        }

        private DataSet GetDataSet(string query, CommandType commandtype)
        {
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            _objSqlCmd.CommandText = query;
            _objSqlCmd.CommandType = commandtype;
            adapter.SelectCommand = _objSqlCmd;
            DataSet dsResultSet = new DataSet();
            try
            {
                adapter.Fill(dsResultSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //objSqlCmd.Parameters.Clear();

                //if (objSQLCon.State == System.Data.ConnectionState.Open)
                //{
                //    objSQLCon.Close();
                //    objSQLCon.Dispose();
                //    objSqlCmd.Dispose();
                //}
            }
            return dsResultSet;
        }

        #endregion

        #region Dispose methods
        public void Dispose()
        {
            objSqlCmd.Parameters.Clear();
            if (objSQLCon.State == ConnectionState.Open)
            {
                objSQLCon.Close();
            }

            objSQLCon.Dispose();
            objSqlCmd.Dispose();
        }
        #endregion

        public void insertAuditTrail(string sUserName, string sPage, string sTransaction, string sMessage, string sIPAddress)
        {
            AddParameter(new SqlParameter("@UserName", sUserName));
            AddParameter(new SqlParameter("@PageName", sPage));
            AddParameter(new SqlParameter("@Transaction", sTransaction));
            AddParameter(new SqlParameter("@Message", sMessage));
            AddParameter(new SqlParameter("@IPAddress", sIPAddress));

            ExecuteNonQuery("InsertAuditTrail", CommandType.StoredProcedure);
        }

        public void insertLoginHistory(string sUserName, string sType, string sIPAddress)
        {
            AddParameter(new SqlParameter("@UserName", sUserName));
            AddParameter(new SqlParameter("@Type", sType));
            AddParameter(new SqlParameter("@IPAddress", sIPAddress));

            ExecuteNonQuery("InsertLogInHistory", CommandType.StoredProcedure);
        }
        #endregion
    }
}
