<%@ page language="C#" autoeventwireup="true" inherits="Admin_DirectUpload, App_Web_45n1udx5" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        DirectUpload.initialise();
    });
</script>
<form id="frmDirectUpload" runat="server">
    <div id="ChartDirectUploadDisplay" style="border-bottom:1px solid #d3d3d3;height:40px;margin-top:5px;">
            <div class="floatleft" style="padding: 10px 2px 2px 20px;">
                Processing From Date</div>
            <div class="floatleft" style="padding-left: 5px;margin-top:5px;width:60px;margin-left:5px;">
                <input type="text" id="txtFromdate" style="width: 80px;" readonly class="text ui-widget-content ui-corner-all" />
            </div>
            <div class="floatleft" style="padding: 10px 2px 2px 20px;margin-left:30px;">
                To Date
            </div>
            <div class="floatleft" style="padding-left: 2px;margin-top:5px;width:60px;margin-left:5px;">
                <input type="text" id="txtTodate" style="width: 80px;" readonly class="text ui-widget-content ui-corner-all" />
            </div>
            <div class="floatleft" style="padding: 2px 2px 2px 8px;margin-left:30px;">
            </div>
            <div class="floatleft" style="padding-left: 2px;margin-top:5px;">
                <input type="button" id="btnDirectUploadChartsView" value="View" class="buttons buttons-gray" />
            </div>
     <div class="floatleft" style="padding: 9px 2px 2px 10px;margin-left:5px;">
  <input type="button" id="btnDirectUploadToClient"  class="button" value="Direct Upload"  />
    </div>
    <div id="directUpload-dialog" title="Direct upload to client" style="display: none;">
        <p>
            The selected charts would be moved to completed status for the client.
        </p>
        <p>
            Do you want to continue?</p>
        <input id="btnok" type="button" class="buttons buttons-gray" tabindex="1" value="Ok"
            name="directUpload-dialog" />
        <input id="btnCancel" type="button" class="buttons buttons-gray" tabindex="2" value="Cancel" />
    </div>
      </div>
 <table id="tblDirectUploadCharts" cellpadding="0" cellspacing="0" class="display">
        <thead id="thDirectUploadCharts">
            <tr>
                <th>
                    Select<br /> 
                    <input type="checkbox" id="chkSelectAll" title="Select All" value="0" style="margin-left:14px;"/>
                </th>
                <th class="leftAlign">
                    ChartId
                </th>
                <th class="leftAlign">
                    Received Date
                </th>
                <th class="leftAlign">
                    <label id="lblClientReferenceHeading" class="labelClient">
                    </label>
                </th>
                <th class="leftAlign">
                    Reference
                </th>
                <th class="leftAlign">
                    Filename
                </th>
                <th class="leftAlign">
                    User Name
                </th>
            </tr>
        </thead>
        <tbody id="trCharts">
        </tbody>
    </table>
</form>
