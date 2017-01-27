using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

using EPS.Utilities;

namespace EPS.Interfaces
{
    public interface IChartInfo
    {
        Entities.ChartInfo GetChartInfoById(Int64 id);
        IEnumerable<Entities.ChartInfo> GetChartInfoByEmployee(int clientProjectId, string userName, int levelNumber);
        
        void UpdateChartInfoStatus(Entities.ChartInfo chartInfo);
        void UpdateChartInfoStatus(Entities.ChartInfo chartInfo, Int64 previousLevelUpdatedBy);
        int UploadCharts(DataSet ds, Int64 loggedOnId, int clientProjectId);
        int BulkDeallocate(System.Data.DataSet ds, Int64 loggedOnId, int clientProjectId, Boolean deleteAll);

        void RequestChart(int clientProjectId, int levelNumber, string userName);
        void RequestChartByClientReference(int clientProjectId, int levelNumber, string userName, string clientReference);
        void DeleteClientAllocatedCharts(int clientProjectId, string xmlData);
        void DirectUploadCharts(int clientProjectId, string epsSysUserName, string xmlData);
        int GetChartsPendingCount(int clientProjectId, int levelNumber);

        IEnumerable<Entities.ChartInfo> GetWIPChartInfo(string userName, int clientProjectId, int levelNumber, int status, string fromDate, string toDate);
        IEnumerable<Entities.ChartInfo> GetChartInfoByEmployeeId(int clientProjectId, string userName, int levelNumber);
        IEnumerable<Entities.ChartInfo> GetChartsForDeallocation(int clientProjectId, DateTime fromDate, DateTime toDate, int levelNumber);
        IEnumerable<Entities.ChartInfo> GetChartsForDirectUpload(int clientProjectId, DateTime fromDate, DateTime toDate);
        
        List<Entities.ChartInfo> GetChartInfoForReallocation(int clientProjectId);

        Entities.ChartMoreInfo GetChartMoreInfoByChartIdAndLevel(Entities.ClientProject clientProject, Int64 chartId, int levelNumber);
        IEnumerable<Entities.ChartMoreInfo> GetChartMoreInfoByChartId(int clientProjectId, Int64 chartId);

        IEnumerable<Entities.ChartProduction> GetChartProductionByChartMoreInfoId(int clientProjectId, Int64 chartMoreInfoId, int levelNumber);
        
        IEnumerable<Entities.ChartAudit> GetChartAuditByChartId(Int64 chartId);
        void InsertChartAudit(Entities.ChartAudit chartAudit);
        DataSet GetChartAudits(int clientProjectId, DateTime? fromDate, DateTime? toDate);
        void UpdateL3ChartAuditLevelDisagreement(int clientProjectId, string xmlData);

        System.Data.DataSet GetChartInfoByOverallStatus(int clientProjectId, Helper.ChartDateTypes chartDateType, DateTime fromDate, DateTime toDate, string overallStatus, int? levelNumber, int? levelStatusId);
        System.Data.DataSet GetChartInfoForInvoice(int clientProjectId, DateTime fromDate, DateTime toDate, Boolean isCompleted);
        //System.Data.DataSet GetInfoForUserPerformance(int clientProjectId, DateTime fromDate, DateTime toDate, string username, Int32 levelNumber, Boolean isByDxCode = false);
        System.Data.DataSet GetInfoForUserPerformance(string clientProjectId, DateTime fromDate, DateTime toDate, string username, Int32 levelNumber, Boolean isByDxCode = false);
        //System.Data.DataSet GetInfoForUserPerformance(int clientProjectId, DateTime fromDate, DateTime toDate, string username, Int32 levelNumber, Boolean isByDxCode = false, Boolean isAdmin=false);
        System.Data.DataSet GetSummaryInfoForAging(int clientProjectId, DateTime fromDate, DateTime toDate);
        System.Data.DataSet GetAgingReportDetails(int clientProjectId, DateTime fromDate, DateTime toDate, Helper.DateRangeType dateRangeType, Boolean isCompleted);
    }
}
