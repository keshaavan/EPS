var MasterData = {};
MasterData.isLookupInsert = false;
MasterData.isCommentsInsert = false;
MasterData.isQueueInsert = false;
MasterData.oTable_new = null;
MasterData.oTable = null;
MasterData.oTable_Queue = null;

MasterData.initialise = function () {
    $('#divLookupEdit').hide();
    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.IsUserAuthorized, uParam, 'MasterData.isAuthorised', true, true);
}


MasterData.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {
        var tabs = $("#tabs").tabs();
        tabs.find(".ui-tabs-nav").sortable({
            axis: "x",
            stop: function () {
                tabs.tabs("refresh");
            }
        });

        // Tab 1
        MasterData.PrepareLookupValuesCategories();
        MasterData.PrepareLookUpValues();
        MasterData.isLookupInsert = false;

        // Tab 2 
        MasterData.CommentsCategories();
        MasterData.PrepareCommentsCategoriesTable();
        MasterData.isCommentsInsert = false;
        $('#btnLookupAdd').attr("disabled", "disabled");
        $('#btnCategoryAdd').attr("disabled", "disabled");

        // Tab 3 Queue
        MasterData.PrepareQueueTable();
        MasterData.LoadQueueDatum();
        MasterData.isQueueInsert = false;

        ///// Queue

        $('#cmdAddQueue').click(function () {
            MasterData.isCommentsInsert = true;
            $('#HeaderQueue').html("Add Queue");
            $('#divQueueAddOrEdit').show("slide", { direction: "left" }, 500);
            $('#TxtQueuename').focus();
            $('#hdnQueue').val("");
            $('#hidflag').val("I");
            $('#TxtQueuename').val("");
            $('#ChkQActive').prop('checked', true);
        });

        $("#btnQueueCancel").click(function () {
            $('#divQueueAddOrEdit').hide("slide", { direction: "left" }, 500);
        });


        $("#btnQueueSave").click(function () {
            var Flag = $('#hidflag').val();
            //            if (MasterData.isQueueInsert == false)
            //                var QueueId = $('#hdnQueue').val();
            //            else
            //                var QueueId = 0

            if (Flag == 'I') {
                var QueueId = 0
                MasterData.isQueueInsert = true;
            }
            else {
                var QueueId = $('#hdnQueue').val();
                MasterData.isQueueInsert = false;
            }

            var Queuename = $('#TxtQueuename').val().replace(/'/g, "\\'");

            if ($('#ChkQActive').prop('checked') == true)
                var QueueIsActive = true;
            else
                var QueueIsActive = false;

            if ($.trim(Queuename) == '') {
                Notification.Information(Notification.enumMsg.RequiredQueueName);
                return false;
            }
           

            var sParam = "{ Flag:'" + Flag + "', QueueId:'" + QueueId + "', Queuename: '" + $.trim(Queuename) + "', QueueIsActive: '" + QueueIsActive + "' }";
            ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.InsertUpdateQueue, sParam, 'MasterData.InsertUpdateQueue', true, true);
            $('#divQueueAddOrEdit').hide("slide", { direction: "left" }, 500);
        });

        ///// Queue

        $('#txtLookupDisplayOrder').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;

            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });

        $('#txtCommentDisplayOrder').keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match('^[0-9]+$'))
                return true;

            Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
            return false;
        });

        $('#btnLookupAdd').click(function () {
            MasterData.isLookupInsert = true;
            $('#headerlookup').html("Add new Look up Values");
            $('#divLookupEdit').show("slide", { direction: "left" }, 500);
            $('#txtLookupName').focus();
            $('#hdnLookupId').val("");
            $('#txtLookupName').val("");
            $('#txtLookupDisplayOrder').val("");
            $('#chkLookupIsActive').prop('checked', true);
        });

        $('#btnCategoryAdd').click(function () {
            MasterData.isCommentsInsert = true;
            $('#Headercomments').html("Add comment");
            $('#divEditCommentCategory').show("slide", { direction: "left" }, 500);
            $('#txtCommentDescription').focus();
            $('#hdnCommentId').val("");
            $('#txtCommentDescription').val("");
            $('#txtCommentDisplayOrder').val("");
            $('#chkCommentsActive').prop('checked', true);
        });



        $("#btnLookupSave").click(function () {
            var lookupId = $('#hdnLookupId').val();
            var lookupName = $('#txtLookupName').val().replace(/'/g, "\\'");
            var lookupDisplayOrder = $('#txtLookupDisplayOrder').val();
            var isActive = $('#chkLookupIsActive').attr('checked') ? 1 : 0;
            var e = document.getElementById("ddlLookupValues");
            var lookupCategory = $.trim(e.options[e.selectedIndex].value);

            if ($.trim(lookupName) == '') {
                Notification.Information(Notification.enumMsg.RequiredLookupName);
                return false;
            }
            if ($.trim(lookupDisplayOrder) == '') {
                Notification.Information(Notification.enumMsg.RequiredDisplayOrder);
                return false;
            }

            $('#divLookupEdit').hide("slide", { direction: "left" }, 500);

            if (MasterData.isLookupInsert == false) {
                var sParam = "{ lookupId: " + lookupId + ", lookupName: '" + $.trim(lookupName) + "', lookupDisplayOrder: " + lookupDisplayOrder + ", lookupCategory: '" + lookupCategory + "', isActive:" + isActive + " }";
                ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.UpdateLookupData, sParam, 'MasterData.UpdateLookupComplete', true, true);
            }
            else {
                var sParam = "{ lookupName:'" + $.trim(lookupName) + "', lookupDisplayOrder: " + lookupDisplayOrder + ", lookupCategory: '" + lookupCategory + "', isActive: " + isActive + " }";
                ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.InsertLookupData, sParam, 'MasterData.InsertLookupComplete', true, true);
            }
        });

        $("#btnLookupCancel").click(function () {
            $('#divLookupEdit').hide("slide", { direction: "left" }, 500);
        });

        $("#btnCommentsSave").click(function () {
            if (MasterData.isCommentsInsert == false)
                var commentId = $('#hdnCommentId').val();
            else
                var commentId = 0

            var commentDescription = $('#txtCommentDescription').val().replace(/'/g, "\\'");
            var e = document.getElementById("ddlCommentsCategories");
            var commentCategoryId = $.trim(e.options[e.selectedIndex].value);
            var commentDisplayOrder = $('#txtCommentDisplayOrder').val();

            if ($('#chkCommentsActive').prop('checked') == true)
                var commentIsActive = true;
            else
                var commentIsActive = false;

            if ($.trim(commentDescription) == '') {
                Notification.Information(Notification.enumMsg.RequiredCommentDescription);
                return false;
            }
            if ($.trim(commentDisplayOrder) == '') {
                Notification.Information(Notification.enumMsg.RequiredDisplayOrder);
                return false;
            }
            $('#divEditCommentCategory').hide("slide", { direction: "left" }, 500);

            var sParam = "{ commentId:" + commentId + ", commentDescription: '" + $.trim(commentDescription) + "', commentCategoryId: " + commentCategoryId + ", displayOrder: '" + commentDisplayOrder + "', commentIsActive: '" + commentIsActive + "' }";
            ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.InsertUpdateComments, sParam, 'MasterData.InsertUpdateComments', true, true);
        });

        $("#btnCommentsCancel").click(function () {
            $('#divEditCommentCategory').hide("slide", { direction: "left" }, 500);
        });

        $('#ddlLookupValues').change(function () {
            var e = document.getElementById("ddlLookupValues");
            if ($.trim(e.options[e.selectedIndex].value) == "")
                $('#btnLookupAdd').attr("disabled", "disabled");
            else
                $('#btnLookupAdd').removeAttr("disabled", "disabled");

            MasterData.LoadLookupDatum();
        });

        $('#ddlCommentsCategories').change(function () {
            var e = document.getElementById("ddlCommentsCategories");
            if ($.trim(e.options[e.selectedIndex].value) == "0")
                $('#btnCategoryAdd').attr("disabled", "disabled");
            else
                $('#btnCategoryAdd').removeAttr("disabled", "disabled");

            MasterData.LoadCommentsCategories();
        });
    }
    else {
        window.location.href = "Logout.aspx";
    }
}

