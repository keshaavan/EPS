<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MasterData.aspx.cs" Inherits="Admin_MasterData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        MasterData.initialise();

    });
</script>
<form id="frmMasterData" runat="server">
<div id="tabs">
    <ul>
        <li><a href="#tabs-1" id="tab1">Lookups</a></li>
        <li><a href="#tabs-2" id="tab2">Comment</a></li>
        <li><a href="#tabs-3" id="tab3">Queue</a></li>
    </ul>
    <div id="tabs-1">
        <div id="Lookupcategory" style="border-bottom: 1px solid #d3d3d3; height: 40px; margin-top: 0px;">
            <div class="floatleft" style="padding: 10px 2px 2px 10px;">
                Lookup category
            </div>
            <div class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 5px;">
                <select id="ddlLookupValues" data-placeholder="Choose a Lookup" class="chzn-select-deselect"
                    style="width: 350px;" tabindex="3">
                    <option value=""></option>
                </select>
                <script type="text/javascript">
                    $(".chzn-select").chosen();
                    $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
                </script>
            </div>
            <div class="floatleft" style="padding: 2px 2px 2px 10px; margin-left: 290px;">
                <input type="button" id="btnLookupAdd" value="Add" class="buttons buttons-gray" />
            </div>
        </div>
        <table id="tblLookupDatum" cellpadding="0" cellspacing="0" style="width: 100%;">
            <thead id="thLookupDatum">
                <tr>
                    <th class="centerAlign labelMasterdata">
                        Action
                    </th>
                    <th class="leftAlign labelMasterdata">
                        Client
                    </th>
                    <th class="leftAlign labelMasterdata">
                        Name
                    </th>
                    <th class="leftAlign labelMasterdata">
                        Category
                    </th>
                    <th class="centerAlign labelMasterdata">
                        Display Order
                    </th>
                    <th class="centerAlign labelMasterdata">
                        Is Active?
                    </th>
                </tr>
            </thead>
            <tbody id="trLookupDatum">
            </tbody>
        </table>
        <div id="divLookupEdit" class="divPanelLookUp">
            <div id="Top" class="DivTop">
                <h3 id="headerlookup">
                    Add new Look up Values</h3>
            </div>
            <table cellpadding="0" cellspacing="0" style="height: 120px; width: 100%; margin-top: 5px;
                margin-left: 20px;">
                <tr>
                    <td align="left" style="height: 30px; width: 20%;">
                        <input type="hidden" id="hdnLookupId" />
                        Name
                    </td>
                    <td align="left" style="height: 30px; width: 80%;">
                        <input type="text" placeholder="" style="width: 80%;" name="Name" tabindex="1" id="txtLookupName"
                            maxlength="255" class="text ui-widget-content ui-corner-all" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 30px;">
                        Display Order
                    </td>
                    <td align="left" style="height: 30px;">
                        <input type="text" placeholder="Enter Display Order" tabindex="2" name="DisplayOrder"
                            id="txtLookupDisplayOrder" style="width: 40%;" maxlength="4" class="text ui-widget-content ui-corner-all" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 30px;">
                        Is Active?
                    </td>
                    <td align="left" style="height: 30px;">
                        <input type="checkbox" id="chkLookupIsActive" tabindex="3" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 30px;">
                    </td>
                    <td align="left" style="height: 30px;">
                        <input type="button" id="btnLookupSave" value="Save" tabindex="4" class="buttons buttons-gray" />
                        <input type="button" id="btnLookupCancel" value="Cancel" tabindex="5" class="buttons buttons-gray" />
                    </td>
                </tr>
            </table>
        </div>
    </div>


    <div id="tabs-2">
        <div id="Commentcategory" style="border-bottom: 1px solid #d3d3d3; height: 40px;
            margin-top: 0px;">
            <div class="floatleft" style="padding: 10px 2px 2px 10px;">
                Comment category
            </div>
            <div class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 5px;">
                <select id="ddlCommentsCategories" data-placeholder="Choose a Comment" class="chzn-select-deselect"
                    style="width: 350px;" tabindex="3">
                    <option value=""></option>
                </select>
            </div>
            <div class="floatleft" style="padding: 2px 2px 2px 10px; margin-left: 290px;">
                <input type="button" id="btnCategoryAdd" value="Add" class="buttons buttons-gray" />
                <script type="text/javascript">
                    $(".chzn-select").chosen();
                    $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
                </script>
            </div>
        </div>
        <table id="tblCommentsCategories" cellpadding="0" cellspacing="0" style="width: 100%;">
            <thead id="thCommentsCategories">
                <tr>
                    <th class="centerAlign labelMasterdata">
                        Action
                    </th>
                    <th class="leftAlign labelMasterdata">
                        Description
                    </th>
                    <th class="rightAlign labelMasterdata">
                        Display Order
                    </th>
                    <th class="centerAlign labelMasterdata">
                        Is Active?
                    </th>
                </tr>
            </thead>
            <tbody id="tbCommentsCategories">
            </tbody>
        </table>
        <div id="divEditCommentCategory" class="divPanelLookUp">
            <div id="Div2" class="DivTop">
                <h3 id="Headercomments">
                    Add new comments</h3>
            </div>
            <table cellpadding="0px" cellspacing="0px" style="height: 120px; width: 100%; margin-top: 5px;
                margin-left: 20px;">
                <tr>
                    <td align="left" style="height: 30px; width: 20%;">
                        <input type="hidden" id="hdnCommentId" />
                        Description
                    </td>
                    <td align="left" style="height: 30px; width: 80%;">
                        <input type="text" placeholder="" style="width: 80%;" name="Name" id="txtCommentDescription"
                            maxlength="255" tabindex="1" class="text ui-widget-content ui-corner-all" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 30px;">
                        Display Order
                    </td>
                    <td align="left" style="height: 30px;">
                        <input type="text" placeholder="Enter Display Order" tabindex="2" name="DisplayOrder"
                            id="txtCommentDisplayOrder" style="width: 40%;" maxlength="4" class="text ui-widget-content ui-corner-all" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 30px;">
                        Is Active?
                    </td>
                    <td align="left" style="height: 30px;">
                        <input id="chkCommentsActive" type="checkbox" tabindex="3" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 30px;">
                    </td>
                    <td align="left" style="height: 30px;">
                        <input type="button" id="btnCommentsSave" value="Save" class="buttons buttons-gray"
                            tabindex="4" />
                        <input type="button" id="btnCommentsCancel" value="Cancel" class="buttons buttons-gray"
                            tabindex="5" />
                    </td>
                </tr>
            </table>
        </div>
    </div>


    <div id="tabs-3">
        <div id="DivQueue" style="border-bottom: 1px solid #d3d3d3; height: 60px; margin-top: 0px;">
            <table>
                <tr>
                    <td>
                        
                      <b>Client Project:</b>
                       
                    </td>
                    <td>
                        <%=Client%> - <%= ProjectName %>
                    </td>                    
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" id="cmdAddQueue" value="Add New Queue" class="buttons buttons-gray" />
                    </td>
                </tr>
            </table>
        </div>

        <table id="tblQueue" cellpadding="0" cellspacing="0" style="width: 100%;">
            <thead id="thQueue">
                <tr>
                    <th class="centerAlign labelMasterdata">
                        Action
                    </th>
                    <th class="leftAlign labelMasterdata">
                        Queue
                    </th>
                    <th class="centerAlign labelMasterdata">
                        Is Active?
                    </th>
                </tr>
            </thead>
            <tbody id="tbQueue">
            </tbody>
        </table>

        <div id="divQueueAddOrEdit" class="divPanelLookUp">
            <div id="div3" class="DivTop">
                <h3 id="HeaderQueue">
                    Add new Queue</h3>
            </div>
            <table cellpadding="0px" cellspacing="0px" style="height: 120px; width: 100%; margin-top: 5px;
                margin-left: 20px;">
                <tr>
                    <td align="left" style="height: 30px; width: 20%;">
                        <input type="hidden" id="hdnQueue" />
                        Queue
                    </td>
                    <td align="left" style="height: 30px; width: 80%;">
                        <input type="text" placeholder="" style="width: 80%;" name="Name" id="TxtQueuename" maxlength="255"
                            tabindex="1" class="text ui-widget-content ui-corner-all" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 30px;">
                        Is Active?
                    </td>
                    <td align="left" style="height: 30px;">
                        <input id="ChkQActive" type="checkbox" tabindex="3" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 30px;">
                    </td>
                    <td align="left" style="height: 30px;">
                        <input type="button" id="btnQueueSave" value="Save" class="buttons buttons-gray" tabindex="4" />
                        <input type="button" id="btnQueueCancel" value="Cancel" class="buttons buttons-gray" tabindex="5" />
                    </td>
                </tr>
            </table>
        </div>

        <input type="hidden" value="" id="hidflag" />
    </div>
</div>
</form>
