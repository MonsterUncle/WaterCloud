using System;
using System.ComponentModel.DataAnnotations;
using Chloe.Annotations;

namespace WaterCloud.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻类别实体类
    /// </summary>
    [TableAttribute("cms_articlecategory")]
    public class ArticleCategoryEntity : IEntity<ArticleCategoryEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage="新闻类别名称不能为空")]
        public string F_FullName { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "新闻类别父级不能为空")]
        public string F_ParentId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        public int? F_SortCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        public string F_Description { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        /// <returns></returns>
        public string F_LinkUrl { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        /// <returns></returns>
        public string F_ImgUrl { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        /// <returns></returns>
        public string F_SeoTitle { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        /// <returns></returns>
        public string F_SeoKeywords { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        /// <returns></returns>
        public string F_SeoDescription { get; set; }
        /// <summary>
        /// 是否热门
        /// </summary>
        /// <returns></returns>
        public bool? F_IsHot { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        /// <returns></returns>
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_DeleteUserId { get; set; }
    }
}
