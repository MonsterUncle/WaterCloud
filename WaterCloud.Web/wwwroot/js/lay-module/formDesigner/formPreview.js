/**
 +------------------------------------------------------------------------------------+
 + ayq-layui-form-designer(layui表单设计器)
 +------------------------------------------------------------------------------------+
 + ayq-layui-form-designer v1.1.6
 * MIT License By http://116.62.237.101:8009/
 + 作者：谁家没一个小强
 + 官方：
 + 时间：2021-06-23
 +------------------------------------------------------------------------------------+
 + 版权声明：该版权完全归谁家没一个小强所有，可转载使用和学习，但请务必保留版权信息
 +------------------------------------------------------------------------------------+
 + 本项目是一个基于layui表单组件的表单设计器
 + 1.本项目基于Layui、Jquery、Sortable
 + 2.项目已经基本实现了拖动布局，父子布局
 + 3.项目实现了大部分基于Layui的Form表单控件布局，包括输入框、编辑器、下拉、单选、单选组、多选组、日期、滑块、评分、轮播、图片、颜色选择、图片上传、文件上传
 + 4.表单数据的获取与回显,禁用全表单
 +------------------------------------------------------------------------------------+
 */
layui.define(['layer', 'laytpl', 'element', 'form', 'slider', 'laydate', 'rate', 'colorpicker', 'layedit', 'carousel', 'upload', 'formField',"numberInput","iconPicker", "cron","labelGeneration"]
    , function (exports) {
        var $ = layui.jquery
            , layer = layui.layer
            , laytpl = layui.laytpl
            , setter = layui.cache
            , element = layui.element
            , slider = layui.slider
            , laydate = layui.laydate
            , rate = layui.rate
            , colorpicker = layui.colorpicker
            , carousel = layui.carousel
            , form = layui.form
            , upload = layui.upload
            , layedit = layui.layedit
            , formField = layui.formField
            , hint = layui.hint
            , numberInput = layui.numberInput
            , iconPicker = layui.iconPicker
            , cron = layui.cron
            , labelGeneration = layui.labelGeneration
            , iceEditorObjects = {}
            , labelGenerationObjects = {}
            , signObjects = {}
            , files = []
            , images = []
            , guid = function () {
                var d = new Date().getTime();
                if (window.performance && typeof window.performance.now === "function") {
                    d += performance.now(); //use high-precision timer if available
                }
                var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = (d + Math.random() * 16) % 16 | 0;
                    d = Math.floor(d / 16);
                    return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
                });
                return uuid;
            }
            , lang = {
                id: "标识",
                label: "标题",
                index: "序号",
                tag: "表单类型",
                tagIcon: '图标',
                width: '宽度',
                height: "高度",
                span: '网格宽度',
                placeholder: "placeholder",
                defaultValue: "默认值",
                labelWidth: "文本宽度",
                clearable: "是否清楚",
                prepend: "前缀",
                append: "",
                prefixIcon: '前缀图标',
                suffixIcon: '后缀图标',
                maxlength: "最大长度",
                showWordLimit: "是否限制字符",
                readonly: "只读",
                disabled: "禁用",
                required: "必填",
                columns: "列数",
                options: "选项",
                switchValue: "默认值",
                maxValue: "最大值",
                minValue: "最小值",
                stepValue: "步长",
                datetype: "日期类型",
                dateformat: "日期格式",
                half: "显示半星",
                theme: "皮肤",
                rateLength: "星星个数",
                interval: "间隔毫秒",
                autoplay: "自动播放",
                startIndex: "开始位置",
                full: "是否全屏",
                arrow: "鼠标样式",
                contents: "内容",
                document: '帮助文档',
                input: "输入框",
                select: "下拉",
                checkbox: "多选组",
                radio: "单选组",
                date: "日期",
                editor: "编辑器",
                slider: "滑块",
                image: "图片",
                grid: "一行多列",
                colorpicker: "颜色选择器",
                textarea: "多行文本",
                rate: "评分控件",
                switch: "开关",
                password: "密码框",
                carousel: "轮播",
                uploadUrl: "上传路径",
                urlPrefix: "上传前缀",
                uploadData: '上传参数',
                expression: "验证",
                file: "文件",
                numberInput:"排序文本框",
                iconPicker:"图标选择器",
                cron:"Cron表达式",
                cronUrl:"运行路径",
                bottom:"按钮组件",
            }
            , MOD_NAME = 'formPreview'
            , ELEM = '.layui-form-designer'
            , TPL_SUBMIT = ['<div class="layui-form-item">'
                , '<div class="layui-input-block">'
                , '<button type="submit" class="layui-btn" lay-submit="" lay-filter="demo1">提交</button>'
                , '<button type="reset" class="layui-btn layui-btn-primary">重置</button>'
                , '</div>'
                , '</div>'].join('')
            //外部接口

            , formPreview = {
                index: layui.formPreview ? (layui.formPreview.index + 10000) : 0

                //设置全局项

                , set: function (options) {
                    var that = this;
                    that.config = $.extend({}
                        , that.config
                        , options);
                    return that;
                }

                //事件监听

                , on: function (events
                , callback) {
                    return layui.onevent.call(this
                        , MOD_NAME
                        , events
                        , callback);
                }
            }
            /**
             * 操作当前实例
             * id 表示当前实例的索引 默认就是内部的 index，如果id存在值 那么就从已经存在的获取
             */

            , thisIns = function () {
                var that = this
                    , options = that.config;
                return { reload: function (options) {
                        that.reload.call(that
                            , options);
                    },getOptions:function () {
                        return options || null;
                    },getData : function () {
                        return options.data || null;
                    },geticeEditorObjects:function () {
                        return iceEditorObjects || null;
                    },getImages:function () {
                        return images || null;
                    },getFiles:function () {
                        return files || null;
                    },getFormData:function () {
                        return that.getFormData() || null;
                    },setFormData:function (json) {
                        return that.setFormData(json) || null;
                    },globalDisable:function () {
                        return that.globalDisable() || null;
                    },globalNoDisable:function () {
                        return that.globalNoDisable() || null;
                    },

                }
            }

            , getThisInsConfig = function (id) {
                var config = thisIns.config[id];
                if (!config) {
                    hint.error('The ID option was not found in the table instance');
                }
                return config || null;
            }

            , Class = function (options) {
                var that = this;
                that.index = ++formPreview.index; //增加实例，index 也是要增加
                that.config = $.extend({}
                    , that.config
                    , formPreview.config
                    , options);
                that.render();

            };


        /* 此方法最后一道 commom.js 中 */
        String.prototype.format = function (args) {
            var result = this;
            if (arguments.length > 0) {
                if (arguments.length == 1 && typeof (args) == "object") {
                    for (var key in args) {
                        if (args[key] != undefined) {
                            var reg = new RegExp("({" + key + "})"
                                , "g");
                            result = result.replace(reg
                                , args[key]);
                        }
                    }
                } else {
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i] != undefined) {
                            var reg = new RegExp("({[" + i + "]})"
                                , "g");
                            result = result.replace(reg
                                , arguments[i]);
                        }
                    }
                }
            }
            return result;
        }

        Class.prototype.config = {
            version: "1.0.0"
            , formName: "表单示例"
            , Author: "谁家没一个小强"
            , formId: "id"
            , generateId: 0
            , data: []
            , formData:{}
            , selectItem: undefined
        };

        /* 自动生成ID 当前页面自动排序*/
        Class.prototype.autoId = function (tag) {
            var that = this,
                options = that.config;
            options.generateId = options.generateId + 1;
            return tag + '_' + options.generateId;
        };

        /* 组件定义 */
        Class.prototype.components = {
            input: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _readonly = json.readonly ? 'readonly=""' : '';
                    var _required = json.required ? 'required' : '';
                    if (json.expression !== null && json.expression !== undefined ) {
                        if (json.expression !== '') {
                            _required = _required + '|' + json.expression;
                        }
                    }
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width: {2}px;">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="margin-left: {0}px">'.format(json.labelWidth);
                    _html += '<input name="{0}" value="{1}" placeholder="{3}" class="layui-input{7}" lay-vertype="tips" lay-verify="{6}" {4} {5} style="width:{2}">'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.width, json.placeholder, _readonly, _disabled, _required, _disabledClass);
                    _html += '</div>';
                    // if(selected){
                    // 	_html +='<div class="widget-view-action"><i class="layui-icon layui-icon-file"></i><i class="layui-icon layui-icon-delete"></i></div><div class="widget-view-drag"><i class="layui-icon layui-icon-screen-full"></i></div>';
                    // }
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.input));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;
                }
            },
            numberInput: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _required = json.required ? 'required' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label" style="width:{1}px;">{0}:</label>'.format( json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="width:calc({0} - {1}px);margin-left: {1}px;">'.format(json.width,json.labelWidth);
                    _html += '<input name="{0}" id="{9}" value="{1}" placeholder="{3}" class="layui-input{5}" lay-vertype="tips" min="{6}" max="{7}" step="{8}"  {4} style="width:{2}">'
                        .format(json.id, json.defaultValue ? json.defaultValue : '0', json.width, json.placeholder, _disabled , _disabledClass,json.minValue,json.maxValue,json.stepValue,json.tag + json.id);
                    _html += '</div>';
                    // if(selected){
                    // 	_html +='<div class="widget-view-action"><i class="layui-icon layui-icon-file"></i><i class="layui-icon layui-icon-delete"></i></div><div class="widget-view-drag"><i class="layui-icon layui-icon-screen-full"></i></div>';
                    // }
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.numberInput));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;
                }
            },
            labelGeneration: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label" style="width: {1}px">{0}:</label>'.format(json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="width:calc({0} - {1}px);margin-left: {1}px;">'.format(json.width,json.labelWidth);
                    if (json.disabled) {
                        _html += '<div class="iceEditor-disabled"></div>';
                    }
                    _html += '<div id="{0}" style="width: {1}"></div>'.format(json.tag + json.id,json.width);
                    _html += '</div>';
                    // if(selected){
                    // 	_html +='<div class="widget-view-action"><i class="layui-icon layui-icon-file"></i><i class="layui-icon layui-icon-delete"></i></div><div class="widget-view-drag"><i class="layui-icon layui-icon-screen-full"></i></div>';
                    // }
                    _html += '</div>';
                    return _html;
                },
                update: function (json,value) {
                    $('#' + json.id + ' .layui-input-block').empty();
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<div id="{0}"></div>'.format(json.tag + json.id);
                    $('#' + json.id + ' .layui-input-block').append(_html);
                    var labelGenerationObject = labelGeneration.render({
                        elem:'#' + json.tag + json.id,
                        data:value,
                        isEnter:json.isEnter
                    });
                    labelGenerationObjects[json.id] = labelGenerationObject;
                    if (json.disabled) {
                        $("#" + json.id).find(".layui-input-block").append('<div class="iceEditor-disabled"></div>');
                    } else {
                        $("#" + json.id).find(".iceEditor-disabled").remove();
                    }
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.labelGeneration));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;
                },
            },
            iconPicker: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _required = json.required ? 'required' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width: {2}px">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="width:calc({0} - {1}px);margin-left: {1}px;">'.format(json.width,json.labelWidth);
                    _html += '<input type="cronExpression" name="{0}" id="{5}" value="{1}" lay-verify="{6}" placeholder="{2}" class="layui-input{4}" lay-filter="iconPicker" lay-vertype="tips" {3}>'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.placeholder, _disabled, _disabledClass,json.tag + json.id,_required);
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                }, update: function (json,value) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    $('#' + json.id + ' .layui-input-block').empty();
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<input name="{0}" value="{1}" id="{6}" placeholder="{3}" class="layui-input{5}" lay-filter="{6}" lay-vertype="tips" {4} style="width:{2}">'
                        .format(json.id, value ? value : '', json.width, json.placeholder, _disabled, _disabledClass,json.tag + json.id);
                    $('#' + json.id + ' .layui-input-block').append(_html);
                    iconPicker.render({
                        // 选择器，推荐使用input
                        elem: '#' + json.tag + json.id,
                        // 数据类型：fontClass/unicode，推荐使用fontClass
                        type: 'fontClass',
                        // 是否开启搜索：true/false，默认true
                        search: json.iconPickerSearch,
                        // 是否开启分页：true/false，默认true
                        page: json.iconPickerPage,
                        // 每页显示数量，默认12
                        limit: json.iconPickerLimit,
                        // 每个图标格子的宽度：'43px'或'20%'
                        cellWidth: json.iconPickerCellWidth,
                        // 点击回调
                        click: function (data) {
                            //console.log(data);
                        },
                        // 渲染成功后的回调
                        success: function(d) {
                            //console.log(d);
                        }
                    });
                    iconPicker.checkIcon(json.tag + json.id, value);
                    if (json.disabled) {
                        $("#" + json.id).find(".layui-input-block").append('<div class="iceEditor-disabled"></div>');
                    } else {
                        $("#" + json.id).find(".iceEditor-disabled").remove();
                    }
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.iconPicker));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;
                },
            },
            cron: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _required = json.required ? 'required' : '';
                    var _width = json.width.replace(/[^\d]/g,'');
                    if(''!=_width){
                        _width = 100 - parseInt(_width);
                    }
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width: {2}px">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.required ? '*' : '',json.labelWidth);
                    _html += '<div class="layui-input-block" style="width:calc({0} - {1}px);margin-left: {1}px;">'.format(json.width,json.labelWidth);
                    _html += '<input type="cronExpression" name="{0}" id="{5}" value="{1}" lay-verify="{6}" placeholder="{2}" class="layui-input{4}" lay-filter="iconPicker" lay-vertype="tips" {3}>'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.placeholder, _disabled, _disabledClass,json.tag + json.id,_required);
                    if (!json.disabled) {
                        _html += '<button id="{0}-button" style="position: absolute;top: 0;right: 0px;cursor: pointer;" type="button" class="layui-btn">生成</button>'.format(json.tag + json.id);
                    }
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                update: function (json,value) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    $('#' + json.id + ' .layui-input-block').empty();
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<input name="{0}" value="{1}" id="{6}" placeholder="{3}" class="layui-input{5}" lay-filter="iconPicker" lay-vertype="tips" {4} style="width:{2}">'
                        .format(json.id, value ? value : '', json.width, json.placeholder, _disabled, _disabledClass,json.tag + json.id);
                    if (!json.disabled) {
                        var _width = json.width.replace(/[^\d]/g,'');
                        if(''!=_width){
                            _width = 100 - parseInt(_width);
                        }
                        _html += '<button id="{0}-button" style="position: absolute;top: 0;right: {1}%;cursor: pointer;" type="button" class="layui-btn">生成</button>'.format(json.tag + json.id,_width);
                        $('#' + json.id + ' .layui-input-block').append(_html);
                        cron.render({
                            elem: "#" + json.tag + json.id + "-button", // 绑定元素
                            url: json.cronUrl, // 获取最近运行时间的接口
                            value: value, // 默认值
                            done: function (cronStr) {
                                $("#" + json.tag + json.id).val(cronStr);
                            },
                        });
                    } else {
                        $('#' + json.id + ' .layui-input-block').append(_html);
                    }
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.cron));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;
                },
            },
            password: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _readonly = json.readonly ? 'readonly=""' : '';
                    var _required = json.required ? 'lay-verify="required"' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width: {2}px">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="margin-left: {0}px">'.format(json.labelWidth);
                    _html += '<input type="password" name="{0}" placeholder="{3}" value="{1}" autocomplete="off" style="width:{2}" {4}  {5} {6} class="layui-input{7}">'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.width, json.placeholder, _readonly, _disabled, _required, _disabledClass);
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.password));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            select: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _required = json.required ? 'required' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width: {2}px">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="width:calc({0} - {1}px);margin-left: {1}px;">'.format(json.width,json.labelWidth);
                    _html += '<select name="{0}" lay-verify="{3}" style="width:{1}" {2}>'.format(json.id, json.width, _disabled,_required);
                    /* if (json.defaultValue === undefined) {
                         _html += '<option value="{0}" checked>{1}</option>'.format('', '请选择');
                     }*/
                    for (var i = 0; i < json.options.length; i++) {
                        if (json.options[i].checked) {
                            _html += '<option value="{0}" selected="">{1}</option>'.format(json.options[i].value, json.options[i].text);
                        } else {
                            _html += '<option value="{0}">{1}</option>'.format(json.options[i].value, json.options[i].text);
                        }
                    }
                    _html += '</select>'
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.select));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;
                }
            },
            radio: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label" style="width: {1}px;">{0}:</label>'.format(json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="margin-left: {0}px">'.format(json.labelWidth);
                    for (var i = 0; i < json.options.length; i++) {
                        if (json.options[i].checked) {
                            _html += '<input type="radio" name="{0}" value="{1}" title="{2}" checked="" {3}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled);
                        } else {
                            _html += '<input type="radio" name="{0}" value="{1}" title="{2}" {3}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled);
                        }
                    }
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.radio));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            checkbox: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _required = json.required ? 'lay-verify="otherReq"' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width: {2}px">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="width:calc({0} - {1}px);margin-left: {1}px;">'.format(json.width,json.labelWidth);
                    for (var i = 0; i < json.options.length; i++) {
                        if (json.options[i].checked) {
                            _html += '<input type="checkbox" name="{0}[{1}]" title="{2}" checked="" {3} {4}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled,_required);
                        } else {
                            _html += '<input type="checkbox" name="{0}[{1}]" title="{2}" {3} {4}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled,_required);
                        }
                    }
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _required = json.required ? 'lay-verify="otherReq"' : '';
                    $('#' + json.id + ' .layui-input-block').empty();
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    for (var i = 0; i < json.options.length; i++) {
                        _html += '<input type="checkbox" name="{0}[{1}]" title="{2}" {3} {4}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled,_required);
                    }
                    $('#' + json.id + ' .layui-input-block').append(_html);
                    form.render('checkbox');
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.checkbox));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            switch: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label" style="width: {1}px;">{0}:</label>'.format(json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="width:calc({0} - {1}px);border: 1px solid #d2d2d2;border-left: 0px;margin-left: {1}px">'.format(json.width,json.labelWidth);
                    _html += '<input type="checkbox" name="{0}" lay-skin="switch" lay-text="ON|OFF" {1} class="{2}" {3}>'.format(json.id, _disabled, _disabledClass, json.switchValue ? 'checked' : '');
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.switch));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            slider: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label" style="width: {1}px;">{0}:</label>'.format(json.label,json.labelWidth);
                    _html += '<div class="layui-input-block layui-form" style="width:calc({0} - {1}px);margin-left: {1}px">'.format(json.width,json.labelWidth);
                    _html += '<div id="{0}" class="widget-slider"></div>'.format(json.tag + json.id);
                    _html += '<input name="{0}" type="hidden" value="{1}"></input>'.format(json.id,json.defaultValue);
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                update: function (json,value) {
                    slider.render({
                        elem: '#' + json.tag + json.id,
                        value: value, //初始值
                        min: json.minValue,
                        max: json.maxValue,
                        step: json.stepValue,
                        disabled: json.disabled,
                        input:json.isInput,
                        change: function(_value){
                            $("#" + json.id).find("input[name=" + json.id + "]").val(_value);
                        }
                    });
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.slider));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            date: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _disabledStyle = json.disabled ? ' pointer-events: none;' : '';
                    var _required = json.required ? 'required' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width: {2}px;">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="width:calc({0} - {1}px);margin-left: {1}px;">'.format(json.width,json.labelWidth);
                    _html += '<input id="{0}" name="{0}" lay-verify="{3}" class="layui-input icon-date widget-date {1}" style="line-height: 40px;{2}"></input>'.format(json.tag + json.id,_disabledClass,_disabledStyle,_required);
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.date));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },bottom: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _coustomCss = "";
                    if (json.buttonSize === "layui-btn-lg") {
                        _coustomCss = "custom-lg";
                    }
                    if (json.buttonSize === "") {
                        _coustomCss = "custom-zc";
                    }
                    if (json.buttonSize === "layui-btn-sm") {
                        _coustomCss = "custom-sm";
                    }
                    if (json.buttonSize === "layui-btn-xs") {
                        _coustomCss = "custom-xs";
                    }
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    if (json.isLabel) {
                        _html += '<label class="layui-form-label" style="width: {1}px;">{0}:</label>'.format(json.label,json.labelWidth);
                    }
                    _html += '<div class="layui-input-block" style="margin-left: 0px;">';
                    if (json.disabled) {
                        _html += '<button id="{0}" type="button" class="layui-btn {1} layui-btn-disabled {2}"><i class="layui-icon {3}"></i> {4}</button>'.format(json.id + json.tag, json.buttonSize,_coustomCss ,json.buttonIcon,json.buttonVlaue);
                    }else {
                        _html += '<button id="{0}" type="button" class="layui-btn {1} {2} {3}"><i class="layui-icon {4}"></i> {5}</button>'.format(json.id + json.tag, json.buttonSize, json.buttonType,_coustomCss ,json.buttonIcon,json.buttonVlaue);
                    }
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.bottom));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;
                }
            },sign: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label" style="width: {1}px">{0}:</label>'.format(json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="margin-left: {0}px;">'.format(json.labelWidth);
                    if (json.disabled) {
                        _html += '<button id="{0}" type="button" class="layui-btn layui-btn-normal layui-btn-disabled custom-zc"><i class="layui-icon {1}"></i> {2}</button>'.format(json.id + json.tag ,json.buttonIcon,json.buttonVlaue);
                    }else {
                        _html += '<button id="{0}" type="button" class="layui-btn  layui-btn-normal custom-zc"><i class="layui-icon {1}"></i> {2}</button>'.format(json.id + json.tag ,json.buttonIcon,json.buttonVlaue);
                    }
                    if (json.data !== "") {
                        _html += '<div class="signImg"><img src="{0}"></div>'.format(json.data);
                    }
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                update: function (json,value) {
                    $('#' + json.id + ' .layui-input-block').empty();
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    if (json.disabled) {
                        _html += '<button id="{0}" type="button" class="layui-btn layui-btn-normal layui-btn-disabled custom-zc"><i class="layui-icon {1}"></i> {2}</button>'.format(json.id + json.tag ,json.buttonIcon,json.buttonVlaue);
                    }else {
                        _html += '<button id="{0}" type="button" class="layui-btn  layui-btn-normal custom-zc"><i class="layui-icon {1}"></i> {2}</button>'.format(json.id + json.tag ,json.buttonIcon,json.buttonVlaue);
                    }
                    if (value !== "") {
                        _html += '<div class="signImg"><img src="{0}"></div>'.format(value);
                        signObjects[json.id] = value;
                    }
                    $('#' + json.id + ' .layui-input-block').append(_html);
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.sign));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;
                },
            },dateRange: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _disabledStyle = json.disabled ? ' pointer-events: none;' : '';
                    var _required = json.required ? 'required' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<div class="layui-inline">';
                    _html += '<label class="layui-form-label {0}" style="width: {2}px;">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-inline" id="{0}" style="line-height: 40px;{1}">'.format(json.tag + json.id,_disabledStyle);
                    _html += '<div class="layui-input-inline">';
                    _html += '<input id="start-{0}" lay-verify="{3}" name="start{2}" class="layui-input {1}" autocomplete="off" placeholder="开始日期"></input>'.format(json.tag + json.id,_disabledClass,json.id,_required);
                    _html += '</div>';
                    _html += '<div class="layui-form-mid">-</div>';
                    _html += '<div class="layui-input-inline">';
                    _html += '<input id="end-{0}" lay-verify="{3}" name="end{2}" class="layui-input {1}" autocomplete="off" placeholder="结束日期"></input>'.format(json.tag + json.id,_disabledClass,json.id,_required);
                    _html += '</div>';
                    _html += '</div>';
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.dateRange));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                },
            },
            rate: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _readonly = json.readonly ? 'readonly=""' : '';
                    var _required = json.required ? 'required=""' : '';
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width:{2}px;">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="margin-left: {0}px">'.format(json.labelWidth);
                    _html += '<div id="{0}" class="widget-rate"></div>'.format(json.tag + json.id);
                    _html += '<input name="{0}" type="hidden" value="{1}"></input>'.format(json.id,json.defaultValue);
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                update: function (json,value) {
                    rate.render({
                        elem: '#' + json.tag + json.id,
                        value: value,
                        text: json.text,
                        half: json.half,
                        length: json.rateLength,
                        readonly: json.readonly,
                        choose: function(_value){
                            $("#" + json.id).find("input[name=" + json.id + "]").val(_value);
                        }
                    });
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.rate));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            carousel: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    // _html +='<label class="layui-form-label {0}">{1}:</label>'.format(json.required?'layui-form-required':'',json.label);
                    // _html +='<div class="layui-input-block">';
                    _html += '<div class="layui-carousel" id="{0}">'.format(json.tag + json.id);
                    _html += '<div carousel-item>';
                    for (var i = 0; i < json.options.length; i++) {
                        _html += '<div><img src="{0}" /></div>'.format(json.options[i].value);
                    }
                    _html += '</div>';//end for div carousel-item
                    _html += '</div>';//end for class=layui-coarousel
                    // _html +='</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.carousel));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            colorpicker: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}" style="width: {2}px;">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="margin-left: {0}px">'.format(json.labelWidth);
                    if (json.disabled) {
                        _html += '<div class="iceEditor-disabled"></div>';
                    }
                    _html += '<div id="{0}" class="widget-rate"></div>'.format(json.tag + json.id);
                    _html += '<input name="{0}" type="hidden" value="{1}"></input>'.format(json.id,json.defaultValue);
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                update: function (json,value) {
                    if (json.disabled) {
                        $("#" + json.id).find(".layui-input-block").append('<div class="iceEditor-disabled"></div>');
                    } else {
                        $("#" + json.id).find(".iceEditor-disabled").remove();
                    }
                    colorpicker.render({
                        elem: '#' + json.tag + json.id
                        ,color: value
                        ,format: 'rgb'
                        ,predefine: true
                        ,alpha: true
                        ,done: function (color) {
                            $("#" + json.id).find("input[name=" + json.id + "]").val(color);
                        }
                    });
                    $("#" + json.id).find("input[name=" + json.id + "]").val(value);
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.colorpicker));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            image: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _html = '<div class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label);
                    _html += '<div class="layui-input-block">';

                    _html += '<div class="layui-upload">';
                    _html += '<button type="button" class="layui-btn" id="{0}">多图片上传</button>'.format(json.tag + json.id);
                    _html += '<input class="layui-hide" hidden type="text" id="{0}" lay-verify="{2}" name="{0}" value="{1}">'.format(json.id, json.defaultValue ? json.defaultValue : '', json.required ? 'required' : '');
                    _html += '<blockquote class="layui-elem-quote layui-quote-nm" style="margin-top: 10px;width: 88%">预览图：';
                    _html += '<div class="layui-upload-list uploader-list" style="overflow: auto;" id="uploader-list-{0}">'.format(json.id);
                    _html += '</div>';
                    _html += '</blockquote>';
                    _html += '</div>';


                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.image));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                }
            },
            textarea: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _required = json.required ? 'lay-verify="required"' : '';
                    var _readonly = json.readonly ? 'readonly=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _html = '<div id="{0}" class="layui-form-item layui-form-text {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label);
                    _html += '<div class="layui-input-block">';
                    _html += '<textarea name="{0}" placeholder="{3}" width="{2}" class="layui-textarea{6}" {4} {5} {7}>{1}</textarea>'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.width, json.placeholder, _disabled, _required, _disabledClass, _readonly);
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.textarea));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;

                }
            },
            editor: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _html = '<div id="{0}" class="layui-form-item layui-form-text {2}" style="width: {4}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index,json.width);
                    _html += '<label class="layui-form-label" style="width: {1}">{0}:</label>'.format(json.label,json.width);
                    _html += '<div class="layui-input-block">';
                    _html += '<div id="{0}"></div>'.format(json.tag + json.id);
                    _html += '</div>';
                    // if(selected){
                    // 	_html +='<div class="widget-view-action"><i class="layui-icon layui-icon-file"></i><i class="layui-icon layui-icon-delete"></i></div><div class="widget-view-drag"><i class="layui-icon layui-icon-screen-full"></i></div>';
                    // }
                    _html += '</div>';
                    return _html;
                },
                update: function (json,value) {
                    var e = iceEditorObjects[json.id];
                    e.setValue(value);
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.editor));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    return _json;

                }
            },
            grid: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _html = '<div id="{0}" class="layui-form-item layui-row grid {2}"  data-id="{0}" data-tag="{1}" data-index="{3}" >'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    var colClass = 'layui-col-md6';
                    if (json.columns.length == 3) {
                        colClass = 'layui-col-md4';
                    } else if (json.columns.length == 4) {
                        colClass = 'layui-col-md3';
                    } else if (json.columns.length == 6) {
                        colClass = 'layui-col-md2';
                    }
                    for (var i = 0; i < json.columns.length; i++) {
                        _html += '<div class="{2} widget-col-list column{3}{0}" data-index="{0}" data-parentindex="{1}">'.format(i, json.index, colClass,json.id);
                        //some html
                        _html += '</div>';
                    }

                    // if(selected){
                    // 	_html +='<div class="widget-view-action"><i class="layui-icon layui-icon-file"></i><i class="layui-icon layui-icon-delete"></i></div><div class="widget-view-drag"><i class="layui-icon layui-icon-screen-full"></i></div>';
                    // }
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 默认是一个一行两列的布局对象
                    var _json = JSON.parse(JSON.stringify(formField.grid));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
                    _json.index = index;
                    if (columncount > 2) {
                        var _col = {
                            span: 12,
                            list: [],
                        };
                        for (var i = _json.columns.length; i < columncount; i++) {
                            _json.columns.splice(i, 0, _col);
                        }
                    }
                    return _json;

                }
            },
            file: {
                /**
                 * 根据json对象生成html对象
                 * @param {object} json
                 * @param {boolean} selected true 表示选择当前
                 * */
                render: function (json, selected) {
                    if (selected === undefined) {
                        selected = false;
                    }
                    var _html = '<div class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label);
                    _html += '<div class="layui-input-block">';

                    _html += '<div class="layui-upload">';
                    _html += '<button type="button" class="layui-btn layui-btn-normal" id="{0}">选择多文件</button> '.format(json.tag + json.id);
                    _html += ' <div class="layui-upload-list" style="max-width: 1000px;"><table class="layui-table">';
                    _html += '<colgroup><col><col width="150"><col width="260"><col width="150"></colgroup>';
                    _html += '<thead><tr><th>文件名</th><th>大小</th><th>上传进度</th><th>操作</th></tr></thead>';
                    _html += '<tbody id="list-{0}"></tbody></table></div>'.format(json.tag + json.id);
                    _html += '<button type="button" class="layui-btn" id="listAction-{0}">开始上传</button>'.format(json.tag + json.id);
                    _html += '</div>';
                    _html += '</blockquote>';
                    _html += '</div>';
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.file));
                    _json.id = id == undefined ? guid() : id;
                    _json.index = index;
                    return _json;

                },
                /* 根据 json 对象显示对应的属性 */
                property: function (json) {
                    $('#columnProperty').empty();
                    var _html = '';
                    _html = renderCommonProperty(json);//获取通用属性HTML字符串
                    //处理特殊字符串
                    for (var key in json) {
                        if (key === 'index') {
                            continue;
                        }

                    }
                    $('#columnProperty').append(_html);
                }
            },

        };


        //渲染视图
        Class.prototype.render = function () {
            var that = this
                , options = that.config;
            options.elem = $(options.elem);
            options.id = options.id || options.elem.attr('id') || that.index;
            if (!options.formStyle) {
                options.elem.html('<div  class="layui-form" style="height:100%;" id="formPreviewForm" lay-filter="formPreviewForm"></div>');
            }
            else {
                options.elem.html('<div  class="layui-form layui-form-pane" style="height:100%;" id="formPreviewForm" lay-filter="formPreviewForm"></div>');
            }
            that.renderForm();
            if (options.readonly) {
                var readForm = $('#formPreviewForm');
                readForm.find('input,textarea,select').prop('disabled', true);
                readForm.find('input,textarea,select').removeAttr('lay-verify');
                readForm.find('.layui-layedit iframe').contents().find('body').prop('contenteditable', false);
            }
        };

        //获取表单数据回调
        Class.prototype.getFormData = function () {
            //获取表单区域所有值
            var json = form.val("formPreviewForm");
            delete json.file;
            for(let key  in iceEditorObjects){
                json[key] = iceEditorObjects[key].getHTML();
            }
            for(let key  in labelGenerationObjects){
                json[key] = labelGenerationObjects[key].getData();
            }
            for(let key  in signObjects){
                json[key] = signObjects[key];
            }
            return json;
        }

        /**
         * 找到当前节点
         */
        Class.prototype.findJsonItem = function (json, id) {
            var that = this,
                options = that.config;
            for (var i = 0; i < json.length; i++) {
                if (json[i].id === id) {
                    return json[i];
                } else {
                    if (json[i].tag === 'grid') {
                        for (var j = 0; j < json[i].columns.length; j++) {
                            if (json[i].columns[j].list.length > 0) {
                                var _item = that.findJsonItem(json[i].columns[j].list, id);
                                if (_item) {
                                    return _item;
                                }
                            }
                        }
                    }
                }
            }
        }

        //全局禁用
        Class.prototype.globalDisable = function () {
            var that = this
                , options = that.config;
            that.findItemToDisable(options.data);
            that.renderForm();
        }

        //全局取消禁用
        Class.prototype.globalNoDisable = function () {
            var that = this
                , options = that.config;
            that.findItemToNoDisable(options.data);
            that.renderForm();
        }

        //递归改变禁用属性
        Class.prototype.findItemToDisable = function (jsondata) {
            var that = this
                , options = that.config;
            $.each(jsondata, function (index, item) {
                if (item.tag === 'grid') {
                    $.each(item.columns, function (index2, item2) {
                        //获取当前的 DOM 对象
                        if (item2.list.length > 0) {
                            that.findItemToDisable(item2.list);
                        }
                    });
                } else {
                    item.disabled = true;
                    item.readonly = true;
                }
            });
        }

        //递归改变禁用属性
        Class.prototype.findItemToNoDisable = function (jsondata) {
            var that = this
                , options = that.config;
            $.each(jsondata, function (index, item) {
                if (item.tag === 'grid') {
                    $.each(item.columns, function (index2, item2) {
                        //获取当前的 DOM 对象
                        if (item2.list.length > 0) {
                            that.findItemToNoDisable(item2.list);
                        }
                    });
                } else {
                    item.disabled = false;
                    item.readonly = false;
                }
            });
        }


        //获取表单数据回调
        Class.prototype.setFormData = function (json) {
            var that = this,
                options = that.config;
            //获取表单区域所有值
            for(let key  in json){
                if (key.indexOf("[")!= -1 && key.indexOf("]")!= -1) {
                    var check = key.substring(0,key.indexOf("["));
                    var item = that.findJsonItem(options.data,check);
                    if (item === undefined) {
                        continue;
                    }
                    that.components[item.tag].update(item);
                    continue;
                }
                var item = that.findJsonItem(options.data,key);
                if (item === undefined) {
                    continue;
                }
                if (item.tag === 'editor' || item.tag === 'rate'
                    || item.tag === 'slider' || item.tag === 'labelGeneration'
                    || item.tag === 'sign' || item.tag === 'iconPicker'
                    || item.tag === 'cron' || item.tag === 'colorpicker') {
                    that.components[item.tag].update(item,json[key]);
                }
                //设值
                if (item.tag === 'image') {
                    $('#uploader-list-' + item.id).empty();
                    var imageList = json[key].split(',');
                    for (var i = 0; i < imageList.length; i++) {
                        $('#uploader-list-' + item.id).append(
                            '<div id="" class="file-iteme">' +
                            '<div class="handle"><i class="layui-icon layui-icon-delete"></i></div>' +
                            '<img style="width: 100px;height: 100px;" src=' + item.urlPrefix + imageList[i] + '>' +
                            '<div class="info">' + imageList[i] + '</div>' +
                            '</div>'
                        );
                    }
                }
            }
            form.val("formPreviewForm",json);
            options.formData = json;
            return json;
        }


        /* 递归渲染组件 */
        Class.prototype.renderComponents = function (jsondata, elem) {
            var that = this
                , options = that.config;
            $.each(jsondata, function (index, item) {
                item.index = index;//设置index 仅仅为了传递给render对象，如果存在下级子节点那么 子节点的也要变动
                if (options.selectItem === undefined) {
                    elem.append(that.components[item.tag].render(item, false));
                } else {
                    if (options.selectItem.id === item.id) {
                        elem.append(that.components[item.tag].render(item, true));
                        //显示当前的属性
                        that.components[item.tag].property(item);
                    } else {
                        elem.append(that.components[item.tag].render(item, false));
                    }
                }
                if (item.tag === 'grid') {
                    $.each(item.columns, function (index2, item2) {
                        //获取当前的 DOM 对象
                        if (item2.list.length > 0) {
                            var elem2 = $('#' + item.id + ' .widget-col-list.column' + item.id + index2);
                            that.renderComponents(item2.list, elem2);
                        }
                    });
                } else if (item.tag === 'slider') {
                    //定义初始值
                    slider.render({
                        elem: '#' + item.tag + item.id,
                        value: item.defaultValue, //初始值
                        min: item.minValue,
                        max: item.maxValue,
                        step: item.stepValue,
                        disabled: item.disabled,
                        input:item.isInput,
                        change: function(value){
                            $("#" + item.id).find("input[name=" + item.id + "]").val(value);
                        }
                    });
                } else if (item.tag === 'numberInput') {
                    //定义初始值
                    numberInput.render({
                        elem:'#' + item.tag + item.id
                    });
                    var _width = item.width.replace(/[^\d]/g,'');
                    if(''!=_width){
                        _width = 100 - parseInt(_width);
                    }
                    $('#' + item.id + ' .layui-input-block .layui-number-input-btn').css("right",_width + "%");
                    if (item.disabled) {
                        $('#' + item.id + ' .layui-input-block .layui-number-input-btn').css("z-index","-1");
                    }
                } else if (item.tag === 'checkbox') {
                    var bool = false;
                    //定义初始值
                    form.verify({
                        otherReq: function (value, item) {
                            var verifyName = $(item).attr('name'), verifyType = $(item).attr('type')
                                , formElem = $(item).parents(".layui-form"),
                                verifyElem = formElem.find("input[name='"+verifyName+"']"),
                                verifyElems = formElem.find("input")
                                , isTrue = verifyElem.is(":checked"),
                                focusElem = verifyElem.next().find("i.layui-icon");//焦点元素
                            for (let i = 0; i < verifyElems.length; i++) {
                                if (verifyElems[i].checked) {
                                    return false;
                                }
                            }
                            if (!isTrue || !value) {
                                focusElem.css(verifyType == "radio" ? {"color": "#FF5722"} : {"border-color": "#FF5722"});
                                //对非输入框设置焦点
                                focusElem.first().attr("tabIndex", "1").css("outline", "0").blur(function () {
                                    focusElem.css(verifyType == 'radio' ? {"color": ""} : {"border-color": ""});
                                }).focus();
                                return "必填项不能为空";
                            }
                        }
                    });
                } else if (item.tag === 'date') {
                    laydate.render({
                        elem: '#' + item.tag + item.id,
                        type: item.datetype,
                        format: item.dateformat,
                        value: item.dateDefaultValue,
                        min: item.dataMinValue,
                        max: item.dataMaxValue,
                    });
                } else if (item.tag === 'sign') {
                    $('#' + item.id + item.tag).click(function () {
                        layer.open({
                            type: 2,
                            title: '手写签名',
                            btn: ['保存','关闭'], //可以无限个按钮
                            yes: function(index, layero){
                                //do something
                                var iframe = window['layui-layer-iframe' + index];
                                var data = iframe.getCanvasData();
                                signObjects[item.id] = data;
                                item.data = data;
                                $('#' + item.id + ' .layui-input-block div').empty();
                                var _html = '<div class="signImg"><img src="{0}"></div>'.format(item.data);
                                $('#' + item.id + ' .layui-input-block').append(_html);
                                layer.close(index); //如果设定了yes回调，需进行手工关闭
                            },
                            btn2: function (index, layero) {
                                layer.close(index);
                            },
                            closeBtn: 1, //不显示关闭按钮
                            shade: [0],
                            area: ['60%', '60%'],
                            offset: 'auto', //右下角弹出
                            anim: 2,
                            content: ['./handwrittenSignature.html', 'yes'], //iframe的url，no代表不显示滚动条
                            success:function (layero,index) {
                            }
                        });
                    });
                } else if (item.tag === 'labelGeneration') {
                    var labelGenerationObject = labelGeneration.render({
                        elem:'#' + item.tag + item.id,
                        data:item.dateDefaultValue,
                        isEnter:item.isEnter
                    });
                    labelGenerationObjects[item.id] = labelGenerationObject;
                } else if (item.tag === 'cron' && !item.disabled) {
                    cron.render({
                        elem: "#" + item.tag + item.id + "-button", // 绑定元素
                        url: item.cronUrl, // 获取最近运行时间的接口
                        // value: $("#cron").val(), // 默认值
                        done: function (cronStr) {
                            $("#" + item.tag + item.id).val(cronStr);
                        },
                    });
                } else if (item.tag === 'iconPicker'){
                    iconPicker.render({
                        // 选择器，推荐使用input
                        elem: '#' + item.tag + item.id,
                        // 数据类型：fontClass/unicode，推荐使用fontClass
                        type: 'fontClass',
                        // 是否开启搜索：true/false，默认true
                        search: item.iconPickerSearch,
                        // 是否开启分页：true/false，默认true
                        page: item.iconPickerPage,
                        // 每页显示数量，默认12
                        limit: item.iconPickerLimit,
                        // 每个图标格子的宽度：'43px'或'20%'
                        cellWidth: item.iconPickerCellWidth,
                        // 点击回调
                        click: function (data) {
                            //console.log(data);
                        },
                        // 渲染成功后的回调
                        success: function(d) {
                            //console.log(d);
                        }
                    });
                    iconPicker.checkIcon(item.tag + item.id, item.defaultValue);
                } else if (item.tag === 'dateRange') {
                    laydate.render({
                        elem: '#' + item.tag + item.id,
                        type: item.datetype,
                        format: item.dateformat,
                        //value: item.dateDefaultValue,
                        min: item.dataMinValue,
                        max: item.dataMaxValue,
                        range: ['#start-' + item.tag + item.id, '#end-' + item.tag + item.id]
                    });
                    if (item.dateRangeDefaultValue !== null && item.dateRangeDefaultValue !== ""
                        && item.dateRangeDefaultValue !== undefined) {
                        var split = item.dateRangeDefaultValue.split(" - ");
                        $('#start-' + item.tag + item.id).val(split[0]);
                        $('#end-' + item.tag + item.id).val(split[1]);
                    }
                } else if (item.tag === 'rate') {
                    rate.render({
                        elem: '#' + item.tag + item.id,
                        value: item.defaultValue,
                        text: item.text,
                        half: item.half,
                        length: item.rateLength,
                        readonly: item.readonly,
                        choose: function(value){
                            $("#" + item.id).find("input[name=" + item.id + "]").val(value);
                        }
                    });
                } else if (item.tag === 'colorpicker') {
                    colorpicker.render({
                        elem: '#' + item.tag + item.id
                        ,color: item.defaultValue
                        ,format: 'rgb'
                        ,predefine: true
                        ,alpha: true
                        ,done: function (color) {
                            $("#" + item.id).find("input[name=" + item.id + "]").val(color);
                        }
                    });
                } else if (item.tag === 'editor') {
                    var e = new ice.editor(item.tag + item.id);
                    e.width=item.width;   //宽度
                    e.height=item.height;  //高度
                    e.uploadUrl = item.uploadUrl; //上传文件路径
                    e.uploadData = !!item.uploadData ? JSON.parse(item.uploadData) : null;//上传文件参数
                    e.disabled=item.disabled;
                    e.create();
                    iceEditorObjects[item.id] = e;
                } else if (item.tag === 'carousel') {
                    carousel.render({
                        elem: '#' + item.tag + item.id,
                        width: item.width,//设置容器宽度
                        height: item.height,//设置容器宽度
                        arrow: item.arrow, //始终显示箭头
                        interval: item.interval,
                        anim: item.anim,
                        autoplay: item.autoplay
                    });
                } else if (item.tag === 'image') {
                    let itemConfig = item;
                    upload.render({
                        elem: '#' + item.tag + item.id
                        , url: '' + item.uploadUrl + ''
                        , data: !!item.uploadData ? JSON.parse(item.uploadData) : null
                        , multiple: true
                        , before: function (obj) {
                            layer.msg('图片上传中...', {
                                icon: 16,
                                shade: 0.01,
                                time: 0
                            })
                        }
                        , done: function (res) {
                            var imageList = !!$('#' + itemConfig.id).val() ? $('#' + itemConfig.id).val().split(','):[];
                            layer.close(layer.msg());//关闭上传提示窗口
                            //上传完毕
                            if (Array.isArray(res.data)) {
                                for (var i = 0; i < res.data.length; i++) {
                                    $('#uploader-list-' + item.id).append(
                                        '<div id="" class="file-iteme">' +
                                        '<div class="handle"><i class="layui-icon layui-icon-delete"></i></div>' +
                                        '<img style="width: 100px;height: 100px;" src=' + itemConfig.urlPrefix + res.data[i].src + '>' +
                                        '<div class="info">' + res.data[i].title + '</div>' +
                                        '</div>'
                                    );
                                    imageList.push(res.data[i].src);
                                }
                            }
                            else {
                                $('#uploader-list-' + item.id).append(
                                    '<div id="" class="file-iteme">' +
                                    '<div class="handle"><i class="layui-icon layui-icon-delete"></i></div>' +
                                    '<img style="width: 100px;height: 100px;" src=' + itemConfig.urlPrefix + res.data.src + '>' +
                                    '<div class="info">' + res.data.title + '</div>' +
                                    '</div>'
                                );
                                imageList.push(res.data.src);
                            }
                            $('#' + itemConfig.id).val(imageList.join(','));
                        }
                    });
                } else if (item.tag === 'file') {
                    let itemConfig = item;
                    upload.render({
                        elem: '#' + item.tag + item.id
                        , elemList: $('#list-' + item.tag + item.id) //列表元素对象
                        , url: '' + item.uploadUrl + ''
                        , data: !!item.uploadData ? JSON.parse(item.uploadData) : null
                        , accept: 'file'
                        , multiple: true
                        , number: 3
                        , auto: false
                        , bindAction: '#listAction-' + item.tag + item.id
                        , choose: function (obj) {
                            var that = this;
                            var files = this.files = obj.pushFile(); //将每次选择的文件追加到文件队列
                            //读取本地文件
                            obj.preview(function (index, file, result) {
                                var tr = $(['<tr id="upload-' + index + '">'
                                    , '<td>' + file.name + '</td>'
                                    , '<td>' + (file.size / 1014).toFixed(1) + 'kb</td>'
                                    , '<td><div class="layui-progress" lay-filter="progress-demo-' + index + '"><div class="layui-progress-bar" lay-percent=""></div></div></td>'
                                    , '<td>'
                                    , '<button class="layui-btn layui-btn-xs demo-reload layui-hide">重传</button>'
                                    , '<button class="layui-btn layui-btn-xs layui-btn-danger demo-delete">删除</button>'
                                    , '</td>'
                                    , '</tr>'].join(''));

                                //单个重传
                                tr.find('.demo-reload').on('click', function () {
                                    obj.upload(index, file);
                                });

                                //删除
                                tr.find('.demo-delete').on('click', function () {
                                    delete files[index]; //删除对应的文件
                                    tr.remove();
                                    uploadListIns.config.elem.next()[0].value = ''; //清空 input file 值，以免删除后出现同名文件不可选
                                });

                                that.elemList.append(tr);
                                element.render('progress'); //渲染新加的进度条组件
                            });
                        }
                        , done: function (res, index, upload) { //成功的回调
                            var that = this;
                            //if(res.code == 0){ //上传成功
                            var tr = that.elemList.find('tr#upload-' + index)
                                , tds = tr.children();
                            tds.eq(3).html(''); //清空操作
                            delete this.files[index]; //删除文件队列已经上传成功的文件
                            return;
                            //}
                            this.error(index, upload);
                        }
                        , allDone: function (obj) { //多文件上传完毕后的状态回调
                            console.log(obj)
                        }
                        , error: function (index, upload) { //错误回调
                            var that = this;
                            var tr = that.elemList.find('tr#upload-' + index)
                                , tds = tr.children();
                            tds.eq(3).find('.demo-reload').removeClass('layui-hide'); //显示重传
                        }
                        , progress: function (n, elem, e, index) {
                            element.progress('progress-demo-' + index, n + '%'); //执行进度条。n 即为返回的进度百分比
                        }
                    });
                }
            });
        };

        /* 重新渲染设计区*/
        Class.prototype.renderForm = function () {
            var that = this
                , options = that.config;
            var elem = $('#formPreviewForm');
            //清空
            elem.empty();
            that.renderComponents(options.data, elem);
            if (!!options.openSubmit) {
                elem.append(TPL_SUBMIT);
            }
            that.setFormData(options.formData);
            form.render();//一次性渲染表单
            if (options.data.length != 0) {
                for (let i = 0; i < options.data.length ; i++) {
                    if (options.data[i].tag === 'grid') {
                        for (let j = 0; j < options.data[i].columns.length; j++) {
                            for (let k = 0; k < options.data[i].columns[j].list.length; k++) {
                                if (options.data[i].columns[j].list[k].tag === 'select') {
                                    $('#' + options.data[i].columns[j].list[k].id + ' .layui-input-block div.layui-unselect.layui-form-select').css({width: '{0}'.format(options.data[i].columns[j].list[k].width)});
                                }
                            }
                        }
                    } else {
                        if (options.data[i].tag === 'select') {
                            $('#' + options.data[i].id + ' .layui-input-block div.layui-unselect.layui-form-select').css({width: '{0}'.format(options.data[i].width)});
                        }
                    }
                }
            }
        };


        //核心入口 初始化一个 regionSelect 类
        formPreview.render = function (options) {
            var defaults = {
                readonly: true,//是否只读
                formStyle:false,
            };
            options = $.extend(defaults, options);
            var ins = new Class(options);
            return thisIns.call(ins);
        };

        exports('formPreview'
            , formPreview);


    });