/* Global Variables/Enum */

var ServiceBroker = {};

var g_sBadResponse = "Error while communicating with server.";
var g_sInternetNA = "Oops..Internet seems to be disconnected.";
var g_sError = "Error";

var enumWebPage = { "Default": "Site.master",
    "Home": "Home.aspx",
    "EMP": "Setup/Emp.aspx",
    "Level1": "TaskAllocation/Level1.aspx",
    "Level2": "TaskAllocation/Level2.aspx",
    "Level3": "TaskAllocation/Level3.aspx",
    "UserCreation": "Admin/UserCreation.aspx",
    "ChartReallocation": "Admin/ChartReallocation.aspx",
    "ProductionStatus": "Reports/ProductionStatus.aspx",
    "ChartStatus": "Reports/ChartStatus.aspx",
    "ClientChartDeallocation": "Admin/ClientChartDeallocation.aspx",
    "ChartImport": "Admin/ChartImport.aspx",
    "MasterData": "Admin/MasterData.aspx",
    "ClientConfiguration": "Admin/ClientConfiguration.aspx",
    "Invoice": "Reports/Invoice.aspx",
    "PerformanceSummary": "Reports/PerformanceSummary.aspx",
    "AgingReport": "Reports/AgingReport.aspx",
    "DirectUpload": "Admin/DirectUpload.aspx",
    "ChartbulkDellaocate": "Admin/ChartbulkDellaocate.aspx",
    "UserMapping": "Admin/UserMapping.aspx",
    "ChartBulkReallocation":"Admin/ChartBulkReallocation.aspx",
    "Login":"Account/Login.aspx"
    //"PerformanceSummary": "Reports/PerformanceSummary.aspx"
};

