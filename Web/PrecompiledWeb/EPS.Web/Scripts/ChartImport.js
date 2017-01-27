var ChartImport = {};

ChartImport.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ChartImport, enumWebMethod.IsUserAuthorized, uParam, 'ChartImport.isAuthorised', true, true);
}

ChartImport.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $('#dvUpload').html('<iframe id=\'fileupload\' src=\'FileUpload.aspx\'></iframe>');
        $('#dvUpload').hide();

        $('#btnUpload').click(function () {
            $('#dvUpload').show();
        });
    }
    else{
        window.location.href = "Logout.aspx";
    }
}