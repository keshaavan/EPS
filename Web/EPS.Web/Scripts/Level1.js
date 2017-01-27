var Level1 = {};

Level1.oTable = null;
Level1.oTable_Part2 = null;
Level1.levelStatusId = 0;
Level1.RequestL1ChartClicked = false;
Level1.varClientId = 0;
Level1.RefreshClicked = true;
Level1.CommentStatusName = null;
Level1.Charts = null;
Level1.LastColour = 0;
Level1.NxtFlat = null;
Level1.sTotalCount = 0;

Level1.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.IsUserAuthorized, uParam, 'Level1.isAuthorised', true, true);
}

Level1.isAuthorised = function (msg) {

    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        Level1.RefreshClicked = true;
        $("input[id=btnL1RequestChart]").attr("disabled", "disabled");
        $("input[id=btnL1RequestChart]").attr("class", "button_disabled");
        $("#lblClientReferenceHeadingPendingCharts").html(appUser.ClientReferenceLabel);
        $("#lblClientReferenceHeadingOtherCharts").html(appUser.ClientReferenceLabel);
        $("#lblClientReferenceHeading").html(appUser.ClientReferenceLabel);
        $("#lblClientReferenceHeadingName").html(appUser.ClientReferenceLabel);

        if (appUser.IsL1Auto != "True") {
            $("#txtL1SearchClientReferance").css('visibility', 'visible');
            $('#txtL1SearchClientReferance').focus();
        }
        else
            $("#txtL1SearchClientReferance").css('visibility', 'hidden');
        Level1.LoadPendingChartsCount();

        HideWIPEdit();

        Level1.PrepareStatus();
        Level1.PrepareStatus1();
        Level1.PreparePendingCharts();
        Level1.PrepareOtherCharts();

        $('#txtNoOfPages').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });
        $('#txtNoOfPagesUpdate').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });
        $('#txtNoOfDxCodesName').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });

        $('#txtNoOfDxCodes').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });

        $("#btnCancel").click(function () {
            $('#dvEditCmd').hide("slide", { direction: "left" }, 500);
        });

        $("#tblOtherChartsview_next").click(function () {
            var tmpcount = 25;
            if (Level1.sTotalCount > tmpcount) {
            Level1.BindOtherCharts("next");
             }

        });

        $("#tblOtherChartsview_previous").click(function () {
            //Level1.NxtFlat = "Y";
        });

        $("#btnUpdate").click(function () {
            var errorMsgs = "";
            var filename = $("#lblDisplayFileNames").text();
            var noofPages = $('#txtNoOfPagesUpdate').val();
            var noofDxCodes = $('#txtNoOfDxCodesName').val();
            var clientprojectid = $('#hdnClientProjectId').val();
            var levelnumber = $('#hdnLevelnumber').val();

            if (noofPages.trim().length == 0) {
                noofPages = 0;
            }

            if (noofDxCodes.trim().length == 0) {
                noofDxCodes = 0;
            }

            if (errorMsgs.trim().length > 0) {
                $("input[id=btnUpdate]").removeAttr("disabled");
                $("input[id=btnUpdate]").attr("class", "button");
                errorMsgs = "<br/>The following fields are mandatory:<br/>" + errorMsgs + "<br/>";
                $('#lblErr1').html(errorMsgs);
            } else {

                var sParam = "{  filename: '" + filename + "', clientprojectid: " + clientprojectid + ", levelnumber: " + levelnumber + ", noofDxCodes: " + noofDxCodes + ", noofPages: " + noofPages + "}";
                ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.SaveL1DataCommandStatusInfo, sParam, 'Level1.OtherL1EditComments', true, true);
                $('#dvEditCmd').hide("slide", { direction: "left" }, 500);
            }
        });

        $("#btnClose").click(function () {
            $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
        });

        $("#btnSave").click(function () {
            Level1.RequestL1ChartClicked = false;
            $("input[id=btnSave]").attr("disabled", "disabled");
            $("input[id=btnSave]").attr("class", "button_disabled");

            var errorMsgs = "";
            var chartId = $('#hdnChartId').val();
            var chartMoreInfoId = $('#hdnChartMoreInfoId').val();
            var statusId = $('#ddlStatus').val();
            var statusCommentId = $('#ddlStatusComment').val();
            var noPages = $('#txtNoOfPages').val();
            var noDxCodes = $('#txtNoOfDxCodes').val();
            var overallStatus = "L1 - " + $("#ddlStatus option:selected").text();

            if (statusId == 0 || statusId.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Status<br />";
            }

            if (statusCommentId == 0 || statusCommentId.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Status Comments<br />";
            }

            if (noPages.trim().length == 0) {
                noPages = 0;
            }

            if (noDxCodes.trim().length == 0) {
                noDxCodes = 0;
            }

            if (errorMsgs.trim().length > 0) {
                $("input[id=btnSave]").removeAttr("disabled");
                $("input[id=btnSave]").attr("class", "button");
                errorMsgs = "<br/>The following fields are mandatory:<br/>" + errorMsgs + "<br/>";
                $('#lblErr').html(errorMsgs);
            }
            else {
                var sParam = "{ chartId: " + chartId + ", chartMoreId: " + chartMoreInfoId + ", levelStatusId: " + statusId + ", levelStatusCommentId: " + statusCommentId + ", noOfPages: " + noPages + ", noOfDxCodes: " + noDxCodes + ", overallStatus: '" + overallStatus + "' }";

                if (statusId == 1) {
                    if (confirm("Do you want to complete the Chart?")) {
                        ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.SaveL1DataChartInfo, sParam, 'Level1.PendingL1EditCompleted', true, true);
                        $('#dvEditWIP').hide("slide", { direction: "left" }, 500);

                    }
                    else {
                        $("input[id=btnSave]").removeAttr("disabled");
                        $("input[id=btnSave]").attr("class", "button");
                    }
                }
                else if (statusId == 3) {
                    if (confirm("Do you want to save as an invalid Chart?")) {
                        ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.SaveL1DataChartInfo, sParam, 'Level1.InvalidEditCompleted', true, true);
                        $('#dvEditWIP').hide("slide", { direction: "left" }, 500);

                    }
                    else {
                        $("input[id=btnSave]").removeAttr("disabled");
                        $("input[id=btnSave]").attr("class", "button");
                    }
                }
                else {
                    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.SaveL1DataChartInfo, sParam, 'Level1.PendingL1EditCompleted', true, true);
                    $('#dvEditWIP').hide("slide", { direction: "left" }, 500);

                }

            }
        });

        $("#btnL1Refresh").click(function () {
            Level1.RefreshClicked = true;
            Level1.RefreshBothSection();
        });


        $('#ddlStatus').change(function () {
            var commentCategoryId = this.value;
            $('#ddlStatusComment').empty();
            $('#ddlStatusComment').append($("<option></option>").val("0").html("--Select One--"));

            var sParam = "{ commentCategoryId :" + commentCategoryId + " }";
            ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1StatusComments, sParam, 'Level1.BindStatusComments', true, true);
        });

        $('#ddlStatusName').change(function () {
            var commentCategoryId = this.value;
            $('#ddlStatusCommentName').empty();
            $('#ddlStatusCommentName').append($("<option></option>").val("0").html("--Select One--"));

            var sParam = "{ commentCategoryId :" + commentCategoryId + " }";
            ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1StatusComments, sParam, 'Level1.BindStatusCommentsName', true, true);
        });


        $("#btnL1RequestChart").click(function () {
            Level1.RequestL1ChartClicked = true;
            Level1.RefreshClicked = false;

            if (appUser.IsL1Auto != "True") {
                var strSearch = $('#txtL1SearchClientReferance').val();
                $.trim(strSearch);
                if (strSearch == "") {
                    alert("Please enter " + appUser.ClientReferenceLabel);
                }
                else {
                    strSearch = strSearch.replace(/'/g, "\\'");
                    $("input[id=btnL1RequestChart]").attr("disabled", "disabled");
                    $("input[id=btnL1RequestChart]").attr("class", "button_disabled");

                    var sParam_new = "{ clientReference: '" + strSearch + "' }";
                    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.RequestL1ChartByClientReference, sParam_new, 'Level1.BindRequestChartL1', true, true);
                }
            }
            else {
                $("input[id=btnL1RequestChart]").attr("disabled", "disabled");
                $("input[id=btnL1RequestChart]").attr("class", "button_disabled");
                var sParam = "{ }";
                ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.RequestL1Chart, sParam, 'Level1.BindRequestChartL1', true, true);
            }
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

Level1.LoadPendingChartsCount = function () {
    var sParam_chartCount = "{}";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.L1PendingChartCount, sParam_chartCount, 'Level1.BindL1PendingChartCount', true, true);
}

