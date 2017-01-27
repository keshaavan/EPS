var SiteMasterVar = {};
SiteMasterVar.g_sPage = "home.aspx";
SiteMasterVar.g_sMenuID = null;
SiteMasterVar.g_iMenuRefreshMSec = 3600000; //Refresh Dynamic menu every 1 hr
SiteMasterVar.g_sTitlePrefix = "EPS - ";

var g_currentSelected = '';
var g_previousSelected = '';
var s_currentFilterOptionsId = '';
var passwordLength = 8;

$(document).ready(function () {
    if (validateBrowser()) {
        $('#page').show();
        initialiseNotification();
        handleInitialLoad();
        setCurrentPageHighlighting();
        wireUpEvents();

        setNavHighlighting();
        asyncPageLoad();

        SetNavigationForUser();
    }

    $("#changepasswordAnchor").click(function () {
        openChangePasswordDialog();
    });
    /*$("#lblVersion").click(function () {
        versionInfoDialog();
    });*/
    
    $("#btnUpdate").click(function () {
        SaveChangePassword();
    });

    $("#oldpassword").keydown(function (event) {
        if (event.keyCode == 32) {
            event.preventDefault();
        }
    });
    //    $("#newpassword").keyup(function() {

    //    var inputVal = $(this).val();
    //    var characterReg =/^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^+=]).*$/;
    //                if(!characterReg.test(inputVal)) {
    //                    Notification.Information(Notification.enumMsg.PasswordFormatError);
    //                }
    //       });
    $("#newpassword").keydown(function (event) {
        if (event.keyCode == 32) {
            event.preventDefault();
        }
    });

    $("#ConfirmNewPassword").keydown(function (event) {
        if (event.keyCode == 32) {
            event.preventDefault();
        }
    });

    $("#btnReset").click(function () {
        $('#ChangepasswordDialog').dialog("close");
        ResetPasswordChange();
    });
});

function openChangePasswordDialog() {
    $("#ChangepasswordDialog").dialog({
        dialogClass: "OverlayDialog",
        modal: true,
        height: 340,
        width: 670,
        close: function (ev, ui) {
            var uParam = "{ }";
            ServiceBroker.callWebMethod(enumWebPage.Home, enumWebMethod.PasswordExpired, uParam, 'PasswordExpired', true, true);
        }
    });
    ResetPasswordChange();
    UserPasswordChange();
}
function versionInfoDialog() {
    $("#versioninfo").dialog({
        dialogClass: "OverlayDialog",
        modal: true,
        height: 450,
        width: 670,
       });
}
PasswordExpired = function (msg) {
    if (msg) {
        openChangePasswordDialog();
    }
}

function UserPasswordChange() {
    $('#oldpassword').focus();
}

function ResetPasswordChange() {
    $('#oldpassword').focus();
    $('#oldpassword').val('');
    $('#newpassword').val('');
    $('#ConfirmNewPassword').val('');
}

function SaveChangePassword() {
    var oldPassword = $.trim($('#oldpassword').val());
    var newPassword = $.trim($('#newpassword').val());
    var confirmPassword = $.trim($('#ConfirmNewPassword').val());
    var newPasswordLength = $.trim($('#newpassword').val()).length;
    var confirmPasswordLength = $.trim($('#ConfirmNewPassword').val()).length;
    var passwordvalidation = /^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+=]).*$/;

    if (oldPassword == '') {
        Notification.Information(Notification.enumMsg.RequiredOldPassword);
        return false;
    }
    else if (newPassword == '') {
        Notification.Information(Notification.enumMsg.RequiredNewPassword);
        return false;
    }
    else if (confirmPassword == '') {
        Notification.Information(Notification.enumMsg.RequiredConfirmPassword);
        return false;
    }
    else if (newPassword != confirmPassword) {
        Notification.Information(Notification.enumMsg.InvalidNewPasswordMatch);
        return false;
    }
    else if (newPasswordLength < passwordLength) {
        Notification.Information(Notification.enumMsg.InvalidPasswordMinLength + passwordLength);
        return false;
    }
    else if (confirmPasswordLength < passwordLength) {
        Notification.Information(Notification.enumMsg.InvalidPasswordMinLength + passwordLength);
        return false;
    }
    else if (oldPassword == newPassword) {
        Notification.Information(Notification.enumMsg.SamePasswordError);
        return false;
    }
    else if (!passwordvalidation.test(newPassword)) {
        Notification.Information(Notification.enumMsg.PasswordFormatError);
        return false;
    }
    else if (!passwordvalidation.test(confirmPassword)) {
        Notification.Information(Notification.enumMsg.PasswordFormatError);
        return false;
    }
    else {
        var vParam = "{ oldPassword: '" + oldPassword + "', newPassword: '" + newPassword + "' }";

        ServiceBroker.callWebMethod(enumWebPage.Home, enumWebMethod.UpdatePassword, vParam, 'ChangePasswordSuccess', true, true);
        ResetPasswordChange();
    }
}

