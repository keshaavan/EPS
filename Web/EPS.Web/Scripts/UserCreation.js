var UserCreation = {};

UserCreation.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.IsUserAuthorized, uParam, 'UserCreation.isAuthorised', true, true);
}

UserCreation.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        UserCreation.PrepareEmployeeInfo();
        UserCreation.EditBoxLoadHide();

        UserCreation.BindClientProject();
        UserCreation.BindLocation();

        $("#btnReset").click(function () {
            UserCreation.ResetField();
            UserCreation.EditBox();
            UserCreation.jqueryDataTableBoxShow();
        });
        $("#btnCancel").click(function () {
            // UserCreation.ResetField();
            // UserCreation.EditBox();
            //UserCreation.jqueryDataTableBoxShow();
            $('#dvResetPassword').hide("slide", { direction: "left" }, 500);
            UserCreation.PasswordResetField();
        });

        $("#btnUpdate").click(function () {
            UserCreation.SaveData();
        });

        $("#btnResetpassword").click(function () {
            UserCreation.resetNewPassword();
        });

        $("#btnUnlockAccount").click(function () {
            UserCreation.UnlockAccount();
        });

        $('#FirstName').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match(/^\s*[a-zA-Z0-9,\s]+\s*$/))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });

        $('#LastName').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match(/^\s*[a-zA-Z0-9,\s]+\s*$/))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });

        $("#txtpassword").keydown(function (event) {
            if (event.keyCode == 32) {
                event.preventDefault();
            }
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

UserCreation.PrepareEmployeeInfo = function () {
    UserCreation.oTable = $('#tblEmployeeinfo').dataTable({
        "sScrollX": "100%",
        "aoColumns": [
                       { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "2%", "bSortable": false,
                           "fnRender": function (o, val) {
                               if (o.aData[0].toString() == 'Edit')
                               
                                   return "<a style='cursor:hand;' href='javascript:UpdatePrepare(\"" + o.aData[2].toString() + "\",\"" + o.aData[3].toString() + "\",\"" + o.aData[4].toString() + "\",\"" + o.aData[5].toString() + "\",\"" + o.aData[6].toString() + "\",\"" + o.aData[7].toString() + "\",\"" + o.aData[8].toString() + "\",\"" + o.aData[9].toString() + "\",\"" + o.aData[10].toString() + "\",\"" + o.aData[11].toString() + "\",\"" + o.aData[12].toString() + "\",\"" + o.aData[13].toString() + "\",\"" + o.aData[14].toString() + "\",\"" + o.aData[15].toString() + "\");'><img src='Images/Edit.png' alt='Edit' /></a>";
                               else
                                   return o.aData[0];
                           }
                       },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "2%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[1].toString() == 'Reset')
                                    return "<a style='cursor:hand;' href='javascript:passwordUpdatePrepare(\"" + o.aData[3].toString() + "\");'>" + o.aData[1] + "</a>";
                                else
                                    return o.aData[1];
                            }
                        },
			            { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "5%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "5%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "5%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "14%" },
                        //{ "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "4%" },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "5%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "5%" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "5%" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "5%" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "5%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "5%" }
		            ],

        "iDisplayLength": 25,
        "aaSorting": [[2, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Prospects",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Prospects",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
    UserCreation.LoadData();
}

UserCreation.LoadData = function (msg) {
    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.getEmployeeData, sParam, 'UserCreation.BindPendingEmployee', true, true);
    $(document).ajaxStop($.unblockUI);
}

UserCreation.BindPendingEmployee = function (msg) {
    if (hasNoError(msg, true)) {
        UserCreation.oTable.fnClearTable();
        UserCreation.oTable.fnAddData(msg);
    }
}

UserCreation.UnlockAccount = function () {
    var userName = $('#UserName').val();

    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{ userName:'" + userName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.UnlockAccount, sParam, 'UserCreation.UnlockAccountSuccess', true, true);
    $(document).ajaxStop($.unblockUI);

}

UserCreation.UnlockAccountSuccess = function () {
    Notification.Information(Notification.enumMsg.AccountUnlockedMessage);
    UserCreation.EditBox();
    UserCreation.jqueryDataTableBoxShow();

    UserCreation.LoadData();
}

UserCreation.SaveData = function () {
    var employeeId = $('#Employeeid').val();
    var firstname = $.trim($('#FirstName').val());
    var lastname = $.trim($('#LastName').val());

    var e = document.getElementById("ClientProjectdrp");
    var clientprojectid = e.options[e.selectedIndex].value;

    var e = document.getElementById("Locationdrp");
    var locationid = e.options[e.selectedIndex].value;

    var e = document.getElementById("LevelNumberdrp");
    var levelNo = e.options[e.selectedIndex].value;

    var IsAdmin = $('#chkIsAdmin').attr('checked') ? 1 : 0;
    var IsReportUser = $('#chkIsReportUser').attr('checked') ? 1 : 0;
    var IsActive = $('#chkIsActive').attr('checked') ? 1 : 0;

    if (employeeId.trim() == '') {
        Notification.Information(Notification.enumMsg.RequiredEmployeeId);
        return false;
    }
    else if (firstname == '') {
        Notification.Information(Notification.enumMsg.RequiredFirstName);
        return false;
    }
    else if (lastname == '') {
        Notification.Information(Notification.enumMsg.RequiredLastName);
        return false;
    }
    else if (clientprojectid.trim() == '') {
        Notification.Information(Notification.enumMsg.RequiredProject);
        return false;
    }
    else if (locationid.trim() == '') {
        Notification.Information(Notification.enumMsg.RequiredLocation);
        return false;
    }
    else if (levelNo.trim() == '') {
        Notification.Information(Notification.enumMsg.RequiredEmployeeLevel);
        return false;
    }
    else {
        var vParam = "{ employeeId:'" + employeeId + "', firstName: '" + firstname + "', lastName: '" + lastname + "', locationId: '" + locationid + "', levelNumber: '" + levelNo + "', clientProjectId: '" + clientprojectid + "', isAdmin: '" + IsAdmin + "', isReportUser: '" + IsReportUser + "', isActive: '" + IsActive + "'}";

        ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.UpdateEmployeeinfo, vParam, 'UserCreation.UpdateEmployee', true, true);
        UserCreation.BindEmployee_Refer();
    }
}

UserCreation.BindEmployee_Refer = function () {
    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.getEmployeeData, sParam, 'UserCreation.BindPendingEmployee', true, true);
    $(document).ajaxStop($.unblockUI);
}

UserCreation.UpdateEmployee = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
            Notification.Information(Notification.enumMsg.UpdateMessage);
            UserCreation.EditBoxLoadHide();
            UserCreation.jqueryDataTableBoxShow();
        }
        else {
            Notification.Information(Notification.enumMsg.UpdateErrorMsg);
        }
    }
    else {
        Notification.Error(Notification.enumMsg.UpdateErrorMsg);
    }
}

