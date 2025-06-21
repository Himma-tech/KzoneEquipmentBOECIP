using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Himma.Common.DAL.Variable
{
    /// <summary>
    /// 全局参数
    /// 版本：1.0
    /// 更新历史：
    /// </summary>
    public class GlobalVariable
    {
        //private static BasAppConfigDAL appConfigDAL = new BasAppConfigDAL();
       
        public static Action<int> mesModeChanged;
        /// <summary>
        /// 获取系统参数//获取的系统参数其实就是最有意义的的值，而不是汉字名字
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        private static string GetConfigValue(string configName)
        {
            try
            {
                //BasAppConfigModel appConfigModels = appConfigDAL.CurrentDb.GetSingle(model => model.Name == configName); //appConfigDAL.Db.Queryable<AppConfigModel>().Where(model => model.Name.Contains(configName)).ToList();
                //return appConfigModels?.Value ?? "";
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="configCommon"></param>
        /// <returns></returns>
        public static string GetConfigValueByCommon(string configCommon)
        {
            try
            {
                //BasAppConfigModel appConfigModels = appConfigDAL.CurrentDb.GetSingle(model => model.Common == configCommon); //appConfigDAL.Db.Queryable<AppConfigModel>().Where(model => model.Name.Contains(configName)).ToList();
                //return appConfigModels.Value;
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        ///// 获取系统配置
        ///// </summary>
        ///// <param name="configName"></param>
        ///// <returns></returns>
        //public static BasAppConfigModel GetConfigValueByProcess(string configName)
        //{
        //    try
        //    {
        //        //BasAppConfigModel appConfigModel = appConfigDAL.CurrentDb.GetSingle(model => model.Name == configName && model.ProcessNo == ProcessNo); //appConfigDAL.Db.Queryable<AppConfigModel>().Where(model => model.Name.Contains(configName)).ToList();
        //        //return appConfigModel;
                
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// 基础信息修改
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AppValue"></param>
        /// <returns></returns>
        //private static void ChangeConfig(string configName, string configValue)
        //{
        //    try
        //    {
        //        appConfigDAL.CurrentDb.Update(model => new BasAppConfigModel() { Value = configValue }, model => model.Name == configName);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}


      

        //public static bool UpperToPlcHasChange = true;
        public static bool UpperToPlcFloatDataHasChange = true;
        /// <summary>
        /// 发送给plc的信号
        /// </summary>
        //public static Com.Models.CommonModel.PLCObservableCollection<int> UpperToPlc = new Com.Models.CommonModel.PLCObservableCollection<int>();
        //public static Com.Models.CommonModel.PLCObservableCollection<float> UpperToPlcFloatData = new Com.Models.CommonModel.PLCObservableCollection<float>();

        /// <summary>
        /// PLC给上位机的信号
        /// </summary>
        public static ObservableCollection<int> PLCVariables = new ObservableCollection<int>();
        ///// <summary>
        /////  发送给plc的信号 回读
        ///// </summary>
        public static ObservableCollection<int> UpperToPlcRead = new ObservableCollection<int>();


        //private static int machineState = 10;

        /// <summary>
        /// 是否刷新前台批次号界面
        /// </summary>
        public static bool materbrush = false;
        /// <summary>
        /// 设备状态
        /// </summary>
        public static int MachineState
        {
            get
            {
                return int.Parse(GetConfigValue("MachineState"));
            }
            set
            {
                //ChangeConfig("MachineState", value.ToString());
            }
        }
        /// <summary>
        /// 工序号
        /// </summary>
        public static string ProcessNo = ConfigurationManager.AppSettings["ProcessNo"];
        /// <summary>
        /// 底壳穿透焊LWM计数
        /// </summary>
        public static int LwmSum
        {
            get
            {
                return int.Parse(GetConfigValue("LwmSum"));
            }
            set
            {
                //ChangeConfig("LwmSum", value.ToString());
            }
        }
        /// <summary>
        /// 底壳穿透焊LWM计数
        /// </summary>
        public static int LwmLastValue
        {
            get
            {
                return int.Parse(GetConfigValue("LwmLastValue"));
            }
            set
            {
               // ChangeConfig("LwmLastValue", value.ToString());
            }
        }
        /// <summary>
        ///  A013出站初始个数
        /// </summary>
        public static int A013UsedNum = 0;
        /// <summary>
        ///  A013出站同时使用超时个数
        /// </summary>
        public static int A013ControlNum = int.Parse(ConfigurationManager.AppSettings["A013ControlNum"] ?? "2");
        /// <summary>
        /// A013出站信号量
        /// </summary>
        public static Semaphore A013ControlSem = new Semaphore(A013ControlNum, A013ControlNum);
        /// <summary>
        ///  A079入站初始个数
        /// </summary>
        public static int A079UsedNum = 0;
        /// <summary>
        ///  A079入站同时使用超时个数
        /// </summary>
        public static int A079ControlNum = int.Parse(ConfigurationManager.AppSettings["A079ControlNum"] ?? "2");
        /// <summary>
        /// A079入站信号量
        /// </summary>
        public static Semaphore A079ControlSem = new Semaphore(A079ControlNum, A079ControlNum);
        /// <summary>
        ///  A075托杯绑定初始个数
        /// </summary>
        public static int A075UsedNum = 0;
        /// <summary>
        ///  A075托杯绑定同时使用超时个数
        /// </summary>
        public static int A075ControlNum = int.Parse(ConfigurationManager.AppSettings["A075ControlNum"] ?? "2");
        /// <summary>
        /// A075托杯绑定信号量
        /// </summary>
        public static Semaphore A075ControlSem = new Semaphore(A075ControlNum, A075ControlNum);
        /// <summary>
        ///  MES接口超时设定值
        /// </summary>
        public static int MESTimeOut = int.Parse(ConfigurationManager.AppSettings["MESTimeOut"] ?? "800");

        /// <summary>
        /// 设备编码
        /// </summary>
        public static string EqptNo
        {
            get => GetConfigValue(nameof(EqptNo)) ?? "";
          //  set => ChangeConfig(nameof(EqptNo), value);
        }
        /// <summary>
        /// 账户
        /// </summary>
        public static string UserAccount
        {
            get
            {
                return GetConfigValue("UserAccount");
            }
            set
            {
                //ChangeConfig("UserAccount", value);
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get
            {
                return GetConfigValue("Password");
            }
            set
            {
               // ChangeConfig("Password", value);
            }
        }
       
      
     
        /// <summary>
        /// 托杯清洗机
        /// </summary>
        public static string WashMachineNo
        {
            get
            {
                return GetConfigValue("WashMachineNo") ?? "CupWash";
            }
            set
            {
               // ChangeConfig("WashMachineNo", value);
            }
        }
     
        /// <summary>
        /// 用户信息
        /// </summary>
        public static string UserId
        {
            get
            {
                return GetConfigValue("UserId");
            }
            set
            {
                //ChangeConfig("UserId", value);
            }
        }
       
        /// <summary>
        /// PLC所需参数，用于一台上位机拖多个PLC，除制浆外默认都是一台机，
        /// 预热和热压后续可考虑融合在一起，用1拖2
        /// </summary>
        public class PLCParam
        {
            /// <summary>
            /// PLC使用的IP
            /// </summary>
            public string PLCIPAddress
            {
                get; set;
            }
            /// <summary>
            /// PLC端口号
            /// </summary>
            public int PLCPort
            {
                get; set;
            }
            /// <summary>
            /// PLC名字
            /// </summary>
            public string Name
            {
                get; set;
            }
            /// <summary>
            /// 工序号
            /// </summary>
            public string ProcessNo
            {
                get; set;
            }
            /// <summary>
            /// 工位编码
            /// </summary>
            public string StationCode
            {
                get; set;
            }
            /// <summary>
            /// 工序编码
            /// </summary>
            public string ProcessCode
            {
                get; set;
            }
            /// <summary>
            /// 设备编码
            /// </summary>
            public string EqptNo
            {
                get; set;
            }
            /// <summary>
            /// 设备运行状态,默认为1
            /// </summary>
            public int MachineState
            {
                get; set;
            } = 1;
            /// <summary>
            /// 设备运行状态,默认为1
            /// </summary>
            public int OldMachineState
            {
                get; set;
            } = 1;

        }
        public static List<PLCParam> PLCParams { get; set; } = new List<PLCParam>();

        /// <summary>
        /// PLC地址
        /// </summary>
        public static string PLCIPAddress
        {
            get; set;
        }
        /// <summary>
        /// PLC端口
        /// </summary>
        public static int PLCPort
        {
            get; set;
        }

        /// <summary>
        /// plc循环信号执行时间
        /// </summary>
        public static string PLCCycleTime
        {
            get
            {
                return ConfigurationManager.AppSettings["PLCCycleTime"];
            }
        }
        /// <summary>
        
        /// <summary>
        /// 实时数据是否发送给mes
        /// 不存在返回True
        /// </summary>
        //public static string RealTimeMesEnable
        //{
        //    get
        //    {
        //        return GetConfigValueByProcess("RealTimeMesEnable")?.Value ?? "true";
        //    }
        //}

        /// <summary>
        /// 实时数据保存间隔
        /// 不存在则返回60000
        /// </summary>
        public static string RealTimeSaveInternal
        {
            get
            {
                return ConfigurationManager.AppSettings["RealTimeSaveInternal"] ?? "5000";
            }
        }
        /// <summary>
        /// 实时数据发送给mes的时间间隔
        /// 不存在则返回60000
        /// </summary>
        //public static string RealTimeSaveInternalMes
        //{
        //    get
        //    {
        //        return GetConfigValueByProcess("RealTimeSaveInternalMes")?.Value ?? "60000";
        //    }
        //}
        /// <summary>
        /// 实时数据保存路径
        /// </summary>
        public static string RealTimeSavePath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["RealTimeSavePath"] ?? @"D:\RealTimeData";
                }
                catch (Exception ex)
                {
                    return @"D:\RealTimeData";
                }

            }
        }
        /// <summary>
        /// 采集数据命名
        /// </summary>
        public static string RealTimeSaveName = ConfigurationManager.AppSettings["RealTimeSaveName"] ?? "扭矩数据监控";
        /// <summary>
        /// 内部路径
        /// </summary>
        public static string SaveDataPath
        {
            get
            {
                return ConfigurationManager.AppSettings["SaveDataPath"];
            }
        }
        /// <summary>
        /// 外部路径
        /// </summary>
        public static string SaveDataOutPath
        {
            get
            {
                return ConfigurationManager.AppSettings["SaveDataOutPath"] ?? "D:\\MESData";
            }
        }
        /// <summary>
        /// 蓝标数据路径FlawCoordinateFilePath
        /// </summary>
        public static string CsvFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["CsvFilePath"] ?? "D:\\FlawCoordinateFilePath";
            }
        }
      
       
        /// <summary>
        /// 设备型号
        /// </summary>
        public static string MachineModel
        {
            get
            {
                return GetConfigValue("MachineModel");
            }
            set
            {
                //ChangeConfig("MachineModel", value);
            }
        }

        /// <summary>
        /// 一次注液数据库连接字符串
        /// </summary>
        public static string FirstInjectionDbConnectionString
        {
            get
            {
                return GetConfigValue("FirstInjectionDbConnectionString");
            }
            set
            {
                //ChangeConfig("FirstInjectionDbConnectionString", value);
            }
        }
        /// <summary>
        /// 极耳软连接超声波焊接连接字符串
        /// </summary>
        public static string LeadSoftConnectWeldDbConnectionString
        {
            get
            {
                return GetConfigValue("LeadSoftConnectWeldDbConnectionString");
            }
            set
            {
                //ChangeConfig("LeadSoftConnectWeldDbConnectionString", value);
            }
        }
        /// <summary>
        /// 检测配对接连接字符串
        /// </summary>
        public static string DectionPairingDbConnectionString
        {
            get
            {
                return GetConfigValue("DectionPairingDbConnectionString");
            }
            set
            {
                //ChangeConfig("DectionPairingDbConnectionString", value);
            }
        }
        /// <summary>
        /// 浮点型数据间隔
        /// </summary>
        //public static int FloatInternal
        //{
        //    get
        //    {
        //        return int.Parse(GetConfigValueByProcess("FloatInternal")?.Value ?? "50");
        //    }
        //}
        /// <summary>
        /// 字符串型数据间隔
        /// </summary>
        //public static int StringInternal
        //{
        //    get
        //    {
        //        return int.Parse(GetConfigValueByProcess("StringInternal")?.Value ?? "10");
        //    }
        //}
        /// <summary>
        /// 协议编号
        /// </summary>
        public static string ContractNumber
        {
            get; set;
        }
        /// <summary>
        /// Mes过程数据间隔
        /// </summary>
        public static int DevicesProcessMes
        {
            get
            {
                return int.Parse(GetConfigValue("DevicesProcessMes") ?? "60");
            }
        }
        /// <summary>
        /// Mes心跳间隔
        /// </summary>
        public static int HeartBeatMes
        {
            get
            {
                return int.Parse(GetConfigValue("HeartBeatMes") ?? "3");
            }
        }
        /// <summary>
        /// mes是否使用
        /// </summary>
        public static bool MesEnable
        {
            get
            {
                return bool.Parse(GetConfigValue("MesEnable") ?? "true");
            }
        }
        private static int _MesScan;
        /// <summary>
        /// MES 是否在线 -- 10表示MES在线,20表示MES屏蔽
        /// </summary>
        public static int MesScan
        {
            get => _MesScan;
            set
            {
                _MesScan = value;
                mesModeChanged?.Invoke(value);
            }
        }
        private static int _MesCanTub;
        /// <summary>
        /// MES--套膜工艺参数屏蔽信号 -- 0表示不屏蔽,1表示屏蔽
        /// </summary>
        public static int MesCanTub
        {
            get => _MesCanTub;
            set
            {
                _MesCanTub = value;
            }
        }
        private static int _MesOutput;
        /// <summary>
        ///  主轴扭矩采集信号--0表示不需要采集,1表示需要采集
        /// </summary>
        public static int MesOutput
        {
            get => _MesOutput;
            set
            {
                _MesOutput = value;
            }
        }
        private static int _MesIJPmode;
        /// <summary>
        /// 喷码机--喷码模式标识信号 -- 0表示普通单喷码模式,1表示开启双喷码模式(电芯码和日期码)
        /// </summary>
        public static int MesIJPmode
        {
            get => _MesIJPmode;
            set
            {
                _MesIJPmode = value;
            }
        }
        private static int _MesPEXupload;
        /// <summary>
        /// JP40底部穿透--托杯交换机双开模式标识信号 -- 0表示普通单开模式,10表示开启双开模式(C100和JP40)
        /// </summary>
        public static int MesPEXupload
        {
            get => _MesPEXupload;
            set
            {
                _MesPEXupload = value;
            }
        }

        public static bool UploadMachineState
        {
            get
            {
                return bool.Parse(GetConfigValue("UploadMachineState") ?? "true");
            }
        }
        public static bool UploadMachineAlarm
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["UploadMachineAlarm"] ?? "true");
            }
        }
        /// <summary>
        /// 语言
        /// </summary>
        public static string Language
        {
            get
            {
                return GetConfigValue("Language");
            }
            set
            {
               // ChangeConfig("Language", value);
            }
        }
        /// <summary>
        /// 全局线程生命状态
        /// </summary>
        public static bool State { get; set; } = true;
        //private static ObservableCollection<FirstInjectionDataModel> _firstInjectionDataModels = new ObservableCollection<FirstInjectionDataModel>();


        
      
        /// <summary>
        /// 托杯清洗间隔
        /// </summary>
        public static int CupClearCycle
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CupClearCycle"]);
            }
        }
        /// <summary>
        /// 产能数据保存路径
        /// </summary>
        public static string ProductionDataSavePath
        {
            get
            {
                return ConfigurationManager.AppSettings["ProductionDataSavePath"];
            }
        }
       
        /// <summary>
        /// 夜班开始时间
        /// </summary>
        public static DateTime NightShift = Convert.ToDateTime(ConfigurationManager.AppSettings["ShiftNight"] ?? "09:00:00");

        /// <summary>
        /// 白班开始时间
        /// </summary>
        public static DateTime DayShift = Convert.ToDateTime(ConfigurationManager.AppSettings["ShiftDay"] ?? "21:00:00");

        /// <summary>
        /// 版本号下发间隔
        /// </summary>
        public static int VersionCheckCycle
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["VersionCheckCycle"] ?? "86000") * 1000;
            }
        }
        /// <summary>
        /// 下发时间间隔
        /// </summary>
        public static int TimeCheckCycle
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["TimeCheckCycle"] ?? "3600") * 1000;
            }
        }

        /// <summary>
        /// 钢壳供料机压力数据偏移量
        /// </summary>
        public static int CanSupplyIndex
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CanSupplyIndex"] ?? "0");
            }
        }
        /// <summary>
        /// 钢壳供料数据量限制
        /// </summary>
        public static int CansupplyDataLimit
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CansupplyDataLimit"] ?? "0");
            }
        }
        /// <summary>
        /// MES本地校验开关（0-A013；1-本地；2-Web_A013）
        /// </summary>
        public static int MesLocalCheck
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MesLocalCheck"] ?? "0");
            }
        }
        /// <summary>
        /// MES入站A079（0-默认；3-可重复入站;5-用于喷码获取DATECODE用电芯码+月日期码组合）
        /// </summary>
        public static int MesType = int.Parse(ConfigurationManager.AppSettings["MesType"] ?? "0");
        /// <summary>
        /// MES入站A079（true 不解绑； false 解绑）
        /// </summary>
        public static bool A079UnbindFlag = bool.Parse(ConfigurationManager.AppSettings["A079UnbindFlag"] ?? "true");
        /// <summary>
        /// 泰坦URL
        /// </summary>
        public static string TitansURL = ConfigurationManager.AppSettings["TitansURL"];
        /// <summary>
        /// A113上传参数printer-每台机配置不同
        /// </summary>
        public static string A113PRINTER = ConfigurationManager.AppSettings["A113PRINTER"] ?? "C100-02";
        /// <summary>
        /// 辅料Loator上传LoadMode
        /// </summary>
        public static string Locator = ConfigurationManager.AppSettings["Locator"] ?? "";
        /// <summary>
        ///EquipmentID
        /// </summary>
        public static string EquipmentID = ConfigurationManager.AppSettings["EquipmentID"] ?? "KTZP000B";
        /// <summary>
        /// 辅料LoadMode--0 或者空代表:正常上料; 1 代表:预上料; 2 代表:预上料校验
        /// </summary>
        public static string LoadMode = ConfigurationManager.AppSettings["LoadMode"] ?? "0";
        /// <summary>
        /// 辅料OperationType上传
        /// </summary>
        public static string OperationType = ConfigurationManager.AppSettings["OperationType"] ?? "";
        /// <summary>
        /// 辅料下料数量阈值GlobalVariable.FeedingOutTH
        /// </summary>
        public static int FeedingOutTH = int.Parse(ConfigurationManager.AppSettings["FeedingOutTH"] ?? "200");
        /// <summary>
        /// 辅料下料从MES获取系统数量和阈值GlobalVariable.FeedingOutTotal对比
        /// </summary>
        public static int FeedingOutTotal = int.Parse(ConfigurationManager.AppSettings["FeedingOutTotal"] ?? "500");
        /// <summary>
        /// 是否启用刷卡权限单独下发给PLC-- 默认0-表示：不下发； 1-表示：下发
        /// </summary>
        public static int A039LoginEventFlag = int.Parse(ConfigurationManager.AppSettings["A039LoginEventFlag"] ?? "0");
        /// <summary>
        /// 进站是否需要申请月日期码-- 默认0-表示：不需要； 4-表示：需要申请 
        /// </summary>
        public static int A080DateCodeType = int.Parse(ConfigurationManager.AppSettings["A080DateCodeType"] ?? "4");
        /// <summary>
        /// 是否记录debug日志--默认0-表示：记录； 1表示：不记录
        /// </summary>
        public static int IsShowDebugLog = int.Parse(ConfigurationManager.AppSettings["IsShowDebugLog"] ?? "0");
        /// <summary>
        /// 是否记录Mesg日志--默认0-表示：不记录； 1表示：记录
        /// </summary>
        public static int IsShowMesgLog = int.Parse(ConfigurationManager.AppSettings["IsShowMesgLog"] ?? "0");
        /// <summary>
        /// LCC读取超时时间
        /// </summary>
        public static int LCCReadTimeOut = int.Parse(ConfigurationManager.AppSettings["LCCReadTimeOut"] ?? "5000");
        /// <summary>
        /// 设备实时数据采集（0-不启用；1-启用--采集）
        /// </summary>
        public static int UploadRealTimeData = int.Parse(ConfigurationManager.AppSettings["UploadRealTimeData"] ?? "1");
        /// <summary>
        /// 设备产能数据采集（0-不启用；1-启用--采集）
        /// </summary>
        public static int UploadProductionData = int.Parse(ConfigurationManager.AppSettings["UploadProductionData"] ?? "0");
        /// <summary>
        /// QaCheck接口（0-不上传；1-上传）--用于漏液检查机
        /// </summary>
        public static int UpQaCheck = int.Parse(ConfigurationManager.AppSettings["UpQaCheck"] ?? "0");
        /// <summary>
        ///发送NG电芯码给VOG（0-不启用；1-启用发送）
        /// </summary>
        public static int UploadNGdata = int.Parse(ConfigurationManager.AppSettings["UploadNGdata"] ?? "0");
        /// <summary>
        /// Heart（0-不上传；1-上传）--
        /// </summary>
        public static int UpHeart = int.Parse(ConfigurationManager.AppSettings["UpHeart"] ?? "0");
       

        public static int AnodeWeldPressure
        {
            get => int.Parse(ConfigurationManager.AppSettings["AnodeWeldPressure"] ?? "0");
        }
        public static int AnodeWeldPosition
        {
            get => int.Parse(ConfigurationManager.AppSettings["AnodeWeldPosition"] ?? "0");
        }
        /// <summary>
        /// A085接口Code
        /// </summary>
        public static string A085Code
        {
            get
            {
                return ConfigurationManager.AppSettings["A085Code"];
            }
        }
        /// <summary>
        /// A085接口Code
        /// </summary>
        public static string A085Operation
        {
            get
            {
                return ConfigurationManager.AppSettings["A085Operation"];
            }
        }
        /// <summary>
        /// C100pto底部穿透焊-参数名-入壳压力
        /// </summary>
        public static string ParamDesc = ConfigurationManager.AppSettings["ParamDesc"] ?? "入壳压力";
        /// <summary>
        /// C100pto底部穿透焊-参数ID-C06821
        /// </summary>
        public static string ParamID = ConfigurationManager.AppSettings["ParamID"] ?? "C06821";
        /// <summary>
        /// ccd远程文件批量上传处理数量 每次批量处理的图片数量
        /// </summary>
        public static int CCDRemoutPathCount = int.Parse(ConfigurationManager.AppSettings["CCDRemoutPathCount"] ?? "40");
       
        
    }
}

