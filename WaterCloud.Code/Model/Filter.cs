namespace WaterCloud.Code
{
	public class Filter
	{
		public string Key { get; set; }
		public string Value { get; set; }
		public string Contrast { get; set; }

		public string Text { get; set; }
	}

	public class FilterList
	{
		/// <summary>
		/// and
		/// </summary>
		public string Operation { get; set; }

		public string Filters { get; set; }
		public string Description { get; set; }
	}
}