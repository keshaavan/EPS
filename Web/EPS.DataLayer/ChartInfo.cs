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
    partial class ChartInfo : IDisposable
    {
        SqlDatabase db;

        public ChartInfo()
        {
            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public Entities.ChartInfo GetChartInfoById(Int64 id)
        {
            var chartInfo = new Entities.ChartInfo();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartInfoById))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignChartInfo(reader, chartInfo);
                    }
                }
            }

            return chartInfo;
        }

        public IEnumerable<Entities.ChartInfo> GetChartInfoByEmployee(int clientProjectId, string userName, int levelNumber)
        {
            var chartInfoes = new List<Entities.ChartInfo>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartInfoByEmployee))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.VarChar, 256);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, userName));

                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, levelNumber));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartInfo = new Entities.ChartInfo();
                        AssignChartInfo(reader, chartInfo);
                        chartInfoes.Add(chartInfo);
                    }
                }
            }

            return chartInfoes;
        }


        public IEnumerable<Entities.ChartAuditComments> GetChartAuditComments(string ChartMoreInfoId)
        {
            var chartAudit = new List<Entities.ChartAuditComments>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartAuditComments))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ChartMoreInfoId, SqlDbType.VarChar, 256);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ChartMoreInfoId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartaudit = new Entities.ChartAuditComments();
                        GetChartAuditComments(reader, chartaudit);
                        chartAudit.Add(chartaudit);
                    }
                }
            }

            return chartAudit;
        }


        public IEnumerable<Entities.ChartAuditComments> DeleteChartData(int Id)
        {
            var chartAudit = new List<Entities.ChartAuditComments>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateChartAudit))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ChartMoreInfoId, SqlDbType.VarChar, 256);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, Id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartaudit = new Entities.ChartAuditComments();
                        GetChartAuditComments(reader, chartaudit);
                        chartAudit.Add(chartaudit);
                    }
                }
            }

            return chartAudit;
        }









        //public void UpdateCommentStatus(Entities.ChartMoreInfo chartmoreInfo)
        //{
        //    using (DbConnection conn = db.CreateConnection())
        //    {
        //        if (conn.State == ConnectionState.Open)
        //            conn.Close();

        //        conn.Open();
        //        DbTransaction trans = conn.BeginTransaction();

        //        try
        //        {
        //            SqlParameter sqlParam;

        //            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateCommentStatus))
        //            {                        
        //                sqlParam = new SqlParameter(DBResources.param_LevelStatusCommentId, SqlDbType.Int);
        //                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartmoreInfo.LevelStatusCommentId));
        //                db.ExecuteNonQuery(sqlCommand, trans);
        //            }

        //            trans.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            trans.Rollback();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (conn.State == ConnectionState.Open)
        //                conn.Close();
        //        }
        //    }
        //}



        public void UpdateChartInfoStatus(Entities.ChartInfo chartInfo)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlParameter sqlParam;

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateChartInfoStatus))
                    {
                        sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.Id));

                        sqlParam = new SqlParameter(DBResources.param_NoOfPages, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.NoOfPages));

                        sqlParam = new SqlParameter(DBResources.param_OverallStatus, SqlDbType.VarChar, 512);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.OverallStatus));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateChartMoreInfoStatus))
                    {
                        sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.Id));

                        sqlParam = new SqlParameter(DBResources.param_LevelStatusId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.LevelStatusId));

                        sqlParam = new SqlParameter(DBResources.param_LevelStatusCommentId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (chartInfo.ChartMoreInfo.LevelStatusCommentId == 0) ? new Nullable<Int32>() : chartInfo.ChartMoreInfo.LevelStatusCommentId));

                        sqlParam = new SqlParameter(DBResources.param_LevelUpdatedBy, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.LevelUpdatedBy));

                        sqlParam = new SqlParameter(DBResources.param_NumberOfDxCode, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.NumberOfDxCode));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }



        public void UpdateCommentInfoStatus(Entities.ChartInfo chartInfo)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlParameter sqlParam;
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateChartMoreInfoEditable))
                    {


                        sqlParam = new SqlParameter(DBResources.param_fileName, SqlDbType.VarChar);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.FileName));


                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ClientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.LevelNumber));


                        sqlParam = new SqlParameter(DBResources.param_NumberOfDxCode, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.NumberOfDxCode));

                        sqlParam = new SqlParameter(DBResources.param_NoOfPages, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.NoOfPages));




                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        public void ActiveInactiveUserMapping(int selectedCheckBox, int drpValue, string username)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();


                try
                {

                    SqlParameter sqlParam;

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateChartMoreInfoEditable))
                    {                        

                        sqlParam = new SqlParameter(DBResources.param_InactiveProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, selectedCheckBox));

                        sqlParam = new SqlParameter(DBResources.param_ActiveProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, drpValue));

                        sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.NVarChar, 256);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, username));

                        
                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }


        public void UpdateChartInfoStatus(Entities.ChartInfo chartInfo, Int64 previousLevelUpdatedBy)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlParameter sqlParam;

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateChartMoreInfoReallocate))
                    {
                        sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.Id));

                        sqlParam = new SqlParameter(DBResources.param_ChartId, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.ChartId));

                        sqlParam = new SqlParameter(DBResources.param_LevelStatusId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.LevelStatusId));

                        sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.LevelNumber));

                        sqlParam = new SqlParameter(DBResources.param_LevelUpdatedBy, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ChartMoreInfo.LevelUpdatedBy));

                        sqlParam = new SqlParameter(DBResources.param_PreviousLevelUpdatedBy, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, previousLevelUpdatedBy));

                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartInfo.ClientProjectId));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        public int UploadCharts(System.Data.DataSet ds, Int64 loggedOnId, int clientProjectId)
        {
            int retValue = 0;

            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    //Clear the staging tables for the ClientProjectId passed.
                    using (DbCommand sqlCommand = db.GetSqlStringCommand(String.Format("DELETE FROM {0} WHERE ClientProjectId = {1}; ", DBResources.bc_dest_EPS_Stage, clientProjectId)))
                    {
                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    //Perform the insert of data
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_ImportChartData))
                    {
                        string xmlData = ds.GetXml();

                        var sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_xmlData, SqlDbType.Xml);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, xmlData));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    //Perform the Merge
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_ChartInfoBulkMerge))
                    {
                        var sqlParam = new SqlParameter(DBResources.param_UploadedBy, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, loggedOnId));

                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_RecordsAffected, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Output, 0));

                        db.ExecuteNonQuery(sqlCommand, trans);

                        retValue = Convert.ToInt32(sqlCommand.Parameters[DBResources.param_RecordsAffected].Value);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }

            return retValue;
        }

        public int BulkDeallocate(System.Data.DataSet ds, Int64 loggedOnId, int clientProjectId, Boolean deleteAll)
        {
            int retValue = 0;

            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    //Clear the staging tables for the ClientProjectId passed.
                    using (DbCommand sqlCommand = db.GetSqlStringCommand(String.Format("DELETE FROM {0} WHERE ClientProjectId = {1}; ", DBResources.bc_dest_EPS_Stage_BulkDelete, clientProjectId)))
                    {
                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    //Perform the insert of data
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_ImportBulkDeallocate))
                    {
                        string xmlData = ds.GetXml();

                        var sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_UploadedBy, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, loggedOnId));

                        sqlParam = new SqlParameter(DBResources.param_xmlData, SqlDbType.Xml);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, xmlData));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    //Perform the deallocation
                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_ChartBulkDeallocate))
                    {
                        var sqlParam = new SqlParameter(DBResources.param_UploadedBy, SqlDbType.BigInt);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, loggedOnId));

                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_DeleteAll, SqlDbType.Bit);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, deleteAll));

                        sqlParam = new SqlParameter(DBResources.param_RecordsAffected, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Output, 0));

                        sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout * 2;

                        db.ExecuteNonQuery(sqlCommand, trans);

                        retValue = Convert.ToInt32(sqlCommand.Parameters[DBResources.param_RecordsAffected].Value);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

                return retValue;
            }
        }

        public int GetChartsPendingCount(int clientProjectId, int levelNumber)
        {
            int retValue = 0;

            try
            {
                using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartsPendingCount))
                {
                    var sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.BigInt);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                    sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, levelNumber));

                    sqlParam = new SqlParameter(DBResources.param_RecordsAffected, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Output, 0));

                    db.ExecuteNonQuery(sqlCommand);

                    retValue = Convert.ToInt32(sqlCommand.Parameters[DBResources.param_RecordsAffected].Value);
                }

                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void RequestChart(int clientProjectId, int levelNumber, string userName)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    string spName = string.Format(DBResources.sp_RequestLevelChart, levelNumber);

                    SqlParameter sqlParam;

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(spName))
                    {
                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.NVarChar, 256);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, userName));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        public void RequestChartByClientReference(int clientProjectId, int levelNumber, string userName, string clientReference)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    string spName = string.Format(DBResources.sp_RequestLevelChartByClientReference, levelNumber); ;

                    SqlParameter sqlParam;

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(spName))
                    {
                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.NVarChar, 256);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, userName));

                        sqlParam = new SqlParameter(DBResources.param_ClientReference, SqlDbType.VarChar, 30);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientReference));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        public IEnumerable<Entities.ChartInfo> GetWIPChartInfo(string userName, int clientProjectId, int levelNumber, int status, string fromDate, string toDate)
        {
            var chartInfoes = new List<Entities.ChartInfo>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetWIPChartInfo))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.VarChar, 256);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, userName));

                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, levelNumber));

                sqlParam = new SqlParameter(DBResources.param_LevelStatusId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, status));

                //sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                //sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, string.IsNullOrEmpty(FromDate) ? DBNull.Value.ToString() : FromDate));

                //sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                //sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, string.IsNullOrEmpty(ToDate) ? DBNull.Value.ToString() : ToDate));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartInfo = new Entities.ChartInfo();
                        AssignChartInfo(reader, chartInfo);
                        chartInfoes.Add(chartInfo);
                    }
                }
            }
            return chartInfoes;
        }

        public System.Data.DataTable GetChartsForReallocation(int clientProjectId)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartsForReallocation))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                return db.ExecuteDataSet(sqlCommand).Tables[0];
            }
        }

        public IEnumerable<Entities.ChartInfo> GetChartsForDeallocation(int clientProjectId, DateTime fromDate, DateTime toDate, int levelNumber)
        {
            var chartInfoes = new List<Entities.ChartInfo>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartInfoDeallocation))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.DateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.DateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, levelNumber));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartInfo = new Entities.ChartInfo();
                        // AssignChartInfo(reader, chartInfo);
                        AssignChartInfoDeallocation(reader, chartInfo);
                        chartInfoes.Add(chartInfo);
                    }
                }
            }
            return chartInfoes;
        }

        public IEnumerable<Entities.ChartInfo> GetChartsForDirectUpload(int clientProjectId, DateTime fromDate, DateTime toDate)
        {
            var chartInfoes = new List<Entities.ChartInfo>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartInfoDirectUpload))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.DateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.DateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartInfo = new Entities.ChartInfo();
                        AssignChartInfoDirectUpload(reader, chartInfo);
                        chartInfoes.Add(chartInfo);
                    }
                }
            }
            return chartInfoes;
        }

        public System.Data.DataSet GetChartInfoByOverallStatus(int clientProjectId, Helper.ChartDateTypes chartDateType, DateTime fromDate, DateTime toDate,
            string overallStatus, int? levelNumber, int? levelStatusId)
        {
            try
            {
                using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_rptGetChartDetail))
                {
                    SqlParameter sqlParam;
                    sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                    sqlParam = new SqlParameter(DBResources.param_ChartDateType, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (int)chartDateType));

                    sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                    sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                    sqlParam = new SqlParameter(DBResources.param_OverallStatus, SqlDbType.VarChar, 512);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, string.IsNullOrEmpty(overallStatus) ? null : overallStatus));

                    sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (levelNumber.HasValue == true) ? levelNumber.Value : new Nullable<int>()));

                    sqlParam = new SqlParameter(DBResources.param_LevelStatusId, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (levelStatusId.HasValue == true) ? levelStatusId.Value : new Nullable<int>()));

                    sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                    return db.ExecuteDataSet(sqlCommand);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public System.Data.DataSet GetChartData(int clientprojectid,Helper.ChartDateTypes datatype, DateTime fromDate, DateTime toDate, string username, string fromstatusId)
        {
            try
            {
                using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartBulkReallocation))
                {
                    SqlParameter sqlParam;

                    sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientprojectid));                    

                    sqlParam = new SqlParameter(DBResources.param_DateType, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (int)datatype));

                    sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                    sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                    sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.VarChar, 512);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, username ));

                    sqlParam = new SqlParameter(DBResources.param_FromStatus, SqlDbType.VarChar,512);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromstatusId));                    

                    sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                    return db.ExecuteDataSet(sqlCommand);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        


        public System.Data.DataSet GetChartInfoForInvoice(int clientProjectId, DateTime fromDate, DateTime toDate, Boolean isCompleted)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_rptGetChartInfoForInvoice))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                sqlParam = new SqlParameter(DBResources.param_IsCompleted, SqlDbType.Bit);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, isCompleted));

                sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                return db.ExecuteDataSet(sqlCommand);
            }
        }

        public void DeleteClientAllocatedCharts(int clientProjectId, string xmlData)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlParameter sqlParam;

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_DeleteClientAllocatedCharts))
                    {
                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_xmlData, SqlDbType.Xml);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, xmlData));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }




        //public void GetUserDetails()
        //{
        //    using (DbConnection conn = db.CreateConnection())
        //    {
        //        if (conn.State == ConnectionState.Open)
        //            conn.Close();

        //        conn.Open();
        //        DbTransaction trans = conn.BeginTransaction();

        //        try
        //        {

        //            DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetInActivateProjectList);
        //            db.ExecuteNonQuery(sqlCommand, trans);
        //            trans.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            trans.Rollback();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (conn.State == ConnectionState.Open)
        //                conn.Close();
        //        }
        //    }
        //}




        public void DirectUploadCharts(int clientProjectId, string epsSysUserName, string xmlData)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlParameter sqlParam;

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_DirectUploadCharts))
                    {
                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_EPSSysUserName, SqlDbType.VarChar, 256);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, epsSysUserName));

                        sqlParam = new SqlParameter(DBResources.param_xmlData, SqlDbType.Xml);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, xmlData));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        public System.Data.DataSet GetInfoForUserPerformance(string clientProjectId, DateTime fromDate, DateTime toDate, string username, int levelNumber, bool isByDxCode = false)
        {
            try
            {
                using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_rptGetInfoForUserPerformace))
                {
                    SqlParameter sqlParam;
                    sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.VarChar, 500);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                    sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.VarChar, 256);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, username));

                    sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, levelNumber));

                    sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                    sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                    sqlParam = new SqlParameter(DBResources.param_isByDxCode, SqlDbType.Bit);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, isByDxCode));

                    sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                    return db.ExecuteDataSet(sqlCommand);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public System.Data.DataSet GetSummaryInfoForAging(int clientProjectId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_rptGetAgingSummaryReport))
                {
                    SqlParameter sqlParam;
                    sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                    sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                    sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                    sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                    return db.ExecuteDataSet(sqlCommand);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public System.Data.DataSet GetUserDetails()
        {
            try
            {

                DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetInActivateProjectList);

                sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                return db.ExecuteDataSet(sqlCommand);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public System.Data.DataSet GetActiveProjectList()
        {
            try
            {

                DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetActiveProjectList);

                sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                return db.ExecuteDataSet(sqlCommand);

            }
            catch (Exception)
            {
                throw;
            }
        }

        


        public System.Data.DataSet GetAgingReportDetails(int clientProjectId, DateTime fromDate, DateTime toDate, Helper.DateRangeType dateRangeType, bool isCompleted)
        {
            try
            {
                string sp_name = (isCompleted) ? DBResources.sp_rptGetAgingReportCompleted : DBResources.sp_rptGetAgingReportIncomplete;

                using (DbCommand sqlCommand = db.GetStoredProcCommand(sp_name))
                {
                    SqlParameter sqlParam;
                    sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                    sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, fromDate));

                    sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, toDate));

                    sqlParam = new SqlParameter(DBResources.param_DateRangeType, SqlDbType.Int);
                    sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (int)dateRangeType));

                    sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                    return db.ExecuteDataSet(sqlCommand);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            db = null;
        }
    }
}
