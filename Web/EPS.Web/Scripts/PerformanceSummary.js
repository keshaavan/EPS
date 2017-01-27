var PerformanceSummary = {};
PerformanceSummary.oTable1 = null;
PerformanceSummary.viewPressed = true;
var queueStatus;
PerformanceSummary.initialise = function () {
    PerformanceSummary.isFlag = "Y";
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.PerformanceSummary, enumWebMethod.IsUserAuthorized, uParam, 'PerformanceSummary.isAuthorised', true, true);
}

PerformanceSummary.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $("#xlsImage_Performance").click(function () {
            var e = document.getElementById("LevelDropdownList");
            var level = e.options[e.selectedIndex].value;
            var qDrop = document.getElementById("Queuedrp");
            var queueStatus = qDrop.options[qDrop.selectedIndex].value;
            var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

            if (appUser.IsAdmin == "True") {
                //if (queueStatus == "") {
                // queueStatus = "0";
                if (queueStatusText == "--All Queues--") {
                    $('#Queuedrp > option').each(function () {
                        if ($.trim(queueStatus).length == 0) {
                            queueStatus = this.value;
                            return queueStatus;
                        }
                        else {
                            queueStatus += ',' + this.value;
                            return queueStatus;
                        }
                    });
                }
                if (queueStatus == "") {
                    Notification.Information(Notification.enumMsg.QueueStatus);
                    return false;
                }
            }
            else {
                if (level == "") {
                    Notification.Information(Notification.enumMsg.RequiredLevel);
                    return false;
                }

                if (queueStatus == "") {
                    Notification.Information(Notification.enumMsg.QueueStatus);
                    return false;
                }
            }

            if (PerformanceSummary.viewPressed != true) {
                return false;
            }
            var ws_ExcelGenerateURL = "Handler/PerformanceSummary.ashx";
            var sHTMLTag = "<form method='post' action='" + ws_ExcelGenerateURL + "' style='top:-3333333333px;' id='XlsDailyReport'>";
            sHTMLTag += "<input type='hidden' name='levelNumber' value='" + $('#LevelDropdownList').val() + "' >";
            sHTMLTag += "<input type='hidden' name='fromDatedp' value='" + $('#txtFromdate').val() + "' >";
            sHTMLTag += "<input type='hidden' name='toDatedp' value='" + $('#txtTodate').val() + "' >";
            sHTMLTag += "<input type='hidden' name='username' value='" + $('#EmployeeDropdownList').val() + "' >";
            sHTMLTag += "<input type='hidden' name='queueID' value='" + queueStatus + "' >";
            sHTMLTag += "</form>";

            $('body').prepend(sHTMLTag);
            $('#XlsDailyReport').submit();
            $("#XlsDailyReport").remove();
            PerformanceSummary.viewPressed = false;
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
            setDate: currentDate,
            maxDate: new Date
        });

        $("#txtFromdate").val(currentDate.getDate().toString() + '/' + month[currentDate.getMonth()] + '/' + currentDate.getFullYear().toString());

        $("#txtTodate").datepicker({
            dateFormat: "dd/M/yy",
            setDate: currentDate
        });

        $("#txtTodate").val(currentDate.getDate().toString() + '/' + month[currentDate.getMonth()] + '/' + currentDate.getFullYear().toString());
        PerformanceSummary.PrepareQueueBindStatus();

        //        //var oTable = $('#tblPerformanceReport').dataTable();
        //        if (appUser.IsAdmin == "False") {
        //            PerformanceSummary.oTable1.fnSetColumnVis(8, false);
        //            PerformanceSummary.oTable1.fnSetColumnVis(9, false);
        //        }
        //        else {
        //            PerformanceSummary.oTable1.fnSetColumnVis(8, true);
        //            PerformanceSummary.oTable1.fnSetColumnVis(9, true);
        //        }


        var levelNumberDropdownlist = appUser.LevelNumber;
        if (levelNumberDropdownlist != "3") {
            $('#LevelDropdownList').find('option[value="' + levelNumberDropdownlist + '"]').attr('selected', 'selected');
            $('#LevelDropdownList').trigger("liszt:updated");
        }
        else {
            $('#LevelDropdownList').find('option[value="1"]').attr('selected', 'selected');
            $('#LevelDropdownList').trigger("liszt:updated");
        }

        if (appUser.IsAdmin == "True") {
            $('#lblEmployee').attr('style', 'display:block;');
            $('#EmployeeNameDropdownList').attr('style', 'display:block;');

            PerformanceSummary.loadEmployeeInfo();
        }
        else {
            $("#employeeDiv").attr('style', 'display: none');
            $("#levelLabelDiv").attr('style', 'padding: 10px 2px 2px 20px;');

            PerformanceSummary.PreparePerformance();

            if (appUser.IsAdmin == "False") {
                $(".psummary").hide();
                $("#thchdxcode").html("#Charts Audited / <br>DxCodes");                
            }
            else {
                $(".psummary").show();
                $("#thchdxcode").html("DxCodes");                
            }            
        }

        $("#btnViewPerformanceSummary").click(function () {
            PerformanceSummary.LoadPerformanceSummaryDetails();
            PerformanceSummary.viewPressed = true;
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

PerformanceSummary.PrepareQueueBindStatus = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.PerformanceSummary, enumWebMethod.GetQueue, sParam, 'PerformanceSummary.BindQueueStatusDropdown', true, true);
}

