var ChartStatus = {};
ChartStatus.oTable = null;
ChartStatus.oTable2 = null;
ChartStatus.oTable3 = null;

var levelNumber, levelStatusId, chartStatus, chartDateType, fromDate, toDate;

ChartStatus.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ChartStatus, enumWebMethod.IsUserAuthorized, uParam, 'ChartStatus.isAuthorised', true, true);
}

ChartStatus.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $("#xlsImage_ChartStatus").click(function () {
            var e = document.getElementById("Statusdrp");
            var levelStatus = e.options[e.selectedIndex].value;

            var levelStatusText = e.options[e.selectedIndex].text;

            if (levelStatus == "" && levelStatusText == "") {
                Notification.Information(Notification.enumMsg.RequiredLevelStatus);
                return false;
            }

            var qDrop = document.getElementById("Queuedrp");
            var queueStatus = qDrop.options[qDrop.selectedIndex].value;
            var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

            if (queueStatus == "") {
                Notification.Information(Notification.enumMsg.QueueStatus);
                return false;
            }

            chartStatus = $('#Statusdrp').val();
            chartDateType = $('#dateTypeDropdownList').val();
            fromDate = $('#txtFromdate').val();
            toDate = $('#txtTodate').val()

            levelNumber = "";
            levelStatusId = "";
            queueId = $('#Queuedrp').val();

            if (chartStatus != "") {
                var chartStatusVals = chartStatus.split("|");
                levelNumber = chartStatusVals[0];
                levelStatusId = chartStatusVals[1];

                chartStatus = "";
            }

            if (chartDateType == 0 && levelStatusId == 1000) {
                //raise notification error if unassigned is chosen
                Notification.Error(Notification.enumMsg.UnassignedLeveStstusError);
                return false;
            }
            else {
                if (levelStatusId == 1000 || levelStatusId == 1001) {
                    if (levelStatusId == 1000)
                        chartStatus = "Unassigned";
                    else
                        chartStatus = "Completed";

                    levelNumber = "";
                    levelStatusId = "";
                }
            }

            var ws_ExcelGenerateURL = "Handler/ChartStatus.ashx";

            var sHTMLTag = "<form method='post' action='" + ws_ExcelGenerateURL + "' style='top:-3333333333px;' id='XlsDailyReport'>";
            sHTMLTag += "<input type='hidden' name='levelStatus' value='" + chartStatus + "' >";
            sHTMLTag += "<input type='hidden' name='fromDatedp' value='" + fromDate + "' >";
            sHTMLTag += "<input type='hidden' name='toDatedp' value='" + toDate + "' >";
            sHTMLTag += "<input type='hidden' name='chartDateType' value='" + chartDateType + "' >";
            sHTMLTag += "<input type='hidden' name='levelNumber' value='" + levelNumber + "' >";
            sHTMLTag += "<input type='hidden' name='levelStatusId' value='" + levelStatusId + "' >";
            sHTMLTag += "<input type='hidden' name='queueId' value='" + queueId + "' >";

            sHTMLTag += "</form>";
            $('body').prepend(sHTMLTag);
            $('#XlsDailyReport').submit();
            $("#XlsDailyReport").remove();
        });

        //////////////////////////////////////////////////////////////
        //xlsImage_ChartStatusComments
        $("#xlsImage_ChartStatusComments").click(function () {
            var e = document.getElementById("Statusdrp");
            var levelStatus = e.options[e.selectedIndex].value;

            var levelStatusText = e.options[e.selectedIndex].text;

            if (levelStatus == "" && levelStatusText == "") {
                Notification.Information(Notification.enumMsg.RequiredLevelStatus);
                return false;
            }
            var ws_ExcelGenerateURL = "Handler/ChartStatusComments.ashx";
            var fromDatedp = $('#txtFromdate').val()
            var toDatedp = $('#txtTodate').val()
            var qDrop = document.getElementById("Queuedrp");
            var queueStatus = qDrop.options[qDrop.selectedIndex].value;
            var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

            if (queueStatus == "") {
                Notification.Information(Notification.enumMsg.QueueStatus);
                return false;
            }

            var sHTMLTag = "<form method='post' action='" + ws_ExcelGenerateURL + "' style='top:-3333333333px;' id='XlsDailyReport'>";
            sHTMLTag += "<input type='hidden' name='fromDatedp' value='" + fromDatedp + "' >";
            sHTMLTag += "<input type='hidden' name='toDatedp' value='" + toDatedp + "' >";
            sHTMLTag += "<input type='hidden' name='queueDrp' value='" + queueStatus + "' >";
            sHTMLTag += "</form>";
            $('body').prepend(sHTMLTag);
            $('#XlsDailyReport').submit();
            $("#XlsDailyReport").remove();
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

        ChartStatus.PrepareCharts();
        ChartStatus.PrepareBindStatus();
        ChartStatus.PrepareChartAudit();
        ChartStatus.PrepareProductionTimeTaken();
        ChartStatus.PrepareQueueBindStatus();

        $("#clientReferenceHeading").html(appUser.ClientReferenceLabel);
        $("#btnChartStatus").click(function () {
            var e = document.getElementById("Statusdrp");
            var levelStatus = e.options[e.selectedIndex].value;
            var levelStatusText = e.options[e.selectedIndex].text;

            var qDrop = document.getElementById("Queuedrp");
            var queueStatus = qDrop.options[qDrop.selectedIndex].value;
            var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

            if (levelStatus == "" && levelStatusText == "") {
                Notification.Information(Notification.enumMsg.RequiredLevelStatus);
                return false;
            }

            if (queueStatus == "") {
                Notification.Information(Notification.enumMsg.QueueStatus);
                return false;
            }

            ChartStatus.LoadChartStatusData();
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

ChartStatus.PrepareBindStatus = function () {
    var sParam = "{ userName: '" + appUser.UserName + "'}";
    ServiceBroker.callWebMethod(enumWebPage.ChartStatus, enumWebMethod.getChartStatuses, sParam, 'ChartStatus.BindStatusDropdown', true, true);
}

ChartStatus.BindStatusDropdown = function (msg) {
    $('#Statusdrp').append($("<option></option>").val("").html("--All Statuses--"));
    $("#Statusdrp").trigger("liszt:updated");
    $('#Statusdrp').append($("<option></option>").val("0|1000").html("Unassigned"));
    $("#Statusdrp").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#Statusdrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#Statusdrp").trigger("liszt:updated");
    });
    $('#Statusdrp').append($("<option></option>").val("0|1001").html("Completed"));
    $("#Statusdrp").trigger("liszt:updated");
}


ChartStatus.PrepareQueueBindStatus = function () {
    var sParam = "{ userName: '" + appUser.UserName + "'}";
    ServiceBroker.callWebMethod(enumWebPage.ChartStatus, enumWebMethod.GetQueue, sParam, 'ChartStatus.BindQueueStatusDropdown', true, true);
}

ChartStatus.BindQueueStatusDropdown = function (msg) {
    $('#Queuedrp').append($("<option></option>").val(""));
    $("#Queuedrp").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#Queuedrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#Queuedrp").trigger("liszt:updated");
    });
}



