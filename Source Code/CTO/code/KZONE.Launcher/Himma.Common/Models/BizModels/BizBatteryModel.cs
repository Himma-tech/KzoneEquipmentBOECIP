using Himma.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimmaCommon.Models.BizModels
{
    public class BizBatteryModel
    {
        public int Id { get; set; }
        public string BinName { get; set; }
        public string TrayNo { get; set; } = "";
       // public TrayLoadType TrayLoadType { get; set; }
        public int ChannelId { get; set; }
        public string BatteryNo { get; set; }
        public int BatteryId { get; set; }
        public BatteryType BatteryType { get; set; }
    }


    public class BizExchangeBatteryModel
    {
        public string BatteryNo { get; set; }
        public int VirtualNo { get; set; }
        public int BatteryType { get; set; }
        public string SCupNo { get; set; }
        public string JCupNo { get; set; }
        public int MesResult { get; set; }
        public DateTime OutDate { get; set; }
        public DateTime CreatDate { get; set; }

    }
    public class BizBatteryInfoModel
    {
        public string BatteryNo { get; set; }//电芯编码
        public int VirtualNo { get; set; }//虚拟号
        public DateTime CreateDate { get; set; }//创建时间
        public string BeJCupNo { get; set; }//前托杯交换_紧托杯码
        public string BeSCupNo { get; set; }//前托杯交换_松托杯码
        public DateTime ScankDate { get; set; }//扫码时间
        public DateTime PackDate { get; set; }//组盘时间
        public string TrayNo { get; set; }//托盘编码
        public int ChannelNo { get; set; }//通道编码
        public DateTime InDate { get; set; }//入库时间
        public string LocNo { get; set; }//炉腔
        public int LocOffset { get; set; }//炉腔层数
        public DateTime OutDate { get; set; }//出库时间
        public DateTime UnpackDate { get; set; }//拆盘时间
        public string AfJCupNo { get; set; }//后托杯交换_紧托杯码
        public string AfSCupNo { get; set; }//后托杯交换_松托杯码
        public int SaveType { get; set; }//前台保存信息类型

    }
    public class BizBatteryHipotModel
    {
        public int BatteryId { get; set; }
        public string BatteryNo { get; set; }
        public string HipotValue { get; set; }
        public string TrayNo { get; set; }
    }

    /// <summary>
    /// 水电芯
    /// </summary>
    public class BizWBatteryModel : BizBatteryModel
    {
        /// <summary>
        /// 腔体内偏移
        /// </summary>
        public int BinOffset { get; set; } = 0;
        /// <summary>
        /// 夹具内电芯数量
        /// </summary>
        public int BatteryCount { get; set; }
        /// <summary>
        /// 出炉时间
        /// </summary>
        public string OutDate { get; set; }
        /// <summary>
        /// 下料时间
        /// </summary>
        public string UnpackDate { get; set; }
        /// <summary>
        /// 下料结束时间
        /// </summary>
        public string UnpackFinishDate { get; set; }
        /// <summary>
        /// 下料工位名称
        /// </summary>
        public string LocName { get; set; }
    }
}
