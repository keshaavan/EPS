var ChartBulkReallocation = {};

var chartDateType, fromDate, toDate;

ChartBulkReallocation.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ChartBulkReallocation, enumWebMethod.IsUserAuthorized, uParam, 'ChartBulkReallocation.isAuthorised', true, true);
}
ChartBulkReallocation.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
       // alert('hi');
        $("#lblClientReferenceHeading").html(appUser.ClientReferenceLabel);

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

        ChartBulkReallocation.PrepareCharts();

        $("#FromStatusdrp").change(function () {
            ChartBulkReallocation.loadToStatusInfo();
        });

        $("#btnMoved").click(function () {
            ChartBulkReallocation.MoveChartStatusInfo();


        });
        $("#btnChartStatus").click(function () {
            ChartBulkReallocation.ChartStatusInfo();
        });

        ChartBulkReallocation.loadEmployeeInfo();
        ChartBulkReallocation.loadFromStatusInfo();
    }
    else {
        window.location.href = "Logout.aspx";
    }
}


ChartBulkReallocation.PrepareCharts = function () {
    ChartBulkReallocation.oTable = $('#tblCharts').dataTable({
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

}
ChartBulkReallocation.LoadChartData = function () {
    // $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });

    var dataType = $("#dateTypeDropdownList option:selected").val();
    var fromDate = $('#txtFromdate').val();
    var toDate = $('#txtTodate').val();
    var username = $("#EmployeeDropdownList option:selected").val();
    var fromstatusId = $("#FromStatusdrp option:selected").val();
    var tostatusId = $("#ToStatusdrp option:selected").val();

    var sParam = "{ datatype:" + dataType + ", fromDate: '" + fromDate + "', toDate: '" + toDate + "', username: '" + username + "', fromstatusId: '" + fromstatusId + "'}";
    ServiceBroker.callWebMethod(enumWebPage.ChartBulkReallocation, enumWebMethod.LoadChartData, sParam, 'ChartBulkReallocation.BindCharts', true, true);
}

ChartBulkReallocation.BindCharts = function (msg) {
    if (hasNoError(msg, true)) {
        ChartBulkReallocation.oTable.fnClearTable();
        ChartBulkReallocation.oTable.fnAddData(msg);
        // unblock when ajax activity stops 
        ChartBulkReallocation.PrepareCharts();
        $(document).ajaxStop($.unblockUI);
    }
}


ChartBulkReallocation.MoveChartStatusInfo = function () {

    var e = document.getElementById("FromStatusdrp");
    var fromStatus = e.options[e.selectedIndex].value;
    var fromStatusText = e.options[e.selectedIndex].text;

    var qDrop = document.getElementById("ToStatusdrp");
    var toStatus = qDrop.options[qDrop.selectedIndex].value;
    var toStatusText = qDrop.options[qDrop.selectedIndex].text;

//    if (fromStatus == "" && fromStatusText == "") {
//        Notification.Information(Notification.enumMsg.RequiredFromStatus);
//        return false;
//    }
//    if (toStatus == "") {
//        Notification.Information(Notification.enumMsg.ToStatus);
//        return false;
//    }
//    if (toStatusText == "--All Statuses--") {
//        Notification.Information(Notification.enumMsg.ToStatus);
//        return false;
//    }

}
ChartBulkReallocation.ChartStatusInfo = function () {
    var e = document.getElementById("FromStatusdrp");
    var fromStatus = e.options[e.selectedIndex].value;
    var fromStatusText = e.options[e.selectedIndex].text;

    var qDrop = document.getElementById("ToStatusdrp");
    var toStatus = qDrop.options[qDrop.selectedIndex].value;
    var toStatusText = qDrop.options[qDrop.selectedIndex].text;

//    if (fromStatus == "" && fromStatusText == "") {
//        Notification.Information(Notification.enumMsg.RequiredFromStatus);
//        return false;
//    }

//    if (toStatus == "") {
//        Notification.Information(Notification.enumMsg.ToStatus);
//        return false;
//    }
//    if (toStatusText == "--All Statuses--") {
//        Notification.Information(Notification.enumMsg.ToStatus);
//        return false;
//    }
    ChartBulkReallocation.LoadChartData();

}

ChartBulkReallocation.loadFromStatusInfo = function () {

    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.ChartBulkReallocation, enumWebMethod.getChartStatuses, sParam, 'ChartBulkReallocation.BindFromStatusDropdown', true, true);
}

ChartBulkReallocation.BindFromStatusDropdown = function (msg) {
    $('#FromStatusdrp').append($("<option></option>").val("").html("--All Statuses--"));
    $("#FromStatusdrp").trigger("liszt:updated");
    //$('#FromStatusdrp').append($("<option></option>").val("0|1000").html("Unassigned"));
    //$("#FromStatusdrp").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#FromStatusdrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#FromStatusdrp").trigger("liszt:updated");
    });
    //    $('#FromStatusdrp').append($("<option></option>").val("0|1001").html("Completed"));
    //    $("#FromStatusdrp").trigger("liszt:updated");
}


ChartBulkReallocation.loadToStatusInfo = function (id) {
    var statusId = $("#FromStatusdrp option:selected").val();
    var sParam = "{ id: " + statusId + "}";
    ServiceBroker.callWebMethod(enumWebPage.ChartBulkReallocation, enumWebMethod.getCharttoStatuses, sParam, 'ChartBulkReallocation.BindToStatusDropdown', true, true);
}

ChartBulkReallocation.BindToStatusDropdown = function (msg) {
    $('#ToStatusdrp').empty();
    $('#ToStatusdrp').append($("<option></option>").val("").html("--All Statuses--"));
    $("#ToStatusdrp").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#ToStatusdrp').append($("<option></option>").html(item[0]));
        $("#ToStatusdrp").trigger("liszt:updated");
    });
}





ChartBulkReallocation.loadEmployeeInfo = function () {
    //    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{}";
    ServiceBroker.callWebMethod(enumWebPage.ChartBulkReallocation, enumWebMethod.getEmployeeInfo, sParam, 'ChartBulkReallocation.BindEmployeeDropdown', true, true);
}

ChartBulkReallocation.BindEmployeeDropdown = function (msg) {
    $('#EmployeeDropdownList').append($("<option></option>").val("").html("--All Employees--"));
    $("#EmployeeDropdownList").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#EmployeeDropdownList').append($("<option></option>").val(item[0]).html(item[1]));
        $("#EmployeeDropdownList").trigger("liszt:updated");
    });

    $('#EmployeeDropdownList').find('option[value=""]').attr('selected', 'selected');
    $('#EmployeeDropdownList').trigger("liszt:updated");

    //ChartBulkReallocation.PrepareChartBulkReallocation();

}