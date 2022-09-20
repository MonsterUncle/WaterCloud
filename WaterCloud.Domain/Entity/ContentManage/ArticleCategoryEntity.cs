using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCloud.Domain.ContentManage
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2020-06-09 19:42
	/// 描 述：新闻类别实体类
	/// </summary>
	[SugarTable("cms_articlecategory")]
	public class ArticleCategoryEntity : IEntity<ArticleCategoryEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
	{
		/// <summary>
		/// 主键Id
		/// </summary>
		/// <returns></returns>
		[SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true, ColumnDescription = "主键Id")]
		public string F_Id { get; set; }

		/// <summary>
		/// 类别名称
		/// </summary>
		/// <returns></returns>
		[Required(ErrorMessage = "新闻类别名称不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "F_FullName", ColumnDataType = "nvarchar(100)", ColumnDescription = "类别名称", UniqueGroupNameList = new string[] { "cms_articlecategory" })]
		public string F_FullName { get; set; }

		/// <summary>
		/// 父级Id
		/// </summary>
		/// <returns></returns>
		[Required(ErrorMessage = "新闻类别父级不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "F_ParentId", ColumnDataType = "nvarchar(50)", ColumnDescription = "父级Id")]
		public string F_ParentId { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		/// <returns></returns>
		[Required(ErrorMessage = "排序不能为空")]
		[Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
		[SugarColumn(IsNullable = false, ColumnDescription = "排序")]
		public int? F_SortCode { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "longtext", ColumnDescription = "描述")]
		public string F_Description { get; set; }

		/// <summary>
		/// 链接地址
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_LinkUrl", ColumnDataType = "longtext", ColumnDescription = "链接地址")]
		public string F_LinkUrl { get; set; }

		/// <summary>
		/// 图片地址
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_ImgUrl", ColumnDataType = "longtext", ColumnDescription = "图片地址")]
		public string F_ImgUrl { get; set; }

		/// <summary>
		/// SEO标题
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_SeoTitle", ColumnDataType = "longtext", ColumnDescription = "SEO标题")]
		public string F_SeoTitle { get; set; }

		/// <summary>
		/// SEO关键字
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_SeoKeywords", ColumnDataType = "longtext", ColumnDescription = "SEO关键字")]
		public string F_SeoKeywords { get; set; }

		/// <summary>
		/// SEO描述
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_SeoDescription", ColumnDataType = "longtext", ColumnDescription = "SEO描述")]
		public string F_SeoDescription { get; set; }

		/// <summary>
		/// 是否热门
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnDescription = "是否热门")]
		public bool? F_IsHot { get; set; }

		/// <summary>
		/// 是否启用
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnDescription = "是否热门")]
		public bool? F_EnabledMark { get; set; }

		/// <summary>
		/// 删除标志
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnDescription = "删除标志")]
		public bool? F_DeleteMark { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
		public DateTime? F_CreatorTime { get; set; }

		/// <summary>
		/// 创建人Id
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
		public string F_CreatorUserId { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
		public DateTime? F_LastModifyTime { get; set; }

		/// <summary>
		/// 修改人Id
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
		public string F_LastModifyUserId { get; set; }

		/// <summary>
		/// 删除时间
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
		public DateTime? F_DeleteTime { get; set; }

		/// <summary>
		/// 删除人Id
		/// </summary>
		/// <returns></returns>
		[SugarColumn(IsNullable = true, ColumnName = "F_DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
		public string F_DeleteUserId { get; set; }
	}
}