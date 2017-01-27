<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserCreation.aspx.cs" Inherits="Admin_UserCreation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        UserCreation.initialise();

    });
             
</script>
<form id="frmUserCreation" runat="server">
<div id="EmployeeinfoContainer">
    <table id="tblEmployeeinfo" cellpadding="0" cellspacing="0">
        <thead id="thPendingCharts">
            <tr>
                <th class="leftAlign">
                    Action
                </th>
                <th class="leftAlign">
                    Password
                </th>
                <th class="leftAlign">
                    Employee Id
                </th>
                <th class="leftAlign">
                    User Name
                </th>
                <th class="leftAlign">
                    First Name
                </th>
                <th class="leftAlign">
                    Last Name
                </th>
                <th class="leftAlign">
                    Project
                </th>
               <%-- <th class="leftAlign">
                    Project Status
                </th>--%>
                <th class="leftAlign">
                    Queue
                </th>
                <th class="leftAlign">
                    Location
                </th>
                <th class="centerAlign">
                    Level Number
                </th>
                <th class="centerAlign">
                    Is Admin?
                </th>
                <th class="centerAlign">
                    Is Report User?
                </th>
                <th class="centerAlign">
                    User Status
                </th>
                <th class="leftAlign">
                    Employee Id
                </th>
                <th class="leftAlign">
                    Location Id
                </th>
                <th class="leftAlign">
                    Locked?
                </th>
            </tr>
        </thead>
        <tbody id="trPendingCharts">
        </tbody>
    </table>
</div>
<div id="editbox" class="toggler">
    <div id="effect" class="ui-widget-content ui-corner-all">
        <table cellpadding="0px" cellspacing="10px" style="height: 300px; width: 950px;">
            <tr>
                <td align="right" style="height: 5px; width: 10%;">
                </td>
                <td align="left" style="height: 5px; width: 20%;">
                    <input type="hidden" placeholder="" name="Employeeid" id="Employeeid" class="text ui-widget-content ui-corner-all" />
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 5px; width: 10%;">
                    User Name
                </td>
                <td align="left" style="height: 5px; width: 20%;">
                    <input type="text" placeholder="" name="UserName" id="UserName" maxlength="25" class="text ui-widget-content ui-corner-all"
                        readonly />
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    First Name
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input type="text" placeholder="Enter First Name" maxlength="25" name="FirstName"
                        id="FirstName" class="text ui-widget-content ui-corner-all " tabindex="1" />
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Last Name
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input type="text" placeholder="Enter Last Name" tabindex="2" name="LastName" id="LastName"
                        class="text ui-widget-content ui-corner-all" />
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Project
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <select id="ClientProjectdrp" data-placeholder="Choose a Project" class="chzn-select-deselect"
                        style="width: 340px;" tabindex="3">
                        <option value=""></option>
                    </select>
                    <script type="text/javascript">
                        $(".chzn-select").chosen();
                        $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
                    </script>
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Location
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <select id="Locationdrp" data-placeholder="Choose a Location" class="chzn-select-deselect"
                        style="width: 340px;" tabindex="4">
                        <option value=""></option>
                    </select>
                    <script type="text/javascript">
                        $(".chzn-select").chosen();
                        $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
                    </script>
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Level Number
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <select id="LevelNumberdrp" data-placeholder="Choose a Level" name="se" class="chzn-select-deselect"
                        style="width: 340px;" tabindex="5">
                        <option value=""></option>
                        <option value="1">Level 1</option>
                        <option value="2">Level 2</option>
                        <option value="3">Level 3</option>
                    </select>
                    <script type="text/javascript">
                        $(".chzn-select").chosen();
                        $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
                    </script>
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Is Admin?
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input id="chkIsAdmin" name="ChkisAdmin" type="checkbox" tabindex="6" />
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Is Report User?
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input id="chkIsReportUser" name="chkIsReportUser" type="checkbox" tabindex="7" />
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Is Active?
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input id="chkIsActive" name="ChkStatus" type="checkbox" tabindex="8" />
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 20px; width: 10%;">
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input id="btnUpdate" type="button" class="buttons buttons-gray" tabindex="9" value="Update"
                        name="addNew" />
                    <input id="btnReset" type="button" class="buttons buttons-gray" tabindex="10" value="Cancel" />
                    <input id="btnUnlockAccount" type="button" class="buttons buttons-gray" tabindex="11"
                        name="btnUnlockAccount" value="Unlock Account" />
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="dvResetPassword" class="divPasswordReset">
    <div id="Top" class="DivTop">
        <h3>
            Password Reset</h3>
    </div>
    <table cellpadding="0px" cellspacing="10px" style="height: 120px; width: 500px;">
        <tr>
            <td align="right" style="height: 5px; width: 10%;">
            </td>
            <td align="left" style="height: 5px; width: 20%;">
                <input type="hidden" placeholder="" name="Employeeid" id="txtemployeeid" />
            </td>
        </tr>
        <tr>
            <td align="left" style="height: 5px; width: 20%;">
                User Name
            </td>
            <td align="left" style="height: 5px; width: 50%;">
                <input type="text" placeholder="" name="User Name" id="txtusername" maxlength="25"
                    class="text ui-widget-content ui-corner-all" readonly />
            </td>
        </tr>
        <tr>
            <td align="left" style="height: 20px; width: 20%;">
                New Password
            </td>
            <td align="left" style="height: 20px; width: 50%;">
                <input type="password" placeholder="Enter New Password" tabindex="2" name="newpassword"
                    id="txtpassword" class="text ui-widget-content ui-corner-all" />
            </td>
        </tr>
        <tr>
            <td align="left" style="height: 20px; width: 20%;">
            </td>
            <td align="left" style="height: 20px; width: 50%;">
                <input id="btnResetpassword" type="button" class="buttons buttons-gray" tabindex="3"
                    value="Update" />
                <input id="btnCancel" type="button" class="buttons buttons-gray" tabindex="4" value="Cancel" />
            </td>
        </tr>
    </table>
</div>
</form>
