using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻管理实体类
    /// </summary>
    [SugarTable("cms_articlenews")]
    public class ArticleNewsEntity : IEntity<ArticleNewsEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 文章主键Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 类别Id
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "新闻类别不能为空")]
        [SugarColumn(IsNullable = false, ColumnName = "CategoryId", ColumnDataType = "nvarchar(50)", ColumnDescription = "类别Id", UniqueGroupNameList = new string[] { "cms_articlenews" })]
        public string CategoryId { get; set; }
        /// <summary>
        /// 类别名称（不映射任何列）
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsIgnore=true)]
        public string CategoryName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "新闻标题不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "Title", ColumnDataType = "nvarchar(200)", ColumnDescription = "标题", UniqueGroupNameList = new string[] { "cms_articlenews" })]
        public string Title { get; set; }
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
        [SugarColumn(IsNullable = true, ColumnName = "SeoKeywords", ColumnDataType = "longtext", ColumnDescription = "SEO关键字")]
        public string SeoKeywords { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "SeoDescription",ColumnDataType = "longtext", ColumnDescription = "SEO描述")]
        public string SeoDescription { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Tags", ColumnDataType = "longtext", ColumnDescription = "标签")]
        public string Tags { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Zhaiyao", ColumnDataType = "longtext", ColumnDescription = "摘要")]
        public string Zhaiyao { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Description",ColumnDataType = "longtext", ColumnDescription = "内容")]
        public string Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        [SugarColumn(IsNullable = true, ColumnDescription = "排序")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否置顶")]
        public bool? IsTop { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否推荐")]
        public bool? IsHot { get; set; }
        /// <summary>
        /// 是否热门
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否热门")]
        public bool? IsRed { get; set; }
        /// <summary>
        /// 点击次数
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "点击次数")]
        public int? Click { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Source",ColumnDataType = "nvarchar(50)", ColumnDescription = "来源")]
        public string Source { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Author",ColumnDataType = "nvarchar(50)", ColumnDescription = "作者")]
        public string Author { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否启用")]
        public bool? EnabledMark { get; set; }
        /// <summary>
        /// 逻辑删除标志
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "逻辑删除标志")]
        public bool? DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "最后修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "最后修改人")]
        public string LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人")]
        public string DeleteUserId { get; set; }
    }
}
