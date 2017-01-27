var Level3 = {};

Level3.oTable = null;
Level3.oTable_Part2 = null;
Level3.levelStatusId = 0;
Level3.RequestL3ChartClicked = false;
var chartAuditCount = 1;
Level3.oTable_AuditLog = null;
var l3DisagreeOptions = [];
Level3.oTable_AuditLogView = null;

Level3.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.IsUserAuthorized, uParam, 'Level3.isAuthorised', true, true);
}

Level3.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        $("input[id=btnL3RequestChart]").attr("disabled", "disabled");
        $("input[id=btnL3RequestChart]").attr("class", "button_disabled");
        $("#lblClientReferenceHeading4ChartUpdate").html(appUser.ClientReferenceLabel);
        $("#lblClientReferenceHeadingOtherCharts").html(appUser.ClientReferenceLabel);
        $("#lblClientReferenceHeadingActiveCharts").html(appUser.ClientReferenceLabel);
        $("#lblCommentsHeadingClientReference").html(appUser.ClientReferenceLabel);

        if (appUser.IsL3Auto != "True") {
            $("#txtL3SearchClientReferance").css('visibility', 'visible');
            $('#txtL3SearchClientReferance').focus();
        }
        else
            $("#txtL3SearchClientReferance").css('visibility', 'hidden');

        Level3.LoadPendingChartsCount();
        HideWIPEdit();
        Level3.PrepareStatus();
        Level3.PreparePendingCharts();
        Level3.PrepareOtherCharts();
        Level3.PrepareAuditLog();
        Level3.PrepareAuditViewLog();

        HideWIPEditAuditComments();
        Level3.PrepareErrorDescription();
        Level3.PrepareErrorCategory();
        Level3.PrepareErrorSubCategory();

        $('#txtL3NoOfPages').keypress(function (e) {
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

        $('#txtL3NoOfDxCodes').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;

            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });


        $("#btnL3Close").click(function () {
            $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
        });

        $("#btnL3Save").click(function () {
            $("input[id=btnL3Save]").attr("disabled", "disabled");
            $("input[id=btnL3Save]").attr("class", "button_disabled");

            var errorMsgs = "";
            var chartId = $('#hdnChartId').val();
            var chartMoreInfoId = $('#hdnChartMoreInfoId').val();
            var statusId = $('#ddlL3Status').val();
            var statusCommentId = $('#ddlL3StatusComment').val();
            var noPages = $('#txtL3NoOfPages').val();
            var noDxCodes = $('#txtL3NoOfDxCodes').val();
            var overallStatus = "L3 - " + $("#ddlL3Status option:selected").text();

            Level3.RequestL3ChartClicked = false;

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
                $("input[id=btnL3Save]").removeAttr("disabled");
                $("input[id=btnL3Save]").attr("class", "button");

                errorMsgs = "<br/>The following fields are mandatory:<br/>" + errorMsgs + "<br/>";
                $('#lblErr').html(errorMsgs);
            }
            else {
                var sParam = "{ chartId: " + chartId + ", chartMoreId: " + chartMoreInfoId + ", levelStatusId: " + statusId + ", levelStatusCommentId: " + statusCommentId + ", noOfPages: " + noPages + ", noOfDxCodes: " + noDxCodes + ", overallStatus: '" + overallStatus + "' }";

                if (statusId == 1) {
                    if (confirm("Do you want to complete the Chart?")) {
                        ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.SaveL3DataChartInfo, sParam, 'Level3.PendingEditCompleted', true, true);
                        $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
                    }
                    else {
                        $("input[id=btnL3Save]").removeAttr("disabled");
                        $("input[id=btnL3Save]").attr("class", "button");
                    }
                }
                else if (statusId == 3) {
                    if (confirm("Do you want to save as an invalid Chart?")) {
                        ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.SaveL3DataChartInfo, sParam, 'Level3.PendingEditCompleted', true, true);
                        $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
                    }
                    else {
                        $("input[id=btnL3Save]").removeAttr("disabled");
                        $("input[id=btnL3Save]").attr("class", "button");
                    }
                }
                else {
                    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.SaveL3DataChartInfo, sParam, 'Level3.PendingEditCompleted', true, true);
                    $('#dvEditWIP').hide("slide", { direction: "left" }, 500);
                }
            }
        });

        $("#btnL3ActionClose").click(function () {
            $('#dvEditAuditComments').hide("slide", { direction: "left" }, 500);
        });

        $("#btnAuditLogClose").click(function () {
            $('#AuditLogDialog').dialog('close');
        });

        $("#btnAuditLogSave").click(function () {
            Level3.saveAuditLogs();
        });

        $("#btnAuditLogViewClose").click(function () {
            $('#AuditLogViewDialog').dialog('close');
        });

        $("#btnL3ActionSave").click(function () {
            $("input[id=btnL3ActionSave]").attr("disabled", "disabled");
            $("input[id=btnL3ActionSave]").attr("class", "button_disabled");

            var errorMsgs = "";
            var commentsChartId = $('#hdnCommentsChartId').val();  // Chart Id
            var commentsChartMoreInfoId = $('#hdnCommentsChartMoreInfoId').val(); // Chart More Info Id
            var commentsNoPages = $('#txtCommentsPageNo').val();
            var commentsCorrectedValues = $.trim($('#txtCommentsCorrectedValues').val());

            var commentsErrorDescriptiondrp = $("#CommentsErrorDescriptiondrp option:selected").text()

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
                $("input[id=btnL3ActionSave]").removeAttr("disabled");
                $("input[id=btnL3ActionSave]").attr("class", "button");
                errorMsgs = "<br/>The following fields are mandatory:<br/>" + errorMsgs + "<br/>";
                $('#lblCommentsErr').html(errorMsgs);
            }
            else {
                var sParam = "{ chartMoreInfoId: " + commentsChartMoreInfoId + ", pageNumbers: '" + commentsNoPages + "', correctedValue: '" + commentsCorrectedValues + "', comments: '" + commentsErrorDescriptiondrp + "', errorCategoryId: " + commentsErrorCategorydrp + ", errorSubCategoryId: " + commentsErrorSubCategorydrp + ",additionalComments: '" + additionalComments + "' }";
                ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.SaveL3AuditComments, sParam, 'Level3.AuditCommentsEditCompleted', true, true);
                $('#dvEditAuditComments').hide("slide", { direction: "left" }, 500);
            }
        });

        $("#btnL3Refresh").click(function () {
            Level3.RefreshBothSection();
        });

        $('#ddlL3Status').change(function () {
            var commentCategoryId = this.value;

            $('#ddlL3StatusComment').empty();
            $('#ddlL3StatusComment').append($("<option></option>").val("").html("--Select One--"));

            var sParam = "{ commentCategoryId :" + commentCategoryId + "  }";
            ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetL3StatusComments, sParam, 'Level3.BindStatusComments', true, true);
        });

        $("#btnL3RequestChart").click(function () {
            Level3.RequestL3ChartClicked = true;

            if (appUser.IsL3Auto != "True") {
                var strSearch = $('#txtL3SearchClientReferance').val();
                $.trim(strSearch);
                if (strSearch == "") {
                    alert("Please enter " + appUser.ClientReferenceLabel);
                }
                else {
                    strSearch = strSearch.replace(/'/g, "\\'");
                    $("input[id=btnL3RequestChart]").attr("disabled", "disabled");
                    $("input[id=btnL3RequestChart]").attr("class", "button_disabled");

                    var sParam_new = "{ clientReference: '" + strSearch + "' }";
                    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.RequestL3ChartByClientReference, sParam_new, 'Level3.BindRequestChartL3', true, true);
                }
            }
            else {
                $("input[id=btnL3RequestChart]").attr("disabled", "disabled");
                $("input[id=btnL3RequestChart]").attr("class", "button_disabled");

                var sParam = "{ }";
                ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.RequestL3Chart, sParam, 'Level3.BindRequestChart', true, true);
            }
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

Level3.BindRequestChartL3 = function (msg) {
    Level3.RefreshBothSection();
 
}

Level3.BindRequestChart = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
           
            Level3.RefreshBothSection();
        }
        else if (msg[0] == 0 ){
            Notification.Information(Notification.enumMsg.ChartRequestTimeOutStatus);
        }
    }
    else {
        Notification.Error(Notification.enumMsg.ChartRequestTimeOutStatus);
       
    }

}

