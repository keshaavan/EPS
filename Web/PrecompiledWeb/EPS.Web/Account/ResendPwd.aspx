

<%@ page language="C#" autoeventwireup="true" inherits="Account_ResendPwd, App_Web_pdssljn0" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>  
    <%--<link type="text/css" rel="Stylesheet" href="bootstrap.min.css" />
    <link type="text/css" rel="Stylesheet" href="Account.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js" type="text/JavaScript"></script>
    <script type="text/javascript" language="javascript" src="bootstrap.min.js"></script>--%>

    <link type="text/css" rel="Stylesheet" href="../Styles/Account.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#divClose').click(function () {
                $('#dvSupport').modal('hide');
            });
        });
    </script>

     <style type="text/css">
       
        .button
        {
            font-size: 12px;
            font-weight: bold;
            width: 300px;
            height: 23px;
            background: #016EBC;
            border-style: groove;
            border-width: 0px;
            color: #ffffff;
            cursor: pointer;
        }
    </style>
    </head>
<body class="login">
    <form id="frmLogin" runat="server">
     <div id="loginheader" class="radius">
        <div id="loginlogo">
        </div>
    </div>
    <div class="login radius">
    <div class="lMain">
        <div class="lHeader">            
        </div>
        <div class="lBody">
            <p>
                <b>We have sent you an email with a link
                    <br />
                    to reset your password.</b>
            </p>
            <p>
                An email will only be sent if the email you provided is associated to an EPS
                account. If you don't receive an email please ensure you are providing us with the
                correct email address.
            </p>
            <p>
                <asp:Button ID="btnResend" CssClass="button" runat="server" Text="Resend Password Reset Instructions"
                    ClientIDMode="Static" onclick="btnResend_Click"  />
            </p>
            <p>
                <a href="Login.aspx">Login</a> 
            </p>
        </div>
    </div>
  </div>
    </form>
</body>
</html>
