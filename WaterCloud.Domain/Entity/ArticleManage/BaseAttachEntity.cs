/*******************************************************************************
 * Copyright © 2018 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud
 * Website：
*********************************************************************************/

using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Chloe.Annotations;

namespace WaterCloud.Entity.BaseDataManage
{
    //Store_Knowledge_Attach
    
    [TableAttribute("Bas_ArticleAttach")]
    public class BaseAttachEntity : IEntityV2<BaseAttachEntity>
    {

        /// <summary>
        /// T_Id
        /// </summary>		
        public string T_Id { get; set; }
        /// <summary>
        /// 文档标题
        /// </summary>		
        public string T_DocId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>		
        public string T_Name { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>		
        public string T_Path { get; set; }
        /// <summary>
        /// 尺寸大小
        /// </summary>		
        public string T_Size { get; set; }
        /// <summary>
        /// 文件类型[image，doc，xls,pdf]
        /// </summary>		
        public string T_Type { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>		
        public string T_Thums { get; set; }
        /// <summary>
        /// 下载次数
        /// </summary>		
        public string T_Down { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? T_CreateTime { get; set; }
    }
}

