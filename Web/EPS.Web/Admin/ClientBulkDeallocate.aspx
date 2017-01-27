<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientBulkDeallocate.aspx.cs" Inherits="Admin_ClientBulkDeallocate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        ClientBulkDeallocate.initialise();

        $('#dvUploadDeallocate').html('<iframe id=\'fileuploaddeallocate\' src=\'FileUploadDeallocate.aspx\' class=\'fileUpload\' frameborder=\'0\' ></iframe>');
        $('#dvUploadDeallocate').hide();

        $('#btnUploadDeallocate').click(function () {
            $('#dvUploadDeallocate').show();
        });
    });
</script>
<form id="frmChartDeallocateImport" enctype="multipart/form-data">
    <div style="border-bottom:#d3d3d3 solid 1px; margin:0px 0px 0px 0px;width:100%; height:40px;">
        <label style="font-size: 1.2em;color: #666666;font-variant: small-caps; margin-left:15px;">Click to upload file for deallocation</label>
        <input type="button" id="btnUploadDeallocate"  class="buttons buttons-gray"  style=" width:26px;height:26px;margin-top:5px;margin-left:15px; background-image:url(Images/File_Upload.png);" />
    </div>
    <asp:label id="lblError" runat="server" text="" visible="false" />
    <div id="dvUploadDeallocate" style="margin:10px 0px 0px 30px;"/>
</form>
