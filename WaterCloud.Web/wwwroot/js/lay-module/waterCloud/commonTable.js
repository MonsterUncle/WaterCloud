/**
 * date:2020/09/29
 * author:Mr.Q
 * version:1.6
 * description:watercloud 主体框架扩展
 */
layui.define(["jquery", "layer", 'table', 'soulTable','common'], function (exports) {
    var $ = layui.jquery,
        layer = layui.layer,     
        soulTable = layui.soulTable,       
        common = layui.common,       
        table = layui.table;

    var obj = {
        //table渲染封装里面有字段权限
        rendertable: function (options) {
            var defaults = {
                elem: '#currentTableId',//主键
                toolbar: '#toolbarDemo',//工具栏
                defaultToolbar: ['filter', 'exports', 'print'],//默认工具栏
                method: 'get',//请求方法
                cellMinWidth: 100,//最小宽度
                limit: 10,//每页数据 默认
                limits: [10, 20, 30, 40, 50],
                id:'currentTableId',
                height: 'full-110',
                loading: false,
                page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
                    layout: ['skip', 'prev', 'page', 'next', 'limit', 'count'] //自定义分页布局
                    ,curr: 1 //设定初始在第 1 页
                    , groups: 3 //只显示 1 个连续页码
                    , first: false //不显示首页
                    , last: false //不显示尾页
                },
                authorizeFields: true, // 字段权限开关
                autoColumnWidth: true,
                overflow: { // 默认所有表格都超出
                    type: 'tips'
                    , hoverTime: 300 // 悬停时间，单位ms, 悬停 hoverTime 后才会显示，默认为 0
                    , color: 'black' // 字体颜色
                    , bgColor: 'white' // 背景色
                    , header: true, // 表头支持 overflow
                    total: true // 合计行支持 overflow
                },
                contextmenu: {
                    header: [
                        {
                            name: '复制',
                            icon: 'layui-icon layui-icon-template',
                            click: function (obj) {
                                soulTable.copy(obj.text)
                                layer.msg('复制成功！')
                            }
                        },
                        {
                            name: '导出excel',
                            icon: 'layui-icon layui-icon-download-circle',
                            click: function () {
                                soulTable.export(this.id)
                            }
                        },
                        {
                            name: '重载表格',
                            icon: 'layui-icon layui-icon-refresh-1',
                            click: function () {
                                table.reload(this.id)
                            }
                        }
                    ],
                    // 表格内容右键菜单配置
                    body: [
                        {
                            name: '复制',
                            icon: 'layui-icon layui-icon-template',
                            click: function (obj) {
                                soulTable.copy(obj.text)
                                layer.msg('复制成功！')
                            }
                        }
                    ],
                    // 合计栏右键菜单配置
                    total: [{
                        name: '复制',
                        icon: 'layui-icon layui-icon-template',
                        click: function (obj) {
                            soulTable.copy(obj.text)
                            layer.msg('复制成功！')
                        }
                    }]
                },
                excel: {
                    filename: '表格信息' + new Date().formatDate() + '.xlsx',
                },
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
                }
            };
            var doneCallback = options.done;
            var options = $.extend(defaults, options);
            //ie缓存问题
            options.url = common.urlAddTime(options.url);
            //字段权限
            if (options.authorizeFields) {
                options.cols = common.tableAuthorizeFields(options.cols);
            }
            options.done = function (res, curr, count) {
                //关闭加载
                //layer.closeAll('loading');
                //固定列引发的问题
                //table.resize(options.id);
                //obj.tableResize(options.id);
                if (doneCallback) {
                    doneCallback(res, curr, count);
                }
                soulTable.render(this)
            };
            return table.render(options);
        },
        //table刷新
        reloadtable: function (options) {
            layer.load(0, { shade: false });
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
                        layout: ['skip', 'prev', 'page', 'next', 'limit', 'count'] //自定义分页布局
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
    }
    exports("commonTable", obj);
});