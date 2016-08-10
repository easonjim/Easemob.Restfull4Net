using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Easemob.Restfull4Net.Helper
{
    /// <summary>
    /// 日志跟踪
    /// 说明：采用TraceListener
    /// 执行顺序：Open->LogBegin->Log->LogEnd-Close
    /// </summary>
    public static partial class LogTraceHelper
    {
        private static TraceListener _traceListener = null;
        private static readonly object TraceLock = new object();
        private const string DefaultFilePath = "EasemobLogs";

        /// <summary>
        /// 执行所有日志记录操作时执行的任务（发生在LogEnd记录日志之后）
        /// </summary>
        public static Action<string, string> OnLogFunc;

        /// <summary>
        /// 打开日志访问
        /// </summary>
        /// <param name="filePath"></param>
        internal static void Open(string filePath=null)
        {
            Close();
            lock (TraceLock)
            {
                var logDir = string.Concat(System.AppDomain.CurrentDomain.BaseDirectory ,"/", (filePath ?? DefaultFilePath));
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                string logFile = Path.Combine(logDir, string.Format("{0}.log",DateTime.Now.ToString("yyyy-MM-dd")));
                System.IO.TextWriter logWriter = new System.IO.StreamWriter(logFile, true);
                _traceListener = new TextWriterTraceListener(logWriter);
                System.Diagnostics.Trace.Listeners.Add(_traceListener);
                System.Diagnostics.Trace.AutoFlush = true;
            }
        }

        /// <summary>
        /// 关闭日志访问
        /// </summary>
        internal static void Close()
        {
            lock (TraceLock)
            {
                if (_traceListener != null && System.Diagnostics.Trace.Listeners.Contains(_traceListener))
                {
                    _traceListener.Close();
                    System.Diagnostics.Trace.Listeners.Remove(_traceListener);
                }
            }
        }

        /// <summary>
        /// 统一时间格式
        /// </summary>
        private static void TimeLog()
        {
            Log(string.Format("[{0}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff")));
        }

        /// <summary>
        /// 减少一格缩进
        /// </summary>
        private static void Unindent()
        {
            lock (TraceLock)
            {
                System.Diagnostics.Trace.Unindent();
            }
        }

        /// <summary>
        /// 增加一格缩进
        /// </summary>
        private static void Indent()
        {
            lock (TraceLock)
            {
                System.Diagnostics.Trace.Indent();
            }
        }

        /// <summary>
        /// 刷新输出缓冲区
        /// </summary>
        private static void Flush()
        {
            lock (TraceLock)
            {
                System.Diagnostics.Trace.Flush();
            }
        }

        /// <summary>
        /// 开始写日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="logFilePath">日志文件存放文件夹</param>
        private static void LogBegin(string title = null,string logFilePath = null)
        {
            Open(logFilePath);
            Log("");
            if (title != null)
            {
                Log(string.Format("[{0}]", title));
            }
            TimeLog();
            Indent();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">内容</param>
        private static void Log(string message)
        {
            lock (TraceLock)
            {
                System.Diagnostics.Trace.WriteLine(message);
            }
        }

        /// <summary>
        /// 结束写日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="type">类型</param>
        /// <param name="message">内容</param>
        private static void LogEnd(string title = null,string type=null,string message = null)
        {
            Unindent();
            if (title != null)
            {
                Log(string.Format("[{0}]", title));
            }
            Log("");
            Flush();
            Close();

            if (OnLogFunc != null)
            {
                OnLogFunc(type,message);
            }
        }

        /// <summary>
        /// 日志输出统一方法
        /// </summary>
        /// <param name="path">文本路径，如URL，程序集地址等</param>
        /// <param name="content">内容</param>
        /// <param name="type">类型</param>
        /// <param name="startTitle">开始的标题</param>
        /// <param name="endTitle">结束的标题</param>
        /// <param name="logFilePath">日志存放文件夹</param>
        /// <param name="isDebug">是否为Debug，默认为false</param>
        public static void SendLog(string content, string path = null, string type = null, string startTitle = null, string endTitle = null, string logFilePath = null, bool isDebug = false)
        {
            if (isDebug)
            {
                LogBegin(startTitle ?? "日志输出开始", logFilePath);
                Log(string.Format("Path：\r\n\t{0}", path ?? "Null"));
                Log(string.Format("Content：\r\n\t{0}", content));
                LogEnd(endTitle ?? "日志输出结束", type, content);
            }
        }
    }
}
