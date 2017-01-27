<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Level2.aspx.cs" Inherits="TaskAllocation_Level2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        Level2.initialise();
    });
</script>
<!--<form id="form1" runat="server">-->
<div id="divBtns">
    <label class="labelClient">
        Pending :</label><label id="lblPendingChartCount" style="margin-left: 15px;"></label>
    <input type="text" placeholder="Search" id="txtL2SearchClientReferance" maxlength="30"
        class="textSearch ui-widget-content ui-corner-all" style="margin-left: 35px;" />
    <input type="button" id="btnL2RequestChart" value="Request Chart" class="button" />
    <input type="button" id="btnL2Refresh" value="Refresh Page" class="button" />
    <input type="hidden" id="heldStatus" value="<%= StatusWorkinProgress %>" />
</div>
<div id="dvEditWIP" class="divPanel">
    <h3>
        Level 2 - Chart Update</h3>
    <hr />
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
        <label id="lblDisplayGMPI">
        </label>
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        FileName</div>
    <div class="floatleft">
        <label id="lblDisplayFileName">
        </label>
    </div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Status</div>
    <div class="floatleft">
        <select id="ddlL2Status">
        </select></div>
    <div class="floatclear">
    </div>
    <div class="floatleft labeling">
        Status Comments</div>
    <div class="floatleft">
        <select id="ddlL2StatusComment">
        </select></div>
    <div class="floatclear">
    </div>
    <hr />
    <table style="height: 30px; width: 100%;">
        <tr>
            <td style="height: 30px; width: 100%;" colspan="3">
                <div class="floatrightTotal">
                    L1 &nbsp;</div>
            </td>
        </tr>
        <tr>
            <td style="height: 5px; width: 20%;">
                No of Encounters
            </td>
            <td style="height: 30px; width: 20%;">
                <input type="text" id="txtL2NoOfPages" value="" style="width: 60%;" class="text ui-widget-content ui-corner-all"
                    maxlength="4" />
            </td>
            <td style="height: 30px; width: 60%;">
                <label id="lblL1TotalEncounters" />
            </td>
        </tr>
        <tr>
            <td style="height: 30px; width: 20%;">
                No of DxCodes
            </td>
            <td style="height: 30px; width: 20%;">
                <input type="text" id="txtL2NoOfDxCodes" value="" style="width: 60%;" class="text ui-widget-content ui-corner-all"
                    maxlength="4" />
            </td>
            <td style="height: 30px; width: 60%;">
                <label id="lblL1TotalDxCode" />
            </td>
        </tr>
    </table>
    <hr />
    <div class="floatleft">
        <input type="button" id="btnL2Save" value="Save" class="button" />
        <input type="button" id="btnL2Close" value="Close" class="button" /></div>
