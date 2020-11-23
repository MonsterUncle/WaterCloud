﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WaterCloud.Code;

namespace WaterCloud.Web
{
    /// <summary>
    /// 模型验证过滤器
    /// </summary>
    public class ModelActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string message = "";
            if (!context.ModelState.IsValid)
            {
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        message = message += error.ErrorMessage + "|";
                    }
                }
                if (message.Length>0)
                {
                    message = message.Substring(0, message.Length - 1);
                }
                context.Result = new JsonResult(new AjaxResult { state = ResultType.error.ToString(), message = message });
            } 
        }
    }
}