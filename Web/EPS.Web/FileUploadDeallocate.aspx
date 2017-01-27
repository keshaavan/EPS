<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileUploadDeallocate.aspx.cs"
    Inherits="FileUploadDeallocate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        function confirmBulkDeallocation() {
            var removeAll = document.getElementById('<%=chkRemoveAll.ClientID%>').checked;  //.attr('checked') ? 1 : 0;
            var confirmText = "Only charts at L1 that have not been completed or 'Unassigned' will be deleted.\nDo you want to continue?";

            if (removeAll)
                confirmText = "All information for the charts will be removed irrespective of the status.\nDo you want to continue?";

            return confirm(confirmText);
        }
    </script>
</head>
<body style="background-color: #F2F6F8">
    <form id="frmChartsBulkDeallocate" runat="server">
    <div>
        <asp:CheckBox ID="chkRemoveAll" runat="server" TextAlign="Left" Text="Do you want to delete all the data pertaining to the excel sheet being uploaded?" /><br />
        <br />
        <asp:FileUpload ID="fileUploadDeallocate" runat="server" /><br /><i><u>Note:</u></i>&nbsp;<asp:Label ID="lblNoteDeallocation" runat="server" /><br /><br />
        <asp:Button ID="btnDeallocate" runat="server" Text="Start deallocation" OnClick="btnDeallocate_Click"
            OnClientClick="javascript: return confirmBulkDeallocation();" />
        <br />
        <asp:Label ID="lblSchema" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Panel ID="pnlLoading" runat="server" Visible="false">
            <img src="Images/loadinfo.net(3).gif" alt="Deallocating" />Deallocating.......
        </asp:Panel>
    </div>
    </form>
</body>
</html>