Level1.BindRequestChartL1 = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
            Level1.RefreshBothSection();
        }
        else if (msg[0] == 0) {
            Notification.Information(Notification.enumMsg.ChartRequestTimeOutStatus);
        }
    }
    else {
        Notification.Error(Notification.enumMsg.ChartRequestTimeOutStatus);
    }
}

Level1.BindStatusComments = function (msg) {
    $.each(msg, function (index, item) {
        $('#ddlStatusComment').append($("<option></option>").val(item[0]).html(item[1]));
    });
}
Level1.BindStatusCommentsName = function (msg) {
    $.each(msg, function (index, item) {
        $('#ddlStatusCommentName').append($("<option></option>").val(item[0]).html(item[1]));
    });
    $('#ddlStatusCommentName').find("option[value='" + Level1.CommentStatusName + "']").attr('selected', 'selected')
}

Level1.BindL1PendingChartCount = function (msg) {
    if (hasNoError(msg, true)) {
        $('#lblPendingChartCount').text(msg);
    }
}

Level1.PrepareStatus = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1ChartStatuses, sParam, 'Level1.BindStatus', true, true);
}
Level1.PrepareStatus1 = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1ChartStatuses, sParam, 'Level1.BindStatuscomm', true, true);

}

Level1.GetL1StatusComment = function () {
    var commentCategoryId = this.value;
    $('#ddlStatusCommentName').empty();
    $('#ddlStatusCommentName').append($("<option></option>").val("0").html("--Select One--"));

    var sParam = "{ commentCategoryId :" + commentCategoryId + " }";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1StatusComments, sParam, 'Level1.BindStatusCommentsName', true, true);
}

