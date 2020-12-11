!function(t, e) {
    "object" == typeof exports && "object" == typeof module ? module.exports = e() : 
    "function" == typeof define && define.amd ? define([], e) :
    "object" == typeof exports ? exports.wangEditor = e() :
    t.wangEditor = e()
}(window, (function() {
    return function(t) {
        var e = {};
        function n(o) {
            if (e[o])
                return e[o].exports;
            var r = e[o] = {
                i: o,
                l: !1,
                exports: {}
            };
            return t[o].call(r.exports, r, r.exports, n),
            r.l = !0,
            r.exports
        }
        return n.m = t,
        n.c = e,
        n.d = function(t, e, o) {
            n.o(t, e) || Object.defineProperty(t, e, {
                enumerable: !0,
                get: o
            })
        }
        ,
        n.r = function(t) {
            "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(t, Symbol.toStringTag, {
                value: "Module"
            }),
            Object.defineProperty(t, "__esModule", {
                value: !0
            })
        }
        ,
        n.t = function(t, e) {
            if (1 & e && (t = n(t)),
            8 & e)
                return t;
            if (4 & e && "object" == typeof t && t && t.__esModule)
                return t;
            var o = Object.create(null);
            if (n.r(o),
            Object.defineProperty(o, "default", {
                enumerable: !0,
                value: t
            }),
            2 & e && "string" != typeof t)
                for (var r in t)
                    n.d(o, r, function(e) {
                        return t[e]
                    }
                    .bind(null, r));
            return o
        }
        ,
        n.n = function(t) {
            var e = t && t.__esModule ? function() {
                return t.default
            }
            : function() {
                return t
            }
            ;
            return n.d(e, "a", e),
            e
        }
        ,
        n.o = function(t, e) {
            return Object.prototype.hasOwnProperty.call(t, e)
        }
        ,
        n.p = "",
        n(n.s = 125)
    }([function(t, e) {
        t.exports = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        }
    }
    , function(t, e, n) {
        t.exports = n(126)
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(105))
          , i = o(n(106))
          , a = o(n(107))
          , u = o(n(60))
          , l = o(n(108))
          , c = o(n(23))
          , s = o(n(39))
          , f = o(n(6))
          , d = o(n(113))
          , p = o(n(50))
          , A = o(n(1));
        (0,
        A.default)(e, "__esModule", {
            value: !0
        }),
        e.DomElement = void 0;
        var v = [];
        function h(t) {
            return (0,
            p.default)(Array.prototype).call(t)
        }
        function g(t) {
            var e = []
              , n = [];
            return e = (0,
            d.default)(t) ? t : t.split(";"),
            (0,
            f.default)(e).call(e, (function(t) {
                var e, o = (0,
                s.default)(e = t.split(":")).call(e, (function(t) {
                    return (0,
                    c.default)(t).call(t)
                }
                ));
                2 === o.length && n.push(o[0] + ":" + o[1])
            }
            )),
            n
        }
        var m = function() {
            function t(e) {
                if (this.selector = "",
                this.elems = [],
                this.length = this.elems.length,
                this.dataSource = new l.default,
                e) {
                    if (e instanceof t)
                        return e;
                    var n = [];
                    this.selector = e;
                    var o, r, i = e.nodeType;
                    if (9 === i)
                        n = [e];
                    else if (1 === i)
                        n = [e];
                    else if (function(t) {
                        return !!t && (t instanceof HTMLCollection || t instanceof NodeList)
                    }(e))
                        n = h(e);
                    else if (e instanceof Array)
                        n = e;
                    else if ("string" == typeof e) {
                        var a;
                        e = (0,
                        c.default)(a = e.replace("/\n/mg", "")).call(a),
                        0 === (0,
                        u.default)(e).call(e, "<") ? (o = e,
                        (r = document.createElement("div")).innerHTML = o,
                        n = h(r.children)) : n = function(t) {
                            return h(document.querySelectorAll(t))
                        }(e)
                    }
                    var s = n.length;
                    if (!s)
                        return this;
                    for (var f = 0; f < s; f++)
                        this.elems.push(n[f]);
                    this.length = s
                }
            }
            return (0,
            A.default)(t.prototype, "id", {
                get: function() {
                    return this.elems[0].id
                },
                enumerable: !1,
                configurable: !0
            }),
            t.prototype.forEach = function(t) {
                for (var e = 0; e < this.length; e++) {
                    var n = this.elems[e];
                    if (!1 === t.call(n, n, e))
                        break
                }
                return this
            }
            ,
            t.prototype.clone = function(t) {
                var e;
                void 0 === t && (t = !1);
                var n = [];
                return (0,
                f.default)(e = this.elems).call(e, (function(e) {
                    n.push(e.cloneNode(!!t))
                }
                )),
                y(n)
            }
            ,
            t.prototype.get = function(t) {
                void 0 === t && (t = 0);
                var e = this.length;
                return t >= e && (t %= e),
                y(this.elems[t])
            }
            ,
            t.prototype.first = function() {
                return this.get(0)
            }
            ,
            t.prototype.last = function() {
                var t = this.length;
                return this.get(t - 1)
            }
            ,
            t.prototype.on = function(t, e, n) {
                var o;
                return t ? ("function" == typeof e && (n = e,
                e = ""),
                (0,
                f.default)(o = this).call(o, (function(o) {
                    if (e) {
                        var r = function(t) {
                            var o = t.target;
                            o.matches(e) && n.call(o, t)
                        };
                        o.addEventListener(t, r),
                        v.push({
                            elem: o,
                            selector: e,
                            fn: n,
                            agentFn: r
                        })
                    } else
                        o.addEventListener(t, n)
                }
                ))) : this
            }
            ,
            t.prototype.off = function(t, e, n) {
                var o;
                return t ? ("function" == typeof e && (n = e,
                e = ""),
                (0,
                f.default)(o = this).call(o, (function(o) {
                    if (e) {
                        for (var r = -1, i = 0; i < v.length; i++) {
                            var u = v[i];
                            if (u.selector === e && u.fn === n && u.elem === o) {
                                r = i;
                                break
                            }
                        }
                        if (-1 !== r) {
                            var l = (0,
                            a.default)(v).call(v, r, 1)[0].agentFn;
                            o.removeEventListener(t, l)
                        }
                    } else
                        o.removeEventListener(t, n)
                }
                ))) : this
            }
            ,
            t.prototype.attr = function(t, e) {
                var n;
                return null == e ? this.elems[0].getAttribute(t) || "" : (0,
                f.default)(n = this).call(n, (function(n) {
                    n.setAttribute(t, e)
                }
                ))
            }
            ,
            t.prototype.addClass = function(t) {
                var e;
                return t ? (0,
                f.default)(e = this).call(e, (function(e) {
                    if (e.className) {
                        var n = e.className.split(/\s/);
                        n = (0,
                        i.default)(n).call(n, (function(t) {
                            return !!(0,
                            c.default)(t).call(t)
                        }
                        )),
                        (0,
                        u.default)(n).call(n, t) < 0 && n.push(t),
                        e.className = n.join(" ")
                    } else
                        e.className = t
                }
                )) : this
            }
            ,
            t.prototype.removeClass = function(t) {
                var e;
                return t ? (0,
                f.default)(e = this).call(e, (function(e) {
                    if (e.className) {
                        var n = e.className.split(/\s/);
                        n = (0,
                        i.default)(n).call(n, (function(e) {
                            return !(!(e = (0,
                            c.default)(e).call(e)) || e === t)
                        }
                        )),
                        e.className = n.join(" ")
                    }
                }
                )) : this
            }
            ,
            t.prototype.hasClass = function(t) {
                if (void 0 === t && (t = ""),
                !t)
                    return !1;
                var e = this.elems[0];
                if (!e.className)
                    return !1;
                var n = e.className.split(/\s/);
                return (0,
                r.default)(n).call(n, t)
            }
            ,
            t.prototype.css = function(t, e) {
                var n, o;
                return o = "" == e ? "" : t + ":" + e + ";",
                (0,
                f.default)(n = this).call(n, (function(e) {
                    var n, r = (0,
                    c.default)(n = e.getAttribute("style") || "").call(n);
                    if (r) {
                        var i = g(r);
                        i = (0,
                        s.default)(i).call(i, (function(e) {
                            return 0 === (0,
                            u.default)(e).call(e, t) ? o : e
                        }
                        )),
                        "" != o && (0,
                        u.default)(i).call(i, o) < 0 && i.push(o),
                        "" == o && (i = g(i)),
                        e.setAttribute("style", i.join("; "))
                    } else
                        e.setAttribute("style", o)
                }
                ))
            }
            ,
            t.prototype.getBoundingClientRect = function() {
                return this.elems[0].getBoundingClientRect()
            }
            ,
            t.prototype.show = function() {
                return this.css("display", "block")
            }
            ,
            t.prototype.hide = function() {
                return this.css("display", "none")
            }
            ,
            t.prototype.children = function() {
                var t = this.elems[0];
                return t ? y(t.children) : null
            }
            ,
            t.prototype.childNodes = function() {
                var t = this.elems[0];
                return t ? y(t.childNodes) : null
            }
            ,
            t.prototype.append = function(t) {
                var e;
                return (0,
                f.default)(e = this).call(e, (function(e) {
                    (0,
                    f.default)(t).call(t, (function(t) {
                        e.appendChild(t)
                    }
                    ))
                }
                ))
            }
            ,
            t.prototype.remove = function() {
                var t;
                return (0,
                f.default)(t = this).call(t, (function(t) {
                    if (t.remove)
                        t.remove();
                    else {
                        var e = t.parentElement;
                        e && e.removeChild(t)
                    }
                }
                ))
            }
            ,
            t.prototype.isContain = function(t) {
                var e = this.elems[0]
                  , n = t.elems[0];
                return e.contains(n)
            }
            ,
            t.prototype.getSizeData = function() {
                return this.elems[0].getBoundingClientRect()
            }
            ,
            t.prototype.getNodeName = function() {
                return this.elems[0].nodeName
            }
            ,
            t.prototype.getClientHeight = function() {
                return this.elems[0].clientHeight
            }
            ,
            t.prototype.find = function(t) {
                return y(this.elems[0].querySelectorAll(t))
            }
            ,
            t.prototype.text = function(t) {
                var e;
                return t ? (0,
                f.default)(e = this).call(e, (function(e) {
                    e.innerHTML = t
                }
                )) : this.elems[0].innerHTML.replace(/<[^>]+>/g, (function() {
                    return ""
                }
                ))
            }
            ,
            t.prototype.html = function(t) {
                var e = this.elems[0];
                return t ? (e.innerHTML = t,
                this) : e.innerHTML
            }
            ,
            t.prototype.val = function() {
                var t, e = this.elems[0];
                return (0,
                c.default)(t = e.value).call(t)
            }
            ,
            t.prototype.focus = function() {
                var t;
                return (0,
                f.default)(t = this).call(t, (function(t) {
                    t.focus()
                }
                ))
            }
            ,
            t.prototype.prev = function() {
                return y(this.elems[0].previousElementSibling)
            }
            ,
            t.prototype.next = function() {
                return y(this.elems[0].nextElementSibling)
            }
            ,
            t.prototype.parent = function() {
                return y(this.elems[0].parentElement)
            }
            ,
            t.prototype.parentUntil = function(t, e) {
                var n = e || this.elems[0];
                if ("BODY" === n.nodeName)
                    return null;
                var o = n.parentElement;
                return null == o ? null : o.matches(t) ? y(o) : this.parentUntil(t, o)
            }
            ,
            t.prototype.equal = function(e) {
                return e instanceof t ? this.elems[0] === e.elems[0] : e instanceof HTMLElement && this.elems[0] === e
            }
            ,
            t.prototype.insertBefore = function(t) {
                var e, n = y(t).elems[0];
                return n ? (0,
                f.default)(e = this).call(e, (function(t) {
                    n.parentNode.insertBefore(t, n)
                }
                )) : this
            }
            ,
            t.prototype.insertAfter = function(t) {
                var e, n = y(t).elems[0], o = n && n.nextSibling;
                return n ? (0,
                f.default)(e = this).call(e, (function(t) {
                    var e = n.parentNode;
                    o ? e.insertBefore(t, o) : e.appendChild(t)
                }
                )) : this
            }
            ,
            t.prototype.data = function(t, e) {
                if (null == e)
                    return this.dataSource.get(t);
                this.dataSource.set(t, e)
            }
            ,
            t.prototype.getNodeTop = function(t) {
                if (this.length < 1)
                    return this;
                var e = this.parent();
                return t.$textElem.equal(e) ? this : e.getNodeTop(t)
            }
            ,
            t.prototype.getOffsetData = function() {
                var t = this.elems[0];
                return {
                    top: t.offsetTop,
                    left: t.offsetLeft,
                    width: t.offsetWidth,
                    height: t.offsetHeight,
                    parent: t.offsetParent
                }
            }
            ,
            t
        }();
        function y(t) {
            return new m(t)
        }
        e.DomElement = m,
        e.default = y
    }
    , function(t, e, n) {
        t.exports = n(173)
    }
    , function(t, e, n) {
        "use strict";
        var o = n(7)
          , r = n(62).f
          , i = n(87)
          , a = n(12)
          , u = n(44)
          , l = n(18)
          , c = n(15)
          , s = function(t) {
            var e = function(e, n, o) {
                if (this instanceof t) {
                    switch (arguments.length) {
                    case 0:
                        return new t;
                    case 1:
                        return new t(e);
                    case 2:
                        return new t(e,n)
                    }
                    return new t(e,n,o)
                }
                return t.apply(this, arguments)
            };
            return e.prototype = t.prototype,
            e
        };
        t.exports = function(t, e) {
            var n, f, d, p, A, v, h, g, m = t.target, y = t.global, w = t.stat, x = t.proto, E = y ? o : w ? o[m] : (o[m] || {}).prototype, b = y ? a : a[m] || (a[m] = {}), _ = b.prototype;
            for (d in e)
                n = !i(y ? d : m + (w ? "." : "#") + d, t.forced) && E && c(E, d),
                A = b[d],
                n && (v = t.noTargetGet ? (g = r(E, d)) && g.value : E[d]),
                p = n && v ? v : e[d],
                n && typeof A == typeof p || (h = t.bind && n ? u(p, o) : t.wrap && n ? s(p) : x && "function" == typeof p ? u(Function.call, p) : p,
                (t.sham || p && p.sham || A && A.sham) && l(h, "sham", !0),
                b[d] = h,
                x && (c(a, f = m + "Prototype") || l(a, f, {}),
                a[f][d] = p,
                t.real && _ && !_[d] && l(_, d, p)))
        }
    }
    , function(t, e, n) {
        t.exports = n(290)
    }
    , function(t, e, n) {
        t.exports = n(168)
    }
    , function(t, e, n) {
        (function(e) {
            var n = function(t) {
                return t && t.Math == Math && t
            };
            t.exports = n("object" == typeof globalThis && globalThis) || n("object" == typeof window && window) || n("object" == typeof self && self) || n("object" == typeof e && e) || Function("return this")()
        }
        ).call(this, n(129))
    }
    , function(t, e, n) {
        var o = n(7)
          , r = n(66)
          , i = n(15)
          , a = n(56)
          , u = n(68)
          , l = n(92)
          , c = r("wks")
          , s = o.Symbol
          , f = l ? s : s && s.withoutSetter || a;
        t.exports = function(t) {
            return i(c, t) || (u && i(s, t) ? c[t] = s[t] : c[t] = f("Symbol." + t)),
            c[t]
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(113))
          , i = o(n(114))
          , a = o(n(83))
          , u = o(n(50))
          , l = o(n(1))
          , c = function() {
            for (var t = 0, e = 0, n = arguments.length; e < n; e++)
                t += arguments[e].length;
            var o = Array(t)
              , r = 0;
            for (e = 0; e < n; e++)
                for (var i = arguments[e], a = 0, u = i.length; a < u; a++,
                r++)
                    o[r] = i[a];
            return o
        };
        (0,
        l.default)(e, "__esModule", {
            value: !0
        }),
        e.deepClone = e.isFunction = e.debounce = e.throttle = e.arrForEach = e.forEach = e.replaceSpecialSymbol = e.replaceHtmlSymbol = e.getRandom = e.UA = void 0,
        e.UA = {
            _ua: navigator.userAgent,
            isWebkit: function() {
                return /webkit/i.test(this._ua)
            },
            isIE: function() {
                return "ActiveXObject"in window
            }
        },
        e.getRandom = function(t) {
            var e;
            return void 0 === t && (t = ""),
            t + (0,
            u.default)(e = Math.random().toString()).call(e, 2)
        }
        ,
        e.replaceHtmlSymbol = function(t) {
            return t.replace(/</gm, "&lt;").replace(/>/gm, "&gt;").replace(/"/gm, "&quot;").replace(/(\r\n|\r|\n)/g, "<br/>")
        }
        ,
        e.replaceSpecialSymbol = function(t) {
            return t.replace(/&lt;/gm, "<").replace(/&gt;/gm, ">").replace(/&quot;/gm, '"')
        }
        ,
        e.forEach = function(t, e) {
            for (var n in t) {
                if (Object.prototype.hasOwnProperty.call(t, n))
                    if (!1 === e(n, t[n]))
                        break
            }
        }
        ,
        e.arrForEach = function(t, e) {
            var n, o, r = t.length || 0;
            for (n = 0; n < r && (o = t[n],
            !1 !== e.call(t, o, n)); n++)
                ;
        }
        ,
        e.throttle = function(t, e) {
            void 0 === e && (e = 200);
            var n = !1;
            return function() {
                for (var o = [], r = 0; r < arguments.length; r++)
                    o[r] = arguments[r];
                n || (n = !0,
                (0,
                a.default)((function() {
                    n = !1,
                    t.call.apply(t, c([null], o))
                }
                ), e))
            }
        }
        ,
        e.debounce = function(t, e) {
            void 0 === e && (e = 200);
            var n = 0;
            return function() {
                for (var o = [], r = 0; r < arguments.length; r++)
                    o[r] = arguments[r];
                n && window.clearTimeout(n),
                n = window.setTimeout((function() {
                    n = 0,
                    t.call.apply(t, c([null], o))
                }
                ), e)
            }
        }
        ,
        e.isFunction = function(t) {
            return "function" == typeof t
        }
        ,
        e.deepClone = function t(e) {
            if ("object" !== (0,
            i.default)(e) || "function" == typeof e || null === e)
                return e;
            var n;
            for (var o in (0,
            r.default)(e) && (n = []),
            (0,
            r.default)(e) || (n = {}),
            e)
                Object.prototype.hasOwnProperty.call(e, o) && (n[o] = t(e[o]));
            return n
        }
    }
    , function(t, e, n) {
        var o = n(12)
          , r = n(15)
          , i = n(82)
          , a = n(16).f;
        t.exports = function(t) {
            var e = o.Symbol || (o.Symbol = {});
            r(e, t) || a(e, t, {
                value: i.f(t)
            })
        }
    }
    , function(t, e) {
        t.exports = function(t) {
            try {
                return !!t()
            } catch (t) {
                return !0
            }
        }
    }
    , function(t, e) {
        t.exports = {}
    }
    , function(t, e) {
        t.exports = function(t) {
            return "object" == typeof t ? null !== t : "function" == typeof t
        }
    }
    , function(t, e, n) {
        var o = n(11);
        t.exports = !o((function() {
            return 7 != Object.defineProperty({}, 1, {
                get: function() {
                    return 7
                }
            })[1]
        }
        ))
    }
    , function(t, e) {
        var n = {}.hasOwnProperty;
        t.exports = function(t, e) {
            return n.call(t, e)
        }
    }
    , function(t, e, n) {
        var o = n(14)
          , r = n(86)
          , i = n(20)
          , a = n(51)
          , u = Object.defineProperty;
        e.f = o ? u : function(t, e, n) {
            if (i(t),
            e = a(e, !0),
            i(n),
            r)
                try {
                    return u(t, e, n)
                } catch (t) {}
            if ("get"in n || "set"in n)
                throw TypeError("Accessors not supported");
            return "value"in n && (t[e] = n.value),
            t
        }
    }
    , function(t, e, n) {
        var o = n(12);
        t.exports = function(t) {
            return o[t + "Prototype"]
        }
    }
    , function(t, e, n) {
        var o = n(14)
          , r = n(16)
          , i = n(42);
        t.exports = o ? function(t, e, n) {
            return r.f(t, e, i(1, n))
        }
        : function(t, e, n) {
            return t[e] = n,
            t
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(6)), a = r(n(1)), u = r(n(3)), l = r(n(5)), c = (o = function(t, e) {
            return (o = l.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            u.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), s = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        a.default)(e, "__esModule", {
            value: !0
        });
        var f = s(n(2))
          , d = s(n(84))
          , p = s(n(122))
          , A = function(t) {
            function e(e, n, o) {
                var r = t.call(this, e, n) || this;
                o.title = n.i18next.t("menus.dropListMenu." + o.title);
                var a, u = "zh-CN" === n.config.lang ? "" : "w-e-drop-list-tl";
                "" !== u && "list" === o.type && (0,
                i.default)(a = o.list).call(a, (function(t) {
                    var e = t.$elem
                      , n = f.default(e.children());
                    if (n.length > 0) {
                        var o = null == n ? void 0 : n.getNodeName();
                        o && "I" === o && e.addClass(u)
                    }
                }
                ));
                var l = new p.default(r,o);
                return r.dropList = l,
                e.on("mouseenter", (function() {
                    var t;
                    null != n.selection.getRange() && (e.css("z-index", n.zIndex.get("menu")),
                    (0,
                    i.default)(t = n.txt.eventHooks.dropListMenuHoverEvents).call(t, (function(t) {
                        return t()
                    }
                    )),
                    l.showTimeoutId = window.setTimeout((function() {
                        l.show()
                    }
                    ), 200))
                }
                )).on("mouseleave", (function() {
                    e.css("z-index", "auto"),
                    l.hideTimeoutId = window.setTimeout((function() {
                        l.hide()
                    }
                    ))
                }
                )),
                r
            }
            return c(e, t),
            e
        }(d.default);
        e.default = A
    }
    , function(t, e, n) {
        var o = n(13);
        t.exports = function(t) {
            if (!o(t))
                throw TypeError(String(t) + " is not an object");
            return t
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = function() {
            return void 0 === o && (o = Boolean(window && document && document.all && !window.atob)),
            o
        }, i = function() {
            var t = {};
            return function(e) {
                if (void 0 === t[e]) {
                    var n = document.querySelector(e);
                    if (window.HTMLIFrameElement && n instanceof window.HTMLIFrameElement)
                        try {
                            n = n.contentDocument.head
                        } catch (t) {
                            n = null
                        }
                    t[e] = n
                }
                return t[e]
            }
        }(), a = [];
        function u(t) {
            for (var e = -1, n = 0; n < a.length; n++)
                if (a[n].identifier === t) {
                    e = n;
                    break
                }
            return e
        }
        function l(t, e) {
            for (var n = {}, o = [], r = 0; r < t.length; r++) {
                var i = t[r]
                  , l = e.base ? i[0] + e.base : i[0]
                  , c = n[l] || 0
                  , s = "".concat(l, " ").concat(c);
                n[l] = c + 1;
                var f = u(s)
                  , d = {
                    css: i[1],
                    media: i[2],
                    sourceMap: i[3]
                };
                -1 !== f ? (a[f].references++,
                a[f].updater(d)) : a.push({
                    identifier: s,
                    updater: h(d, e),
                    references: 1
                }),
                o.push(s)
            }
            return o
        }
        function c(t) {
            var e = document.createElement("style")
              , o = t.attributes || {};
            if (void 0 === o.nonce) {
                var r = n.nc;
                r && (o.nonce = r)
            }
            if (Object.keys(o).forEach((function(t) {
                e.setAttribute(t, o[t])
            }
            )),
            "function" == typeof t.insert)
                t.insert(e);
            else {
                var a = i(t.insert || "head");
                if (!a)
                    throw new Error("Couldn't find a style target. This probably means that the value for the 'insert' parameter is invalid.");
                a.appendChild(e)
            }
            return e
        }
        var s, f = (s = [],
        function(t, e) {
            return s[t] = e,
            s.filter(Boolean).join("\n")
        }
        );
        function d(t, e, n, o) {
            var r = n ? "" : o.media ? "@media ".concat(o.media, " {").concat(o.css, "}") : o.css;
            if (t.styleSheet)
                t.styleSheet.cssText = f(e, r);
            else {
                var i = document.createTextNode(r)
                  , a = t.childNodes;
                a[e] && t.removeChild(a[e]),
                a.length ? t.insertBefore(i, a[e]) : t.appendChild(i)
            }
        }
        function p(t, e, n) {
            var o = n.css
              , r = n.media
              , i = n.sourceMap;
            if (r ? t.setAttribute("media", r) : t.removeAttribute("media"),
            i && btoa && (o += "\n/*# sourceMappingURL=data:application/json;base64,".concat(btoa(unescape(encodeURIComponent(JSON.stringify(i)))), " */")),
            t.styleSheet)
                t.styleSheet.cssText = o;
            else {
                for (; t.firstChild; )
                    t.removeChild(t.firstChild);
                t.appendChild(document.createTextNode(o))
            }
        }
        var A = null
          , v = 0;
        function h(t, e) {
            var n, o, r;
            if (e.singleton) {
                var i = v++;
                n = A || (A = c(e)),
                o = d.bind(null, n, i, !1),
                r = d.bind(null, n, i, !0)
            } else
                n = c(e),
                o = p.bind(null, n, e),
                r = function() {
                    !function(t) {
                        if (null === t.parentNode)
                            return !1;
                        t.parentNode.removeChild(t)
                    }(n)
                }
                ;
            return o(t),
            function(e) {
                if (e) {
                    if (e.css === t.css && e.media === t.media && e.sourceMap === t.sourceMap)
                        return;
                    o(t = e)
                } else
                    r()
            }
        }
        t.exports = function(t, e) {
            (e = e || {}).singleton || "boolean" == typeof e.singleton || (e.singleton = r());
            var n = l(t = t || [], e);
            return function(t) {
                if (t = t || [],
                "[object Array]" === Object.prototype.toString.call(t)) {
                    for (var o = 0; o < n.length; o++) {
                        var r = u(n[o]);
                        a[r].references--
                    }
                    for (var i = l(t, e), c = 0; c < n.length; c++) {
                        var s = u(n[c]);
                        0 === a[s].references && (a[s].updater(),
                        a.splice(s, 1))
                    }
                    n = i
                }
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        t.exports = function(t) {
            var e = [];
            return e.toString = function() {
                return this.map((function(e) {
                    var n = function(t, e) {
                        var n = t[1] || ""
                          , o = t[3];
                        if (!o)
                            return n;
                        if (e && "function" == typeof btoa) {
                            var r = (a = o,
                            u = btoa(unescape(encodeURIComponent(JSON.stringify(a)))),
                            l = "sourceMappingURL=data:application/json;charset=utf-8;base64,".concat(u),
                            "/*# ".concat(l, " */"))
                              , i = o.sources.map((function(t) {
                                return "/*# sourceURL=".concat(o.sourceRoot || "").concat(t, " */")
                            }
                            ));
                            return [n].concat(i).concat([r]).join("\n")
                        }
                        var a, u, l;
                        return [n].join("\n")
                    }(e, t);
                    return e[2] ? "@media ".concat(e[2], " {").concat(n, "}") : n
                }
                )).join("")
            }
            ,
            e.i = function(t, n, o) {
                "string" == typeof t && (t = [[null, t, ""]]);
                var r = {};
                if (o)
                    for (var i = 0; i < this.length; i++) {
                        var a = this[i][0];
                        null != a && (r[a] = !0)
                    }
                for (var u = 0; u < t.length; u++) {
                    var l = [].concat(t[u]);
                    o && r[l[0]] || (n && (l[2] ? l[2] = "".concat(n, " and ").concat(l[2]) : l[2] = n),
                    e.push(l))
                }
            }
            ,
            e
        }
    }
    , function(t, e, n) {
        t.exports = n(201)
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = function(t) {
            function e(e, n) {
                return t.call(this, e, n) || this
            }
            return l(e, t),
            e
        }(c(n(84)).default);
        e.default = s
    }
    , function(t, e, n) {
        var o = n(14)
          , r = n(11)
          , i = n(15)
          , a = Object.defineProperty
          , u = {}
          , l = function(t) {
            throw t
        };
        t.exports = function(t, e) {
            if (i(u, t))
                return u[t];
            e || (e = {});
            var n = [][t]
              , c = !!i(e, "ACCESSORS") && e.ACCESSORS
              , s = i(e, 0) ? e[0] : l
              , f = i(e, 1) ? e[1] : void 0;
            return u[t] = !!n && !r((function() {
                if (c && !o)
                    return !0;
                var t = {
                    length: -1
                };
                c ? a(t, 1, {
                    enumerable: !0,
                    get: l
                }) : t[1] = 1,
                n.call(t, s, f)
            }
            ))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(120))
          , i = o(n(61))
          , a = o(n(6))
          , u = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var l = u(n(2))
          , c = n(40)
          , s = function() {
            function t(e, n) {
                this.menu = e,
                this.conf = n,
                this.$container = l.default('<div class="w-e-panel-container"></div>');
                var o = e.editor;
                o.txt.eventHooks.clickEvents.push(t.hideCurAllPanels),
                o.txt.eventHooks.toolbarClickEvents.push(t.hideCurAllPanels),
                o.txt.eventHooks.dropListMenuHoverEvents.push(t.hideCurAllPanels)
            }
            return t.prototype.create = function() {
                var e = this
                  , n = this.menu;
                if (!t.createdMenus.has(n)) {
                    var o = this.conf
                      , r = this.$container
                      , u = o.width || 300
                      , s = n.editor.$toolbarElem.getBoundingClientRect()
                      , f = n.$elem.getBoundingClientRect()
                      , d = s.height + s.top - f.top
                      , p = (s.width - u) / 2 + s.left - f.left;
                    r.css("width", u + "px").css("margin-top", d + "px").css("margin-left", p + "px").css("z-index", n.editor.zIndex.get("panel"));
                    var A = l.default('<i class="w-e-icon-close w-e-panel-close"></i>');
                    r.append(A),
                    A.on("click", (function() {
                        e.remove()
                    }
                    ));
                    var v = l.default('<ul class="w-e-panel-tab-title"></ul>')
                      , h = l.default('<div class="w-e-panel-tab-content"></div>');
                    r.append(v).append(h);
                    var g = o.height;
                    g && h.css("height", g + "px").css("overflow-y", "auto");
                    var m = o.tabs || []
                      , y = []
                      , w = [];
                    (0,
                    a.default)(m).call(m, (function(t, e) {
                        if (t) {
                            var n = t.title || ""
                              , o = t.tpl || ""
                              , r = l.default('<li class="w-e-item">' + n + "</li>");
                            v.append(r);
                            var i = l.default(o);
                            h.append(i),
                            y.push(r),
                            w.push(i),
                            0 === e ? (r.data("active", !0),
                            r.addClass("w-e-active")) : i.hide(),
                            r.on("click", (function() {
                                r.data("active") || ((0,
                                a.default)(y).call(y, (function(t) {
                                    t.data("active", !1),
                                    t.removeClass("w-e-active")
                                }
                                )),
                                (0,
                                a.default)(w).call(w, (function(t) {
                                    t.hide()
                                }
                                )),
                                r.data("active", !0),
                                r.addClass("w-e-active"),
                                i.show())
                            }
                            ))
                        }
                    }
                    )),
                    r.on("click", (function(t) {
                        t.stopPropagation()
                    }
                    )),
                    n.$elem.append(r),
                    (0,
                    a.default)(m).call(m, (function(t, n) {
                        if (t) {
                            var o = t.events || [];
                            (0,
                            a.default)(o).call(o, (function(t) {
                                var o = t.selector
                                  , r = t.type
                                  , a = t.fn || c.EMPTY_FN
                                  , u = w[n];
                                (0,
                                i.default)(u).call(u, o).on(r, (function(t) {
                                    t.stopPropagation(),
                                    a(t) && e.remove()
                                }
                                ))
                            }
                            ))
                        }
                    }
                    ));
                    var x = (0,
                    i.default)(r).call(r, "input[type=text],textarea");
                    x.length && x.get(0).focus(),
                    t.hideCurAllPanels(),
                    n.setPanel(this),
                    t.createdMenus.add(n)
                }
            }
            ,
            t.prototype.remove = function() {
                var e = this.menu
                  , n = this.$container;
                n && n.remove(),
                t.createdMenus.delete(e)
            }
            ,
            t.hideCurAllPanels = function() {
                var e;
                0 !== t.createdMenus.size && (0,
                a.default)(e = t.createdMenus).call(e, (function(t) {
                    var e = t.panel;
                    e && e.remove()
                }
                ))
            }
            ,
            t.createdMenus = new r.default,
            t
        }();
        e.default = s
    }
    , function(t, e, n) {
        var o = n(64)
          , r = n(43);
        t.exports = function(t) {
            return o(r(t))
        }
    }
    , function(t, e) {
        var n = {}.toString;
        t.exports = function(t) {
            return n.call(t).slice(8, -1)
        }
    }
    , function(t, e, n) {
        var o = n(73)
          , r = n(16).f
          , i = n(18)
          , a = n(15)
          , u = n(154)
          , l = n(8)("toStringTag");
        t.exports = function(t, e, n, c) {
            if (t) {
                var s = n ? t : t.prototype;
                a(s, l) || r(s, l, {
                    configurable: !0,
                    value: e
                }),
                c && !o && i(s, "toString", u)
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = function(t) {
            function e(e, n) {
                return t.call(this, e, n) || this
            }
            return l(e, t),
            e.prototype.setPanel = function(t) {
                this.panel = t
            }
            ,
            e
        }(c(n(84)).default);
        e.default = s
    }
    , function(t, e) {
        t.exports = function(t) {
            if ("function" != typeof t)
                throw TypeError(String(t) + " is not a function");
            return t
        }
    }
    , function(t, e, n) {
        var o, r, i, a = n(149), u = n(7), l = n(13), c = n(18), s = n(15), f = n(55), d = n(45), p = u.WeakMap;
        if (a) {
            var A = new p
              , v = A.get
              , h = A.has
              , g = A.set;
            o = function(t, e) {
                return g.call(A, t, e),
                e
            }
            ,
            r = function(t) {
                return v.call(A, t) || {}
            }
            ,
            i = function(t) {
                return h.call(A, t)
            }
        } else {
            var m = f("state");
            d[m] = !0,
            o = function(t, e) {
                return c(t, m, e),
                e
            }
            ,
            r = function(t) {
                return s(t, m) ? t[m] : {}
            }
            ,
            i = function(t) {
                return s(t, m)
            }
        }
        t.exports = {
            set: o,
            get: r,
            has: i,
            enforce: function(t) {
                return i(t) ? r(t) : o(t, {})
            },
            getterFor: function(t) {
                return function(e) {
                    var n;
                    if (!l(e) || (n = r(e)).type !== t)
                        throw TypeError("Incompatible receiver, " + t + " required");
                    return n
                }
            }
        }
    }
    , function(t, e) {
        t.exports = !0
    }
    , function(t, e, n) {
        var o = n(43);
        t.exports = function(t) {
            return Object(o(t))
        }
    }
    , function(t, e, n) {
        var o = n(54)
          , r = Math.min;
        t.exports = function(t) {
            return t > 0 ? r(o(t), 9007199254740991) : 0
        }
    }
    , function(t, e, n) {
        var o = n(12)
          , r = n(7)
          , i = function(t) {
            return "function" == typeof t ? t : void 0
        };
        t.exports = function(t, e) {
            return arguments.length < 2 ? i(o[t]) || i(r[t]) : o[t] && o[t][e] || r[t] && r[t][e]
        }
    }
    , function(t, e) {
        t.exports = {}
    }
    , function(t, e, n) {
        var o = n(44)
          , r = n(64)
          , i = n(34)
          , a = n(35)
          , u = n(79)
          , l = [].push
          , c = function(t) {
            var e = 1 == t
              , n = 2 == t
              , c = 3 == t
              , s = 4 == t
              , f = 6 == t
              , d = 5 == t || f;
            return function(p, A, v, h) {
                for (var g, m, y = i(p), w = r(y), x = o(A, v, 3), E = a(w.length), b = 0, _ = h || u, S = e ? _(p, E) : n ? _(p, 0) : void 0; E > b; b++)
                    if ((d || b in w) && (m = x(g = w[b], b, y),
                    t))
                        if (e)
                            S[b] = m;
                        else if (m)
                            switch (t) {
                            case 3:
                                return !0;
                            case 5:
                                return g;
                            case 6:
                                return b;
                            case 2:
                                l.call(S, g)
                            }
                        else if (s)
                            return !1;
                return f ? -1 : c || s ? s : S
            }
        };
        t.exports = {
            forEach: c(0),
            map: c(1),
            filter: c(2),
            some: c(3),
            every: c(4),
            find: c(5),
            findIndex: c(6)
        }
    }
    , function(t, e, n) {
        t.exports = n(206)
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.urlRegex = e.imgRegex = e.EMPTY_FN = void 0,
        e.EMPTY_FN = function() {}
        ,
        e.imgRegex = /\.(gif|jpg|jpeg|png)$/i,
        e.urlRegex = /^(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-.,@?^=%&amp;:/~+#]*[\w\-@?^=%&amp;/~+#])?/
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = o(n(1))
          , a = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var u = a(n(2))
          , l = function() {
            function t(t, e, n) {
                this.editor = t,
                this.$targetElem = e,
                this.conf = n,
                this._show = !1,
                this._isInsertTextContainer = !1;
                var o = u.default("<div></div>");
                o.addClass("w-e-tooltip"),
                this.$container = o
            }
            return t.prototype.getPositionData = function() {
                var t = this.$container
                  , e = 0
                  , n = 0
                  , o = document.documentElement.scrollTop
                  , r = this.$targetElem.getBoundingClientRect()
                  , i = this.editor.$textElem.getBoundingClientRect()
                  , a = this.$targetElem.getOffsetData()
                  , l = u.default(a.parent)
                  , c = this.editor.$textElem.elems[0].scrollTop;
                if (this._isInsertTextContainer = l.equal(this.editor.$textContainerElem),
                this._isInsertTextContainer) {
                    var s = l.getClientHeight()
                      , f = a.top
                      , d = a.left
                      , p = a.height
                      , A = f - c;
                    A > 25 ? (e = A - 20 - 15,
                    t.addClass("w-e-tooltip-up")) : A + p + 20 < s ? (e = A + p + 10,
                    t.addClass("w-e-tooltip-down")) : (e = (A > 0 ? A : 0) + 20 + 10,
                    t.addClass("w-e-tooltip-down")),
                    n = d < 0 ? 0 : d
                } else
                    r.top < 20 || r.top - i.top < 20 ? (e = r.bottom + o + 5,
                    t.addClass("w-e-tooltip-down")) : (e = r.top + o - 20 - 15,
                    t.addClass("w-e-tooltip-up")),
                    n = r.left < 0 ? 0 : r.left;
                return {
                    top: e,
                    left: n
                }
            }
            ,
            t.prototype.appendMenus = function() {
                var t = this
                  , e = this.conf
                  , n = this.editor
                  , o = this.$targetElem
                  , i = this.$container;
                e.length;
                (0,
                r.default)(e).call(e, (function(e, r) {
                    var a = e.$elem
                      , l = u.default("<div></div>");
                    l.addClass("w-e-tooltip-item-wrapper "),
                    l.append(a),
                    i.append(l),
                    a.on("click", (function(r) {
                        r.preventDefault(),
                        e.onClick(n, o) && t.remove()
                    }
                    ))
                }
                ))
            }
            ,
            t.prototype.create = function() {
                var t = this.editor
                  , e = this.$container;
                this.appendMenus();
                var n = this.getPositionData()
                  , o = n.top
                  , r = n.left;
                e.css("top", o + "px"),
                e.css("left", r + "px"),
                e.css("z-index", t.zIndex.get("tooltip")),
                this._isInsertTextContainer ? this.editor.$textContainerElem.append(e) : u.default("body").append(e),
                this._show = !0
            }
            ,
            t.prototype.remove = function() {
                this.$container.remove(),
                this._show = !1
            }
            ,
            (0,
            i.default)(t.prototype, "isShow", {
                get: function() {
                    return this._show
                },
                enumerable: !1,
                configurable: !0
            }),
            t
        }();
        e.default = l
    }
    , function(t, e) {
        t.exports = function(t, e) {
            return {
                enumerable: !(1 & t),
                configurable: !(2 & t),
                writable: !(4 & t),
                value: e
            }
        }
    }
    , function(t, e) {
        t.exports = function(t) {
            if (null == t)
                throw TypeError("Can't call method on " + t);
            return t
        }
    }
    , function(t, e, n) {
        var o = n(31);
        t.exports = function(t, e, n) {
            if (o(t),
            void 0 === e)
                return t;
            switch (n) {
            case 0:
                return function() {
                    return t.call(e)
                }
                ;
            case 1:
                return function(n) {
                    return t.call(e, n)
                }
                ;
            case 2:
                return function(n, o) {
                    return t.call(e, n, o)
                }
                ;
            case 3:
                return function(n, o, r) {
                    return t.call(e, n, o, r)
                }
            }
            return function() {
                return t.apply(e, arguments)
            }
        }
    }
    , function(t, e) {
        t.exports = {}
    }
    , function(t, e, n) {
        var o = n(18);
        t.exports = function(t, e, n, r) {
            r && r.enumerable ? t[e] = n : o(t, e, n)
        }
    }
    , function(t, e, n) {
        n(156);
        var o = n(157)
          , r = n(7)
          , i = n(58)
          , a = n(18)
          , u = n(37)
          , l = n(8)("toStringTag");
        for (var c in o) {
            var s = r[c]
              , f = s && s.prototype;
            f && i(f) !== l && a(f, l, c),
            u[c] = u.Array
        }
    }
    , function(t, e, n) {
        var o = n(28);
        t.exports = Array.isArray || function(t) {
            return "Array" == o(t)
        }
    }
    , function(t, e, n) {
        var o = n(11)
          , r = n(8)
          , i = n(78)
          , a = r("species");
        t.exports = function(t) {
            return i >= 51 || !o((function() {
                var e = [];
                return (e.constructor = {})[a] = function() {
                    return {
                        foo: 1
                    }
                }
                ,
                1 !== e[t](Boolean).foo
            }
            ))
        }
    }
    , function(t, e, n) {
        t.exports = n(213)
    }
    , function(t, e, n) {
        var o = n(13);
        t.exports = function(t, e) {
            if (!o(t))
                return t;
            var n, r;
            if (e && "function" == typeof (n = t.toString) && !o(r = n.call(t)))
                return r;
            if ("function" == typeof (n = t.valueOf) && !o(r = n.call(t)))
                return r;
            if (!e && "function" == typeof (n = t.toString) && !o(r = n.call(t)))
                return r;
            throw TypeError("Can't convert object to primitive value")
        }
    }
    , function(t, e) {}
    , function(t, e, n) {
        "use strict";
        var o = n(148).charAt
          , r = n(32)
          , i = n(67)
          , a = r.set
          , u = r.getterFor("String Iterator");
        i(String, "String", (function(t) {
            a(this, {
                type: "String Iterator",
                string: String(t),
                index: 0
            })
        }
        ), (function() {
            var t, e = u(this), n = e.string, r = e.index;
            return r >= n.length ? {
                value: void 0,
                done: !0
            } : (t = o(n, r),
            e.index += t.length,
            {
                value: t,
                done: !1
            })
        }
        ))
    }
    , function(t, e) {
        var n = Math.ceil
          , o = Math.floor;
        t.exports = function(t) {
            return isNaN(t = +t) ? 0 : (t > 0 ? o : n)(t)
        }
    }
    , function(t, e, n) {
        var o = n(66)
          , r = n(56)
          , i = o("keys");
        t.exports = function(t) {
            return i[t] || (i[t] = r(t))
        }
    }
    , function(t, e) {
        var n = 0
          , o = Math.random();
        t.exports = function(t) {
            return "Symbol(" + String(void 0 === t ? "" : t) + ")_" + (++n + o).toString(36)
        }
    }
    , function(t, e, n) {
        var o, r = n(20), i = n(153), a = n(72), u = n(45), l = n(94), c = n(65), s = n(55), f = s("IE_PROTO"), d = function() {}, p = function(t) {
            return "<script>" + t + "<\/script>"
        }, A = function() {
            try {
                o = document.domain && new ActiveXObject("htmlfile")
            } catch (t) {}
            var t, e;
            A = o ? function(t) {
                t.write(p("")),
                t.close();
                var e = t.parentWindow.Object;
                return t = null,
                e
            }(o) : ((e = c("iframe")).style.display = "none",
            l.appendChild(e),
            e.src = String("javascript:"),
            (t = e.contentWindow.document).open(),
            t.write(p("document.F=Object")),
            t.close(),
            t.F);
            for (var n = a.length; n--; )
                delete A.prototype[a[n]];
            return A()
        };
        u[f] = !0,
        t.exports = Object.create || function(t, e) {
            var n;
            return null !== t ? (d.prototype = r(t),
            n = new d,
            d.prototype = null,
            n[f] = t) : n = A(),
            void 0 === e ? n : i(n, e)
        }
    }
    , function(t, e, n) {
        var o = n(73)
          , r = n(28)
          , i = n(8)("toStringTag")
          , a = "Arguments" == r(function() {
            return arguments
        }());
        t.exports = o ? r : function(t) {
            var e, n, o;
            return void 0 === t ? "Undefined" : null === t ? "Null" : "string" == typeof (n = function(t, e) {
                try {
                    return t[e]
                } catch (t) {}
            }(e = Object(t), i)) ? n : a ? r(e) : "Object" == (o = r(e)) && "function" == typeof e.callee ? "Arguments" : o
        }
    }
    , function(t, e, n) {
        var o = n(20)
          , r = n(159)
          , i = n(35)
          , a = n(44)
          , u = n(160)
          , l = n(161)
          , c = function(t, e) {
            this.stopped = t,
            this.result = e
        };
        (t.exports = function(t, e, n, s, f) {
            var d, p, A, v, h, g, m, y = a(e, n, s ? 2 : 1);
            if (f)
                d = t;
            else {
                if ("function" != typeof (p = u(t)))
                    throw TypeError("Target is not iterable");
                if (r(p)) {
                    for (A = 0,
                    v = i(t.length); v > A; A++)
                        if ((h = s ? y(o(m = t[A])[0], m[1]) : y(t[A])) && h instanceof c)
                            return h;
                    return new c(!1)
                }
                d = p.call(t)
            }
            for (g = d.next; !(m = g.call(d)).done; )
                if ("object" == typeof (h = l(d, y, m.value, s)) && h && h instanceof c)
                    return h;
            return new c(!1)
        }
        ).stop = function(t) {
            return new c(!0,t)
        }
    }
    , function(t, e, n) {
        t.exports = n(193)
    }
    , function(t, e, n) {
        t.exports = n(265)
    }
    , function(t, e, n) {
        var o = n(14)
          , r = n(63)
          , i = n(42)
          , a = n(27)
          , u = n(51)
          , l = n(15)
          , c = n(86)
          , s = Object.getOwnPropertyDescriptor;
        e.f = o ? s : function(t, e) {
            if (t = a(t),
            e = u(e, !0),
            c)
                try {
                    return s(t, e)
                } catch (t) {}
            if (l(t, e))
                return i(!r.f.call(t, e), t[e])
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = {}.propertyIsEnumerable
          , r = Object.getOwnPropertyDescriptor
          , i = r && !o.call({
            1: 2
        }, 1);
        e.f = i ? function(t) {
            var e = r(this, t);
            return !!e && e.enumerable
        }
        : o
    }
    , function(t, e, n) {
        var o = n(11)
          , r = n(28)
          , i = "".split;
        t.exports = o((function() {
            return !Object("z").propertyIsEnumerable(0)
        }
        )) ? function(t) {
            return "String" == r(t) ? i.call(t, "") : Object(t)
        }
        : Object
    }
    , function(t, e, n) {
        var o = n(7)
          , r = n(13)
          , i = o.document
          , a = r(i) && r(i.createElement);
        t.exports = function(t) {
            return a ? i.createElement(t) : {}
        }
    }
    , function(t, e, n) {
        var o = n(33)
          , r = n(89);
        (t.exports = function(t, e) {
            return r[t] || (r[t] = void 0 !== e ? e : {})
        }
        )("versions", []).push({
            version: "3.6.4",
            mode: o ? "pure" : "global",
            copyright: "© 2020 Denis Pushkarev (zloirock.ru)"
        })
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(151)
          , i = n(91)
          , a = n(95)
          , u = n(29)
          , l = n(18)
          , c = n(46)
          , s = n(8)
          , f = n(33)
          , d = n(37)
          , p = n(90)
          , A = p.IteratorPrototype
          , v = p.BUGGY_SAFARI_ITERATORS
          , h = s("iterator")
          , g = function() {
            return this
        };
        t.exports = function(t, e, n, s, p, m, y) {
            r(n, e, s);
            var w, x, E, b = function(t) {
                if (t === p && B)
                    return B;
                if (!v && t in M)
                    return M[t];
                switch (t) {
                case "keys":
                case "values":
                case "entries":
                    return function() {
                        return new n(this,t)
                    }
                }
                return function() {
                    return new n(this)
                }
            }, _ = e + " Iterator", S = !1, M = t.prototype, C = M[h] || M["@@iterator"] || p && M[p], B = !v && C || b(p), k = "Array" == e && M.entries || C;
            if (k && (w = i(k.call(new t)),
            A !== Object.prototype && w.next && (f || i(w) === A || (a ? a(w, A) : "function" != typeof w[h] && l(w, h, g)),
            u(w, _, !0, !0),
            f && (d[_] = g))),
            "values" == p && C && "values" !== C.name && (S = !0,
            B = function() {
                return C.call(this)
            }
            ),
            f && !y || M[h] === B || l(M, h, B),
            d[e] = B,
            p)
                if (x = {
                    values: b("values"),
                    keys: m ? B : b("keys"),
                    entries: b("entries")
                },
                y)
                    for (E in x)
                        (v || S || !(E in M)) && c(M, E, x[E]);
                else
                    o({
                        target: e,
                        proto: !0,
                        forced: v || S
                    }, x);
            return x
        }
    }
    , function(t, e, n) {
        var o = n(11);
        t.exports = !!Object.getOwnPropertySymbols && !o((function() {
            return !String(Symbol())
        }
        ))
    }
    , function(t, e, n) {
        var o = n(93)
          , r = n(72);
        t.exports = Object.keys || function(t) {
            return o(t, r)
        }
    }
    , function(t, e, n) {
        var o = n(27)
          , r = n(35)
          , i = n(71)
          , a = function(t) {
            return function(e, n, a) {
                var u, l = o(e), c = r(l.length), s = i(a, c);
                if (t && n != n) {
                    for (; c > s; )
                        if ((u = l[s++]) != u)
                            return !0
                } else
                    for (; c > s; s++)
                        if ((t || s in l) && l[s] === n)
                            return t || s || 0;
                return !t && -1
            }
        };
        t.exports = {
            includes: a(!0),
            indexOf: a(!1)
        }
    }
    , function(t, e, n) {
        var o = n(54)
          , r = Math.max
          , i = Math.min;
        t.exports = function(t, e) {
            var n = o(t);
            return n < 0 ? r(n + e, 0) : i(n, e)
        }
    }
    , function(t, e) {
        t.exports = ["constructor", "hasOwnProperty", "isPrototypeOf", "propertyIsEnumerable", "toLocaleString", "toString", "valueOf"]
    }
    , function(t, e, n) {
        var o = {};
        o[n(8)("toStringTag")] = "z",
        t.exports = "[object z]" === String(o)
    }
    , function(t, e) {
        t.exports = function() {}
    }
    , function(t, e) {
        t.exports = function(t, e, n) {
            if (!(t instanceof e))
                throw TypeError("Incorrect " + (n ? n + " " : "") + "invocation");
            return t
        }
    }
    , function(t, e, n) {
        var o = n(36);
        t.exports = o("navigator", "userAgent") || ""
    }
    , function(t, e, n) {
        "use strict";
        var o = n(31)
          , r = function(t) {
            var e, n;
            this.promise = new t((function(t, o) {
                if (void 0 !== e || void 0 !== n)
                    throw TypeError("Bad Promise constructor");
                e = t,
                n = o
            }
            )),
            this.resolve = o(e),
            this.reject = o(n)
        };
        t.exports.f = function(t) {
            return new r(t)
        }
    }
    , function(t, e, n) {
        var o, r, i = n(7), a = n(76), u = i.process, l = u && u.versions, c = l && l.v8;
        c ? r = (o = c.split("."))[0] + o[1] : a && (!(o = a.match(/Edge\/(\d+)/)) || o[1] >= 74) && (o = a.match(/Chrome\/(\d+)/)) && (r = o[1]),
        t.exports = r && +r
    }
    , function(t, e, n) {
        var o = n(13)
          , r = n(48)
          , i = n(8)("species");
        t.exports = function(t, e) {
            var n;
            return r(t) && ("function" != typeof (n = t.constructor) || n !== Array && !r(n.prototype) ? o(n) && null === (n = n[i]) && (n = void 0) : n = void 0),
            new (void 0 === n ? Array : n)(0 === e ? 0 : e)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(51)
          , r = n(16)
          , i = n(42);
        t.exports = function(t, e, n) {
            var a = o(e);
            a in t ? r.f(t, a, i(0, n)) : t[a] = n
        }
    }
    , function(t, e) {
        t.exports = "\t\n\v\f\r                　\u2028\u2029\ufeff"
    }
    , function(t, e, n) {
        var o = n(8);
        e.f = o
    }
    , function(t, e, n) {
        t.exports = n(246)
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = o(n(1))
          , a = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var u = a(n(26))
          , l = function() {
            function t(t, e) {
                var n = this;
                this.$elem = t,
                this.editor = e,
                this._active = !1,
                t.on("click", (function(t) {
                    var o;
                    u.default.hideCurAllPanels(),
                    (0,
                    r.default)(o = e.txt.eventHooks.menuClickEvents).call(o, (function(t) {
                        return t()
                    }
                    )),
                    t.stopPropagation(),
                    null != e.selection.getRange() && n.clickHandler(t)
                }
                ))
            }
            return t.prototype.clickHandler = function(t) {}
            ,
            t.prototype.active = function() {
                this._active = !0,
                this.$elem.addClass("w-e-active")
            }
            ,
            t.prototype.unActive = function() {
                this._active = !1,
                this.$elem.removeClass("w-e-active")
            }
            ,
            (0,
            i.default)(t.prototype, "isActive", {
                get: function() {
                    return this._active
                },
                enumerable: !1,
                configurable: !0
            }),
            t
        }();
        e.default = l
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(60))
          , i = o(n(6))
          , a = o(n(121))
          , u = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var l = n(9)
          , c = u(n(313))
          , s = u(n(314))
          , f = function() {
            function t(t) {
                this.editor = t
            }
            return t.prototype.alert = function(t, e) {
                var n = this.editor.config.customAlert;
                n ? n(t) : window.alert(t),
                e && console.error("wangEditor: " + e)
            }
            ,
            t.prototype.insertImg = function(t) {
                var e = this
                  , n = this.editor
                  , o = n.config
                  , r = function(t, e) {
                    return void 0 === e && (e = "validate."),
                    n.i18next.t(e + t)
                };
                n.cmd.do("insertHTML", '<img src="' + t + '" style="max-width:100%;"/>'),
                o.linkImgCallback(t);
                var i = document.createElement("img");
                i.onload = function() {
                    i = null
                }
                ,
                i.onerror = function() {
                    e.alert(r("插入图片错误"), "wangEditor: " + r("插入图片错误") + "，" + r("图片链接") + ' "' + t + '"，' + r("下载链接失败")),
                    i = null
                }
                ,
                i.onabort = function() {
                    return i = null
                }
                ,
                i.src = t
            }
            ,
            t.prototype.uploadImg = function(t) {
                var e = this;
                if (t.length) {
                    var n = this.editor
                      , o = n.config
                      , u = function(t) {
                        return n.i18next.t("validate." + t)
                    }
                      , f = o.uploadImgServer
                      , d = o.uploadImgShowBase64
                      , p = o.uploadImgMaxSize
                      , A = p / 1024 / 1024
                      , v = o.uploadImgMaxLength
                      , h = o.uploadFileName
                      , g = o.uploadImgParams
                      , m = o.uploadImgParamsWithUrl
                      , y = o.uploadImgHeaders
                      , w = o.uploadImgHooks
                      , x = o.uploadImgTimeout
                      , E = o.withCredentials
                      , b = o.customUploadImg;
                    if (b || f || d) {
                        var _ = []
                          , S = [];
                        if (l.arrForEach(t, (function(t) {
                            var e = t.name
                              , n = t.size;
                            e && n && (!1 !== /\.(jpg|jpeg|png|bmp|gif|webp)$/i.test(e) ? p < n ? S.push("【" + e + "】" + u("大于") + " " + A + "M") : _.push(t) : S.push("【" + e + "】" + u("不是图片")))
                        }
                        )),
                        S.length)
                            this.alert(u("图片验证未通过") + ": \n" + S.join("\n"));
                        else if (_.length > v)
                            this.alert(u("一次最多上传") + v + u("张图片"));
                        else if (b && "function" == typeof b) {
                            var M;
                            b(_, (0,
                            a.default)(M = this.insertImg).call(M, this))
                        } else {
                            var C = new FormData;
                            if ((0,
                            i.default)(_).call(_, (function(t, e) {
                                var n = h || t.name;
                                _.length > 1 && (n += e + 1),
                                C.append(n, t)
                            }
                            )),
                            f) {
                                var B = f.split("#");
                                f = B[0];
                                var k = B[1] || "";
                                (0,
                                i.default)(l).call(l, g, (function(t, e) {
                                    m && ((0,
                                    r.default)(f).call(f, "?") > 0 ? f += "&" : f += "?",
                                    f = f + t + "=" + e),
                                    C.append(t, e)
                                }
                                )),
                                k && (f += "#" + k);
                                var I = c.default(f, {
                                    timeout: x,
                                    formData: C,
                                    headers: y,
                                    withCredentials: !!E,
                                    beforeSend: function(t) {
                                        if (w.before)
                                            return w.before(t, n, _)
                                    },
                                    onTimeout: function(t) {
                                        e.alert(u("上传图片超时")),
                                        w.timeout && w.timeout(t, n)
                                    },
                                    onProgress: function(t, e) {
                                        var o = new s.default(n);
                                        e.lengthComputable && (t = e.loaded / e.total,
                                        o.show(t))
                                    },
                                    onError: function(t) {
                                        e.alert(u("上传图片错误"), u("上传图片错误") + "，" + u("服务器返回状态") + ": " + t.status),
                                        w.error && w.error(t, n)
                                    },
                                    onFail: function(t, o) {
                                        e.alert(u("上传图片失败"), u("上传图片返回结果错误") + "，" + u("返回结果") + ": " + o),
                                        w.fail && w.fail(t, n, o)
                                    },
                                    onSuccess: function(t, o) {
                                        if (w.customInsert) {
                                            var r;
                                            w.customInsert((0,
                                            a.default)(r = e.insertImg).call(r, e), o, n)
                                        } else {
                                            if ("0" != o.errno)
                                                return e.alert(u("上传图片失败"), u("上传图片返回结果错误") + "，" + u("返回结果") + " errno=" + o.errno),
                                                void (w.fail && w.fail(t, n, o));
                                            var l = o.data;
                                            (0,
                                            i.default)(l).call(l, (function(t) {
                                                e.insertImg(t)
                                            }
                                            )),
                                            w.success && w.success(t, n, o)
                                        }
                                    }
                                });
                                "string" == typeof I && this.alert(I)
                            } else
                                d && l.arrForEach(t, (function(t) {
                                    var n = e
                                      , o = new FileReader;
                                    o.readAsDataURL(t),
                                    o.onload = function() {
                                        this.result && n.insertImg(this.result.toString())
                                    }
                                }
                                ))
                        }
                    }
                }
            }
            ,
            t
        }();
        e.default = f
    }
    , function(t, e, n) {
        var o = n(14)
          , r = n(11)
          , i = n(65);
        t.exports = !o && !r((function() {
            return 7 != Object.defineProperty(i("div"), "a", {
                get: function() {
                    return 7
                }
            }).a
        }
        ))
    }
    , function(t, e, n) {
        var o = n(11)
          , r = /#|\.prototype\./
          , i = function(t, e) {
            var n = u[a(t)];
            return n == c || n != l && ("function" == typeof e ? o(e) : !!e)
        }
          , a = i.normalize = function(t) {
            return String(t).replace(r, ".").toLowerCase()
        }
          , u = i.data = {}
          , l = i.NATIVE = "N"
          , c = i.POLYFILL = "P";
        t.exports = i
    }
    , function(t, e, n) {
        var o = n(89)
          , r = Function.toString;
        "function" != typeof o.inspectSource && (o.inspectSource = function(t) {
            return r.call(t)
        }
        ),
        t.exports = o.inspectSource
    }
    , function(t, e, n) {
        var o = n(7)
          , r = n(150)
          , i = o["__core-js_shared__"] || r("__core-js_shared__", {});
        t.exports = i
    }
    , function(t, e, n) {
        "use strict";
        var o, r, i, a = n(91), u = n(18), l = n(15), c = n(8), s = n(33), f = c("iterator"), d = !1;
        [].keys && ("next"in (i = [].keys()) ? (r = a(a(i))) !== Object.prototype && (o = r) : d = !0),
        null == o && (o = {}),
        s || l(o, f) || u(o, f, (function() {
            return this
        }
        )),
        t.exports = {
            IteratorPrototype: o,
            BUGGY_SAFARI_ITERATORS: d
        }
    }
    , function(t, e, n) {
        var o = n(15)
          , r = n(34)
          , i = n(55)
          , a = n(152)
          , u = i("IE_PROTO")
          , l = Object.prototype;
        t.exports = a ? Object.getPrototypeOf : function(t) {
            return t = r(t),
            o(t, u) ? t[u] : "function" == typeof t.constructor && t instanceof t.constructor ? t.constructor.prototype : t instanceof Object ? l : null
        }
    }
    , function(t, e, n) {
        var o = n(68);
        t.exports = o && !Symbol.sham && "symbol" == typeof Symbol.iterator
    }
    , function(t, e, n) {
        var o = n(15)
          , r = n(27)
          , i = n(70).indexOf
          , a = n(45);
        t.exports = function(t, e) {
            var n, u = r(t), l = 0, c = [];
            for (n in u)
                !o(a, n) && o(u, n) && c.push(n);
            for (; e.length > l; )
                o(u, n = e[l++]) && (~i(c, n) || c.push(n));
            return c
        }
    }
    , function(t, e, n) {
        var o = n(36);
        t.exports = o("document", "documentElement")
    }
    , function(t, e, n) {
        var o = n(20)
          , r = n(155);
        t.exports = Object.setPrototypeOf || ("__proto__"in {} ? function() {
            var t, e = !1, n = {};
            try {
                (t = Object.getOwnPropertyDescriptor(Object.prototype, "__proto__").set).call(n, []),
                e = n instanceof Array
            } catch (t) {}
            return function(n, i) {
                return o(n),
                r(i),
                e ? t.call(n, i) : n.__proto__ = i,
                n
            }
        }() : void 0)
    }
    , function(t, e, n) {
        var o = n(7);
        t.exports = o.Promise
    }
    , function(t, e, n) {
        var o = n(46);
        t.exports = function(t, e, n) {
            for (var r in e)
                n && n.unsafe && t[r] ? t[r] = e[r] : o(t, r, e[r], n);
            return t
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(36)
          , r = n(16)
          , i = n(8)
          , a = n(14)
          , u = i("species");
        t.exports = function(t) {
            var e = o(t)
              , n = r.f;
            a && e && !e[u] && n(e, u, {
                configurable: !0,
                get: function() {
                    return this
                }
            })
        }
    }
    , function(t, e, n) {
        var o = n(20)
          , r = n(31)
          , i = n(8)("species");
        t.exports = function(t, e) {
            var n, a = o(t).constructor;
            return void 0 === a || null == (n = o(a)[i]) ? e : r(n)
        }
    }
    , function(t, e, n) {
        var o, r, i, a = n(7), u = n(11), l = n(28), c = n(44), s = n(94), f = n(65), d = n(101), p = a.location, A = a.setImmediate, v = a.clearImmediate, h = a.process, g = a.MessageChannel, m = a.Dispatch, y = 0, w = {}, x = function(t) {
            if (w.hasOwnProperty(t)) {
                var e = w[t];
                delete w[t],
                e()
            }
        }, E = function(t) {
            return function() {
                x(t)
            }
        }, b = function(t) {
            x(t.data)
        }, _ = function(t) {
            a.postMessage(t + "", p.protocol + "//" + p.host)
        };
        A && v || (A = function(t) {
            for (var e = [], n = 1; arguments.length > n; )
                e.push(arguments[n++]);
            return w[++y] = function() {
                ("function" == typeof t ? t : Function(t)).apply(void 0, e)
            }
            ,
            o(y),
            y
        }
        ,
        v = function(t) {
            delete w[t]
        }
        ,
        "process" == l(h) ? o = function(t) {
            h.nextTick(E(t))
        }
        : m && m.now ? o = function(t) {
            m.now(E(t))
        }
        : g && !d ? (i = (r = new g).port2,
        r.port1.onmessage = b,
        o = c(i.postMessage, i, 1)) : !a.addEventListener || "function" != typeof postMessage || a.importScripts || u(_) || "file:" === p.protocol ? o = "onreadystatechange"in f("script") ? function(t) {
            s.appendChild(f("script")).onreadystatechange = function() {
                s.removeChild(this),
                x(t)
            }
        }
        : function(t) {
            setTimeout(E(t), 0)
        }
        : (o = _,
        a.addEventListener("message", b, !1))),
        t.exports = {
            set: A,
            clear: v
        }
    }
    , function(t, e, n) {
        var o = n(76);
        t.exports = /(iphone|ipod|ipad).*applewebkit/i.test(o)
    }
    , function(t, e, n) {
        var o = n(20)
          , r = n(13)
          , i = n(77);
        t.exports = function(t, e) {
            if (o(t),
            r(e) && e.constructor === t)
                return e;
            var n = i.f(t);
            return (0,
            n.resolve)(e),
            n.promise
        }
    }
    , function(t, e) {
        t.exports = function(t) {
            try {
                return {
                    error: !1,
                    value: t()
                }
            } catch (t) {
                return {
                    error: !0,
                    value: t
                }
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(11);
        t.exports = function(t, e) {
            var n = [][t];
            return !!n && o((function() {
                n.call(null, e || function() {
                    throw 1
                }
                , 1)
            }
            ))
        }
    }
    , function(t, e, n) {
        t.exports = n(176)
    }
    , function(t, e, n) {
        t.exports = n(185)
    }
    , function(t, e, n) {
        t.exports = n(189)
    }
    , function(t, e, n) {
        t.exports = n(197)
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(7)
          , i = n(110)
          , a = n(11)
          , u = n(18)
          , l = n(59)
          , c = n(75)
          , s = n(13)
          , f = n(29)
          , d = n(16).f
          , p = n(38).forEach
          , A = n(14)
          , v = n(32)
          , h = v.set
          , g = v.getterFor;
        t.exports = function(t, e, n) {
            var v, m = -1 !== t.indexOf("Map"), y = -1 !== t.indexOf("Weak"), w = m ? "set" : "add", x = r[t], E = x && x.prototype, b = {};
            if (A && "function" == typeof x && (y || E.forEach && !a((function() {
                (new x).entries().next()
            }
            )))) {
                v = e((function(e, n) {
                    h(c(e, v, t), {
                        type: t,
                        collection: new x
                    }),
                    null != n && l(n, e[w], e, m)
                }
                ));
                var _ = g(t);
                p(["add", "clear", "delete", "forEach", "get", "has", "set", "keys", "values", "entries"], (function(t) {
                    var e = "add" == t || "set" == t;
                    !(t in E) || y && "clear" == t || u(v.prototype, t, (function(n, o) {
                        var r = _(this).collection;
                        if (!e && y && !s(n))
                            return "get" == t && void 0;
                        var i = r[t](0 === n ? 0 : n, o);
                        return e ? this : i
                    }
                    ))
                }
                )),
                y || d(v.prototype, "size", {
                    configurable: !0,
                    get: function() {
                        return _(this).collection.size
                    }
                })
            } else
                v = n.getConstructor(e, t, m, w),
                i.REQUIRED = !0;
            return f(v, t, !1, !0),
            b[t] = v,
            o({
                global: !0,
                forced: !0
            }, b),
            y || n.setStrong(v, t, m),
            v
        }
    }
    , function(t, e, n) {
        var o = n(45)
          , r = n(13)
          , i = n(15)
          , a = n(16).f
          , u = n(56)
          , l = n(200)
          , c = u("meta")
          , s = 0
          , f = Object.isExtensible || function() {
            return !0
        }
          , d = function(t) {
            a(t, c, {
                value: {
                    objectID: "O" + ++s,
                    weakData: {}
                }
            })
        }
          , p = t.exports = {
            REQUIRED: !1,
            fastKey: function(t, e) {
                if (!r(t))
                    return "symbol" == typeof t ? t : ("string" == typeof t ? "S" : "P") + t;
                if (!i(t, c)) {
                    if (!f(t))
                        return "F";
                    if (!e)
                        return "E";
                    d(t)
                }
                return t[c].objectID
            },
            getWeakData: function(t, e) {
                if (!i(t, c)) {
                    if (!f(t))
                        return !0;
                    if (!e)
                        return !1;
                    d(t)
                }
                return t[c].weakData
            },
            onFreeze: function(t) {
                return l && p.REQUIRED && f(t) && !i(t, c) && d(t),
                t
            }
        };
        o[c] = !0
    }
    , function(t, e, n) {
        "use strict";
        var o = n(16).f
          , r = n(57)
          , i = n(97)
          , a = n(44)
          , u = n(75)
          , l = n(59)
          , c = n(67)
          , s = n(98)
          , f = n(14)
          , d = n(110).fastKey
          , p = n(32)
          , A = p.set
          , v = p.getterFor;
        t.exports = {
            getConstructor: function(t, e, n, c) {
                var s = t((function(t, o) {
                    u(t, s, e),
                    A(t, {
                        type: e,
                        index: r(null),
                        first: void 0,
                        last: void 0,
                        size: 0
                    }),
                    f || (t.size = 0),
                    null != o && l(o, t[c], t, n)
                }
                ))
                  , p = v(e)
                  , h = function(t, e, n) {
                    var o, r, i = p(t), a = g(t, e);
                    return a ? a.value = n : (i.last = a = {
                        index: r = d(e, !0),
                        key: e,
                        value: n,
                        previous: o = i.last,
                        next: void 0,
                        removed: !1
                    },
                    i.first || (i.first = a),
                    o && (o.next = a),
                    f ? i.size++ : t.size++,
                    "F" !== r && (i.index[r] = a)),
                    t
                }
                  , g = function(t, e) {
                    var n, o = p(t), r = d(e);
                    if ("F" !== r)
                        return o.index[r];
                    for (n = o.first; n; n = n.next)
                        if (n.key == e)
                            return n
                };
                return i(s.prototype, {
                    clear: function() {
                        for (var t = p(this), e = t.index, n = t.first; n; )
                            n.removed = !0,
                            n.previous && (n.previous = n.previous.next = void 0),
                            delete e[n.index],
                            n = n.next;
                        t.first = t.last = void 0,
                        f ? t.size = 0 : this.size = 0
                    },
                    delete: function(t) {
                        var e = p(this)
                          , n = g(this, t);
                        if (n) {
                            var o = n.next
                              , r = n.previous;
                            delete e.index[n.index],
                            n.removed = !0,
                            r && (r.next = o),
                            o && (o.previous = r),
                            e.first == n && (e.first = o),
                            e.last == n && (e.last = r),
                            f ? e.size-- : this.size--
                        }
                        return !!n
                    },
                    forEach: function(t) {
                        for (var e, n = p(this), o = a(t, arguments.length > 1 ? arguments[1] : void 0, 3); e = e ? e.next : n.first; )
                            for (o(e.value, e.key, this); e && e.removed; )
                                e = e.previous
                    },
                    has: function(t) {
                        return !!g(this, t)
                    }
                }),
                i(s.prototype, n ? {
                    get: function(t) {
                        var e = g(this, t);
                        return e && e.value
                    },
                    set: function(t, e) {
                        return h(this, 0 === t ? 0 : t, e)
                    }
                } : {
                    add: function(t) {
                        return h(this, t = 0 === t ? 0 : t, t)
                    }
                }),
                f && o(s.prototype, "size", {
                    get: function() {
                        return p(this).size
                    }
                }),
                s
            },
            setStrong: function(t, e, n) {
                var o = e + " Iterator"
                  , r = v(e)
                  , i = v(o);
                c(t, e, (function(t, e) {
                    A(this, {
                        type: o,
                        target: t,
                        state: r(t),
                        kind: e,
                        last: void 0
                    })
                }
                ), (function() {
                    for (var t = i(this), e = t.kind, n = t.last; n && n.removed; )
                        n = n.previous;
                    return t.target && (t.last = n = n ? n.next : t.state.first) ? "keys" == e ? {
                        value: n.key,
                        done: !1
                    } : "values" == e ? {
                        value: n.value,
                        done: !1
                    } : {
                        value: [n.key, n.value],
                        done: !1
                    } : (t.target = void 0,
                    {
                        value: void 0,
                        done: !0
                    })
                }
                ), n ? "entries" : "values", !n, !0),
                s(e)
            }
        }
    }
    , function(t, e, n) {
        var o = n(43)
          , r = "[" + n(81) + "]"
          , i = RegExp("^" + r + r + "*")
          , a = RegExp(r + r + "*$")
          , u = function(t) {
            return function(e) {
                var n = String(o(e));
                return 1 & t && (n = n.replace(i, "")),
                2 & t && (n = n.replace(a, "")),
                n
            }
        };
        t.exports = {
            start: u(1),
            end: u(2),
            trim: u(3)
        }
    }
    , function(t, e, n) {
        t.exports = n(210)
    }
    , function(t, e, n) {
        var o = n(217)
          , r = n(220);
        function i(e) {
            return t.exports = i = "function" == typeof r && "symbol" == typeof o ? function(t) {
                return typeof t
            }
            : function(t) {
                return t && "function" == typeof r && t.constructor === r && t !== r.prototype ? "symbol" : typeof t
            }
            ,
            i(e)
        }
        t.exports = i
    }
    , function(t, e, n) {
        n(10)("iterator")
    }
    , function(t, e, n) {
        var o = n(93)
          , r = n(72).concat("length", "prototype");
        e.f = Object.getOwnPropertyNames || function(t) {
            return o(t, r)
        }
    }
    , function(t, e) {
        e.f = Object.getOwnPropertySymbols
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = {
            zIndex: 1e4
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.getPasteImgs = e.getPasteHtml = e.getPasteText = void 0;
        var a = n(9)
          , u = i(n(274));
        function l(t) {
            var e = t.clipboardData
              , n = "";
            return n = null == e ? window.clipboardData && window.clipboardData.getData("text") : e.getData("text/plain"),
            a.replaceHtmlSymbol(n)
        }
        e.getPasteText = l,
        e.getPasteHtml = function(t, e, n) {
            void 0 === e && (e = !0),
            void 0 === n && (n = !1);
            var o = t.clipboardData
              , r = "";
            if (o && (r = o.getData("text/html")),
            !r) {
                var i = l(t);
                if (!i)
                    return "";
                r = "<p>" + i + "</p>"
            }
            return r = u.default(r, e, n)
        }
        ,
        e.getPasteImgs = function(t) {
            var e = [];
            if (l(t))
                return e;
            var n = t.clipboardData.items;
            return n ? ((0,
            r.default)(a).call(a, n, (function(t, n) {
                var o = n.type;
                /image/i.test(o) && e.push(n.getAsFile())
            }
            )),
            e) : e
        }
    }
    , function(t, e, n) {
        t.exports = n(276)
    }
    , function(t, e, n) {
        t.exports = n(283)
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = o(n(1))
          , a = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var u = a(n(2))
          , l = n(40)
          , c = function() {
            function t(t, e) {
                var n = this;
                this.hideTimeoutId = 0,
                this.showTimeoutId = 0,
                this.menu = t,
                this.conf = e;
                var o = u.default('<div class="w-e-droplist"></div>')
                  , i = u.default("<p>" + e.title + "</p>");
                i.addClass("w-e-dp-title"),
                o.append(i);
                var a = e.list || []
                  , c = e.type || "list"
                  , s = e.clickHandler || l.EMPTY_FN
                  , f = u.default('<ul class="' + ("list" === c ? "w-e-list" : "w-e-block") + '"></ul>');
                (0,
                r.default)(a).call(a, (function(t) {
                    var e = t.$elem
                      , o = t.value
                      , r = u.default('<li class="w-e-item"></li>');
                    e && (r.append(e),
                    f.append(r),
                    r.on("click", (function() {
                        s(o),
                        n.hideTimeoutId = window.setTimeout((function() {
                            n.hide()
                        }
                        ))
                    }
                    )))
                }
                )),
                o.append(f),
                o.on("mouseleave", (function() {
                    n.hideTimeoutId = window.setTimeout((function() {
                        n.hide()
                    }
                    ))
                }
                )),
                this.$container = o,
                this.rendered = !1,
                this._show = !1
            }
            return t.prototype.show = function() {
                this.hideTimeoutId && clearTimeout(this.hideTimeoutId);
                var t = this.menu.$elem
                  , e = this.$container;
                if (!this._show) {
                    if (this.rendered)
                        e.show();
                    else {
                        var n = t.getSizeData().height || 0
                          , o = this.conf.width || 100;
                        e.css("margin-top", n + "px").css("width", o + "px"),
                        t.append(e),
                        this.rendered = !0
                    }
                    this._show = !0
                }
            }
            ,
            t.prototype.hide = function() {
                this.showTimeoutId && clearTimeout(this.showTimeoutId);
                var t = this.$container;
                this._show && (t.hide(),
                this._show = !1)
            }
            ,
            (0,
            i.default)(t.prototype, "isShow", {
                get: function() {
                    return this._show
                },
                enumerable: !1,
                configurable: !0
            }),
            t
        }();
        e.default = c
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = function(t) {
            var e = t.selection.getSelectionContainerElem();
            return !!e && "A" === e.getNodeName()
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = function(t) {
            var e = t.selection.getSelectionContainerElem();
            return !!e && !("CODE" != e.getNodeName() && "PRE" != e.getNodeName() && "CODE" != e.parent().getNodeName() && "PRE" != e.parent().getNodeName() && !/hljs/.test(e.parent().attr("class")))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        n(130),
        n(132),
        n(136),
        n(138),
        n(140),
        n(142),
        n(144);
        var r = o(n(167));
        try {
            document
        } catch (t) {
            throw new Error("请在浏览器环境下运行")
        }
        e.default = r.default
    }
    , function(t, e, n) {
        var o = n(127);
        t.exports = o
    }
    , function(t, e, n) {
        n(128);
        var o = n(12).Object
          , r = t.exports = function(t, e, n) {
            return o.defineProperty(t, e, n)
        }
        ;
        o.defineProperty.sham && (r.sham = !0)
    }
    , function(t, e, n) {
        var o = n(4)
          , r = n(14);
        o({
            target: "Object",
            stat: !0,
            forced: !r,
            sham: !r
        }, {
            defineProperty: n(16).f
        })
    }
    , function(t, e) {
        var n;
        n = function() {
            return this
        }();
        try {
            n = n || new Function("return this")()
        } catch (t) {
            "object" == typeof window && (n = window)
        }
        t.exports = n
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(131);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        (e = n(22)(!1)).push([t.i, '.w-e-toolbar,\n.w-e-text-container,\n.w-e-menu-panel {\n  padding: 0;\n  margin: 0;\n  box-sizing: border-box;\n  background-color: #fff;\n  /*表情菜单样式*/\n  /*分割线样式*/\n}\n.w-e-toolbar .eleImg,\n.w-e-text-container .eleImg,\n.w-e-menu-panel .eleImg {\n  cursor: pointer;\n  display: inline-block;\n  font-size: 18px;\n  padding: 0 3px;\n}\n.w-e-toolbar *,\n.w-e-text-container *,\n.w-e-menu-panel * {\n  padding: 0;\n  margin: 0;\n  box-sizing: border-box;\n}\n.w-e-toolbar hr,\n.w-e-text-container hr,\n.w-e-menu-panel hr {\n  cursor: pointer;\n  display: block;\n  height: 0px;\n  border: 0;\n  border-top: 3px solid #ccc;\n  margin: 20px 0;\n}\n.w-e-clear-fix:after {\n  content: "";\n  display: table;\n  clear: both;\n}\n.w-e-drop-list-item {\n  position: relative;\n  top: 1px;\n  padding-right: 7px;\n  color: #333 !important;\n}\n.w-e-drop-list-tl {\n  padding-left: 10px;\n  text-align: left;\n}\n', ""]),
        t.exports = e
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(133);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        var o = n(22)
          , r = n(134)
          , i = n(135);
        e = o(!1);
        var a = r(i);
        e.push([t.i, "@font-face {\n  font-family: 'w-e-icon';\n  src: url(" + a + ') format(\'truetype\');\n  font-weight: normal;\n  font-style: normal;\n}\n[class^="w-e-icon-"],\n[class*=" w-e-icon-"] {\n  /* use !important to prevent issues with browser extensions that change fonts */\n  font-family: \'w-e-icon\' !important;\n  speak: none;\n  font-style: normal;\n  font-weight: normal;\n  font-variant: normal;\n  text-transform: none;\n  line-height: 1;\n  /* Better Font Rendering =========== */\n  -webkit-font-smoothing: antialiased;\n  -moz-osx-font-smoothing: grayscale;\n}\n.w-e-icon-close:before {\n  content: "\\f00d";\n}\n.w-e-icon-upload2:before {\n  content: "\\e9c6";\n}\n.w-e-icon-trash-o:before {\n  content: "\\f014";\n}\n.w-e-icon-header:before {\n  content: "\\f1dc";\n}\n.w-e-icon-pencil2:before {\n  content: "\\e906";\n}\n.w-e-icon-paint-brush:before {\n  content: "\\f1fc";\n}\n.w-e-icon-image:before {\n  content: "\\e90d";\n}\n.w-e-icon-play:before {\n  content: "\\e912";\n}\n.w-e-icon-location:before {\n  content: "\\e947";\n}\n.w-e-icon-undo:before {\n  content: "\\e965";\n}\n.w-e-icon-redo:before {\n  content: "\\e966";\n}\n.w-e-icon-quotes-left:before {\n  content: "\\e977";\n}\n.w-e-icon-list-numbered:before {\n  content: "\\e9b9";\n}\n.w-e-icon-list2:before {\n  content: "\\e9bb";\n}\n.w-e-icon-link:before {\n  content: "\\e9cb";\n}\n.w-e-icon-happy:before {\n  content: "\\e9df";\n}\n.w-e-icon-bold:before {\n  content: "\\ea62";\n}\n.w-e-icon-underline:before {\n  content: "\\ea63";\n}\n.w-e-icon-italic:before {\n  content: "\\ea64";\n}\n.w-e-icon-strikethrough:before {\n  content: "\\ea65";\n}\n.w-e-icon-table2:before {\n  content: "\\ea71";\n}\n.w-e-icon-paragraph-left:before {\n  content: "\\ea77";\n}\n.w-e-icon-paragraph-center:before {\n  content: "\\ea78";\n}\n.w-e-icon-paragraph-right:before {\n  content: "\\ea79";\n}\n.w-e-icon-terminal:before {\n  content: "\\f120";\n}\n.w-e-icon-page-break:before {\n  content: "\\ea68";\n}\n.w-e-icon-cancel-circle:before {\n  content: "\\ea0d";\n}\n.w-e-icon-font:before {\n  content: "\\ea5c";\n}\n.w-e-icon-text-heigh:before {\n  content: "\\ea5f";\n}\n.w-e-icon-paint-format:before {\n  content: "\\e90c";\n}\n.w-e-icon-indent-increase:before {\n  content: "\\ea7b";\n}\n.w-e-icon-indent-decrease:before {\n  content: "\\ea7c";\n}\n.w-e-icon-row-height:before {\n  content: "\\e9be";\n}\n.w-e-icon-fullscreen_exit:before {\n  content: "\\e900";\n}\n.w-e-icon-fullscreen:before {\n  content: "\\e901";\n}\n.w-e-icon-split-line:before {\n  content: "\\ea0b";\n}\n.w-e-icon-checkbox-checked:before {\n  content: "\\ea52";\n}\n', ""]),
        t.exports = e
    }
    , function(t, e, n) {
        "use strict";
        t.exports = function(t, e) {
            return e || (e = {}),
            "string" != typeof (t = t && t.__esModule ? t.default : t) ? t : (/^['"].*['"]$/.test(t) && (t = t.slice(1, -1)),
            e.hash && (t += e.hash),
            /["'() \t\n]/.test(t) || e.needQuotes ? '"'.concat(t.replace(/"/g, '\\"').replace(/\n/g, "\\n"), '"') : t)
        }
    }
    , function(t, e, n) {
        "use strict";
        n.r(e),
        e.default = "data:font/woff;base64,d09GRgABAAAAABrcAAsAAAAAGpAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABPUy8yAAABCAAAAGAAAABgDxIPFWNtYXAAAAFoAAABJAAAAST/4dF7Z2FzcAAAAowAAAAIAAAACAAAABBnbHlmAAAClAAAFTAAABUwrNIRu2hlYWQAABfEAAAANgAAADYbkuK6aGhlYQAAF/wAAAAkAAAAJAkjBWhobXR4AAAYIAAAAKAAAACglYcEbmxvY2EAABjAAAAAUgAAAFJegllAbWF4cAAAGRQAAAAgAAAAIAAzALZuYW1lAAAZNAAAAYYAAAGGmUoJ+3Bvc3QAABq8AAAAIAAAACAAAwAAAAMD7wGQAAUAAAKZAswAAACPApkCzAAAAesAMwEJAAAAAAAAAAAAAAAAAAAAARAAAAAAAAAAAAAAAAAAAAAAQAAA8fwDwP/AAEADwABAAAAAAQAAAAAAAAAAAAAAIAAAAAAAAwAAAAMAAAAcAAEAAwAAABwAAwABAAAAHAAEAQgAAAA+ACAABAAeAAEAIOkB6QbpDekS6UfpZul36bnpu+m+6cbpy+nf6gvqDepS6lzqX+pl6nHqeep88A3wFPEg8dzx/P/9//8AAAAAACDpAOkG6QzpEulH6WXpd+m56bvpvunG6cvp3+oL6g3qUupc6l/qYupx6nfqe/AN8BTxIPHc8fz//f//AAH/4xcEFwAW+xb3FsMWphaWFlUWVBZSFksWRxY0FgkWCBXEFbsVuRW3FawVpxWmEBYQEA8FDkoOKwADAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAH//wAPAAEAAAAAAAAAAAACAAA3OQEAAAAAAQAAAAAAAAAAAAIAADc5AQAAAAABAAAAAAAAAAAAAgAANzkBAAAAAAQAQQABA78DfwAFAAsAEQAXAAABMxUhETMDESEVIxUBNTMRITURNSERIzUC/8D+wn5+AT7A/gJ+/sIBPn4Cv34BPvyCAT5+wAK+wP7Cfv4Cfv7CwAAAAAAEAEEAAQO/A38ABQALABEAFwAAASERIzUjEzUzESE1AREhFSMVERUzFSERAoEBPn7AwH7+wv3AAT7AwP7CA3/+wsD9fsD+wn4BwgE+fsD+/sB+AT4AAAAAAgAA/8AEAAPAAAQAEwAAATcBJwEDLgEnEzcBIwEDJQE1AQcBgIABwED+QJ8XOzJjgAGAwP6AwAKAAYD+gE4BQEABwED+QP6dMjsXARFOAYD+gP2AwAGAwP6AgAACAAD/wAQAA4AAKQAtAAABESM1NCYjISIGHQEUFjMhMjY9ATMRIRUjIgYVERQWOwEyNjURNCYrATUBITUhBADAJhr9QBomJhoCwBomgP3AIA0TEw2ADRMTDSABQP1AAsABgAGAQBomJhrAGiYmGkD/AIATDf7ADRMTDQFADRNAAYBAAAAEAAAAAAQAA4AAEAAhAC0ANAAAATgBMRE4ATEhOAExETgBMSE1ISIGFREUFjMhMjY1ETQmIwcUBiMiJjU0NjMyFhMhNRMBMzcDwPyAA4D8gBomJhoDgBomJhqAOCgoODgoKDhA/QDgAQBA4ANA/QADAEAmGv0AGiYmGgMAGibgKDg4KCg4OP24gAGA/sDAAAACAAAAQAQAA0AAOAA8AAABJicuAScmIyIHDgEHBgcGBw4BBwYVFBceARcWFxYXHgEXFjMyNz4BNzY3Njc+ATc2NTQnLgEnJicBEQ0BA9U2ODl2PD0/Pz08djk4NgsHCAsDAwMDCwgHCzY4OXY8PT8/PTx2OTg2CwcICwMDAwMLCAcL/asBQP7AAyAIBgYIAgICAggGBggpKipZLS4vLy4tWSoqKQgGBggCAgICCAYGCCkqKlktLi8vLi1ZKiop/eABgMDAAAAAAAIAwP/AA0ADwAAbACcAAAEiBw4BBwYVFBceARcWMTA3PgE3NjU0Jy4BJyYDIiY1NDYzMhYVFAYCAEI7OlcZGTIyeDIyMjJ4MjIZGVc6O0JQcHBQUHBwA8AZGVc6O0J4fX3MQUFBQcx9fXhCOzpXGRn+AHBQUHBwUFBwAAABAAAAAAQAA4AAKwAAASIHDgEHBgcnESEnPgEzMhceARcWFRQHDgEHBgcXNjc+ATc2NTQnLgEnJiMCADUyMlwpKSOWAYCQNYtQUEVGaR4eCQkiGBgeVSggIC0MDCgoi15dagOACgsnGxwjlv6AkDQ8Hh5pRkVQKygpSSAhGmAjKytiNjY5al1eiygoAAEAAAAABAADgAAqAAATFBceARcWFzcmJy4BJyY1NDc+ATc2MzIWFwchEQcmJy4BJyYjIgcOAQcGAAwMLSAgKFUeGBgiCQkeHmlGRVBQizWQAYCWIykpXDIyNWpdXosoKAGAOTY2YisrI2AaISBJKSgrUEVGaR4ePDSQAYCWIxwbJwsKKCiLXl0AAAAAAgAAAEAEAQMAACYATQAAEzIXHgEXFhUUBw4BBwYjIicuAScmNSc0Nz4BNzYzFSIGBw4BBz4BITIXHgEXFhUUBw4BBwYjIicuAScmNSc0Nz4BNzYzFSIGBw4BBz4B4S4pKT0REhIRPSkpLi4pKT0REgEjI3pSUV1AdS0JEAcIEgJJLikpPRESEhE9KSkuLikpPRESASMjelJRXUB1LQkQBwgSAgASET0pKS4uKSk9ERISET0pKS4gXVFSeiMjgDAuCBMKAgESET0pKS4uKSk9ERISET0pKS4gXVFSeiMjgDAuCBMKAgEAAAYAQP/ABAADwAADAAcACwARAB0AKQAAJSEVIREhFSERIRUhJxEjNSM1ExUzFSM1NzUjNTMVFREjNTM1IzUzNSM1AYACgP2AAoD9gAKA/YDAQEBAgMCAgMDAgICAgICAAgCAAgCAwP8AwED98jJAkjwyQJLu/sBAQEBAQAAGAAD/wAQAA8AAAwAHAAsAFwAjAC8AAAEhFSERIRUhESEVIQE0NjMyFhUUBiMiJhE0NjMyFhUUBiMiJhE0NjMyFhUUBiMiJgGAAoD9gAKA/YACgP2A/oBLNTVLSzU1S0s1NUtLNTVLSzU1S0s1NUsDgID/AID/AIADQDVLSzU1S0v+tTVLSzU1S0v+tTVLSzU1S0sABQAAAEAFYAMAAAMABwALAA4AEQAAEyEVIRUhFSEVIRUhARc3NScHAAOA/IADgPyAA4D8gAPgwMDAwAMAwEDAQMABQMDAQMDAAAAAAAMAAAAABAADoAADAA0AFAAANyEVISUVITUTIRUhNSElCQEjESMRAAQA/AAEAPwAgAEAAQABAP1gASABIOCAQEDAQEABAICAwAEg/uD/AAEAAAAAAAIAHv/MA+IDtAAzAGQAAAEiJicmJyY0NzY/AT4BMzIWFxYXFhQHBg8BBiInJjQ/ATY0Jy4BIyIGDwEGFBcWFAcOASMDIiYnJicmNDc2PwE2MhcWFA8BBhQXHgEzMjY/ATY0JyY0NzYyFxYXFhQHBg8BDgEjAbgKEwgjEhISEiPAI1kxMVkjIxISEhIjWA8sDw8PWCkpFDMcHDMUwCkpDw8IEwq4MVkjIxISEhIjWA8sDw8PWCkpFDMcHDMUwCkpDw8PKxAjEhISEiPAI1kxAUQIByQtLV4tLSTAIiUlIiQtLV4tLSRXEBAPKw9YKXQpFBUVFMApdCkPKxAHCP6IJSIkLS1eLS0kVxAQDysPWCl0KRQVFRTAKXQpDysQDw8kLS1eLS0kwCIlAAAAAAUAAP/ABAADwAAbADcAUwBfAGsAAAUyNz4BNzY1NCcuAScmIyIHDgEHBhUUFx4BFxYTMhceARcWFRQHDgEHBiMiJy4BJyY1NDc+ATc2EzI3PgE3NjcGBw4BBwYjIicuAScmJxYXHgEXFic0NjMyFhUUBiMiJiU0NjMyFhUUBiMiJgIAal1eiygoKCiLXl1qal1eiygoKCiLXl1qVkxMcSAhISBxTExWVkxMcSAhISBxTExWKysqUSYmIwUcG1Y4Nz8/NzhWGxwFIyYmUSor1SUbGyUlGxslAYAlGxslJRsbJUAoKIteXWpqXV6LKCgoKIteXWpqXV6LKCgDoCEgcUxMVlZMTHEgISEgcUxMVlZMTHEgIf4JBgYVEBAUQzo6VhgZGRhWOjpDFBAQFQYG9yg4OCgoODgoKDg4KCg4OAAAAQAAAUAEAAJAAA8AABMVFBYzITI2PQE0JiMhIgYAEw0DwA0TEw38QA0TAiDADRMTDcANExMAAAADAAD/wAQAA8AAGwA3AEMAAAEiBw4BBwYVFBceARcWMzI3PgE3NjU0Jy4BJyYDIicuAScmNTQ3PgE3NjMyFx4BFxYVFAcOAQcGEwcnBxcHFzcXNyc3AgBqXV6LKCgoKIteXWpqXV6LKCgoKIteXWpWTExxICEhIHFMTFZWTExxICEhIHFMTEqgoGCgoGCgoGCgoAPAKCiLXl1qal1eiygoKCiLXl1qal1eiygo/GAhIHFMTFZWTExxICEhIHFMTFZWTExxICECoKCgYKCgYKCgYKCgAAIAAP/ABAADwAAPABUAAAEhIgYVERQWMyEyNjURNCYBJzcXARcDgP0ANUtLNQMANUtL/gvtWpMBM1oDwEs1/QA1S0s1AwA1S/zl7lqSATJaAAAAAAEAZf/AA5sDwAApAAABIiYjIgcOAQcGFRQWMy4BNTQ2NzAHBgIHBgcVIRMzNyM3HgEzMjY3DgEDIERoRnFTVG0aG0lIBg1lShAQSzw8WQE9bMYs1zQtVSYuUBgdPQOwEB4dYT4/QU07CyY3mW8DfX7+xY+QIxkCAID2CQ83awkHAAAAAAIAAAAABAADgAAJABcAACUzByczESM3FyMlEScjETMVITUzESMHEQOAgKCggICgoID/AEDAgP6AgMBAwMDAAgDAwMD/AID9QEBAAsCAAQAAAwDAAAADQAOAABYAHwAoAAABPgE1NCcuAScmIyERITI3PgE3NjU0JgEzMhYVFAYrARMjETMyFhUUBgLEHCAUFEYuLzX+wAGANS8uRhQURP6EZSo8PClmn5+fLD4+AdsiVC81Ly5GFBT8gBQURi4vNUZ0AUZLNTVL/oABAEs1NUsAAAAAAgDAAAADQAOAAB8AIwAAATMRFAcOAQcGIyInLgEnJjURMxEUFhceATMyNjc+ATUBIRUhAsCAGRlXOjtCQjs6VxkZgBsYHEkoKEkcGBv+AAKA/YADgP5gPDQ1ThYXFxZONTQ8AaD+YB44FxgbGxgXOB7+oIAAAAAAAQCAAAADgAOAAAsAAAEVIwEzFSE1MwEjNQOAgP7AgP5AgAFAgAOAQP0AQEADAEAAAQAAAAAEAAOAAD0AAAEVIx4BFRQGBw4BIyImJy4BNTMUFjMyNjU0JiMhNSEuAScuATU0Njc+ATMyFhceARUjNCYjIgYVFBYzMhYXBADrFRY1MCxxPj5xLDA1gHJOTnJyTv4AASwCBAEwNTUwLHE+PnEsMDWAck5OcnJOO24rAcBAHUEiNWIkISQkISRiNTRMTDQ0TEABAwEkYjU1YiQhJCQhJGI1NExMNDRMIR8AAAAKAAAAAAQAA4AAAwAHAAsADwATABcAGwAfACMAJwAAExEhEQE1IRUdASE1ARUhNSMVITURIRUhJSEVIRE1IRUBIRUhITUhFQAEAP2AAQD/AAEA/wBA/wABAP8AAoABAP8AAQD8gAEA/wACgAEAA4D8gAOA/cDAwEDAwAIAwMDAwP8AwMDAAQDAwP7AwMDAAAAFAAAAAAQAA4AAAwAHAAsADwATAAATIRUhFSEVIREhFSERIRUhESEVIQAEAPwAAoD9gAKA/YAEAPwABAD8AAOAgECA/wCAAUCA/wCAAAAAAAUAAAAABAADgAADAAcACwAPABMAABMhFSEXIRUhESEVIQMhFSERIRUhAAQA/ADAAoD9gAKA/YDABAD8AAQA/AADgIBAgP8AgAFAgP8AgAAABQAAAAAEAAOAAAMABwALAA8AEwAAEyEVIQUhFSERIRUhASEVIREhFSEABAD8AAGAAoD9gAKA/YD+gAQA/AAEAPwAA4CAQID/AIABQID/AIAAAAAABgAAAAAEAAOAAAMABwALAA8AEwAWAAATIRUhBSEVIRUhFSEVIRUhBSEVIRkBBQAEAPwAAYACgP2AAoD9gAKA/YD+gAQA/AABAAOAgECAQIBAgECAAQABgMAAAAAGAAAAAAQAA4AAAwAHAAsADwATABYAABMhFSEFIRUhFSEVIRUhFSEFIRUhARElAAQA/AABgAKA/YACgP2AAoD9gP6ABAD8AAEA/wADgIBAgECAQIBAgAKA/oDAAAEAPwA/AuYC5gAsAAAlFA8BBiMiLwEHBiMiLwEmNTQ/AScmNTQ/ATYzMh8BNzYzMh8BFhUUDwEXFhUC5hBOEBcXEKioEBcWEE4QEKioEBBOEBYXEKioEBcXEE4QEKioEMMWEE4QEKioEBBOEBYXEKioEBcXEE4QEKioEBBOEBcXEKioEBcAAAAGAAAAAAMlA24AFAAoADwATQBVAIIAAAERFAcGKwEiJyY1ETQ3NjsBMhcWFTMRFAcGKwEiJyY1ETQ3NjsBMhcWFxEUBwYrASInJjURNDc2OwEyFxYTESERFBcWFxYzITI3Njc2NQEhJyYnIwYHBRUUBwYrAREUBwYjISInJjURIyInJj0BNDc2OwE3Njc2OwEyFxYfATMyFxYVASUGBQgkCAUGBgUIJAgFBpIFBQglCAUFBQUIJQgFBZIFBQglCAUFBQUIJQgFBUn+AAQEBQQCAdsCBAQEBP6AAQAbBAa1BgQB9wYFCDcaGyb+JSYbGzcIBQUFBQixKAgXFhe3FxYWCSiwCAUGAhL+twgFBQUFCAFJCAUGBgUI/rcIBQUFBQgBSQgFBgYFCP63CAUFBQUIAUkIBQYGBf5bAh394w0LCgUFBQUKCw0CZkMFAgIFVSQIBgX94zAiIyEiLwIgBQYIJAgFBWAVDw8PDxVgBQUIAAIABwBJA7cCrwAaAC4AAAkBBiMiLwEmNTQ/AScmNTQ/ATYzMhcBFhUUBwEVFAcGIyEiJyY9ATQ3NjMhMhcWAU7+9gYHCAUdBgbh4QYGHQUIBwYBCgYGAmkFBQj92wgFBQUFCAIlCAUFAYX+9gYGHAYIBwbg4QYHBwYdBQX+9QUIBwb++yUIBQUFBQglCAUFBQUAAAABACMAAAPdA24AswAAJSInJiMiBwYjIicmNTQ3Njc2NzY3Nj0BNCcmIyEiBwYdARQXFhcWMxYXFhUUBwYjIicmIyIHBiMiJyY1NDc2NzY3Njc2PQERNDU0NTQnNCcmJyYnJicmJyYjIicmNTQ3NjMyFxYzMjc2MzIXFhUUBwYjBgcGBwYdARQXFjMhMjc2PQE0JyYnJicmNTQ3NjMyFxYzMjc2MzIXFhUUBwYHIgcGBwYVERQXFhcWFzIXFhUUBwYjA8EZMzIaGTIzGQ0IBwkKDQwREAoSAQcV/n4WBwEVCRITDg4MCwcHDhs1NRoYMTEYDQcHCQkLDBAPCRIBAgECAwQEBQgSEQ0NCgsHBw4aNTUaGDAxGA4HBwkKDA0QEAgUAQcPAZAOBwEUChcXDw4HBw4ZMzIZGTExGQ4HBwoKDQ0QEQgUFAkREQ4NCgsHBw4AAgICAgwLDxEJCQEBAwMFDETgDAUDAwUM1FENBgECAQgIEg8MDQICAgIMDA4RCAkBAgMDBQ1FIQHQAg0NCAgODgoKCwsHBwMGAQEICBIPDA0CAgICDQwPEQgIAQIBBgxQtgwHAQEHDLZQDAYBAQYHFg8MDQICAgINDA8RCAgBAQIGDU/95kQMBgICAQkIEQ8MDQAAAgAA/7cD/wO3ABMAOQAAATIXFhUUBwIHBiMiJyY1NDcBNjMBFhcWHwEWBwYjIicmJyYnJjUWFxYXFhcWMzI3Njc2NzY3Njc2NwObKB4eGr5MN0VINDQ1AW0hKf34FyYnLwECTEx7RzY2ISEQEQQTFBAQEhEJFwgPEhMVFR0dHh4pA7cbGigkM/6ZRjQ1NElJMAFLH/2xKx8fDSh6TUwaGy4vOjpEAw8OCwsKChYlGxoREQoLBAQCAAEAAAAAAABiKug1Xw889QALBAAAAAAA24pPIwAAAADbik8jAAD/twVgA8AAAAAIAAIAAAAAAAAAAQAAA8D/wAAABYAAAP//BWAAAQAAAAAAAAAAAAAAAAAAACgEAAAAAAAAAAAAAAACAAAABAAAQQQAAEEEAAAABAAAAAQAAAAEAAAABAAAwAQAAAAEAAAABAAAAAQAAEAEAAAABYAAAAQAAAAEAAAeBAAAAAQAAAAEAAAABAAAAAQAAGUEAAAABAAAwAQAAMAEAACABAAAAAQAAAAEAAAABAAAAAQAAAAEAAAABAAAAAMlAD8DJQAAA74ABwQAACMD/wAAAAAAAAAKABQAHgBKAHYApADmAS4BkgHQAhYCXALQAw4DWAN+A6gEPgTeBPoFZAWOBdAF+AY6BnYGjgbmBy4HVgd+B6gH1ggECEgJAAlKCjwKmAAAAAEAAAAoALQACgAAAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAOAK4AAQAAAAAAAQAHAAAAAQAAAAAAAgAHAGAAAQAAAAAAAwAHADYAAQAAAAAABAAHAHUAAQAAAAAABQALABUAAQAAAAAABgAHAEsAAQAAAAAACgAaAIoAAwABBAkAAQAOAAcAAwABBAkAAgAOAGcAAwABBAkAAwAOAD0AAwABBAkABAAOAHwAAwABBAkABQAWACAAAwABBAkABgAOAFIAAwABBAkACgA0AKRpY29tb29uAGkAYwBvAG0AbwBvAG5WZXJzaW9uIDEuMABWAGUAcgBzAGkAbwBuACAAMQAuADBpY29tb29uAGkAYwBvAG0AbwBvAG5pY29tb29uAGkAYwBvAG0AbwBvAG5SZWd1bGFyAFIAZQBnAHUAbABhAHJpY29tb29uAGkAYwBvAG0AbwBvAG5Gb250IGdlbmVyYXRlZCBieSBJY29Nb29uLgBGAG8AbgB0ACAAZwBlAG4AZQByAGEAdABlAGQAIABiAHkAIABJAGMAbwBNAG8AbwBuAC4AAAADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(137);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        (e = n(22)(!1)).push([t.i, ".w-e-toolbar {\n  display: flex;\n  padding: 0 6px;\n  flex-wrap: wrap;\n  /* 单个菜单 */\n}\n.w-e-toolbar .w-e-menu {\n  position: relative;\n  display: flex;\n  width: 40px;\n  height: 40px;\n  align-items: center;\n  justify-content: center;\n  text-align: center;\n  cursor: pointer;\n}\n.w-e-toolbar .w-e-menu i {\n  color: #999;\n}\n.w-e-toolbar .w-e-menu:hover {\n  background-color: #F6F6F6;\n}\n.w-e-toolbar .w-e-menu:hover i {\n  color: #333;\n}\n.w-e-toolbar .w-e-active i {\n  color: #1e88e5;\n}\n.w-e-toolbar .w-e-active:hover i {\n  color: #1e88e5;\n}\n", ""]),
        t.exports = e
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(139);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        (e = n(22)(!1)).push([t.i, '.w-e-text-container {\n  position: relative;\n}\n.w-e-text-container .w-e-progress {\n  position: absolute;\n  background-color: #1e88e5;\n  top: 0;\n  left: 0;\n  height: 1px;\n}\n.w-e-text-container .placeholder {\n  color: #D4D4D4;\n  position: absolute;\n  font-size: 11pt;\n  line-height: 22px;\n  left: 10px;\n  top: 10px;\n  -webkit-user-select: none;\n     -moz-user-select: none;\n      -ms-user-select: none;\n          user-select: none;\n  z-index: -1;\n}\n.w-e-text {\n  padding: 0 10px;\n  overflow-y: auto;\n}\n.w-e-text p,\n.w-e-text h1,\n.w-e-text h2,\n.w-e-text h3,\n.w-e-text h4,\n.w-e-text h5,\n.w-e-text table,\n.w-e-text pre {\n  margin: 10px 0;\n  line-height: 1.5;\n}\n.w-e-text ul,\n.w-e-text ol {\n  margin: 10px 0 10px 20px;\n}\n.w-e-text blockquote {\n  display: block;\n  border-left: 8px solid #d0e5f2;\n  padding: 5px 10px;\n  margin: 10px 0;\n  line-height: 1.4;\n  font-size: 100%;\n  background-color: #f1f1f1;\n}\n.w-e-text code {\n  display: inline-block;\n  background-color: #f1f1f1;\n  border-radius: 3px;\n  padding: 3px 5px;\n  margin: 0 3px;\n}\n.w-e-text pre code {\n  display: block;\n}\n.w-e-text table {\n  border-top: 1px solid #ccc;\n  border-left: 1px solid #ccc;\n}\n.w-e-text table td,\n.w-e-text table th {\n  border-bottom: 1px solid #ccc;\n  border-right: 1px solid #ccc;\n  padding: 3px 5px;\n  min-height: 30px;\n}\n.w-e-text table th {\n  border-bottom: 2px solid #ccc;\n  text-align: center;\n  background-color: #f1f1f1;\n}\n.w-e-text:focus {\n  outline: none;\n}\n.w-e-text img {\n  cursor: pointer;\n}\n.w-e-text img:hover {\n  box-shadow: 0 0 5px #333;\n}\n.w-e-tooltip {\n  display: flex;\n  color: #f1f1f1;\n  background-color: rgba(0, 0, 0, 0.75);\n  box-shadow: 0 2px 8px 0 rgba(0, 0, 0, 0.15);\n  border-radius: 4px;\n  padding: 4px 5px 6px;\n  position: absolute;\n}\n.w-e-tooltip-up::after {\n  content: "";\n  position: absolute;\n  top: 100%;\n  left: 50%;\n  margin-left: -5px;\n  border: 5px solid rgba(0, 0, 0, 0);\n  border-top-color: rgba(0, 0, 0, 0.73);\n}\n.w-e-tooltip-down::after {\n  content: "";\n  position: absolute;\n  bottom: 100%;\n  left: 50%;\n  margin-left: -5px;\n  border: 5px solid rgba(0, 0, 0, 0);\n  border-bottom-color: rgba(0, 0, 0, 0.73);\n}\n.w-e-tooltip-item-wrapper {\n  cursor: pointer;\n  font-size: 14px;\n  margin: 0 5px;\n}\n.w-e-tooltip-item-wrapper:hover {\n  color: #ccc;\n  text-decoration: underline;\n}\n', ""]),
        t.exports = e
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(141);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        (e = n(22)(!1)).push([t.i, '.w-e-menu .w-e-panel-container {\n  position: absolute;\n  top: 0;\n  left: 50%;\n  border: 1px solid #ccc;\n  border-top: 0;\n  box-shadow: 1px 1px 2px #ccc;\n  color: #333;\n  background-color: #fff;\n  text-align: left;\n  /* 为 emotion panel 定制的样式 */\n  /* 上传图片的 panel 定制样式 */\n}\n.w-e-menu .w-e-panel-container .w-e-panel-close {\n  position: absolute;\n  right: 0;\n  top: 0;\n  padding: 5px;\n  margin: 2px 5px 0 0;\n  cursor: pointer;\n  color: #999;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-close:hover {\n  color: #333;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-title {\n  list-style: none;\n  display: flex;\n  font-size: 14px;\n  margin: 2px 10px 0 10px;\n  border-bottom: 1px solid #f1f1f1;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-title .w-e-item {\n  padding: 3px 5px;\n  color: #999;\n  cursor: pointer;\n  margin: 0 3px;\n  position: relative;\n  top: 1px;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-title .w-e-active {\n  color: #333;\n  border-bottom: 1px solid #333;\n  cursor: default;\n  font-weight: 700;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content {\n  padding: 10px 15px 10px 15px;\n  font-size: 16px;\n  /* 输入框的样式 */\n  /* 按钮的样式 */\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content input:focus,\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content textarea:focus,\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content button:focus {\n  outline: none;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content textarea {\n  width: 100%;\n  border: 1px solid #ccc;\n  padding: 5px;\n  margin-top: 10px;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content textarea:focus {\n  border-color: #1e88e5;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content input[type=text] {\n  border: none;\n  border-bottom: 1px solid #ccc;\n  font-size: 14px;\n  height: 20px;\n  color: #333;\n  text-align: left;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content input[type=text].small {\n  width: 30px;\n  text-align: center;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content input[type=text].block {\n  display: block;\n  width: 100%;\n  margin: 10px 0;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content input[type=text]:focus {\n  border-bottom: 2px solid #1e88e5;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content .w-e-button-container button {\n  font-size: 14px;\n  color: #1e88e5;\n  border: none;\n  padding: 5px 10px;\n  background-color: #fff;\n  cursor: pointer;\n  border-radius: 3px;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content .w-e-button-container button.left {\n  float: left;\n  margin-right: 10px;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content .w-e-button-container button.right {\n  float: right;\n  margin-left: 10px;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content .w-e-button-container button.gray {\n  color: #999;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content .w-e-button-container button.red {\n  color: #c24f4a;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content .w-e-button-container button:hover {\n  background-color: #f1f1f1;\n}\n.w-e-menu .w-e-panel-container .w-e-panel-tab-content .w-e-button-container:after {\n  content: "";\n  display: table;\n  clear: both;\n}\n.w-e-menu .w-e-panel-container .w-e-emoticon-container .w-e-item {\n  cursor: pointer;\n  font-size: 18px;\n  padding: 0 3px;\n  display: inline-block;\n}\n.w-e-menu .w-e-panel-container .w-e-up-img-container {\n  text-align: center;\n}\n.w-e-menu .w-e-panel-container .w-e-up-img-container .w-e-up-btn {\n  display: inline-block;\n  color: #999;\n  cursor: pointer;\n  font-size: 60px;\n  line-height: 1;\n}\n.w-e-menu .w-e-panel-container .w-e-up-img-container .w-e-up-btn:hover {\n  color: #333;\n}\n', ""]),
        t.exports = e
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(143);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        (e = n(22)(!1)).push([t.i, ".w-e-toolbar .w-e-droplist {\n  position: absolute;\n  left: 0;\n  top: 0;\n  background-color: #fff;\n  border: 1px solid #f1f1f1;\n  border-right-color: #ccc;\n  border-bottom-color: #ccc;\n}\n.w-e-toolbar .w-e-droplist .w-e-dp-title {\n  text-align: center;\n  color: #999;\n  line-height: 2;\n  border-bottom: 1px solid #f1f1f1;\n  font-size: 13px;\n}\n.w-e-toolbar .w-e-droplist ul.w-e-list {\n  list-style: none;\n  line-height: 1;\n}\n.w-e-toolbar .w-e-droplist ul.w-e-list li.w-e-item {\n  color: #333;\n  padding: 5px 0;\n}\n.w-e-toolbar .w-e-droplist ul.w-e-list li.w-e-item:hover {\n  background-color: #f1f1f1;\n}\n.w-e-toolbar .w-e-droplist ul.w-e-block {\n  list-style: none;\n  text-align: left;\n  padding: 5px;\n}\n.w-e-toolbar .w-e-droplist ul.w-e-block li.w-e-item {\n  display: inline-block;\n  padding: 3px 5px;\n}\n.w-e-toolbar .w-e-droplist ul.w-e-block li.w-e-item:hover {\n  background-color: #f1f1f1;\n}\n", ""]),
        t.exports = e
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)(n(145));
        Element.prototype.matches || (Element.prototype.matches = function(t) {
            for (var e = this.ownerDocument.querySelectorAll(t), n = e.length; n >= 0 && e.item(n) !== this; n--)
                ;
            return n > -1
        }
        ),
        window.Promise = o.default
    }
    , function(t, e, n) {
        t.exports = n(146)
    }
    , function(t, e, n) {
        var o = n(147);
        t.exports = o
    }
    , function(t, e, n) {
        n(52),
        n(53),
        n(47),
        n(158),
        n(165),
        n(166);
        var o = n(12);
        t.exports = o.Promise
    }
    , function(t, e, n) {
        var o = n(54)
          , r = n(43)
          , i = function(t) {
            return function(e, n) {
                var i, a, u = String(r(e)), l = o(n), c = u.length;
                return l < 0 || l >= c ? t ? "" : void 0 : (i = u.charCodeAt(l)) < 55296 || i > 56319 || l + 1 === c || (a = u.charCodeAt(l + 1)) < 56320 || a > 57343 ? t ? u.charAt(l) : i : t ? u.slice(l, l + 2) : a - 56320 + (i - 55296 << 10) + 65536
            }
        };
        t.exports = {
            codeAt: i(!1),
            charAt: i(!0)
        }
    }
    , function(t, e, n) {
        var o = n(7)
          , r = n(88)
          , i = o.WeakMap;
        t.exports = "function" == typeof i && /native code/.test(r(i))
    }
    , function(t, e, n) {
        var o = n(7)
          , r = n(18);
        t.exports = function(t, e) {
            try {
                r(o, t, e)
            } catch (n) {
                o[t] = e
            }
            return e
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(90).IteratorPrototype
          , r = n(57)
          , i = n(42)
          , a = n(29)
          , u = n(37)
          , l = function() {
            return this
        };
        t.exports = function(t, e, n) {
            var c = e + " Iterator";
            return t.prototype = r(o, {
                next: i(1, n)
            }),
            a(t, c, !1, !0),
            u[c] = l,
            t
        }
    }
    , function(t, e, n) {
        var o = n(11);
        t.exports = !o((function() {
            function t() {}
            return t.prototype.constructor = null,
            Object.getPrototypeOf(new t) !== t.prototype
        }
        ))
    }
    , function(t, e, n) {
        var o = n(14)
          , r = n(16)
          , i = n(20)
          , a = n(69);
        t.exports = o ? Object.defineProperties : function(t, e) {
            i(t);
            for (var n, o = a(e), u = o.length, l = 0; u > l; )
                r.f(t, n = o[l++], e[n]);
            return t
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(73)
          , r = n(58);
        t.exports = o ? {}.toString : function() {
            return "[object " + r(this) + "]"
        }
    }
    , function(t, e, n) {
        var o = n(13);
        t.exports = function(t) {
            if (!o(t) && null !== t)
                throw TypeError("Can't set " + String(t) + " as a prototype");
            return t
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(27)
          , r = n(74)
          , i = n(37)
          , a = n(32)
          , u = n(67)
          , l = a.set
          , c = a.getterFor("Array Iterator");
        t.exports = u(Array, "Array", (function(t, e) {
            l(this, {
                type: "Array Iterator",
                target: o(t),
                index: 0,
                kind: e
            })
        }
        ), (function() {
            var t = c(this)
              , e = t.target
              , n = t.kind
              , o = t.index++;
            return !e || o >= e.length ? (t.target = void 0,
            {
                value: void 0,
                done: !0
            }) : "keys" == n ? {
                value: o,
                done: !1
            } : "values" == n ? {
                value: e[o],
                done: !1
            } : {
                value: [o, e[o]],
                done: !1
            }
        }
        ), "values"),
        i.Arguments = i.Array,
        r("keys"),
        r("values"),
        r("entries")
    }
    , function(t, e) {
        t.exports = {
            CSSRuleList: 0,
            CSSStyleDeclaration: 0,
            CSSValueList: 0,
            ClientRectList: 0,
            DOMRectList: 0,
            DOMStringList: 0,
            DOMTokenList: 1,
            DataTransferItemList: 0,
            FileList: 0,
            HTMLAllCollection: 0,
            HTMLCollection: 0,
            HTMLFormElement: 0,
            HTMLSelectElement: 0,
            MediaList: 0,
            MimeTypeArray: 0,
            NamedNodeMap: 0,
            NodeList: 1,
            PaintRequestList: 0,
            Plugin: 0,
            PluginArray: 0,
            SVGLengthList: 0,
            SVGNumberList: 0,
            SVGPathSegList: 0,
            SVGPointList: 0,
            SVGStringList: 0,
            SVGTransformList: 0,
            SourceBufferList: 0,
            StyleSheetList: 0,
            TextTrackCueList: 0,
            TextTrackList: 0,
            TouchList: 0
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r, i, a, u = n(4), l = n(33), c = n(7), s = n(36), f = n(96), d = n(46), p = n(97), A = n(29), v = n(98), h = n(13), g = n(31), m = n(75), y = n(28), w = n(88), x = n(59), E = n(162), b = n(99), _ = n(100).set, S = n(163), M = n(102), C = n(164), B = n(77), k = n(103), I = n(32), T = n(87), N = n(8), R = n(78), D = N("species"), Q = "Promise", P = I.get, H = I.set, F = I.getterFor(Q), O = f, j = c.TypeError, L = c.document, U = c.process, Y = s("fetch"), z = B.f, G = z, $ = "process" == y(U), J = !!(L && L.createEvent && c.dispatchEvent), K = T(Q, (function() {
            if (!(w(O) !== String(O))) {
                if (66 === R)
                    return !0;
                if (!$ && "function" != typeof PromiseRejectionEvent)
                    return !0
            }
            if (l && !O.prototype.finally)
                return !0;
            if (R >= 51 && /native code/.test(O))
                return !1;
            var t = O.resolve(1)
              , e = function(t) {
                t((function() {}
                ), (function() {}
                ))
            };
            return (t.constructor = {})[D] = e,
            !(t.then((function() {}
            ))instanceof e)
        }
        )), V = K || !E((function(t) {
            O.all(t).catch((function() {}
            ))
        }
        )), W = function(t) {
            var e;
            return !(!h(t) || "function" != typeof (e = t.then)) && e
        }, X = function(t, e, n) {
            if (!e.notified) {
                e.notified = !0;
                var o = e.reactions;
                S((function() {
                    for (var r = e.value, i = 1 == e.state, a = 0; o.length > a; ) {
                        var u, l, c, s = o[a++], f = i ? s.ok : s.fail, d = s.resolve, p = s.reject, A = s.domain;
                        try {
                            f ? (i || (2 === e.rejection && et(t, e),
                            e.rejection = 1),
                            !0 === f ? u = r : (A && A.enter(),
                            u = f(r),
                            A && (A.exit(),
                            c = !0)),
                            u === s.promise ? p(j("Promise-chain cycle")) : (l = W(u)) ? l.call(u, d, p) : d(u)) : p(r)
                        } catch (t) {
                            A && !c && A.exit(),
                            p(t)
                        }
                    }
                    e.reactions = [],
                    e.notified = !1,
                    n && !e.rejection && Z(t, e)
                }
                ))
            }
        }, q = function(t, e, n) {
            var o, r;
            J ? ((o = L.createEvent("Event")).promise = e,
            o.reason = n,
            o.initEvent(t, !1, !0),
            c.dispatchEvent(o)) : o = {
                promise: e,
                reason: n
            },
            (r = c["on" + t]) ? r(o) : "unhandledrejection" === t && C("Unhandled promise rejection", n)
        }, Z = function(t, e) {
            _.call(c, (function() {
                var n, o = e.value;
                if (tt(e) && (n = k((function() {
                    $ ? U.emit("unhandledRejection", o, t) : q("unhandledrejection", t, o)
                }
                )),
                e.rejection = $ || tt(e) ? 2 : 1,
                n.error))
                    throw n.value
            }
            ))
        }, tt = function(t) {
            return 1 !== t.rejection && !t.parent
        }, et = function(t, e) {
            _.call(c, (function() {
                $ ? U.emit("rejectionHandled", t) : q("rejectionhandled", t, e.value)
            }
            ))
        }, nt = function(t, e, n, o) {
            return function(r) {
                t(e, n, r, o)
            }
        }, ot = function(t, e, n, o) {
            e.done || (e.done = !0,
            o && (e = o),
            e.value = n,
            e.state = 2,
            X(t, e, !0))
        }, rt = function(t, e, n, o) {
            if (!e.done) {
                e.done = !0,
                o && (e = o);
                try {
                    if (t === n)
                        throw j("Promise can't be resolved itself");
                    var r = W(n);
                    r ? S((function() {
                        var o = {
                            done: !1
                        };
                        try {
                            r.call(n, nt(rt, t, o, e), nt(ot, t, o, e))
                        } catch (n) {
                            ot(t, o, n, e)
                        }
                    }
                    )) : (e.value = n,
                    e.state = 1,
                    X(t, e, !1))
                } catch (n) {
                    ot(t, {
                        done: !1
                    }, n, e)
                }
            }
        };
        K && (O = function(t) {
            m(this, O, Q),
            g(t),
            o.call(this);
            var e = P(this);
            try {
                t(nt(rt, this, e), nt(ot, this, e))
            } catch (t) {
                ot(this, e, t)
            }
        }
        ,
        (o = function(t) {
            H(this, {
                type: Q,
                done: !1,
                notified: !1,
                parent: !1,
                reactions: [],
                rejection: !1,
                state: 0,
                value: void 0
            })
        }
        ).prototype = p(O.prototype, {
            then: function(t, e) {
                var n = F(this)
                  , o = z(b(this, O));
                return o.ok = "function" != typeof t || t,
                o.fail = "function" == typeof e && e,
                o.domain = $ ? U.domain : void 0,
                n.parent = !0,
                n.reactions.push(o),
                0 != n.state && X(this, n, !1),
                o.promise
            },
            catch: function(t) {
                return this.then(void 0, t)
            }
        }),
        r = function() {
            var t = new o
              , e = P(t);
            this.promise = t,
            this.resolve = nt(rt, t, e),
            this.reject = nt(ot, t, e)
        }
        ,
        B.f = z = function(t) {
            return t === O || t === i ? new r(t) : G(t)
        }
        ,
        l || "function" != typeof f || (a = f.prototype.then,
        d(f.prototype, "then", (function(t, e) {
            var n = this;
            return new O((function(t, e) {
                a.call(n, t, e)
            }
            )).then(t, e)
        }
        ), {
            unsafe: !0
        }),
        "function" == typeof Y && u({
            global: !0,
            enumerable: !0,
            forced: !0
        }, {
            fetch: function(t) {
                return M(O, Y.apply(c, arguments))
            }
        }))),
        u({
            global: !0,
            wrap: !0,
            forced: K
        }, {
            Promise: O
        }),
        A(O, Q, !1, !0),
        v(Q),
        i = s(Q),
        u({
            target: Q,
            stat: !0,
            forced: K
        }, {
            reject: function(t) {
                var e = z(this);
                return e.reject.call(void 0, t),
                e.promise
            }
        }),
        u({
            target: Q,
            stat: !0,
            forced: l || K
        }, {
            resolve: function(t) {
                return M(l && this === i ? O : this, t)
            }
        }),
        u({
            target: Q,
            stat: !0,
            forced: V
        }, {
            all: function(t) {
                var e = this
                  , n = z(e)
                  , o = n.resolve
                  , r = n.reject
                  , i = k((function() {
                    var n = g(e.resolve)
                      , i = []
                      , a = 0
                      , u = 1;
                    x(t, (function(t) {
                        var l = a++
                          , c = !1;
                        i.push(void 0),
                        u++,
                        n.call(e, t).then((function(t) {
                            c || (c = !0,
                            i[l] = t,
                            --u || o(i))
                        }
                        ), r)
                    }
                    )),
                    --u || o(i)
                }
                ));
                return i.error && r(i.value),
                n.promise
            },
            race: function(t) {
                var e = this
                  , n = z(e)
                  , o = n.reject
                  , r = k((function() {
                    var r = g(e.resolve);
                    x(t, (function(t) {
                        r.call(e, t).then(n.resolve, o)
                    }
                    ))
                }
                ));
                return r.error && o(r.value),
                n.promise
            }
        })
    }
    , function(t, e, n) {
        var o = n(8)
          , r = n(37)
          , i = o("iterator")
          , a = Array.prototype;
        t.exports = function(t) {
            return void 0 !== t && (r.Array === t || a[i] === t)
        }
    }
    , function(t, e, n) {
        var o = n(58)
          , r = n(37)
          , i = n(8)("iterator");
        t.exports = function(t) {
            if (null != t)
                return t[i] || t["@@iterator"] || r[o(t)]
        }
    }
    , function(t, e, n) {
        var o = n(20);
        t.exports = function(t, e, n, r) {
            try {
                return r ? e(o(n)[0], n[1]) : e(n)
            } catch (e) {
                var i = t.return;
                throw void 0 !== i && o(i.call(t)),
                e
            }
        }
    }
    , function(t, e, n) {
        var o = n(8)("iterator")
          , r = !1;
        try {
            var i = 0
              , a = {
                next: function() {
                    return {
                        done: !!i++
                    }
                },
                return: function() {
                    r = !0
                }
            };
            a[o] = function() {
                return this
            }
            ,
            Array.from(a, (function() {
                throw 2
            }
            ))
        } catch (t) {}
        t.exports = function(t, e) {
            if (!e && !r)
                return !1;
            var n = !1;
            try {
                var i = {};
                i[o] = function() {
                    return {
                        next: function() {
                            return {
                                done: n = !0
                            }
                        }
                    }
                }
                ,
                t(i)
            } catch (t) {}
            return n
        }
    }
    , function(t, e, n) {
        var o, r, i, a, u, l, c, s, f = n(7), d = n(62).f, p = n(28), A = n(100).set, v = n(101), h = f.MutationObserver || f.WebKitMutationObserver, g = f.process, m = f.Promise, y = "process" == p(g), w = d(f, "queueMicrotask"), x = w && w.value;
        x || (o = function() {
            var t, e;
            for (y && (t = g.domain) && t.exit(); r; ) {
                e = r.fn,
                r = r.next;
                try {
                    e()
                } catch (t) {
                    throw r ? a() : i = void 0,
                    t
                }
            }
            i = void 0,
            t && t.enter()
        }
        ,
        y ? a = function() {
            g.nextTick(o)
        }
        : h && !v ? (u = !0,
        l = document.createTextNode(""),
        new h(o).observe(l, {
            characterData: !0
        }),
        a = function() {
            l.data = u = !u
        }
        ) : m && m.resolve ? (c = m.resolve(void 0),
        s = c.then,
        a = function() {
            s.call(c, o)
        }
        ) : a = function() {
            A.call(f, o)
        }
        ),
        t.exports = x || function(t) {
            var e = {
                fn: t,
                next: void 0
            };
            i && (i.next = e),
            r || (r = e,
            a()),
            i = e
        }
    }
    , function(t, e, n) {
        var o = n(7);
        t.exports = function(t, e) {
            var n = o.console;
            n && n.error && (1 === arguments.length ? n.error(t) : n.error(t, e))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(31)
          , i = n(77)
          , a = n(103)
          , u = n(59);
        o({
            target: "Promise",
            stat: !0
        }, {
            allSettled: function(t) {
                var e = this
                  , n = i.f(e)
                  , o = n.resolve
                  , l = n.reject
                  , c = a((function() {
                    var n = r(e.resolve)
                      , i = []
                      , a = 0
                      , l = 1;
                    u(t, (function(t) {
                        var r = a++
                          , u = !1;
                        i.push(void 0),
                        l++,
                        n.call(e, t).then((function(t) {
                            u || (u = !0,
                            i[r] = {
                                status: "fulfilled",
                                value: t
                            },
                            --l || o(i))
                        }
                        ), (function(t) {
                            u || (u = !0,
                            i[r] = {
                                status: "rejected",
                                reason: t
                            },
                            --l || o(i))
                        }
                        ))
                    }
                    )),
                    --l || o(i)
                }
                ));
                return c.error && l(c.value),
                n.promise
            }
        })
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(33)
          , i = n(96)
          , a = n(11)
          , u = n(36)
          , l = n(99)
          , c = n(102)
          , s = n(46);
        o({
            target: "Promise",
            proto: !0,
            real: !0,
            forced: !!i && a((function() {
                i.prototype.finally.call({
                    then: function() {}
                }, (function() {}
                ))
            }
            ))
        }, {
            finally: function(t) {
                var e = l(this, u("Promise"))
                  , n = "function" == typeof t;
                return this.then(n ? function(n) {
                    return c(e, t()).then((function() {
                        return n
                    }
                    ))
                }
                : t, n ? function(n) {
                    return c(e, t()).then((function() {
                        throw n
                    }
                    ))
                }
                : t)
            }
        }),
        r || "function" != typeof i || i.prototype.finally || s(i.prototype, "finally", u("Promise").prototype.finally)
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = o(n(1))
          , a = o(n(3))
          , u = a.default ? function(t, e, n, o) {
            void 0 === o && (o = n),
            (0,
            i.default)(t, o, {
                enumerable: !0,
                get: function() {
                    return e[n]
                }
            })
        }
        : function(t, e, n, o) {
            void 0 === o && (o = n),
            t[o] = e[n]
        }
          , l = a.default ? function(t, e) {
            (0,
            i.default)(t, "default", {
                enumerable: !0,
                value: e
            })
        }
        : function(t, e) {
            t.default = e
        }
          , c = function(t) {
            if (t && t.__esModule)
                return t;
            var e = {};
            if (null != t)
                for (var n in t)
                    Object.hasOwnProperty.call(t, n) && u(e, t, n);
            return l(e, t),
            e
        }
          , s = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var f = s(n(2))
          , d = n(9)
          , p = s(n(248))
          , A = s(n(261))
          , v = s(n(263))
          , h = s(n(264))
          , g = s(n(282))
          , m = s(n(358))
          , y = s(n(359))
          , w = c(n(360))
          , x = s(n(361))
          , E = c(n(362))
          , b = s(n(365))
          , _ = s(n(366))
          , S = s(n(24))
          , M = s(n(122))
          , C = s(n(19))
          , B = s(n(26))
          , k = s(n(30))
          , I = s(n(41))
          , T = 1
          , N = function() {
            function t(t, e) {
                if (this.beforeDestroyHooks = [],
                this.id = "wangEditor-" + T++,
                this.toolbarSelector = t,
                this.textSelector = e,
                null == t)
                    throw new Error("错误：初始化编辑器时候未传入任何参数，请查阅文档");
                this.config = d.deepClone(p.default),
                this.$toolbarElem = f.default("<div></div>"),
                this.$textContainerElem = f.default("<div></div>"),
                this.$textElem = f.default("<div></div>"),
                this.toolbarElemId = "",
                this.textElemId = "",
                this.isFocus = !1,
                this.selection = new A.default(this),
                this.cmd = new v.default(this),
                this.txt = new h.default(this),
                this.menus = new g.default(this),
                this.undo = new b.default(this),
                this.zIndex = new _.default
            }
            return t.prototype.initSelection = function(t) {
                y.default(this, t)
            }
            ,
            t.prototype.create = function() {
                this.zIndex.init(this),
                x.default(this),
                m.default(this),
                this.txt.init(),
                this.menus.init(),
                E.default(this),
                this.initSelection(!0),
                w.default(this)
            }
            ,
            t.prototype.change = function() {
                w.changeHandler(this)
            }
            ,
            t.prototype.beforeDestroy = function(t) {
                return this.beforeDestroyHooks.push(t),
                this
            }
            ,
            t.prototype.destroy = function() {
                var t, e = this;
                (0,
                r.default)(t = this.beforeDestroyHooks).call(t, (function(t) {
                    return t.call(e)
                }
                )),
                this.$toolbarElem.remove(),
                this.$textContainerElem.remove()
            }
            ,
            t.prototype.fullScreen = function() {
                E.setFullScreen(this)
            }
            ,
            t.prototype.unFullScreen = function() {
                E.setUnFullScreen(this)
            }
            ,
            t.menuConstructors = {},
            t.$ = f.default,
            t
        }();
        N.menuConstructors = {
            BtnMenu: S.default,
            DropList: M.default,
            DropListMenu: C.default,
            Panel: B.default,
            PanelMenu: k.default,
            Tooltip: I.default
        },
        e.default = N
    }
    , function(t, e, n) {
        n(47);
        var o = n(169)
          , r = n(58)
          , i = Array.prototype
          , a = {
            DOMTokenList: !0,
            NodeList: !0
        };
        t.exports = function(t) {
            var e = t.forEach;
            return t === i || t instanceof Array && e === i.forEach || a.hasOwnProperty(r(t)) ? o : e
        }
    }
    , function(t, e, n) {
        var o = n(170);
        t.exports = o
    }
    , function(t, e, n) {
        n(171);
        var o = n(17);
        t.exports = o("Array").forEach
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(172);
        o({
            target: "Array",
            proto: !0,
            forced: [].forEach != r
        }, {
            forEach: r
        })
    }
    , function(t, e, n) {
        "use strict";
        var o = n(38).forEach
          , r = n(104)
          , i = n(25)
          , a = r("forEach")
          , u = i("forEach");
        t.exports = a && u ? [].forEach : function(t) {
            return o(this, t, arguments.length > 1 ? arguments[1] : void 0)
        }
    }
    , function(t, e, n) {
        var o = n(174);
        t.exports = o
    }
    , function(t, e, n) {
        n(175);
        var o = n(12).Object;
        t.exports = function(t, e) {
            return o.create(t, e)
        }
    }
    , function(t, e, n) {
        n(4)({
            target: "Object",
            stat: !0,
            sham: !n(14)
        }, {
            create: n(57)
        })
    }
    , function(t, e, n) {
        var o = n(177);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(178)
          , r = n(180)
          , i = Array.prototype
          , a = String.prototype;
        t.exports = function(t) {
            var e = t.includes;
            return t === i || t instanceof Array && e === i.includes ? o : "string" == typeof t || t === a || t instanceof String && e === a.includes ? r : e
        }
    }
    , function(t, e, n) {
        n(179);
        var o = n(17);
        t.exports = o("Array").includes
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(70).includes
          , i = n(74);
        o({
            target: "Array",
            proto: !0,
            forced: !n(25)("indexOf", {
                ACCESSORS: !0,
                1: 0
            })
        }, {
            includes: function(t) {
                return r(this, t, arguments.length > 1 ? arguments[1] : void 0)
            }
        }),
        i("includes")
    }
    , function(t, e, n) {
        n(181);
        var o = n(17);
        t.exports = o("String").includes
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(182)
          , i = n(43);
        o({
            target: "String",
            proto: !0,
            forced: !n(184)("includes")
        }, {
            includes: function(t) {
                return !!~String(i(this)).indexOf(r(t), arguments.length > 1 ? arguments[1] : void 0)
            }
        })
    }
    , function(t, e, n) {
        var o = n(183);
        t.exports = function(t) {
            if (o(t))
                throw TypeError("The method doesn't accept regular expressions");
            return t
        }
    }
    , function(t, e, n) {
        var o = n(13)
          , r = n(28)
          , i = n(8)("match");
        t.exports = function(t) {
            var e;
            return o(t) && (void 0 !== (e = t[i]) ? !!e : "RegExp" == r(t))
        }
    }
    , function(t, e, n) {
        var o = n(8)("match");
        t.exports = function(t) {
            var e = /./;
            try {
                "/./"[t](e)
            } catch (n) {
                try {
                    return e[o] = !1,
                    "/./"[t](e)
                } catch (t) {}
            }
            return !1
        }
    }
    , function(t, e, n) {
        var o = n(186);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(187)
          , r = Array.prototype;
        t.exports = function(t) {
            var e = t.filter;
            return t === r || t instanceof Array && e === r.filter ? o : e
        }
    }
    , function(t, e, n) {
        n(188);
        var o = n(17);
        t.exports = o("Array").filter
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(38).filter
          , i = n(49)
          , a = n(25)
          , u = i("filter")
          , l = a("filter");
        o({
            target: "Array",
            proto: !0,
            forced: !u || !l
        }, {
            filter: function(t) {
                return r(this, t, arguments.length > 1 ? arguments[1] : void 0)
            }
        })
    }
    , function(t, e, n) {
        var o = n(190);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(191)
          , r = Array.prototype;
        t.exports = function(t) {
            var e = t.splice;
            return t === r || t instanceof Array && e === r.splice ? o : e
        }
    }
    , function(t, e, n) {
        n(192);
        var o = n(17);
        t.exports = o("Array").splice
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(71)
          , i = n(54)
          , a = n(35)
          , u = n(34)
          , l = n(79)
          , c = n(80)
          , s = n(49)
          , f = n(25)
          , d = s("splice")
          , p = f("splice", {
            ACCESSORS: !0,
            0: 0,
            1: 2
        })
          , A = Math.max
          , v = Math.min;
        o({
            target: "Array",
            proto: !0,
            forced: !d || !p
        }, {
            splice: function(t, e) {
                var n, o, s, f, d, p, h = u(this), g = a(h.length), m = r(t, g), y = arguments.length;
                if (0 === y ? n = o = 0 : 1 === y ? (n = 0,
                o = g - m) : (n = y - 2,
                o = v(A(i(e), 0), g - m)),
                g + n - o > 9007199254740991)
                    throw TypeError("Maximum allowed length exceeded");
                for (s = l(h, o),
                f = 0; f < o; f++)
                    (d = m + f)in h && c(s, f, h[d]);
                if (s.length = o,
                n < o) {
                    for (f = m; f < g - o; f++)
                        p = f + n,
                        (d = f + o)in h ? h[p] = h[d] : delete h[p];
                    for (f = g; f > g - o + n; f--)
                        delete h[f - 1]
                } else if (n > o)
                    for (f = g - o; f > m; f--)
                        p = f + n - 1,
                        (d = f + o - 1)in h ? h[p] = h[d] : delete h[p];
                for (f = 0; f < n; f++)
                    h[f + m] = arguments[f + 2];
                return h.length = g - o + n,
                s
            }
        })
    }
    , function(t, e, n) {
        var o = n(194);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(195)
          , r = Array.prototype;
        t.exports = function(t) {
            var e = t.indexOf;
            return t === r || t instanceof Array && e === r.indexOf ? o : e
        }
    }
    , function(t, e, n) {
        n(196);
        var o = n(17);
        t.exports = o("Array").indexOf
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(70).indexOf
          , i = n(104)
          , a = n(25)
          , u = [].indexOf
          , l = !!u && 1 / [1].indexOf(1, -0) < 0
          , c = i("indexOf")
          , s = a("indexOf", {
            ACCESSORS: !0,
            1: 0
        });
        o({
            target: "Array",
            proto: !0,
            forced: l || !c || !s
        }, {
            indexOf: function(t) {
                return l ? u.apply(this, arguments) || 0 : r(this, t, arguments.length > 1 ? arguments[1] : void 0)
            }
        })
    }
    , function(t, e, n) {
        var o = n(198);
        t.exports = o
    }
    , function(t, e, n) {
        n(199),
        n(52),
        n(53),
        n(47);
        var o = n(12);
        t.exports = o.Map
    }
    , function(t, e, n) {
        "use strict";
        var o = n(109)
          , r = n(111);
        t.exports = o("Map", (function(t) {
            return function() {
                return t(this, arguments.length ? arguments[0] : void 0)
            }
        }
        ), r)
    }
    , function(t, e, n) {
        var o = n(11);
        t.exports = !o((function() {
            return Object.isExtensible(Object.preventExtensions({}))
        }
        ))
    }
    , function(t, e, n) {
        var o = n(202);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(203)
          , r = String.prototype;
        t.exports = function(t) {
            var e = t.trim;
            return "string" == typeof t || t === r || t instanceof String && e === r.trim ? o : e
        }
    }
    , function(t, e, n) {
        n(204);
        var o = n(17);
        t.exports = o("String").trim
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(112).trim;
        o({
            target: "String",
            proto: !0,
            forced: n(205)("trim")
        }, {
            trim: function() {
                return r(this)
            }
        })
    }
    , function(t, e, n) {
        var o = n(11)
          , r = n(81);
        t.exports = function(t) {
            return o((function() {
                return !!r[t]() || "​᠎" != "​᠎"[t]() || r[t].name !== t
            }
            ))
        }
    }
    , function(t, e, n) {
        var o = n(207);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(208)
          , r = Array.prototype;
        t.exports = function(t) {
            var e = t.map;
            return t === r || t instanceof Array && e === r.map ? o : e
        }
    }
    , function(t, e, n) {
        n(209);
        var o = n(17);
        t.exports = o("Array").map
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(38).map
          , i = n(49)
          , a = n(25)
          , u = i("map")
          , l = a("map");
        o({
            target: "Array",
            proto: !0,
            forced: !u || !l
        }, {
            map: function(t) {
                return r(this, t, arguments.length > 1 ? arguments[1] : void 0)
            }
        })
    }
    , function(t, e, n) {
        var o = n(211);
        t.exports = o
    }
    , function(t, e, n) {
        n(212);
        var o = n(12);
        t.exports = o.Array.isArray
    }
    , function(t, e, n) {
        n(4)({
            target: "Array",
            stat: !0
        }, {
            isArray: n(48)
        })
    }
    , function(t, e, n) {
        var o = n(214);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(215)
          , r = Array.prototype;
        t.exports = function(t) {
            var e = t.slice;
            return t === r || t instanceof Array && e === r.slice ? o : e
        }
    }
    , function(t, e, n) {
        n(216);
        var o = n(17);
        t.exports = o("Array").slice
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(13)
          , i = n(48)
          , a = n(71)
          , u = n(35)
          , l = n(27)
          , c = n(80)
          , s = n(8)
          , f = n(49)
          , d = n(25)
          , p = f("slice")
          , A = d("slice", {
            ACCESSORS: !0,
            0: 0,
            1: 2
        })
          , v = s("species")
          , h = [].slice
          , g = Math.max;
        o({
            target: "Array",
            proto: !0,
            forced: !p || !A
        }, {
            slice: function(t, e) {
                var n, o, s, f = l(this), d = u(f.length), p = a(t, d), A = a(void 0 === e ? d : e, d);
                if (i(f) && ("function" != typeof (n = f.constructor) || n !== Array && !i(n.prototype) ? r(n) && null === (n = n[v]) && (n = void 0) : n = void 0,
                n === Array || void 0 === n))
                    return h.call(f, p, A);
                for (o = new (void 0 === n ? Array : n)(g(A - p, 0)),
                s = 0; p < A; p++,
                s++)
                    p in f && c(o, s, f[p]);
                return o.length = s,
                o
            }
        })
    }
    , function(t, e, n) {
        t.exports = n(218)
    }
    , function(t, e, n) {
        var o = n(219);
        t.exports = o
    }
    , function(t, e, n) {
        n(115),
        n(53),
        n(47);
        var o = n(82);
        t.exports = o.f("iterator")
    }
    , function(t, e, n) {
        t.exports = n(221)
    }
    , function(t, e, n) {
        var o = n(222);
        n(241),
        n(242),
        n(243),
        n(244),
        n(245),
        t.exports = o
    }
    , function(t, e, n) {
        n(223),
        n(52),
        n(224),
        n(226),
        n(227),
        n(228),
        n(229),
        n(115),
        n(230),
        n(231),
        n(232),
        n(233),
        n(234),
        n(235),
        n(236),
        n(237),
        n(238),
        n(239),
        n(240);
        var o = n(12);
        t.exports = o.Symbol
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(11)
          , i = n(48)
          , a = n(13)
          , u = n(34)
          , l = n(35)
          , c = n(80)
          , s = n(79)
          , f = n(49)
          , d = n(8)
          , p = n(78)
          , A = d("isConcatSpreadable")
          , v = p >= 51 || !r((function() {
            var t = [];
            return t[A] = !1,
            t.concat()[0] !== t
        }
        ))
          , h = f("concat")
          , g = function(t) {
            if (!a(t))
                return !1;
            var e = t[A];
            return void 0 !== e ? !!e : i(t)
        };
        o({
            target: "Array",
            proto: !0,
            forced: !v || !h
        }, {
            concat: function(t) {
                var e, n, o, r, i, a = u(this), f = s(a, 0), d = 0;
                for (e = -1,
                o = arguments.length; e < o; e++)
                    if (g(i = -1 === e ? a : arguments[e])) {
                        if (d + (r = l(i.length)) > 9007199254740991)
                            throw TypeError("Maximum allowed index exceeded");
                        for (n = 0; n < r; n++,
                        d++)
                            n in i && c(f, d, i[n])
                    } else {
                        if (d >= 9007199254740991)
                            throw TypeError("Maximum allowed index exceeded");
                        c(f, d++, i)
                    }
                return f.length = d,
                f
            }
        })
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(7)
          , i = n(36)
          , a = n(33)
          , u = n(14)
          , l = n(68)
          , c = n(92)
          , s = n(11)
          , f = n(15)
          , d = n(48)
          , p = n(13)
          , A = n(20)
          , v = n(34)
          , h = n(27)
          , g = n(51)
          , m = n(42)
          , y = n(57)
          , w = n(69)
          , x = n(116)
          , E = n(225)
          , b = n(117)
          , _ = n(62)
          , S = n(16)
          , M = n(63)
          , C = n(18)
          , B = n(46)
          , k = n(66)
          , I = n(55)
          , T = n(45)
          , N = n(56)
          , R = n(8)
          , D = n(82)
          , Q = n(10)
          , P = n(29)
          , H = n(32)
          , F = n(38).forEach
          , O = I("hidden")
          , j = R("toPrimitive")
          , L = H.set
          , U = H.getterFor("Symbol")
          , Y = Object.prototype
          , z = r.Symbol
          , G = i("JSON", "stringify")
          , $ = _.f
          , J = S.f
          , K = E.f
          , V = M.f
          , W = k("symbols")
          , X = k("op-symbols")
          , q = k("string-to-symbol-registry")
          , Z = k("symbol-to-string-registry")
          , tt = k("wks")
          , et = r.QObject
          , nt = !et || !et.prototype || !et.prototype.findChild
          , ot = u && s((function() {
            return 7 != y(J({}, "a", {
                get: function() {
                    return J(this, "a", {
                        value: 7
                    }).a
                }
            })).a
        }
        )) ? function(t, e, n) {
            var o = $(Y, e);
            o && delete Y[e],
            J(t, e, n),
            o && t !== Y && J(Y, e, o)
        }
        : J
          , rt = function(t, e) {
            var n = W[t] = y(z.prototype);
            return L(n, {
                type: "Symbol",
                tag: t,
                description: e
            }),
            u || (n.description = e),
            n
        }
          , it = c ? function(t) {
            return "symbol" == typeof t
        }
        : function(t) {
            return Object(t)instanceof z
        }
          , at = function(t, e, n) {
            t === Y && at(X, e, n),
            A(t);
            var o = g(e, !0);
            return A(n),
            f(W, o) ? (n.enumerable ? (f(t, O) && t[O][o] && (t[O][o] = !1),
            n = y(n, {
                enumerable: m(0, !1)
            })) : (f(t, O) || J(t, O, m(1, {})),
            t[O][o] = !0),
            ot(t, o, n)) : J(t, o, n)
        }
          , ut = function(t, e) {
            A(t);
            var n = h(e)
              , o = w(n).concat(ft(n));
            return F(o, (function(e) {
                u && !lt.call(n, e) || at(t, e, n[e])
            }
            )),
            t
        }
          , lt = function(t) {
            var e = g(t, !0)
              , n = V.call(this, e);
            return !(this === Y && f(W, e) && !f(X, e)) && (!(n || !f(this, e) || !f(W, e) || f(this, O) && this[O][e]) || n)
        }
          , ct = function(t, e) {
            var n = h(t)
              , o = g(e, !0);
            if (n !== Y || !f(W, o) || f(X, o)) {
                var r = $(n, o);
                return !r || !f(W, o) || f(n, O) && n[O][o] || (r.enumerable = !0),
                r
            }
        }
          , st = function(t) {
            var e = K(h(t))
              , n = [];
            return F(e, (function(t) {
                f(W, t) || f(T, t) || n.push(t)
            }
            )),
            n
        }
          , ft = function(t) {
            var e = t === Y
              , n = K(e ? X : h(t))
              , o = [];
            return F(n, (function(t) {
                !f(W, t) || e && !f(Y, t) || o.push(W[t])
            }
            )),
            o
        };
        (l || (B((z = function() {
            if (this instanceof z)
                throw TypeError("Symbol is not a constructor");
            var t = arguments.length && void 0 !== arguments[0] ? String(arguments[0]) : void 0
              , e = N(t)
              , n = function(t) {
                this === Y && n.call(X, t),
                f(this, O) && f(this[O], e) && (this[O][e] = !1),
                ot(this, e, m(1, t))
            };
            return u && nt && ot(Y, e, {
                configurable: !0,
                set: n
            }),
            rt(e, t)
        }
        ).prototype, "toString", (function() {
            return U(this).tag
        }
        )),
        B(z, "withoutSetter", (function(t) {
            return rt(N(t), t)
        }
        )),
        M.f = lt,
        S.f = at,
        _.f = ct,
        x.f = E.f = st,
        b.f = ft,
        D.f = function(t) {
            return rt(R(t), t)
        }
        ,
        u && (J(z.prototype, "description", {
            configurable: !0,
            get: function() {
                return U(this).description
            }
        }),
        a || B(Y, "propertyIsEnumerable", lt, {
            unsafe: !0
        }))),
        o({
            global: !0,
            wrap: !0,
            forced: !l,
            sham: !l
        }, {
            Symbol: z
        }),
        F(w(tt), (function(t) {
            Q(t)
        }
        )),
        o({
            target: "Symbol",
            stat: !0,
            forced: !l
        }, {
            for: function(t) {
                var e = String(t);
                if (f(q, e))
                    return q[e];
                var n = z(e);
                return q[e] = n,
                Z[n] = e,
                n
            },
            keyFor: function(t) {
                if (!it(t))
                    throw TypeError(t + " is not a symbol");
                if (f(Z, t))
                    return Z[t]
            },
            useSetter: function() {
                nt = !0
            },
            useSimple: function() {
                nt = !1
            }
        }),
        o({
            target: "Object",
            stat: !0,
            forced: !l,
            sham: !u
        }, {
            create: function(t, e) {
                return void 0 === e ? y(t) : ut(y(t), e)
            },
            defineProperty: at,
            defineProperties: ut,
            getOwnPropertyDescriptor: ct
        }),
        o({
            target: "Object",
            stat: !0,
            forced: !l
        }, {
            getOwnPropertyNames: st,
            getOwnPropertySymbols: ft
        }),
        o({
            target: "Object",
            stat: !0,
            forced: s((function() {
                b.f(1)
            }
            ))
        }, {
            getOwnPropertySymbols: function(t) {
                return b.f(v(t))
            }
        }),
        G) && o({
            target: "JSON",
            stat: !0,
            forced: !l || s((function() {
                var t = z();
                return "[null]" != G([t]) || "{}" != G({
                    a: t
                }) || "{}" != G(Object(t))
            }
            ))
        }, {
            stringify: function(t, e, n) {
                for (var o, r = [t], i = 1; arguments.length > i; )
                    r.push(arguments[i++]);
                if (o = e,
                (p(e) || void 0 !== t) && !it(t))
                    return d(e) || (e = function(t, e) {
                        if ("function" == typeof o && (e = o.call(this, t, e)),
                        !it(e))
                            return e
                    }
                    ),
                    r[1] = e,
                    G.apply(null, r)
            }
        });
        z.prototype[j] || C(z.prototype, j, z.prototype.valueOf),
        P(z, "Symbol"),
        T[O] = !0
    }
    , function(t, e, n) {
        var o = n(27)
          , r = n(116).f
          , i = {}.toString
          , a = "object" == typeof window && window && Object.getOwnPropertyNames ? Object.getOwnPropertyNames(window) : [];
        t.exports.f = function(t) {
            return a && "[object Window]" == i.call(t) ? function(t) {
                try {
                    return r(t)
                } catch (t) {
                    return a.slice()
                }
            }(t) : r(o(t))
        }
    }
    , function(t, e, n) {
        n(10)("asyncIterator")
    }
    , function(t, e) {}
    , function(t, e, n) {
        n(10)("hasInstance")
    }
    , function(t, e, n) {
        n(10)("isConcatSpreadable")
    }
    , function(t, e, n) {
        n(10)("match")
    }
    , function(t, e, n) {
        n(10)("matchAll")
    }
    , function(t, e, n) {
        n(10)("replace")
    }
    , function(t, e, n) {
        n(10)("search")
    }
    , function(t, e, n) {
        n(10)("species")
    }
    , function(t, e, n) {
        n(10)("split")
    }
    , function(t, e, n) {
        n(10)("toPrimitive")
    }
    , function(t, e, n) {
        n(10)("toStringTag")
    }
    , function(t, e, n) {
        n(10)("unscopables")
    }
    , function(t, e, n) {
        n(29)(Math, "Math", !0)
    }
    , function(t, e, n) {
        var o = n(7);
        n(29)(o.JSON, "JSON", !0)
    }
    , function(t, e, n) {
        n(10)("asyncDispose")
    }
    , function(t, e, n) {
        n(10)("dispose")
    }
    , function(t, e, n) {
        n(10)("observable")
    }
    , function(t, e, n) {
        n(10)("patternMatch")
    }
    , function(t, e, n) {
        n(10)("replaceAll")
    }
    , function(t, e, n) {
        n(247);
        var o = n(12);
        t.exports = o.setTimeout
    }
    , function(t, e, n) {
        var o = n(4)
          , r = n(7)
          , i = n(76)
          , a = [].slice
          , u = function(t) {
            return function(e, n) {
                var o = arguments.length > 2
                  , r = o ? a.call(arguments, 2) : void 0;
                return t(o ? function() {
                    ("function" == typeof e ? e : Function(e)).apply(this, r)
                }
                : e, n)
            }
        };
        o({
            global: !0,
            bind: !0,
            forced: /MSIE .\./.test(i)
        }, {
            setTimeout: u(r.setTimeout),
            setInterval: u(r.setInterval)
        })
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(249))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = i(n(254))
          , u = i(n(255))
          , l = i(n(118))
          , c = i(n(256))
          , s = i(n(257))
          , f = i(n(258))
          , d = i(n(259))
          , p = i(n(260))
          , A = (0,
        r.default)({}, a.default, u.default, l.default, s.default, c.default, f.default, d.default, p.default, {
            linkCheck: function(t, e) {
                return !0
            }
        }, {
            linkImgCheck: function(t) {
                return !0
            }
        });
        e.default = A
    }
    , function(t, e, n) {
        t.exports = n(250)
    }
    , function(t, e, n) {
        var o = n(251);
        t.exports = o
    }
    , function(t, e, n) {
        n(252);
        var o = n(12);
        t.exports = o.Object.assign
    }
    , function(t, e, n) {
        var o = n(4)
          , r = n(253);
        o({
            target: "Object",
            stat: !0,
            forced: Object.assign !== r
        }, {
            assign: r
        })
    }
    , function(t, e, n) {
        "use strict";
        var o = n(14)
          , r = n(11)
          , i = n(69)
          , a = n(117)
          , u = n(63)
          , l = n(34)
          , c = n(64)
          , s = Object.assign
          , f = Object.defineProperty;
        t.exports = !s || r((function() {
            if (o && 1 !== s({
                b: 1
            }, s(f({}, "a", {
                enumerable: !0,
                get: function() {
                    f(this, "b", {
                        value: 3,
                        enumerable: !1
                    })
                }
            }), {
                b: 2
            })).b)
                return !0;
            var t = {}
              , e = {}
              , n = Symbol();
            return t[n] = 7,
            "abcdefghijklmnopqrst".split("").forEach((function(t) {
                e[t] = t
            }
            )),
            7 != s({}, t)[n] || "abcdefghijklmnopqrst" != i(s({}, e)).join("")
        }
        )) ? function(t, e) {
            for (var n = l(t), r = arguments.length, s = 1, f = a.f, d = u.f; r > s; )
                for (var p, A = c(arguments[s++]), v = f ? i(A).concat(f(A)) : i(A), h = v.length, g = 0; h > g; )
                    p = v[g++],
                    o && !d.call(A, p) || (n[p] = A[p]);
            return n
        }
        : s
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var o = "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal"
          , r = "http://img.t.sinajs.cn/t35/style/images/common/face/ext/normal";
        e.default = {
            menus: ["head", "bold", "fontSize", "fontName", "italic", "underline", "strikeThrough", "indent", "lineHeight", "foreColor", "backColor", "link", "list", "justify", "quote", "emoticon", "image", "video", "table", "code", "splitLine", "undo", "redo"],
            fontNames: ["黑体", "仿宋", "楷体", "标楷体", "华文仿宋", "华文楷体", "宋体", "微软雅黑", "Arial", "Tahoma", "Verdana", "Times New Roman", "Courier New"],
            fontSizes: {
                "x-small": {
                    name: "10px",
                    value: "1"
                },
                small: {
                    name: "13px",
                    value: "2"
                },
                normal: {
                    name: "16px",
                    value: "3"
                },
                large: {
                    name: "18px",
                    value: "4"
                },
                "x-large": {
                    name: "24px",
                    value: "5"
                },
                "xx-large": {
                    name: "32px",
                    value: "6"
                },
                "xxx-large": {
                    name: "48px",
                    value: "7"
                }
            },
            colors: ["#000000", "#eeece0", "#1c487f", "#4d80bf", "#c24f4a", "#8baa4a", "#7b5ba1", "#46acc8", "#f9963b", "#ffffff"],
            languageType: ["Bash", "C", "C#", "C++", "CSS", "Java", "JavaScript", "JSON", "TypeScript", "Plain text", "Html", "XML", "SQL", "Go", "Kotlin", "Lua", "Markdown", "PHP", "Python", "Shell Session", "Ruby"],
            languageTab: "　　　　",
            emotions: [{
                title: "默认",
                type: "image",
                content: [{
                    alt: "[坏笑]",
                    src: o + "/50/pcmoren_huaixiao_org.png"
                }, {
                    alt: "[舔屏]",
                    src: o + "/40/pcmoren_tian_org.png"
                }, {
                    alt: "[污]",
                    src: o + "/3c/pcmoren_wu_org.png"
                }]
            }, {
                title: "新浪",
                type: "image",
                content: [{
                    src: r + "/7a/shenshou_thumb.gif",
                    alt: "[草泥马]"
                }, {
                    src: r + "/60/horse2_thumb.gif",
                    alt: "[神马]"
                }, {
                    src: r + "/bc/fuyun_thumb.gif",
                    alt: "[浮云]"
                }]
            }, {
                title: "emoji",
                type: "emoji",
                content: "😀 😃 😄 😁 😆 😅 😂 😊 😇 🙂 🙃 😉 😓 😪 😴 🙄 🤔 😬 🤐".split(/\s/)
            }, {
                title: "手势",
                type: "emoji",
                content: ["🙌", "👏", "👋", "👍", "👎", "👊", "✊", "️👌", "✋", "👐", "💪", "🙏", "️👆", "👇", "👈", "👉", "🖕", "🖐", "🤘"]
            }],
            lineHeights: ["1", "1.15", "1.6", "2", "2.5", "3"],
            undoLimit: 20
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var o = n(40);
        e.default = {
            onchangeTimeout: 200,
            onchange: o.EMPTY_FN,
            onfocus: o.EMPTY_FN,
            onblur: o.EMPTY_FN
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = {
            pasteFilterStyle: !0,
            pasteIgnoreImg: !1,
            pasteTextHandle: function(t) {
                return t
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = {
            styleWithCSS: !1
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var o = n(40);
        e.default = {
            showLinkImg: !0,
            linkImgCallback: o.EMPTY_FN,
            uploadImgServer: "",
            uploadImgShowBase64: !1,
            uploadImgMaxSize: 5242880,
            uploadImgMaxLength: 100,
            uploadFileName: "",
            uploadImgParams: {},
            uploadImgParamsWithUrl: !1,
            uploadImgHeaders: {},
            uploadImgHooks: {},
            uploadImgTimeout: 1e4,
            withCredentials: !1,
            customUploadImg: null
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = {
            focus: !0,
            height: 300,
            placeholder: "请输入正文",
            zIndexFullScreen: 1e4,
            showFullScreen: !0
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = {
            lang: "zh-CN",
            languages: {
                "zh-CN": {
                    wangEditor: {
                        "插入": "插入",
                        "默认": "默认",
                        "创建": "创建",
                        "修改": "修改",
                        "如": "如",
                        "请输入正文": "请输入正文",
                        menus: {
                            dropListMenu: {
                                "设置标题": "设置标题",
                                "背景颜色": "背景颜色",
                                "文字颜色": "文字颜色",
                                "设置字号": "设置字号",
                                "设置字体": "设置字体",
                                "设置缩进": "设置缩进",
                                "对齐方式": "对齐方式",
                                "设置行高": "设置行高",
                                "序列": "序列",
                                head: {
                                    "正文": "正文"
                                },
                                indent: {
                                    "增加缩进": "增加缩进",
                                    "减少缩进": "减少缩进"
                                },
                                justify: {
                                    "靠左": "靠左",
                                    "居中": "居中",
                                    "靠右": "靠右"
                                },
                                list: {
                                    "无序列表": "无序列表",
                                    "有序列表": "有序列表"
                                }
                            },
                            panelMenus: {
                                emoticon: {
                                    "默认": "默认",
                                    "新浪": "新浪",
                                    emoji: "emoji",
                                    "手势": "手势"
                                },
                                image: {
                                    "图片链接": "图片链接",
                                    "上传图片": "上传图片",
                                    "网络图片": "网络图片"
                                },
                                link: {
                                    "链接": "链接",
                                    "链接文字": "链接文字",
                                    "取消链接": "取消链接",
                                    "查看链接": "查看链接"
                                },
                                video: {
                                    "插入视频": "插入视频"
                                },
                                table: {
                                    "行": "行",
                                    "列": "列",
                                    "的": "的",
                                    "表格": "表格",
                                    "添加行": "添加行",
                                    "删除行": "添加行",
                                    "添加列": "添加列",
                                    "删除列": "删除列",
                                    "设置表头": "设置表头",
                                    "取消表头": "取消表头",
                                    "插入表格": "插入表格",
                                    "删除表格": "删除表格"
                                },
                                code: {
                                    "删除代码": "删除代码",
                                    "修改代码": "修改代码",
                                    "插入代码": "插入代码"
                                }
                            }
                        },
                        validate: {
                            "张图片": "张图片",
                            "大于": "大于",
                            "图片链接": "图片链接",
                            "不是图片": "不是图片",
                            "返回结果": "返回结果",
                            "上传图片超时": "上传图片超时",
                            "上传图片错误": "上传图片错误",
                            "上传图片失败": "上传图片失败",
                            "插入图片错误": "插入图片错误",
                            "一次最多上传": "一次最多上传",
                            "下载链接失败": "下载链接失败",
                            "图片验证未通过": "图片验证未通过",
                            "服务器返回状态": "服务器返回状态",
                            "上传图片返回结果错误": "上传图片返回结果错误",
                            "请替换为支持的图片类型": "请替换为支持的图片类型",
                            "您插入的网络图片无法识别": "您插入的网络图片无法识别",
                            "您刚才插入的图片链接未通过编辑器校验": "您刚才插入的图片链接未通过编辑器校验"
                        }
                    }
                },
                en: {
                    wangEditor: {
                        "插入": "insert",
                        "默认": "default",
                        "创建": "create",
                        "修改": "edit",
                        "如": "like",
                        "请输入正文": "please enter the text",
                        menus: {
                            dropListMenu: {
                                "设置标题": "title",
                                "背景颜色": "background",
                                "文字颜色": "font color",
                                "设置字号": "font size",
                                "设置字体": "font family",
                                "设置缩进": "indent",
                                "对齐方式": "align",
                                "设置行高": "line heihgt",
                                "序列": "list",
                                head: {
                                    "正文": "text"
                                },
                                indent: {
                                    "增加缩进": "indent",
                                    "减少缩进": "outdent"
                                },
                                justify: {
                                    "靠左": "left",
                                    "居中": "center",
                                    "靠右": "right"
                                },
                                list: {
                                    "无序列表": "unordered",
                                    "有序列表": "ordered"
                                }
                            },
                            panelMenus: {
                                emoticon: {
                                    "默认": "default",
                                    "新浪": "sina",
                                    emoji: "emoji",
                                    "手势": "gesture"
                                },
                                image: {
                                    "图片链接": "image link",
                                    "上传图片": "upload image",
                                    "网络图片": "network image"
                                },
                                link: {
                                    "链接": "link",
                                    "链接文字": "link text",
                                    "取消链接": "unlink",
                                    "查看链接": "view links"
                                },
                                video: {
                                    "插入视频": "insert video"
                                },
                                table: {
                                    "行": "rows",
                                    "列": "columns",
                                    "的": " ",
                                    "表格": "table",
                                    "添加行": "insert row",
                                    "删除行": "delete row",
                                    "添加列": "insert column",
                                    "删除列": "delete column",
                                    "设置表头": "set header",
                                    "取消表头": "cancel header",
                                    "插入表格": "insert table",
                                    "删除表格": "delete table"
                                },
                                code: {
                                    "删除代码": "delete code",
                                    "修改代码": "edit code",
                                    "插入代码": "insert code"
                                }
                            }
                        },
                        validate: {
                            "张图片": "images",
                            "大于": "greater than",
                            "图片链接": "image link",
                            "不是图片": "is not image",
                            "返回结果": "return results",
                            "上传图片超时": "upload image timeout",
                            "上传图片错误": "upload image error",
                            "上传图片失败": "upload image failed",
                            "插入图片错误": "insert image error",
                            "一次最多上传": "once most at upload",
                            "下载链接失败": "download link failed",
                            "图片验证未通过": "image validate failed",
                            "服务器返回状态": "server return status",
                            "上传图片返回结果错误": "upload image return results error",
                            "请替换为支持的图片类型": "please replace with a supported image type",
                            "您插入的网络图片无法识别": "the network picture you inserted is not recognized",
                            "您刚才插入的图片链接未通过编辑器校验": "the image link you just inserted did not pass the editor verification"
                        }
                    }
                }
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(2))
          , i = n(9)
          , a = o(n(262))
          , u = function() {
            function t(t) {
                this.editor = t,
                this._currentRange = null
            }
            return t.prototype.getRange = function() {
                return this._currentRange
            }
            ,
            t.prototype.saveRange = function(t) {
                if (t)
                    this._currentRange = t;
                else {
                    var e = window.getSelection();
                    if (0 !== e.rangeCount) {
                        var n = e.getRangeAt(0)
                          , o = this.getSelectionContainerElem(n);
                        if (o)
                            if ("false" !== o.attr("contenteditable") && !o.parentUntil("[contenteditable=false]"))
                                this.editor.$textElem.isContain(o) && (this._currentRange = n)
                    }
                }
            }
            ,
            t.prototype.collapseRange = function(t) {
                void 0 === t && (t = !1);
                var e = this._currentRange;
                e && e.collapse(t)
            }
            ,
            t.prototype.getSelectionText = function() {
                var t = this._currentRange;
                return t ? t.toString() : ""
            }
            ,
            t.prototype.getSelectionContainerElem = function(t) {
                var e, n;
                if (e = t || this._currentRange)
                    return n = e.commonAncestorContainer,
                    r.default(1 === n.nodeType ? n : n.parentNode)
            }
            ,
            t.prototype.getSelectionStartElem = function(t) {
                var e, n;
                if (e = t || this._currentRange)
                    return n = e.startContainer,
                    r.default(1 === n.nodeType ? n : n.parentNode)
            }
            ,
            t.prototype.getSelectionEndElem = function(t) {
                var e, n;
                if (e = t || this._currentRange)
                    return n = e.endContainer,
                    r.default(1 === n.nodeType ? n : n.parentNode)
            }
            ,
            t.prototype.isSelectionEmpty = function() {
                var t = this._currentRange;
                return !(!t || !t.startContainer || t.startContainer !== t.endContainer || t.startOffset !== t.endOffset)
            }
            ,
            t.prototype.restoreSelection = function() {
                var t = window.getSelection()
                  , e = this._currentRange;
                t && e && (t.removeAllRanges(),
                t.addRange(e))
            }
            ,
            t.prototype.createEmptyRange = function() {
                var t, e = this.editor, n = this.getRange();
                if (n && this.isSelectionEmpty())
                    try {
                        i.UA.isWebkit() ? (e.cmd.do("insertHTML", "&#8203;"),
                        n.setEnd(n.endContainer, n.endOffset + 1),
                        this.saveRange(n)) : (t = r.default("<strong>&#8203;</strong>"),
                        e.cmd.do("insertElem", t),
                        this.createRangeByElem(t, !0))
                    } catch (t) {}
            }
            ,
            t.prototype.createRangeByElem = function(t, e, n) {
                if (t.length) {
                    var o = t.elems[0]
                      , r = document.createRange();
                    n ? r.selectNodeContents(o) : r.selectNode(o),
                    null != e && r.collapse(e),
                    this.saveRange(r)
                }
            }
            ,
            t.prototype.getSelectionRangeTopNodes = function(t) {
                var e = new a.default(t);
                return e.init(),
                e.getSelectionNodes()
            }
            ,
            t
        }();
        e.default = u
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(2))
          , i = function() {
            function t(t) {
                this.editor = t,
                this.$nodeList = [],
                this.$startElem = r.default(t.selection.getSelectionStartElem()).getNodeTop(this.editor),
                this.$endElem = r.default(t.selection.getSelectionEndElem()).getNodeTop(this.editor)
            }
            return t.prototype.init = function() {
                this.recordSelectionNodes(r.default(this.$startElem))
            }
            ,
            t.prototype.addNodeList = function(t) {
                this.$nodeList.push(r.default(t))
            }
            ,
            t.prototype.isEndElem = function(t) {
                var e;
                return null === (e = this.$endElem) || void 0 === e ? void 0 : e.equal(t)
            }
            ,
            t.prototype.getNextSibling = function(t) {
                return r.default(t.elems[0].nextSibling)
            }
            ,
            t.prototype.recordSelectionNodes = function(t) {
                var e = t.getNodeTop(this.editor);
                e.length > 0 && (this.addNodeList(e),
                this.isEndElem(e) || this.recordSelectionNodes(this.getNextSibling(e)))
            }
            ,
            t.prototype.getSelectionNodes = function() {
                return this.$nodeList
            }
            ,
            t
        }();
        e.default = i
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(2))
          , i = function() {
            function t(t) {
                this.editor = t
            }
            return t.prototype.do = function(t, e) {
                var n = this.editor;
                n.config.styleWithCSS && document.execCommand("styleWithCSS", !1, "true");
                var o = n.selection;
                if (o.getRange()) {
                    switch (o.restoreSelection(),
                    t) {
                    case "insertHTML":
                        this.insertHTML(e);
                        break;
                    case "insertElem":
                        this.insertElem(e);
                        break;
                    default:
                        this.execCommand(t, e)
                    }
                    n.menus.changeActive(),
                    o.saveRange(),
                    o.restoreSelection(),
                    n.change()
                }
            }
            ,
            t.prototype.insertHTML = function(t) {
                var e = this.editor
                  , n = e.selection.getRange();
                null != n && (this.queryCommandSupported("insertHTML") ? this.execCommand("insertHTML", t) : n.insertNode && (n.deleteContents(),
                n.insertNode(r.default(t).elems[0]),
                e.selection.collapseRange()))
            }
            ,
            t.prototype.insertElem = function(t) {
                var e = this.editor.selection.getRange();
                null != e && e.insertNode && (e.deleteContents(),
                e.insertNode(t.elems[0]))
            }
            ,
            t.prototype.execCommand = function(t, e) {
                document.execCommand(t, !1, e)
            }
            ,
            t.prototype.queryCommandValue = function(t) {
                return document.queryCommandValue(t)
            }
            ,
            t.prototype.queryCommandState = function(t) {
                return document.queryCommandState(t)
            }
            ,
            t.prototype.queryCommandSupported = function(t) {
                return document.queryCommandSupported(t)
            }
            ,
            t
        }();
        e.default = i
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = o(n(60))
          , a = o(n(23))
          , u = o(n(61))
          , l = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var c = l(n(2))
          , s = l(n(269))
          , f = n(9)
          , d = l(n(281))
          , p = function() {
            function t(t) {
                this.editor = t,
                this.eventHooks = {
                    changeEvents: [],
                    dropEvents: [],
                    clickEvents: [],
                    keyupEvents: [],
                    tabUpEvents: [],
                    tabDownEvents: [],
                    enterUpEvents: [],
                    enterDownEvents: [],
                    deleteUpEvents: [],
                    deleteDownEvents: [],
                    pasteEvents: [],
                    linkClickEvents: [],
                    codeClickEvents: [],
                    textScrollEvents: [],
                    toolbarClickEvents: [],
                    imgClickEvents: [],
                    imgDragBarMouseDownEvents: [],
                    tableClickEvents: [],
                    menuClickEvents: [],
                    dropListMenuHoverEvents: [],
                    splitLineEvents: []
                }
            }
            return t.prototype.init = function() {
                this._saveRange(),
                this._bindEventHooks(),
                s.default(this)
            }
            ,
            t.prototype.togglePlaceholder = function() {
                var t, e = this.html(), n = (0,
                u.default)(t = this.editor.$textContainerElem).call(t, ".placeholder");
                n.hide(),
                e && "<p><br></p>" !== e && " " !== e || n.show()
            }
            ,
            t.prototype.clear = function() {
                this.html("<p><br></p>"),
                this.editor.change()
            }
            ,
            t.prototype.html = function(t) {
                var e = this.editor
                  , n = e.$textElem;
                if (null == t) {
                    var o = n.html();
                    return o = (o = (o = o.replace(/\u200b/gm, "")).replace(/<p><\/p>/gim, "")).replace(/<p><br\/?><\/p>$/gim, "")
                }
                "" === (t = (0,
                a.default)(t).call(t)) && (t = "<p><br></p>"),
                0 !== (0,
                i.default)(t).call(t, "<") && (t = "<p>" + t + "</p>"),
                n.html(t),
                this.editor.change(),
                e.initSelection()
            }
            ,
            t.prototype.getJSON = function() {
                var t = this.editor.$textElem;
                return d.default(t)
            }
            ,
            t.prototype.text = function(t) {
                var e = this.editor
                  , n = e.$textElem;
                e.$textContainerElem;
                if (null == t) {
                    var o = n.text();
                    return o = o.replace(/\u200b/gm, "")
                }
                n.text("<p>" + t + "</p>"),
                this.editor.change(),
                e.initSelection()
            }
            ,
            t.prototype.append = function(t) {
                var e = this.editor
                  , n = e.$textElem;
                0 !== (0,
                i.default)(t).call(t, "<") && (t = "<p>" + t + "</p>"),
                n.append(c.default(t)),
                this.editor.change(),
                e.initSelection()
            }
            ,
            t.prototype._saveRange = function() {
                var t = this.editor
                  , e = t.$textElem;
                function n() {
                    t.selection.saveRange(),
                    t.menus.changeActive()
                }
                e.on("keyup", n),
                e.on("mousedown", (function() {
                    e.on("mouseleave", n)
                }
                )),
                e.on("mouseup", (function() {
                    n(),
                    e.off("mouseleave", n)
                }
                ))
            }
            ,
            t.prototype._bindEventHooks = function() {
                var t = this.editor
                  , e = t.$textElem
                  , n = this.eventHooks;
                function o(t) {
                    t.preventDefault()
                }
                e.on("click", (function(t) {
                    var e = n.clickEvents;
                    (0,
                    r.default)(e).call(e, (function(e) {
                        return e(t)
                    }
                    ))
                }
                )),
                e.on("keyup", (function(t) {
                    if (13 === t.keyCode) {
                        var e = n.enterUpEvents;
                        (0,
                        r.default)(e).call(e, (function(e) {
                            return e(t)
                        }
                        ))
                    }
                }
                )),
                e.on("keyup", (function(t) {
                    var e = n.keyupEvents;
                    (0,
                    r.default)(e).call(e, (function(e) {
                        return e(t)
                    }
                    ))
                }
                )),
                e.on("keyup", (function(t) {
                    if (8 === t.keyCode) {
                        var e = n.deleteUpEvents;
                        (0,
                        r.default)(e).call(e, (function(e) {
                            return e(t)
                        }
                        ))
                    }
                }
                )),
                e.on("keydown", (function(t) {
                    if (8 === t.keyCode) {
                        var e = n.deleteDownEvents;
                        (0,
                        r.default)(e).call(e, (function(e) {
                            return e(t)
                        }
                        ))
                    }
                }
                )),
                e.on("paste", (function(t) {
                    if (!f.UA.isIE()) {
                        t.preventDefault();
                        var e = n.pasteEvents;
                        (0,
                        r.default)(e).call(e, (function(e) {
                            return e(t)
                        }
                        ))
                    }
                }
                )),
                e.on("keydown", (function(e) {
                    if (!e.ctrlKey && !e.metaKey || 90 !== e.keyCode)
                        return !1;
                    e.preventDefault(),
                    e.shiftKey ? t.undo.redo() : t.undo.undo()
                }
                )),
                e.on("keyup", (function(t) {
                    if (9 === t.keyCode) {
                        t.preventDefault();
                        var e = n.tabUpEvents;
                        (0,
                        r.default)(e).call(e, (function(e) {
                            return e(t)
                        }
                        ))
                    }
                }
                )),
                e.on("keydown", (function(t) {
                    if (9 === t.keyCode) {
                        t.preventDefault();
                        var e = n.tabDownEvents;
                        (0,
                        r.default)(e).call(e, (function(e) {
                            return e(t)
                        }
                        ))
                    }
                }
                )),
                e.on("scroll", f.throttle((function(t) {
                    var e = n.textScrollEvents;
                    (0,
                    r.default)(e).call(e, (function(e) {
                        return e(t)
                    }
                    ))
                }
                ), 100)),
                c.default(document).on("dragleave", o).on("drop", o).on("dragenter", o).on("dragover", o),
                t.beforeDestroy((function() {
                    c.default(document).off("dragleave", o).off("drop", o).off("dragenter", o).off("dragover", o)
                }
                )),
                e.on("drop", (function(t) {
                    t.preventDefault();
                    var e = n.dropEvents;
                    (0,
                    r.default)(e).call(e, (function(e) {
                        return e(t)
                    }
                    ))
                }
                )),
                e.on("click", (function(t) {
                    var e = null
                      , o = t.target
                      , i = c.default(o);
                    if ("A" === i.getNodeName())
                        e = i;
                    else {
                        var a = i.parentUntil("a");
                        null != a && (e = a)
                    }
                    if (null != e) {
                        var u = n.linkClickEvents;
                        (0,
                        r.default)(u).call(u, (function(t) {
                            return t(e)
                        }
                        ))
                    }
                }
                )),
                e.on("click", (function(t) {
                    var e = null
                      , o = t.target
                      , i = c.default(o);
                    if ("IMG" !== i.getNodeName() || i.elems[0].getAttribute("class") && "eleImg" === i.elems[0].getAttribute("class") || i.elems[0].getAttribute("alt") || (t.stopPropagation(),
                    e = i),
                    null != e) {
                        var a = n.imgClickEvents;
                        (0,
                        r.default)(a).call(a, (function(t) {
                            return t(e)
                        }
                        ))
                    }
                }
                )),
                e.on("click", (function(t) {
                    var e = null
                      , o = t.target
                      , i = c.default(o);
                    if ("PRE" === i.getNodeName())
                        e = i;
                    else {
                        var a = i.parentUntil("pre");
                        null != a && (e = a)
                    }
                    if (null != e) {
                        var u = n.codeClickEvents;
                        (0,
                        r.default)(u).call(u, (function(t) {
                            return t(e)
                        }
                        ))
                    }
                }
                )),
                e.on("click", (function(e) {
                    var o = null
                      , i = e.target
                      , a = c.default(i);
                    if ("HR" === a.getNodeName() && (o = a),
                    null != o) {
                        t.selection.createRangeByElem(o),
                        t.selection.restoreSelection();
                        var u = n.splitLineEvents;
                        (0,
                        r.default)(u).call(u, (function(t) {
                            return t(o)
                        }
                        ))
                    }
                }
                )),
                t.$toolbarElem.on("click", (function(t) {
                    var e = n.toolbarClickEvents;
                    (0,
                    r.default)(e).call(e, (function(e) {
                        return e(t)
                    }
                    ))
                }
                )),
                t.$textContainerElem.on("mousedown", (function(t) {
                    var e = t.target;
                    if (c.default(e).hasClass("w-e-img-drag-rb")) {
                        var o = n.imgDragBarMouseDownEvents;
                        (0,
                        r.default)(o).call(o, (function(t) {
                            return t()
                        }
                        ))
                    }
                }
                )),
                e.on("click", (function(t) {
                    var e, o = t.target;
                    if (null != (e = c.default(o).parentUntil("TABLE", o))) {
                        var i = n.tableClickEvents;
                        (0,
                        r.default)(i).call(i, (function(t) {
                            return t(e)
                        }
                        ))
                    }
                }
                )),
                e.on("keydown", (function(t) {
                    if (13 === t.keyCode) {
                        var e = n.enterDownEvents;
                        (0,
                        r.default)(e).call(e, (function(e) {
                            return e(t)
                        }
                        ))
                    }
                }
                ))
            }
            ,
            t
        }();
        e.default = p
    }
    , function(t, e, n) {
        var o = n(266);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(267)
          , r = Array.prototype;
        t.exports = function(t) {
            var e = t.find;
            return t === r || t instanceof Array && e === r.find ? o : e
        }
    }
    , function(t, e, n) {
        n(268);
        var o = n(17);
        t.exports = o("Array").find
    }
    , function(t, e, n) {
        "use strict";
        var o = n(4)
          , r = n(38).find
          , i = n(74)
          , a = n(25)
          , u = !0
          , l = a("find");
        "find"in [] && Array(1).find((function() {
            u = !1
        }
        )),
        o({
            target: "Array",
            proto: !0,
            forced: u || !l
        }, {
            find: function(t) {
                return r(this, t, arguments.length > 1 ? arguments[1] : void 0)
            }
        }),
        i("find")
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(270))
          , i = o(n(271))
          , a = o(n(272))
          , u = o(n(273))
          , l = o(n(280));
        e.default = function(t) {
            var e = t.editor
              , n = t.eventHooks;
            r.default(e, n.enterUpEvents, n.enterDownEvents),
            i.default(e, n.deleteUpEvents, n.deleteDownEvents),
            a.default(e, n.tabDownEvents),
            u.default(e, n.pasteEvents),
            l.default(e, n.imgClickEvents)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(2));
        e.default = function(t, e, n) {
            function o(e) {
                var n = r.default("<p><br></p>");
                n.insertBefore(e),
                t.selection.createRangeByElem(n, !0),
                t.selection.restoreSelection(),
                e.remove()
            }
            e.push((function() {
                var e = t.$textElem
                  , n = t.selection.getSelectionContainerElem()
                  , r = n.parent();
                "<code><br></code>" !== r.html() ? r.equal(e) && "P" !== n.getNodeName() && (n.text() || o(n)) : o(n)
            }
            )),
            n.push((function(e) {
                var n;
                t.selection.saveRange(null === (n = getSelection()) || void 0 === n ? void 0 : n.getRangeAt(0)),
                t.selection.getSelectionContainerElem().id === t.textElemId && (e.preventDefault(),
                t.cmd.do("insertHTML", "<p><br></p>"))
            }
            ))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(23))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = i(n(2));
        e.default = function(t, e, n) {
            e.push((function() {
                var e, n = t.$textElem, o = (0,
                r.default)(e = n.html().toLowerCase()).call(e);
                if (!o || "<br>" === o) {
                    var i = a.default("<p><br/></p>");
                    n.html(" "),
                    n.append(i),
                    t.selection.createRangeByElem(i, !1, !0),
                    t.selection.restoreSelection()
                }
            }
            )),
            n.push((function(e) {
                var n, o = t.$textElem;
                "<p><br></p>" !== (0,
                r.default)(n = o.html().toLowerCase()).call(n) || e.preventDefault()
            }
            ))
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = function(t, e) {
            e.push((function() {
                if (t.cmd.queryCommandSupported("insertHTML")) {
                    var e = t.selection.getSelectionContainerElem();
                    if (e) {
                        var n = e.parent()
                          , o = e.getNodeName()
                          , r = n.getNodeName();
                        "CODE" == o || "CODE" === r || "PRE" === r || /hljs/.test(r) ? t.cmd.do("insertHTML", t.config.languageTab) : t.cmd.do("insertHTML", "&nbsp;&nbsp;&nbsp;&nbsp;")
                    }
                }
            }
            ))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(23));
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var i = n(119)
          , a = n(9)
          , u = n(40);
        function l(t) {
            var e = t;
            return e = (e = (e = e.replace(/<br>|<br\/>/gim, "")).replace(/<div>/gim, "<p>").replace(/<\/div>/gim, "</p>")).replace(/<p><\/p>/gim, "<p><br></p>"),
            (0,
            r.default)(e).call(e)
        }
        e.default = function(t, e) {
            e.push((function(e) {
                var n = t.config
                  , o = n.pasteFilterStyle
                  , r = n.pasteIgnoreImg
                  , c = n.pasteTextHandle
                  , s = i.getPasteHtml(e, o, r)
                  , f = i.getPasteText(e);
                f = f.replace(/\n/gm, "<br>");
                var d = t.selection.getSelectionContainerElem();
                if (d) {
                    var p, A = null == d ? void 0 : d.getNodeName(), v = null == d ? void 0 : d.getNodeTop(t), h = "";
                    if (v.elems[0] && (h = null == v ? void 0 : v.getNodeName()),
                    "CODE" === A || "PRE" === h)
                        return c && a.isFunction(c) && (f = "" + (c(f) || "")),
                        void t.cmd.do("insertHTML", (p = f,
                        p.replace(/<br>|<br\/>/gm, "\n").replace(/<[^>]+>/gm, "")));
                    if (u.urlRegex.test(f))
                        return t.cmd.do("insertHTML", '<a href="' + f + '" target="_blank">' + f + "</a>");
                    if (s)
                        try {
                            c && a.isFunction(c) && (s = "" + (c(s) || "")),
                            t.cmd.do("insertHTML", "" + l(s))
                        } catch (e) {
                            c && a.isFunction(c) && (f = "" + (c(f) || "")),
                            t.cmd.do("insertHTML", "" + l(f))
                        }
                }
            }
            ))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(105))
          , i = o(n(6))
          , a = o(n(23))
          , u = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var l = n(275)
          , c = u(n(279));
        function s(t, e) {
            var n;
            return t = (0,
            a.default)(n = t.toLowerCase()).call(n),
            !!l.IGNORE_TAGS.has(t) || !(!e || "img" !== t)
        }
        e.default = function(t, e, n) {
            void 0 === e && (e = !0),
            void 0 === n && (n = !1);
            var o = []
              , u = "";
            (new c.default).parse(t, {
                startElement: function(t, c) {
                    if (function(t) {
                        (t = (0,
                        a.default)(t).call(t)) && (l.EMPTY_TAGS.has(t) || (u = t))
                    }(t),
                    !s(t, n)) {
                        var f = l.NECESSARY_ATTRS.get(t) || []
                          , d = [];
                        (0,
                        i.default)(c).call(c, (function(t) {
                            var n = t.name;
                            "style" !== n ? !1 !== (0,
                            r.default)(f).call(f, n) && d.push(t) : e || d.push(t)
                        }
                        ));
                        var p = function(t, e) {
                            var n = "";
                            n = "<" + t;
                            var o = [];
                            return (0,
                            i.default)(e).call(e, (function(t) {
                                o.push(t.name + '="' + t.value + '"')
                            }
                            )),
                            o.length > 0 && (n = n + " " + o.join(" ")),
                            n = n + (l.EMPTY_TAGS.has(t) ? "/" : "") + ">"
                        }(t, d);
                        o.push(p)
                    }
                },
                characters: function(t) {
                    (t = (0,
                    a.default)(t).call(t)) && (s(u, n) || o.push(t))
                },
                endElement: function(t) {
                    if (!s(t, n)) {
                        var e = function(t) {
                            return "</" + t + ">"
                        }(t);
                        o.push(e),
                        u = ""
                    }
                },
                comment: function(t) {}
            });
            var f = o.join("");
            return f = function(t) {
                var e = /<span>(.*?)<\/span>/;
                return t.replace(/<span>.*?<\/span>/gi, (function(t) {
                    var n = t.match(e);
                    return null == n ? "" : n[1]
                }
                ))
            }(f)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(108))
          , i = o(n(120));
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.TOP_LEVEL_TAGS = e.EMPTY_TAGS = e.NECESSARY_ATTRS = e.IGNORE_TAGS = void 0,
        e.IGNORE_TAGS = new i.default(["doctype", "!doctype", "html", "head", "meta", "body", "script", "style", "link", "frame", "iframe", "title", "svg", "center"]),
        e.NECESSARY_ATTRS = new r.default([["img", ["src", "alt"]], ["a", ["href", "target"]], ["td", ["colspan", "rowspan"]], ["th", ["colspan", "rowspan"]]]),
        e.EMPTY_TAGS = new i.default(["area", "base", "basefont", "br", "col", "hr", "img", "input", "isindex", "embed"]),
        e.TOP_LEVEL_TAGS = new i.default(["h1", "h2", "h3", "h4", "h5", "p", "ul", "ol", "table", "blockquote", "pre", "hr", "form"])
    }
    , function(t, e, n) {
        var o = n(277);
        t.exports = o
    }
    , function(t, e, n) {
        n(278),
        n(52),
        n(53),
        n(47);
        var o = n(12);
        t.exports = o.Set
    }
    , function(t, e, n) {
        "use strict";
        var o = n(109)
          , r = n(111);
        t.exports = o("Set", (function(t) {
            return function() {
                return t(this, arguments.length ? arguments[0] : void 0)
            }
        }
        ), r)
    }
    , function(t, e) {
        function n() {}
        n.prototype = {
            handler: null,
            startTagRe: /^<([^>\s\/]+)((\s+[^=>\s]+(\s*=\s*((\"[^"]*\")|(\'[^']*\')|[^>\s]+))?)*)\s*\/?\s*>/m,
            endTagRe: /^<\/([^>\s]+)[^>]*>/m,
            attrRe: /([^=\s]+)(\s*=\s*((\"([^"]*)\")|(\'([^']*)\')|[^>\s]+))?/gm,
            parse: function(t, e) {
                e && (this.contentHandler = e);
                for (var n, o, r, i = !1, a = this; t.length > 0; )
                    "\x3c!--" == t.substring(0, 4) ? -1 != (r = t.indexOf("--\x3e")) ? (this.contentHandler.comment(t.substring(4, r)),
                    t = t.substring(r + 3),
                    i = !1) : i = !0 : "</" == t.substring(0, 2) ? this.endTagRe.test(t) ? (RegExp.leftContext,
                    n = RegExp.lastMatch,
                    o = RegExp.rightContext,
                    n.replace(this.endTagRe, (function() {
                        return a.parseEndTag.apply(a, arguments)
                    }
                    )),
                    t = o,
                    i = !1) : i = !0 : "<" == t.charAt(0) && (this.startTagRe.test(t) ? (RegExp.leftContext,
                    n = RegExp.lastMatch,
                    o = RegExp.rightContext,
                    n.replace(this.startTagRe, (function() {
                        return a.parseStartTag.apply(a, arguments)
                    }
                    )),
                    t = o,
                    i = !1) : i = !0),
                    i && (-1 == (r = t.indexOf("<")) ? (this.contentHandler.characters(t),
                    t = "") : (this.contentHandler.characters(t.substring(0, r)),
                    t = t.substring(r))),
                    i = !0
            },
            parseStartTag: function(t, e, n) {
                var o = this.parseAttributes(e, n);
                this.contentHandler.startElement(e, o)
            },
            parseEndTag: function(t, e) {
                this.contentHandler.endElement(e)
            },
            parseAttributes: function(t, e) {
                var n = this
                  , o = [];
                return e.replace(this.attrRe, (function(e, r, i, a, u, l, c) {
                    o.push(n.parseAttribute(t, e, r, i, a, u, l, c))
                }
                )),
                o
            },
            parseAttribute: function(t, e, n) {
                var o = "";
                arguments[7] ? o = arguments[8] : arguments[5] ? o = arguments[6] : arguments[3] && (o = arguments[4]);
                var r = !o && !arguments[3];
                return {
                    name: n,
                    value: r ? null : o
                }
            }
        },
        t.exports = n
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = function(t, e) {
            e.push((function(e) {
                t.selection.createRangeByElem(e),
                t.selection.restoreSelection()
            }
            ))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = n(9)
          , u = i(n(2));
        e.default = function t(e) {
            var n = []
              , o = e.childNodes() || [];
            return (0,
            r.default)(o).call(o, (function(e) {
                var o, r = e.nodeType;
                if (3 === r && (o = e.textContent || "",
                o = a.replaceHtmlSymbol(o)),
                1 === r) {
                    (o = o = {}).tag = e.nodeName.toLowerCase();
                    for (var i = [], l = e.attributes || [], c = l.length || 0, s = 0; s < c; s++) {
                        var f = l[s];
                        i.push({
                            name: f.name,
                            value: f.value
                        })
                    }
                    o.attrs = i,
                    o.children = t(u.default(e))
                }
                o && n.push(o)
            }
            )),
            n
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(121))
          , i = o(n(83))
          , a = o(n(6))
          , u = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var l = u(n(288))
          , c = function() {
            function t(t) {
                this.editor = t,
                this.menuList = [],
                this.constructorList = l.default
            }
            return t.prototype.extend = function(t, e) {
                e && "function" == typeof e && (this.constructorList[t] = e)
            }
            ,
            t.prototype.init = function() {
                var t, e = this, n = this.editor.config;
                (0,
                a.default)(t = n.menus).call(t, (function(t) {
                    var n = e.constructorList[t];
                    if (null != n && "function" == typeof n) {
                        var o = new n(e.editor);
                        o.key = t,
                        e.menuList.push(o)
                    }
                }
                )),
                this._addToToolbar()
            }
            ,
            t.prototype._addToToolbar = function() {
                var t, e = this.editor.$toolbarElem;
                (0,
                a.default)(t = this.menuList).call(t, (function(t) {
                    var n = t.$elem;
                    n && e.append(n)
                }
                ))
            }
            ,
            t.prototype.menuFind = function(t) {
                for (var e = this.menuList, n = 0, o = e.length; n < o; n++)
                    if (e[n].key === t)
                        return e[n];
                return e[0]
            }
            ,
            t.prototype.changeActive = function() {
                var t;
                (0,
                a.default)(t = this.menuList).call(t, (function(t) {
                    var e;
                    (0,
                    i.default)((0,
                    r.default)(e = t.tryChangeActive).call(e, t), 100)
                }
                ))
            }
            ,
            t
        }();
        e.default = c
    }
    , function(t, e, n) {
        var o = n(284);
        t.exports = o
    }
    , function(t, e, n) {
        var o = n(285)
          , r = Function.prototype;
        t.exports = function(t) {
            var e = t.bind;
            return t === r || t instanceof Function && e === r.bind ? o : e
        }
    }
    , function(t, e, n) {
        n(286);
        var o = n(17);
        t.exports = o("Function").bind
    }
    , function(t, e, n) {
        n(4)({
            target: "Function",
            proto: !0
        }, {
            bind: n(287)
        })
    }
    , function(t, e, n) {
        "use strict";
        var o = n(31)
          , r = n(13)
          , i = [].slice
          , a = {}
          , u = function(t, e, n) {
            if (!(e in a)) {
                for (var o = [], r = 0; r < e; r++)
                    o[r] = "a[" + r + "]";
                a[e] = Function("C,a", "return new C(" + o.join(",") + ")")
            }
            return a[e](t, n)
        };
        t.exports = Function.bind || function(t) {
            var e = o(this)
              , n = i.call(arguments, 1)
              , a = function() {
                var o = n.concat(i.call(arguments));
                return this instanceof a ? u(e, o.length, o) : e.apply(t, o)
            };
            return r(e.prototype) && (a.prototype = e.prototype),
            a
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(289))
          , i = o(n(293))
          , a = o(n(294))
          , u = o(n(298))
          , l = o(n(299))
          , c = o(n(300))
          , s = o(n(301))
          , f = o(n(303))
          , d = o(n(305))
          , p = o(n(306))
          , A = o(n(307))
          , v = o(n(308))
          , h = o(n(309))
          , g = o(n(311))
          , m = o(n(331))
          , y = o(n(335))
          , w = o(n(337))
          , x = o(n(338))
          , E = o(n(340))
          , b = o(n(341))
          , _ = o(n(342))
          , S = o(n(351))
          , M = o(n(355));
        e.default = {
            bold: r.default,
            head: i.default,
            italic: u.default,
            link: a.default,
            underline: l.default,
            strikeThrough: c.default,
            fontName: s.default,
            fontSize: f.default,
            justify: d.default,
            quote: p.default,
            backColor: A.default,
            foreColor: v.default,
            video: h.default,
            image: g.default,
            indent: m.default,
            emoticon: y.default,
            list: w.default,
            lineHeight: x.default,
            undo: E.default,
            redo: b.default,
            table: _.default,
            code: S.default,
            splitLine: M.default
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(24))
          , f = c(n(2))
          , d = function(t) {
            function e(e) {
                var n = f.default('<div class="w-e-menu">\n                <i class="w-e-icon-bold"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                var t = this.editor
                  , e = t.selection.isSelectionEmpty();
                e && t.selection.createEmptyRange(),
                t.cmd.do("bold"),
                e && (t.selection.collapseRange(),
                t.selection.restoreSelection())
            }
            ,
            e.prototype.tryChangeActive = function() {
                this.editor.cmd.queryCommandState("bold") ? this.active() : this.unActive()
            }
            ,
            e
        }(s.default);
        e.default = d
    }
    , function(t, e, n) {
        var o = n(291);
        t.exports = o
    }
    , function(t, e, n) {
        n(292);
        var o = n(12);
        t.exports = o.Object.setPrototypeOf
    }
    , function(t, e, n) {
        n(4)({
            target: "Object",
            stat: !0
        }, {
            setPrototypeOf: n(95)
        })
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(19))
          , f = c(n(2))
          , d = function(t) {
            function e(e) {
                var n = this
                  , o = f.default('<div class="w-e-menu"><i class="w-e-icon-header"></i></div>')
                  , r = {
                    width: 100,
                    title: "设置标题",
                    type: "list",
                    list: [{
                        $elem: f.default("<h1>H1</h1>"),
                        value: "<h1>"
                    }, {
                        $elem: f.default("<h2>H2</h2>"),
                        value: "<h2>"
                    }, {
                        $elem: f.default("<h3>H3</h3>"),
                        value: "<h3>"
                    }, {
                        $elem: f.default("<h4>H4</h4>"),
                        value: "<h4>"
                    }, {
                        $elem: f.default("<h5>H5</h5>"),
                        value: "<h5>"
                    }, {
                        $elem: f.default("<p>" + e.i18next.t("menus.dropListMenu.head.正文") + "</p>"),
                        value: "<p>"
                    }],
                    clickHandler: function(t) {
                        n.command(t)
                    }
                };
                return n = t.call(this, o, e, r) || this
            }
            return l(e, t),
            e.prototype.command = function(t) {
                var e = this.editor
                  , n = e.selection.getSelectionContainerElem();
                n && e.$textElem.equal(n) || e.cmd.do("formatBlock", t)
            }
            ,
            e.prototype.tryChangeActive = function() {
                var t = this.editor.cmd.queryCommandValue("formatBlock");
                /^h/i.test(t) ? this.active() : this.unActive()
            }
            ,
            e
        }(s.default);
        e.default = d
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(30))
          , f = c(n(2))
          , d = c(n(295))
          , p = c(n(123))
          , A = c(n(26))
          , v = c(n(296))
          , h = function(t) {
            function e(e) {
                var n, o = f.default('<div class="w-e-menu"><i class="w-e-icon-link"></i></div>');
                return n = t.call(this, o, e) || this,
                v.default(e),
                n
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                var t, e = this.editor;
                if (this.isActive) {
                    if (!(t = e.selection.getSelectionContainerElem()))
                        return;
                    this.createPanel(t.text(), t.attr("href"))
                } else
                    e.selection.isSelectionEmpty() ? this.createPanel("", "") : this.createPanel(e.selection.getSelectionText(), "")
            }
            ,
            e.prototype.createPanel = function(t, e) {
                var n = d.default(this.editor, t, e);
                new A.default(this,n).create()
            }
            ,
            e.prototype.tryChangeActive = function() {
                var t = this.editor;
                p.default(t) ? this.active() : this.unActive()
            }
            ,
            e
        }(s.default);
        e.default = h
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(23))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = n(9)
          , u = i(n(2))
          , l = i(n(123));
        e.default = function(t, e, n) {
            var o, i = a.getRandom("input-link"), c = a.getRandom("input-text"), s = a.getRandom("btn-ok"), f = a.getRandom("btn-del"), d = l.default(t) ? "inline-block" : "none";
            function p() {
                if (l.default(t)) {
                    var e = t.selection.getSelectionContainerElem();
                    e && (t.selection.createRangeByElem(e),
                    t.selection.restoreSelection(),
                    o = e)
                }
            }
            return {
                width: 300,
                height: 0,
                tabs: [{
                    title: t.i18next.t("menus.panelMenus.link.链接"),
                    tpl: '<div>\n                        <input \n                            id="' + c + '" \n                            type="text" \n                            class="block" \n                            value="' + e + '" \n                            placeholder="' + t.i18next.t("menus.panelMenus.link.链接文字") + '"/>\n                        </td>\n                        <input \n                            id="' + i + '" \n                            type="text" \n                            class="block" \n                            value="' + n + '" \n                            placeholder="' + t.i18next.t("如") + ' https://..."/>\n                        </td>\n                        <div class="w-e-button-container">\n                            <button id="' + s + '" class="right">\n                                ' + t.i18next.t("插入") + '\n                            </button>\n                            <button id="' + f + '" class="gray right" style="display:' + d + '">\n                                ' + t.i18next.t("menus.panelMenus.link.取消链接") + "\n                            </button>\n                        </div>\n                    </div>",
                    events: [{
                        selector: "#" + s,
                        type: "click",
                        fn: function() {
                            var e, n, o = u.default("#" + i), a = u.default("#" + c), s = (0,
                            r.default)(e = o.val()).call(e), f = (0,
                            r.default)(n = a.val()).call(n);
                            if (s && (f || (f = s),
                            function(e, n) {
                                var o = t.config.linkCheck(e, n);
                                if (void 0 === o)
                                    ;
                                else {
                                    if (!0 === o)
                                        return !0;
                                    alert(o)
                                }
                                return !1
                            }(f, s)))
                                return function(e, n) {
                                    l.default(t) ? (p(),
                                    t.cmd.do("insertHTML", '<a href="' + n + '" target="_blank">' + e + "</a>")) : t.cmd.do("insertHTML", '<a href="' + n + '" target="_blank">' + e + "</a>")
                                }(f, s),
                                !0
                        }
                    }, {
                        selector: "#" + f,
                        type: "click",
                        fn: function() {
                            return function() {
                                if (l.default(t)) {
                                    p();
                                    var e = o.text();
                                    t.cmd.do("insertHTML", "<span>" + e + "</span>")
                                }
                            }(),
                            !0
                        }
                    }]
                }]
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(297));
        e.default = function(t) {
            r.default(t)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r, i, a = o(n(2)), u = o(n(41));
        function l(t) {
            var e = [{
                $elem: a.default("<span>" + i.i18next.t("menus.panelMenus.link.查看链接") + "</span>"),
                onClick: function(t, e) {
                    var n = e.attr("href");
                    return window.open(n, "_target"),
                    !0
                }
            }, {
                $elem: a.default("<span>" + i.i18next.t("menus.panelMenus.link.取消链接") + "</span>"),
                onClick: function(t, e) {
                    t.selection.createRangeByElem(e),
                    t.selection.restoreSelection();
                    var n = e.text();
                    return t.cmd.do("insertHTML", "<span>" + n + "</span>"),
                    !0
                }
            }];
            (r = new u.default(i,t,e)).create()
        }
        function c() {
            r && (r.remove(),
            r = null)
        }
        e.default = function(t) {
            i = t,
            t.txt.eventHooks.linkClickEvents.push(l),
            t.txt.eventHooks.clickEvents.push(c),
            t.txt.eventHooks.keyupEvents.push(c),
            t.txt.eventHooks.toolbarClickEvents.push(c),
            t.txt.eventHooks.menuClickEvents.push(c),
            t.txt.eventHooks.textScrollEvents.push(c)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(24))
          , f = c(n(2))
          , d = function(t) {
            function e(e) {
                var n = f.default('<div class="w-e-menu">\n                <i class="w-e-icon-italic"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                var t = this.editor
                  , e = t.selection.isSelectionEmpty();
                e && t.selection.createEmptyRange(),
                t.cmd.do("italic"),
                e && (t.selection.collapseRange(),
                t.selection.restoreSelection())
            }
            ,
            e.prototype.tryChangeActive = function() {
                this.editor.cmd.queryCommandState("italic") ? this.active() : this.unActive()
            }
            ,
            e
        }(s.default);
        e.default = d
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(24))
          , f = c(n(2))
          , d = function(t) {
            function e(e) {
                var n = f.default('<div class="w-e-menu">\n                <i class="w-e-icon-underline"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                var t = this.editor
                  , e = t.selection.isSelectionEmpty();
                e && t.selection.createEmptyRange(),
                t.cmd.do("underline"),
                e && (t.selection.collapseRange(),
                t.selection.restoreSelection())
            }
            ,
            e.prototype.tryChangeActive = function() {
                this.editor.cmd.queryCommandState("underline") ? this.active() : this.unActive()
            }
            ,
            e
        }(s.default);
        e.default = d
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(24))
          , f = c(n(2))
          , d = function(t) {
            function e(e) {
                var n = f.default('<div class="w-e-menu">\n                <i class="w-e-icon-strikethrough"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                var t = this.editor
                  , e = t.selection.isSelectionEmpty();
                e && t.selection.createEmptyRange(),
                t.cmd.do("strikeThrough"),
                e && (t.selection.collapseRange(),
                t.selection.restoreSelection())
            }
            ,
            e.prototype.tryChangeActive = function() {
                this.editor.cmd.queryCommandState("strikeThrough") ? this.active() : this.unActive()
            }
            ,
            e
        }(s.default);
        e.default = d
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(19))
          , f = c(n(2))
          , d = c(n(302))
          , p = function(t) {
            function e(e) {
                var n = this
                  , o = f.default('<div class="w-e-menu">\n                <i class="w-e-icon-font"></i>\n            </div>')
                  , r = {
                    width: 100,
                    title: "设置字体",
                    type: "list",
                    list: new d.default(e.config.fontNames).getItemList(),
                    clickHandler: function(t) {
                        n.command(t)
                    }
                };
                return n = t.call(this, o, e, r) || this
            }
            return l(e, t),
            e.prototype.command = function(t) {
                this.editor.cmd.do("fontName", t)
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(s.default);
        e.default = p
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = i(n(2))
          , u = function() {
            function t(t) {
                var e = this;
                this.itemList = [],
                (0,
                r.default)(t).call(t, (function(t) {
                    e.itemList.push({
                        $elem: a.default("<p style=\"font-family:'" + t + "'\">" + t + "</p>"),
                        value: t
                    })
                }
                ))
            }
            return t.prototype.getItemList = function() {
                return this.itemList
            }
            ,
            t
        }();
        e.default = u
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(19))
          , f = c(n(2))
          , d = c(n(304))
          , p = function(t) {
            function e(e) {
                var n = this
                  , o = f.default('<div class="w-e-menu">\n                <i class="w-e-icon-text-heigh"></i>\n            </div>')
                  , r = {
                    width: 160,
                    title: "设置字号",
                    type: "list",
                    list: new d.default(e.config.fontSizes).getItemList(),
                    clickHandler: function(t) {
                        n.command(t)
                    }
                };
                return n = t.call(this, o, e, r) || this
            }
            return l(e, t),
            e.prototype.command = function(t) {
                this.editor.cmd.do("fontSize", t)
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(s.default);
        e.default = p
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(2))
          , i = function() {
            function t(t) {
                for (var e in this.itemList = [],
                t) {
                    var n = t[e];
                    this.itemList.push({
                        $elem: r.default('<p style="font-size:' + e + '">' + n.name + "</p>"),
                        value: n.value
                    })
                }
            }
            return t.prototype.getItemList = function() {
                return this.itemList
            }
            ,
            t
        }();
        e.default = i
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(19))
          , f = c(n(2))
          , d = function(t) {
            function e(e) {
                var n = this
                  , o = f.default('<div class="w-e-menu"><i class="w-e-icon-paragraph-left"></i></div>')
                  , r = {
                    width: 100,
                    title: "对齐方式",
                    type: "list",
                    list: [{
                        $elem: f.default('<p>\n                            <i class="w-e-icon-paragraph-left w-e-drop-list-item"></i>\n                            ' + e.i18next.t("menus.dropListMenu.justify.靠左") + "\n                        </p>"),
                        value: "justifyLeft"
                    }, {
                        $elem: f.default('<p>\n                            <i class="w-e-icon-paragraph-center w-e-drop-list-item"></i>\n                            ' + e.i18next.t("menus.dropListMenu.justify.居中") + "\n                        </p>"),
                        value: "justifyCenter"
                    }, {
                        $elem: f.default('<p>\n                            <i class="w-e-icon-paragraph-right w-e-drop-list-item"></i>\n                            ' + e.i18next.t("menus.dropListMenu.justify.靠右") + "\n                        </p>"),
                        value: "justifyRight"
                    }],
                    clickHandler: function(t) {
                        n.command(t)
                    }
                };
                return n = t.call(this, o, e, r) || this
            }
            return l(e, t),
            e.prototype.command = function(t) {
                var e = this.editor
                  , n = e.selection.getSelectionContainerElem();
                n && e.$textElem.equal(n) || e.cmd.do(t, t)
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(s.default);
        e.default = d
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(2))
          , f = n(9)
          , d = function(t) {
            function e(e) {
                var n = s.default('<div class="w-e-menu">\n                <i class="w-e-icon-quotes-left"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                var t = this.editor
                  , e = t.selection.isSelectionEmpty()
                  , n = t.selection.getSelectionContainerElem()
                  , o = n.getNodeName();
                if (e && t.selection.createEmptyRange(),
                f.UA.isIE()) {
                    var r = void 0;
                    if ("P" === o)
                        return r = n.text(),
                        s.default("<blockquote>" + r + "</blockquote>").insertAfter(n),
                        void n.remove();
                    "BLOCKQUOTE" === o && (r = n.text(),
                    s.default("<p>" + r + "</p>").insertAfter(n),
                    n.remove())
                } else
                    "BLOCKQUOTE" === o ? t.cmd.do("formatBlock", "<p>") : t.cmd.do("formatBlock", "<blockquote>");
                e && (t.selection.collapseRange(),
                t.selection.restoreSelection())
            }
            ,
            e.prototype.tryChangeActive = function() {
                "blockquote" === this.editor.cmd.queryCommandValue("formatBlock") ? this.active() : this.unActive()
            }
            ,
            e
        }(c(n(24)).default);
        e.default = d
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(39)), a = r(n(1)), u = r(n(3)), l = r(n(5)), c = (o = function(t, e) {
            return (o = l.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            u.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), s = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        a.default)(e, "__esModule", {
            value: !0
        });
        var f = s(n(19))
          , d = s(n(2))
          , p = function(t) {
            function e(e) {
                var n, o = this, r = d.default('<div class="w-e-menu">\n                <i class="w-e-icon-paint-brush"></i>\n            </div>'), a = {
                    width: 120,
                    title: "背景颜色",
                    type: "inline-block",
                    list: (0,
                    i.default)(n = e.config.colors).call(n, (function(t) {
                        return {
                            $elem: d.default('<i style="color:' + t + ';" class="w-e-icon-paint-brush"></i>'),
                            value: t
                        }
                    }
                    )),
                    clickHandler: function(t) {
                        o.command(t)
                    }
                };
                return o = t.call(this, r, e, a) || this
            }
            return c(e, t),
            e.prototype.command = function(t) {
                this.editor.cmd.do("backColor", t)
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(f.default);
        e.default = p
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(39)), a = r(n(1)), u = r(n(3)), l = r(n(5)), c = (o = function(t, e) {
            return (o = l.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            u.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), s = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        a.default)(e, "__esModule", {
            value: !0
        });
        var f = s(n(19))
          , d = s(n(2))
          , p = function(t) {
            function e(e) {
                var n, o = this, r = d.default('<div class="w-e-menu">\n                <i class="w-e-icon-pencil2"></i>\n            </div>'), a = {
                    width: 120,
                    title: "文字颜色",
                    type: "inline-block",
                    list: (0,
                    i.default)(n = e.config.colors).call(n, (function(t) {
                        return {
                            $elem: d.default('<i style="color:' + t + ';" class="w-e-icon-pencil2"></i>'),
                            value: t
                        }
                    }
                    )),
                    clickHandler: function(t) {
                        o.command(t)
                    }
                };
                return o = t.call(this, r, e, a) || this
            }
            return c(e, t),
            e.prototype.command = function(t) {
                this.editor.cmd.do("foreColor", t)
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(f.default);
        e.default = p
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(2))
          , f = c(n(26))
          , d = c(n(30))
          , p = c(n(310))
          , A = function(t) {
            function e(e) {
                var n = s.default('<div class="w-e-menu">\n                <i class="w-e-icon-play"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                this.createPanel("")
            }
            ,
            e.prototype.createPanel = function(t) {
                var e = p.default(this.editor, t);
                new f.default(this,e).create()
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(d.default);
        e.default = A
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(23))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = n(9)
          , u = i(n(2));
        e.default = function(t, e) {
            var n = a.getRandom("input-iframe")
              , o = a.getRandom("btn-ok");
            return {
                width: 300,
                height: 0,
                tabs: [{
                    title: t.i18next.t("menus.panelMenus.video.插入视频"),
                    tpl: '<div>\n                        <input \n                            id="' + n + '" \n                            type="text" \n                            class="block" \n                            placeholder="' + t.i18next.t("如") + '：<iframe src=... ></iframe>"/>\n                        </td>\n                        <div class="w-e-button-container">\n                            <button id="' + o + '" class="right">\n                                ' + t.i18next.t("插入") + "\n                            </button>\n                        </div>\n                    </div>",
                    events: [{
                        selector: "#" + o,
                        type: "click",
                        fn: function() {
                            var e, o = u.default("#" + n), i = (0,
                            r.default)(e = o.val()).call(e);
                            if (i)
                                return function(e) {
                                    t.cmd.do("insertHTML", e + "<p><br></p>")
                                }(i),
                                !0
                        }
                    }]
                }]
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(30))
          , f = c(n(2))
          , d = c(n(312))
          , p = c(n(26))
          , A = c(n(319))
          , v = function(t) {
            function e(e) {
                var n, o = f.default('<div class="w-e-menu"><i class="w-e-icon-image"></i></div>');
                return n = t.call(this, o, e) || this,
                A.default(e),
                n
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                this.createPanel()
            }
            ,
            e.prototype.createPanel = function() {
                var t = d.default(this.editor);
                new p.default(this,t).create()
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(s.default);
        e.default = v
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(23))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = n(9)
          , u = i(n(2))
          , l = i(n(85))
          , c = n(40);
        e.default = function(t) {
            var e = t.config
              , n = new l.default(t)
              , o = a.getRandom("up-trigger-id")
              , i = a.getRandom("up-file-id")
              , s = a.getRandom("input-link-url")
              , f = a.getRandom("btn-link")
              , d = function(e, n) {
                return void 0 === n && (n = "menus.panelMenus.image."),
                t.i18next.t(n + e)
            }
              , p = 1 === e.uploadImgMaxLength ? "" : 'multiple="multiple"'
              , A = [{
                title: d("上传图片"),
                tpl: '<div class="w-e-up-img-container">\n                    <div id="' + o + '" class="w-e-up-btn">\n                        <i class="w-e-icon-upload2"></i>\n                    </div>\n                    <div style="display:none;">\n                        <input id="' + i + '" type="file" ' + p + ' accept="image/jpg,image/jpeg,image/png,image/gif,image/bmp"/>\n                    </div>\n                </div>',
                events: [{
                    selector: "#" + o,
                    type: "click",
                    fn: function() {
                        var t = u.default("#" + i).elems[0];
                        if (!t)
                            return !0;
                        t.click()
                    }
                }, {
                    selector: "#" + i,
                    type: "change",
                    fn: function() {
                        var t = u.default("#" + i).elems[0];
                        if (!t)
                            return !0;
                        var e = t.files;
                        return e.length && n.uploadImg(e),
                        !0
                    }
                }]
            }, {
                title: d("网络图片"),
                tpl: '<div>\n                    <input \n                        id="' + s + '" \n                        type="text" \n                        class="block"\n                        placeholder="' + d("图片链接") + '"/>\n                    </td>\n                    <div class="w-e-button-container">\n                        <button id="' + f + '" class="right">' + d("插入", "") + "</button>\n                    </div>\n                </div>",
                events: [{
                    selector: "#" + f,
                    type: "click",
                    fn: function() {
                        var t, o = u.default("#" + s), i = (0,
                        r.default)(t = o.val()).call(t);
                        if (i && function(t) {
                            var n = !0;
                            c.imgRegex.test(t) || (n = !1);
                            var o = e.linkImgCheck(t);
                            if (void 0 === o)
                                !1 === n && console.log(d("您刚才插入的图片链接未通过编辑器校验", "validate."));
                            else if (!0 === o) {
                                if (!1 !== n)
                                    return !0;
                                alert(d("您插入的网络图片无法识别", "validate.") + "，" + d("请替换为支持的图片类型", "validate.") + "：jpg | png | gif ...")
                            } else
                                alert(o);
                            return !1
                        }(i))
                            return n.insertImg(i),
                            !0
                    }
                }]
            }]
              , v = {
                width: 300,
                height: 0,
                tabs: []
            };
            return window.FileReader && (e.uploadImgShowBase64 || e.uploadImgServer || e.customUploadImg) && v.tabs.push(A[0]),
            e.showLinkImg && v.tabs.push(A[1]),
            v
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(114))
          , i = o(n(6));
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = n(9);
        e.default = function(t, e) {
            var n = new XMLHttpRequest;
            if (n.open("POST", t),
            n.timeout = e.timeout || 1e4,
            n.ontimeout = function() {
                console.error("wangEditor - 请求超时"),
                e.onTimeout && e.onTimeout(n)
            }
            ,
            n.upload && (n.upload.onprogress = function(t) {
                var n = t.loaded / t.total;
                e.onProgress && e.onProgress(n, t)
            }
            ),
            e.headers && (0,
            i.default)(a).call(a, e.headers, (function(t, e) {
                n.setRequestHeader(t, e)
            }
            )),
            n.withCredentials = !!e.withCredentials,
            e.beforeSend) {
                var o = e.beforeSend(n);
                if (o && "object" === (0,
                r.default)(o) && o.prevent)
                    return o.msg
            }
            return n.onreadystatechange = function() {
                if (4 === n.readyState) {
                    var t = n.status;
                    if (!(t < 200 || t >= 300 && t < 400)) {
                        if (t >= 400)
                            return console.error("wangEditor - XHR 报错，状态码 " + t),
                            void (e.onError && e.onError(n));
                        var o, i = n.responseText;
                        if ("object" !== (0,
                        r.default)(i))
                            try {
                                o = JSON.parse(i)
                            } catch (t) {
                                return console.error("wangEditor - 返回结果不是 JSON 格式", i),
                                void (e.onFail && e.onFail(n, i))
                            }
                        else
                            o = i;
                        e.onSuccess(n, o)
                    }
                }
            }
            ,
            n.send(e.formData || null),
            n
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(315))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = i(n(2))
          , u = function() {
            function t(t) {
                this.editor = t,
                this.$textContainer = t.$textContainerElem,
                this.$bar = a.default('<div class="w-e-progress"></div>'),
                this.isShow = !1,
                this.time = 0,
                this.timeoutId = 0
            }
            return t.prototype.show = function(t) {
                var e = this;
                if (!this.isShow) {
                    this.isShow = !0;
                    var n = this.$bar;
                    this.$textContainer.append(n),
                    (0,
                    r.default)() - this.time > 100 && t <= 1 && (n.css("width", 100 * t + "%"),
                    this.time = (0,
                    r.default)());
                    var o = this.timeoutId;
                    o && clearTimeout(o),
                    this.timeoutId = window.setTimeout((function() {
                        e.hide()
                    }
                    ), 500)
                }
            }
            ,
            t.prototype.hide = function() {
                this.$bar.remove(),
                this.isShow = !1,
                this.time = 0,
                this.timeoutId = 0
            }
            ,
            t
        }();
        e.default = u
    }
    , function(t, e, n) {
        t.exports = n(316)
    }
    , function(t, e, n) {
        var o = n(317);
        t.exports = o
    }
    , function(t, e, n) {
        n(318);
        var o = n(12);
        t.exports = o.Date.now
    }
    , function(t, e, n) {
        n(4)({
            target: "Date",
            stat: !0
        }, {
            now: function() {
                return (new Date).getTime()
            }
        })
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(320))
          , i = o(n(321))
          , a = o(n(322))
          , u = o(n(330));
        e.default = function(t) {
            r.default(t),
            i.default(t),
            a.default(t),
            u.default(t)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = n(119)
          , i = o(n(85));
        function a(t, e) {
            if (!function(t, e) {
                var n = t.config
                  , o = n.pasteFilterStyle
                  , i = n.pasteIgnoreImg
                  , a = r.getPasteHtml(e, o, i)
                  , u = r.getPasteText(e);
                return !!a || !!u
            }(e, t)) {
                var n = r.getPasteImgs(t);
                if (n.length)
                    new i.default(e).uploadImg(n)
            }
        }
        e.default = function(t) {
            t.txt.eventHooks.pasteEvents.push((function(e) {
                a(e, t)
            }
            ))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r, i = o(n(85));
        function a(t) {
            var e = t.dataTransfer && t.dataTransfer.files;
            e && e.length && new i.default(r).uploadImg(e)
        }
        e.default = function(t) {
            r = t,
            t.txt.eventHooks.dropEvents.push(a)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(323))
          , i = o(n(61))
          , a = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var u = a(n(2));
        n(328);
        var l, c = n(9), s = function(t, e, n, o, r) {
            t.attr("style", "\n      width:" + e + "px;\n      height:" + n + "px;\n      left:" + o + "px;\n      top:" + r + "px;\n    ")
        }, f = function(t, e) {
            var n = u.default('<div class="w-e-img-drag-mask">\n            <div class="w-e-img-drag-show-size"></div>\n            <div class="w-e-img-drag-rb"></div>\n         </div>');
            return function(t, e, n) {
                e.on("click", (function(t) {
                    t.stopPropagation()
                }
                )),
                e.on("mousedown", ".w-e-img-drag-rb", (function(o) {
                    if (o.preventDefault(),
                    l) {
                        var a = o.clientX
                          , c = o.clientY
                          , f = n.getBoundingClientRect()
                          , d = l.getBoundingClientRect()
                          , p = d.width
                          , A = d.height
                          , v = d.left - f.left
                          , h = d.top - f.top
                          , g = p / A
                          , m = p
                          , y = A
                          , w = u.default(document);
                        w.on("mousemove", E),
                        w.on("mouseup", b),
                        w.on("mouseleave", x)
                    }
                    function x() {
                        w.off("mousemove", E),
                        w.off("mouseup", b)
                    }
                    function E(t) {
                        t.stopPropagation(),
                        t.preventDefault(),
                        m = p + (t.clientX - a),
                        y = A + (t.clientY - c),
                        m / y != g && (y = m / g),
                        m = (0,
                        r.default)(m.toFixed(2)),
                        y = (0,
                        r.default)(y.toFixed(2)),
                        (0,
                        i.default)(e).call(e, ".w-e-img-drag-show-size").text(m.toFixed(2).replace(".00", "") + "px * " + y.toFixed(2).replace(".00", "") + "px"),
                        s(e, m, y, v, h)
                    }
                    function b() {
                        l.attr("width", m + ""),
                        l.attr("height", y + "");
                        var n = l.getBoundingClientRect();
                        s(e, m, y, n.left - f.left, n.top - f.top),
                        x(),
                        t.change()
                    }
                }
                ))
            }(t, n, e),
            n.hide(),
            e.append(n),
            n
        };
        e.default = function(t) {
            var e = t.$textContainerElem
              , n = f(t, e);
            t.txt.eventHooks.imgClickEvents.push((function(t) {
                if (c.UA.isIE())
                    return !1;
                t && (l = t,
                function(t, e) {
                    var n = t.getBoundingClientRect()
                      , o = l.getBoundingClientRect()
                      , a = o.width.toFixed(2)
                      , u = o.height.toFixed(2);
                    (0,
                    i.default)(e).call(e, ".w-e-img-drag-show-size").text(a + "px * " + u + "px"),
                    s(e, (0,
                    r.default)(a), (0,
                    r.default)(u), o.left - n.left, o.top - n.top),
                    e.show()
                }(e, n))
            }
            ));
            var o = function() {
                !function(t) {
                    (0,
                    i.default)(t).call(t, ".w-e-img-drag-mask").hide()
                }(e)
            };
            t.txt.eventHooks.textScrollEvents.push(o),
            t.txt.eventHooks.keyupEvents.push(o),
            t.txt.eventHooks.toolbarClickEvents.push(o),
            t.txt.eventHooks.menuClickEvents.push(o),
            document.onclick = o,
            t.txt.eventHooks.changeEvents.push(o)
        }
    }
    , function(t, e, n) {
        t.exports = n(324)
    }
    , function(t, e, n) {
        var o = n(325);
        t.exports = o
    }
    , function(t, e, n) {
        n(326);
        var o = n(12);
        t.exports = o.parseFloat
    }
    , function(t, e, n) {
        var o = n(4)
          , r = n(327);
        o({
            global: !0,
            forced: parseFloat != r
        }, {
            parseFloat: r
        })
    }
    , function(t, e, n) {
        var o = n(7)
          , r = n(112).trim
          , i = n(81)
          , a = o.parseFloat
          , u = 1 / a(i + "-0") != -1 / 0;
        t.exports = u ? function(t) {
            var e = r(String(t))
              , n = a(e);
            return 0 === n && "-" == e.charAt(0) ? -0 : n
        }
        : a
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(329);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        (e = n(22)(!1)).push([t.i, ".w-e-text-container {\n  overflow: hidden;\n}\n.w-e-img-drag-mask {\n  position: absolute;\n  z-index: 1;\n  border: 1px dashed #ccc;\n  box-sizing: border-box;\n}\n.w-e-img-drag-mask .w-e-img-drag-rb {\n  position: absolute;\n  right: -5px;\n  bottom: -5px;\n  width: 16px;\n  height: 16px;\n  border-radius: 50%;\n  background: #ccc;\n  cursor: se-resize;\n}\n.w-e-img-drag-mask .w-e-img-drag-show-size {\n  min-width: 110px;\n  height: 22px;\n  line-height: 22px;\n  font-size: 14px;\n  color: #999;\n  position: absolute;\n  left: 0;\n  top: 0;\n  background-color: #999;\n  color: #fff;\n  border-radius: 2px;\n  padding: 0 5px;\n}\n", ""]),
        t.exports = e
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r, i, a = o(n(2)), u = o(n(41));
        function l(t) {
            var e = [{
                $elem: a.default("<span class='w-e-icon-trash-o'></span>"),
                onClick: function(t, e) {
                    return t.selection.createRangeByElem(e),
                    t.selection.restoreSelection(),
                    t.cmd.do("delete"),
                    !0
                }
            }];
            (r = new u.default(i,t,e)).create()
        }
        function c() {
            r && (r.remove(),
            r = null)
        }
        e.default = function(t) {
            i = t,
            t.txt.eventHooks.imgClickEvents.push(l),
            t.txt.eventHooks.clickEvents.push(c),
            t.txt.eventHooks.keyupEvents.push(c),
            t.txt.eventHooks.toolbarClickEvents.push(c),
            t.txt.eventHooks.menuClickEvents.push(c),
            t.txt.eventHooks.textScrollEvents.push(c),
            t.txt.eventHooks.imgDragBarMouseDownEvents.push(c),
            t.txt.eventHooks.changeEvents.push(c)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(6)), a = r(n(1)), u = r(n(3)), l = r(n(5)), c = (o = function(t, e) {
            return (o = l.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            u.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), s = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        a.default)(e, "__esModule", {
            value: !0
        });
        var f = s(n(2))
          , d = s(n(19))
          , p = s(n(332))
          , A = function(t) {
            function e(e) {
                var n = this
                  , o = f.default('<div class="w-e-menu">\n                <i class="w-e-icon-indent-increase"></i>\n            </div>')
                  , r = {
                    width: 130,
                    title: "设置缩进",
                    type: "list",
                    list: [{
                        $elem: f.default('<p>\n                            <i class="w-e-icon-indent-increase w-e-drop-list-item"></i>\n                            ' + e.i18next.t("menus.dropListMenu.indent.增加缩进") + "\n                        <p>"),
                        value: "increase"
                    }, {
                        $elem: f.default('<p>\n                            <i class="w-e-icon-indent-decrease w-e-drop-list-item"></i>\n                            ' + e.i18next.t("menus.dropListMenu.indent.减少缩进") + "\n                        <p>"),
                        value: "decrease"
                    }],
                    clickHandler: function(t) {
                        n.command(t),
                        e.undo.afterChange()
                    }
                };
                return n = t.call(this, o, e, r) || this
            }
            return c(e, t),
            e.prototype.command = function(t) {
                var e = this.editor
                  , n = e.selection.getSelectionContainerElem();
                if (n && e.$textElem.equal(n)) {
                    var o = e.selection.getSelectionRangeTopNodes(e);
                    o.length > 0 && (0,
                    i.default)(o).call(o, (function(n) {
                        p.default(f.default(n), t, e)
                    }
                    ))
                } else
                    n && n.length > 0 && (0,
                    i.default)(n).call(n, (function(n) {
                        p.default(f.default(n), t, e)
                    }
                    ));
                e.selection.restoreSelection(),
                this.tryChangeActive()
            }
            ,
            e.prototype.tryChangeActive = function() {
                var t = this.editor
                  , e = t.selection.getSelectionStartElem()
                  , n = f.default(e).getNodeTop(t);
                n.length <= 0 || ("" != n.elems[0].style.paddingLeft ? this.active() : this.unActive())
            }
            ,
            e
        }(d.default);
        e.default = A
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(333))
          , i = o(n(334));
        e.default = function(t, e, n) {
            var o = t.getNodeTop(n);
            /^P$/i.test(o.getNodeName()) && ("increase" === e ? r.default(o) : "decrease" === e && i.default(o))
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(50));
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = function(t) {
            var e = t.elems[0];
            if ("" === e.style.paddingLeft)
                t.css("padding-left", "2em");
            else {
                var n = e.style.paddingLeft
                  , o = (0,
                r.default)(n).call(n, 0, n.length - 2)
                  , i = Number(o) + 2;
                t.css("padding-left", i + "em")
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(50));
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = function(t) {
            var e = t.elems[0];
            if ("" !== e.style.paddingLeft) {
                var n = e.style.paddingLeft
                  , o = (0,
                r.default)(n).call(n, 0, n.length - 2)
                  , i = Number(o) - 2;
                i > 0 ? t.css("padding-left", i + "em") : t.css("padding-left", "")
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(2))
          , f = c(n(30))
          , d = c(n(26))
          , p = c(n(336))
          , A = function(t) {
            function e(e) {
                var n = s.default('<div class="w-e-menu">\n                <i class="w-e-icon-happy"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.createPanel = function() {
                var t = p.default(this.editor);
                new d.default(this,t).create()
            }
            ,
            e.prototype.clickHandler = function() {
                this.createPanel()
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(f.default);
        e.default = A
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(23))
          , i = o(n(106))
          , a = o(n(39))
          , u = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var l = u(n(2));
        e.default = function(t) {
            var e = t.config.emotions;
            function n(t) {
                var e, n, o = [];
                "image" == t.type ? (o = (0,
                a.default)(e = t.content).call(e, (function(t) {
                    return "string" == typeof t ? "" : '<span  title="' + t.alt + '">\n                    <img class="eleImg" style src="' + t.src + '" alt="[' + t.alt + ']">\n                </span>'
                }
                )),
                o = (0,
                i.default)(o).call(o, (function(t) {
                    return "" !== t
                }
                ))) : o = (0,
                a.default)(n = t.content).call(n, (function(t) {
                    return '<span class="eleImg" title="' + t + '">' + t + "</span>"
                }
                ));
                return o.join("").replace(/&nbsp;/g, "")
            }
            return {
                width: 300,
                height: 230,
                tabs: (0,
                a.default)(e).call(e, (function(e) {
                    return {
                        title: t.i18next.t("menus.panelMenus.emoticon." + e.title),
                        tpl: "<div>" + n(e) + "</div>",
                        events: [{
                            selector: ".eleImg",
                            type: "click",
                            fn: function(e) {
                                var n, o, i = l.default(e.target);
                                "IMG" === i.getNodeName() ? n = (0,
                                r.default)(o = i.parent().html()).call(o) : n = "<span>" + i.html() + "</span>";
                                return t.cmd.do("insertHTML", n),
                                !0
                            }
                        }]
                    }
                }
                ))
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(2))
          , f = function(t) {
            function e(e) {
                var n = this
                  , o = s.default('<div class="w-e-menu">\n                <i class="w-e-icon-list2"></i>\n            </div>')
                  , r = {
                    width: 130,
                    title: "序列",
                    type: "list",
                    list: [{
                        $elem: s.default('\n                        <p>\n                            <i class="w-e-icon-list2 w-e-drop-list-item"></i>\n                            ' + e.i18next.t("menus.dropListMenu.list.无序列表") + "\n                        <p>"),
                        value: "insertUnorderedList"
                    }, {
                        $elem: s.default('<p>\n                            <i class="w-e-icon-list-numbered w-e-drop-list-item"></i>\n                            ' + e.i18next.t("menus.dropListMenu.list.有序列表") + "\n                        <p>"),
                        value: "insertOrderedList"
                    }],
                    clickHandler: function(t) {
                        n.command(t)
                    }
                };
                return n = t.call(this, o, e, r) || this
            }
            return l(e, t),
            e.prototype.command = function(t) {
                var e = this.editor
                  , n = e.$textElem;
                if (e.selection.restoreSelection(),
                !e.cmd.queryCommandState(t)) {
                    var o = s.default(e.selection.getSelectionContainerElem())
                      , r = s.default(o.elems[0]).parentUntil("TABLE", o.elems[0]);
                    if (!(r && "TABLE" === s.default(r.elems[0]).getNodeName() || (e.cmd.do(t),
                    "LI" === o.getNodeName() && (o = o.parent()),
                    !1 === /^ol|ul$/i.test(o.getNodeName()) || o.equal(n)))) {
                        var i = o.parent();
                        i.equal(n) || (o.insertAfter(i),
                        i.remove(),
                        e.selection.restoreSelection(),
                        this.tryChangeActive())
                    }
                }
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(c(n(19)).default);
        e.default = f
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(60)), a = r(n(6)), u = r(n(1)), l = r(n(3)), c = r(n(5)), s = (o = function(t, e) {
            return (o = c.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            l.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), f = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        u.default)(e, "__esModule", {
            value: !0
        });
        var d = f(n(19))
          , p = f(n(2))
          , A = f(n(339))
          , v = n(9)
          , h = function(t) {
            function e(e) {
                var n = this
                  , o = p.default('<div class="w-e-menu">\n                    <i class="w-e-icon-row-height"></i>\n                </div>')
                  , r = {
                    width: 100,
                    title: "设置行高",
                    type: "list",
                    list: new A.default(e,e.config.lineHeights).getItemList(),
                    clickHandler: function(t) {
                        e.selection.saveRange(),
                        n.command(t)
                    }
                };
                return n = t.call(this, o, e, r) || this
            }
            return s(e, t),
            e.prototype.command = function(t) {
                var e, n = this, o = window.getSelection ? window.getSelection() : document.getSelection(), r = ["P"], u = this.editor, l = "";
                u.selection.restoreSelection();
                var c = p.default(u.selection.getSelectionContainerElem())
                  , s = p.default(u.selection.getSelectionContainerElem())
                  , f = p.default(u.selection.getSelectionStartElem()).elems[0]
                  , d = ""
                  , A = []
                  , h = "";
                if (c && u.$textElem.equal(c)) {
                    if (v.UA.isIE())
                        return;
                    var g = []
                      , m = []
                      , y = []
                      , w = p.default(u.selection.getSelectionStartElem())
                      , x = p.default(u.selection.getSelectionEndElem())
                      , E = null === (e = u.selection.getRange()) || void 0 === e ? void 0 : e.commonAncestorContainer.childNodes;
                    m.push(this.getDom(w.elems[0])),
                    null == E || (0,
                    a.default)(E).call(E, (function(t, e) {
                        t === n.getDom(w.elems[0]) && g.push(e),
                        t === n.getDom(x.elems[0]) && g.push(e)
                    }
                    ));
                    var b = 0
                      , _ = void 0;
                    for (y.push(this.getDom(w.elems[0])); m[b] !== this.getDom(x.elems[0]); )
                        _ = p.default(m[b].nextElementSibling).elems[0],
                        -1 !== (0,
                        i.default)(r).call(r, p.default(_).getNodeName()) ? (y.push(_),
                        m.push(_)) : m.push(_),
                        b++;
                    if ("P" !== p.default(m[0]).getNodeName()) {
                        b = 0;
                        for (var S = 0; S < m.length; S++)
                            if ("P" === p.default(m[S]).getNodeName()) {
                                b = S;
                                break
                            }
                        if (0 === b)
                            return;
                        for (var M = 0; M !== b; )
                            m.shift(),
                            M++
                    }
                    return this.setRange(m[0], m[m.length - 1]),
                    (0,
                    a.default)(m).call(m, (function(e) {
                        d = e.getAttribute("style"),
                        A = d ? d.split(";") : [],
                        h = "",
                        "P" === p.default(e).getNodeName() ? (h = t ? n.styleProcessing(A) + "line-height:" + t + ";" : n.styleProcessing(A),
                        l = l + "<" + p.default(e).getNodeName().toLowerCase() + ' style="' + h + '">' + e.innerHTML + "</" + p.default(e).getNodeName().toLowerCase() + ">") : (h = n.styleProcessing(A),
                        l = l + "<" + p.default(e).getNodeName().toLowerCase() + ' style="' + h + '">' + e.innerHTML + "</" + p.default(e).getNodeName().toLowerCase() + ">")
                    }
                    )),
                    this.action(l, u),
                    f = s.elems[0],
                    void this.setRange(f.children[g[0]], f.children[g[1]])
                }
                f = this.getDom(f),
                -1 !== (0,
                i.default)(r).call(r, p.default(f).getNodeName()) && (d = f.getAttribute("style"),
                A = d ? d.split(";") : [],
                null == o || o.selectAllChildren(f),
                u.selection.saveRange(),
                t ? (h = d ? this.styleProcessing(A) + "line-height:" + t + ";" : "line-height:" + t + ";",
                l = "<" + p.default(f).getNodeName().toLowerCase() + ' style="' + h + '">' + f.innerHTML + "</" + p.default(f).getNodeName().toLowerCase() + ">",
                "BLOCKQUOTE" === p.default(f).getNodeName() ? p.default(f).css("line-height", t) : this.action(l, u)) : d && (h = this.styleProcessing(A),
                l = "" === h ? "<" + p.default(f).getNodeName().toLowerCase() + ">" + f.innerHTML + "</" + p.default(f).getNodeName().toLowerCase() + ">" : "<" + p.default(f).getNodeName().toLowerCase() + ' style="' + h + '">' + f.innerHTML + "</" + p.default(f).getNodeName().toLowerCase() + ">",
                this.action(l, u)))
            }
            ,
            e.prototype.getDom = function(t) {
                var e = p.default(t).elems[0];
                if (!e.parentNode)
                    return e;
                return e = function t(e, n) {
                    var o = p.default(e.parentNode);
                    return n.$textElem.equal(o) ? e : t(o.elems[0], n)
                }(e, this.editor)
            }
            ,
            e.prototype.action = function(t, e) {
                e.cmd.do("insertHTML", t)
            }
            ,
            e.prototype.styleProcessing = function(t) {
                var e = "";
                return (0,
                a.default)(t).call(t, (function(t) {
                    "" !== t && -1 === (0,
                    i.default)(t).call(t, "line-height") && (e = e + t + ";")
                }
                )),
                e
            }
            ,
            e.prototype.setRange = function(t, e) {
                var n = this.editor
                  , o = window.getSelection ? window.getSelection() : document.getSelection();
                null == o || o.removeAllRanges();
                var r = document.createRange()
                  , i = t
                  , a = e;
                r.setStart(i, 0),
                r.setEnd(a, 1),
                null == o || o.addRange(r),
                n.selection.saveRange(),
                null == o || o.removeAllRanges(),
                n.selection.restoreSelection()
            }
            ,
            e.prototype.tryChangeActive = function() {
                var t = this.editor
                  , e = t.selection.getSelectionContainerElem();
                if (!e || !t.$textElem.equal(e)) {
                    var n = p.default(t.selection.getSelectionStartElem())
                      , o = (n = this.getDom(n.elems[0])).getAttribute("style") ? n.getAttribute("style") : "";
                    o && -1 !== (0,
                    i.default)(o).call(o, "line-height") ? this.active() : this.unActive()
                }
            }
            ,
            e
        }(d.default);
        e.default = h
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = i(n(2))
          , u = function() {
            function t(t, e) {
                var n = this;
                this.itemList = [{
                    $elem: a.default("<span>" + t.i18next.t("默认") + "</span>"),
                    value: ""
                }],
                (0,
                r.default)(e).call(e, (function(t) {
                    n.itemList.push({
                        $elem: a.default("<span>" + t + "</span>"),
                        value: t
                    })
                }
                ))
            }
            return t.prototype.getItemList = function() {
                return this.itemList
            }
            ,
            t
        }();
        e.default = u
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(2))
          , f = function(t) {
            function e(e) {
                var n = s.default('<div class="w-e-menu">\n                <i class="w-e-icon-undo"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                this.editor.undo.undo()
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(c(n(24)).default);
        e.default = f
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(2))
          , f = function(t) {
            function e(e) {
                var n = s.default('<div class="w-e-menu">\n                <i class="w-e-icon-redo"></i>\n            </div>');
                return t.call(this, n, e) || this
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                this.editor.undo.redo()
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(c(n(24)).default);
        e.default = f
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(30))
          , f = c(n(2))
          , d = c(n(343))
          , p = c(n(26))
          , A = c(n(347))
          , v = function(t) {
            function e(e) {
                var n, o = f.default('<div class="w-e-menu"><i class="w-e-icon-table2"></i></div>');
                return n = t.call(this, o, e) || this,
                A.default(e),
                n
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                this.createPanel()
            }
            ,
            e.prototype.createPanel = function() {
                var t = d.default(this.editor);
                new p.default(this,t).create()
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(s.default);
        e.default = v
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = n(9)
          , i = o(n(2));
        n(344);
        var a = o(n(346));
        e.default = function(t) {
            var e = new a.default(t)
              , n = r.getRandom("w-col-id")
              , o = r.getRandom("w-row-id")
              , u = r.getRandom("btn-link")
              , l = "menus.panelMenus.table."
              , c = function(e) {
                return t.i18next.t(e)
            }
              , s = [{
                title: c(l + "插入表格"),
                tpl: '<div>\n                    <div class="w-e-table">\n                        <span>' + c("创建") + '</span>\n                        <input id="' + o + '"  type="text" class="w-e-table-input" value="5"/></td>\n                        <span>' + c(l + "行") + '</span>\n                        <input id="' + n + '" type="text" class="w-e-table-input" value="5"/></td>\n                        <span>' + (c(l + "列") + c(l + "的") + c(l + "表格")) + '</span>\n                    </div>\n                    <div class="w-e-button-container">\n                        <button id="' + u + '" class="right">' + c("插入") + "</button>\n                    </div>\n                </div>",
                events: [{
                    selector: "#" + u,
                    type: "click",
                    fn: function() {
                        var t = Number(i.default("#" + n).val())
                          , r = Number(i.default("#" + o).val());
                        return t && r && e.createAction(r, t),
                        !0
                    }
                }]
            }]
              , f = {
                width: 330,
                height: 0,
                tabs: []
            };
            return f.tabs.push(s[0]),
            f
        }
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(345);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        (e = n(22)(!1)).push([t.i, ".w-e-table {\n  display: flex;\n}\n.w-e-table .w-e-table-input {\n  width: 40px;\n  text-align: center!important;\n  margin: 0 5px;\n}\n", ""]),
        t.exports = e
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(2))
          , i = function() {
            function t(t) {
                this.editor = t
            }
            return t.prototype.createAction = function(t, e) {
                var n = this.editor
                  , o = r.default(n.selection.getSelectionContainerElem())
                  , i = r.default(o.elems[0]).parentUntil("UL", o.elems[0])
                  , a = r.default(o.elems[0]).parentUntil("OL", o.elems[0]);
                if (!i && !a) {
                    var u = this.createTableHtml(t, e);
                    n.cmd.do("insertHTML", u)
                }
            }
            ,
            t.prototype.createTableHtml = function(t, e) {
                for (var n = "", o = "", r = 0; r < t; r++) {
                    o = "";
                    for (var i = 0; i < e; i++)
                        o += 0 === r ? "<th></th>" : "<td></td>";
                    n = n + "<tr>" + o + "</tr>"
                }
                return '<table border="0" width="100%" cellpadding="0" cellspacing="0"><tbody>' + n + "</tbody></table><p><br></p>"
            }
            ,
            t
        }();
        e.default = i
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(348));
        e.default = function(t) {
            r.default(t)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r, i, a = o(n(2)), u = o(n(41)), l = o(n(349)), c = o(n(350));
        function s(t) {
            var e = new c.default(i)
              , n = function(t, e) {
                return void 0 === e && (e = "menus.panelMenus.table."),
                i.i18next.t(e + t)
            }
              , o = [{
                $elem: a.default("<span>" + n("删除表格") + "</span>"),
                onClick: function(t, e) {
                    return t.selection.createRangeByElem(e),
                    t.selection.restoreSelection(),
                    t.cmd.do("insertHTML", "<p><br></p>"),
                    !0
                }
            }, {
                $elem: a.default("<span>" + n("添加行") + "</span>"),
                onClick: function(t, n) {
                    if (d(t))
                        return !0;
                    var o = a.default(t.selection.getSelectionStartElem())
                      , r = e.getRowNode(o.elems[0]);
                    if (!r)
                        return !0;
                    var i = Number(e.getCurrentRowIndex(n.elems[0], r))
                      , u = e.getTableHtml(n.elems[0])
                      , c = e.getTableHtml(l.default.ProcessingRow(a.default(u), i).elems[0]);
                    return t.selection.createRangeByElem(n),
                    t.selection.restoreSelection(),
                    t.cmd.do("insertHTML", c),
                    !0
                }
            }, {
                $elem: a.default("<span>" + n("删除行") + "</span>"),
                onClick: function(t, n) {
                    if (d(t))
                        return !0;
                    var o = a.default(t.selection.getSelectionStartElem())
                      , r = e.getRowNode(o.elems[0]);
                    if (!r)
                        return !0;
                    var i = Number(e.getCurrentRowIndex(n.elems[0], r))
                      , u = e.getTableHtml(n.elems[0])
                      , c = l.default.DeleteRow(a.default(u), i).elems[0].childNodes[0].childNodes.length
                      , s = "";
                    return t.selection.createRangeByElem(n),
                    t.selection.restoreSelection(),
                    s = 0 === c ? "<p><br></p>" : e.getTableHtml(l.default.DeleteRow(a.default(u), i).elems[0]),
                    t.cmd.do("insertHTML", s),
                    !0
                }
            }, {
                $elem: a.default("<span>" + n("添加列") + "</span>"),
                onClick: function(t, n) {
                    if (d(t))
                        return !0;
                    var o = a.default(t.selection.getSelectionStartElem())
                      , r = e.getCurrentColIndex(o.elems[0])
                      , i = e.getTableHtml(n.elems[0])
                      , u = e.getTableHtml(l.default.ProcessingCol(a.default(i), r).elems[0]);
                    return t.selection.createRangeByElem(n),
                    t.selection.restoreSelection(),
                    t.cmd.do("insertHTML", u),
                    !0
                }
            }, {
                $elem: a.default("<span>" + n("删除列") + "</span>"),
                onClick: function(t, n) {
                    if (d(t))
                        return !0;
                    var o = a.default(t.selection.getSelectionStartElem())
                      , r = e.getCurrentColIndex(o.elems[0])
                      , i = e.getTableHtml(n.elems[0])
                      , u = l.default.DeleteCol(a.default(i), r).elems[0].childNodes[0].childNodes[0].childNodes.length
                      , c = "";
                    return t.selection.createRangeByElem(n),
                    t.selection.restoreSelection(),
                    c = 1 === u ? "<p><br></p>" : e.getTableHtml(l.default.DeleteCol(a.default(i), r).elems[0]),
                    t.cmd.do("insertHTML", c),
                    !0
                }
            }, {
                $elem: a.default("<span>" + n("设置表头") + "</span>"),
                onClick: function(t, n) {
                    if (d(t))
                        return !0;
                    var o = a.default(t.selection.getSelectionStartElem())
                      , r = e.getRowNode(o.elems[0]);
                    if (!r)
                        return !0;
                    var i = Number(e.getCurrentRowIndex(n.elems[0], r));
                    0 !== i && (i = 0);
                    var u = e.getTableHtml(n.elems[0])
                      , c = e.getTableHtml(l.default.setTheHeader(a.default(u), i, "th").elems[0]);
                    return t.selection.createRangeByElem(n),
                    t.selection.restoreSelection(),
                    t.cmd.do("insertHTML", c),
                    !0
                }
            }, {
                $elem: a.default("<span>" + n("取消表头") + "</span>"),
                onClick: function(t, n) {
                    var o = a.default(t.selection.getSelectionStartElem())
                      , r = e.getRowNode(o.elems[0]);
                    if (!r)
                        return !0;
                    var i = Number(e.getCurrentRowIndex(n.elems[0], r));
                    0 !== i && (i = 0);
                    var u = e.getTableHtml(n.elems[0])
                      , c = e.getTableHtml(l.default.setTheHeader(a.default(u), i, "td").elems[0]);
                    return t.selection.createRangeByElem(n),
                    t.selection.restoreSelection(),
                    t.cmd.do("insertHTML", c),
                    !0
                }
            }];
            (r = new u.default(i,t,o)).create()
        }
        function f() {
            r && (r.remove(),
            r = null)
        }
        function d(t) {
            var e = t.selection.getSelectionStartElem()
              , n = t.selection.getSelectionEndElem();
            return (null == e ? void 0 : e.elems[0]) !== (null == n ? void 0 : n.elems[0])
        }
        e.default = function(t) {
            i = t,
            t.txt.eventHooks.tableClickEvents.push(s),
            t.txt.eventHooks.clickEvents.push(f),
            t.txt.eventHooks.keyupEvents.push(f),
            t.txt.eventHooks.toolbarClickEvents.push(f),
            t.txt.eventHooks.menuClickEvents.push(f),
            t.txt.eventHooks.textScrollEvents.push(f)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = o(n(107))
          , a = o(n(50))
          , u = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var l = u(n(2));
        function c(t, e) {
            for (; 0 !== t.childNodes.length; )
                t.removeChild(t.childNodes[0]);
            for (var n = 0; n < e.length; n++)
                t.appendChild(e[n])
        }
        function s(t) {
            var e = t.elems[0].childNodes[0];
            return "COLGROUP" === e.nodeName && (e = t.elems[0].childNodes[t.elems[0].childNodes.length - 1]),
            e
        }
        e.default = {
            ProcessingRow: function(t, e) {
                for (var n = s(t), o = (0,
                a.default)(Array.prototype).apply(n.childNodes), r = o[0].childNodes.length, u = document.createElement("tr"), f = 0; f < r; f++) {
                    var d = document.createElement("td");
                    u.appendChild(d)
                }
                return (0,
                i.default)(o).call(o, e + 1, 0, u),
                c(n, o),
                l.default(n.parentNode)
            },
            ProcessingCol: function(t, e) {
                for (var n = s(t), o = (0,
                a.default)(Array.prototype).apply(n.childNodes), u = function(t) {
                    var n, a = [];
                    for ((0,
                    r.default)(n = o[t].childNodes).call(n, (function(t) {
                        a.push(t)
                    }
                    )); 0 !== o[t].childNodes.length; )
                        o[t].removeChild(o[t].childNodes[0]);
                    var u = "TH" !== l.default(a[0]).getNodeName() ? document.createElement("td") : document.createElement("th");
                    (0,
                    i.default)(a).call(a, e + 1, 0, u);
                    for (var c = 0; c < a.length; c++)
                        o[t].appendChild(a[c])
                }, f = 0; f < o.length; f++)
                    u(f);
                return c(n, o),
                l.default(n.parentNode)
            },
            DeleteRow: function(t, e) {
                var n = s(t)
                  , o = (0,
                a.default)(Array.prototype).apply(n.childNodes);
                return (0,
                i.default)(o).call(o, e, 1),
                c(n, o),
                l.default(n.parentNode)
            },
            DeleteCol: function(t, e) {
                for (var n = s(t), o = (0,
                a.default)(Array.prototype).apply(n.childNodes), u = function(t) {
                    var n, a = [];
                    for ((0,
                    r.default)(n = o[t].childNodes).call(n, (function(t) {
                        a.push(t)
                    }
                    )); 0 !== o[t].childNodes.length; )
                        o[t].removeChild(o[t].childNodes[0]);
                    (0,
                    i.default)(a).call(a, e, 1);
                    for (var u = 0; u < a.length; u++)
                        o[t].appendChild(a[u])
                }, f = 0; f < o.length; f++)
                    u(f);
                return c(n, o),
                l.default(n.parentNode)
            },
            setTheHeader: function(t, e, n) {
                for (var o = s(t), u = (0,
                a.default)(Array.prototype).apply(o.childNodes), f = u[e].childNodes, d = document.createElement("tr"), p = function(t) {
                    var e, o = document.createElement(n);
                    (0,
                    r.default)(e = f[t].childNodes).call(e, (function(t) {
                        o.appendChild(t)
                    }
                    )),
                    d.appendChild(o)
                }, A = 0; A < f.length; A++)
                    p(A);
                return (0,
                i.default)(u).call(u, e, 1, d),
                c(o, u),
                l.default(o.parentNode)
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = i(n(2))
          , u = function() {
            function t(t) {
                this.editor = t
            }
            return t.prototype.getRowNode = function(t) {
                var e, n = a.default(t).elems[0];
                return n.parentNode ? n = null === (e = a.default(n).parentUntil("TR", n)) || void 0 === e ? void 0 : e.elems[0] : n
            }
            ,
            t.prototype.getCurrentRowIndex = function(t, e) {
                var n, o = 0, i = t.childNodes[0];
                return "COLGROUP" === i.nodeName && (i = t.childNodes[t.childNodes.length - 1]),
                (0,
                r.default)(n = i.childNodes).call(n, (function(t, n) {
                    t === e && (o = n)
                }
                )),
                o
            }
            ,
            t.prototype.getCurrentColIndex = function(t) {
                var e, n, o = 0, i = "TD" === a.default(t).getNodeName() || "TH" === a.default(t).getNodeName() ? t : null === (n = a.default(t).parentUntil("TD", t)) || void 0 === n ? void 0 : n.elems[0], u = a.default(i).parent();
                return (0,
                r.default)(e = u.elems[0].childNodes).call(e, (function(t, e) {
                    t === i && (o = e)
                }
                )),
                o
            }
            ,
            t.prototype.getTableHtml = function(t) {
                return '<table border="0" width="100%" cellpadding="0" cellspacing="0">' + a.default(t).html() + "</table>"
            }
            ,
            t
        }();
        e.default = u
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(39)), a = r(n(1)), u = r(n(3)), l = r(n(5)), c = (o = function(t, e) {
            return (o = l.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            u.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), s = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        a.default)(e, "__esModule", {
            value: !0
        }),
        e.formatCodeHtml = void 0;
        var f = s(n(30))
          , d = s(n(2))
          , p = n(9)
          , A = s(n(352))
          , v = s(n(124))
          , h = s(n(26))
          , g = s(n(353));
        e.formatCodeHtml = function(t, e) {
            return e ? (e = function(t) {
                var e = t.match(/<pre[\s|\S]+?\/pre>/g);
                return null === e || (0,
                i.default)(e).call(e, (function(e) {
                    t = t.replace(e, e.replace(/<\/code><code>/g, "\n").replace(/<br>/g, ""))
                }
                )),
                t
            }(e = function t(e) {
                var n, o = e.match(/<span\sclass="hljs[\s|\S]+?\/span>/gm);
                if (!o || !o.length)
                    return e;
                for (var r = (0,
                i.default)(n = p.deepClone(o)).call(n, (function(t) {
                    return (t = t.replace(/<span\sclass="hljs[^>]+>/, "")).replace(/<\/span>/, "")
                }
                )), a = 0; a < o.length; a++)
                    e = e.replace(o[a], r[a]);
                return t(e)
            }(e)),
            e = p.replaceSpecialSymbol(e)) : e
        }
        ;
        var m = function(t) {
            function e(e) {
                var n, o = d.default('<div class="w-e-menu"><i class="w-e-icon-terminal"></i></div>');
                return n = t.call(this, o, e) || this,
                g.default(e),
                n
            }
            return c(e, t),
            e.prototype.insertLineCode = function(t) {
                var e = this.editor
                  , n = d.default("<code>" + t + "</code>");
                e.cmd.do("insertElem", n),
                e.selection.createRangeByElem(n, !1),
                e.selection.restoreSelection()
            }
            ,
            e.prototype.clickHandler = function() {
                var t = this.editor
                  , e = t.selection.getSelectionText();
                this.isActive || (t.selection.isSelectionEmpty() ? this.createPanel("", "") : this.insertLineCode(e))
            }
            ,
            e.prototype.createPanel = function(t, e) {
                var n = A.default(this.editor, t, e);
                new h.default(this,n).create()
            }
            ,
            e.prototype.tryChangeActive = function() {
                var t = this.editor;
                v.default(t) ? this.active() : this.unActive()
            }
            ,
            e
        }(f.default);
        e.default = m
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(39))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var a = n(9)
          , u = i(n(2))
          , l = i(n(124));
        e.default = function(t, e, n) {
            a.getRandom("code");
            var o, i = a.getRandom("input-iframe"), c = a.getRandom("select"), s = (a.getRandom("input-code"),
            a.getRandom("input-text"),
            a.getRandom("btn-ok"));
            function f(e) {
                l.default(t) && function() {
                    if (!l.default(t))
                        return;
                    var e = t.selection.getSelectionStartElem()
                      , n = null == e ? void 0 : e.getNodeTop(t);
                    if (!n)
                        return;
                    t.selection.createRangeByElem(n),
                    t.selection.restoreSelection(),
                    n
                }(),
                t.cmd.do("insertHTML", e);
                var n = t.selection.getSelectionStartElem()
                  , o = null == n ? void 0 : n.getNodeTop(t);
                u.default("<p><br></p>").insertAfter(o)
            }
            var d = function(e) {
                return t.i18next.t(e)
            };
            return {
                width: 500,
                height: 0,
                tabs: [{
                    title: d("menus.panelMenus.code.插入代码"),
                    tpl: '<div>\n                        <select name="" id="' + c + '">\n                            ' + (0,
                    r.default)(o = t.config.languageType).call(o, (function(t) {
                        return "<option " + (n == t ? "selected" : "") + ' value ="' + t + '">' + t + "</option>"
                    }
                    )) + '\n                        </select>\n                        <textarea id="' + i + '" type="text" class="wang-code-textarea" placeholder="" style="height: 160px">' + e.replace(/&quot;/g, '"') + '</textarea>\n                        <div class="w-e-button-container">\n                            <button id="' + s + '" class="right">' + (l.default(t) ? d("修改") : d("插入")) + "</button>\n                        </div>\n                    </div>",
                    events: [{
                        selector: "#" + s,
                        type: "click",
                        fn: function() {
                            var e, n = document.getElementById(i), o = u.default("#" + c).val(), r = n.value;
                            if (e = t.highlight ? t.highlight.highlightAuto(r).value : "<xmp>" + r + "</xmp>",
                            r)
                                return !l.default(t) && (f('<pre type="' + o + '"><code>' + e + "</code></pre>"),
                                !0)
                        }
                    }]
                }]
            }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(354));
        e.default = function(t) {
            r.default(t)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r, i, a = o(n(2)), u = o(n(41));
        function l(t) {
            var e, n, o = [{
                $elem: a.default("<span>" + (e = "删除代码",
                void 0 === n && (n = "menus.panelMenus.code."),
                i.i18next.t(n + e) + "</span>")),
                onClick: function(t, e) {
                    return e.remove(),
                    !0
                }
            }];
            (r = new u.default(i,t,o)).create()
        }
        function c() {
            r && (r.remove(),
            r = null)
        }
        e.default = function(t) {
            i = t,
            t.txt.eventHooks.codeClickEvents.push(l),
            t.txt.eventHooks.clickEvents.push(c),
            t.txt.eventHooks.toolbarClickEvents.push(c),
            t.txt.eventHooks.menuClickEvents.push(c),
            t.txt.eventHooks.textScrollEvents.push(c)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o, r = n(0), i = r(n(1)), a = r(n(3)), u = r(n(5)), l = (o = function(t, e) {
            return (o = u.default || {
                __proto__: []
            }instanceof Array && function(t, e) {
                t.__proto__ = e
            }
            || function(t, e) {
                for (var n in e)
                    e.hasOwnProperty(n) && (t[n] = e[n])
            }
            )(t, e)
        }
        ,
        function(t, e) {
            function n() {
                this.constructor = t
            }
            o(t, e),
            t.prototype = null === e ? (0,
            a.default)(e) : (n.prototype = e.prototype,
            new n)
        }
        ), c = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        i.default)(e, "__esModule", {
            value: !0
        });
        var s = c(n(24))
          , f = c(n(2))
          , d = c(n(356))
          , p = function(t) {
            function e(e) {
                var n, o = f.default('<div class="w-e-menu"><i class="w-e-icon-split-line"></i></div>');
                return n = t.call(this, o, e) || this,
                d.default(e),
                n
            }
            return l(e, t),
            e.prototype.clickHandler = function() {
                var t = this.editor
                  , e = t.selection.getRange()
                  , n = t.selection.getSelectionContainerElem();
                if (n) {
                    var o = f.default(n.elems[0])
                      , r = o.parentUntil("TABLE", n.elems[0])
                      , i = o.children();
                    "CODE" !== o.getNodeName() && (r && "TABLE" === f.default(r.elems[0]).getNodeName() || i && 0 !== i.length && "IMG" === f.default(i.elems[0]).getNodeName() && !(null == e ? void 0 : e.collapsed) || this.createSplitLine())
                }
            }
            ,
            e.prototype.createSplitLine = function() {
                this.editor.cmd.do("insertHTML", "<hr/>")
            }
            ,
            e.prototype.tryChangeActive = function() {}
            ,
            e
        }(s.default);
        e.default = p
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(357));
        e.default = function(t) {
            r.default(t)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r, i, a = o(n(2)), u = o(n(41));
        function l(t) {
            var e = [{
                $elem: a.default("<span>" + i.i18next.t("menus.panelMenus.删除") + "</span>"),
                onClick: function(t, e) {
                    return t.selection.createRangeByElem(e),
                    t.selection.restoreSelection(),
                    t.cmd.do("delete"),
                    !0
                }
            }];
            (r = new u.default(i,t,e)).create()
        }
        function c() {
            r && (r.remove(),
            r = null)
        }
        e.default = function(t) {
            i = t,
            t.txt.eventHooks.splitLineEvents.push(l),
            t.txt.eventHooks.clickEvents.push(c),
            t.txt.eventHooks.keyupEvents.push(c),
            t.txt.eventHooks.toolbarClickEvents.push(c),
            t.txt.eventHooks.menuClickEvents.push(c),
            t.txt.eventHooks.textScrollEvents.push(c)
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(2))
          , i = n(9)
          , a = "1px solid #c9d8db"
          , u = "#FFF"
          , l = "1px solid #EEE";
        e.default = function(t) {
            var e, n, o, c, s = t.toolbarSelector, f = r.default(s), d = t.textSelector, p = t.config.height, A = t.i18next;
            null == d ? (e = r.default("<div></div>"),
            n = r.default("<div></div>"),
            c = f.children(),
            f.append(e).append(n),
            e.css("background-color", u).css("border", a).css("border-bottom", l),
            n.css("border", a).css("border-top", "none").css("height", p + "px")) : (e = f,
            c = (n = r.default(d)).children()),
            (o = r.default("<div></div>")).attr("contenteditable", "true").css("width", "100%").css("height", "100%");
            var v = r.default("<div>" + A.t(t.config.placeholder) + "</div>");
            v.addClass("placeholder"),
            c && c.length ? (o.append(c),
            v.hide()) : o.append(r.default("<p><br></p>")),
            n.append(o),
            n.append(v),
            e.addClass("w-e-toolbar").css("z-index", t.zIndex.get("toolbar")),
            n.addClass("w-e-text-container"),
            n.css("z-index", t.zIndex.get()),
            o.addClass("w-e-text");
            var h = i.getRandom("toolbar-elem");
            e.attr("id", h);
            var g = i.getRandom("text-elem");
            o.attr("id", g);
            var m = n.getClientHeight();
            m !== o.getClientHeight() && o.css("min-height", m + "px"),
            t.$toolbarElem = e,
            t.$textContainerElem = n,
            t.$textElem = o,
            t.toolbarElemId = h,
            t.textElemId = g
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(2));
        e.default = function t(e, n) {
            var o = e.$textElem
              , i = o.children();
            if (!i || !i.length)
                return o.append(r.default("<p><br></p>")),
                void t(e);
            var a = i.last();
            if (n) {
                var u = a.html().toLowerCase()
                  , l = a.getNodeName();
                if ("<br>" !== u && "<br/>" !== u || "P" !== l)
                    return o.append(r.default("<p><br></p>")),
                    void t(e)
            }
            e.config.focus && (e.selection.createRangeByElem(a, !1, !0),
            e.selection.restoreSelection())
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(6))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.changeHandler = void 0;
        var a = n(9)
          , u = i(n(2))
          , l = "";
        function c(t) {
            var e, n = t.config.onchange, o = t.txt.html() || "";
            o.length === l.length && o === l || ((0,
            r.default)(e = t.txt.eventHooks.changeEvents).call(e, (function(t) {
                return t()
            }
            )),
            n(o),
            t.txt.togglePlaceholder(),
            l = o)
        }
        e.changeHandler = c,
        e.default = function(t) {
            !function(t) {
                l = t.txt.html() || "";
                var e = t.$textContainerElem
                  , n = t.$toolbarElem
                  , o = !0;
                e.on("compositionstart", (function() {
                    return o = !1
                }
                )),
                e.on("compositionend", (function() {
                    o = !0,
                    t.change()
                }
                ));
                var r = t.config.onchangeTimeout
                  , i = a.debounce(c, r);
                e.on("click", (function() {
                    o && i(t)
                }
                )).on("keyup", (function() {
                    o && i(t)
                }
                )),
                n.on("click", (function() {
                    i(t)
                }
                ))
            }(t),
            function(t) {
                function e(e) {
                    var n = e.target
                      , o = u.default(n)
                      , r = t.$textElem
                      , i = t.$toolbarElem
                      , a = r.isContain(o)
                      , l = i.isContain(o)
                      , c = i.elems[0] == e.target;
                    if (a)
                        t.isFocus || function(t) {
                            var e = t.config.onfocus
                              , n = t.txt.html() || "";
                            e(n)
                        }(t),
                        t.isFocus = !0;
                    else {
                        if (l && !c)
                            return;
                        t.isFocus && function(t) {
                            var e = t.config.onblur
                              , n = t.txt.html() || "";
                            e(n)
                        }(t),
                        t.isFocus = !1
                    }
                }
                t.isFocus = !1,
                u.default(document).on("click", e),
                t.beforeDestroy((function() {
                    u.default(document).off("click", e)
                }
                ))
            }(t)
        }
    }
    , function(t, e, n) {
        "use strict";
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.default = function(t) {
            var e = t.config
              , n = e.lang
              , o = e.languages;
            if (null == t.i18next)
                t.i18next = {
                    t: function(t) {
                        var e = t.split(".");
                        return e[e.length - 1]
                    }
                };
            else
                try {
                    t.i18next.init({
                        ns: "wangEditor",
                        lng: n,
                        defaultNS: "wangEditor",
                        resources: o
                    })
                } catch (t) {
                    throw new Error("i18next:" + t)
                }
        }
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(61))
          , i = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        }),
        e.setUnFullScreen = e.setFullScreen = void 0;
        var a = i(n(2));
        n(363);
        e.setFullScreen = function(t) {
            var e = a.default(t.toolbarSelector)
              , n = t.$textContainerElem
              , o = t.$toolbarElem
              , i = (0,
            r.default)(o).call(o, "i.w-e-icon-fullscreen")
              , u = t.config;
            i.removeClass("w-e-icon-fullscreen"),
            i.addClass("w-e-icon-fullscreen_exit"),
            e.addClass("w-e-full-screen-editor"),
            e.css("z-index", u.zIndexFullScreen),
            n.css("height", "100%")
        }
        ,
        e.setUnFullScreen = function(t) {
            var e = a.default(t.toolbarSelector)
              , n = t.$textContainerElem
              , o = t.$toolbarElem
              , i = (0,
            r.default)(o).call(o, "i.w-e-icon-fullscreen_exit")
              , u = t.config;
            i.removeClass("w-e-icon-fullscreen_exit"),
            i.addClass("w-e-icon-fullscreen"),
            e.removeClass("w-e-full-screen-editor"),
            e.css("z-index", "auto"),
            n.css("height", u.height + "px")
        }
        ;
        e.default = function(t) {
            if (!t.textSelector && t.config.showFullScreen) {
                var n = t.$toolbarElem
                  , o = a.default('<div class="w-e-menu">\n            <i class="w-e-icon-fullscreen"></i>\n        </div>');
                o.on("click", (function(n) {
                    var o;
                    (0,
                    r.default)(o = a.default(n.currentTarget)).call(o, "i").hasClass("w-e-icon-fullscreen") ? e.setFullScreen(t) : e.setUnFullScreen(t)
                }
                )),
                n.append(o)
            }
        }
    }
    , function(t, e, n) {
        var o = n(21)
          , r = n(364);
        "string" == typeof (r = r.__esModule ? r.default : r) && (r = [[t.i, r, ""]]);
        var i = {
            insert: "head",
            singleton: !1
        };
        o(r, i);
        t.exports = r.locals || {}
    }
    , function(t, e, n) {
        (e = n(22)(!1)).push([t.i, ".w-e-full-screen-editor {\n  position: fixed;\n  width: 100%!important;\n  height: 100%!important;\n  left: 0;\n  top: 0;\n}\n", ""]),
        t.exports = e
    }
    , function(t, e, n) {
        "use strict";
        var o = n(0)
          , r = o(n(83));
        (0,
        o(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var i = function() {
            function t(t) {
                var e = this;
                this.undoString = "",
                this.editor = t,
                this.undoStack = [],
                this.redoStack = [],
                this.flag = !1,
                (0,
                r.default)((function() {
                    var n = t.txt.html();
                    "string" == typeof n && (e.undoString = n)
                }
                ), 0),
                t.txt.eventHooks.changeEvents.push((function() {
                    e.afterChange()
                }
                ))
            }
            return t.prototype.undo = function() {
                var t = this.undoStack.pop()
                  , e = this.editor.config.undoLimit;
                return "string" != typeof t ? "" : (this.flag = !0,
                this.redoStack.push(this.undoString),
                e && this.undoStack.length > e && this.undoStack.shift(),
                this.undoString = t,
                this.editor.txt.html(t),
                t)
            }
            ,
            t.prototype.redo = function() {
                var t = this.redoStack.pop()
                  , e = this.editor.config.undoLimit;
                return "string" != typeof t ? "" : (this.flag = !0,
                this.undoStack.push(this.undoString),
                e && this.redoStack.length > e && this.redoStack.shift(),
                this.undoString = t,
                this.editor.txt.html(t),
                t)
            }
            ,
            t.prototype.afterChange = function() {
                var t = this.editor.txt.html();
                return "string" == typeof t && (this.flag ? (this.flag = !1,
                !1) : (this.undoStack.push(this.undoString),
                this.undoString = t,
                this.redoStack.length = 0,
                void (this.flag = !1)))
            }
            ,
            t
        }();
        e.default = i
    }
    , function(t, e, n) {
        "use strict";
        var o = function(t) {
            return t && t.__esModule ? t : {
                default: t
            }
        };
        (0,
        n(0)(n(1)).default)(e, "__esModule", {
            value: !0
        });
        var r = o(n(118))
          , i = function() {
            function t() {
                this.tier = {
                    menu: 2,
                    panel: 2,
                    toolbar: 1,
                    tooltip: 1
                },
                this.baseZIndex = r.default.zIndex
            }
            return t.prototype.get = function(t) {
                return t && this.tier[t] ? this.baseZIndex + this.tier[t] : this.baseZIndex
            }
            ,
            t.prototype.init = function(t) {
                this.baseZIndex == r.default.zIndex && (this.baseZIndex = t.config.zIndex)
            }
            ,
            t
        }();
        e.default = i
    }
    ]).default
}
));

layui.define(function(exports) {
    exports('wangEditor', window.wangEditor);
});
