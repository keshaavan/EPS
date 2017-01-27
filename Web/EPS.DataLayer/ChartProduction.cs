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
        public IEnumerable<Entities.ChartProduction> GetChartProductionByChartMoreInfoId(int clientProjectId, long chartMoreInfoId, int levelNumber)
        {
            var chartProductions = new List<ChartProduction>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetChartProductionByChartMoreInfoIdAndLevel))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_ChartMoreInfoId, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, chartMoreInfoId));

                sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, levelNumber));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var chartProduction = new ChartProduction();
                        AssignChartProduction(reader, chartProduction);
                        chartProductions.Add(chartProduction);
                    }
                }
            }

            return chartProductions;
        }
    }
}