//----------------------------Show and Hide--------------------------------
UserCreation.EditBoxLoadHide = function () {
    $("#effect").hide();
}

UserCreation.EditBox = function () {
    var selectedEffect = "blind";

    var options = {};
    $("#effect").hide(selectedEffect, options, 500);
}

UserCreation.EditBoxShow = function () {

    var selectedEffect = "blind";

    var options = {};
    $("#effect").show(selectedEffect, options, 500);
}

UserCreation.jqueryDataTableBoxHide = function () {
    var selectedEffect = "blind";

    var options = {};
    $("#EmployeeinfoContainer").hide(selectedEffect, options, 500);
}

UserCreation.jqueryDataTableBoxShow = function () {
    var selectedEffect = "blind";

    var options = {};
    $("#EmployeeinfoContainer").show(selectedEffect, options, 500);
}

function passwordUpdatePrepare(username) {
    $('#dvResetPassword').show("slide", { direction: "left" }, 500);
    $('#txtusername').val(username);
    $('#txtpassword').focus();
}

UserCreation.resetNewPassword = function () {
    var userName = $.trim($('#txtusername').val());
    var newPassword = $.trim($('#txtpassword').val());

    var passwordvalidation = /^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+=]).*$/;
    if (userName == '') {
        Notification.Information(Notification.enumMsg.RequiredUserName);
        return false;
    }
    else if (newPassword.length == 0) {
        Notification.Information(Notification.enumMsg.RequiredNewPassword);
        return false;
    }
    else if (!passwordvalidation.test(newPassword)) {
        Notification.Information(Notification.enumMsg.PasswordFormatError);
        $('#txtpassword').val('');
        $('#txtpassword').focus();
        return false;

    }
    else {
        var sParam = "{ userName:'" + userName + "', newPassword: '" + newPassword + "'}";
        ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.ResetPassword, sParam, 'UserCreation.resetPasswordSuccess', true, true);
    }
}

