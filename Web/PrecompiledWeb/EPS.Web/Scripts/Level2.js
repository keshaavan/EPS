var Level2 = {};

Level2.oTable = null;
Level2.oTable_Part2 = null;
Level2.oTable_Part3 = null;
Level2.levelStatusId = 0;
Level2.RequestL2ChartClicked = false;
Level2.Charts = null;
Level2.LastColour = 0;
Level2.NxtFlat = null;
Level2.sTotalCount = 0;

Level2.initialise = function () {
    //$("#hdnlevel2pagesize").val("25");
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.IsUserAuthorized, uParam, 'Level2.isAuthorised', true, true);
}

Level2.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {

        Level2.EditBoxLoadHide();
        $("input[id=btnL2RequestChart]").attr("disabled", "disabled");
        $("input[id=btnL2RequestChart]").attr("class", "button_disabled");
        $("#lblClientReferenceHeading4ChartUpdate").html(appUser.ClientReferenceLabel);
        $("#lblClientReferenceHeading4ChartUpdate1").html(appUser.ClientReferenceLabel);
        $("#lblClientReferenceHeadingOtherCharts").html(appUser.ClientReferenceLabel);
        $("#lblClientReferenceHeadingActiveCharts").html(appUser.ClientReferenceLabel);
        $("#lblCommentsHeadingClientReference").html(appUser.ClientReferenceLabel);
        $("#lblCommentsHeadingClientReferenceUpdate").html(appUser.ClientReferenceLabel);

        if (appUser.IsL2Auto != "True") {
            $("#txtL2SearchClientReferance").css('visibility', 'visible');
            $('#txtL2SearchClientReferance').focus();
        }
        else
            $("#txtL2SearchClientReferance").css('visibility', 'hidden');
        Level2.LoadPendingChartsCount();
        HideWIPEdit();


        Level2.PrepareStatus();
        Level2.PreparePendingCharts();
        Level2.PrepareOtherCharts();
        Level2.PrepareChartAudit();


        HideWIPEditAuditComments();
        Level2.PrepareErrorDescription();
        Level2.PrepareErrorCategory();
        Level2.PrepareErrorSubCategory();


        $('#txtL2NoOfPages').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });


        $('#txtCommentsPageNo').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });

        $('#txtCommentsPageNoUpdate').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });


        $('#txtL2NoOfDxCodes').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;
            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });

        $("#btnL2Close").click(function () {
            $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
        });

        $("#btnL2ActionUpdateClose").click(function () {
            Level2.Clear();
            Level2.EditBox();
            Level2.jqueryDataTableBoxShow();
        });


        $("#tblOtherChartsview_next").click(function () {
            // Level2.BindOtherCharts("next");
            var tmpcount = 25;
            if (Level2.sTotalCount > tmpcount) {
                Level2.BindOtherCharts("next");
            }
        });

        $("#tblOtherChartsview_previous").click(function () {
           // Level2.NxtFlat = "Y";
        });


        $("#btnL2ActionUpdate").click(function () {

            $("input[id=btnL2ActionUpdate]").attr("disabled", "disabled");
            $("input[id=btnL2ActionUpdate]").attr("class", "button_disabled");
            var btnValue = $('#btnL2ActionUpdate').val();
            var errorMsgs = "";
            var chartId = $('#hdnCommentsChartIdUpdate').val();
            var chartMoreInfoId = $('#hdnMoreInfoId1').val();
            var Id = $('#hdnid').val();
            var filename = $("#lblCommentsFilenameUpdate").text();
            var commentsNoPagesUpdate = $('#txtCommentsPageNoUpdate').val();
            var commentsCorrectedValuesUpdate = $.trim($('#txtCommentsCorrectedValuesUpdate').val());
            var commentsErrorDescriptiondrp = $("#CommentsErrorDescriptiondrpUpdate option:selected").text();
            var commentsErrorCategorydrp = $('#CommentsErrorCategorydrpUpdate').val();
            var commentsErrorSubCategorydrp = $('#CommentsErrorSubCategorydrpUpdate').val();

            var additionalComments = $.trim($('#txtAdditionalCommentsUpdate').val());
            if (commentsNoPagesUpdate.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Page Number<br />";
            }


            if (!commentsCorrectedValuesUpdate == "") {
                commentsCorrectedValuesUpdate = commentsCorrectedValuesUpdate.replace(/'/g, "\\'");
            }

            if (commentsCorrectedValuesUpdate.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Corrected Values<br />";
            }

            if (!additionalComments == "") {
                additionalComments = additionalComments.replace(/'/g, "\\'");
            }

            if (commentsErrorDescriptiondrp == "--Select One--") {
                commentsErrorDescriptiondrp = "";
            }

            if (!commentsErrorDescriptiondrp == "") {
                commentsErrorDescriptiondrp = commentsErrorDescriptiondrp.replace(/'/g, "\\'");
            }

            if (commentsErrorDescriptiondrp == 0) {
                errorMsgs = errorMsgs + "   * Error Description<br />";
            }

            if (commentsErrorCategorydrp == 0) {
                errorMsgs = errorMsgs + "   * Error Category<br />";
            }

            if (commentsErrorSubCategorydrp == 0) {
                errorMsgs = errorMsgs + "   * Error Sub Category<br />";
            }
            if (additionalComments.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Additional Comments<br />";
            }

            if (errorMsgs.trim().length > 0) {
                $("input[id=btnL2ActionUpdate]").removeAttr("disabled");
                $("input[id=btnL2ActionUpdate]").attr("class", "button");
                errorMsgs = "<br/>The following fields are mandatory:<br/>" + errorMsgs + "<br/>";
                $('#lblCommentsErr1').html(errorMsgs);
            }
            else {

                $("input[id=btnL2ActionUpdate]").removeAttr("disabled");
                $("input[id=btnL2ActionUpdate]").attr("class", "button");


                if (btnValue == "Save") {

                    var sParam = "{ chartMoreInfoId: " + chartMoreInfoId + ", pageNumbers: '" + commentsNoPagesUpdate + "', correctedValue: '" + commentsCorrectedValuesUpdate + "', comments: '" + commentsErrorDescriptiondrp + "', errorCategoryId: " + commentsErrorCategorydrp + ", errorSubCategoryId: " + commentsErrorSubCategorydrp + ",additionalComments: '" + additionalComments + "' }";
                    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.SaveL2AuditComments, sParam, 'Level2.AuditCommentsEditCompleted', true, true);

                } else {

                    var sParam = "{ ChartMoreInfoId: " + chartMoreInfoId + ", Id: '" + Id + "', PageNumbers: '" + commentsNoPagesUpdate + "', CorrectedValue: '" + commentsCorrectedValuesUpdate + "', Comments: '" + commentsErrorDescriptiondrp + "' , ErrorCategoryId: " + commentsErrorCategorydrp + ", ErrorSubCategoryId: " + commentsErrorSubCategorydrp + ", AdditionalComments: '" + additionalComments + "' }";
                    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.SaveL2AuditUpdateComments, sParam, 'Level2.AuditCommentsUpdateCompleted', true, true);
                }

                LoadChartAuditMoreInfoList(chartMoreInfoId);
                Level2.Clear();
                Level2.EditBox();
                Level2.jqueryDataTableBoxShow();
            }
        });


        $("#btnL2Save").click(function () {
            $("input[id=btnL2Save]").attr("disabled", "disabled");
            $("input[id=btnL2Save]").attr("class", "button_disabled");

            var errorMsgs = "";
            var chartId = $('#hdnChartId').val();
            var chartMoreInfoId = $('#hdnChartMoreInfoId').val();
            var statusId = $('#ddlL2Status').val();
            var statusCommentId = $('#ddlL2StatusComment').val();
            var noPages = $('#txtL2NoOfPages').val();
            var noDxCodes = $('#txtL2NoOfDxCodes').val();
            var overallStatus = "L2 - " + $("#ddlL2Status option:selected").text();

            Level2.RequestL2ChartClicked = false;

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
                $("input[id=btnL2Save]").removeAttr("disabled");
                $("input[id=btnL2Save]").attr("class", "button");

                errorMsgs = "<br/>The following fields are mandatory:<br/>" + errorMsgs + "<br/>";
                $('#lblErr').html(errorMsgs);
            }
            else {
                var sParam = "{ chartId: " + chartId + ", chartMoreId: " + chartMoreInfoId + ", levelStatusId: " + statusId + ", levelStatusCommentId: " + statusCommentId + ", noOfPages: " + noPages + ", noOfDxCodes: " + noDxCodes + ", overallStatus: '" + overallStatus + "' }";

                if (statusId == 1) {
                    if (confirm("Do you want to complete the Chart?")) {
                        ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.SaveL2DataChartInfo, sParam, 'Level2.PendingEditCompleted', true, true);
                        $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
                    }
                    else {
                        $("input[id=btnL2Save]").removeAttr("disabled");
                        $("input[id=btnL2Save]").attr("class", "button");
                    }
                }
                else if (statusId == 3) {
                    if (confirm("Do you want to save as an invalid Chart?")) {
                        ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.SaveL2DataChartInfo, sParam, 'Level2.InvalidEditCompleted', true, true);
                        $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
                    }
                    else {
                        $("input[id=btnL2Save]").removeAttr("disabled");
                        $("input[id=btnL2Save]").attr("class", "button");
                    }
                }
                else {
                    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.SaveL2DataChartInfo, sParam, 'Level2.PendingEditCompleted', true, true);
                    $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
                }
            }
        });

        $("#btnL2ActionClose").click(function () {
            $('#dvEditAuditComments').hide("slide", { direction: "left" }, 500);
        });


        $("#btnL2ActionSave").click(function () {
            $("input[id=btnL2ActionSave]").attr("disabled", "disabled");
            $("input[id=btnL2ActionSave]").attr("class", "button_disabled");

            var errorMsgs = "";
            var commentsChartId = $('#hdnCommentsChartId').val();  // Chart Id
            var commentsChartMoreInfoId = $('#hdnCommentsChartMoreInfoId').val(); // Chart More Info Id
            var commentsNoPages = $('#txtCommentsPageNo').val();
            var commentsCorrectedValues = $.trim($('#txtCommentsCorrectedValues').val());

            var commentsErrorDescriptiondrp = $("#CommentsErrorDescriptiondrp option:selected").text();

            var commentsErrorCategorydrp = $('#CommentsErrorCategorydrp').val();
            var commentsErrorSubCategorydrp = $('#CommentsErrorSubCategorydrp').val();

            var additionalComments = $.trim($('#txtAdditionalComments').val());
            if (!additionalComments == "") {
                additionalComments = additionalComments.replace(/'/g, "\\'");
            }

            if (commentsErrorDescriptiondrp == "--Select One--") {
                commentsErrorDescriptiondrp = "";
            }

            if (!commentsErrorDescriptiondrp == "") {
                commentsErrorDescriptiondrp = commentsErrorDescriptiondrp.replace(/'/g, "\\'");
            }

            if (!commentsCorrectedValues == "") {
                commentsCorrectedValues = commentsCorrectedValues.replace(/'/g, "\\'");
            }

            if (commentsNoPages.trim().length == 0) {
                commentsNoPages = 0;
            }
            if (commentsCorrectedValues.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Page Number<br />";
            }

            if (commentsCorrectedValues.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Corrected Values<br />";
            }

            if (commentsErrorDescriptiondrp == 'Choose Error Description' || commentsErrorDescriptiondrp.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Error Description<br />";
            }

            if (commentsErrorCategorydrp == 0 || commentsErrorCategorydrp.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Error Category<br />";
            }

            if (commentsErrorSubCategorydrp == 0 || commentsErrorSubCategorydrp.trim().length == 0) {
                errorMsgs = errorMsgs + "   * Error Sub Category<br />";
            }

            if (errorMsgs.trim().length > 0) {
                $("input[id=btnL2ActionSave]").removeAttr("disabled");
                $("input[id=btnL2ActionSave]").attr("class", "button");
                errorMsgs = "<br/>The following fields are mandatory:<br/>" + errorMsgs + "<br/>";
                $('#lblCommentsErr').html(errorMsgs);
            }
            else {
                var sParam = "{ chartMoreInfoId: " + commentsChartMoreInfoId + ", pageNumbers: '" + commentsNoPages + "', correctedValue: '" + commentsCorrectedValues + "', comments: '" + commentsErrorDescriptiondrp + "', errorCategoryId: " + commentsErrorCategorydrp + ", errorSubCategoryId: " + commentsErrorSubCategorydrp + ",additionalComments: '" + additionalComments + "' }";
                ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.SaveL2AuditComments, sParam, 'Level2.AuditCommentsEditCompleted', true, true);
                $('#dvEditAuditComments').hide("slide", { direction: "left" }, 500);
            }
        });

        $("#btnL2Refresh").click(function () {
            Level2.RefreshBothSection();
        });

        $('#ddlL2Status').change(function () {
            var commentCategoryId = this.value;

            $('#ddlL2StatusComment').empty();
            $('#ddlL2StatusComment').append($("<option></option>").val("").html("--Select One--"));

            var sParam = "{ commentCategoryId :" + commentCategoryId + "  }";
            ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetL2StatusComments, sParam, 'Level2.BindStatusComments', true, true);
        });

        $("#btnL2RequestChart").click(function () {
            Level2.RequestL2ChartClicked = true;

            if (appUser.IsL2Auto != "True") {
                var strSearch = $('#txtL2SearchClientReferance').val();
                $.trim(strSearch);
                if (strSearch == "") {
                    alert("Please enter " + appUser.ClientReferenceLabel);
                }
                else {
                    strSearch = strSearch.replace(/'/g, "\\'");
                    $("input[id=btnL2RequestChart]").attr("disabled", "disabled");
                    $("input[id=btnL2RequestChart]").attr("class", "button_disabled");

                    var sParam_new = "{ clientReference: '" + strSearch + "' }";
                    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.RequestL2ChartByClientReference, sParam_new, 'Level2.BindRequestChartL2', true, true);
                }
            }
            else {
                $("input[id=btnL2RequestChart]").attr("disabled", "disabled");
                $("input[id=btnL2RequestChart]").attr("class", "button_disabled");
                var sParam = "{ }";
                ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.RequestL2Chart, sParam, 'Level2.BindRequestChart', true, true);
            }
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

Level2.BindRequestChartL2 = function (msg) {
    Level2.RefreshBothSection();
}

Level2.BindStatusComments = function (msg) {
    $.each(msg, function (index, item) {
        $('#ddlL2StatusComment').append($("<option></option>").val(item[0]).html(item[1]));
    });
}

Level2.BindRequestChart = function (msg) {

    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
            Level2.RefreshBothSection();

        }
        else if (msg[0] == 0) {
            Notification.Information(Notification.enumMsg.ChartRequestTimeOutStatus);
        }
    }
    else {
        Notification.Error(Notification.enumMsg.ChartRequestTimeOutStatus);

    }
}

