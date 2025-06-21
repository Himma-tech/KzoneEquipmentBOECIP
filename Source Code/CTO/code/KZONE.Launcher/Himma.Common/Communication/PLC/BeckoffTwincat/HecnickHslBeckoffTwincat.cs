using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.Xml;
using System.IO;
using Prism.Common;
using System.Diagnostics;
using System.Security.Cryptography;
using SqlSugar;
using System.Collections.Concurrent;
using Himma.Common.Log;
using Himma.Common.Communication.Interface;

namespace Himma.Common.Communication.PLC.BeckoffTwincat
{
    ///// <summary>
    ///// 倍福PLC
    ///// </summary>
    //public class HecnickHslBeckoffTwincat : IHimmaPLC, IPLC
    //{
    //    /// <summary>
    //    /// 异步事件推送
    //    /// </summary>
    //    IAsyncPublisher<Com.Common.MessageType, PLCVariableModel> _asyncPublisher = GlobalMessagePipe.GetAsyncPublisher<Com.Common.MessageType, PLCVariableModel>();

    //    private readonly ConcurrentQueue<(string variableName, short value)> _updateQueue = new ConcurrentQueue<(string, short)>();
    //    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    //    private int _UpdatePLCVariableTime = GlobalConfig.UpdatePLCVariableTime;
    //    #region                   
    //    private string _PLC_HeartBeat_Write = ".D3[1]";
    //    private string _PLC_HeartBeat_Read = ".D3[2]";
    //    private string logname = "Plc";
    //    private string logname2 = "plcVariableCheck";
    //    private string _logName = "BeckoffPlcSubscribe";
    //    private string _LogName = "BeckoffPlc";
    //    DateTime startTime, endTime = DateTime.Now;
    //    public BeckhoffAdsClient communicRead = new BeckhoffAdsClient();
    //    public BeckhoffAdsClient communicWrite = new BeckhoffAdsClient();
    //    public BeckhoffAdsClient communicReadScan = new BeckhoffAdsClient();
    //    private List<BasPLCVariableModel> _basPLCVariableModels = new List<BasPLCVariableModel>();
    //    private BasPLCVariableDAL _basPLCVariableDAL = new BasPLCVariableDAL();
    //    private BasParaVariableDAL _basParaVariableDal = new BasParaVariableDAL();
    //    private BasSignalInfoDAL _basSignalInfoDAL = new BasSignalInfoDAL();
    //    private List<BasSignalInfoModel> _basSignalInfoModels = new List<BasSignalInfoModel>();
    //    /// <summary>
    //    /// 监控变量
    //    /// </summary>
    //    private ObservableCollections.ObservableList<PLCVariableModel> _monitorPLCVariables = new ObservableCollections.ObservableList<PLCVariableModel>();
    //    private List<ObservableCollection<PLCVariableModel>> _monitorPLCVariableList = new List<ObservableCollection<PLCVariableModel>>();
    //    /// <summary>
    //    /// 读写变量
    //    /// </summary>
    //    private List<PLCVariableModel> _rwVariablePLCModels = new List<PLCVariableModel>();
    //    private int _cycleTime;
    //    private IEventAggregator _eventAggregator;
    //    private readonly TaskFactory _taskFactory = new TaskFactory();
    //    private int _upperToPlcHeart = 0;
    //    public PLCObservableCollection<int> UpperToPlc { get; set; } = new PLCObservableCollection<int>(); //给PLC返回的int值变量
    //    public PLCObservableCollection<float> UpperToPlcFloatData { get; set; } = new PLCObservableCollection<float>();//给PLC返回的float变量
    //    public ObservableCollection<int> PLCVariables { get; set; } //PLC的触发值
    //    public ObservableCollection<int> UpperToPlcRead = new ObservableCollection<int>(); //回读PLC变量值，用于校验写入是否正确
    //    public List<BasParaVariableModel> _paraList { get; set; }
    //    public List<BasParaVariableModel> ProductionVariableList { get; set; }
    //    public List<BasParaVariableModel> WindFlawCoordinateList { get; set; }
    //    public List<BasParaVariableModel> RealTimeVariableList { get; set; }
    //    public List<BasParaVariableModel> _monitorVariableList { get; set; }
    //    public string[] ArrayName { get; set; }
    //    public string ProcessNo { get; set; }
    //    public string StationCode { get; set; }
    //    public string ProcessCode { get; set; }
    //    public string EqptNo { get; set; }
    //    public int Port { get; set; }
    //    public string Ip { get; set; }
    //    public int Timeout { get; set; }
    //    private string _name = "";
    //    public string Name
    //    {
    //        get { return _name; }
    //        set
    //        {
    //            _name = value;

