using SqlSugar;
using System;

namespace WaterCloud.Domain.InfoManage
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2020-07-29 16:44
	/// 描 述：信息历史实体类
	/// </summary>
	[SugarTable("oms_messagehis")]
	public class MessageHistoryEntity : IEntity<MessageHistoryEntity>, ICreationAudited
	{
		/// <summary>
		/// 主键Id
		/// </summary>
		/// <returns></returns>
		[SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true, ColumnDescription = "主键Id")]
		public string F_Id { get; set; }

		/// <summary>
		/// 信息Id
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_MessageId", ColumnDataType = "nvarchar(50)", ColumnDescription = "信息Id")]
		public string F_MessageId { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
		public DateTime? F_CreatorTime { get; set; }

		/// <summary>
		/// 创建用户主键
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建用户主键")]
		public string F_CreatorUserId { get; set; }

		/// <summary>
		/// 创建用户
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建用户")]
		public string F_CreatorUserName { get; set; }
	}
}