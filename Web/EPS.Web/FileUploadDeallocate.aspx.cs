using System;
using System.Web;
using System.Data;
using System.Text;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using System.Web.Security;
using System.Web.Script.Services;

using System.Data.Entity;
using System.Data.SqlClient;

using EPS.Utilities;
using EPS.Resources;
using EPS.Entities;

public partial class FileUploadDeallocate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblNoteDeallocation.Text = string.Format("Please limit your upload filesize to {0} charts.", ConfigurationHelper.BulkUploadForDeallocationLimit);
    }

    protected void btnDeallocate_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsUserAuthorized)
            {
                if (fileUploadDeallocate.HasFile)
                {
                    string filename = Server.MapPath("UploadedFiles") + "\\" + fileUploadDeallocate.FileName;

                    string fileExtn = filename.Substring(filename.Length - 5);

                    if (fileExtn == ".xlsx" || fileExtn.Substring(fileExtn.Length - 4) == ".xls")
                    {
                        if (System.IO.File.Exists(filename))
                            System.IO.File.Delete(filename);

                        fileUploadDeallocate.SaveAs(filename);
                        UploadExcel(filename);
                    }
                    else
                    {
                        lblSchema.Text = ValidationMessages.InvalidFileFormatForUpload;
                        lblSchema.Visible = true;
                        lblSchema.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblSchema.Text = ValidationMessages.NoExcelFileSelected;
                    lblSchema.Visible = true;
                    lblSchema.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblSchema.Text = ValidationMessages.InvalidAccessToUploadPage;
                lblSchema.Visible = true;
                lblSchema.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void UploadExcel(string fileName)
    {
        try
        {
            lblSchema.Visible = false;
            lblSchema.Text = String.Empty;

            DataSet ds = new DataSet();
            StringBuilder errString = new StringBuilder();
            ds = clsExcelData.ReadExcel(fileName, "Sheet1");

            if (ds == null)
            {
                lblSchema.Text = ValidationMessages.InvalidUploadFileSheetName;
                lblSchema.Visible = true;
                lblSchema.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                pnlLoading.Visible = true;

                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                ds.DataSetName = "ImportData";
                ds.Tables[0].Columns.Add(new DataColumn(DBResources.col_ClientProjectId, typeof(int)));
                ds.Tables[0].AsEnumerable().ToList().ForEach(p => p.SetField<int>(DBResources.col_ClientProjectId, profile.ClientProjectId));
                ds.Tables[0].TableName = DBResources.bc_dest_EPS_Stage_BulkDelete;

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.DataType == typeof(DateTime) && dc.DateTimeMode == DataSetDateTime.UnspecifiedLocal)
                            dc.DateTimeMode = DataSetDateTime.Unspecified;
                    }
                }

                var importCount = ds.Tables[0].Rows.Count;
                if (importCount <= ConfigurationHelper.BulkUploadForDeallocationLimit)
                {
                    var chartInfoBL = new EPS.BusinessLayer.ChartInfo();
                    var recCount = chartInfoBL.BulkDeallocate(ds, profile.LoggedOnId, profile.ClientProjectId, chkRemoveAll.Checked);

                    lblSchema.Text = string.Format(ValidationMessages.FileDeallocatedSuccessfully, importCount, recCount);
                    lblSchema.Visible = true;
                    lblSchema.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblSchema.Text = string.Format(ValidationMessages.ExceededBulkUploadLimit, ConfigurationHelper.BulkUploadForDeallocationLimit);
                    lblSchema.Visible = true;
                    lblSchema.ForeColor = System.Drawing.Color.Red;
                }

                pnlLoading.Visible = false;
            }
        }
        catch (Exception)
        {
            pnlLoading.Visible = false;

            lblSchema.Text = ValidationMessages.InvalidDataExistsInUpload;
            lblSchema.Visible = true;
            lblSchema.ForeColor = System.Drawing.Color.Red;
        }
    }

    public Boolean IsUserAuthorized
    {
        get
        {
            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                return false;

            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            if (profile != null && !profile.IsAdmin)
                return false;

            MembershipUser User = Membership.GetUser(profile.UserName, true);
            if (User.LastPasswordChangedDate < DateTime.Now.AddDays(EPS.Utilities.ConfigurationHelper.PasswordExpiryPeriod))
                return false;

            return true;
        }
    }
}