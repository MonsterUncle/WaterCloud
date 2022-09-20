using System.Collections.Generic;

namespace WaterCloud.Domain
{
	public class MenuInfoEntity
	{
		public string title { get; set; }
		public string href { get; set; }
		public string icon { get; set; }
		public string target { get; set; }
		public List<MenuInfoEntity> child { get; set; }
	}
}