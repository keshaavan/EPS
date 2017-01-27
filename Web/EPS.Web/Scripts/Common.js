
/*********************************************************
* General utilities
*********************************************************/

// A debug logging function
var bDEBUG = false;

//DBNull value converted to 'N/A' on server side
var c_sUnavailable = 'N/A';
var c_iMillion = 1000000;
var c_sHyphen = "-";
var c_sStickyNoteClosed = 'snclosed';


// console not available in all browsers
var console = console || { "log": function () { } };

function debug(sMessage) {
    if (bDEBUG)
        alert(sMessage);
    else
        console.log(sMessage);
}

var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
var dateFormats = { "MMddyyyy": "MMddyyyy", "yyyy": "yyyy", "MMMdd": "MMMdd", "MMddyy": "MMddyy", "MMMddtt": "MMMddtt" }; //currently used date formats for LoanStar application

function formatPercent(nVal, iNumDecimalDigits) {

    if (nVal == c_sUnavailable ) {
        return c_sUnavailable;
    }

    if (iNumDecimalDigits == null)
        iNumDecimalDigits = 1;

    return Number(nVal).formatPercent(iNumDecimalDigits);
}

function formatAppendPercent(nVal, iDecimal) {

    
    if (nVal == c_sUnavailable) {
        return c_sUnavailable;
    }

    if (iDecimal == null)
        iDecimal = 1;

    return Number(nVal).toFixed(iDecimal) + "%";
}

Number.prototype.formatPercent = function (c) {

    if (c == null || n == c_sUnavailable)
        c = 2;

    var n = this;

    if (n == c_sUnavailable || isNaN(n) || n == null)
        return c_sUnavailable;

    var i = n * 100;

    if (i.toFixed(c) == 0) {
        return c_sHyphen;
    }

    return i.toFixed(c) + "%";
};


function formatString(sVal) { return sVal; }

function formatAddress(sVal) { return sVal; }

/*********************************************
* Formatter for Dates using formats in dateFormats collection object.
*********************************************/
function formatDate(sVal, format) {
    if (sVal == null || sVal == '' || sVal == c_sUnavailable)
        return c_sUnavailable;

    var dDate = new Date(sVal);
    var nMonth = dDate.getMonth() + 1;
    var nDate = dDate.getDate();
    var nYear = dDate.getFullYear();
    var nHours = dDate.getHours();
    var nMinutes = dDate.getMinutes();

    var sDate = "";

    switch (format) {
        case dateFormats.yyyy:
            sDate = nYear.toString();
            break;
        case dateFormats.MMMdd:
            sDate = monthNames[nMonth - 1] + " " + nDate.toString();
            break;
        case dateFormats.MMddyyyy:
            sDate = nMonth.toString() + "/" + nDate.toString() + "/" + nYear.toString();
            break;
        case dateFormats.MMddyy:
            var sMonth = (nMonth > 9) ? nMonth.toString() : '0' + nMonth.toString();
            var sDate = (nDate > 9) ? nDate.toString() : '0' + nDate.toString();
            var sYear = nYear.toString().substr(nYear.toString().length - 2, nYear.toString().length);
            sDate = sMonth + '/' + sDate + '/' + sYear;
            break;
        case dateFormats.MMMddtt:
            var sHours = (nHours <= 12) ? nHours.toString() : (nHours - 12).toString();
            var sMinutes = nMinutes.toString();
            sDate += formatDate(dDate, dateFormats.MMMdd).toString();
            sDate += " at ";
            sDate += (nHours < 10) ? "0" + nHours.toString() : nHours.toString();
            sDate += ":"
            sDate += (nMinutes < 10) ? "0" + nMinutes.toString() : nMinutes.toString();
            sDate += (nHours < 12) ? "am" : "pm";
            break;
        default:
            var sMonth = (nMonth > 9) ? nMonth.toString() : '0' + nMonth.toString();
            var sDate = (nDate > 9) ? nDate.toString() : '0' + nDate.toString();
            var sYear = nYear;//.toString().substr(nYear.toString().length - 2, nYear.toString().length);
            sDate = sMonth + '/' + sDate + '/' + sYear;
            break;
    }
    return sDate;
}

