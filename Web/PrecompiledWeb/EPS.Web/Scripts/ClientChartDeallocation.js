var ClientChartDeallocation = {};

ClientChartDeallocation.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ClientChartDeallocation, enumWebMethod.IsUserAuthorized, uParam, 'ClientChartDeallocation.isAuthorised', true, true);
}

ClientChartDeallocation.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $("#lblClientReferenceHeading").html(appUser.ClientReferenceLabel);
        ClientChartDeallocation.EditBoxHide();

        $('#chkSelectAll').click(function (evt) {
            $('#tblCharts').find(':checkbox').attr('checked', this.checked);
        });
        var currentDate = new Date();
        var month = new Array();
        month[0] = "Jan";
        month[1] = "Feb";
        month[2] = "Mar";
        month[3] = "Apr";
        month[4] = "May";
        month[5] = "Jun";
        month[6] = "Jul";
        month[7] = "Aug";
        month[8] = "Sep";
        month[9] = "Oct";
        month[10] = "Nov";
        month[11] = "Dec";

        $("#txtFromdate").datepicker({
            dateFormat: "dd/M/yy",
            setDate: currentDate
        });

        $("#txtFromdate").val(currentDate.getDate().toString() + '/' + month[currentDate.getMonth()] + '/' + currentDate.getFullYear().toString());

        $("#txtTodate").datepicker({
            dateFormat: "dd/M/yy",
            setDate: currentDate
        });

        $("#txtTodate").val(currentDate.getDate().toString() + '/' + month[currentDate.getMonth()] + '/' + currentDate.getFullYear().toString());
        ClientChartDeallocation.PrepareCharts();

        $("#btnDelete").click(function () {
            ClientChartDeallocation.DeleteCharts();
        });

        $("#btnChartStatus").click(function () {
            ClientChartDeallocation.PrepareBindCharts();
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

ClientChartDeallocation.PrepareCharts = function () {
    ClientChartDeallocation.oTable = $('#tblCharts').dataTable({
        "sScrollX": "100%",
        "aoColumns": [
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "8%", "bSortable": false,
                            "fnRender": function (o, val) {
                                return "<input id='" + o.aData[1] + "' class='loan-checkbox' data-clientChartid='" + o.aData[1] + "' type='checkbox' >";
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "12%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "19%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "18%" }
		            ],
        "iDisplayLength": 100,
        "aaSorting": [[1, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');

    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });

    var sParam = "{ fromDate: '" + $('#txtFromdate').val() + "', toDate: '" + $('#txtTodate').val() + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ClientChartDeallocation, enumWebMethod.getWIPClientChartData, sParam, 'ClientChartDeallocation.BindCharts', true, true);
}

ClientChartDeallocation.PrepareBindCharts = function () {
    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    $('#chkSelectAll').prop('checked', false);
    var sParam = "{ fromDate: '" + $('#txtFromdate').val() + "', toDate: '" + $('#txtTodate').val() + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ClientChartDeallocation, enumWebMethod.getWIPClientChartData, sParam, 'ClientChartDeallocation.BindCharts', true, true);
}

ClientChartDeallocation.BindCharts = function (msg) {
    if (hasNoError(msg, true)) {
        ClientChartDeallocation.oTable.fnClearTable();
        ClientChartDeallocation.oTable.fnAddData(msg);
        // unblock when ajax activity stops 
        $(document).ajaxStop($.unblockUI);
    }
}

ClientChartDeallocation.EditBoxHide = function () {
    var selectedEffect = "blind";
    var options = {};

    $("#ChartDeallocationDisplay").show();
    $("#ChartReallocationEditBox").hide();
}

ClientChartDeallocation.DeleteCharts = function () {
    var selectedCheckBox = '';
    var control = $('input:checkbox')
    var checkAll = 0;

    if ($('#chkSelectAll').prop('checked') == true) {
        $('#chkSelectAll').prop('checked', false);
        checkAll = 1;
    }

    for (var i = 0; i < control.length; i++) {
        var ctrlVal = "|" + control[i].value + "|";
        var isChecked = $('#' + control[i].id + '').attr('checked') ? 1 : 0;
        if (isChecked == 1) {
            var ctrlid = "" + control[i].id + "|";
            selectedCheckBox = selectedCheckBox + ctrlid;
        }
    }

    if (checkAll == 1)
        $('#chkSelectAll').prop('checked', true);

    if (selectedCheckBox != '') {
        if (confirm("Alert\n---------------------------------------------------------------\n All Processing information of these charts will be permanently deleted and cannot be recovered.\n\n  Do you want to continue?\n---------------------------------------------------------------", "Confirm Delete")) {
            var vParam = "{ chartIds :'" + selectedCheckBox + "'}";
            ServiceBroker.callWebMethod(enumWebPage.ClientChartDeallocation, enumWebMethod.deleteChartList, vParam, 'ClientChartDeallocation.DeleteChartConfirm', true, true);
            Notification.Information(Notification.enumMsg.ChartDeletedSuccessfully);
        }
        else {
            ClientChartDeallocation.DeleteChartConfirm();
        }
    }
    else {
        Notification.Information(Notification.enumMsg.NoChartsSelectedForDelete);
        return false;
    }
}

ClientChartDeallocation.DeleteChartConfirm = function () {
    $('#delete-dialog').dialog("close");
    ClientChartDeallocation.PrepareBindCharts();
}