Level2.LoadPendingChartsCount = function () {
    var vParam_chartCount = "{}";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.L2PendingChartCount, vParam_chartCount, 'Level2.BindL2PendingChartCount', true, true);
}

Level2.BindL2PendingChartCount = function (msg) {
    if (hasNoError(msg, true)) {
        $('#lblPendingChartCount').text(msg);
    }
}
Level2.PrepareStatus = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetL2ChartStatuses, sParam, 'Level2.BindChartStatus', true, true);
}

Level2.BindChartStatus = function (msg) {
    $('#ddlL2Status').append($("<option></option>").val("").html("--Select One--"));
    $('#ddlL2StatusComment').append($("<option></option>").val("").html("--Select One--"));

    $.each(msg, function (index, item) {
        $('#ddlL2Status').append($("<option></option>").val(item[0]).html(item[1]));
    });
}



Level2.PrepareChartAudit = function () {
    Level2.oTable_Part3 = $('#tblAuditComments').dataTable({
        "sScrollX": "100%",
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "bSortable": false },
                        { "sSortDataType": "dom-anchor", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[7].toString() == 'Edit')
                                    return "<a style='color:blue;cursor:hand;' onclick='javascrip:EditL2AuditComments(\"" + o.aData[0].toString() + "\",\"" + o.aData[1].toString() + "\",\"" + o.aData[2].toString() + "\",\"" + o.aData[3].toString() + "\",\"" + o.aData[4].toString() + "\",\"" + o.aData[5].toString() + "\",\"" + o.aData[6].toString() + "\");'> Edit </a>";
                                else
                                    return "Add";
                            }
                        },
                        { "sSortDataType": "dom-anchor", "sClass": "leftAlign", "sWidth": "20%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[8].toString() == 'Delete')
                                    return "<a style='color:blue; cursor:hand;' onclick='javascrip:DeleteL2AuditComments(\"" + o.aData[0].toString() + "\");'> Delete </a>";
                                else
                                    return "Delete";
                            }
                        }
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


}

