using Himma.Common.Communication.Contract;
using Himma.Common.Enums;
using Himma.Common.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using System.Text;
using System.Configuration;
using Himma.Common.Log;

namespace Himma.Common.Models.Base
{
    /// <summary>
    /// PLC模型基类
    /// </summary>
    public abstract class BasePLC : BaseModel
    {
        /// <summary>
        /// 基准性能信息存放目录
        /// </summary>
        public string BenchmarkInfoPath = System.IO.Path.Combine(ConfigurationManager.AppSettings["LogFilePath"], "BenchmarkInfo");
        /// <summary>
        /// 是否使用连接池
        /// </summary>
        public bool UsedPool { get; set; }
        /// <summary>
        /// 是否使用保存CSV
        /// </summary>
        public bool UseSaveCSV = false;
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public readonly Guid UUID = Guid.NewGuid();
        #region 信号量

        ///// <summary>
        ///// 写 信号量
        ///// </summary>
        //private readonly Lazy<Semaphore> _semaphoreWrite = new Lazy<Semaphore>(() => new Semaphore(10, 15));
        ///// <summary>
        ///// 读 信号量
        ///// </summary>
        //private readonly Lazy<Semaphore> _semaphoreRead = new Lazy<Semaphore>(() => new Semaphore(10, 20));
        /////// <summary>
        /////// 读 扫描 信号量 执行特殊动作专用信号量
        /////// </summary>
        //private readonly Lazy<Semaphore> _semaphoreReadAll = new Lazy<Semaphore>(() => new Semaphore(10, 15));
        #endregion
        /// <summary>
        /// 本地远程状态
        /// </summary>
        //public ClassVarEvent<LocalRemote> StatusLocalOrRemote { get; set; } = new ClassVarEvent<LocalRemote>();
        public Action<bool> PlcConnectChangeAction;
        /// <summary>
        /// PLC编码
        /// </summary>
        public string PLCNo { get; set; }
        public object _lock = new object();
        public string localAmsNetId { get; set; }
        public string targetAmsNetId { get; set; }

