using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCloud.Domain.SystemManage
{
	/// <summary>
	/// 单据规则模型
	/// </summary>
	public class CodeRuleFormatEntity
	{
		/// <summary>
		/// 编码前缀类型 1-固定参数 2-日期 3-年 4-月 5-日 6-周别 7-周几 8-小时 9-上午下午 10-班别 11-流水号 12-自定义 13-通配符
		/// </summary>
		[Range(1, 15)]
		public int FormatType { get; set; }

		/// <summary>
		/// 格式化字符串
		/// </summary>
		public string FormatString { get; set; }

		/// <summary>
		/// 自定义日,月,周别,周几,班别,上午下午
		/// </summary>
		public string[] DiyDate { get; set; }

		/// <summary>
		/// 流水号初始值，默认：1
		/// </summary>
		public int? InitValue { get; set; } = 1;

		/// <summary>
		/// 流水号最大值
		/// </summary>
		public int? MaxValue { get; set; }

		/// <summary>
		/// 步长
		/// </summary>
		public decimal? Increment { get; set; } = 1;

		/// <summary>
		/// 进制
		/// </summary>
		[Range(2, 36)]
		public int ToBase { get; set; } = 10;
	}
}