</div>
<div id="editbox" style="display:none;">
    <div id="effect" class="ui-widget-content ui-corner-all" style="height: 800px; margin-left: 0px;
        margin-top: 0px; width: 1562px;">
        <table cellpadding="0px" cellspacing="10px" style="height: 300px; width: 950px; margin-left: 200px;
            margin-top: 25px;">
            <tr>
                <td>
                    <input type="hidden" id="hdnMoreInfoId1" />
                    <input type="hidden" id="hdnid" />
                    <input type="hidden" id="hdnfilename" />
                </td>
                <td align="left" style="height: 20px; width: 10%;">
                    <label id="lblCommentsErr1" style="width: 300px; color: Red;">
                    </label>
                </td>
            </tr>
            <%--  <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    <label id="lblCommentsHeadingClientReferenceUpdate" class="labelClient" />
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <label id="lblCommentsHeadingClientReferenceDataUpdate">
                    </label>
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    File Name
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <label id="lblCommentsFilenameUpdate">
                    </label>
                </td>
            </tr>--%>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Page Number
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input type="text" id="txtCommentsPageNoUpdate" value="" style="width: 48.5%; margin-top: 5px;"
                        class="text ui-widget-content ui-corner-all" maxlength="4" tabindex="1" />
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 10px; width: 10%;">
                    Corrected Values
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input type="text" id="txtCommentsCorrectedValuesUpdate" value="" style="width: 48.5%;
                        margin-top: 5px;" class="text ui-widget-content ui-corner-all" maxlength="15"
                        tabindex="2" />
                    (ICD_9 Code / Measure /NA)
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Error Description
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <select id="CommentsErrorDescriptiondrpUpdate" data-placeholder="Choose Error Description"
                        tabindex="3" class="chzn-select-deselect" style="width: 350px;">
                        <option value=""></option>
                    </select>
                    <script type="text/javascript">
                        $(".chzn-select").chosen();
                        $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
                    </script>
                    <%--<select i="CommentsErrorDescriptiondrpUpdate" />--%>
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 20px; width: 10%;">
                    Error Category
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <select id="CommentsErrorCategorydrpUpdate" data-placeholder="Choose Error Category"
                        class="chzn-select-deselect" tabindex="4" style="width: 350px;">
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
                    Error SubCategory
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <select id="CommentsErrorSubCategorydrpUpdate" data-placeholder="Choose Error Sub Category"
                        tabindex="5" class="chzn-select-deselect" style="width: 350px;" tabindex="1">
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
                    Additional Comments
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <textarea id="txtAdditionalCommentsUpdate" rows="5" cols="60" tabindex="6" maxlength="500"></textarea>
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 20px; width: 10%;">
                </td>
                <td align="left" style="height: 20px; width: 20%;">
                    <input id="btnL2ActionUpdate" type="button" class="button" tabindex="7" value=""
                        name="addNew" />
                    <input id="btnL2ActionUpdateClose" type="button" class="button" tabindex="8" value="Cancel" />
                </td>
            </tr>
        </table>
        <hr />
        <div id="dvChartAuditComments">
            <table id="tblAuditComments" width="100%">
                <thead id="thChartComments">
                    <tr>
                        <th class="centerAlign">
                            Id
                        </th>
                        <th class="centerAlign" style="white-space: nowrap;">
                            Page Number
                        </th>
                        <th class="centerAlign" style="white-space: nowrap;">
                            Corrected Values
                        </th>
                        <th class="centerAlign" style="white-space: nowrap;">
                            Error Description
                        </th>
                        <th class="centerAlign" style="white-space: nowrap;">
                            Error Category
                        </th>
                        <th class="centerAlign" style="white-space: nowrap;">
                            Error SubCategory
                        </th>
                        <th class="centerAlign" style="white-space: nowrap;">
                            Additional Comments
                        </th>
                        <th class="centerAlign" style="white-space: nowrap;">
                            Edit
                        </th>
                        <th class="centerAlign" style="white-space: nowrap;">
                            Delete
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
</div>
<div id="dvEditAuditComments" class="divPanelAuditComments">
    <h3>
        Level 2 - Chart Comments</h3>
    <hr />
    <div style="float: left; width: 300px;">
        <label id="lblCommentsErr" style="width: 300px; color: Red;">
        </label>
    </div>
    <br />
    <br />
    <br />
    <div class="floatclear">
        &nbsp;</div>
    <input type="hidden" id="hdnCommentsChartId" />
    <input type="hidden" id="hdnCommentsChartMoreInfoId" />
    <input type="hidden" id="hdnClientProjectId" />
    <input type="hidden" id="hdnLevelnumber" />
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
        <input type="text" id="txtCommentsCorrectedValues" value="" style="width: 50%;" class="text ui-widget-content ui-corner-all"
            maxlength="50" tabindex="2" />
        (ICD_9 Code / Measure /NA)
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
        <input type="button" id="btnL2ActionSave" value="Save" class="button" tabindex="7" />
        <input type="button" id="btnL2ActionClose" value="Close" class="button" tabindex="8" /></div>
</div>
<div id="Container">
    <div style="background: #dcdcdc; color: #696969; padding: 7px 0px 7px 3px; font-weight: bold;">
        Work In Progress And Pending Charts</div>
    <div id="dvActiveCharts">
        <table id="tblPendingCharts" cellpadding="0" cellspacing="0">
            <thead id="thPendingCharts">
                <tr>
                    <th class="leftAlign">
                        ChartId
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
                    <th class="centerAlign">
                        Status
                    </th>
                    <th class="centerAlign">
                        ChartMoreInfoId
                    </th>
                    <th class="centerAlign">
                        Comments
                    </th>
                </tr>
            </thead>
            <tbody id="trPendingCharts">
            </tbody>
        </table>
    </div>
</div>
<div id="ContainerCompleted">
    <div style="background: #dcdcdc; color: #696969; padding: 7px 0px 7px 3px; margin-top: 40px;
        font-weight: bold;">
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
                    <th class="leftAlign">
                        Status Comment
                    </th>
                    <th class="leftAlign">
                        ChartMoreInfoId
                    </th>
                    <th class="leftAlign">
                        #DxCodes
                    </th>
                    <th class="leftAlign never" id="thEdit">
                        Edit
                    </th>
                </tr>
            </thead>
            <tbody id="trOtherCharts" />
        </table>
    </div>
</div>
<input type="hidden" runat="server" clientidmode="Static" id="hdnlevel2pagesize" value="25" />
<!--</form>-->