Level2.PreparePendingCharts = function () {
    Level2.oTable = $('#tblPendingCharts').dataTable({
        "sScrollX": "100%",
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
			            { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "15%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[6].toString() == 'Work In Progress')
                                    return "<a style='cursor:hand;' onclick='javascrip:EditL2PendingWIP(\"" + o.aData[0].toString() + "\",\"" + (escape(o.aData[1])).toString() + "\",\"" + o.aData[3].toString() + "\",\"" + escape(o.aData[4]) + "\",\"" + o.aData[7].toString() + "\",\"" + o.aData[9].toString() + "\");'>" + o.aData[6] + "</a>";
                                else {
                                    return "<a style='color: #696969;cursor:default;text-decoration:none;' >" + o.aData[6] + "</a>";
                                }
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "10%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[8].toString() == 'Work In Progress')
                                    return "<a style='cursor:hand;' onclick='javascrip:EditL2AuditCommentsWIP(\"" + o.aData[0].toString() + "\",\"" + (escape(o.aData[1])).toString() + "\",\"" + escape(o.aData[4]) + "\",\"" + o.aData[7].toString() + "\");'> Add </a>";
                                else
                                    return "Add";
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false }
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

    Level2.LoadPendingCharts();
}

Level2.LoadPendingCharts = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetL2PendingChartData, sParam, 'Level2.BindPendingChartsL2', true, true);
}

