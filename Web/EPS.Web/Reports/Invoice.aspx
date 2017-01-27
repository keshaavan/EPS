<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoice.aspx.cs" Inherits="Reports_Invoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        Invoice.initialise();
    });
             
</script>
<form id="frmInvoice" runat="server">
<div id="invoiceHeader" style="border-bottom: 1px solid #d3d3d3; height: 40px; margin-top: 5px;">
    <div class="floatleft" style="padding: 10px 2px 2px 10px;">
        Received From Date
    </div>
    <div class="floatleft" style="padding-left: 5px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <input type="text" id="txtFromdate" readonly="readonly"  style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
    <div class="floatleft" style="padding: 10px 2px 2px 20px; margin-left: 30px;">
        To Date
    </div>
    <div class="floatleft" style="padding-left: 2px; margin-top: 5px; width: 60px; margin-left: 5px;">
        <input type="text" id="txtTodate" readonly="readonly" style="width: 80px;" class="text ui-widget-content ui-corner-all" />
    </div>
    <div class="floatleft" style="padding: 10px 2px 2px 20px; margin-left: 30px;">
        Level
    </div>
    <div class="floatleft" style="padding: 5px 2px 2px 10px; margin-left: 0px;">
        <select id="LevelDropdownList" class="chzn-select-deselect" style="width: 150px;"
            tabindex="1">
            <option value=""></option>
            <option value="1">Level 1</option>
            <option value="0">Completed</option>
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding: 10px 1px 2px 20px; margin-left: 10px;">
        Queue
    </div>
    <div class="floatleft" style="padding: 5px 2px 2px 10px; margin-left: 0px;">
        <select id="Queuedrp" class="chzn-select-deselect" style="width: 200px;"
            tabindex="1">
            <option value="" />
        </select>
        <script type="text/javascript">
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });     
        </script>
    </div>
    <div class="floatleft" style="padding: 2px 2px 2px 8px; margin-left: 10px;">
    </div>
    <div class="floatleft" style="padding-left: 2px; margin-top: 2px;">
        <input type="button" id="btnViewInvoice" value="View" class="buttons buttons-gray" />
    </div>
    <div class="floatright" style="padding-left: 2px; margin-top: 5px; margin-right: 10px;">
        <img id="xlsImage_Invoice" src="../Images/xls.png"  alt="Export Invoice" class="ExcelExport"
            title="Export Invoice" />
    </div>
</div>
<table id="tblInvoiceReport" cellpadding="0" cellspacing="0" class="grid">
    <thead id="thblInvoiceReport">
        <tr>
            <th class="leftAlign">
                Received Date
            </th>
            <th class="leftAlign">
                Filename
            </th>
            <th class="leftAlign">
                <label id="clientReferenceHeading" class="labelClient" />
            </th>
            <th class="leftAlign">
                Completed Date
            </th>
        </tr>
    </thead>
    <tbody id="trblInvoiceReport">
    </tbody>
</table>
</form>
