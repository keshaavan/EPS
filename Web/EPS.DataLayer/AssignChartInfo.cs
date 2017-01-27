using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using EPS.Resources;

namespace EPS.DataLayer
{
    partial class ChartInfo
    {
        private Entities.ChartInfo AssignChartInfo(IDataReader reader, Entities.ChartInfo chartInfo)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                chartInfo.Id = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_ClientProjectId);
            if (!reader.IsDBNull(i))
                chartInfo.ClientProjectId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_ClientMarket);
            if (!reader.IsDBNull(i))
                chartInfo.ClientMarket = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_ReceivedDate);
            if (!reader.IsDBNull(i))
                chartInfo.ReceivedDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_ClientReference);
            if (!reader.IsDBNull(i))
                chartInfo.ClientReference = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_FileName);
            if (!reader.IsDBNull(i))
                chartInfo.FileName = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_NoOfPages);
            if (!reader.IsDBNull(i))
                chartInfo.NoOfPages = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_OverallStatus);
            if (!reader.IsDBNull(i))
                chartInfo.OverallStatus = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_UploadedDate);
            if (!reader.IsDBNull(i))
                chartInfo.UploadedDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_UploadedBy);
            if (!reader.IsDBNull(i))
                chartInfo.UploadedBy = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_ChartMoreInfoId);           
            if (reader.GetName(i).Equals(DBResources.col_ChartMoreInfoId, StringComparison.InvariantCultureIgnoreCase))
            {
                i = reader.GetOrdinal(DBResources.col_ChartMoreInfoId);
                if (!reader.IsDBNull(i))
                    chartInfo.ChartMoreInfoId = reader.GetInt64(i);                
            }
            
            return chartInfo;
        }

        
        private Entities.ChartInfo AssignChartInfoDeallocation(IDataReader reader, Entities.ChartInfo chartInfo)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                chartInfo.Id = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_ClientProjectId);
            if (!reader.IsDBNull(i))
                chartInfo.ClientProjectId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_ClientMarket);
            if (!reader.IsDBNull(i))
                chartInfo.ClientMarket = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_ReceivedDate);
            if (!reader.IsDBNull(i))
                chartInfo.ReceivedDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_ClientReference);
            if (!reader.IsDBNull(i))
                chartInfo.ClientReference = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_FileName);
            if (!reader.IsDBNull(i))
                chartInfo.FileName = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_NoOfPages);
            if (!reader.IsDBNull(i))
                chartInfo.NoOfPages = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_OverallStatus);
            if (!reader.IsDBNull(i))
                chartInfo.OverallStatus = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_UploadedDate);
            if (!reader.IsDBNull(i))
                chartInfo.UploadedDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_UploadedBy);
            if (!reader.IsDBNull(i))
                chartInfo.UploadedBy = reader.GetInt64(i);            

            return chartInfo;
        }


        private Entities.ChartInfo AssignChartInfoDirectUpload(IDataReader reader, Entities.ChartInfo chartInfo)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                chartInfo.Id = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_ClientProjectId);
            if (!reader.IsDBNull(i))
                chartInfo.ClientProjectId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_ClientMarket);
            if (!reader.IsDBNull(i))
                chartInfo.ClientMarket = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_ReceivedDate);
            if (!reader.IsDBNull(i))
                chartInfo.ReceivedDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_ClientReference);
            if (!reader.IsDBNull(i))
                chartInfo.ClientReference = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_FileName);
            if (!reader.IsDBNull(i))
                chartInfo.FileName = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_NoOfPages);
            if (!reader.IsDBNull(i))
                chartInfo.NoOfPages = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_OverallStatus);
            if (!reader.IsDBNull(i))
                chartInfo.OverallStatus = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_UploadedDate);
            if (!reader.IsDBNull(i))
                chartInfo.UploadedDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_UploadedBy);
            if (!reader.IsDBNull(i))
                chartInfo.UploadedBy = reader.GetInt64(i);

            return chartInfo;
        }

        private Entities.ChartMoreInfo AssignChartMoreInfo(IDataReader reader, Entities.ChartMoreInfo chartMoreInfo)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                chartMoreInfo.Id = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_ChartId);
            if (!reader.IsDBNull(i))
                chartMoreInfo.ChartId = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_LevelStatusId);
            if (!reader.IsDBNull(i))
                chartMoreInfo.LevelStatusId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_LevelStatusCommentId);
            if (!reader.IsDBNull(i))
                chartMoreInfo.LevelStatusCommentId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_LevelUpdatedBy);
            if (!reader.IsDBNull(i))
                chartMoreInfo.LevelUpdatedBy = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_LevelNumber);
            if (!reader.IsDBNull(i))
                chartMoreInfo.LevelNumber = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_LevelStartDate);
            if (!reader.IsDBNull(i))
                chartMoreInfo.LevelStartDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_LevelEndDate);
            if (!reader.IsDBNull(i))
                chartMoreInfo.LevelEndDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_NumberOfDxCode);
            if (!reader.IsDBNull(i))
                chartMoreInfo.NumberOfDxCode = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_CompletionDate);
            if (!reader.IsDBNull(i))
                chartMoreInfo.CompletionDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_InvalidDate);
            if (!reader.IsDBNull(i))
                chartMoreInfo.InvalidDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_HoldDate);
            if (!reader.IsDBNull(i))
                chartMoreInfo.HoldDate = reader.GetDateTime(i);            

            return chartMoreInfo;
        }

        private Entities.ChartAudit AssignChartAudit(IDataReader reader, Entities.ChartAudit chartAudit)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                chartAudit.Id = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_ChartMoreInfoId);
            if (!reader.IsDBNull(i))
                chartAudit.ChartMoreInfoId = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_PageNumbers);
            if (!reader.IsDBNull(i))
                chartAudit.PageNumbers = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_CorrectedValue);
            if (!reader.IsDBNull(i))
                chartAudit.CorrectedValue = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_Comments);
            if (!reader.IsDBNull(i))
                chartAudit.Comments = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_ErrorCategoryId);
            if (!reader.IsDBNull(i))
                chartAudit.ErrorCategoryId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_ErrorSubCategoryId);
            if (!reader.IsDBNull(i))
                chartAudit.ErrorSubCategoryId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_AdditionalComments);
            if (!reader.IsDBNull(i))
                chartAudit.AdditionalComments = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_LastUpdatedDate);
            if (!reader.IsDBNull(i))
                chartAudit.LastUpdatedDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_L3LevelDisagreeId);
            if (!reader.IsDBNull(i))
                chartAudit.L3LevelDisagreeId = reader.GetInt32(i);

            return chartAudit;
        }

        private Entities.ChartAuditComments GetChartAuditComments(IDataReader reader, Entities.ChartAuditComments chartAudit)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                chartAudit.Id = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_PageNumbers);
            if (!reader.IsDBNull(i))
                chartAudit.PageNumbers = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_CorrectedValue);
            if (!reader.IsDBNull(i))
                chartAudit.CorrectedValue = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_Comments);
            if (!reader.IsDBNull(i))
                chartAudit.Comments = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_AdditionalComments);
            if (!reader.IsDBNull(i))
                chartAudit.AdditionalComments = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_ErrorCategoryId);
            if (!reader.IsDBNull(i))
                chartAudit.ErrorCategoryId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_ErrorSubCategoryId);
            if (!reader.IsDBNull(i))
                chartAudit.ErrorSubCategoryId = reader.GetInt32(i);

            return chartAudit;
        }

        private Entities.ChartProduction AssignChartProduction(IDataReader reader, Entities.ChartProduction chartProduction)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                chartProduction.Id = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_ChartMoreInfoId);
            if (!reader.IsDBNull(i))
                chartProduction.ChartMoreInfoId = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_LevelStatusId);
            if (!reader.IsDBNull(i))
                chartProduction.LevelStatusId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_LevelStatusCommentId);
            if (!reader.IsDBNull(i))
                chartProduction.LevelStatusCommentId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_LevelUpdatedBy);
            if (!reader.IsDBNull(i))
                chartProduction.LevelUpdatedBy = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_LevelNumber);
            if (!reader.IsDBNull(i))
                chartProduction.LevelNumber = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_StartDatetime);
            if (!reader.IsDBNull(i))
                chartProduction.StartDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_EndDatetime);
            if (!reader.IsDBNull(i))
                chartProduction.EndDate = reader.GetDateTime(i);

            i = reader.GetOrdinal(DBResources.col_TimeTaken);
            if (!reader.IsDBNull(i))
                chartProduction.TimeTaken = reader.GetInt32(i);

            return chartProduction;
        }
    }
}