    //        }
    //    }
    //    private bool _isConnected = false;
    //    /// <summary>
    //    /// 是否连接
    //    /// </summary>
    //    public bool IsConnected
    //    {
    //        get { return _isConnected; }
    //        set
    //        {
    //            _isConnected = value;
    //            _eventAggregator.GetEvent<BasComponentMsg>().Publish(new BasComponentMsg()//通知
    //            {
    //                ComponentName = Name,
    //                ComponentState = value
    //            });
    //        }
    //    }
    //    /// <summary>
    //    /// PLC当前心跳值
    //    /// </summary>
    //    public ClassVarEvent<short> PlcHeartBreath = new ClassVarEvent<short>();
    //    /// <summary>
    //    /// PLC当前心跳值 订阅变量
    //    /// </summary>
    //    public ClassVarEvent<short> PlcHeartBreath_Notification = new ClassVarEvent<short>();
    //    /// <summary>
    //    /// PLC当前心跳值 写入 订阅变量
    //    /// </summary>
    //    public ClassVarEvent<short> PlcHeartWriteBreath_Notification = new ClassVarEvent<short>();
    //    public event Action<object, object> PLCNotifyEventArgs;
    //    public event EventHandler<PLCSignalModel> SignalChanged;
    //    public event Action<int, object> EvtNotificationEx;
    //    #endregion
    //    public void Active()
    //    {
    //        //var retResultRead = communicRead.Connect(Ip, Port, 500);
    //        //var retResultWrite = communicWrite.Connect(Ip, Port, 500);
    //        //var retResultReadHeart = communicReadHeart.Connect(Ip, Port, 500);
    //        //var retResultWriteHeart = communicWriteHeart.Connect(Ip, Port, 500);
    //        //if (!retResultRead.IsSuccess || !retResultWrite.IsSuccess || !retResultWriteHeart.IsSuccess || !retResultWriteHeart.IsSuccess)
    //        //{
    //        //    LogHelper.Error($"PLC重连失败:{Ip}", logname);
    //        //}
    //        //else
    //        //{
    //        //    IsConnected = true;
    //        //    LogHelper.Info($"PLC重连成功:{Ip}", logname);
    //        //}

