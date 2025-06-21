using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common
{
    public static class Const
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly string CACHED_SYSPARAM = "CACHED_SYSPARAM";
        public static readonly string CACHED_MES = "CACHED_MES"; // MES接口信息
        public static readonly string CACHED_MES_DTL = "CACHED_MES_DTL"; // MES接口参数信息
        public static readonly string CACHED_PLC = "CACHED_PLC";
        public static readonly string CACHED_PLC_BAKING = "CACHED_PLC_BAKING";
        public static readonly string CACHED_PLC_TRANS = "CACHED_PLC_TRANS";



        public static readonly string CACHED_PLC_SCAN = "CACHED_PLC_SCAN";
        public static readonly string CACHED_PLC_EXCHANGE = "CACHED_PLC_EXCHANGE";
        public static readonly string CACHED_STATION_BATTERY_SCAN = "CACHED_STATION_BATTERY_SCAN";
        public static readonly string CACHED_STATION_TRAY_SCAN = "CACHED_STATION_TRAY_SCAN";
        public static readonly string CACHED_STATION_HANDY_TRAY = "CACHED_STATION_HANDY_TRAY";
        public static readonly string CACHED_STATION_FEEDING = "CACHED_STATION_FEEDING";
        public static readonly string CACHED_STATION_UNLOAD = "CACHED_STATION_UNLOAD";
        public static readonly string CACHED_TRANS_ARM = "CACHED_TRANS_ARM";
        public static readonly string CACHED_STATION_PACK = "CACHED_STATION_PACK";
        public static readonly string CACHED_STATION_UNPACK = "CACHED_STATION_UNPACK";
        public static readonly string CACHED_STATION_SCAN = "CACHED_STATION_SCAN";
        public static readonly string CACHED_STATION_OUT = "CACHED_STATION_OUT";
        public static readonly string CACHED_BAKING_STOVE = "CACHED_BAKING_STOVE";
        public static readonly string CACHED_BIZ_BAKING_STOVE = "CACHED_BIZ_BAKING_STOVE";
        public static readonly string CACHED_STATION_HIPOT = "CACHED_STATION_HIPOT";
        public static readonly string CACHED_BATTERY_CACHE = "CACHED_BATTERY_CACHE";
        public static readonly string CACHED_ALARM_ITEMS = "CACHED_ALARM_ITEMS";
        public static readonly string CACHED_DOOR = "CACHED_DOOR";  // 门
        public static readonly string SERVICE_VERSIONGET = "SERVICE_VERSIONGET";
        public static readonly string CACHED_EXCHANGEIN = "EXCHANGEIN"; // 上料托杯交换
        public static readonly string CACHED_EXCHANGEOUT = "EXCHANGEOUT"; // 下料托杯交换
        public static readonly string CACHED_EXCHANGEBATTERY = "CACHED_EXCHANGEBATTERY"; // 托杯交换电芯数据
        public static readonly string CACHED_UNPACKBETTERY = "CACHED_UNPACKBETTERY"; // 拆盘电芯缓存
        public static readonly string CACHED_CUPCLEAR = "CACHED_CUPCLEAR";//托杯清洗数据
        public static readonly string CACHED_REDIS = "CACHED_REDIS";  //redis

        public static readonly string SYSPARAM_SERVICE = "SYSPARAM_SERVICE";
        public static readonly string PLC_SERVICE = "PLC_SERVICE";
        public static readonly string BATTERY_SCAN_SERVICE = "BATTERY_SCAN_SERVICE";
        public static readonly string TRAY_SCAN_SERVICE = "TRAY_SCAN_SERVICE";
        public static readonly string HAND_TRAY_SERVICE = "HAND_TRAY_SERVICE";
        public static readonly string CACHE_SERVICE = "CACHE_SERVICE";
        public static readonly string CACHE_REDIS = "CACHE_REDIS";
        public static readonly string JOB_SERVICE = "JOB_SERVICE";
        public static readonly string TRANS_ARM_SERVICE = "TRANS_ARM_SERVICE";
        public static readonly string PACK_SERVICE = "PACK_SERVICE";
        public static readonly string SCAN_SERVICE = "SCAN_SERVICE";  // 扫码服务
        public static readonly string BAKING_SERVICE = "BAKING_SERVICE";
        public static readonly string UNPACK_SERVICE = "UNPACK_SERVICE";
        public static readonly string CHECK_SERVICE = "CHECK_SERVICE";
        public static readonly string DOOR_SERVICE = "DOOR_SERVICE";  // 门服务
        public static readonly string UPLOADMES_SERVICE = "UPLOADMES_SERVICE"; // ng表电芯上传MES服务
        public static readonly string EXCHANGE_SERVICE = "EXCHANGE_SERVICE"; // 托杯交换服务
        public static readonly string SERVICE_CALCULATE = "SERVICE_CALCULATE"; // 托杯交换服务

        public static readonly string BATTERY_VIRTUAL_NO = "BATTERY_VIRTUAL_NO";
        public static readonly string BIZ_TYPE_NO_VACUUM = "10607";
        public static readonly string SYS_PARAM_KEY_FOR_BARCODE = "1023";     // 条码长度限制
        public static readonly string SYS_PARAM_KEY_FOR_CHECK_TYPE = "1017";  // 检测方式（1 全检 2 抽检）
        public static readonly string SYS_PARAM_KEY_FOR_BATTERY_COUNT = "1002"; // 托盘电芯数
        public static readonly string SYS_PARAM_KEY_FOR_WATER_STAND = "1003"; // 水含量标准值
        public static readonly string SYS_PARAM_KEY_FOR_OUT_LIMIT = "1011";   // 出库任务限制数
        public static readonly string SYS_PARAM_KEY_FOR_WATER_UPLOAD_TYPE = "1032"; // 水含量上传模式（1自动 2手动）
        public static readonly string SYS_PARAM_KEY_FOR_UNPACK_OUTTIME = "2201";  // 下料超时
        public static readonly string SYS_PARAM_KEY_FOR_OUTREMIND = "2203";  // 下料时限提醒
        public static readonly string SYS_PARAM_KEY_FOR_AUTOBAKING = "1040"; // 入炉后是否自动烘烤 （0自动 1非自动）
        public static readonly string LOG_KEY = "RollingLogFileAppender";
        public static readonly string MES_UserRight = "MES_UserRight";
        public static readonly string MES_TrayCheck = "MES_TrayCheck";
        public static readonly string MES_BatteryInStation = "MES_BatteryInStation";
        public static readonly string MES_BatteryTrayBind = "MES_BatteryTrayBind";
        public static readonly string MES_ProdParaCollect = "MES_ProdParaCollect";
        public static readonly string MES_TrayUnbind = "MES_TrayUnbind";
        public static readonly string MES_EquipmentDataCollect = "MES_EquipmentDataCollect";
        public static readonly string MES_OutStation = "MES_OutStation";
        public static readonly string PRODUCT_TYPE = "STANDARD";
        public static readonly string STOVE_ALARM = "StoveAlarm";
        public static readonly string TRAN_ALARM = "TranAlarm";
        public static readonly string CACHED_STATION_CHECK = "CACHED_STATION_CHECK";
        // 当前用户
        public static readonly string CURRENT_USER = "Current_User";
    }
}
