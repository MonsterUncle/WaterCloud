using Chloe.Infrastructure.Interception;
using System;
using System.Data;

namespace WaterCloud.DataBase
{
    public class DbCommandInterceptor : IDbCommandInterceptor
    {
        /* 执行 DbCommand.ExecuteReader() 时调用 */
        public void ReaderExecuting(IDbCommand command, DbCommandInterceptionContext<IDataReader> interceptionContext)
        {
            interceptionContext.DataBag.Add("startTime", DateTime.Now);
            Console.WriteLine(command.CommandText);
        }
        /* 执行 DbCommand.ExecuteReader() 后调用 */
        public void ReaderExecuted(IDbCommand command, DbCommandInterceptionContext<IDataReader> interceptionContext)
        {
            DateTime startTime = (DateTime)(interceptionContext.DataBag["startTime"]);
            Console.WriteLine(DateTime.Now.Subtract(startTime).TotalMilliseconds);
            if (interceptionContext.Exception == null)
                Console.WriteLine(interceptionContext.Result.FieldCount);
        }

        /* 执行 DbCommand.ExecuteNonQuery() 时调用 */
        public void NonQueryExecuting(IDbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            interceptionContext.DataBag.Add("startTime", DateTime.Now);
            Console.WriteLine(command.CommandText);
        }
        /* 执行 DbCommand.ExecuteNonQuery() 后调用 */
        public void NonQueryExecuted(IDbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            DateTime startTime = (DateTime)(interceptionContext.DataBag["startTime"]);
            Console.WriteLine(DateTime.Now.Subtract(startTime).TotalMilliseconds);
            if (interceptionContext.Exception == null)
                Console.WriteLine(interceptionContext.Result);
        }

        /* 执行 DbCommand.ExecuteScalar() 时调用 */
        public void ScalarExecuting(IDbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            interceptionContext.DataBag.Add("startTime", DateTime.Now);
            Console.WriteLine(command.CommandText);
        }
        /* 执行 DbCommand.ExecuteScalar() 后调用 */
        public void ScalarExecuted(IDbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            DateTime startTime = (DateTime)(interceptionContext.DataBag["startTime"]);
            Console.WriteLine(DateTime.Now.Subtract(startTime).TotalMilliseconds);
            if (interceptionContext.Exception == null)
                Console.WriteLine(interceptionContext.Result);
        }
    }
}
