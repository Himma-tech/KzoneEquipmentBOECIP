/*****************************************************
 * 20141128  Add Property CreateTime lastUpdateTime For Job Create  and Update  tom 
 * 
 * 
 * 
 * 
 * 
 * ***************************************************/




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KZONE.ConstantParameter;
using System.ComponentModel;

namespace KZONE.Entity
{
    /// <summary>
    /// 對應File, 修改Property後呼叫Save(), 會序列化存檔
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Job : EntityFile, ICloneable
    {
        #region BC Variable

        private string _jobKey = string.Empty;
        private string _vcrJobID = string.Empty;
        private eGLASSIDREAD_RESULT _vCR_Result = eGLASSIDREAD_RESULT.UNUSED;
        private string _sourcePortID = string.Empty;
        private string _targetPortID = string.Empty;
        private string _sourceCassetteID = string.Empty;
        private string _sourceSlotNo = string.Empty;
        private bool _removethelastFlag = false; //機台做Remove動作，而且是LastFlag的wip
        private string _currentEQPNo = string.Empty;
        private int _currentUnitNo = 0;
        private string _lineRecipeName = string.Empty;
        private DateTime _jobProcessStartTime = DateTime.Now; //Job Service
        //private List<HoldInfo> _holdInforList = new List<HoldInfo>();
        private string _chamberName = string.Empty;
        private DateTime _createTime = DateTime.Now;
        private string _lastFlag = "N";
        private bool _removeFlag = false; //機台做Remove動作時，不能真得刪除wip，只能更新此flag
        private string _removeReason = string.Empty;
        private int _reworkCount = 0;
        //private List<BatchInformation> _batchInfos = new List<BatchInformation>();
        //  private List<Qtime> _qtimeList = new List<Qtime>();
        private string _toSlotNo = string.Empty;//logoff 时填写的SlotNo
        private string _oxInformation = string.Empty;
        private string _panelJudge = string.Empty;
        private string _panelGrade = string.Empty;

        private DateTime _lastUpdateTime = DateTime.Now;
        private string _planId = string.Empty; //2016-11-14  给Changer Mode 使用
        private eHostMode _createHostMode = eHostMode.OFFLINE;
        private string _coolRunInspectionReservalitionSignal = string.Empty;
        public string prositionAB { get; set; }

        private bool _isReportQtimer = false;

        public DateTime ProStratTime { get; set; }

        public DateTime ProEndTime { get; set; }

        public ProcessFlow processFlow { get; set; }

        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }


