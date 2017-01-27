<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReSetPwd.aspx.cs" Inherits="Account_ReSetPwd" %>

<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--  <link href='http://fonts.googleapis.com/css?family=Montserrat' rel='stylesheet' type='text/css' />--%>
    <%--<link type="text/css" rel="Stylesheet" href="Account.css" />--%>
    <link type="text/css" rel="Stylesheet" href="../Styles/Account.css" />
    <link type="text/css" rel="Stylesheet" href="bootstrap.min.css" />
    <script type="text/javascript" src='https://www.google.com/recaptcha/api.js'></script>
    <style type="text/css">
        /* Popover Body */
        .popover-content
        {
            background-color: #FFFFD1;
        }
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
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js" type="text/JavaScript"></script>
    <script type="text/javascript" language="javascript" src="bootstrap.min.js"></script>
    <script type="text/javascript" src='https://www.google.com/recaptcha/api.js'></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#chgPwd").click(function () {
                var Pwd = $("#Password").val();
                var RePwd = $("#RePassword").val();

                if (Pwd == "") {
                    $('#Password').popover({ 'content': 'Please Enter Password' });
                    $('#Password').popover("show");
                    return false;
                }

                if (RePwd == "") {
                    $('#RePassword').popover("destroy");
                    $('#RePassword').popover({ 'content': 'Please Enter Re-Password' });
                    $('#RePassword').popover("show");
                    return false;
                }

                if (RePwd != Pwd) {
                    $('#RePassword').popover("destroy");
                    $('#RePassword').popover({ 'content': 'Password Does Not Match' });
                    $('#RePassword').popover("show");
                    return false;
                }

                $('#Password').popover("destroy");
                $('#RePassword').popover("destroy");
                return true;
            });

            $('a.modalButton').on('click', function (e) {
                var src = $(this).attr('data-src');
                var height = $(this).attr('data-height') || 300;
                var width = $(this).attr('data-width') || 400;

                $("#myModal iframe").attr({ 'src': src,
                    'height': height,
                    'width': width
                });
            });

            $('#divClose').click(function () {
                $('#dvSupport').modal('hide');
            });
        });
    </script>
</head>
<body class="login">
    <form id="frmLogin" runat="server">
    <div id="loginheader" class="radius">
        <div id="loginlogo">
        </div>
    </div>
    <div class="login radius">
        <div class="lMain  lMainFpwd" id="divForgotPassword" runat="server">
            <div class="lHeader" style="min-height: 125px;">
                <span class="failureNotification">
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
            </div>
            <div class="lBody">
                <p>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" placeholder="Password"
                        ClientIDMode="Static" class="form-control" data-toggle="popover" data-placement="right"
                        data-container="body"></asp:TextBox>
                </p>
                <p>
                    <asp:TextBox ID="RePassword" runat="server" TextMode="Password" placeholder="Confirm Password"
                        class="form-control" ClientIDMode="Static" data-toggle="popover" data-placement="right"
                        data-container="body"></asp:TextBox>
                </p>
                <p>
                    <recaptcha:RecaptchaControl ID="recaptcha" Visible="true" Enabled="true" runat="server"
                        Theme="white" PrivateKey="6Lc64SETAAAAABMwAbUsx2eq-qAnKjPEQ55FZcMU" PublicKey="6Lc64SETAAAAAKFOq1JzNhXtV907CUdTWvKLkop4" />
                </p>
                <br />
                <p>
                    <asp:Button ID="chgPwd" runat="server" CssClass="button" Text="Change Password" ClientIDMode="Static"
                        OnClick="chgPwd_Click" />
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
