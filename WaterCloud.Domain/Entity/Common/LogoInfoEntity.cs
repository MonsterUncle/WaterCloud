namespace WaterCloud.Domain
{
	public class LogoInfoEntity
	{
		public string title { get; set; }
		public string href { get; set; }
		public string image { get; set; }

		public LogoInfoEntity()
		{
			title = "WaterCloud";
			href = "";
			image = "../icon/favicon.ico";
		}
	}
}