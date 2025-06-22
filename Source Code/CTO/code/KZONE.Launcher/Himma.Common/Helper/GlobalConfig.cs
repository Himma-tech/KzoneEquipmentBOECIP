using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Himma.Common
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    public static class GlobalConfig
    {
        public static int PLCSleepInterval = int.Parse(ConfigurationManager.AppSettings["PLCSleepInterval"]);
        /// <summary>
        /// 调度PLC刷新间隔
        /// </summary>
        public static int PLCReadInterval = int.Parse(ConfigurationManager.AppSettings["PLCReadInterval"]);
        /// <summary>
        /// 组盘前扫码信号刷新间隔
        /// </summary>
        public static int PLCReadScanInterval = int.Parse(ConfigurationManager.AppSettings["PLCReadScanInterval"] ?? "200");
        public static bool IsEnabledPLC = bool.Parse(ConfigurationManager.AppSettings["IsEnabledPLC"]);
        public static int JobExecuteInterval = int.Parse(ConfigurationManager.AppSettings["JobExecuteInterval"]);
        public static int TaskSerchInterval = int.Parse(ConfigurationManager.AppSettings["TaskSerchInterval"]);
        public static int RedisSaveInterval = int.Parse(ConfigurationManager.AppSettings["RedisSaveInterval"]);
        public static int DoorTaskSerchInterval = int.Parse(ConfigurationManager.AppSettings["DoorTaskSerchInterval"]);
        public static int BakingTaskSerchInterval = int.Parse(ConfigurationManager.AppSettings["BakingTaskSerchInterval"]);  // baking任务搜索间隔
        public static int TransTaskSearchInterval = int.Parse(ConfigurationManager.AppSettings["TransTaskSearchInterval"]);  // 输送任务搜索间隔
        public static int HeartToPlcInterval = int.Parse(ConfigurationManager.AppSettings["HeartToPlcInterval"]); // 给PLC写心跳的时间间隔：毫秒
        // 指令下发失败，重新下发间隔
        public static int ExceptionTryInterval = int.Parse(ConfigurationManager.AppSettings["ExceptionTryInterval"]);
        // 步骤执行异常，重试最大次数
        public static int ErrorRetryMaxCount = int.Parse(ConfigurationManager.AppSettings["ErrorRetryMaxCount"]);
        // 温度数据外部存储路径（公开）
        public static string RunningDataSavePath = ConfigurationManager.AppSettings["RunningDataSavePath"];
        // 温度数据内部存储路径（不公开）
        public static string BakingTempFilePath = ConfigurationManager.AppSettings["BakingTempFilePath"];
        // 温度数据实际获取路径（实际使用）
        public static string BakingDataGetPath = ConfigurationManager.AppSettings["BakingDataGetPath"];
        public static bool CallProcJob = false;
        // baking数据保存周期
        public static int BakingDataCollectInterval = int.Parse(ConfigurationManager.AppSettings["BakingDataCollectInterval"]);
        // Mes超时上传间隔
        public static int MesOutTimeUpLoadInterval = int.Parse(ConfigurationManager.AppSettings["MesOutTimeUpLoadInterval"]);
        //是否记录debug日志
        public static int IsShowDebugLog = int.Parse(ConfigurationManager.AppSettings["IsShowDebugLog"]);
        /// <summary>
        /// 触发读取通道数
        /// </summary>
        public static int ReadVarChangeNum = int.Parse(ConfigurationManager.AppSettings["ReadVarChangeNum"] ?? "25");
        /// <summary>
        /// 打印机名称
        /// </summary>
        public static string PrinterName = ConfigurationManager.AppSettings["PrinterName"];
        /// <summary>
        /// 打印机模板路径
        /// </summary>
        public static string PrinteModePath = ConfigurationManager.AppSettings["PrinteModePath"];

        /// <summary>
        /// 是否启用键鼠互锁
        /// </summary>
        public static bool KeyBoardIsUsed = ConfigurationManager.AppSettings["KeyBoardIsUsed"] == "1";
        /// <summary>
        /// 键鼠驱动路径
        /// </summary>
        public static string KeyBoardPath = ConfigurationManager.AppSettings["KeyBoardPath"];
        /// <summary>
        /// 是否启用公里数统计
        /// </summary>
        public static bool RGVPositonIsUsed = ConfigurationManager.AppSettings["RGVPositonIsUsed"] == "1";
        /// <summary>
        /// MES版本信息
        /// </summary>
        public static string MESVerison = ConfigurationManager.AppSettings["MESVerison"];
        // 任务请求间隔
        public static int RequestDownInterval = int.Parse(ConfigurationManager.AppSettings["RequestDownInterval"]);
        /// <summary>
        /// 标签打印纸单卷总数量
        /// </summary>
        public static int PrintPaperMaxCout = 1000;
        /// <summary>
        /// 标签打印纸当前消耗数量
        /// </summary>
        public static int PrintPaperUsedCout = 0;
        /// <summary>
        /// 标签打印纸报警预警数量
        /// </summary>
        public static int PrintPaperAlarmCout = 50;
        /// <summary>
        /// 下料一次上传多少电芯数据
        /// </summary>
        public static int UploadBatteryCount = 20;
        /// <summary>
        /// 版本号更新间隔
        /// </summary>
        public static int VersionUpdateInterval = int.Parse(ConfigurationManager.AppSettings["VersionUpdateInterval"]);

        public static int PackPlcKeepConnectInterval = int.Parse(ConfigurationManager.AppSettings["PackPlcKeepConnectInterval"] ?? "60000");
        public static string PackPlcKeepConnectValue = ConfigurationManager.AppSettings["PackPlcKeepConnectValue"] ?? "stMesProc.ar_iScanCheckDown_01[9]";
        public static int IntervalAddTime = int.Parse(ConfigurationManager.AppSettings["IntervalAddTime"] ?? "80");
        /// <summary>
        /// MES返回重码标记
        /// </summary>
        public static string MuTiCodingConfig = ConfigurationManager.AppSettings["MuTiCodingConfig"] ?? "有入站但是与电芯当前工序;还未出站;工序跳站";

        public static string A085Code = ConfigurationManager.AppSettings["A085Code"];

        public static string A085CodeWaterCheck = ConfigurationManager.AppSettings["A085CodeWaterCheck"];


        public static string A085Operation = ConfigurationManager.AppSettings["A085Operation"];


        /// <summary>
        /// 产能数据保存间隔
        /// </summary>
        public static int YeildMonitorIntval = int.Parse(ConfigurationManager.AppSettings["YeildMonitorIntval"] ?? "60000");

        /// <summary>
        /// 晚班开始时间
        /// </summary>
        public static DateTime NightShift = Convert.ToDateTime(ConfigurationManager.AppSettings["NightShift"] ?? "20:00:00");
        /// <summary>
        /// 白班开始时间
        /// </summary>
        public static DateTime DayShift = Convert.ToDateTime(ConfigurationManager.AppSettings["DayShift"] ?? "08:00:00");

        // 来料扫码间隔打印日志
        public static int ScanReadInterval = int.Parse(ConfigurationManager.AppSettings["ScanReadInterval"] ?? "100");
        // 写心跳超时间隔打印日志
        public static int WriteHeartInterval = int.Parse(ConfigurationManager.AppSettings["WriteHeartInterval"] ?? "500");

        public static int CalculateTaskSerchInterval = int.Parse(ConfigurationManager.AppSettings["CalculateTaskSerchInterval"] ?? "0");
        public static string BkCalculateWaterDataPath = ConfigurationManager.AppSettings["BkCalculateWaterDataPath"];
        public static string CalculateWaterDataPath = ConfigurationManager.AppSettings["CalculateWaterDataPath"];
        public static string PressureDataPath = ConfigurationManager.AppSettings["PressureDataPath"];
        public static string BasWaterDataPath_1 = ConfigurationManager.AppSettings["BasWaterDataPath_1"];
        public static string BasWaterDataPath = ConfigurationManager.AppSettings["BasWaterDataPath"];
        //监控水含量脚本外部进程
        public static string WarterCheck_ProcessIsOn = ConfigurationManager.AppSettings["WarterCheck_ProcessIsOn"];
        public static string ProcessName_WaterCheck = ConfigurationManager.AppSettings["ProcessName_WaterCheck"];
        public static string BatName_WaterCheck = ConfigurationManager.AppSettings["BatName_WaterCheck"];
        public static string ProcessDirectory_WaterCheck = ConfigurationManager.AppSettings["ProcessDirectory_WaterCheck"];
        public static string ProcessDirectory_HCS = ConfigurationManager.AppSettings["ProcessDirectory_HCS"];
        public static int MornitorProcessInterval = int.Parse(ConfigurationManager.AppSettings["MornitorProcessInterval"] ?? "60000");


        // 报警刷新线程屏蔽写1，烘烤文件写入scv线程屏蔽写2，都屏蔽写99；不屏蔽线程写0；
        public static int IsCloseThread = int.Parse(ConfigurationManager.AppSettings["IsCloseThread"] ?? "99");

        public static int StoveBakingTimeLimit = int.Parse(ConfigurationManager.AppSettings["StoveBakingTimeLimit"] ?? "9600");

        // 配置同时处理线程数
        public static int RunThreadNum = int.Parse(ConfigurationManager.AppSettings["RunThreadNum"] ?? "512");
        // 是否启用前后托杯交换批量读取功能，0：不启用；非0: 启用；
        public static int IsUsedReadArray = int.Parse(ConfigurationManager.AppSettings["IsUsedReadArray"] ?? "1");
        /// <summary>
        /// Redis服务器127.0.0.1:6379 
        /// </summary>
        public static string RedisIP = ConfigurationManager.AppSettings["RedisIP"] ?? "127.0.0.1:6379";
        /// <summary>
        ///Redis池大小 默认 10
        /// </summary>
        public static int RedisPoolSize = int.Parse(ConfigurationManager.AppSettings["RedisPoolSize"] ?? "5");

        // 是否启用redis队列异步 0：不启用；非0: 启用；
        public static int IsRedisArrayAsync = int.Parse(ConfigurationManager.AppSettings["IsRedisArrayAsync"] ?? "1");

        // LCC读取超时时间
        public static int LCCReadTimeOut = int.Parse(ConfigurationManager.AppSettings["LCCReadTimeOut"] ?? "5000");
        //GetProcessParameters接口类型
        public static string ProcessType = ConfigurationManager.AppSettings["ProcessType"] ?? "CELL";
        //GetProcessParameters接口操作
        public static string ProcessOperation = ConfigurationManager.AppSettings["ProcessOperation"] ?? "P00284";

        /// <summary>
        /// 是否启用BeginInvoke 0：默认启用；1：部分同步；99：全部为同步
        /// </summary>
        public static int IsBeginInvokeFlag = int.Parse(ConfigurationManager.AppSettings["IsBeginInvokeFlag"] ?? "1");

        //数据库参数表刷新间隔
        public static int SysParamInterval = int.Parse(ConfigurationManager.AppSettings["SysParamInterval" ?? "1"] ?? "100");
        /// <summary>
        /// 托杯交换PLC信号刷新间隔
        /// </summary>
        public static int ExchangePLCReadInterval = int.Parse(ConfigurationManager.AppSettings["ExchangePLCReadInterval"] ?? "150");
        /// <summary>
        /// 烘烤PLC 信号刷新间隔
        /// </summary>
        public static int BakingPLCReadInterval = int.Parse(ConfigurationManager.AppSettings["BakingPLCReadInterval"] ?? "200");
        /// <summary>
        /// 是否启用 通讯连接池 1:启用；非1禁用
        /// </summary>
        public static int IsUsedPool = int.Parse(ConfigurationManager.AppSettings["IsUsedPool"] ?? "1");
        /// <summary>
        /// 适用机型 1:C100；2:JP30
        /// </summary>
        public static int BakingModel = int.Parse(ConfigurationManager.AppSettings["BakingModel"] ?? "1");
        /// <summary>
        /// 是否启用任务链调度模式  1:启用  2：不启用
        /// </summary>
        public static int IsUseTaskChain = int.Parse(ConfigurationManager.AppSettings["IsUseTaskChain"] ?? "1");

        /// <summary>
        /// 是否启用 耗时日志保存CSV 1:启用； 非1禁用
        /// </summary>
        public static int IsUsedSaveCSV = int.Parse(ConfigurationManager.AppSettings["IsUsedSaveCSV"] ?? "1");

        /// <summary>
        /// 数据库连接池最大数
        /// </summary>
        public static int MaxPoolSize = int.Parse(ConfigurationManager.AppSettings["MaxPoolSize"] ?? "30");

        /// <summary>
        /// 数据库连接池最小数
        /// </summary>
        public static int MinPoolSize = int.Parse(ConfigurationManager.AppSettings["MinPoolSize"] ?? "30");
        /// <summary>
        /// 上料机器人IP地址
        /// </summary>
        public static string RobotIp1 = ConfigurationManager.AppSettings["RobotIp1"] ?? "192.168.252.40";
        /// <summary>
        /// 下料机器人地址
        /// </summary>
        public static string RobotIp2 = ConfigurationManager.AppSettings["RobotIp2"] ?? "192.168.252.41";
        /// <summary>
        /// Log压缩备份多少天之前的可配置-默认30
        /// </summary>
        public static int LogBackupsMaxDay = int.Parse(ConfigurationManager.AppSettings["LogBackupsMaxDay"] ?? "30");

        /// <summary>
        /// Log压缩备份-0直接压缩备份；1-交接班时间进行
        /// </summary>
        public static int IsUseLogBackups = int.Parse(ConfigurationManager.AppSettings["IsUseLogBackups"] ?? "1");
        /// <summary>
        ///  Log压缩备份路径
        /// </summary>
        public static string LogBackupsPath = ConfigurationManager.AppSettings["LogBackupsPath"] ?? "E:\\LogBackups";
        /// <summary>
        /// MES入站A079（0-默认；3-可重复入站）
        /// </summary>
        public static int MesType = int.Parse(ConfigurationManager.AppSettings["MesType"] ?? "0");
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
        /// 入站接口校验超时时间
        /// </summary>
        public static int BatteryInTimeOut = int.Parse(ConfigurationManager.AppSettings["BatteryInTimeOut"] ?? "500");
        /// <summary>
        /// 出站接口校验超时时间
        /// </summary>
        public static int BatteryOutTimeOut = int.Parse(ConfigurationManager.AppSettings["BatteryOutTimeOut"] ?? "1000");
        /// <summary>
        /// 绑定接口校验超时时间
        /// </summary>
        public static int BatteryBindingTimeOut = int.Parse(ConfigurationManager.AppSettings["BatteryBindingTimeOut"] ?? "500");
        /// <summary>
        /// 线程阈值设置；
        /// </summary>
        public static int ThreadNum = int.Parse(ConfigurationManager.AppSettings["ThreadNum"] ?? "180");
    }
}