    //    }
    //    public bool DisActive()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public void IniPLCAds(DataTable variableDataTable)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public void Initial(string ip, int port, int timeout)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public HecnickHslBeckoffTwincat(IEventAggregator eventAggregator) : base()
    //    {
    //        _eventAggregator = eventAggregator;
    //    }
    //    /// <summary>
    //    /// PLC连接
    //    /// </summary>
    //    /// <param name="ipdress">地址</param>
    //    /// <param name="port">端口,欧姆龙可以置0，CIP无影响</param>
    //    /// <param name="cycleTime">循环时间</param>
    //    public void PLCInit(string ipdress, int port, int cycleTime = 80, string name = "")
    //    {
    //        try
    //        {
    //            Ip = ipdress;
    //            Port = port;
    //            IsConnected = false;
    //            PlcHeartBreath.MyValue = 1;
    //            var adsstring = $"{ipdress}.1.1";
    //            var retResult = communicRead.Connect(adsstring, port, GlobalVariable.LCCReadTimeOut);
    //            var retResult2 = communicWrite.Connect(adsstring, port, GlobalVariable.LCCReadTimeOut);
    //            var retResult3 = communicReadScan.Connect(adsstring, port, GlobalVariable.LCCReadTimeOut);
    //            IsConnected = retResult.IsSuccess && retResult2.IsSuccess && retResult3.IsSuccess;
    //            if (IsConnected)
    //            {
    //                LogHelper.Info($"PLC连接成功:{ipdress} | {adsstring}", logname);
    //            }
    //            else
    //            {
    //                LogHelper.Info($"PLC连接失败:{ipdress} | {adsstring} error {retResult.Message}|{retResult2.Message}|{retResult3.Message}", logname);
    //            }
    //            InitPLCVariable();
    //            InitMoniVar();
    //            InitUnloadInfo();
    //            InitCash();
    //            InitSignal();
    //            _taskFactory.StartNew(() => { HeartBeat(name); }, TaskCreationOptions.LongRunning);//心跳               
    //        }
    //        catch (Exception e)
    //        {
    //            LogHelper.Error($"PLC连接异常:{ipdress}，{e.ToString()}");
    //        }
    //    }
    //    #region function
    //    /// <summary>
    //    /// 初始化读写变量
    //    /// </summary>
    //    private void InitPLCVariable()
    //    {
    //        _basPLCVariableModels = _basPLCVariableDAL.GetList();
    //        foreach (var item in _basPLCVariableModels.FindAll(model => model.RwFlag != 2 && model.PLCType == PlcType.BECKOFF.ToString()).ToList())
    //        {
    //            _rwVariablePLCModels.Add(new PLCVariableModel()//读写变量
    //            {
    //                VariableDescrition = item.VariableDescription,
    //                VariableName = item.VariableName,
    //                VariableHandle = item.HandleNo,
    //                VariableValue = 0
    //            });
    //        }
    //    }
    //    /// <summary>
    //    /// 获取信号名称
    //    /// </summary>
    //    private void InitSignal()
    //    {
    //        var list = _basPLCVariableModels.FindAll(model => model.RwFlag == 2 && model.PLCType == "BECKOFF");
    //        ArrayName = new string[list.Count];
    //        for (int n = 0; n < list.Count; n++)
    //        {
    //            ArrayName[n] = list[n].VariableName;
    //        }
    //    }
    //    /// <summary>
    //    /// 初始化监控变量
    //    /// </summary>
    //    private void InitMoniVar()
    //    {
    //        var list = _basPLCVariableModels.FindAll(model => model.RwFlag == 2 && model.PLCType == "BECKOFF");
    //        _basSignalInfoModels = _basSignalInfoDAL.GetList(model => model.ProcessNo == ProcessNo || model.ProcessNo == "ALL");
    //        var listnew = _basSignalInfoModels.FindAll(x => list.FirstOrDefault(y => y.VariableName == x.VariableName) != null);
    //        foreach (var item in listnew)
    //        {
    //            var _cacheobj = new PLCVariableModel()//订阅变量
    //            {
    //                VariableDescrition = item.SignalDesciption,
    //                VariableName = item.VariableName,
    //                VariableHandle = item.SignalArrayIndex,
    //                VariableDownHandle = item.SignalDownIndex,
    //                VariableValue = 0,
    //                BizType = item.BizType,
    //                ComponentSid = item.ComponentSid
    //            };
    //            _cacheobj.PropertyChanged += obj_PropertyChanged;
    //            _monitorPLCVariables.Add(_cacheobj);
    //        }
    //    }
    //    /// <summary>
    //    /// 初始化下料信息
    //    /// </summary>
    //    private void InitUnloadInfo()
    //    {
    //        var basParaVariableModels = _basParaVariableDal.GetList().FindAll(model => model.ProcessNo == ProcessNo && model.UsedFlag == 1);
    //        _paraList = basParaVariableModels.FindAll(model => model.StationName.Contains("下料工位"));
    //        RealTimeVariableList = basParaVariableModels.FindAll(model => model.UsedFlag == 1 && model.StationName == "实时数据采集工位");
    //        ProductionVariableList = basParaVariableModels.FindAll(model => model.UsedFlag == 1 && model.StationName == "产出工位");
    //        WindFlawCoordinateList = basParaVariableModels.FindAll(model => model.UsedFlag == 1 && model.StationName == "卷绕蓝标工位");
    //    }
    //    /// <summary>
    //    /// 初始化缓存变量
    //    /// </summary>
    //    private void InitCash()
    //    {
    //        for (int i = 0; i < GlobalConfig.PlcVariLenght; i++) //写入变量初始化
    //        {
    //            //PLCVariables.Add(0);
    //            UpperToPlcRead.Add(0);
    //        }
    //    }
    //    /// <summary>
    //    /// 心跳
    //    /// </summary>
    //    public async void HeartBeat(string name)
    //    {
    //        while (GlobalVariable.State)
    //        {
    //            await Task.Delay(GlobalConfig.HeartBeatInterval);
    //            //判断订阅的写入变量是否没有变化 如果没有变化 因为这个变量是上位机写的 代表着订阅可能存在失效
    //            var heatrbeatouttime = GlobalConfig.PlcHeartBreakOffOutTime;
    //            if (PlcHeartWriteBreath_Notification.TimeInterval > heatrbeatouttime)
    //            {
    //                IsConnected = false;
    //                LogHelper.Warn($"【订阅】【{this.Name}】plc的写入心跳信号超过{heatrbeatouttime}秒没有发生变化，当前读取到的心跳值是{PlcHeartWriteBreath_Notification.MyValue} 订阅可能已经断开");
    //            }
    //            var taskReadheart = RunReadHeart(name);
    //            var taskWriteheart = RunWriteHeart();
    //            await Task.WhenAll(taskReadheart, taskWriteheart);
    //        }
    //    }
    //    /// <summary>
    //    /// 写心跳
    //    /// </summary>
    //    private async Task RunWriteHeart()
    //    {
    //        try
    //        {
    //            //只有读取PLC变量正常的情况下才写入正常
    //            if (IsConnected)
    //            {
    //                _upperToPlcHeart = _upperToPlcHeart < 255 ? _upperToPlcHeart += 1 : 1;
    //                //新增JP40底部穿透--托杯交换机双开模式-写入心跳做区分-  10-C100;20-JP40;30-同开
    //                if (GlobalVariable.EqptNo == "KTZP000B")
    //                {

