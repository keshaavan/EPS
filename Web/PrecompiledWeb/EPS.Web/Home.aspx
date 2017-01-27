<%@ page language="C#" autoeventwireup="true" inherits="Home, App_Web_chlyqs0y" %>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        Home.initialise();
    });
</script>
<form id="homeForm" runat="server">
<div>
    <div id="dvCompletedCount" style="display:none;">
        <div style="float: left; padding-left: 32px;">
            From Date</div>
        <div class="floatleft" style="padding-left: 5px; margin-left: 5px; width: 90px;">
            <input type="text" id="txtFromdate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
        </div>
        <div style="float: left; padding-left: 32px;">
            To Date
        </div>
        <div class="floatleft" style="padding-left: 5px; margin-left: 5px; width: 90px;">
            <input type="text" id="txtTodate" readonly style="width: 80px;" class="text ui-widget-content ui-corner-all" />
        </div>
        <div class="floatleft" style="padding-left: 37px;">
            <input type="button" id="btnViewPerformanceSummary" value="View" class="buttons buttons-gray" />
        </div>
    </div>
    <div class="columns">
        <div class="grid_2 first">
            <div class="message info closeable">
                <span id="homepageText" />
            </div>
        </div>
    </div>
    <input type="hidden" id="hdn1" />
</div>
</form>
