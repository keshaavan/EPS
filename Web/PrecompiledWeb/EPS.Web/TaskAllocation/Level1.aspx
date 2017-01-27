<%@ page language="C#" autoeventwireup="true" inherits="TaskAllocation_Level1, App_Web_1siyun3w" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        Level1.initialise();
    });
</script>

<style type="text/css" >
    .CompletedbgColor
    {
        background-color:#E0A28C;
    }
    .HoldbgColor
    {
        background-color:#A4DA8C;
    }
</style>

<div id="divBtns">
    <label class="labelClient">
        Pending :</label><label id="lblPendingChartCount" style="margin-left: 15px;"></label>
    <input type="text" placeholder="Search" id="txtL1SearchClientReferance" maxlength="30"
        class="textSearch ui-widget-content ui-corner-all" style="margin-left: 35px;">
    <input type="button" id="btnL1RequestChart" value="Request Chart" class="button" />
    <input type="button" id="btnL1Refresh" value="Refresh Page" class="button" />
</div>
<div id="dvEditWIP" class="divPanel">
    <div id="Top" class="DivTop">
        <h3>
            Level 1 - Chart Update</h3>
    </div>
    <div style="float: left; width: 300px;">
        <label id="lblErr" style="color: Red; width: 300px;" />
    </div>
    <div class="floatclear">
        &nbsp;</div>
    <div class="floatleft labeling">
        Market</div>
    <div class="floatleft">
        <input type="hidden" id="hdnChartId" />
        <input type="hidden" id="hdnChartMoreInfoId" />
        <label id="lblDisplayMarket" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        <label id="lblClientReferenceHeading" class="labelClient" />
    </div>
    <div class="floatleft">
        <label id="lblDisplayGMPI" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        FileName</div>
    <div class="floatleft">
        <label id="lblDisplayFileName" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Status</div>
    <div class="floatleft">
        <select id="ddlStatus" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Status Comments</div>
    <div class="floatleft">
        <select id="ddlStatusComment" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        No of Encounters</div>
    <div class="floatleft">
        <input type="text" id="txtNoOfPages" value="" style="width: 100%;" class="text ui-widget-content ui-corner-all"
            maxlength="4" /></div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        No of DxCodes</div>
    <div class="floatleft">
        <input type="text" id="txtNoOfDxCodes" value="" style="width: 100%;" class="text ui-widget-content ui-corner-all"
            maxlength="4" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        &nbsp;</div>
    <div class="class">
        &nbsp;</div>
    <hr />
    <div class="floatleft">
        <input type="button" id="btnSave" value="Save" class="button" />
        <input type="button" id="btnClose" value="Close" class="button" /></div>
</div>
<%--Editcomments--%>
<div id="dvEditCmd" class="divPanel">
    <div id="Top1" class="DivTop">
        <h3>
            Level 1 - Status Comments</h3>
    </div>
    <div style="float: left; width: 300px;">
        <label id="lblErr1" style="color: Red; width: 300px;" />
    </div>

    <div class="floatclear">
        &nbsp;</div>
    <div class="floatleft labeling">
        Market</div>
    <div class="floatleft">
        <input type="hidden" id="hdnChartIdUpdate" />
        <input type="hidden" id="hdnChartMoreInfoIdUpdate" />
        <input type="hidden" id="hdnClientProjectId" />
        <input type="hidden" id="hdnLevelnumber" />
        <label id="lblDisplayMarketName" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        <label id="lblClientReferenceHeadingName" class="labelClient" />
    </div>
    <div class="floatleft">
        <label id="lblDisplayGMPIName" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        FileName</div>
    <div class="floatleft">
        <label id="lblDisplayFileNames" />        
        <input type="hidden" id="hdndisplayfilename" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Status</div>
    <div class="floatleft" >
        <select id="ddlStatusName"  />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Status Comments</div>
    <div class="floatleft" >
        <select id="ddlStatusCommentName" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        No of Encounters</div>
    <div class="floatleft">
        <input type="text" id="txtNoOfPagesUpdate" value="" style="width: 100%;" class="text ui-widget-content ui-corner-all"
            maxlength="4"  />
            <input type="hidden" id="hdnnoofpages" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        No of DxCodes</div>
    <div class="floatleft">
        <input type="text" id="txtNoOfDxCodesName" value="" style="width: 100%;" class="text ui-widget-content ui-corner-all"
            maxlength="4"  />
            <input type="hidden" id="hdnnoofdxcodes" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        &nbsp;</div>
    <div class="class">
        &nbsp;</div>
    <hr />
    <div class="floatleft">
        <input type="button" id="btnUpdate" value="Update" class="button" />
        <input type="button" id="btnCancel" value="cancel" class="button" /></div>
</div>
<div class="WIPChartsHeading">
    Work In Progress And Pending Charts</div>
<div id="dvActiveCharts">
    <table id="tblPendingCharts" cellpadding="0" cellspacing="0">
        <thead id="thPendingCharts">
            <tr>
                <th class="leftAlign">
                    ChartId
                </th>
                <th class="leftAlign">
                    <label id="lblClientReferenceHeadingPendingCharts" class="labelClient" />
                </th>
                <th class="centerAlign">
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
                <th class="centerAlign">
                    Status
                </th>
            </tr>
        </thead>
        <tbody id="trPendingCharts">
        </tbody>
    </table>
</div>
<div class="OtherChartsHeading">
    Completed, Hold Charts
</div>
<div id="dvInActiveCharts">
    <table id="tblOtherChartsview" cellpadding="0" cellspacing="0">
        <thead id="thOtherCharts">
            <tr>
                <th class="leftAlign">
                    ChartId
                </th>
                <th class="leftAlign">
                    <label id="lblClientReferenceHeadingOtherCharts" class="labelClient" />
                </th>
                <th class="centerAlign">
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
                <th class="centerAlign">
                    Status
                </th>
                <th class="LeftAlign">
                    Status Comment
                </th>
                <th class="LeftAlign">
                    ChartMoreInfoId
                </th>
                <th class="LeftAlign">
                    #DxCodes
                </th>
                <th class="LeftAlign never" id="thEdit" >
                    Edit
                </th>                
            </tr>
        </thead>
        <tbody id="trOtherCharts" />
    </table>
</div>
<input type="hidden" runat="server" clientidmode="Static" id="hdnpagesize" value="25" />