Level3.LoadPendingChartsCount = function () {
    var vParam_chartCount = "{}";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.L3PendingChartCount, vParam_chartCount, 'Level3.BindL3PendingChartCount', true, true);   
}

Level3.BindL3PendingChartCount = function (msg) {
    if (hasNoError(msg, true)) {
        $('#lblPendingChartCount').text(msg);
    }
}
Level3.PreparePendingCharts = function () {
    Level3.oTable = $('#tblPendingCharts').dataTable({
        "sScrollX": "100%",
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "10%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[1].toString() == 'Work In Progress')
                                    return "<a style='cursor:hand;' onclick='javascript:EditL3PendingWIP(\"" + o.aData[0].toString() + "\",\"" + (escape(o.aData[4])).toString() + "\",\"" + o.aData[6].toString() + "\",\"" + escape(o.aData[7]) + "\",\"" + o.aData[15].toString() + "\");'>" + o.aData[1] + "</a>";
                                else {
                                    return "<a style='color: #696969;cursor:default;text-decoration:none;' >" + o.aData[1] + "</a>";
                                }
                            }
                        },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "5%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[2].toString() == 'Work In Progress')
                                    return "<a style='cursor:hand;' onclick='javascript:EditL3AuditCommentsWIP(\"" + o.aData[0].toString() + "\",\"" + (escape(o.aData[4])).toString() + "\",\"" + escape(o.aData[7]) + "\",\"" + o.aData[15].toString() + "\");'> Add </a>";
                                else
                                    return "<a style='color: #696969;cursor:default;text-decoration:none;' > Add</a>";

                            }
                        },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "5%", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[16].toString() == 'Work In Progress') {

                                    return "<a style='cursor:hand;' onclick='javascript:ViewL2Comments(\"" + o.aData[0].toString() + "\",\"" + 0 + "\");'> View </a>";
                                }
                                else {
                                    return "<a style='color: #696969;cursor:default;text-decoration:none;' >View</a>";
                                }
                            }
                        },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "15%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "8%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "8%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "15%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bSortable": false, "bVisible": false },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "8%", "bSortable": false },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "13%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "", "bSortable": false}
		            ],
        "iDisplayLength": 20,
        "aaSorting": [[0, 'desc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Records)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');

    Level3.LoadPendingCharts();
}

