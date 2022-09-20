using Microsoft.AspNetCore.Http;
using System;

namespace WaterCloud.Code
{
	public static partial class Extensions
	{
		public static bool IsNullOrZero(this object value)
		{
			if (value == null || value.ParseToString().Trim() == "0")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			if (request.Headers != null)
				return request.Headers["X-Requested-With"] == "XMLHttpRequest";
			return false;
		}
	}
}