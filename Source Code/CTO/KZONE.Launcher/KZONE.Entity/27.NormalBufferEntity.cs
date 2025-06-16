using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    [Serializable]
    public class NormalBuffer
    {
        /// <summary>
        /// 以下為CF Specail:DailyBuffer用BufferNo是指Buffer數量,HKC目前(2016/12/12)都只有1個Buffer,所以BufferNo都是1
        /// </summary>
        private string _bufferNo = "1";
        private eCFBufSlotType _cFBufSlotType;
        private string _cFBufSlotCstSeqNo;
        private string _cFBufslotCstSlotNo;
        private eCFBufSlotStatus _cFBufslotStatus;
        private string _bufferSlotNo;
        private string _reservedCstSeqNo;

        #region Common的NormalBuffer
        /// <summary>
        /// 以下為Common NormalBuffer用
        /// </summary>
        private eBUFStatus _bufStatus = eBUFStatus.Unused;

        private string _bufGlassCount = "0";
        private string _settingofUpLevelCount = "0";
        private string _settingofDownLevelCount = "0";

        public eBUFStatus BufStatus
        {
            get { return _bufStatus; }
            set { _bufStatus = value; }
        }

        public string BufGlassCount
        {
            get { return _bufGlassCount; }
            set { _bufGlassCount = value; }
        }

        public string SettingofUpLevelCount
        {
            get { return _settingofUpLevelCount; }
            set { _settingofUpLevelCount = value; }
        }

        public string SettingofDownLevelCount
        {
            get { return _settingofDownLevelCount; }
            set { _settingofDownLevelCount = value; }
        }
        #endregion

        public string BufferNo
        {
            get { return _bufferNo; }
            set { _bufferNo = value; }
        }

        public string ReservedCstSeqNo
        {
            get { return _reservedCstSeqNo; }
            set { _reservedCstSeqNo = value; }
        }
        private string _reservedCstSlotNo;

        public string ReservedCstSlotNo
        {
            get { return _reservedCstSlotNo; }
            set { _reservedCstSlotNo = value; }
        }
        private string _reservedJobId;

        public string ReservedJobId
        {
            get { return _reservedJobId; }
            set { _reservedJobId = value; }
        }
        private string _reservedJobPpid;

        public string ReservedJobPpid
        {
            get { return _reservedJobPpid; }
            set { _reservedJobPpid = value; }
        }
        private string _targetEqp;

        public string TargetEqp
        {
            get { return _targetEqp; }
            set { _targetEqp = value; }
        }
        private string _reservationReason;

        public string ReservationReason
        {
            get { return _reservationReason; }
            set { _reservationReason = value; }
        }

       private string _reservationHour;

       public string ReservationHour
       {
           get 
           { 
               if(_reservationHour == "")
               {
                   _reservationHour = "0";
               }
                    return _reservationHour; 
           }
           set { _reservationHour = value; }
       }
       private string _reservationMin;

       public string ReservationMin
       {
           get 
           { 
               if(_reservationMin == "")
               {
                   _reservationMin = "0";
               }
                    return _reservationMin; 
           }
           set { _reservationMin = value; }
       }

        public string BufferSlotNo
        {
            get { return _bufferSlotNo; }
            set { _bufferSlotNo = value; }
        }

        public eCFBufSlotType CFBufSlotType
        {
            get { return _cFBufSlotType; }
            set { _cFBufSlotType = value; }
        }

        public string CFBufSlotCstSeqNo
        {
            get { return _cFBufSlotCstSeqNo; }
            set { _cFBufSlotCstSeqNo = value; }
        }

        public string CFBufSlotCstSlotNo
        {
            get { return _cFBufslotCstSlotNo; }
            set { _cFBufslotCstSlotNo = value; }
        }

        public eCFBufSlotStatus CFBufSlotStatus
        {
            get { return _cFBufslotStatus; }
            set { _cFBufslotStatus = value; }
        }

        public NormalBuffer(string bufferNo, string slotNo)
        {
            BufferNo = bufferNo;
            BufferSlotNo = slotNo;
           

        }

        public NormalBuffer()
        {
            
        }

        public NormalBuffer(string bufNo, eBUFStatus bufStatus, string bufGlassCount, string SetofUpLevelCount, string SetofDownLevelCount)
        {
            BufferNo = bufNo;
            BufStatus = bufStatus;
            BufGlassCount = bufGlassCount;
            SettingofUpLevelCount = SetofUpLevelCount;
            SettingofDownLevelCount = SetofDownLevelCount;
        }
    }
}
