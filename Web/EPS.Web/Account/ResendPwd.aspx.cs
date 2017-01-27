

#region "Header Files"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Account_ResendPwd : System.Web.UI.Page
{
    #region "Page Load"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { }
    }
    #endregion

    #region "ReSendPwdButton"  
    
    protected void btnResend_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Account/FPwd.aspx", false);
    }
    #endregion
}