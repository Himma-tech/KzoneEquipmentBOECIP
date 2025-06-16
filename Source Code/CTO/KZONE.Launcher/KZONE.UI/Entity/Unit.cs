using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.UI
{
    public class Unit
    {
        public string ServerName { get; set; }
        public string LineID{ get; set; }
        public string NodeID{ get; set; }
        public int UnitNo{ get; set; }
        public string UnitID{ get; set; }
        public string NodeNo{ get; set; }
        public string UnitType{ get; set; }
        public string UseRunMode { get; set; }  // N : 沒有run mode , Y: 可設定 & 須顯示 , R : 只顯示 不可設定run mode
        public string HSMSStatus { get; set; }
        public string SubUnit { get; set; }

        #region For CELL Buffer
        public int BufferWarningCount { get; set; }  //Buffer Warning Glass Setting Count
        public int BufferCurrentCount { get; set; }  //Buffer Current Glass Count
        public int BufferTotalSlotCount { get; set; }  //Buffer Total Slot Count
        public int BufferWarningStatus { get; set; }  //1：Set ; 2：Reset
        public int BufferStoreOverAlive { get; set; }  //1：Set ; 2：Reset
        #endregion
        
   //     public eEQPStatus UnitStatus { get; set; }

        public int TotalGlassCount { get; set; }
        public int ProductGlassCount { get; set; }
        public int HistoryGlassCount { get; set; }
        public int TFTGlassCount { get; set; }
        public int CFGlassCount { get; set; }
        public int DailyGlassCount { get; set; }
        public int AlarmCode { get; set; }
        public string UnitRunMode { get; set; }

        public Unit()
        {
           // UnitStatus= eEQPStatus.Unused;
            TFTGlassCount = 0;
            CFGlassCount = 0;
            AlarmCode = 0;
            UnitRunMode=string.Empty ;
            SubUnit = string.Empty;
        }

        #region

        //public void SetUnitInfo(EquipmentStatusReply.UNITc unitData)
        //{
        //    int _num = 0;
        //    ProductGlassCount = (int.TryParse(unitData.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(unitData.PRODUCTGLASSCOUNT) : 0;
       
        //    UnitStatus = (eEQPStatus)int.Parse(unitData.CURRENTSTATUS);
       
        //    UnitRunMode = unitData.UNITRUNMODE;
        //    AlarmCode = (int.TryParse(unitData.ALARMCODE, out _num) ==true) ? int.Parse(unitData.ALARMCODE) : 0;
        //    HSMSStatus = unitData.HSMSSTATUS.ToString();

        //}

        //public void SetUnitInfo(EquipmentStatusReport.UNITc unitData)
        //{
        //    int _num = 0;
            
        //    this.CFGlassCount = (int.TryParse(unitData.CFGLASSCNT, out _num) == true) ? int.Parse(unitData.CFGLASSCNT) : 0;
        //    this.UnitStatus = (eEQPStatus)int.Parse(unitData.CURRENTSTATUS);
        //    this.TFTGlassCount = (int.TryParse(unitData.TFTGLASSCNT, out _num) == true) ? int.Parse(unitData.TFTGLASSCNT) : 0;
        //    this.DailyGlassCount = (int.TryParse(unitData.DAILYMONITORGLASSCNT, out _num) ==true) ? int.Parse(unitData.DAILYMONITORGLASSCNT) : 0 ;
        //    this.ProductGlassCount = (int.TryParse(unitData.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(unitData.PRODUCTGLASSCOUNT) : 0;
        //    this.AlarmCode = (int.TryParse(unitData.ALARMCODE, out _num) == true) ? int.Parse(unitData.ALARMCODE) : 0;
        //    UnitRunMode = unitData.UNITRUNMODE;
        //    HSMSStatus = unitData.HSMSSTATUS.ToString();

        //}

        //public void SetUnitInfo(AllDataUpdateReply.UNITc unitData)
        //{
        //    int _num = 0;
        //    this.HistoryGlassCount = (int.TryParse(unitData.HISTORYGLASSCOUNT, out _num) == true) ? int.Parse(unitData.HISTORYGLASSCOUNT) : 0;
        //    this.TotalGlassCount = (int.TryParse(unitData.TOTALGLASSCOUNT, out _num) == true) ? int.Parse(unitData.TOTALGLASSCOUNT) : 0;
        //    this.ProductGlassCount = (int.TryParse(unitData.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(unitData.PRODUCTGLASSCOUNT) : 0;
        //    this.ProductGlassCount = (int.TryParse(unitData.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(unitData.PRODUCTGLASSCOUNT) : 0;
        //    this.UnitStatus = (eEQPStatus)int.Parse(unitData.CURRENTSTATUS);
        //    this.AlarmCode = (int.TryParse(unitData.ALARMCODE, out _num) == true) ? int.Parse(unitData.ALARMCODE) : 0;
        //    UnitRunMode = unitData.UNITRUNMODE;
        //    HSMSStatus = unitData.HSMSSTATUS.ToString();
        //}

        #endregion
    }
}
