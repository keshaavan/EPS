var ClientConfiguration = {};

ClientConfiguration.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.IsUserAuthorized, uParam, 'ClientConfiguration.isAuthorised', true, true);
}

ClientConfiguration.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $('.jqte-test').jqte();
        var jqteStatus = true;
        var jqtToolbar = true;
        ClientConfiguration.LoadData();      

        $('#txtFactorialization').keypress(function (event) {
            return isNumber(event, this)
        });


        function isNumber(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (
            (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
            (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
            (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        $('#txtFactorialization').bind("cut copy paste", function (e) {
            e.preventDefault();
        });

        $('#txtFactorialization').focus(function () {
            this.selectionStart = this.selectionEnd = -1;
        });


        $("#btnClientConfiguration").click(function () {
            if (appUser.IsAdmin == "True") {
                $('.jqte_editor').attr('contenteditable', true);
                $('#accessControl').attr('style', 'display:block;');
                $('#chkIsL3Auto').removeAttr('disabled');
                $('#chkIsL2Auto').removeAttr('disabled');
                $('#chkIsL1Auto').removeAttr('disabled');
                $('#txtFactorialization').removeAttr('disabled');

                $('#editAccessControl').attr('style', 'display:none;');
            }
            else {
                $('#accessControl').attr('style', 'display:none;');
            }

        });

        $("#btnUpdateClientConfiguration").click(function () {
            ClientConfiguration.SaveData();
        });

        $("#btnCancelClientConfiguration").click(function () {
            ClientConfiguration.ResetAll();
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}


ClientConfiguration.ResetAll = function () {

    ClientConfiguration.LoadData();
    $('#accessControl').attr('style', 'display:none;');
    $('#editAccessControl').attr('style', 'display:block;');
    $('#chkIsL3Auto').attr("disabled", "disabled");
    $('#chkIsL2Auto').attr("disabled", "disabled");
    $('#chkIsL1Auto').attr("disabled", "disabled");
    $('#txtFactorialization').attr("disabled", "disabled");
    //may11
}

ClientConfiguration.SaveData = function () {

    var chkIsL1Auto = $('#chkIsL1Auto').attr('checked') ? 1 : 0;
    var chkIsL2Auto = $('#chkIsL2Auto').attr('checked') ? 1 : 0;
    var chkIsL3Auto = $('#chkIsL3Auto').attr('checked') ? 1 : 0;
    var homePageText = $('.jqte_editor').html();
    var Factorialization = $('#txtFactorialization').val();

        if (Factorialization.trim().length == 0) {
            Factorialization = 0.0;
        }


    //may11
    var vParam = "{ IsL1Auto:'" + chkIsL1Auto + "', IsL2Auto: '" + chkIsL2Auto + "', IsL3Auto: '" + chkIsL3Auto + "', homePageContent: '" + escape(homePageText) + "', factorialization: " + Factorialization + " }";
    ServiceBroker.callWebMethod(enumWebPage.ClientConfiguration, enumWebMethod.UpdateClientProjectInfo, vParam, 'ClientConfiguration.Updated', true, true);
}

ClientConfiguration.Updated = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
            Notification.Information(Notification.enumMsg.UpdateMessage);
            window.location.href = 'Default.aspx#Home.aspx';
            return false;
        }
        else {
            Notification.Information(Notification.enumMsg.UpdateErrorMsg);
        }
    }
    else {
        Notification.Error(Notification.enumMsg.UpdateErrorMsg);
    }

}

ClientConfiguration.LoadData = function (msg) {
    var iParam = "{ clientProjectId:'" + appUser.ClientProjectId + "'}";
    ServiceBroker.callWebMethod(enumWebPage.ClientConfiguration, enumWebMethod.GetClientConfiguration, iParam, 'ClientConfiguration.BindPending', true, true);
}

ClientConfiguration.BindPending = function (msg) {
    if (hasNoError(msg, true)) {

        var IsL1Auto = msg[1];
        var IsL2Auto = msg[2];
        var IsL3Auto = msg[3];

        $('#lblClientProject').text(msg[0]);

        if (IsL1Auto == 'True') { $('#chkIsL1Auto').prop('checked', true); }
        if (IsL1Auto == 'False') { $('#chkIsL1Auto').prop('checked', false); }

        if (IsL2Auto == 'True') { $('#chkIsL2Auto').prop('checked', true); }
        if (IsL2Auto == 'False') { $('#chkIsL2Auto').prop('checked', false); }

        if (IsL3Auto == 'True') { $('#chkIsL3Auto').prop('checked', true); }
        if (IsL3Auto == 'False') { $('#chkIsL3Auto').prop('checked', false); }
        $('.jqte_editor').html(msg[4]);
        $('.jqte_editor').attr('contenteditable', false);
        $('#txtFactorialization').val(msg[5]);
    }
}