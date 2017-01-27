<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserMapping.aspx.cs" Inherits="Admin_UserMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        UserMapping.initialise();

    });    
</script>
<form id="frmUserMapping" runat="server">
<div id="UserMapping" style="border-bottom: 1px solid #d3d3d3; height: 40px; margin-top: 5px;">
    <div class="floatleft" style="padding: 9px 2px 2px 10px; margin-left: 5px;">
        <input type="button" id="btnUserMapping" class="button" value="User Mapping" />
    </div>
    <div id="directUpload-dialog" title="Direct upload to client" style="display: none;">
        <p>
            The selected user would be moved to activate.
        </p>
        <p>
            Do you want to continue?</p>
        <input id="btnok" type="button" class="buttons buttons-gray" tabindex="1" value="Ok"
            name="directUpload-dialog" />
        <input id="btnCancel" type="button" class="buttons buttons-gray" tabindex="2" value="Cancel" />
    </div>
</div>
<table id="tblUserMapping" cellpadding="0" cellspacing="0" class="display">
    <thead id="thUserMapping">
        <tr>
            <th>
                Select<br />
                <input type="checkbox" id="chkSelectAll" title="Select All" value="0" style="margin-left: 14px;" />
            </th>                       
            <th class="centerAlign">
                User Name
            </th>            
            <th class="centerAlign">
                Mapped Project
            </th>            
            <th class="centerAlign">
                Activate Project
            </th>  
        </tr>
    </thead>
    <tbody id="trUserMapping">
    </tbody>
</table>
</form>
