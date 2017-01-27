<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Account_Register"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <link type="text/css" rel="Stylesheet" href="../Styles/Account.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript">
        function ValidateClient(sender, args) {
            var project = document.getElementById('RegisterUser_CreateUserStepContainer_Project').value;

            if (project == '0' && project == 'Choose a Project' && project == '') {
                args.IsValid = false;  // field is empty 
            }
            else {
                args.IsValid = true;
            }
        }


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

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#RegisterUser_CreateUserStepContainer_Password").keydown(function (event) {
                if (event.keyCode == 32) {
                    event.preventDefault();
                }
            });

            $("#RegisterUser_CreateUserStepContainer_ConfirmPassword").keydown(function (event) {
                if (event.keyCode == 32) {
                    event.preventDefault();
                }
            });
            $("#RegisterUser_CreateUserStepContainer_UserName").keydown(function (event) {
                if (event.keyCode == 32) {
                    event.preventDefault();
                }
            });
            $("#RegisterUser_CreateUserStepContainer_UserName").keydown(function (event) {
                if (event.keyCode == 32) {
                    event.preventDefault();
                }
            });
            $('#RegisterUser_CreateUserStepContainer_UserName').keypress(function (e) {
                if (String.fromCharCode(e.keyCode).match(/^\s*[a-zA-Z0-9,\s]+\s*$/))
                    return true;
                alert('No special characters allowed');
                return false;
            });

            $('#RegisterUser_CreateUserStepContainer_FirstName').keypress(function (e) {
                if (String.fromCharCode(e.keyCode).match(/^\s*[a-zA-Z0-9,\s]+\s*$/))
                    return true;

                alert('No special characters allowed');
                return false;
            });

            $('#RegisterUser_CreateUserStepContainer_Lastname').keypress(function (e) {
                if (String.fromCharCode(e.keyCode).match(/^\s*[a-zA-Z0-9,\s]+\s*$/))
                    return true;

                alert('No special characters allowed');
                return false;
            });
        });
    </script>
    <style type="text/css">
        .water
        {
            color: Gray;
        }
    </style>
