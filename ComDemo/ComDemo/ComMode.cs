using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComDemo
{
    public class SerialPortEventArgs : EventArgs
    {
        public Byte[] receivedBytes = null;
    }
    public delegate void SerialPortEventHandler(Object sender, SerialPortEventArgs e);


    public class ComMode
    {
        private static readonly ComMode instance = new ComMode();

        //构造函数
        public ComMode()
        {

        }

        public static ComMode GetInstance()
        {
            return instance;
        }

        /*********************************************************
        * LOG显示相关函数
        * ******************************************************/
        public void Log(String str)
        {
            Logger logger = Logger.GetInstance();
            logger.WriteLog(LogLevel.Debug, str + "\r\n", true);
        }

        public void LogError(String str)
        {
            Logger logger = Logger.GetInstance();
            logger.WriteLog(LogLevel.Error, str + "\r\n", true);
        }

        /*************************串口驱动代码*****************************/
        //串口设备
        public SerialPort sp = new SerialPort();

        public event SerialPortEventHandler comReceiveDataEvent = null;

        //创建锁，用于保证数据完整性
        private Object thisLock = new Object();


        //发送数据
        public bool Send(Byte[] bytes)
        {
            if (!sp.IsOpen)
            {
                return false;
            }

            try
            {
                sp.Write(bytes, 0, bytes.Length);
            }
            catch (System.Exception)
            {
                return false;   //write failed
            }

            return true;        //write successfully
        }

        //接收数据处理
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //判断串口是否有数据可读
            //BytesToRead为获取接收缓冲区中数据的字节数 
            if (sp.BytesToRead <= 0)
            {
                return;
            }

            //通过互斥锁保证数据读的完整性
            lock (thisLock)
            {
                int len = 0, tempLen = 0;
                int RxBufLen = 4096;    //当作宏定义，接收缓冲区最大长度
                Byte[] data = new Byte[RxBufLen];

                try
                {
                    tempLen = sp.BytesToRead;
                    while (0 != tempLen)
                    {
                        Byte[] tempBuf = new Byte[tempLen];
                        //从 SerialPort 输入缓冲区中读取
                        sp.Read(tempBuf, 0, tempLen);
                        if (len + tempLen <= RxBufLen)
                        {
                            tempBuf.CopyTo(data, len);
                            len += tempLen;
                        }
                        //当次把数据读完
                        tempLen = sp.BytesToRead;
                    }
                }
                catch (System.Exception)
                {
                    //catch read exception
                }

                if (len > 0)
                {
                    //new 委托对象
                    SerialPortEventArgs args = new SerialPortEventArgs();
                    //保证传过去的数据无无效的脏数据
                    Byte[] dataValid = new Byte[len];
                    Array.Copy(data, dataValid, len);
                    args.receivedBytes = dataValid;
                    //通知应用解析数据
                    if (comReceiveDataEvent != null)
                    {
                        comReceiveDataEvent.Invoke(this, args);
                    }
                }
            }
        }

        //打开串口
        public bool Open(string portName, String baudRate)
        {
            //获取一个值，该值指示 SerialPort 对象的打开或关闭状态
            if (sp.IsOpen)
            {
                //重复执行开启操作时，检测到打开的情况下，先执行关闭操作，然后再执行开启操作
                Close();
            }
            //设置串口参数
            sp.PortName = portName;
            sp.BaudRate = Convert.ToInt32(baudRate);
            sp.DataBits = 8;
            sp.RtsEnable = true;
            sp.DtrEnable = true;

            try
            {
                sp.StopBits = StopBits.One;
                sp.Parity = Parity.None;
                sp.Handshake = Handshake.None;
                //获取或设置写入操作未完成时发生超时之前的毫秒数
                sp.WriteTimeout = 1000;
                //打开串口
                sp.Open();
                sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        //关闭串口
        public bool Close()
        {
            try
            {
                if (sp.IsOpen)
                {
                    sp.DataReceived -= DataReceived;
                    //重复执行开启操作时，检测到打开的情况下，先执行关闭操作，然后再执行开启操作
                    sp.Close(); //close the serial port
                }
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /*************************串口发送接口*****************************/
        //界面显示List的数据结构
        public struct comTxData_t
        {
            public Byte[] data;
        };

        //用于接收数据的节点
        private List<comTxData_t> comTxList = new List<comTxData_t>();
        //创建锁，用于保证数据完整性
        private Object txLock = new Object();
        //对接队列默认值
        private comTxData_t txDataDefault;
        //通知线程处理变量
        public AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        //线程结束标志
        public bool txThdRunOpt = false;

        //发送串口线程初始化
        public void ComTxInit()
        {
            //创建串口发送处理线程
            Thread comTxTd = new Thread(new ThreadStart(ThreadComTx));
            //设置线程为后台线程.(设置成后台线程后,前台主线程关闭,则此后台线程将强制关闭)
            comTxTd.IsBackground = true;
            comTxTd.Start();
        }

        public void ComTxDeinit()
        {
            txThdRunOpt = false;
            //通知线程执行
            _autoResetEvent.Set();
        }


        private void Add(comTxData_t item)
        {
            //增加节点
            try
            {
                lock (txLock)
                {
                    comTxList.Add(item);
                }
            }
            catch (System.Exception)
            {
                //catch read exception
            }
        }

        private void Remove(comTxData_t item)
        {
            //增加节点
            try
            {
                lock (txLock)
                {
                    comTxList.Remove(item);
                }
            }
            catch (System.Exception)
            {
                //catch read exception
            }
        }

        private int IsEmpty()
        {
            //增加节点
            try
            {
                int cnt = 0;
                lock (txLock)
                {
                    cnt = comTxList.Count;
                }
                return cnt;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        private void Clear()
        {
            //增加节点
            try
            {
                lock (txLock)
                {
                    comTxList.Clear();
                }
            }
            catch (System.Exception)
            {

            }
        }

        private comTxData_t GetData()
        {
            if (0 == IsEmpty())
            {
                return txDataDefault;
            }

            //增加节点
            try
            {
                lock (txLock)
                {
                    return comTxList[0];
                }
            }
            catch (System.Exception)
            {
                return txDataDefault;
            }
        }

        //数组发送
        public void ComSend(Byte[] data)
        {
            comTxData_t txData = new comTxData_t();
            txData.data = data;
            this.Add(txData);
            //通知线程执行
            _autoResetEvent.Set();
        }

        //字符串发送
        public void ComSend(String str)
        {
            if (str != null && str != "")
            {
                comTxData_t txData = new comTxData_t();
                txData.data = Encoding.Default.GetBytes(str);
                this.Add(txData);
                //通知线程执行
                _autoResetEvent.Set();
            }
        }

        //接收数据显示处理线程
        private void ThreadComTx()
        {
            try
            {
                //线程初始化执行代码
                txThdRunOpt = true;
                //执行线程运行程序
                while (true)
                {
                    //等待事件通知处理
                    _autoResetEvent.WaitOne();

                    //判断退出线程
                    if (txThdRunOpt == false)
                    {
                        this.Clear();
                        this.Close();
                        break;
                    }

                    //线程执行函数
                    while (0 != this.IsEmpty())
                    {
                        //获取数据
                        comTxData_t txData = this.GetData();
                        if (txData.data.Length != 0)
                        {
                            Send(txData.data);
                        }
                        //删除数据
                        this.Remove(txData);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ComTx " + ex);
            }
            finally
            {
                //Log("ComTx 线程退出");
            }
        }

        /*********************************************************
        * 串口对外操作接口
        *********************************************************/

        public bool DevOpen(string portName, String baudRate)
        {
            if (true == Open(portName, baudRate))
            {
                ComTxInit();
                Log("ComDev Open Success");
                return true;
            }
            else
            {
                LogError("ComDev Open fail");
                return false;
            }
        }

        public bool DevClose()
        {
            ComTxDeinit();
            Log("ComDev Close");
            return true;
        }

        public void ComBufClear()
        {
            Clear();
        }
    }
}
