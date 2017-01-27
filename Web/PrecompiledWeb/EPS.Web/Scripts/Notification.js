/***************************************
    Variables for notification.
**************************************/
var Notification = {};

Notification.enumMsg = { 
    NoSpecialCharactersMessage: "No special characters allowed",
    OnlyNumberMessage: "Only numebers are allowed",
    InsertMessage: "Inserted Successfully",
    UpdateMessage: "Updated Successfully",
    PasswordUpdateMessage: "Your password has been updated successfully",
    PasswordResetMessage: "The password has been reset successfully",
    AccountUnlockedMessage: "The account has been unlocked successfully",
    UpdateErrorMsg: "Failure Update",
    RecordNotFoundMessage: "No Records Found",
    ChartMoveToWIPPendingCharts: "Chart moved to WIP and Pending Charts section",
    ChartMoveToCompletedHoldInvalidCharts: "Chart moved to Completed and Hold Charts section",
    ChartMoveToInvalidCharts: "Invalid Charts are not shown in Completed and Hold Charts section",
    //ChartMoveToCompletedHoldInvalidCharts: "Chart moved to Completed, Hold and Invalid Charts section",
    AuditCommentAddedSuccess: "Audit comment added successfully",
    ChartsNotAvailable: "No chart available",
    ReallocationEmployeeNotChosen: "Please select an employee",
    RequiredOldPassword: "Old password is required",
    RequiredNewPassword: "New password is required",
    RequiredConfirmPassword: "Confirm new password is required",
    InvalidNewPasswordMatch: "The passwords do not match",
    InvalidPasswordMinLength: "Minimum password length is : ",
    SamePasswordError: "The new password cannot be the same as that of the old one.",
    PasswordFormatError: "Your password must be atleast 8 characters long, and contain atleast one number and one special character as well as one UPPER case letter",
    NoChartsSelectedForDelete: "Select chart(s) to delete",
    NoChartsSelectedForDirectUpload: "Select chart(s) for direct upload",
    RequiredEmployeeId: "Employee id is required",
    RequiredFirstName: "First name is required",
    RequiredLastName: "Last name is required",
    RequiredProject: "Project is required",
    RequiredLocation: "Location is required",
    RequiredEmployeeLevel: "Employee level is required",
    AlreadyExistUpdate: "Update Failed, Given Name Already Exist",
    AlreadyExistInsert: "Insert Failed, Given Name Already Exist",
    ChartDeletedSuccessfully: "Chart Deleted Successfully",
    ChartdirectUploadedSuccessfully: "Chart Direct Uploaded Successfully",
    
    CommentDescriptionAlreadyExists: "Comment description already exists.",
    RequiredLookupName: "Lookup name is required.",
    RequiredCommentDescription: "Comment Description is required.",
    RequiredDisplayOrder: "Display order is required.",
    RequiredUserName: "Username is required.",
    NoChartAuditLogSelected: "Select level disagree to Update",
    UnassignedLeveStstusError: "For date type as processed date, you cannot choose level status as unassigned",
    RequiredLevel: "Level cannot be empty.",
    RequiredLevelStatus: "Level status cannot be empty.",
    ChartRequestTimeOutStatus: "Request could not be completed,Please try after sometime.",
    QueueStatus: "Queue cannot be empty.",
    RequiredQueueName: "Queue name is required.",
    QueueAlreadyExists: "Queue already exists.",
    DeleteMessage:"Deleted successfully",
    AddedMessage:"Saved successfully",
    NoUserSelectedForActivateProjects: "Select user(s) for activate projects",
    ActivateSuccessfully:"Activated successfully",
    RequiredFromStatus: "From status cannot be empty.",
    ToStatus: "Status cannot be empty."

};
                      
                 
var MessageSettings = 
{
    messageStyle: 
    {        
        "opacity": 1,
        "-ms-filter": "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)",
        "filter": "alpha(opacity = 100)",        
        "border-radius": "2px",
        "border": "1px solid #F0C36D",
        "box-shadow": "#999 0 0 8px",
        "padding": "6px 9px",
        "background-color": "#F9EDBE",
        "color": "#000",
        "font": "13px sans-serif"        
    },
    messageStyleHover: 
    {        
        "-ms-filter": "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)",
        "filter": "alpha(opacity = 100)",
        "opacity": 1,
        "box-shadow": "#000 0 0 12px"
    },

    TOINMilliSec: 4000,
    position: ["top", "left"],

    holder: $("<div id='divMsgNotify'></div>")
};


/***************************************
    Initialise function to set up, 
    and configure notification box.
**************************************/
function initialiseNotification() 
{
    MessageSettings.holder.css("position", "absolute");
    MessageSettings.holder.css("z-index", 100);
    MessageSettings.holder.css(MessageSettings.position[0], "10px");
    MessageSettings.holder.css(MessageSettings.position[1], "47%");
    $("body").append(MessageSettings.holder);

    $(window).scroll(function () {        
        var msgTopPos = $(window).scrollTop() >= 10 ? window.pageYOffset : 10;
        $('#divMsgNotify').css('top', msgTopPos + 'px');       
    });
}

function removeSpecialChar(str) 
{
    return str.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/>/g, '&gt;').replace(/</g, '&lt;').replace(/'/g, '&#39;');
}


/***************************************
    Function to show message box.    
**************************************/
Notification.showMessage = function (sMsg, iTimeOutInMS, sImg) {
    var notificationBox = $("<div id='divMsgNotify1'></div>");
    notificationBox.css(MessageSettings.messageStyle);

    var textElement = $("<div/>");
    textElement.css("display", "inline-block");
    textElement.css("vertical-align", "middle");
    textElement.css("padding", "0 5px");

    if (sMsg) {
        if (sImg != "" || sImg != null) {
            var imgDivElement = $("<div style='float:left;padding-right:5px;'/>");
            var imgElement = "<img src='" + sImg + "' />";
            imgDivElement.append(imgElement)
            textElement.append(imgDivElement)
        }

        var messageElement = $("<div style='float:left;' />");
        messageElement.append(removeSpecialChar(sMsg));
        textElement.append(messageElement);
    }

    if (!iTimeOutInMS) {
        iTimeOutInMS = MessageSettings.TOINMilliSec;
    }
    notificationBox.delay(iTimeOutInMS).fadeOut();
    notificationBox.bind("click", function () {
        //notificationBox.hide();
        $("#divMsgNotify").empty();
    });

    $("#divMsgNotify").empty();

    //Add notification box
    notificationBox.append(textElement);
    MessageSettings.holder.css("display", "none");
    MessageSettings.holder.prepend(notificationBox);
    var iWinWidth = $(window).width() / 2;
    var iMsgWidth = MessageSettings.holder.width() / 2 + 8;
    MessageSettings.holder.css(MessageSettings.position[1], iWinWidth - iMsgWidth + 'px');    
    MessageSettings.holder.css("display", "inline");
};

Notification.Error = function (sMsg, iTimeOutInMS) {
    Notification.showMessage(sMsg, iTimeOutInMS, 'images/error.png');
};

Notification.Information = function (sMsg, iTimeOutInMS) {
    Notification.showMessage(sMsg, iTimeOutInMS, 'images/info.png');
};

Notification.Warning = function (sMsg, iTimeOutInMS) {
    Notification.showMessage(sMsg, iTimeOutInMS, 'images/warning.png');
};