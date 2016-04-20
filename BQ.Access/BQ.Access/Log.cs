using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace BQ.Access
{
    /// <summary>
    /// 日志操作类
    /// </summary>
    public class Log
    {       
        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="isException">是否是出错信息</param>
        public static void WriteLogMessage(string message, bool isException)
        {
            string logFolderPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName + "\\Log\\";//Directory.GetParent(codeBase.Substring(8)).Parent.FullName + "\\Log\\";
            string logFilePath = string.Format("{0}BQ_{1}.log", logFolderPath, DateTime.Now.ToString("yyyy_MM_dd"));
            StreamWriter streamWriter;
            if (!Directory.Exists(logFolderPath))
                Directory.CreateDirectory(logFolderPath);
            //检查日志文件是否存在
            if (File.Exists(logFilePath))
                streamWriter = new StreamWriter(File.Open(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read));
            else
                streamWriter = new StreamWriter(File.Open(logFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read));
            //是否异常日志
            if (isException)
                streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " Error : " + message);
            else
                streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " Normal : " + message);
            streamWriter.WriteLine("------------------------------------------------------------------------------");
            streamWriter.Flush();
            streamWriter.Close();
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="className">类名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="message">消息</param>
        /// <param name="isException">是否异常日志</param>
        public static void WriteLogMessage(string className, string methodName, string message, bool isException)
        {
            string logInfo = string.Format("类名称：{0} 方法名称：{1} 消息：{2}", className, methodName, message);
            WriteLogMessage(logInfo, isException);
        }
    }
}