</head>
<body class="login">
    <form id="frmRegister" runat="server">
    <div id="loginheader" class="radius">
        <div id="loginlogo">
        </div>
        <!--Register Logo-->
    </div>
    <!--Register Header-->
    <div class="login radius">
        <asp:CreateUserWizard ID="RegisterUser" runat="server" OnCreatedUser="RegisterUser_CreatedUser"
            OnInit="RegisterUser_Init" OnCreatingUser="RegisterUser_CreatingUser" OnPreRender="RegisterUser_PreRender"
            PasswordHintText="Password must be at least 8 characters long, contain at least one one lower case letter, one upper case letter, one digit and one special character."
            PasswordRegularExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+=]).*$"
            InvalidPasswordErrorMessage="Your password must be atleast 8 characters long, and contain atleast one number and one special character as well as one UPPER case letter"
            DuplicateUserNameErrorMessage="The user has already registered.">
            <LayoutTemplate>
                <asp:PlaceHolder ID="wizardStepPlaceholder" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="navigationPlaceholder" runat="server"></asp:PlaceHolder>
            </LayoutTemplate>
            <WizardSteps>
                <asp:CreateUserWizardStep ID="RegisterUserWizardStep" runat="server">
                    <ContentTemplate>
                        <h2>
                            Create a New Account
                        </h2>
                        <span class="failureNotification">
                            <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="false"></asp:Literal>
                        </span>
                        <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="failureNotification"
                            ValidationGroup="RegisterUserValidationGroup" />
                        <div class="accountInfo">
                            <fieldset class="register">
                                <p>
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                    <asp:TextBox ID="UserName" runat="server" MaxLength="20" CssClass="textEntry" TabIndex="1"
                                        placeholder="ESPLXXXX" onclick="this.value='';"></asp:TextBox><asp:RequiredFieldValidator
                                            ControlToValidate="UserName" CssClass="failureNotification" Display="Dynamic"
                                            ErrorMessage="User Name is required." ID="UserNameRequired" runat="server" ToolTip="User Name is required."
                                            ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">First Name:</asp:Label>
                                    <asp:TextBox ID="FirstName" TabIndex="2" runat="server" MaxLength="20" CssClass="textEntry"
                                        placeholder="First Name" onclick="this.value='';"></asp:TextBox><asp:RequiredFieldValidator
                                            ControlToValidate="FirstName" CssClass="failureNotification" Display="Dynamic"
                                            ErrorMessage="First Name is required." ID="FirstNameRequired" runat="server"
                                            ToolTip="First Name is required." ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">Last Name:</asp:Label>
                                    <asp:TextBox ID="LastName" TabIndex="3" runat="server" MaxLength="20" CssClass="textEntry"
                                        placeholder="Last Name" onclick="this.value='';"></asp:TextBox><asp:RequiredFieldValidator
                                            ControlToValidate="LastName" CssClass="failureNotification" Display="Dynamic"
                                            ErrorMessage="Last Name is required." ID="RequiredFieldValidator1" runat="server"
                                            ToolTip="Last Name is required." ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                    <asp:TextBox ID="Email" TabIndex="4" runat="server" MaxLength="256" CssClass="textEntry"></asp:TextBox><asp:CustomValidator
                                        ID="EmailCustomValidator" runat="server" ControlToValidate="Email" Display="Dynamic"
                                        Text="*" CssClass="failureNotification" SetFocusOnError="true" ValidateEmptyText="true"
                                        ErrorMessage="Invalid E-mail" ToolTip="Invalid E-mail" ClientValidationFunction="ValidateEmail"
                                        ValidationGroup="RegisterUserValidationGroup" />
                                </p>
                                <p>
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                    <asp:TextBox ID="Password" TabIndex="5" runat="server" EnableViewState="true" MaxLength="128"
                                        CssClass="passwordEntry" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                                            ControlToValidate="Password" CssClass="failureNotification" Display="Dynamic"
                                            ErrorMessage="Password is required." ID="PasswordRequired" runat="server" ToolTip="Password is required."
                                            ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="passwordRegex" runat="server" ControlToValidate="Password"
                                        ErrorMessage="Your password must be atleast 8 characters long, and contain atleast one number and one special character as well as one UPPER case letter"
                                        SetFocusOnError="true" CssClass="failureNotification" Display="Dynamic" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+=]).*$"
                                        ValidationGroup="RegisterUserValidationGroup">*</asp:RegularExpressionValidator>
                                </p>
                                <p>
                                    <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                    <asp:TextBox ID="ConfirmPassword" TabIndex="6" runat="server" EnableViewState="true"
                                        MaxLength="128" CssClass="passwordEntry" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                                            ControlToValidate="ConfirmPassword" CssClass="failureNotification" Display="Dynamic"
                                            ErrorMessage="Confirm Password is required." ID="ConfirmPasswordRequired" runat="server"
                                            ToolTip="Confirm Password is required." ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator><asp:CompareValidator
                                                ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                                                ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                                </p>
                                <p>
                                    <asp:Label ID="ProjectLabel" runat="server" AssociatedControlID="Project">Project:</asp:Label>
                                    <asp:DropDownList ID="Project" TabIndex="7" runat="server" CssClass="register" DataValueField="id"
                                        DataTextField="ProjectName" AutoPostBack="false">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvProject" runat="server" CssClass="failureNotification"
                                        ErrorMessage="Project is required." InitialValue="0" ControlToValidate="Project"
                                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <asp:Label ID="LocationLabel" runat="server" AssociatedControlID="Location">Location:</asp:Label>
                                    <asp:DropDownList ID="Location" TabIndex="8" runat="server" CssClass="register" AutoPostBack="false"
                                        DataTextField="Name" DataValueField="Id">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="LocationCompareValidator" runat="server" CssClass="failureNotification"
                                        ErrorMessage="Location is required." ValueToCompare="0" ControlToValidate="Location"
                                        Operator="NotEqual" ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                                </p>
                            </fieldset>
                            <p class="submitButton">
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="CreateUserButton" runat="server" TabIndex="9" CommandName="MoveNext"
                                                    Text="Create User" CssClass="button" ValidationGroup="RegisterUserValidationGroup" />
                                            </td>
                                            <td>
                                                <asp:Button ID="CancelPushButton" TabIndex="10" runat="server" PostBackUrl="~/Account/Login.aspx"
                                                    CausesValidation="False" CssClass="button" CommandName="Cancel" Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </p>
                        </div>
                    </ContentTemplate>
                    <CustomNavigationTemplate>
                    </CustomNavigationTemplate>
                </asp:CreateUserWizardStep>
            </WizardSteps>
        </asp:CreateUserWizard>
    </div>
    <!--Register Content-->
    </form>
</body>
</html>
