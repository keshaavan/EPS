<%@ page language="C#" autoeventwireup="true" inherits="Reports_ChartStatus, App_Web_vzebaebx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        ChartStatus.initialise();
    });
             
</script>
<form id="form1" runat="server">
<div id="loansTableContainer" style="height:75px;border-bottom:1px solid #d3d3d3;margin-top:10px;" class="BlockUI">
    <div class="floatleft" style="padding: 2px;">
        Date Type</div>
    <div class="floatleft" style="padding-left: 8px;">
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
    <div class="floatleft" style="padding: 2px;margin-left:5px;">
        Level Status</div>
    <div class="floatleft" style="padding-left: 8px;margin-left:5px;">
        <select id="Statusdrp" class="chzn-select-deselect" style="width: 200px;" tabindex="1">
            <option value="" />
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding-left: 2px;margin-left:6px;">
    Queue
    </div>
    <div class="floatleft" style="padding-left: 8px;margin-left:5px;">
        <select id="Queuedrp" class="chzn-select-deselect" style="width: 200px;" tabindex="1">
            <option value="" />
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding-left: 1px;margin-left:15px;">
        <input type="button" id="btnChartStatus" value="View" class="buttons buttons-gray" />
    </div>
    <br />
    <br />
    <div class="floatleft">
        From Date</div>
    <div class="floatleft" style="padding-left: 5px;margin-left:5px;width:90px;">
        <input type="text" id="txtFromdate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
    <div class="floatleft" style="padding: 5px 0px 2px 5px;margin-left:8px;">
        To Date
    </div>
    <div class="floatleft" style="padding-left: 5px;margin-left:5px; width:90px;">
        <input type="text" id="txtTodate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
 <br />
 <br />
 <br />   
    <div id="top" style="float: right; padding-right: 10px;">
        <img id="xlsImage_ChartStatus" src="Images/xls.png" class="ExcelExport" title="Export Chart Status" />
        <img id="xlsImage_ChartStatusComments" src="Images/xls.png" class="ExcelExport" title="Export Audit Log" />
    </div>
   
</div>
 <table id="tblChartStatus" cellpadding="0" cellspacing="0" class="display" style="margin: 4px;">
        <thead id="thtblChartStatus">
            <tr valign="top">
                <th rowspan="2">
                </th>
                <th rowspan="2" style="vertical-align:bottom;">
                    Audit Count
                </th>
                <th rowspan="2" style="vertical-align:bottom;">
                    Reference
                </th>
                <th rowspan="2" style="vertical-align:bottom;">
                    <label id="clientReferenceHeading" class="labelClient" />
                </th>
                <th rowspan="2" style="vertical-align:bottom;">
                    Filename
                </th>
                <th rowspan="2" style="vertical-align:bottom;">
                    Imported Date
                </th>
                <th rowspan="2" style="vertical-align:bottom;">
                    Received Date
                </th>
                <th rowspan="2" style="vertical-align:bottom;">
                    Overall Status
                </th>
                <th rowspan="2" style="vertical-align:bottom;">
                    #Encounters
                </th>
                <th colspan="6" style="background-color: #D6E0EB;">
                    L1
                </th>
                <th colspan="6" style="background-color: #EBF0FA;">
                    L2
                </th>
                <th colspan="6" style="background-color: #D6E0EB;">
                    L3
                </th>
                <th rowspan="2">
                    ChartMoreInfoId
                </th>
            </tr>
            <tr>
                <th>
                    Start Date
                </th>
                <th>
                    End Date
                </th>
                <th>
                    Time Taken
                </th>
                <th>
                    Resource
                </th>
                <th>
                    Status Comment
                </th>
                <th>
                    #DxCodes
                </th>
                <th>
                    Start Date
                </th>
                <th>
                    End Date
                </th>
                <th>
                    Time Taken
                </th>
                <th>
                    Resource
                </th>
                <th>
                    Status Comment
                </th>
                <th>
                    #DxCodes
                </th>
                <th>
                    Start Date
                </th>
                <th>
                    End Date
                </th>
                <th>
                    Time Taken
                </th>
                <th>
                    Resource
                </th>
                <th>
                    Status Comment
                </th>
                <th>
                    #DxCodes
                </th>
                <th>
                    #L1ProductionTimeTaken
                </th>
                 <th>
                    #L2ProductionTimeTaken
                </th>
                 <th>
                    #L3ProductionTimeTaken
                </th>
                 <th>
                    L1ChartMoreInfoId
                </th>
                 <th>
                    L2ChartMoreInfoId
                </th>
                 <th>
                   L3ChartMoreInfoId
                </th>
            </tr>
        </thead>
        <tbody id="trtblChartStatus">
        </tbody>
    </table>
<div id="ChartAuditDialog" title="Chart Audit" style="display: none;">
    <table id="tblChartAuditLog" cellpadding="0" cellspacing="0" class="grid">
        <thead id="thtblChartAuditLog">
            <tr>
                <th class="leftAlign">
                    Last Updated
                </th>
                <th class="leftAlign">
                    Page Numbers
                </th>
                <th class="leftAlign">
                    Corrected Value
                </th>
                <th class="leftAlignColor">
                    Comments
                </th>
                <th class="leftAlignColor">
                    Error Category
                </th>
                <th class="leftAlignColor">
                    Error Subcategory
                </th>
                <th class="leftAlignColor">
                    Additional Comments
                </th>
                <th class="leftAlignColor">
                    Level disagree with?
                </th>
            </tr>
        </thead>
        <tbody id="trtblChartAuditLog">
        </tbody>
    </table>
</div>
<div id="ProductionTimeTaken" title="Chart Production Times" style="display: none;">
    <table id="tblProductionTimeTaken" cellpadding="0" cellspacing="0" class="grid">
        <thead id="thtblProduction">
            <tr>
                <th class="centerAlign">
                    Start DateTime
                </th>
                <th class="centerAlign">
                    End DateTime
                </th>
                <th class="centerAlign">
                    Production Time
                </th>
                <th class="leftAlign">
                    Level Status
                </th>
                <th class="leftAlign">
                   Comments
                </th>
                <th class="leftAlign">
                    Level UpdatedBy
                </th>
               
            </tr>
        </thead>
        <tbody id="tbtblProduction">
        </tbody>
    </table>
</div>

</form>
