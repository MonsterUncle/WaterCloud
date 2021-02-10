using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using WaterCloud.Code;

namespace WaterCloud.Web
{
	public class AuthorizeAttribute : ValidationAttribute
    {
        /// <summary>
        /// 显示的名称
        /// </summary>
        public string _authorize { get; set; }
        public AuthorizeAttribute(string authorize)
        {
            _authorize = authorize.ToLower();
        }
    }
}