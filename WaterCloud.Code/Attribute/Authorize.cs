using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCloud.Code
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class Authorize : Attribute
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string _authorize { get; set; }
        private Authorize()
		{

		}
        public Authorize(string authorize)
        {
            _authorize = authorize.ToLower();
        }
    }
}