Level2.BindPendingChartsL2 = function (msg) {
    $('#txtL2SearchClientReferance').val("");

    if (hasNoError(msg, true)) {
        Level2.oTable.fnClearTable();
        Level2.oTable.fnAddData(msg);
        if (msg.toString().length > 0) {
            $("input[id=btnL2RequestChart]").attr("disabled", "disabled");
            $("input[id=btnL2RequestChart]").attr("class", "button_disabled");
            levelStatusId = 14;
        }
        else {
            $("input[id=btnL2RequestChart]").removeAttr("disabled");
            $("input[id=btnL2RequestChart]").attr("class", "button");

            levelStatusId = 15;

            if (Level2.RequestL2ChartClicked) {
                Notification.Error(Notification.enumMsg.ChartsNotAvailable);
                Level2.RequestL2ChartClicked = false;
            }
        }
    }
}

Level2.PrepareOtherCharts = function () {
    var watcolumn = false;
    Level2.oTable_Part2 = $('#tblOtherChartsview').dataTable({
        "sScrollX": "100%",
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
			            { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "14%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "14%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-anchor", "bSortable": false, "sClass": "centerAlign", "sWidth": "8%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[6].toString() == 'Hold')
                                    return "<a style='cursor:hand;' onclick='javascrip:EditL2OtherStatus(\"" + o.aData[0].toString() + "\",\"" + o.aData[8].toString() + "\");'>" + o.aData[6] + "</a>";
                                else
                                    return o.aData[6];
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
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
                                         return "<a style='cursor:hand;' onclick='javascrip:EditL2StatusComments(\"" + o.aData[16].toString() + "\");'><img src='Images/Edit.png' alt='Edit' /></a>";
                                     else
                                         return "";
                                 }
                             }
                         },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bVisible": false }
		            ],
        "iDisplayLength": 25,
        "aaSorting": [[5, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Records)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
    Level2.LoadOtherCharts();
}