    //                    _upperToPlcHeart = _upperToPlcHeart < 255 ? _upperToPlcHeart += 1 : 1;
    //                    //写入PLC读取的心跳变量值
    //                    var val = communicWrite.Write(_PLC_HeartBeat_Write, _upperToPlcHeart);
    //                    if (!val.IsSuccess)
    //                    {
    //                        IsConnected = false;
    //                        LogHelper.Error($"{this.Name}plc心跳写入失败：" + val.Message, logname);
    //                    }
    //                    var heart = communicWrite.Write(".D3[3]", _upperToPlcHeart);
    //                    if (!heart.IsSuccess)
    //                    {
    //                        IsConnected = false;
    //                        LogHelper.Error($"{this.Name}plc心跳写入失败：" + heart.Message, logname);
    //                    }
    //                }
    //                else
    //                {
    //                    //写入PLC读取的心跳变量值
    //                    var val = communicWrite.Write(_PLC_HeartBeat_Write, _upperToPlcHeart);
    //                    if (!val.IsSuccess)
    //                    {
    //                        IsConnected = false;
    //                        LogHelper.Error($"{this.Name}plc心跳写入失败：" + val.Message, logname);
    //                    }
    //                    if (GlobalVariable.UpHeart == 1)
    //                    {
    //                        var heart = communicWrite.Write(".D3[5]", _upperToPlcHeart);
    //                        if (!heart.IsSuccess)
    //                        {
    //                            IsConnected = false;
    //                            LogHelper.Error($"{this.Name}plc心跳写入失败：" + heart.Message, logname);
    //                        }
    //                    }
    //                }
    //            }
    //            //IsConnected = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            await Task.Delay(GlobalConfig.HeartBeatInterval);
    //            IsConnected = false;
    //            LogHelper.Error($"{this.Name}plc心跳写入失败：" + ex, logname);
    //        }
    //    }
    //    /// <summary>
    //    /// 读心跳
    //    /// </summary>
    //    private async Task RunReadHeart(string name)
    //    {
    //        try
    //        {
    //            var val = communicRead.Read<short>(_PLC_HeartBeat_Read);
    //            if (val.IsSuccess)
    //            {
    //                PlcHeartBreath.MyValue = val.Content;
    //            }
    //            var heatrbeatouttime = GlobalConfig.PlcHeartBreakOffOutTime;
    //            if (PlcHeartBreath.TimeInterval > heatrbeatouttime)
    //            {
    //                IsConnected = false;
    //                LogHelper.Warn($"【心跳】【{this.Name}】plc的心跳信号超过{heatrbeatouttime}秒没有发生变化，当前读取到的心跳值是{PlcHeartBreath.MyValue}");
    //                //取消订阅
    //                //UnRegisterNotifications(name);
    //                //Active();                    
    //            }
    //            else
    //            {
    //                if (!IsConnected)
    //                {
    //                    IsConnected = true;
    //                    LogHelper.Info($"{this.Name}plc开启订阅");
    //                    RegisterNotifications(name);
    //                    //启动线程处理更新队列—20250311
    //                    Task.Run(() => ProcessUpdateQueue(_cancellationTokenSource.Token));
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            IsConnected = false;
    //            LogHelper.Error($"{this.Name}plc心跳读取异常：" + ex, logname);
    //        }
    //    }
    //    private Dictionary<string, int> _monitorPLCVariablesCacheDictionary = new Dictionary<string, int>();
    //    /// <summary>
    //    /// 获取变量订阅标签集合 分散
    //    /// </summary>
    //    /// <returns></returns>
    //    private void GetNotificationsArrayV2()
    //    {
    //        _monitorPLCVariablesCacheDictionary.Clear();
    //        var list = _basPLCVariableModels.FindAll(model => model.RwFlag == 2 && model.PLCType == "BECKOFF");
    //        ArrayName = new string[list.Count];
    //        for (int n = 0; n < list.Count; n++)
    //        {
    //            ArrayName[n] = list[n].VariableName;
    //        }
    //        int j = 0;
    //        var result = Read(ArrayName);
    //        for (int k = 0; k < result.Length; k++)
    //        {
    //            var data = (short[])result[k];
    //            var listsignal = _basSignalInfoModels.FindAll(x => x.VariableName == ArrayName[k]);
    //            int cacheindex = 0;
    //            for (int i = j; i < j + listsignal.Count; i++)
    //            {
    //                _monitorPLCVariables[i].VariableValue = data[_monitorPLCVariables[i].VariableHandle];
    //                var valuename = $"{ArrayName[k]}[{cacheindex}]";
    //                if (!valuename.StartsWith("."))
    //                {
    //                    valuename = "." + valuename;
    //                }
    //                if (!_monitorPLCVariablesCacheDictionary.ContainsKey(valuename))
    //                {
    //                    _monitorPLCVariablesCacheDictionary.Add(valuename, i);
    //                }
    //                cacheindex++;
    //            }
    //            j += listsignal.Count;
    //        }
    //        //添加PLC心跳订阅回读
    //        _monitorPLCVariablesCacheDictionary.Add(_PLC_HeartBeat_Read, 1000000);
    //        _monitorPLCVariablesCacheDictionary.Add(_PLC_HeartBeat_Write, 1000001);
    //    }
    //    /// <summary>
    //    /// 开始变量订阅
    //    /// </summary>
    //    private void RegisterNotifications(string name)
    //    {
    //        #region 倍福监控
    //        //启动监控     
    //        GetNotificationsArrayV2();
    //        var plcarray = _monitorPLCVariablesCacheDictionary.Keys.ToArray();
    //        try
    //        {
    //            communicReadScan.UnRegisterNotifications(plcarray);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"{name}注销plc批量标签订阅失败。{ex.ToString()}", _logName);
    //        }
    //        LogHelper.Debug($"{name}plc批量标签订阅 {string.Join("|", plcarray)}", _logName);
    //        var initregisterresult = communicReadScan.InitRegister(GlobalConfig.LCCRegister, NotificationActionHandle);
    //        var registerresult = communicReadScan.RegisterNotifications(plcarray);
    //        if (registerresult.IsSuccess & initregisterresult.IsSuccess)
    //        {
    //            LogHelper.Debug($"{name}plc批量标签订阅成功", _logName);
    //        }
    //        else
    //        {
    //            LogHelper.Error($"{name}plc批量标签订阅失败。{registerresult.Message} | {initregisterresult.Message}", _logName);
    //        }
    //        #endregion
    //    }
    //    /// <summary>
    //    /// LCC定义计数器
    //    /// </summary>
    //    private long _LCC_Trigger_Tick = 1;
    //    /// <summary>
    //    /// 订阅变量赋值耗时
    //    /// </summary>
    //    private long _BeckoffPlc_Time = 10;
    //    /// <summary>
    //    /// LCC变量监控事件_倍福
    //    /// </summary>
    //    /// <param name="subResult"></param>
    //    private void NotificationActionHandle(RespResult<string, object, DateTime> subResult)
    //    {
    //        var _st = Stopwatch.StartNew();
    //        try
    //        {
    //            _LCC_Trigger_Tick++;
    //            if (subResult.IsSuccess)
    //            {
    //                if (subResult.Content1 == _PLC_HeartBeat_Read)//判定是否心跳回读
    //                {
    //                    var val = (short)subResult.Content2;
    //                    PlcHeartBreath_Notification.MyValue = val;
    //                    var heatrbeatouttime = GlobalConfig.PlcHeartBreakOffOutTime;
    //                    if (PlcHeartBreath_Notification.TimeInterval > heatrbeatouttime)
    //                    {
    //                        IsConnected = false;
    //                        LogHelper.Warn($"【订阅】【{this.Name}】plc的心跳信号超过{heatrbeatouttime}秒没有发生变化，当前读取到的心跳值是{PlcHeartBreath_Notification.MyValue}", logname);
    //                    }
    //                }
    //                else if (subResult.Content1 == _PLC_HeartBeat_Write)
    //                {
    //                    var val = (short)subResult.Content2;
    //                    PlcHeartWriteBreath_Notification.MyValue = val;
    //                    var heatrbeatouttime = GlobalConfig.PlcHeartBreakOffOutTime;
    //                    if (PlcHeartWriteBreath_Notification.TimeInterval > heatrbeatouttime)
    //                    {
    //                        IsConnected = false;
    //                        LogHelper.Warn($"【订阅】【{this.Name}】plc的写入心跳信号超过{heatrbeatouttime}秒没有发生变化，当前读取到的心跳值是{PlcHeartWriteBreath_Notification.MyValue}");
    //                    }
    //                }
    //                else
    //                {
    //                    if (subResult.Content2 is short)
    //                    {
    //                        var data = (short)subResult.Content2;
    //                        var noTWvaluename = subResult.Content1;
    //                        // 移除原来的Task.Run异步任务_在底部穿透焊多信号高并发情况下，发现有多个任务争用同一资源，异步任务太多，导致上下文切换，影响性能_可能导致程序卡顿
    //                        // 考虑先将更新请求放入队列直接在主线程中处理变量赋值
    //                        // 使用ConcurrentQueue来存储待处理的变量更新请求，在独立的线程中处理请求，避免主线程被阻塞出现卡顿_2025-03-11
    //                        _updateQueue.Enqueue((noTWvaluename, data));
    //                        _st.Stop();
    //                        if (_st.ElapsedMilliseconds > _BeckoffPlc_Time)
    //                        {
    //                            LogHelper.Debug($"{_LCC_Trigger_Tick}【{this.Name}】监控到订阅变量[{noTWvaluename}]值更新变更为=>{data}," +
    //                             $"变化时间@{subResult.Content3:HH:mm:ss:fff};spends:{_st.ElapsedMilliseconds}ms", _LogName);
    //                        }
    //                        _st.Restart();
    //                    }
    //                    else
    //                    {
    //                        LogHelper.Debug($"【{this.Name}】plc监控到订阅变量{subResult.Content1}值不是期望类型 类型为{subResult.Content2.GetType().Name}", _logName);
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                LogHelper.Debug($"【{this.Name}】plc监控到订阅异常：[{subResult.Content1}]-{subResult.Message} @{subResult.Content3}", _logName);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Debug($"【{this.Name}】plc监控到订阅异常：[{subResult.Content1}]-{ex.ToString()}", _logName);
    //        }
    //        finally
    //        {
    //            _st.Stop();
    //        }
    //    }
    //    #endregion
    //    /// <summary>
    //    /// 一次性读取PLC内容多个标签
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="ValueName">变量名</param>
    //    /// <returns></returns>
    //    public object[] Read(string[] ValueNamelist)
    //    {
    //        var cmd = ValueNamelist.Select(x => "." + x.ToString()).ToArray();
    //        try
    //        {
    //            var rResult = communicRead.Read(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                var values = rResult.Content;
    //                return (object[])values;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"{Name}PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"{Name}PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    /// 异步一次性读取PLC内容多个标签
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="ValueName">变量名</param>
    //    /// <returns></returns>
    //    public async Task<object[]> ReadAsync(string[] ValueNamelist)
    //    {
    //        //var cmd = "." + ValueName;
    //        var cmd = ValueNamelist.Select(x => "." + x.ToString()).ToArray();
    //        try
    //        {
    //            var rResult = await communicRead.ReadAsync(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                var values = rResult.Content;
    //                return (object[])values;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"{Name}PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"{Name}PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    /// 读取PLC内容
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="ValueName">变量名</param>
    //    /// <returns></returns>
    //    public T Read<T>(string ValueName)
    //    {

