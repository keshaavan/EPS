using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Interfaces;
using EPS.Resources;
using EPS.Utilities;

namespace EPS.DataLayer
{
    public partial class ChartInfoManager : IChartInfo, IDisposable
    {
        ChartInfo chartInfoObject;

        public ChartInfoManager()
        {
            chartInfoObject = new ChartInfo();
        }

        public Entities.ChartInfo GetChartInfoById(Int64 id)
        {
            try
            {
                var chartInfo = chartInfoObject.GetChartInfoById(id);

                if (chartInfo.ClientProjectId > 0)
                {
                    var clientProjectManager = new ClientProjectManager();
                    chartInfo.ClientProject = clientProjectManager.GetClientProjectById(chartInfo.ClientProjectId);
                }

                return chartInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.ChartMoreInfo GetChartMoreInfoByChartIdAndLevel(Entities.ClientProject clientProject, Int64 chartId, int levelNumber)
        {
            try
            {
                var chartMoreInfo = chartInfoObject.GetChartMoreInfoByChartIdAndLevel(chartId, levelNumber);

                var lookupManager = new LookupManager();
                var commentsManager = new CommentsManager();
                var employeeManager = new EmployeeManager();
                chartMoreInfo.LevelStatus = lookupManager.GetLookupById(null, chartMoreInfo.LevelStatusId);
                chartMoreInfo.LevelStatusComment = commentsManager.GetCommentsById(clientProject.Id, chartMoreInfo.LevelStatusCommentId);
                chartMoreInfo.Employee = employeeManager.GetEmployeeByEmployeeLevelId(clientProject.Id, chartMoreInfo.LevelUpdatedBy);

                return chartMoreInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartMoreInfo> GetChartMoreInfoByChartId(Int32 clientProjectId, long chartId)
        {
            try
            {
                return chartInfoObject.GetChartMoreInfoByChartId(clientProjectId, chartId).OrderBy(ci => ci.LevelNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartMoreInfo> GetChartMoreInfoByChartMoreId(Int32 clientProjectId, long chartMoreId)
        {
            try
            {
                return chartInfoObject.GetChartMoreInfoByChartMoreId(clientProjectId, chartMoreId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<Entities.ChartInfo> GetChartInfoByEmployee(int clientProjectId, string userName, int levelNumber)
        {
            try
            {
                var chartInfoes = chartInfoObject.GetChartInfoByEmployee(clientProjectId, userName, levelNumber).OrderBy(ci => ci.ReceivedDate);

                foreach (var chartInfo in chartInfoes)
                {
                    var clientProjectManager = new ClientProjectManager();
                    chartInfo.ClientProject = clientProjectManager.GetClientProjectById(clientProjectId);

                    chartInfo.ChartMoreInfo = GetChartMoreInfoByChartIdAndLevel(chartInfo.ClientProject, chartInfo.Id, levelNumber);
                }

                return chartInfoes;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<Entities.ChartAuditComments> GetChartAuditComments(string ChartMoreInfoId)
        {
            try
            {
                var chartAudits = chartInfoObject.GetChartAuditComments(ChartMoreInfoId);
                return chartAudits;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartAuditComments> DeleteChartData(int Id)
        {
            try
            {
                var chartAudits = chartInfoObject.DeleteChartData(Id);
                return chartAudits;
            }
            catch (Exception)
            {
                throw;
            }
        }




        public void InsertChartAudit(Entities.ChartAudit chartAudit)
        {
            try
            {
                chartInfoObject.InsertChartAudit(chartAudit);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdateChartAudit(Entities.ChartAuditComments chartAuditComments)
        {
            try
            {
                chartInfoObject.UpdateChartAudit(chartAuditComments);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteChartAudit(Entities.ChartAuditComments chartAuditComments)
        {
            try
            {
                chartInfoObject.DeleteChartAudit(chartAuditComments);
            }
            catch (Exception)
            {
                throw;
            }
        }




        public void UpdateChartInfoStatus(Entities.ChartInfo chartInfo)
        {
            try
            {
                chartInfoObject.UpdateChartInfoStatus(chartInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCommentInfoStatus(Entities.ChartInfo chartInfo)
        {
            try
            {
                chartInfoObject.UpdateCommentInfoStatus(chartInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public void ActiveInactiveUserMapping(int selectedCheckBox, int drpValue, string username)
        {
            try
            {
                chartInfoObject.ActiveInactiveUserMapping(selectedCheckBox, drpValue, username);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdateChartInfoStatus(Entities.ChartInfo chartInfo, Int64 previousLevelUpdatedBy)
        {
            try
            {
                chartInfoObject.UpdateChartInfoStatus(chartInfo, previousLevelUpdatedBy);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UploadCharts(System.Data.DataSet ds, Int64 loggedOnId, int clientProjectId)
        {
            try
            {
                return chartInfoObject.UploadCharts(ds, loggedOnId, clientProjectId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int BulkDeallocate(System.Data.DataSet ds, Int64 loggedOnId, int clientProjectId, Boolean deleteAll)
        {
            try
            {
                return chartInfoObject.BulkDeallocate(ds, loggedOnId, clientProjectId, deleteAll);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetChartsPendingCount(int clientProjectId, int levelNumber)
        {
            try
            {
                return chartInfoObject.GetChartsPendingCount(clientProjectId, levelNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void RequestChart(int clientProjectId, int levelNumber, string userName)
        {
            try
            {
                chartInfoObject.RequestChart(clientProjectId, levelNumber, userName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RequestChartByClientReference(int clientProjectId, int levelNumber, string userName, string clientReference)
        {
            try
            {
                chartInfoObject.RequestChartByClientReference(clientProjectId, levelNumber, userName, clientReference);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartInfo> GetWIPChartInfo(string userName, int clientProjectId, int levelNumber, int status, string fromDate, string toDate)
        {
            try
            {
                var chartInfoes = chartInfoObject.GetWIPChartInfo(userName, clientProjectId, levelNumber, status, fromDate, toDate);
                foreach (var chartInfo in chartInfoes)
                {
                    var clientProjectManager = new ClientProjectManager();
                    chartInfo.ClientProject = clientProjectManager.GetClientProjectById(clientProjectId);
                    chartInfo.ChartMoreInfo = GetChartMoreInfoByChartIdAndLevel(chartInfo.ClientProject, chartInfo.Id, levelNumber);

                    if (levelNumber == 2)
                        chartInfo.PreviousChartMoreInfo = GetChartMoreInfoByChartIdAndLevel(chartInfo.ClientProject, chartInfo.Id, 1);
                }

                return chartInfoes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Entities.ChartInfo> GetChartInfoForReallocation(int clientProjectId)
        {
            try
            {
                var dt = chartInfoObject.GetChartsForReallocation(clientProjectId);

                List<Entities.ChartInfo> chartInfoes = new List<Entities.ChartInfo>();

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    var chartInfo = chartInfoObject.GetChartInfoById(Convert.ToInt64(dr[DBResources.col_Id]));
                    var levelNumber = Convert.ToInt32(dr[DBResources.col_LevelNumber]);
                    var clientProjectManager = new ClientProjectManager();

                    chartInfo.ClientProject = clientProjectManager.GetClientProjectById(clientProjectId);
                    chartInfo.ChartMoreInfo = GetChartMoreInfoByChartIdAndLevel(chartInfo.ClientProject, chartInfo.Id, levelNumber);

                    if (levelNumber > 1)
                        chartInfo.PreviousChartMoreInfo = GetChartMoreInfoByChartIdAndLevel(chartInfo.ClientProject, chartInfo.Id, levelNumber - 1);

                    chartInfoes.Add(chartInfo);
                }

                return chartInfoes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartInfo> GetChartInfoByEmployeeId(int clientProjectId, string userName, int levelNumber)
        {
            try
            {
                var chartInfoes = chartInfoObject.GetChartInfoByEmployee(clientProjectId, userName, levelNumber);

                foreach (var chartInfo in chartInfoes)
                {
                    var clientProjectManager = new ClientProjectManager();
                    chartInfo.ClientProject = clientProjectManager.GetClientProjectById(clientProjectId);
                    chartInfo.ChartMoreInfo = GetChartMoreInfoByChartIdAndLevel(chartInfo.ClientProject, chartInfo.Id, levelNumber);
                }
                return chartInfoes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            chartInfoObject = null;
        }

        public void DeleteClientAllocatedCharts(int clientProjectId, string xmlData)
        {
            try
            {
                chartInfoObject.DeleteClientAllocatedCharts(clientProjectId, xmlData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DirectUploadCharts(int clientProjectId, string epsSysUserName, string xmlData)
        {
            try
            {
                chartInfoObject.DirectUploadCharts(clientProjectId, epsSysUserName, xmlData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public void GetUserDetails()
        //{
        //    try
        //    {
        //        chartInfoObject.GetUserDetails();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        


        public IEnumerable<Entities.ChartInfo> GetChartsForDeallocation(int clientProjectId, DateTime fromDate, DateTime toDate, int levelNumber)
        {
            try
            {
                var chartInfoes = chartInfoObject.GetChartsForDeallocation(clientProjectId, fromDate, toDate, levelNumber);

                foreach (var chartInfo in chartInfoes)
                {
                    var clientProjectManager = new ClientProjectManager();
                    chartInfo.ClientProject = clientProjectManager.GetClientProjectById(clientProjectId);
                    chartInfo.ChartMoreInfo = GetChartMoreInfoByChartIdAndLevel(chartInfo.ClientProject, chartInfo.Id, levelNumber);
                }

                return chartInfoes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartInfo> GetChartsForDirectUpload(int clientProjectId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var chartInfoes = chartInfoObject.GetChartsForDirectUpload(clientProjectId, fromDate, toDate);

                foreach (var chartInfo in chartInfoes)
                {
                    var clientProjectManager = new ClientProjectManager();
                    chartInfo.ClientProject = clientProjectManager.GetClientProjectById(clientProjectId);
                    chartInfo.ChartMoreInfo = GetChartMoreInfoByChartIdAndLevel(chartInfo.ClientProject, chartInfo.Id, 1);
                }

                return chartInfoes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartAudit> GetChartAuditByChartId(long chartId)
        {
            try
            {
                var chartAudits = chartInfoObject.GetChartAuditByChartId(chartId);

                foreach (var chartAudit in chartAudits)
                {
                    var lookupManager = new LookupManager();
                    chartAudit.ErrorCategory = lookupManager.GetLookupById(null, chartAudit.ErrorCategoryId);
                    chartAudit.ErrorSubCategory = lookupManager.GetLookupById(null, chartAudit.ErrorSubCategoryId);

                    chartAudit.ChartMoreInfo = chartInfoObject.GetChartMoreInfoById(chartAudit.ChartMoreInfoId);

                    if (chartAudit.L3LevelDisagreeId.HasValue)
                        chartAudit.L3LevelDisagree = lookupManager.GetLookupById(null, chartAudit.L3LevelDisagreeId.Value);
                }

                return chartAudits;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateL3ChartAuditLevelDisagreement(int clientProjectId, string xmlData)
        {
            try
            {
                chartInfoObject.UpdateL3ChartAuditLevelDisagreement(clientProjectId, xmlData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartProduction> GetChartProductionByChartMoreInfoId(int clientProjectId, long chartMoreInfoId, int levelNumber)
        {
            try
            {
                var chartProductions = chartInfoObject.GetChartProductionByChartMoreInfoId(clientProjectId, chartMoreInfoId, levelNumber);

                foreach (var chartProduction in chartProductions)
                {
                    var lookupManager = new LookupManager();
                    var commentsManager = new CommentsManager();
                    var employeeManager = new EmployeeManager();

                    chartProduction.LevelStatus = lookupManager.GetLookupById(null, chartProduction.LevelStatusId);

                    if (chartProduction.LevelStatusCommentId.HasValue)
                        chartProduction.LevelStatusComment = commentsManager.GetCommentsById(clientProjectId, chartProduction.LevelStatusCommentId.Value);

                    chartProduction.Employee = employeeManager.GetEmployeeByEmployeeLevelId(clientProjectId, chartProduction.LevelUpdatedBy);
                }

                return chartProductions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "report functions"
        public System.Data.DataSet GetChartInfoByOverallStatus(int clientProjectId, Helper.ChartDateTypes chartDateType, DateTime fromDate, DateTime toDate,
            string overallStatus, int? levelNumber, int? levelStatusId)
        {
            try
            {
                return chartInfoObject.GetChartInfoByOverallStatus(clientProjectId, chartDateType, fromDate, toDate, overallStatus, levelNumber, levelStatusId);
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
                return chartInfoObject.GetChartData(clientprojectid,datatype, fromDate, toDate, username, fromstatusId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public System.Data.DataSet GetChartAudits(int clientProjectId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                return chartInfoObject.GetChartAudits(clientProjectId, fromDate, toDate);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //June14
        public System.Data.DataSet GetChartFromstatus()
        {
            try
            {
                return chartInfoObject.GetChartFromstatus();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public System.Data.DataSet GetCharttostatus(int id)
        {
            try
            {
                return chartInfoObject.GetCharttostatus(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public System.Data.DataSet GetChartInfoForInvoice(int clientProjectId, DateTime fromDate, DateTime toDate, Boolean isCompleted)
        {
            try
            {
                return chartInfoObject.GetChartInfoForInvoice(clientProjectId, fromDate, toDate, isCompleted);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public System.Data.DataSet GetInfoForUserPerformance(string clientProjectId, DateTime fromDate, DateTime toDate, string username, int levelNumber, bool isByDxCode = false)        
        
        {
            try
            {
                return chartInfoObject.GetInfoForUserPerformance(clientProjectId, fromDate, toDate, username, levelNumber, isByDxCode);                
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
                return chartInfoObject.GetSummaryInfoForAging(clientProjectId, fromDate, toDate);
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
                return chartInfoObject.GetAgingReportDetails(clientProjectId, fromDate, toDate, dateRangeType, isCompleted);
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
                return chartInfoObject.GetUserDetails();
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
                return chartInfoObject.GetActiveProjectList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        

        #endregion


        //public System.Data.DataSet GetInfoForUserPerformance(int clientProjectId, DateTime fromDate, DateTime toDate, string username, int levelNumber, bool isByDxCode = false)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
