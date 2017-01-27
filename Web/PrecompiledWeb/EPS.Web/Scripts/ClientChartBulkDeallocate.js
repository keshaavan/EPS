var ClientBulkDeallocate = {};

ClientBulkDeallocate.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ClientBulkDeallocate, enumWebMethod.IsUserAuthorized, uParam, 'ClientBulkDeallocate.isAuthorised', true, true);
}

ClientBulkDeallocate.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $('#dvUpload').html('<iframe id=\'fileupload\' src=\'FileUploadDeallocate.aspx\'></iframe>');
        $('#dvUpload').hide();

        $('#btnUpload').click(function () {
            $('#dvUpload').show();
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