///////////////////////////////
MasterData.PrepareLookupValuesCategories = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.GetLookupDataCategories, sParam, 'MasterData.BindLookupCategories', true, true);
}

////////////////////////////////////////////////////////////
MasterData.BindLookupCategories = function (msg) {
    $('#ddlLookupValues').append($("<option></option>").val("").html("--Select One--"));
    $("#ddlLookupValues").trigger("liszt:updated");
    var itm;
    $.each(msg, function (item, Val) {
        $('#ddlLookupValues').append($("<option></option>").val(Val).html(Val));
        $("#ddlLookupValues").trigger("liszt:updated");
    });

    $('#ddlLookupValues option:nth(1)').attr("selected", "selected");
    $("#ddlLookupValues").trigger("liszt:updated");

    MasterData.LoadLookupDatum();
}

MasterData.PrepareLookUpValues = function () {
    MasterData.oTable = $('#tblLookupDatum').dataTable({
      "sScrollX": "100%",
       // "bAutoWidth": true,
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sSortable": null, "sClass": "centerAlign", "sWidth": "8%", "bSortable": false,
                            "fnRender": function (o, val) {
                                return "<a style='cursor:hand;colour: #696969;' onclick='javascrip:EditLookupMasterData(\"" + o.aData[0].toString() + "\",\"" + escape(o.aData[2]) + "\",\"" + o.aData[4] + "\",\"" + o.aData[5] + "\");'><img src='Images/Edit.png' alt='Edit' /></a>";
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "12%", "bSortable": true },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "24%", "bSortable": true },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "15%", "bSortable": true },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "15%", "bSortable": true },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "10%", "bSortable": true }
		            ],
        "iDisplayLength": 25,
        "aaSorting": [[4, 'desc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Records)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });
    $('.dataTables_empty').text('No data');
    //MasterData.LoadLookupDatum();
}

//InsertUpdateComments
MasterData.InsertUpdateComments = function (msg) {
    if (msg == 1) {
        if (MasterData.isCommentsInsert == true)
            Notification.Information(Notification.enumMsg.InsertMessage);
        else
            Notification.Information(Notification.enumMsg.UpdateMessage);

        MasterData.LoadCommentsCategories();
    }
    else if (msg == 0) {
        Notification.Information(Notification.enumMsg.UpdateErrorMsg);
    }
    else
        Notification.Information(Notification.enumMsg.CommentDescriptionAlreadyExists);
}



MasterData.InsertLookupComplete = function (msg) {
    if (msg == 1) {
        Notification.Information(Notification.enumMsg.InsertMessage);
        MasterData.LoadLookupDatum();
    }
    else if (msg == 0) {
        Notification.Information(Notification.enumMsg.UpdateErrorMsg);
    }
    else if (msg == 2) {
        Notification.Information(Notification.enumMsg.AlreadyExistInsert);
    }
}

MasterData.UpdateLookupComplete = function (msg) {
    if (msg == 1) {
        Notification.Information(Notification.enumMsg.UpdateMessage);
        MasterData.LoadLookupDatum();
    }
    else if (msg == 0) {
        Notification.Information(Notification.enumMsg.UpdateErrorMsg);
    }
    else if (msg == 2) {
        Notification.Information(Notification.enumMsg.AlreadyExistUpdate);
    }
}

function EditLookupMasterData(id, name, displayOrder, isActive) {
    MasterData.isLookupInsert = false;
    $('#headerlookup').html("Edit Look up Values");
    $('#divLookupEdit').show("slide", { direction: "left" }, 500);
    $('#txtLookupName').focus();
    $('#hdnLookupId').val(id);
    $('#txtLookupName').val(unescape(name));
    $('#txtLookupDisplayOrder').val(displayOrder);

    if (isActive == 'True') { $('#chkLookupIsActive').prop('checked', true); }
    if (isActive == 'False') { $('#chkLookupIsActive').prop('checked', false); }
}

MasterData.LoadLookupDatum = function () {
    var e = document.getElementById("ddlLookupValues");
    var lookupCategory = $.trim(e.options[e.selectedIndex].value);

    //if (lookupCategory != '') {
    var sParam = "{ category : '" + lookupCategory + "' }";
    ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.GetLookupDatum, sParam, 'MasterData.RebindBindLookUpDatum', true, true);
    //    }
}