Level3.LoadPendingCharts = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetL3PendingChartData, sParam, 'Level3.BindPendingChartsL3', true, true);
}

Level3.BindPendingChartsL3 = function (msg) {
    $('#txtL3SearchClientReferance').val("");

    if (hasNoError(msg, true)) {
        Level3.oTable.fnClearTable();
        Level3.oTable.fnAddData(msg);
        if (msg.toString().length > 0) {
            $("input[id=btnL3RequestChart]").attr("disabled", "disabled");
            $("input[id=btnL3RequestChart]").attr("class", "button_disabled");
            levelStatusId = 14;
        }
        else {
            $("input[id=btnL3RequestChart]").removeAttr("disabled");
            $("input[id=btnL3RequestChart]").attr("class", "button");

            levelStatusId = 15;

            if (Level3.RequestL3ChartClicked) {
                Notification.Error(Notification.enumMsg.ChartsNotAvailable);
                Level3.RequestL3ChartClicked = false;
            }
        }
    }
}

function EditL3PendingWIP(chartId, clientReference, market, filename, chartMoreId) {
    $("input[id=btnL3Save]").removeAttr("disabled");
    $("input[id=btnL3Save]").attr("class", "button");

    $('#dvEditWIP').show("slide", { direction: "left" }, 500);
    $('#lblDisplayMarket').text(market);
    $('#lblDisplayClientReference').text(unescape(clientReference));
    $('#lblDisplayFileName').text(unescape(filename));
    $('#hdnChartId').val(chartId);
    $('#hdnChartMoreInfoId').val(chartMoreId);
    $('#ddlL3Status option:nth(0)').attr("selected", "selected");
    $('#ddlL3StatusComment option:nth(0)').attr("selected", "selected");
    $('#txtL3NoOfPages').val("");
    $('#txtL3NoOfDxCodes').val("");
    $('#lblErr').html("");
    $('#ddlL3Status').focus();
    LoadL1AandL2ChartMoreInfoList(chartId);
}

Level3.PrepareStatus = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetL3ChartStatuses, sParam, 'Level3.BindChartStatus', true, true);
}