ChartStatus.PrepareCharts = function () {
    ChartStatus.oTable = $('#tblChartStatus').dataTable({
        "bAutoWidth": false,
        "sScrollX": "100%",
        "bProcessing": false,
        "aoColumns": [
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": false,
                            "fnRender": function (o, val) {
                                return "<a style='cursor:hand; color:blue;' href='javascript:ViewChartAuditLogs(\"" + o.aData[27] + "\",\"" + o.aData[1] + "\");'>Audit</a>";
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign" },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },

                         { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": true,
                             "fnRender": function (o, val) {
                                 if (o.aData[28] > 1)
                                     return "<a style='cursor:hand; color:blue;' href='javascript:ViewProductionTimeTaken(\"" + o.aData[31] + "\", 1);'>" + o.aData[11] + "</a>";
                                 else
                                     return o.aData[11];
                             }
                         },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign" },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },

                         { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": true,
                             "fnRender": function (o, val) {
                                 if (o.aData[29] > 1)
                                     return "<a style='cursor:hand; color:blue;' href='javascript:ViewProductionTimeTaken(\"" + o.aData[32] + "\", 2);'>" + o.aData[17] + "</a>";
                                 else
                                     return o.aData[17];
                             }
                         },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign" },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                          { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "bSortable": true,
                              "fnRender": function (o, val) {
                                  if (o.aData[30] > 1)
                                      return "<a style='cursor:hand; color:blue;' href='javascript:ViewProductionTimeTaken(\"" + o.aData[33] + "\", 3);'>" + o.aData[23] + "</a>";
                                  else
                                      return o.aData[23];
                              }
                          },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false }
		            ],
        "iDisplayLength": 30,
        "aaSorting": [[6, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."

        }
    });

    $('.dataTables_empty').text('');

   // ChartStatus.LoadChartStatusData();
}