function formatToDecimal(sVal, iDigits) {
    if (sVal == null || sVal == '' || sVal == c_sUnavailable) {
        return c_sUnavailable;
    }

    if (Number(sVal).toFixed(iDigits) == 0)
        return c_sHyphen;

    if (iDigits == null || iDigits == 0 || iDigits == '' || iDigits == undefined) {
        return Math.round(sVal);
    }

    return Math.round(sVal * Math.pow(10, iDigits)) / Math.pow(10, iDigits);
}

function formatMoney(sVal) {
    if (sVal == c_sUnavailable) {
        return sVal;
    }

    if (sVal == 0) {
        return c_sHyphen;
    }

    return Number(sVal).formatMoney(0);
}

// A formatter for numbers
// c = number of decimal places
Number.prototype.formatMoney = function (c) {
    var n = this;
    c = isNaN(c = Math.abs(c)) ? 2 : c;
    var d = "."; // d == undefined ? "," : d;
    var t = ",";  // = t == undefined ? "." : t;
    var s = n < 0 ? "-" : "";
    var i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "";
    var j = (j = i.length) > 3 ? j % 3 : 0;

    return "$" + s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

Number.prototype.formatToMillion = function (c) {
    var n = this;
    if (n == null || n == '' || n == c_sUnavailable || n == 0) {
        return 0;
    }

    c = isNaN(c = Math.abs(c)) ? 2 : c;

    if ((n / c_iMillion).toFixed(c) == 0)
        return c_sHyphen;

    return Number((n / c_iMillion).toFixed(c));
};

// Formatter for String type value in millions.
function formatMoneyInMillion(sVal, iNumDecimalDigits) {

    if (sVal == c_sUnavailable || sVal == null) {
        return c_sHyphen
    }

    return Number(sVal).formatMoneyInMillion(iNumDecimalDigits);
}

// Formatter for Numeric type value in millions */
Number.prototype.formatMoneyInMillion = function (c) {
    var n = this;

    if (n == null || isNaN(n)) {
        return c_sHyphen;
    }
    var val = n / c_iMillion;
    //    c = isNaN(c = Math.abs(c)) ? 2 : c;
    //    var d = ".";
    //    var t = ",";
    //    var s = n < 0 ? "-" : "";
    //    var i = (parseInt(n = Math.abs(+n || 0).toFixed(c)) / c_iMillion) + "";
    //    var j = (j = i.length) > 3 ? j % 3 : 0;

    return val.formatMoney(c) + "mm";
}

// A formatter for weird (namely MSFT) dates
function parseDateString(sDate) {
    var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
    var reMsAjax = /^\/Date\((d|-|.*)\)\/$/;

    var a = reISO.exec(sDate);
    if (a)
        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));

    a = reMsAjax.exec(sDate);
    if (a) {
        var b = a[1].split(/[+\-,.]/);
        return new Date(+b[0]);
    }

    return null;
}


/*********************************************
* Other Util functions.
*********************************************/

// Draws a color scale representing iVal, if the scale range is [iVal, iHigh]
function formatScale(iVal, iLow, iHigh) {
    var sHTML = "";
    var iRed = 20;
    var iGreen = 255;
    var iNumSteps = (iHigh - iLow) / 2; // one for Red and one for Green
    var iColorInc = Math.floor(Math.abs((iGreen - iRed) / iNumSteps)); // have to do floor or could "overflow" colors
    var sColor;

    // "normalizes" the two colors
    iGreen = iGreen - (iGreen - iRed) % iColorInc;

    for (var i = iLow; i <= iHigh; i++) {

        var sRtgClass = "divRatingScale";
        if (i == iVal) {
            sRtgClass += " divRatingScaleSelected";
        }

        //        if (i <= iVal) {
        sColor = "rgb(" + iRed + "," + iGreen + ",0)"

        // Does color gradient... first increment Red, then decrement Green
        if (iRed <= (iGreen - iColorInc))
            iRed += iColorInc;
        else
            iGreen -= iColorInc;
        //        }
        //        else {
        //            sColor = "White";
        //        }

        sHTML += "<div class='" + sRtgClass + "' style='background-color:" + sColor + "'></div>";
    }

    sHTML += "<div class='divRatingAnnotation'>&nbsp;" + iVal + " / " + iHigh + "</div>";
    return sHTML;
}