Level1.BindStatuscomm = function (msg) {
    $('#ddlStatusName').append($("<option></option>").val("0").html("--Select One--"));
    $('#ddlStatusCommentName').append($("<option></option>").val("0").html("--Select One--"));

    $.each(msg, function (index, item) {
        $('#ddlStatusName').append($("<option></option>").val(item[0]).html(item[1]));
    });
}
Level1.BindStatus = function (msg) {
    $('#ddlStatus').append($("<option></option>").val("0").html("--Select One--"));
    $('#ddlStatusComment').append($("<option></option>").val("0").html("--Select One--"));

    $.each(msg, function (index, item) {
        $('#ddlStatus').append($("<option></option>").val(item[0]).html(item[1]));
    });
}

Level1.PreparePendingCharts = function () {
    Level1.oTable = $('#tblPendingCharts').dataTable({
        "sScrollX": "100%",
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
			            { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false, "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "12%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "25%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-text", "sSortable": null, "sClass": "centerAlign", "sWidth": "15%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[6].toString() == 'Work In Progress') {
                                    return "<a style='cursor:hand;' onclick='javascrip:EditPendingWIP(\"" + (escape(o.aData[0])).toString() + "\",\"" + (escape(o.aData[1])).toString() + "\",\"" + o.aData[3].toString() + "\",\"" + escape(o.aData[4]) + "\",\"" + o.aData[7].toString() + "\");'>" + o.aData[6] + "</a>";
                                }
                                else {
                                    return "<a style='color: #696969;cursor:default;text-decoration:none;' >" + o.aData[6] + "</a>";
                                }
                            }
                        }
		            ],
        "iDisplayLength": 25,
        "aaSorting": [[6, 'desc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Records)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });
    $('.dataTables_empty').text('Loading Data...');
    Level1.LoadPendingCharts();
}

Level1.LoadPendingCharts = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1PendingChartData, sParam, 'Level1.BindPendingChartsL1', true, true);
}

