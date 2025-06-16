using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    /// <summary>
    /// RobotWIP: 需要存檔的資料.
    /// 為了動態載入 Robot DLL, 因此區分 RobotWIPFile 和 RobotWIP
    /// RobotWIPFile 是 Robot 對 Job 額外存檔的資料
    /// RobotWIP 是 Robot 每次迴圈即時產生的計算用物件
    /// </summary>
    [Serializable]
    public class RobotJobFile : ICloneable
    {
        /// <summary>
        /// 玻璃狀態
        /// </summary>
        public enum GLASS_STATUS
        {
            /// <summary>
            /// 沒有狀態
            /// </summary>
            NONE = 0,

            /// <summary>
            /// 新建玻璃
            /// </summary>
            NewWIP = 1,

            /// <summary>
            /// Wait Cassette Start
            /// </summary>
            WaitStart = 2,

            /// <summary>
            /// Cassette Start 後, 等待被取出
            /// </summary>
            WaitProcess = 3,

            /// <summary>
            /// RCS 判斷玻璃不能被取出
            /// </summary>
            SkipProcess = 4,

            /// <summary>
            /// 玻璃已被取出, 投入產線
            /// </summary>
            InProcess = 5,

            /// <summary>
            /// 機台上報玻璃移除
            /// </summary>
            Remove = 6,

            /// <summary>
            /// 機台上報玻璃破片
            /// </summary>
            Scrap = 7,

            /// <summary>
            /// 結束, 回到原 Cassette 原 Slot
            /// </summary>
            End = 8,

            /// <summary>
            /// 結束, Changer Mode 搬到 Target Port, 或者 NG Mode 搬到 NG Port
            /// </summary>
            OtherSlotEnd = 9,

            /// <summary>
            /// 結束, 因為 Force Clean Out 回 Port
            /// </summary>
            ForceCleanOutEnd = 10,

            /// <summary>
            /// 有料無帳
            /// </summary>
            LostWIP = 11,

            /// <summary>
            /// 等 Port Status 恢復
            /// </summary>
            WaitResume = 12
        }

        [NonSerialized]
        private string _reasonOfNotSend = string.Empty;

        /// <summary>
        /// RouteName, 非 Cool Run 時由 Indexer Run Mode 決定 Route
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// true表示 Glass 是因為 Prefetch 才被 Robot 取出
        /// </summary>
        public bool FetchOutByPrefetch { get; set; }

        /// <summary>
        /// RCS 對 Glass 的 StepID 記錄, 儲存 RCS SCAN 時即時計算的結果
        /// </summary>
        public int LastStepID { get; set; }

        /// <summary>
        /// RCS 對 Glass 的 StepID 記錄, 儲存 RCS SCAN 時即時計算的結果, 提供給 Slot Process Status 判斷
        /// </summary>
        public int RealStepID { get; set; }

        /// <summary>
        /// RCS 對 Glass 的 Tracking Data History 記錄, Glass 進 Stage 或 出 Stage 都要 On Tracking Data History Bit, 在 Arm Load Unload 的時候更新
        /// </summary>
        public string LastTrackingDataHistory { get; set; }

        /// <summary>
        /// StageName, Indexer 取片時是從哪一個 Stage 取出, 提供 TTP Daily Check 計算 Real Step.
        /// </summary>
        public string GetFromStage { get; set; }

        /// <summary>
        /// StageName, Indexer 放片時是放哪一個 Stage
        /// </summary>
        public string PutToStage { get; set; }

        /// <summary>
        /// 當前所在的位置不允許出片的原因, 空字串表示允許出片, 提供給 L1_ReserveJobSignal 做判斷, 不存檔
        /// </summary>
        public string ReasonOfNotSend
        {
            get { return _reasonOfNotSend; }
            set { _reasonOfNotSend = value; }
        }

        public GLASS_STATUS GlassStatus { get; set; }

        /// <summary>
        /// Cassette 裡要出片的第一片
        /// </summary>
        public bool FirstGlass { get; set; }

        /// <summary>
        /// Cassette 裡要出片的最後一片
        /// </summary>
        public bool LastGlass { get; set; }

        /// <summary>
        /// A1PHL, 同 Cassette 內相同 Recipe 的最後一片
        /// </summary>
        public bool LotEndIndicate { get; set; }

        public RobotJobFile()
        {
            int tracking_data_his = 32;

            RouteName = string.Empty;
            FetchOutByPrefetch = false;
            LastStepID = 0;
            RealStepID = 0;
            LastTrackingDataHistory = string.Empty.PadLeft(tracking_data_his, '0');
            GetFromStage = string.Empty;
            PutToStage = string.Empty;
            ReasonOfNotSend = "New WIP";// 尚未 Download Cassette Data, 要填 ReasonOfNotSend, 避免 L1_ReserveJobSignal 閃滅. 被 RCS 拿去做判斷, 不可隨意改值
            GlassStatus = GLASS_STATUS.NewWIP;
            FirstGlass = false;
            LastGlass = false;
            LotEndIndicate = false;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public RobotJobFile CoolRunClone()
        {
            RobotJobFile ret = (RobotJobFile)this.MemberwiseClone();
            int tracking_data_his = 32;
            // RouteName = string.Empty; Route Name 要保留
            ret.LastStepID = 1;
            ret.RealStepID = 1;
            ret.LastTrackingDataHistory = string.Empty.PadLeft(tracking_data_his, '0');
            ret.GetFromStage = string.Empty;
            ret.PutToStage = string.Empty;
            ret.ReasonOfNotSend = "Wait Cool Run Start";// 尚未 Download Cassette Data, 要填 ReasonOfNotSend, 避免 L1_ReserveJobSignal 閃滅. 被 RCS 拿去做判斷, 不可隨意改值
            ret.GlassStatus = GLASS_STATUS.WaitStart;
            ret.FirstGlass = false;
            ret.LastGlass = false;
            ret.LotEndIndicate = false;
            return ret;
        }

        //brent add 20170606 進Cleaner出過時間,則出片用,A1ISP/MSP的清洗機使用
        public DateTime CleanerOverTimeBegin { get; set; }
	}
}
