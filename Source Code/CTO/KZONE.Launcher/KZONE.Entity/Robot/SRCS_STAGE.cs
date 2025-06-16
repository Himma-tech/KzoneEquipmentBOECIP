using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;

namespace KZONE.Entity
{
    public class SRCS_STAGE : EntityData
    {
        private void SetStringProertyToStringEmpty()
        {
            PropertyInfo[] properties = typeof(SRCS_STAGE).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(this, string.Empty, null);
            }
        }

        #region Constructors

        public SRCS_STAGE()
        {
            SetStringProertyToStringEmpty();
        }

        #endregion

        #region Public Properties

        public virtual long OBJECTKEY { get; set; }

        public virtual string SERVERNAME { get; set; }

        public virtual string STAGENAME { get; set; }

        public virtual int STAGE_POSITION { get; set; }

        public virtual string SCAN_METHOD { get; set; }

        public virtual string LINEID { get; set; }

        public virtual string NODENO { get; set; }

        public virtual string PORT_ID { get; set; }

        public virtual string NODE_ID { get; set; }

        public virtual string ROBOTNAME { get; set; }

        public virtual int STAGE_PRIORITY { get; set; }

        public virtual string UPSTREAM_PATHNO { get; set; }

        public virtual string DOWNSTREAM_PATHNO { get; set; }

        /// <summary>
        /// Send Out Glass Data Block, 以分號隔開;
        /// "01" : 表示 Glass 從 Stage 出來, Glass Data 會帶在 Sending Job Data Report#01
        /// "02;03" : 表示 Glass 從 Stage 出來, Glass Data 會帶在 Sending Job Data Report#02 和 Sending Job Data Report#03
        /// </summary>
        public virtual string SEND_GDB { get; set; }

        /// <summary>
        /// Receive In Glass Data Block, 以分號隔開;
        /// "01" : 表示 Glass 進 Stage, Glass Data 會帶在 Receiving Job Data Report#01
        /// "02;03" : 表示 Glass 進 Stage, Glass Data 會帶在 Receiving Job Data Report#02 和 Receiving Job Data Report#03
        /// </summary>
        public virtual string RECV_GDB { get; set; }

        public virtual string STAGETYPE { get; set; }

        public virtual string CASSETTETYPE { get; set; }

        public virtual int SLOTCOUNT { get; set; }

        public virtual int MAXSLOT { get; set; }

        /// <summary>
        /// 以分號隔開;
        /// "0" : 表示 Glass 從 Stage 進去或出來，Tracking Data 都應該要 On 第零個 Bit;
        /// "1;2" : 表示 Glass 從 Stage 進去或出來，Tracking Data 都應該要 On 第一和(或)第二個 Bit，(和/或)由 Robot DLL 決定
        /// </summary>
        public virtual string GLASSBIT { get; set; }

        /// <summary>
        /// "0" : 表示 Glass 要放進 Stage 重新檢測
        /// </summary>
        public virtual string REDOBIT { get; set; }

        /// <summary>
        /// "1" : 0 表示進任意 Stage; 從 1 開始表示 Glass 要指定放進 Stage
        /// </summary>
        public virtual string INSPBIT { get; set; }

        /// <summary>
        /// Stage Recipe 是 Job Data 中的第幾個 Recipe Item
        /// 若有多個則以分號隔開，如 CVD Chamber
        /// "0": 表示 Job Data 中的第 0 個 Recipe Item
        /// "1;2": 表示 Job Data 中的第 1 個及第 2 個 Recipe Item
        /// </summary>
        public virtual string RECIPEINDEX { get; set; }

        public virtual string SLOTFETCHSEQ { get; set; }

        public virtual string SLOTSTORESEQ { get; set; }

        public virtual string ARM_FETCH_SEQ { get; set; }

        public virtual string ARM_STORE_SEQ { get; set; }

        public virtual string GETREADYFLAG { get; set; }

        public virtual int GETREADYSLOT { get; set; }

        public virtual string GETREADYARM { get; set; }

        public virtual string PUTREADYFLAG { get; set; }

        public virtual string PREFETCHFLAG { get; set; }

        public virtual string BOTHARMFLAG { get; set; }

        public virtual string EXCHANGE_TYPE { get; set; }

        #endregion
    }
}
