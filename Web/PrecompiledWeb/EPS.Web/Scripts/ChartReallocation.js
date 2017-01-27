var ChartReallocation = {};

ChartReallocation.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ChartReallocation, enumWebMethod.IsUserAuthorized, uParam, 'ChartReallocation.isAuthorised', true, true);
}

ChartReallocation.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $("#lblClientReferenceHeading").html(appUser.ClientReferenceLabel);
        $("#lblEditClientReferenceHeading").html(appUser.ClientReferenceLabel);
        ChartReallocation.EditBoxHide();
        ChartReallocation.PreparePendingCharts();

        $('#btnUpdate').click(function () {
            if ($('#Employeedrp').val().trim().length == 0) {
                Notification.Error(Notification.enumMsg.ReallocationEmployeeNotChosen);
                return false;
            }
            //Condition
            var sParam = "{ chartId : " + $('#hdnChartId').val() + ", assignToEmployeeId : " + $('#Employeedrp').val() + ", clientProjectId : " + $('#hdnClientProjectId').val() + ", levelStatus : '" + $('#Status').val() + "', previousUserName : '" + $('#hdnUserName').val() + "' }";
            ServiceBroker.callWebMethod(enumWebPage.ChartReallocation, enumWebMethod.AssignChartWithEmployee, sParam, 'ChartReallocation.ChartAssigned', true, true);
        });

        $("#btnReset").click(function () {
            $("#ChartReallocationDisplay").show();
            $("#ChartReallocationEditBox").hide();
            $("#Employeedrp").val('');
        });

        $("#txtFromdate").datepicker();

        $("#txtToDate").datepicker();
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

ChartReallocation.ChartAssigned = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
            ChartReallocation.EditBoxHide();
            ChartReallocation.PrepareBindEmployee();

            Notification.Information(Notification.enumMsg.UpdateMessage);
        }
        else {
            Notification.Error(Notification.enumMsg.RecordNotFoundMessage);
        }
    }
    else {
        Notification.Error(Notification.enumMsg.UpdateErrorMsg);
    }
}

ChartReallocation.PreparePendingCharts = function () {
    ChartReallocation.oTable = $('#tblWIPCharts').dataTable({
        "sScrollX": "100%",
        "aoColumns": [
                  { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "8%", "bSortable": false,
                      "fnRender": function (o, val) {
                          return "<a style='cursor:hand; color:blue;' href='javascript:EditChartReallocation(\"" + o.aData[1] + "\",\"" + escape(o.aData[2]) + "\",\"" + o.aData[3] + "\",\"" + escape(o.aData[4]) + "\",\"" + escape(o.aData[5]) + "\",\"" + o.aData[6] + "\",\"" + o.aData[7] + "\",\"" + o.aData[8] + "\",\"" + o.aData[9] + "\",\"" + o.aData[10] + "\",\"" + o.aData[11] + "\",\"" + o.aData[12] + "\",\"" + o.aData[13] + "\",\"" + o.aData[14] + "\");'><img src='Images/Edit.png' alt='Edit' /></a>";
                      }
                  },
			            { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "16%" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "10%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "15%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign",  "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "11%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "19%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false }
		            ],

        "iDisplayLength": 30,
        "aaSorting": [[2, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');

    ChartReallocation.PrepareBindEmployee();
}

ChartReallocation.PrepareBindEmployee = function () {
    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' }); 
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.ChartReallocation, enumWebMethod.getWIPChartData, sParam, 'ChartReallocation.BindPendingEmployee', true, true);
}

ChartReallocation.BindPendingEmployee = function (msg) {
    if (hasNoError(msg, true)) {
        ChartReallocation.oTable.fnClearTable();
        ChartReallocation.oTable.fnAddData(msg);
        // unblock when ajax activity stops 
        $(document).ajaxStop($.unblockUI); 
    }
}

ChartReallocation.EditBoxHide = function () {
    var selectedEffect = "blind";

    var options = {};

    $("#ChartReallocationDisplay").show();
    $("#ChartReallocationEditBox").hide();
}

ChartReallocation.EditBoxShow = function () {
    var selectedEffect = "blind";

    var options = {};

    $("#ChartReallocationDisplay").hide();
    $("#ChartReallocationEditBox").show();
}

function EditChartReallocation(chartInfoId, clientReference, receivedDate, clientMarket, fileName, clientProject, status, charMoreInfoId, userName, assignedTo, userId, clientProjectId, levelNumber, prevUserName) {
    ChartReallocation.EditBoxShow();

    $('#hdnChartId').val(chartInfoId);
    $('#ClientReferenceId').val(unescape(clientReference));
    $('#ReceivedDate').val(receivedDate);
    $('#Market').val(unescape(clientMarket));
    $('#FileName').val(unescape(fileName));
    $('#ClientProjectName').val(clientProject);
    $('#Status').val(status);
    $('#hdnUserName').val(userName);
    $('#hdnClientProjectId').val(clientProjectId);
    $('#hdnLevelNumber').val(levelNumber);
    $('#Employeedrp').html('');

    $('#Employeedrp option:nth(0)').attr("selected", "selected");
    $("#Employeedrp").trigger("liszt:updated");
    ChartReallocation.PrepareEmployeeDropdown(clientProjectId, userName, prevUserName);
    $('.chzn-drop .chzn-search input[type="text"]').focus();
}

ChartReallocation.PrepareEmployeeDropdown = function (clientProjectId, username, prevUserName) {
    var sParam = "{ clientProjectId : " + clientProjectId + ", levelNumber : " + $('#hdnLevelNumber').val() + ", chartUserName : '" + username + "', prevUserName: '" + prevUserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ChartReallocation, enumWebMethod.getEmployeeData, sParam, 'ChartReallocation.BindEmployeeDropdown', true, true);
}

ChartReallocation.BindEmployeeDropdown = function (msg) {
    $('#Employeedrp').append($("<option></option>").val("").html("--Select One--"));
    $("#Employeedrp").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#Employeedrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#Employeedrp").trigger("liszt:updated");
    });
}