var enumWebMethod = {
    "GetMenus": "GetMenus",
    "GetSubMenus": "GetSubMenus",
    "getLinkData": "getLinkData",
    "IsUserAuthorized": "IsUserAuthorized",
    "PasswordExpired": "PasswordExpired",
    "UpdatePassword": "UpdatePassword",
    "GetHomePageText": "GetHomePageText",

    "GetL1PendingChartData": "GetL1PendingChartData",
    "GetL1ChartStatuses": "GetL1ChartStatuses",
    "GetL1StatusComments": "GetL1StatusComments",
    "SaveL1DataChartInfo": "SaveL1DataChartInfo",
    "RequestL1Chart": "RequestL1Chart",
    "GetL1OtherChartData": "GetL1OtherChartData",
    "RequestL1ChartByClientReference": "RequestL1ChartByClientReference",
    "L1PendingChartCount": "L1PendingChartCount",

    "getEmployeeData": "getEmployeeData",
    "getClientProjectData": "getClientProjectData",
    "getLocationtData": "getLocationtData",
    "UpdateEmployeeinfo": "UpdateEmployeeinfo",
    "ResetPassword": "ResetPassword",
    "UnlockAccount": "UnlockAccount",

    "GetErrorCategories": "GetErrorCategories",
    "GetErrorSubCategories": "GetErrorSubCategories",
    "GetErrorDescriptions": "GetErrorDescriptions",
    "SaveL2AuditComments": "SaveL2AuditComments",

    "getWIPChartData": "getWIPChartData",
    "getWIPClientChartData": "getWIPClientChartData",
    "commentCategoryId": "commentCategoryId",
    "getEmployeeData": "getEmployeeData",
    "AssignChartWithEmployee": "AssignChartWithEmployee",
    "getDailyProductionSummary": "getDailyProductionSummary",

    "GetL2PendingChartData": "GetL2PendingChartData",
    "GetL2ChartStatuses": "GetL2ChartStatuses",
    "GetL2StatusComments": "GetL2StatusComments",
    "SaveL2DataChartInfo": "SaveL2DataChartInfo",
    "RequestL2Chart": "RequestL2Chart",
    "GetL2OtherChartData": "GetL2OtherChartData",
    "RequestL2ChartByClientReference": "RequestL2ChartByClientReference",
    "GetChartMoreInfoByChartId": "GetChartMoreInfoByChartId",
    "L2PendingChartCount": "L2PendingChartCount",

    "RequestL3Chart": "RequestL3Chart",
    "GetL3PendingChartData": "GetL3PendingChartData",
    "GetL3ChartStatuses": "GetL3ChartStatuses",
    "SaveL3DataChartInfo": "SaveL3DataChartInfo",
    "GetL3StatusComments": "GetL3StatusComments",
    "GetL3ErrorCategories": "GetL3ErrorCategories",
    "GetL3ErrorSubCategories": "GetL3ErrorSubCategories",
    "GetL3ErrorDescriptions": "GetL3ErrorDescriptions",
    "SaveL3AuditComments": "SaveL3AuditComments",
    "GetL3OtherChartData": "GetL3OtherChartData",
    "GetChartAuditLogsByChartId": "GetChartAuditLogsByChartId",
    "GetLevelDisagreeOptions": "GetLevelDisagreeOptions",
    "SaveChartAuditList": "SaveChartAuditList",
    "RequestL3ChartByClientReference": "RequestL3ChartByClientReference",
    "GetL1andL2ChartMoreInfoByChartId": "GetL1andL2ChartMoreInfoByChartId",
    "L3PendingChartCount": "L3PendingChartCount",

    "GetProductionStatisticsData": "GetProductionStatisticsData",
    "getChartStatuses": "getChartStatuses",
    "GetChartInfoByOverallStatus": "GetChartInfoByOverallStatus",
    "GetChartProductionByChartMoreInfoId": "GetChartProductionByChartMoreInfoId",
    "deleteChartList": "deleteChartList",
    "GetChartAuditLogsByChartId": "GetChartAuditLogsByChartId",
    "GetQueue": "GetQueue",

    "GetLookupDataCategories": "GetLookupDataCategories",
    "GetLookupDatum": "GetLookupDatum",
    "UpdateLookupData": "UpdateLookupData",
    "InsertLookupData": "InsertLookupData",
    "GetCommentsCategories": "GetCommentsCategories",
    "GetCommentsByCategoryTable": "GetCommentsByCategoryTable",
    "InsertUpdateComments": "InsertUpdateComments",
    "InsertUpdateQueue": "InsertUpdateQueue",

    "GetChartInfoForInvoiceReport": "GetChartInfoForInvoiceReport",
    "GetPerformanceSummaryReport": "GetPerformanceSummaryReport",
    "getEmployeeInfo": "getEmployeeInfo",
    "GetAgingSummaryReport": "GetAgingSummaryReport",
    "GetAgingReportForCompleted": "GetAgingReportForCompleted",
    "GetAgingReportForIncomplete": "GetAgingReportForIncomplete",

    "GetClientConfiguration": "GetClientConfiguration",
    "UpdateClientProjectInfo": "UpdateClientProjectInfo",
    "GetChartsForDirectUpload": "GetChartsForDirectUpload",
    "DirectUploadCharts": "DirectUploadCharts",
    "SaveL1DataCommandStatusInfo": "SaveL1DataCommandStatusInfo",
    "GetL1BindCmdStatus": "GetL1BindCmdStatus",
    "SaveL2DataChartInfoUpdate": "SaveL2DataChartInfoUpdate",
    "GetChartMoreInfoByChartId1":"GetChartMoreInfoByChartId1",
    "GetChartAuditComments":"GetChartAuditComments",
    "GetL2ChartAuditData":"GetL2ChartAuditData",
    "GetErrorDescriptionsUpdate": "GetErrorDescriptionsUpdate",
    "SaveL2AuditUpdateComments": "SaveL2AuditUpdateComments",
    "GetErrorCategoriesUpdate": "GetErrorCategoriesUpdate",
    "GetErrorSubCategoriesUpdate":"GetErrorSubCategoriesUpdate",
    "DeleteCommentsData":"DeleteCommentsData",
    "DeleteChartAuditUpdateComments":"DeleteChartAuditUpdateComments",
    "GetCompletedChartCount": "GetCompletedChartCount",
    "L1CompletedChartCount":"L1CompletedChartCount",
    "getAllUser":"getAllUser",
    "GetUserDetails":"GetUserDetails",
    "GetActiveProjectList":"GetActiveProjectList",
    "getFromStatusInfo":"getFromStatusInfo",
    "getCharttoStatuses":"getCharttoStatuses",
    "LoadChartData":"LoadChartData"
};

/* Function to Check if the user is online */
/* navigator.onLine checks only if the user is available in the network but does not check the internet connection 
need  to evaluate a mechanism to check internet connection.*/
ServiceBroker.validateInternetStatus = function () {
    return navigator.onLine;
}

/* Function to create error jon object.*/
ServiceBroker.formatErrorJSON = function (sResMessage) {
    var jsonObjRes = { "Error": sResMessage };
    return jsonObjRes;
}