Level2.LoadOtherCharts = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetL2OtherChartData, sParam, 'Level2.BindOtherCharts', true, true);
}

Level2.BindOtherCharts = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg != "next") {
            Level2.oTable_Part2.fnClearTable();
            Level2.oTable_Part2.fnAddData(msg);
            Level2.Charts = msg;
        }
    }

    var newcompletedValue = 0;
    var newcount = 0;
    //var color = ["Grey", "Green", "Violet", "Yellow", "Blue", "Orange", "Pink", "Brown", "Purple", "Sandal"];
    var color = ["#D0E55B", "#67C2ED", "#AA7FFF", "#FDE473", "#FDC3B6", "#D47FFF", "#FDBEE1", "#A3B1FD", "#9AE9EA", "#FFAAFF"];    

    var CompletedRowCount = 0;
    var HoldRowCount = 0;
    var status = '';
    var count = 0;

    if (Level2.LastColor == "9") {
        Level2.LastColour = 1;
    }

    //INIT CALL
    if (Level2.LastColour == 0 && msg != "previous") {
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
                    CompletedRowCount = 0;
                }
            }
        });
        $("#hdnlevel2pagesize").val(tmp);
        Level2.LastColour = parseInt(newcount) + 1;
    }
    else if (Level2.LastColour != 0 && msg != "previous") {
        newcount = Level2.LastColour;
        $("#tblOtherChartsview > tbody > tr").each(function () {
            status = $(this).find("td:eq(4)")[0].innerText;
            if (status == "Completed") {
                CompletedRowCount = CompletedRowCount + 1;
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
                    Level2.LastColour = newcount;
                    CompletedRowCount = 0;
                }
            }
        });
        Level2.LastColour = parseInt(newcount) + 1;
    }
}

