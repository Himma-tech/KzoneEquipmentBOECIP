using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Prism.Ioc;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Globalization;
using MiniExcelLibs;
using System.Globalization;
using Himma.Common.Log;
namespace Himma.Common.Business.Base
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    public class BaseBll : IBusiness
    {
        //#region **Properties**
        //private BasMachineInfoDAL _basMachineInfoDal = new BasMachineInfoDAL();
        //private BatteryAccess _batteryAccess = new BatteryAccess();
        //public event EventHandler<PLCSignalModel> SignalChanged;
        //private BizNgCodeSortationDAL _bizNgCodeSortationDAL = new BizNgCodeSortationDAL();
        //public bool UpperToPlcFloatDataEnble = false;
        //public float NeedWashFlag = 0;
        //public float AlarmSignal = 0;//托杯清洗机-by-20231020
        ////public float LWMFlag = 0;
        //public readonly IFastReportDLL SignReport;
        //public IMES_Api api_mes;
        ///// <summary>
        ///// 实时数据
        ///// </summary>
        //public List<BasParaVariableModel> RealTimeVariableList;
        //public BizWorkOrderInfoDAL bizWorkOrderInfoDAL = new BizWorkOrderInfoDAL(); //工单信息查询
        ///// <summary>
        ///// 文件读写
        ///// </summary>
        //public readonly IFileHelper _ifileHelper;
        ////private readonly IEventAggregator _ieventAggregator;
        //public readonly IContainerProvider _icontainerProvider;
        //public readonly List<IScanner> _scanners = new List<IScanner>();
        //public readonly List<IHiPort> _hipots = new List<IHiPort>();
        //public readonly List<IAgv> _agvs = new List<IAgv>();
        //public readonly List<IConfocal> _Confocals = new List<IConfocal>();
        //private readonly List<BasComponentModel> _basComponentModels;
        ///// <summary>
        ///// //结果数据
        ///// </summary>
        //public readonly BizBatteryResultDAL _bizBatteryResultDal;
        //private readonly BasParaVariableDAL _basParaVariableDal;//品质数据配置表（品质数据即参数表）
        ////private readonly BasComponentDAL _basComponentDal;//部件
        //private readonly BizParamaterChangeDAL _bizParamaterChangeDal;//参数修改记录表操作
        //public readonly BizBatteryCodeRecordDAL _bizBatteryCodeRecordDAL;//记录扫码
        ///// <summary>
        ///// 物料DAL
        ///// </summary>
        //public readonly BasMaterialInfoDAL BasMaterialInfoDal;//上位机手动录入参数
        ///// <summary>
        ///// //品质数据
        ///// </summary>
        //public List<BasParaVariableModel> _paraList;
        //public List<BasParaVariableModel> ProductionVariableList;
        //public List<BasParaVariableModel> WindFlawCoordinateList; //卷绕蓝标
        //public List<BasParaVariableModel> _monitorVariableList;//监控数据
        //private ObservableCollection<BizMonitorParaModel> _monitorParaModels = new ObservableCollection<BizMonitorParaModel>();
        //public readonly TaskFactory _taskFactory = new TaskFactory();
        //private StringBuilder _realTimeFileHeader = new StringBuilder();//实时数据文件头
        //private readonly ulong[] _cacheAlarmCode = new ulong[64];//缓存报警信息，用于和当前报警信息比较
        //private List<bool> _saveState = new List<bool>() { false, false };//保存状态
        //public object obj = new object();
        //private MachineStateBll _machineStateBll;//设备状态
        //private MachineAlarmBll _machineAlarmBll;//设备报警
        //public int FloatInternal = 0;//不同工站下料数据间隔
        //public int StringInternal = 0;//不同工站下料数据间隔
        //private static BizCupClearDAL _bizCupClearDAL = new BizCupClearDAL();
        //private static BizBatteryDAL _bizBatteryModels = new BizBatteryDAL();
        //private static BizBatteryResultDAL _bizWaoDataModels = new BizBatteryResultDAL();
        //private static BizCansupplyDataDAL _bizCansupplyDataDal = new BizCansupplyDataDAL();
        //private static BizPuckExchangeDataDAL _bizPuckExchangeDataDal = new BizPuckExchangeDataDAL();
        //private static BizBatterySNDAL _bizBatterySNDAL = new BizBatterySNDAL();
        //public string DataTitle = "";
        //public static Action<string, float> dataSend;
        //private int ProductionDataTime = 60000;//生产数据时间间隔
        //#endregion **Properties**

        /// <summary>
        /// 基类
        /// </summary>
        /// <param name="containerProvider"></param>
        //public BaseBll(IContainerProvider containerProvider)
        //{
        //    try
        //    {
        //        _ifileHelper = containerProvider.Resolve<IFileHelper>();
        //        api_mes = containerProvider.Resolve<IMES_Api>();
        //        _icontainerProvider = containerProvider;
        //        _bizBatteryResultDal = new BizBatteryResultDAL();
        //        _basParaVariableDal = new BasParaVariableDAL();
        //        _bizParamaterChangeDal = new BizParamaterChangeDAL();
        //        _bizBatteryCodeRecordDAL = new BizBatteryCodeRecordDAL();
        //        BasMaterialInfoDal = new BasMaterialInfoDAL();
        //        _machineStateBll = new MachineStateBll(_icontainerProvider);//监控设备状态
        //        //_machineAlarmBll = new MachineAlarmBll(_icontainerProvider);//报警状态
        //        FloatInternal = GlobalVariable.FloatInternal;
        //        StringInternal = GlobalVariable.StringInternal;

        //        var basParaVariableModels = _basParaVariableDal.GetList().FindAll(model => model.ProcessNo == GlobalVariable.ProcessNo);
        //        _paraList = basParaVariableModels.FindAll(model => model.StationName.Contains("下料工位") && model.UsedFlag == 1);
        //        RealTimeVariableList = basParaVariableModels.FindAll(model => model.UsedFlag == 1 && model.StationName == "实时数据采集工位");
        //        _monitorVariableList = basParaVariableModels.FindAll(model => model.ParaType == 1 && model.UsedFlag == 1 && model.StationName == "监控参数");
        //        ProductionVariableList = basParaVariableModels.FindAll(model => model.UsedFlag == 1 && model.StationName == "产出工位");
        //        WindFlawCoordinateList = basParaVariableModels.FindAll(model => model.UsedFlag == 1 && model.StationName == "卷绕蓝标工位");

        //        //获取下料数据标题
        //        DataTitle = GetTitle(_paraList);
        //        GlobalVariable.EqptNo = DeivceEquipmentidPlcInfo.lstDeivceEquipmentidPlcInfos[0]?.EquipmentID; //设备ID 
        //        LogHelper.Debug($"设备[{GlobalVariable.EqptNo}]_获取出站生产数据标题内容：{DataTitle}");

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("程序初始化异常:" + ex);
        //    }
        //}
        #region 部件
        /// <summary>
        /// 部件方法体
        /// </summary>
        /// <param name="component"></param>
        /// <param name="rspValueHandle">反馈测试值的下标</param>
        /// <param name="rspSignalHandle">反馈结果的下标</param>
        /// <param name="objectMsg">对象信息</param>
        /// <param name="signal">信号</param>
        //public virtual void ComponentMethodBody(IComponent component, int rspValueHandle, string Vardes, string objectMsg, object signal, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// 读码校验
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        //public (short, string) BarcodeCheck(IComponent component)
        //{
        //    string testValue;//测试数据
        //    bool res;//获取数据结果
        //    short rsp = 1;
        //    (res, testValue) = component.GetValue();  // (bool result, string code) GetValue();//GetValue()其实就是直接获取条码（code）方法，而其中res仅仅只是一个结果反馈标志，存储true，当然是在连接正常状态下
        //    LogHelper.Info($"{component.Name}扫码枪反馈数据：{testValue}");
        //    testValue = testValue.Replace("\r", "").Replace("\n", "");
        //    //字符串AscII化
        //    testValue = Regex.Replace(testValue, @"[^\u0020-\u007E]", string.Empty).Trim();

        //    if (!res || testValue.ToUpper().Contains("ERROR") || string.IsNullOrEmpty(testValue.Trim()))//res不为true，或者没扫上码包含ERROR，或者去空格之后还为空
        //    {
        //        LogHelper.Error($"{component.Name}获取数据失败：{testValue}");//将testvalue值反馈出来                                                                         // return (3, string.Empty);//扫码失败
        //        return (3, "ERR");//扫码失败
        //    }
        //    LogHelper.Debug($"{component.Name}获取数据成功：{testValue}");
        //    LogHelper.Debug($"{component.Name}反馈PLC扫码内容{testValue}");
        //    return (rsp, testValue);
        //}
        #endregion 部件
        /// <summary>
        /// 上位机初始化
        /// </summary>
        //public void Init()
        //{
        //    foreach (var param in GlobalVariable.PLCParams)
        //    {
        //        var plc = _icontainerProvider.Resolve<IHimmaPLC>(param.PLCIPAddress);
        //        _taskFactory.StartNew(() => { TimeMonitor(plc); }, TaskCreationOptions.LongRunning);//时间监控

        //        _taskFactory.StartNew(() => {
        //            //只记录主PLC数据
        //            //if (param.ProcessNo == GlobalVariable.ProcessNo)
        //            //{
        //            IsMesCanMnitor(plc);
        //            //}
        //            //托杯交换机PLC数据
        //            if (GlobalVariable.ProcessNo == "AnodeWeld" && plc.ProcessNo == "PuckExchange")
        //            {
        //                IsMesPEXlMnitor(plc);
        //            }
        //        }, TaskCreationOptions.LongRunning);//监控是否屏蔽MES，套膜MES参数屏蔽

        //        _taskFactory.StartNew(() => {
        //            //只有清洗涂油工序可用  ---新增套膜机-by-20231008 -因采集主轴扭矩-变更启用条件
        //            if (GlobalVariable.UploadRealTimeData == 1)
        //            {
        //                GetRealTimeData(plc);
        //                //DeviceMonitorData(plc);
        //            }

        //        }, TaskCreationOptions.LongRunning);//实时采集数据循环

        //        //_taskFactory.StartNew(() => { _machineAlarmBll.DeviceAlarmMonitor(plc); }, TaskCreationOptions.LongRunning);//报警监控

        //        _taskFactory.StartNew(() => {
        //            if (param.ProcessNo == GlobalVariable.ProcessNo)
        //            {
        //                _machineStateBll.MachineStateMonitor(plc);
        //            }
        //        }, TaskCreationOptions.LongRunning);//设备状态监控

        //        _taskFactory.StartNew(() => {
        //            ////只记录主PLC数据
        //            //if (param.ProcessNo == GlobalVariable.ProcessNo || param.ProcessNo == "PuckExchange")
        //            //{
        //            //    SaveProductionData(plc);
        //            //}
        //            //
        //            SaveProductionData(plc);
        //        }, TaskCreationOptions.LongRunning);//

        //        _taskFactory.StartNew(() => {
        //            //只有入壳/负极壳体底部焊接机工序可用
        //            if (param.ProcessNo == GlobalVariable.WashMachineNo)
        //            {
        //                //LWMDataSave();
        //                UpdateWashFlag();
        //            }
        //        }, TaskCreationOptions.LongRunning);//托杯清洗循环判断
        //    }
        //    _taskFactory.StartNew(() => { SaveBlankingDataFromRedis(); }, TaskCreationOptions.LongRunning);//存储结果数据循环

        //    // _taskFactory.StartNew(() => { UpDateVersionData(); }, TaskCreationOptions.LongRunning);//存储结果数据循环

        //    SaveCupClearData();
        //    LogHelper.Info($"设备[{GlobalVariable.EqptNo}]_上位机初始化完成");
        //}

        //#region 部件初始化

        ///// <summary>
        ///// 部件初始化   InitComponent()方法其实就是自动扫码枪的初始化,因为类型type为1只有自动扫码枪
        ///// </summary>
        //public void InitComponent()
        //{
        //    try
        //    {
        //        List<BasComponentModel> componentModels = new List<BasComponentModel>();
        //        foreach (var param in GlobalVariable.PLCParams)
        //        {
        //            componentModels.AddRange(new BasComponentDAL().GetList().FindAll(model => model.ProcessNo == param.ProcessNo && model.UsedFlag == 1));//componentModels 是组件集合类的一个实例
        //        }
        //        //List<BasComponentModel> componentModels = new BasComponentDAL().GetList().FindAll(model => model.ProcessNo == GlobalVariable.ProcessNo&& model.UsedFlag == 1);//componentModels 是组件集合类的一个实例
        //        foreach (var v in componentModels)
        //        {
        //            switch (v.Type)
        //            {
        //                case 1: //扫码枪业务
        //                    InitScanner(v);
        //                    break;

        //                case 2: //Hipot业务
        //                    InitHipot(v);
        //                    break;
        //                case 3: //AGV业务
        //                    InitAgv(v);
        //                    break;
        //                case 4: //点激光测试
        //                    InitConfocals(v);
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error($"部件初始化异常!{ex}");
        //    }
        //}

        ///// <summary>
        ///// 初始化扫码枪
        ///// </summary>
        ///// <param name="v"></param>
        //private void InitScanner(BasComponentModel v)
        //{
        //    if (string.IsNullOrEmpty(v.Parameter))
        //    {
        //        LogHelper.Error(v.Name + "没有配置参数，未加载！");
        //        return;
        //    }
        //    var Scanner = _icontainerProvider.Resolve<IScanner>(v.Class);
        //    Scanner.InitCompnent(v.Parameter.Split(';')[0], int.Parse(v.Parameter.Split(';')[1]));
        //    Scanner.Name = v.Name;
        //    Scanner.ProcessNo = v.ProcessNo;
        //    Scanner.Sid = v.Sid;
        //    Scanner.Connect();
        //    Scanner.InScan = v.InScan;
        //    //判断设备启用的能力集合
        //    Scanner.PowerList = new List<Common.ComponentPower>();

        //    if (!string.IsNullOrEmpty(v.Remarks))
        //    {
        //        var PowerList = Com.Common.Tool.CommonEnum.GetEnumItems<Common.ComponentPower>();
        //        foreach (var item in v.Remarks.Split(','))
        //        {
        //            var P = PowerList.FirstOrDefault(T => T.Id == Convert.ToInt32(item));
        //            if (P != null)
        //            {
        //                var enumP = (Common.ComponentPower)P.Id;
        //                if (!Scanner.PowerList.Contains(enumP))
        //                {
        //                    LogHelper.Info($"{v.Name}已启用能力集[{P.text}]");
        //                    Scanner.PowerList.Add(enumP);
        //                }
        //            }
        //        }
        //    }
        //    _scanners.Add(Scanner);
        //}

        //public void InitHipot(BasComponentModel v)
        //{
        //    if (string.IsNullOrEmpty(v.Parameter))
        //    {
        //        LogHelper.Error(v.Name + "没有配置参数，未加载！");
        //        return;
        //    }
        //    var Hipot = _icontainerProvider.Resolve<IHiPort>(v.Class);
        //    Hipot.InitCompnent(v.Parameter.Split(';')[0], int.Parse(v.Parameter.Split(';')[1]));
        //    Hipot.Name = v.Name;
        //    Hipot.ProcessNo = v.ProcessNo;
        //    Hipot.Sid = v.Sid;
        //    Hipot.Connect();
        //    Hipot.ReConnectAsync();
        //    Hipot.InScan = v.InScan;
        //    //Scanner._barcode_return += Scanner__barcode_return;
        //    //判断设备启用的能力集合
        //    _hipots.Add(Hipot);
        //}
        //public void InitAgv(BasComponentModel v)
        //{
        //    if (string.IsNullOrEmpty(v.Parameter))
        //    {
        //        LogHelper.Error(v.Name + "没有配置参数，未加载！");
        //        return;
        //    }
        //    var Agv = _icontainerProvider.Resolve<IAgv>(v.Class);
        //    Agv.InitCompnent(v.Parameter.Split(';')[0], int.Parse(v.Parameter.Split(';')[1]));
        //    Agv.Name = v.Name;
        //    Agv.ProcessNo = v.ProcessNo;
        //    Agv.Sid = v.Sid;
        //    Agv.InScan = v.InScan;
        //    Agv.Remarks = v.Remarks;
        //    Agv.Connect();
        //    Agv.ReConnectAsync();
        //    Agv._getdata += Agv__getdata;
        //    _agvs.Add(Agv);
        //}
        ///// <summary>
        ///// 点激光测试
        ///// </summary>
        ///// <param name="v"></param>
        //public void InitConfocals(BasComponentModel v)
        //{
        //    if (string.IsNullOrEmpty(v.Parameter))
        //    {
        //        LogHelper.Error(v.Name + "点激光测试没有配置参数，未加载成功！");
        //        return;
        //    }
        //    var confocal = _icontainerProvider.Resolve<IConfocal>(v.Class);
        //    confocal.InitCompnent(v.Parameter.Split(';')[0], int.Parse(v.Parameter.Split(';')[1]));
        //    confocal.Name = v.Name;
        //    confocal.ProcessNo = v.ProcessNo;
        //    confocal.Sid = v.Sid;
        //    confocal.InScan = v.InScan;
        //    confocal.Remarks = v.Remarks;
        //    confocal.Connect();
        //    confocal.ReConnectAsync();
        //    confocal._getdata += Confocal__getdata;
        //    _Confocals.Add(confocal);
        //}
        ///// <summary>
        ///// 点激光交互
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="data"></param>
        //public async void Confocal__getdata(object obj, string data)
        //{
        //    var msg = "";
        //    string PseudoCode = "";
        //    int RequestID = 0;
        //    int CheckResult = 1;
        //    var Msg = "OK";
        //    var RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    var request = data.JsonToObject<ConfocalJsonModel_response>();

        //    var PointLaser = _Confocals.FirstOrDefault(x => x.ProcessNo == GlobalVariable.ProcessNo);
        //    var param = GlobalVariable.PLCParams.FirstOrDefault(x => x.ProcessNo == _agvs.First().Remarks);
        //    var PLC = _icontainerProvider.Resolve<IHimmaPLC>(param.PLCIPAddress);
        //    try
        //    {
        //        if (request != null)
        //        {
        //            RequestID = (int)request?.RequestID;
        //            PseudoCode = request?.PseudoCode;
        //            CheckResult = (int)request?.CheckResult;
        //            Msg = request?.Msg;
        //            if (CheckResult == 2 || CheckResult == 3)
        //            {
        //                await PLC.WriteAsync(6, PseudoCode, 3);//给PLC写入请求电芯码的虚拟码  
        //                await PLC.WriteAsync(19, (short)Status.MesNg, 3); //写给plc结果反馈
        //                LogHelper.Debug($"点激光反馈虚拟码-[{PseudoCode}]-疑似褶皱,给PLC下发[{Status.MesNg.GetDescription()}]踢料信号", "WindPointLaser");
        //                //新增反馈褶皱的数据保存本地
        //                string Confocals = "卷绕点激光褶皱数据";
        //                string Confocalsheader = "生产时间,电芯虚拟码,结果信号,检测状态";
        //                string ConfocalsData =
        //                           $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{PseudoCode},{CheckResult},{Msg}";
        //                await _ifileHelper.SaveCSVFileAsync(GlobalVariable.RealTimeSavePath, ConfocalsData, Confocalsheader, Confocals);
        //            }
        //            else
        //            {
        //                await PLC.WriteAsync(6, PseudoCode, 3);//给PLC写入请求电芯码的虚拟码
        //                await PLC.WriteAsync(19, (short)Status.OK, 3); //写结果反馈
        //                LogHelper.Debug($"点激光反馈虚拟码-[{PseudoCode}]-检测结果[{Msg}],给PLC下发[{Status.OK.GetDescription()}]信号", "WindPointLaser");
        //            }
        //            LogHelper.Debug($"收到点激光反馈虚拟码-[{PseudoCode}]-的数据【标志[{RequestID}],结果信号[{CheckResult}],检测结果[{Msg}]】", "WindPointLaser");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await PLC.WriteAsync(19, (short)Status.LocalCheckNg, 3);
        //        LogHelper.Debug($"点激光交互时发生异常: {ex};给PLC反馈[{Status.LocalCheckNg.GetDescription()}]信号", "WindPointLaser");
        //    }
        //}
        //#endregion 部件初始化

        //#region 线程实时刷新 FUNCTION    
        ///// <summary>
        ///// PLC->设备实时数据采集-UploadRealTimeData
        ///// </summary>
        //public virtual async void GetRealTimeData(IHimmaPLC PLC)
        //{
        //    int internalTime = int.Parse(GlobalVariable.RealTimeSaveInternal);
        //    while (GlobalVariable.State)
        //    {
        //        //按客户要求对设备扭矩进行实时采集
        //        if (GlobalVariable.MesOutput == 1)
        //        {
        //            //-主轴扭矩采集
        //            string Path = "";
        //            string Filename = "";
        //            //var EquipmentID = GlobalVariable.EqptNo;
        //            var EquipmentName = PLC.Name;
        //            var Datatime = DateTime.Now.ToString("yyyy-MM-dd");
        //            Path = GlobalConfig.TorqueFilesPath;
        //            Filename = $"{EquipmentName}_" + $"{Datatime}_" + "扭矩数据";

        //            try
        //            {
        //                StringBuilder dataStr = new StringBuilder();
        //                StringBuilder headerStr = new StringBuilder();
        //                float[] data = await PLC.ReadAsync<float[]>("D2017");
        //                if (PLC.RealTimeVariableList.Count <= 0)
        //                {
        //                    return;
        //                }
        //                dataStr.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",");
        //                headerStr.Append("采集时间" + ",");
        //                foreach (var item in PLC.RealTimeVariableList)
        //                {
        //                    if (item.ParaName.Contains("扭矩"))//
        //                    {
        //                        if (item.ParaType == 1)//浮点型
        //                        {
        //                            item.value = data[item.ArrayNo].ToString("f3");
        //                        }
        //                        dataStr.Append(item.value + ",");
        //                        headerStr.Append(item.ParaName + ",");
        //                    }

        //                }
        //                await _ifileHelper.SaveCSVFileAsync(Path, dataStr.ToString(), headerStr.ToString(), Filename);

        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Error($"扭矩采集出现异常" + ex.ToString());
        //            }
        //        }
        //        //原清洗涂油机和套膜机实时采集数据保留
        //        if (GlobalVariable.ProcessNo == "WashOil" || GlobalVariable.ProcessNo == "Tub")
        //        {
        //            try
        //            {
        //                StringBuilder dataStr = new StringBuilder();
        //                StringBuilder headerStr = new StringBuilder();
        //                float[] data = await PLC.ReadAsync<float[]>("D2017");
        //                if (PLC.RealTimeVariableList.Count <= 0)
        //                {
        //                    return;
        //                }
        //                dataStr.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",");
        //                headerStr.Append("采集时间" + ",");
        //                foreach (var item in PLC.RealTimeVariableList)
        //                {
        //                    if (!item.ParaName.Contains("扭矩"))//
        //                    {
        //                        if (item.ParaType == 1)//浮点型
        //                        {
        //                            item.value = data[item.ArrayNo].ToString("f3");
        //                        }
        //                        dataStr.Append(item.value + ",");
        //                        headerStr.Append(item.ParaName + ",");
        //                    }
        //                }
        //                var fileName = "实时采集数据";
        //                await _ifileHelper.SaveCSVFileAsync(GlobalVariable.RealTimeSavePath, dataStr.ToString(), headerStr.ToString(), fileName);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Error($"实时数据采集出现异常" + ex.ToString());

        //            }
        //        }
        //        await Task.Delay(internalTime);
        //    }
        //}
        ///// <summary>
        ///// 从redis数组保存数据
        ///// </summary>
        //public virtual async void SaveBlankingDataFromRedis()
        //{
        //    while (GlobalVariable.State)
        //    {
        //        try
        //        {
        //            string logName = "RedisData";
        //            await Task.Delay(GlobalConfig.RedisCollectIntval);
        //            await SaveBizBatteryResultModel(logName);
        //            await DisposebizBatteryModels(logName);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.Error($"redis数据存储异常:" + ex.ToString());
        //        }

        //    }
        //}
        //List<BizBatteryResultModel> BizBatteryResultModelist = new List<BizBatteryResultModel>();
        //List<BizBatteryResultModel> BizBatteryResultModelistOut = new List<BizBatteryResultModel>();
        //private async Task SaveBizBatteryResultModel(string logName)
        //{
        //    string errorData = "";
        //    try
        //    {
        //        BizBatteryResultModelist.Clear();
        //        var RedisCount = await CommonRedis.ListLengthAsync("BizBatteryResultModel");
        //        if (RedisCount > 0)
        //        {
        //            for (int i = 0; i < RedisCount; i++)
        //            {
        //                // string RedisReturn = await CommonRedis.ListGetFromRightAsyncString("BizBatteryResultModel");
        //                string RedisReturn = await CommonRedis.ListGetFromRightAsyncString("BizBatteryResultModel");
        //                errorData = RedisReturn;
        //                if (string.IsNullOrEmpty(RedisReturn))
        //                {
        //                    await Task.Delay(500);
        //                    LogHelper.Error("【BizBatteryResultModel】Redis数据取出异常：数据为空", logName);
        //                }
        //                else
        //                {
        //                    await Task.Delay(100);
        //                    var BBR = RedisReturn.JsonToObject<BizBatteryResultModel>();
        //                    BizBatteryResultModelist.Add(BBR);
        //                }
        //            }
        //        }

        //        if (BizBatteryResultModelist.Count > 0)
        //        {
        //            await SaveDataNew(BizBatteryResultModelist);
        //            BizBatteryResultModelist.Clear();
        //        }

        //        BizBatteryResultModelistOut.Clear();
        //        var RedisCountOut = await CommonRedis.ListLengthAsync("BizBatteryResultModelOut");
        //        if (RedisCountOut > 0)
        //        {
        //            for (int i = 0; i < RedisCountOut; i++)
        //            {
        //                //string RedisReturn = await CommonRedis.ListGetFromRightAsyncString("BizBatteryResultModelOut");
        //                string RedisReturn = await CommonRedis.ListGetFromRightAsyncString("BizBatteryResultModelOut");
        //                errorData = RedisReturn;
        //                if (string.IsNullOrEmpty(RedisReturn))
        //                {
        //                    await Task.Delay(500);
        //                    LogHelper.Error("【BizBatteryResultModelOut】Redis数据取出异常：数据为空", logName);
        //                }
        //                else
        //                {
        //                    await Task.Delay(100);
        //                    var BBR = RedisReturn.JsonToObject<BizBatteryResultModel>();
        //                    BizBatteryResultModelistOut.Add(BBR);
        //                }
        //            }
        //        }

        //        if (BizBatteryResultModelistOut.Count > 0)
        //        {
        //            await SaveDataNewOut(BizBatteryResultModelistOut);
        //            BizBatteryResultModelistOut.Clear();
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        await Task.Delay(500);
        //        LogHelper.Error($"[{errorData}]Redis保存数据失败：" + ex, logName);
        //        LogHelper.Error(errorData, "SaveBlankingDataFromRedis");
        //    }
        //}
        //List<BizBatteryModel> bizBatteryModelsInlist = new List<BizBatteryModel>();
        //List<BizBatteryModel> bizBatteryModelsDelSomelist = new List<BizBatteryModel>();
        //List<BizBatteryModel> bizBatteryModelsDelSiglist = new List<BizBatteryModel>();
        ///// <summary>
        ///// 处理biz_battery表数据
        ///// </summary>
        ///// <param name="logName"></param>
        //private async Task DisposebizBatteryModels(string logName)
        //{
        //    try
        //    {
        //        bizBatteryModelsInlist.Clear();
        //        //biz_battery插入数据 
        //        var bizBatteryModelsIn = await CommonRedis.ListLengthAsync("bizBatteryModelsIn");
        //        if (bizBatteryModelsIn > 0)
        //        {
        //            for (int i = 0; i < bizBatteryModelsIn; i++)
        //            {
        //                //string RedisReturn = await CommonRedis.ListGetFromRightAsyncString("bizBatteryModelsIn");
        //                string RedisReturn = await CommonRedis.ListGetFromRightAsyncString("bizBatteryModelsIn");
        //                if (string.IsNullOrEmpty(RedisReturn))
        //                {
        //                    await Task.Delay(500);
        //                    LogHelper.Error("redis数据bizBatteryModelsIn取出异常：数据为空", logName);
        //                }
        //                else
        //                {
        //                    var BBR = JsonConvert.DeserializeObject<BizBatteryModel>(RedisReturn);
        //                    bizBatteryModelsInlist.Add(BBR);
        //                }
        //            }
        //        }

        //        if (bizBatteryModelsInlist.Count > 0)
        //        {
        //            _bizBatteryModels.Insert(bizBatteryModelsInlist);
        //            bizBatteryModelsInlist.Clear();
        //        }


        //        bizBatteryModelsDelSomelist.Clear();
        //        //biz_battery删除一段时间数据
        //        var bizBatteryModelsDelSome = await CommonRedis.ListLengthAsync("bizBatteryModelsDelSome");
        //        if (bizBatteryModelsDelSome > 0)
        //        {
        //            for (int i = 0; i < bizBatteryModelsDelSome; i++)
        //            {
        //                //string RedisReturn = await CommonRedis.ListGetFromRightAsyncString("bizBatteryModelsDelSome");
        //                string RedisReturn = await CommonRedis.ListGetFromRightAsyncString("bizBatteryModelsDelSome");
        //                if (string.IsNullOrEmpty(RedisReturn))
        //                {
        //                    await Task.Delay(500);
        //                    LogHelper.Error("redis数据bizBatteryModelsIn取出异常：数据为空", logName);
        //                }
        //                else
        //                {
        //                    var BBR = JsonConvert.DeserializeObject<BizBatteryModel>(RedisReturn);
        //                    bizBatteryModelsDelSomelist.Add(BBR);
        //                }
        //            }
        //        }

        //        if (bizBatteryModelsDelSomelist.Count > 0)
        //        {
        //            var maxDate = bizBatteryModelsDelSomelist.Select(x => x.CreatDate).Max();
        //            _bizBatteryModels.Delete(x => x.CreatDate <= maxDate);
        //            bizBatteryModelsDelSomelist.Clear();
        //        }

        //        bizBatteryModelsDelSiglist.Clear();
        //        //biz_battery删除单个数据
        //        var bizBatteryModelsDelSig = await CommonRedis.ListLengthAsync("bizBatteryModelsDelSig");
        //        if (bizBatteryModelsDelSig > 0)
        //        {
        //            for (int i = 0; i < bizBatteryModelsDelSig; i++)
        //            {
        //                //var RedisReturn = await CommonRedis.ListGetFromRightAsyncString("bizBatteryModelsDelSig");
        //                var RedisReturn = await CommonRedis.ListGetFromRightAsyncString("bizBatteryModelsDelSig");
        //                if (string.IsNullOrEmpty(RedisReturn))
        //                {
        //                    await Task.Delay(500);
        //                    LogHelper.Error("redis数据bizBatteryModelsIn取出异常：数据为空", logName);
        //                }
        //                else
        //                {
        //                    var BBR = JsonConvert.DeserializeObject<BizBatteryModel>(RedisReturn);
        //                    bizBatteryModelsDelSiglist.Add(BBR);
        //                }
        //            }
        //        }

        //        _bizBatteryModels.Delete(x => bizBatteryModelsDelSiglist.Select(m => m.Guid).Contains(x.Guid));
        //        bizBatteryModelsDelSiglist.Clear();
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.Error($"处理biz_battery表异常：{e}", logName);
        //    }
        //}
        ///// <summary>
        ///// 保存产能数据
        ///// </summary>
        //public virtual async void SaveProductionData(IHimmaPLC PLC)
        //{
        //    //设备编号：EquipmentID;
        //    //班次：-分白夜班
        //    //生产时间：DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    //产能数据：PLC D2016 
        //    //班次时间：-分白夜班
        //    //24中早上8点到20点认为是白班，20点到第二天8点是夜班，由于我们是8点10分开始统计，所以判断的节点应该是9点和21点
        //    while (GlobalVariable.State)
        //    {
        //        try
        //        {
        //            string path = "";
        //            string timePath = "";
        //            string filename = "";
        //            DateTime now = DateTime.Now;
        //            string EquipmentID = GlobalVariable.EqptNo;
        //            path = GlobalVariable.ProductionDataSavePath + "/" + EquipmentID;
        //            var EquipmentName = PLC.Name;
        //            // 每小时的第10分钟执行
        //            if (DateTime.Now.Minute == 10)
        //            {
        //                var dic = new List<Dictionary<string, object>>();
        //                var dicSource = new Dictionary<string, object>();
        //                var titleCommonArr = GlobalConfig.ProductionStatisticsCommonTitle.Split(',');
        //                string Ehour = $"{DateTime.Now.AddHours(-1).ToString("HH:00")}-{DateTime.Now.ToString("HH:00")}";
        //                dicSource.Add(titleCommonArr[0], EquipmentID);
        //                dicSource.Add(titleCommonArr[1], Ehour);
        //                int[] PData = await PLC.ReadAsync<int[]>(2016); // 读取PLC数据

        //                foreach (var item in PLC.ProductionVariableList)
        //                {
        //                    if (item.ParaType == 3)
        //                    {
        //                        item.value = PData[item.ArrayNo].ToString();
        //                    }
        //                    dicSource.Add(item.ParaName, item.value);
        //                }
        //                dic.Add(dicSource);

        //                // 判断当前时间以确定班次和文件存储路径
        //                if (now.TimeOfDay >= new TimeSpan(9, 0, 0) && now.TimeOfDay < new TimeSpan(21, 0, 0))
        //                {
        //                    timePath = now.ToString("yyyy-MM-dd");
        //                    filename = "白班产能" + $"{EquipmentName}" + @".csv";
        //                }
        //                else
        //                {
        //                    if (now.TimeOfDay < new TimeSpan(9, 0, 0))
        //                    {
        //                        timePath = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        //                        filename = "夜班产能" + $"{EquipmentName}" + @".csv";
        //                    }
        //                    else
        //                    {
        //                        timePath = now.ToString("yyyy-MM-dd");
        //                        filename = "夜班产能" + $"{EquipmentName}" + @".csv";
        //                    }
        //                }
        //                var filePath = System.IO.Path.Combine(path, timePath, filename);
        //                if (!File.Exists(filePath))
        //                {
        //                    var directoryInfo = new DirectoryInfo(System.IO.Path.GetDirectoryName(filePath));
        //                    if (!directoryInfo.Exists)
        //                    {
        //                        directoryInfo.Create();
        //                    }
        //                    await MiniExcel.SaveAsAsync(filePath, dic, excelType: ExcelType.CSV);
        //                }
        //                else
        //                {
        //                    using (FileStream stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan))
        //                    {
        //                        stream.Insert(dic, excelType: ExcelType.CSV);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.Error("每小时产能数据保存失败：" + ex);
        //        }
        //        await Task.Delay(ProductionDataTime);
        //    }
        //}
        ///// <summary>
        ///// 从结果参数表搜索参数
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="Name"></param>
        ///// <returns></returns>
        //public string GetParaValueByName(List<BizBatteryDetailModel> list, string Name)
        //{
        //    var item = list.FirstOrDefault(model => model.ParaName.Trim().Contains(Name));

        //    if (item != null)
        //    {
        //        return item.ParaValue;
        //    }
        //    else
        //    {
        //        return "未知";
        //    }
        //}
        //List<string> datalist = new List<string>();
        //List<string> dataNGlist = new List<string>();
        ///// <summary>
        ///// 入站保存数据到本地文件
        ///// </summary>
        ///// <param name="bizBatteryResultModel"></param>
        ///// <param name="bizBatteryDetailModels"></param>
        ///// <param name="objectMsg"></param>
        //public async Task SaveDataNew(List<BizBatteryResultModel> bizBatteryResultModellist)
        //{
        //    ////保存本地文件
        //    try
        //    {
        //        datalist.Clear();
        //        dataNGlist.Clear();
        //        string filePath = GlobalVariable.SaveDataPath;
        //        if (string.IsNullOrEmpty(filePath))
        //        {
        //            return;
        //        }
        //        if (!System.IO.Directory.Exists(filePath))
        //        {
        //            System.IO.Directory.CreateDirectory(filePath);
        //        }

        //        // LogHelper.Debug($"日志记录：{bizBatteryResultModellist.Count}","Test");
        //        //标题
        //        var headerTitle = $"主电芯码,托杯码,生产时间,工位,station,质量,明细";
        //        for (int i = 0; i < bizBatteryResultModellist.Count; i++)
        //        {
        //            var result = bizBatteryResultModellist[i].Result.ToLower();
        //            var singledata = $"{bizBatteryResultModellist[i].Guid}," +
        //                $"{bizBatteryResultModellist[i].Rfid}," +
        //                $"{bizBatteryResultModellist[i].CreateTime}," +
        //                $"{bizBatteryResultModellist[i].Station}," +
        //                $"{bizBatteryResultModellist[i].StationIndex}," +
        //                $"{(result == "ok" ? "OK" : $"NG: {bizBatteryResultModellist[i].Ngcode}")}," +
        //                $"{bizBatteryResultModellist[i].Detail.Replace(",", "，")}";

        //            datalist.Add(singledata);
        //            if (result != "ok" && result != "")
        //            {
        //                dataNGlist.Add(singledata);
        //            }
        //        }

        //        //LogHelper.Debug($"日志记录：{datalist.ToJson()}", "Test");
        //        await _ifileHelper.SaveCSVFileAsync(filePath, datalist, headerTitle, "入站生产数据", true, GlobalConfig.IsDivHour);
        //        if (dataNGlist.Count > 0)
        //        {
        //            await _ifileHelper.SaveCSVFileAsync(filePath, dataNGlist, headerTitle, "NG数据");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error($"【生产数据】文件写入失败：{ex}");
        //    }
        //}
        ///// <summary>
        ///// 出站保存数据到本地文件
        ///// </summary>
        ///// <param name="bizBatteryResultModel"></param>
        ///// <param name="bizBatteryDetailModels"></param>
        ///// <param name="objectMsg"></param>
        //public async Task SaveDataNewOut(List<BizBatteryResultModel> bizBatteryResultModellist)
        //{
        //    ////保存本地文件
        //    try
        //    {
        //        datalist.Clear();
        //        dataNGlist.Clear();
        //        string filePath = GlobalVariable.SaveDataPath;
        //        if (string.IsNullOrEmpty(filePath))
        //        {
        //            return;
        //        }
        //        if (!System.IO.Directory.Exists(filePath))
        //        {
        //            System.IO.Directory.CreateDirectory(filePath);
        //        }

        //        // LogHelper.Debug($"日志记录：{bizBatteryResultModellist.Count}","Test");
        //        //标题 DataTitle前面带逗号  
        //        var headerTitle = $"主电芯码,托杯码,生产时间,工位,station,质量{DataTitle}";
        //        for (int i = 0; i < bizBatteryResultModellist.Count; i++)
        //        {
        //            var result = bizBatteryResultModellist[i].Result.ToLower();

        //            //内容
        //            var datamodel = JsonConvert.DeserializeObject<List<SaveBatteryOutDataModel>>(bizBatteryResultModellist[i].Detail);
        //            var tltledata = "";
        //            datamodel.ForEach(x => {
        //                tltledata += "," + x.ParamValue;
        //            });

        //            var singledata = $"{bizBatteryResultModellist[i].Guid}," +
        //                $"{bizBatteryResultModellist[i].Rfid}," +
        //                $"{bizBatteryResultModellist[i].CreateTime}," +
        //                $"{bizBatteryResultModellist[i].Station}," +
        //                $"{bizBatteryResultModellist[i].StationIndex}," +
        //                $"{(result == "ok" ? "OK" : $"NG: {bizBatteryResultModellist[i].Ngcode}")}" +
        //                $"{tltledata}";

        //            datalist.Add(singledata);
        //            if (result != "ok" && result != "")
        //            {
        //                dataNGlist.Add(singledata);
        //            }
        //        }

        //        //LogHelper.Debug($"日志记录：{datalist.ToJson()}", "Test");
        //        await _ifileHelper.SaveCSVFileAsync(filePath, datalist, headerTitle, "出站生产数据", true, GlobalConfig.IsDivHour);
        //        if (dataNGlist.Count > 0)
        //        {
        //            await _ifileHelper.SaveCSVFileAsync(filePath, dataNGlist, headerTitle, "NG数据");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error($"【生产数据】文件写入失败：{ex}");
        //    }
        //}
        //List<string> svcdata = new List<string>();
        ///// <summary>
        ///// LWM数据保存
        ///// </summary>
        ///// <param name="lwmSaveModellist"></param>
        ///// <returns></returns>
        //public async Task<bool> SaveLWMDataNew(List<LWMSaveModel> lwmSaveModellist)
        //{
        //    try
        //    {
        //        if (lwmSaveModellist.Count == 0)
        //        {
        //            LogHelper.Error($"LWM保存lwmSaveModellist数量为0");
        //            return false;
        //        }

        //        svcdata.Clear();
        //        string filePath = GlobalVariable.SaveDataPath;
        //        if (string.IsNullOrEmpty(filePath))
        //        {
        //            return false;
        //        }
        //        if (!System.IO.Directory.Exists(filePath))
        //        {
        //            System.IO.Directory.CreateDirectory(filePath);
        //        }

        //        lwmSaveModellist.ForEach(lwmSaveModel =>
        //        {
        //            var datastring = $"{lwmSaveModel.batteryno},{lwmSaveModel.id},{lwmSaveModel.Integral1}," +
        //            $"{lwmSaveModel.Integral2},{lwmSaveModel.Integral3},{lwmSaveModel.savetime}";
        //            svcdata.Add(datastring);
        //        });

        //        string header = "电芯码,SN,功率1,功率2,功率3,生产时间";
        //        await _ifileHelper.SaveCSVFileAsync(filePath, svcdata, header, "LWM数据");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error($"LWM数据保存文件写入失败：{ex}");
        //        return false;
        //    }
        //}

        //List<string> csvdata = new List<string>();
        ///// <summary>
        ///// Laser数据保存
        ///// </summary>
        ///// <param name="LaserSaveModellist"></param>
        ///// <returns></returns>
        //public async Task<bool> SaveLaserData(List<LWMSaveModel> LaserSaveModellist)
        //{
        //    try
        //    {
        //        if (LaserSaveModellist.Count == 0)
        //        {
        //            LogHelper.Error($"Laser保存LaserSaveModellist数量为0");
        //            return false;
        //        }

        //        csvdata.Clear();
        //        string filePath = GlobalVariable.SaveDataPath;
        //        if (string.IsNullOrEmpty(filePath))
        //        {
        //            return false;
        //        }
        //        if (!System.IO.Directory.Exists(filePath))
        //        {
        //            System.IO.Directory.CreateDirectory(filePath);
        //        }

        //        LaserSaveModellist.ForEach(laserSaveModel =>
        //        {
        //            var datastring = $"{laserSaveModel.batteryno},{laserSaveModel.id},{laserSaveModel.remoteid}," +
        //            $"{laserSaveModel.IPGPower1},{laserSaveModel.IPGPower2},{laserSaveModel.lasertime1},{laserSaveModel.lasertime2},{laserSaveModel.savetime}";
        //            csvdata.Add(datastring);
        //        });

        //        string header = "电芯码,LaserSN1,LaserSN2,泵浦电流1,泵浦电流2,出光时间1,出光时间2,生产时间";
        //        await _ifileHelper.SaveCSVFileAsync(filePath, csvdata, header, "Laser数据");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error($"Laser数据保存文件写入失败：{ex}");
        //        return false;
        //    }
        //}
        ///// <summary>
        ///// Laser2数据读取保存
        ///// </summary>
        ///// <param name="LaserSaveModellist"></param>
        ///// <returns></returns>
        //public async Task<bool> SaveLaserData2(List<LWMSaveModel> LaserSaveModellist)
        //{
        //    try
        //    {
        //        if (LaserSaveModellist.Count == 0)
        //        {
        //            LogHelper.Error($"Laser保存LaserSaveModellist数量为0");
        //            return false;
        //        }

        //        csvdata.Clear();
        //        string filePath = GlobalVariable.SaveDataPath;
        //        if (string.IsNullOrEmpty(filePath))
        //        {
        //            return false;
        //        }
        //        if (!System.IO.Directory.Exists(filePath))
        //        {
        //            System.IO.Directory.CreateDirectory(filePath);
        //        }

        //        LaserSaveModellist.ForEach(laserSaveModel =>
        //        {
        //            var datastring = $"{laserSaveModel.batteryno},{laserSaveModel.id},{laserSaveModel.remoteid}," +
        //            $"{laserSaveModel.IPGPower1},{laserSaveModel.lasertime1},{laserSaveModel.savetime}";
        //            csvdata.Add(datastring);
        //        });

        //        string header = "电芯码,LaserSN1,LaserSN2,功率1,出光时间1,生产时间";
        //        await _ifileHelper.SaveCSVFileAsync(filePath, csvdata, header, "Laser数据");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error($"Laser数据保存文件写入失败：{ex}");
        //        return false;
        //    }
        //}

        //#endregion 线程实时刷新 FUNCTION

        //#region 过程数据变量触发

        ///// <summary>
        ///// 电芯入站
        ///// </summary>
        ///// <param name="pLCVariableModel"></param>
        ///// <param name="PLC"></param>
        //public virtual async void BatteryForIn(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    //
        //    StringBuilder sb = StringBuilderCache.Get();
        //    string _log = "BatteryForIn";
        //    object signal = plcVariableModel.VariableValue;
        //    int rspValueHandle = plcVariableModel.VariableHandle;
        //    int rspdownHandle = plcVariableModel.VariableDownHandle;
        //    var signaldes = plcVariableModel.VariableDescrition;
        //    string batteryRfid = "";
        //    string batteryGuid = "";
        //    //dynamic CheckResult = null;
        //    MesResultModel CheckResult = null;
        //    var status = Status.OK;  //0：复位；1:OK；2; NG；3.上位机异常；4.MES异常;5.清洗；
        //    DateTime GetTime = DateTime.Now;
        //    DateTime RequestTime = DateTime.Now;
        //    try
        //    {
        //        var signalchange = int.Parse(signal.ToString());
        //        sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|{plcVariableModel.VariableName}[{rspValueHandle}]信号[{signalchange}]状态为：{(signalchange == 1 ? "请求" : "默认")};");

        //        if (signalchange == 0)
        //        {
        //            await PLC.WriteAsync(20, batteryGuid, rspValueHandle);//写入plc  电芯GUID
        //            await PLC.WriteAsync(6, batteryRfid, rspValueHandle);//写入plc  电芯RFID
        //            await PLC.WriteAsync(7, (short)Status.Default, rspdownHandle);//写入plc

        //            sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|给PLC反馈结果:{Status.Default.GetDescription()}信号[{(short)Status.Default}]成功;");
        //            LogHelper.Debug(StringBuilderCache.ToStringRecycle(sb), _log);
        //            return;
        //        }
        //        batteryRfid = await PLC.ReadAsync<string>(2021, rspValueHandle);

        //        sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|读取D{2021}[{rspValueHandle}]获取当前RFID:{batteryRfid};");

        //        if (string.IsNullOrEmpty(batteryRfid) || batteryRfid.Length != GlobalConfig.RFIDLenght)
        //        {
        //            status = string.IsNullOrEmpty(batteryRfid) ? Status.NoRfid : Status.LenthNg;

        //            sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|RFID:{batteryRfid}为空或者长度不符,校验结果:{status.GetDescription()};");

        //            LogHelper.Error($"【进站】RFID:{batteryRfid}为空或者长度不符,校验结果:{status.GetDescription()};", _log);
        //        }
        //        //不加batteryRfid.Length == GlobalVariable.RFIDLenght 判断，batteryRfid.Substring(0, 3)会报异常；
        //        if (batteryRfid.Length == GlobalConfig.RFIDLenght)
        //        {
        //            if (!string.IsNullOrEmpty(GlobalConfig.RFIDPrefix) && !GlobalConfig.RFIDPrefix.Contains(batteryRfid.Substring(0, 3)))
        //            {
        //                status = Status.RfidNg;
        //                sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|RFID:{batteryRfid}编码规则有误,前三位不为{GlobalConfig.RFIDPrefix},校验结果:{status.GetDescription()};");
        //                LogHelper.Error($"【进站】RFID:{batteryRfid}编码规则有误,前三位不为{GlobalConfig.RFIDPrefix},校验结果:{status.GetDescription()};", _log);
        //            }
        //            //var codekey = _basMachineInfoDal.GetList(m => m.ProcessNo == GlobalVariable.ProcessNo && m.UsedFlag == 1).FirstOrDefault().StationCode;
        //            var codekey = GlobalVariable.PLCParams.FirstOrDefault(m => m.ProcessNo == GlobalVariable.ProcessNo)?.StationCode;
        //            if (!string.IsNullOrEmpty(codekey) && !codekey.Contains(batteryRfid.Substring(3, 1)))
        //            {
        //                status = Status.RfidNg;
        //                sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|RFID:{batteryRfid}编码不适用当前工序,第四位不为{codekey};校验结果:{status.GetDescription()};");

        //                LogHelper.Error($"【进站】RFID:{batteryRfid}编码不适用当前工序,第四位不为{codekey};", _log);
        //            }
        //        }
        //        //本地校验OK
        //        if (status == Status.OK)
        //        {
        //            //MES在线
        //            if (GlobalVariable.MesScan != 20)
        //            {
        //                RequestTime = DateTime.Now;
        //                int type = GlobalVariable.MesType;
        //                string Type = type.ToString();
        //                CheckResult = api_mes.GetBatteryGuid(batteryRfid, Type, true);

        //                status = status == Status.LenthNg ? Status.LenthNg : CheckResult.result ? status : Status.MesNg;
        //                batteryGuid = CheckResult.GUID;
        //                sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|RFID:{batteryRfid}进站获取GUID:{CheckResult.GUID},MES结果:{(status.GetDescription())},MES接口反馈耗时:{(DateTime.Now - RequestTime).TotalMilliseconds}ms;");

        //                if (CheckResult.MutiCodingFlag == 1)
        //                {
        //                    //调用A085接口
        //                    var SerialNos = new List<string> { batteryGuid };
        //                    var Codes = new List<string> { GlobalVariable.A085Code };
        //                    api_mes.UpdateEmptyCup(SerialNos, Codes, GlobalVariable.A085Operation);
        //                }
        //                //MES反馈超时
        //                if (CheckResult.message.Contains("超时"))
        //                {
        //                    status = Status.MesTimeOut;
        //                    LogHelper.Error($"【进站】RFID:{batteryRfid}校验电芯码超时",
        //                        _log);
        //                }
        //                else if (string.IsNullOrEmpty(batteryGuid) || batteryGuid.Length < GlobalConfig.GUIDLenght)
        //                {
        //                    status = Status.MesNg;
        //                    LogHelper.Error($"【进站】RFID:{batteryRfid},MES反馈的GUID:{batteryGuid}为空或者长度不符,校验结果：{status.GetDescription()};", _log);
        //                }
        //            }
        //            else
        //            {
        //                //生成指定长度的GUID码；
        //                batteryGuid = CreatBatteryGuid(GlobalConfig.GUIDLenght);
        //            }
        //        }
        //        //  GlobalVariable.BatteryOkCount += 1;
        //        await PLC.WriteAsync(20, batteryGuid, rspValueHandle);//写入plc  电芯GUID
        //        await PLC.WriteAsync(6, batteryRfid, rspValueHandle);//写入plc  电芯RFID
        //        await PLC.WriteAsync(7, (short)status, rspdownHandle);//写入plc

        //        sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|MES:{(GlobalVariable.MesScan != 20 ? "在线" : "离线")}状态;" +
        //            $"给PLC反馈:D{6}[{rspValueHandle}]RFID:{batteryRfid};D{20}[{rspValueHandle}]GUID:{batteryGuid};D{7}[{rspValueHandle}]结果:{status.GetDescription()}信号[{(short)status}]成功;");

        //        if ((DateTime.Now - GetTime).TotalMilliseconds > GlobalConfig.BatteryOutTimeOut)
        //        { sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|RFID:{batteryRfid}进站获取GUID:{CheckResult.GUID},总耗时：{(DateTime.Now - GetTime).TotalMilliseconds}ms;"); }

        //        LogHelper.Debug(StringBuilderCache.ToStringRecycle(sb), _log);
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobalVariable.BatteryNgCount += 1;
        //        await PLC.WriteAsync(7, (short)Status.LocalCheckNg, rspdownHandle);//上位机异常
        //        sb.AppendLine($"[进站]{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}|{plcVariableModel.VariableName}[{rspValueHandle}]处理异常,给PLC反馈D{7}[{rspValueHandle}]结果:{Status.LocalCheckNg.GetDescription()}信号[{(short)Status.LocalCheckNg}]成功;");
        //        LogHelper.Debug(StringBuilderCache.ToStringRecycle(sb), _log);
        //        LogHelper.Error($"【进站】{plcVariableModel.VariableName}[{rspValueHandle}]电芯进站处理异常:" + ex);
        //    }
        //    SaveDataToRedis(new BizBatteryResultModel()
        //    {
        //        Rfid = batteryRfid,
        //        Guid = batteryGuid,
        //        Result = status == Status.OK ? "OK" : "NG",
        //        Ngcode = status.GetDescription(),
        //        Model = "",
        //        Detail = new List<SaveBatteryOutDataModel>()
        //        {
        //            new SaveBatteryOutDataModel()
        //            {
        //                ParamDesc = "mes反馈", ParamValue = CheckResult?.message
        //            }
        //        }.ToJson(),
        //        Isuploaded = (byte)(GlobalVariable.MesScan != 20 ? 0 : 1),
        //        CreateTime = DateTime.Now,
        //        Station = signaldes,
        //    });
        //}
        ///// <summary>
        ///// 辅料入站(空托杯/辅料托杯)-用于底部穿透焊托杯交换机-后续取消上料
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        //public virtual async void FeedingForIn(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    var msg = "";
        //    string _log = "FeedingForIn";
        //    object signal = plcVariableModel.VariableValue;
        //    int rspValueHandle = plcVariableModel.VariableHandle;
        //    int rspdownHandle = plcVariableModel.VariableDownHandle;
        //    var signaldes = plcVariableModel.VariableDescrition;
        //    try
        //    {
        //        var signalchange = int.Parse(signal.ToString());
        //        string feedingRfid = "";
        //        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【{signaldes}】请求状态变更：{(signalchange == 1 ? "请求" : "默认")}；" +
        //            $"下标通道号：{rspValueHandle};反馈下标通道号： {rspdownHandle};\r\n";
        //        if (signalchange == 0)
        //        {
        //            await PLC.WriteAsync(8, feedingRfid, rspdownHandle);//写入plc  Rfid
        //            await PLC.WriteAsync(9, (short)Status.Default, rspdownHandle);//写入plc
        //            msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【{signaldes}】复位PLC成功;下发状态:" + $"{Status.Default.GetDescription()}({(short)Status.Default});\r\n";
        //            LogHelper.Debug(msg, _log);
        //            return;
        //        }
        //        var status = Status.OK;

        //        feedingRfid = await PLC.ReadAsync<string>(2023, rspValueHandle);
        //        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【{signaldes}】获取辅料托杯码RFID：{feedingRfid}，下标：{rspValueHandle};\r\n";
        //        if (string.IsNullOrEmpty(feedingRfid) || feedingRfid.Length != GlobalConfig.RFIDLenght)
        //        {
        //            status = string.IsNullOrEmpty(feedingRfid) ? Status.NoRfid : Status.LenthNg;
        //            LogHelper.Error($"【{signaldes}】本地校验辅料托杯码失败，RFID为空或者长度不符，RFID:{feedingRfid}，下标：{rspValueHandle}", _log);
        //        }
        //        //不加batteryRfid.Length == GlobalConfig.RFIDLenght 判断，batteryRfid.Substring(0, 3)会报异常；
        //        if (feedingRfid.Length == GlobalConfig.RFIDLenght)
        //        {
        //            if (!string.IsNullOrEmpty(GlobalConfig.RFIDPrefix) && !GlobalConfig.RFIDPrefix.Contains(feedingRfid.Substring(0, 3)))
        //            {
        //                status = Status.RfidNg;
        //                LogHelper.Error($"【{signaldes}】托杯码编码规则有误，前三位不为{GlobalConfig.RFIDPrefix}，RFID:{feedingRfid}，" +
        //                    $"下标通道号：{rspValueHandle};反馈下标通道号： {rspdownHandle};", _log);
        //            }
        //            //var codekey = _basMachineInfoDal.GetList(m => m.ProcessNo == GlobalVariable.ProcessNo && m.UsedFlag == 1).FirstOrDefault().StationCode;
        //            var codekey = GlobalVariable.PLCParams.FirstOrDefault(m => m.ProcessNo == GlobalVariable.ProcessNo)?.StationCode;
        //            if (!string.IsNullOrEmpty(codekey) && !codekey.Contains(feedingRfid.Substring(3, 1)))
        //            {
        //                status = Status.RfidNg;
        //                LogHelper.Error($"【{signaldes}】托杯码编码规则有误，第四位不为{codekey}，RFID:{feedingRfid}，" +
        //                    $"下标通道号：{rspValueHandle};反馈下标通道号： {rspdownHandle};", _log);
        //            }
        //        }
        //        await PLC.WriteAsync(8, feedingRfid, rspdownHandle);//写入plc  Rfid
        //        await PLC.WriteAsync(9, (short)status, rspdownHandle);//写入plc
        //        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【{signaldes}】下发PLC成功，MES状态：{(GlobalVariable.MesScan != 20 ? "在线" : "离线")}；下发状态:" +
        //           $"{status.GetDescription()}({(short)status});\r\n";
        //        LogHelper.Debug(msg, _log);
        //    }
        //    catch (Exception ex)
        //    {
        //        await PLC.WriteAsync(9, (short)Status.LocalCheckNg, rspdownHandle);//上位机异常
        //        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【{signaldes}】下发PLC成功，下发状态:上位机异常\r\n";
        //        LogHelper.Debug(msg, _log);
        //        LogHelper.Error($"【{signaldes}】{plcVariableModel.VariableName}【{plcVariableModel.VariableHandle}】辅料入站异常：" + ex);
        //    }
        //}
        ///// <summary>
        ///// 电芯出站
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        //public virtual async void BatteryForOut(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    var msg = "";
        //    string _log = "BatteryForOut";
        //    object signal = plcVariableModel.VariableValue;
        //    int rspValueHandle = plcVariableModel.VariableHandle;
        //    int rspdownHandle = plcVariableModel.VariableDownHandle;
        //    var signaldes = plcVariableModel.VariableDescrition;
        //    dynamic CheckResult = null;
        //    string BatteryGuid = "";
        //    DateTime RequestTime = DateTime.Now;
        //    try
        //    {
        //        var signalchange = int.Parse(signal.ToString());
        //        msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]" +
        //            $"{plcVariableModel.VariableName}[{rspValueHandle}]信号[{signalchange}]状态为：{(signalchange == 1 ? "请求" : "默认")}；\r\n";

        //        if (signalchange == 0)
        //        {
        //            await PLC.WriteAsync(25, (short)Status.Default, rspdownHandle); //写入plc
        //            msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]" +
        //                $"给PLC反馈D{25}[{rspdownHandle}]结果:{Status.Default.GetDescription()}信号[{(short)Status.Default}]成功;\r\n";

        //            LogHelper.Debug(msg, _log);
        //            return;
        //        }
        //        var status = Status.OK;
        //        BatteryGuid = await PLC.ReadAsync<string>(2062, rspValueHandle);
        //        msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]读取D{2062}[{rspValueHandle}]当前电芯码GUID:{BatteryGuid};\r\n";

        //        if (string.IsNullOrEmpty(BatteryGuid) || BatteryGuid.Length < GlobalConfig.GUIDLenght)
        //        {
        //            status = string.IsNullOrEmpty(BatteryGuid) ? Status.NoRfid : Status.LenthNg;
        //            LogHelper.Error($"【出站】电芯码GUID:{BatteryGuid}为空或者长度不符,校验结果:{status.GetDescription()};", _log);
        //        }
        //        var DataResult = await SaveBatteryOutDataAsync(plcVariableModel, PLC, BatteryGuid); //下料保存数据                                                                                                    //
        //        status = (DataResult.result == 1 || status != Status.OK) ? status : DataResult.result == 4 ? Status.MesNg : DataResult.result == 3 ? Status.MesTimeOut : Status.LocalCheckNg;
        //        var time = DateTime.Now;
        //        await PLC.WriteAsync(25, (short)status, rspdownHandle); //写入plc
        //        var timeinter = DateTime.Now.Subtract(time).TotalMilliseconds;
        //        msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]MES:{(GlobalVariable.MesScan != 20 ? "在线" : "离线")}状态;" +
        //         $"给PLC反馈:D{25}[{rspdownHandle}]结果:{status.GetDescription()}信号[{(short)status}]成功;spends：{timeinter}ms;\r\n";

        //        if ((DateTime.Now - RequestTime).TotalMilliseconds > GlobalConfig.BatteryOutTimeOut)
        //        { msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]GUID:{BatteryGuid},出站耗时：{(DateTime.Now - RequestTime).TotalMilliseconds}毫秒;\r\n"; }

        //        LogHelper.Debug(msg, _log);
        //    }
        //    catch (Exception ex)
        //    {
        //        await PLC.WriteAsync(25, (short)Status.LocalCheckNg, rspdownHandle); //上位机异常
        //        msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]" +
        //             $"给PLC反馈:D{25}[{rspdownHandle}]结果:{Status.LocalCheckNg.GetDescription()}信号[{(short)Status.LocalCheckNg}]成功;\r\n";

        //        LogHelper.Debug(msg, _log);
        //        LogHelper.Error(
        //             $"【出站】{plcVariableModel.VariableName}[{rspValueHandle}]电芯出站异常：" +
        //             ex);
        //    }
        //}
        ///// <summary>
        ///// 辅料上料
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        //public virtual async void FeedingInRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    var msg = "";
        //    string _log = "FeedingInRequest";
        //    object signal = plcVariableModel.VariableValue;
        //    int rspValueHandle = plcVariableModel.VariableHandle;
        //    int rspdownHandle = plcVariableModel.VariableDownHandle;
        //    var signaldes = plcVariableModel.VariableDescrition;
        //    try
        //    {
        //        //预上料模式:PLC触发上料时进行MES换料请求，调用A025，
        //        //所传的报警ID可配置,后监听是否收到A047,若收到表示换料成功,否则换料失败。(超时时间暂定10s) --by-202310
        //        var signalchange = int.Parse(signal.ToString());
        //        int feedingNum = -1;

        //        msg += $"【辅料上料】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]{plcVariableModel.VariableName}[{rspValueHandle}]信号[{signalchange}]状态为：{(signalchange == 1 ? "请求" : signalchange == 2 ? "辅料上料触发" : "默认")}；\r\n";

        //        if (signalchange == 0)
        //        {
        //            //await PLC.WriteAsync(46, feedingNum, rspdownHandle);//写入plc  来料总数
        //            //await PLC.WriteAsync(47, (short)Status.Default, rspdownHandle);//写入plc  反馈确认
        //            //msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【{signaldes}】复位PLC成功;下发状态:" + $"{Status.Default.GetDescription()}({(short)Status.Default});\r\n";
        //            LogHelper.Debug(msg, _log);
        //            return;
        //        }
        //        var status = Status.OK;
        //        int strandedCount = 0;
        //        string feedingNo = "";
        //        if (signalchange == 1)
        //        {
        //            feedingNo = await PLC.ReadAsync<string>(2011, rspValueHandle);//获取辅料码

        //            msg += $"【辅料上料】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]读取D{2011}[{rspValueHandle}]获取当前批次号:{feedingNo};\r\n";

        //            if (string.IsNullOrEmpty(feedingNo))
        //            {
        //                status = Status.NoRfid;
        //                LogHelper.Error($"【辅料上料】批次:{feedingNo}的校验结果：{status.GetDescription()};", _log);
        //            }
        //            //MES校验
        //            if (status == Status.OK)
        //            {
        //                //MES在线
        //                if (GlobalVariable.MesScan != 20)
        //                {
        //                    //辅料上料
        //                    var CheckResult = api_mes.FeedingInCheck(feedingNo);
        //                    status = CheckResult.result ? status : CheckResult.flag == 1 ? Status.MesNg : Status.MesNg;
        //                    //feedingNum = int.Parse(api_mes.GetA047Result(feedingNo));
        //                    //上料数量获取由原A047更改为QueryMaterialsProcessed接口获取                        
        //                    var CheckResult2 = api_mes.Web_QueryMaterialsProcessed(feedingNo);
        //                    msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]批次号:{feedingNo}上料,从MES系统查询到数量为[{CheckResult2.num}];";
        //                    feedingNum = CheckResult2.num;
        //                }
        //            }
        //            await PLC.WriteAsync(47, (short)status, rspdownHandle);//写入plc  反馈确认
        //            await PLC.WriteAsync(46, feedingNum, rspdownHandle);//写入plc  来料总数

        //            msg += $"【辅料上料】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]MES:{(GlobalVariable.MesScan != 20 ? "在线" : "离线")}状态;" +
        //                   $"给PLC反馈:批次{feedingNo}的数量[{feedingNum}]结果:{status.GetDescription()}信号[{(short)status}]成功;\r\n";

        //            LogHelper.Debug(msg, _log);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        await PLC.WriteAsync(47, 2, rspdownHandle);//上位机异常
        //        msg += $"【辅料上料】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]{plcVariableModel.VariableName}[{rspValueHandle}]处理异常,给PLC反馈D{47}[{rspValueHandle}]结果:{Status.LocalCheckNg.GetDescription()}信号[{(short)Status.LocalCheckNg}]成功;\r\n";

        //        LogHelper.Debug(msg, _log);
        //        LogHelper.Error($"【辅料上料】{plcVariableModel.VariableName}[{rspValueHandle}]处理异常：" + ex);
        //    }
        //}
        ///// <summary>
        ///// 辅料下料
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        //public virtual async void FeedingOutRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    var msg = "";
        //    string _log = "FeedingOutRequest";
        //    object signal = plcVariableModel.VariableValue;
        //    int rspValueHandle = plcVariableModel.VariableHandle;
        //    int rspdownHandle = plcVariableModel.VariableDownHandle;
        //    var signaldes = plcVariableModel.VariableDescrition;
        //    try
        //    {
        //        //预上料模式:PLC触发下料时直接返回OK信号给PLC--by-2023-10

        //        var signalchange = int.Parse(signal.ToString());

        //        msg += $"【辅料下料】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]{plcVariableModel.VariableName}[{rspValueHandle}]信号[{signalchange}]状态为：{(signalchange == 1 ? "请求" : signalchange == 2 ? "辅料下料触发" : "默认")}；\r\n";

        //        if (signalchange == 0)
        //        {
        //            await PLC.WriteAsync(48, (short)Status.Default, rspdownHandle);//写入plc  反馈确认
        //            msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【{signaldes}】复位PLC成功;下发状态:" + $"{Status.Default.GetDescription()}({(short)Status.Default});\r\n";
        //            LogHelper.Debug(msg, _log);
        //            return;
        //        }
        //        var status = Status.OK;  //0：复位；1:OK；
        //        string LabelNumber = null;
        //        int MaterialQuality = 0;

        //        string feedingNo = "";
        //        string feedingNum = "";
        //        int strandedCount = 0;//剩余数量
        //        if (signalchange == 1)
        //        {
        //            feedingNo = await PLC.ReadAsync<string>(2014, rspValueHandle);//获取辅料码
        //            strandedCount = await PLC.ReadAsync<int>(2013, rspValueHandle);//获取辅料剩余数量

        //            msg += $"【辅料下料】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]读取D{2014}[{rspValueHandle}]获取当前批次号:{feedingNo};D{2013}[{rspValueHandle}]获取剩余数量[{strandedCount}];\r\n";

        //            if (string.IsNullOrEmpty(feedingNo))
        //            {
        //                status = Status.NoRfid;
        //                LogHelper.Error($"【辅料下料】批次:{feedingNo}的校验结果：{status.GetDescription()};", _log);
        //            }
        //            //MES校验
        //            if (status == Status.OK)
        //            {
        //                //MES在线
        //                if (GlobalVariable.MesScan != 20)
        //                {
        //                    //辅料系统消耗数量查询
        //                    var CheckResult2 = api_mes.Web_QueryMaterialsProcessed(feedingNo);
        //                    msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]批次号:{feedingNo}开始下料,PLC发来的剩余数量为[{strandedCount}],从MES系统查询到剩余数量为[{CheckResult2.num}];";
        //                    if (strandedCount < GlobalVariable.FeedingOutTH)
        //                    {
        //                        //收到下料物料剩余数量小于设置阈值，上传剩余数量为0；
        //                        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}],批次号:{feedingNo}下料,收到PLC发来的剩余数量为[{strandedCount}],小于设置阈值故上传剩余数量为[{0}];";

        //                        strandedCount = 0;

        //                    }
        //                    //辅料下料校验
        //                    var CheckResult = api_mes.FeedingOutCheck(feedingNo, strandedCount);
        //                    status = CheckResult.result ? status : CheckResult.flag == 1 ? Status.MesNg : Status.MesNg;
        //                }
        //            }
        //            await PLC.WriteAsync(48, (short)status, rspdownHandle);//写入plc  反馈确认                   

        //            msg += $"【辅料下料】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]MES:{(GlobalVariable.MesScan != 20 ? "在线" : "离线")}状态;" +
        //                  $"给PLC反馈:批次{feedingNo}的下料结果:{status.GetDescription()}信号[{(short)status}]成功;\r\n";

        //            LogHelper.Debug(msg, _log);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await PLC.WriteAsync(48, 2, rspdownHandle);//上位机异常
        //        msg += $"【辅料下料】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]{plcVariableModel.VariableName}[{rspValueHandle}]处理异常,给PLC反馈D{48}[{rspValueHandle}]结果:{Status.LocalCheckNg.GetDescription()}信号[{(short)Status.LocalCheckNg}]成功;\r\n";

        //        LogHelper.Debug(msg, _log);
        //        LogHelper.Error($"【辅料下料】{plcVariableModel.VariableName}[{rspValueHandle}]处理异常：" + ex);
        //    }
        //}
        ///// <summary>
        ///// 12装盘机绑盘触发
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        ///// <exception cref="NotImplementedException"></exception>
        //public virtual void TrayloadBindRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}
        //public virtual void HipotRealTimeRead(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void HipotHandleRead(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void HipotHandleWrite(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}
        //public virtual void WindNGForOut(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}
        //public virtual void WindFlawCoordinate(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// 设备维修
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        ///// <exception cref="NotImplementedException"></exception>
        //public virtual void RepairRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// 扫码请求
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        ///// <exception cref="NotImplementedException"></exception>
        //public virtual void ScannerRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// 下料出站数据保存和上传MES
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        ///// <returns></returns>
        //public virtual async Task<dynamic> SaveBatteryOutDataAsync(PLCVariableModel plcVariableModel, IHimmaPLC PLC, string BatteryGuid)
        //{
        //    var msg = "";
        //    var _log = "BatteryOutData";
        //    DateTime GetTime = DateTime.Now;
        //    DateTime DebugTime = DateTime.Now;
        //    object signal = plcVariableModel.VariableValue;
        //    int rspIndex = plcVariableModel.VariableHandle;
        //    string signaldes = plcVariableModel.VariableDescrition;
        //    var mesResult = true;
        //    List<Tuple<string, int>> outFlags = new List<Tuple<string, int>>();
        //    var Result = 1;//1:ok;2:ng;3:mes返回null；4mes  NG
        //    var message = "";
        //    string BatteryRfid = "";
        //    try
        //    {
        //        var signalchange = int.Parse(signal.ToString());
        //        if (signalchange == 0) return new { result = Result, message };
        //        msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]获取GUID:[{BatteryGuid}]的出站数据;\r\n";
        //        //参数list
        //        List<SaveBatteryOutDataModel> saveBatteryOutDataModels = new List<SaveBatteryOutDataModel>();
        //        List<SaveBatteryOutDataModel> uploadModels = new List<SaveBatteryOutDataModel>();
        //        List<SaveBatteryOutDataModel> saveModels = new List<SaveBatteryOutDataModel>();
        //        float[] floatData = await PLC.ReadAsync<float[]>(2063, rspIndex * GlobalConfig.FloateDataUpIntval, GlobalConfig.FloateDataUpIntval);
        //        string[] stringData = await PLC.ReadAsync<string[]>(2064, rspIndex * GlobalConfig.StrDataUpIntval, GlobalConfig.StrDataUpIntval);
        //        int intData = await PLC.ReadAsync<int>(2065, rspIndex);
        //        var station = await PLC.ReadAsync<string>(2061, rspIndex);

        //        foreach (var item in PLC._paraList)
        //        {
        //            string value = string.Empty;
        //            switch (item.ParaType)
        //            {
        //                case 1://浮点类型
        //                    value = floatData[item.ArrayNo].ToString();
        //                    break;
        //                case 2://字符串类型
        //                    value = stringData[item.ArrayNo];
        //                    break;
        //                default:
        //                    continue;
        //                    break;
        //            }
        //            var model = new SaveBatteryOutDataModel()
        //            {
        //                ParamID = item.ParaId,
        //                ParamDesc = item.ParaName,
        //                ParamValue = value
        //            };
        //            saveBatteryOutDataModels.Add(model);
        //            if (!string.IsNullOrEmpty(item.ParaId))
        //            {
        //                uploadModels.Add(model);
        //            }
        //            if (!string.IsNullOrEmpty(item.Remark))
        //            {
        //                saveModels.Add(model);
        //            }
        //        }
        //        saveModels.Add(new SaveBatteryOutDataModel()
        //        {
        //            ParamID = "2",
        //            ParamDesc = "时间",
        //            ParamValue = DateTime.Now.ToString()
        //        });
        //        BatteryRfid = stringData[0];
        //        msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]处理GUID:[{BatteryGuid}]的数据耗时:{(DateTime.Now - DebugTime).TotalMilliseconds}ms;\r\n";
        //        DebugTime = DateTime.Now;
        //        //MES在线
        //        if (GlobalVariable.MesScan != 20 && !string.IsNullOrEmpty(BatteryGuid) && BatteryGuid.Length == GlobalConfig.GUIDLenght)
        //        {
        //            var saveBatteryOutDataList = new List<SaveBatteryOutDataList>
        //            {
        //                new SaveBatteryOutDataList()
        //                {
        //                    SaveBatteryOutDataModels = uploadModels,
        //                    BatteryGuid = BatteryGuid
        //                }
        //            };
        //            var CheckResult = api_mes.UploadBatteryForOut(saveBatteryOutDataList, station);
        //            mesResult = CheckResult.result;
        //            message = CheckResult.message;
        //            outFlags = CheckResult.outFlags;
        //            if (outFlags.FirstOrDefault(x => x.Item1 == BatteryGuid)?.Item2 == 4)
        //            {
        //                //调用A085接口
        //                var SerialNos = new List<string>
        //                {
        //                    BatteryGuid
        //                };
        //                var Codes = new List<string>
        //                {
        //                    GlobalVariable.A085Code
        //                };
        //                api_mes.UpdateEmptyCup(SerialNos, Codes, GlobalVariable.A085Operation);
        //            }
        //            Result = (mesResult && outFlags.FirstOrDefault(x => x.Item1 == BatteryGuid)?.Item2 == 1) ? 1 : CheckResult.flag == 0 ? 3 : 4;
        //            msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]GUID:[{BatteryGuid}]出站MES反馈结果：{(Result == 1 ? "OK" : "NG")};MES反馈耗时：{(DateTime.Now - DebugTime).TotalMilliseconds}ms;\r\n";

        //        }
        //        #region redis

        //        // 数据存redis
        //        SaveDataToRedis(new BizBatteryResultModel()
        //        {
        //            Rfid = BatteryRfid,
        //            Guid = BatteryGuid,
        //            Result = Result == 1 ? "OK" : "NG",
        //            Ngcode = "出站",
        //            Model = "",
        //            Detail = saveBatteryOutDataModels.ToJson(),
        //            Isuploaded = (byte)(GlobalVariable.MesScan != 20 ? 0 : 1),
        //            CreateTime = DateTime.Now,
        //            Station = signaldes,
        //            StationIndex = station,
        //        }, 0, 2);
        //        #endregion redis
        //    }
        //    catch (Exception ex)
        //    {
        //        msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]数据处理异常:{ex};\r\n";
        //        LogHelper.Debug(msg, _log);
        //        LogHelper.Error($"【出站】异常{plcVariableModel.VariableName}[{rspIndex}]:{ex.StackTrace}", _log);
        //        Result = 2;
        //        message = "出站数据保存异常";
        //        return new { result = Result, message };
        //    }
        //    msg += $"【出站】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]GUID:[{BatteryGuid}]数据上传保存总耗时:{(DateTime.Now - GetTime).TotalMilliseconds}ms;\r\n";
        //    LogHelper.Debug(msg, _log);
        //    //数据保存预留
        //    return new { result = Result, message };
        //}

        //public async void UpdateWashFlag()
        //{
        //    try
        //    {
        //        var param = GlobalVariable.PLCParams.FirstOrDefault(x => x.ProcessNo == GlobalVariable.WashMachineNo);
        //        if (param == null)
        //        {
        //            NeedWashFlag = 0;
        //            LogHelper.Warn($"获取托杯清洗机失败，未在数据库配置此机型/此机型无需进行托杯清洗", "Wash");
        //            return;
        //        }
        //        var PLC = _icontainerProvider.Resolve<IHimmaPLC>(param.PLCIPAddress);
        //        if (PLC == null)
        //        {
        //            NeedWashFlag = 0;
        //            LogHelper.Warn($"获取托杯清洗机失败，未搜索到plc地址", "Wash");
        //            return;
        //        }
        //        while (GlobalVariable.State)
        //        {
        //            await Task.Delay(1000);
        //            NeedWashFlag = await PLC.ReadAsync<short>("D2217[0]");
        //            //LoggerHelper.Debug($"读取托杯清洗机信号成功，值为：【{NeedWashFlag}】", "Wash");
        //            AlarmSignal = await PLC.ReadAsync<short>("D2217[1]");
        //            if (AlarmSignal == 1)
        //            {
        //                await PLC.WriteAsync("D21[1]", (short)(AlarmSignal));
        //                LogHelper.Debug($"读取到托杯清洗机信号值为：【{NeedWashFlag}】，反馈写给底部穿透焊plc成功", "Wash");
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.Error($"读取托杯清洗机信号异常，异常信息：【{e}】", "Wash");
        //    }
        //}
        //public async void LWMDataSave()
        //{
        //    try
        //    {
        //        var param = GlobalVariable.PLCParams.FirstOrDefault(x => x.ProcessNo == "AnodeWeld");
        //        var PLC = _icontainerProvider.Resolve<IHimmaPLC>(param.PLCIPAddress);
        //        if (PLC == null)
        //        {
        //            LogHelper.Error($"LWM数据保存未搜索到plc地址", "LWM");
        //            return;
        //        }
        //        while (GlobalVariable.State)
        //        {
        //            await Task.Delay(1000);
        //            NeedWashFlag = await PLC.ReadAsync<short>("D2217[0]");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.Error($"LWM保存数据保存异常，异常信息：【{e}】", "LWM");
        //    }
        //}
        ///// <summary>
        ///// 读取设备模式状态读取--MES是否在线--是否套膜模式--是否需要采集--是否开启喷双码模式
        ///// </summary>
        ///// <param name="plc"></param>
        //public async void IsMesCanMnitor(IHimmaPLC plc)
        //{
        //    GlobalVariable.MesScan = 0;
        //    GlobalVariable.MesCanTub = 0;
        //    GlobalVariable.MesOutput = 0;
        //    GlobalVariable.MesIJPmode = 0;
        //    if (plc == null)
        //    {
        //        LogHelper.Error($"未搜索到plc地址", "IsControl");
        //        return;
        //    }
        //    while (GlobalVariable.State)
        //    {
        //        try
        //        {
        //            await Task.Delay(1000);
        //            GlobalVariable.MesOutput = await plc.ReadAsync<short>("D2218[2]");
        //            //只记录主PLC数据
        //            if (plc.ProcessNo == GlobalVariable.ProcessNo)
        //            {
        //                GlobalVariable.MesScan = await plc.ReadAsync<short>("D2218[0]");
        //                GlobalVariable.MesCanTub = await plc.ReadAsync<short>("D2218[1]");
        //                GlobalVariable.MesIJPmode = await plc.ReadAsync<short>("D2218[3]");
        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            LogHelper.Error($"读取plc管控信号异常，异常信息：【{e}】", "IsControl");
        //        }
        //    }
        //}
        ///// <summary>
        ///// 托杯交换机双开模式标识信号 -- 0表示普通单开模式,10表示开启双开模式(C100和JP40)
        ///// </summary>
        ///// <param name="plc"></param>
        //public async void IsMesPEXlMnitor(IHimmaPLC plc)
        //{
        //    GlobalVariable.MesPEXupload = 0;
        //    if (plc == null)
        //    {
        //        LogHelper.Error($"未搜索到plc地址", "IsControl");
        //        return;
        //    }
        //    while (GlobalVariable.State)
        //    {
        //        try
        //        {
        //            await Task.Delay(1000);

        //            GlobalVariable.MesPEXupload = await plc.ReadAsync<short>("D2218[4]");
        //            //LogHelper.Debug($"读取{plc.ProcessNo}管控信号值为{GlobalVariable.MesPEXupload}", "IsControl");
        //        }
        //        catch (Exception e)
        //        {
        //            LogHelper.Error($"读取托杯交换机PLC管控信号异常，异常信息：【{e}】", "IsControl");
        //        }
        //    }
        //}
        ///// <summary>
        ///// 定期下发时间给plc
        ///// </summary>
        ///// <param name="plc"></param>
        //public async void TimeMonitor(IHimmaPLC plc)
        //{
        //    if (plc == null)
        //    {
        //        return;
        //    }
        //    while (GlobalVariable.State)
        //    {
        //        try
        //        {
        //            await Task.Delay(60000);
        //            await plc.WriteAsync("D2[0]", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //            await plc.WriteAsync("D1", 1);
        //            LogHelper.Info($"[{plc.Name}]时间更新成功:【{DateTime.Now.ToString("yyyyMMddHHmmss")}】");
        //            //获取线程数据
        //            if (GlobalConfig.UsedThreadFlag == 1) { GetWorkingThreadCount(); }
        //            await Task.Delay(GlobalVariable.TimeCheckCycle);
        //        }
        //        catch (Exception e)
        //        {
        //            await Task.Delay(GlobalVariable.TimeCheckCycle);
        //            LogHelper.Error($"时间更新异常，异常信息：【{e}】");
        //        }

        //    }
        //}
        //public void SaveCupClearData()
        //{
        //    //biz_cup_clear 托杯清洗电芯记录
        //    List<BizCupClearModel> bizCupClearModels = _bizCupClearDAL.GetList();
        //    CacheManager.Save("bizCupClearModels", bizCupClearModels);

        //    //biz_battery 记录电芯出站入站顺序
        //    List<BizBatteryModel> bizBatteryModels = _bizBatteryModels.GetList();
        //    var bizBatteryModels_observable = new ObservableList<BizBatteryModel>(bizBatteryModels);
        //    CacheManager.Save("bizBatteryModels", bizBatteryModels_observable);
        //    //分选机缓存托杯和电芯码用来分流
        //    List<BizBatteryResultModel> bizWaoDataModels = _bizWaoDataModels.GetList();
        //    var bizWaoDataModels_observable = new ObservableList<BizBatteryResultModel>(bizWaoDataModels);
        //    CacheManager.Save("bizWaoDataModels", bizWaoDataModels_observable);
        //    // 托杯交换后biz_s_cup表数据
        //    List<BizCansupplyDataModel> bizCansupplyDataModels = _bizCansupplyDataDal.GetList();
        //    var bizCansupplyDataModels_observable = new ObservableList<BizCansupplyDataModel>(bizCansupplyDataModels);
        //    CacheManager.Save("bizCansupplyDataModels", bizCansupplyDataModels_observable);
        //    // JP30入壳机的托杯交换机数据
        //    List<BizPuckExchangeDataModel> bizPuckExchangeDataModels = _bizPuckExchangeDataDal.GetList();
        //    var bizPuckExchangeDataModels_observable = new ObservableList<BizPuckExchangeDataModel>(bizPuckExchangeDataModels);
        //    CacheManager.Save("bizPuckExchangeDataModels", bizPuckExchangeDataModels_observable);

        //    //LWM数据
        //    List<BizBatterySNModel> bizBatterySNMdels = _bizBatterySNDAL.GetList();
        //    CacheManager.Save("bizBatterySNMdels", bizBatterySNMdels);
        //}

        ///// <summary>
        ///// 获取生产数据保存标题
        ///// </summary>
        //private string GetTitle(List<BasParaVariableModel> models)
        //{
        //    string title = "";
        //    try
        //    {
        //        models.ForEach(para => {
        //            title += "," + para.ParaName;
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error($"获取生产数据标签异常：" + ex);
        //    }
        //    return title;
        //}

        ///// <summary>
        ///// 托杯交换
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        ///// <exception cref="NotImplementedException"></exception>
        //public virtual async void CupChangeRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    var msg = "";
        //    string _log = "CupChangeRequest";
        //    object signal = plcVariableModel.VariableValue;
        //    int rspValueHandle = plcVariableModel.VariableHandle;
        //    int rspdownHandle = plcVariableModel.VariableDownHandle;
        //    var signaldes = plcVariableModel.VariableDescrition;
        //    DateTime RequestTime = DateTime.Now;
        //    try
        //    {
        //        string batteryData = "";
        //        string batteryRfidNew = "";
        //        string batteryGuid = "";
        //        var status = Status.OK;

        //        var signalchange = int.Parse(signal.ToString());
        //        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【托杯绑定】{plcVariableModel.VariableName}[{rspValueHandle}]信号[{signalchange}]状态为：{(signalchange == 1 ? "请求" : "默认")}；\r\n";

        //        if (signalchange == 0)
        //        {
        //            await PLC.WriteAsync(28, (short)Status.Default, rspdownHandle);//写入plc
        //            msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]" +
        //                  $"给PLC反馈:D{28}[{rspdownHandle}]结果:{Status.Default.GetDescription()}信号[{(short)Status.Default}]成功;\r\n";

        //            LogHelper.Debug(msg, _log);
        //            return;
        //        }

        //        batteryGuid = await PLC.ReadAsync<string>(2055, rspValueHandle);
        //        batteryRfidNew = await PLC.ReadAsync<string>(2056, rspValueHandle);
        //        msg += $"【托杯绑定】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]" +
        //            $"读取D{2055}[{rspValueHandle}]GUID:{batteryGuid};读取D{2056}[{rspValueHandle}]RFID:{batteryRfidNew};\r\n";

        //        if (string.IsNullOrEmpty(batteryGuid) || string.IsNullOrEmpty(batteryRfidNew) || batteryRfidNew.Length != GlobalConfig.RFIDLenght)
        //        {
        //            status = Status.NoRfid; if (batteryRfidNew.Length != GlobalConfig.RFIDLenght) { status = Status.LenthNg; }
        //            LogHelper.Error($"【托杯绑定】RFID:{batteryRfidNew};GUID:{batteryGuid};托杯绑定本地校验NG[{status.GetDescription()}]", _log);

        //        }
        //        if (status == Status.OK && !GlobalConfig.RFIDEncode.Contains(batteryRfidNew.Substring(0, 4)) && GlobalVariable.MesScan != 20)
        //        {
        //            var CheckResult = api_mes.RfidBindGuid(batteryRfidNew, batteryGuid);
        //            status = Status.RfidNg;
        //            LogHelper.Error($"【托杯绑定】RFID:{batteryRfidNew};出站托杯绑定编码规则有误，前四位不为[{GlobalConfig.RFIDEncode}],校验NG[{status.GetDescription()}]", _log);

        //        }
        //        //MES在线
        //        if (GlobalVariable.MesScan != 20 && status == Status.OK)
        //        {
        //            var CheckResult = api_mes.RfidBindGuid(batteryRfidNew, batteryGuid);
        //            //var CheckResult = await api_mes.RfidBindGuidAsync(batteryRfidNew, batteryGuid);
        //            status = CheckResult.result ? status : CheckResult.flag == 1 ? Status.MesNg : Status.MesTimeOut;
        //        }

        //        await PLC.WriteAsync(28, (short)status, rspdownHandle);//写入plc
        //        msg += $"【托杯绑定】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]MES:{(GlobalVariable.MesScan != 20 ? "在线" : "离线")}状态;" +
        //          $"给PLC反馈:D{28}[{rspValueHandle}]结果:{status.GetDescription()}信号[{(short)status}]成功;\r\n";

        //        if ((DateTime.Now - RequestTime).TotalMilliseconds > GlobalConfig.BatteryBindingTimeOut)
        //        { msg += $"【托杯绑定】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]RFID:[{batteryRfidNew}];GUID:[{batteryGuid}];托杯绑定总耗时：{(DateTime.Now - RequestTime).TotalMilliseconds}ms;\r\n"; }

        //        LogHelper.Debug(msg, _log);
        //    }
        //    catch (Exception ex)
        //    {
        //        await PLC.WriteAsync(28, (short)Status.LocalCheckNg, rspdownHandle);//上位机异常
        //        msg += $"【托杯绑定】[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]" +
        //              $"给PLC反馈:D{28}[{rspdownHandle}]结果:{Status.LocalCheckNg.GetDescription()}信号[{(short)Status.LocalCheckNg}]成功;\r\n";

        //        LogHelper.Debug(msg, _log);
        //        LogHelper.Error($"【托杯绑定】{plcVariableModel.VariableName}[{rspValueHandle}]托杯绑定异常：" + ex);
        //    }
        //}
        ///// <summary>
        ///// 喷码信息上传
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        ///// <exception cref="NotImplementedException"></exception>
        //public virtual void PrintRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// 实时数据采集
        ///// </summary>
        ///// <param name="signal"></param>
        //public virtual void CollectRealTimeData(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    //string _log = "CollectRealTimeData";
        //    //object signal = plcVariableModel.VariableValue;
        //    //int rspValueHandle = plcVariableModel.VariableHandle;
        //    //int rspdownHandle = plcVariableModel.VariableDownHandle;
        //    //var signaldes = plcVariableModel.VariableDescrition;
        //    //try
        //    //{
        //    //    var signalchange = int.Parse(signal.ToString());
        //    //    LogHelper.Debug($"【{signaldes}】请求状态变更：{(signalchange == 1 ? "请求" : "默认")}；" +
        //    //        $"下标通道号：{rspValueHandle};反馈下标通道号： {rspdownHandle};", _log);
        //    //    if (signalchange == 0) return;  //变更默认不执行
        //    //    var status = 1;  //0：复位；1:OK；2; NG；3.上位机异常；4.MES异常;5.清洗；
        //    //    await PLC.WriteAsync(6, status, rspdownHandle);//写入plc
        //    //    LogHelper.Debug($"【{signaldes}】下发PLC成功，MES状态：{(GlobalVariable.MesScan ? "在线" : "离线")}；下发状态:" +
        //    //        $"{(status == 1 ? "OK" : status == 2 ? "NG" : status == 3 ? "上位机异常" : status == 4 ? "MES返回NG" : status == 5 ? "清洗" : "未知")}({status})", _log);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    await PLC.WriteAsync(6, 3, rspdownHandle);//上位机异常
        //    //    LogHelper.Debug($"【{signaldes}】下发PLC成功，下发状态:上位机异常", _log);
        //    //    LogHelper.Error($"{plcVariableModel.VariableName}【{plcVariableModel.VariableHandle}】电芯入站异常：" + ex.Message);
        //    //}
        //}

        //#endregion 过程数据变量触发

       
       
        ///// <summary>
        ///// AGV上料
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="data"></param>
        ////private async void Agv__getdata(object obj, string data)
        ////{
        ////    var msg = "";
        ////    string LotNo = "";
        ////    string ContainerBarcode = "";
        ////    int LocalStationID = 0;
        ////    var status = 1;
        ////    var ResultMsg = "物料校验成功";
        ////    var Code = 0;
        ////    var Msg = "成功";
        ////    var res = new AgvJsonModel_response();
        ////    var request = data.JsonToObject<AgvJsonModel_request>();
        ////    var agv = _agvs.FirstOrDefault(x => x.ProcessNo == GlobalVariable.ProcessNo);
        ////    try
        ////    {
        ////        var param = GlobalVariable.PLCParams.FirstOrDefault(x => x.ProcessNo == _agvs.First().Remarks);
        ////        var PLC = _icontainerProvider.Resolve<IHimmaPLC>(param.PLCIPAddress);
        ////        if (request != null)
        ////        {
        ////            var component = _scanners.FirstOrDefault(t => t.ProcessNo == PLC.ProcessNo);
        ////            LotNo = request.GoodsList.FirstOrDefault()?.ContainerBarcode;
        ////            if (component != null)
        ////            {
        ////                msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【辅料AGV上料，扫码枪触发】{component.Name} ；\r\n";
        ////                if (!component.IsConnected)//如果连接状态异常
        ////                {
        ////                    //component.ReConnectAsync();
        ////                    LogHelper.Error($"【辅料AGV上料，扫码枪触发】{component.Name}未连接,请处理！", "AGV");
        ////                    Code = 1;
        ////                    Msg = "失败";
        ////                    ResultMsg = "扫码枪未连接";
        ////                    res.RequestID = request.RequestID;
        ////                    res.Code = Code.ToString();
        ////                    res.Msg = Msg.ToString();
        ////                    res.Data = new List<AgvJsonModel_response_data>();
        ////                    agv?.SendData(res.ToJson() + "$$");
        ////                    msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【辅料AGV上料，扫码枪未连接】{component.Name}给AGV反馈{res.ToJson()} ；\r\n";
        ////                    LogHelper.Error(msg, "AGV");
        ////                    return;
        ////                }
        ////                var res2 = BarcodeCheck(component); //读码校验                      
        ////                if (res2.Item1 == 1)
        ////                {
        ////                    string trayNo = res2.Item2;
        ////                    msg +=
        ////                        $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【辅料AGV上料，扫码枪触发】{component.Name} 条码为：{trayNo} , AVG条码为{LotNo}\r\n";
        ////                    if (trayNo != LotNo)
        ////                    {

        ////                        status = 2;
        ////                        Code = 1;
        ////                        Msg = "条码和实物不匹配";
        ////                        ResultMsg = "条码和实物不匹配";
        ////                    }
        ////                    msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【辅料AGV上料，扫码枪触发】{component.Name} 获取条码成功,校验结果：【{(status == 1 ? "OK" : "NG")}】\r\n";
        ////                }
        ////                if (res2.Item1 == 3)
        ////                {
        ////                    status = 2;
        ////                    Code = 1;
        ////                    Msg = "ERROR";
        ////                    ResultMsg = "ERROR";
        ////                    msg += $"[{DateTime.Now.ToString("yyyy-l-dd HH:mn:55 fff")}] 【辅料AGV上料，扫码枪读码】{component.Name} 扫码ERROR\r\n";
        ////                }
        ////            }
        ////            try
        ////            {
        ////                res.RequestID = request.RequestID;
        ////                res.Code = Code.ToString();
        ////                res.Msg = Msg.ToString();
        ////                res.Data = new List<AgvJsonModel_response_data>();
        ////                request.GoodsList.ForEach(x =>
        ////                {
        ////                    res.Data.Add(new AgvJsonModel_response_data()
        ////                    {
        ////                        LotNo = x.LotNo,
        ////                        ContainerBarcode = x.ContainerBarcode,
        ////                        CheckResult = status == 1 ? "1" : "0",
        ////                        CheckResultMsg = ResultMsg
        ////                    });
        ////                    if (PLC.ProcessNo == "Wind")
        ////                    {
        ////                        ContainerBarcode = x.ContainerBarcode.Trim().ToString();
        ////                    }
        ////                    else { LotNo = x.LotNo.Trim().ToString(); }
        ////                });
        ////                agv?.SendData(res.ToJson() + "$$");
        ////                LogHelper.Debug($"向AGV发送反馈完成：{res.ToJson()}", "AGV");
        ////            }
        ////            catch (Exception e)
        ////            {
        ////                Code = 1;
        ////                Msg = "发送的请求不符合要求";
        ////                res.RequestID = request.RequestID;
        ////                res.Code = Code.ToString();
        ////                res.Msg = Msg.ToString();
        ////                res.Data = new List<AgvJsonModel_response_data>();
        ////                LogHelper.Error($"AGV物料信息获取异常：" + e, "AGV");
        ////                agv?.SendData(res.ToJson() + "$$");
        ////                LogHelper.Error($"向AGV发送物料异常反馈：{res.ToJson()}", "AGV");
        ////            }
        ////        }
        ////        if (PLC.ProcessNo == "Wind")
        ////        {
        ////            // LocalStationID 面向机台从左到右的轴分别是1、2、3、4
        ////            LocalStationID = int.Parse(request.LocalStationID);
        ////            switch (LocalStationID)
        ////            {
        ////                case 1://浮点类型
        ////                    await PLC.WriteAsync("D53[0]", ContainerBarcode);
        ////                    //await PLC.WriteAsync("D52[0]", (short)(status == 1 ? 1 : 2));
        ////                    break;
        ////                case 2://字符串类型
        ////                    await PLC.WriteAsync("D53[1]", ContainerBarcode);
        ////                    //await PLC.WriteAsync("D52[1]", (short)(status == 1 ? 1 : 2));
        ////                    break;

        ////                case 3:
        ////                    await PLC.WriteAsync("D53[2]", ContainerBarcode);
        ////                    //await PLC.WriteAsync("D52[2]", (short)(status == 1 ? 1 : 2));
        ////                    break;
        ////                case 4:
        ////                    await PLC.WriteAsync("D53[3]", ContainerBarcode);
        ////                    //await PLC.WriteAsync("D52[3]", (short)(status == 1 ? 1 : 2));
        ////                    break;

        ////            }
        ////            //await PLC.WriteAsync("D53[0]", ContainerBarcode);
        ////            //await PLC.WriteAsync("D52[0]", (short)(status == 1 ? 1 : 2));
        ////        }
        ////        else
        ////        {
        ////            //辅料预上料模式:
        ////            //接收到AGV传的批次号后,读PLC中上一批次号,进行比较,若相同则直接给PLC返回OK结果,否则进行MES预上料,并将MES结果反馈PLC；

        ////            await PLC.WriteAsync("D53[0]", LotNo);
        ////            await PLC.WriteAsync("D52[0]", (short)(status == 1 ? 1 : 2));
        ////        }
        ////        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]AGV物料信号,下发plc数据成功：卷筒码:【{ContainerBarcode}】,批次号：【{LotNo}】,轴:【{LocalStationID}】结果：【{(status == 1 ? "OK" : "NG")}】\r\n";
        ////        LogHelper.Debug(msg, "AGV");
        ////    }
        ////    catch (Exception e)
        ////    {

        ////        Code = 1;
        ////        Msg = "失败";
        ////        ResultMsg = "扫码枪未连接";
        ////        res.RequestID = request.RequestID;
        ////        res.Code = Code.ToString();
        ////        res.Msg = Msg.ToString();
        ////        res.Data = new List<AgvJsonModel_response_data>();
        ////        agv?.SendData(res.ToJson() + "$$");
        ////        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}],向AGV发送物料异常反馈：{res.ToJson()};  异常{e}\r\n;";
        ////        LogHelper.Error(msg, "AGV");
        ////    }
        ////}
        //#region 折叠
        ////public async void SaveDataToRedis(BizBatteryResultModel bizBatteryResultModel, int count = 0)
        ////{
        ////    var msg = "";
        ////    try
        ////    {
        ////        //Task.Run(() =>
        ////        //{
        ////        DateTime DebugTime = DateTime.Now;
        ////        //// 数据存redis
        ////        count++;
        ////        var RedisReturn = await  CommonRedis.ListAddToLeftAsyncString("BizBatteryResultModel", bizBatteryResultModel.ToJson());
        ////        if (RedisReturn > 0)
        ////        {
        ////            msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【存储数据至Redis】 耗时{(DateTime.Now - DebugTime).TotalMilliseconds}毫秒\r\n";
        ////        }
        ////        else if (count < 3)
        ////        {
        ////            msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]【存储数据至Redis】第{count}次失败，正在尝试重新存储 耗时{(DateTime.Now - DebugTime).TotalMilliseconds}毫秒\r\n";
        ////            SaveDataToRedis(bizBatteryResultModel, count);
        ////        }
        ////        LogHelper.Debug(msg, "SaveDataToRedis");
        ////        // });
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        LogHelper.Error($"【存储数据至Redis】 count:[{count}]，异常：" + e, "SaveDataToRedis");
        ////        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]存储至Redis异常\r\n";
        ////        LogHelper.Debug(msg, "SaveDataToRedis");
        ////    }
        ////}
        //#endregion

        /// <summary>
        /// 生产数据保存
        /// </summary>
        /// <param name="bizBatteryResultModel"></param>
        /// <param name="count"></param>
        /// <param name="BetteryType">1：入站；2：出站；默认：1</param>
        //public async void SaveDataToRedis(BizBatteryResultModel bizBatteryResultModel, int count = 0, int BetteryType = 1)
        //{
        //    var msg = "";
        //    try
        //    {
        //        DateTime DebugTime = DateTime.Now;
        //        // 数据存redis
        //        if (BetteryType == 1) //入站
        //        {
        //            //await  CommonRedis.ListAddToLeftAsyncString("BizBatteryResultModel", bizBatteryResultModel.ToJson());
        //            await CommonRedis.ListAddToLeftAsyncString("BizBatteryResultModel", bizBatteryResultModel.ToJson());
        //        }
        //        else //出站
        //        {
        //            //await  CommonRedis.ListAddToLeftAsyncString("BizBatteryResultModelOut", bizBatteryResultModel.ToJson());
        //            await CommonRedis.ListAddToLeftAsyncString("BizBatteryResultModelOut", bizBatteryResultModel.ToJson());
        //        }
        //        //LogHelper.Debug(msg, "SaveDataToRedis");
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.Error($"【存储数据至Redis】 count:[{count}]，异常：" + e, "SaveDataToRedis");
        //        msg += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]存储至Redis异常\r\n";
        //        LogHelper.Debug(msg, "SaveDataToRedis");
        //    }
        //}

        /// <summary>
        /// 生成指定长度的随机字符串
        /// </summary>
        /// <param name="intLength">随机字符串长度</param>
        /// <param name="booNumber">生成的字符串中是否包含数字</param>
        /// <param name="booSmallword">生成的字符串中是否包含小写字母</param>
        /// <param name="booBigword">生成的字符串中是否包含大写字母</param>
        /// <returns></returns>
        public virtual string CreatBatteryGuid(int intLength, bool booNumber = true, bool booSmallword = true, bool booBigword = true)
        {
            //定义
            Random ranA = new Random();
            int intResultRound = 0;
            int intA = 0;
            string strB = "";
            while (intResultRound < intLength)
            {
                //生成随机数A，表示生成类型
                //1=数字，2=符号，3=小写字母，4=大写字母
                intA = ranA.Next(1, 5);
                //如果随机数A=1，则运行生成数字
                //生成随机数A，范围在0-10
                //把随机数A，转成字符
                //生成完，位数+1，字符串累加，结束本次循环

                if (intA == 1 && booNumber)
                {
                    intA = ranA.Next(0, 10);
                    strB = intA.ToString() + strB;
                    intResultRound = intResultRound + 1;
                    continue;
                }
                //如果随机数A=3，则运行生成小写字母
                //生成随机数A，范围在97-122
                //把随机数A，转成字符
                //生成完，位数+1，字符串累加，结束本次循环

                if (intA == 3 && booSmallword == true)
                {
                    intA = ranA.Next(97, 123);
                    strB = ((char)intA).ToString() + strB;
                    intResultRound = intResultRound + 1;
                    continue;
                }
                //如果随机数A=4，则运行生成大写字母
                //生成随机数A，范围在65-90
                //把随机数A，转成字符
                //生成完，位数+1，字符串累加，结束本次循环

                if (intA == 4 && booBigword == true)
                {
                    intA = ranA.Next(65, 89);
                    strB = ((char)intA).ToString() + strB;
                    intResultRound = intResultRound + 1;
                    continue;
                }
            }
            return strB;
        }

        public static int batteryGuidCounter = 1; // 递增计数器，初始为 1

        public virtual string CreatBatteryGuid(int length)
        {
            //GUID 的组成部分 固定的前缀3位[HJ4] + 1位的年末位[5] + 2位的周次[09] + 2位的月份[02]  + 后几位[0001-9999]
            string GUIDPrefix = "HJ4"; // 固定前缀"HJ4"GlobalConfig.GUIDPrefix
            string lastDigitOfYear = DateTime.Now.Year.ToString().Last().ToString(); // 当前年份最后一位
            // 使用 GregorianCalendar 获取当前日期是这一年的第几周
            var calendar = new GregorianCalendar();
            int weekOfYear = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday); // 以星期一为一周的第一天
            string weekString = weekOfYear.ToString("D2"); // 转换为两位数
            string monthString = DateTime.Now.ToString("MM"); // 当前月份（MM格式）

            // 计算递增数的位数
            int incrementLength = length - (GUIDPrefix.Length + 1 + 2 + 2); // 减去前面8位，确定后面递增数的位数
            if (incrementLength <= 0)
            {
                throw new InvalidOperationException("GUID长度配置不正确，无法生成递增数。");
            }

            // 生成递增数（需要格式化为适当的位数）
            string incrementString = batteryGuidCounter.ToString($"D{incrementLength}");

            // 组合生成最终的 batteryGuid
            string batteryGuid = $"{GUIDPrefix}{lastDigitOfYear}{weekString}{monthString}{incrementString}";

            // 更新递增计数器，确保它在 0001 到 9999 之间循环
            batteryGuidCounter++;
            if (batteryGuidCounter > 9999)
            {
                batteryGuidCounter = 1; // 重置为 0001
            }

            return batteryGuid;
        }
        //public virtual async void PEXuploadRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC)
        //{
        //    throw new NotImplementedException();
        //}
        Int64 old_threadNum = 0;
        /// <summary>
        /// 获取当前线程池使用情况
        /// </summary>
        public void GetWorkingThreadCount()
        {

            int MaxWorkerThreads, miot, AvailableWorkerThreads, aiot;
            //获得最大的线程数量
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out miot);

            //获得可用的线程数量
            ThreadPool.GetAvailableThreads(out AvailableWorkerThreads, out aiot);

            //返回线程池中活动的线程数
            var newnum = MaxWorkerThreads - AvailableWorkerThreads;
            ProcessThreadCollection threadCollection = Process.GetCurrentProcess().Threads;

            var newmsg = $"当前线程数:{threadCollection.Count};当前活跃的线程数:{newnum};当前最大的线程数:{MaxWorkerThreads};当前可用的线程数:{AvailableWorkerThreads}";

            if (threadCollection.Count > old_threadNum && threadCollection.Count > GlobalConfig.ThreadNum) //
            {
                LogHelper.Debug(newmsg, "Thread");
                old_threadNum = threadCollection.Count;
            }
        }

        public void InitComponent()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }
    }
}