        /// <summary>
        /// plcIP地址
        /// </summary>
        public string PLCName { get; set; }
        public string IPAddress { get; set; }
        public string IpPort { get; set; }
        public string PlcType { get; set; }
        private bool connected;
        public bool Connected
        {
            get { return connected; }
            set
            {
                if (connected != value)
                {
                    PlcConnectChangeAction?.Invoke(value);
                }
                connected = value;
            }
        }
        public bool IsEnabled
        {
            get
            {
                if (UsedFlag == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public int UsedFlag { get; set; } = 0;
        /// <summary>
        /// 运行状态
        /// </summary>
        public RunStatus RunStatus { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public DevStatus DevStatus { get; set; }

        public PlcStatus PlcStatus { get; set; }
        /// <summary>
        /// 联机/脱机
        /// </summary>
        public OnLineStatus OnLineStatus { get; set; }

        /// <summary>
        /// 是否使用LCC
        /// </summary>
        public LCCType lCCType { get; set; }

        public CommunicType communicType;
        /// <summary>
        /// PLC对应变量数组
        /// </summary>
        public string[] VarList { get; set; } = new string[0];
        //自动状态
        public AutoStatus AutoStatus { get; set; }
        //读取间隔
        public int PLCReadInterval = 100; // 默认100ms
        //重写次数
        public int PLCWriteTryMaxCount = 4; // PLCWrite失败尝试重写次数

        //事件PLC状态
        public ClassVarEvent<PlcStatus> PlcStatusMonitor = new ClassVarEvent<PlcStatus>();
        //事件PLC自动状态
        public ClassVarEvent<AutoStatus> AutoStatusMonitor = new ClassVarEvent<AutoStatus>();
        /// <summary>
        /// 上位机当前心跳值
        /// </summary>
        public ClassVarEvent<short> UpHeartBreath = new ClassVarEvent<short>();
        /// <summary>
        /// PLC当前心跳值
        /// </summary>
        public ClassVarEvent<short> PlcHeartBreath = new ClassVarEvent<short>();
        /// <summary>
        /// PLC交互标志
        /// </summary>
        public bool InteractiveFlag = false;

        /// <summary>
        /// mes编码
        /// </summary>
        public string MesNo { get; set; }

        /// <summary>
        /// plc连接数
        /// </summary>
        public int ConnectionNum { get; set; }

        /// <summary>
        /// 通讯断连计数
        /// </summary>
        public int OffLineNum { get; set; } = 0;

        /// <summary>
        /// 重连次数
        /// </summary>
        public int RetryNum { get; set; }

        /// <summary>
        /// 重连间隔时间
        /// </summary>
        public int RetryInteralTime { get; set; }
        /// <summary>
        /// 通讯连接池
        /// </summary>
        //protected PlcOperatorPool CommunicationPool = null;
        /// <summary>
        /// 基础连接池
        /// </summary>

        //protected PlcOperatorPoolBase CommunicationPoolBase = null;
        /// <summary>
        /// 接口
        /// </summary>
        //public IPlcOperator operationBase;
        public IPlcOperator operationRead;
        public IPlcOperator operationReadScan;
        public IPlcOperator operationWrite;
        public IPlcOperator operationReadHeart;
        public IPlcOperator operationWriteHeart;
        public Action OnPLCReadFinished;
        protected string PlcHeartReadVar;
        protected string WriteToPlcHeartVal;
        private int PlcHeartBreakOffOutTime;
        public event EventHandler<PLCSignalModel> SignalChanged;


        /// <summary>
        /// 构造函数 PLC
        /// </summary>
        /// <param name="omronType">欧姆龙类型</param>
        /// <param name="heartReadVar">心跳读取的参数名称</param>
        /// <param name="isEnabledPlc">是否启用</param>
        [Obsolete]
        public BasePLC(string heartReadVar, string heartWriteVar, int timeOutSec, OmronType omronType)
        {
            //RunStatus = RunStatus.Stopped;
            //PlcHeartBreath.MyValue = 1;
            //UpHeartBreath.MyValue = 1;
            //OnLineStatus = OnLineStatus.OffLine;
            //PlcStatus = PlcStatus.Default;
            //PlcStatusMonitor.MyValue = PlcStatus.Default;
            //AutoStatusMonitor.MyValue = AutoStatus.Default;
            //Connected = false;
            //PlcHeartReadVar = heartReadVar;
            //PlcHeartBreakOffOutTime = timeOutSec;
            //WriteToPlcHeartVal = heartWriteVar;
            //operationRead = new OmronOperator(omronType);
            //operationWrite = new OmronOperator(omronType);
            //operationReadHeart = new OmronOperator(omronType);
            //operationWriteHeart = new OmronOperator(omronType);
            //lCCType = LCCType.NotIsLCC;
        }
        /// <summary>
        /// 新PLC构造函数   使用LCC
        /// </summary>
        /// <param name="heartReadVar"></param>
        /// <param name="heartWriteVar"></param>
        /// <param name="timeOutSec"></param>
        /// <param name="type"></param>
        public BasePLC(string heartReadVar, string heartWriteVar, int timeOutSec, CommunicType type)
        {
            UseSaveCSV = GlobalConfig.IsUsedSaveCSV == 1;
            RunStatus = RunStatus.Stopped;
            PlcHeartBreath.MyValue = 1;
            UpHeartBreath.MyValue = 1;
            OnLineStatus = OnLineStatus.OffLine;
            PlcStatus = PlcStatus.Default;
            PlcStatusMonitor.MyValue = PlcStatus.Default;
            AutoStatusMonitor.MyValue = AutoStatus.Default;
            Connected = false;
            PlcHeartReadVar = heartReadVar;
            PlcHeartBreakOffOutTime = timeOutSec;
            WriteToPlcHeartVal = heartWriteVar;
            communicType = type;
            if (UsedPool)
            {
                if (type == CommunicType.BeckhoffAds6)
                {
                    //operationRead = new BeckhoffTwincatCommunic();
                    //operationReadScan = new BeckhoffTwincatCommunic();
                    //operationBase = new BeckhoffTwincatCommunic();
                    //operationWrite = new BeckhoffTwincatCommunic();
                    //operationReadHeart = new BeckhoffTwincatCommunic();
                    //operationWriteHeart = new BeckhoffTwincatCommunic();
                }
                else
                {
                    //operationRead = new OmronCommunic();
                    //perationReadScan = new OmronCommunic();
                    //operationBase = new OmronCommunic();
                    //operationWrite = new OmronCommunic();
                    //operationReadHeart = new OmronCommunic();
                    //operationWriteHeart = new OmronCommunic();
                }
            }
            else
            {
                if (type == CommunicType.BeckhoffAds6)
                {
                    //operationRead = new BeckhoffTwincatCommunic();
                    //operationReadScan = new BeckhoffTwincatCommunic();
                    //operationWrite = new BeckhoffTwincatCommunic();
                    //operationReadHeart = new BeckhoffTwincatCommunic();
                    //operationWriteHeart = new BeckhoffTwincatCommunic();
                }
                else
                {
                    //operationRead = new OmronCommunic();
                    //operationReadScan = new OmronCommunic();
                    //operationWrite = new OmronCommunic();
                    //operationReadHeart = new OmronCommunic();
                    //operationWriteHeart = new OmronCommunic();
                }
            }
            lCCType = LCCType.IsLCC;
        }
        /// <summary>
        /// 启动PLC
        /// </summary>
        public void Active()
        {
            try
            {
                if (UsedPool)
                {
                    if (lCCType == LCCType.IsLCC) operationReadScan.ActiveNew(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    else operationReadScan.Active(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);

                    //operationBase.Active(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);

                    //var set = new PlcOperatorPoolConnectionInfo(communicType,
                    //localAmsNetId, targetAmsNetId, IPAddress);
                    //set.PlcHeartReadVar = PlcHeartReadVar;
                    //set.MaxPoolSize = 3;
                    //set.MinPoolSize = 3;  //原来Min:2;现改为跟Max一样为:10;
                    //set.ConnectTimeout = TimeSpan.FromMilliseconds(GlobalConfig.LCCReadTimeOut);
                    //set.UseLCC = true;
                    //CommunicationPool = new PlcOperatorPool(set);
                    //CommunicationPool.UUID = set.UUID;

                    //基础链接
                   // var set_base = new PlcOperatorPoolConnectionInfoBase(communicType,
                   //localAmsNetId, targetAmsNetId, IPAddress);
                   // set_base.PlcHeartReadVar = PlcHeartReadVar;
                   // set_base.MaxPoolSize = 3;
                   // set_base.MinPoolSize = 3;   //原来Min:1;现改为跟Max一样为：5；
                   // set_base.ConnectTimeout = TimeSpan.FromMilliseconds(GlobalConfig.LCCReadTimeOut);
                   // if (communicType == CommunicType.OmronCip)
                   // {
                   //     set_base.UseLCC = false;
                   // }
                   // CommunicationPoolBase = new PlcOperatorPoolBase(set_base);
                   // //CommunicationPoolBase.UUID = set.UUID;

                   // var basepoolstatus = CommunicationPoolBase.CheckAvailable();
                   // var readstatus = operationReadScan.IsActived();
                   // var poolstatus = CommunicationPool.CheckAvailable();
                   // if (readstatus & poolstatus & basepoolstatus)
                   // {
                   //     LogHelper.Info($"LCC链接 {IPAddress} {PLCNo} 初始化成功", "plc");
                   //     OnLineStatus = OnLineStatus.OnLine;
                   //     return;
                   // }
                    //else
                    //{

                    //    LogHelper.Error($"LCC链接 {IPAddress} {PLCNo} 初始化失败 Read:{readstatus} BasePool：{basepoolstatus} ProPool{poolstatus}", "plc");
                    //}

                }
                else
                {
                    if (lCCType == LCCType.IsLCC) operationRead.ActiveNew(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    else operationRead.Active(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    if (lCCType == LCCType.IsLCC) operationReadScan.ActiveNew(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    else operationReadScan.Active(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    if (lCCType == LCCType.IsLCC) operationWrite.ActiveNew(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    else operationWrite.Active(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    if (lCCType == LCCType.IsLCC) operationReadHeart.ActiveNew(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    else operationReadHeart.Active(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    if (lCCType == LCCType.IsLCC) operationWriteHeart.ActiveNew(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);
                    else operationWriteHeart.Active(localAmsNetId, targetAmsNetId, IPAddress, GlobalConfig.LCCReadTimeOut);


                    if (operationRead.IsActived() && operationReadScan.IsActived() && operationWrite.IsActived() && operationReadHeart.IsActived() && operationWriteHeart.IsActived())
                    {
                        OnLineStatus = OnLineStatus.OnLine;
                        return;
                    }
                    ActiveError?.Invoke(new Exception("plc.IsActived()为false"));
                    OnLineStatus = OnLineStatus.OffLine;
                }


               

            }
            catch (Exception ex)
            {
                //OnLineStatus = OnLineStatus.OffLine;
                ActiveError?.Invoke(ex);
               
            }
        }

        /// <summary>
        /// 事件 心跳完成
        /// </summary>
        public event EventHandler OnSystemHeartFinished;
        public abstract void ExecuteReadAll();
        public abstract Task ExecuteReadAllAsync(BasePLC plc);

        public abstract void ExecuteReadData();

        //PLC写入错误回调
        protected Action<Exception> SystemToPlcError;
        //PLC激活错误回调
        protected Action<Exception> ActiveError;
        //激活异常
        public bool IsActiveError = false;

        public virtual void ReadSystemHeart()
        {
            using (var mini = new CommonMiniProfiler("PlcWriteHeartToPlcAsync"))
            {
                if (UsedPool)
                {
                    short val = 0;
                    using (var set = mini.Step("ReadAsync"))
                    {
                        val = Read<short>(PlcHeartReadVar);
                        PlcHeartBreath.MyValue = val;
                        if (PlcHeartBreath.TimeInterval > PlcHeartBreakOffOutTime)
                        {
                            LogHelper.Error($"【读取】【{this}】的心跳信号超过{PlcHeartBreakOffOutTime}秒没有发生变化，当前读取到的心跳值是{PlcHeartBreath.MyValue}");
                            OnLineStatus = OnLineStatus.OffLine;
                        }
                        else
                        {
                            OnLineStatus = OnLineStatus.OnLine;
                        }
                    }
                }
                else
                {
                    short val = 0;
                    using (var set = mini.Step("ReadAsync"))
                    {
                        val = operationReadHeart.Read<short>(PlcHeartReadVar);
                        PlcHeartBreath.MyValue = val;
                        if (PlcHeartBreath.TimeInterval > PlcHeartBreakOffOutTime)
                        {
                            LogHelper.Error($"【读取】【{this}】的心跳信号超过{PlcHeartBreakOffOutTime}秒没有发生变化，当前读取到的心跳值是{PlcHeartBreath.MyValue}");
                            OnLineStatus = OnLineStatus.OffLine;
                            //PlcStatusMonitor.MyValue = PlcStatus.Fault; // PLC状态故障
                        }
                        else
                        {
                            OnLineStatus = OnLineStatus.OnLine;
                            //PlcStatusMonitor.MyValue = PlcStatus.Running;// PLC状态正常
                        }
                    }
                }
                if (UseSaveCSV)
                {
                    CommonMiniProfiler.SaveCSV(mini.GetInfoData(), BenchmarkInfoPath, this.PLCNo + "BasePlcWriteHeartToPlcAsync");
                }
            }
        }
        public virtual async Task ReadSystemHeartAsync()
        {
            try
            {
                using (var mini = new CommonMiniProfiler("PlcWriteHeartToPlcAsync"))
                {
                    if (UsedPool)
                    {
                        short val = 0;
                        using (var set = mini.Step("ReadAsync"))
                        {
                            //using (var proxy = CommunicationPool.Get())
                            //{
                            //    val = await proxy.Value.ReadAsync<short>(PlcHeartReadVar);
                            //    PlcHeartBreath.MyValue = val;
                            //    if (PlcHeartBreath.TimeInterval > PlcHeartBreakOffOutTime)
                            //    {
                            //        LogHelper.Error($"【读取】【{this}】的心跳信号超过{PlcHeartBreakOffOutTime}秒没有发生变化，当前读取到的心跳值是{PlcHeartBreath.MyValue}");
                            //        OnLineStatus = OnLineStatus.OffLine;
                            //    }
                            //    else
                            //    {
                            //        OnLineStatus = OnLineStatus.OnLine;
                            //    }
                            //}

                        }
                        using (var set = mini.Step("Invoke"))
                        {
                            //OnSystemHeartFinished?.Invoke(this, new PlcEventArg(val));
                        }
                    }
                    else
                    {
                        short val = 0;
                        using (var set = mini.Step("ReadAsync"))
                        {
                            val = await operationReadHeart.ReadAsync<short>(PlcHeartReadVar);
                            PlcHeartBreath.MyValue = val;
                            if (PlcHeartBreath.TimeInterval > PlcHeartBreakOffOutTime)
                            {
                                LogHelper.Error($"【读取】【{this}】的心跳信号超过{PlcHeartBreakOffOutTime}秒没有发生变化，当前读取到的心跳值是{PlcHeartBreath.MyValue}");
                                OnLineStatus = OnLineStatus.OffLine;
                                //PlcStatusMonitor.MyValue = PlcStatus.Fault; // PLC状态故障
                            }
                            else
                            {
                                OnLineStatus = OnLineStatus.OnLine;
                                //PlcStatusMonitor.MyValue = PlcStatus.Running;// PLC状态正常
                            }
                        }

                        using (var set = mini.Step("Invoke"))
                        {
                            //OnSystemHeartFinished?.Invoke(this, new PlcEventArg(val));
                        }
                    }
                    if (UseSaveCSV)
                    {
                        CommonMiniProfiler.SaveCSV(mini.GetInfoData(), BenchmarkInfoPath, this.PLCNo + "BasePlcWriteHeartToPlcAsync");
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        private int WriteHeartToPlcErrorTick = 0;
        /// <summary>
        /// 给PLC写心跳信号
        /// </summary>
        /// <returns></returns>
        public virtual void WriteHeartToPlc()
        {
            try
            {
                if (UsedPool)
                {
                    UpHeartBreath.MyValue = (short)(UpHeartBreath.MyValue >= 255 ? 1 : (UpHeartBreath.MyValue + 1));
                    var _st_r = Stopwatch.StartNew();

                    var ClientID = 0L;
                    try
                    {
                        using (var mini = new CommonMiniProfiler("BasePlcWriteHeartToPlcAsync"))
                        {
                            using (var step = mini.Step("WriteAsync"))
                            {
                                Write(WriteToPlcHeartVal, UpHeartBreath.MyValue);
                            }
                            _st_r.Stop();
                            if (_st_r.Elapsed.TotalMilliseconds > GlobalConfig.WriteHeartInterval)
                            {
                                LogHelper.Error($"{this.PLCNo}  ClientID:{ClientID} [BASE] 单个方法：写入心跳变量间隔时间：{_st_r.Elapsed.TotalMilliseconds}毫秒；" +
                                                $"超过设置时间间隔{GlobalConfig.WriteHeartInterval}毫秒；性能： {mini.GetInfo()}", "Read");
                            }
                            if (UseSaveCSV)
                            {
                                CommonMiniProfiler.SaveCSV(mini.GetInfoData(), BenchmarkInfoPath, this.PLCNo + "BasePlcWriteHeartToPlcAsync");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    finally
                    {
                        _st_r.Stop();
                    }

                }
                else
                {
                    UpHeartBreath.MyValue = (short)(UpHeartBreath.MyValue >= 255 ? 1 : (UpHeartBreath.MyValue + 1));
                    var tiembefor = DateTime.Now;
                    operationWriteHeart.Write(WriteToPlcHeartVal, UpHeartBreath.MyValue);
                    var InternalTime = DateTime.Now.Subtract(tiembefor).TotalMilliseconds;
                    if (InternalTime > GlobalConfig.WriteHeartInterval)
                    {
                        LogHelper.Error($"{this.PLCNo} [BASE] 单个方法：写入心跳变量间隔时间：{InternalTime}毫秒；超过设置时间间隔{GlobalConfig.WriteHeartInterval}毫秒；", "Read");
                    }
                }
                WriteHeartToPlcErrorTick = 0;
                OnLineStatus = OnLineStatus.OnLine;
            }
            catch (Exception ex)
            {
                WriteHeartToPlcErrorTick++;
                var err = $"【指令】【{this}】[BASE] 上位机写心跳信号（{UpHeartBreath.MyValue}）时发生异常：{ex} Tick:{WriteHeartToPlcErrorTick}";
                if (WriteHeartToPlcErrorTick > 0)
                {
                    OnLineStatus = OnLineStatus.OffLine;
                }
                throw new Exception(err);
            }
        }
        /// <summary>
        /// 给PLC写心跳信号
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task WriteHeartToPlcAsync()
        {
            try
            {
                if (UsedPool)
                {
                    UpHeartBreath.MyValue = (short)(UpHeartBreath.MyValue >= 255 ? 1 : (UpHeartBreath.MyValue + 1));
                    var _st_r = Stopwatch.StartNew();

                    var ClientID = 0L;
                    try
                    {
                        using (var mini = new CommonMiniProfiler("PlcWriteHeartToPlcAsync"))
                        {
                            using (var step = mini.Step("WriteAsync"))
                            {

                                //await proxy.WriteAsync(WriteToPlcHeartVal, UpHeartBreath.MyValue);

                                //using (var proxy = CommunicationPool.Get())
                                //{
                                //    //var int32 = Convert.ToInt32(UpHeartBreath.MyValue);
                                //    proxy.Value.Write(WriteToPlcHeartVal, UpHeartBreath.MyValue);
                                //}
                                //operationBase.Write(WriteToPlcHeartVal, Convert.ToInt32(UpHeartBreath.MyValue) );
                            }
                            _st_r.Stop();
                            if (_st_r.Elapsed.TotalMilliseconds > GlobalConfig.WriteHeartInterval)
                            {
                                LogHelper.Error($"{this.PLCNo} ClientID:{ClientID} 单个方法：写入心跳变量间隔时间：{_st_r.Elapsed.TotalMilliseconds}毫秒；" +
                                                $"超过设置时间间隔{GlobalConfig.WriteHeartInterval}毫秒；性能： {mini.GetInfo()}", "Read");
                            }
                            if (UseSaveCSV)
                            {
                                CommonMiniProfiler.SaveCSV(mini.GetInfoData(), BenchmarkInfoPath, this.PLCNo + "PlcWriteHeartToPlcAsync");
                            }
                        }



                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    finally
                    {
                        _st_r.Stop();
                    }

                }
                else
                {
                    UpHeartBreath.MyValue = (short)(UpHeartBreath.MyValue >= 255 ? 1 : (UpHeartBreath.MyValue + 1));
                    var tiembefor = DateTime.Now;
                    operationWriteHeart.Write(WriteToPlcHeartVal, UpHeartBreath.MyValue);
                    var InternalTime = DateTime.Now.Subtract(tiembefor).TotalMilliseconds;
                    if (InternalTime > GlobalConfig.WriteHeartInterval)
                    {
                        LogHelper.Error($"{this.PLCNo} 单个方法：写入心跳变量间隔时间：{InternalTime}毫秒；超过设置时间间隔{GlobalConfig.WriteHeartInterval}毫秒；", "Read");
                    }
                }
                WriteHeartToPlcErrorTick = 0;
                OnLineStatus = OnLineStatus.OnLine;
            }
            catch (Exception ex)
            {
                WriteHeartToPlcErrorTick++;
                var err = $"【指令】【{this}】上位机写心跳信号（{UpHeartBreath.MyValue}）时发生异常：{ex} Tick:{WriteHeartToPlcErrorTick}";
                if (WriteHeartToPlcErrorTick > 0)
                {
                    OnLineStatus = OnLineStatus.OffLine;
                }
                throw new Exception(err);
            }
        }
        /// <summary>
        /// 给PLC写心跳信号 使用基础连接
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task WriteHeartToPlcAsyncBase()
        {
            try
            {
                if (UsedPool)
                {
                    UpHeartBreath.MyValue = (short)(UpHeartBreath.MyValue >= 255 ? 1 : (UpHeartBreath.MyValue + 1));
                    var _st_r = Stopwatch.StartNew();

                    var ClientID = 0L;
                    try
                    {
                        using (var mini = new CommonMiniProfiler("BasePlcWriteHeartToPlcAsync"))
                        {
                            using (var step = mini.Step("WriteAsync"))
                            {

                                //await proxy.WriteAsync(WriteToPlcHeartVal, UpHeartBreath.MyValue);

                                //using (var proxy = CommunicationPoolBase.Get())
                                //{
                                //    //var int32 = Convert.ToInt32(UpHeartBreath.MyValue);
                                //    proxy.Value.Write(WriteToPlcHeartVal, UpHeartBreath.MyValue);
                                //}
                                //operationBase.Write(WriteToPlcHeartVal, Convert.ToInt32(UpHeartBreath.MyValue) );
                            }
                            _st_r.Stop();
                            if (_st_r.Elapsed.TotalMilliseconds > GlobalConfig.WriteHeartInterval)
                            {
                                LogHelper.Error($"{this.PLCNo}  ClientID:{ClientID} [BASE] 单个方法：写入心跳变量间隔时间：{_st_r.Elapsed.TotalMilliseconds}毫秒；" +
                                                $"超过设置时间间隔{GlobalConfig.WriteHeartInterval}毫秒；性能： {mini.GetInfo()}", "Read");
                            }
                            if (UseSaveCSV)
                            {
                                CommonMiniProfiler.SaveCSV(mini.GetInfoData(), BenchmarkInfoPath, this.PLCNo + "BasePlcWriteHeartToPlcAsync");
                            }
                        }



                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    finally
                    {
                        _st_r.Stop();
                    }

                }
                else
                {
                    UpHeartBreath.MyValue = (short)(UpHeartBreath.MyValue >= 255 ? 1 : (UpHeartBreath.MyValue + 1));
                    var tiembefor = DateTime.Now;
                    operationWriteHeart.Write(WriteToPlcHeartVal, UpHeartBreath.MyValue);
                    var InternalTime = DateTime.Now.Subtract(tiembefor).TotalMilliseconds;
                    if (InternalTime > GlobalConfig.WriteHeartInterval)
                    {
                        LogHelper.Error($"{this.PLCNo} [BASE] 单个方法：写入心跳变量间隔时间：{InternalTime}毫秒；超过设置时间间隔{GlobalConfig.WriteHeartInterval}毫秒；", "Read");
                    }
                }
                WriteHeartToPlcErrorTick = 0;
                OnLineStatus = OnLineStatus.OnLine;
            }
            catch (Exception ex)
            {
                WriteHeartToPlcErrorTick++;
                var err = $"【指令】【{this}】[BASE] 上位机写心跳信号（{UpHeartBreath.MyValue}）时发生异常：{ex} Tick:{WriteHeartToPlcErrorTick}";
                if (WriteHeartToPlcErrorTick > 0)
                {
                    OnLineStatus = OnLineStatus.OffLine;
                }
                throw new Exception(err);
            }
        }
        /// <summary>
        /// 给PLC写屏蔽信号
        /// </summary>
        /// <returns></returns>
        public virtual void WriteMesStatusToPlc(int flag)
        {
            try
            {
                if (UsedPool)
                {
                    //using (var proxy = CommunicationPool.Get())
                    //{
                    //    if (communicType == CommunicType.BeckhoffAds6)
                    //    {
                    //        proxy.Value.Write(".stMesStatus.ar_iMesDown[20]", (short)flag);
                    //    }
                    //    else
                    //    {
                    //        proxy.Value.Write("stMesStatus.ar_iMesDown[20]", (short)flag);

                    //    }
                    //}
                }
                else
                {
                    if (communicType == CommunicType.BeckhoffAds6)
                    {
                        operationWriteHeart.Write(".stMesStatus.ar_iMesDown[20]", (short)flag);
                    }
                    else
                    {
                        operationWriteHeart.Write("stMesStatus.ar_iMesDown[20]", (short)flag);

                    }
                }

            }
            catch (Exception ex)
            {
                var err = $"【指令】【{this}】上位机写mes状态信号（{flag}）时发生异常：{ex}";
                LogHelper.Error(err, "plc");
            }
        }
        /// <summary>
        /// 给PLC写屏蔽信号
        /// </summary>
        /// <returns></returns>
        public virtual async Task WriteMesStatusToPlcAsync(int flag)
        {
            try
            {
                //_semaphoreWrite.Value.WaitOne();
                if (UsedPool)
                {
                    //using (var proxy = CommunicationPool.Get())
                    //{
                    //    if (communicType == CommunicType.BeckhoffAds6)
                    //    {
                    //        proxy.Value.Write(".stMesStatus.ar_iMesDown[20]", (short)flag);
                    //    }
                    //    else
                    //    {
                    //        proxy.Value.Write("stMesStatus.ar_iMesDown[20]", (short)flag);

                    //    }
                    //}
                }
                else
                {
                    if (communicType == CommunicType.BeckhoffAds6)
                    {
                        operationWriteHeart.Write(".stMesStatus.ar_iMesDown[20]", (short)flag);
                    }
                    else
                    {
                        operationWriteHeart.Write("stMesStatus.ar_iMesDown[20]", (short)flag);

                    }
                }



            }
            catch (Exception ex)
            {
                var err = $"【指令】【{this}】上位机写mes状态信号（{flag}）时发生异常：{ex}";
                LogHelper.Error(err, "plc");
            }
            finally
            {
                //_semaphoreWrite.Value.Release();
            }
        }

        /// <summary>
        /// 启动或者停止
        /// </summary>
        public abstract void OnRunStatusChange();


        /// <summary>
        /// 重试
        /// </summary>
        public ResiliencePipeline WritePolly = CommonPolly.Creat(10, TimeSpan.FromSeconds(2), args =>
        {

            LogHelper.Debug($"写入plc变量后重试第{args.AttemptNumber}次", "plc");
            return true;
        });
        /// <summary>
        /// 写入PLC
        /// </summary>
        /// <param name="variableName">变量名称</param>
        /// <param name="writeData">值</param>
        public void WriteAndCheck<T>(string variableName, T writeData)
        {
            try
            {
                //_semaphoreWrite.Value.WaitOne();
                bool DownSuccess = false; // 下发成功标志
                int iFaultCount = 0;


                try
                {
                    WritePolly.Execute(() =>
                    {
                        //using (var proxy = CommunicationPool.Get())
                        //{

                        //    proxy.Value.Write(variableName, writeData);
                        //    Thread.CurrentThread.Join(TimeSpan.FromMilliseconds(20));
                        //    var data = proxy.Value.Read<T>(variableName);
                        //    if (!writeData.Equals(data))
                        //    {
                        //        var msg = $"写入plc值与读取值不一致,变量：【{variableName}】，下发值：【{writeData}】；读取值：【{data}】";
                        //        LogHelper.Debug(msg, "plc");
                        //        throw new Exception(msg);
                        //    }
                        //}
                    });
                }
                catch (Exception e)
                {
                    var msg = $"PLC变量：{variableName}写入失败！错误提示【{e}】";
                    LogHelper.Error(msg, "plc");
                    throw;
                }

                //while (!DownSuccess)
                //{
                //    try
                //    {
                //        var res = CommunicationPool.GetRes();
                //        var plc = res.Value;
                //        using (var proxy = CommunicationPool.Get())
                //        {
                //            proxy.Write(variableName, writeData);
                //        }
                //        DownSuccess = true;
                //        //SaveSignal(variableName, writeData, 1);
                //        //Thread.Sleep(30);
                //        this.OnLineStatus = OnLineStatus.OnLine;
                //    }
                //    catch (Exception ex)
                //    {
                //        if (iFaultCount > PLCWriteTryMaxCount)
                //        {
                //            ////this.OnLineStatus = OnLineStatus.OffLine;
                //            var msg = $"PLC变量：{variableName}写入失败{iFaultCount}次！错误提示【{ex}】";
                //            LogHelper.Error(msg, "plc");
                //            throw new Exception(msg);
                //        }
                //        iFaultCount++;
                //        //Thread.Sleep(100);
                //        Thread.CurrentThread.Join(100);
                //    }
                //}
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //_semaphoreWrite.Value.Release();
            }

        }



        /// <summary>
        /// 写入PLC
        /// </summary>
        /// <param name="variableName">变量名称</param>
        /// <param name="writeData">值</param>
        public void Write<T>(string variableName, T writeData)
        {
            try
            {
                //_semaphoreWrite.Value.WaitOne();
                bool DownSuccess = false; // 下发成功标志
                int iFaultCount = 0;
                while (!DownSuccess)
                {
                    try
                    {
                        if (UsedPool)
                        {
                            //using (var proxy = CommunicationPool.Get())
                            //{
                            //    proxy.Value.Write(variableName, writeData);
                            //}
                        }
                        else
                        {
                            operationWrite.Write(variableName, writeData);
                        }

                        DownSuccess = true;
                        //SaveSignal(variableName, writeData, 1);
                        //Thread.Sleep(30);
                        this.OnLineStatus = OnLineStatus.OnLine;
                    }
                    catch (Exception ex)
                    {
                        if (iFaultCount > PLCWriteTryMaxCount)
                        {
                            ////this.OnLineStatus = OnLineStatus.OffLine;
                            var msg = $"PLC变量：{variableName}写入失败{iFaultCount}次！错误提示【{ex}】";
                            LogHelper.Error(msg, "plc");
                            throw new Exception(msg);
                        }
                        iFaultCount++;
                        //Thread.Sleep(100);
                        Thread.CurrentThread.Join(100);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //_semaphoreWrite.Value.Release();
            }

        }

        /// <summary>
        /// 异步写入PLC
        /// </summary>
        /// <param name="variableName">变量名称</param>
        /// <param name="writeData">值</param>
        public async Task WriteAsync<T>(string variableName, T writeData)
        {
            try
            {
                //_semaphoreWrite.Value.WaitOne();
                bool DownSuccess = false; // 下发成功标志
                int iFaultCount = 0;
                while (!DownSuccess)
                {
                    try
                    {
                        if (UsedPool)
                        {
                            //using (var proxy = CommunicationPool.Get())
                            //{
                            //    proxy.Value.Write(variableName, writeData);
                            //}
                        }
                        else
                        {
                            await operationWrite.WriteAsync(variableName, writeData);
                        }

                        DownSuccess = true;
                        //SaveSignal(variableName, writeData, 1);
                        //Thread.Sleep(30);
                        this.OnLineStatus = OnLineStatus.OnLine;
                    }
                    catch (Exception ex)
                    {
                        //this.OnLineStatus = OnLineStatus.OffLine;

                        if (iFaultCount > PLCWriteTryMaxCount)
                        {
                            var msg = $"PLC变量：{variableName}写入失败{iFaultCount}次！错误提示【{ex}】";
                            throw new Exception(msg);
                        }

                        iFaultCount++;
                        await Task.Delay(100);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //_semaphoreWrite.Value.Release();
            }

        }

        /// <summary>
        /// 读取PLC变量
        /// </summary>
        /// <typeparam name="T">变量类型</typeparam>
        /// <param name="ReadVariable">变量名称</param>
        /// <returns></returns>
        //public T Read<T>(string ReadVariable)
        //{
        //    return operation.Read<T>(ReadVariable);
        //}

        //public T Read<T>(string ReadVariable, int length = 0)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            //using (var proxy = CommunicationPool.Get())
        //            //{
        //            //    var data = proxy.Value.Read<T>(ReadVariable, length);
        //            //    return data;
        //            //}
        //        }
        //        else
        //        {
        //            var data = operationRead.Read<T>(ReadVariable, length);
        //            return data;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}


        /// <summary>
        /// 读取PLC变量异步
        /// </summary>
        /// <typeparam name="T">变量类型</typeparam>
        /// <param name="ReadVariable">变量名称</param>
        /// <returns></returns>
        //public T Read<T>(string ReadVariable)
        //{
        //    return operation.Read<T>(ReadVariable);
        //}

        //public async Task<T> ReadAsync<T>(string ReadVariable, int length = 0)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            //using (var proxy = CommunicationPool.Get())
        //            //{
        //            //    var data = await proxy.Value.ReadAsync<T>(ReadVariable, length);
        //            //    return data;
        //            //}
        //        }
        //        else
        //        {
        //            var data = await operationRead.ReadAsync<T>(ReadVariable, length);
        //            return data;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}
        private int ReadExcuteAsyncTick = 0;
        //public async Task<T> ReadExcuteAsync<T>(string ReadVariable, int length = 0)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            //using (var proxy = CommunicationPool.Get())
        //            //{
        //            //    var data = await proxy.Value.ReadAsync<T>(ReadVariable, length);
        //            //    LogHelper.Debug($"变量名：{ReadVariable}，扫描值：{JsonConvert.SerializeObject(data)}", "ReadExcuteAsync");
        //            //    return data;
        //            //}
        //        }
        //        else
        //        {
        //            var _readExcuteAsyncTick = Interlocked.Increment(ref ReadExcuteAsyncTick);
        //            if (_readExcuteAsyncTick % 2 == 0)
        //            {
        //                var data = await operationReadHeart.ReadAsync<T>(ReadVariable, length);
        //                LogHelper.Debug($"变量名：{ReadVariable}，扫描值：{JsonConvert.SerializeObject(data)}", "ReadExcuteAsync");
        //                return data;
        //            }
        //            else
        //            {
        //                var data = await operationWriteHeart.ReadAsync<T>(ReadVariable, length);
        //                LogHelper.Debug($"变量名：{ReadVariable}，扫描值：{JsonConvert.SerializeObject(data)}", "ReadExcuteAsync");
        //                return data;
        //            }
        //        }


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}
        //public async Task<T> ReadExcuteAsync<T>(string ReadVariable)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            //using (var proxy = CommunicationPool.Get())
        //            //{
        //            //    var data = await proxy.Value.ReadAsync<T>(ReadVariable);
        //            //    LogHelper.Debug($"变量名：{ReadVariable}，ClientID :{proxy.Value.ClientID} 扫描值：{JsonConvert.SerializeObject(data)}", "ReadExcuteAsync");
        //            //    return data;
        //            //}
        //        }
        //        else
        //        {
        //            var _readExcuteAsyncTick = Interlocked.Increment(ref ReadExcuteAsyncTick);
        //            if (_readExcuteAsyncTick % 2 == 0)
        //            {
        //                var data = await operationReadHeart.ReadAsync<T>(ReadVariable);
        //                LogHelper.Debug($"变量名：{ReadVariable}，扫描值：{JsonConvert.SerializeObject(data)}", "ReadExcuteAsync");
        //                return data;
        //            }
        //            else
        //            {
        //                var data = await operationWriteHeart.ReadAsync<T>(ReadVariable);
        //                LogHelper.Debug($"变量名：{ReadVariable}，扫描值：{JsonConvert.SerializeObject(data)}", "ReadExcuteAsync");
        //                return data;
        //            }
        //        }


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}
        public T ReadScan<T>(string ReadVariable, int length = 0)
        {
            try
            {
                //_semaphoreRead.Value.WaitOne();
                if (UsedPool)
                {
                    //var res = CommunicationPool.GetRes();
                    //var plc = res.Value;
                    //using (var proxy = CommunicationPool.Get())
                    //{
                    //    var data = proxy.Read<T>(ReadVariable, length);
                    //    return data;
                    //}
                    var data = operationReadScan.Read<T>(ReadVariable, length);
                    return data;
                }
                else
                {
                    var data = operationReadScan.Read<T>(ReadVariable, length);
                    return data;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // _semaphoreRead.Value.Release();
            }
        }
        public T ReadScan<T>(string ReadVariable)
        {
            try
            {
                //_semaphoreRead.Value.WaitOne();
                if (UsedPool)
                {
                    //var res = CommunicationPool.GetRes();
                    //var plc = res.Value;
                    //using (var proxy = CommunicationPool.Get())
                    //{
                    //    var data = proxy.Read<T>(ReadVariable, length);
                    //    return data;
                    //}
                    var data = operationReadScan.Read<T>(ReadVariable);
                    return data;
                }
                else
                {
                    var data = operationReadScan.Read<T>(ReadVariable);
                    return data;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //_semaphoreRead.Value.Release();
            }
        }
        public async Task<T> ReadScanAsync<T>(string ReadVariable, int length = 0)
        {
            try
            {
                // _semaphoreRead.Value.WaitOne();
                if (UsedPool)
                {
                    //var res = CommunicationPool.GetRes();
                    //var plc = res.Value;
                    //using (var proxy = CommunicationPool.Get())
                    //{

                    //    var data = await proxy.ReadAsync<T>(ReadVariable, length);
                    //    return data;
                    //}
                    var data = await operationReadScan.ReadAsync<T>(ReadVariable, length);
                    return data;
                }
                else
                {
                    var data = await operationReadScan.ReadAsync<T>(ReadVariable, length);
                    return data;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // _semaphoreRead.Value.Release();
            }
        }
        public async Task<T> ReadScanAsync<T>(string ReadVariable)
        {
            try
            {
                // _semaphoreRead.Value.WaitOne();
                if (UsedPool)
                {
                    //var res = CommunicationPool.GetRes();
                    //var plc = res.Value;
                    //using (var proxy = CommunicationPool.Get())
                    //{

                    //    var data = await proxy.ReadAsync<T>(ReadVariable, length);
                    //    return data;
                    //}
                    var data = await operationReadScan.ReadAsync<T>(ReadVariable);
                    return data;
                }
                else
                {
                    var data = await operationReadScan.ReadAsync<T>(ReadVariable);
                    return data;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // _semaphoreRead.Value.Release();
            }
        }
        //public T Read<T>(string ReadVariable)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsePool)
        //        {
        //            using (var proxy = CommunicationPool.Get())
        //            {
        //                var data = proxy.Value.Read<T>(ReadVariable);
        //                return data;
        //            }
        //        }
        //        else
        //        {
        //            var data = operationRead.Read<T>(ReadVariable);
        //            return data;
        //        }
        //        //SaveSignal(ReadVariable, data, 0);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}

        public T Read<T>(string ReadVariable)
        {
            try
            {

                bool DownSuccess = false; // 读取成功标志
                int iFaultCount = 0;
                while (!DownSuccess)
                {
                    try
                    {
                        if (UsedPool)
                        {
                            //using (var proxy = CommunicationPool.Get())
                            //{
                            //    var data = proxy.Value.Read<T>(ReadVariable);
                            //    DownSuccess = true;
                            //    return data;
                            //}
                        }
                        else
                        {
                            var data = operationRead.Read<T>(ReadVariable);
                            DownSuccess = true;
                            return data;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (iFaultCount > PLCWriteTryMaxCount)
                        {
                            var msg = $"PLC变量：{ReadVariable}读取失败{iFaultCount}次！错误提示【{ex}】";
                            LogHelper.Error(msg, "plc");
                            throw new Exception(msg);
                        }
                        iFaultCount++;
                        Thread.CurrentThread.Join(50);
                    }

                }
                return default;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                //_semaphoreRead.Value.Release();
            }
        }

        /// <summary>
        /// 读取多次
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ReadVariable"></param>
        /// <returns></returns>
        public T ReadTimes<T>(string ReadVariable)
        {
            try
            {

                bool DownSuccess = false; // 下发成功标志
                int iFaultCount = 0;
                while (!DownSuccess)
                {
                    try
                    {
                        if (UsedPool)
                        {
                            //using (var proxy = CommunicationPool.Get())
                            //{
                            //    var data = proxy.Value.Read<T>(ReadVariable);
                            //    DownSuccess = true;
                            //    return data;
                            //}
                        }
                        else
                        {
                            var data = operationRead.Read<T>(ReadVariable);
                            DownSuccess = true;
                            return data;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (iFaultCount > PLCWriteTryMaxCount)
                        {
                            var msg = $"PLC变量：{ReadVariable}读取失败{iFaultCount}次！错误提示【{ex}】";
                            LogHelper.Error(msg, "plc");
                            throw new Exception(msg);
                        }
                        iFaultCount++;
                        Thread.CurrentThread.Join(50);
                    }

                }
                return default;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                //_semaphoreRead.Value.Release();
            }
        }

        //public async Task<T> ReadAsync<T>(string ReadVariable)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            //using (var proxy = CommunicationPool.Get())
        //            //{
        //            //    var data = await proxy.Value.ReadAsync<T>(ReadVariable);
        //            //    return data;
        //            //}
        //        }
        //        else
        //        {
        //            return await operationRead.ReadAsync<T>(ReadVariable);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}
        //public string[] ReadString(string variable, int startIdx, int length)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            using (var proxy = CommunicationPool.Get())
        //            {
        //                var data = proxy.Value.ReadString(variable, startIdx, length);
        //                return data;
        //            }
        //        }
        //        else
        //        {
        //            return operationRead.ReadString(variable, startIdx, length);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}
        //public string ReadString(string ReadVariable)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            //using (var proxy = CommunicationPool.Get())
        //            //{
        //            //    var data = proxy.Value.ReadString(ReadVariable);
        //            //    return data;
        //            //}
        //        }
        //        else
        //        {
        //            return operationRead.ReadString(ReadVariable);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}
        //public object[] ReadArrayString(string[] variable)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            //using (var proxy = CommunicationPool.Get())
        //            //{
        //            //    var data = proxy.Value.ReadArrayString(variable);
        //            //    return data;
        //            //}
        //        }
        //        else
        //        {
        //            return operationRead.ReadArrayString(variable);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}
        //public async Task<object[]> ReadArrayStringAsync(string[] variable)
        //{
        //    try
        //    {
        //        //_semaphoreRead.Value.WaitOne();
        //        if (UsedPool)
        //        {
        //            //using (var proxy = CommunicationPool.Get())
        //            //{
        //            //    var data = await proxy.Value.ReadArrayStringAsync(variable);
        //            //    return data;
        //            //}
        //        }
        //        else
        //        {
        //            return await operationRead.ReadArrayStringAsync(variable);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        //_semaphoreRead.Value.Release();
        //    }
        //}
        private PLCSignalModel signal = new PLCSignalModel();
        /// <summary>
        /// 信号交互页面保存信号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SaveSignal(string name, dynamic data, int type)
        {
            try
            {
                signal.name = name;
                signal.data = data;
                signal.type = type;
                SignalChanged?.Invoke(this, signal);
            }
            catch (Exception ex)
            {
                LogHelper.Error($"信号交互页面保存plc数据失败：【{ex.ToString()}】", "SaveSignal");
            }

        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="t">线程</param>
        /// <returns></returns>
        protected string TakeTaskExceptionMsg(Task t)
        {
            var em = t.Exception == null ? string.Empty : t.Exception.Message;
            var msg = string.Empty;
            if (t.Exception.InnerException != null)
                t.Exception.InnerExceptions.ToList().ForEach(m => { msg += m.Message; });
            return em + ":" + msg;
        }

        /// <summary>
        /// 重写
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{PLCNo}]";
        }
        /// <summary>
        /// 获取连接池信息
        /// </summary>
        /// <returns></returns>
        public string GetPoolInfo()
        {
            if (UsedPool)
            {
                var sb = new StringBuilder();
                sb.AppendLine("===========Start=============");
                sb.AppendLine("CommunicationPool");
                //sb.AppendLine($"KEY:{CommunicationPool.Key}");
                //sb.AppendLine(CommunicationPool.StatisticsFullily);
                //sb.AppendLine("========================");
                //sb.AppendLine("CommunicationPoolBase");
                //sb.AppendLine($"KEY:{CommunicationPoolBase.Key}");
                //sb.AppendLine(CommunicationPoolBase.StatisticsFullily);
                sb.AppendLine("============END============");
                return sb.ToString();
            }
            else
            {

                return "";
            }
        }
        /// <summary>
        /// 自动清理连接池
        /// </summary>
        public void AutoClean()
        {
            //if (UsedPool)
            //{
            //    CommunicationPoolBase.AutoClean(3600 * 2);
            //    CommunicationPool.AutoClean(3600 * 2);
            //}
        }
        /// <summary>
        /// 自动测试连接池所有链接
        /// </summary>
        public void AutoPing()
        {
            if (UsedPool)
            {
                //CommunicationPoolBase.AutoPing();
                //CommunicationPool.AutoPing();
            }

        }

    }
}
