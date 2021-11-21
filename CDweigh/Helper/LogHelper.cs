using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDweigh.Helper
{
    class LogHelper
    {
        //数据库日志
        private static readonly log4net.ILog logSql = log4net.LogManager.GetLogger("LogSql");
        //文件日志
        private static readonly log4net.ILog logtxt = log4net.LogManager.GetLogger("loginfo");
        private static readonly log4net.ILog OperateSql = log4net.LogManager.GetLogger("OperateSql");
        public static void Info(string info)
        {

            if (logtxt.IsInfoEnabled)
            {
                logtxt.Info(info);
            }
        }

        public static void Warn(string info)
        {
            if (logSql.IsWarnEnabled)
            {
                logSql.Warn(info);
            }
            if (logtxt.IsWarnEnabled)
            {
                logtxt.Warn(info);
            }
        }


        public static void Error(Exception ex)
        {
            if (logSql.IsErrorEnabled)
            {
                logSql.Error(ex);
            }
            if (logtxt.IsErrorEnabled)
            {
                logtxt.Error(ex);
            }
        }
        public static void Error(object message)
        {
            if (logSql.IsErrorEnabled)
            {
                logSql.Error(message);
            }
            if (logtxt.IsErrorEnabled)
            {
                logtxt.Error(message);
            }
        }
        public static void Error(string info, Exception ex)
        {
            if (logSql.IsErrorEnabled)
            {
                logSql.Error(info, ex);
            }
            if (logtxt.IsErrorEnabled)
            {
                logtxt.Error(info, ex);
            }
        }
        public static void Sql_Info(string info, string user)
        {
            if (OperateSql.IsErrorEnabled)
            {
                OperateSql.Info(user+ info);
            }
        }


    }
    public class message_Log
    {
        string log_operator;
        string log_operation;
        public message_Log(string user, string info)
        {
            this.log_operator = user;
            this.log_operation = info;
        }
    }


}
