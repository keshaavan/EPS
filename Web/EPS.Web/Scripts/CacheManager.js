var CacheManager = {};

CacheManager.DontCacheList = ["TaskAllocation/Level1.aspx", "TaskAllocation/Level2.aspx", "TaskAllocation/Level3.aspx", "Reports/ChartStatus.aspx", "Reports/DailyProductionSummary.aspx", "Reports/ProductionStatus.aspx", "Admin/UserCreation.aspx", "Admin/ChartReallocation.aspx", "Admin/ClientchartDeallocation.aspx", "Admin/MasterData.aspx"];
CacheManager.add = function (sKey, sVal) {
    if ($.inArray(sKey, CacheManager.DontCacheList) < 0) {
        $(this).data(CacheManager.removeQryStr(sKey), sVal);
    }
}

CacheManager.get = function (sKey) {

    return $(this).data(CacheManager.removeQryStr(sKey));
}

CacheManager.exist = function (sKey) {

    if ($(this).data(CacheManager.removeQryStr(sKey)) == null) {
        return false;
    }

    return true;
}

CacheManager.removeQryStr = function (sKey) {

    return sKey.split('?')[0].toLowerCase();
}