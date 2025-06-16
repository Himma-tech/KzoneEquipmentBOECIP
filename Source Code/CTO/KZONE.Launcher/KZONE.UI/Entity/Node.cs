using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KZONE.UI
{
    public class Node
    {
        #region DB
        public string LineID { get; set; }
        public string ServerName { get; set; }
        public string NodeNo { get; set; }
        public string NodeID { get; set; }
        public string NodeName { get; set; }

        public string ReportMode { get; set; }  
        public string NodeAttribute { get; set; }
        public string OPISpecialType { get; set; }
        public string DefaultRecipeNo { get; set; }

        public int UnitCount { get; set; }
        public int RecipeLen { get; set; }
        public List<int> RecipeSeq { get; set; }
               
        public bool RecipeRegisterCheck { get; set; }
        public bool RecipeParameterCheck { get; set; }

        public bool APCReport { get; set; }
        public bool EnergyReport { get; set; }
        public int APCReportTime { get; set; }
        public int EnergyReportTime { get; set; }

        public string UseRunMode { get; set; } //Y : 有run mode 且OPI提供run mode切換, R : 有run mode 且OPI不提供run mode切換 ,N: 沒有run mode
        public bool UseIndexerMode { get; set; }
        public bool MaterialStatus { get; set; }  //0: OFF ,1: ON

        public string UseEDCReport { get; set; }

        public string LastReceiveGlassID { get; set; }  //最後收片的glass id
        public string LastReceiveDateTime { get; set; } //最後收片的時間
        public bool IsOverTackTime { get; set; }        //是否超過tack time設定未收片
        #endregion

      //  public eEQPStatus EQPStatus { get; set; }

        public string RecipeName { get; set; }

        public string EQPRunMode { get; set; }

        public string EQPIndexerRunMode { get; set; }
      
        public List<string> AllowUnitRunMode { get; set; }  //記錄哪些EQ run mode下允許設定unit run mode 
        public List<LineRunMode> LineRunModes { get; set; }  //EQ Run mode
        public List<LineRunMode> LineUnitRunModes { get; set; } //Unit Run Mode

    
        public eEQPOperationMode OperationMode { get; set; }

        public eAutoControlMode AutoControlMode { get; set; }

        #region count
        public int TotalGlassCount { get; set; }
        public int ProductGlassCount { get; set; }
        public int HistoryGlassCount { get; set; }
        public int UVMASKGlassCount { get; set; }
        public int DailyMonitorGlassCount { get; set; }
        public int TotalCount { get; set; }
        public int TFTGlassCount { get; set; }
        public int CFGlassCount { get; set; }
        public int DummyGlassCount { get; set; }
        public int ThroughDummyGlassCount { get; set; }
        public int ThicknessDummyGlassCount { get; set; }
        
        #endregion
                  
        #region Equipment Status
        public string EquipmentAlive { get; set; }
        public eCIMMode CIMMode { get; set; }
        public bool AlarmStatus { get; set; }
        public string AutoRecipeChange { get; set; }
        public int GroupNumber { get; set; }
        public int AlarmCode { get; set; }
      
        #endregion

        #region Special Mode Report

        public string ByPassMode { get; set; }
        public string ForceClearout { get; set; }
        public eTurnTable TurnTableMode { get; set; }
        public eBufferMode BufferMode { get; set; } //For CF ISP L5_Sputter Handle
        public ePreSputterMode PrePutterMode { get; set; }//For CF ISP L3_CV1
        #endregion

        #region Job Data Check Mode
        public bool JobDataCheckMode { get; set; }
        public bool COAVersionCheckMode  { get; set; }
        public bool JobDuplicateCheckMode { get; set; }
        public bool PorductIDCheckMode { get; set; }
        public bool GroupIndexCheckMode { get; set; }
        public bool ProductTypeCheckMode { get; set; }
        public string RecipeIDCheckMode { get; set; }
        #endregion

        public int CassetteQTime { get; set; }
        public string CSTOperationMode { get; set; }
        public string HSMSStatus { get; set; }
        public string HSMSControlMode { get; set; }

        public int InspectionIdleTime { get; set; }

        public List<string> Lst_TrackTimeUnit { get; set; }
        public Dictionary<string, string> Dic_TrackDelayTime { get; set; }
        public List<DCR> VCRs { get; set; }
        public List<InterLock> InterLocks { get; set; }
        public List<SamplingSide> SamplingSides { get; set; }
        public List<Dispatch> Dispatches { get; set; }
        public List<Buffer> Buffer { get; set; }
        //public Dictionary<string, BCS_EachPositionReply> Dic_Position { get; set; } //Key: position unit no (兩碼)
        //public BCS_EquipmentAlarmStatusReply BC_EquipmentAlarmStatusReply;
        //public BCS_IonizerFanModeReportReply BC_IonizerFanModeReportReply;
        //public BCS_DefectCodeReply BC_DefectCodeReportReply;
        //public BCS_BufferSlotInfoReply BC_BufferSlotInfoReply;
        //public BCS_RTCControlInfo BC_RTCControlInfo;
        public eControlMode InitialControlMode { get; set; }
        public Node()
        {
            EQPRunMode = string.Empty;
            RecipeName = string.Empty;
            Buffer = new List<Buffer>();
            LastReceiveGlassID=string.Empty ;
            LastReceiveDateTime=string.Empty ;
            IsOverTackTime=false;

            AllowUnitRunMode = new List<string>();
            LineRunModes = new List<LineRunMode>();
            LineUnitRunModes = new List<LineRunMode>();

            InspectionIdleTime = 0;

            RecipeSeq = new List<int>();

            //Dic_Position = new Dictionary<string, BCS_EachPositionReply>();
            
            Lst_TrackTimeUnit = new List<string>();
            Dic_TrackDelayTime = new Dictionary<string, string>();
            //BC_EquipmentAlarmStatusReply = new BCS_EquipmentAlarmStatusReply();
            //BC_IonizerFanModeReportReply = new BCS_IonizerFanModeReportReply();
            //BC_DefectCodeReportReply = new BCS_DefectCodeReply();
            //BC_BufferSlotInfoReply = new BCS_BufferSlotInfoReply();
            //BC_RTCControlInfo = new BCS_RTCControlInfo();
            InitialControlMode = eControlMode.NomalMode;
        
        }
        #region

        //#region Get Node Info -- EquipmentStatusReport
        //public void SetNodeInfo(EquipmentStatusReport NodeData)
        //{
        //    int _num = 0;

        //    #region Count
        //    this.CFGlassCount = (int.TryParse(NodeData.BODY.CFGLASSCNT, out _num) == true) ? int.Parse(NodeData.BODY.CFGLASSCNT) : 0;
        //    this.DummyGlassCount = (int.TryParse(NodeData.BODY.DMYGLASSCNT, out _num) == true) ? int.Parse(NodeData.BODY.DMYGLASSCNT) : 0;
        //    this.TFTGlassCount = (int.TryParse(NodeData.BODY.TFTGLASSCNT, out _num) == true) ? int.Parse(NodeData.BODY.TFTGLASSCNT) : 0;
        //    this.ThicknessDummyGlassCount = (int.TryParse(NodeData.BODY.THICKNESSDMYGLASSCNT, out _num) == true) ? int.Parse(NodeData.BODY.THICKNESSDMYGLASSCNT) : 0;
        //    this.ThroughDummyGlassCount = (int.TryParse(NodeData.BODY.THROUGHDMYGLASSCNT, out _num) == true) ? int.Parse(NodeData.BODY.THROUGHDMYGLASSCNT) : 0;
        //    this.UVMASKGlassCount = (int.TryParse(NodeData.BODY.UVMASKGLASSCNT, out _num) == true) ? int.Parse(NodeData.BODY.UVMASKGLASSCNT) : 0;
        //    this.DailyMonitorGlassCount = (int.TryParse(NodeData.BODY.DAILYMONITORGLASSCNT, out _num) == true) ? int.Parse(NodeData.BODY.DAILYMONITORGLASSCNT) : 0;
        //    this.ProductGlassCount = (int.TryParse(NodeData.BODY.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.BODY.PRODUCTGLASSCOUNT) : 0;
        //    this.TotalGlassCount = (int.TryParse(NodeData.BODY.TOTALGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.BODY.TOTALGLASSCOUNT) : 0;
        //    this.HistoryGlassCount = (int.TryParse(NodeData.BODY.HISTORYGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.BODY.HISTORYGLASSCOUNT) : 0;
        //    #endregion

        //    #region Word
        //    this.OperationMode = NodeData.BODY.AUTOMANUALMODE.Contains(eEQPOperationMode.MANUAL.ToString())?eEQPOperationMode.MANUAL : eEQPOperationMode.AUTO;
        //    if (NodeData.BODY.AUTOCONTROLMODE == string.Empty && NodeData.BODY.AUTOCONTROLMODE != "Disable" && NodeData.BODY.AUTOCONTROLMODE != "Enable") this.AutoControlMode = eAutoControlMode.Unkown;
        //    else this.AutoControlMode = NodeData.BODY.AUTOCONTROLMODE == "Disable" ? eAutoControlMode.Disable : eAutoControlMode.Enable;
           
        //    this.CIMMode = NodeData.BODY.EQUIPMENTCIMMODE == string.Empty ? eCIMMode.OFF : (eCIMMode)int.Parse(NodeData.BODY.EQUIPMENTCIMMODE);
        //    this.RecipeName = NodeData.BODY.CURRENTRECIPEID;
        //    this.EQPStatus = NodeData.BODY.CURRENTSTATUS == string.Empty ? eEQPStatus.Unused : (eEQPStatus)int.Parse(NodeData.BODY.CURRENTSTATUS);
        //    this.AlarmStatus = NodeData.BODY.LOCALALARMSTATUS == "ON" ? true : false;
            
        //    this.BufferMode = NodeData.BODY.BUFFER_MODE == string.Empty ? eBufferMode.Unknown : (eBufferMode)int.Parse(NodeData.BODY.BUFFER_MODE);
        //    this.PrePutterMode = NodeData.BODY.PRE_SPUTTER_MODE == string.Empty ? ePreSputterMode.Unknown : (ePreSputterMode)int.Parse(NodeData.BODY.PRE_SPUTTER_MODE);
        //    this.HSMSStatus = NodeData.BODY.HSMSSTATUS.ToString();
        //    this.HSMSControlMode = NodeData.BODY.HSMSCONTROLMODE.ToString();
        //    this.EquipmentAlive = NodeData.BODY.EQALIVE == "1" ? "Alive" : "Down";
        //    this.AutoRecipeChange = NodeData.BODY.AUTORECIPECHANGEMODE;
        //    this.ByPassMode = NodeData.BODY.PASSMODE;
        //    ForceClearout = NodeData.BODY.FORCECLEAROUT_MODE;
        //    this.LastReceiveGlassID = NodeData.BODY.LASTGLASSID==null? string.Empty : NodeData.BODY.LASTGLASSID;
        //    this.LastReceiveDateTime = NodeData.BODY.LASTRECIVETIME == null ? string.Empty : NodeData.BODY.LASTRECIVETIME;
        //    this.AlarmCode = (int.TryParse(NodeData.BODY.ALARMCODE, out _num) == true) ? int.Parse(NodeData.BODY.ALARMCODE) : 0;
        //    this.GroupNumber = (int.TryParse(NodeData.BODY.GROUPNUMBER, out _num) == true) ? int.Parse(NodeData.BODY.GROUPNUMBER) : 0;//加GroupNumber Report
        //    this.EQPIndexerRunMode = NodeData.BODY.EQUIPMENTINDEXERRUNMODE.ToString();
           
        //    if (NodeData.BODY.EQUIPMENTRUNMODE == string.Empty)
        //        this.EQPRunMode = string.Empty;
        //    else
        //    {
        //        string _newRunMode = GetRunModeDesc(NodeData.BODY.EQUIPMENTRUNMODE);
        //        this.EQPRunMode = _newRunMode;
        //    }
        //    #endregion

        //    #region Bit
        //    if (NodeData.BODY.TURNTABLEMODE == "1" || NodeData.BODY.TURNTABLEMODE == "0")
        //        this.TurnTableMode = NodeData.BODY.TURNTABLEMODE == "1" ? eTurnTable.Enable : eTurnTable.Disable;
        //    else this.TurnTableMode = eTurnTable.Unused;
        //    this.RecipeIDCheckMode = NodeData.BODY.RECIPECHECKMODE;
        //    MaterialStatus = NodeData.BODY.MATERIALSTATUS == "1" ? true : false;  //0: OFF,1: ON

        //    #endregion

        //    #region List
        //    foreach (EquipmentStatusReport.UNITc _unit in NodeData.BODY.UNITLIST)
        //    {
        //        string _unitKey = NodeData.BODY.EQUIPMENTNO.PadRight(3, ' ') + _unit.UNITNO.PadLeft(2, '0');
        //        if (FormMainMDI.G_OPIAp.Dic_Unit.ContainsKey(_unitKey))
        //        {
        //            FormMainMDI.G_OPIAp.Dic_Unit[_unitKey].SetUnitInfo(_unit);
        //        }
        //    }

        //    foreach (EquipmentStatusReport.DCRc _vcr in NodeData.BODY.DCRLIST)
        //    {
        //        DCR vcr = this.VCRs.Find(d => d.DCRNO.Equals(_vcr.DCRNO.ToString().PadLeft(2,'0')));
        //        if (vcr == null) continue;
        //        vcr.Status = _vcr.DCRENABLEMODE==string.Empty ? eDCRMode.DISABLE: (eDCRMode)int.Parse(_vcr.DCRENABLEMODE);
        //    }

        //    foreach (EquipmentStatusReport.DISPATCHc _dispatch in NodeData.BODY.DISPATCHLIST)
        //    {
        //        Dispatch dispaych = this.Dispatches.Find(r => r.DISPATCHNO.Equals(_dispatch.DISPATCHNO.ToString().PadLeft(2, '0')));
        //        if (dispaych == null) continue;
        //        dispaych.Status = _dispatch.DISPATCHMODE == string.Empty ? eDispatchMode.Unused : (eDispatchMode)int.Parse(_dispatch.DISPATCHMODE);
        //    }

        //    foreach (EquipmentStatusReport.BUFFERc _buffer in NodeData.BODY.BUFFERLIST)
        //    {
        //        Buffer = GetBuffers(NodeData.BODY.EQUIPMENTNO.ToString(), Convert.ToInt32(NodeData.BODY.BUFFERLIST.Count));
        //        if (string.IsNullOrEmpty(_buffer.BUFFERNO)) continue;
        //        Buffer buffer = this.Buffer.Find(r => r.BUFFERNO.Equals(_buffer.BUFFERNO.ToString().PadLeft(2, '0')));
        //        if (buffer == null) continue;
        //        buffer.Status = _buffer.BF_STATUS == string.Empty ? eBufferStatus.Unused : (eBufferStatus)int.Parse(_buffer.BF_STATUS);
        //        buffer.BFGLASSCOUNT = _buffer.BF_GLASS_COUNT.ToString();
        //        buffer.DOWNCOUNT = _buffer.BF_DOWN_LEVEL_COUNT.ToString();
        //        buffer.UPCOUNT = _buffer.BF_UP_LEVEL_COUNT.ToString();
        //    }

        //    foreach (EquipmentStatusReport.STOPBITc _interlock in NodeData.BODY.STOPBITLIST)
        //    {
        //        InterLock interlock = this.InterLocks.Find(d => d.PLCTrxNo.Equals(_interlock.STOPBITNO.ToString().PadLeft(2, '0')));
        //        if (interlock == null) continue;
        //        interlock.Status = _interlock.STOPBITSTATUS==string.Empty ? eStopBitMode.OFF :(eStopBitMode)int.Parse(_interlock.STOPBITSTATUS);
        //    }

        //    foreach (EquipmentStatusReport.SAMPLINGSIDEc _side in NodeData.BODY.SAMPLINGSIDELIST)
        //    {
        //        SamplingSide samplingSize = this.SamplingSides.Find(d => d.ItemName.Equals(_side.ITEMNAME.ToString()));
        //        if (samplingSize == null) continue;
        //        samplingSize.SideStatus = _side.SIDESTATUS ;
        //    }
        //    #endregion
        //}
        //#endregion

        //#region Get Node Info -- EquipmentStatusReply
        //public void SetNodeInfo(EquipmentStatusReply NodeData)
        //{
        //    int _num = 0;
        //    #region Count
        //    this.UVMASKGlassCount = (int.TryParse(NodeData.BODY.UVMASKGLASSCNT, out _num) == true) ? int.Parse(NodeData.BODY.UVMASKGLASSCNT) : 0;
        //    this.ProductGlassCount = (int.TryParse(NodeData.BODY.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.BODY.PRODUCTGLASSCOUNT) : 0;
        //    this.TotalGlassCount = (int.TryParse(NodeData.BODY.TOTALGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.BODY.TOTALGLASSCOUNT) : 0;
        //    this.HistoryGlassCount = (int.TryParse(NodeData.BODY.HISTORYGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.BODY.HISTORYGLASSCOUNT) : 0;
        //    #endregion

        //    #region Word
        //    this.OperationMode = NodeData.BODY.AUTOMANUALMODE.ToString().Contains(eEQPOperationMode.AUTO.ToString()) ? eEQPOperationMode.AUTO : eEQPOperationMode.MANUAL;

        //    if (NodeData.BODY.AUTOCONTROLMODE == string.Empty && NodeData.BODY.AUTOCONTROLMODE != "Disable" && NodeData.BODY.AUTOCONTROLMODE != "Enable") this.AutoControlMode = eAutoControlMode.Unkown;
        //    else this.AutoControlMode = NodeData.BODY.AUTOCONTROLMODE == "Disable" ? eAutoControlMode.Disable : eAutoControlMode.Enable;

        //    this.CIMMode = (eCIMMode)int.Parse(NodeData.BODY.EQUIPMENTCIMMODE);
        //    this.RecipeName = NodeData.BODY.CURRENTRECIPEID;
        //    this.EQPStatus = NodeData.BODY.CURRENTSTATUS == string.Empty ? eEQPStatus.Unused : (eEQPStatus)int.Parse(NodeData.BODY.CURRENTSTATUS);
        //    this.EQPRunMode = NodeData.BODY.EQUIPMENTRUNMODE;
        //    this.AlarmStatus = (NodeData.BODY.LOCALALARMSTATUS == "ON" ? true : false);
        //    this.HSMSStatus = NodeData.BODY.HSMSSTATUS.ToString();
        //    this.HSMSControlMode = NodeData.BODY.HSMSCONTROLMODE.ToString();
        //    this.EquipmentAlive = NodeData.BODY.EQALIVE == "1" ? "Alive" : "Down";
        //    this.AutoRecipeChange = NodeData.BODY.AUTORECIPECHANGEMODE;
        //    this.ByPassMode = NodeData.BODY.PASSMODE;
        //    ForceClearout = NodeData.BODY.FORCECLEAROUT_MODE;
        //    this.LastReceiveGlassID = NodeData.BODY.LASTGLASSID == null ? string.Empty : NodeData.BODY.LASTGLASSID;
        //    this.LastReceiveDateTime = NodeData.BODY.LASTRECIVETIME == null ? string.Empty : NodeData.BODY.LASTRECIVETIME;
        //    this.GroupNumber = (int.TryParse(NodeData.BODY.GROUPNUMBER, out _num) == true) ? int.Parse(NodeData.BODY.GROUPNUMBER) : 0;
        //    this.AlarmCode = (int.TryParse(NodeData.BODY.ALARMCODE, out _num) == true) ? int.Parse(NodeData.BODY.ALARMCODE) : 0;
        //    this.BufferMode = NodeData.BODY.BUFFER_MODE == string.Empty ? eBufferMode.Unknown : (eBufferMode)int.Parse(NodeData.BODY.BUFFER_MODE);
        //    this.PrePutterMode = NodeData.BODY.PRE_SPUTTER_MODE == string.Empty ? ePreSputterMode.Unknown : (ePreSputterMode)int.Parse(NodeData.BODY.PRE_SPUTTER_MODE);  
        //    this.EQPIndexerRunMode = NodeData.BODY.EQUIPMENTINDEXERRUNMODE.ToString();
        //    if (NodeData.BODY.EQUIPMENTRUNMODE == string.Empty)
        //        this.EQPRunMode = string.Empty;
        //    else
        //    {
        //        string _newRunMode =GetRunModeDesc(NodeData.BODY.EQUIPMENTRUNMODE);
        //        this.EQPRunMode = _newRunMode;
        //    }
        //    #endregion

        //    #region Bit
        //    if (NodeData.BODY.TURNTABLEMODE == "1" || NodeData.BODY.TURNTABLEMODE == "0")
        //    this.TurnTableMode = NodeData.BODY.TURNTABLEMODE == "1" ? eTurnTable.Enable : eTurnTable.Disable;
        //    else this.TurnTableMode=eTurnTable.Unused;
        //    this.RecipeIDCheckMode = NodeData.BODY.RECIPECHECKMODE ;
        //    this.MaterialStatus = NodeData.BODY.MATERIALSTATUS == "1" ? true : false;  //0: OFF,1: ON
        //    #endregion
          
        //    #region List
        //    foreach (EquipmentStatusReply.UNITc _unit in NodeData.BODY.UNITLIST)
        //    {

        //        string _unitKey = NodeData.BODY.EQUIPMENTNO.PadRight(3, ' ') + _unit.UNITNO.PadLeft(2, '0');
        //        if (FormMainMDI.G_OPIAp.Dic_Unit.ContainsKey(_unitKey))
        //        {
        //            FormMainMDI.G_OPIAp.Dic_Unit[_unitKey].SetUnitInfo(_unit);
        //        }
        //    }

        //    foreach (EquipmentStatusReply.DCRc _vcr in NodeData.BODY.DCRLIST)
        //    {
        //        DCR vcr = this.VCRs.Find(d => d.DCRNO.Equals(_vcr.DCRNO.ToString().PadLeft(2, '0')));
        //        if (vcr == null) continue;
        //        vcr.Status = _vcr.DCRENABLEMODE==string.Empty ? eDCRMode.DISABLE : (eDCRMode)int.Parse(_vcr.DCRENABLEMODE);
        //    }

        //    foreach (EquipmentStatusReply.DISPATCHc _dispatch in NodeData.BODY.DISPATCHLIST)
        //    {
        //        Dispatch dispaych = this.Dispatches.Find(r => r.DISPATCHNO.Equals(_dispatch.DISPATCHNO.ToString().PadLeft(2, '0')));
        //        if (dispaych == null) continue;
        //        dispaych.Status = _dispatch.DISPATCHMODE == string.Empty ? eDispatchMode.Unused : (eDispatchMode)int.Parse(_dispatch.DISPATCHMODE);
        //    }

        //    foreach (EquipmentStatusReply.BUFFERc _buffer in NodeData.BODY.BUFFERLIST)
        //    {
        //        Buffer = GetBuffers(NodeData.BODY.EQUIPMENTNO.ToString(), Convert.ToInt32(NodeData.BODY.BUFFERLIST.Count));
        //        if (string.IsNullOrEmpty(_buffer.BUFFERNO)) continue;
        //        Buffer buffer = this.Buffer.Find(r => r.BUFFERNO.Equals(_buffer.BUFFERNO.ToString().PadLeft(2, '0')));
        //        if (buffer == null) continue;
        //        buffer.Status = _buffer.BF_STATUS == string.Empty ? eBufferStatus.Unused : (eBufferStatus)int.Parse(_buffer.BF_STATUS);
        //        buffer.BFGLASSCOUNT = _buffer.BF_GLASS_COUNT.ToString();
        //        buffer.DOWNCOUNT = _buffer.BF_DOWN_LEVEL_COUNT.ToString();
        //        buffer.UPCOUNT = _buffer.BF_UP_LEVEL_COUNT.ToString();

        //    }

        //    foreach (EquipmentStatusReply.STOPBITc _interlock in NodeData.BODY.STOPBITLIST)
        //    {
        //        InterLock interlock = this.InterLocks.Find(d => d.PLCTrxNo.Equals(_interlock.STOPBITNO.ToString().PadLeft(2, '0')));
        //        if (interlock == null) continue;
        //        interlock.Status = _interlock.STOPBITSTATUS==string.Empty ? eStopBitMode.OFF : (eStopBitMode)int.Parse(_interlock.STOPBITSTATUS);
        //    }

        //    foreach (EquipmentStatusReply.SAMPLINGSIDEc _side in NodeData.BODY.SAMPLINGSIDELIST)
        //    {
        //        SamplingSide samplingSize = this.SamplingSides.Find(d => d.ItemName.Equals(_side.ITEMNAME.ToString()));
        //        if (samplingSize == null) continue;
        //        samplingSize.SideStatus = _side.SIDESTATUS;
        //    }
        //    #endregion
        //}
        //#endregion

        //#region Get Node Info -- AllDataUpdateReply
        //public void SetNodeInfo(AllDataUpdateReply.EQUIPMENTc NodeData)
        //{
        //    int _num = 0;

        //    #region Count
        //    this.ProductGlassCount = (int.TryParse(NodeData.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.PRODUCTGLASSCOUNT) : 0;
        //    this.HistoryGlassCount = (int.TryParse(NodeData.HISTORYGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.HISTORYGLASSCOUNT) : 0;
        //    this.TotalGlassCount = (int.TryParse(NodeData.TOTALGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.TOTALGLASSCOUNT) : 0;
        //    this.UVMASKGlassCount = (int.TryParse(NodeData.UVMASKGLASSCNT, out _num) == true) ? int.Parse(NodeData.UVMASKGLASSCNT) : 0;
        //    #endregion

        //    #region Word
        //    this.OperationMode = NodeData.AUTOMANUALMODE.Contains(eEQPOperationMode.MANUAL.ToString())? eEQPOperationMode.MANUAL : eEQPOperationMode.AUTO;

        //    if (NodeData.AUTOCONTROLMODE == string.Empty && NodeData.AUTOCONTROLMODE != "Enable" && NodeData.AUTOCONTROLMODE != "Disable") this.AutoControlMode = eAutoControlMode.Unkown;
        //    else this.AutoControlMode = NodeData.AUTOCONTROLMODE == "Disable" ? eAutoControlMode.Disable : eAutoControlMode.Enable;

        //    this.CIMMode = (eCIMMode)int.Parse(NodeData.EQUIPMENTCIMMODE);
        //    this.BufferMode = NodeData.BUFFER_MODE == string.Empty ? eBufferMode.Unknown : (eBufferMode)int.Parse(NodeData.BUFFER_MODE);
        //    this.PrePutterMode = NodeData.PRE_SPUTTER_MODE == string.Empty ? ePreSputterMode.Unknown : (ePreSputterMode)int.Parse(NodeData.PRE_SPUTTER_MODE);
        //    this.RecipeName = NodeData.CURRENTRECIPEID;
        //    this.EQPStatus = NodeData.CURRENTSTATUS == string.Empty ? eEQPStatus.Unused : (eEQPStatus)int.Parse(NodeData.CURRENTSTATUS);
        //    this.AlarmStatus = NodeData.LOCALALARMSTATUS == "ON" ? true : false;
        //    this.HSMSStatus = NodeData.HSMSSTATUS.ToString();
        //    this.HSMSControlMode = NodeData.HSMSCONTROLMODE.ToString();
        //    this.EquipmentAlive = NodeData.EQALIVE == "1" ? "Alive" : "Down";
        //    this.AutoRecipeChange = NodeData.AUTORECIPECHANGEMODE;
        //    this.ByPassMode = NodeData.PASSMODE;
        //    ForceClearout = NodeData.FORCECLEAROUT_MODE;
        //    this.LastReceiveGlassID = NodeData.LASTGLASSID == null ? string.Empty : NodeData.LASTGLASSID;
        //    this.LastReceiveDateTime = NodeData.LASTRECIVETIME == null ? string.Empty : NodeData.LASTRECIVETIME;
        //    this.GroupNumber = (int.TryParse(NodeData.GROUPNUMBER, out _num) == true) ? int.Parse(NodeData.GROUPNUMBER) : 0;
        //    this.AlarmCode = (int.TryParse(NodeData.ALARMCODE, out _num) == true) ? int.Parse(NodeData.ALARMCODE) : 0;
        //    this.EQPRunMode = GetRunModeDesc(NodeData.EQUIPMENTRUNMODE);
        //    this.EQPIndexerRunMode = NodeData.EQUIPMENTINDEXERRUNMODE.ToString();

        //    #endregion

        //    #region Bit
        //    if (NodeData.TURNTABLEMODE == "1" || NodeData.TURNTABLEMODE == "0")
        //        this.TurnTableMode = NodeData.TURNTABLEMODE == "1" ? eTurnTable.Enable : eTurnTable.Disable;
        //    else this.TurnTableMode = eTurnTable.Unused;
        //    this.RecipeIDCheckMode = NodeData.RECIPECHECKMODE;
        //    this.MaterialStatus = NodeData.MATERIALSTATUS == "1" ? true : false;  //0: OFF,1: ON
        //    #endregion

        //    #region List
        //    foreach (AllDataUpdateReply.UNITc _unit in NodeData.UNITLIST)
        //    {
        //        string _unitKey = NodeData.EQUIPMENTNO.PadRight(3, ' ') + _unit.UNITNO.PadLeft(2, '0');
        //        if (FormMainMDI.G_OPIAp.Dic_Unit.ContainsKey(_unitKey))
        //        {
        //            FormMainMDI.G_OPIAp.Dic_Unit[_unitKey].SetUnitInfo(_unit);
        //        }
        //    }

        //    foreach (AllDataUpdateReply.PORTc _port in NodeData.PORTLIST)
        //    {
        //        int _portNo = 0;
        //        int.TryParse(_port.PORTNO, out _portNo);

        //        string _portKey = NodeData.EQUIPMENTNO.PadRight(3, ' ') + _portNo.ToString();
        //        if (FormMainMDI.G_OPIAp.Dic_Port.ContainsKey(_portKey))
        //        {
        //            FormMainMDI.G_OPIAp.Dic_Port[_portKey].SetPortInfo(_port);
        //        }
        //    }

        //    foreach (AllDataUpdateReply.DENSEBOXc _dense in NodeData.DENSEBOXLIST)
        //    {
        //        string _denseKey = NodeData.EQUIPMENTNO.PadRight(3, ' ') + _dense.PORTNO.PadRight(2, '0');
        //        if (FormMainMDI.G_OPIAp.Dic_Dense.ContainsKey(_denseKey))
        //        {
        //            FormMainMDI.G_OPIAp.Dic_Dense[_denseKey].SetDenseInfo(_dense);
        //        }
        //    }

        //    foreach (AllDataUpdateReply.DCRc _vcr in NodeData.DCRLIST)
        //    {
        //        DCR vcr = this.VCRs.Find(d => d.DCRNO.Equals(_vcr.DCRNO.ToString().PadLeft(2, '0')));
        //        if (vcr == null) continue;
        //        vcr.Status = _vcr.DCRENABLEMODE==string.Empty ? eDCRMode.DISABLE : (eDCRMode)int.Parse(_vcr.DCRENABLEMODE);
        //    }

        //    foreach (AllDataUpdateReply.BUFFERc _buffer in NodeData.BUFFERLIST)
        //    {
        //        if (string.IsNullOrEmpty(_buffer.BUFFERNO)) continue;
        //        Buffer buffer = this.Buffer.Find(r => r.BUFFERNO.Equals(_buffer.BUFFERNO.ToString().PadLeft(2, '0')));
        //        if (buffer == null) continue;
        //        buffer.Status = _buffer.BUFFERNO == string.Empty ? eBufferStatus.Unused : (eBufferStatus)int.Parse(_buffer.BUFFERNO);
        //    }

        //    foreach (AllDataUpdateReply.DISPATCHc _dispatch in NodeData.DISPATCHLIST)
        //    {
        //        Dispatch dispaych = this.Dispatches.Find(r => r.DISPATCHNO.Equals(_dispatch.DISPATCHNO.ToString().PadLeft(2, '0')));
        //        if (dispaych == null) continue;
        //        dispaych.Status = _dispatch.DISPATCHMODE == string.Empty ? eDispatchMode.Unused : (eDispatchMode)int.Parse(_dispatch.DISPATCHMODE);
        //    }

        //    foreach (AllDataUpdateReply.STOPBITc _interlock in NodeData.STOPBITLIST)
        //    {
        //        InterLock interlock = this.InterLocks.Find(d => d.PLCTrxNo.Equals(_interlock.STOPBITNO.ToString().PadLeft(2, '0')));
        //        if (interlock == null) continue;
        //        interlock.Status =_interlock.STOPBITSTATUS==string.Empty ? eStopBitMode.OFF: (eStopBitMode)int.Parse(_interlock.STOPBITSTATUS);
        //    }

        //    foreach (AllDataUpdateReply.SAMPLINGSIDEc _side in NodeData.SAMPLINGSIDELIST)
        //    {
        //        SamplingSide _samplingSize = this.SamplingSides.Find(d => d.ItemName.Equals(_side.ITEMNAME.ToString()));
        //        if (_samplingSize == null) continue;
        //        _samplingSize.SideStatus = _side.SIDESTATUS;
        //    }
        //    #endregion
        //}
        //#endregion

        //#region Get Node Info -- AllEquipmentStatusReply
        //public void SetNodeInfo(AllEquipmentStatusReply.EQUIPMENTc NodeData)
        //{
        //    int _num = 0;

        //    #region Count
        //    this.TotalGlassCount = (int.TryParse(NodeData.TOTALGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.TOTALGLASSCOUNT) : 0;
        //    this.HistoryGlassCount = (int.TryParse(NodeData.HISTORYGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.HISTORYGLASSCOUNT) : 0;
        //    this.ProductGlassCount = (int.TryParse(NodeData.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.PRODUCTGLASSCOUNT) : 0;
        //    this.UVMASKGlassCount = (int.TryParse(NodeData.UVMASKGLASSCNT, out _num) == true) ? int.Parse(NodeData.UVMASKGLASSCNT) : 0;
        //    #endregion

        //    #region Word
        //    this.OperationMode =NodeData.AUTOMANUALMODE.Contains(eEQPOperationMode.MANUAL.ToString())?eEQPOperationMode.MANUAL: eEQPOperationMode.AUTO;

        //    if (NodeData.AUTOCONTROLMODE == string.Empty && NodeData.AUTOCONTROLMODE != "Disable" && NodeData.AUTOCONTROLMODE != "Enable") this.AutoControlMode = eAutoControlMode.Unkown;
        //    else this.AutoControlMode = NodeData.AUTOCONTROLMODE == "Disable" ? eAutoControlMode.Disable : eAutoControlMode.Enable;
            
        //    this.CIMMode = (eCIMMode)int.Parse(NodeData.EQUIPMENTCIMMODE);
        //    this.BufferMode = NodeData.BUFFER_MODE == string.Empty ? eBufferMode.Unknown : (eBufferMode)int.Parse(NodeData.BUFFER_MODE);
        //    this.PrePutterMode = NodeData.PRE_SPUTTER_MODE == string.Empty ? ePreSputterMode.Unknown : (ePreSputterMode)int.Parse(NodeData.PRE_SPUTTER_MODE);
        //    this.RecipeName = NodeData.CURRENTRECIPEID;
        //    this.EQPStatus =NodeData.CURRENTSTATUS==string.Empty ? eEQPStatus.Unused: (eEQPStatus)int.Parse(NodeData.CURRENTSTATUS);
        //    this.AlarmStatus = (NodeData.LOCALALARMSTATUS == "ON" ? true : false);
        //    this.HSMSStatus = NodeData.HSMSSTATUS.ToString();
        //    this.HSMSControlMode = NodeData.HSMSCONTROLMODE.ToString();
        //    this.EquipmentAlive = NodeData.EQALIVE == "1" ? "Alive" : "Down";
        //    this.AutoRecipeChange = NodeData.AUTORECIPECHANGEMODE;
        //    this.ByPassMode = NodeData.PASSMODE;
        //    ForceClearout = NodeData.FORCECLEAROUT_MODE;
        //    this.LastReceiveGlassID = NodeData.LASTGLASSID == null ? string.Empty : NodeData.LASTGLASSID;
        //    this.LastReceiveDateTime = NodeData.LASTRECIVETIME == null ? string.Empty : NodeData.LASTRECIVETIME;
        //    this.GroupNumber = (int.TryParse(NodeData.GROUPNUMBER, out _num) == true) ? int.Parse(NodeData.GROUPNUMBER) : 0;
        //    this.AlarmCode = (int.TryParse(NodeData.ALARMCODE, out _num) == true) ? int.Parse(NodeData.ALARMCODE) : 0;
        //    this.EQPRunMode = GetRunModeDesc(NodeData.EQUIPMENTRUNMODE);
        //    this.EQPIndexerRunMode = NodeData.EQUIPMENTINDEXERRUNMODE.ToString();
     
        //    #endregion

        //    #region Bit
        //    if (NodeData.TURNTABLEMODE == "1" || NodeData.TURNTABLEMODE == "0")
        //        this.TurnTableMode = NodeData.TURNTABLEMODE == "1" ? eTurnTable.Enable : eTurnTable.Disable;
        //    else this.TurnTableMode = eTurnTable.Unused;
        //    this.RecipeIDCheckMode = NodeData.RECIPECHECKMODE;
        //    MaterialStatus = NodeData.MATERIALSTATUS == "1" ? true : false;  //0: OFF,1: ON
        //    #endregion

        //    #region List
        //    foreach (AllEquipmentStatusReply.DCRc _vcr in NodeData.DCRLIST)
        //    {
        //        DCR vcr = this.VCRs.Find(d => d.DCRNO.Equals(_vcr.DCRNO.ToString().PadLeft(2, '0')));
        //        if (vcr == null) continue;
        //        vcr.Status = _vcr.DCRENABLEMODE==string.Empty ? eDCRMode.DISABLE:(eDCRMode)int.Parse(_vcr.DCRENABLEMODE);
        //    }

        //    foreach (AllEquipmentStatusReply.BUFFERc _buffer in NodeData.BUFFERLIST)
        //    {
        //        Buffer = GetBuffers(NodeData.EQUIPMENTNO.ToString(), Convert.ToInt32(NodeData.BUFFERLIST.Count));
        //        if (string.IsNullOrEmpty(_buffer.BUFFERNO)) continue;
        //        Buffer buffer = this.Buffer.Find(r => r.BUFFERNO.Equals(_buffer.BUFFERNO.ToString().PadLeft(2, '0')));
        //        if (buffer == null) continue;
        //        buffer.Status = _buffer.BF_STATUS == string.Empty ? eBufferStatus.Unused : (eBufferStatus)int.Parse(_buffer.BF_STATUS);
        //        buffer.BFGLASSCOUNT = _buffer.BF_GLASS_COUNT.ToString();
        //        buffer.DOWNCOUNT = _buffer.BF_DOWN_LEVEL_COUNT.ToString();
        //        buffer.UPCOUNT = _buffer.BF_UP_LEVEL_COUNT.ToString();

        //    }

        //    foreach (AllEquipmentStatusReply.DISPATCHc _dispatch in NodeData.DISPATCHLIST)
        //    {

        //        Dispatch dispaych = this.Dispatches.Find(r => r.DISPATCHNO.Equals(_dispatch.DISPATCHNO.ToString().PadLeft(2, '0')));
        //        if (dispaych == null) continue;
        //        dispaych.Status = _dispatch.DISPATCHMODE == string.Empty ? eDispatchMode.Unused : (eDispatchMode)int.Parse(_dispatch.DISPATCHMODE);
        //    }

        //    foreach (AllEquipmentStatusReply.STOPBITc _interlock in NodeData.STOPBITLIST)
        //    {
        //        InterLock interlock = this.InterLocks.Find(d => d.PLCTrxNo.Equals(_interlock.STOPBITNO.ToString().PadLeft(2, '0')));
        //        if (interlock == null) continue;
        //        interlock.Status = _interlock.STOPBITSTATUS==string.Empty ? eStopBitMode.OFF : (eStopBitMode)int.Parse(_interlock.STOPBITSTATUS);
        //    }

        //    foreach (AllEquipmentStatusReply.SAMPLINGSIDEc _side in NodeData.SAMPLINGSIDELIST)
        //    {
        //        SamplingSide samplingSize = this.SamplingSides.Find(d => d.ItemName.Equals(_side.ITEMNAME.ToString()));
        //        if (samplingSize == null) continue;
        //        samplingSize.SideStatus = _side.SIDESTATUS ;
        //    }
        //    #endregion
        //}
        //#endregion

        //#region Get Node Info -- EquipmentInformationReport
        //public void SetNodeInfo(EquipmentInformationReport.EQUIPMENTc NodeData)
        //{
        //    int _num = 0;

        //    #region Count
        //    this.TotalGlassCount = (int.TryParse(NodeData.TOTALGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.TOTALGLASSCOUNT) : 0;
        //    this.HistoryGlassCount = (int.TryParse(NodeData.HISTORYGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.HISTORYGLASSCOUNT) : 0;
        //    this.ProductGlassCount = (int.TryParse(NodeData.PRODUCTGLASSCOUNT, out _num) == true) ? int.Parse(NodeData.PRODUCTGLASSCOUNT) : 0;
        //    this.UVMASKGlassCount = (int.TryParse(NodeData.UVMASKGLASSCNT, out _num) == true) ? int.Parse(NodeData.UVMASKGLASSCNT) : 0;
           
        //    #endregion

        //    #region Word
        //    this.OperationMode = NodeData.AUTOMANUALMODE.Contains(eEQPOperationMode.MANUAL.ToString()) ? eEQPOperationMode.MANUAL : eEQPOperationMode.AUTO;

        //    if (NodeData.AUTOCONTROLMODE == string.Empty && NodeData.AUTOCONTROLMODE != "Disable" && NodeData.AUTOCONTROLMODE != "Enable") this.AutoControlMode = eAutoControlMode.Unkown;
        //    else this.AutoControlMode = NodeData.AUTOCONTROLMODE == "Disable" ? eAutoControlMode.Disable : eAutoControlMode.Enable;

        //    this.CIMMode = (eCIMMode)int.Parse(NodeData.EQUIPMENTCIMMODE);
        //    this.BufferMode = NodeData.BUFFER_MODE == string.Empty ? eBufferMode.Unknown : (eBufferMode)int.Parse(NodeData.BUFFER_MODE);
        //    this.PrePutterMode = NodeData.PRE_SPUTTER_MODE == string.Empty ? ePreSputterMode.Unknown : (ePreSputterMode)int.Parse(NodeData.PRE_SPUTTER_MODE);
        //    this.RecipeName = NodeData.CURRENTRECIPEID;
        //    this.EQPStatus = NodeData.CURRENTSTATUS == string.Empty ? eEQPStatus.Unused : (eEQPStatus)int.Parse(NodeData.CURRENTSTATUS);
           
        //    this.AlarmStatus = (NodeData.LOCALALARMSTATUS == "ON" ? true : false);
            
        //    this.HSMSStatus = NodeData.HSMSSTATUS.ToString();
        //    this.HSMSControlMode = NodeData.HSMSCONTROLMODE.ToString();
        //    this.EquipmentAlive = NodeData.EQALIVE == "1" ? "Alive" : "Down";
           
        //    this.AutoRecipeChange = NodeData.AUTORECIPECHANGEMODE;
        //    this.ByPassMode = NodeData.PASSMODE;
        //    ForceClearout = NodeData.FORCECLEAROUT_MODE;
            
        //    this.LastReceiveGlassID = NodeData.LASTGLASSID == null ? string.Empty : NodeData.LASTGLASSID;
        //    this.LastReceiveDateTime = NodeData.LASTRECIVETIME == null ? string.Empty : NodeData.LASTRECIVETIME;
        //    this.GroupNumber = (int.TryParse(NodeData.GROUPNUMBER, out _num) == true) ? int.Parse(NodeData.GROUPNUMBER) : 0;
        //    this.AlarmCode = (int.TryParse(NodeData.ALARMCODE, out _num) == true) ? int.Parse(NodeData.ALARMCODE) : 0;
        //    this.EQPRunMode = GetRunModeDesc(NodeData.EQUIPMENTRUNMODE);
        //    this.EQPIndexerRunMode = NodeData.EQUIPMENTINDEXERRUNMODE.ToString();
        //    #endregion

        //    #region Bit
        //    if (NodeData.TURNTABLEMODE == "1" || NodeData.TURNTABLEMODE == "0")
        //        this.TurnTableMode = NodeData.TURNTABLEMODE == "1" ? eTurnTable.Enable : eTurnTable.Disable;
        //    else this.TurnTableMode = eTurnTable.Unused;
           
        //    this.RecipeIDCheckMode = NodeData.RECIPECHECKMODE;
          
        //    MaterialStatus = NodeData.MATERIALSTATUS == "1" ? true : false;  //0: OFF,1: ON
            
        //    #endregion

        //    #region List
        //    foreach (EquipmentInformationReport.DCRc _vcr in NodeData.DCRLIST)
        //    {
        //        DCR vcr = this.VCRs.Find(d => d.DCRNO.Equals(_vcr.DCRNO.ToString().PadLeft(2, '0')));
        //        if (vcr == null) continue;
        //        vcr.Status = _vcr.DCRENABLEMODE == string.Empty ? eDCRMode.DISABLE : (eDCRMode)int.Parse(_vcr.DCRENABLEMODE);
        //    }

        //    foreach (EquipmentInformationReport.BUFFERc _buffer in NodeData.BUFFERLIST)
        //    {
        //        Buffer = GetBuffers(NodeData.EQUIPMENTNO.ToString(), Convert.ToInt32(NodeData.BUFFERLIST.Count));
        //        if (string.IsNullOrEmpty(_buffer.BUFFERNO)) continue;
        //        Buffer buffer = this.Buffer.Find(r => r.BUFFERNO.Equals(_buffer.BUFFERNO.ToString().PadLeft(2, '0')));
        //        if (buffer == null) continue;
        //        buffer.Status = _buffer.BF_STATUS == string.Empty ? eBufferStatus.Unused : (eBufferStatus)int.Parse(_buffer.BF_STATUS);
        //        buffer.BFGLASSCOUNT = _buffer.BF_GLASS_COUNT.ToString();
        //        buffer.DOWNCOUNT = _buffer.BF_DOWN_LEVEL_COUNT.ToString();
        //        buffer.UPCOUNT = _buffer.BF_UP_LEVEL_COUNT.ToString();

        //    }

        //    foreach (EquipmentInformationReport.DISPATCHc _dispatch in NodeData.DISPATCHLIST)
        //    {

        //        Dispatch dispaych = this.Dispatches.Find(r => r.DISPATCHNO.Equals(_dispatch.DISPATCHNO.ToString().PadLeft(2, '0')));
        //        if (dispaych == null) continue;
        //        dispaych.Status = _dispatch.DISPATCHMODE == string.Empty ? eDispatchMode.Unused : (eDispatchMode)int.Parse(_dispatch.DISPATCHMODE);
        //    }

        //    foreach (EquipmentInformationReport.STOPBITc _interlock in NodeData.STOPBITLIST)
        //    {
        //        InterLock interlock = this.InterLocks.Find(d => d.PLCTrxNo.Equals(_interlock.STOPBITNO.ToString().PadLeft(2, '0')));
        //        if (interlock == null) continue;
        //        interlock.Status = _interlock.STOPBITSTATUS == string.Empty ? eStopBitMode.OFF : (eStopBitMode)int.Parse(_interlock.STOPBITSTATUS);
        //    }

        //    foreach (EquipmentInformationReport.SAMPLINGSIDEc _side in NodeData.SAMPLINGSIDELIST)
        //    {
        //        SamplingSide samplingSize = this.SamplingSides.Find(d => d.ItemName.Equals(_side.ITEMNAME.ToString()));
        //        if (samplingSize == null) continue;
        //        samplingSize.SideStatus = _side.SIDESTATUS;
        //    }
        //    #endregion
        //}
        //#endregion

        #endregion
        #region 添加Buffer No
        private List<Buffer> GetBuffers(string NodeNo, int _bufferCount)
        {
            List<Buffer> lstBufferNode = new List<Buffer>();
            for (int i = 1; i <= _bufferCount; i++)
            {
                Buffer buffer = new Buffer();

                buffer.BUFFERNO = i.ToString().PadLeft(2, '0');

                lstBufferNode.Add(buffer);
            }

            return lstBufferNode;
        }
        #endregion

        #region Get RunMode Desc
        public string GetRunModeDesc(string RunMode)
        {
            int _num = 0;

            //若傳送值為數值型態，否則則當為傳送run mode描述 --2015-03-12 by 昌爺
            bool _isNum = int.TryParse(RunMode, out _num);

            if (_isNum)
            {
                if (_num == 0) return string.Format("{0}-UnKnown", RunMode);

                LineRunMode _runMode =LineRunModes.Find(r => r.RunModeNo.Equals(_num.ToString()));

                if (_runMode == null) return string.Format("{0}-UnKnown", RunMode);

                return _runMode.RunModeDesc;
            }
            else 
            { 
                return RunMode;
            }            
        }
        #endregion
    } 
    
    public class SamplingSide
    {
        private string itemName = string.Empty;
        private string sideSataus = string.Empty ;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public string SideStatus
        {
            get { return sideSataus; }
            set { sideSataus = value == "1" ? "1:Enable" : "0:Disable" ; }
        }
       
    }
    public class IndexerRobotStage
    {
        private eRobotOperationMode operationMode = eRobotOperationMode.UnKnown;
        private string robotPosNo = string.Empty;
        private string direction = string.Empty;
        private string desc = string.Empty;
        private string localNo = string.Empty;
        private string localID = string.Empty;

        public string LocalNo
        {
            get { return localNo; }
            set { localNo = value; }
        }

        public string LocalID
        {
            get { return localID; }
            set { localID = value; }
        }

        public string RobotPosNo
        {
            get { return robotPosNo; }
            set { robotPosNo = value; }
        }

        //1：Received, 2：Sent,3：Both
        public string Direction
        {
            get { return direction; }
            set 
            { 
                switch (value)
                {
                    case "1": direction = "1:Received"; break;
                    case "2": direction = "2:Sent"; break;
                    case "3": direction = "3:Both"; break;
                    default: direction = "Unknown";  break;
                }
            }
        }

        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }

        public eRobotOperationMode OperationMode
        {
            get { return operationMode; }
            set { operationMode = value; }
        }
    }
    public class DCR
    {
        private string dcrNO = string.Empty;
        public string DCRNO
        {
            get { return dcrNO; }
            set { dcrNO = value; }
        }
        public string  Status { get; set; }
      
    }

    public class Buffer
    {
        
        private string bufferNo = string.Empty;
        private string bufferGlassCount = string.Empty;
        private string bufferUpLevelCount = string.Empty;
        private string bufferDownLevelCount = string.Empty;
        public string BUFFERNO
        {
            get { return bufferNo; }
            set { bufferNo = value; }
        }

        public string BFGLASSCOUNT
        {
            get { return bufferGlassCount; }
            set { bufferGlassCount = value; }
        }

        public string UPCOUNT
        {
            get { return bufferUpLevelCount; }
            set { bufferUpLevelCount = value; }
        }

        public string DOWNCOUNT
        {
            get { return bufferDownLevelCount; }
            set { bufferDownLevelCount = value; }
        }

        public eBufferStatus Status { get; set; }
       
    }

    public class Dispatch
    {
        private string dispatch = string.Empty;
        public string DISPATCHNO
        {
            get { return dispatch; }
            set { dispatch = value; }
        }
        public eDispatchMode Status { get; set; }
    }
    public class InterLock
    {
        private string plcTrxNo = string.Empty;
        
        private string plcDescription = string.Empty;

        public string PLCTrxNo
        {
            get { return plcTrxNo.PadLeft(2,'0'); }
            set { plcTrxNo = value.PadLeft(2, '0'); }
            //防止人員在DB只輸入一碼,故補位2碼 add by sy.wu
        }
     
        public string Description
        {
            get { return plcDescription; }
            set { plcDescription = value; }
        }
        public eStopBitMode Status { get; set; }

    }
}
