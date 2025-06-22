using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Enums
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    /// <summary>
    /// 编码
    /// </summary>
    public enum EnumEncoding
    {
        ASCII,
        UTF8,
        GB2312,
        DEFAULT,
        HEX
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        [Description("普通日志")]
        Normal = 1,
        [Description("错误日志")]
        Error = 2,
        [Description("报警日志")]
        Warning = 3,
        [Description("调用MES接口日志")]
        MesInterface = 4,
        [Description("调用存储过程日志")]
        Proc = 5,
        [Description("PLC交互")]
        PLC = 6,
        [Description("PLC交互错误日志")]
        PLCError = 12,
        [Description("Baking炉交互")]
        Baking = 7,
        [Description("界面监控")]
        Monitor = 8,
        [Description("操作数据库日志")]
        Sql = 9,
        [Description("系统日志")]
        System = 10,
        [Description("扫码枪")]
        Scanner = 11,
        [Description("PLC读取")]
        PLCRead = 13,
        [Description("PLC数据跳变")]
        PLCDataChange = 14,
        [Description("向PLC写入数据")]
        WriteToPLC = 15
    }

    /// <summary>
    /// 提示框信息类型
    /// </summary>
    public enum TextMessageType
    {
        Info = 1,
        Warn = 2,
        Error = 3
    }

    public enum SystemNoticeType
    {
        ShowTextMessage = 0,
        ForceExitApplication = 255
    }

    /// <summary>
    /// 可用状态
    /// 0-不可用
    /// 1-可用
    /// </summary>
    public enum UseableStatus
    {
        Disabled = 0,
        Enabled = 1
    }

    /// <summary>
    /// 连接信号
    /// 0-断开
    /// 1-弱
    /// 2-中等
    /// 3-强
    /// </summary>
    public enum SignalStatus
    {
        Disconnected = 0,
        Low = 1,
        Medium = 2,
        High = 3,
    }

    /// <summary>
    /// 到位信号
    /// </summary>
    public enum ArriveFlag
    {
        Default = 0,  //默认
        Arrive = 1  //到位
    }

    public enum VacuumStatus
    {
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 抽真空中
        /// </summary>
        [Description("抽真空中")]
        Vacuuming = 1,
        /// <summary>
        /// 抽真空完成
        /// </summary>
        [Description("抽真空中")]
        Vacuumed = 2,
        /// <summary>
        /// 破真空中
        /// </summary>
        [Description("破真空中")]
        UnVacuuming = 3,
        /// <summary>
        /// 破真空中
        /// </summary>
        [Description("破真空完成")]
        UnVacuumed = 4,
        /// <summary>
        /// 保压中
        /// </summary>
        [Description("保压中")]
        Keeping = 5,
        /// <summary>
        /// 空闲
        /// </summary>
        [Description("空闲")]
        Free = 9
    }
    public enum VacuumReq
    {
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 常压下抽真空请求
        /// </summary>
        [Description("常压下抽真空请求")]
        AStatusVacuum = 1,
        /// <summary>
        /// 保压下抽真空请求
        /// </summary>
        [Description("保压下抽真空请求")]
        BStatusVacuum = 2,
    }

    public enum HotingStatus
    {
        /// <summary>
        /// 未加热
        /// </summary>
        [Description("未加热")]
        Default = 0,
        /// <summary>
        /// 加热中
        /// </summary>
        [Description("加热中")]
        Hoting = 1,
    }

    public enum AlarmStatus
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 报警中
        /// </summary>
        [Description("报警")]
        Alarming = 1,
    }

    public enum OrderStatus
    {
        [Description("空闲")]
        Default = 0,
        [Description("组盘中")]
        Pack = 1001,
        [Description("换盘中")]
        ChangeTray = 1005,
        [Description("异常出库")]
        AbnormalOutBound = 1006,
        [Description("入库完成")]
        InBound = 2000,
        [Description("待烘烤")]
        WaitBaking1 = 2001,
        [Description("待烘烤")]
        WaitBaking2 = 3001,
        [Description("待烘烤")]
        WaitBaking3 = 3010,
        [Description("待烘烤")]
        WaitBaking4 = 3011,
        [Description("烘烤中")]
        Baking = 3002,
        [Description("烘烤异常")]
        AbnormalBaking = 3004,
        [Description("烘烤结束")]
        EndBaking1 = 3003,
        [Description("烘烤结束")]
        EndBaking2 = 3005,
        [Description("待破真空")]
        Wait = 3006,
        [Description("待检测")]
        WaitCheck = 4001,
        [Description("检测待收数")]
        WaitCal = 4010,
        [Description("水含量检测中")]
        Calculating = 4011,
        [Description("待出库")]
        WaitOutBound = 5001,
        [Description("出库中")]
        OutBounding = 5002,
    }

    /// <summary>
    /// 工位有载无载状态
    /// </summary>
    public enum LoadStatus
    {
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 空载
        /// </summary>
        [Description("空载")]
        Empty = 1,
        /// <summary>
        /// 有载
        /// </summary>
        [Description("有载")]
        Loaded = 2
    }

    /// <summary>
    /// 工位类型
    /// </summary>
    public enum LocType
    {
        [Description("上料缓存")]
        PackCache = 105,
        [Description("下料缓存")]
        UnPackCache = 107,
        [Description("回流通道")]
        BackCache = 113
    }

    /// <summary>
    /// MES接口
    /// 1-电芯出站与数据采集
    /// </summary>
    public enum MESInterface
    {
        miBatchdataCollectForSfc = 1
    }

    public enum TransArmRunningStatus
    {
        [Description("默认")]
        Default = 1,
        [Description("行走中")]
        Walking = 2,
        [Description("取货中")]
        Getting = 3,
        [Description("放货中")]
        Putting = 4
    }

    /// <summary>
    /// PLC类别
    /// 1-bakingPLC
    /// 2-输送PLC
    /// </summary>
    public enum PLCType
    {
        PLC_BAKNG = 1,
        PLC_TRANS = 2,
        PLC_ROBOT = 3,
        PLC_CHANGE = 4
    }

    /// <summary>
    /// 设备类型
    /// 10-输送线大机械手
    /// 20-上料机械手
    /// 30-下料机械手
    /// 40-炉腔
    /// </summary>
    public enum DevType
    {
        PackArm = 20,
        UnPackArm = 30,
        BakingBin = 70,
        TransArm = 80,
    }

    /// <summary>
    /// 电芯类别
    /// 1-正常电芯
    /// 2-假电芯
    /// </summary>
    public enum BatteryType
    {
        Default = 0,
        Normal = 1,
        Fake = 2
    }

    /// <summary>
    /// Baking曲线类别
    /// </summary>
    public enum BakingChartType
    {
        [Description("当前腔体真空值")]
        Vacuum = 0,
        [Description("控制温度值")]
        ControlTemp = 2,
        [Description("巡检温度值")]
        PatrolTemp = 3,
        [Description("报警温度值")]
        AlarmTemp = 4,
    }

    /// <summary>
    /// 系统参数
    /// </summary>
    public enum SysParams
    {
        Tray_Count = 1001,                   //腔体托盘数
        Tray_Battery_Qty = 1002,             //托盘电芯数量
        Check_Stand = 1003,                  //检测标准设置
        Water_Channel_Offset = 1004,         //水含量电芯偏移量
        Water_Channel_Id = 1005,             //水含量电芯通道号
        PPM = 1006,                          //产能设置PPM
        Scan_IntervalTime = 1007,            //扫码工位扫描间隔（秒）
        Pack_IntervalTime = 1008,            //上料时间间隔（秒）
        Default_Baking_IntervalTime = 1009,  //默认烘烤时间(秒）5小时
        Default_Check_CycleTime = 1010,      //默认检测周期(秒) 15分钟
        Analysis_CycleTime = 1011,           //统计分析周期(秒）
        Together_PackQty = 1012,             //同时执行的上料任务数
        PackStartTime_IntervalTime = 1013,   //上料任务启动间隔(电芯数)
        Sampling_Ratio = 1014,               //抽检比例(SAMPLING RATIO)%
        Execute_Model = 1015                 //自动/手动(1:自动;0:手动;)
    }

    /// <summary>
    /// 指令下发状态
    /// </summary>
    public enum CMDToPlc
    {
        [Description("空闲")]
        Free = 0,
        [Description("指令下发")]
        Download = 1,
        [Description("确认完成")]
        Confirm = 2,
        [Description("强制完成")]
        ForcedFinish = 3,
        [Description("确认清除")]
        ConfirmClear = 4
    }

    public enum BatteryCheckStatus
    {
        Default = 0,
        OK = 1,
        NG = 2
    }

    /// <summary>
    /// PLC运行状态
    /// 0-停止
    /// 1-运行
    /// </summary>
    public enum RunStatus
    {
        Running = 1,
        Stopped = 0
    }

    /// <summary>
    /// 设备运行状态
    /// 0-默认
    /// 1-运行
    /// 2-停机
    /// </summary>
    public enum DevStatus
    {
        [Description("默认")]
        Default = 0,
        [Description("运行")]
        Running = 1,
        [Description("停止")]
        Stopped = 2
    }

    /// <summary>
    /// 环境状态
    /// 0-默认
    /// 1-运行
    /// 2-停机
    /// </summary>
    public enum EnverimentStatus
    {
        [Description("默认")]
        Default = 0,
        [Description("正常")]
        OK = 1,
        [Description("异常")]
        NG = 2
    }

    /// <summary>
    /// 报警信息有无
    /// 调度PLC
    /// </summary>
    public enum ExistsAlarms
    {
        [Description("默认")]
        Default = 0,
        [Description("有报警")]
        Exists = 1,
        [Description("无报警")]
        None = 2
    }

    /// <summary>
    /// PLC状态
    /// </summary>
    public enum PlcStatus
    {
        [Description("默认")]
        Default = 0,
        [Description("停机")]
        Stopped = 1,
        [Description("正常")]
        Running = 2,
        [Description("待机")]
        Paused = 3,
        [Description("故障")]
        Fault = 4
    }

    /// <summary>
    /// 结束状态
    /// 1:正常结束；2:强制结束；4:强制取消 3:异常结束
    /// </summary>
    public enum FinishStatus
    {
        Normal = 1,
        Force = 2,
        ForceCancel = 4,
        AbNormal = 3
    }

    /// <summary>
    /// 烘烤任务结束状态
    /// </summary>
    public enum BakingFinishStatus
    {
        [Description("正常结束")]
        Normal = 1,
        [Description("异常结束")]
        Error = 2
    }

    /// <summary>
    /// 炉腔报警状态
    /// 炉腔PLC
    /// </summary>
    public enum ExistAlarms
    {
        [Description("无报警")]
        Default = 0,
        [Description("有报警")]
        Exist = 1
    }

    /// <summary>
    /// 任务步骤
    /// </summary>
    public enum TaskStep
    {
        [Description("未执行")]
        Default = 0,
        [Description("执行中")]
        Executing = 1,
        [Description("执行完毕")]
        Completed = 2,
        [Description("已下发")]
        Downloaded = 3
    }

    /// <summary>
    /// Omron类型
    /// </summary>
    public enum OmronType
    {
        [Description("NXCompolet")]
        NX = 1,
        [Description("NJCompolet")]
        NJ = 2
    }

    /// <summary>
    /// PLC类型
    /// </summary>
    public enum LCCType
    {
        [Description("Default")]
        Default = 0,
        [Description("IsLCC")]
        IsLCC = 1,
        [Description("NotIsLCC")]
        NotIsLCC = 2
    }




    /// <summary>
    /// 自动手动状态
    /// </summary>
    public enum AutoStatus
    {
        [Description("默认")]
        Default = 0,
        [Description("手动")]
        Manual = 1,
        [Description("自动")]
        Auto = 2
    }
    /// <summary>
    /// 联机/脱机
    /// </summary>
    public enum OnLineStatus
    {
        [Description("默认")]
        Default = 0,
        [Description("联机")]
        OnLine = 1,
        [Description("脱机")]
        OffLine = 2
    }
    /// <summary>
    /// 报警等级
    /// </summary>
    public enum AlarmLevel
    {
        [Description("黄色报警")]
        Yellow = 1,
        [Description("橙色报警")]
        Oranger = 2,
        [Description("红色报警")]
        Red = 3
    }

    /// <summary>
    /// 检测模式
    /// </summary>
    public enum CheckType
    {
        [Description("全检")]
        AllCheck = 1,
        [Description("抽检")]
        SpotCheck = 2
    }

    public enum ScanRead
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 读取
        /// </summary>
        [Description("读取")]
        Read = 1
    }

    public enum ScanOurReadPress
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 出料数据OK读取
        /// </summary>
        [Description("读取OK")]
        ReadOK = 1,
        /// <summary>
        /// 出料数据NG读取
        /// </summary>
        [Description("压力NG")]
        PressNG = 2,
        /// <summary>
        /// 出料数据NG读取
        /// </summary>
        [Description("客户MES_NG")]
        CustemMesNG = 3,
        /// <summary>
        /// 出料数据NG读取
        /// </summary>
        [Description("其他NG")]
        OthersNG = 4
    }

    public enum KeyBoardType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// A上
        /// </summary>
        [Description("A上")]
        A_Up = 1,
        /// <summary>
        /// A下
        /// </summary>
        [Description("A下")]
        A_Down = 2,
        /// <summary>
        /// B上
        /// </summary>
        [Description("B上")]
        B_Up = 3,
        /// <summary>
        /// B下
        /// </summary>
        [Description("B下")]
        B_Down = 4
    }

    /// <summary>
    /// 电芯托杯类型
    /// </summary>
    public enum BatteryGuidType
    {
        /// <summary>
        /// 电芯码GUID
        /// </summary>
        [Description("电芯码GUID")]
        BatteryNo = 0,
        /// <summary>
        /// 紧托杯
        /// </summary>
        [Description("紧托杯")]
        JCup = 1,
        /// <summary>
        /// 松托杯
        /// </summary>
        [Description("松托杯")]
        SCup = 2
    }

    public enum CommunicType
    {
        /// <summary>
        /// 倍福
        /// </summary>
        [Description("倍福")]
        BeckhoffAds6 = 0,
        /// <summary>
        /// 欧姆龙
        /// </summary>
        [Description("欧姆龙")]
        OmronCip = 1
    }
    public enum PuckExchangeType
    {
        /// <summary>
        /// 电芯入站
        /// </summary>
        [Description("电芯入站")]
        BatteryIn = 0,
        /// <summary>
        /// 电芯出站
        /// </summary>
        [Description("电芯出站")]
        BatteryOut = 1,
        /// <summary>
        /// 上料托杯交换
        /// </summary>
        [Description("上料托杯交换")]
        ExchangeForIn = 2,
        /// <summary>
        /// 下料托杯交换
        /// </summary>
        [Description("下料托杯交换")]
        ExchangeForOut = 3,
        /// <summary>
        /// 空托杯校验
        /// </summary>
        [Description("空托杯校验")]
        CupIn = 4
    }
    /// <summary>
    ///  Reids电芯信息存储时间节点
    /// </summary>
    public enum RedisSaveType
    {
        /// <summary>
        /// 电芯入站
        /// </summary>
        [Description("前托杯交换")]
        BeforePuckExchange = 0,
        /// <summary>
        /// 组盘电芯扫码
        /// </summary>
        [Description("电芯扫码")]
        Scan = 1,
        /// <summary>
        /// 组盘
        /// </summary>
        [Description("组盘")]
        Pack = 2,
        /// <summary>
        /// 入炉
        /// </summary>
        [Description("入炉")]
        InStove = 3,
        /// <summary>
        /// 出炉
        /// </summary>
        [Description("出炉")]
        OutStove = 4,
        /// <summary>
        /// 拆盘
        /// </summary>
        [Description("拆盘")]
        Unpack = 5,
        /// <summary>
        /// 后托杯交换
        /// </summary>
        [Description("后托杯交换")]
        AfterPuckExchange = 6,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")]
        Finish = 7

    }
    /// <summary>
    /// 项目模式
    /// </summary>
    public enum BakingModelEnum
    {
        C100 = 1,
        JP30 = 2
    }




    public class GetEnumInfo
    {
        /// <summary>
        /// 根据enum值获取description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
