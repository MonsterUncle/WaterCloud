using System.Collections.Generic;

namespace WaterCloud.Code
{
	public class Flow
	{
		public string title { get; set; }
		public int initNum { get; set; }
		public List<FlowLine> lines { get; set; }
		public List<FlowNode> nodes { get; set; }
		public List<FlowArea> areas { get; set; }
	}
}