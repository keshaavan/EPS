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
    partial class ChartInfo 
    {
        public Entities.ChartMoreInfo GetChartMoreInfoByChartIdAndLevel(Int64 chartId, int levelNumber)
        {
            var chartMoreInfo = new ChartMoreInfo();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartMoreInfoByChartIdAndLevel))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ChartId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartId));

                sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, levelNumber));
                
                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignChartMoreInfo(reader, chartMoreInfo);
                    }
                }
            }

            return chartMoreInfo;
        }

        public IEnumerable<Entities.ChartMoreInfo> GetChartMoreInfoByChartId(Int32 clientProjectId, Int64 chartId)
        {
            var chartMoreInfoes = new  List<ChartMoreInfo>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartMoreInfoByChartId))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_ChartId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartMoreInfo = new ChartMoreInfo();
                        AssignChartMoreInfo(reader, chartMoreInfo);
                        chartMoreInfoes.Add(chartMoreInfo);
                    }
                }
            }

            return chartMoreInfoes;
        }


        public IEnumerable<Entities.ChartMoreInfo> GetChartMoreInfoByChartMoreId(Int32 clientProjectId, Int64 chartMoreId)
        {
            var chartMoreInfoes = new List<ChartMoreInfo>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartAuditComments))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_ChartMoreInfoId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartMoreId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartMoreInfo = new ChartMoreInfo();
                        AssignChartMoreInfo(reader, chartMoreInfo);
                        chartMoreInfoes.Add(chartMoreInfo);
                    }
                }
            }

            return chartMoreInfoes;
        }


        public Entities.ChartMoreInfo GetChartMoreInfoById(Int64 id)
        {
            var chartMoreInfo = new Entities.ChartMoreInfo();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartMoreInfoById))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignChartMoreInfo(reader, chartMoreInfo);
                    }
                }
            }

            return chartMoreInfo;
        }
    }
}