Level1.BindPendingChartsL1 = function (msg) {
    $('#txtL1SearchClientReferance').val("");
    if (hasNoError(msg, true)) {
        Level1.oTable.fnClearTable();
        Level1.oTable.fnAddData(msg);
        if (msg.toString().length > 0) {
            $("input[id=btnL1RequestChart]").attr("disabled", "disabled");
            $("input[id=btnL1RequestChart]").attr("class", "button_disabled");
            levelStatusId = 14;
        }
        else {
            $("input[id=btnL1RequestChart]").removeAttr("disabled");
            $("input[id=btnL1RequestChart]").attr("class", "button");
            levelStatusId = 15;
            if (Level1.RequestL1ChartClicked) {
                if (Level1.RefreshClicked == false) {
                    Notification.Error(Notification.enumMsg.ChartsNotAvailable);
                    Level1.RequestL1ChartClicked = false;
                }
            }
        }
    }
}

Level1.PrepareOtherCharts = function () {
    var watcolumn = false;
    Level1.oTable_Part2 = $('#tblOtherChartsview').dataTable({
        "sScrollX": "100%",
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
			            { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "14%", "bSortable": false },

                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "8%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "14%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-anchor", "bSortable": false, "sClass": "centerAlign", "sWidth": "10%",
                            "fnRender": function (o, val) {
                                if (o.aData[6].toString() == 'Hold')
                                    return "<a style='cursor:hand;' onclick='javascrip:EditL1OtherStatus(\"" + o.aData[0].toString() + "\",\"" + o.aData[8].toString() + "\");'>" + o.aData[6] + "</a>";
                                else
                                    return o.aData[6];
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "8%", "bSortable": false },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "8%", "bSortable": false,"bVisible": false,
                            "fnRender": function (o, val) {
                                var arr = o.aData[13].split(',');
                                if ($.inArray(appUser.ClientKey, arr) < 0) {
                                    watcolumn = true;
                                    var oTable = $('#tblOtherChartsview').dataTable();
                                    oTable.fnSetColumnVis(10, false);
                                    return "";
                                }
                                else {
                                    if (o.aData[6].toString() == 'Completed')
                                        return "<a style='cursor:hand; display:none;' onclick='javascrip:EditL1StatusComments(\"" + (escape(o.aData[0])).toString() + "\",\"" + (escape(o.aData[1])).toString() + "\",\"" + o.aData[3].toString() + "\",\"" + o.aData[4] + "\",\"" + o.aData[7].toString() + "\",\"" + o.aData[9].toString() + "\",\"" + o.aData[10].toString() + "\",\"" + o.aData[11].toString() + "\",\"" + o.aData[12].toString() + "\",\"" + o.aData[13].toString() + "\",\"" + o.aData[14].toString() + "\",\"" + o.aData[15].toString() + "\");'><img src='Images/Edit.png' alt='Edit' /></a>";
                                    else
                                        return "";
                                }
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false }

		            ],
        "iDisplayLength": 25,
        "aaSorting": [[5, 'desc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Records)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });
    $('.dataTables_empty').text('Loading Data...');
    Level1.LoadOtherCharts();
}

Level1.LoadOtherCharts = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1OtherChartData, sParam, 'Level1.BindOtherCharts', true, true);
}

Level1.BindOtherCharts = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg != "next") {
            Level1.oTable_Part2.fnClearTable();
            Level1.oTable_Part2.fnAddData(msg);
            Level1.Charts = msg;
            Level1.sTotalCount = msg.length;
        }
    }

    var newcompletedValue = 0;
    var newcount = 0;
    var color = ["#D0E55B", "#67C2ED", "#AA7FFF", "#FDE473", "#FDC3B6", "#D47FFF", "#FDBEE1", "#A3B1FD", "#9AE9EA", "#FFAAFF"];    
    //var color = ["Grey", "Green", "Violet", "Yellow", "Blue", "Orange", "Pink", "Brown", "Purple", "Sandal"];

    var CompletedRowCount = 0;
    var HoldRowCount = 0;
    var status = '';
    var count = 0;

    if (Level1.LastColor == "9") {
        Level1.LastColour = 1;
    }

    if (Level1.LastColour == 0 && msg != "previous") {

        $("#tblOtherChartsview > tbody > tr").each(function () {
            status = $(this).find("td:eq(4)")[0].innerText;
            if (status == "Completed") {
                CompletedRowCount = CompletedRowCount + 1;
            }
        });

        var tmp = "";
        CompletedRowCount = 0;

        $("#tblOtherChartsview > tbody > tr").each(function (index) {
            status = $(this).find("td:eq(4)")[0].innerText;

            if (status == "Completed" && CompletedRowCount <= 25) {
                $('#tblOtherChartsview').css("color", "black");
                $(this).css("background-color", color[newcount]);
                $(this).find("td:eq(1)").css("background-color", color[newcount]);

                newcompletedValue = newcompletedValue + 1;
                if (newcompletedValue % 25 == 0) {
                    newcount = newcount + 1;
                    newcompletedValue = 0;
                    CompletedRowCount = 0;
                }

            }
        });
        $("#hdnpagesize").val(tmp);
        Level1.LastColour = parseInt(newcount) + 1;
    }
    else if (Level1.LastColour != 0 && msg != "previous") {
        newcount = Level1.LastColour;

        $("#tblOtherChartsview > tbody > tr").each(function () {
            status = $(this).find("td:eq(4)")[0].innerText;
            if (status == "Completed") {
                CompletedRowCount = Level1.LastColour + 1;
            }

        });
        CompletedRowCount = 0;

        $("#tblOtherChartsview > tbody > tr").each(function (index) {
            status = $(this).find("td:eq(4)")[0].innerText;

            if (status == "Completed") {
                CompletedRowCount = CompletedRowCount + 1;
            }

            if (status == "Completed" && CompletedRowCount <= 25) {
                $('#tblOtherChartsview').css("color", "black");
                $(this).css("background-color", color[newcount]);
                $(this).find("td:eq(1)").css("background-color", color[newcount]);

                newcompletedValue = newcompletedValue + 1;
                if (newcompletedValue % 25 == 0) {
                    newcount = newcount + 1;
                    newcompletedValue = 0;
                    Level1.LastColour = newcount;
                    CompletedRowCount = 0;
                }
            }
        });
        Level1.LastColour = parseInt(newcount) + 1;
    }
}

