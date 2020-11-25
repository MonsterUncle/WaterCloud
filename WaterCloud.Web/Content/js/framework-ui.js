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
        change: null
    };
    var options = $.extend(defaults, options);
    //ie缓存问题
    if (!!options.url) {
        if (options.url.indexOf("?") >= 0) {
            options.url = options.url + '&v=' + new Date();
        }
        else {
            options.url = options.url + '?v=' + new Date();
        }
    }
    var $element = $(this);
    if (options.url != "") {
        $.ajax({
            url: options.url,
            data: options.param,
            dataType: "json",
            async: false,
            success: function (data) {
                $.each(data, function (i) {
                    $element.append($("<option></option>").val(data[i][options.id]).html(data[i][options.text]));
                });
                //$element.select2({
                //    minimumResultsForSearch: options.search == true ? 0 : -1
                //});
                $element.on("change", function (e) {
                    if (options.change != null) {
                        options.change(data[$(this).find("option:selected").index()]);
                    }
                    //$("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
                });
            }
        });
    } 
    //  else {
    //    $element.select2({
    //        minimumResultsForSearch: -1
    //    });
    //}
}
//combobox绑定
$.fn.bindComboBox= function (options) {
    var defaults = {
        id: "id",
        text: "text",
        search: false,
        url: "",
        param: [],
        change: null
    };
    var options = $.extend(defaults, options);
    //ie缓存问题
    if (!!options.url) {
        if (options.url.indexOf("?") >= 0) {
            options.url = options.url + '&v=' + new Date();
        }
        else {
            options.url = options.url + '?v=' + new Date();
        }
    }
    var $element = $(this);
    if (options.url != "") {
        $.ajax({
            url: options.url,
            data: options.param,
            dataType: "json",
            async: false,
            success: function (data) {
                $.each(data, function (i) {
                    $element.append($("<option></option>").val(data[i][options.id]).html(data[i][options.text]));
                });
                $element.select2({
                    minimumResultsForSearch: options.search == true ? 1 : 0
                });
                $element.on("change", function (e) {
                    if (options.change != null) {
                        options.change(data[$(this).find("option:selected").index()]);
                    }
                    $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
                });
            }
        });
    } else {
        $element.select2({
            minimumResultsForSearch: -1
        });
    }
}
//radiobox绑定
$.fn.bindRadioBox = function (options) {
    var defaults = {
        id: "id",
        text: "text",
        param: [],
        checked: "",
        change: null
    };
    var options = $.extend(defaults, options);
    //ie缓存问题
    if (!!options.url) {
        if (options.url.indexOf("?") >= 0) {
            options.url = options.url + '&v=' + new Date();
        }
        else {
            options.url = options.url + '?v=' + new Date();
        }
    }
    var $element = $(this);

    $("[id=" + $element.attr("id") + "]").each(function () {
        $(this).css("display", "none");
        if ($(this).checked == true) {
            $lable.addClass("active");
        }
        var $lable = $(this).parent('label');
        $lable.on("click", function (e) {
            $(this).siblings().removeClass("active");
            $(this).addClass("active");
            $(this).children('input').checked = true;
        });
    });
}

/*WaterCloud_iframe
* 初始化bootstrap的按钮组btn-group
* .btn-group 中放置按钮和下拉菜单即可；
*/
$.fn.bindBtnGroup = function (options) {
    var defaults = {
        id: "id",
        text: "text",
        data: [],
        onClick: null
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    $element.removeAttr('data-value');
    $element.attr('data-value','');
    $.each(options.data, function (i) {
        var active = options.data[i]['checked'] == true ? "active" : "";
        $element.append('<a class="btn btn-default ' + active + '" data-value="' + options.data[i][options.id] + '">' + options.data[i][options.text] + '</a>');
    });
    $element.find("a.btn-default").on("click", function () {
        $(this).siblings().removeClass("active");
        $(this).addClass("active");
        $element.attr('data-value', $(this).attr('data-value'));
        if (options.onClick != null) {
            options.onClick();
        }
    });
}
/*
* 初始化bootstrap的按钮下拉菜单
* .btn-group 中放置按钮和下拉菜单dropdown-menu即可；
*/
$.fn.bindBtnDropdown = function (options) {
    var defaults = {
        id: "id",
        text: "text",
        data: [],
        onClick: null
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    var $dropdown = $element.find(".dropdown-menu");

    $.each(options.data, function (i) {
        $dropdown.append('<li><a href="javascript:void()" data-value="' + options.data[i][options.id] + '">' + options.data[i][options.text] + '</a></li>');
    });
    $element.find(".dropdown-menu li").on("click", function () {
        var text = $(this).find('a').html();
        var value = $(this).find('a').attr('data-value');
        $element.find(".dropdown-text").html(text).attr('data-value', value);
        if (options.onClick != null) {
            options.onClick();
        }
    });

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