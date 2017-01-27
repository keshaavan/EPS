<%@ page language="C#" autoeventwireup="true" inherits="Reports_ProductionStatus, App_Web_vzebaebx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        ProductionStatus.initialise();
    });
</script>
<form id="frmProductionStatus" runat="server">
<div id="ProductionStatusHeader" style="border-bottom: 1px solid #d3d3d3; height: 40px;
    margin-top: 5px;">
    <div class="floatleft" style="padding: 10px 2px 2px 10px;">
        Received From Date
    </div>
    <div class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <input type="text" id="txtFromdate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
    <div class="floatleft" style="padding: 10px 2px 2px 20px; margin-left: 30px;">
        To Date
    </div>
    <div class="floatleft" style="padding-left: 2px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <input type="text" id="txtTodate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
     <div class="floatleft" style="padding: 10px 2px 2px 20px; margin-left: 30px;">
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
    <div class="floatleft" style="padding: 2px 2px 2px 8px; margin-left: 30px;">
    </div>
    <div class="floatleft" style="padding-left: 150px; margin-top: 4px;">
        <input type="button" id="btnViewProductionStatus" value="View" class="buttons buttons-gray" />
    </div>
    <div class="floatright" style="padding-left: 2px; margin-top: 5px; margin-right: 10px;">
        <img id="xlsImage_ProductionStatus" src="Images/xls.png" alt="Export " class="ExcelExport"
            title="Export Production Status" />
    </div>
</div>
<table id="tblProductionStatusReport" cellpadding="0" cellspacing="0" class="grid">
    <thead>
        <tr valign="bottom">
            <th rowspan="2" style="vertical-align: top;">
                Date
            </th>
            <th colspan="3" style="text-align: center; background-color: Gray; color: White;">
                # Charts
            </th>
            <th colspan="5" style="background-color: #D6E0EB;">
                L1
            </th>
            <th colspan="5" style="background-color: #EBF0FA;">
                L2
            </th>
            <th colspan="5" style="background-color: #D6E0EB;">
                L3
            </th>
        </tr>
        <tr>
            <th>
                Imported
            </th>
            <th>
                Unassigned
            </th>
            <th>
                Completed
            </th>
            <th>
                Work In Progress
            </th>
            <th>
                Pending
            </th>
            <th>
                Completed
            </th>
            <th>
                Hold
            </th>
            <th>
                Invalid
            </th>
            <th>
                Work In Progress
            </th>
            <th>
                Pending
            </th>
            <th>
                Completed
            </th>
            <th>
                Hold
            </th>
            <th>
                Invalid
            </th>
            <th>
                Work In Progress
            </th>
            <th>
                Pending
            </th>
            <th>
                Completed
            </th>
            <th>
                Hold
            </th>
            <th>
                Invalid
            </th>
        </tr>
    </thead>
    <tbody id="trtblProductionStatus">
    </tbody>
</table>
</form>
