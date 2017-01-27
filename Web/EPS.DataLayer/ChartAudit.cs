using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using EPS.Entities;
using EPS.Resources;
using EPS.Utilities;

namespace EPS.DataLayer
{
    partial class ChartInfo : IDisposable
    {
        public IEnumerable<Entities.ChartAudit> GetChartAuditByChartMoreInfoId(Int64 chartMoreInfoId)
        {
            var chartAudits = new List<Entities.ChartAudit>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartAuditByChartMoreInfoId))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ChartMoreInfoId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartMoreInfoId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartAudit = new ChartAudit();
                        AssignChartAudit(reader, chartAudit);
                        chartAudits.Add(chartAudit);
                    }
                }
            }

            return chartAudits;
        }

        public Entities.ChartAudit GetChartAuditById(Int64 id)
        {
            var chartAudit = new Entities.ChartAudit();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartAuditById))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignChartAudit(reader, chartAudit);
                    }
                }
            }

            return chartAudit;
        }

        public void InsertChartAudit(Entities.ChartAudit chartAudit)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_InsertChartAudit))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ChartMoreInfoId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAudit.ChartMoreInfoId));                

                sqlParam = new SqlParameter(DBResources.param_PageNumbers, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAudit.PageNumbers.Trim()));

                sqlParam = new SqlParameter(DBResources.param_CorrectedValue, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAudit.CorrectedValue.Trim()));

                sqlParam = new SqlParameter(DBResources.param_Comments, SqlDbType.VarChar, 1024);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAudit.Comments.Trim()));

                sqlParam = new SqlParameter(DBResources.param_ErrorCategoryId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAudit.ErrorCategoryId));

                sqlParam = new SqlParameter(DBResources.param_ErrorSubCategoryId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAudit.ErrorSubCategoryId));

                sqlParam = new SqlParameter(DBResources.param_AdditionalComments, SqlDbType.VarChar, -1);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAudit.AdditionalComments));

                sqlParam = new SqlParameter(DBResources.param_L3LevelDisagreeId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAudit.L3LevelDisagreeId));

                db.ExecuteNonQuery(sqlCommand);
            }
        }




        public void UpdateChartAudit(Entities.ChartAuditComments chartAuditComments)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateChartAudit))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ChartMoreInfoId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.ChartMoreInfoId));

                sqlParam = new SqlParameter(DBResources.param_AuditId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.Id));

                sqlParam = new SqlParameter(DBResources.param_PageNumbers, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.PageNumbers.Trim()));

                sqlParam = new SqlParameter(DBResources.param_CorrectedValue, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.CorrectedValue.Trim()));

                sqlParam = new SqlParameter(DBResources.param_Comments, SqlDbType.VarChar, 1024);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.Comments.Trim()));                

                sqlParam = new SqlParameter(DBResources.param_ErrorCategoryId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.ErrorCategoryId));

                sqlParam = new SqlParameter(DBResources.param_ErrorSubCategoryId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.ErrorSubCategoryId));

                sqlParam = new SqlParameter(DBResources.param_AdditionalComments, SqlDbType.VarChar, -1);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.AdditionalComments));

                sqlParam = new SqlParameter(DBResources.param_IsDelete, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, 1));


                db.ExecuteNonQuery(sqlCommand);
            }
        }

        public void DeleteChartAudit(Entities.ChartAuditComments chartAuditComments)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateChartAudit))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ChartMoreInfoId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.ChartMoreInfoId));

                sqlParam = new SqlParameter(DBResources.param_AuditId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartAuditComments.Id));
                sqlParam = new SqlParameter(DBResources.param_PageNumbers, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, string.Empty));

                sqlParam = new SqlParameter(DBResources.param_CorrectedValue, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, string.Empty));

                sqlParam = new SqlParameter(DBResources.param_Comments, SqlDbType.VarChar, 1024);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, string.Empty));

                sqlParam = new SqlParameter(DBResources.param_ErrorCategoryId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, 0));

                sqlParam = new SqlParameter(DBResources.param_ErrorSubCategoryId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, 0));

                sqlParam = new SqlParameter(DBResources.param_AdditionalComments, SqlDbType.VarChar, -1);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, string.Empty));

                sqlParam = new SqlParameter(DBResources.param_IsDelete, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, 0));

                db.ExecuteNonQuery(sqlCommand);
            }
        }



        public System.Data.DataSet GetChartAudits(int clientProjectId, DateTime? fromDate, DateTime? toDate)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_rptGetChartAudits))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_FromDate, SqlDbType.SmallDateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ((fromDate.HasValue == true) ? fromDate.Value : new Nullable<DateTime>())));

                sqlParam = new SqlParameter(DBResources.param_ToDate, SqlDbType.SmallDateTime);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ((toDate.HasValue == true) ? toDate.Value : new Nullable<DateTime>())));

                sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                return db.ExecuteDataSet(sqlCommand);
            }
        }

        public System.Data.DataSet GetChartFromstatus()
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_BulkReallocation_FROM_Status))
            {
                //SqlParameter sqlParam;
                //sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.Int);
                //sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, Id));

                sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                return db.ExecuteDataSet(sqlCommand);
            }
        }
        public System.Data.DataSet GetCharttostatus(int id)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_BulkReallocation_TO_Status))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_toStatusID, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                sqlCommand.CommandTimeout = ConfigurationHelper.ConnectionTimeout;

                return db.ExecuteDataSet(sqlCommand);
            }
        }

        


        public IEnumerable<Entities.ChartAudit> GetChartAuditByChartId(long chartId)
        {
            var chartAudits = new List<Entities.ChartAudit>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartAuditByChartInfoId))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ChartId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartAudit = new ChartAudit();
                        AssignChartAudit(reader, chartAudit);
                        chartAudits.Add(chartAudit);
                    }
                }
            }

            return chartAudits;
        }

        public void UpdateL3ChartAuditLevelDisagreement(int clientProjectId, string xmlData)
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

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_UpdateL3ChartAuditLevelDisagreement))
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
    }
}