Level3.BindChartStatus = function (msg) {
    $('#ddlL3Status').append($("<option></option>").val("").html("--Select One--"));
    $('#ddlL3StatusComment').append($("<option></option>").val("").html("--Select One--"));

    $.each(msg, function (index, item) {
        $('#ddlL3Status').append($("<option></option>").val(item[0]).html(item[1]));
    });
}

Level3.BindStatusComments = function (msg) {
    $.each(msg, function (index, item) {
        $('#ddlL3StatusComment').append($("<option></option>").val(item[0]).html(item[1]));
    });
}

Level3.PrepareErrorDescription = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetL3ErrorDescriptions, sParam, 'Level3.BindErrorDescriptions', true, true);
}

Level3.BindErrorDescriptions = function (msg) {
    $('#CommentsErrorDescriptiondrp').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorDescriptiondrp").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorDescriptiondrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorDescriptiondrp").trigger("liszt:updated");
    });
}

Level3.PrepareErrorCategory = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetL3ErrorCategories, sParam, 'Level3.BindErrorCategory', true, true);
}

Level3.BindErrorCategory = function (msg) {
    $('#CommentsErrorCategorydrp').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorCategorydrp").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorCategorydrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorCategorydrp").trigger("liszt:updated");
    });
}

Level3.PrepareErrorSubCategory = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetL3ErrorSubCategories, sParam, 'Level3.BindErrorSubCategory', true, true);
}

Level3.BindErrorSubCategory = function (msg) {
    $('#CommentsErrorSubCategorydrp').append($("<option></option>").val("").html("--Select One--"));
    $("#CommentsErrorSubCategorydrp").trigger("liszt:updated");

    $.each(msg, function (index, item) {
        $('#CommentsErrorSubCategorydrp').append($("<option></option>").val(item[0]).html(item[1]));
        $("#CommentsErrorSubCategorydrp").trigger("liszt:updated");
    });
}
function LoadL1AandL2ChartMoreInfoList(chartId) {


    var sParam = "{ chartId: " + chartId + "}";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetL1andL2ChartMoreInfoByChartId, sParam, 'Level3.BindChartMoreInfo', true, true);

}
Level3.BindChartMoreInfo = function (msg) {
    if (hasNoError(msg, true)) {
        $('#lblL1TotalEncounters').text(msg[0][0]);
        $('#lblL1TotalDxCode').text(msg[0][1]);
        $('#lblL2TotalEncounters').text(msg[1][0]);
        $('#lblL2TotalDxCode').text(msg[1][1]);
    }
}
function EditL3AuditCommentsWIP(chartId, clientReference, filename, chartMoreInfoId) {
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

    $("input[id=btnL3ActionSave]").removeAttr("disabled");
    $("input[id=btnL3ActionSave]").attr("class", "button");
    $('#lblCommentsErr').html("");
}

function EditL3OtherStatus(chartId, chartMoreInfoId) {
    if (confirm("Do you want to work in the 'Hold' chart?")) {
        var overallStatus = "";
        if (levelStatusId == 15)
            overallStatus = "L3 - Work In Progress";
        else
            overallStatus = "L3 - Pending";

        var sParam = "{ chartId: " + chartId + ", chartMoreId: " + chartMoreInfoId + ", levelStatusId: " + levelStatusId + ", levelStatusCommentId: " + 0 + ", noOfPages: " + 0 + ", noOfDxCodes: " + 0 + ", overallStatus: '" + overallStatus + "' }";
        ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.SaveL3DataChartInfo, sParam, 'Level3.OtherEditCompleted', true, true);
    }
}