ChangePasswordSuccess = function (msg) {
    if (hasNoError(msg, true)) {
        if (msg[0] == 1) {
            $('#ChangepasswordDialog').dialog("close");
            Notification.Information(Notification.enumMsg.PasswordUpdateMessage);
        }
        else {
            Notification.Error(msg[0]);
        }
    }
    else {
        Notification.Error(Notification.enumMsg.UpdateErrorMsg);
    }
}

function SetNavigationForUser() {
    if (appUser.IsAdmin == "True") {
        $('#Admin').attr('style', 'display:block;');
    }
    else {
        $('#Admin').attr('style', 'display:none;');
        $('#AdminSubMenuItem').attr('style', 'display:none;');
    }

    if (appUser.IsReportUser == "True") {
        $('#Reports').attr('style', 'display:block;');
    }
    else{
        $('#Reports').attr('style', 'display:none;');
        $('#ReportsSubMenuItem').attr('style', 'display:none;');
    }

    if (appUser.LevelNumber == 1) {
        $('#Level2').attr('style', 'display:none;');
        $('#Level3').attr('style', 'display:none;');
    }
    else if (appUser.LevelNumber == 2) {
        $('#Level1').attr('style', 'display:none;');
        $('#Level3').attr('style', 'display:none;');
    }
    else if (appUser.LevelNumber == 3) {
        $('#Level1').attr('style', 'display:none;');
        $('#Level2').attr('style', 'display:none;');
    }
}

function validateBrowser() {
    if ($.browser.msie != null && $.browser.version < 7) {
        $('#page').hide();
        $('#advBrowser').show();
        return false;
    }
    // Step 1) Ajax Support Check
    var ajaxSupp;

    try {
        ajaxSupp = new XMLHttpRequest();    // Opera 8.0+, Firefox, Safari
    }
    catch (e) {
        try {
            ajaxSupp = new ActiveXObject("Msxml2.XMLHTTP"); // Internet Explorer Browsers
        }
        catch (e) {
            try {
                ajaxSupp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e) {
                $('#page').hide();
                $('#advBrowser').show();
                return false;
            }
        }
    }

    return true;
}

