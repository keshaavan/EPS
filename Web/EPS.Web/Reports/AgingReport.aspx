<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgingReport.aspx.cs" Inherits="Reports_Aging" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        AgingReport.initialise();
    });
             
</script>
<form id="frmAgingReport" runat="server">
<div id="AgingHeader" style="border-bottom: 1px solid #d3d3d3; height: 40px; margin-top: 5px;">
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
    <div class="floatleft" style="padding-left: 150px; margin-top: 5px;">
        <input type="button" id="btnViewAgingReport" value="View" class="buttons buttons-gray" />
    </div>
</div>
<table id="tblAgingSummaryReport" cellpadding="0" cellspacing="0" class="grid">
    <thead id="thblAgingSummaryReport">
        <tr>
            <th rowspan="2" valign="bottom">
                Status
            </th>
            <th colspan="5" style="background-color: #D6E0EB;">
                Days
            </th>
        </tr>
        <tr>
            <th>
                0 - 7
            </th>
            <th>
                8 - 14
            </th>
            <th>
                15 - 21
            </th>
            <th>
                22 - 28
            </th>
            <th>
                28+
            </th>
        </tr>
    </thead>
    <tbody id="trblAgingSummaryReport">
    </tbody>
</table>
<div id="AgingReportCompletedViewDialog" title="Aging Completed/Invalid View" style="display: none;">
    <div id="Div1" style="width: 100%;">
        <div class="floatleft" style="padding: 12px 0px 0px 10px;">
            Date Range :
            <label id="lblCompletedDateRange" class="labelClient" />
        </div>
    </div>
    <table id="tblAgingCompletedReportView" cellpadding="0" cellspacing="0" class="grid">
        <thead id="thtblAgingCompletedReportView">
            <tr>
                <th>
                    Received Date
                </th>
                <th>
                    L1 Invalid
                </th>
                <th>
                    L2 Invalid
                </th>
                <th class="leftAlign">
                    L3 Invalid
                </th>
                <th class="leftAlign">
                    Completed
                </th>
            </tr>
        </thead>
        <tbody id="trtblAgingCompletedReportView">
        </tbody>
    </table>
    <div class="class">
        &nbsp;</div>
    <hr />
    <div>
        <input type="button" id="btnAgingCompletedReportViewClose" value="Close" class="button" />
    </div>
</div>
<div id="AgingReportIncompleteViewDialog" title="Aging Incomplete View" style="display: none;">
    <div id="Div2" style="width: 100%;">
        <div class="floatleft" style="padding: 12px 0px 0px 10px;">
            Date Range :
            <label id="lblIncompleteDateRange" class="labelClient" />
        </div>
    </div>
    <br />
    <table id="tblAgingIncompleteReportView" cellpadding="0" cellspacing="0" class="grid">
        <thead id="thtblAgingIncompleteReportView">
            <tr>
                <th rowspan="2">
                    Received Date
                </th>
                <th rowspan="2">
                    Unassigned
                </th>
                <th colspan="4" style="background-color: #D6E0EB;">
                    Level 1
                </th>
                <th colspan="3" style="background-color: #EBF0FA;">
                    Level 2
                </th>
                <th colspan="3" style="background-color: #D6E0EB;">
                    Level 3
                </th>
            </tr>
            <tr>
                <th>
                    Work In Progress
                </th>
                <th>
                    Pending
                </th>
                <th>
                    Hold
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
                    Hold
                </th>
                <th>
                    Work In Progress
                </th>
                <th>
                    Pending
                </th>
                <th>
                    Hold
                </th>
            </tr>
        </thead>
        <tbody id="trtblAgingIncompleteReportView">
        </tbody>
    </table>
    <div class="class">
        &nbsp;</div>
    <hr />
    <div>
        <input type="button" id="btnAgingIncompleteReportViewClose" value="Close" class="button" />
    </div>
</div>
</form>
