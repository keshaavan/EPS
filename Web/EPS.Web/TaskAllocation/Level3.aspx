<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Level3.aspx.cs" Inherits="TaskAllocation_Level3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        Level3.initialise();
    });
</script>
<form id="frmLevel3" runat="server">
<div id="divBtns">
<label class="labelClient" >Pending :</label><label id="lblPendingChartCount" style="margin-left:15px;"></label>
    <input type="text" placeholder="Search" id="txtL3SearchClientReferance" maxlength="30"
        class="textSearch ui-widget-content ui-corner-all" style="margin-left:35px;">
    <input type="button" id="btnL3RequestChart" value="Request Chart" class="button" />
    <input type="button" id="btnL3Refresh" value="Refresh Page" class="button" />
    <input type="hidden" id="heldStatus" value="<%= StatusWorkinProgress %>" />
</div>
<div id="dvEditWIP" class="divPanel">
    <div id="Top" class="DivTop">
        <h3>
            Level 3 - Chart Update</h3>
    </div>
    <div style="float: left; width: 300px;">
        <label id="lblErr" style="color: Red; width: 300px;">
        </label>
    </div>
    <div class="floatclear">
        &nbsp;</div>
    <div class="floatleft labeling">
        Reference</div>
    <div class="floatleft">
        <input type="hidden" id="hdnChartId" />
        <input type="hidden" id="hdnChartMoreInfoId" />
        <label id="lblDisplayMarket">
            Phoenix</label></div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        <label id="lblClientReferenceHeading4ChartUpdate" class="labelClient" />
    </div>
    <div class="floatleft">
        <label id="lblDisplayClientReference" />
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
        <select id="ddlL3Status">
        </select></div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Status Comments</div>
    <div class="floatleft">
        <select id="ddlL3StatusComment">
        </select>
    </div>
      <div class="floatclear">
    </div>
   
       <hr />
     <table style="height:30px;width:100%;">
      <tr  >
      <td  style="height:30px;width:100%;" colspan="3"><div class="floatrightTotal"></div></td>
        </tr>
         <tr >
        <td  style="height:5px;width:20%;" >No of Encounters </td>
         <td  style="height:30px;width:20%;" ><input type="text" id="txtL3NoOfPages" value="" style="width: 65%;" class="text ui-widget-content ui-corner-all"
            maxlength="4" /></td>
          <td  style="height:30px;width:60%;">&nbsp;&nbsp;&nbsp;&nbsp;<label id="lblL1TotalEncountersl1" />&nbsp;&nbsp;&nbsp;&nbsp;(<label id="lblL2TotalEncounters" />)</td>
        </tr>
         <tr >
        <td  style="height:30px;width:20%;" >No of DxCodes </td>
         <td  style="height:30px;width:20%;" >  <input type="text" id="txtL3NoOfDxCodes" style="width: 65%;" value="" class="text ui-widget-content ui-corner-all"
            maxlength="4" /> </td>
          <td  style="height:30px;width:60%;">&nbsp;&nbsp;&nbsp;&nbsp;(L1- <label id="lblL1TotalDxCode" />)&nbsp;&nbsp;&nbsp;&nbsp;(L2- <label id="lblL2TotalDxCode" />)</td>
        </tr>
        </table>

    <hr />
    <div class="floatleft">
        <input type="button" id="btnL3Save" value="Save" class="button" />
        <input type="button" id="btnL3Close" value="Close" class="button" /></div>
</div>
<div id="dvEditAuditComments" class="divPanelAuditComments">
    <div id="divhedar" class="DivTop">
        <h3>
            Level 3 - Chart Comments</h3>
    </div>
    <div style="float: left; width: 300px;">
        <label id="lblCommentsErr" style="width: 320px; color: Red;">
        </label>
    </div>
    <br />
    <br />
    <br />
    <div class="floatclear">
        &nbsp;</div>
    <input type="hidden" id="hdnCommentsChartId" />
    <input type="hidden" id="hdnCommentsChartMoreInfoId" />
    <div class="floatleft labeling">
        <label id="lblCommentsHeadingClientReference" class="labelClient" />
    </div>
    <div class="floatleft">
        <label id="lblCommentsHeadingClientReferenceData">
        </label>
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        FileName</div>
    <div class="floatleft">
        <label id="lblCommentsFilename">
        </label>
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Page Number
    </div>
    <div class="floatleft" style="width: 50%;">
        <input type="text" id="txtCommentsPageNo" value="" style="width: 50%;" class="text ui-widget-content ui-corner-all"
            maxlength="4" tabindex="1" />
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Corrected Values
    </div>
    <div class="floatleft" style="width: 50%;">
        <input type="text" id="txtCommentsCorrectedValues" value="" maxlength="50" style="width: 50%;"
            class="text ui-widget-content ui-corner-all" tabindex="2" />&nbsp;(ICD_9 Code/Measure/NA)
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Error Description</div>
    <div class="floatleft">
        <select id="CommentsErrorDescriptiondrp" data-placeholder="Choose Error Description"
            tabindex="3" class="chzn-select-deselect" style="width: 350px;" tabindex="1">
            <option value=""></option>
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Error Category</div>
    <div class="floatleft">
        <select id="CommentsErrorCategorydrp" data-placeholder="Choose Error Category" class="chzn-select-deselect"
            tabindex="4" style="width: 350px;" tabindex="1">
            <option value=""></option>
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Error Subcategory</div>
    <div class="floatleft">
        <select id="CommentsErrorSubCategorydrp" data-placeholder="Choose Error Sub Category"
            tabindex="5" class="chzn-select-deselect" style="width: 350px;" tabindex="1">
            <option value=""></option>
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Additional Comments</div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        <textarea id="txtAdditionalComments" rows="5" cols="60" tabindex="6" maxlength="500"></textarea></div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        &nbsp;</div>
    <div class="class">
        &nbsp;</div>
    <hr />
    <div class="floatleft">
        <input type="button" id="btnL3ActionSave" value="Save" class="button" tabindex="7" />
        <input type="button" id="btnL3ActionClose" value="Close" class="button" tabindex="8" /></div>
