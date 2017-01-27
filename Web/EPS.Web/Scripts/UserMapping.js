var UserMapping = {};

UserMapping.initialise = function () {
    var uParam = "{ userName: '" + appUser.UserName + "'}";
    ServiceBroker.callWebMethod(enumWebPage.UserMapping, enumWebMethod.IsUserAuthorized, uParam, 'UserMapping.isAuthorised', true, true);
}

UserMapping.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
    }
    else if (msg) {

        $('#chkSelectAll').click(function (evt) {

            $('.display').find(':checkbox').attr('checked', this.checked);
        });

        UserMapping.PrepareCharts();
        $("#btnUserMapping").click(function () {
             alert('hi');
           // UserMapping.ActiveInactiveUserMapping();
        });

        UserMapping.PrepareBindCharts();
       UserMapping.BindActiveProjects();
    }
    else {
        window.location.href = "Logout.aspx";
    }
}


UserMapping.PrepareCharts = function () {
    UserMapping.oTable = $('#tblUserMapping').dataTable({
        "sScrollX": "100%",
        "aoColumns": [
                        { "sSortDataType": "dom-anchor", "sSortable": null, "sClass": "centerAlign", "sWidth": "8%", "bSortable": false,
                            "fnRender": function (o, val) {
                                return "<input id='" + o.aData[1] + "' class='loan-checkbox' data-clientChartid='" + o.aData[1] + "' type='checkbox' >";
                            }
                        },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "10%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "20%", "bSortable": false },
                        { "sSortDataType": "dom-text", "sClass": "centerAlign", "sWidth": "30%", "bSortable": false,
                            "mRender": function () {
                                var strDropdown = "<select style='width:400px;' id=selProvider </select>";
                               // UserMapping.BindActiveProjects();
                                return strDropdown;
                            }
                            
                        }
                        
		            ],
        "iDisplayLength": 30,
        "aaSorting": [[1, 'asc']],
        "oLanguage": { "sInfo": "Showing _START_ to _END_ of _TOTAL_ Records",
            "sInfoFiltered": "(filtered from _MAX_ total Prospects)",
            "sInfoEmpty": "No Records Found",
            "sLoadingRecords": "Please wait - loading..."           
        }        
    });
    
    $('.dataTables_empty').text('Loading Data...');
}

UserMapping.PrepareBindCharts = function () {
    $.blockUI({ message: '<h1><img src="Images/loadinfo.net(3).gif" /> Just a moment...</h1>' });
    $('#chkSelectAll').prop('checked', false);

    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.UserMapping, enumWebMethod.GetUserDetails, sParam, 'UserMapping.BindCharts', true, true);

}

UserMapping.BindActiveProjects = function () {
    var sParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.UserMapping, enumWebMethod.GetActiveProjectList, sParam, 'UserMapping.BindActive', true, true);
}

UserMapping.BindActive = function (msg) {
    if (hasNoError(msg, true)) {
        $.each(msg, function (index, item) {
            $('#selProvider').append($('<option>', {
                value: item[0],
                text: item[1]
            }));       
        });
        $(document).ajaxStop($.unblockUI);
    }
}


UserMapping.BindCharts = function (msg) {
    if (hasNoError(msg, true)) {
        UserMapping.oTable.fnClearTable();
        UserMapping.oTable.fnAddData(msg);
        // unblock when ajax activity stops 
        $(document).ajaxStop($.unblockUI);
    }
}

UserMapping.ActiveInactiveUserMapping = function () {
    var selectedCheckBox = '';
    var control = $('input:checkbox')
    var checkAll = 0;

    if ($('#chkSelectAll').prop('checked') == true) {
        $('#chkSelectAll').prop('checked', false);
        checkAll = 1;
    }

    for (var i = 0; i < control.length; i++) {
        var ctrlVal = "|" + control[i].value + "|";
        var isChecked = $('#' + control[i].id + '').attr('checked') ? 1 : 0;
        if (isChecked == 1) {
            var ctrlid = "" + control[i].id + "|";
            selectedCheckBox = selectedCheckBox + ctrlid;
        }
    }

    $('#selProvider option').each(function () {
        alert($(this).val());
    });
    var drpValue = "";
    


    if (checkAll == 1)
        $('#chkSelectAll').prop('checked', true);

    if (selectedCheckBox != '') {
        if (confirm("Alert\n---------------------------------------------------------------\n All selected User would be Mapped.\n\n  Do you want to continue?\n---------------------------------------------------------------", "Confirm direct upload")) {
            var vParam = "{ InactivateId : " + selectedCheckBox + " , ActivateprojectId : " + drpValue +"}";
            ServiceBroker.callWebMethod(enumWebPage.UserMapping, enumWebMethod.ActiveInactiveUserMapping, vParam, 'UserMapping.ActiveInactiveUserMappingConfirm', true, true);
            Notification.Information(Notification.enumMsg.ActivateSuccessfully);

        }
        else {
            ActiveInactiveUserMappingConfirm();
        }
    }
    else {
        Notification.Information(Notification.enumMsg.NoUserSelectedForActivateProjects);
        return false;
    }
}
UserMapping.ActiveInactiveUserMappingConfirm = function () {
    $('#delete-dialog').dialog("close");
    UserMapping.PrepareBindCharts();
}