Level3.PrepareOtherCharts = function () {
    Level3.oTable_Part2 = $('#tblOtherChartsview').dataTable({
        "sScrollX": "100%",
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
                         { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "8%", "bSortable": false,
                             "fnRender": function (o, val) {
                                 if (o.aData[1].toString() == 'Hold')
                                     return "<a style='cursor:hand;' onclick='javascript:EditL3OtherStatus(\"" + o.aData[0].toString() + "\",\"" + o.aData[15].toString() + "\");'>" + o.aData[1] + "</a>";
                                 else {
                                     return "<a style='color: #696969;cursor:default;text-decoration:none;' >" + o.aData[1] + "</a>";
                                 }
                             }
                         },

                          { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "8%", "bVisible": false, "bSortable": false,
                              "fnRender": function (o, val) {
                                  if (o.aData[2].toString() == 'Work In Progress')
                                      return "<a style='cursor:hand;' onclick='javascript:EditL3AuditCommentsWIP(\"" + o.aData[0].toString() + "\",\"" + (escape(o.aData[4])).toString() + "\",\"" + escape(o.aData[7]) + "\",\"" + o.aData[15].toString() + "\");'> Add </a>";
                                  else
                                      return "Add";
                              }
                          },
                           { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "8%", "bSortable": false,
                               "fnRender": function (o, val) {
                                   return "<a style='cursor:hand;' onclick='javascript:ViewL2Comments(\"" + o.aData[0].toString() + "\",\"" + 1 + "\");'> View </a>";
                               }
                           },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "15%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "5%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "15%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "10%", "bSortable": false },

                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false, "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "0%", "bSortable": false}
		            ],
        "iDisplayLength": 20,
        "aaSorting": [[6, 'desc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Records)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."

        }
    });

    $('.dataTables_empty').text('Loading Data...');

    Level3.LoadOtherCharts();
}

Level3.LoadOtherCharts = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetL3OtherChartData, sParam, 'Level3.BindOtherCharts', true, true);
}

Level3.BindOtherCharts = function (msg) {
    if (hasNoError(msg, true)) {
        Level3.oTable_Part2.fnClearTable();
        Level3.oTable_Part2.fnAddData(msg);
    }
}

function HideWIPEdit() {
    $('#dvEditWIP').hide();
}

function HideWIPEditAuditComments() {
    $('#dvEditAuditComments').hide();
}

Level3.RefreshBothSection = function () {
    Level3.LoadPendingChartsCount();
    Level3.LoadPendingCharts();
    Level3.LoadOtherCharts();
}

Level3.PendingEditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.ChartMoveToCompletedHoldInvalidCharts);
    Level3.RefreshBothSection();
}

Level3.OtherEditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.ChartMoveToWIPPendingCharts);
    Level3.RefreshBothSection();
}

Level3.AuditCommentsEditCompleted = function (msg) {
    Notification.Information(Notification.enumMsg.AuditCommentAddedSuccess);
}

Level3.PrepareLevelDisagreeOptions = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetLevelDisagreeOptions, sParam, 'Level3.BindLevelDisagreeOptions', true, true);
}

Level3.BindLevelDisagreeOptions = function (msg) {
    l3DisagreeOptions[0] = new Array(2);
    l3DisagreeOptions[0][0] = "0";
    l3DisagreeOptions[0][1] = "--Select One--";

    var iLen = msg.length;
    for (var i = 0; i < iLen; i++) {
        l3DisagreeOptions[i + 1] = new Array(2);
        l3DisagreeOptions[i + 1][0] = msg[i][0];
        l3DisagreeOptions[i + 1][1] = msg[i][1];
    }
}

Level3.PrepareAuditLog = function () {
    Level3.oTable_AuditLog = $('#tblAuditLog').dataTable({
        "bAutoWidth": false,
        "sScrollX": "",
        "bProcessing": true,
        "aoColumns": [
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[9] == "2") {
                                    if (o.aData[11] != "0") {
                                        return "<input id='chk_" + o.aData[12] + "'  onchange='javascript:changeSelectedValue(\"" + o.aData[12] + "\");' class='loan-checkbox' data-clientChartid='" + o.aData[12] + "' type='checkbox' checked>";
                                    }
                                    else {
                                        return "<input id='chk_" + o.aData[12] + "'  onchange='javascript:changeSelectedValue(\"" + o.aData[12] + "\");' class='loan-checkbox' data-clientChartid='" + o.aData[12] + "' type='checkbox'>";
                                    }
                                }
                                else {
                                    return "";
                                }
                            }
                        },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "", "bSortable": false,
                            "fnRender": function (o, val) {
                                if (o.aData[9] == "2") {
                                    var r = '<select id="sel_' + o.aData[12] + '">';
                                    for (var i = 0; i < l3DisagreeOptions.length; i++) {
                                        if (o.aData[11] == l3DisagreeOptions[i][0]) {
                                            r += '<option value="' + l3DisagreeOptions[i][0] + '" selected>' + l3DisagreeOptions[i][1] + '</option>';
                                        }
                                        else {
                                            r += '<option value="' + l3DisagreeOptions[i][0] + '">' + l3DisagreeOptions[i][1] + '</option>';
                                        }
                                    }
                                    return r + '</select>'; 
                                }
                                else {
                                    return "";
                                }
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "bVisible": false }
		            ],
        "iDisplayLength": 10,
        "aaSorting": [[2, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
}

ViewL2Comments = function (chartId, viewOnly) {
    var vParam = "{ chartId: " + chartId + "}";

    if (viewOnly == "1") {
        $("#AuditLogViewDialog").dialog({
            modal: true,
            height: 700,
            width: 1200
        });

        ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetChartAuditLogsByChartId, vParam, 'Level3.BindChartAuditViewLogsData ', true, true);
    }
    else {
        $("#AuditLogDialog").dialog({
            modal: true,
            height: 700,
            width: 1200
        });

        Level3.PrepareLevelDisagreeOptions();
        ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.GetChartAuditLogsByChartId, vParam, 'Level3.BindChartAuditLogsData ', true, true);
    }
}

