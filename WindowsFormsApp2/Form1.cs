using WindowsFormsApp2;
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
using System.Text.RegularExpressions;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
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

                LogBox.SelectionColor = clr;
                LogBox.AppendText(e.strData);
                LogBox.ScrollToCaret(); //滚动到底部
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
                if (updateBtn.Text == "取消升级")
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
            open_btn.Text = "打开串口";
            //打开串口刷新button
            refreshCom_Btn.Enabled = true;
            refreshCom_Btn.Enabled = true;
            baudrate_cbb.Enabled = true;
            serial_port_cbb.Enabled = true;
        }

        //检测当前实际串口情况
        public void comPortCheck()
        //获取串口显示清单
        {
            string[] ArrayComPortsNames = SerialPort.GetPortNames();
            //用于表示当前刷写的串口清单中是否有当前连接的串口
            bool comIsValid = false;

            //清除串口显示列表缓存
            serial_port_cbb.Items.Clear();
            //未检测到串口下处理
            if (ArrayComPortsNames.Length == 0)
            {
                //串口列表显示界面清空
                serial_port_cbb.Text = "";
                comStaInit();
            }
            else
            {
                //数据排序，按默认规则排序
                Array.Sort(ArrayComPortsNames);
                //更新串口显示清单
                for (int i = 0; i < ArrayComPortsNames.Length; i++)
                {
                    serial_port_cbb.Items.Add(ArrayComPortsNames[i]);
                    //判断是否存在
                    if (gCom.LinkPort == ArrayComPortsNames[i])
                    {
                        comIsValid = true;
                    }
                }
                //若有串口则显示之前连接串口
                if (comIsValid == true)
                {
                    serial_port_cbb.Text = gCom.LinkPort;
                }
                //若无匹配串口则显示重新遍历的第一个串口
                else
                {
                    serial_port_cbb.Text = ArrayComPortsNames[0];
                    gCom.LinkSta = false;
                }
                gCom.LinkPort = serial_port_cbb.Text;
            }
        }

        //串口开关操作
        public bool comOpenOpt(bool opt)
        {
            //执行打开串口
            if (true == opt)
            {
                if (true == ComDev.DevOpen(serial_port_cbb.Text, baudrate_cbb.Text))
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
                open_btn.Text = "打开串口";
                refreshCom_Btn.Enabled = true;
                baudrate_cbb.Enabled = true;
                serial_port_cbb.Enabled = true;
                updateBtn.Enabled = false;
            }
            else
            {
                open_btn.Text = "关闭串口";
                refreshCom_Btn.Enabled = false;
                baudrate_cbb.Enabled = false;
                serial_port_cbb.Enabled = false;
                updateBtn.Enabled = true;
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

        private void open_btn_Click(object sender, EventArgs e)
        {
            //更新当前串口数
            comPortCheck();
            if (gCom.LinkPort == "")
            {
                open_btn.Text = "打开串口";
                comControlView(false);
                LogError("找不到串口设备");
                return;
            }

            //执行串口
            if (open_btn.Text == "打开串口")
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
            baudrate_cbb.Items.Add(2400);
            baudrate_cbb.Items.Add(4800);
            baudrate_cbb.Items.Add(9600);
            baudrate_cbb.Items.Add(19200);
            baudrate_cbb.Items.Add(38400);
            baudrate_cbb.Items.Add(57600);
            baudrate_cbb.Items.Add(115200);
            baudrate_cbb.Items.Add(460800);
            baudrate_cbb.Items.Add(1000000);
            baudrate_cbb.Items.ToString();
            //设置默认显示的波特率
            baudrate_cbb.Text = baudrate_cbb.Items[6].ToString();

            //开机串口全局变量初始化
            comStaInit();
            //搜索当前电脑有多少个串口
            comPortCheck();
        }

        private void refreshCom_Btn_Click(object sender, EventArgs e)
        {
            //更新当前串口数
            comPortCheck();
        }


        private void filePathBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string path = filePathBox.Text;
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
                    filePathBox.Text = file.FileName;
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }


        private void UpdControlEnable(bool swi)
        {
            open_btn.Enabled = swi;
            filePathBtn.Enabled = swi;
            filePathBox.Enabled = swi;
            if (swi == false)
            {
                updateBtn.Text = "取消升级";
            }
            else
            {
                updateBtn.Text = "固件升级";
            }
        }

        //判断s19文件是否合法
        static bool IsS19FileValid(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (!IsValidS19Line(line))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
        static bool IsValidS19Line(string line)
        {
            // Example: S113100018000022C0C900001AC0E0000018C0D3
            // Check for a valid S19 record pattern using regular expression
            string pattern = @"^S[0-9A-F]{2}[0-9A-F]{2}[0-9A-F]+[0-9A-F]{2}$";
            if (Regex.IsMatch(line, pattern))
            {
                // Additional validation logic could be added here, e.g., checksum verification
                return true;
            }

            return false;
        }
        private void updateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //按键根据用户操作显示按键执行内容
                if (updateBtn.Text == "固件升级")
                {
                    //每次更新前更新LOG显示界面
                    LogBox.Text = "";

                    // 判断界面的升级文件是否合法
                    if (IsS19FileValid(filePathBox.Text))
                    {
                        Log("开始固件升级");
                        // 通知升级
                        fwUpd.Start();
                        UpdControlEnable(false);
                    }
                    else
                    {
                        LogError("S19文件不合法");
                    }
                    
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
