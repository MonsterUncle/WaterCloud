/**

 @Name��layui.carousel �ֲ�ģ��
 @Author������
 @License��MIT
    
 */

layui.define('jquery', function (exports) {
    "use strict";

    var $ = layui.$
        , hint = layui.hint()
        , device = layui.device()

        //�ⲿ�ӿ�
        , carousel = {
            config: {} //ȫ��������

            //����ȫ����
            , set: function (options) {
                var that = this;
                that.config = $.extend({}, that.config, options);
                return that;
            }

            //�¼�����
            , on: function (events, callback) {
                return layui.onevent.call(this, MOD_NAME, events, callback);
            }
        }

        //�ַ�����
        , MOD_NAME = 'carousel', ELEM = '.layui-carousel', THIS = 'layui-this', SHOW = 'layui-show', HIDE = 'layui-hide', DISABLED = 'layui-disabled'

        , ELEM_ITEM = '>*[carousel-item]>*', ELEM_LEFT = 'layui-carousel-left', ELEM_RIGHT = 'layui-carousel-right', ELEM_PREV = 'layui-carousel-prev', ELEM_NEXT = 'layui-carousel-next', ELEM_ARROW = 'layui-carousel-arrow', ELEM_IND = 'layui-carousel-ind'

        //������
        , Class = function (options) {
            var that = this;
            that.config = $.extend({}, that.config, carousel.config, options);
            that.render();
        };

    //Ĭ������
    Class.prototype.config = {
        width: '600px'
        , height: '280px'
        , full: false //�Ƿ�ȫ��
        , arrow: 'hover' //�л���ͷĬ����ʾ״̬��hover/always/none
        , indicator: 'inside' //ָʾ��λ�ã�inside/outside/none
        , autoplay: true //�Ƿ��Զ��л�
        , interval: 3000 //�Զ��л���ʱ���������ܵ���800ms
        , anim: '' //�������ͣ�default/updown/fade
        , trigger: 'click' //ָʾ���Ĵ�����ʽ��click/hover
        , index: 0 //��ʼ��ʼ������
    };

    //�ֲ���Ⱦ
    Class.prototype.render = function () {
        var that = this
            , options = that.config;

        options.elem = $(options.elem);
        if (!options.elem[0]) return;
        that.elemItem = options.elem.find(ELEM_ITEM);

        if (options.index < 0) options.index = 0;
        if (options.index >= that.elemItem.length) options.index = that.elemItem.length - 1;
        if (options.interval < 800) options.interval = 800;

        //�Ƿ�ȫ��ģʽ
        if (options.full) {
            options.elem.css({
                position: 'fixed'
                , width: '100%'
                , height: '100%'
                , zIndex: 9999
            });
        } else {
            options.elem.css({
                width: options.width
                , height: options.height
            });
        }

        options.elem.attr('lay-anim', options.anim);

        //��ʼ����״̬
        that.elemItem.eq(options.index).addClass(THIS);

        //ָʾ���ȶ���
        if (that.elemItem.length <= 1) return;
        that.indicator();
        that.arrow();
        that.autoplay();
        that.events();
    };

    //�����ֲ�
    Class.prototype.reload = function (options) {
        var that = this;
        clearInterval(that.timer);
        that.config = $.extend({}, that.config, options);
        that.render();
    };

    //��ȡ��һ���ȴ���Ŀ������
    Class.prototype.prevIndex = function () {
        var that = this
            , options = that.config;

        var prevIndex = options.index - 1;
        if (prevIndex < 0) {
            prevIndex = that.elemItem.length - 1;
        }
        return prevIndex;
    };

    //��ȡ��һ���ȴ���Ŀ������
    Class.prototype.nextIndex = function () {
        var that = this
            , options = that.config;

        var nextIndex = options.index + 1;
        if (nextIndex >= that.elemItem.length) {
            nextIndex = 0;
        }
        return nextIndex;
    };

    //��������
    Class.prototype.addIndex = function (num) {
        var that = this
            , options = that.config;

        num = num || 1;
        options.index = options.index + num;

        //index���ܳ����ֲ�������
        if (options.index >= that.elemItem.length) {
            options.index = 0;
        }
    };

    //�����ݼ�
    Class.prototype.subIndex = function (num) {
        var that = this
            , options = that.config;

        num = num || 1;
        options.index = options.index - num;

        //index���ܳ����ֲ�������
        if (options.index < 0) {
            options.index = that.elemItem.length - 1;
        }
    };

    //�Զ��ֲ�
    Class.prototype.autoplay = function () {
        var that = this
            , options = that.config;

        if (!options.autoplay) return;
        clearInterval(that.timer);

        that.timer = setInterval(function () {
            that.slide();
        }, options.interval);
    };

    //��ͷ
    Class.prototype.arrow = function () {
        var that = this
            , options = that.config;

        //ģ��
        var tplArrow = $([
            '<button class="layui-icon ' + ELEM_ARROW + '" lay-type="sub">' + (options.anim === 'updown' ? '&#xe619;' : '&#xe603;') + '</button>'
            , '<button class="layui-icon ' + ELEM_ARROW + '" lay-type="add">' + (options.anim === 'updown' ? '&#xe61a;' : '&#xe602;') + '</button>'
        ].join(''));

        //Ԥ���������
        options.elem.attr('lay-arrow', options.arrow);

        //�����ظ�����
        if (options.elem.find('.' + ELEM_ARROW)[0]) {
            options.elem.find('.' + ELEM_ARROW).remove();
        };
        options.elem.append(tplArrow);

        //�¼�
        tplArrow.on('click', function () {
            var othis = $(this)
                , type = othis.attr('lay-type')
            that.slide(type);
        });
    };

    //ָʾ��
    Class.prototype.indicator = function () {
        var that = this
            , options = that.config;

        //ģ��
        var tplInd = that.elemInd = $(['<div class="' + ELEM_IND + '"><ul>'
            , function () {
                var li = [];
                layui.each(that.elemItem, function (index) {
                    li.push('<li' + (options.index === index ? ' class="layui-this"' : '') + '></li>');
                });
                return li.join('');
            }()
            , '</ul></div>'].join(''));

        //Ԥ���������
        options.elem.attr('lay-indicator', options.indicator);

        //�����ظ�����
        if (options.elem.find('.' + ELEM_IND)[0]) {
            options.elem.find('.' + ELEM_IND).remove();
        };
        options.elem.append(tplInd);

        if (options.anim === 'updown') {
            tplInd.css('margin-top', -(tplInd.height() / 2));
        }

        //�¼�
        tplInd.find('li').on(options.trigger === 'hover' ? 'mouseover' : options.trigger, function () {
            var othis = $(this)
                , index = othis.index();
            if (index > options.index) {
                that.slide('add', index - options.index);
            } else if (index < options.index) {
                that.slide('sub', options.index - index);
            }
        });
    };

    //�����л�
    Class.prototype.slide = function (type, num) {
        var that = this
            , elemItem = that.elemItem
            , options = that.config
            , thisIndex = options.index
            , filter = options.elem.attr('lay-filter');

        if (that.haveSlide) return;

        //��������
        if (type === 'sub') {
            that.subIndex(num);
            elemItem.eq(options.index).addClass(ELEM_PREV);
            setTimeout(function () {
                elemItem.eq(thisIndex).addClass(ELEM_RIGHT);
                elemItem.eq(options.index).addClass(ELEM_RIGHT);
            }, 50);
        } else { //Ĭ�ϵ�����
            that.addIndex(num);
            elemItem.eq(options.index).addClass(ELEM_NEXT);
            setTimeout(function () {
                elemItem.eq(thisIndex).addClass(ELEM_LEFT);
                elemItem.eq(options.index).addClass(ELEM_LEFT);
            }, 50);
        };

        //�Ƴ�������
        setTimeout(function () {
            elemItem.removeClass(THIS + ' ' + ELEM_PREV + ' ' + ELEM_NEXT + ' ' + ELEM_LEFT + ' ' + ELEM_RIGHT);
            elemItem.eq(options.index).addClass(THIS);
            that.haveSlide = false; //����
        }, 300);

        //ָʾ������
        that.elemInd.find('li').eq(options.index).addClass(THIS)
            .siblings().removeClass(THIS);

        that.haveSlide = true;

        layui.event.call(this, MOD_NAME, 'change(' + filter + ')', {
            index: options.index
            , prevIndex: thisIndex
            , item: elemItem.eq(options.index)
        });
    };

    //�¼�����
    Class.prototype.events = function () {
        var that = this
            , options = that.config;

        if (options.elem.data('haveEvents')) return;

        //�����Ƴ�����
        options.elem.on('mouseenter', function () {
            clearInterval(that.timer);
        }).on('mouseleave', function () {
            that.autoplay();
        });

        options.elem.data('haveEvents', true);
    };

    //�������
    carousel.render = function (options) {
        var inst = new Class(options);
        return inst;
    };

    exports(MOD_NAME, carousel);
});


