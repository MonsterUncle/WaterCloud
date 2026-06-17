using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    public class LogHelper
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region 写文本日志

        public static void Write(string logContent)
        {
            if (string.IsNullOrWhiteSpace(logContent)) return;
            _logger.Info(logContent);
        }

        public static void WriteWithTime(string logContent)
        {
            if (string.IsNullOrWhiteSpace(logContent)) return;
            _logger.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine + logContent);
        }

        #endregion 写文本日志

        #region 写异常日志

        public static void Write(Exception ex)
        {
            if (ex == null) return;
            _logger.Error(ex, ex.Message);
        }

        public static void WriteWithTime(Exception ex)
        {
            if (ex == null) return;
            var msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine + GetExceptionMessage(ex);
            _logger.Error(ex, msg);
        }

        public static void WriteWithTime(ExceptionContext ex)
        {
            if (ex == null || ex.Exception == null) return;
            var error = ex.Exception;
            var logMessage = new LogMessage();
            logMessage.OperationTime = DateTime.Now;
            logMessage.Url = ex.HttpContext.Request.GetDisplayUrl();
            logMessage.Class = ex.ActionDescriptor?.DisplayName ?? "服务器配置问题";
            logMessage.Ip = WebHelper.Ip;
            logMessage.Host = ex.HttpContext.Request.Host.ToString();
            var current = OperatorProvider.Provider.GetCurrent();
            if (current != null)
                logMessage.UserName = current.UserCode + "（" + current.UserName + "）";
            var err = error.GetOriginalException();
            logMessage.ExceptionInfo = err.Message;
            logMessage.ExceptionSource = err.Source;
            logMessage.ExceptionRemark = err.StackTrace;
            var content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine + ExceptionFormat(logMessage);
            _logger.Error(error, content);
        }

        #endregion 写异常日志

        #region 写日志到指定路径（兼容旧API，底层走NLog）

        public static void Write(string logPath, string logContent)
        {
            if (string.IsNullOrWhiteSpace(logContent)) return;
            _logger.Info("[{0}] {1}", logPath, logContent);
        }

        public static void Write(string logPath, string logFileName, string logContent)
        {
            if (string.IsNullOrWhiteSpace(logContent)) return;
            _logger.Info("[{0}/{1}] {2}", logPath, logFileName, logContent);
        }

        #endregion 写日志到指定路径

        #region 公共工具方法

        public static string ExceptionFormat(LogMessage logMessage)
        {
            var sb = new StringBuilder();
            sb.Append("1. 调试: >> 操作时间: " + logMessage.OperationTime + "   操作人: " + logMessage.UserName + " \r\n");
            sb.Append("2. 地址: " + logMessage.Url + "    \r\n");
            sb.Append("3. 类名: " + logMessage.Class + " \r\n");
            sb.Append("4. 主机: " + logMessage.Host + "   Ip  : " + logMessage.Ip + " \r\n");
            sb.Append("5. 异常: " + logMessage.ExceptionInfo + "\r\n");
            sb.Append("6. 来源: " + logMessage.ExceptionSource + "\r\n");
            sb.Append("7. 实例: " + logMessage.ExceptionRemark + "\r\n");
            sb.Append("-----------------------------------------------------------------------------------------------------------------------------\r\n");
            return sb.ToString();
        }

        public static string ExMsgFormat(string message)
        {
            if (message == null) return null;
            if (message.Contains("An exception occurred while executing DbCommand."))
            {
                if (message.Contains("Duplicate entry '") && message.Contains("key"))
                    message = "数据违反唯一约束，请检查";
                else if (message.Contains("Data too long for column"))
                    message = "数据长度过长，请检查";
                else
                    message = "数据操作异常，请联系管理员";
            }
            else
            {
                if (message.Contains("Object reference not set to an instance of an object."))
                    message = "操作对象为空，请联系管理员";
                else if (message.Contains("Value cannot be null"))
                    message = "值为空，请联系管理员";
            }
            if (!IsHasCHZN(message))
                message = "程序内部异常，请联系管理员";
            return message;
        }

        #endregion 公共工具方法

        #region 私有方法

        private static string GetExceptionMessage(Exception ex)
        {
            if (ex == null) return string.Empty;
            var msg = ex.Message + Environment.NewLine;
            var orig = ex.GetOriginalException();
            if (orig != null && orig.Message != ex.Message)
                msg += orig.Message + Environment.NewLine;
            msg += ex.StackTrace + Environment.NewLine;
            return msg;
        }

        private static bool IsHasCHZN(string inputData)
        {
            return new Regex("[\u4e00-\u9fa5]").Match(inputData).Success;
        }

        #endregion 私有方法
    }
}
