var Login = {};
//alert('hi');

Login.initialise = function () {
    $("#LoginUser_UserName").focus();

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

    $("#LoginUser_ClientDropdownList").change(function () {
        //alert('hi');
        $('#LoginUser_ProjectDropdownList').removeAttr('disabled');
        var Id = $('option:selected', this).val();
       // alert(Id);
        var param = "{id:'" + Id + "'}";
        ServiceBroker.callWebMethod(enumWebLoginPage.Login, enumWebLoginMethod.getClientProjectList, param, 'Login.BindProjectsDetails', true, true);
        //ServiceBroker.callWebMethod(enumWebPage.UserCreation, enumWebMethod.getClientProjectData, sParam, 'UserCreation.ClientProjectBindedSuccess', true, true);
        // Login.LoadData();
    });
    Login.BindProjectsDetails = function (msg) {
        //$('#LoginUser_ProjectDropdownList').append($("<option></option>").val(""));
        $("select[id=LoginUser_ProjectDropdownList] > option").remove();
        //    $("#LoginUser_ProjectDropdownList").trigger("liszt:updated");
        $.each(msg, function (index, item) {
            $('#LoginUser_ProjectDropdownList').append($("<option></option>").val(item[0]).html(item[1]));
            $("#LoginUser_ProjectDropdownList").trigger("liszt:updated");
        });
    }

    $("#LoginUser_ProjectDropdownList").change(function (id) {
        alert('hi');
        $('#LoginUser_QueueDropdownList').removeAttr('disabled');
        //var Id = $('option:selected', this).val();
        //var param = "{id:" + Id + " }";
    });
}
//Login.LoadData = function () {

//    alert(Id);

//}