UserCreation.resetPasswordSuccess = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
            $('#dvResetPassword').hide("slide", { direction: "left" }, 500);
            Notification.Information(Notification.enumMsg.PasswordResetMessage);
        }
        else {
            Notification.Error(msg[0]);
        }
    }
    else {
        Notification.Error(Notification.enumMsg.UpdateErrorMsg);
    }
}


function UpdatePrepare(Id, username, firstName, lastName, clientProject, queue, location, levelNumber, isAdmin, isReportUser, isActive, empId, locationId, isLockedOut) {
    UserCreation.jqueryDataTableBoxHide();
    UserCreation.EditBoxShow();
    UserCreation.ResetField();

    $('#FirstName').focus();
    $('#Employeeid').val(empId);
    $('#UserName').val(username);
    $('#FirstName').val(firstName);
    $('#LastName').val(lastName);

    if (isAdmin == 'True') { $('#chkIsAdmin').prop('checked', true); }
    if (isAdmin == 'False') { $('#chkIsAdmin').prop('checked', false); }

    if (isReportUser == 'True') { $('#chkIsReportUser').prop('checked', true); }
    if (isReportUser == 'False') { $('#chkIsReportUser').prop('checked', false); }

    if (isActive == 'True') { $('#chkIsActive').prop('checked', true); }
    if (isActive == 'False') { $('#chkIsActive').prop('checked', false); }    

    $('#LevelNumberdrp').find('option[value="' + levelNumber + '"]').attr('selected', 'selected');
    $('#LevelNumberdrp').trigger("liszt:updated");
        
    $('#ClientProjectdrp').find('option[value=' + Id + ']').attr('selected', 'selected');
    $('#ClientProjectdrp').trigger("liszt:updated");

    $('#Locationdrp').find('option[value=' + locationId + ']').attr('selected', 'selected');
    $('#Locationdrp').trigger("liszt:updated");

    if (isLockedOut != 'True') {
        $("#btnUnlockAccount").css('visibility', 'hidden');
    }
    else {
        $("#btnUnlockAccount").css('visibility', 'visible');
    }
}




UserCreation.ResetField = function () {
    $('#Employeeid').val('');
    $('#UserName').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $("#Locationdrp").val('');
    $('#LevelNumberdrp').val('');
    $('#ClientProjectdrp').val('');
}

UserCreation.PasswordResetField = function () {
    $('#txtusername').val('');
    $('#txtpassword').val('');
    $('#txtpassword').focus();
}

//----------------------Binding Dropdownlist---------------------------
UserCreation.BindClientProject = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.getClientProjectData, sParam, 'UserCreation.ClientProjectBindedSuccess', true, true);
}

UserCreation.BindLocation = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.getLocationtData, sParam, 'UserCreation.LocationBindedSuccess', true, true);
}

UserCreation.ClientProjectBindedSuccess = function (msg) {
    $("select[id=ClientProjectdrp] > option").remove();

    $.each(msg, function (index, item) {
        $('#ClientProjectdrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#ClientProjectdrp").trigger("liszt:updated");
    });
}

UserCreation.LocationBindedSuccess = function (msg) {
    $("select[id=Locationdrp] > option").remove();
    $.each(msg, function (index, item) {
        $('#Locationdrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#Locationdrp").trigger("liszt:updated");
    });
}
