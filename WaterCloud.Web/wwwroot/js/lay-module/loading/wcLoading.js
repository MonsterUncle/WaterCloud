var wc_load_options = {
   time: 1000,
   content: "waterlcloud..."
};

!function (content, options) {
    var load_config = parseInt(sessionStorage.getItem('layuiminiBgcolorId')) || 0;


    function templateFun(options) {
        //默认蓝色
        if (!load_config || load_config == 0 || load_config == 1 || load_config == 5) {
            return "<div class='wc-loading blue_theme'>" +
                "<div class='ball-loader'>" +
                "<span></span><span></span><span></span><span></span>" +
                "</div>" +
                "</div>";
        }
        //绿色
        else if (load_config == 2 || load_config == 3 || load_config == 4) {
            return "<div class='wc-loading'>" +
                "<div class='ball-loader'>" +
                "<span></span><span></span><span></span><span></span>" +
                "</div>" +
                "</div>";
        }
            //橙色
        else {
            return "<div class='wc-loading orange_theme'>" +
                "<div class='ball-loader'>" +
                "<span></span><span></span><span></span><span></span>" +
                "</div>" +
                "</div>";
        }

   }

   function headerInit(content, options) {
      options = options || {};
      if (typeof content == "string") {
         options["content"] = content || wc_load_options.content;
      } else if (typeof content == "object") {
         options = content;
      }
      options.time = options.time || wc_load_options.time;
      options.content = options.content || wc_load_options.content;
      return options;
   }

   wc_load_options = headerInit(content, options);
   var template = templateFun(wc_load_options);
   document.writeln(template);
}();

var wcLoading = {
   close: function (time, dom) {
      time = time || wc_load_options.time;
      dom = dom || document.getElementsByClassName("wc-loading")[0];
      var setTime1 = setTimeout(function () {
         clearTimeout(setTime1);
         dom.classList.add("close");
         var setTime2 = setTimeout(function () {
            clearTimeout(setTime2);
            dom.parentNode.removeChild(dom);/**删除当前节点*/
         }, 800);
      }, time);
   }
};


