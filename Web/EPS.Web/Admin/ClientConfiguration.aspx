<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientConfiguration.aspx.cs"
    Inherits="Admin_Clientconfiguration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        ClientConfiguration.initialise();

    });
             
</script>
<form id="frmClientconfiguration" runat="server">
<div id="PerformanceSummaryHeader" style="border-bottom: #d3d3d3 solid 1px; height: 50px;
    width: 100%;">
    <table style="width: 100%;" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 50%; padding-top: 10px;" colspan="1">
                <div id="editAccessControl" style="display: block; padding-left: 10px; margin-left: 0px;">
                    <input type="button" id="btnClientConfiguration" value="Edit Client Configuration"
                        class="buttons buttons-gray" style="margin-left: 20px;" />
                </div>
                <div id="accessControl" style="display: none; padding-left: 0px; margin-left: 20px;">
                    <input type="button" id="btnUpdateClientConfiguration" value="Update" class="button"
                        style="margin-left: 20px;" />
                    <input type="button" id="btnCancelClientConfiguration" value="Cancel" class="button" />
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="dvClientConfiguration" class="divClientConfig">
    <table cellpadding="0px" cellspacing="2px" style="height: 120px;">
        <tr>
            <td align="left" style="width: 50%; position: absolute; padding-left: 13px;" colspan="2">
                Client-Project
            </td>
            <td align="left" style="width: 50%; position: absolute; padding-left: 110px;">
                <label id="lblClientProject" />
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 50%; position: absolute; padding-left: 15px;" colspan="2">
                Is L1 Auto?
            </td>
            <td align="left" style="width: 10%; position: absolute; padding-left: 110px;">
                <input type="checkbox" id="chkIsL1Auto" title="L1 Auto?" value="0" disabled="disabled" />
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 15px; position: absolute;" colspan="2">
                Is L2 Auto?
            </td>
            <td align="left" style="width: 10%; position: absolute; padding-left: 110px;">
                <input type="checkbox" id="chkIsL2Auto" title="L2 Auto?" value="0" disabled="disabled" />
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 15px; position: absolute;" colspan="2">
                Is L3 Auto?
            </td>
            <td align="left" style="width: 10%; position: absolute; padding-left: 110px;">
                <input type="checkbox" id="chkIsL3Auto" title="L3 Auto?" value="0" disabled="disabled" />
            </td>
        </tr>

        <tr>
            <td align="left" style="padding-left: 15px; position: absolute;" colspan="2">
                Factorialization
            </td>
            <td align="left" style="width: 10%; position: absolute; padding-left: 113px;">
                
                <input type="text" id="txtFactorialization" value="" style="width:100%"  class="text ui-widget-content ui-corner-all"
                    maxlength="7" disabled="disabled"/>
            </td>
        </tr>
    </table>
    <div style="padding: 10px 0px 0px 0px; padding-left: 20px; width: 70%">
        <%--<div id="ChartDirectUploadDisplay" style="border-bottom:1px solid #d3d3d3;height:40px;margin-top:5px;">
            <div class="floatleft" style="padding: 10px 2px 2px 20px;">
                Home Page Text</div>
            <div class="floatleft" style="padding-left: 5px;margin-top:5px;width:60px;margin-left:5px;">                               
            </div>
            <div class="floatleft" style="padding: 10px 2px 2px 20px;margin-left:30px;">                  
            </div>           
      </div>--%>
        <textarea name="textarea" class="jqte-test"></textarea>
    </div>
</div>
</form>
