<%@ page language="C#" autoeventwireup="true" inherits="FileUpload, App_Web_chlyqs0y" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color:#F2F6F8">
    <form id="form1" runat="server" >
    <div >
        <asp:FileUpload ID="fileUploadClientProject" runat="server"/>
        <asp:Button ID="btnUpload" runat="server" Text="Import" OnClick="btnUpload_Click" />
        <br />
        <asp:Label ID="lblSchema" runat="server" Text="" Visible="false"   ></asp:Label>
    </div>
    </form>
</body>
</html>