ChartStatus.LoadChartStatusData = function (msg) {
    // unblock when ajax activity stops 
    $(document).ajaxStop($.unblockUI);
    chartStatus = $('#Statusdrp').val();
    chartDateType = $('#dateTypeDropdownList').val();
    fromDate = $('#txtFromdate').val();
    toDate = $('#txtTodate').val()

    levelNumber = "";
    levelStatusId = "";
    queueId = $('#Queuedrp').val();

    if (chartStatus != "") {
        var chartStatusVals = chartStatus.split("|");
        levelNumber = chartStatusVals[0];
        levelStatusId = chartStatusVals[1];

        chartStatus = "";
    }

    if (chartDateType == 0 && levelStatusId == 1000) {
        //raise notification error if unassigned is chosen
        Notification.Error(Notification.enumMsg.UnassignedLeveStstusError);
        return false;
    }
    else {
        if (levelStatusId == 1000 || levelStatusId == 1001) {
            if (levelStatusId == 1000)
                chartStatus = "Unassigned";
            else
                chartStatus = "Completed";

            levelNumber = "";
            levelStatusId = "";
        }
    }

    if (queueId == "") {
        Notification.Information(Notification.enumMsg.QueueStatus);
        return false;
    }

    var sParam = "{ chartStatus: '" + chartStatus + "', fromDate: '" + fromDate + "', toDate: '" + toDate + "', chartDateType:'"
        + chartDateType + "', levelNumber: '" + levelNumber + "', levelStatusId: '" + levelStatusId + "', queueId: '" + queueId + "', userName: '" + appUser.UserName + "'}";
    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    ServiceBroker.callWebMethod(enumWebPage.ChartStatus, enumWebMethod.GetChartInfoByOverallStatus, sParam, 'ChartStatus.BindChartStatusData', true, true);
}

ChartStatus.BindChartStatusData = function (msg) {
    if (hasNoError(msg, true)) {
        ChartStatus.oTable.fnClearTable();
        ChartStatus.oTable.fnAddData(msg);
    }
    // unblock when ajax activity stops 
    $(document).ajaxStop($.unblockUI);
}

ViewChartAuditLogs = function (chartId, chartAuditCount) {
    if (chartAuditCount != 0) {
        $("#ChartAuditDialog").dialog({
            // dialogClass: "OverlayDialog",
            modal: true,
            height: 600,
            width: 1200
        });
        //  $('#dvEditWIP').show("slide", { direction: "left" }, 500);
        var vParam = "{ chartId: " + chartId + "}";
        ServiceBroker.callWebMethod(enumWebPage.ChartStatus, enumWebMethod.GetChartAuditLogsByChartId, vParam, 'ChartStatus.BindChartAuditLogsData', true, true);
    }
    else {
        Notification.Information("No audit records present");
        return false;
    }
}

ChartStatus.PrepareChartAudit = function () {
    ChartStatus.oTable2 = $('#tblChartAuditLog').dataTable({
        "bAutoWidth": true,
        "sScrollX": "",
        "bProcessing": false,
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "centerAlign" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" }
		            ],
        "iDisplayLength": 10,
        "aaSorting": [[1, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
}

ChartStatus.BindChartAuditLogsData = function (msg) {
    if (hasNoError(msg, true)) {
        ChartStatus.oTable2.fnClearTable();
        ChartStatus.oTable2.fnAddData(msg);
    }
}

/////////////////////////////////////////////////////////////////////////
$('.ExcelExport').hover(function () {
    // Hover over code
    var title = $(this).attr('title');
    $(this).data('tipText', title).removeAttr('title');
    $('<p class="tooltip"></p>')
        .text(title)
        .appendTo('body')
        .fadeIn('slow');
}, function () {
    // Hover out code
    $(this).attr('title', $(this).data('tipText'));
    $('.tooltip').remove();
}).mousemove(function (e) {
    var mousex = e.pageX + 20; //Get X coordinates
    var mousey = e.pageY + 10; //Get Y coordinates
    $('.tooltip')
        .css({ top: mousey, left: mousex })
});
//---------------------Time Taken Dialog----------------------------//
ViewProductionTimeTaken = function (chartMoreInfoId, levelNumber) {
    $("#ProductionTimeTaken").dialog({
        // dialogClass: "OverlayDialog",
        modal: true,
        height: 600,
        width: 1300
    });

    var QID = $('#Queuedrp').val();
    var vParam = "{ chartMoreInfoId: " + chartMoreInfoId + ", levelNumber:" + levelNumber + ", QueueID:" + QID + " userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ChartStatus, enumWebMethod.GetChartProductionByChartMoreInfoId, vParam, 'ChartStatus.BindProductionTimeTaken', true, true);
}


ChartStatus.PrepareProductionTimeTaken = function () {
    ChartStatus.oTable3 = $('#tblProductionTimeTaken').dataTable({
        "bAutoWidth": false,
        "sScrollX": "",
        "bProcessing": true,
        "bFilter": false,
        "bPaginate": false,
        "bInfo": false,
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "15%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "15%", "bSortable": false }

		            ],
        "iDisplayLength": 10,
        "aaSorting": [[0, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
}

ChartStatus.BindProductionTimeTaken = function (msg) {
    if (hasNoError(msg, true)) {
        ChartStatus.oTable3.fnClearTable();
        ChartStatus.oTable3.fnAddData(msg);
    }
}
