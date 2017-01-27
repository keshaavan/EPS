var Login = {};

Login.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.Login, enumWebMethod.IsUserAuthorized, uParam, 'Login.isAuthorised', true, true);
}

Login.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {

    }
    else {
        window.location.href = "Logout.aspx";
    }
}