Level3.BindChartAuditLogsData = function (msg) {
    if (hasNoError(msg, true)) {
        Level3.oTable_AuditLog.fnClearTable();
        Level3.oTable_AuditLog.fnAddData(msg);
    }
}

changeSelectedValue = function (chartid) {
    var chkid = 'chk_' + chartid;

    var isChecked = $('#' + chkid + '').attr('checked') ? 1 : 0;

    if (isChecked == 0) {
        $('#sel_' + chartid + ' option:nth(0)').attr("selected", "selected");
       // $('#sel_' + chartid + ' option').attr("disabled", "disabled");
    }
    else {
       // $('#sel_' + chartid + ' option').attr("disabled", false);
    }
}

Level3.saveAuditLogs = function () {
    var selectedCheckBoxes = '';
    var flag = true;
    var control = $('input:checkbox');

    for (var i = 0; (i < control.length) && flag; i++) {
        var controlId = control[i].id;

        var isChecked = $('#' + controlId + '').attr('checked') ? 1 : 0;

        var chartAuditId = controlId.replace("chk_", "");

        if (isChecked == 1) {
            var e = document.getElementById("sel_" + chartAuditId);
            var levelDisagreeid = e.options[e.selectedIndex].value;
            if (levelDisagreeid == 0) {
                flag = false;
            }
            else {
                selectedCheckBoxes = selectedCheckBoxes + chartAuditId + ";" + levelDisagreeid + "|";
            }
        }
        else
            selectedCheckBoxes = selectedCheckBoxes + chartAuditId + ";0|";
    }

    if (flag) {
        var vParam = "{ chartAuditIds :'" + selectedCheckBoxes + "' }";
        ServiceBroker.callWebMethod(enumWebPage.Level3, enumWebMethod.SaveChartAuditList, vParam, 'Level3.ChartAuditConfirm', true, true);
    }
    else {
        Notification.Information(Notification.enumMsg.NoChartAuditLogSelected);
        return false;
    }
}

Level3.ChartAuditConfirm = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
            $('#AuditLogDialog').dialog('close');
            Notification.Information(Notification.enumMsg.UpdateMessage);
        }
        else {
            Notification.Error('Error');
        }
    }
}

Level3.PrepareAuditViewLog = function () {
    Level3.oTable_AuditLogView = $('#tblAuditLogView').dataTable({
        "bAutoWidth": false,
        "sScrollX": "",
        "bProcessing": true,
        "aoColumns": [
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "leftAlign", "sWidth": "", "bSortable": false, "bVisible": false,
                            "fnRender": function (o, val) {
                                return "";
                            }
                        },
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "leftAlign", "sWidth": "", "bSortable": false,
                            "fnRender": function (o, val) {
                                return o.aData[10];
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign" },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign" }
		            ],
        "iDisplayLength": 10,
        "aaSorting": [[2, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });

    $('.dataTables_empty').text('Loading Data...');
}

Level3.BindChartAuditViewLogsData = function (msg) {
    if (hasNoError(msg, true)) {
        Level3.oTable_AuditLogView.fnClearTable();
        Level3.oTable_AuditLogView.fnAddData(msg);
    }
}