/* Function used for calling "call back function". */
ServiceBroker.processCallBackFunction = function (sCallBackFunction, sResMessage) {
    //    var sCallBackFun = window[sCallBackFunction];
    //    sCallBackFun(sResMessage);
    eval(sCallBackFunction)(sResMessage);
}


/*
Generic function to call service request it accepts the following parameters.

1) enumWebPage :- The Name of the page.
2) enumWebMethod :- The name of the web method in the above mentioned page.
3) sWebMethodParams :- Proper formated json parameters string for the above selected method.
4) sCallBackMethod :- Call back function to which the response would be passed.
5) bIsJSONParse :- boolean to check if JSON parsing the response is required before calling callback method

If the function is able to connected to server the JSON response would be sent. 
In case of any error either in client side or server side a generic error message is passed to the call back function.
*/

ServiceBroker.callWebMethod = function (enumWebPage, enumWebMethod, sWebMethodParams, sCallBackMethod, bIsJSONParse, bDontShowLoadingImg) {
    // If argument is not supplied or is set to false
    if (!bDontShowLoadingImg)
        $('#divProcessImage').show();

    //Check if the User is online. If not return empty.
    if (!ServiceBroker.validateInternetStatus()) {
        ServiceBroker.processCallBackFunction(sCallBackMethod, ServiceBroker.formatErrorJSON(g_sInternetNA));
        if (!bDontShowLoadingImg)
            $('#divProcessImage').hide();

        return;
    }

    try {

        if (enumWebPage == null || enumWebMethod == null) {
            ServiceBroker.processCallBackFunction(sCallBackMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
            if (!bDontShowLoadingImg)
                $('#divProcessImage').hide();

            return;
        }
        debug("Request sent " + Date.now());
        jQuery.ajax({
            type: "POST",
            url: enumWebPage + "/" + enumWebMethod,
            data: sWebMethodParams,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (sJsonDataResponse) {
                debug("JSON Received - " + sJsonDataResponse.d);
                if (bIsJSONParse)
                    ServiceBroker.processCallBackFunction(sCallBackMethod, jQuery.parseJSON(sJsonDataResponse.d));
                else
                    ServiceBroker.processCallBackFunction(sCallBackMethod, sJsonDataResponse.d);

                debug("response received" + Date.now());

                if (!bDontShowLoadingImg)
                    $('#divProcessImage').hide();

                return false;
            },
            error: function (jqXHR, sExpMsg) {
                if (sExpMsg != 'error') {
                    ServiceBroker.processCallBackFunction(sCallBackMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
                }

                if (!bDontShowLoadingImg)
                    $('#divProcessImage').hide();
            }
        });

    } catch (e) {
        debug("Error - " + e);
        ServiceBroker.processCallBackFunction(sCallBackMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
        if (!bDontShowLoadingImg)
            $('#divProcessImage').hide();

        return false;
    }
}


ServiceBroker.callPageLoad = function (enumWebPage, sCallBackMethod) {
    //Check if the User is online. If not return empty.
    if (!ServiceBroker.validateInternetStatus()) {
        ServiceBroker.processCallBackFunction(sCallBackMethod, ServiceBroker.formatErrorJSON(g_sInternetNA));
        return;
    }

    //Check if data available in Cache
    if (CacheManager.exist(enumWebPage)) {
        var sCachedHtml = CacheManager.get(enumWebPage);
        ServiceBroker.processCallBackFunction(sCallBackMethod, sCachedHtml);
        return;
    }

    //    var sQS = enumWebPage.split('?')[1];
    //    if (sQS != undefined) {
    //        alert('&clearcache');
    //        enumWebPage = enumWebPage + "&clearcache=" + new Date().getTime().toString();
    //    }
    //    else {
    //        alert('?clearcache');
    //        enumWebPage = enumWebPage + "?clearcache=" + new Date().getTime().toString();
    //    }

    try {
        if (enumWebPage == null) {
            ServiceBroker.processCallBackFunction(sCallBackMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
            return;
        }

        jQuery.ajax({
            type: "get",
            url: enumWebPage,
            contentType: "text/html;",
            dataType: "html",
            success: function (sHTMLData) {
                //Add to cache
                CacheManager.add(enumWebPage, sHTMLData);

                //debug("HTML Data Received - " + sHTMLData);                
                ServiceBroker.processCallBackFunction(sCallBackMethod, sHTMLData);
                return false;
            },
            error: function (msg) {
                ServiceBroker.processCallBackFunction(sCallBackMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
                return false;
            }
        });

    } catch (e) {
        debug("Error - " + e);
        return false;
    }
}