function DeleteL2AuditComments(Id) {

    var chartMoreInfoId = $('#hdnMoreInfoId1').val();
    var sParam = "{ chartMoreInfoId: " + chartMoreInfoId + ", Id: " + Id + "}";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.DeleteChartAuditUpdateComments, sParam, 'Level2.BindDeleteChartMoreInfo', true, true);
    Level2.Clear();
    LoadChartAuditMoreInfoList(chartMoreInfoId);
    Level2.EditBox();
    Level2.jqueryDataTableBoxShow();
}
Level2.BindDeleteChartMoreInfo = function (msg) {
    Notification.Information(Notification.enumMsg.DeleteMessage);
}

function EditL2PendingWIP(chartId, clientReference, market, filename, chartMoreId, ChartMoreInfoId) {

    $("input[id=btnL2Save]").removeAttr("disabled");
    $("input[id=btnL2Save]").attr("class", "button");
    $('#dvEditWIP').show("slide", { direction: "left" }, 500);
    $('#lblDisplayMarket').text(market);
    $('#lblDisplayGMPI').text(unescape(clientReference));
    $('#lblDisplayFileName').text(unescape(filename));
    $('#hdnChartId').val(chartId);
    $('#hdnChartMoreInfoId').val(chartMoreId);
    $('#ddlL2Status option:nth(0)').attr("selected", "selected");
    $('#ddlL2StatusComment option:nth(0)').attr("selected", "selected");
    $('#txtL2NoOfPages').val("");
    $('#txtL2NoOfDxCodes').val("");
    $('#lblErr').html("");
    $('#ddlL2Status').focus();
    LoadChartMoreInfoList(chartId);
}
function LoadChartMoreInfoList(chartId) {
    var sParam = "{ chartId: " + chartId + "}";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetChartMoreInfoByChartId, sParam, 'Level2.BindChartMoreInfo', true, true);
}
Level2.BindChartMoreInfo = function (msg) {
    if (hasNoError(msg, true)) {
        $('#lblL1TotalEncounters').text(msg[0][0]);
        $('#lblL1TotalDxCode').text(msg[0][1]);
    }
}

function EditL2StatusComments(chartMoreInfoId) {
    Level2.Clear();
    Level2.jqueryDataTableBoxHide();
    Level2.EditBoxShow();    
    $('#hdnMoreInfoId1').val(chartMoreInfoId);
    $('#btnL2ActionUpdate').val("Save");
    LoadChartAuditMoreInfoList(chartMoreInfoId);
    $('#CommentsErrorDescriptiondrpUpdate').empty();
    $('#CommentsErrorSubCategorydrpUpdate').empty();
    $('#CommentsErrorCategorydrpUpdate').empty();

    Level2.BindErrorDescription();
    Level2.BindErrorCategories();
    Level2.BindErrorSubCategories();
}

function LoadChartAuditMoreInfoList(chartMoreInfoId) {

    var sParam = "{ sChartMoreInfoId :" + chartMoreInfoId + " }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetChartAuditComments, sParam, 'Level2.BindChartAuditCommentsName', true, true);
}


Level2.BindChartAuditCommentsName = function (msg) {
    if (hasNoError(msg, true)) {
        Level2.oTable_Part3.fnClearTable();
        Level2.oTable_Part3.fnAddData(msg);
    }
}

Level2.Clear = function () {
    $('#lblCommentsHeadingClientReferenceDataUpdate').val('');
    $('#lblCommentsFilenameUpdate').val('');
    $('#txtCommentsPageNoUpdate').val('');
    $("#txtCommentsCorrectedValuesUpdate").val('');
    $('#CommentsErrorDescriptiondrpUpdate option').prop('selected', false).trigger('chosen:updated');
    $('#CommentsErrorSubCategorydrpUpdate option').prop('selected', false).trigger('chosen:updated');
    $('#CommentsErrorCategorydrpUpdate option').prop('selected', false).trigger('chosen:updated');
    $("#txtAdditionalCommentsUpdate").val('');
    $('#lblCommentsErr1').html("");
}

