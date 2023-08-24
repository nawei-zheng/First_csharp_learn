using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComDemo
{
    public partial class ComDemo : Form
    {
        public ComDemo()
        {
            InitializeComponent();
        }

        

        /*********************************************************
        * LOG显示相关函数
        * ******************************************************/
        public void LoggerInit()
        {
            Logger logger = Logger.GetInstance();
            //接收数据委托处理匹配界面，升级线程信息同步
            logger.loggerEvent += new LoggerEventHandler(LoggerEventFunc);
        }

        public void LoggerEventFunc(Object sender, LoggerEventArgs e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Invoke(new Action<Object, LoggerEventArgs>(LoggerEventFunc), sender, e);
                }
                catch (System.Exception)
                {
                    //disable form destroy exception
                }
                return;
            }

            try
            {
                //根据LOG等级设定颜色和格式
                System.Drawing.Color clr = new Color();
                switch (e.logLevel)
                {
                    case LogLevel.Error:
                        clr = Color.Red;
                        break;
                    case LogLevel.Info:
                        clr = Color.Blue;
                        break;
                    case LogLevel.Debug:
                        clr = Color.Green;
                        break;
                    case LogLevel.Default:
                        clr = Color.Black;
                        break;
                    default:
                        return;
                    //break;
                }

                rb_Log.SelectionColor = clr;
                rb_Log.AppendText(e.strData);
                rb_Log.ScrollToCaret(); //滚动到底部
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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

        public void Log(Byte[] data)
        {
            Logger logger = Logger.GetInstance();
            logger.WriteLog(LogLevel.Debug, Cmn.bytesToString(data) + "\r\n", true);
        }

        /*********************************************************
        * 串口驱动类操作
        * ******************************************************/
        ComMode ComDev = ComMode.GetInstance();
        public void ComDevInit()
        {
            //接收数据委托处理匹配界面，升级线程信息同步
            ComDev.comReceiveDataEvent += new SerialPortEventHandler(SerialRxEventFunc);
        }

        public void SerialRxEventFunc(Object sender, SerialPortEventArgs e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Invoke(new Action<Object, SerialPortEventArgs>(SerialRxEventFunc), sender, e);
                }
                catch (System.Exception)
                {
                    //disable form destroy exception
                }
                return;
            }

            try
            {
                if (bt_UpdOpt.Text == "取消升级")
                {
                    //串口接收数据处理
                    fwUpd.RxComAdd(e.receivedBytes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*********************************************************
        * 升级界面信息同步操作
        * ******************************************************/
        FwUpd fwUpd = FwUpd.GetInstance();
        public void FwUpdInit()
        {
            //接收数据委托处理匹配界面，升级线程信息同步
            fwUpd.fwUpdStaEvent += new FwUpdStaEventHandler(FwUpdStaEvent);       
        }

        public void FwUpdStaEvent(Object sender, FwUpdStaEventArgs e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Invoke(new Action<Object, FwUpdStaEventArgs>(FwUpdStaEvent), sender, e);
                }
                catch (System.Exception)
                {
                    //disable form destroy exception
                }
                return;
            }

            try
            {
                //TODO
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /*********************************************************
         * 自定义添加函数
         * ******************************************************/

        //串口状态
        public struct comSta_t
        {
            //当前串口连接端口号
            public string LinkPort;
            //当前串口连接状态
            public bool LinkSta;
            //波特率
            public string band;
        };
        public comSta_t gCom = new comSta_t();


        //串口全局变量状态初始化
        public void comStaInit()
        {
            gCom.LinkSta = false;
            gCom.LinkPort = "";
            //更新串口相关显示
            bt_ComOpen.Text = "打开串口";
            //打开串口刷新button
            bt_ComCheck.Enabled = true;
            bt_ComCheck.Enabled = true;
            cb_ComBandList.Enabled = true;
            cb_ComNumList.Enabled = true;
        }

        //检测当前实际串口情况
        public void comPortCheck()
        {
            //获取串口显示清单
            string[] ArrayComPortsNames = SerialPort.GetPortNames();
            //用于表示当前刷写的串口清单中是否有当前连接的串口
            bool comIsValid = false;

            //清除串口显示列表缓存
            cb_ComNumList.Items.Clear();
            //未检测到串口下处理
            if (ArrayComPortsNames.Length == 0)
            {
                //串口列表显示界面清空
                cb_ComNumList.Text = "";
                comStaInit();
            }
            else
            {
                //数据排序，按默认规则排序
                Array.Sort(ArrayComPortsNames);
                //更新串口显示清单
                for (int i = 0; i < ArrayComPortsNames.Length; i++)
                {
                    cb_ComNumList.Items.Add(ArrayComPortsNames[i]);
                    //判断是否存在
                    if (gCom.LinkPort == ArrayComPortsNames[i])
                    {
                        comIsValid = true;
                    }
                }
                //若有串口则显示之前连接串口
                if (comIsValid == true)
                {
                    cb_ComNumList.Text = gCom.LinkPort;
                }
                //若无匹配串口则显示重新遍历的第一个串口
                else
                {
                    cb_ComNumList.Text = ArrayComPortsNames[0];
                    gCom.LinkSta = false;
                }
                gCom.LinkPort = cb_ComNumList.Text;
            }
        }

        //串口开关操作
        public bool comOpenOpt(bool opt)
        {
            //执行打开串口
            if (true == opt)
            {
                if (true == ComDev.DevOpen(cb_ComNumList.Text, cb_ComBandList.Text))
                {
                    gCom.LinkSta = true;
                    return true;
                }
                else
                {
                    gCom.LinkSta = false;
                    return false;
                }
            }
            //执行关闭串口
            else
            {
                gCom.LinkSta = false;
                return ComDev.DevClose();
            }
        }

        //串口数据发送
        public void comSend(string data)
        {
            ComDev.ComSend(data);
        }



        /*********************************************************
         * 控件处理函数
         * ******************************************************/
        void comControlView(bool swi)
        {
            if (false == swi)
            {
                bt_ComOpen.Text = "打开串口";
                bt_ComCheck.Enabled = true;
                cb_ComBandList.Enabled = true;
                cb_ComNumList.Enabled = true;
                bt_UpdOpt.Enabled = false;
            }
            else
            {
                bt_ComOpen.Text = "关闭串口";
                bt_ComCheck.Enabled = false;
                cb_ComBandList.Enabled = false;
                cb_ComNumList.Enabled = false;
                bt_UpdOpt.Enabled = true;
            }
        }

        //当读开的线程不能操作界面控件，需要通过事件通知主线程处理
        public delegate void PortLinkEventHandler(Object sender);
        public event PortLinkEventHandler portLinkEvent = null;

        public void ComPortDisEventFunc(Object sender)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Invoke(new Action<Object>(ComPortDisEventFunc), sender);
                }
                catch (System.Exception)
                {
                    //disable form destroy exception
                }
                return;
            }

            //关闭成功失败均按串口已关闭处理
            comOpenOpt(false);
            comControlView(false);
            comPortCheck();
        }


        public bool GetPortLinkSta()
        {
            //获取串口显示清单
            string[] ArrayComPortsNames = SerialPort.GetPortNames();

            //未检测到串口下处理
            if (ArrayComPortsNames.Length != 0)
            {
                //数据排序，按默认规则排序
                Array.Sort(ArrayComPortsNames);
                //更新串口显示清单
                for (int i = 0; i < ArrayComPortsNames.Length; i++)
                {
                    //判断是否存在
                    if (gCom.LinkPort == ArrayComPortsNames[i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void ThreadComPortCheck()
        {
            try
            {
                while (true)
                {
                    gCom.LinkSta = GetPortLinkSta();
                    if (gCom.LinkSta == false)
                    {
                        portLinkEvent += new PortLinkEventHandler(ComPortDisEventFunc);
                        //事件委托通知
                        if (portLinkEvent != null)
                        {
                            portLinkEvent.Invoke(this);
                        }
                        return;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        public void ComPortCheckInit()
        {
            Thread comPortTd = new Thread(new ThreadStart(ThreadComPortCheck));
            comPortTd.IsBackground = true;
            comPortTd.Start();
        }

        private void bt_ComOpen_Click(object sender, EventArgs e)
        {
            //更新当前串口数
            comPortCheck();
            if (gCom.LinkPort == "")
            {
                bt_ComOpen.Text = "打开串口";
                comControlView(false);
                LogError("找不到串口设备");
                return;
            }

            //执行串口
            if (bt_ComOpen.Text == "打开串口")
            {
                if (true == comOpenOpt(true))
                {
                    comControlView(true);

                    //新建线程实时检测串口是否连接正常
                    ComPortCheckInit();
                }
            }
            else
            {
                //关闭成功失败均按串口已关闭处理
                comOpenOpt(false);
                comControlView(false);
            }
        }

        //软件运行加载项
        private void ComDemo_Load(object sender, EventArgs e)
        {
            //LOG显示初始化
            LoggerInit();
            ComDevInit();
            FwUpdInit();

            //波特率可选列表初始化
            cb_ComBandList.Items.Add(2400);
            cb_ComBandList.Items.Add(4800);
            cb_ComBandList.Items.Add(9600);
            cb_ComBandList.Items.Add(19200);
            cb_ComBandList.Items.Add(38400);
            cb_ComBandList.Items.Add(57600);
            cb_ComBandList.Items.Add(115200);
            cb_ComBandList.Items.Add(460800);
            cb_ComBandList.Items.Add(1000000);
            cb_ComBandList.Items.ToString();
            //设置默认显示的波特率
            cb_ComBandList.Text = cb_ComBandList.Items[6].ToString();

            //开机串口全局变量初始化
            comStaInit();
            //搜索当前电脑有多少个串口
            comPortCheck();
        }

        private void bt_ComCheck_Click(object sender, EventArgs e)
        {
            //更新当前串口数
            comPortCheck();
        }


        private void bt_LoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                string path = tb_FilePath.Text;
                OpenFileDialog file = new OpenFileDialog();
                //格式限制
                string dirPath;
                try
                {
                    dirPath = Path.GetDirectoryName(path);
                    if (false == Directory.Exists(dirPath))
                    {
                        dirPath = Environment.CurrentDirectory;
                    }
                }
                catch
                {
                    dirPath = Environment.CurrentDirectory;
                }

                file.InitialDirectory = dirPath;
                file.Filter = "|*.bin;*.srec;*.s19;*.hex";
                if (file.ShowDialog() == DialogResult.OK)
                {
                    //记录文件地址和目录
                    tb_FilePath.Text = file.FileName;
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }


        private void UpdControlEnable(bool swi)
        {
            bt_ComOpen.Enabled = swi;
            bt_LoadFile.Enabled = swi;
            tb_FilePath.Enabled = swi;
            if (swi == false)
            {
                bt_UpdOpt.Text = "取消升级";
            }
            else
            {
                bt_UpdOpt.Text = "固件升级";
            }
        }

        private void bt_UpdOpt_Click(object sender, EventArgs e)
        {
            try
            {
                //按键根据用户操作显示按键执行内容
                if (bt_UpdOpt.Text == "固件升级")
                {
                    //每次更新前更新LOG显示界面
                    rb_Log.Text = "";
                    Log("开始固件升级");
                    // 第1步：判断界面的升级文件是否存在
                    //TODO
                    // 第2步：判断s19文件是否合法
                    //TODO
                    // 第3步：通知升级
                    fwUpd.Start();
                    UpdControlEnable(false);
                }
                else
                {
                    Log("取消固件升级");
                    //执行取消操作
                    fwUpd.Stop();
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }    
        }
    }
}
