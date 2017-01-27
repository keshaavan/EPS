/* Global Variables/Enum */

var ServiceBroker = {};

var g_sBadResponse = "Error while communicating with server.";
var g_sInternetNA = "Oops..Internet seems to be disconnected.";
var g_sError = "Error";

var enumWebLoginPage = { 
    "Login":"Login.aspx"
};

var enumWebLoginMethod = {
   "getClientProjectList":"getClientProjectList"
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

1) enumWebLoginPage :- The Name of the page.
2) enumWebMethod :- The name of the web method in the above mentioned page.
3) sWebMethodParams :- Proper formated json parameters string for the above selected method.
4) enumWebLoginMethod :- Call back function to which the response would be passed.
5) bIsJSONParseLogin :- boolean to check if JSON parsing the response is required before calling callback method

If the function is able to connected to server the JSON response would be sent. 
In case of any error either in client side or server side a generic error message is passed to the call back function.
*/

ServiceBroker.callWebMethod = function (enumWebLoginPage, enumWebLoginMethod, sWebMethodLoginParams, sCallBackLoginMethod, bIsJSONParseLogin, bDontShowLoadingImg) {
   // alert(sWebMethodLoginParams);
    // If argument is not supplied or is set to false
    if (!bDontShowLoadingImg)
        $('#divProcessImage').show();

    //Check if the User is online. If not return empty.
    if (!ServiceBroker.validateInternetStatus()) {
        ServiceBroker.processCallBackFunction(enumWebLoginMethod, ServiceBroker.formatErrorJSON(g_sInternetNA));
        if (!bDontShowLoadingImg)
            $('#divProcessImage').hide();

        return;
    }

    try {

        if (enumWebLoginPage == null || enumWebLoginMethod == null) {
            ServiceBroker.processCallBackFunction(enumWebLoginMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
            if (!bDontShowLoadingImg)
                $('#divProcessImage').hide();

            return;
        }
        //debug("Request sent " + Date.now());
        jQuery.ajax({
            type: "POST",
            url: enumWebLoginPage + "/" + enumWebLoginMethod,
            data: sWebMethodParams,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (sJsonDataResponse) {
                //  debug("JSON Received - " + sJsonDataResponse.d);
                if (bIsJSONParseLogin)
                    ServiceBroker.processCallBackFunction(enumWebLoginMethod, jQuery.parseJSON(sJsonDataResponse.d));
                else
                    ServiceBroker.processCallBackFunction(enumWebLoginMethod, sJsonDataResponse.d);

                //  debug("response received" + Date.now());

                if (!bDontShowLoadingImg)
                    $('#divProcessImage').hide();

                return false;
            },
            error: function (jqXHR, sExpMsg) {
                if (sExpMsg != 'error') {
                    ServiceBroker.processCallBackFunction(enumWebLoginMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
                }

                if (!bDontShowLoadingImg)
                    $('#divProcessImage').hide();
            }
        });

    } catch (e) {
        //debug("Error - " + e);
        ServiceBroker.processCallBackFunction(enumWebLoginMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
        if (!bDontShowLoadingImg)
            $('#divProcessImage').hide();

        return false;
    }
}


ServiceBroker.callPageLoad = function (enumWebLoginPage, enumWebLoginMethod) {
    //Check if the User is online. If not return empty.
    if (!ServiceBroker.validateInternetStatus()) {
        ServiceBroker.processCallBackFunction(enumWebLoginMethod, ServiceBroker.formatErrorJSON(g_sInternetNA));
        return;
    }

    //Check if data available in Cache
    if (CacheManager.exist(enumWebLoginPage)) {
        var sCachedHtml = CacheManager.get(enumWebLoginPage);
        ServiceBroker.processCallBackFunction(enumWebLoginMethod, sCachedHtml);
        return;
    }

    //    var sQS = enumWebLoginPage.split('?')[1];
    //    if (sQS != undefined) {
    //        alert('&clearcache');
    //        enumWebLoginPage = enumWebLoginPage + "&clearcache=" + new Date().getTime().toString();
    //    }
    //    else {
    //        alert('?clearcache');
    //        enumWebLoginPage = enumWebLoginPage + "?clearcache=" + new Date().getTime().toString();
    //    }

    try {
        if (enumWebLoginPage == null) {
            ServiceBroker.processCallBackFunction(enumWebLoginMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
            return;
        }

        jQuery.ajax({
            type: "get",
            url: enumWebLoginPage,
            contentType: "text/html;",
            dataType: "html",
            success: function (sHTMLData) {
                //Add to cache
                CacheManager.add(enumWebLoginPage, sHTMLData);

                //debug("HTML Data Received - " + sHTMLData);                
                ServiceBroker.processCallBackFunction(enumWebLoginMethod, sHTMLData);
                return false;
            },
            error: function (msg) {
                ServiceBroker.processCallBackFunction(enumWebLoginMethod, ServiceBroker.formatErrorJSON(g_sBadResponse));
                return false;
            }
        });

    } catch (e) {
     //   debug("Error - " + e);
        return false;
    }
}


