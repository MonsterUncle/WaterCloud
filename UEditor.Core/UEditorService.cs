using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UEditor.Core.Handlers;
using Microsoft.AspNetCore.Hosting;

namespace UEditor.Core
{
    public class UEditorService
    {
        private UEditorActionCollection actionList;

        public UEditorService(IHostingEnvironment env, UEditorActionCollection actions)
        {
            //这里要注意区分Web根目录 和 内容根目录的区别：

            //Web根目录是指提供静态内容的根目录，即asp.net core应用程序根目录下的wwwroot目录
            //内容根目录是指应用程序的根目录，即asp.net core应用的应用程序根目录
            //Config.WebRootPath = env.WebRootPath;
            Config.WebRootPath = env.ContentRootPath;
            actionList = actions;
        }

        public void DoAction(HttpContext context)
        {
            var action = context.Request.Query["action"];
            if (actionList.ContainsKey(action))
                actionList[action].Invoke(context);
            else
                new NotSupportedHandler(context).Process();
        }
    }
}
