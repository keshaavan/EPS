var Home = {};
Home.initialise = function () {

    $("#btnViewPerformanceSummary").click(function () {
        var sfromDate = $('#txtFromdate').val();
        var stoDate = $('#txtTodate').val();
        var iParam = "{ sfromDate: '" + $('#txtFromdate').val() + "', stoDate: '" + $('#txtTodate').val() + "'}";
        ServiceBroker.callWebMethod(enumWebPage.Home, enumWebMethod.GetCompletedChartCount, iParam, 'Home.BindCountValue', true, true);
    });


    var uParam = "{ userName: '" + appUser.UserName + "' }";
    ServiceBroker.callWebMethod(enumWebPage.Home, enumWebMethod.IsUserAuthorized, uParam, 'Home.isAuthorised', true, true);

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

}

Home.isAuthorised = function (msg) {
    if (msg == "ChangePassword") {
        openChangePasswordDialog();
        Home.LoadCompletedChartCount();
        Home.LoadHomepageText();
    }
    else if (msg) {
        Home.LoadCompletedChartCount();
        Home.LoadHomepageText();
    }
    else {
        window.location.href = "Logout.aspx";
    }
}





Home.BindCountValue = function (msg) {
    if (hasNoError(msg, true)) {
        $('#hdn1').val(msg);
    }
    Home.LoadHomepageText();
}


Home.LoadCompletedChartCount = function (msg) {
    var sfromDate = $('#txtFromdate').val();
    var stoDate = $('#txtTodate').val();
    var iParam = "{ sfromDate: '" + $('#txtFromdate').val() + "', stoDate: '" + $('#txtTodate').val() + "'}";
    ServiceBroker.callWebMethod(enumWebPage.Home, enumWebMethod.GetCompletedChartCount, iParam, 'Home.BindCountValue', true, true);
}

Home.LoadHomepageText = function (msg) {
    var iParam = "{ }";
    ServiceBroker.callWebMethod(enumWebPage.Home, enumWebMethod.GetHomePageText, iParam, 'Home.BindText', true, true);
}

Home.BindText = function (msg) {
    if (hasNoError(msg, true)) {
        var result = unescape(msg[0]);
        var ival = $('#hdn1').val();
        var resultfact = msg[1] * ival;
        if (resultfact != null && resultfact != 0) {
            $('#dvCompletedCount').show();
            result += "<br/><br/> <b><h2> Welcome " + appUser.UserName + "  Your Factorialization is : " + resultfact.toFixed(2) + "</h2></b>";
        }
        $('#homepageText').html(result);
    }
}