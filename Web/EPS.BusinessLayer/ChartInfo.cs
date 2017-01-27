using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Entities;
using EPS.DataLayer;
using EPS.Utilities;
using EPS.Resources;

using System.Data;

namespace EPS.BusinessLayer
{
    public partial class ChartInfo : IDisposable
    {
        ChartInfoManager manager;
        IEnumerable<Entities.ChartInfo> charts = null;

        public ChartInfo()
        {
            manager = new ChartInfoManager();
        }

        public IEnumerable<Entities.ChartInfo> GetChartInfoByEmployee(int clientProjectId, string userName, int levelNumber, bool isCompleted)
        {
            try
            {
                charts = manager.GetChartInfoByEmployee(clientProjectId, userName, levelNumber);

                IEnumerable<Entities.ChartInfo> chartList;

                if (!isCompleted)
                    chartList = charts.Where(a => a.ChartMoreInfo.LevelStatusId == (int)Helper.ChartStatus.Pending || a.ChartMoreInfo.LevelStatusId == (int)Helper.ChartStatus.WorkInProgress).ToList();
                else
                    chartList = charts.Where(a => a.ChartMoreInfo.LevelStatusId != (int)Helper.ChartStatus.Pending && a.ChartMoreInfo.LevelStatusId != (int)Helper.ChartStatus.WorkInProgress).ToList();

                return chartList;
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
               return manager.GetChartAuditComments(ChartMoreInfoId);
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
                return manager.DeleteChartData(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public int UploadCharts(DataSet ds, Int64 loggedOnId, int clientProjectId)
        {
            try
            {
                return manager.UploadCharts(ds, loggedOnId, clientProjectId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int BulkDeallocate(DataSet ds, Int64 loggedOnId, int clientProjectId, Boolean deleteAll = false)
        {
            try
            {
                return manager.BulkDeallocate(ds, loggedOnId, clientProjectId, deleteAll);
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
                return manager.GetChartsPendingCount(clientProjectId, levelNumber);
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
                manager.UpdateChartInfoStatus(chartInfo);
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
                manager.UpdateCommentInfoStatus(chartInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //public void ActiveInactiveUserMapping(Entities.UserMapping usermapping)
        //{
        //    try
        //    {
        //        manager.ActiveInactiveUserMapping(usermapping);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        public void UpdateChartInfoStatus(Entities.ChartInfo chartInfo, Int64 previousLevelUpdatedBy)
        {
            try
            {
                manager.UpdateChartInfoStatus(chartInfo, previousLevelUpdatedBy);
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
                charts = manager.GetWIPChartInfo(userName, clientProjectId, levelNumber, status, fromDate, toDate);

                return charts.Where(a => a.ChartMoreInfo.LevelStatusId != (int)Helper.ChartStatus.Completed);
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
                return manager.GetChartInfoForReallocation(clientProjectId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.ChartInfo> GetChartsForDeallocation(int clientProjectId, DateTime fromDate, DateTime toDate, int levelNumber)
        {
            try
            {
                return manager.GetChartsForDeallocation(clientProjectId, fromDate, toDate, levelNumber);
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
                return manager.GetChartsForDirectUpload(clientProjectId, fromDate, toDate);
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
                var empCharts = GetChartInfoByEmployee(clientProjectId, userName, levelNumber, false);

                if (empCharts.Count() > 0)
                {
                    throw new ApplicationException(ValidationMessages.WIPChartsExist);
                }
                else
                    manager.RequestChart(clientProjectId, levelNumber, userName);
            }
            catch (ApplicationException)
            {
                throw;
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
                if (string.IsNullOrEmpty(clientReference))
                {
                    switch (clientProjectId)
                    {
                        case 3:
                            throw new ApplicationException(ValidationMessages.EmptyClientReference_MMM);
                    }
                }

                var empCharts = GetChartInfoByEmployee(clientProjectId, userName, levelNumber, false);

                if (empCharts.Count() > 0)
                {
                    throw new ApplicationException(ValidationMessages.WIPChartsExist);
                }
                else
                    manager.RequestChartByClientReference(clientProjectId, levelNumber, userName, clientReference);

            }
            catch (ApplicationException)
            {
                throw;
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
                manager.InsertChartAudit(chartAudit);
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
                manager.UpdateChartAudit(chartAuditComments);
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
                manager.DeleteChartAudit(chartAuditComments);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public void DeleteClientAllocatedCharts(int clientProjectId, List<string> charts)
        {
            try
            {
                var sBuilder = new StringBuilder("<ChartInfoData>");
                foreach (var chartId in charts)
                {
                    sBuilder.Append(string.Format("<ChartInfo><Id>{0}</Id></ChartInfo>", chartId));
                }
                sBuilder.Append("</ChartInfoData>");

                manager.DeleteClientAllocatedCharts(clientProjectId, sBuilder.ToString());
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
        //        //var sBuilder = new StringBuilder("<ChartInfoData>");
        //        //foreach (var chartId in charts)
        //        //{
        //        //    sBuilder.Append(string.Format("<ChartInfo><Id>{0}</Id></ChartInfo>", chartId));
        //        //}
        //        //sBuilder.Append("</ChartInfoData>");

        //        manager.GetUserDetails();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        public void DirectUploadCharts(int clientProjectId, string epsSysUserName, List<string> charts)
        {
            try
            {
                var sBuilder = new StringBuilder("<ChartInfoData>");
                foreach (var chartId in charts)
                {
                    sBuilder.Append(string.Format("<ChartInfo><Id>{0}</Id></ChartInfo>", chartId));
                }
                sBuilder.Append("</ChartInfoData>");

                manager.DirectUploadCharts(clientProjectId, epsSysUserName, sBuilder.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            manager = null;
        }

        public Entities.ChartMoreInfo GetChartMoreInfoByChartIdAndLevel(Entities.ClientProject clientProject, Int64 chartId, int levelNumber)
        {
            try
            {
                return manager.GetChartMoreInfoByChartIdAndLevel(clientProject, chartId, levelNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.ChartInfo GetChartInfoById(long chartId)
        {
            try
            {
                return manager.GetChartInfoById(chartId);
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
                return manager.GetChartMoreInfoByChartId(clientProjectId, chartId);
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
                return manager.GetChartMoreInfoByChartMoreId(clientProjectId, chartMoreId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<Entities.ChartAudit> GetChartAuditByChartId(Int64 chartId)
        {
            try
            {
                return manager.GetChartAuditByChartId(chartId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateL3ChartAuditLevelDisagreement(int clientProjectId, List<string> chartAudits)
        {
            try
            {
                var sBuilder = new StringBuilder("<ChartAuditData>");
                foreach (var chartAuditInfo in chartAudits)
                {
                    string[] chartAudit = chartAuditInfo.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    if (chartAudit[1] != "0")
                        sBuilder.Append(string.Format("<ChartAudit><Id>{0}</Id><L3LevelDisagreeId>{1}</L3LevelDisagreeId></ChartAudit>", chartAudit[0], chartAudit[1]));
                    else
                        sBuilder.Append(string.Format("<ChartAudit><Id>{0}</Id></ChartAudit>", chartAudit[0]));
                }

                sBuilder.Append("</ChartAuditData>");

                manager.UpdateL3ChartAuditLevelDisagreement(clientProjectId, sBuilder.ToString());
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
                return manager.GetChartProductionByChartMoreInfoId(clientProjectId, chartMoreInfoId, levelNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region "report functions"
        public DataSet GetChartAudits(int clientProjectId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                return manager.GetChartAudits(clientProjectId, fromDate, toDate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //June14
        public DataSet GetChartFromstatus()
        {
            try
            {
                return manager.GetChartFromstatus();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet GetCharttostatus(int id)
        {
            try
            {
                return manager.GetCharttostatus(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetChartData(int clientprojectid,Helper.ChartDateTypes datatype, DateTime fromDate, DateTime toDate, string username, string fromstatusId)
        {
            try
            {
                return manager.GetChartData(clientprojectid,datatype, fromDate, toDate, username, fromstatusId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetChartInfoByOverallStatus(int clientProjectId, Helper.ChartDateTypes chartDateType, DateTime fromDate, DateTime toDate,
                string overallStatus, int? levelNumber, int? levelStatusId)
        {
            try
            {
                return manager.GetChartInfoByOverallStatus(clientProjectId, chartDateType, fromDate, toDate, overallStatus, levelNumber, levelStatusId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetChartInfoForInvoice(int clientProjectId, DateTime fromDate, DateTime toDate, Boolean isCompleted)
        {
            try
            {
                return manager.GetChartInfoForInvoice(clientProjectId, fromDate, toDate, isCompleted);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public DataSet GetInfoForUserPerformance(string clientProjectId, DateTime fromDate, DateTime toDate, string username, int levelNumber, bool isByDxCode)
        {
            try
            {
                var ds = manager.GetInfoForUserPerformance(clientProjectId, fromDate, toDate, username, levelNumber, isByDxCode);                

                if (levelNumber == 2)
                {
                    ds.Tables[0].Columns.Add(DBResources.col_QualityControl, typeof(System.String));
                }
                else
                {
                    ds.Tables[0].Columns.Add(DBResources.col_QualityControl, typeof(System.Decimal));
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (levelNumber == 2)
                        {
                            dr[DBResources.col_QualityControl] = "NA";
                        }
                        else
                        {
                            if (Convert.ToInt32(dr[DBResources.col_TotalCharts]) > 0)
                            {
                                var num = Convert.ToDecimal(dr[DBResources.col_ErrorChartsCount]) / Convert.ToInt32(dr[DBResources.col_TotalCharts]);
                                dr[DBResources.col_QualityControl] = Convert.ToDecimal(100) - (num * 100);
                            }
                            else
                            {
                                dr[DBResources.col_QualityControl] = 0D;
                            }
                        }
                    }
                }

                return ds;
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
                return manager.GetSummaryInfoForAging(clientProjectId, fromDate, toDate);
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
                return manager.GetAgingReportDetails(clientProjectId, fromDate, toDate, dateRangeType, isCompleted);
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
                return manager.GetUserDetails();
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
                return manager.GetActiveProjectList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        

        #endregion
    }
}
