using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    public static class SysParamConfig
    {
        /// <summary>
        /// 托盘电芯数量 1002
        /// </summary>
        public static string TrayBatteryNum = "1002";

        /// <summary>
        /// 水含量检测标准设置 1003
        /// </summary>
        public static string WaterCheckStand = "1003";

        /// <summary>
        /// 自动/手动(1:自动;0:手动;);系统 1006
        /// </summary>
        public static string DispothIsAuto = "1006";

        /// <summary>
        /// 入库控制【1:启用；0：禁止】 1007
        /// </summary>
        public static string IsAllowIn = "1007";

        /// <summary>
        /// 出库控制【1:启用；0：禁止】 1008
        /// </summary>
        public static string IsAllowOut = "1008";

        /// <summary>
        /// 系统托盘数上限 1009
        /// </summary>
        public static string TrayMaxCapacity = "1009";

        /// <summary>
        /// 是否启用MES【1:启用；0：禁止】 1011
        /// </summary>
        public static string IsMesOnline = "1011";

        /// <summary>
        /// 电芯条码长度(Guid) 1012
        /// </summary>
        public static string GuidLength = "1012";

        /// <summary>
        /// 托盘码长度 1013
        /// </summary>
        public static string TrayNoLength = "1013";

        /// <summary>
        /// 清料模式【1:启用；0：禁止】 1030
        /// </summary>
        public static string DevClearMode = "1030";

        /// <summary>
        /// 紧托杯条码长度限制(J_Rfid) 1031
        /// </summary>
        public static string JCupLength = "1031";

        /// <summary>
        /// 松托杯条码长度限制(S_Rfid) 1032
        /// </summary>
        public static string SCupLength = "1032";
        /// <summary>
        /// 主副温度探头个数  1033
        /// </summary>
        public static string TempCount = "1033";
        /// <summary>
        /// 单次夹取电芯数量  1035
        /// </summary>
        public static string SingleGetCount = "1035";
        /// <summary>
        /// 两个调度机器人的安全距离  1036
        /// </summary>
        public static string SafeDistance = "1036";
        /// <summary>
        /// 是否开启本地A013出站校验【0：A013;1: A013_local;2:A013_web】  1037
        /// </summary>
        public static string A013_localSwitch = "1037";
        /// <summary>
        /// 判断高温NG最少点数  1108
        /// </summary>
        public static string MaxTempCountLimit = "1108";
        /// <summary>
        /// 判断低温NG最少点数  1109
        /// </summary>
        public static string MinTempCountLimit = "1109";
        /// <summary>
        /// 异常温度数据过滤值下限  1110
        /// </summary>
        public static string errorTemp = "1110";

        /// <summary>
        /// 标签打印纸单卷总数量  1111
        /// </summary>
        public static string PrtPaperMaxCout = "1111";
        /// <summary>
        /// 标签打印纸当前消耗数量  1112
        /// </summary>
        public static string PrtPaperUsedCout = "1112";
        /// <summary>
        /// 标签打印纸报警预警数量  1113
        /// </summary>
        public static string PrtPaperAlarmCout = "1113";
        /// <summary>
        /// 超温温度  1115
        /// </summary>
        public static string UpTempLimit = "1115";
        /// <summary>
        /// 低温温度  1116
        /// </summary>
        public static string LowTempLimt = "1116";
        /// <summary>
        /// 烘烤多久之后开始计算温度  1117
        /// </summary>
        public static string timeSpanByTimeLimit = "1117";
        /// <summary>
        /// 正常温度点数限制  1118
        /// </summary>
        public static string NormalTempCountLimit = "1118";
        /// <summary>
        ///  调机模式【1:启用；0：禁止】 1034
        /// </summary>
        public static string IsDevTestMode = "1034";

        /// <summary>
        /// A上键盘实例路径 3001
        /// </summary>
        public static string AUpBoard = "3001";
        /// <summary>
        /// A上鼠标实例路径 3002
        /// </summary>
        public static string AUpKey = "3002";
        /// <summary>
        /// A下键盘实例路径 3003
        /// </summary>
        public static string ADownBoard = "3003";
        /// <summary>
        /// A下鼠标实例路径 3004
        /// </summary>
        public static string ADownKey = "3004";
        /// <summary>
        /// B上键盘实例路径 3005
        /// </summary>
        public static string BUpBoard = "3005";
        /// <summary>
        /// B上鼠标实例路径 3006
        /// </summary>
        public static string BUpKey = "3006";
        /// <summary>
        /// B下键盘实例路径 3007
        /// </summary>
        public static string BDownBoard = "3007";
        /// <summary>
        /// B下鼠标实例路径 3008
        /// </summary>
        public static string BDownKey = "3008";

        /// <summary>
        /// 上次记录的水含量夹具码 4001
        /// </summary>
        public static string LastWaterTrayNo = "4001";

        /// <summary>
        /// 上料调度运行里程数【单位：KM】 4002
        /// </summary>
        public static string RGVRunDis_Feeding = "4002";

        /// <summary>
        /// 上料调度运行里程数【单位：KM】 4003
        /// </summary>
        public static string RGVRunDis_Unload = "4003";
        /// <summary>
        /// 是否启用线程信息记录 0：默认启用；非0不启用
        /// </summary>
        public static string UsedThreadFlag = "4008";

        public static string NeedToWaterCheckCount = "4009";
        /// <summary>
        /// 适用机型 1:C100；2:JP30
        /// </summary>
        public static string BakingModel = "4200";

    }
}
