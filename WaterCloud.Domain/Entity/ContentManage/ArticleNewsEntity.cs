using System;
using Chloe.Annotations;

namespace WaterCloud.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻管理实体类
    /// </summary>
    [TableAttribute("cms_articlenews")]
    public class ArticleNewsEntity : IEntity<ArticleNewsEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 文章主键Id
        /// </summary>
        /// <returns></returns>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 类别Id
        /// </summary>
        /// <returns></returns>
        public string F_CategoryId { get; set; }
        /// <summary>
        /// 类别名称（不映射任何列）
        /// </summary>
        /// <returns></returns>
        [NotMappedAttribute]
        public string F_CategoryName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public string F_Title { get; set; }
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
        /// 标签
        /// </summary>
        /// <returns></returns>
        public string F_Tags { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        /// <returns></returns>
        public string F_Zhaiyao { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        public string F_Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public int? F_SortCode { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        /// <returns></returns>
        public bool? F_IsTop { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        /// <returns></returns>
        public bool? F_IsHot { get; set; }
        /// <summary>
        /// 是否热门
        /// </summary>
        /// <returns></returns>
        public bool? F_IsRed { get; set; }
        /// <summary>
        /// 点击次数
        /// </summary>
        /// <returns></returns>
        public int? F_Click { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        /// <returns></returns>
        public string F_Source { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        /// <returns></returns>
        public string F_Author { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 逻辑删除标志
        /// </summary>
        /// <returns></returns>
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        /// <returns></returns>
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        /// <returns></returns>
        public string F_DeleteUserId { get; set; }
    }
}