MasterData.RebindBindLookUpDatum = function (msg) {
    if (hasNoError(msg, true)) {
        MasterData.oTable.fnClearTable();
        MasterData.oTable.fnAddData(msg);
    }
}
  
///////////////////////////////////////////////////////////////////////////////////////////// TAB 2 //////////////////////////////////////////////////////////////////////////////////////////////
MasterData.PrepareCommentsCategoriesTable = function () {
    MasterData.oTable_new = $('#tblCommentsCategories').dataTable({
        "bAutoWidth": false,
        "sScrollX": "",
        "bProcessing": false,
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sSortable": null, "sClass": "centerAlign", "sWidth": "30%", "bSortable": false,
                            "fnRender": function (o, val) {
                                return "<a style='cursor:hand;colour: #696969;' onclick='javascrip:EdiCommentstMasterData(\"" + o.aData[0].toString() + "\",\"" + escape(o.aData[1]) + "\", \"" + o.aData[2] + "\", \"" + o.aData[3] + "\");'><img src='Images/Edit.png' alt='Edit' /></a>";
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "40%", "bSortable": true },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "15%", "bSortable": true },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "15%", "bSortable": true }
		            ],
        "iDisplayLength": 25,
        "aaSorting": [[2, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Records)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });
    $('.dataTables_empty').text('No data available in table');
    //alert(MasterData.oTable_new);
    
}

