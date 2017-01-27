<%@ page language="C#" autoeventwireup="true" inherits="Admin_ChartReallocation, App_Web_45n1udx5" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        ChartReallocation.initialise();
    });
</script>
<form id="frmChartReallocation" runat="server">
<div class="toggler">
    <div id="ChartReallocationEditBox" class="ui-widget-content ui-corner-all">
        <table cellpadding="0px" cellspacing="10px" style="height: 200px; width: 1000px;
            margin-left: 2px;">
            <tr>
                <td style="height: 5px; width: 10%; text-align: right; vertical-align: middle;">
                    <label id="lblEditClientReferenceHeading" style="font-style: normal; color:Black;" />
                </td>
                <td style="height: 5px; width: 20%; text-align: left; padding-top: 8px;">
                    <input type="text" placeholder="" name="ClientReferenceId" id="ClientReferenceId"
                        class="text ui-widget-content ui-corner-all" readonly="true" />
                </td>
            </tr>
            <tr>
                <td style="height: 20px; width: 10%; text-align: right; vertical-align: middle;">
                    Received Date
                </td>
                <td style="height: 20px; width: 20%; text-align: left; padding-top: 8px;">
                    <input type="text" name="ReceivedDate" id="ReceivedDate" class="text ui-widget-content ui-corner-all "
                        readonly="true" />
                </td>
            </tr>
            <tr>
                <td style="height: 20px; width: 10%; text-align: right; vertical-align: middle;">
                    Reference
                </td>
                <td style="height: 20px; width: 20%; text-align: left; padding-top: 8px;">
                    <input type="text" name="Market" id="Market" class="text ui-widget-content ui-corner-all"
                        readonly="true" />
                </td>
            </tr>
            <tr>
                <td style="height: 20px; width: 10%; text-align: right; vertical-align: middle;">
                    Filename
                </td>
                <td style="height: 20px; width: 20%; text-align: left; padding-top: 8px;">
                    <input type="text" name="FileName" id="FileName" class="text ui-widget-content ui-corner-all"
                        readonly="true" />
                </td>
            </tr>
            <tr>
                <td style="height: 20px; width: 10%; text-align: right; vertical-align: middle;">
                    Client Project Name
                </td>
                <td style="height: 20px; width: 20%; text-align: left; padding-top: 8px;">
                    <input type="text" name="ClientProjectName" id="ClientProjectName" class="text ui-widget-content ui-corner-all"
                        readonly="true" />
                </td>
            </tr>
            <tr>
                <td style="height: 20px; width: 10%; text-align: right; vertical-align: middle;">
                    Status
                </td>
                <td style="height: 20px; width: 20%; text-align: left; padding-top: 8px;">
                    <input type="text" name="Status" id="Status" class="text ui-widget-content ui-corner-all"
                        readonly="true" />
                </td>
            </tr>
            <tr>
                <td style="height: 20px; width: 10%; text-align: right; vertical-align: middle;">
                    Employee
                </td>
                <td style="height: 20px; width: 20%; text-align: left; padding-top: 8px;">
                    <select id="Employeedrp" data-placeholder="Choose an Employee" class="chzn-select-deselect"
                        style="width: 350px;" tabindex="1">
                        <option value=""></option>
                    </select>
                    <script type="text/javascript">
                        $(".chzn-select").chosen();
                        $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
                    </script>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 20px; width: 10%;">
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input id="btnUpdate" type="button" value="Update" name="Update" class="buttons buttons-gray" />
                    <input id="btnReset" type="button" value="Cancel" class="buttons buttons-gray" />
                </td>
            </tr>
        </table>
    </div>
    <div id="ChartReallocationDisplay">
        <table id="tblWIPCharts" cellpadding="0" cellspacing="0" class="display" >
            <thead id="thWIPCharts">
                <tr>
                    <th class="leftAlign">
                        Action
                    </th>
                    <th class="leftAlign">
                        ChartId
                    </th>
                    <th class="leftAlign">
                        <label id="lblClientReferenceHeading" class="labelClient">
                        </label>
                    </th>
                    <th class="leftAlign">
                        Received Date
                    </th>
                    <th class="leftAlign">
                        Reference
                    </th>
                    <th class="leftAlign">
                        Filename
                    </th>
                    <th class="leftAlign">
                        Client-Project Name
                    </th>
                    <th class="leftAlign">
                        Status
                    </th>
                    <th class="leftAlign">
                        ChartMoreInfoId
                    </th>
                    <th class="leftAlign">
                        User Name
                    </th>
                    <th class="leftAlign">
                        Assigned To
                    </th>
                    <th class="leftAlign">
                        EmployeeId
                    </th>
                    <th class="leftAlign">
                        ClientProjectId
                    </th>
                    <th class="leftAlign">
                        Level Number
                    </th>
                    <th class="leftAlign">
                        PrevUserName
                    </th>
                </tr>
            </thead>
            <tbody id="trWIPCharts">
            </tbody>
        </table>
    </div>
    <input type="hidden" id="hdnChartId" />
    <input type="hidden" id="hdnClientProjectId" />
    <input type="hidden" id="hdnLevelNumber" />
    <input type="hidden" id="hdnUserName" />
</div>
</form>
