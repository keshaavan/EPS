<%@ page language="C#" autoeventwireup="true" inherits="Reports_PerformanceSummary_, App_Web_vzebaebx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        PerformanceSummary.initialise();
    });
             
</script>
<form id="frmPerformanceSummary" runat="server">
<div id="PerformanceSummaryHeader" style="border-bottom: 1px solid #d3d3d3; height: 85px;
    margin-top: 5px;">
    <div id="employeeDiv">
        <div class="floatleft" style="padding: 10px 2px 2px 10px;">
            Choose Employee
        </div>
        <div class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 5px;">
            <select id="EmployeeDropdownList" class="chzn-select-deselect" style="width: 250px;
                padding-top: 10px;" tabindex="1">
                <option value=""></option>
            </select>
            <script type="text/javascript">
                $(".chzn-select").chosen();
                $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
            </script>
        </div>
    </div>
    <div class="floatleft" style="padding: 10px 2px 2px 20px; margin-left: 180px;" id="levelLabelDiv">
        Choose Level
    </div>
    <div class="floatleft" style="padding: 5px 2px 2px 10px; margin-left: 0px;">
        <select id="LevelDropdownList" class="chzn-select-deselect" style="width: 100px;"
            tabindex="1">
            <option value=""></option>
            <option value="1">Level 1</option>
            <option value="2">Level 2</option>
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding: 10px 2px 2px 5px; margin-left: 5px;">
        Queue
    </div>
    <div class="floatleft" style="padding-left: 2px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <select id="Queuedrp" class="chzn-select-deselect" style="width: 200px;" tabindex="1">
            <option value="" />
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding-left: 150px; margin-top: 3px;">
        <input type="button" id="btnViewPerformanceSummary" value="View" class="buttons buttons-gray" />
    </div>
    <br />
    <br />
    <br />
    <div class="floatleft" style="padding: 3px 2px 2px 20px; margin-left: -9px;">
        From Date</div>
    <div class="floatleft" style="padding-left: 5px; margin-left: 5px; width: 90px;">
        <input type="text" id="txtFromdate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
    <div class="floatleft" style="padding: 3px 0px 2px 5px; margin-left: 8px;">
        To Date
    </div>
    <div class="floatleft" style="padding-left: 5px; margin-left: 5px; width: 90px;">
        <input type="text" id="txtTodate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
    <%--<div style="padding: 10px 2px 2px 10px;">
        From Date
    </div>
    <div  class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <input type="text" id="txtFromdate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
    <div class="floatleft" style="padding: 10px 2px 2px 20px; margin-left: 30px;">
        To Date
    </div>
    <div class="floatleft" style="padding-left: 2px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <input type="text" id="txtTodate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
    
    <div class="floatleft" style="padding: 2px 2px 2px 8px; margin-left: 30px;">
    </div>--%>
    <div class="floatright" style="padding-left: 2px; margin-top: 5px; margin-right: 10px;">
        <img id="xlsImage_Performance" src="Images/xls.png" alt="Export " class="ExcelExport"
            title="Export Performance Summary" />
    </div>
</div>
<table id="tblPerformanceReport" cellpadding="0" cellspacing="0" class="grid">
    <thead id="thblPerformanceReport">
        <tr>
            <th>
                Resource Name
            </th>
            <th>
                Level
            </th>
            <th>
                #Charts Completed
            </th>
            <th>
                #Invalid Charts
            </th>
            <th>
                #Errors
            </th>
            <th id="thchdxcode">
                <%--#Charts Audited/<br />--%>
                DxCodes
            </th>
             <th>
                Quality Control (%)
            </th>  
            <th >
                Queue
            </th>
            <th class="psummary">
                #Charts Audited
            </th>                     
            <th class="psummary">
                QC Rate
            </th>
        </tr>
    </thead>
    <tbody id="trblPerformanceReport">
    </tbody>
</table>
</form>