PerformanceSummary.BindQueueStatusDropdown = function (msg) {

    if (appUser.IsAdmin == "True") {



        $('#Queuedrp').append($("<option></option>").val("").html("--All Queues--"));
        $("#Queuedrp").trigger("liszt:updated");
        $.each(msg, function (index, item) {
            $('#Queuedrp').append($("<option></option>").val(item[0]).html(item[1]));
            $("#Queuedrp").trigger("liszt:updated");
        });

        $('#Queuedrp').find('option[value=""]').attr('selected', 'selected');
        $('#Queuedrp').trigger("liszt:updated");




    }
    else {
        $('#Queuedrp').append($("<option></option>").val("").html("--Select Queue--"));
        $("#Queuedrp").trigger("liszt:updated");
        $.each(msg, function (index, item) {
            $('#Queuedrp').append($("<option></option>").val(item[0]).html(item[1]));
            $("#Queuedrp").trigger("liszt:updated");
        });
    }
}

PerformanceSummary.PreparePerformance = function () {
    var Qccolumn = false;
    var auditcolumn = false;

    PerformanceSummary.oTable1 = $('#tblPerformanceReport').dataTable({
        "bAutoWidth": true,
        "sScrollX": "",
        "bProcessing": false,
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "20%" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "8%" },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "8%",
                            "fnRender": function (o, val) {
                                if (appUser.IsAdmin == "False") {
                                    auditcolumn = true;
                                    var oTable = $('#tblPerformanceReport').dataTable();
                                    oTable.fnSetColumnVis(8, false);
                                    return "";
                                }
                                else {
                                    return val;
                                }
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "8%",
                            "fnRender": function (o, val) {
                                if (appUser.IsAdmin == "False") {
                                    Qccolumn = true;
                                    var oTable = $('#tblPerformanceReport').dataTable();
                                    oTable.fnSetColumnVis(9, false);
                                    return "";
                                }
                                else {
                                    return val;
                                }
                            }
                        }
		            ],
        "iDisplayLength": 30,
        "aaSorting": [[2, 'desc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
}

PerformanceSummary.loadEmployeeInfo = function () {
    //    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{}";
    ServiceBroker.callWebMethod(enumWebPage.PerformanceSummary, enumWebMethod.getEmployeeInfo, sParam, 'PerformanceSummary.BindEmployeeDropdown', true, true);
}

PerformanceSummary.BindEmployeeDropdown = function (msg) {
    $('#EmployeeDropdownList').append($("<option></option>").val("").html("--All Employees--"));
    $("#EmployeeDropdownList").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#EmployeeDropdownList').append($("<option></option>").val(item[0]).html(item[1]));
        $("#EmployeeDropdownList").trigger("liszt:updated");
    });

    $('#EmployeeDropdownList').find('option[value=""]').attr('selected', 'selected');
    $('#EmployeeDropdownList').trigger("liszt:updated");

    PerformanceSummary.PreparePerformance();
    // PerformanceSummary.LoadPerformanceSummaryDetails();
}

PerformanceSummary.LoadPerformanceSummaryDetails = function (msg) {
    //    var levelNumberDropdownlist = $('#LevelDropdownList').val();
    //    var username = $('#EmployeeDropdownList').val();


    //    if (levelNumberDropdownlist == "") {
    //        Notification.Information(Notification.enumMsg.RequiredLevel);
    //        return false;
    //    }

    //    if (appUser.IsAdmin) {
    //        var queueStatus = $("#Queuedrp").val();        
    //    }
    //    else {

    //        var qDrop = document.getElementById("Queuedrp");
    //        var queueStatus = qDrop.options[qDrop.selectedIndex].value;
    //        var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

    //        if (queueStatus == "") {
    //            Notification.Information(Notification.enumMsg.QueueStatus);
    //            return false;
    //        }
    //    }

    var levelNumberDropdownlist = $('#LevelDropdownList').val();
    var username = $('#EmployeeDropdownList').val();
    var qDrop = document.getElementById("Queuedrp");
    queueStatus = qDrop.options[qDrop.selectedIndex].value;
    var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

    if (appUser.IsAdmin == "True") {
        //if (queueStatus == "") {
        // queueStatus = "0";
//        if (queueStatus == "") {
        if (queueStatusText == "--All Queues--") {
            $('#Queuedrp > option').each(function () {
                if ($.trim(queueStatus).length == 0) {
                    queueStatus = this.value;
                    return queueStatus;
                }
                else {
                    queueStatus += ',' + this.value;
                    return queueStatus;
                }
            });

        }
        if (queueStatus == "") {
            Notification.Information(Notification.enumMsg.QueueStatus);
            return false;
        }
    }
    else {
        if (levelNumberDropdownlist == "") {
            Notification.Information(Notification.enumMsg.RequiredLevel);
            return false;
        }

        if (queueStatus == "") {
            Notification.Information(Notification.enumMsg.QueueStatus);
            return false;
        }
    }

    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{ levelNumber: '" + levelNumberDropdownlist + "', fromDate: '" + $('#txtFromdate').val() + "', toDate: '" + $('#txtTodate').val() + "', userName: '" + username + "', queueID: '" + queueStatus + "' }";
    ServiceBroker.callWebMethod(enumWebPage.PerformanceSummary, enumWebMethod.GetPerformanceSummaryReport, sParam, 'PerformanceSummary.BindPerformanceSummary', true, true);
}

PerformanceSummary.BindPerformanceSummary = function (msg) {
    if (hasNoError(msg, true)) {
        PerformanceSummary.oTable1.fnClearTable();
        PerformanceSummary.oTable1.fnAddData(msg);
    }
    // unblock when ajax activity stops 
    $(document).ajaxStop($.unblockUI);
}