    //        var cmd = "." + ValueName;
    //        try
    //        {
    //            var rResult = communicRead.Read<T>(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(ValueName, rResult.Content, 0);
    //                return rResult.Content;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }

    //    }
    //    /// <summary>
    //    /// 异步读取PLC内容
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="ValueName">变量名</param>
    //    /// <returns></returns>
    //    public async Task<T> ReadAsync<T>(string ValueName)
    //    {

    //        var cmd = "." + ValueName;
    //        try
    //        {
    //            var rResult = await communicRead.ReadAsync<T>(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(ValueName, rResult.Content, 0);
    //                return rResult.Content;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    /// 读取PLC内容
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="handleNo">变量下标</param>
    //    /// <returns></returns>
    //    public T Read<T>(int handleNo)
    //    {
    //        var cmd = "." + _basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName;
    //        try
    //        {
    //            var rResult = communicRead.Read<T>(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(_basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName, rResult.Content, 0);
    //                return rResult.Content;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    /// 异步读取PLC内容
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="handleNo">变量下标</param>
    //    /// <returns></returns>
    //    public async Task<T> ReadAsync<T>(int handleNo)
    //    {
    //        var cmd = "." + _basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName;
    //        try
    //        {
    //            var rResult = await communicRead.ReadAsync<T>(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(_basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName, rResult.Content, 0);
    //                return rResult.Content;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    /// 读取string类型数据(OMRON)
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="handleNo"></param>
    //    /// <param name="arrayno"></param>
    //    /// <returns></returns>
    //    public string Read(int handleNo, int arrayno)
    //    {
    //        var cmd = "." + _basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{arrayno}]";
    //        try
    //        {
    //            var rResult = communicRead.Read(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(_basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{arrayno}]", rResult.Content.ToString(), 0);
    //                return rResult.Content.ToString();
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return "";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    /// 异步读取string类型数据(OMRON)
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="handleNo"></param>
    //    /// <param name="arrayno"></param>
    //    /// <returns></returns>
    //    public async Task<string> ReadAsync(int handleNo, int arrayno)
    //    {
    //        var cmd = "." + _basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{arrayno}]";
    //        try
    //        {
    //            var rResult = await communicRead.ReadAsync(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(_basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{arrayno}]", rResult.Content.ToString(), 0);
    //                return rResult.Content.ToString();
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return "";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    ///  读取PLC内容
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="handleNo">变量下标</param>
    //    /// <param name="arrayno"></param>
    //    public T Read<T>(int handleNo, int arrayno)
    //    {
    //        var cmd = "." + _basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{arrayno}]";
    //        try
    //        {
    //            var rResult = communicRead.Read<T>(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(_basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{arrayno}]", rResult.Content, 0);
    //                return rResult.Content;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    ///  异步读取PLC内容
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="handleNo">变量下标</param>
    //    /// <param name="arrayno"></param>
    //    public async Task<T> ReadAsync<T>(int handleNo, int arrayno)
    //    {
    //        var cmd = "." + _basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{arrayno}]";
    //        try
    //        {
    //            var rResult = await communicRead.ReadAsync<T>(cmd);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(_basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{arrayno}]", rResult.Content, 0);
    //                return rResult.Content;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    /// 变量读取
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="handleNo">变量句柄</param>
    //    /// <param name="startindex">起始位置</param>
    //    /// <param name="length">读取长度</param>
    //    /// <returns></returns>
    //    /// <exception cref="Exception"></exception>
    //    public T Read<T>(int handleNo, int startindex, int length = 0)
    //    {
    //        var cmd = "." + _basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{startindex}]";
    //        try
    //        {
    //            var rResult = communicRead.Read<T>(cmd, (ushort)length);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(cmd, rResult.Content, 0);
    //                return rResult.Content;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    /// <summary>
    //    /// 异步变量读取
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="handleNo">变量句柄</param>
    //    /// <param name="startindex">起始位置</param>
    //    /// <param name="length">读取长度</param>
    //    /// <returns></returns>
    //    /// <exception cref="Exception"></exception>
    //    public async Task<T> ReadAsync<T>(int handleNo, int startindex, int length = 0)
    //    {
    //        var cmd = "." + _basPLCVariableModels.Find(model => model.HandleNo == handleNo).VariableName + $"[{startindex}]";
    //        try
    //        {
    //            var rResult = await communicRead.ReadAsync<T>(cmd, (ushort)length);
    //            if (rResult.IsSuccess)
    //            {
    //                SaveSignal(cmd, rResult.Content, 0);
    //                return rResult.Content;
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC读取失败{cmd}" + rResult.Message, logname);
    //                return default;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC读取异常{cmd}" + ex, logname);
    //            return default;
    //        }
    //    }
    //    public void SaveSignal(string name, dynamic data, int type)
    //    {
    //        return;
    //        try
    //        {
    //            PLCSignalModel signal = new PLCSignalModel()
    //            {
    //                name = name,
    //                data = data.ToString(),
    //                type = type
    //            };
    //            SignalChanged?.Invoke(this, signal);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"信号交互页面保存plc数据失败：【{ex.ToString()}】", "SaveSignal");
    //        }
    //    }
    //    /// <summary>
    //    /// 写入PLC内容
    //    /// </summary>
    //    /// <param name="handleNo">变量下标</param>
    //    /// /// <param name="value">值</param>
    //    public void Write(int handleNo, object value)
    //    {
    //        try
    //        {
    //            var finf = _rwVariablePLCModels.FirstOrDefault(model => model.VariableHandle == handleNo);
    //            if (finf != null)
    //            {
    //                var cmd = "." + finf.VariableName;
    //                var rResult = communicWrite.Write(cmd, value);
    //                if (!rResult.IsSuccess)
    //                {
    //                    LogHelper.Error($"PLC写入变量失败{cmd};{value} ,原因：" + rResult.Message, logname);
    //                }
    //                SaveSignal(finf.VariableName, value, 1);
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC写入变量失败{finf.VariableName};{value} 找不到VariableHandle", logname);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC写入变量失败{handleNo};{value}" + ex, logname);
    //        }
    //    }
    //    /// <summary>
    //    /// 异步写入PLC内容
    //    /// </summary>
    //    /// <param name="handleNo">变量下标</param>
    //    /// /// <param name="value">值</param>
    //    public async Task WriteAsync(int handleNo, object value)
    //    {
    //        try
    //        {
    //            var finf = _rwVariablePLCModels.FirstOrDefault(model => model.VariableHandle == handleNo);
    //            if (finf != null)
    //            {
    //                var cmd = "." + finf.VariableName;
    //                var rResult = await communicWrite.WriteAsync(cmd, value);
    //                if (!rResult.IsSuccess)
    //                {
    //                    LogHelper.Error($"PLC写入变量失败{cmd};{value} ,原因：" + rResult.Message, logname);
    //                }
    //                SaveSignal(finf.VariableName, value, 1);
    //            }
    //            else
    //            {
    //                LogHelper.Error($"PLC写入变量失败{finf.VariableName};{value} 找不到VariableHandle", logname);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC写入变量失败{handleNo};{value}" + ex, logname);
    //        }
    //    }
    //    /// <summary>
    //    /// 写入PLC内容
    //    /// </summary>
    //    /// <param name="variableName">变量名</param>
    //    /// /// <param name="value">值</param>
    //    public void Write(string variableName, object value)
    //    {
    //        var cmd = "." + variableName;
    //        try
    //        {
    //            var rResult = communicWrite.Write(cmd, value);
    //            if (!rResult.IsSuccess)
    //            {
    //                LogHelper.Error($"PLC写入变量失败{variableName};{value} ,原因：" + rResult.Message, logname);
    //            }
    //            SaveSignal(variableName, value, 1);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC写入变量失败{variableName};{value}" + ex, logname);
    //        }
    //    }
    //    /// <summary>
    //    /// 异步写入PLC内容
    //    /// </summary>
    //    /// <param name="variableName">变量名</param>
    //    /// /// <param name="value">值</param>
    //    public async Task WriteAsync(string variableName, object value)
    //    {
    //        var cmd = "." + variableName;
    //        try
    //        {
    //            var rResult = await communicWrite.WriteAsync(cmd, value);
    //            if (!rResult.IsSuccess)
    //            {
    //                LogHelper.Error($"PLC写入变量失败{variableName};{value} ,原因：" + rResult.Message, logname);
    //            }
    //            SaveSignal(variableName, value, 1);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC写入变量失败{variableName};{value}" + ex, logname);
    //        }
    //    }
    //    /// <summary>
    //    /// 写入PLC String类型
    //    /// </summary>
    //    /// <param name="handleNo">变量下标</param>
    //    /// <param name="value">值</param>
    //    public void Write(int handleNo, object value, int arrayNo)
    //    {
    //        var cmd = "." + _rwVariablePLCModels.Find(model => model.VariableHandle == handleNo).VariableName +
    //                  $"[{arrayNo}]";
    //        try
    //        {
    //            var rResult = communicWrite.Write(cmd, value);
    //            if (!rResult.IsSuccess)
    //            {
    //                LogHelper.Error($"PLC写入变量失败{cmd},原因：" + rResult.Message, logname);
    //            }
    //            SaveSignal(_rwVariablePLCModels.Find(model => model.VariableHandle == handleNo).VariableName + $"[{arrayNo}]", value, 1);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC写入变量失败{cmd}" + ex, logname);
    //        }
    //    }
    //    /// <summary>
    //    /// 异步写入PLC String类型
    //    /// </summary>
    //    /// <param name="handleNo">变量下标</param>
    //    /// <param name="value">值</param>
    //    public async Task WriteAsync(int handleNo, object value, int arrayNo)
    //    {
    //        var cmd = "." + _rwVariablePLCModels.Find(model => model.VariableHandle == handleNo).VariableName +
    //                  $"[{arrayNo}]";
    //        try
    //        {
    //            var rResult = await communicWrite.WriteAsync(cmd, value);
    //            if (!rResult.IsSuccess)
    //            {
    //                LogHelper.Error($"PLC写入变量失败{cmd},原因：" + rResult.Message, logname);
    //            }
    //            SaveSignal(_rwVariablePLCModels.Find(model => model.VariableHandle == handleNo).VariableName + $"[{arrayNo}]", value, 1);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"PLC写入变量失败{cmd}" + ex, logname);
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    (bool, Dictionary<string, bool>) IPLC.IsConnected()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    /// <summary>
    //    /// 订阅变量的值发生变化
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    private void obj_PropertyChanged(object sender, PLCVariableModel e)
    //    {
    //        e.PLCIPAddress = Ip;
    //        _asyncPublisher.PublishAsync(Com.Common.MessageType.GlobalPLC, e);
    //    }
    //    /// <summary>
    //    /// 使用ConcurrentQueue将多个更新请求并在线程中批量处理，从而提高处理效率
    //    /// </summary>
    //    /// <param name="cancellationToken"></param>
    //    /// <returns></returns>
    //    private async Task ProcessUpdateQueue(CancellationToken cancellationToken)
    //    {
    //        while (!cancellationToken.IsCancellationRequested)
    //        {
    //            while (_updateQueue.TryDequeue(out var update))
    //            {
    //                UpdateVariable(update.variableName, update.value);
    //            }
    //            await Task.Delay(_UpdatePLCVariableTime); // 100ms
    //        }
    //    }
    //    private object _lock = new object();
    //    /// <summary>
    //    /// 更新变量值
    //    /// </summary>
    //    /// <param name="variableName"></param>
    //    /// <param name="value"></param>
    //    private void UpdateVariable(string variableName, short value)
    //    {
    //        if (_monitorPLCVariablesCacheDictionary.TryGetValue(variableName, out int index))
    //        {
    //            //这里还是得考虑使用锁lock确保线程安全，在访问共享变量时，避免竞争条件_2024-03-11
    //            lock (_lock)
    //            {
    //                if (index < _monitorPLCVariables.Count)
    //                {
    //                    var variableToUpdate = _monitorPLCVariables[index];
    //                    if (variableToUpdate.VariableValue != value)
    //                    {
    //                        variableToUpdate.VariableValue = value;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    public T Read<T>(string variableName, int[] readArgs = null)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public object ReadMultiple(string[] variableNames)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    class PLCVariableModel_Cache
    //    {
    //        public PLCVariableModel_Cache(PLCVariableModel pLCVariableModel)
    //        {
    //            this.VariableHandle = pLCVariableModel.VariableHandle;
    //            this.VariableName = pLCVariableModel.VariableName;
    //            this.VariableValue = pLCVariableModel.VariableValue;
    //            this.VariableValue_Old = pLCVariableModel.VariableValue;
    //            this.AddCacheTime = DateTime.Now;
    //        }
    //        /// <summary>
    //        /// 变量触发句柄
    //        /// </summary>
    //        public int VariableHandle { get; set; }
    //        /// <summary>
    //        /// 变量值
    //        /// </summary>
    //        public string VariableName { get; set; }
    //        /// <summary>
    //        /// 变量值
    //        /// </summary>
    //        public short VariableValue { get; set; }
    //        /// <summary>
    //        /// 变量值 旧
    //        /// </summary>
    //        public short VariableValue_Old { get; set; }
    //        /// <summary>
    //        /// 添加缓存时间
    //        /// </summary>
    //        public DateTime AddCacheTime { get; set; }
    //    }
    //}
}
