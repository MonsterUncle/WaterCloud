var clients = {};
var currentUser = {};
$(function () {
    if (!!top.clients && top.clients.hasOwnProperty("dataItems")) {
        clients = top.clients;
    }
    else {
        clients = $.clientsInit();
    }
    if (!!top.currentUser && top.clients.hasOwnProperty("F_Id")) {
        currentUser = top.currentUser;
    }
    else {
        currentUser = $.userInit();
    }
})
$.clientsInit = function () {
    var dataJson = {
        dataItems: [],
        authorizeButton: [],
        authorizeFields: [],
        moduleFields: [],
        printTemplates: [],
    };
    var init = function () {
        $.ajax({
            url: "/ClientsData/GetClientsDataJson?v=" + new Date().Format("yyyy-MM-dd hh:mm:ss"),
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                dataJson.dataItems = data.dataItems;
                dataJson.authorizeButton = data.authorizeButton;
                dataJson.authorizeFields = data.authorizeFields;
                dataJson.moduleFields = data.moduleFields;
                dataJson.printTemplates = data.printTemplates;
            }
        });
    }
    init();
    return dataJson;
}
$.userInit = function () {
    var dataJson = {};
    var init = function () {
        $.ajax({
            url: "/ClientsData/GetUserCode?v=" + new Date().Format("yyyy-MM-dd hh:mm:ss"),
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                dataJson = data;
            }
        });
    }
    init();
    return dataJson;
}