function EditPendingWIP(chartId, clientReference, market, filename, chartMoreId) {
    $('#dvEditWIP').show("slide", { direction: "left" }, 500);
    $('#ddlStatus').focus();
    $('#lblDisplayMarket').text(market);
    $('#lblDisplayGMPI').text(unescape(clientReference));
    $('#lblDisplayFileName').text(unescape(filename));
    $('#hdnChartId').val(unescape(chartId));
    $('#hdnChartMoreInfoId').val(chartMoreId);
    $('#ddlStatus option:nth(0)').attr("selected", "selected");
    $('#ddlStatusComment option:nth(0)').attr("selected", "selected");
    $('#txtNoOfPages').val("");
    $('#txtNoOfDxCodes').val("");
    $("input[id=btnSave]").removeAttr("disabled");
    $("input[id=btnSave]").attr("class", "button");
    $('#lblErr').html("");
}

function EditL1OtherStatus(chartId, chartMoreInfoId) {
    if (confirm("Do you want to work in the 'Hold' chart?")) {
        var overallStatus = "";
        if (levelStatusId == 15)
            overallStatus = "L1 - Work In Progress";
        else
            overallStatus = "L1 - Pending";
        var sParam = "{ chartId: " + chartId + ", chartMoreId: " + chartMoreInfoId + ", levelStatusId: " + levelStatusId + ", levelStatusCommentId: " + 0 + ", noOfPages: " + 0 + ", noOfDxCodes: " + 0 + ", overallStatus: '" + overallStatus + "' }";
        ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.SaveL1DataChartInfo, sParam, 'Level1.OtherL1EditCompleted', true, true);
    }
}