// Takes the value and appends the noun, accounting for singular/plural
function formatValueAppendNoun(iVal, sNounSingularForm) {
    var sNoun = sNounSingularForm;

    if (iVal != 1)
        sNoun = sNoun + "s";

    return iVal + " " + sNoun;
}

// Returns whether the PARSED JSON object has errors
function hasNoError(sParsedJSON, bShowError) {

    if (sParsedJSON.Error == undefined) {
        return true;
    }
    else {

        if (bShowError) {
            Notification.Error(sParsedJSON.Error);
        }

        return false;
    }
}

// Get value corresponding to supplied querystring parameter
function querySt(paramName) {
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == paramName) {
            return ft[1];
        }
    }
}

// Gets current protocol 
function protocol() {
    return window.location.protocol;
}

//Function to indicate, if the current protocol is https 
function isHttps() {
    return protocol().toLowerCase() == "https:" ? true : false;
}

function getTextAreaRowCount(sElemValue, textLengthPerLine, minTextAreaHeight, maxTextAreaHeight) {
    var rowLength = sElemValue.split('&#10;').length;
    if (rowLength == null || rowLength == 1) {
        if (sElemValue != null && sElemValue.length > 0)
            return Math.ceil(sElemValue.length / textLengthPerLine) + 1; //return ((sElemValue.length / textLengthPerLine) + 1 > maxTextAreaHeight) ? maxTextAreaHeight : (sElemValue.length / textLengthPerLine) + 1;

        return minTextAreaHeight;
    }
    if (rowLength > maxTextAreaHeight)
        return maxTextAreaHeight;

    return rowLength;
}

function htmlEncode(sInput) {
    Encoder.EncodeType = "numerical";
    return Encoder.htmlEncode(sInput);
}

function autoExpand(txtBox, event, minRowCount, maxRows) {
    var therows = 0
    var thetext = document.getElementById(txtBox.id).value;
    var newtext = thetext.split("\n");

    therows += newtext.length
    if (therows > maxRows)
        therows = maxRows;

    if (therows < minRowCount)
        therows = minRowCount;

    document.getElementById(txtBox.id).rows = therows;

    return false;
}

/*********************************************
* Functions for Sticky notes.
*********************************************/

var StickyNotes = {};

// Show all notes given the current page number
StickyNotes.showNotes = function (sPageName, sTab) {
    $.each(StickyNotes.enumStickyNotesMsg, function (key, value) {
        var oNoteObj = value;

        if (oNoteObj.pagename == sPageName) {
            if (!sTab || sTab == oNoteObj.tab){
                StickyNotes.show(oNoteObj.id, oNoteObj.headerText, oNoteObj.bodyText, oNoteObj.relativeElement, oNoteObj.adjx, oNoteObj.adjy);
            } 
        }
    });
};