function handleInitialLoad() {
    if (document.location.hash != "") {
        SiteMasterVar.g_sPage = document.location.hash.replace(/^\//, '').replace('#', '').replace('#', '');
        SiteMasterVar.g_sMenuID = getMenuID(SiteMasterVar.g_sPage, "MenuID");
    }

    ServiceBroker.callPageLoad(SiteMasterVar.g_sPage, "asyncPageLoadResponse");
}

/*********************************************************
* Scripts Used for Async Page Load.
*********************************************************/
function asyncPageLoad() {
    $("#sidebarMainMenu").find('a').click(function (e) {
        e.preventDefault();
        $(this).append
        handleNavigation(this);
    });
}

function asyncPageLoadSavedSearch(sMenuID) {
    $('#' + sMenuID).find('a').click(function (e) {
        e.preventDefault();
        handleNavigation(this);
    });
}


/*********************************************************
* The following code is all navigation-related stuff
*********************************************************/
function handleNavigation(ctrlObj) {
    SiteMasterVar.g_sPage = $(ctrlObj).attr("href");

    SiteMasterVar.g_sMenuID = getMenuID(SiteMasterVar.g_sPage, "MenuID");
    $('#sidebarMainMenu').children().removeClass('menuItemCurrent');
    $(ctrlObj).parent().addClass('menuItemCurrent');
    document.location.hash = SiteMasterVar.g_sPage; //Fires hashchange event.
}

function asyncPageLoadResponse(sResHTML) {
    if (hasNoError(sResHTML, true)) {
        $('#contentPayload').html(sResHTML);
        setCurrentPageHighlighting();
    }
}

function getMenuID(sURL, sQueryStringName) {
    if (sURL.split('?')[1] != undefined) {
        hu = sURL.split('?')[1];
        gy = hu.split("&");
        for (i = 0; i < gy.length; i++) {
            ft = gy[i].split("=");
            if (ft[0] == sQueryStringName) {
                return ft[1];
            }
        }
    }
    return null;
}

function setNavHighlighting(sParentElem) {
    if (sParentElem == null)
        sParentElem = "#sidebar";

    $(sParentElem).find('.menuItem').live('mouseenter', function (event) {
        g_currentSelected = $(this).attr('id');
        $(this).addClass('menuItemHover');
    }).live('mouseleave', function (event) {
        g_previousSelected = $(this).attr('id');
        $(this).removeClass('menuItemHover');
    });
}

// find which nav element the current page pertains to and set a class to highlight it
function setCurrentPageHighlighting(sNavID, sNavTitle) {
    var sDashBoardMsg = "Hello, " + appUser.UserName + "!"; //UserNiceName

    // Navigation items -- elementID and associated page
    var arNavItems = new Array();
    arNavItems["home.aspx"] = { navid: "Home", msg: sDashBoardMsg };
    arNavItems["admin/masterdata.aspx"] = { navid: "MasterData", msg: "Master Data" };
    arNavItems["admin/usercreation.aspx"] = { navid: "UserCreation", msg: "User Updation" };
    arNavItems["admin/chartimport.aspx"] = { navid: "ChartImport", msg: "Chart Import" };
    arNavItems["admin/chartreallocation.aspx"] = { navid: "ChartReallocation", msg: "Chart Reallocation" };
    arNavItems["admin/chartbulkreallocation.aspx"] = { navid: "ChartBulkReallocation", msg: "Chart Bulk Reallocation" };
    arNavItems["taskallocation/level1.aspx"] = { navid: "Level1", msg: "Level 1" };
    arNavItems["taskallocation/level2.aspx"] = { navid: "Level2", msg: "Level 2" };
    arNavItems["taskallocation/level3.aspx"] = { navid: "Level3", msg: "Level 3" };
    arNavItems["reports/dailyproductionsummary.aspx"] = { navid: "DailyProductionSummary", msg: "Daily Production Summary" };
       
    arNavItems["reports/productionstatus.aspx"] = { navid: "ProductionStatus", msg: "Current Production Status" };
    
    arNavItems["reports/chartstatus.aspx"] = { navid: "ChartStatusReport", msg: "Chart Status" };
    arNavItems["reports/agingreport.aspx"] = { navid: "AgingReport", msg: "Aging Report" };
    arNavItems["reports/performancesummary.aspx"] = { navid: "PerformanceSummary", msg: "Performance Summary" };
    arNavItems["reports/invoice.aspx"] = { navid: "InvoiceReport", msg: "Invoice Summary" };
    arNavItems["admin/clientchartdeallocation.aspx"] = { navid: "ClientChartDeallocation", msg: "Chart Deallocation" };
    arNavItems["admin/clientconfiguration.aspx"] = { navid: "ClientConfiguration", msg: "Client Configuration" };
    arNavItems["admin/directupload.aspx"] = { navid: "DirectUpload", msg: "Direct Upload to Client" };
    arNavItems["admin/clientbulkdeallocate.aspx"] = { navid: "ClientChartBulkDeallocate", msg: "Client Chart Bulk Deallocation" };

    arNavItems["admin/usermapping.aspx"] = { navid: "UserMapping", msg: "User Mapping" };
    //arNavItems["reports/PerformanceSummary.aspx"] = { navid: "PerformanceSummary", msg: "PerformanceSummary.aspx" };

    var oPageNavItem = null;

    if (sNavID) {
        oPageNavItem = { navid: sNavID, msg: sNavTitle };
    }
    else {
        var docName = SiteMasterVar.g_sPage;
        var urlElems = docName.split('?');

        if (urlElems.length > 1) {
            var sMenuID = urlElems[1].split('=')[1];

            if (sMenuID != undefined) {
                docName = sMenuID;
                oPageNavItem = { navid: sMenuID, msg: $('#' + sMenuID).find('a').text() };
            }
        }
        else {
            // Set menu higlighting based on which page is currently displayed
            // Default: need to handle potential for docName to be blank, etc)..            
            oPageNavItem = arNavItems[docName.toLowerCase()];
        }
    }

    //if nav item is undefined/null/false then set nav item as home
    if (!oPageNavItem)
        oPageNavItem = arNavItems["home.aspx"];

    // Clear old highlights, if necessary
    $("#sidebar").find(".menuItemCurrent").removeClass("menuItemCurrent");

    if (oPageNavItem.navid == "UserCreation" || oPageNavItem.navid == "ChartImport" || oPageNavItem.navid == "ChartReallocation" || oPageNavItem.navid == "ChartBulkReallocation" || oPageNavItem.navid == "MasterData"
        || oPageNavItem.navid == "Level1" || oPageNavItem.navid == "DailyProductionSummary" || oPageNavItem.navid == "ProductionStatus" || oPageNavItem.navid == "ChartStatusReport"
        || oPageNavItem.navid == "ClientChartDeallocation" || oPageNavItem.navid == "Level3" || oPageNavItem.navid == "AgingReport" || oPageNavItem.navid == "PerformanceSummary"
        || oPageNavItem.navid == "InvoiceReport" || oPageNavItem.navid == "ClientConfiguration" || oPageNavItem.navid == "DirectUpload" || oPageNavItem.navid == "ClientChartBulkDeallocate" || oPageNavItem.navid == "UserMapping") {

        var oPage = "#" + oPageNavItem.navid;
        // highlights 'Results' navigation item
        $(oPage).addClass("menuItemCurrent");
    }

    // highlight appropriate navigation item
    // and update content header title
    $("#" + oPageNavItem.navid).addClass("menuItemCurrent");

    // if message is null - get it from inside the nav node
    var sMsg = oPageNavItem.msg;
    if (!sMsg)
        sMsg = $("#" + oPageNavItem.navid).find("a").text();

    updateContentTitle(sMsg);
}

//Update Content Title and Welcome Board
function updateContentTitle(sVal) {
    if (sVal) {
        $("#divWelcome").text(sVal);
        $(document).attr("title", SiteMasterVar.g_sTitlePrefix + sVal);
    }
}

function wireUpEvents() {
    $("#page").css('min-height', $(window).height() - $("#header-container").height() - 25);

    $("#content").css('min-height', $(window).height() - $("#header-container").height() - 25);

    $("#imgLogout").on("click", function () {
        var relObj = $("#Login");
        var pos = relObj.position();
        var height = relObj.height();
        var width = 125; //relObj.width();

        $("#Settingsmenu").css({
            "top": $("#header-container").height() + "px",
            "left": $('#content').position().left + $('#content').width() - width + "px", //$('#Login').width()
            "width": width + "px"
        })

        $("#Settingsmenu").slideToggle();
    });


    $("#imgQueue").on("click", function () {
        var relObj = $("#Login");
        var pos = relObj.position();
        var height = relObj.height();
        var width = 275; //relObj.width();

        $("#queueMenu").css({
            "top": $("#header-container").height() + "px",
            "left": $('#content').position().left + $('#content').width() - width - 150 + "px", //$('#Login').width()
            "width": width + "px"
        })

        $("#queueMenu").slideToggle();
    });

    //Hash change event.
    window.onhashchange = function () {
        debug('Hash change');
        SiteMasterVar.g_sPage = document.location.hash.replace(/^\//, '').replace('#', '').replace('#', '');
        SiteMasterVar.g_sMenuID = getMenuID(SiteMasterVar.g_sPage, "MenuID");

        if (SiteMasterVar.g_sPage == null || SiteMasterVar.g_sPage == "") {
            SiteMasterVar.g_sPage = "Home.aspx";
        }
        $('#divWelcome').parent().show();
        ServiceBroker.callPageLoad(SiteMasterVar.g_sPage, "asyncPageLoadResponse");
    };
}