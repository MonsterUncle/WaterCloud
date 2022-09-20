using System.Collections.Generic;

namespace WaterCloud.Code
{
	public class FilterSo
	{
		/**
	 * 唯一id
	 */
		public long id { get; set; }
		/**
		 * 前缀 and、or
		 */
		public string prefix { get; set; }
		/**
		 * 模式 in、condition、date
		 */
		public string mode { get; set; }
		/**
		 * 字段名
		 */
		public string field { get; set; }
		/**
		 * 筛选类型
		 */
		public string type { get; set; }
		/**
		 * 是否有分隔符
		 */
		public string split { get; set; }
		/**
		 * 筛选值
		 */
		public string value { get; set; }
		/**
		 * 筛选值
		 */
		public List<string> values { get; set; }

		/**
		 * 子组数据
		 */
		public List<FilterSo> children { get; set; }
	}
}