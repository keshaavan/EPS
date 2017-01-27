<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: Log In ::</title>
    <link type="text/css" rel="Stylesheet" href="../Styles/Account.css" />
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
     <script src="Login.js" type="text/javascript"></script>
    <%-- <script src="ServiceBroker.js" type="text/javascript"></script>--%>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            Login.initialise();

        });             
    </script>
  <%--  <script type="text/javascript">
        $(document).ready(function () {
            $("#LoginUser_UserName").focus();

            $("#LoginUser_Password").keydown(function (event) {
                if (event.keyCode == 32) {
                    event.preventDefault();
                }
            });

            $("#LoginUser_UserName").keydown(function (event) {
                if (event.keyCode == 32) {
                    event.preventDefault();
                }
            });

            $(".water").each(function () {
                $tb = $(this);
                if ($tb.val() != this.title) {
                    $tb.removeClass("water");
                }
            });

            $(".water").focus(function () {
                $tb = $(this);
                if ($tb.val() == this.title) {
                    $tb.val("");
                    $tb.removeClass("water");
                }
            });

            $(".water").blur(function () {
                $tb = $(this);
                if ($.trim($tb.val()) == '') {
                    $tb.val(this.title);
                    $tb.addClass("water");
                }
            });

             Login.initialise();

            //            $("#LoginUser_ClientDropdownList").change(function (id) {
            //                //alert('hi');                
            //                $('#LoginUser_ProjectDropdownList').removeAttr('disabled');
            //                //var Id = $('option:selected', this).val();
            //                //var param = "{id:" + Id + " }";

            //            });

            //            $("#LoginUser_ProjectDropdownList").change(function (id) {
            //                //alert('hi');                
            //                $('#LoginUser_QueueDropdownList').removeAttr('disabled');
            //                //var Id = $('option:selected', this).val();
            //                //var param = "{id:" + Id + " }";

            //            });

        });
    </script>--%>
    <style type="text/css">
        .water
        {
            color: Gray;
        }
    </style>
</head>
<body class="login">
    <form id="frmLogin" runat="server">
    <div id="loginheader" class="radius">
        <div id="loginlogo">
        </div>
        <!--Login Logo-->
    </div>
    <!--Login Header-->
    <div class="login radius">
        <h2>
            Log In<asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false"
                CssClass="registernewuser">Register New User</asp:HyperLink>
        </h2>
        <%-- <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false"
            OnInit="LoginUser_Init" OnLoggingIn="LoginUser_LoggingIn" OnLoggedIn="LoginUser_LoggedIn">
            <LayoutTemplate>--%>
        <span class="failureNotification">
            <asp:Literal ID="FailureText" runat="server"></asp:Literal>
        </span>
        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
            ValidationGroup="LoginUserValidationGroup" />
        <div class="accountInfo">
            <fieldset class="login">
                <p>
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                    <asp:TextBox ID="UserName" runat="server" MaxLength="20" CssClass="textEntry" TabIndex="1"
                        placeholder="ESPLXXXX" onclick="this.value='';"></asp:TextBox><asp:RequiredFieldValidator
                            ControlToValidate="UserName" CssClass="failureNotification" Display="Dynamic"
                            ErrorMessage="User Name is required." ID="UserNameRequired" runat="server" ToolTip="User Name is required."
                            ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                </p>
                <p>
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                    <asp:TextBox ID="Password" TabIndex="2" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                        ID="PasswordRequired" runat="server" ControlToValidate="Password" CssClass="failureNotification"
                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                </p>
                <p>
                    <asp:Label ID="lblClientProject" runat="server" AssociatedControlID="ClientDropdownList">Client:</asp:Label>
                    <asp:DropDownList ID="ClientDropdownList" TabIndex="3" runat="server" CssClass="register"
                        DataTextField="Name" DataValueField="Id" ValidationGroup="LoginUserValidationGroup"
                        ClientIDMode="Static"><%-- AutoPostBack="true" OnSelectedIndexChanged="ClientDropdownList_SelectedIndexChanged1"--%>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvClientProject" runat="server" ControlToValidate="ClientDropdownList"
                        CssClass="failureNotification" ErrorMessage="Client is required." ToolTip="Client is required."
                        ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                </p>
                <p>
                    <asp:Label ID="lblProject" runat="server" AssociatedControlID="ProjectDropdownList">Project:</asp:Label>
                    <asp:DropDownList ID="ProjectDropdownList" TabIndex="4" runat="server" CssClass="register"
                        DataTextField="Name" DataValueField="Id"  ValidationGroup="LoginUserValidationGroup"
                        ClientIDMode="Static"> 
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvProject" runat="server" ControlToValidate="ProjectDropdownList"
                        CssClass="failureNotification" ErrorMessage="Project is required." ToolTip="Project is required."
                        ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                </p>
                <p>
                    <asp:Label ID="lblQueue" runat="server" AssociatedControlID="QueueDropdownList">Queue:</asp:Label>
                    <asp:DropDownList ID="QueueDropdownList" TabIndex="5" runat="server" CssClass="register"
                        ValidationGroup="LoginUserValidationGroup" ClientIDMode="Static" DataTextField="Name"
                        DataValueField="Id">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvQueue" runat="server" ControlToValidate="QueueDropdownList"
                        CssClass="failureNotification" ErrorMessage="Queue is required." ToolTip="Queue is required."
                        ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                </p>
            </fieldset>
            <a href="FPwd.aspx" class="forgetPwd">Forgot Password</a>
          <%--  <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false"
                OnInit="LoginUser_Init" OnLoggingIn="LoginUser_LoggingIn" OnLoggedIn="LoginUser_LoggedIn">
                <LayoutTemplate>--%>
                    <p class="submitButton">
                        <asp:Button ID="LoginButton" runat="server" TabIndex="4" CssClass="button" CommandName="Login"
                            Text="Log In" OnClick="LoginButton_Click" ValidationGroup="LoginUserValidationGroup"  />
                    </p>
                    </div>
              <%--  </LayoutTemplate>
            </asp:Login>--%>
            <%--<asp:HiddenField ID="hidprojcount" runat="server" ClientIDMode="Static" />--%>
        </div>
    </form>
</body>
</html>
