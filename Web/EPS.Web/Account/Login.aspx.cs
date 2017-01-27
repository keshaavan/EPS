using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Profile;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.Security;

using EPS.BusinessLayer;
using EPS.Resources;

public partial class Account_Login : System.Web.UI.Page
{
    #region Page Events

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /// 'Put user code to initialize the page here
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!IsPostBack)
            {
                string msg = string.Empty;
                if (Request.QueryString["msg"] != null && Request.QueryString["msg"].ToString().Trim().Length > 0)
                    msg = Request.QueryString["msg"].ToString();

                if (msg.Length > 0)
                {
                    Literal errmsg = (Literal)Page.FindControl("FailureText");
                    errmsg.Text = msg.ToString();
                }
                else
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect("~/Default.aspx#Home.aspx");
                    }
                }
                BindClients();
                //Set URL for the 'Register New User' hyperlink with the ReturnUrl querystring, if there is no querystring available then pass the Login page as a querystring
                //if (Request.QueryString["ReturnUrl"] != null && Request.QueryString["ReturnUrl"].ToString().Trim().Length > 0)
                //    RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
                //else
                //    RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode("~/Account/Login.aspx");
            }

            ////Set URL for the 'Register New User' hyperlink with the ReturnUrl querystring, if there is no querystring available then pass the Login page as a querystring
            if (Request.QueryString["ReturnUrl"] != null && Request.QueryString["ReturnUrl"].ToString().Trim().Length > 0)
                RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            else
                RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode("~/Account/Login.aspx");


        }
        catch (System.Threading.ThreadAbortException)
        {
            //Do nothing
        }
        catch (Exception ex)
        {
            ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
        }
    }

    #endregion

    #region LoginUser Events

    /// <summary>
    /// Handles the LoggedIn event of the LoginUser control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    //protected void LoginUser_LoggedIn(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DropDownList ClientDropdownList = (DropDownList)Page.FindControl("ClientDropdownList");

    //        var employeeBLObject = new EmployeeInfo();
    //        var employee = employeeBLObject.GetEmployeeinProject(Convert.ToInt32(ClientDropdownList.SelectedValue), UserName.Text.Trim());


    //        var profileBase = ProfileBase.Create(UserName.Text);

    //        profileBase[DBResources.col_EmployeeId] = employee.Id.ToString();
    //        profileBase[DBResources.col_FirstName] = employee.FirstName;
    //        profileBase[DBResources.col_LastName] = employee.LastName;
    //        profileBase[DBResources.col_UserName] = employee.UserName;
    //        profileBase[DBResources.col_LocationId] = employee.LocationId;

    //        profileBase[DBResources.col_LoggedOnId] = employee.EmployeeLevel.Id;
    //        profileBase[DBResources.col_ClientProjectId] = employee.EmployeeLevel.ClientProjectId;
    //        profileBase[DBResources.col_LevelNumber] = employee.EmployeeLevel.LevelNumber;
    //        profileBase[DBResources.col_IsAdmin] = employee.EmployeeLevel.isAdmin;
    //        profileBase[DBResources.col_IsReportUser] = employee.EmployeeLevel.IsReportUser;

    //        profileBase[DBResources.col_Client] = employee.EmployeeLevel.ClientProject.Client;
    //        profileBase[DBResources.col_Project] = employee.EmployeeLevel.ClientProject.Project;
    //        profileBase[DBResources.col_Queue] = employee.EmployeeLevel.ClientProject.Queue;
    //        profileBase[DBResources.col_QueueKey] = employee.EmployeeLevel.ClientProject.QueueKey;
    //        profileBase[DBResources.col_ClientReferenceLabel] = employee.EmployeeLevel.ClientProject.ClientReferenceLabel;
    //        profileBase[DBResources.col_IsByDxCode] = employee.EmployeeLevel.ClientProject.IsByDxCode;

    //        profileBase[DBResources.col_IsL1Auto] = employee.EmployeeLevel.ClientProject.IsL1Auto;
    //        profileBase[DBResources.col_IsL2Auto] = employee.EmployeeLevel.ClientProject.IsL2Auto;
    //        profileBase[DBResources.col_IsL3Auto] = employee.EmployeeLevel.ClientProject.IsL3Auto;
    //        profileBase[DBResources.col_ClientKey] = employee.EmployeeLevel.ClientProject.ClientKey;
    //        profileBase[DBResources.col_ProjectKey] = employee.EmployeeLevel.ClientProject.ProjectKey;

    //        profileBase.Save();
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
    //    }
    //}
    #endregion

    public void BindClients()
    {
        DropDownList clientDropdownList = (DropDownList)Page.FindControl("ClientDropdownList");
        try
        {
            //if (clientDropdownList.SelectedItem.Value != "")
            //{
            using (var objClientProject = new ClientProject())
            {
                var clientProjects = objClientProject.GetAllClientProjects(true);
                //clientDropdownList.DataSource = (from cp in clientProjects
                //                                 select new { Id = cp.Id, Name = cp.Client + " - " + cp.Project + " - " + cp.Queue }).Distinct().ToList();

                clientDropdownList.DataSource = (from cp in clientProjects
                                                 select new { Id = cp.ClientKey, Name = cp.Client }).Distinct().ToList();
                clientDropdownList.DataBind();
                clientDropdownList.Items.Insert(0, new ListItem(StringMessages.listItem_ChooseProjectClient, ""));
            }
            //}

        }
        catch (Exception)
        {
            throw;
        }
    }


    //protected void LoginUser_Init(object sender, EventArgs e)
    //{
    //    DropDownList clientDropdownList = (DropDownList)Page.FindControl("ClientDropdownList");

    //    DropDownList projectDropdownList = (DropDownList)Page.FindControl("ProjectDropdownList");

    //    try
    //    {
    //        //if (clientDropdownList.SelectedItem.Value != "")
    //        //{
    //            using (var objClientProject = new ClientProject())
    //            {
    //                var clientProjects = objClientProject.GetAllClientProjects(true);
    //                //clientDropdownList.DataSource = (from cp in clientProjects
    //                //                                 select new { Id = cp.Id, Name = cp.Client + " - " + cp.Project + " - " + cp.Queue }).Distinct().ToList();

    //                clientDropdownList.DataSource = (from cp in clientProjects
    //                                                 select new { Id = cp.ClientKey, Name = cp.Client }).Distinct().ToList();
    //                clientDropdownList.DataBind();
    //                clientDropdownList.Items.Insert(0, new ListItem(StringMessages.listItem_ChooseProjectClient, ""));
    //            }
    //        //}

    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    //protected void LoginUser_LoggingIn(object sender, LoginCancelEventArgs e)
    //{
    //    string userName = UserName.Text.Trim();

    //    DropDownList clientDropdownList = (DropDownList)Page.FindControl("QueueDropdownList");

    //    Literal errmsg = (Literal)Page.FindControl("FailureText");

    //    using (var vEmployee = new EmployeeInfo())
    //    {
    //        //Client-project must be chosen
    //        if (string.IsNullOrEmpty(clientDropdownList.SelectedValue))
    //        {
    //            e.Cancel = true;
    //            LoginUser.FailureText = ValidationMessages.RequiredClientProject;
    //            errmsg.Text = ValidationMessages.RequiredClientProject;
    //        }

    //        else
    //        {
    //            var result = vEmployee.GetEmployeeinProject(Convert.ToInt32(clientDropdownList.SelectedValue), userName);

    //            //User does not belong to client-project
    //            if (result == null)
    //            {
    //                e.Cancel = true;
    //                LoginUser.FailureText = ValidationMessages.InvalidAccessClientProject;
    //                errmsg.Text = ValidationMessages.InvalidAccessClientProject;
    //            }
    //        }
    //    }

    //    //Check if the user is already logged on. If so, deny the user from logging on
    //    MembershipUser user = Membership.GetUser(userName, false);
    //    if (user != null && (DateTime.Now.AddMinutes(-(Membership.UserIsOnlineTimeWindow)) < user.LastActivityDate))
    //    {
    //        e.Cancel = true;
    //        FailureText.Text = ValidationMessages.UserAlreadyLoggedOn;
    //        errmsg.Text = ValidationMessages.UserAlreadyLoggedOn;
    //    }
    //}

    protected void ClientDropdownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList clientDropdownList = (DropDownList)Page.FindControl("ClientDropdownList");
        DropDownList projectDropdownList = (DropDownList)Page.FindControl("ProjectDropdownList");
        try
        {
            using (var objClientProject = new ClientProject())
            {
                var clientProjects = objClientProject.GetAllProjects(clientDropdownList.SelectedValue);
                projectDropdownList.DataSource = (from cp in clientProjects
                                                  select new { Id = cp.ProjectKey, Name = cp.Project }).Distinct().ToList();
                projectDropdownList.Enabled = true;
                //clientDropdownList.DataSource = (from cp in clientProjects
                //                                 select new { Id = cp.ClientKey, Name = cp.Client }).Distinct().ToList();                
                projectDropdownList.DataBind();
                projectDropdownList.Items.Insert(0, new ListItem("--Select Project--", ""));
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    //protected void ProjectDropdownList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DropDownList projectDropdownList = (DropDownList)Page.FindControl("ProjectDropdownList");
    //    DropDownList queueDropdownList = (DropDownList)Page.FindControl("DDLQueueList");

    //    try
    //    {
    //        using (var objClientProject = new ClientProject())
    //        {
    //            var clientProjects = objClientProject.GetAllQueues(projectDropdownList.SelectedValue.ToString());
    //            queueDropdownList.DataSource = (from cp in clientProjects
    //                                            select new { Id = cp.Id, Name = cp.Queue }).Distinct().ToList();

    //            queueDropdownList.DataBind();
    //            queueDropdownList.Items.Insert(0, new ListItem("--Select Queues--", ""));
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    //protected void ProjectDropdownList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DropDownList projectDropdownList = (DropDownList)Page.FindControl("ProjectDropdownList");
    //    DropDownList queueDropdownList = (DropDownList)Page.FindControl("QueueDropdownList");

    //    try
    //    {
    //        using (var objClientProject = new ClientProject())
    //        {
    //            var clientProjects = objClientProject.GetAllQueues(projectDropdownList.SelectedValue.ToString());
    //            queueDropdownList.DataSource = (from cp in clientProjects
    //                                            select new { Id = cp.Id, Name = cp.Queue });

    //            queueDropdownList.DataBind();
    //            queueDropdownList.Items.Insert(0, new ListItem("--Select Queues--", ""));
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}



    [WebMethod(EnableSession = true)]
    public static string getClientProjectList(string clientkey)
    {

        try
        {
            if (clientkey != "")
            {
                using (var objClientProject = new ClientProject())
                {
                    var clientProjects = objClientProject.GetAllProjects(clientkey);
                    var lstItem = (from cp in clientProjects
                                   select new { Id = cp.ProjectKey, Name = cp.Project }).Distinct().ToList();
                    var json = JsonConvert.SerializeObject(lstItem).ToString();
                    return json;
                }
            }
            else
            {
                return "";
            }
        }
        catch (Exception)
        {
            throw;
        }

        //try
        //{
        //    //DropDownList projectDropdownList = (DropDownList)Page.FindControl("ProjectDropdownList");
        //    //DropDownList queueDropdownList = (DropDownList)Page.FindControl("QueueDropdownList");
        //    using (var objClientProject = new ClientProject())
        //    {
        //        var clientProjects = objClientProject.GetAllProjects(clientkey);

        //        var lstItem = (from cp in clientProjects
        //                       select new { Id = cp.ProjectKey, Name = cp.Project }).ToList();
        //        var json = JsonConvert.SerializeObject(lstItem).ToString();
        //        return json;
        //    }

        //    //using (var objClientProject = new ClientProject())
        //    //{
        //    //    var dt = objClientProject.GetAllProjects(clientkey);
        //    //    var lstItem = dt.Tables[0].AsEnumerable().Select(p => new List<string> {                                                
        //    //            p.Field<string>("ProjectKey").ToString(),
        //    //            p.Field<string>("Project").ToString()
        //    //    }).ToList();
        //    //    var json = JsonConvert.SerializeObject(lstItem).ToString();
        //    //    return json;
        //    //}

        //}
        //catch (Exception)
        //{
        //    throw;
        //}

       
    }


    [WebMethod(EnableSession = true)]
    public static string getClientProjectQueueList(string projectkey)
    {
        try
        {
            //using (var objClientProject = new ClientProject())
            {
            //    var dt = objClientProject.GetAllQueues(projectkey);
            //    var lstItem = dt.Tables[0].AsEnumerable().Select(p => new List<string> {                                                
            //            p.Field<int>("Id").ToString(),
            //            p.Field<string>("Queue").ToString()
            //    }).ToList();
            //    var json = JsonConvert.SerializeObject(lstItem).ToString();
            //    return json;
                if (projectkey != "")
                {
                    using (var objClientProject = new ClientProject())
                    {
                        var clientProjects = objClientProject.GetAllQueues(projectkey);
                        var lstItem = (from cp in clientProjects
                                       select new { Id = cp.Id, Name = cp.Queue }).Distinct().ToList();
                        var json = JsonConvert.SerializeObject(lstItem).ToString();
                        return json;
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void ClientDropdownList_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DropDownList clientDropdownList = (DropDownList)Page.FindControl("ClientDropdownList");

        DropDownList projectDropdownList = (DropDownList)Page.FindControl("ProjectDropdownList");
        try
        {
            if (clientDropdownList.SelectedValue != "")
            {
                using (var objClientProject = new ClientProject())
                {
                    var clientProjects = objClientProject.GetAllProjects(clientDropdownList.SelectedValue);
                    projectDropdownList.DataSource = (from cp in clientProjects
                                                      select new { Id = cp.ProjectKey, Name = cp.Project }).Distinct().ToList();
                    projectDropdownList.DataBind();
                    projectDropdownList.Items.Insert(0, new ListItem("--Select Project--", ""));
                }
            }
        }
        catch (Exception)
        {
            throw;
        }

        //DropDownList projectDropdownList = (DropDownList)LoginUser.FindControl("ProjectDropdownList");
        //DropDownList queueDropdownList = (DropDownList)Page.FindControl("QueueDropdownList");

        //try
        //{
        //    if (projectDropdownList.SelectedValue != "")
        //    {
        //        using (var objClientProject = new ClientProject())
        //        {
        //            var clientProjects = objClientProject.GetAllQueues(projectDropdownList.SelectedValue.ToString());
        //            queueDropdownList.DataSource = (from cp in clientProjects
        //                                            select new { Id = cp.Id, Name = cp.Queue }).Distinct().ToList();

        //            queueDropdownList.DataBind();
        //            queueDropdownList.Items.Insert(0, new ListItem("--Select Queues--", ""));
        //        }
        //    }
        //}
        //catch (Exception)
        //{
        //    throw;
        //}
    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string userName = UserName.Text.Trim();

        DropDownList clientDropdownList = (DropDownList)Page.FindControl("QueueDropdownList");

        Literal errmsg = (Literal)Page.FindControl("FailureText");

        using (var vEmployee = new EmployeeInfo())
        {
            //Client-project must be chosen
            if (string.IsNullOrEmpty(clientDropdownList.SelectedValue))
            {

                FailureText.Text = ValidationMessages.RequiredClientProject;
                errmsg.Text = ValidationMessages.RequiredClientProject;
            }

            else
            {
                var result = vEmployee.GetEmployeeinProject(Convert.ToInt32(clientDropdownList.SelectedValue), userName);

                //User does not belong to client-project
                if (result == null)
                {
                    FailureText.Text = ValidationMessages.InvalidAccessClientProject;
                    errmsg.Text = ValidationMessages.InvalidAccessClientProject;
                }
            }
        }

        //Check if the user is already logged on. If so, deny the user from logging on
        MembershipUser user = Membership.GetUser(userName, false);
        if (user != null && (DateTime.Now.AddMinutes(-(Membership.UserIsOnlineTimeWindow)) < user.LastActivityDate))
        {
            FailureText.Text = ValidationMessages.UserAlreadyLoggedOn;
            errmsg.Text = ValidationMessages.UserAlreadyLoggedOn;
        }
        else
        {
            CreateUserProfile();
            Response.Redirect("~/Default.aspx#Home.aspx");
        }
    }

    public void CreateUserProfile()
    {
        try
        {
            DropDownList ClientDropdownList = (DropDownList)Page.FindControl("QueueDropdownList");

            var employeeBLObject = new EmployeeInfo();
            var employee = employeeBLObject.GetEmployeeinProject(Convert.ToInt32(ClientDropdownList.SelectedValue), UserName.Text.Trim());


            var profileBase = ProfileBase.Create(UserName.Text);

            profileBase[DBResources.col_EmployeeId] = employee.Id.ToString();
            profileBase[DBResources.col_FirstName] = employee.FirstName;
            profileBase[DBResources.col_LastName] = employee.LastName;
            profileBase[DBResources.col_UserName] = employee.UserName;
            profileBase[DBResources.col_LocationId] = employee.LocationId;

            profileBase[DBResources.col_LoggedOnId] = employee.EmployeeLevel.Id;
            profileBase[DBResources.col_ClientProjectId] = employee.EmployeeLevel.ClientProjectId;
            profileBase[DBResources.col_LevelNumber] = employee.EmployeeLevel.LevelNumber;
            profileBase[DBResources.col_IsAdmin] = employee.EmployeeLevel.isAdmin;
            profileBase[DBResources.col_IsReportUser] = employee.EmployeeLevel.IsReportUser;

            profileBase[DBResources.col_Client] = employee.EmployeeLevel.ClientProject.Client;
            profileBase[DBResources.col_Project] = employee.EmployeeLevel.ClientProject.Project;
            profileBase[DBResources.col_Queue] = employee.EmployeeLevel.ClientProject.Queue;
            profileBase[DBResources.col_QueueKey] = employee.EmployeeLevel.ClientProject.QueueKey;
            profileBase[DBResources.col_ClientReferenceLabel] = employee.EmployeeLevel.ClientProject.ClientReferenceLabel;
            profileBase[DBResources.col_IsByDxCode] = employee.EmployeeLevel.ClientProject.IsByDxCode;

            profileBase[DBResources.col_IsL1Auto] = employee.EmployeeLevel.ClientProject.IsL1Auto;
            profileBase[DBResources.col_IsL2Auto] = employee.EmployeeLevel.ClientProject.IsL2Auto;
            profileBase[DBResources.col_IsL3Auto] = employee.EmployeeLevel.ClientProject.IsL3Auto;
            profileBase[DBResources.col_ClientKey] = employee.EmployeeLevel.ClientProject.ClientKey;
            profileBase[DBResources.col_ProjectKey] = employee.EmployeeLevel.ClientProject.ProjectKey;

            profileBase.Save();
            Session["Profile"] = profileBase;
        }
        catch (Exception ex)
        {
            ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
        }
    }

    //[WebMethod(EnableSession = true)]
    //public static string GetClientsDetails()
    //{
    //    try
    //    {
    //        using (var objClientProject = new ClientProject())
    //        {
    //            var dt = objClientProject.GetAllClients();
    //            var lstItem = dt.Tables[0].AsEnumerable().Select(p => new List<string> {                                                
    //                p.Field<string>("ClientKey").ToString(),
    //                    p.Field<string>("Client").ToString()
    //            }).ToList();
    //            var json = JsonConvert.SerializeObject(lstItem).ToString();
    //            return json;
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}



}