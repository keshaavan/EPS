using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace EPS.Utilities
{
    public static class Helper
    {
        public static SqlParameter AssignSqlParameter(SqlParameter param, ParameterDirection direction, object value)
        {
            param.Direction = direction;

            if (direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput)
                param.Value = value;

            return param;
        }

        public enum ChartStatus
        {
            Pending = 14,
            WorkInProgress = 15,
            Completed = 1,
            Hold = 2,
            Invalid = 3
        }

        public enum ChartFromStatus
        {
            Pending = 14,
            WorkInProgress = 15,            
            Hold = 2,
            Invalid = 3
        }


        public enum ChartDateTypes
        {
            ProcessedDate = 0,
            ReceivedDate = 1,
            ImportedDate = 2
        }

        public enum DateRangeType
        {
            LessThan10 = 0,
            Between10And20 = 1,
            Between20And30 = 2,
            MoreThan30 = 3
        }
    }
}