function EditL1StatusComments(chartId, clientReference, market, filename, chartMoreId, dxcode, statusid, statuscmdid, pages, clientid, clientprojectid, levelnumber) {
    $('#dvEditCmd').show("slide", { direction: "left" }, 500);
    Level1.CommentStatusName = statuscmdid;
    Level1.ResetField();
    $('#txtNoOfPagesName').focus()

    $('#lblDisplayMarketName').text(market);
    $('#lblDisplayGMPIName').text(unescape(clientReference));
    $('#lblDisplayFileNames').text(unescape(filename));

    $('#hdndisplayfilename').text(unescape(filename));
    $('#hdnClientProjectId').val(clientprojectid);
    $('#hdnLevelnumber').val(levelnumber);

    $('#hdnChartIdUpdate').val(unescape(chartId));
    $('#hdnChartMoreInfoIdUpdate').val(chartMoreId);
    $('#ddlStatusName').find('option[value="' + statusid + '"]').attr('selected', 'selected');
    $('#ddlStatusName').trigger("liszt:updated");


    var commentCategoryId = statusid;
    $('#ddlStatusCommentName').empty();
    $('#ddlStatusCommentName').append($("<option></option>").val("0").html("--Select One--"));

    var sParam = "{ commentCategoryId :" + commentCategoryId + " }";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1StatusComments, sParam, 'Level1.BindStatusCommentsName', true, true);

    $('#ddlStatusCommentName').trigger("liszt:updated");
    $('#txtNoOfPagesUpdate').val(pages);
    $('#txtNoOfDxCodesName').val(dxcode);
    $("input[id=btnUpdate]").removeAttr("disabled");

    $("input[id=btnUpdate]").attr("class", "button");
    $("#ddlStatusName").prop("disabled", true);
    $("#ddlStatusCommentName").prop("disabled", true);
}

Level1.RefreshBothSection = function () {
    Level1.LoadPendingChartsCount();
    Level1.LoadPendingCharts();
    Level1.LoadOtherCharts();
    Level1.LastColour = 0;
}

Level1.PendingL1EditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.ChartMoveToCompletedHoldInvalidCharts);
    Level1.RefreshBothSection();
}

Level1.InvalidEditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.ChartMoveToInvalidCharts);
    Level1.RefreshBothSection();
}

Level1.OtherL1EditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.ChartMoveToWIPPendingCharts);
    Level1.RefreshBothSection();
}
Level1.OtherL1EditComments = function (msg) {
    Notification.Information(Notification.enumMsg.UpdateMessage);
    Level1.RefreshBothSection();
}

Level1.ResetField = function () {

    $('#lblDisplayMarketName').val('');
    $('#lblDisplayGMPIName').val('');
    $('#lblDisplayFileNames').val('');

    $("#ddlStatusName").val('');
    $('#ddlStatusCommentName').val('');
    $('#txtNoOfPagesName').val('');
    $('#txtNoOfDxCodesName').val('');
}

Level1.BindStatuscmd = function () {

    var commentCategoryId = $('#hdnChartMoreInfoId').val();
    $('#ddlStatusCommentName').empty();
    $('#ddlStatusCommentName').append($("<option></option>").val("0").html("--Select One--"));
    var sParam = "{ commentCategoryId :" + commentCategoryId + " }";
    ServiceBroker.callWebMethod(enumWebPage.Level1, enumWebMethod.GetL1StatusComments, sParam, 'Level1.BindStatuscmd1', true, true);

}
Level1.BindStatuscmd1 = function (msg) {
    $.each(msg, function (index, item) {
        $('#ddlStatusCommentName').append($("<option></option>").val(item[0]).html(item[1]));
    });
}

function HideWIPEdit() {
    $('#dvEditWIP').hide();
}
