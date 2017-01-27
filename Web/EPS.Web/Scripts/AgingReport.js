var AgingReport = {};
AgingReport.oTable1 = null;
AgingReport.oTable_Incomplete = null;
AgingReport.oTable_Completed = null;

AgingReport.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.AgingReport, enumWebMethod.IsUserAuthorized, uParam, 'AgingReport.isAuthorised', true, true);
}

AgingReport.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
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

        if (currentDate.getMonth() == 1) {
            $("#txtFromdate").val(currentDate.getDate().toString() + '/' + month[10] + '/' + (currentDate.getFullYear() - 1).toString());
        }
        else if ((currentDate.getMonth() == 2)) {
            $("#txtFromdate").val(currentDate.getDate().toString() + '/' + month[11] + '/' + (currentDate.getFullYear() - 1).toString());
        }
        else {
            $("#txtFromdate").val(currentDate.getDate().toString() + '/' + month[currentDate.getMonth() - 2] + '/' + currentDate.getFullYear().toString());
        }

        $("#txtTodate").datepicker({
            dateFormat: "dd/M/yy",
            setDate: currentDate
        });

        $("#txtTodate").val(currentDate.getDate().toString() + '/' + month[currentDate.getMonth()] + '/' + currentDate.getFullYear().toString());

        AgingReport.PrepareSummaryReport();
        AgingReport.PrepareAgingCompletedViewData();
        AgingReport.PrepareAgingIncompleteViewData();
        //AgingReport.LoadReportSummaryDetails();
        AgingReport.PrepareQueueBindStatus();

        $("#btnViewAgingReport").click(function () {
            AgingReport.LoadReportSummaryDetails();
        });

        $("#btnAgingCompletedReportViewClose").click(function () {
            $('#AgingReportCompletedViewDialog').dialog('close');
        });

        $("#btnAgingIncompleteReportViewClose").click(function () {
            $('#AgingReportIncompleteViewDialog').dialog('close');
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

AgingReport.PrepareQueueBindStatus = function () {
    var sParam = "{ userName: '" + appUser.UserName + "'}";
    ServiceBroker.callWebMethod(enumWebPage.AgingReport, enumWebMethod.GetQueue, sParam, 'AgingReport.BindQueueStatusDropdown', true, true);
}

AgingReport.BindQueueStatusDropdown = function (msg) {
    $('#Queuedrp').append($("<option></option>").val(""));
    $("#Queuedrp").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#Queuedrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#Queuedrp").trigger("liszt:updated");
    });
}

