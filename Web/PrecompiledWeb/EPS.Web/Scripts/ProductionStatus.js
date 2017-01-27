var ProductionStatus = {};
ProductionStatus.oTable1 = null;
ProductionStatus.viewPressed = true;

ProductionStatus.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.ProductionStatus, enumWebMethod.IsUserAuthorized, uParam, 'ProductionStatus.isAuthorised', true, true);    
}

ProductionStatus.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $("#xlsImage_ProductionStatus").click(function () {
            if (ProductionStatus.viewPressed != true) {
                return false;
            }

            var qDrop = document.getElementById("Queuedrp");
            var queueStatus = qDrop.options[qDrop.selectedIndex].value;
            var queueStatusText = qDrop.options[qDrop.selectedIndex].text;
//            if (queueStatus == "") {
//                Notification.Information(Notification.enumMsg.QueueStatus);
//                return false;
//            }

            if (appUser.IsAdmin == "True") {
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

                if (queueStatus == "") {
                    Notification.Information(Notification.enumMsg.QueueStatus);
                    return false;
                }
            }


            var ws_ExcelGenerateURL = "Handler/ProductionStatus.ashx";

            var sHTMLTag = "<form method='post' action='" + ws_ExcelGenerateURL + "' style='top:-3333333333px;' id='XlsDailyReport'>";
            sHTMLTag += "<input type='hidden' name='fromDatedp' value='" + $('#txtFromdate').val() + "' >";
            sHTMLTag += "<input type='hidden' name='toDatedp' value='" + $('#txtTodate').val() + "' >";
            sHTMLTag += "<input type='hidden' name='queueID' value='" + queueStatus + "' >";
            sHTMLTag += "</form>";

            $('body').prepend(sHTMLTag);
            $('#XlsDailyReport').submit();
            $("#XlsDailyReport").remove();

            ProductionStatus.viewPressed = false;
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

        ProductionStatus.PrepareProductionStatus();
        //ProductionStatus.GetProductionStatusData();
        ProductionStatus.PrepareQueueBindStatus();

        $("#btnViewProductionStatus").click(function () {
            ProductionStatus.GetProductionStatusData();
            ProductionStatus.viewPressed = true;
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

ProductionStatus.PrepareQueueBindStatus = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.ProductionStatus, enumWebMethod.GetQueue, sParam, 'ProductionStatus.BindQueueStatusDropdown', true, true);
}

ProductionStatus.BindQueueStatusDropdown = function (msg) {

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

ProductionStatus.PrepareProductionStatus = function () {
    ProductionStatus.oTable1 = $('#tblProductionStatusReport').dataTable({
        "bAutoWidth": false,
        "sScrollX": "100%",
        "bProcessing": false,
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

ProductionStatus.GetProductionStatusData = function () {
    var qDrop = document.getElementById("Queuedrp");
    var queueStatus = qDrop.options[qDrop.selectedIndex].value;
    var queueStatusText = qDrop.options[qDrop.selectedIndex].text;

    if (appUser.IsAdmin == "True") {        
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

        if (queueStatus == "") {
            Notification.Information(Notification.enumMsg.QueueStatus);
            return false;
        }
    }

    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    var sParam = "{ fromDate: '" + $('#txtFromdate').val() + "', toDate: '" + $('#txtTodate').val() + "', queueId: '" + queueStatus + "'}";
    ServiceBroker.callWebMethod(enumWebPage.ProductionStatus, enumWebMethod.GetProductionStatisticsData, sParam, 'ProductionStatus.ProductionStatusSuccess', true, true);
}

ProductionStatus.ProductionStatusSuccess = function (msg) {
    if (hasNoError(msg, true)) {
        ProductionStatus.oTable1.fnClearTable();
        ProductionStatus.oTable1.fnAddData(msg);
    }
    // unblock when ajax activity stops 
    $(document).ajaxStop($.unblockUI); 
}