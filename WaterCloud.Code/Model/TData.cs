﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code.Model
{
    /// <summary>
    /// 数据传输对象
    /// </summary>
    public class TData
    {
        /// <summary>
        /// 操作结果，Tag为1代表成功，0代表失败，其他的验证返回结果，可根据需要设置
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// 提示信息或异常信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 扩展Message
        /// </summary>
        public string Description { get; set; }
    }

    public class TData<T> : TData
    {
        /// <summary>
        /// 列表的记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Result { get; set; }

    }

}