AgingReport.PrepareSummaryReport = function () {
    AgingReport.oTable1 = $('#tblAgingSummaryReport').dataTable({
        "bAutoWidth": true,
        "sScrollX": "",
        "bProcessing": true,
        "bFilter": false,
        "bPaginate": false,
        "bInfo": false,
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[1] > 0)
                                    return "<a style='cursor:hand; color:blue;' href='javascript:ViewAgingReportDetails(\"" + o.aData[6] + "\", 0);'>" + o.aData[1] + "</a>";
                                else
                                    return "0";
                            }
                        },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[2] > 0)
                                    return "<a style='cursor:hand; color:blue;' href='javascript:ViewAgingReportDetails(\"" + o.aData[6] + "\", 1);'>" + o.aData[2] + "</a>";
                                else
                                    return "0";
                            }
                        },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[3] > 0)
                                    return "<a style='cursor:hand; color:blue;' href='javascript:ViewAgingReportDetails(\"" + o.aData[6] + "\", 2);'>" + o.aData[3] + "</a>";
                                else
                                    return "0";
                            }
                        },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[4] > 0)
                                    return "<a style='cursor:hand; color:blue;' href='javascript:ViewAgingReportDetails(\"" + o.aData[6] + "\", 3);'>" + o.aData[4] + "</a>";
                                else
                                    return "0";
                            }
                        },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[5] > 0)
                                    return "<a style='cursor:hand; color:blue;' href='javascript:ViewAgingReportDetails(\"" + o.aData[6] + "\", 4);'>" + o.aData[5] + "</a>";
                                else
                                    return "0";
                            }
                        }
		            ],
        "iDisplayLength": 30,
        "aaSorting": [[0, 'desc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
}

AgingReport.LoadReportSummaryDetails = function (msg) {
    var qDrop = document.getElementById("Queuedrp");
    var queueStatus = qDrop.options[qDrop.selectedIndex].value;
    var queueStatusText = qDrop.options[qDrop.selectedIndex].text;
    if (queueStatus == "") {
        Notification.Information(Notification.enumMsg.QueueStatus);
        return false;
    }
    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{ fromDate: '" + $('#txtFromdate').val() + "', toDate: '" + $('#txtTodate').val() + "', queueID: '" + queueStatus + "', userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.AgingReport, enumWebMethod.GetAgingSummaryReport, sParam, 'AgingReport.BindReportSummaryData', true, true);
}

AgingReport.BindReportSummaryData = function (msg) {
    if (hasNoError(msg, true)) {
        AgingReport.oTable1.fnClearTable();
        AgingReport.oTable1.fnAddData(msg);

        AgingReport.oTable_Completed.fnClearTable();
        AgingReport.oTable_Incomplete.fnClearTable();
    }
    // unblock when ajax activity stops 
    $(document).ajaxStop($.unblockUI); 
}

ViewAgingReportDetails = function (isCompleted, dateRangeType) {
    var fromDate = $("#txtFromdate").val();
    var toDate = $('#txtTodate').val();
    var qDrop = document.getElementById("Queuedrp");
    var queueStatus = qDrop.options[qDrop.selectedIndex].value;
    var queueStatusText = qDrop.options[qDrop.selectedIndex].text;
    if (queueStatus == "") {
        Notification.Information(Notification.enumMsg.QueueStatus);
        return false;
    }
    var vParam = "{ isCompleted: '" + isCompleted + "', fromDate: '" + fromDate + "', toDate: '" + toDate + "', dateRangeType: '" + dateRangeType + "', queueID: '" + queueStatus + "', userName: '" + appUser.UserName + "'}";
    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    if (isCompleted == "True") {
        $("#AgingReportCompletedViewDialog").dialog({
            modal: true,
            height: 700,
            width: 1200
        });

        switch (dateRangeType) {
            case 0:
                $('#lblCompletedDateRange').html(" <= 7 days");
                break;
            case 1:
                $('#lblCompletedDateRange').html(" 8 - 14 days");
                break;
            case 2:
                $('#lblCompletedDateRange').html(" 15 - 21 days");
                break;
            case 3:
                $('#lblCompletedDateRange').html(" 22 - 28 days");
                break;
            default:
                $('#lblCompletedDateRange').html(" 28+ days");
                break;
        }

        ServiceBroker.callWebMethod(enumWebPage.AgingReport, enumWebMethod.GetAgingReportForCompleted, vParam, 'AgingReport.BindAgingViewCompletedData', true, true);
    }
    else {
        $("#AgingReportIncompleteViewDialog").dialog({
            modal: true,
            height: 700,
            width: 1300
        });

        switch (dateRangeType) {
            case 0:
                $('#lblIncompleteDateRange').html(" <= 7 days");
                break;
            case 1:
                $('#lblIncompleteDateRange').html(" 8 - 14 days");
                break;
            case 2:
                $('#lblIncompleteDateRange').html(" 15 - 21 days");
                break;
            case 3:
                $('#lblIncompleteDateRange').html(" 22 - 28 days");
                break;
            default:
                $('#lblIncompleteDateRange').html(" 28+ days");
                break;
        }

        ServiceBroker.callWebMethod(enumWebPage.AgingReport, enumWebMethod.GetAgingReportForIncomplete, vParam, 'AgingReport.BindAgingViewIncompleteData', true, true);
    }
}

AgingReport.BindAgingViewCompletedData = function (msg) {
    if (hasNoError(msg, true)) {
        AgingReport.oTable_Completed.fnClearTable();
        AgingReport.oTable_Completed.fnAddData(msg);
    }
    // unblock when ajax activity stops 
    $(document).ajaxStop($.unblockUI); 
}

AgingReport.BindAgingViewIncompleteData = function (msg) {
    if (hasNoError(msg, true)) {
        AgingReport.oTable_Incomplete.fnClearTable();
        AgingReport.oTable_Incomplete.fnAddData(msg);
    }
    // unblock when ajax activity stops 
    $(document).ajaxStop($.unblockUI); 
}

AgingReport.PrepareAgingCompletedViewData = function () {
    AgingReport.oTable_Completed = $('#tblAgingCompletedReportView').dataTable({
        "bAutoWidth": true,
        "sScrollX": "",
        "bProcessing": true,
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false }
		            ],
        "iDisplayLength": 30,
        "aaSorting": [[0, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
}

AgingReport.PrepareAgingIncompleteViewData = function () {
    AgingReport.oTable_Incomplete = $('#tblAgingIncompleteReportView').dataTable({
        "bAutoWidth": true,
        "sScrollX": "",
        "bProcessing": true,
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "bSortable": false }
		            ],
        "iDisplayLength": 30,
        "aaSorting": [[0, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
}
