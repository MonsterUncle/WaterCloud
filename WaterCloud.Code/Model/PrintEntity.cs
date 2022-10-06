using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code.Model
{
    public class PrintEntity
    {
		public string cmd { get; set; } = "print";
		public string requestId { get; set; }
		public PrintDetail data { get; set; }
	}
	public class PrintDetail
	{
		public PrintInitInfo printIniInfo { get; set; }
		public object data { get; set; }
	}
	public class PrintInitInfo
	{
		public string filePath { get; set; }
		public string realName { get; set; }
		public int? printType { get; set; } = 1;
		public string printName { get; set; } = "";
		public bool landscape { get; set; } = true;
		public string paperSize { get; set; }
		public string duplex { get; set; }
		public bool? isBatch { get; set; } = false;
	}
}
