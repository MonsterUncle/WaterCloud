layui.define(['table', 'laypage', 'jquery', 'element', 'laytpl'], function (exports) {
	"use strict";
	var filePath = layui.cache.modules.cardTable
		.substr(0, layui.cache.modules.cardTable.lastIndexOf('/'));
	// 引入tablePlug.css
	layui.link(filePath + '/cardTable.css');
	var MOD_NAME = 'cardTable',
		$ = layui.jquery,
		element = layui.element,
		laytpl = layui.laytpl,
		laypage = layui.laypage;
	var _instances = {};  // 记录所有实例
	/* 默认参数 */
	var defaultOption = {
		elem: "#currentTableId",// 构建的模型
		url: "",// 数据 url 连接
		loading: true,//是否加载
		limit: 0, //每页数量默认是每行数量的双倍
		linenum: 4, //每行数量 2,3,4,6
		currentPage: 1,//当前页
		data: [],       //静态数据
		limits: [],     //页码
		page: true, //是否分页
		image: false,//图片模式
		layout: ['count', 'prev', 'page', 'next', 'limit', 'skip'],//分页控件
		request: {
			pageName: 'page' //页码的参数名称，默认：page
			, limitName: 'limit' //每页数据量的参数名，默认：limit
			, idName: 'id'       //主键名称，默认：id
			, titleName: 'title' //标题名称，默认：title
			, imageName: 'image' //图片地址，默认：image
			, remarkName: 'remark' //备注名称，默认：remark
			, timeName: 'time' //时间名称，默认：time
		},
		response: {
			statusName: 'code' //规定数据状态的字段名称，默认：code
			, statusCode: 0 //规定成功的状态码，默认：0
			, msgName: 'msg' //规定状态信息的字段名称，默认：msg
			, countName: 'count' //规定数据总数的字段名称，默认：count
			, dataName: 'data' //规定数据列表的字段名称，默认：data
		},
		// 完 成 函 数
		done: function (res, curr, count) {

		},
		toolbar: null
	};
	var card = function (opt) {
		_instances[opt.elem.substring(1)] = this;
		this.reload(opt);
	};
	/** 参数设置 */
	card.prototype.initOptions = function (opt) {
		this.option = $.extend(true, {}, defaultOption, opt);
		if (!this.option.limit || this.option.limit == 0) {
			this.option.limit = this.option.linenum * 2;
		}
		if (!this.option.limits || this.option.limits.length == 0) {
			this.option.limits = [this.option.limit];
		}
	};
	card.prototype.init = function () {
		var option = this.option;
		var url = option.url;
		var html = "";
		html += option.loading == true ? '      <div class="ew-table-loading">' : '      <div class="ew-table-loading layui-hide">';
		html += '         <i class="layui-icon layui-icon-loading layui-anim layui-anim-rotate layui-anim-loop"></i>';
		html += '      </div>';
		$(option.elem).html(html);
		// 根 据 请 求 方 式 获 取 数 据
		html = "";
		if (!!url) {
			if (url.indexOf("?") >= 0) {
				url = url + '&v=1.0.0';
			}
			else {
				url = url + '?v=1.0.0';
			}
			if (!!option.page) {
				url = url + '&' + option.request.limitName + '=' + option.limit;
				url = url + '&' + option.request.pageName + '=' + option.currentPage;
			}
			if (!!option.where) {
				for (let key in option.where) {
					url = url + '&' + key + '=' + option.where[key];
				}
			}
			var data = getData(url);
			data = initData(data, option);
			if (data.code != option.response.statusCode) {
				option.data = [];
				option.count = 0;
			} else {
				option.data = data.data;
				option.count = data.count;
			}

		}
		else {
			if (!option.alldata) {
				option.alldata = option.data;
			}
			if (option.page) {
				var data = [];
				option.count = option.alldata.length;
				for (var i = (option.currentPage - 1) * option.limit; i < option.currentPage * option.limit && i < option.alldata.length; i++) {
					data.push(option.alldata[i]);
				}
				option.data = data;
			}
		}
		// 根据结果进行相应结构的创建
		if (!!option.data && option.data.length > 0) {
			html = createComponent(option.linenum, option.data, option.toolbar, option.image);
			html += "<div id='cardpage'></div>";
		}
		else {
			html = "<p>没有数据</p>";
		}
		$(option.elem).html(html);
		if (option.page) {
			// 初始化分页组件
			laypage.render({
				elem: 'cardpage'
				, count: option.count, limit: option.limit, limits: option.limits, curr: option.currentPage
				, layout: option.layout
				, jump: function (obj, first) {
					option.limit = obj.limit;
					option.currentPage = obj.curr;
					if (!first) {
						_instances[option.elem.substring(1)].reload(option);
					}
				}
			});
		}
		typeof option.done === 'function' && option.done(option, option.curr, option.count);
	}
	card.prototype.reload = function (opt) {
		this.initOptions(this.option ? $.extend(true, this.option, opt) : opt);
		this.init();  // 初始化表格
		this.events(); //事件
	}
	//表单事件
	card.prototype.on = function (events, callback) {
		return layui.onevent.call(this, MOD_NAME, events, callback);
	};
	card.prototype.events = function () {
		var that = this;
		var option = that.option;
		var filter = $(option.elem).attr('lay-filter');
		var elem = option.elem.substring(1);
		let timer = null;
		$(option.elem).unbind();
		//行事件
		$(option.elem).on('click', 'div[type=card]', function () { //单击行
			clearTimeout(timer);
			var that = this;
			timer = setTimeout(function () {
				getCheckedData(that, elem);
				var reElem = that;
				layui.event.call(that, MOD_NAME, 'row(' + filter + ')', {
					elem: that
					, value: _instances[elem].option.checkedItem
					, othis: reElem
				});
			}, 300)
		}).on('dblclick', 'div[type=card]', function () { //双击行
			clearTimeout(timer);
			getCheckedData(this, elem);
			var reElem = this;
			layui.event.call(this, MOD_NAME, 'rowDouble(' + filter + ')', {
				elem: this
				, value: _instances[elem].option.checkedItem
				, othis: reElem
			});
		});
		if (!!option.toolbar && !option.image) {
			$(option.elem).unbind('click');
		}
		//行工具条操作事件
		$(option.elem).on('click', '*[lay-event]', function () {
			getCheckedData(this, elem);
			layui.event.call(this, MOD_NAME, 'tool(' + filter + ')', { event: $(this).attr('lay-event'), value: _instances[elem].option.checkedItem });
		});
	};
	function getCheckedData(obj, elem) {
		var item = {};
		if (!obj.id) {
			return getCheckedData(obj.parentElement, elem);
		}
		var reElem = obj;
		$(reElem).addClass('layui-table-click').siblings().removeClass('layui-table-click');
		item.id = reElem.id;
		if (!obj.id || obj.nodeName != "DIV") {
			item.id = item.id.replace('card_', '');
		}
		var option = _instances[elem].option;
		for (var i = 0; i < option.data.length; i++) {
			if (item.id == option.data[i].id) {
				item[option.request.idName] = option.data[i].id;
				item[option.request.imageName] = option.data[i].image;
				item[option.request.titleName] = option.data[i].title;
				item[option.request.remarkName] = option.data[i].remark;
				item[option.request.timeName] = option.data[i].time;
				item = $.extend(JSON.parse(JSON.stringify(option.data[i])), item);
				if (option.request.idName != "id") {
					delete item.id;
				}
				if (option.request.imageName != "image") {
					delete item.image;
				}
				if (option.request.titleName != "title") {
					delete item.title;
				}
				if (option.request.remarkName != "remark") {
					delete item.remark;
				}
				if (option.request.timeName != "time") {
					delete item.time;
				}
				_instances[elem].option.checkedItem = item;
				break;
			}
		}
		return item;
	}
	function createComponent(linenum, data, toolbar, image) {
		var html = "<div class='cloud-card-component'>"
		var content = createCards(linenum, data, toolbar, image);
		var page = "";
		content = content + page;
		html += content + "</div>"
		return html;
	}
	/** 创建指定数量的卡片 */
	function createCards(linenum, data, toolbar, image) {
		var content = "<div class='layui-row layui-col-space30'>";
		for (var i = 0; i < data.length; i++) {
			content += createCard(linenum, data[i], i, toolbar, image);
		}
		content += "</div>";
		return content;
	}
	/** 创建一个卡片 */
	function createCard(linenum, item, index, toolbar, image) {
		var line = 12 / linenum;
		var tplData = $.extend(true, {
			LAY_INDEX: index
		}, item)
		var template = '';
		if (!!toolbar) {
			template = laytpl($('#' + toolbar).html() || '').render(tplData);
		}
		if (image) {
			return '<div type=card id=card_' + item.id + ' class="layui-col-md' + line + ' ew-datagrid-item" data-index="' + index + '" data-number="1"> <div class="project-list-item">' + (!!item.image ? ' <img class="project-list-item-cover" src="' + item.image + '">' : '') + ' </div > </div > ';
		}
		else {
			return '<div type=card id=card_' + item.id + ' class="layui-col-md' + line + ' ew-datagrid-item" data-index="' + index + '" data-number="1"> <div class="project-list-item">' + (!!item.image ? ' <img class="project-list-item-cover" src="' + item.image + '">' : '') + '<div class="project-list-item-body"> <h2>' + item.title + '</h2> <div class="project-list-item-text layui-text">' + item.remark + '</div> <div class="project-list-item-desc"> <span class="time">' + item.time + '</span>' + (!!template ? ' <div class="ew-head-list">' + template + '</div>' : '') + ' </div> </div > </div > </div > ';
		}

	}
	/** 格式化返回参数 */
	function initData(tempData, option) {
		var data = {};
		data.code = tempData[option.response.statusName];
		data.msg = tempData[option.response.msgName];
		data.count = tempData[option.response.countName];
		var dataList = tempData[option.response.dataName];
		data.data = [];
		for (var i = 0; i < dataList.length; i++) {
			var item = {};
			item.id = dataList[i][option.request.idName];
			item.image = dataList[i][option.request.imageName];
			item.title = dataList[i][option.request.titleName];
			item.remark = dataList[i][option.request.remarkName];
			item.time = dataList[i][option.request.timeName];
			item = $.extend(JSON.parse(JSON.stringify(dataList[i])), item);
			if (option.request.idName != "id") {
				delete item[option.request.idName];
			}
			if (option.request.imageName != "image") {
				delete item[option.request.imageName];
			}
			if (option.request.titleName != "title") {
				delete item[option.request.titleName];
			}
			if (option.request.remarkName != "remark") {
				delete item[option.request.remarkName];
			}
			if (option.request.timeName != "time") {
				delete item[option.request.timeName];
			}
			data.data.push(item);
		}
		return data;
	}
	/** 同 步 请 求 获 取 数 据 */
	function getData(url) {
		$.ajaxSettings.async = false;
		var redata = null;
		$.getJSON(url, function (data) {
			redata = data;
		}).fail(function () {
			redata = null;
		});
		return redata;
	}
	////卡片点击事件
	//window.cardTableCheckedCard = function (elem,obj) {
	//	$(obj).addClass('layui-table-click').siblings().removeClass('layui-table-click');
	//	var item = {};
	//	item.id = obj.id;
	//	item.image = $(obj).find('.project-list-item-cover')[0].src;
	//	item.title = $(obj).find('h2')[0].innerHTML;
	//	item.remark = $(obj).find('.project-list-item-text')[0].innerHTML;
	//	item.time = $(obj).find('.time')[0].innerHTML;
	//	_instances[elem.id].option.checkedItem = item;
	//}
	/** 对外提供的方法 */
	var tt = {
		/* 渲染 */
		render: function (options) {
			return new card(options);
		},
		/* 重载 */
		reload: function (id, opt) {
			_instances[id].option.checkedItem = null;
			_instances[id].reload(opt);
		},
		/* 获取选中数据 */
		getChecked: function (id) {
			var option = _instances[id].option;
			var data = option.checkedItem;
			if (!data) {
				return null;
			}
			return data;
		},
		/* 获取表格数据 */
		getAllData: function (id) {
			var option = _instances[id].option;
			var data = [];
			for (var i = 0; i < option.data.length; i++) {
				var item = {};
				item[option.request.idName] = option.data[i].id;
				item[option.request.imageName] = option.data[i].image;
				item[option.request.titleName] = option.data[i].title;
				item[option.request.remarkName] = option.data[i].remark;
				item[option.request.timeName] = option.data[i].time;
				item = $.extend(JSON.parse(JSON.stringify(option.data[i])), item);
				if (option.request.idName != "id") {
					delete item.id;
				}
				if (option.request.imageName != "image") {
					delete item.image;
				}
				if (option.request.titleName != "title") {
					delete item.title;
				}
				if (option.request.remarkName != "remark") {
					delete item.remark;
				}
				if (option.request.timeName != "time") {
					delete item.time;
				}
				data.push(item);
			}
			return data;
		},
		on: function (events, callback) {
			return this.on(this, MOD_NAME, events, callback);
		}
	}
	exports(MOD_NAME, tt);
})