        [Category("BC")]
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set { _lastUpdateTime = value; }
        }


        [ReadOnly(true)]
        [Category("BC")]
        public string JobKey
        {
            get { return _jobKey; }
            set { _jobKey = value; }
        }
        [Category("BC")]
        public string VCRJobID
        {
            get { return _vcrJobID; }
            set { _vcrJobID = value; }
        }

        public eGLASSIDREAD_RESULT VCR_Result
        {
            get { return _vCR_Result; }
            set { _vCR_Result = value; }
        }

        [Category("BC")]
        public string SourcePortID
        {
            get { return _sourcePortID; }
            set { _sourcePortID = value; }
        }
        [Category("BC")]
        public string TargetPortID
        {
            get { return _targetPortID; }
            set { _targetPortID = value; }
        }
        [Category("BC")]
        public string SourceCstID
        {
            get { return _sourceCassetteID; }
            set { _sourceCassetteID = value; }
        }

        [Category("BC")]
        public string SourceSlotNo
        {
            get { return _sourceSlotNo; }
            set { _sourceSlotNo = value; }
        }

        [Category("BC")]
        public string CurrentEQPNo
        {
            get { return _currentEQPNo; }
            set { _currentEQPNo = value; }
        }

        [Category("BC")]
        public int CurrentUnitNo
        {
            get { return _currentUnitNo; }
            set { _currentUnitNo = value; }
        }
        // [Category("BC")]
        //public List<BatchInformation> BatchInfos {
        //    get { return _batchInfos; }
        //    set { _batchInfos = value; }
        //}
        /// <summary>
        /// 機台做Remove動作時，不能真得刪除wip，只能更新此flag
        /// 可能在Regesiter回來
        /// </summary>
        [Category("BC")]
        public bool RemoveFlag
        {
            get { return _removeFlag; }
            set { _removeFlag = value; }
        }

        /// <summary>
        /// Remove Reason
        /// </summary>
        [Category("BC")]
        public string RemoveReason
        {
            get { return _removeReason; }
            set { _removeReason = value; }
        }

        /// <summary>
        /// 機台做Remove動作時，不能真得刪除wip，只能更新此flag
        /// 可能在Regesiter回來
        /// </summary>
        [Category("BC")]
        public bool RemoveandtheLastFlag
        {
            get { return _removethelastFlag; }
            set { _removethelastFlag = value; }
        }
        /// <summary>
        /// Job  Process Start  Time 
        ///  从Port 中报Fetch 是更新
        /// </summary>
        [Category("BC")]
        public DateTime JobProcessStartTime
        {
            get { return _jobProcessStartTime; } //Job Service
            set { _jobProcessStartTime = value; }
        }

        /// <summary>
        /// 目前BCS正在使用的LineRecipeName
        /// </summary>
        [Category("BC")]
        public string LineRecipeName
        {
            get { return _lineRecipeName; }
            set { _lineRecipeName = value; }
        }

        /// <summary>
        /// Last Flag
        /// </summary>
        [Category("BC")]
        public string LastFlag
        {
            get { return _lastFlag; }
            set { _lastFlag = value; }
        }

        [Category("BC")]
        public int ReworkCount
        {
            get { return _reworkCount; }
            set { _reworkCount = value; }
        }
        [Category("BC")]
        public string ToSlotNo
        {
            get { return _toSlotNo; }
            set { _toSlotNo = value; }
        }
        [Category("BC")]
        public string PlanId
        {
            get { return _planId; }
            set { _planId = value; }
        }

        [Category("BC")]
        public eHostMode CreateHostMode
        {
            get { return _createHostMode; }
            set { _createHostMode = value; }
        }
        [Category("BC")]
        public int ISequenceNo
        {
            get
            {
                return _cassetteSequenceNo;
            }
        }
        [Category("BC")]
        public int IJobSequenceNo
        {
            get
            {
                return _jobSequenceNo;
            }
        }
        [Category("BC")]
        public string CoolRunInspectionReservalitionSignal
        {
            get { return _coolRunInspectionReservalitionSignal; }
            set { _coolRunInspectionReservalitionSignal = value; }
        }
        /// <summary>
        ///判断是否有Report Qtimer 给MES  
        /// </summary>
        [Category("BC")]
        public bool IsReportQtimer
        {
            get { return _isReportQtimer; }
            set { _isReportQtimer = value; }
        }
        #endregion

        public RobotJobFile RobotJobFile { get; set; }

        #region Other

        #endregion


        #region JOB DATA 

        public string PrositonNo { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        private int _cassetteSequenceNo;
        private int _jobSequenceNo;
        private string _jobId;
        private eJobType _jobType = eJobType.Unknown;

        [Category("PLC")]
        public string JobId
        {
            get { return _jobId; }
            set { _jobId = value; }
        }
        [Category("PLC")]
        public int CassetteSequenceNo
        {
            get { return _cassetteSequenceNo; }
            set { _cassetteSequenceNo = value; }
        }
        [Category("PLC")]
        public int JobSequenceNo
        {
            get { return _jobSequenceNo; }
            set { _jobSequenceNo = value; }
        }

      
        public string GroupNumber
        {
            get;
            set;
        }
        public string GlassType
        {
            get;
            set;
        }
        public string GlassJudge
        {
            get;
            set;
        }

        public string ProcessSkipFlag
        {
            get;
            set;
        }
        public string LastGlassFlag
        {
            get;
            set;
        }
        public string CIMModeCreate
        {
            get;
            set;
        }
        public string SamplingFlag
        {
            get;
            set;
        }
       
        public string InspectionJudgeResult
        {
            get;
            set;
        }
        public string InspectionReservationSignal
        {
            get;
            set;
        }
        public string ProcessReservationSignal
        {
            get;
            set;
        }
        public string TrackingDataHistory
        {
            get;
            set;
        }
        public string EquipmentSpecialFlag
        {
            get;
            set;
        }
        public string GlassID
        {
            get;
            set;
        }
        public string SorterGrade
        {
            get;
            set;
        }
        public string GlassGrade
        {
            get;
            set;
        }
        public string FromPortNo
        {
            get;
            set;
        }
        public string TargetPortNo
        {
            get;
            set;
        }
        public string TargetSlotNo
        {
            get;
            set;
        }
        public string TargetCassetteID
        {
            get;
            set;
        }
        public string Reserve
        {
            get;
            set;
        }
        public string PPID
        {
            get;
            set;
        }


        //22003,22004 Extend
      
        public string ChangerFlag
        {
            get;
            set;
        }
        #endregion

        #region new 

            public string PRODID { get; set; }
            public string OperID { get; set; }
            public string LotID { get; set; }
            public string PPID1 { get; set; }
            public string PPID2 { get; set; }
            public string PPID3 { get; set; }
            public string PPID4 { get; set; }
            public string PPID5 { get; set; }
            public string PPID6 { get; set; }
            public string PPID7 { get; set; }
            public string PPID8 { get; set; }
            public string PPID9 { get; set; }
            public string PPID10 { get; set; }
            public string PPID11 { get; set; }
            public string PPID12 { get; set; }
            public string PPID13 { get; set; }
            public string PPID14 { get; set; }
            public string PPID15 { get; set; }
            public string PPID16 { get; set; }
            public string PPID17 { get; set; }
            public string PPID18 { get; set; }
            public string PPID19 { get; set; }
            public string PPID20 { get; set; }
            public string PPID21 { get; set; }
            public string PPID22 { get; set; }
            public string PPID23 { get; set; }
            public string PPID24 { get; set; }
            public string PPID25 { get; set; }
            public string PPID26 { get; set; }
            public string PPID27 { get; set; }
            public string PPID28 { get; set; }
            public string PPID29 { get; set; }
            public string PPID30 { get; set; }
            public string PPID31 { get; set; }
            public string PPID32 { get; set; }
            public string PPID33 { get; set; }
            public string PPID34 { get; set; }
            public string PPID35 { get; set; }
            public string PPID36 { get; set; }
            public string PPID37 { get; set; }
            public string PPID38 { get; set; }
            public string PPID39 { get; set; }
            public string PPID40 { get; set; }
            public string JobType { get; set; }
            public string JobID { get; set; }
            public string LotSequenceNumber { get; set; }
            public string SlotSequenceNumber { get; set; }
            public string PropertyCode { get; set; }
            public string JobJudgeCode { get; set; }
            public string JobGradeCode { get; set; }
            public string SubstrateType { get; set; }
            public string ProcessingFlagMachineLocalNo { get; set; }
            public string InspectionFlagMachineLocalNo { get; set; }
            public string SkipFlagMachineLocalNo { get; set; }
            public string JobSize { get; set; }
            public string GlassThickness { get; set; }
            public string JobAngle { get; set; }
            public string JobFlip { get; set; }
            public string ProcessingCount { get; set; }
            public string InspectionJudgeData1 { get; set; }
            public string InspectionJudgeData2 { get; set; }
            public string InspectionJudgeData3 { get; set; }
            public string InspectionJudgeDataNotUsed1 { get; set; }
            public string InspectionJudgeData4 { get; set; }
            public string InspectionJudgeData5 { get; set; }
            public string InspectionJudgeData6 { get; set; }
            public string InspectionJudgeDataNotUsed2 { get; set; }
            public string InspectionJudgeData7 { get; set; }
            public string InspectionJudgeData8 { get; set; }
            public string InspectionJudgeData9 { get; set; }
            public string InspectionJudgeDataNotUsed3 { get; set; }
            public string RepairMode { get; set; }
            public string JobIDPair { get; set; }
            public string PairLotSequenceNumber { get; set; }
            public string PairSlotSequenceNumber { get; set; }
            public string JobJudgeCodePair { get; set; }
            public string MMGCode { get; set; }
            public string PanelInchSizeX { get; set; }
            public string PanelInchSizeY { get; set; }
            public string PanelGradeCode { get; set; }
            public string SpecialFlag { get; set; }
            public string ProductionType { get; set; }
            public string PhotoMode { get; set; }
            public string LineCode { get; set; }
            public string AbnormalFlag { get; set; }
            public string NGMark { get; set; }
            public string CellProcessingFlag { get; set; }
            public string CellFlag { get; set; }
            public string OptionValue { get; set; }
            public string Reserved { get; set; }
        

        #endregion


        #region MES

        #endregion

        #region Shop Special Job Data 

        #endregion

        public Job()
        {
            _createTime = _jobProcessStartTime = DateTime.Now;

        }

        public Job(int _cstSeqNo, int _slotNo)
        {
            SetNewJobKey(_cstSeqNo, _slotNo);
        }

        public void SetNewJobKey(int _cstSeqNo, int _slotNo)
        {
            _cassetteSequenceNo = _cstSeqNo;
            _jobSequenceNo = _slotNo;
            _jobKey = _cassetteSequenceNo + "_" + JobSequenceNo;
            _filename = string.Format("{0}_{1}.bin", _cassetteSequenceNo, _jobSequenceNo);
            _createTime = _jobProcessStartTime = DateTime.Now;
        }

        /// <summary>
        /// Clone  Function
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Job job = (Job)this.MemberwiseClone();

            return job;
        }
    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class HoldInfo : ICloneable
    {
        private string _nodeNo = "";

        public string NodeNo
        {
            get { return _nodeNo; }
            set { _nodeNo = value; }
        }
        private string _nodeId = "";

        public string NodeID
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }
        private string _unitNo = "";

        public string UnitNo
        {
            get { return _unitNo; }
            set { _unitNo = value; }
        }
        private string _unitId = "";

        public string UnitID
        {
            get { return _unitId; }
            set { _unitId = value; }
        }
        private string _operatorId = "";

        public string OperatorID
        {
            get { return _operatorId; }
            set { _operatorId = value; }
        }

        private string _holdReason = "";

        public string HoldReason
        {
            get { return _holdReason; }
            set { _holdReason = value; }
        }

        public object Clone()
        {
            HoldInfo holdInfo = (HoldInfo)this.MemberwiseClone();
            return holdInfo;
        }
    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ProcessFlow : ICloneable
    {
        private string _machineName;
        private DateTime _startTime = DateTime.MinValue;
        private DateTime _endTime = DateTime.MinValue;
        private string _slotNo;

        private SerializableDictionary<string, ProcessFlow> _unitProcessFlows = new SerializableDictionary<string, ProcessFlow>();

        private List<ProcessFlow> _extendUnitProcessFlows = new List<ProcessFlow>();

        public SerializableDictionary<string, ProcessFlow> UnitProcessFlows
        {
            get { return _unitProcessFlows; }
            set { _unitProcessFlows = value; }
        }

        public List<ProcessFlow> ExtendUnitProcessFlows
        {
            get
            {
                return _extendUnitProcessFlows == null ? _extendUnitProcessFlows = new List<ProcessFlow>() : _extendUnitProcessFlows;
            }

            set { _extendUnitProcessFlows = value; }

        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        /// <summary>
        /// MachineName =EquipmentID_UnitID
        /// 没有UnitID
        /// MachineName=EquipemntID
        /// </summary>
        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }

        public string SlotNO
        {
            get { return _slotNo; }
            set { _slotNo = value; }
        }

        public object Clone()
        {
            ProcessFlow flow = (ProcessFlow)this.MemberwiseClone();
            flow.UnitProcessFlows = new SerializableDictionary<string, ProcessFlow>();
            if (this.UnitProcessFlows != null)
            {
                foreach (string key in this.UnitProcessFlows.Keys)
                {
                    flow.UnitProcessFlows.Add(key, (ProcessFlow)this.UnitProcessFlows[key].Clone());
                }
            }
            flow.ExtendUnitProcessFlows = new List<ProcessFlow>();
            if (this.ExtendUnitProcessFlows != null && this.ExtendUnitProcessFlows.Count > 0)
            {
                foreach (ProcessFlow f in this.ExtendUnitProcessFlows)
                {
                    flow.ExtendUnitProcessFlows.Add((ProcessFlow)f.Clone());
                }
            }
            return flow;
        }
    }
}
