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
layui.define(["layer", "laytpl", "element", "form", "slider", "laydate", "rate", "colorpicker", "layedit", "carousel", "upload", "formField","numberInput","iconPicker", "cron","labelGeneration"]
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
                dateDefaultValue:'默认时间',
                labelWidth: "文本宽度",
                clearable: "是否清楚",
                prepend: "前缀",
                append: "追加",
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
                dataMaxValue: "最大日期",
                dataMinValue: "最小日期",
                stepValue: "步长",
                datetype: "日期类型",
                dateformat: "日期格式",
                half: "显示半星",
                theme: "皮肤",
                rateLength: "星星个数",
                interval: "间隔毫秒",
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
                editor: "iceEditor编辑器",
                slider: "滑块",
                image: "图片",
                grid: "一行多列",
                colorpicker: "颜色选择器",
                textarea: "多行文本",
                rate: "评分控件",
                switch: "开关",
                password: "密码框",
                carousel: "轮播",
                text: "显示文本",
                uploadUrl: "上传路径",
                urlPrefix: "上传前缀",
                uploadData: '上传参数',
                expression: "验证",
                file: "文件",
                autoplay: "自动切换",
                anim: "切换方式",
                arrow: "切换箭头",
                tab:"tab选项卡",
                tabHeaders:"tab标题",
                isInput:"显示输入框",
                dateRange:"日期范围",
                dateRangeDefaultValue:"默认范围",
                numberInput:"排序文本框",
                iconPicker:"图标选择器",
                iconPickerSearch:"是否搜索",
                iconPickerPage:"是否分页",
                iconPickerLimit:"显示数量",
                iconPickerCellWidth:"图标宽度",
                cron:"Cron表达式",
                cronUrl:"运行路径",
                labelGeneration:"标签组件",
                isEnter:"是否回车",
                buttonIcon:"按钮图标",
                buttonType:"按钮类型",
                isLabel:"显示标签",
                buttonSize:"组件尺寸",
                bottom:"按钮组件",
                buttonVlaue:"按钮文字",
                sign:"sign签名组件",
            }
            , expressions = [{text: '默认', value: ""}
                , {text: '数字', value: 'number'}
                , {text: '邮箱', value: 'email'}
                , {text: '手机', value: 'phone'}
                , {text: '身份证', value: 'identity'}
                , {text: '日期', value: 'date'}
                , {text: '网址', value: 'url'}
                , {text: '密码', value: 'pass'}
            ], anims = [{text: '左右切换', value: 'default'}
                , {text: '上下切换', value: 'updown'}
                , {text: '渐隐渐显切换', value: 'fade'}
            ], arrows = [{text: '悬停显示', value: 'hover'}
                , {text: '始终显示', value: 'always'}
                , {text: '始终不显示', value: 'none'}]
            , datetypes = [{text: '年选择器', value: 'year'}
                , {text: '年月选择器', value: 'month'}
                , {text: '时间选择器', value: 'time'}
                , {text: '日期选择器', value: 'date'}
                , {text: '日期时间选择器', value: 'datetime'}]
            , buttonTypes = [{text: '原始', value: 'layui-btn-primary'}
                , {text: '默认', value: ""}
                , {text: '百搭', value: 'layui-btn-normal'}
                , {text: '暖色', value: 'layui-btn-warm'}
                , {text: '警告', value: ' layui-btn-danger'}]
            , buttonSizes = [{text: '大型', value: 'layui-btn-lg'}
                , {text: '默认', value: ""}
                , {text: '小型', value: 'layui-btn-sm'}
                , {text: '迷你', value: 'layui-btn-xs'}]
            , dateformats = ["yyyy年MM月", "yyyy-MM-dd", "dd/MM/yyyy", "yyyyMMdd", "yyyy-MM-dd HH:mm:ss", "yyyy年MM月dd日 HH时mm分ss秒"]
            , renderCommonProperty = function (json) {
                var _html = '';
                for (var key in json) {
                    if (key === 'index') {
                        continue;
                    }
                    if (key === 'tag' || key === 'id' || key === 'uploadUrl' || key === 'urlPrefix' || key === 'uploadData' || key === 'document' || key === 'interval' || key === 'cronUrl') { //只读字段
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        if (key === 'tag') {
                            _html += '    <input type="text" readonly id="{0}" name="{0}" value="{1}" required lay-verify="required" placeholder="请输入内容" autocomplete="off" class="layui-input">'
                                .format(key, json[key] == undefined ? '' : json[key]);
                        }
                        else {
                            _html += '    <input type="text" id="{0}" name="{0}" value="{1}" required lay-verify="required" placeholder="请输入内容" autocomplete="off" class="layui-input">'
                                .format(key, json[key] == undefined ? '' : json[key]);
                        }

                        _html += '  </div>';
                        _html += '</div>';
                    } else if (key === 'readonly' || key === 'disabled' || key === 'required' || key === 'half' || key === "text" || key === "autoplay"
                        || key === "full" || key === "verification" || key === 'autoplay' || key === 'isInput' || key === 'expression' || key === 'iconPickerSearch'
                        || key === 'iconPickerPage' || key === 'isEnter' || key === 'isLabel') {
                        var yes = "是";
                        var no = "否";
                        if (key === 'isInput') {
                            _html += '<div class="layui-form-item" >';
                            _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                            _html += '  <div class="layui-input-block">';
                            _html += '    <input type="checkbox" id="{1}" {0} name="{1}" lay-skin="switch" lay-text="{2}|{3}">'.format(json[key] ? 'checked' : '', key, yes, no);
                            _html += '  </div>';
                            _html += '</div>';
                        }
                        if (key === 'autoplay' || key === 'iconPickerSearch'|| key === 'iconPickerPage') {
                            _html += '<div class="layui-form-item" >';
                            _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                            _html += '  <div class="layui-input-block">';
                            _html += '    <input type="checkbox" id="{1}" {0} name="{1}" lay-skin="switch" lay-text="{2}|{3}">'.format(json[key] ? 'checked' : '', key, yes, no);
                            _html += '  </div>';
                            _html += '</div>';
                        }
                        if (key === 'readonly') {
                            yes = "只读";
                            no = "可写";
                            _html += '<div class="layui-form-item" >';
                            _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                            _html += '  <div class="layui-input-block">';
                            _html += '    <input type="checkbox" id="{1}" {0} name="{1}" lay-skin="switch" lay-text="{2}|{3}">'.format(json[key] ? 'checked' : '', key, yes, no);
                            _html += '  </div>';
                            _html += '</div>';
                        }
                        if (key === 'isEnter' || key === 'isLabel') {
                            _html += '<div class="layui-form-item" >';
                            _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                            _html += '  <div class="layui-input-block">';
                            _html += '    <input type="checkbox" id="{1}" {0} name="{1}" lay-skin="switch" lay-text="{2}|{3}">'.format(json[key] ? 'checked' : '', key, yes, no);
                            _html += '  </div>';
                            _html += '</div>';
                        }
                        if (key === 'disabled') {
                            yes = "隐藏";
                            no = "显示";
                            _html += '<div class="layui-form-item" >';
                            _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                            _html += '  <div class="layui-input-block">';
                            _html += '    <input type="checkbox" id="{1}" {0} name="{1}" lay-skin="switch" lay-text="{2}|{3}">'.format(json[key] ? 'checked' : '', key, yes, no);
                            _html += '  </div>';
                            _html += '</div>';
                        }
                        if (key === 'required') {
                            yes = "必填";
                            no = "可选";
                            _html += '<div class="layui-form-item" >';
                            _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                            _html += '  <div class="layui-input-block">';
                            _html += '    <input type="checkbox" id="{1}" {0} name="{1}" lay-skin="switch" lay-text="{2}|{3}">'.format(json[key] ? 'checked' : '', key, yes, no);
                            _html += '  </div>';
                            _html += '</div>';
                        }
                        if (key === 'expression') {
                            _html += '<div class="layui-form-item" >';
                            _html += '  <label class="layui-form-label">验证</label>';
                            _html += '  <div class="layui-input-block">';
                            _html += '<select name="{0}" lay-verify="required">'.format(key);
                            for (var i = 0; i < expressions.length; i++) {
                                if (expressions[i].value === json.expression) {
                                    _html += '<option value="{0}" selected="">{1}</option>'.format(expressions[i].value, expressions[i].text);
                                } else {
                                    _html += '<option value="{0}">{1}</option>'.format(expressions[i].value, expressions[i].text);
                                }
                            }
                            _html += '</select>'
                            _html += '  </div>';
                            _html += '</div>';
                        }
                    } else if (key === 'defaultValue' || key === 'label' || key === 'height' || key === 'placeholder' || key === 'document'
                        || key === 'minValue' || key === 'maxValue' || key === 'stepValue' || key === 'rateLength'
                        || key === 'iconPickerLimit' || key === 'iconPickerCellWidth'|| key === 'buttonVlaue') {
                            _html += '<div class="layui-form-item" >';
                            _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                            _html += '  <div class="layui-input-block">';
                            _html += '    <input type="text" id="{0}" name="{0}" value="{1}" required lay-verify="required" placeholder="请输入{2}" autocomplete="off" class="layui-input">'
                                .format(key, json[key] == undefined ? '' : json[key], lang[key]);
                            _html += '  </div>';
                            _html += '</div>';
                    } else if (key === 'width' || key === 'labelWidth') {
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '<div id="{0}" class="widget-slider" style="top: 16px;"></div>'.format(key);
                        _html += '    <input type="hidden" id="{0}"  name="{0}" value="{1}" readonly="readonly" style="background:#eeeeee!important;width: 100%;" placeholder="请选择{2}" autocomplete="off" class="layui-input">'
                            .format(key + "-value", '', lang[key]);
                        _html += '  </div>';
                        _html += '</div>';
                    } else if (key === 'switchValue') {
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '     <input type="checkbox" name="{0}" {1} lay-skin="switch" lay-text="ON|OFF">'
                            .format(key, json[key] ? 'checked' : '');
                        _html += '  </div>';
                        _html += '</div>';
                    } else if (key === 'datetype') {
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '<select name="{0}" lay-verify="required">'.format(key);
                        for (var i = 0; i < datetypes.length; i++) {
                            if (datetypes[i].value === json.datetype) {
                                _html += '<option value="{0}" selected="">{1}</option>'.format(datetypes[i].value,datetypes[i].text);
                            } else {
                                _html += '<option value="{0}">{1}</option>'.format(datetypes[i].value,datetypes[i].text);
                            }
                        }
                        _html += '</select>'
                        _html += '  </div>';
                        _html += '</div>';
                    } else if (key === 'dateformat') {
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '<select name="{0}" lay-verify="required">'.format(key);
                        for (var i = 0; i < dateformats.length; i++) {
                            if (dateformats[i] === json.dateformat) {
                                _html += '<option value="{0}" selected="">{0}</option>'.format(dateformats[i]);
                            } else {
                                _html += '<option value="{0}">{0}</option>'.format(dateformats[i]);
                            }
                        }
                        _html += '</select>'
                        _html += '  </div>';
                        _html += '</div>';

                    } else if (key === 'contents') {
                        //处理
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '    <button style="margin: 5px 0px 0px 30px;" type="button" id="select-option-add" class="layui-btn layui-btn-primary layui-btn-sm"><i class="layui-icon layui-icon-addition"></i>增加选项</button>'
                        //_html += '    <button type="button" id="select-option-datasource" class="layui-btn layui-btn-primary layui-btn-sm"><i class="layui-icon layui-icon-addition"></i>设置数据源</button>'
                        _html += '  </div>';
                        _html += '</div>';
                        _html += '<div id="{0}">'.format(json.tag);
                        //选项
                        for (var i = 0; i < json.contents.length; i++) {
                            _html += '<div class="layui-form-item option contents-options" data-index="{0}">'.format(i);

                            _html += '  <div class="layui-inline" style="margin-right: 0px;width:220px; margin-left:30px;">';
                            _html += '   <input type="text" name="{0}-text"  autocomplete="off" value="{1}" class="layui-input">'.format(json.tag, json.contents[i]);
                            _html += '  </div>';
                            _html += '  <div class="layui-inline" style="margin-right: 0px;">';
                            _html += '   <i class="layui-icon layui-icon-slider contents-option-drag" style="color:blue;font-size:20px;"></i>';
                            _html += '   <i class="layui-icon layui-icon-delete contents-option-delete" style="color:red;font-size:20px;"></i>';
                            _html += '  </div>';
                            _html += '</div>';
                            //向 .option .layui-inline 添加drag事件并且必须设在 contents-option-drag 中才能拖动
                        }
                        _html += '</div>';
                    } else if (key === 'options') {
                        //处理
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '    <button style="margin: 5px 0px 0px 30px;" type="button" id="select-option-add" class="layui-btn layui-btn-primary layui-btn-sm"><i class="layui-icon layui-icon-addition"></i>增加选项</button>'
                        //_html += '    <button type="button" id="select-option-datasource" class="layui-btn layui-btn-primary layui-btn-sm"><i class="layui-icon layui-icon-addition"></i>设置数据源</button>'
                        _html += '  </div>';
                        _html += '</div>';
                        _html += '<div id="{0}">'.format(json.tag);
                        //选项
                        for (var i = 0; i < json.options.length; i++) {
                            _html += '<div class="layui-form-item option select-options" data-index="{0}">'.format(i);
                            _html += '  <div class="layui-inline" style="width: 30px; margin-right: 0px;">';
                            if (json.tag === 'checkbox') {
                                if (json.options[i].checked) {
                                    _html += '    <input type="checkbox" name="{0}" lay-skin="primary" title="" checked="">'.format(json.tag);
                                } else {
                                    _html += '    <input type="checkbox" name="{0}" lay-skin="primary" title="">'.format(json.tag);
                                }
                            } else {
                                if (json.options[i].checked) {
                                    _html += '   <input type="radio" name="{0}"  checked="">'.format(json.tag);
                                } else {
                                    _html += '   <input type="radio" name="{0}">'.format(json.tag);
                                }

                            }

                            _html += '  </div>';
                            _html += '  <div class="layui-inline" style="margin-right: 0px;width:110px;">';
                            _html += '   <input type="text" name="{0}-text"  autocomplete="off" value="{1}" class="layui-input">'.format(json.tag, json.options[i].text);
                            _html += '  </div>';
                            _html += '  <div class="layui-inline" style="margin-right: 0px;width:110px;">';
                            _html += '   <input type="text" name="{0}-value"  autocomplete="off" value="{1}" class="layui-input">'.format(json.tag, json.options[i].value);
                            _html += '  </div>';
                            _html += '  <div class="layui-inline" style="margin-right: 0px;">';
                            _html += '   <i class="layui-icon layui-icon-slider select-option-drag" style="color:blue;font-size:20px;"></i>';
                            _html += '   <i class="layui-icon layui-icon-delete select-option-delete" style="color:red;font-size:20px;"></i>';
                            _html += '  </div>';
                            _html += '</div>';
                            //向 .option .layui-inline 添加drag事件并且必须设在 select-option-drag 中才能拖动
                        }
                        _html += '</div>';
                    } else if (key === 'columns') {
                        var columnCount = 2;
                        columnCount = json[key].length;
                        //处理
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '<select name="{0}" lay-verify="required">'.format(key);
                        for (var i = 2; i <= 12; i++) {
                            if (i === columnCount) {
                                _html += '<option value="{0}" selected="">{0}</option>'.format(i);
                            } else {
                                _html += '<option value="{0}">{0}</option>'.format(i);
                            }
                        }
                        _html += '</select>'
                        _html += '</div>';
                        _html += '</div>';
                    } else if (key === 'anim') {
                        //处理
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '<select name="{0}" lay-verify="required">'.format(key);
                        for (var i = 0; i < anims.length; i++) {
                            if (anims[i].value === json.anim) {
                                _html += ' <option value="{1}" selected="">{0}</option>'.format(anims[i].text, anims[i].value);
                            } else {
                                _html += ' <option value="{1}">{0}</option>'.format(anims[i].text, anims[i].value);
                            }
                        }
                        _html += '</select>'
                        _html += '</div>';
                        _html += '</div>';
                    } else if (key === 'arrow') {
                        //处理
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '<select name="{0}" lay-verify="required">'.format(key);
                        for (var i = 0; i < arrows.length; i++) {
                            if (arrows[i].value === json.arrow) {
                                _html += ' <option value="{1}" selected="">{0}</option>'.format(arrows[i].text, arrows[i].value);
                            }else {
                                _html += ' <option value="{1}">{0}</option>'.format(arrows[i].text, arrows[i].value);
                            }
                        }
                        _html += '</select>'
                        _html += '</div>';
                        _html += '</div>';
                    }else if (key === 'buttonType') {
                        //处理
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '<select name="{0}" lay-verify="required">'.format(key);
                        for (var i = 0; i < buttonTypes.length; i++) {
                            if (buttonTypes[i].value === json.buttonType) {
                                if (buttonTypes[i].value === "") {
                                    _html += ' <option selected="">{0}</option>'.format(buttonTypes[i].text);
                                }else {
                                    _html += ' <option value="{1}" selected="">{0}</option>'.format(buttonTypes[i].text, buttonTypes[i].value);
                                }
                            }else {
                                _html += ' <option value="{1}">{0}</option>'.format(buttonTypes[i].text, buttonTypes[i].value);
                            }
                        }
                        _html += '</select>'
                        _html += '</div>';
                        _html += '</div>';
                    }else if (key === 'buttonSize') {
                        //处理
                        _html += '<div class="layui-form-item" >';
                        _html += '  <label class="layui-form-label">{0}</label>'.format(lang[key]);
                        _html += '  <div class="layui-input-block">';
                        _html += '<select name="{0}" lay-verify="required">'.format(key);
                        for (var i = 0; i < buttonSizes.length; i++) {
                            if (buttonSizes[i].value === json.buttonSize) {
                                if (buttonSizes[i].value === "") {
                                    _html += ' <option selected="">{0}</option>'.format(buttonSizes[i].text);
                                }else {
                                    _html += ' <option value="{1}" selected="">{0}</option>'.format(buttonSizes[i].text, buttonSizes[i].value);
                                }
                            }else {
                                _html += ' <option value="{1}">{0}</option>'.format(buttonSizes[i].text, buttonSizes[i].value);
                            }
                        }
                        _html += '</select>'
                        _html += '</div>';
                        _html += '</div>';
                    } else if (key === 'dataMaxValue' || key === 'dataMinValue') {
                        _html += '<div id="{0}" class="layui-form-item">'.format(key + json.id);
                        _html += '<label class="layui-form-label">{0}:</label>'.format(lang[key]);
                        _html += '<div class="layui-input-block"">';
                        _html += '<div id="{0}" class="layui-input icon-date widget-date" style="line-height: 40px;"></div>'.format(key + json.tag + json.id);
                        _html += '</div>';
                        _html += '</div>';
                    } else if (key === 'dateDefaultValue') {
                        _html += '<div id="{0}" class="layui-form-item">'.format(key + json.id);
                        _html += '<label class="layui-form-label">{0}:</label>'.format(lang[key]);
                        _html += '<div class="layui-input-block"">';
                        _html += '<div id="{0}" class="layui-input icon-date widget-date" style="line-height: 40px;"></div>'.format(key + json.tag + json.id);
                        _html += '</div>';
                        _html += '</div>';
                    }else if (key === 'dateRangeDefaultValue') {
                        _html += '<div id="{0}" class="layui-form-item">'.format(key + json.id);
                        _html += '<label class="layui-form-label">{0}:</label>'.format(lang[key]);
                        _html += '<div class="layui-input-block"">';
                        _html += '<div id="{0}" class="layui-input icon-date widget-date" style="line-height: 40px;"></div>'.format(key + json.tag + json.id);
                        _html += '</div>';
                        _html += '</div>';
                    }else if (key === 'buttonIcon') {
                        _html += '<div id="{0}" class="layui-form-item" ">'.format(key + json.id);
                        _html += '<label class="layui-form-label">{0}:</label>'.format(lang[key]);
                        _html += '<div class="layui-input-block"">';
                        _html += '<div id="{0}" class="layui-input icon-date widget-date" style="line-height: 40px;"></div>'.format(json.tag + json.id + "property");
                        _html += '</div>';
                        _html += '</div>';
                    }
                }
                return _html;
            }
            //模块名称常量

            , MOD_NAME = 'formDesigner'
            , ELEM = '.layui-form-designer'
            , TP_MAIN = ['<div class="layui-layout layui-layout-admin">'
                , '<div class="layui-header">'
                , '<div class="layui-logo">表单设计器</div>'
                , '<!-- 头部区域（可配合layui已有的水平导航） -->'
                , '<ul class="layui-nav layui-layout-left">'
                , '<li class="layui-nav-item"><a href=""></a></li>'
                , '</ul>'
                , '<ul class="layui-nav layui-layout-right">'
                , '<li class="layui-nav-item">'
                , '<a id="saveJson" href="#" class="saveJson">保存</a>'
                , '</li>'
                , '<li class="layui-nav-item">'
                , '<a id="btnImportJson" href="#" class="importJson">导入数据</a>'
                , '</li>'
                , '<li class="layui-nav-item">'
                , '<a id="btnExportJson" href="#" class="exportJson">导出数据</a>'
                , '</li>'
                , '<li class="layui-nav-item">'
                , '<a id="btnTemplateList" href="#" class="templateList">模板</a>'
                , '</li>'
                , '<li class="layui-nav-item">'
                , '<a href="#" class="previewForm">预览</a>'
                , '</li>'
                , '<li class="layui-nav-item">'
                , '<a href="#" class="generateCode">生成代码</a>'
                , '</li>'
                , '</ul>'
                , '</div>'
                , '<div class="layui-side">'
                , '<div class="layui-side-scroll" style="width: 260px;">'
                , '<!-- 左侧导航区域（可配合layui已有的垂直导航） -->'
                , '<div class="layui-tab layui-tab-brief" lay-filter="components-list">'
                , '<ul class="layui-tab-title">'
                , '<li class="layui-this">组件</li>'
                , '</ul>'
                , '<div class="layui-tab-content">'
                , '<div class="layui-tab-item layui-show">'
                , '<div class="components-list" id="components-form-list">'
                , '</div>'
                , '</div>'
                , '<div class="layui-tab-item components-list" id="components-form-list">开发中...</div>'
                , '</div>'
                , '</div>'
                , '</div>'
                , '</div>'
                , '<div class="layui-body">'
                , '<!-- 内容主体区域 -->'
                , '<form class="layui-form" id="designerForm" style="height:98%;">'
                , '<div class="layui-form" id="formDesignerForm" lay-filter="formDesignerForm">'
                , '<div class="layui-row layui-empty">'
                , '从左侧拖拽控件到此设计区域来添加字段'
                , '</div>'
                , '</div>'
                , '<button type="button" class="layui-btn" style="display: none" lay-submit lay-filter="*">立即提交</button>'
                , '</form>'
                , '</div>'
                , '<div class="layui-side-right">'
                , '<div class="layui-side-scroll" style="width: 350px;">'
                , '<!-- 属性导航 -->'
                , '<form class="layui-form  layui-form-pane">'
                , '<div class="layui-tab layui-tab-brief" lay-filter="form-attr">'
                , '<ul class="layui-tab-title">'
                , '<li class="layui-this">字段设置</li>'
                , '<li>表单设置</li>'
                , '</ul>'
                , '<div class="layui-tab-content">'
                , '<div class="layui-tab-item layui-show" id="columnProperty">'
                , '</div>'
                , '<div class="layui-tab-item" id="formProperty">'
                , '<!--表单ID-->'
                , '<div class="layui-form-item">'
                , '<label class="layui-form-label">表单ID</label>'
                , '<div class="layui-input-block">'
                , '<input type="text" name="formId" required="" lay-verify="required"'
                , 'placeholder="请输入表单ID" autocomplete="off" class="layui-input">'
                , '</div>'
                , '</div>'
                , '<!--//表单ID-->'
                , '<!--表单名称-->'
                , '<div class="layui-form-item">'
                , '<label class="layui-form-label">表单名称</label>'
                , '<div class="layui-input-block">'
                , '<input type="text" name="formName" required="" lay-verify="required"'
                , 'placeholder="请输入表单名称" autocomplete="off" class="layui-input">'
                , '</div>'
                , '</div>'
                , '<!--//end-->'
                , '</div>'
                , '</div>'
                , '</div>'
                , '</form>'
                , '</div>'
                , '</div>'
                , '</div>'].join('')
            , TP_HTML_VIEW = ['<div class="htmlcodeview layui-layer-wrap" style="display: none;">'
                , '<textarea class="site-demo-text" id="generate-code-view"></textarea>'
                , '<a href="javascript:;" class="layui-btn layui-btn-normal" style="margin-right:20px;" id="copy-html-code">复制代码</a>'
                , '</div>'].join('')
            , TP_IMPORT_VIEW = ['<div class="importjsoncodeview layui-layer-wrap" style="display: none;">'
                , '<textarea class="site-demo-text" id="import-json-code-view"></textarea>'
                , '<a href="javascript:;" class="layui-btn layui-btn-normal" style="margin-right:20px;" id="import-json-code">导入数据</a>'
                , '</div>'].join('')
            , TP_ABOUT_VIEW = [''].join('')
            //外部接口
            , formDesigner = {
                index: layui.formDesigner ? (layui.formDesigner.index + 10000) : 0
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
             * 操作当前实例elem
             * id 表示当前实例的索引 默认就是内部的 index，如果id存在值 那么就从已经存在的获取
             */

            , thisIns = function () {
                var that = this
                    , options = that.config;
                return {
                    reload: function (options) {
                        that.reload.call(that
                            , options);
                    },getOptions:function () {
                        return options || null;
                    },getData : function () {
                        return options.data || null;
                    },geticeEditorObjects:function () {
                        return iceEditorObjects || null;
                    }
                }
            }

            , getThisInsConfig = function (id) {
                var config = thisIns.config[id];
                if (!config) {
                    hint.error('在表实例中找不到ID选项');
                }
                return config || null;
            }
            , Class = function (options) {
                var that = this;
                that.index = ++formDesigner.index; //增加实例，index 也是要增加JSON.stringify(options.data, null, 4)
                that.config = $.extend({}
                    , that.config
                    , formDesigner.config
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
        /* 删除json中的文件并返回上一个节点*/
        Class.prototype.deleteJsonItem = function (json, id) {
            var that = this,
                options = that.config;
            for (var i = 0; i < json.length; i++) {
                if (json[i].id === id) {
                    json.splice(i, 1);
                    if (i > 0) {
                        return json[i - 1];
                    }
                    break;
                }else {
                    if (json[i].tag === 'grid') {
                        for (var j = 0; j < json[i].columns.length; j++) {
                            if (json[i].columns[j].list.length > 0) {
                                that.deleteJsonItem(json[i].columns[j].list, id);
                            }
                        }
                    }
                }


            }
            return undefined;
        };
        Class.prototype.copyJsonAfterItem = function (json, id) {
            var that = this,
                options = that.config;
            for (var i = 0; i < json.length; i++) {
                if (json[i].id === id) {
                    var _newjson = JSON.parse(JSON.stringify(json[i]));
                    _newjson.id = that.autoId(_newjson.tag);
                    json.splice(i + 1, 0, _newjson);
                    return json[i];
                } else {
                    if (json[i].tag === 'grid') {
                        for (var j = 0; j < json[i].columns.length; j++) {
                            if (json[i].columns[j].list.length > 0) {
                                var _item = that.copyJsonAfterItem(json[i].columns[j].list, id);
                                if (_item) {
                                    return _item;
                                }
                            }
                        }
                    }
                }

            }
            return undefined;
        };
        /**
         * data 表示设计区数据
         * dataSource 表示数据源即一个控件的数据来源
         *
         */
        Class.prototype.config = {
            version: "1.0.0"
            , formName: "表单示例"
            , Author: "谁家没一个小强"
            , formId: "id"
            , generateId: 0
            , data: []
            , dataSource: {}
            , selectItem: undefined
            , htmlCode: {
                css: ''
                , html: ''
                , script: ''
            }
        };

        /* 自动生成ID 当前页面自动排序*/
        Class.prototype.autoId = function (tag) {
            var that = this,
                options = that.config;
            options.generateId = options.generateId + 1;
            return tag + '_' + options.generateId;
        }
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
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _readonly = json.readonly ? 'readonly=""' : '';
                    var _required = json.required ? 'required' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    if (json.expression !== null && json.expression !== undefined ) {
                        if (json.expression !== '') {
                            _required = 'required' + '|' + json.expression;
                        }
                    }
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    if (json.required) {
                        $label.addClass('layui-form-required');
                    }
                    else {
                        $label.removeClass('layui-form-required');
                    }
                    $label.append(json.label + ":");
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<input name="{0}" value="{1}" placeholder="{3}" class="layui-input{7}" lay-vertype="tips" lay-verify="{6}" {4} {5} style="width:{2}">'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.width, json.placeholder, _readonly, _disabled, _required, _disabledClass);
                    $block.append(_html);
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.input));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
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
            bottom: {
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
                update: function (json) {
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    if (json.isLabel) {
                        if ($('#' + json.id).find("label").length === 0) {
                            $('#' + json.id).prepend('<label class="layui-form-label" style="width: {1}px;">{0}:</label>'.format(json.label,json.labelWidth));
                        }else {
                            $label.css("width",json.labelWidth + "px");
                        }
                        $label.empty();
                        $label.append(json.label + ":");
                    }else {
                        $label.remove();
                        $block.css("margin-left","0px");
                    }
                    $block.empty();
                    var _html = '';
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
                    //重绘设计区改id下的所有元素
                    if (json.disabled) {
                        _html += '<button id="{0}" type="button" class="layui-btn {1} layui-btn-disabled {2}"><i class="layui-icon {3}"></i> {4}</button>'.format(json.id + json.tag, json.buttonSize,_coustomCss ,json.buttonIcon,json.buttonVlaue);
                    }else {
                        _html += '<button id="{0}" type="button" class="layui-btn {1} {2} {3}"><i class="layui-icon {4}"></i> {5}</button>'.format(json.id + json.tag, json.buttonSize, json.buttonType,_coustomCss ,json.buttonIcon,json.buttonVlaue);
                    }
                    $block.append(_html);
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.bottom));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
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
            sign: {
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
                update: function (json) {
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $label.append(json.label + ":");
                    $block.css({width: 'calc({0} - {1}px)'.format(json.width,json.labelWidth)});
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    if (json.disabled) {
                        _html += '<button id="{0}" type="button" class="layui-btn layui-btn-normal layui-btn-disabled custom-zc"><i class="layui-icon {1}"></i> {2}</button>'.format(json.id + json.tag ,json.buttonIcon,json.buttonVlaue);
                    }else {
                        _html += '<button id="{0}" type="button" class="layui-btn  layui-btn-normal custom-zc"><i class="layui-icon {1}"></i> {2}</button>'.format(json.id + json.tag ,json.buttonIcon,json.buttonVlaue);
                    }
                    if (json.data !== "") {
                        _html += '<div class="signImg"><img src="{0}"></div>'.format(json.data);
                    }
                    $block.append(_html);
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.sign));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
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
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label" style="width: {1}px;">{0}:</label>'.format( json.label,json.labelWidth);
                    _html += '<div class="layui-input-block" style="margin-left: {0}px;">'.format(json.labelWidth);
                    if (json.disabled) {
                        _html += '<div class="iceEditor-disabled"></div>';
                    }
                    _html += '<input name="{0}" id="{6}" value="{1}" placeholder="{3}" class="layui-input{5}" lay-filter="{6}" lay-vertype="tips" {4} style="width:{2}">'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.width, json.placeholder, _disabled, _disabledClass,json.tag + json.id);
                    _html += '</div>';
                    _html += '</div>';
                    return _html;
                },
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $label.empty();
                    $block.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $label.append(json.label + ":");
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<input name="{0}" value="{1}" id="{6}" placeholder="{3}" class="layui-input{5}" lay-filter="{6}" lay-vertype="tips" {4} style="width:{2}">'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.width, json.placeholder, _disabled, _disabledClass,json.tag + json.id);
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
                    iconPicker.checkIcon(json.tag + json.id, json.defaultValue);
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
                    _html += '<label class="layui-form-label {0}" style="width: {2}px">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
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
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _required = json.required ? 'required' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $block.css({width: 'calc({0} - {1}px)'.format(json.width,json.labelWidth)});
                    if (json.required) {
                        $label.addClass('layui-form-required');
                    }
                    else {
                        $label.removeClass('layui-form-required');
                    }
                    $label.append(json.label + ":");
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<input name="{0}" value="{1}" id="{5}" placeholder="{2}" class="layui-input{4}" lay-verify="{6}" lay-filter="iconPicker" lay-vertype="tips" {3}>'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.placeholder, _disabled, _disabledClass,json.tag + json.id,_required);
                    if (!json.disabled) {
                        var _width = json.width.replace(/[^\d]/g,'');
                        if(''!=_width){
                            _width = 100 - parseInt(_width);
                        }
                        _html += '<button id="{0}-button" style="position: absolute;top: 0;right: 0px;cursor: pointer;" type="button" class="layui-btn">生成</button>'.format(json.tag + json.id);
                        $block.append(_html);
                        cron.render({
                            elem: "#" + json.tag + json.id + "-button", // 绑定元素
                            url: json.cronUrl, // 获取最近运行时间的接口
                            // value: $("#cron").val(), // 默认值
                            done: function (cronStr) {
                                $("#" + json.tag + json.id).val(cronStr);
                            },
                        });
                    } else {
                        $block.append(_html);
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
                    _html += '<label class="layui-form-label" style="width: {1};">{0}:</label>'.format( json.label,json.labelWidth);
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
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $label.append(json.label + ":");
                    $block.css({width: 'calc({0} - {1}px)'.format(json.width,json.labelWidth)});
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<input name="{0}" id="{9}" value="{1}" placeholder="{3}" class="layui-input{5}" lay-vertype="tips" min="{6}" max="{7}" step="{8}"  {4} style="width:{2}">'
                        .format(json.id, json.defaultValue ? json.defaultValue : '0', json.width, json.placeholder, _disabled , _disabledClass,json.minValue,json.maxValue,json.stepValue,json.tag + json.id);
                    $block.append(_html);
                    numberInput.render({
                        elem:'#' + json.tag + json.id
                    });
                    var _width = json.width.replace(/[^\d]/g,'');
                    if(''!=_width){
                        _width = 100 - parseInt(_width);
                    }
                    $('#' + json.id + ' .layui-input-block .layui-number-input-btn').css("right",_width + "%");
                    if (json.disabled) {
                        $('#' + json.id + ' .layui-input-block .layui-number-input-btn').css("z-index","-1");
                    }
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.numberInput));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
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
                update: function (json) {
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $label.append(json.label + ":");
                    $block.css({width: 'calc({0} - {1}px)'.format(json.width,json.labelWidth)});
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<div id="{0}" style="width: {1}"></div>'.format(json.tag + json.id,json.width);
                    $block.append(_html);
                    labelGeneration.render({
                        elem:'#' + json.tag + json.id,
                        data:json.dateDefaultValue,
                        isEnter:json.isEnter
                    });
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
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _readonly = json.readonly ? 'readonly=""' : '';
                    var _required = json.required ? 'lay-verify="required"' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    if (json.required) {
                        $label.addClass('layui-form-required');
                    }
                    else {
                        $label.removeClass('layui-form-required');
                    }
                    $label.append(json.label + ":");
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<input type="password" name="{0}" placeholder="{3}" value="{1}" autocomplete="off" style="width:{2}" {4} {5} {6} class="layui-input{7}">'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.width, json.placeholder, _readonly, _disabled, _required, _disabledClass);
                    $block.append(_html);
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.password));
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
                    _html += '<label class="layui-form-label {0}" style="width: {2}px;">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-input-block layui-form" lay-filter="{0}" style="margin-left: {1}px">'.format(json.id,json.labelWidth);
                    _html += '<select name="{0}" lay-verify="{2}" {1} >'.format(json.id, _disabled,_required);
                    /*if (json.defaultValue === undefined) {
                        _html += '<option value="{0}" selected="">{1}</option>'.format('', '请选择');
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
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _required = json.required ? 'required' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    var _html = '';
                    _html += '<select name="{0}" lay-verify="{2}" {1}>'.format(json.id, _disabled,_required);
                    //重绘设计区改id下的所有元素
                    /* if (json.defaultValue === undefined) {
                         _html += '<option value="{0}" selected="">{1}</option>'.format('', '请选择');
                     }*/
                    for (var i = 0; i < json.options.length; i++) {
                        if (json.options[i].checked) {
                            _html += '<option value="{0}" selected="">{1}</option>'.format(json.options[i].value, json.options[i].text);
                        } else {
                            _html += '<option value="{0}">{1}</option>'.format(json.options[i].value, json.options[i].text);
                        }
                    }
                    _html += '</select>'
                    $('#' + json.id + ' .layui-input-block').append(_html);
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    if (json.required) {
                        $label.addClass('layui-form-required');
                    }
                    else {
                        $label.removeClass('layui-form-required');
                    }
                    $label.append(json.label + ":");
                    form.render('select',json.id);
                    $('#' + json.id + ' .layui-input-block div.layui-unselect.layui-form-select').css({width: '{0}'.format(json.width)});
                    $block.css({width: 'calc({0} - {1}px)'.format(json.width,json.labelWidth)});
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.select));
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
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $label.append(json.label + ":");
                    $block.css({width: 'calc({0} - {1}px)'.format(json.width,json.labelWidth)});
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    for (var i = 0; i < json.options.length; i++) {
                        if (json.options[i].checked) {
                            _html += '<input type="radio" name="{0}" value="{1}" title="{2}" checked="" {3}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled);
                        } else {
                            _html += '<input type="radio" name="{0}" value="{1}" title="{2}" {3}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled);
                        }
                    }
                    $block.append(_html);
                    form.render('radio');
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.radio));
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
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    for (var i = 0; i < json.options.length; i++) {
                        if (json.options[i].checked) {
                            _html += '<input type="checkbox" name="{0}[{1}]" title="{2}" checked="" {3} {4}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled,_required);
                        } else {
                            _html += '<input type="checkbox" name="{0}[{1}]" title="{2}" {3} {4}>'.format(json.id, json.options[i].value, json.options[i].text, _disabled,_required);
                        }
                    }
                    $block.append(_html);
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    if (json.required) {
                        $label.addClass('layui-form-required');
                    }
                    else {
                        $label.removeClass('layui-form-required');
                    }
                    $label.append(json.label + ":");
                    form.render('checkbox');
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.checkbox));
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
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    var _html = '';
                    _html += '<input type="checkbox" name="{0}" lay-skin="switch" lay-text="ON|OFF" {1} class="{2}" {3}>'.format(json.tag, _disabled, _disabledClass, json.switchValue ? 'checked' : '');
                    $block.append(_html);
                    $label.append(json.label + ":");
                    $block.css({width: 'calc({0} - {1}px)'.format(json.width,json.labelWidth)});
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    form.render('checkbox');
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.switch));
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
                update: function (json) {
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $label.append(json.label + ":");
                    $block.css({width: 'calc({0} - {1}px'.format(json.width,json.labelWidth)});
                    slider.render({
                        elem: '#' + json.tag + json.id,
                        value: json.defaultValue, //初始值 
                        min: json.minValue,
                        max: json.maxValue,
                        step: json.stepValue,
                        disabled: json.disabled,
                        input:json.isInput,
                    });
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID  
                    var _json = JSON.parse(JSON.stringify(formField.slider));
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
            dateRange: {
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
                update: function (json) {
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _disabledStyle = json.disabled ? ' pointer-events: none;' : '';
                    var _required = json.required ? 'required' : '';
                    var _html = '<div class="layui-inline">';
                    _html += '<label class="layui-form-label {0}" style="width: {2}px;">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label,json.labelWidth);
                    _html += '<div class="layui-inline" id="{0}" style="line-height: 40px;{1}">'.format(json.tag + json.id,_disabledStyle);
                    _html += '<div class="layui-input-inline">';
                    _html += '<input id="start-{0}" lay-verify="{3}" name="start{2}" class="layui-input {1}" autocomplete="off" placeholder="开始日期" ></input>'.format(json.tag + json.id,_disabledClass,json.id,_required);
                    _html += '</div>';
                    _html += '<div class="layui-form-mid">-</div>';
                    _html += '<div class="layui-input-inline">';
                    _html += '<input id="end-{0}" lay-verify="{3}" name="end{2}" class="layui-input {1}" autocomplete="off" placeholder="结束日期"></input>'.format(json.tag + json.id,_disabledClass,json.id,_required);
                    _html += '</div>';
                    _html += '</div>';
                    _html += '</div>';
                    $('#' + json.id).empty();
                    $('#' + json.id).append(_html);
                    laydate.render({
                        elem: '#' + json.tag + json.id,
                        type: json.datetype,
                        format: json.dateformat,
                        //value: item.dateDefaultValue,
                        min: json.dataMinValue,
                        max: json.dataMaxValue,
                        range: ['#start-' + json.tag + json.id, '#end-' + json.tag + json.id]
                    });
                    if (json.dateRangeDefaultValue !== null && json.dateRangeDefaultValue !== ""
                        && json.dateRangeDefaultValue !== undefined) {
                        var split = json.dateRangeDefaultValue.split(" - ");
                        $('#start-' + json.tag + json.id).val(split[0]);
                        $('#end-' + json.tag + json.id).val(split[1]);
                    }
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID  
                    var _json = JSON.parse(JSON.stringify(formField.dateRange));
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
                update: function (json) {
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _disabledStyle = json.disabled ? ' pointer-events: none;' : '';
                    var _required = json.required ? 'required' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    if (json.required) {
                        $label.addClass('layui-form-required');
                    }
                    else {
                        $label.removeClass('layui-form-required');
                    }
                    $label.append(json.label + ":");
                    $block.css({width: 'calc({0} - {1}px)'.format(json.width,json.labelWidth)});
                    var _html = '<input id="{0}" name="{0}"  lay-verify="{3}" class="layui-input icon-date widget-date {1}" style="line-height: 40px;{2}"></input>'.format(json.tag + json.id,_disabledClass,_disabledStyle,_required);
                    $block.append(_html);
                    laydate.render({
                        elem: '#' + json.tag + json.id,
                        btns: ['confirm'],
                        type: json.datetype,
                        format: json.dateformat,
                        value: json.dateDefaultValue,
                        min: json.dataMinValue,
                        max: json.dataMaxValue,
                    });
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID
                    var _json = JSON.parse(JSON.stringify(formField.date));
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
                update: function (json) {
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $label.append(json.label + ":");
                    rate.render({
                        elem: '#' + json.tag + json.id,
                        value: json.defaultValue,
                        text: json.text,
                        length: json.rateLength,
                        half: json.half,
                        readonly: json.readonly
                    });
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.rate));
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
                    _html += '<div carousel-item class="carousel-item">';
                    for (var i = 0; i < json.options.length; i++) {
                        _html += '<div><img src="{0}" /></div>'.format(json.options[i].value);
                    }
                    _html += '</div>';//end for div carousel-item
                    _html += '</div>';//end for class=layui-carousel
                    // _html +='</div>'; 
                    _html += '</div>';
                    return _html;
                },
                update: function (json) {
                    $('#' + json.id).empty();
                    var _html = '';
                    //重绘设计区改id下的所有元素
                    _html += '<div class="layui-carousel" id="{0}">'.format(json.tag + json.id);
                    _html += '<div carousel-item class="carousel-item">';
                    for (var i = 0; i < json.options.length; i++) {
                        _html += '<div><img src="{0}" /></div>'.format(json.options[i].value);
                    }
                    _html += '</div>';//end for div carousel-item
                    _html += '</div>';//end for class=layui-carousel
                    $('#' + json.id).append(_html);
                    carousel.render({
                        elem: '#' + json.tag + json.id,
                        width: json.width,//设置容器宽度
                        height: json.height,//设置容器宽度
                        arrow: json.arrow, //始终显示箭头
                        interval: json.interval,
                        anim: json.anim,
                        autoplay: json.autoplay
                        //anim: 'updown' //切换动画方式
                    });
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.carousel));
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
                update: function (json) {
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $label.empty();
                    $block.css("margin-left",json.labelWidth);
                    $label.css("width",json.labelWidth);
                    $label.append(json.label + ":");
                    if (json.disabled) {
                        $("#" + json.id).find(".layui-input-block").append('<div class="iceEditor-disabled"></div>');
                    } else {
                        $("#" + json.id).find(".iceEditor-disabled").remove();
                    }
                    colorpicker.render({
                        elem: '#' + json.tag + json.id
                        ,color: json.defaultValue
                        ,format: 'rgb'
                        ,predefine: true
                        ,alpha: true
                        ,done: function (color) {
                            $("#" + json.id).find("input[name=" + json.id + "]").val(color);
                        }
                    });
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.colorpicker));
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
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label);
                    _html += '<div class="layui-input-block">';

                    _html += '<div class="layui-upload">';
                    _html += '<button type="button" class="layui-btn" id="{0}">多图片上传</button>'.format(json.tag + json.id);
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
                    $('#uploadData').val(json[key] == undefined ? '' : json[key]);
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
                update: function (json) {
                    var _disabled = json.disabled ? 'disabled=""' : '';
                    var _required = json.required ? 'lay-verify="required"' : '';
                    var _disabledClass = json.disabled ? ' layui-disabled' : '';
                    var _readonly = json.readonly ? 'readonly=""' : '';
                    var $block = $('#' + json.id + ' .layui-input-block');
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $block.empty();
                    $label.empty();
                    var _html = '';
                    _html += '<textarea name="{0}" placeholder="{3}" width="{2}" class="layui-textarea{6}" {4} {5} {7}>{1}</textarea>'
                        .format(json.id, json.defaultValue ? json.defaultValue : '', json.width, json.placeholder, _disabled, _required, _disabledClass, _readonly);
                    $('#' + json.id + ' .layui-input-block').append(_html);
                    if (json.required) {
                        $label.addClass('layui-form-required');
                    }
                    else {
                        $label.removeClass('layui-form-required');
                    }
                    $label.append(json.label + ":");
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.textarea));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
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
                update: function (json) {
                    var $label = $('#' + json.id + ' .layui-form-label');
                    $label.empty();
                    $label.css("width",json.width);
                    $label.append(json.label + ":");
                    var e = iceEditorObjects[json.id];
                    e.width=json.width;   //宽度
                    e.height=json.height;  //高度
                    e.uploadUrl = json.uploadUrl; //上传文件路径
                    e.urlPrefix = json.urlPrefix;
                    e.uploadData = json.uploadData;
                    e.disabled=json.disabled;
                    e.create();
                },
                /* 获取对象 */
                jsonData: function (id, index, columncount) {
                    //分配一个新的ID 
                    var _json = JSON.parse(JSON.stringify(formField.editor));
                    _json.id = id == undefined ? autoId(_json.tag) : id;
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
                    $('#uploadData').val(json[key] == undefined ? '' : json[key]);
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
                        _html += '<div class="{2} widget-col-list column{0}" data-index="{0}" data-parentindex="{1}">'.format(i, json.index, colClass);
                        //some html
                        _html += '</div>';
                    }
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
                    //如果存在 option 那么向 .option .layui-inline 添加drag事件并且必须设在 select-option-drag 中才能拖动
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
                    var _html = '<div id="{0}" class="layui-form-item {2}"  data-id="{0}" data-tag="{1}" data-index="{3}">'.format(json.id, json.tag, selected ? 'active' : '', json.index);
                    _html += '<label class="layui-form-label {0}">{1}:</label>'.format(json.required ? 'layui-form-required' : '', json.label);
                    _html += '<div class="layui-input-block">';

                    _html += '<div class="layui-upload">';
                    _html += '<button type="button" class="layui-btn layui-btn-normal" id="{0}">选择多文件</button> '.format(json.tag + json.id);
                    _html += ' <div class="layui-upload-list" style="max-width: 1000px;"><table class="layui-table">';
                    _html += '<colgroup><col><col width="150"><col width="260"><col width="150"></colgroup>';
                    _html += '<thead><tr><th>文件名</th><th>大小</th><th>上传进度</th><th>操作</th></tr></thead>';
                    _html += '<tbody id="list-{0}"></tbody></table></div>'.format(json.id);
                    _html += '<button type="button" class="layui-btn" id="listAction-{0}">开始上传</button>'.format(json.id);
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
                    $('#uploadData').val(json[key] == undefined ? '' : json[key]);
                }
            },
        };



        
        /* 如果是grid布局控件 就显示不一样的样式 */
        Class.prototype.addClick = function (evt) {
            var that = this
                , options = that.config;
            $("#formDesignerForm .layui-form-item").on('click', function (e) {
                //当 div 为嵌套关系的时候 阻止事件冒泡
                e.preventDefault();
                e.stopPropagation();
                // console.log("您点击了 formDesignerForm .layui-form-item");
                var index = parseInt($(this)[0].dataset.index);
                var _id = $(this)[0].dataset.id;
                options.selectItem = that.findJsonItem(options.data, _id);
                var tag = $(this)[0].dataset.tag;

                //显示当前的属性
                that.components[tag].property(options.selectItem);
                if (options.selectItem.tag === "select" || options.selectItem.tag === "checkbox"
                    || options.selectItem.tag === "radio") {
                    var sortable = Sortable.create(document.getElementById(options.selectItem.tag), {
                        group: {
                            name: 'propertygroup',
                        },
                        ghostClass: "ghost",
                        handle: '.select-option-drag',
                        dataIdAttr: 'data-index',
                        animation: 150,
                        onEnd: function (evt) {
                            if (options.selectItem === undefined) {
                                return;
                            }
                            var indexArray = sortable.toArray();
                            var newJson = [];
                            for (var i = 0; i < indexArray.length; i++) {
                                newJson.push(options.selectItem.options[indexArray[i]]);
                            }
                            options.selectItem.options = newJson;
                            that.renderForm();
                        }
                    });
                }
                that.bindPropertyEvent();
                //移除其他元素的 siblings() 方法返回被选元素的所有同级元素
                // $(this).siblings('div').removeClass('active');
                //移除 #formDesignerForm .layui-form-item 下所有的 active
                $('#formDesignerForm .layui-form-item').removeClass('active');
                //给当前元素增加class
                $(this).addClass('active');
                var _draghtml1 = '<div class="widget-view-action"  id="widget-view-action"><i class="layui-icon layui-icon-file"></i><i class="layui-icon layui-icon-delete"></i></div><div class="widget-view-drag" id="widget-view-drag"><i class="layui-icon layui-icon-screen-full"></i></div>';
                var len = $(this).children().length;
                if (len <= 12) {
                    //先删除元素
                    $("#formDesignerForm .layui-form-item .widget-view-action").remove();
                    $("#formDesignerForm .layui-form-item .widget-view-drag").remove();
                    // console.log("显示子节点");
                    // console.log($(this).children());
                    if ($('#widget-view-action')) {//已存在
                        $('#widget-view-action').remove();
                    }
                    $(this).children(len - 1).after(_draghtml1);
                }
                /* 向 拷贝 删除 按钮添加 click 动作 */
                that.addCopyDeleteClick();
                //重新渲染
                form.render();
                if (options.data.length != 0) {
                    for (var i = 0; i < options.data.length ; i++) {
                        if (options.data[i].tag === 'grid') {
                            for (var j = 0; j < options.data[i].columns.length; j++) {
                                for (var k = 0; k < options.data[i].columns[j].list.length; k++) {
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
            });

        };

        /* 给字段属性绑定事件 实现双向绑定？*/
        Class.prototype.bindPropertyEvent = function (_json) {
            var that = this
                , options = that.config;
            if (options.data === undefined) {
                return;
            }
            if (typeof (options.data) === 'string') {
                options.data = JSON.parse(options.data);
            }
            var _property = $('#columnProperty');
            var _columns = _property.find('input,textarea,select,checkbox');
            var _json = options.selectItem;
            //没有可以选择的
            if (_json === undefined) {
                return;
            }
            laydate.render({
                elem: '#dataMaxValue' + _json.tag + _json.id,
                format: 'yyyy-MM-dd',
                btns: ['now','confirm'],
                value: _json.dataMaxValue,
                done: function(value, date, endDate){
                    _json.dataMaxValue = value;
                    that.components[_json.tag].update(_json);
                }
            });
            laydate.render({
                elem: '#dataMinValue' + _json.tag + _json.id,
                format: 'yyyy-MM-dd',
                btns: ['now','confirm'],
                value: _json.dataMinValue,
                done: function(value, date, endDate){
                    _json.dataMinValue = value;
                    that.components[_json.tag].update(_json);
                }
            });
            laydate.render({
                elem: '#dateDefaultValue' + _json.tag + _json.id,
                type: _json.datetype,
                format: _json.dateformat,
                value: _json.dateDefaultValue,
                done: function(value, date, endDate){
                    _json.dateDefaultValue = value;
                    that.components[_json.tag].update(_json);
                }
            });
            laydate.render({
                elem: '#dateRangeDefaultValue' + _json.tag + _json.id,
                type: _json.datetype,
                format: _json.dateformat,
                value: _json.dateRangeDefaultValue,
                range:"-",
                done: function(value, date, endDate){
                    _json.dateRangeDefaultValue = value;
                    that.components[_json.tag].update(_json);
                }
            });
            iconPicker.render({
                // 选择器，推荐使用input
                elem: '#' + _json.tag + _json.id + "property",
                // 数据类型：fontClass/unicode，推荐使用fontClass
                type: 'fontClass',
                // 是否开启搜索：true/false，默认true
                search: true,
                // 是否开启分页：true/false，默认true
                page: true,
                // 每页显示数量，默认12
                limit: 12,
                // 每个图标格子的宽度：'43px'或'20%'
                cellWidth: '43px',
                // 点击回调
                click: function (data) {
                    _json.buttonIcon = data.icon;
                    that.components[_json.tag].update(_json);
                },
                // 渲染成功后的回调
                success: function(d) {
                    //console.log(d);
                }
            });
            iconPicker.checkIcon(_json.tag + _json.id + "property", '');
            if (_json.width !== undefined) {
                //定义初始值
                slider.render({
                    elem: '#width',
                    value: _json.width.replace("%",""), //初始值
                    min: 20,
                    max: 100,
                    step: 1,
                    input:true,
                    change: function(value){
                        _json.width = value + "%";
                        that.components[_json.tag].update(_json);
                    }
                });
            }
            if (_json.labelWidth !== undefined) {
                //定义初始值
                slider.render({
                    elem: '#labelWidth',
                    value: _json.labelWidth, //初始值
                    min: 80,
                    max: 300,
                    step: 1,
                    input:true,
                    change: function(value){
                        _json.labelWidth = value;
                        that.components[_json.tag].update(_json);
                    }
                });
            }
            form.on('checkbox', function (data) {
                //data.elem.closest('.layui-form-item')
                if (_json.tag === 'checkbox') {
                    var _index = parseInt($("#" + _json.id + " .layui-input-block div.layui-form-checkbox").index(data.othis[0]));
                    if ($(data.othis[0]).parent().parent().parent().attr("id") === 'checkbox') {
                        _index = parseInt($(data.othis[0]).parent().parent().attr("data-index"));
                    }
                    for (var i = 0; i < _json.options.length; i++) {
                        if (i === _index) {
                            _json.options[i].checked = data.elem.checked;
                            break;
                        }
                    }
                    that.components[_json.tag].update(_json);
                }
            });
            form.on('radio', function (data) {
                //data.elem.closest('.layui-form-item')
                if (_json.tag === 'radio') {
                    var _index = parseInt($("#" + _json.id + " .layui-input-block div.layui-form-radio").index(data.othis[0]));
                    if ($(data.othis[0]).parent().parent().parent().attr("id") === 'radio') {
                        _index = parseInt($(data.othis[0]).parent().parent().attr("data-index"));
                    }
                    for (var i = 0; i < _json.options.length; i++) {
                        if (i === _index) {
                            _json.options[i].checked = true;
                            continue;
                        }
                        _json.options[i].checked = false;
                    }
                    that.components[_json.tag].update(_json);
                } else if (_json.tag === 'select') {
                    var _index = parseInt(data.elem.closest('.layui-form-item').dataset.index);
                    for (var i = 0; i < _json.options.length; i++) {
                        if (i === _index) {
                            _json.options[i].checked = true;
                            continue;
                        }
                        _json.options[i].checked = false;
                    }
                    that.components[_json.tag].update(_json);
                }
            });
            form.on('select', function (data) {
                var _key = data.elem.name;
                var _value = parseInt(data.value);
                var _json = options.selectItem;
                if (_key === 'columns') {
                    var columnCount = _json[_key].length;
                    var nullJson = {
                        span: 12,
                        list: []
                    };
                    if (_value > columnCount) {
                        for (var i = columnCount + 1; i <= _value; i++) {
                            _json[_key].splice(i, 0, nullJson);
                        }
                    } else {
                        _json[_key].splice(_value, columnCount);
                    }
                    that.renderForm();
                } else if (_key === 'dateformat') {
                    if (_json.tag === 'date') {
                        var _html = '<div id="{0}" class="layui-input icon-date widget-date" style="line-height: 40px;"></div>'.format('dateDefaultValue' + _json.tag + _json.id);
                        $('#dateDefaultValue' + _json.id + ' .layui-input-block').empty();
                        $('#dateDefaultValue' + _json.id + ' .layui-input-block').append(_html);
                        _json.dateformat = data.value;
                        var dateClass = laydate.render({
                            elem: '#dateDefaultValue' + _json.tag + _json.id,
                            type: _json.datetype,
                            format: _json.dateformat,
                            value: new Date(),
                            done: function(value, date, endDate){
                                _json.dateDefaultValue = value;
                                that.components[_json.tag].update(_json);
                            }
                        });
                        _json.dateDefaultValue = dateClass.config.elem[0].innerText;
                        that.components[_json.tag].update(_json);
                    }
                    if (_json.tag === 'dateRange') {
                        var _html = '<div id="{0}" class="layui-input icon-date widget-date"></div>'.format('dateDefaultValue' + _json.tag + _json.id);
                        $('#dateDefaultValue' + _json.id + ' .layui-input-block').empty();
                        $('#dateDefaultValue' + _json.id + ' .layui-input-block').append(_html);
                        _json.dateformat = data.value;
                        var _v = layui.util.toDateString(new Date(), _json.dateformat) + " - " + layui.util.toDateString(new Date(), _json.dateformat);
                        laydate.render({
                            elem: '#dateRangeDefaultValue' + _json.tag + _json.id,
                            type: _json.datetype,
                            format: _json.dateformat,
                            value:  _v,
                            range:"-",
                            done: function(value, date, endDate){
                                _json.dateRangeDefaultValue = value;
                                that.components[_json.tag].update(_json);
                            }
                        });
                        _json.dateRangeDefaultValue = _v;
                        that.components[_json.tag].update(_json);
                    }
                } else if (_key === 'datetype') {
                    if (_json.tag === 'date') {
                        var _html = '<div id="{0}" class="layui-input icon-date widget-date" style="line-height: 40px;"></div>'.format('dateDefaultValue' + _json.tag + _json.id);
                        $('#dateDefaultValue' + _json.id + ' .layui-input-block').empty();
                        $('#dateDefaultValue' + _json.id + ' .layui-input-block').append(_html);
                        _json.datetype = data.value;
                        var dateClass = laydate.render({
                            elem: '#dateDefaultValue' + _json.tag + _json.id,
                            type: _json.datetype,
                            format: _json.dateformat,
                            value: new Date(),
                            done: function(value, date, endDate){
                                _json.dateDefaultValue = value;
                                that.components[_json.tag].update(_json);
                            }
                        });
                        _json.dateDefaultValue = dateClass.config.elem[0].innerText;
                        that.components[_json.tag].update(_json);
                    }
                    if (_json.tag === 'dateRange') {
                        var _html = '<div id="{0}" class="layui-input icon-date widget-date"></div>'.format('dateDefaultValue' + _json.tag + _json.id);
                        $('#dateDefaultValue' + _json.id + ' .layui-input-block').empty();
                        $('#dateDefaultValue' + _json.id + ' .layui-input-block').append(_html);
                        _json.datetype = data.value;
                        laydate.render({
                            elem: '#dateRangeDefaultValue' + _json.tag + _json.id,
                            type: _json.datetype,
                            format: _json.dateformat,
                            value: _json.dateRangeDefaultValue,
                            range:"-",
                            done: function(value, date, endDate){
                                _json.dateRangeDefaultValue = value;
                                that.components[_json.tag].update(_json);
                            }
                        });
                        that.components[_json.tag].update(_json);
                    }
                }else if (_key === 'required') {
                    _json.expression = data.value;
                    that.components[_json.tag].update(_json);
                }else if (_key === 'expression') {
                    _json.expression = data.value;
                    that.components[_json.tag].update(_json);
                } else if (_key === 'anim' || _key === 'arrow') {
                    _json[data.elem.name] = data.value;
                    that.components[_json.tag].update(_json);
                }else if (_key === 'buttonType') {
                    _json.buttonType = data.value;
                    that.components[_json.tag].update(_json);
                }else if (_key === 'buttonSize') {
                    _json.buttonSize = data.value;
                    that.components[_json.tag].update(_json);
                }
            });
            form.on('switch', function (data) {
                var _key = data.elem.name;
                var _value = data.elem.checked ? true : false;
                if (_key === 'readonly' || _key == 'disabled' || _key === 'required' || _key === 'half' || _key === 'text' || _key === 'switchValue' || _key === 'isInput'
                    || _key == 'iconPickerSearch' || _key === 'iconPickerPage' || _key === 'isEnter'|| _key === 'isLabel') {
                    _json[_key] = _value;
                    that.components[_json.tag].update(_json);//局部更新
                }
            });
            //options 的添加事件
            if (_json.hasOwnProperty('options')) {
                $('#select-option-add').on('click', function () {
                    //添加html
                    _json.options.splice(_json.options.length + 1, 0, {text: 'option', value: 'value', checked: false});
                    var _htmloption = '';
                    _htmloption += '<div class="layui-form-item option select-options" data-index="{0}">'.format(_json.options.length - 1);
                    _htmloption += '  <div class="layui-inline" style="width: 30px; margin-right: 0px;">';
                    if (_json.tag === 'checkbox') {
                        _htmloption += '   <input type="checkbox" name="{0}" lay-skin="primary" title="">'.format(_json.tag);
                    } else {
                        _htmloption += '   <input type="radio" name="{0}" >'.format(_json.tag);
                    }
                    _htmloption += '  </div>';
                    _htmloption += '  <div class="layui-inline" style="margin-right: 0px;width:110px;">';
                    _htmloption += '   <input type="text" name="{0}-text"  autocomplete="off" value="{1}" class="layui-input">'.format(_json.tag, 'option');
                    _htmloption += '  </div>';
                    _htmloption += '  <div class="layui-inline" style="margin-right: 0px;width:110px;">';
                    _htmloption += '   <input type="text" name="{0}-value"  autocomplete="off" value="{1}" class="layui-input">'.format(_json.tag, 'value');
                    _htmloption += '  </div>';
                    _htmloption += '  <div class="layui-inline" style="margin-right: 0px;">';
                    _htmloption += '   <i class="layui-icon layui-icon-slider select-option-drag" style="color:blue;font-size:20px;"></i>';
                    _htmloption += '   <i class="layui-icon layui-icon-delete select-option-delete" style="color:red;font-size:20px;"></i>';
                    _htmloption += '  </div>';
                    _htmloption += '</div>';
                    $('#columnProperty .select-options').last().after(_htmloption);
                    _html = '';
                    if (_json.tag === 'checkbox') {
                        //同步到设计视图checkbox
                        _html += '<input type="checkbox" name="{0}" title="{1}" >'.format(_json.tag, 'option');
                        $('#' + _json.id + ' .layui-input-block').append(_html);
                    } else if (_json.tag === 'radio') {
                        //同步到设计视图radio
                        _html += '<input type="radio" name="{0}" title="{1}" >'.format(_json.tag, 'option');
                        $('#' + _json.id + ' .layui-input-block').append(_html);
                    } else if (_json.tag === 'carousel') {

                    }
                    if (_json.tag === 'checkbox') {
                        form.render('checkbox');
                    } else if (_json.tag === 'radio') {
                        form.render('radio');
                    } else if (_json.tag == 'carousel') {
                        that.components[_json.tag].update(_json);
                        form.render('radio');
                        carousel.render({
                            elem: '#' + item.tag + item.id,
                            width: item.width,//设置容器宽度
                            arrow: item.arrow, //始终显示箭头
                            //anim: 'updown' //切换动画方式
                        });
                    } else if (_json.tag == 'select') {
                        form.render('select');
                        form.render('radio');
                    }

                });

                //委托监听先关闭在增加 click
                $(document).off('click', '#columnProperty .select-option-delete').on('click', '#columnProperty .select-option-delete', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    //从数据源 options.data 中删除节点
                    if (_json.options.length <= 1) {
                        layer.msg('已达到最低选项，不能继续删除');
                        return;
                    }
                    var _index = $(this).closest('.layui-form-item')[0].dataset.index;
                    if (_index !== undefined) {
                        _json.options.splice(_index, 1);//删除此节点
                    }
                    var checkedDefual = true;
                    for (var i = 0; i < _json.options.length; i++) {
                        if (_json.options[i].checked) {
                            vv = false
                        }
                    }
                    if (checkedDefual) {
                        _json.options[0].checked = true;
                    }
                    //从html中删除节点 改成全部重绘
                    // $(this).closest('.layui-form-item').remove()
                    $('#' + _json.tag).empty();
                    //删除所有的选项
                    //选项
                    var _html = '';
                    for (var i = 0; i < _json.options.length; i++) {
                        _html += '<div class="layui-form-item option select-options" data-index="{0}">'.format(i);
                        _html += '  <div class="layui-inline" style="width: 30px; margin-right: 0px;">';
                        if (_json.tag === 'checkbox') {
                            if (_json.options[i].checked) {
                                _html += '   <input type="checkbox" name="{0}" lay-skin="primary" title="" checked>'.format(_json.tag);
                            } else {
                                _html += '   <input type="checkbox" name="{0}" lay-skin="primary" title="">'.format(_json.tag);
                            }
                        } else {
                            if (_json.options[i].checked) {
                                _html += '   <input type="radio" name="{0}" checked>'.format(_json.tag);
                            } else {
                                _html += '   <input type="radio" name="{0}">'.format(_json.tag);
                            }
                        }
                        _html += '  </div>';
                        _html += '  <div class="layui-inline" style="margin-right: 0px;width:110px;">';
                        _html += '   <input type="text" name="{0}-text"  autocomplete="off" value="{1}" class="layui-input">'.format(_json.tag, _json.options[i].text);
                        _html += '  </div>';
                        _html += '  <div class="layui-inline" style="margin-right: 0px;width:110px;">';
                        _html += '   <input type="text" name="{0}-value"  autocomplete="off" value="{1}" class="layui-input">'.format(_json.tag, _json.options[i].value);
                        _html += '  </div>';
                        _html += '  <div class="layui-inline" style="margin-right: 0px;">';
                        _html += '   <i class="layui-icon layui-icon-slider select-option-drag" style="color:blue;font-size:20px;"></i>';
                        _html += '   <i class="layui-icon layui-icon-delete select-option-delete" style="color:red;font-size:20px;"></i>';
                        _html += '  </div>';
                        _html += '</div>';
                    }
                    $('#' + _json.tag).append(_html);
                    //更新设计区节点
                    that.components[_json.tag].update(_json);
                    if (_json.tag === 'checkbox') {
                        form.render('checkbox');
                    } else if (_json.tag === 'radio') {
                        form.render('radio');
                    } else if (_json.tag == 'select') {
                        form.render('select');
                        form.render('radio');
                    }
                    //向 .option .layui-inline 添加drag事件并且必须设在 select-option-drag 中才能拖动


                });
            }

            $(document).off('click', '.select-options input[type=checkbox]').on('click', '.select-options input[type=checkbox]', function (e) {
                // console.log(e);
                //判断是否选中
                //找到 id=key 下的 option值
                var _options = [];
                //遍历options 的值
                $('#columnProperty .select-options').each(function () {
                    _options.push({
                        text: $(this).find('input[name=select-text]').val(),
                        value: $(this).find('input[name=select-value]').val()
                    });
                    // console.log($(this));
                });

                //_json.options 节点值替换为 _options 值
                _json.options = JSON.parse(JSON.stringify(_options));
            });


            //属性模块的 input keyup
            $(document).off('keyup', '#columnProperty .layui-input').on('keyup', '#columnProperty .layui-input', function () {
                if ($(this).attr("name") !== undefined) {
                    //去改变值
                    //改变json的值
                    var _key = $(this).attr("name");
                    var _value = $(this).val();
                    var _json = options.selectItem;
                    if (_key === 'id') {
                        return;
                    }
                    if (_key === 'label' || _key === 'width' || _key === 'interval'
                        || _key === 'iconPickerLimit'  || _key === 'iconPickerCellWidth'
                        || _key === 'buttonVlaue') {
                        _json[_key] = _value;
                        that.components[_json.tag].update(_json);//局部更新
                    }
                    if (_key === 'uploadUrl' || _key === 'uploadData' || _key === 'urlPrefix' ||_key ===  'cronUrl') {
                        _json[_key] = _value;
                    }
                    if (_key === 'placeholder') {
                        _json[_key] = _value;
                        $('#' + _json.id).find('.layui-input').attr('placeholder', _value);
                    }
                    if (_key === 'height') {
                        _json[_key] = _value;
                        if (_json.tag === 'editor') {
                            that.components[_json.tag].update(_json);//局部更新
                        }
                        if (_json.tag === 'carousel') {
                            _json[_key] = _value;
                            that.components[_json.tag].update(_json);//局部更新
                        }
                    }
                    if (_key == 'uploadImage') {
                        _json[_key] = _value;
                        layedit.build(_json.tag + _json.id, {
                            height: _json['height'],
                            uploadImage: _value
                        }); //建立编辑器
                    }
                    if (_key === 'defaultValue') {
                        _json[_key] = _value;
                        if (_json.tag === 'slider') {
                            var resultNumber = that.replaceNumber(_value);
                            _json[_key] = resultNumber;
                            $(this).val(resultNumber);
                            slider.render({
                                elem: '#' + _json.tag + _json.id,
                                value: _json.defaultValue, //初始值
                                min: _json.minValue,
                                max: _json.maxValue,
                                step: _json.stepValue,
                                disabled: _json.disabled
                            });
                        } else if (_json.tag === 'rate') {
                            rate.render({
                                elem: '#' + _json.tag + _json.id,
                                value: _json.defaultValue,
                                text: _json.text,
                                length: _json.rateLength,
                                half: _json.half,
                                readonly: _json.readonly
                            });
                        } else if (_json.tag == 'textarea') {
                            $('#' + _json.id).find('.layui-textarea').text(_value);
                        } else if (_json.tag == 'colorpicker') {
                            that.components[_json.tag].update(_json);//局部更新
                        } else if (_json.tag == 'iconPicker') {
                            that.components[_json.tag].update(_json);//局部更新
                        } else if (_json.tag == 'numberInput') {
                            var resultNumber = that.replaceNumber(_value);
                            _json[_key] = resultNumber;
                            $(this).val(resultNumber);
                            that.components[_json.tag].update(_json);//局部更新
                        } else {
                            $('#' + _json.id).find('.layui-input').val(_value);
                        }
                    }
                    if (_key === "minValue" || _key === "maxValue" || _key === "stepValue") {
                        var resultNumber = that.replaceNumber(_value);
                        _json[_key] = resultNumber;
                        $(this).val(resultNumber);
                        if (_json.tag === 'slider') {
                            slider.render({
                                elem: '#' + _json.tag + _json.id,
                                value: _json.defaultValue, //初始值
                                min: _json.minValue,
                                max: _json.maxValue,
                                step: _json.stepValue,
                                disabled: _json.disabled
                            });
                        }else if (_json.tag == 'numberInput') {
                            that.components[_json.tag].update(_json);//局部更新
                        }
                    }
                    if (_key === 'rateLength') {
                        _json[_key] = _value;
                        if (_json.tag === 'rate') {
                            rate.render({
                                elem: '#' + _json.tag + _json.id,
                                value: _json.defaultValue,
                                text: _json.text,
                                length: _json.rateLength,
                                half: _json.half,
                                readonly: _json.readonly
                            });
                        }
                    }
                    if (_key === 'document') {
                        _json[_key] = _value;
                    }
                    if (_key === 'readonly') {
                        _json[_key] = _value;
                        $('#' + _json.id).find('.layui-input').attr('readonly', _value);
                    }
                    if (_key === 'select-text' || _key === 'select-value' || _key === 'radio-text' || _key === 'radio-value' || _key === 'checkbox-text' || _key === 'checkbox-value') {
                        //找到 id=key 下的 option值
                        var _index = parseInt($(this).parent().parent().attr("data-index"));
                        if (_key === 'select-text' || _key === 'radio-text' || _key === 'checkbox-text') {
                            _json.options[_index].text = $(this).val();
                        } else {
                            _json.options[_index].value = $(this).val();
                        }
                        that.components[_json.tag].update(_json);//局部更新
                    }
                    if (_key === 'carousel-text' || _key === 'carousel-value') {
                        //找到 id=key 下的 option值
                        var _options = [];
                        //遍历options 的值
                        $('#columnProperty .select-options').each(function () {
                            _options.push({
                                text: $(this).find('input[name=carousel-text]').val(),
                                value: $(this).find('input[name=carousel-value]').val()
                            });
                            // console.log($(this));
                        });
                        //_json.options 节点值替换为 _options 值
                        _json.options = JSON.parse(JSON.stringify(_options));
                        that.components[_json.tag].update(_json);//局部更新
                    }
                }
            });

            $(document).off('blur', '#columnProperty .layui-input').on('blur', '#columnProperty .layui-input', function () {
                if ($(this).attr("name") !== undefined) {
                    //改变json的值
                    var _key = $(this).attr("name");
                    var _value = $(this).val();
                    var _json = options.selectItem;
                    var _oldid = _json.id;
                    if (_key === 'id' && _value !== _oldid) {//标识的更改
                        //检测id是否存在重复
                        var _checkid = that.findJsonItem(options.data, _value);
                        if (_checkid === undefined) {
                            _json[_key] = _value;
                            that.renderForm();
                        } else {
                            //提示层
                            layer.msg('ID已经存在');
                        }
                    }
                }
            });

            //预览


        }


        //递归赋值
        Class.prototype.replaceNumber = function (value) {
            value = value.replace(/[^\d]/g,'');
            if(''!=value){
                value = parseInt(value);
            }
            return value;
        }

        /* 加入copy选项删除 */
        Class.prototype.addCopyDeleteClick = function () {
            var that = this
                , options = that.config;
            if (options.data === undefined) {
                return;
            }
            if (typeof (options.data) === 'string') {
                options.data = JSON.parse(options.data);
            }
            //复制当前节点
            $('#formDesignerForm  .layui-form-item .widget-view-action .layui-icon-file').on('click', function (e) {
                e.stopPropagation();
                //在json中插入
                if (options.data === undefined) {
                    return;
                }
                if (typeof (options.data) === 'string') {
                    options.data = JSON.parse(options.data);
                }
                var _id = document.elementFromPoint(e.pageX, e.pageY).parentElement.parentElement.dataset.id;
                if (_id !== undefined) {
                    options.selectItem = that.copyJsonAfterItem(options.data, _id);
                }

                that.renderForm();
            });
            $('#formDesignerForm  .layui-form-item .layui-icon-delete').on('click', function (e) {
                e.stopPropagation();
                //获取当前组件的组件id
                var _id = document.elementFromPoint(e.pageX, e.pageY).parentElement.parentElement.dataset.id;
                if (_id !== undefined) {
                    options.selectItem = that.deleteJsonItem(options.data, _id);
                    delete iceEditorObjects["editor" + _id]
                }
                //document.elementFromPoint(e.pageX,e.pageY).parentElement.parentElement.parentElement.dataset 获取父grid的信息

                that.renderForm();
                // console.log('click layui-icon layui-icon-delete');
            });
        };


        /* 触发 grid 的 sortablejs 事件*/
        Class.prototype.bindGridSortEvent = function (json) {
            var that = this
                , options = that.config;
            var objs = $('#' + json.id + ' .widget-col-list');
            //遍历他下面的节点
            for (var i = 0; i < objs.length; i++) {
                var el = objs[i];
                var ops = {
                    group: {
                        name: 'formgroup'
                    },
                    handle: '.widget-view-drag',
                    ghostClass: "ghost",
                    animation: 150,
                    onAdd: function (evt) {
                        var parentItem = JSON.parse(JSON.stringify(that.findJsonItem(options.data, evt.item.parentElement.parentElement.dataset.id)));
                        var index = evt.newIndex;
                        var colIndex = evt.item.parentElement.dataset.index;
                        if (evt.item.dataset.id != undefined) {
                            //表示从其他地方移动过来
                            var _fromItem = JSON.parse(JSON.stringify(that.findJsonItem(options.data, evt.item.dataset.id)));
                            var _oldid = _fromItem.id;
                            _fromItem.id = that.autoId(_fromItem.tag);
                            _fromItem.index = index;
                            parentItem.columns[colIndex].list.splice(index + 1, 0, _fromItem);
                            that.findAndCopyJson(options.data,parentItem,evt.item.parentElement.parentElement.dataset.id);
                            that.deleteJsonItem(options.data, _oldid);
                        } else {
                            /* 向指定目标放入数据 splice */
                            var tag = evt.item.dataset.tag;
                            _id = that.autoId(tag);
                            var _newitem = that.components[tag].jsonData(_id, evt.newIndex);
                            _newitem.index = index;
                            parentItem.columns[colIndex].list.splice(index + 1, 0, _newitem);
                            that.findAndCopyJson(options.data,parentItem,evt.item.parentElement.parentElement.dataset.id);
                            options.selectItem = _newitem;
                        }
                        that.renderForm();

                    },
                    //拖动结束
                    onEnd: function (evt) {
                        //console.log(evt);
                    }};
                var gridSortable = Sortable.create(el,ops);
            }

        };

        //递归赋值
        Class.prototype.findAndCopyJson = function (json,parentItem,id) {
            var that = this;
            for (var i = 0; i < json.length; i++) {
                if (json[i].id === id) {
                    json[i] = parentItem;
                } else {
                    if (json[i].tag === 'grid') {
                        for (var j = 0; j < json[i].columns.length; j++) {
                            if (json[i].columns[j].list.length > 0) {
                                that.findAndCopyJson(json[i].columns[j].list,parentItem,id);
                            }
                        }
                    }
                }
            }
        }

        //渲染视图
        Class.prototype.render = function () {
            var that = this
                , options = that.config;
            options.elem = $(options.elem);
            options.id = options.id || options.elem.attr('id') || that.index;

            //cache 模式

            /* 输入型组件 开始*/
            var _listhtml = ""
            _listhtml += '<div class="components-title">{0} </div>'.format(formField.c1.name);
            _listhtml += '<div class="components-draggable" id="c1">';
            $.each(formField.c1.list
                , function (index
                    , item) {
                    _listhtml += '<div class="components-item" data-tag="{0}"><div class="components-body"><i class="icon iconfont icon-input"></i>{1}</div></div>'.format(item
                        , lang[item]);
                });
            _listhtml += '</div>';

            _listhtml += '<div class="components-title">{0} </div>'.format(formField.c2.name);
            _listhtml += '<div class="components-draggable" id="c2">';

            /* 选择型组件 开始*/
            $.each(formField.c2.list
                , function (index
                    , item) {
                    _listhtml += '<div class="components-item" data-tag="{0}"><div class="components-body"><i class="icon iconfont icon-input"></i>{1}</div></div>'.format(item
                        , lang[item]);
                });
            _listhtml += '</div>';

            _listhtml += '<div class="components-title">{0} </div>'.format(formField.c3.name);
            _listhtml += '<div class="components-draggable" id="c3">';

            /* 布局型组件 开始*/
            $.each(formField.c3.list
                , function (index
                    , item) {
                    _listhtml += '<div class="components-item" data-tag="{0}"><div class="components-body"><i class="icon iconfont icon-input"></i>{1}</div></div>'.format(item
                        , lang[item]);
                });
            _listhtml += '</div>';

            _listhtml += '<div class="components-title">{0} </div>'.format(formField.c4.name);
            _listhtml += '<div class="components-draggable" id="c4">';
            /* 选择型组件 开始*/
            $.each(formField.c4.list
                , function (index
                    , item) {
                    _listhtml += '<div class="components-item" data-tag="{0}"><div class="components-body"><i class="icon iconfont icon-input"></i>{1}</div></div>'.format(item
                        , lang[item]);
                });
            _listhtml += '</div>';

            _listhtml += '<div class="components-title">{0} </div>'.format(formField.c5.name);
            _listhtml += '<div class="components-draggable" id="c5">';
            /* 选择型组件 开始*/
            $.each(formField.c5.list
                , function (index
                    , item) {
                    _listhtml += '<div class="components-item" data-tag="{0}"><div class="components-body"><i class="icon iconfont icon-input"></i>{1}</div></div>'.format(item
                        , lang[item]);
                });
            _listhtml += '</div>';

            options.elem.html(TP_MAIN);
            $('#components-form-list').append(_listhtml);
            $('body').append(TP_HTML_VIEW);
            $('body').append(TP_IMPORT_VIEW);//TP_IMPORT_VIEW
            $('body').append(TP_ABOUT_VIEW);
            if (!!options.formStyle) {
                $('#designerForm').addClass('layui-form-pane');
            }
            else {
                $('#designerForm').removeClass('layui-form-pane');
            }
            //排序事件注册
            var sortable1 = Sortable.create(document.getElementById("c1"), {
                group: {
                    name: 'formgroup',
                    pull: 'clone', //克隆本区域元素到其他区域
                    put: false, //禁止本区域实现拖动或拖入
                },
                ghostClass: "ghost",
                sort: false,
                animation: 150,
                onEnd: function (evt) {
                    // console.log('onEnd.foo:', [evt.item, evt.from]);
                    // console.log(evt.oldIndex);
                    // console.log(evt.newIndex);
                    var itemEl = evt.item;
                    // console.log(itemEl);
                }
            });
            var sortable2 = Sortable.create(document.getElementById("c2"), {
                group: {
                    name: 'formgroup',
                    pull: 'clone',
                    put: false, //禁止本区域实现拖动或拖入
                },
                sort: false,
                animation: 150
            });
            var sortable4 = Sortable.create(document.getElementById("c4"), {
                group: {
                    name: 'formgroup',
                    pull: 'clone',
                    put: false, //禁止本区域实现拖动或拖入
                },
                sort: false,
                animation: 150
            });
            var sortable3 = Sortable.create(document.getElementById("c3"), {
                group: {
                    name: 'formgroup',
                    pull: 'clone',
                    put: false, //禁止本区域实现拖动或拖入
                },
                sort: false,
                animation: 150
            });
            var sortable5 = Sortable.create(document.getElementById("c5"), {
                group: {
                    name: 'formgroup',
                    pull: 'clone',
                    put: false, //禁止本区域实现拖动或拖入
                },
                sort: false,
                animation: 150
            });
            //formDesignerForm
            var formItemSort = Sortable.create(document.getElementById("formDesignerForm"), {
                group: {
                    name: 'formgroup'
                },
                handle: '.widget-view-drag',
                ghostClass: "ghost",
                animation: 200,
                /**
                 * 不同元素之间的拷贝
                 * 如果是从组件区域则是创建一个新的节点
                 * 如果是从设计区则是移动一个节点
                 *  */
                onAdd: function (evt) {
                    var columncount = evt.item.dataset.columncount;
                    if (columncount === undefined) {
                        columncount = 2;
                    }
                    if (options.data === undefined) {
                        return;
                    }
                    if (typeof (options.data) === 'string') {
                        options.data = JSON.parse(options.data);
                    }
                    //注意这里的一个bug，newIndex 第一次拖动也是1 第二次拖动也是1
                    if (options.data.length === 0) {
                        evt.newIndex = 0;
                    }
                    if (evt.item.dataset.id !== undefined) {
                        /*根据id的新算法 复制一份副本 删除json中的节点 再插入节点*/
                        var _item = that.findJsonItem(options.data, evt.item.dataset.id);
                        options.selectItem = _item;
                        that.deleteJsonItem(options.data, evt.item.dataset.id);
                        options.data.splice(evt.newIndex + 1, 0, _item);
                    } else {
                        var _id = that.autoId(evt.item.dataset.tag);
                        /* 向现有的表单数据中增加新的数组元素 splice */
                        var _newitem = that.components[evt.item.dataset.tag].jsonData(_id, evt.newIndex, columncount);
                        //如果是 grid 呢，需要知道几列
                        options.selectItem = _newitem;
                        options.data.splice(evt.newIndex, 0, _newitem);//options.data.splice(evt.newIndex + 1, 0, _newitem);
                        /* 如果evt.item.dataset.id 有值表示从已经存在的布局中获取了元素，这个时候要在数据源中删除这个元素*/
                    }

                    //局部更新 只要更新 designer 设计区部分
                    that.renderForm();
                },
                onEnd: function (evt) {
                    var newIndex = evt.newIndex;
                    var oldIndex = evt.oldIndex;
                    //只有当to的目标容器是 formDesignerForm 才出发次逻辑
                    if (evt.to.id === 'formDesignerForm') {
                        that.moveItem(evt.oldIndex, evt.newIndex);
                    }
                }
            });
            //移动视图
            Class.prototype.moveItem = function (oldIndex,newIndex) {
                var that = this
                    , options = that.config;
                var newData = options.data[newIndex];
                var oldData = options.data[oldIndex];
                options.data[newIndex] = oldData;
                options.data[oldIndex] = newData;
            }
            //导出数据
            $('.exportJson').on('click', function () {
                document.getElementById('generate-code-view').value = JSON.stringify(options.data, null, 4);

                layer.open({
                    type: 1
                    , title: 'JSON 数据导出'
                    , id: 'Lay_layer_htmlcodeview'
                    , content: $('.htmlcodeview')
                    , area: ['500px', '400px']
                    , shade: false
                    , resize: false
                    , success: function (layero, index) {

                    }
                    , end: function () {
                        $('.htmlcodeview').css("display","none")
                    }
                });
            });
            //模板数据
            $('.templateList').on('click', function () {
                $.ajax({
                    url: document.location.origin + '/json/demo.json',
                    dataType: "json",
                    async: true,
                    type: "GET",
                    success: function (result) {
                        options.data = result;
                        that.renderForm();
                    }
                });
            });
            //导入数据
            $('.importJson').on('click', function () {
                document.getElementById('import-json-code').value = JSON.stringify(options.data, null, 4);

                layer.open({
                    type: 1
                    , title: 'JSON模板数据导入'
                    , id: 'Lay_layer_importjsoncodeview'
                    , content: $('.importjsoncodeview')
                    , area: ['500px', '400px']
                    , shade: false
                    , resize: false
                    , success: function (layero, index) {
                    }
                    , end: function () {
                        $('.importjsoncodeview').css("display","none")
                    }
                });
            });
            $('.aboutForm').on('click', function () {

                layer.open({
                    type: 1
                    , title: '关于 Layui表单设计器'
                    , id: 'Lay_layer_aboutusview'
                    , content: $('.aboutusview')
                    , area: ['500px', '400px']
                    , shade: false
                    , resize: false
                    , success: function (layero, index) {

                    }
                    , end: function () {
                    }
                });
            });
            $('#copy-html-code').on('click', function () {
                var Url2 = document.getElementById("generate-code-view");
                Url2.select(); // 选择对象
                document.execCommand("Copy"); // 执行
                layer.msg('复制成功');
            });
            $('#import-json-code').on('click', function () {
                var _value = document.getElementById("import-json-code-view").value;
                options.data = JSON.parse(_value);
                that.renderForm();
                layer.closeAll();
                layer.msg('导入成功');
            });
            $('.generateCode').on('click', function () {
                options.htmlCode.script = '';
                var _htmlelem = $('<div style="height:100%;width:100%;"></div>');
                that.generateHtml(options.data, _htmlelem);
                //构件 html  
                var TP_HTML_CODE = ['<!DOCTYPE html>'
                    , '<html>'
                    , '<head>'
                    , '<meta charset="utf-8">'
                    , '<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">'
                    , '<title>表单设计器代码</title>'
                    , '<link rel="stylesheet" href="~/lib/layui/css/layui.css" />'
                    , '<link rel="stylesheet" href="~/js/lay-module/formDesigner/formDesigner.css" />\n' +
                    '  <link rel="stylesheet" href="~/js/lay-module/cron/cron.css" />\n' +
                    '  <link rel="stylesheet" href="~/js/lay-module/labelGeneration/labelGeneration.css" />\n' +
                    '  <link rel="stylesheet" href="~/js/lay-module/formDesigner/formPreview.css" />'
                    , '</head>'
                    , '<body>'
                    , '<div id="testdemo" style="margin: 10px 20px;"><form  class="layui-form" style="height:100%;" id="formPreviewForm">'
                    , '' + _htmlelem.html() + ''
                    , '<div class="layui-form-item">'
                    , '<div class="layui-input-block">'
                    , '<button type="submit" class="layui-btn" lay-submit="" lay-filter="formPreviewForm">提交</button>'
                    , '<button type="reset" class="layui-btn layui-btn-primary">重置</button>'
                    , '</div>'
                    , '</div>'
                    , '</form></div>'
                    , '<script type="text/javascript" src="~/lib/layui/layui.js"></script>'
                    , '<script type="text/javascript" src=  "~/js/Sortable/Sortable.js"> </script>\n' +
                    '  <script type="text/javascript" src="~/js/lay-module/iceEditor/iceEditor.js"></script>\n' +
                    '  <script type="text/javascript" src="~/js/config.js?v=100"></script>'
                    , '<script>'
                    , 'layui.use(["layer", "laytpl", "element", "form", "slider", "laydate", "rate", "colorpicker", "layedit", "carousel", "upload", "formField","numberInput","iconPicker", "cron","labelGeneration"], function () {'
                    , 'var $ = layui.jquery'
                    , ', layer = layui.layer\n' +
                    '  , laytpl = layui.laytpl\n' +
                    '  , setter = layui.cache\n' +
                    '  , element = layui.element\n' +
                    '  , slider = layui.slider\n' +
                    '  , laydate = layui.laydate\n' +
                    '  , rate = layui.rate\n' +
                    '  , colorpicker = layui.colorpicker\n' +
                    '  , carousel = layui.carousel\n' +
                    '  , form = layui.form\n' +
                    '  , upload = layui.upload\n' +
                    '  , layedit = layui.layedit\n' +
                    '  , formField = layui.formField\n' +
                    '  , hint = layui.hint\n' +
                    '  , numberInput = layui.numberInput\n' +
                    '  , iconPicker = layui.iconPicker\n' +
                    '  , cron = layui.cron\n' +
                    '  , labelGeneration = layui.labelGeneration;'
                    , options.htmlCode.script
                    , '});'
                    , '</script>'
                    , '</body>'
                    , '</html>'].join('')
                var tabsize = 4;
                var tabchar = ' ';
                if (tabsize == 1) {
                    tabchar = '\t';
                }


                document.getElementById('generate-code-view').value = style_html(TP_HTML_CODE, tabsize, tabchar, 400);
                layer.open({
                    type: 1
                    , title: 'HTML代码'
                    , id: 'Lay_layer_htmlcodeview'
                    , content: $('.htmlcodeview')
                    , area: ['500px', '400px']
                    , shade: false
                    , resize: false
                    , success: function (layero, index) {
                        layer.style(index, {
                            marginLeft: -220
                        });
                    }
                    , end: function () {
                        $('.htmlcodeview').css("display","none")
                    }
                });

            });
            $('.previewForm').on('click', function () {
                window.localStorage.setItem('layui_form_json', JSON.stringify(options.data));
                layer.confirm('请选择你要预览页面的方式？', {
                    btn: ['弹窗','新页面'] //按钮
                }, function(){
                    //iframe窗
                    layer.open({
                        type: 2,
                        title: '表单预览',
                        btn: ['关闭'], //可以无限个按钮
                        btn1: function (index, layero) {
                            layer.close(index);
                        },
                        closeBtn: 1, //不显示关闭按钮
                        shade: [0],
                        area: ['100%', '100%'],
                        offset: 'auto', //右下角弹出
                        anim: 2,
                        content: ['./designerPreview.html', 'yes'], //iframe的url，no代表不显示滚动条
                        end: function () { //此处用于演示
                            //加载结束
                        }
                    });
                }, function(){
                    window.open("./preview.html");
                });

            });
            $('.saveJson').on('click', function () {
                window.localStorage.setItem('layui_form_json', JSON.stringify(options.data));
            });
            that.renderForm();
        };


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
                        that.bindPropertyEvent(item);
                    } else {
                        elem.append(that.components[item.tag].render(item, false));
                    }
                }
                if (item.tag === 'grid') {
                    that.bindGridSortEvent(item);
                    $.each(item.columns, function (index2, item2) {
                        //获取当前的 DOM 对象
                        if (item2.list.length > 0) {
                            var elem2 = $('#' + item.id + ' .widget-col-list').filter('.column' + index2);
                            if (item2.list.length > 0) {
                                that.renderComponents(item2.list, elem2);
                            }
                        }
                    });
                }  else if (item.tag === 'slider') {
                    //定义初始值
                    slider.render({
                        elem: '#' + item.tag + item.id,
                        value: item.defaultValue, //初始值
                        min: item.minValue,
                        max: item.maxValue,
                        step: item.stepValue,
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
                } else if (item.tag === 'date') {
                    laydate.render({
                        elem: '#' + item.tag + item.id,
                        type: item.datetype,
                        format: item.dateformat,
                        value: item.dateDefaultValue,
                        min: item.dataMinValue,
                        max: item.dataMaxValue,
                    });
                } else if (item.tag === 'labelGeneration') {
                    labelGeneration.render({
                        elem:'#' + item.tag + item.id,
                        data:item.dateDefaultValue,
                        isEnter:item.isEnter
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
                                item.data = data;
                                that.components[item.tag].update(item);
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
                } else if (item.tag === 'iconPicker') {
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
                    iconPicker.checkIcon(item.tag + item.id, '');
                } else if (item.tag === 'dateRange') {
                    laydate.render({
                        elem: '#' + item.tag + item.id,
                        type: item.datetype,
                        format: item.dateformat,
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
                } else if (item.tag === 'cron' && !item.disabled) {
                    cron.render({
                        elem: "#" + item.tag + item.id + "-button", // 绑定元素
                        url: item.cronUrl, // 获取最近运行时间的接口
                        // value: $("#cron").val(), // 默认值
                        done: function (cronStr) {
                            $("#" + item.tag + item.id).val(cronStr);
                        },
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
                            //譬如你可以在回调中把得到的 color 赋值给表单
                        }
                    });
                } else if (item.tag === 'editor') {
                    var e = new ice.editor(item.tag + item.id);
                    e.width=item.width;   //宽度
                    e.height=item.height;  //高度
                    e.uploadUrl = item.uploadUrl; //上传文件路径
                    e.urlPrefix = item.urlPrefix;
                    e.uploadData = item.uploadData;
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
                        autoplay: item.autoplay,
                    });
                } else if (item.tag === 'image') {
                    upload.render({
                        elem: '#' + item.tag + item.id
                        , url: 'https://httpbin.org/post'
                        , multiple: true
                        , before: function (obj) {
                            layer.msg('图片上传中...', {
                                icon: 16,
                                shade: 0.01,
                                time: 0
                            })
                        }
                        , done: function (res) {
                            layer.close(layer.msg());//关闭上传提示窗口
                            //上传完毕
                            $('#uploader-list-' + item.id).append(
                                '<div id="" class="file-iteme">' +
                                '<div class="handle"><i class="layui-icon layui-icon-delete"></i></div>' +
                                '<img style="width: 100px;height: 100px;" src='+ item.urlPrefix + res.data.src + '>' +
                                '<div class="info">' + res.data.title + '</div>' +
                                '</div>'
                            );
                        }
                    });
                } else if (item.tag === 'file') {
                    upload.render({
                        elem: '#' + item.tag + item.id
                        , elemList: $('#list-' + item.id) //列表元素对象
                        , url: '' + item.uploadUrl + ''
                        , accept: 'file'
                        , multiple: true
                        , number: 3
                        , auto: false
                        , bindAction: '#listAction-' + item.id
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

        /* 生成 Html 代码 */
        Class.prototype.generateHtml = function (jsondata, elem) {
            var that = this
                , options = that.config;
            $.each(jsondata, function (index, item) {
                elem.append(that.components[item.tag].render(item, true));
                if (item.tag === 'grid') {
                    $.each(item.columns, function (index2, item2) {
                        //获取当前的 DOM 对象
                        var elem2 = elem.find('#' + item.id + ' .widget-col-list').filter('.column' + index2);
                        if (item2.list.length > 0) {
                            that.generateHtml(item2.list, elem2);
                        }
                    });
                } else if (item.tag === 'slider') {
                    //定义初始值
                    options.htmlCode.script += ['slider.render({',
                        , 'elem: "#' + item.tag + item.id + '" ,'
                        , 'value: ' + item.defaultValue + ','
                        , 'min: ' + item.minValue + ','
                        , 'max: ' + item.maxValue + ','
                        , 'step: ' + item.stepValue + ','
                        , 'input:' + item.isInput + ','
                        , 'change: function(value){'
                        , '$("#' + item.id + '").find("input[name="' + item.id + '"]").val(value);'
                        , ' }'
                        , '});'].join('');

                } else if (item.tag === 'date') {
                    options.htmlCode.script += ['laydate.render({'
                        , 'elem: "#' + item.tag + item.id + '" ,'
                        , 'type:"' + item.datetype + '",'
                        , 'format:"' + item.dateformat + '",'
                        , 'value:"' + item.dateDefaultValue + '",'
                        , 'min:"' + item.dataMinValue + '",'
                        , 'max:"' + item.dataMaxValue + '"});'].join('');
                } else if (item.tag === 'rate') {
                    options.htmlCode.script += ['rate.render({'
                        , 'elem: "#' + item.tag + item.id + '" ,'
                        , 'value: ' + item.defaultValue + ','
                        , 'text: ' + item.text + ','
                        , 'length: ' + item.rateLength + ','
                        , 'half: ' + item.half + ','
                        , 'readonly: ' + item.readonly + ','
                        , 'choose: function(value){'
                        , '$("#' + item.id + '").find("input[name="' + item.id + '"]").val(value);'
                        , '}'
                        , '});'].join('');
                } else if (item.tag === 'colorpicker') {
                    options.htmlCode.script += ['colorpicker.render({'
                        , 'elem: "#' + item.tag + item.id + '"'
                        , ',format: \'rgb\''
                        , ',predefine: true'
                        , ',alpha: true'
                        , ',done: function (color) {'
                        , '$("#' + item.id + '").find("input[name="'+ item.id + '"]").val(color);'
                        , '}'
                        , '});'].join('');
                } else if (item.tag === 'editor') {
                    options.htmlCode.script += ['var e = new ice.editor('+  item.tag + item.id +');'
                    ,'e.width=' + item.width + ';   //宽度'
                    ,'e.height=' + item.height + ';  //高度'
                    ,'e.uploadUrl=' + item.uploadUrl + '; //上传文件路径'
                    ,'e.disabled=' + item.disabled + ';'
                    ,'e.create();'].join('');
                } else if (item.tag === 'carousel') {
                    options.htmlCode.script += ['carousel.render({'
                        , 'elem: "#' + item.tag + item.id + '" '
                        , ',width: "' + item.width + '"'
                        , ',height: "' + item.height + '"'
                        , ',arrow: "' + item.arrow + '"'
                        , ',interval: "' + item.interval + '"'
                        , ',anim: "' + item.anim + '"'
                        , ',autoplay: "' + item.autoplay + '"'
                        , '});'].join('');
                } else if (item.tag === 'numberInput') {
                    options.htmlCode.script += [' numberInput.render({'
                    ,'elem:"#' + item.tag + item.id + '"'
                    ,'});var _width = ' + item.width.replace(/[^\d]/g,'') + ';'
                    ,'if(""!=_width){_width = 100 - parseInt(_width);}'
                    ,'$("#' + item.id + ' .layui-input-block .layui-number-input-btn").css("right",_width + "%");'
                    ,'if (item.disabled) { $("#' + item.id + ' .layui-input-block .layui-number-input-btn").css("z-index","-1");}'].join('');
                } else if (item.tag === 'labelGeneration') {
                    options.htmlCode.script += ['labelGeneration.render({'
                    ,'elem:"#' + item.tag + item.id + '",'
                    ,'data:' + item.dateDefaultValue + ','
                    ,'isEnter:' + item.isEnter + ''
                    ,'});'].join('');
                }  else if (item.tag === 'iconPicker') {
                    options.htmlCode.script += ['iconPicker.render({'
                        ,'elem:"#' + item.tag + item.id + '",'
                        ,'type:"fontClass",'
                        ,'search:' + item.iconPickerSearch + ''
                        ,'page:' + item.iconPickerPage + ''
                        ,'limit:' + item.iconPickerLimit + ''
                        ,'cellWidth:' + item.iconPickerCellWidth + ''
                        ,' click: function (data) {},'
                        ,'success: function(d) {}'
                        ,'});'
                    ,'iconPicker.checkIcon(' + item.tag + item.id + ',"");'].join('');
                } else if (item.tag === 'dateRange') {
                    options.htmlCode.script += ['laydate.render({'
                        ,'elem:"#' + item.tag + item.id + '",'
                        ,'type:' + item.datetype + ','
                        ,'format:' + item.dateformat + ''
                        ,'min:' + item.dataMinValue + ''
                        ,'max:' + item.dataMaxValue + ''
                        ,'range:["#start-' + item.tag + item.id + '", "#end-' + item.tag + item.id + '"]'
                        ,'});'].join('');
                } else if (item.tag === 'cron' && !item.disabled) {
                    options.htmlCode.script += ['cron.render({'
                        ,'elem:"#' + item.tag + item.id + '-button",'
                        ,'url:' + item.cronUrl + ','
                        ,'done: function (cronStr) {'
                        ,'$("#' +  item.tag + item.id + '").val(cronStr);'
                        ,'},'
                        ,'});'].join('');
                }else if (item.tag === 'file') {
                    options.htmlCode.script += ['upload.render({'
                        , 'elem: "#' + item.tag + item.id + '" '
                        , ', url: "' + item.uploadUrl + '"'
                        , ' ,elemList: $("#list-' + item.id + '")'
                        , ',accept: "file"'
                        , ',multiple: true'
                        , ',number: 3'
                        , ',auto: false'
                        , ',bindAction: "#listAction-' + item.id + '"'
                        , ',choose: function(obj){'
                        , 'var that = this;'
                        , 'var files = this.files = obj.pushFile();'
                        , 'obj.preview(function(index, file, result){'
                        , 'var tr = $(["<tr id="upload-"+ index +"">"'
                        , ',"<td>"+ file.name +"</td>"'
                        , ',"<td>"+ (file.size/1014).toFixed(1) +"kb</td>"'
                        , ',"<td><div class="layui-progress" lay-filter="progress-demo-"+ index +""><div class="layui-progress-bar" lay-percent=""></div></div></td>"'
                        , ',"<td>","<button class="layui-btn layui-btn-xs demo-reload layui-hide">重传</button>","<button class="layui-btn layui-btn-xs layui-btn-danger demo-delete">删除</button>","</td>"'
                        , ',"</tr>"].join(""));'
                        , 'tr.find(".demo-reload").on("click", function(){obj.upload(index, file);});'
                        , 'tr.find(".demo-delete").on("click", function(){delete files[index];tr.remove();uploadListIns.config.elem.next()[0].value = ""; });'
                        , 'that.elemList.append(tr);'
                        , 'element.render("progress");}'
                        , ',done: function(res, index, upload)'
                        , '{var that = this;if(res.code == 0){var tr = that.elemList.find("tr#upload-"+ index),tds = tr.children();tds.eq(3).html("");delete this.files[index];return;}this.error(index, upload);}'
                        , ',allDone: function(obj){console.log(obj)}'
                        , ',error: function(index, upload){var that = this;var tr = that.elemList.find("tr#upload-"+ index),'
                        , 'tds = tr.children();tds.eq(3).find(".demo-reload").removeClass("layui-hide");}'
                        , ',progress: function(n, elem, e, index){element.progress("progress-demo-"+ index, n + "%");}'
                        , '});'].join('');
                } else if (item.tag === 'image') {
                    options.htmlCode.script += ['upload.render({'
                        , 'elem: "#' + item.tag + item.id + '" '
                        , ', url: "' + item.uploadUrl + '"'
                        , ', multiple: true'
                        , ', before: function (obj) {'
                        , 'layer.msg("图片上传中...", {'
                        , 'icon: 16,'
                        , 'shade: 0.01,'
                        , 'time: 0'
                        , '})'
                        , '}'
                        , ', done: function (res) {'
                        , 'layer.close(layer.msg());'
                        , '$("#uploader-list-' + item.id + '").append('
                        , '\'<div class="file-iteme"><div class="handle"><i class="layui-icon layui-icon-delete"></i></div><img style="width: 100px;height: 100px;" src="\'+ res.data.src + \'">'
                        , '<div class="info">\'+ res.data.title+\'</div>'
                        , '</div>\''
                        , ');'
                        , '}'
                        , '});'].join('');

                }else if (item.tag === 'checkbox') {
                    options.htmlCode.script += ['form.verify({otherReq: function(value,item){' +
                    'var verifyName=$(item).attr("name"), verifyType=$(item).attr("type")' +
                    ',formElem=$(item).parents(".layui-form"),verifyElem=formElem.find("input[name=\'"+verifyName+"\']")' +
                    ',verifyElems = formElem.find("input"),isTrue= verifyElem.is(":checked"),focusElem = verifyElem.next().find("i.layui-icon");' +
                    'for (let i = 0; i < verifyElems.length; i++) {if (verifyElems[i].checked) {return false;}}' +
                    'if(!isTrue || !value){' +
                    'focusElem.css(verifyType=="radio"?{"color":"#FF5722"}:{"border-color":"#FF5722"});' +
                    'focusElem.first().attr("tabIndex","1").css("outline","0").blur(function() {' +
                    'focusElem.css(verifyType==\'radio\'?{"color":""}:{"border-color":""});' +
                    '}).focus();' +
                    'return "必填项不能为空";}}});'].join('');
                }
            });
        };


        /* 重新渲染设计区*/
        Class.prototype.renderForm = function () {
            var that = this
                , options = that.config;
            var elem = $('#formDesignerForm');
            //清空
            elem.empty();
            that.renderComponents(options.data, elem);

            //选中的节点只有一个
            if (options.selectItem !== undefined) {
                var _draghtml1 = '<div class="widget-view-action"  id="widget-view-action"><i class="layui-icon layui-icon-file"></i><i class="layui-icon layui-icon-delete"></i></div><div class="widget-view-drag" id="widget-view-drag"><i class="layui-icon layui-icon-screen-full"></i></div>';
                var len = $('#' + options.selectItem.id).children().length;
                if ($('#widget-view-action')) {//已存在
                    $('#widget-view-action').remove();
                }
                $('#' + options.selectItem.id).children(len - 1).after(_draghtml1);
                $('#formDesignerForm .layui-form-item').removeClass('active');
                //给当前元素增加class
                $('#' + options.selectItem.id).addClass('active');
                //设置当前选择项目的拷贝删除的事件

                //显示当前的属性
                that.components[options.selectItem.tag].property(options.selectItem);
                if (options.selectItem.tag === "select" || options.selectItem.tag === "checkbox"
                    || options.selectItem.tag === "radio" || options.selectItem.tag === "carousel") {
                    var sortable = Sortable.create(document.getElementById(options.selectItem.tag), {
                        group: {
                            name: 'propertygroup',
                        },
                        ghostClass: "ghost",
                        handle: '.select-option-drag',
                        dataIdAttr: 'data-index',
                        animation: 150,
                        onEnd: function (evt) {
                            if (options.selectItem === undefined) {
                                return;
                            }
                            var indexArray = sortable.toArray();
                            var newJson = [];
                            for (var i = 0; i < indexArray.length; i++) {
                                newJson.push(options.selectItem.options[indexArray[i]]);
                            }
                            options.selectItem.options = newJson;
                            //that.renderForm();
                            that.components[options.selectItem.tag].update(options.selectItem);
                        }
                    });
                }
                that.bindPropertyEvent(options.selectItem);

            }
            that.addClick();

            /* 向 拷贝 删除 按钮添加 click 动作 */
            that.addCopyDeleteClick();
            form.render();
            if (options.data.length != 0) {
                for (var i = 0; i < options.data.length ; i++) {
                    if (options.data[i].tag === 'grid') {
                        for (var j = 0; j < options.data[i].columns.length; j++) {
                            for (var k = 0; k < options.data[i].columns[j].list.length; k++) {
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

        /* 渲染预览框 */
        Class.prototype.renderPreview = function () {
            var that = this
                , options = that.config;

        };

        Class.prototype.reload = function (id
            , options) {
            var that = this;
            options = options || {};//如果是空的话，就赋值 {} 


            that.render();
        }


        //核心入口 初始化一个 regionSelect 类
        formDesigner.render = function (options) {
            var defaults = {
                formStyle: false,//是否方框
            };
            options = $.extend(defaults, options);
            var ins = new Class(options);
            return thisIns.call(ins);
        };

        exports('formDesigner'
            , formDesigner);
    });