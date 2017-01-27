using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Services;
using System.Data;
using Newtonsoft.Json;

using EPS.BusinessLayer;
using EPS.Resources;

public partial class Account_Register : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                TextBox UserNameText = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("UserName");
                UserNameText.Focus();
                TextBox FirstName = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("FirstName");
                //Set querystring value into the ContinueDestinationPageUrl of RegisterUser
                if (Request.QueryString["ReturnUrl"] != null)
                    RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
        }
    }

    #endregion

    #region CreateUserWizard Events

    protected void RegisterUser_Init(object sender, EventArgs e)
    {
        try
        {
            using (var clientProjectObject = new ClientProject())
            {
                var clientProjects = clientProjectObject.GetAllClientProjects(true);

                DropDownList projectDropdownList = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Project");

                if (projectDropdownList != null)
                {
                    projectDropdownList.DataSource = (from cp in clientProjects
                                                      select new { Id = cp.Id, ProjectName = cp.Client + " - " + cp.Project + " - " + cp.Queue}).ToList();
                    projectDropdownList.DataBind();
                    projectDropdownList.Items.Insert(0, new ListItem(StringMessages.listItem_ChooseProject, "0"));
                }
            }

            using (var loookupBL = new Lookup())
            {
                var locations = loookupBL.GetLookupByCategory(null, StringMessages.lookup_Location).Where(l => l.IsActive == true);
                DropDownList locationDropdownList = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Location");

                locationDropdownList.DataSource = (from l in locations
                                                   select new { Id = l.Id, Name = l.Name }).ToList();
                locationDropdownList.DataBind();
                locationDropdownList.Items.Insert(0, new ListItem(StringMessages.listItem_SelectOne, "0"));
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void RegisterUser_PreRender(object sender, EventArgs e)
    {
        try
        {
            TextBox passwordText = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Password");
            TextBox confirmPassword = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("ConfirmPassword");
            DropDownList projectDropdownList = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Project");
            DropDownList locationDropdownList = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Location");

            if (passwordText != null)
            {
                string password = passwordText.Text;
                passwordText.Attributes["value"] = password;
            }
            if (confirmPassword != null)
            {
                string objPassword = confirmPassword.Text;
                confirmPassword.Attributes["value"] = objPassword;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void RegisterUser_CreatingUser(object sender, LoginCancelEventArgs e)
    {
        try
        {
            TextBox FirstName = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("FirstName");
            TextBox LastName = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("LastName");

            DropDownList projectDropdownList = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Project");
            DropDownList locationDropdownList = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Location");

            //Validate the Find Controls instance
            if (FirstName == null || LastName == null || projectDropdownList == null)
            {
                e.Cancel = true;
                return;
            }

            //Verify that the project and location has been selected
            if (projectDropdownList.SelectedIndex == 0 || projectDropdownList.SelectedIndex == -1 || string.IsNullOrEmpty(projectDropdownList.SelectedValue))
            {
                Literal errmsg = (Literal)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("ErrorMessage");
                e.Cancel = true;
                return;
            }

            if (locationDropdownList.SelectedIndex == 0 || locationDropdownList.SelectedIndex == -1 || string.IsNullOrEmpty(locationDropdownList.SelectedValue))
            {
                e.Cancel = true;
                return;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Handles the CreatedUser event of the RegisterUser control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected void RegisterUser_CreatedUser(object sender, EventArgs e)
    {
        try
        {
            //If UserName of RegisterUser is not empty then add role for the UserName in MemberShip and add username, rolename, client, displayname in EPS users table            
            if (!string.IsNullOrEmpty(RegisterUser.UserName))
            {
                MembershipUser user = Membership.GetUser(RegisterUser.UserName);
                Guid userid = (Guid)user.ProviderUserKey;

                user.LastActivityDate = DateTime.UtcNow.AddMinutes(-(Membership.UserIsOnlineTimeWindow + 1));
                Membership.UpdateUser(user);

                TextBox FirstName = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("FirstName");
                TextBox LastName = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("LastName");

                DropDownList projectDropdownList = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Project");
                DropDownList locationDropdownList = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Location");

                using (var employeeBLObject = new EmployeeInfo())
                {
                    var employee = new EPS.Entities.Employee()
                    {
                        Id = userid,
                        FirstName = FirstName.Text.Trim(),
                        LastName = LastName.Text.Trim(),

                        LocationId = Convert.ToInt32(locationDropdownList.SelectedValue),
                        isActive = true,
                        EmployeeLevel = new EPS.Entities.EmployeeLevel()
                        {
                            EmployeeId = userid,
                            LevelNumber = 1,
                            ClientProjectId = Convert.ToInt32(projectDropdownList.SelectedValue),
                            isActive = false,
                            isAdmin = false
                        }
                    };

                    employeeBLObject.InsertUpdateEmployee(employee);
                }

                //Navigate to next page after successfully created user
                string continueUrl = RegisterUser.ContinueDestinationPageUrl;

                if (String.IsNullOrEmpty(continueUrl))
                {
                    continueUrl = "~/Account/Login.aspx";
                }
                Response.Redirect(continueUrl, true);
            }
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
}