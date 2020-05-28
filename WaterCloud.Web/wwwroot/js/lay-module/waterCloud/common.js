/**
 * date:2020/02/27
 * author:Mr.Chung
 * version:2.0
 * description:layuimini 主体框架扩展
 */
layui.define(["jquery", "layer", 'form', 'table', 'tablePlug', 'treetable' , 'xmSelect','miniTab'], function (exports) {
    var $ = layui.jquery,
        form = layui.form,
        miniTab = layui.miniTab,
        layer = layui.layer,
        treetable = layui.treetable,
        tablePlug = layui.tablePlug,
        xmSelect = layui.xmSelect,
        table = layui.table;

    var obj = {
        //table渲染封装里面有字段权限
        rendertable: function (options) {
            var loading = layer.load(0, { shade: false });
            var defaults = {
                elem: '#currentTableId',//主键
                toolbar: '#toolbarDemo',//工具栏
                cellMinWidth: 80,  //全局定义常规单元格的最小宽度，layui 2.2.1 新增
                defaultToolbar: ['filter', 'exports', 'print'],//默认工具栏
                method: 'get',//请求方法
                cellMinWidth: 100,//最小宽度
                limit: 10,//每页数据 默认
                height: $(window).height() > 500 ? 'full-150' : 'full-190',
                loading:false,
                page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
                    layout: ['skip', 'prev', 'page', 'next','count'] //自定义分页布局
                    //,curr: 2 //设定初始在第 5 页
                    , groups: 3 //只显示 1 个连续页码
                    , first: false //不显示首页
                    , last: false //不显示尾页
                },
                smartReloadModel: true, // 是否开启智能reload的模式
                request: {
                    pageName: 'page' //页码的参数名称，默认：page
                    , limitName: 'rows' //每页数据量的参数名，默认：limit
                },
                parseData: function (res) { //res 即为原始返回的数据
                    return {
                        "code": res.state, //解析接口状态
                        "msg": res.message, //解析提示文本
                        "count": res.count, //解析数据长度
                        "data": res.data //解析数据列表
                        };
                    },
                done: function (res, curr, count) { // 使用自定义参数hideAlways隐藏
                    //$(".layui-table-box").find("[data-field='F_Id']").css("display", "none");
                    //关闭加载
                    layer.closeAll('loading');
                }
            };
            var options = $.extend(defaults, options);
            //ie缓存问题
            if (options.url.indexOf("?") >= 0) { 
                options.url = options.url + '&v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            else {
                options.url = options.url + '?v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            var moduleId = top.$(".layui-tab-title>.layui-this").attr("lay-id");
            if (!top.clients.moduleFields[moduleId.split("?")[0]] && top.clients.moduleFields[moduleId.split("?")[0]] == false) {
                var dataJson = top.clients.authorizeFields[moduleId.split("?")[0]];
                $.each(options.cols[0], function (i) {
                    options.cols[0][i].hideAlways = true;
                    options.cols[0][i].hide = true;
                    if (dataJson != undefined) {
                        for (var j = 0; j < dataJson.length; j++) {
                            if (options.cols[0][i].field == dataJson[j].F_EnCode) {
                                options.cols[0][i].hideAlways = false;
                                options.cols[0][i].hide = false;
                                break;
                            }
                        }
                        //dataJson.find(item => {
                        //    if (options.cols[0][i].field == item.F_EnCode) {
                        //        options.cols[0][i].hideAlways = false;
                        //        options.cols[0][i].hide = false;
                        //    }
                        //});
                    }
                });
            };
           return table.render(options);
        },
        //tabletree渲染封装里面有字段权限
        rendertreetable: function (options) {
            //样式不协调，先不加
            var loading = layer.load(0, { shade: false });
            var defaults = {
                elem: '#currentTableId',//主键
                toolbar: '#toolbarDemo',//工具栏
                treeColIndex: 1,	//树形图标显示在第几列
                treeSpid: '0',		//最上级的父级id
                treeIdName: 'F_Id',	//id字段的名称
                treePidName: 'F_ParentId',	//父级节点字段
                loading: false,
                height: $(window).height() > 500 ? 'full-150' : 'full-190',
                method: 'get',//请求方法
                cellMinWidth: 100,//最小宽度
                smartReloadModel: true, // 是否开启智能reload的模式
                page: false,
                icon: false,             
                done: function () {
                    //$(".layui-table-box").find("[data-field='F_Id']").css("display", "none");
                    //关闭加载
                    layer.closeAll('loading');
                }
            };
            var options = $.extend(defaults, options);
            //ie缓存问题
            if (options.url.indexOf("?") >= 0) {
                options.url = options.url + '&v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            else {
                options.url = options.url + '?v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            var moduleId = top.$(".layui-tab-title>.layui-this").attr("lay-id");
            if (!top.clients.moduleFields[moduleId.split("?")[0]] && top.clients.moduleFields[moduleId.split("?")[0]] == false) {
                var dataJson = top.clients.authorizeFields[moduleId.split("?")[0]];
                $.each(options.cols[0], function (i) {
                    options.cols[0][i].hideAlways = true;
                    options.cols[0][i].hide = true;
                    if (dataJson != undefined) {
                        for (var j = 0; j < dataJson.length; j++) {
                            if (options.cols[0][i].field == dataJson[j].F_EnCode) {
                                options.cols[0][i].hideAlways = false;
                                options.cols[0][i].hide = false;
                                break;
                            }
                        }
                        //dataJson.find(item => {
                        //    if (options.cols[0][i].field == item.F_EnCode) {
                        //        options.cols[0][i].hideAlways = false;
                        //        options.cols[0][i].hide = false;
                        //    }
                        //});
                    }
                });
            };
            return treetable.render(options);
        },
        //table刷新
        reloadtable: function (options) {
            var loading = layer.load(0, { shade: false });
            var defaults = {
                elem: 'currentTableId',//主键
                page: true,//分页参数
                curr: 1,//当前页
                where: {}
            };
            var options = $.extend(defaults, options);
            options.where.time = new Date().Format("yyyy-MM-dd hh:mm:ss");
            if (options.page) {
                //执行搜索重载
                table.reload(options.elem, {
                    page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
                        layout: ['skip', 'prev', 'page', 'next', 'count'] //自定义分页布局
                        //,curr: 2 //设定初始在第 5 页
                        , groups: 3 //只显示 1 个连续页码
                        , first: false //不显示首页
                        , last: false, //不显示尾页
                        curr: options.curr
                    }
                    , where: options.where
                }, 'data');
            }
            else {
                //执行搜索重载
                table.reload(options.elem, {
                    where: options.where
                }, 'data');
            }
            //关闭加载
            layer.closeAll('loading');
        },
        //msg
        modalMsg: function (content, type, close) {
            var close = close|| false;
            if (type != undefined) {
                var icon = 0;
                if (type == 'success') {
                    icon = 1;
                }
                else if (type == 'error') {
                    icon = 2;
                }
                else if (type == 'warning') {
                    icon = 7;
                }
                else {
                    icon = 5;
                }
                top.layer.msg(content, {
                    icon: icon, time: 1000, shift: 5,
                }, function () {
                    if (close) {
                        var index = parent.layer.getFrameIndex(window.name);
                        parent.layer.close(index);
                    }
                });
            } else {
                top.layer.msg(content);
            }
        },
        //alert
        modalAlert: function (content, type, close) {
            var close = close || false;
            var icon = 0;
            if (type == 'success') {
                icon = 1;
            }
            if (type == 'error') {
                icon = 2;
            }
            if (type == 'warning') {
                icon = 7;
            }
            top.layer.alert(content, {
                icon: icon,
                title: "系统提示",
                btn: ['确认'],
                btnclass: ['btn btn-primary'],
                yes: function (index) {
                    if (close) {
                        var pindex = parent.layer.getFrameIndex(window.name);
                        parent.layer.close(pindex);
                        top.layer.close(index);
                    }
                    else {
                        top.layer.close(index);
                    }
                }
            });
        },
        //界面关闭
        modalClose: function () {
            var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
            var $IsdialogClose = top.$("#layui-layer" + index).find('.layui-layer-btn').find("#IsdialogClose");
            var IsClose = $IsdialogClose.is(":checked");
            if ($IsdialogClose.length == 0) {
                IsClose = true;
            }
            if (IsClose) {
                parent.layer.close(index);
            } else {
                location.reload();
            }
        },
        //创建界面
        modalOpen: function (options) {
            var defaults = {
                title: '系统窗口',
                width: "100px",
                height: "100px",
                anim: 0,//动画
                isOutAnim:true,//关闭动画
                url: '',
                shade: 0.3,
                btn: ['确认', '关闭'],
                btnclass: ['layui-btn', 'layui-btn-primary'],
                callBack: null,
                end: null,
                yes: function (index, layero) {
                    var iframeWindow = window['layui-layer-iframe' + index]
                        , submitID = 'submit'
                        , submit = layero.find('iframe').contents().find('#' + submitID);
                    submit.trigger('click');
                }
            };
            var options = $.extend(defaults, options);
            var _width = top.$(window).width() > parseInt(options.width.replace('px', '')) ? options.width : top.$(window).width() - 20 + 'px';
            var _height = top.$(window).height() > parseInt(options.height.replace('px', '')) ? options.height : top.$(window).height() - 20 + 'px';
            layer.open({
                type: 2,
                shade: options.shade,
                title: options.title,
                fix: false,
                area: [_width, _height],
                content: options.url,
                btn: options.btn,
                success: function (layero) {
                    $(layero).addClass("scroll-wrapper");//苹果 iframe 滚动条失效解决方式
                },
                //btnclass: options.btnclass,
                yes: function (index, layero) {
                    if (!!options.yes) {
                        options.yes(index, layero);
                    }
                }, cancel: function () {
                    return true;
                },
                end: function () {
                    if (!!options.end) {
                        options.end();
                    }
                }
            });
        },
        //表单提交
        submitForm: function (options) {
            var defaults = {
                url: "",
                param: [],
                success: null,
                close: true
            };
            var options = $.extend(defaults, options);
            //ie缓存
            if (options.url.indexOf("?") >= 0) {
                options.url = options.url + '&v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            else {
                options.url = options.url + '?v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            window.setTimeout(function () {
                if ($('[name=__RequestVerificationToken]').length > 0) {
                    options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
                }
                var index = parent.layer.load(0, {
                    shade: [0.5, '#000'], //0.1透明度的背景
                });
                $.ajax({
                    url: options.url,
                    data: options.param,
                    type: "post",
                    dataType: "json",
                    success: function (data) {
                        if (data.state == "success") {
                            options.success(data);
                            obj.modalMsg(data.message, data.state);
                            if (options.close) {
                                try {
                                    obj.modalClose();
                                }
                                catch(err){
                                    parent.layer.close(index);
                                    miniTab.deleteCurrentByIframe();
                                }

                            }
                        } else {
                            obj.modalAlert(data.message, data.state);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        parent.layer.close(index);
                        obj.modalAlert(errorThrown, "error");
                    },
                    beforeSend: function () {
                    },
                    complete: function () {
                        parent.layer.close(index);
                    }
                });
                return false;
            }, 500);
        },
        //二次确认框
        modalConfirm: function (content, callBack) {
            top.layer.confirm(content, {
                icon: "fa-exclamation-circle",
                title: "系统提示",
                btn: ['确认', '取消'],
                btnclass: ['btn btn-primary', 'btn btn-danger'],
            }, function () {
                callBack(true);
            }, function () {
                callBack(false)
            });
        },
        //文档弹窗
        modalPrompt: function (type, title, callBack) {
            if (type < 0 || type > 3) {
                type = 0;
            }
            top.layer.prompt({
                formType: type,
                value: '',
                title: title,
                btn: ['确认', '取消'],
                btnclass: ['btn btn-primary', 'btn btn-danger'],
            }, function (value, index, elem) {
                top.layer.close(index);
                callBack(true, value);
            });
        },
        //删除
        deleteForm: function (options) {
            var defaults = {
                prompt: "注：您确定要删除该项数据吗？",
                url: "",
                param: [],
                success: null,
                close: false
            };
            var options = $.extend(defaults, options);
            //ie缓存
            if (options.url.indexOf("?") >= 0) {
                options.url = options.url + '&v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            else {
                options.url = options.url + '?v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            if ($('[name=__RequestVerificationToken]').length > 0) {
                options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
            }
            obj.modalConfirm(options.prompt, function (r) {
                if (r) {
                    var index = parent.layer.load(0, {
                        shade: [0.5, '#000'], //0.1透明度的背景
                    });
                    window.setTimeout(function () {
                        $.ajax({
                            url: options.url,
                            data: options.param,
                            type: "post",
                            dataType: "json",
                            success: function (data) {
                                if (data.state == "success") {
                                    options.success(data);
                                    obj.modalMsg(data.message, data.state);
                                    if (options.close) {
                                        obj.modalClose();
                                    }
                                } else {
                                    obj.modalAlert(data.message, data.state);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                parent.layer.close(index);
                                obj.modalMsg(errorThrown, "error");
                            },
                            beforeSend: function () {
                            },
                            complete: function () {
                                parent.layer.close(index);
                            }
                        });
                    }, 500);
                }
            });

        },
        //post提交
        submitPost: function (options) {
            var defaults = {
                prompt: options.title,
                url: "",
                param: [],
                success: null,
                close: false
            };
            var options = $.extend(defaults, options);
            //ie缓存
            if (options.url.indexOf("?") >= 0) {
                options.url = options.url + '&v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            else {
                options.url = options.url + '?v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            if ($('[name=__RequestVerificationToken]').length > 0) {
                options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
            }
            obj.modalConfirm(options.prompt, function (r) {
                if (r) {
                    var index = parent.layer.load(0, {
                        shade: [0.5, '#000'], //0.1透明度的背景
                    });
                    window.setTimeout(function () {
                        $.ajax({
                            url: options.url,
                            data: options.param,
                            type: "post",
                            dataType: "json",
                            success: function (data) {
                                if (data.state == "success") {
                                    options.success(data);
                                    obj.modalMsg(data.message, data.state);
                                    if (options.close) {
                                        obj.modalClose();
                                    }
                                } else {
                                    obj.modalAlert(data.message, data.state);
                                }
       
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                parent.layer.close(index);
                                obj.modalMsg(errorThrown, "error");
                            },
                            beforeSend: function () {
                            },
                            complete: function () {
                                parent.layer.close(index);
                            }
                        });
                    }, 500);
                }
                else {
                    layer.close(index);
                }
            });

        },
        //Form序列化方法
        val: function (filter, formdate){
            var element = $('div[lay-filter=' + filter + ']');
            if (!!formdate) {
                for (var key in formdate) {
                    var $id = element.find('#' + key);
                    var value = $.trim(formdate[key]).replace(/&nbsp;/g, '');
                    var type = $id.attr('type');
                    if ($id.hasClass("select2-hidden-accessible")) {
                        type = "select";
                    }
                    switch (type) {
                        case "checkbox":
                            if (value == "true") {
                                $id.attr("checked", 'checked');
                            } else {
                                $id.removeAttr("checked");
                            }
                            break;
                        case "select":
                            $id.val(value).trigger("change");
                            break;
                        default:
                            $id.val(value);
                            break;
                    }
                };
                return false;
            }
            var postdata = {};
            element.find('input,select,textarea').each(function (r) {
                var $this = $(this);
                var id = $this.attr('id');
                var type = $this.attr('type');
                switch (type) {
                    case "checkbox":
                        postdata[id] = $this.is(":checked");
                        break;
                    case "radio":
                        postdata[id] = $this.is(":checked");
                        break;
                    default:
                        var value = $this.val() == "" ? "&nbsp;" : $this.val();
                        if (!$.request("keyValue")) {
                            value = value.replace(/&nbsp;/g, '');
                        }
                        postdata[id] = value;
                        break;
                }
            });
            if ($('[name=__RequestVerificationToken]').length > 0) {
                postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
            }
            return postdata;
        },
        //父窗体刷新（按钮刷新）
        parentreload: function (filter) {
            parent.$('button[lay-filter="' + filter+'"]').click();//按钮刷新
        },
        //当前页刷新（按钮刷新）
        reload: function (filter) {
            $('button[lay-filter="' + filter + '"]').click();//按钮刷新
        },
        //下载方法
        download : function (url, data, method) {
            if (url && data) {
                data = typeof data == 'string' ? data : jQuery.param(data);
                var inputs = '';
                $.each(data.split('&'), function () {
                    var pair = this.split('=');
                    inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
                });
                $('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
            };
        },
        //设置Form只读
        setReadOnly: function (filter) {
            var readForm = layui.$('[lay-filter="' + filter + '"]');
            readForm.find('input,textarea,select').prop('disabled', true);
            readForm.find('.layui-layedit iframe').contents().find('body').prop('contenteditable', false);
        },
        //权限按钮
        authorizeButton : function (id) {
            var moduleId = top.$(".layui-tab-title>.layui-this").attr("lay-id");
            //var moduleId = top.$("layuiminiTabInfo").attr("id").substr(6);
            
            var dataJson = top.clients.authorizeButton[moduleId.split("?")[0]];
            var $element = $('#' + id);
            $element.find('button[authorize=yes]').attr('authorize', 'no');
            if (dataJson != undefined) {
                $.each(dataJson, function (i) {
                    $element.find("#" + dataJson[i].F_EnCode).attr('authorize', 'yes');
                    $element.find("#" + dataJson[i].F_EnCode).removeClass('layui-hide');
                });
            }
            $element.find("[authorize=no]").parents('button').prev('.split').remove();
            $element.find("[authorize=no]").parents('button').remove();
            $element.find('[authorize=no]').remove();
        },
        //权限字段
        authorizeFields: function (filter) {
            var moduleId = top.$(".layui-tab-title>.layui-this").attr("lay-id");
            var element = $('div[lay-filter=' + filter + ']');
            if (!top.clients.moduleFields[moduleId.split("?")[0]] && top.clients.moduleFields[moduleId.split("?")[0]] == false) {
                var dataJson = top.clients.authorizeFields[moduleId.split("?")[0]];
                element.find('input,select,textarea').each(function (r) {
                    var $this = $(this);
                    var id = $this.attr('id');
                    $this.addClass('layui-hide');
                    if (dataJson != undefined) {
                        for (var i = 0; i < dataJson.length; i++) {
                            if (id == dataJson[i].F_EnCode) {
                                $this.parent().parent().removeClass('layui-hide');
                                $this.removeClass('layui-hide');
                                break;
                            }
                        }
                        //dataJson.find(item => {
                        //    if (id == item.F_EnCode) {
                        //        $this.parent().parent().removeClass('layui-hide');
                        //        $this.removeClass('layui-hide');
                        //    }
                        //});
                    }
                });
            }
            else {
                element.find('input,select,textarea').each(function (r) {
                    var $this = $(this);
                    $this.parent().parent().removeClass('layui-hide');
                });
            }
        },
        //iframe定时器方法
        iframeInterval:function(func, time){
            console.log("启动定时器", func, time);
            //点击iframe对应的标签则直接执行定时器方法。不过注意，这里我默认只执行最后一个定时器方法，如果有多个定时器请自行更改。
            top.$("li[lay-id='" + $(self.frameElement).attr("src") + "']").unbind('click').click(function () {
                console.log(func, "方法调用");
                func.call();
            });
            return setInterval(function () {
                console.log("定时器调用");
                if ($(self.frameElement.parentElement).hasClass("layui-show")) {
                    //判断所在的页面是否显示
                    console.log(func, "方法调用");
                    console.log("定时器间隔" + time);
                    func.call();
                }
            }, time);
        },
        multipleSelectRender: function (options) {
            var defaults = {
                filterable: true,//搜索模式
                autoRow: true,//自动换行
                empty: '没有数据!',//空数据提示
                toolbar: { show: true },//工具栏
                remoteSearch: true,//远程搜索
                paging: true,//分页
                pageSize: 5,//分页大小
                direction: 'auto',//下拉方向
                data: [],
                prop: {
                    name: 'text',
                    value: 'id',
                },
                remoteMethod: function (val, cb, show) {
                    //远程数据方法
                    //val 搜索参数
                    //cb data的值
                    //show 执行完显示值
                }
            };
            var options = $.extend(defaults, options);
            return xmSelect.render(options);
        },
        radioSelectRender: function (options) {
            var defaults = {
                filterable: true,//搜索模式
                toolbar: { show: true, list: ["CLEAR"] },//工具栏
                empty: '没有数据!',//空数据提示
                remoteSearch: true,//远程搜索
                paging: true,//分页
                pageSize: 5,//分页大小
                data: [],
                radio: true,//单选
                clickClose: true,//选择关闭
                direction: 'auto',//下拉方向
                prop: {
                    name: 'text',
                    value: 'id',
                },
                model: {
                    icon: 'hidden',//图标隐藏
                    label: {
                        type: 'text'//显示
                    }
                },
                remoteMethod: function (val, cb, show) {
                    //远程数据方法
                    //val 搜索参数
                    //cb data的值
                    //show 执行完显示值
                }
            };
            var options = $.extend(defaults, options);
            return xmSelect.render(options);
        },
        ajax: function (options) {
            var defaults = {
                dataType: "json",
                async: false,
                type: "GET"
            };
            var options = $.extend(defaults, options);
            //ie缓存问题
            if (options.url.indexOf("?") >= 0) {
                options.url = options.url + '&v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            else {
                options.url = options.url + '?v=' + new Date().Format("yyyy-MM-dd hh:mm:ss");
            }
            return $.ajax(options);
        }
    }
    exports("common", obj);
});