function EditL2OtherStatus(chartId, chartMoreInfoId) {
    if (confirm("Do you want to work in the 'Hold' chart?")) {
        var overallStatus = "";
        if (levelStatusId == 15)
            overallStatus = "L2 - Work In Progress";
        else
            overallStatus = "L2 - Pending";

        var sParam = "{ chartId: " + chartId + ", chartMoreId: " + chartMoreInfoId + ", levelStatusId: " + levelStatusId + ", levelStatusCommentId: " + 0 + ", noOfPages: " + 0 + ", noOfDxCodes: " + 0 + ", overallStatus: '" + overallStatus + "' }";
        ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.SaveL2DataChartInfo, sParam, 'Level2.OtherEditCompleted', true, true);
    }
}

Level2.PrepareErrorDescription = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetErrorDescriptions, sParam, 'Level2.BindErrorDescriptions', true, true);
}

Level2.BindErrorDescriptions = function (msg) {
    $('#CommentsErrorDescriptiondrp').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorDescriptiondrp").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorDescriptiondrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorDescriptiondrp").trigger("liszt:updated");
    });
}


Level2.PrepareErrorCategory = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetErrorCategories, sParam, 'Level2.BindErrorCategory', true, true);
}

Level2.BindErrorCategory = function (msg) {
    $('#CommentsErrorCategorydrp').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorCategorydrp").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorCategorydrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorCategorydrp").trigger("liszt:updated");
    });
}

Level2.PrepareErrorSubCategory = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetErrorSubCategories, sParam, 'Level2.BindErrorSubCategory', true, true);
}

Level2.BindErrorSubCategory = function (msg) {
    $('#CommentsErrorSubCategorydrp').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorSubCategorydrp").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorSubCategorydrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorSubCategorydrp").trigger("liszt:updated");
    });
}

function EditL2AuditCommentsWIP(chartId, clientReference, filename, chartMoreInfoId) {
    $('#dvEditAuditComments').show("slide", { direction: "left" }, 500);
    $('#txtCommentsPageNo').focus();
    $('#lblCommentsHeadingClientReferenceData').text(unescape(clientReference));
    $('#lblCommentsFilename').text(unescape(filename));
    $('#hdnCommentsChartId').val(chartId);
    $('#hdnCommentsChartMoreInfoId').val(chartMoreInfoId);
    $('#txtCommentsPageNo').val("");
    $('#txtCommentsCorrectedValues').val("");

    $('#txtAdditionalComments').val("");
    $('#CommentsErrorDescriptiondrp option:nth(0)').attr("selected", "selected");
    $("#CommentsErrorDescriptiondrp").trigger("liszt:updated");
    $('#CommentsErrorCategorydrp option:nth(0)').attr("selected", "selected");
    $("#CommentsErrorCategorydrp").trigger("liszt:updated");
    $('#CommentsErrorSubCategorydrp option:nth(0)').attr("selected", "selected");
    $("#CommentsErrorSubCategorydrp").trigger("liszt:updated");

    $("input[id=btnL2ActionSave]").removeAttr("disabled");
    $("input[id=btnL2ActionSave]").attr("class", "button");
    $('#lblCommentsErr').html("");
}
function EditL2AuditComments(Id, pagenumber, correctedvalues, errorDescription, errorCategory, errorSubcategory, additionalComments) {    
    Level2.Clear();
    $('#btnL2ActionUpdate').val("Update");
    $('#txtCommentsPageNoUpdate').focus();
    $('#txtCommentsPageNoUpdate').val(pagenumber);
    $('#txtCommentsCorrectedValuesUpdate').val(correctedvalues);
    $('#hdnid').val(Id);
    var moreinfoid = $('#hdnMoreInfoId1').val();
    $('#CommentsErrorDescriptiondrpUpdate').find('option[value="' + errorDescription + '"]').attr('selected', 'selected');
    $('#CommentsErrorDescriptiondrpUpdate').trigger("liszt:updated");
    $('#CommentsErrorCategorydrpUpdate').find('option[value="' + errorCategory + '"]').attr('selected', 'selected');
    $('#CommentsErrorCategorydrpUpdate').trigger("liszt:updated");
    $('#CommentsErrorSubCategorydrpUpdate').find('option[value="' + errorSubcategory + '"]').attr('selected', 'selected');
    $('#CommentsErrorSubCategorydrpUpdate').trigger("liszt:updated");
    $('#txtAdditionalCommentsUpdate').val(additionalComments);
    $('#lblCommentsErr1').html("");
}

