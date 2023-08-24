using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComDemo
{
    //同步显示升级状态
    public class FwUpdStaEventArgs : EventArgs
    {
        public bool mbShow;   //是否需要弹框提示
        public String text;   //升级状态
        public int updProgess;  //升级进度
        public bool finish;     //升级是否结束
    }
    public delegate void FwUpdStaEventHandler(Object sender, FwUpdStaEventArgs e);


    public enum FwUpdMsgT
    {
        E_FWUPD_MSG_INVAILD,
        E_FWUPD_MSG_OUT,    //超时消息
        E_FWUPD_MSG_START,  //开始升级消息
        E_FWUPD_MSG_STOP,   //终止升级消息
        E_FWUPD_MSG_RX,     //接收数据消息
    };

    //界面显示List的数据结构
    public struct msgData_t
    {
        public FwUpdMsgT type;
        public Byte[] data;
    };

    public class FwUpd
    {
        private static Thread ThdUpd;
        private static System.Threading.Timer ThdTmr;
        private static readonly FwUpd instance = new FwUpd();

        public static FwUpd GetInstance()
        {
            return instance;
        }

        /**************************************************************
        * 用于控件LOG显示
        *************************************************************/
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

        /**************************************************************
        * 用于通知主线程控件显示信息
        *************************************************************/
        public event FwUpdStaEventHandler fwUpdStaEvent = null;

        public void StateSync(String sta, int progess)
        {
            FwUpdStaEventArgs args = new FwUpdStaEventArgs();
            args.text = sta;
            args.updProgess = progess;
            args.finish = false;
            args.mbShow = false;

            //处理数据
            if (fwUpdStaEvent != null)
            {
                fwUpdStaEvent.Invoke(this, args);
            }
        }

        public void StateSync(String sta, int progess, bool finish)
        {
            FwUpdStaEventArgs args = new FwUpdStaEventArgs();
            args.text = sta;
            args.updProgess = progess;
            args.finish = finish;
            args.mbShow = false;

            //处理数据
            if (fwUpdStaEvent != null)
            {
                fwUpdStaEvent.Invoke(this, args);
            }
        }

        public void ErrorSync(String sta, bool finish)
        {
            FwUpdStaEventArgs args = new FwUpdStaEventArgs();
            args.text = sta;
            args.updProgess = 0;
            args.finish = finish;
            args.mbShow = true;

            //处理数据
            if (fwUpdStaEvent != null)
            {
                fwUpdStaEvent.Invoke(this, args);
            }
        }

        /**************************************************************
        * 串口接收数据处理
        *************************************************************/
        //Queue的数据结构
        public struct comRxData_t
        {
            public Byte[] data;
        };

        //用于接收数据的节点
        private Queue<comRxData_t> comRxQ = new Queue<comRxData_t>();
        //创建锁，用于保证数据完整性
        private Object rxLock = new Object();
        //对接队列默认值
        private comRxData_t rxDataDefault;
        //通知线程处理变量
        public AutoResetEvent _autoResetRxEvent = new AutoResetEvent(false);

        public void ThreadRxInit()
        {
            gCmdBuf = new Byte[CMD_DATA_MAX_LEN + 10];
            //串口处理初始化操作
            ComRxGetDataInit();
            //创建串口发送处理线程
            Thread comRxTd = new Thread(new ThreadStart(ThreadComRx));
            //设置线程为后台线程.(设置成后台线程后,前台主线程关闭,则此后台线程将强制关闭)
            comRxTd.IsBackground = true;
            comRxTd.Start();
        }

        private void Enqueue(comRxData_t data)
        {
            //增加节点
            try
            {
                lock (rxLock)
                {
                    comRxQ.Enqueue(data);
                }
            }
            catch (System.Exception)
            {
                //catch read exception
            }
        }

        private comRxData_t Dequeue()
        {
            //增加节点
            try
            {
                lock (rxLock)
                {
                    return comRxQ.Dequeue();
                }
            }
            catch (System.Exception)
            {
                return rxDataDefault;
            }
        }

        private int RxIsEmpty()
        {
            //增加节点
            try
            {
                int cnt = 0;
                lock (rxLock)
                {
                    cnt = comRxQ.Count;
                }

                return cnt;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        private void RxClear()
        {
            //增加节点
            try
            {
                lock (rxLock)
                {
                    comRxQ.Clear();
                }
            }
            catch (System.Exception)
            {

            }
        }

        //串口接收添加队列
        public void RxComAdd(Byte data)
        {
            //增加节点
            try
            {
                comRxData_t rxData = new comRxData_t();
                rxData.data = new Byte[1];
                rxData.data[0] = data;
                this.Enqueue(rxData);
                //通知线程执行
                _autoResetRxEvent.Set();
            }
            catch (System.Exception)
            {
                //catch read exception
            }
        }

        //串口接收添加队列
        public void RxComAdd(Byte[] data)
        {
            //增加节点
            try
            {
                //Console.WriteLine(Encoding.Default.GetString(data) +", len="+ data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    comRxData_t rxData = new comRxData_t();
                    rxData.data = new Byte[1];
                    rxData.data[0] = data[i];
                    this.Enqueue(rxData);
                    //通知线程执行
                    _autoResetRxEvent.Set();
                }
            }
            catch (System.Exception)
            {
                //catch read exception
            }
        }

        public void ComBufRxClear()
        {
            RxClear();
        }


        //接收数据显示处理线程
        private void ThreadComRx()
        {
            try
            {
                //线程初始化执行代码

                //执行线程运行程序
                while (true)
                {
                    //等待事件通知处理
                    _autoResetRxEvent.WaitOne();
                    while (0 != this.RxIsEmpty())
                    {
                        comRxData_t val = this.Dequeue();
                        ComRxGetDataCmd(val.data[0]);
                    }

                    //保持和升级线程同步
                    if (updExitFlag == true)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ComRx " + ex);
            }
            finally
            {
                Log("ComRx 线程退出");
            }
        }

        private readonly int CMD_DATA_MAX_LEN = 2048;
        //TBOX Bootloader 协议定义格式
        private readonly byte TBOX_BL_CMD_HEAD_DLE = 0xFF;
        private readonly byte TBOX_BL_CMD_LEN_POS = 1;
        private readonly byte TBOX_BL_CMD_LEN_LEN = 2;
                //数据处理过程
        //串口数据交互类型
        public enum GetCmdSta
        {
            E_GET_CMD_HEAD_DLE, 
            E_GET_CMD_LEN,
            E_GET_CMD_DATA,
            E_GET_CMD_CRC,
            E_GET_CMD_MAX
        };
        private GetCmdSta gGetCmdSta;
        private Byte[] gCmdBuf;
        private int gCmdLen;
        private int gCmdLenSize;
        private int gDataLen;
        private String gProlType = "";

        //串口数据解析初始化
        public void ComRxGetDataInit()
        {
            gGetCmdSta = GetCmdSta.E_GET_CMD_HEAD_DLE;
            gCmdLen = 0;
            gCmdLenSize = 1;
            gDataLen = 0;
            Array.Clear(gCmdBuf, 0x00, gCmdBuf.Length);
        }

        //串口数据解析处理
        public void ComRxGetDataCmd(Byte data)
        {
            switch (gGetCmdSta)
            {
                case GetCmdSta.E_GET_CMD_HEAD_DLE:
                    break;
                case GetCmdSta.E_GET_CMD_LEN:
                    break;
                case GetCmdSta.E_GET_CMD_DATA:
                    break;
                case GetCmdSta.E_GET_CMD_CRC:
                    //数据解析成功后调用接口MsgRxSend
                    break;
                default:
                    break;
            }
        }

        /**************************************************************
        * 模拟消息处理
        *************************************************************/
        //通知线程处理变量
        public AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        //消息列表
        private List<msgData_t> msgList = new List<msgData_t>();
        //创建锁，用于保证数据完整性
        private Object msgLock = new Object();
        //对接队列默认值
        private msgData_t msgDataDefault = new msgData_t();


        /**************************************************************
        * 用于实现线程之前通讯的List操作
        *************************************************************/
        private void Add(msgData_t item)
        {
            //增加节点
            try
            {
                lock (msgLock)
                {
                    msgList.Add(item);
                }
            }
            catch (System.Exception)
            {
                //catch read exception
            }
        }

        private void Remove(msgData_t item)
        {
            //增加节点
            try
            {
                lock (msgLock)
                {
                    msgList.Remove(item);
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
                lock (msgLock)
                {
                    cnt = msgList.Count;
                }
                return cnt;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        private msgData_t GetData()
        {
            if (0 == IsEmpty())
            {
                return msgDataDefault;
            }

            //增加节点
            try
            {
                lock (msgLock)
                {
                    return msgList[0];
                }
            }
            catch (System.Exception)
            {
                return msgDataDefault;
            }
        }

        /**************************************************************
        * 如下部分是线程处理代码
        *************************************************************/
        public bool updEnable = false;
        //线程启动初始化
        public void ThreadInit()
        {
            //线程使能开关设置
            updEnable = true;
            //线程初始化
            UpdSmInit();
            //清空消息队列
            msgList.Clear();
            //将升级指令放到待处理消息内
            msgSend(FwUpdMsgT.E_FWUPD_MSG_START, null);
            //开启定时器
            ThdTmr = new System.Threading.Timer(ThreadTmrCb, null, 10, 20);
            updExitFlag = false;
        }

        //线程退出资源清空
        public void ThreadDeinit()
        {
            //清空消息队列
            msgList.Clear();
            updExitFlag = false;
            //线程使能开关设置
            updEnable = false;
            ThdTmr.Dispose();
        }

        //线程执行程序
        private void ThreadFwUpd()
        {
            try
            {
                //线程初始化执行代码
                FwUpd fwUpd = FwUpd.GetInstance();
                ThreadInit();
                Log("启动升级线程");
                //执行线程运行程序
                while (true)
                {
                    //等待事件通知处理
                    _autoResetEvent.WaitOne();
                    while (0 != this.IsEmpty())
                    {
                        //获取消息队列事件处理
                        msgData_t msg = this.GetData();
                        if (msg.type != FwUpdMsgT.E_FWUPD_MSG_INVAILD)
                        {
                            MsgHander(msg.type, msg.data);
                            //删除数据
                            this.Remove(msg);
                        }
                    }
                    if (updExitFlag == true)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FwUpd " + ex);
                LogError("FwUpd " + ex);
            }
            finally
            {
                ThreadDeinit();
                //Console.WriteLine("FwUpd Couldn't catch the Thread Exception");
                Log("关闭升级线程");
            }
        }

        /**************************************************************
        * 如下部分是主线程控件调用接口
        *************************************************************/
        //点击升级开始相关控件执行操作
        public void Start()
        {
            if (updEnable == false)
            {
                //启动线程
                ThdUpd = new Thread(new ThreadStart(ThreadFwUpd));
                ThdUpd.IsBackground = true;
                ThdUpd.Start();
                //启动串口接收线程
                ThreadRxInit();
            }
            else
            {
                //理论上不存在执行在该处
            }
        }

        //点击升级结束相关控件执行操作
        public void Stop()
        {
            if (updEnable == true)
            {
                //关闭定时器
                msgSend(FwUpdMsgT.E_FWUPD_MSG_STOP, null);
            }
            else
            {
                //理论上不存在执行在该处 
            }
        }

        /**************************************************************
        * 如下部分是主线程消息通讯接口
        *************************************************************/
        public void msgSend(FwUpdMsgT type, Byte[] msgData)
        {
            if (false == updEnable)
            {
                return;
            }

            msgData_t msg = new msgData_t();
            msg.type = type;
            msg.data = msgData;
            this.Add(msg);
            //通知线程执行
            _autoResetEvent.Set();
        }

        //发送定时消息给线程处理
        public void ThreadTmrCb(Object state)
        {
            MsgTimerOut();
        }

        //处理定时器消息
        public void MsgTimerOut()
        {
            msgSend(FwUpdMsgT.E_FWUPD_MSG_OUT, null);
        }

        //处理串口数据接收消息
        public void MsgRxSend(Byte[] data)
        {
            msgSend(FwUpdMsgT.E_FWUPD_MSG_RX, data);
        }

        /**************************************************************
        * 如下部分是升级线程处理消息框架代码
        *************************************************************/
        private void MsgHander(FwUpdMsgT type, Byte[] data)
        {
            switch (type)
            {
                case FwUpdMsgT.E_FWUPD_MSG_RX:
                    MsgRxHander(data);
                    break;
                case FwUpdMsgT.E_FWUPD_MSG_OUT:
                    MsgTimerOutHander();
                    break;
                case FwUpdMsgT.E_FWUPD_MSG_START:
                    MsgStartHander();
                    break;
                case FwUpdMsgT.E_FWUPD_MSG_STOP:
                    MsgStopHander();
                    break;
            }
        }

        //状态机处理函数
        //处理超时消息
        public void MsgTimerOutHander()
        {
            UpdSmEventHander(UpdEvent.E_EVENT_TMOT, null);
        }

        //处理开始消息
        public void MsgStartHander()
        {
            UpdSmEventHander(UpdEvent.E_EVENT_START, null);
        }

        //处理终止消息
        public void MsgStopHander()
        {
            UpdSmEventHander(UpdEvent.E_EVENT_EXIT, null);
        }

        //处理串口数据消息
        public void MsgRxHander(Byte[] data)
        {
            UpdSmEventHander(UpdEvent.E_EVENT_MSG_RX, data);
        }


        /**************************************************************
        * 如下部分是升级当个文件流程状态机实现
        *************************************************************/
        //状态
        public enum UpdStates
        {
            E_STA_IDLE = 0,     //空闲状态
            E_STA_START,        //进入编程模式
            E_STA_AUTH,         //身份认证
            E_STA_ERASE,        //擦除
            E_STA_REQ_DOWNLOAD, //请求下载
            E_STA_TRANS_DATA,   //发送数据
            E_STA_TRANS_EXIT,   //退出传输
            E_STA_CHECKSUM,     //文件校验
            E_STA_RESET,        //复位重启
            E_STA_EXIT,         //退出升级
            E_STA_MAX
        };

        //触发事件
        public enum UpdEvent
        {
            E_EVENT_IDLE = 0,
            E_EVENT_TMOT,
            E_EVENT_START,
            E_EVENT_EXIT,
            E_EVENT_MSG_RX,
            E_EVENT_MAX,
        };

        //定义状态处理函数
        public delegate UpdStates StateMachineHander(UpdEvent evt, Byte[] data);
        //业务处理全局变量
        public UpdSmTab_t[] updSmTab;
        public UpdSmSta updSmSta;
        public bool updExitFlag = false;

        //状态切换全局变量
        public struct UpdSmSta
        {
            private UpdStates NowSta;
            private int Cnt;
            private Byte mode;

            public UpdSmSta(UpdStates sta, int cnt, Byte mode)
                : this()
            {
                this.NowSta = sta;
                this.Cnt = cnt;
                this.mode = mode;
            }

            public UpdStates OptNowSta
            {
                get { return this.NowSta; }
                set { this.NowSta = value; }
            }
            public int OptCnt
            {
                get { return this.Cnt; }
                set { this.Cnt = value; }
            }

            public Byte OptMode
            {
                get { return this.mode; }
                set { this.mode = value; }
            }
        }

        //升级管理状态机表结构
        public struct UpdSmTab_t
        {
            private string name;
            private StateMachineHander func;

            public UpdSmTab_t(string name, StateMachineHander func)
                : this()
            {
                this.name = name;
                this.func = func;
            }

            public string OptName
            {
                get { return this.name; }
                set { }
            }

            public StateMachineHander OptFunc
            {
                get { return this.func; }
                set { }
            }
        }

        //空闲状态下处理函数
        private UpdStates UpdSmIdle(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_IDLE;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    updSmSta.OptCnt = 0;
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_START:
                    ret = UpdStates.E_STA_START;
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    ret = UpdStates.E_STA_EXIT;
                    break;
            }
            return ret;
        }

        //开始状态下处理
        private UpdStates UpdSmStart(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_START;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    StateSync("开始", 0);
                    updSmSta.OptCnt = 0;
                    EcuDscRequire((Byte)eDscMode.E_SESSION_PROGRAMING);
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    EcuDscRequire((Byte)eDscMode.E_SESSION_PROGRAMING);
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    Byte err = EcuDscResponse(data);
                    if (E_UDS_RESP_OK == err)
                    {
                        ComMode com = ComMode.GetInstance();
                        com.ComBufClear();
                        ret = UpdStates.E_STA_AUTH;
                    }
                    else if (E_UDS_RESP_NRC_GENERALREJECT != err)
                    {
                        ret = UpdStates.E_STA_EXIT;
                    }
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    ret = UpdStates.E_STA_EXIT;
                    break;

            }
            return ret;
        }

        private UpdStates UpdSmAuth(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_AUTH;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    break;
            }
            return ret;
        }

        private UpdStates UpdSmErase(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_ERASE;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    break;
            }
            return ret;
        }

        private UpdStates UpdSmReqDownload(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_REQ_DOWNLOAD;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    break;
            }
            return ret;
        }

        private UpdStates UpdSmTransferData(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_TRANS_DATA;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    break;
            }
            return ret;
        }


        private UpdStates UpdSmTransferExit(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_TRANS_EXIT;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    break;
            }
            return ret;
        }

        private UpdStates UpdSmChecksum(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_CHECKSUM;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    break;
            }
            return ret;
        }

        private UpdStates UpdSmReset(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_RESET;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    break;
                case UpdEvent.E_EVENT_EXIT:
                    break;
            }
            return ret;
        }

        private UpdStates UpdSmExit(UpdEvent evt, Byte[] data)
        {
            UpdStates ret = UpdStates.E_STA_EXIT;
            switch (evt)
            {
                case UpdEvent.E_EVENT_IDLE:
                    break;
                case UpdEvent.E_EVENT_TMOT:
                    break;
                case UpdEvent.E_EVENT_MSG_RX:
                    break;
            }
            return ret;
        }

        //状态机执行函数
        public void UpdSmEventHander(UpdEvent evt, Byte[] data)
        {
            UpdStates curSta = updSmSta.OptNowSta;
            UpdStates tranSta = curSta;
            StateMachineHander pFunc;
            //判断传参是否合理
            if ((curSta >= UpdStates.E_STA_MAX) || (evt >= UpdEvent.E_EVENT_MAX))
            {
                return;
            }
        STA_INIT:
            //执行状态机
            pFunc = updSmTab[(int)curSta].OptFunc;
            if (null != pFunc)
            {
                tranSta = pFunc(evt, data);
            }
            //若状态切换则立刻执行
            if (tranSta != curSta)
            {

                //Console.Write("UpdSm:" + curSta.ToString() + "->" + tranSta.ToString() + "\r\n");
                Log("升级状态:" + updSmTab[(int)curSta].OptName + "->" + updSmTab[(int)tranSta].OptName);
                curSta = tranSta;
                updSmSta.OptNowSta = tranSta;
                evt = UpdEvent.E_EVENT_IDLE;
                goto STA_INIT;
            }
        }

        //状态机初始化
        public void UpdSmInit()
        {
            //变量重新new的话会重新初始化变量
            updSmTab = new UpdSmTab_t[]
            {
                new UpdSmTab_t("空闲", UpdSmIdle),
                new UpdSmTab_t("进入编程模式", UpdSmStart),
                new UpdSmTab_t("身份认证", UpdSmAuth),
                new UpdSmTab_t("擦除Flash", UpdSmErase),
                new UpdSmTab_t("请求下载", UpdSmReqDownload),
                new UpdSmTab_t("传输数据", UpdSmTransferData),
                new UpdSmTab_t("传输结束", UpdSmTransferExit),
                new UpdSmTab_t("文件校验", UpdSmChecksum),
                new UpdSmTab_t("重启复位", UpdSmReset),
                new UpdSmTab_t("退出升级", UpdSmExit),
            };
            updSmSta = new UpdSmSta(0, 0, 0);
        }

        //释放状态机资源
        public void UpdSmDeinit()
        {
            //状态机清0
        }

        /**************************************************************
        * 如下是串口相关指令
        *************************************************************/
        private static readonly byte MSG_COM_DATA_LEN = 4;
        private static readonly byte MSG_CMD_POS = 3;
        private static readonly byte MSG_LEN_POS = 4;

        //组包通用接口
        private static int MsgPack(ref Byte[] dstMsg, Byte[] data)
        {
            if (dstMsg.Length < MSG_COM_DATA_LEN + data.Length)
            {
                return 0;
            }

            //消息头
            int pos = 0;
            dstMsg[pos] = 0xFF;
            pos++;
            //数据长度
            Cmn.memcpy(ref dstMsg, pos, Cmn.Uint16ToBytesBig((UInt16)data.Length), 0, 2);
            pos += 2;
            //数据内容
            Cmn.memcpy(ref dstMsg, pos, data, 0, data.Length);
            pos += data.Length;
            //CRC值
            dstMsg[pos] = Cmn.GetXORTboxBl(dstMsg, 1, pos - 1);
            pos++;
            return pos;
        }

        //数据发送接口
        public void MsgUpdSend(Byte[] data)
        {
            //被其它类调用，需要申明一下
            try
            {
                Byte[] msg = new Byte[data.Length + MSG_COM_DATA_LEN];
                int len = MsgPack(ref msg, data);
                if (len != 0)
                {
                    //串口发送数据
                    ComMode comMode = ComMode.GetInstance();
                    comMode.ComSend(msg);
                    if (msg.Length < 50)
                    {
                        Log(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("MsgUpd" + ex);
            }
        }

        //发送指令使用统一的接口
        public void Send(Byte[] data)
        {
            MsgUpdSend(data);
        }

        public void Send(Byte[] data, int len)
        {
            if (len > data.Length)
            {
                return;
            }
            //申明变量
            Byte[] buf = new Byte[len];
            Cmn.memcpy(ref buf, 0, data, 0, len);
            MsgUpdSend(buf);
        }

        /**************************************************************
        * 如下是升级交互判断
        *************************************************************/
        public enum eUdsService
        {
            E_UDS_SRV_DSC = 0x10,           //Diagnostic Session Control (0x10) 服务
            E_UDS_SRV_RESET = 0x11,         //ECU Reset (0x11) 服务
            E_UDS_SRV_SEC_ACCESS = 0x27,    //Security Access (0x27) 服务
            E_UDS_SRV_ROUTINE_CTL = 0x31,   //Routine Control (0x31)服务
            E_UDS_SRV_REQ_DOWNLOAD = 0x34,   //Request Download (0x34)服务
            E_UDS_SRV_TRANS_DATA = 0x36,    //Transfer Data (0x36)服务
            E_UDS_SRV_TRANS_EXIT = 0x37,    //Request Transfer Exit (0x37)服务
        };

        private readonly Byte RESPONSE_SID_OFFESET = 0x40;
        private readonly Byte UDS_RESP_NRC = 0x7F;
        private readonly Byte UDS_RESP_WAIT = 0x78;
        private readonly Byte E_UDS_RESP_OK = 0x00;
        private readonly Byte E_UDS_OPT_FAILE = 0x02;
        private readonly Byte E_UDS_RESP_NRC_GENERALREJECT = 0xFF;

        private Byte gUdsReqSub = 0x00;
        private UInt16 gUdsSeed = 0x0000;
        private UInt32 g32UdsSeed = 0x00000000;

        public enum eDscMode
        {
            E_SESSION_DEFAULT = 0x01,  //0x01 : default Session，默认模式
            E_SESSION_PROGRAMING = 0x02, //0x02 : Programming Session，编程模式
            E_SESSION_EXTEND = 0x03,  //0x03 : extended Diagnostic Session，扩展诊断模式
        };

        public enum eAuthMode
        {
            E_AUTH_ONE = 0x03,  //0x03 : 请求seed
            E_AUTH_TWO = 0x04, //0x04 : 发送key
        };

        public Byte UdsResponseCheck(Byte[] data, Byte Sid, Byte Sub)
        {
            //判断是否为负响应
            if (data[0] == UDS_RESP_NRC)
            {
                //直接返回NRC值
                //判断SID是否一致
                if (data[1] != Sid)
                {
                    return E_UDS_RESP_NRC_GENERALREJECT;
                }

                if (data[2] != UDS_RESP_WAIT)
                {
                    return data[2];
                }
                else
                {
                    return E_UDS_RESP_NRC_GENERALREJECT;
                }
            }
            else
            {
                //正响应判断SRV ID和SID是否一致
                if (data[0] == Sid + RESPONSE_SID_OFFESET)
                {
                    if (data[1] != Sub)
                    {
                        return E_UDS_RESP_NRC_GENERALREJECT;
                    }
                    else
                    {
                        return E_UDS_RESP_OK;
                    }
                }
                else
                {
                    return E_UDS_RESP_NRC_GENERALREJECT;
                }
            }
        }

        public Byte UdsResponseCheck(Byte[] data, Byte Sid)
        {
            //判断是否为负响应
            if (data[0] == UDS_RESP_NRC)
            {
                //直接返回NRC值
                return data[2];
            }

            //正响应判断SRV ID和SID是否一致
            if (data[0] == Sid + RESPONSE_SID_OFFESET)
            {
                return E_UDS_RESP_OK;
            }
            else
            {
                return E_UDS_RESP_NRC_GENERALREJECT;
            }
        }

        //会话模式设置
        public void EcuDscRequire(Byte mode)
        {
            Byte[] data = new Byte[2];
            data[0] = (Byte)eUdsService.E_UDS_SRV_DSC;
            data[1] = mode;
            gUdsReqSub = data[1];
            Send(data);
        }

        //会话模式响应
        public Byte EcuDscResponse(Byte[] data)
        {
            return UdsResponseCheck(data, (Byte)eUdsService.E_UDS_SRV_DSC, gUdsReqSub);
        }

    }
}
