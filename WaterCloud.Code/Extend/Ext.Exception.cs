﻿using System;

namespace WaterCloud.Code
{
	public static partial class Extensions
	{
		public static Exception GetOriginalException(this Exception ex)
		{
			if (ex.InnerException == null) return ex;

			return ex.InnerException.GetOriginalException();
		}
	}
}