using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻类别实体类
    /// </summary>
    [SugarTable("cms_articlecategory")]
    public class ArticleCategoryEntity : IEntity<ArticleCategoryEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage="新闻类别名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "FullName",ColumnDataType = "nvarchar(100)", ColumnDescription = "类别名称", UniqueGroupNameList = new string[] { "cms_articlecategory" })]
        public string FullName { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "新闻类别父级不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "ParentId", ColumnDataType = "nvarchar(50)", ColumnDescription = "父级Id")]
        public string ParentId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        [SugarColumn(IsNullable = false,ColumnDescription = "排序")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Description",ColumnDataType = "longtext", ColumnDescription = "描述")]
        public string Description { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "LinkUrl", ColumnDataType = "longtext", ColumnDescription = "链接地址")]
        public string LinkUrl { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "ImgUrl",ColumnDataType = "longtext", ColumnDescription = "图片地址")]
        public string ImgUrl { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "SeoTitle", ColumnDataType = "longtext", ColumnDescription = "SEO标题")]
        public string SeoTitle { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "SeoKeywords",ColumnDataType = "longtext", ColumnDescription = "SEO关键字")]
        public string SeoKeywords { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "SeoDescription", ColumnDataType = "longtext", ColumnDescription = "SEO描述")]
        public string SeoDescription { get; set; }
        /// <summary>
        /// 是否热门
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否热门")]
        public bool? IsHot { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否热门")]
        public bool? EnabledMark { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除标志")]
        public bool? DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
        public string LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string DeleteUserId { get; set; }
    }
}
