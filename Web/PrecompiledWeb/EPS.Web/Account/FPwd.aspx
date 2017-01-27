<%@ page language="C#" autoeventwireup="true" inherits="Account_FPwd, App_Web_pdssljn0" %>

<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="../Styles/Account.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
     <script type="text/javascript" src='https://www.google.com/recaptcha/api.js'></script>
    <script type="text/javascript">

        function ValidateEmail(email) {
            var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            return expr.test(email);
        };

        $(document).ready(function () {

            $("#SendPwdButton").click(function () {

                var SendPwd = $('#EmailAddress').val();
                if (SendPwd == '') {
                    // $('#EmailAddress').popover({ 'content': 'Please provide Email address.' });
                    // $('#EmailAddress').popover("show");
                    alert("Please Provide Email Address");
                    return false;
                }

                if (!ValidateEmail(SendPwd)) {
                    var popover = $('#EmailAddress').data('bs.popover');
                    //popover.options.content = "Invalid Email address.";
                    //$('#EmailAddress').popover("show");
                    alert("Invalid Email address.");
                    return false;
                }
                $('#EmailAddress').popover("destroy");
                return true;

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
            <div class="lHeader">
                <span class="failureNotification">
                    <asp:Literal ID="lblError" runat="server" EnableViewState="False"></asp:Literal></span>
            </div>
            <div class="lBody">
                <p>
                    Forgot Password?
                </p>
                <p>
                    <asp:TextBox ID="EmailAddress" runat="server" placeholder="Email Address" ClientIDMode="Static"
                        CssClass="textEntry" class="form-control" data-toggle="popover" data-container="body"
                        data-placement="right"></asp:TextBox>
                </p>
                <p>
                    <recaptcha:RecaptchaControl ID="recaptcha" Visible="true" Enabled="true" runat="server"
                        PrivateKey="6Lc64SETAAAAABMwAbUsx2eq-qAnKjPEQ55FZcMU" PublicKey="6Lc64SETAAAAAKFOq1JzNhXtV907CUdTWvKLkop4"
                        Theme="white" />
                </p>
                <br />
                <p>
                    <asp:Button ID="SendPwdButton" CssClass="button" runat="server" Text="Send Password Reset Instructions"
                        Style="width: 220px;" OnClick="SendPwdButton_Click" ClientIDMode="Static" />
                    <asp:Button ID="btnCancel" CssClass="button" runat="server" Text="Cancel" ClientIDMode="Static"
                        OnClick="btnCancel_Click" />
                </p>
                <p>
                </p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
