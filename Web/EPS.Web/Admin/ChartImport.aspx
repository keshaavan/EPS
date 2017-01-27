<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChartImport.aspx.cs" Inherits="Admin_ChartImport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        ChartImport.initialise();

        $('#dvUpload').html('<iframe id=\'fileupload\' src=\'FileUpload.aspx\' class=\'fileUpload\' frameborder=\'0\' ></iframe>');
        $('#dvUpload').hide();

        $('#btnUpload').click(function () {
            $('#dvUpload').show();
        });
        //window.open("FileUpload.aspx");
    });    
</script>
<form id="frmChartImport" enctype="multipart/form-data">
<div style="border-bottom: #d3d3d3 solid 1px; margin: 0px 0px 0px 0px; width: 100%;
    height: 40px;">
    <label style="font-size: 1.2em; color: #666666; font-variant: small-caps; margin-left: 15px;">
        Click to Upload File</label><input type="button" id="btnUpload" class="buttons buttons-gray"
            style="width: 26px; height: 26px; margin-top: 5px; margin-left: 15px; background-image: url(Images/File_Upload.png);" />
</div>
<asp:label id="lblError" runat="server" text="" visible="false" />
<div id="dvUpload" style="margin: 10px 0px 0px 30px;" />
</form>
