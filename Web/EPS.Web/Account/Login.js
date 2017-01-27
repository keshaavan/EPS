var Login = {};

Login.initialise = function () {
    $("#LoginUser_UserName").focus();

    // Login.LoadData();
    //$('#ProjectDropdownList').add("--Choose a Project--");
    $("#LoginUser_Password").keydown(function (event) {
        if (event.keyCode == 32) {
            event.preventDefault();
        }
    });

    $("#LoginUser_UserName").keydown(function (event) {
        if (event.keyCode == 32) {
            event.preventDefault();
        }
    });

    $(".water").each(function () {
        $tb = $(this);
        if ($tb.val() != this.title) {
            $tb.removeClass("water");
        }
    });

    $(".water").focus(function () {
        $tb = $(this);
        if ($tb.val() == this.title) {
            $tb.val("");
            $tb.removeClass("water");
        }
    });

    $(".water").blur(function () {
        $tb = $(this);
        if ($.trim($tb.val()) == '') {
            $tb.val(this.title);
            $tb.addClass("water");
        }
    });

    $("#ClientDropdownList").change(function () {
        var sClientDDL = $("#ClientDropdownList").val();
        if (sClientDDL != "") {
            $('#ProjectDropdownList').removeAttr('disabled');
            var Id = $('option:selected', this).val();
            var param = "{clientkey:'" + Id + "'}";
            jQuery.ajax({
                type: "POST",
                url: "Login.aspx/getClientProjectList",
                data: param,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (sJsonDataResponse) {
                    var dataparse = JSON.parse(sJsonDataResponse.d);
                    var length = dataparse.length;
                    if (length > 0) {
                        $("select[id=ProjectDropdownList] > option").remove();
                        $.each(dataparse, function (index, item) {
                            $('#ProjectDropdownList').append($("<option></option>").val(dataparse[index]["Id"]).html(dataparse[index]["Name"]));
                        });
                        $("#ProjectDropdownList").prepend("<option value='' selected='selected'>--Choose a Project--</option>");
                    }
                    else {
                        $("#ProjectDropdownList").prepend("<option value='' selected='selected'>--No Projects--</option>");
                    }
                },
                error: function (jqXHR, sExpMsg) {
                    //alert("Fail");
                }
            });
        }
        else {
            $("select[id=QueueDropdownList] > option").remove();
            $("select[id=ProjectDropdownList] > option").remove();

            $('#ProjectDropdownList').AddAttr('disabled');
            $('#QueueDropdownList').AddAttr('disabled');
        }
    });


    $("#ProjectDropdownList").change(function () {
        var sProjectDDL = $("#ProjectDropdownList").val();
        if (sProjectDDL != "") {
            $('#QueueDropdownList').removeAttr('disabled');
            var Id = $('option:selected', this).val();
            var param = "{projectkey:'" + Id + "'}";
            jQuery.ajax({
                type: "POST",
                url: "Login.aspx/getClientProjectQueueList",
                data: param,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (sJsonDataResponse) {
                    var dataparse = JSON.parse(sJsonDataResponse.d);
                    var length = dataparse.length;
                    if (length > 0) {
                        $("select[id=QueueDropdownList] > option").remove();
                        $.each(dataparse, function (index, item) {
                            $('#QueueDropdownList').append($("<option></option>").val(dataparse[index]["Id"]).html(dataparse[index]["Name"]));
                        });
                        $("#QueueDropdownList").prepend("<option value='' selected='selected'>--Choose a Queue-</option>");
                    }
                    else {
                        $("#QueueDropdownList").prepend("<option value='' selected='selected'>--No Queues--</option>");
                    }
                },
                error: function (jqXHR, sExpMsg) {
                    //alert("Fail");
                }
            });
        }
        else {
            $("select[id=QueueDropdownList] > option").remove();
            $('#QueueDropdownList').AddAttr('disabled');
        }
    });
}
//Login.LoadData = function () {
////        var Id= $("#ClientDropdownList").val();
////        alert("sample");
////    var param = "";
////    jQuery.ajax({
////        type: "POST",
////        url: "Login.aspx/GetClientsDetails",
////        data: "{ }",
////        contentType: "application/json; charset=utf-8",
////        dataType: "json",
////        success: function (sJsonDataQueue) {
////            var dataQueue = JSON.parse(sJsonDataQueue.d);
////            var queuelength = dataQueue.length;
////            if (queuelength > 0) {
////                $("select[id=ClientDropdownList] > option").remove();
////                $.each(dataQueue, function (index, item) {
////                    $('#ClientDropdownList').append($("<option></option>").val(item[0]).html(item[1]));
////                });
////            }
////        },
////        error: function (jqXHR, sExpMsg) {
////            alert("Fail");
////        }
////    });

//}