Level2.RefreshBothSection = function () {
    Level2.LoadPendingChartsCount();
    Level2.LoadPendingCharts();
    Level2.LoadOtherCharts();
    Level2.LastColour = 0;
}

Level2.PendingEditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.ChartMoveToCompletedHoldInvalidCharts);
    Level2.RefreshBothSection();
}

Level2.InvalidEditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.ChartMoveToInvalidCharts);
    Level2.RefreshBothSection();
}


Level2.CommentUpdate = function (msg) {
    Notification.Information(Notification.enumMsg.UpdateMessage);
    Level2.RefreshBothSection();
}

Level2.OtherEditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.ChartMoveToWIPPendingCharts);
    Level2.RefreshBothSection();
}

Level2.AuditCommentsEditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.AuditCommentAddedSuccess);
}
Level2.AuditCommentsUpdateCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.UpdateMessage);
}
Level2.AuditCommentsAddCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.AddedMessage);
}

function HideWIPEdit() {
    $('#dvEditWIP').hide();
}

function HideWIPEditAuditComments() {
    $('#dvEditAuditComments').hide();
}

//----------------------BindDropdown-----------------------

Level2.BindErrorDescription = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetErrorDescriptionsUpdate, sParam, 'Level2.BindErrorDescriptionSuccess', true, true);
}
Level2.BindErrorDescriptionSuccess = function (msg) {    
    $('#CommentsErrorDescriptiondrpUpdate').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorDescriptiondrpUpdate").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorDescriptiondrpUpdate').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorDescriptiondrpUpdate").trigger("liszt:updated");
    });
}

Level2.BindErrorCategories = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetErrorCategoriesUpdate, sParam, 'Level2.BindErrorCategorySuccess', true, true);
}


Level2.BindErrorCategorySuccess = function (msg) {
    $('#CommentsErrorCategorydrpUpdate').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorCategorydrpUpdate").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorCategorydrpUpdate').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorCategorydrpUpdate").trigger("liszt:updated");
    });
}

Level2.BindErrorSubCategories = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level2, enumWebMethod.GetErrorSubCategoriesUpdate, sParam, 'Level2.BindErrorSubCategoriesSuccess', true, true);
}


Level2.BindErrorSubCategoriesSuccess = function (msg) {
    $('#CommentsErrorSubCategorydrpUpdate').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorSubCategorydrpUpdate").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorSubCategorydrpUpdate').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorSubCategorydrpUpdate").trigger("liszt:updated");
    });
}



//----------------------------Show and Hide--------------------------------
Level2.EditBoxLoadHide = function () {
    $("#effect").hide();
}

Level2.EditBox = function () {
    var selectedEffect = "blind";
    var options = {};
    $("#effect").hide(selectedEffect, options, 500);
}

Level2.EditBoxShow = function () {
    var selectedEffect = "blind";
    var options = {};
    $("#effect").show(selectedEffect, options, 500);
}

Level2.jqueryDataTableBoxHide = function () {
    var selectedEffect = "blind";
    var options = {};
    $("#Container").hide(selectedEffect, options, 500);
    $("#ContainerCompleted").hide(selectedEffect, options, 500);
}

Level2.jqueryDataTableBoxShow = function () {
    var selectedEffect = "blind";
    var options = {};
    $("#Container").show(selectedEffect, options, 500);
    $("#ContainerCompleted").show(selectedEffect, options, 500);
}

Level2.Visibletrue = function () {
    $('#txtCommentsPageNoUpdate').prop('disabled', false);
    $('#txtCommentsCorrectedValuesUpdate').prop('disabled', false);
    $('#txtAdditionalCommentsUpdate').prop('disabled', false);

    $('#CommentsErrorDescriptiondrpUpdate').prop('disabled', false).trigger("liszt:updated");
    $('#CommentsErrorCategorydrpUpdate').prop('disabled', false).trigger("liszt:updated");
    $('#CommentsErrorSubCategorydrpUpdate').prop('disabled', false).trigger("liszt:updated");
}