</div>
<div style="background: #dcdcdc; color: #696969; padding: 7px 0px 7px 3px; font-weight: bold;">
    Work In Progress And Pending Charts</div>
<div id="dvActiveCharts">
    <table id="tblPendingCharts" cellpadding="0" cellspacing="0">
        <thead id="thPendingCharts">
            <tr>
                <th class="leftAlign">
                    ChartId
                </th>
                <th class="centerAlign">
                    Status
                </th>
                <th class="centerAlign">
                    Comments
                </th>
                <th class="leftAlign">
                    Audit Log
                </th>
                <th class="leftAlign">
                    <label id="lblClientReferenceHeadingActiveCharts" class="labelClient" />
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
                <th class="leftAlign">
                    Level2 Status
                </th>
                <th class="leftAlign">
                    Level2 Comments
                </th>
                <th class="leftAlign">
                    Level2 #DxCode
                </th>
                <th class="leftAlign">
                    Level1 Status
                </th>
                <th class="leftAlign">
                    Level1 Comments
                </th>
                <th class="leftAlign">
                    Level1 #DxCode
                </th>
                <th class="centerAlign">
                    L3ChartMoreInfoId
                </th>
                <th class="centerAlign">
                    L3Pending
                </th>
                 <th class="centerAlign">
                    L2 Username
                </th>
            </tr>
        </thead>
        <tbody id="trPendingCharts">
        </tbody>
    </table>
</div>
<div style="background: #dcdcdc; color: #696969; padding: 7px 0px 7px 3px; margin-top: 40px;
    font-weight: bold;">
    Completed, Hold And Invalid Charts</div>
<div id="dvInActiveCharts">
    <table id="tblOtherChartsview" cellpadding="0" cellspacing="0">
        <thead id="thOtherCharts">
            <tr>
                <th class="leftAlign">
                    ChartId
                </th>
                <th class="centerAlign">
                    Status
                </th>
                <th class="centerAlign">
                    Comments
                </th>
                <th class="leftAlign">
                    Audit Log
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
                <th class="leftAlign">
                    Level2 Status
                </th>
                <th class="leftAlign">
                    Level2 Comments
                </th>
                <th class="leftAlign">
                    Level2 #DxCode
                </th>
                <th class="leftAlign">
                    Level1 Status
                </th>
                <th class="leftAlign">
                    Level1 Comments
                </th>
                <th class="leftAlign">
                    Level1 #DxCode
                </th>
                <th class="centerAlign">
                    L3ChartMoreInfoId
                </th>
                 <th class="centerAlign">
                    L2 Username
                </th>
            </tr>
        </thead>
        <tbody id="trOtherCharts" />
    </table>
</div>
<div id="AuditLogDialog" title="Audit Log" style="display: none;">
    <table id="tblAuditLog" cellpadding="0" cellspacing="0" class="grid">
        <thead id="thtblChartAuditLog">
            <tr>
                <th>
                    Disagree?
                </th>
                <th>
                    Level disagree with?
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
                <th class="leftAlign">
                    Last Updated
                </th>
                <th class="leftAlign">
                    Level number
                </th>
                <th class="leftAlign">
                    Level Disagree Name
                </th>
                <th class="leftAlign">
                    Level Disagree Id
                </th>
                <th class="leftAlign">
                    Chart Audit Id
                </th>
            </tr>
        </thead>
        <tbody id="trtblChartAuditLog">
        </tbody>
    </table>
    <div class="class">
        &nbsp;</div>
    <hr />
    <div class="floatleft">
        <input type="button" id="btnAuditLogSave" value="Save" class="button" />
        <input type="button" id="btnAuditLogClose" value="Close" class="button" />
    </div>
</div>
<div id="AuditLogViewDialog" title="Audit Log View" style="display: none;">
    <table id="tblAuditLogView" cellpadding="0" cellspacing="0" class="grid">
        <thead id="thtblChartAuditLogView">
            <tr>
                <th>
                    Disagree?
                </th>
                <th>
                    Level disagree with?
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
                <th class="leftAlign">
                    Last Updated
                </th>
            </tr>
        </thead>
        <tbody id="trtblChartAuditLogView">
        </tbody>
    </table>
    <div class="class">
        &nbsp;</div>
    <hr />
    <div>
        <input type="button" id="btnAuditLogViewClose" value="Close" class="button" />
    </div>
</div>
</form>
