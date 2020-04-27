
var UserInfo = {};// 用户信息
var dingTalkAppConfig = {
    CorpId: 'ding6909867313baf6fc35c2f4657eb6378f',
    AgentId: '211537030'

};//钉钉配置信息
var domainUrl = 'http://' + window.location.host;
/*
* 初始化钉钉，获取钉钉用户信息
* callback:待执行的回调函数
*/
function InitDingTalk(callback) {
    var dataJson = {
        dingTalkAppConfig: {},
        UserInfo: {}
    };
    $.ajax({
        url: "/DingTalk/GetDingTalkCorpConfigJson",
        type: "get",
        dataType: "json",
        data: { url: window.location.href },
        async: false,
        success: function (data) {
            //dataJson.dingTalkAppConfig = data;
            dingTalkAppConfig = data;
            dd.ready(function () {
                //获取免登授权码 
                dd.runtime.permission.requestAuthCode({
                    corpId: dingTalkAppConfig.CorpId,
                    onSuccess: function (result) {
                        GetDingTalkUser(result["code"]);
                        if (typeof (callback) == 'function') {
                            callback(UserInfo);
                        }
                    }.bind(this),
                    onFail: function (err) { ddalert(err); }
                });
            }.bind(this));
            dd.error(function (error) {
               
                /*{
                    errorCode: 1001, //错误码
                    errorMessage: '', //错误信息
                }*/
                dd.device.notification.alert({
                    message: error.errorCode + "," + error.errorMessage,
                    title: "提示",//可传空
                    buttonName: "好的",
                    onSuccess: function () {
                        /*回调*/
                    },
                    onFail: function (err) { }
                });
            });
        }
    });
}

/*
* 对钉钉JSAP进行注册鉴权
* apiList:需要进行鉴权的JSAP列表
*/
function InitJSAPConfig(apiList) {
    $.ajax({
        url: "/DingTalk/GetDingTalkCorpConfigJson",
        type: "get",
        dataType: "json",
        data: { url: window.location.href },
        async: false,
        success: function (data) {
            dingTalkAppConfig = data;
            dd.config({
                agentId: dingTalkAppConfig.AgentId, // 必填，微应用ID
                corpId: dingTalkAppConfig.CorpId,//必填，企业ID
                timeStamp: dingTalkAppConfig.TimeStamp, // 必填，生成签名的时间戳
                nonceStr: dingTalkAppConfig.NonceStr, // 必填，生成签名的随机串
                signature: dingTalkAppConfig.Signature, // 必填，签名
                jsApiList: apiList
            });
            dd.error(function (error) {
                dd.device.notification.alert({
                    message: error.errorCode + "," + error.errorMessage,
                    title: "提示",//可传空
                    buttonName: "好的",
                    onSuccess: function () {
                        /*回调*/
                    },
                    onFail: function (err) { }
                });
            });
        }
    });
   
    
}

/*
* 检查是否获取到用户信息
* callback:待执行的回调函数
*/
function CheckUserLogin(callback) {
    var cookie = $.cookie('userInfo');
    if (cookie != null && typeof (cookie) != undefined) {
        UserInfo = JSON.parse(cookie);
        $.ajax({
            url: "/DingTalk/CheckUserLogin",
            type: "get",
            dataType: "json",
            data: UserInfo,
            async: false,
            success: function (data) {
            }
        });
        if (typeof (callback) == 'function') {
            callback(UserInfo);
            return;
        }
    } else {
        //获取免登授权码 
        dd.runtime.permission.requestAuthCode({
            corpId: dingTalkAppConfig.CorpId,
            onSuccess: function (result) {
                GetDingTalkUser(result["code"]);
                if (typeof (callback) == 'function') {
                    callback(UserInfo);
                }
            }.bind(this),
            onFail: function (err) { ddalert(err); }
        });
        //dd.ready(function () {
           
        //}.bind(this));
    }
   
}

/*
* 通过code获取UserId等用户信息并登陆
* code:钉钉用户code
*/
function GetDingTalkUser(code) {
   
    $.ajax({
        url: "/DingTalk/DingTalkUserLogin",
        type: "get",
        dataType: "json",
        data: { code: code },
        async: false,
        success: function (data) {
            UserInfo = JSON.parse(data.data);
            dd.userId = UserInfo.UserId;
            $.cookie('userInfo', JSON.stringify(UserInfo), { expires: 1, path: '/' })
        }
    });
}

/*
* 在钉钉应用内跳转页面
* url:钉钉用户code
* title:页面标题
*/
+function ($) {
    $.goToPage = function (url, title) {
       
        if (dd.android || dd.ios) {
            dd.biz.util.openLink({
                url: domainUrl + url,//要打开链接的地址
                onSuccess: function (result) {
                    /**/
                },
                onFail: function (err) { }
            })
        } else {
            dd.biz.util.openSlidePanel({
                url: domainUrl + url,//要打开链接的地址
                title: title, //侧边栏顶部标题
                onSuccess: function (result) {
                    /*
                         调用biz.navigation.quit接口进入onSuccess, result为调用biz.navigation.quit传入的数值
                     */
                },
                onFail: function () {
                    /*
                        tips:点击右上角上角关闭按钮会进入onFail
                     */
                }
            })
        }
    }

}($);

function ddalert(msg) {
    dd.device.notification.alert({
        message: msg ,
        title: "提示",//可传空
        buttonName: "好的",
        onSuccess: function () {
            /*回调*/
        },
        onFail: function (err) { }
    });
}

function dtConfirm(msg, callback) {
   
    dd.device.notification.confirm({
        message: msg,
        title: "提示",
        buttonLabels: ['是', '否'],
        onSuccess: function (result) {
            /*
            {
                buttonIndex: 0 //被点击按钮的索引值，Number类型，从0开始
            }
            */
            if (typeof (callback) == 'function') {
                callback(buttonIndex);
            }
        },
        onFail: function (err) { }
    });
}