function EdiCommentstMasterData(id, description, displayOrder, isActive) {
    MasterData.isCommentsInsert = false;
    $('#Headercomments').html("Edit comment");
    $('#divEditCommentCategory').show("slide", { direction: "left" }, 500);
    $('#txtCommentDescription').focus();
    $('#hdnCommentId').val(id);
    $('#txtCommentDescription').val(unescape(description));
    $('#txtCommentDisplayOrder').val(displayOrder);

    if (isActive == "True")
        $('#chkCommentsActive').prop('checked', true);
    else
        $('#chkCommentsActive').prop('checked', false);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////// INITIAL TAB 2 DROP DOWN

MasterData.LoadCommentsCategories = function () {
    var e = document.getElementById("ddlCommentsCategories");
    var commentCategoryId = $.trim(e.options[e.selectedIndex].value);
    var sParam = "{ commentCategoryId : '" + commentCategoryId + "' }";
    ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.GetCommentsByCategoryTable, sParam, 'MasterData.RebindCommentsCategories', true, true);
}

MasterData.RebindCommentsCategories = function (msg) {
    if (hasNoError(msg, true)) {
        MasterData.oTable_new.fnClearTable();
        MasterData.oTable_new.fnAddData(msg);
    }
}

///////////////////////////////////////////////////////////////////////////////// Drop down Tab 2 
MasterData.CommentsCategories = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.GetCommentsCategories, sParam, 'MasterData.BindCommentsCategories', true, true);
}

MasterData.BindCommentsCategories = function (msg) {
    var itm;
    $('#ddlCommentsCategories').append($("<option></option>").val("0").html("--Select One--"));
    $("#ddlCommentsCategories").trigger("liszt:updated");
    $.each(msg, function (index, item) {
        $('#ddlCommentsCategories').append($("<option></option>").val(item[0]).html(item[1]));
        $("#ddlCommentsCategories").trigger("liszt:updated");
    });

    $('#ddlCommentsCategories option:nth(1)').attr("selected", "selected");
    $("#ddlCommentsCategories").trigger("liszt:updated");
}

///////////////////////////////////////////////////////////////////////////////////////////// TAB 3 //////////////////////////////////////////////////////////////////////////////////////////////
MasterData.PrepareQueueTable = function () {
    MasterData.oTable_Queue = $('#tblQueue').dataTable({
        "bAutoWidth": false,
        "sScrollX": "",
        "bProcessing": false,
        "bFilter": false,
        "sDom": '<"bottom"pf>rt<"bottom"i><"clear"l>',
        "aoColumns": [
                        { "sSortDataType": "dom-text", "sSortable": null, "sClass": "centerAlign", "sWidth": "30%", "bSortable": false,
                            "fnRender": function (o, val) {
                                return "<a style='cursor:hand;colour: #696969;' onclick='javascrip:EdiQueueMasterData(\"" + o.aData[0].toString() + "\",\"" + escape(o.aData[1]) + "\", \"" + o.aData[2] + "\");'><img src='Images/Edit.png' title='Edit' /></a>";
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "leftAlign", "sWidth": "40%", "bSortable": true },
                        { "sSortDataType": "dom-text", "sClass": "rightAlign", "sWidth": "15%", "bSortable": true }
		            ],
        "iDisplayLength": 25,
        "aaSorting": [[2, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Records)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."
        }
    });
    $('.dataTables_empty').text('No data available in table');
}

MasterData.LoadQueueDatum = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.MasterData, enumWebMethod.GetQueue, sParam, 'MasterData.bindQueueDatum', true, true);
}

MasterData.bindQueueDatum = function (msg) {
    if (hasNoError(msg, true)) {
        MasterData.oTable_Queue.fnClearTable();
        MasterData.oTable_Queue.fnAddData(msg);
    }
}

function EdiQueueMasterData(id, description, isActive) {
    MasterData.isCommentsInsert = false;
    $('#HeaderQueue').html("Edit Queue");
    $('#divQueueAddOrEdit').show("slide", { direction: "left" }, 500);
    $('#TxtQueuename').val(unescape(description));
    $('#TxtQueuename').focus();
    $('#hdnQueue').val(id);
    $('#hidflag').val("U");

    if (isActive == "True")
        $('#ChkQActive').prop('checked', true);
    else
        $('#ChkQActive').prop('checked', false);
}

//InsertUpdateQueue
MasterData.InsertUpdateQueue = function (msg) {
    if (msg == 0) {
        if (MasterData.isQueueInsert == true)
            Notification.Information(Notification.enumMsg.InsertMessage);
        else
            Notification.Information(Notification.enumMsg.UpdateMessage);

        MasterData.LoadQueueDatum();
    }
    else if (msg == 1) {
        //Notification.Information(Notification.enumMsg.UpdateErrorMsg);
        Notification.Information(Notification.enumMsg.QueueAlreadyExists);
    }
    //    else
    //        Notification.Information(Notification.enumMsg.QueueAlreadyExists);
}



