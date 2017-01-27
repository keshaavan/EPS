<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChartBulkReallocation.aspx.cs"
    Inherits="Admin_ChartBulkReallocation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        ChartBulkReallocation.initialise();
    });
</script>
<form id="frmChartBulkReallocation" runat="server">
<div id="ChartBulkReallocationDisplay" style="height: 40px; margin-top: 5px;">
    <div class="floatleft" style="padding: 10px 2px 2px 20px;">
        Date Type</div>
    <div class="floatleft" style="padding: 4px 1px 4px 2px; padding-left: 23px;">
        <select id="dateTypeDropdownList" class="chzn-select-deselect" style="width: 200px;"
            tabindex="1">
            <option value="0">Processed Date</option>
            <option value="1">Received Date</option>
            <option value="2">Imported Date</option>
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding: 10px 2px 2px 20px;">
        From Date</div>
    <div class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <input type="text" id="txtFromdate" style="width: 80px;" readonly="readonly" class="text ui-widget-content ui-corner-all"
            tabindex="2" />
    </div>
    <div class="floatleft" style="padding: 10px 2px 2px 20px; margin-left: 30px;">
        To Date
    </div>
    <div class="floatleft" style="padding-left: 2px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <input type="text" id="txtTodate" style="width: 80px;" readonly="readonly" class="text ui-widget-content ui-corner-all"
            tabindex="3" />
    </div>
</div>
<div id="ChartBulkReallocation" style=" height: 40px;
    margin-top: 5px;">
    <div class="floatleft" style="padding: 10px 2px 2px 20px;">
       From User
    </div>
    <div class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 13px;">
        <select id="EmployeeDropdownList" class="chzn-select-deselect" style="width: 250px;
            padding-top: 10px;" tabindex="1">
            <option value=""></option>
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>

  <%--  <div class="floatleft" style="padding: 10px 2px 2px 20px;">
        To User
    </div>
    <div class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 13px;">
        <select id="ToEmployeeDropdownlist" class="chzn-select-deselect" style="width: 250px;
            padding-top: 10px;" tabindex="1">
            <option value=""></option>
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>--%>

</div>
<div id="ChartstatusReallocation" style="border-bottom: 1px solid #d3d3d3; height: 40px; margin-top: 5px;">
    <div class="floatleft" style="padding: 10px 2px 2px 20px; ">
        From Status
    </div>
    <div class="floatleft" style="margin-top: 5px; padding-left: 8px; margin-left: 5px;">
        <select id="FromStatusdrp" class="chzn-select-deselect" style="width: 200px;" tabindex="5">
            <option value="" />
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding: 8px 2px 2px 2px; margin-left: 5px;">
        To Status
    </div>
    <div class="floatleft" style="padding-left: 8px; margin-top: 5px; margin-left: 5px;">
        <select id="ToStatusdrp" class="chzn-select-deselect" style="width: 200px;" tabindex="6">
            <option value="" />
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding: 2px 2px 2px 8px; margin-left: 30px;">
    </div>
    <div class="floatleft" style="padding-left: 2px; margin-top: 5px;">
        <input type="button" id="btnMoved" value="Move" class="button" style="display: none;" />
        <input type="button" id="btnChartStatus" value="View" class="button" />
        <%--<input type="button" id="btnDelete" class="button" value="Delete" />--%>
    </div>
</div>
<table id="tblCharts" cellpadding="0" cellspacing="0" class="display">
    <thead id="thCharts">
        <tr>
             <th>
                Select<br />
                <input type="checkbox" id="chkSelectAll" title="Select All" value="0" style="margin-left: 14px;" />
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
                Assigned To
            </th>
        </tr>
    </thead>
    <tbody id="trCharts">
    </tbody>
</table>
<input type="hidden" id="hdnChartId" />
<input type="hidden" id="hdnClientProjectId" />
<input type="hidden" id="hdnLevelNumber" />
<input type="hidden" id="hdnUserName" />
</form>
