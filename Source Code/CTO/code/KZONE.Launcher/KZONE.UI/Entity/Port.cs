using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KZONE.UI
{
    public class Port
    {
        public string LineID { get; set; }
        public string ServerName { get; set; }
        public string NodeNo { get; set; }
        public string NodeID { get; set; }
        public int PortNo { get; set; }
        public string PortNo_S { get; set; }
        public string RelationPortNo { get; set; }  //for DPI ,紀錄有關連的port no (port no by node)
        public string PortID { get; set; }        
        public string PortAttribute { get; set; }
        public string ProcessStartType { get; set; }
        public string PortName { get; set; }
        public int MaxCount { get; set; }
        public bool MapplingEnable { get; set; }
        public bool IsEmptyCST { get; set; }  //是否為空CST - 空CST可不輸入 Port & Cassette 層資料 --for offline下貨畫面使用

       
        public ePortType PortType { get; set; }
        public eCassettePortStatus CassettePortStatus { get; set; }        
        public eCassetteType CassetteType { get; set; }//LoadingPort,UnloadingPort
        public ePortJudge PortJudge { get; set; }
        public ePortEnable PortEnable { get; set; }
        public ePortTransfer PortTransfer { get; set; }
        public eLoadingCassetteType LoadingCassetteType { get; set; }
        public string CassetteSeqNo { get; set; }
        public string CassetteSettingCode { get; set; }
        public string GlassCountINCST { get; set; }
        public string CassetteID { get; set; }        
        public string PortGrade { get; set; }
        public string ProductType { get; set; }
        public int PortGlassCount { get; set; }  //下貨數量
       
        public UnloaderDispatch RobotUnloaderDispatch { get; set; } //Robot Unloader Dispatch

        public string FlowPriority { get; set; }//for MQC insp Flow Priority ，兩碼機台表示 - 030405 => 順序為L3->L4->L5 ; 030000=>第一優先為L3，之後無設定

        //"": No Status
        //"WACSTEDIT": Wait for OPI Edit Cassette Data (for online local/Offline)
        //"WAREMAPEDIT": Wait for OPI remap edit Cassette Data.
        //"WASTART": Wait for OPI Start command
        public string SubCassetteStatus { get; set; } 

        public string JobExistenceSlot { get; set; }  //001 ~ 999 有存在玻璃的slot (00001111...)

        public int GroupNumber { get; set; }
        public string SortGrade { get; set; }
      //  public BCS_SlotPositionReply BC_SlotPositionReply;

        #region for Dense
        public ePackingMode PackingMode { get; set; }
        public string BoxID01 { get; set; }
        public string BoxID02 { get; set; }
        public eUnpackSource UnpackingSource { get; set; }
        public bool DenseBoxDataRequest { get; set; }  //是否可下貨s
        #endregion

        public Port()
        {
            PortGrade = string.Empty;
            SortGrade = string.Empty;
            CassetteSeqNo = string.Empty;
            CassetteID = string.Empty;
            ProductType = string.Empty;
            FlowPriority = string.Empty;
            PortGlassCount = 0;

          //  BC_SlotPositionReply = new BCS_SlotPositionReply();
            RobotUnloaderDispatch = new UnloaderDispatch();

            
        }
        #region

        //public void SetCassettePortInfo(CSTPortStatusReport PortData)
        //{
        //    int _num = 0;

        //    this.LineID = PortData.BODY.LINEID;
        //    this.CassetteID = PortData.BODY.CASSETTEID;
        //    this.CassetteSeqNo = PortData.BODY.CASSETTESEQNO;
        //    this.CassetteSettingCode = PortData.BODY.CASSETTESETTINGCODE;
        //    this.PortGlassCount = (int.TryParse(PortData.BODY.PORTCNT, out _num) == true) ? int.Parse(PortData.BODY.PORTCNT) : 0;
        //    this.PortEnable = PortData.BODY.PORTENABLEMODE == string.Empty ? ePortEnable.Unused : (ePortEnable)int.Parse(PortData.BODY.PORTENABLEMODE);
        //    this.PortGrade = PortData.BODY.PORTGRADE;
        //    this.SortGrade = PortData.BODY.BCSETSORTGRADE;
        //    this.PortJudge = PortData.BODY.PORTJUDGE == string.Empty ? ePortJudge.None : (ePortJudge)int.Parse(PortData.BODY.PORTJUDGE);
        //    this.PortID = PortData.BODY.PORTID;
        //    this.PortNo = Convert.ToInt32(PortData.BODY.PORTNO);
        //    this.PortTransfer = PortData.BODY.PORTTRANSFERMODE == string.Empty ? ePortTransfer.Unused : (ePortTransfer)int.Parse(PortData.BODY.PORTTRANSFERMODE);
        //    if (int.Parse(PortData.BODY.PORTTYPE) < 0 || int.Parse(PortData.BODY.PORTTYPE) > 3) this.PortType = ePortType.UnKnown;
        //    else this.PortType = PortData.BODY.PORTTYPE == string.Empty ? ePortType.UnKnown : (ePortType)int.Parse(PortData.BODY.PORTTYPE);
        //    this.JobExistenceSlot = PortData.BODY.JOBEXISTSLOT.PadRight(MaxCount, '0');
        //    this.SubCassetteStatus = PortData.BODY.SUBCSTSTATE;
        //    this.LoadingCassetteType = PortData.BODY.LOADINGCASSETTETYPE == string.Empty ? eLoadingCassetteType.Unused : (eLoadingCassetteType)int.Parse(PortData.BODY.LOADINGCASSETTETYPE);
        //    this.CassettePortStatus = PortData.BODY.CASSETTEPORTSTATUS == string.Empty ? eCassettePortStatus.Unused : (eCassettePortStatus)int.Parse(PortData.BODY.CASSETTEPORTSTATUS);
        //    this.GroupNumber = (int.TryParse(PortData.BODY.GROUPNUMBER, out _num) == true) ? int.Parse(PortData.BODY.GROUPNUMBER) : 0;
          
        //}

        //public void SetCassettePortInfo(CSTPortStatusReply PortData)
        //{
        //    int _num = 0;

        //    this.LineID = PortData.BODY.LINEID;
        //    this.CassetteID = PortData.BODY.CASSETTEID;
        //    this.CassetteSeqNo = PortData.BODY.CASSETTESEQNO;
        //    this.CassetteSettingCode = PortData.BODY.CASSETTESETTINGCODE;
        //    this.PortGlassCount = (int.TryParse(PortData.BODY.PORTCNT, out _num) == true) ? int.Parse(PortData.BODY.PORTCNT) : 0;
        //    this.PortEnable = PortData.BODY.PORTENABLEMODE == string.Empty ? ePortEnable.Unused : (ePortEnable)int.Parse(PortData.BODY.PORTENABLEMODE);  //PortData.BODY.PORTENABLEMODE;
        //    this.PortGrade = PortData.BODY.PORTGRADE;
        //    this.SortGrade = PortData.BODY.BCSETSORTGRADE;
        //    this.PortJudge = PortData.BODY.PORTJUDGE == string.Empty ? ePortJudge.None : (ePortJudge)int.Parse(PortData.BODY.PORTJUDGE);  //PortData.BODY.PORTMODE;
        //    this.PortID = PortData.BODY.PORTID;
        //    this.PortNo = Convert.ToInt32(PortData.BODY.PORTNO);
        //    if (int.Parse(PortData.BODY.PORTTYPE) < 0 || int.Parse(PortData.BODY.PORTTYPE) > 3) this.PortType = ePortType.UnKnown;
        //    else this.PortType = PortData.BODY.PORTTYPE == string.Empty ? ePortType.UnKnown : (ePortType)int.Parse(PortData.BODY.PORTTYPE);
        //    this.PortTransfer = PortData.BODY.PORTTRANSFERMODE == string.Empty ? ePortTransfer.Unused : (ePortTransfer)int.Parse(PortData.BODY.PORTTRANSFERMODE);
        //    this.JobExistenceSlot = PortData.BODY.JOBEXISTSLOT.PadRight(MaxCount, '0');
        //    this.SubCassetteStatus = PortData.BODY.SUBCSTSTATE;
        //    this.LoadingCassetteType = PortData.BODY.LOADINGCASSETTETYPE == string.Empty ? eLoadingCassetteType.Unused : (eLoadingCassetteType)int.Parse(PortData.BODY.LOADINGCASSETTETYPE);
        //    this.CassettePortStatus = PortData.BODY.CASSETTEPORTSTATUS == string.Empty ? eCassettePortStatus.Unused : (eCassettePortStatus)int.Parse(PortData.BODY.CASSETTEPORTSTATUS);
        //    this.GroupNumber = (int.TryParse(PortData.BODY.GROUPNUMBER, out _num) == true) ? int.Parse(PortData.BODY.GROUPNUMBER) : 0;
        //}
        //public void SetPortInfo(AllDataUpdateReply.PORTc PortData)
        //{
        //    int _num = 0;

        //    this.LineID = PortData.LINEID;
        //    this.CassetteID = PortData.CASSETTEID;
        //    this.CassetteSeqNo = PortData.CASSETTESEQNO;
        //    this.PortGlassCount = (int.TryParse(PortData.PORTCNT, out _num) == true) ? int.Parse(PortData.PORTCNT) : 0;
        //    this.PortEnable = PortData.PORTENABLEMODE==string.Empty ? ePortEnable.Unused:(ePortEnable)int.Parse(PortData.PORTENABLEMODE);
        //    this.PortGrade = PortData.PORTGRADE;
        //    this.SortGrade = PortData.BCSETSORTGRADE;
        //    this.PortJudge =PortData.PORTJUDGE==string.Empty ? ePortJudge.None: (ePortJudge)int.Parse(PortData.PORTJUDGE);  //PortData.PORTMODE;
        //    this.PortID = PortData.PORTID;
        //    this.PortNo = Convert.ToInt32(PortData.PORTNO);
        //    if (int.Parse(PortData.PORTTYPE) < 0 || int.Parse(PortData.PORTTYPE) > 3) this.PortType = ePortType.UnKnown;
        //    else this.PortType = PortData.PORTTYPE == string.Empty ? ePortType.UnKnown : (ePortType)int.Parse(PortData.PORTTYPE);
        //    this.PortTransfer = PortData.PORTTRANSFERMODE==string.Empty ? ePortTransfer.Unused: (ePortTransfer)int.Parse(PortData.PORTTRANSFERMODE);  
        //    this.JobExistenceSlot = PortData.JOBEXISTSLOT.PadRight(MaxCount, '0');
        //    this.SubCassetteStatus = PortData.SUBCSTSTATE;
        //    this.CassettePortStatus = PortData.CASSETTEPORTSTATUS == string.Empty ? eCassettePortStatus.Unused : (eCassettePortStatus)int.Parse(PortData.CASSETTEPORTSTATUS);
        //    this.GroupNumber = (int.TryParse(PortData.GROUPNUMBER, out _num) == true) ? int.Parse(PortData.GROUPNUMBER) : 0;
        //}


        #endregion
    }

    public class UnloaderDispatch           
    {
        public string Grade01 = string.Empty;
        public string Grade02 = string.Empty;
        public string Grade03 = string.Empty;
        public string OperatorID = string.Empty;
    }
}
