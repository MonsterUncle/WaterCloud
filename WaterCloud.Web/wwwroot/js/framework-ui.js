$(function () {
    //document.body.className = localStorage.getItem('config-skin');
    //$("[data-toggle='tooltip']").tooltip();
})
//获取参数
$.request = function (name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}
//json拼接
$.jsonWhere = function (data, action) {
    if (action == null) return;
    var reval = new Array();
    $(data).each(function (i, v) {
        if (action(v)) {
            reval.push(v);
        }
    })
    return reval;
}
//select绑定
$.fn.bindSelect = function (options) {
    var defaults = {
        id: "id",
        text: "text",
        search: false,
        url: "",
        param: [],
        change: null,
        data:null,
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    if (options.data!=null) {
        $.each(options.data, function (i) {
            if (options.id == "") {
                //字典
                $element.append($("<option></option>").val(i).html(options.data[i]));
            }
            else {
                //list
                $element.append($("<option></option>").val(options.data[i][options.id]).html(options.data[i][options.text]));
            }
        });
        $element.on("change", function (e) {
            if (options.change != null) {
                options.change(options.data[$(this).find("option:selected").index()]);
            }
        });
    }
    else if (options.url != "") {
        //ie缓存问题
        if (options.url.indexOf("?") >= 0) {
            options.url = options.url + '&v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
        }
        else {
            options.url = options.url + '?v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
        }
        $.ajax({
            url: options.url,
            data: options.param,
            dataType: "json",
            async: false,
            success: function (data) {
                $.each(data, function (i) {
                    if (options.id=="") {
                        $element.append($("<option></option>").val(data[i]).html(data[i]));
                    }
                    else {
                        $element.append($("<option></option>").val(data[i][options.id]).html(data[i][options.text]));
                    }
                });
                $element.on("change", function (e) {
                    if (options.change != null) {
                        options.change(data[$(this).find("option:selected").index()]);
                    }
                });
            }
        });
    } 
}
// 时间格式方法
Date.prototype.Format = function (fmt) {
    var o = {

        "M+": this.getMonth() + 1, //月份 

        "d+": this.getDate(), //日 

        "h+": this.getHours(), //小时 

        "m+": this.getMinutes(), //分 

        "s+": this.getSeconds(), //秒 

        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 

        "S": this.getMilliseconds() //毫秒 

    };

    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));

    for (var k in o)

        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
Date.prototype.formatDate = function () { //author: meizz   
    var myyear = this.getFullYear();
    var mymonth = this.getMonth() + 1;
    var myweekday = this.getDate();
    var myhour = this.getHours();
    var myminute = this.getMinutes();
    var second = this.getSeconds();
    if (mymonth < 10) {
        mymonth = "0" + mymonth;
    }
    if (myweekday < 10) {
        myweekday = "0" + myweekday;
    }
    if (myhour < 10) {
        myhour = "0" + myhour;
    }
    if (myminute < 10) {
        myminute = "0" + myminute;
    }
    if (second < 10) {
        second = "0" + second;
    }
    return (myyear.toString() + mymonth.toString() + myweekday.toString() + myhour.toString() + myminute.toString() + second.toString());
};
//集合id
function uuid() {
    var s = [];
    var hexDigits = "0123456789abcdef";
    for (var i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
    }
    s[14] = "4"; // bits 12-15 of the time_hi_and_version field to 0010
    s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01
    s[8] = s[13] = s[18] = s[23] = "-";

    var uuid = s.join("");
    return uuid;
}
function utf16toEntities(str) {
    const patt = /[\ud800-\udbff][\udc00-\udfff]/g; // 检测utf16字符正则
    str = str.replace(patt, (char) => {
        let H;
        let L;
        let code;
        let s;

        if (char.length === 2) {
            H = char.charCodeAt(0); // 取出高位
            L = char.charCodeAt(1); // 取出低位
            code = (H - 0xD800) * 0x400 + 0x10000 + L - 0xDC00; // 转换算法
            s = `&#${code};`;
        } else {
            s = char;
        }

        return s;
    });

    return str;
}
// 表情解码
function entitiestoUtf16(strObj) {
    const patt = /&#\d+;/g;
    const arr = strObj.match(patt) || [];

    let H;
    let L;
    let code;

    for (let i = 0; i < arr.length; i += 1) {
        code = arr[i];
        code = code.replace('&#', '').replace(';', '');
        // 高位
        H = Math.floor((code - 0x10000) / 0x400) + 0xD800;
        // 低位
        L = ((code - 0x10000) % 0x400) + 0xDC00;
        code = `&#${code};`;
        const s = String.fromCharCode(H, L);
        strObj = strObj.replace(code, s);
    }
    return strObj;
}