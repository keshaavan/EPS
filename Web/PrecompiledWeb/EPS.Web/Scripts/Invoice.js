var Invoice = {};
Invoice.oTable1 = null;
Invoice.viewPressed = false;

Invoice.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.Invoice, enumWebMethod.IsUserAuthorized, uParam, 'Invoice.isAuthorised', true, true);
}

Invoice.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $("#clientReferenceHeading").html(appUser.ClientReferenceLabel);
        // DailyProductionSummary.DailyProductionSummarytable();

        $("#xlsImage_Invoice").click(function () {
            var e = document.getElementById("LevelDropdownList");
            var level = e.options[e.selectedIndex].value;
            if (level == "") {
                Notification.Information(Notification.enumMsg.RequiredLevel);
                return false;
            }

            if (Invoice.viewPressed != true) {
                return false;
            }

            var ws_ExcelGenerateURL = "Handler/Invoice.ashx";

            var fromDate = $('#txtFromdate').val();
            var toDate = $('#txtTodate').val();
            var e = document.getElementById("LevelDropdownList");
            var level = e.options[e.selectedIndex].value;
            var qDrop = document.getElementById("Queuedrp");
            var queueStatus = qDrop.options[qDrop.selectedIndex].value;
            var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

            if (queueStatus == "") {
                Notification.Information(Notification.enumMsg.QueueStatus);
                return false;
            }
            var sHTMLTag = "<form method='post' action='" + ws_ExcelGenerateURL + "' style='top:-3333333333px;' id='XlsDailyReport'>";
            sHTMLTag += "<input type='hidden' name='fromDate' value='" + fromDate + "' >";
            sHTMLTag += "<input type='hidden' name='toDate' value='" + toDate + "' >";
            sHTMLTag += "<input type='hidden' name='level' value='" + level + "' >";
            sHTMLTag += "<input type='hidden' name='queueID' value='" + queueStatus + "' >";

            sHTMLTag += "</form>";
            $('body').prepend(sHTMLTag);
            $('#XlsDailyReport').submit();
            $("#XlsDailyReport").remove();

            Invoice.viewPressed = false;
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

        Invoice.PrepareInvoice();
        //Invoice.LoadInvoiceDetails();
        Invoice.PrepareQueueBindStatus();

        $("#btnViewInvoice").click(function () {
            Invoice.LoadInvoiceDetails();
            Invoice.viewPressed = true;
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

Invoice.PrepareQueueBindStatus = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Invoice, enumWebMethod.GetQueue, sParam, 'Invoice.BindQueueStatusDropdown', true, true);
}

Invoice.BindQueueStatusDropdown = function (msg) {
    $('#Queuedrp').append($("<option></option>").val(""));
    $("#Queuedrp").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#Queuedrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#Queuedrp").trigger("liszt:updated");
    });
}

Invoice.PrepareInvoice = function () {
    Invoice.oTable1 = $('#tblInvoiceReport').dataTable({
        "bAutoWidth": true,
        "sScrollX": "",
        "bProcessing": true,
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bSortable": false }
		            ],
        "iDisplayLength": 30,
        "aaSorting": [[3, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('No data found.');
}

Invoice.LoadInvoiceDetails = function (msg) {
    var e = document.getElementById("LevelDropdownList");
    var level = e.options[e.selectedIndex].value;
    
    var qDrop = document.getElementById("Queuedrp");
    var queueStatus = qDrop.options[qDrop.selectedIndex].value;
    var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

    if (level.trim() == "") {
        Notification.Information(Notification.enumMsg.RequiredLevel);
        return false;
    }

    
    if (queueStatus == "") {
        Notification.Information(Notification.enumMsg.QueueStatus);
        return false;
    }

    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{fromDate: '" + $('#txtFromdate').val() + "', toDate: '" + $('#txtTodate').val() + "', level: '" + level + "', queueID: '"+queueStatus+"' }";
    ServiceBroker.callWebMethod(enumWebPage.Invoice, enumWebMethod.GetChartInfoForInvoiceReport, sParam, 'Invoice.BindInvoiceData', true, true);
}

Invoice.BindInvoiceData = function (msg) {
    if (hasNoError(msg, true)) {
        Invoice.oTable1.fnClearTable();
        Invoice.oTable1.fnAddData(msg);
    }
    // unblock when ajax activity stops 
    $(document).ajaxStop($.unblockUI); 
}