StickyNotes.enumStickyNotesMsg = {
    defaultSN1: { id: "SN1", pagename: "Default.aspx", headerText: "",
        bodyText: "<b>Want details on individual loans?</b> Click on <u>Loans</u> on the left menu.", relativeElement: "MyLoans", adjx: -100, adjy: 0
    },
    defaultSN2: { id: "SN2", pagename: "Default.aspx", headerText: "",
        bodyText: "Slice and dice your portfolio by clicking on <u>Filter Loans</u>.", relativeElement: "Filter", adjx: -100, adjy: 0
    },
    defaultSN3: { id: "SN3", pagename: "Default.aspx", headerText: "",
        bodyText: "Clicking on the <u>LoanStar Insight</u> (or any table title) can hide/unhide the detail.", relativeElement: "divLoanStarInsightHead", adjx: -330, adjy: -5
    },
    defaultSN4: { id: "SN4", pagename: "Default.aspx", headerText: "",
        bodyText: "Give us new ideas or report bugs by clicking the <u>Feedback tab</u>.", relativeElement: ""
    },
    loansSN1: { id: "SN1", pagename: "Loans.aspx", headerText: "",
        bodyText: "<b>Want to see loan terms?</b> Hover over any loan, and click <u>Inspect</u>.", relativeElement: "divSearchMetaInfo", adjy: 500, adjx: -250
    },
    loansSN2: { id: "SN2", pagename: "Loans.aspx", headerText: "",
        bodyText: "Zero in on the exact loan(s) you're looking for using the <u>Search Bar</u> on the top right, or clicking <u>Show Filters</u> below the search bar.",
        relativeElement: "inputSearch", adjx: 50, adjy: 30
    },
    filterSN1: { id: "SN1", pagename: "Filter.aspx", headerText: "",
        bodyText: "Click the: Details tab for more granular info; Chart tab for single-click reporting.", relativeElement: "Dashboard", tab:"portfolio", adjx: 300
    },
    filterSN2: { id: "SN2", pagename: "Filter.aspx", headerText: "",
        bodyText: "View by Risk Rating or Loan Officer by selecting the relevant field in the <u>By</u> drop-down.", relativeElement: "Dashboard", tab: "details", adjx: 750
    }
};



// Create a sticky note from an empty DIV
// Arguments are:
// sElemID: element ID (string) to use to make the sticky
// sTitle: title string to use
// sContent: content string to use
// sRelativeElem: ID of element to use for placement
StickyNotes.show = function (sElemID, sTitle, sContent, sRelativeElem, iAdjustmentX, iAdjustmentY) {
    var oNote = $("#" + sElemID);

    //Get first 10 chars + length of text from content as unique key.
    var sCookieName = sContent.length >= 10 ? $.trim(sContent.substr(0, 10)) : $.trim(sContent);
    sCookieName = sCookieName.replace(/'/g, '').replace(/:/, '') + sContent.length;

    if ($.cookie(sCookieName) != null) {
        return;
    }

    // Build the Sticky
    if (!oNote.hasClass("Sticky"))
        oNote.addClass("Sticky");

    oNote.html("<a>" +
            "<label>" +
            "<font class='StickyClose' title='permanently remove'>X</font>" +
            "<span>" +
            (sTitle == "" ? "" : "<b>" + sTitle + "</b><br>") +
            sContent +
            "</span>" +
            "</label>" +
            "</a>");

    oNote.find(".StickyClose").on("click", function () {
        oNote.hide();
        oNote.html("");
        var bIsSecure = isHttps();
        $.cookie(sCookieName, c_sStickyNoteClosed, { expires: 5000, path: '/', secure: bIsSecure });
    });

    // And place it
    if (sRelativeElem != "" && sRelativeElem != null) {
        positionByElemt(oNote, $("#" + sRelativeElem), iAdjustmentX, iAdjustmentY);
    }
    else {
        positionByParam(oNote, iAdjustmentX, iAdjustmentY);
    }

    oNote.show();
}

function positionByElemt(oNote, oRelObj, iAdjustmentX, iAdjustmentY) {
    if (!iAdjustmentX)
        iAdjustmentX = 0;
    if (!iAdjustmentY)
        iAdjustmentY = 0;

    var r = { x: oRelObj.offset().left, y: oRelObj.offset().top, width: oRelObj.innerWidth(), height: oRelObj.innerHeight() };
    oNote.css("position", "absolute");
    oNote.css("left", r.x + Math.floor(r.width / 2) + iAdjustmentX);
    oNote.css("top", r.y + Math.floor(r.height / 2) - oNote.innerHeight() + iAdjustmentY);
}

function positionByParam(oNote, iAdjustmentX, iAdjustmentY) {

    oNote.css("position", "absolute");
    oNote.css("left", iAdjustmentX);
    oNote.css("top", iAdjustmentY);
}