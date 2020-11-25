using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    /// <summary>
    /// 操作结果的类，只带有成功标志和错误信息 -> The class that operates the result, with only success flags and error messages
    /// </summary>
    /// <remarks>
    /// 当 <see cref="IsSuccess"/> 为 True 时，忽略 <see cref="Message"/> 及 <see cref="ErrorCode"/> 的值
    /// </remarks>
    public class OperateResult
    {

        #region Constructor

        /// <summary>
        /// 实例化一个默认的结果对象
        /// </summary>
        public OperateResult()
        {
        }

        /// <summary>
        /// 使用指定的消息实例化一个默认的结果对象
        /// </summary>
        /// <param name="msg">错误消息</param>
        public OperateResult(string msg)
        {
            this.Message = msg;
        }

        /// <summary>
        /// 使用错误代码，消息文本来实例化对象
        /// </summary>
        /// <param name="err">错误代码</param>
        /// <param name="msg">错误消息</param>
        public OperateResult(int err, string msg)
        {
            this.ErrorCode = err;
            this.Message = msg;
        }

        #endregion
        /// <summary>
        /// 指示本次操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        ///  跳转的连接
        /// </summary>
        public string LinkUrl
        {
            get; set;
        }

        /// <summary>
        /// 具体的错误描述
        /// </summary>
        public string Message
        {
            get; set;
        }

        /// <summary>
        /// 具体的错误代码
        /// </summary>
        public int ErrorCode { get; set; } = 10000;
        /// <summary>
        /// 创建并返回一个成功的结果对象
        /// </summary>
        /// <returns>成功的结果对象</returns>
        public static OperateResult CreateSuccessResult()
        {
            return new OperateResult()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = StringResources.Language.SuccessText,
            };
        }

        /// <summary>
        /// 创建并返回一个成功的结果对象，并带有一个参数对象
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="value">类型的值对象</param>
        /// <returns>成功的结果对象</returns>
        public static OperateResult<T> CreateSuccessResult<T>(T value)
        {
            return new OperateResult<T>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = StringResources.Language.SuccessText,
                Content = value
            };
        }

        /// <summary>
        /// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="result">之前的结果对象</param>
        /// <returns>带默认泛型对象的失败结果类</returns>
        public static OperateResult<T> CreateFailedResult<T>(OperateResult result)
        {
            return new OperateResult<T>()
            {
                ErrorCode = result.ErrorCode,
                Message = result.Message,
            };
        }
        
    }

    /// <summary>
    /// 操作结果的泛型类，允许带一个用户自定义的泛型对象，推荐使用这个类
    /// </summary>
    /// <typeparam name="T">泛型类</typeparam>
    public class OperateResult<T> : OperateResult
    {
        #region Constructor

        /// <summary>
        /// 实例化一个默认的结果对象
        /// </summary>
        public OperateResult() : base()
        {
        }

        /// <summary>
        /// 使用指定的消息实例化一个默认的结果对象
        /// </summary>
        /// <param name="msg">错误消息</param>
        public OperateResult(string msg) : base(msg)
        {

        }

        /// <summary>
        /// 使用错误代码，消息文本来实例化对象
        /// </summary>
        /// <param name="err">错误代码</param>
        /// <param name="msg">错误消息</param>
        public OperateResult(int err, string msg) : base(err, msg)
        {

        }

        #endregion

        /// <summary>
        /// 用户自定义的泛型数据
        /// </summary>
        public T Content { get; set; }
    }

    public  class MesssageString
    {
       
        public virtual string ConnectedFailed => "连接失败：";
        public virtual string UnknownError => "未知错误";
        public virtual string ErrorCode => "错误代号";
        public virtual string TextDescription => "文本描述";
        public virtual string ExceptionMessage => "错误信息：";
        public virtual string ExceptionSourse => "错误源：";
        public virtual string ExceptionType => "错误类型：";
        public virtual string ExceptionStackTrace => "错误堆栈：";
        public virtual string ExceptopnTargetSite => "错误方法：";
        public virtual string ExceprionCustomer => "用户自定义方法出错：";
        public virtual string SuccessText => "成功";
        public virtual string OrderUnderCcondition => "订单未达到条件";
        public virtual string UserIsNotExist => "用户不存在";

        public virtual string OrderStateNotChange => "订单状态不能更改";
        public virtual string OpenIdIsNullOrEmpty => "openid不能为空";
        public virtual string MemberOrTypeIsNotExit => "会员或会员类型不存在";
    }

    public static class StringResources
    {
        public static MesssageString Language = new MesssageString();
    }
}
