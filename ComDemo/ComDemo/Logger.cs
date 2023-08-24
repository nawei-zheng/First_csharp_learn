using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComDemo
{
    /* ***********************************************************
     * 该部分是用于实现Log显示统一管理
     * ***********************************************************/

    public enum LogLevel
    {
        Disable,
        Default,
        Debug,
        Info,
        Error,
    }

    public struct logData_t
    {
        public string strData;
        public LogLevel logLevel;
    };

    //用于主线程处理LOG显示信息----主线程需要实现该数据的处理接口
    public class LoggerEventArgs : EventArgs
    {
        public string strData;
        public LogLevel logLevel;
    }
    public delegate void LoggerEventHandler(Object sender, LoggerEventArgs e);


    public class Logger
    {
        private static readonly Logger instance = new Logger();
        private static string logFilePath;
        //用于接收数据的节点
        private static List<logData_t> logList = new List<logData_t>();
        //创建锁，用于保证数据完整性
        private static Object logLock = new Object();
        //对接队列默认值
        private static logData_t logDataDefault;
        //通知线程处理变量
        private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        //通知主线程LOG显示接口
        public event LoggerEventHandler loggerEvent = null;

        public static Logger GetInstance()
        {
            return instance;
        }

        public Logger()
        {
            //判断是否目录存在，若不存在则创建
            string logDir = Path.Combine(Application.StartupPath, "ToolLog");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            logFilePath = Path.Combine(logDir, DateTime.Now.ToString("yyyyMMdd") + ".log");
            //创建线程
            //队列默认值
            logDataDefault.strData = "";
            //创建数据显示处理线程
            Thread logTd = new Thread(new ThreadStart(ThreadLog));
            //设置线程为后台线程.(设置成后台线程后,前台主线程关闭,则此后台线程将强制关闭)
            logTd.IsBackground = true;
            logTd.Start();
        }

        public static void Add(logData_t item)
        {
            //增加节点
            try
            {
                lock (logLock)
                {
                    logList.Add(item);
                }
            }
            catch (System.Exception)
            {
                //catch read exception
            }
        }

        public static void Remove(logData_t item)
        {
            //增加节点
            try
            {
                lock (logLock)
                {
                    logList.Remove(item);
                }
            }
            catch (System.Exception)
            {
                //catch read exception
            }
        }

        public static int IsEmpty()
        {
            //增加节点
            try
            {
                int cnt = 0;
                lock (logLock)
                {
                    cnt = logList.Count;
                }
                return cnt;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static logData_t GetData()
        {
            if (0 == IsEmpty())
            {
                return logDataDefault;
            }

            //增加节点
            try
            {
                lock (logLock)
                {
                    return logList[0];
                }
            }
            catch (System.Exception)
            {
                return logDataDefault;
            }
        }

        //字符串发送
        public static void ComAdd(String data, LogLevel logLevel)
        {
            if (data != null && data != "")
            {
                logData_t logData;
                logData.strData = data;
                logData.logLevel = logLevel;
                Add(logData);
                //通知线程执行
                _autoResetEvent.Set();
            }
        }

        //接收数据显示处理线程
        private void ThreadLog()
        {
            try
            {
                //线程初始化执行代码
                LoggerEventArgs args = new LoggerEventArgs();
                String logStr = "";

                //执行线程运行程序
                while (true)
                {
                    //等待事件通知处理
                    _autoResetEvent.WaitOne();
                    // 不能使用foreach,因为在foreach中删除元素时，每一次删除都会导致集合的大小和元素索引值发生变化，从而导致在foreach中删除元素会出现异常。
                    while (0 != IsEmpty())
                    {
                        //List执行Remove动作后，会重新更新数据，每次取第1个数据即可
                        logData_t logData = GetData();
                        logStr += logData.strData;
                        //同步主线程显示
                        args.strData = logData.strData;
                        args.logLevel = logData.logLevel;
                        if (loggerEvent != null)
                        {
                            loggerEvent.Invoke(this, args);
                        }
                        //删除数据
                        Remove(logData);
                    }
                    LogSave(logStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logger " + ex);
            }
            finally
            {
                Console.WriteLine("Logger Couldn't catch the Thread Exception");
            }
        }

        //保存LOG
        public void LogSave(string logStr)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    //去除尾部的换行符
                    logStr = logStr.TrimEnd("\r\n".ToCharArray());
                    sw.WriteLine(logStr);
                }
            }
            catch
            {
            }
        }

        //调试LOG加入队列
        public void WriteLog(LogLevel level, string logMsg, bool time)
        {
            try
            {
                if (true == time)
                {
                    String logStr;
                    logStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + logMsg;
                    ComAdd(logStr, level);
                }
                else
                {
                    ComAdd(logMsg, level);
                }
            }
            catch
            {
                //Nothing to do
            }
        }
    }
}
