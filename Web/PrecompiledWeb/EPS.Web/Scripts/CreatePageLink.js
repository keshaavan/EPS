var CreatwPageLink = {};

CreatwPageLink.initialise = function () {
    //====================================Start initialise=================

        var oTable;
        var tableId = 'JDtbl_DisplayLink';
        var ws_GetData = 'Setup/CreatePageLink.aspx/getLinkData';
        var ws_UpdateData = 'Setup/CreatePageLink.aspx/updateLinksData';
        var ws_AddData = 'Setup/CreatePageLink.aspx/addLinksData';
     
        var isDeleteEnbaled = true;
        var isUpdateEnabled = true;
        var isAddEnabled = true;
        var isDropdownEnabled = true;



//================================Jquery DataTable=======================

         if (isShowGrid == "0") {
              $('#' + tableId).css("display", "none");
              return;
          }
          oTable = $('#' + tableId).dataTable({
              bJQueryUI: true,
              "sPaginationType": "full_numbers",
              "aLengthMenu": [[10,25,50,100], [10,25,50,100]],
              'bProcessing': true,
              'bServerSide': true,
              'bPaginate': true,
              //'iDisplayLength': 10,
            
              'bFilter': true,
              "oLanguage": { "sSearch": "Search the :" },
                   "aoColumns": [
                    {"sType": "html", "sWidth" : "5%"},
                    {"sType": "html", "sWidth" : "15%"},
                    {"sType": "html", "sWidth" : "15%"},
                    {"sType": "html", "sWidth" : "10%"},
                    {"sType": "html", "sWidth" : "30%"},
                    {"sType": "html", "sWidth" : "5%"},
                     ],
              'sAjaxSource': ws_GetData,
              "fnServerData": function (sSource, aoData, fnCallback) { GrabData(sSource, aoData, fnCallback); }
          }).makeEditable({
              
           
          });
              //===================================LoadData==========================================
      function GrabData(sSource, aoData, fnCallback) {
          $.ajax({
              type: "GET",
              url: sSource,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              data: aoData,
              success: function (result) {
                  var myObject = JSON.parse(result.d);
                  if (isUpdateEnabled) {
                      for (var i = 0; i < myObject.aaData.length; i++) {
                          var str2 = "<input id='" + tableId + "_update_" + myObject.aaData[i][0] + "' type='button'  class='Updatebuttonimage'   onclick='UpdatePrepare(" + JSON.stringify(myObject.aaData[i]) + ");'  />";
                        
                          myObject.aaData[i].push(str2);
                      }
                  }
                  fnCallback(myObject);
              },
              error: function (errMsg) {
               
                  Notification.Error("Data Loading Error");
              }
          });
      }
//===============================Button Click Event==========================

      $("#btnAddDatatable").click(function () {


          var validations = EmptyCheck();
          var Operation = $('input[name=addNew]').val();
          if (validations) {


              if (Operation == "+ Add New") {

                  AddMe();

              }
              else if (Operation == "Update") { UpdateMe(); ResetField(); }
          }
      });

      $("#btnReset").click(function () {

          ResetField();

      });

      $('#LinkName').keypress(function (e) {
          if (String.fromCharCode(e.keyCode).match(/^\s*[a-zA-Z,\s]+\s*$/))
              return true;
          Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
          return false;
      });
      $('#MenuDisplayName').keypress(function (e) {
          if (String.fromCharCode(e.keyCode).match(/^\s*[a-zA-Z,\s]+\s*$/))
              return true;
          Notification.Error(Notification.enumMsg.NoSpecialCharactersMessage);
          return false;
      });

      $('#ParentLinkid').keypress(function (e) {
          if (String.fromCharCode(e.keyCode).match(/^[0-9\ ]+$/))
              return true;
          Notification.Error(Notification.enumMsg.OnlyNumberMessage);
          return false;
      });

      //===================================Add Function==========================================
      function AddMe() {
          var Linkid = $('#Linkid').val();
          var LinkName = $('#LinkName').val();
          var MenuDisplayName = $('#MenuDisplayName').val();
          var ParentLinkid = $('#ParentLinkid').val();
          var NavigationURL = $('#NavigationURL').val();
          var status;


          var parameters = "{'sLinkName':'" + LinkName + "','sMenuDisplayName':'" + MenuDisplayName + "','sParentLinkid':'" + ParentLinkid + "','sNavigationurl':'" + NavigationURL + "'}";
          $.ajax({
              type: "POST",
              url: ws_AddData,
              data: parameters,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (msg) {

                  if (msg.d == "True") {
                      Notification.Information(Notification.enumMsg.InsertMessage);
                  }
                  else { Notification.Error("" + msg.d); }



                  oTable.fnDraw();
              },
              error: function (e) {

                  Notification.Error("Failure Add");
              }
          });
      }

      //===================================Update Function==========================================
      function UpdatePrepare(data) {
          $('input[name=addNew]').val("Update");

          $('#Linkid').val(data[0]);
          $('#LinkName').val(data[1]);
          $('#MenuDisplayName').val(data[2]);
          $('#ParentLinkid').val(data[3]);
          $('#NavigationURL').val(data[4]);

      }
      function UpdateMe() {
          var Linkid = $('#Linkid').val();
          var LinkName = $('#LinkName').val();
          var MenuDisplayName = $('#MenuDisplayName').val();
          var ParentLinkid = $('#ParentLinkid').val();
          var NavigationURL = $('#NavigationURL').val();
          var parameters = "{'iLinkid':'" + Linkid + "','sLinkName':'" + LinkName + "','sMenuDisplayName':'" + MenuDisplayName + "','sParentLinkid':'" + ParentLinkid + "','sNavigationurl':'" + NavigationURL + "'}";
          $.ajax({
              type: "POST",
              url: ws_UpdateData,
              data: parameters,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (msg) {
                  Notification.Information(Notification.enumMsg.UpdateMessage);

                  oTable.fnDraw(false);
              },
              error: function (e) {

                  Notification.Error("Failure Update");
              }
          });
      }





      function ResetField() {
          $('#Linkid').val('');
          $('#LinkName').val('');


          $('#MenuDisplayName').val('');

          $('#ParentLinkid').val('');

          $('#NavigationURL').val('');

          $('input[name=addNew]').val("+ Add New");

      }
      function EmptyCheck() {

          var Linkid = $('#Linkid').val();
          var LinkName = $('#LinkName').val();
          var MenuDisplayName = $('#MenuDisplayName').val();
          var ParentLinkid = $('#ParentLinkid').val();
          var NavigationURL = $('#NavigationURL').val();


          if (LinkName.trim() == '') {

              Notification.Error("LinkName is Required");
              return false;
          }
          else if (MenuDisplayName.trim() == '') {


              Notification.Error("MenuDisplayName is Required");
              return false;
          }
          else if (ParentLinkid.trim() == '') {

              Notification.Error("ParentLinkid is Required");

              return false;
          }
          else if (NavigationURL.trim() == '') {
              Notification.Error("NavigationURL is Required");

              return false;
          }

          else {
              return true;
          }

      }
      //====================================End initialise=================

}
  

