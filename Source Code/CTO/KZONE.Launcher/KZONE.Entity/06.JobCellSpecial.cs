using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KZONE.Entity
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
    public class JobCellSpecial:ICloneable
    {
        private string _controlMode = "0"; //INT
        private string _reworkCount = "0"; //INT

        private string _oxrInformation = string.Empty;

        //C1CUT
        private string _panelOXInfo = "0";// 下货的时候给“0” 2016/12/1 Tom
        private string _cassetteSettingCode = string.Empty;
        private string _abnormalCode = string.Empty; //ASCII
        private string _panelSize = "0"; //INT
        private string _turnAngle = "1"; //INT
        private string _crossLineCassetteSettingCode = string.Empty; //ASCII
        private string _panelSizeFlag = "0";
        private string _mmgFlag = "0";
        private string _crossLinePanelSize = "0";
        private string _cutProductID = "0";
        private string _cutCrossProductID = "0";
        private string _cutProductType = "0";
        private string _cutCrossProductType = "0";
        private string _polProductType = "0";
        private string _polProductID = "0";
        private string _crossLinePPID = string.Empty; //ASCII
        private string _crossLineGroupNo = "0";
        private string _locationNo = "0";
        private string _fromSlotNo = "0";
        private bool _cuttingFlag = false;
        //跨Line用
        private string _crossLineFromEqptId = string.Empty;
        private string _crossLineFromPortId = string.Empty;
        private string _crossLineFromEqptOutDate = string.Empty;
        private string _crossLineFromEqptOutTime = string.Empty;
        private string _crossLineToEqptId = string.Empty;
        private string _crossLineToPortId = string.Empty;
        private string _crossLineToEqptInDate = string.Empty;
        private string _crossLineToEqptInTime = string.Empty;
        private string _crossLineRecipeName = string.Empty;
        //C1ODF
        private string _arrayTTPEQVersion = "0"; //INT
        private string _cfCasetteSeqNo = "0";
        private string _cfJobSeqNo = "0";
        private string _tftCasetteSeqNo = "0";
        private string _tftJobSeqNo = "0";
        private bool assemblyFlag = false; //是否做过贴合 bruce.zhan 20170427

        private string _odfBoxChamberOpenTime_01 = string.Empty;
        private string _odfBoxChamberOpenTime_02 = string.Empty;
        private string _odfBoxChamberOpenTime_03 = string.Empty;
        private string _repairCount = "0"; //INT
        private string _uvMaskAlreadyUseCount = "0"; //INT
        private string _criterianumber = "0";
        //C1LOI
        private string _repairResult = "0"; //INT
        private string _runMode = "0"; //INT
        private string _lcdReasonCode = string.Empty;

        //C1LSC M1PST
        private string _toBufferFlag = "0"; //BIN
        // M1OLB
        private string _lSCProcessCount = "0";
        private string _finalJudgeFlag = string.Empty;

        //C1BPI 
        private string _pirwkCnt = "0";
      
        //C1FSA
        private string _chipCount = "0";

        //ODF and Cut 
        //20170218 add pnlScpFlg  for MES Spec 1.43 ,ODF  Line Update, CUT Use tom 
        private string _panelScrapFlag = string.Empty;

        private List<DefectCode> _defectCodes = new List<DefectCode>();
        private List<AbnormalCode> _abnormalCodes = new List<AbnormalCode>();


        [Category("CELL Special")]
        public string OXRInformation
        {
            get { return _oxrInformation; }
            set { _oxrInformation = value; }
        }
        public string CassetteSettingCode
        {
            get { return _cassetteSettingCode; }
            set { _cassetteSettingCode = value; }
        }
        public string AbnormalCode
        {
            get { return _abnormalCode; }
            set { _abnormalCode = value; }
        }
        public string PanelSize
        {
            get { return _panelSize; }
            set { _panelSize = value; }
        }
        public string TurnAngle
        {
            get { return _turnAngle; }
            set { _turnAngle = value; }
        }
        public string CrossLineCassetteSettingCode
        {
            get { return _crossLineCassetteSettingCode; }
            set { _crossLineCassetteSettingCode = value; }
        }
        public string PanelSizeFlag
        {
            get { return _panelSizeFlag; }
            set { _panelSizeFlag = value; }
        }
        public string MMGFlag
        {
            get { return _mmgFlag; }
            set { _mmgFlag = value; }
        }

        public string ChipCount
        {
            get { return _chipCount; }
            set { _chipCount = value; }
        }

        public string PanelOXInfo
        {
            get { return _panelOXInfo; }
            set { _panelOXInfo = value; }
        }
        public string ToBufferFlag
        {
            get { return _toBufferFlag; }
            set { _toBufferFlag = value; }
        }

        public string CrossLinePanelSize
        {
            get { return _crossLinePanelSize; }
            set { _crossLinePanelSize = value; }
        }
        public string CUTProductID
        {
            get { return _cutProductID; }
            set { _cutProductID = value; }
        }
        public string CUTCrossProductID
        {
            get { return _cutCrossProductID; }
            set { _cutCrossProductID = value; }
        }
        public string CUTProductType
        {
            get { return _cutProductType; }
            set { _cutProductType = value; }
        }
        public string CUTCrossProductType
        {
            get { return _cutCrossProductType; }
            set { _cutCrossProductType = value; }
        }
        public string POLProductType
        {
            get { return _polProductType; }
            set { _polProductType = value; }
        }
        public string POLProductID
        {
            get { return _polProductID; }
            set { _polProductID = value; }
        }
        public string CrossLinePPID
        {
            get { return _crossLinePPID; }
            set { _crossLinePPID = value; }
        }

        public string CrossLineGroupNo
        {
            get { return _crossLineGroupNo; }
            set { _crossLineGroupNo = value; }
        }

        public string LocationNo
        {
            get { return _locationNo; }
            set { _locationNo = value; }
        }

        public string FromSlotNo
        {
            get { return _fromSlotNo; }
            set { _fromSlotNo = value; }
        }


        public string ControlMode
        {
            get { return _controlMode; }
            set { _controlMode = value; }
        }
        public string ReworkCount
        {
            get { return _reworkCount; }
            set { _reworkCount = value; }
        }

        public string ArrayTTPEQVersion
        {
            get { return _arrayTTPEQVersion; }
            set { _arrayTTPEQVersion = value; }
        }

        public string CFCasetteSeqNo
        {
            get { return _cfCasetteSeqNo; }
            set { _cfCasetteSeqNo = value; }
        }

        public string CFJobSeqNo
        {
            get { return _cfJobSeqNo; }
            set { _cfJobSeqNo = value; }
        }

        public string TFTCasetteSeqNo
        {
            get { return _tftCasetteSeqNo; }
            set { _tftCasetteSeqNo = value; }
        }

        public string TFTJobSeqNo
        {
            get { return _tftJobSeqNo; }
            set { _tftJobSeqNo = value; }
        }

        public bool AssemblyFlag 
        {
            get { return assemblyFlag; }
            set { assemblyFlag = value; }
        }

        public bool CuttingFlag
        {
            get { return _cuttingFlag; }
            set { _cuttingFlag = value; }
        }

        public string ODFBoxChamberOpenTime_01
        {
            get { return _odfBoxChamberOpenTime_01; }
            set { _odfBoxChamberOpenTime_01 = value; }
        }

        public string ODFBoxChamberOpenTime_02
        {
            get { return _odfBoxChamberOpenTime_02; }
            set { _odfBoxChamberOpenTime_02 = value; }
        }

        public string ODFBoxChamberOpenTime_03
        {
            get { return _odfBoxChamberOpenTime_03; }
            set { _odfBoxChamberOpenTime_03 = value; }
        }

        public string RepairCount
        {
            get { return _repairCount; }
            set { _repairCount = value; }
        }

        public string UvMaskAlreadyUseCount
        {
            get { return _uvMaskAlreadyUseCount; }
            set { _uvMaskAlreadyUseCount = value; }
        }
        
        public string CriteriaNumber
        {
            get { return _criterianumber; }
            set { _criterianumber = value; }
        }

        public string RepairResult
        {
            get { return _repairResult; }
            set { _repairResult = value; }
        }

        public string RunMode
        {
            get { return _runMode; }
            set { _runMode = value; }
        }
        public List<DefectCode> DefectCodes
        {
            get { return _defectCodes; }
            set { _defectCodes = value; }
        }
        public List<AbnormalCode> AbnormalCodes
        {
            get { return _abnormalCodes; }
            set { _abnormalCodes = value; }
        }

         public string LscProCount
        {
            get { return _lSCProcessCount; }
            set { _lSCProcessCount = value; }
        }

         public string LCDResasonCode
         {
             get { return _lcdReasonCode; }
             set { _lcdReasonCode = value; }
         }
         public string FinalJudgeFlag
         {
             get { return _finalJudgeFlag; }
             set { _finalJudgeFlag = value; }
         }

         public string PanelScrapFlag {
             get { return _panelScrapFlag; }
             set { _panelScrapFlag = value; }
         }

         public string PiRwkCnt {
             get { return _pirwkCnt; }
             set { _pirwkCnt = value; }
         }

        public JobCellSpecial()
        {
            _defectCodes = new List<DefectCode>();
            _abnormalCodes = new List<AbnormalCode>();
        }
        public object Clone()
        {
            JobCellSpecial special = (JobCellSpecial)this.MemberwiseClone();
            special.DefectCodes = new List<DefectCode>();
            foreach (DefectCode dc in this.DefectCodes)
            {
                special.DefectCodes.Add(dc.Clone() as DefectCode);
            }
            special.AbnormalCodes = new List<AbnormalCode>();
            foreach (AbnormalCode ac in this.AbnormalCodes){
                special.AbnormalCodes.Add(ac.Clone() as AbnormalCode);
              
            }
            return special;
        }

        #region [Cross Line用]
        public string CrossLineFromEqptId
        {
            get { return _crossLineFromEqptId; }
            set { _crossLineFromEqptId = value; }
        }

        public string CrossLineFromPortId
        {
            get { return _crossLineFromPortId; }
            set { _crossLineFromPortId = value; }
        }

        public string CrossLineFromEqptOutDate
        {
            get { return _crossLineFromEqptOutDate; }
            set { _crossLineFromEqptOutDate = value; }
        }

        public string CrossLineFromEqptOutTime
        {
            get { return _crossLineFromEqptOutTime; }
            set { _crossLineFromEqptOutTime = value; }
        }

        public string CrossLineToEqptId
        {
            get { return _crossLineToEqptId; }
            set { _crossLineToEqptId = value; }
        }

        public string CrossLineToPortId
        {
            get { return _crossLineToPortId; }
            set { _crossLineToPortId = value; }
        }

        public string CrossLineToEqptInDate
        {
            get { return _crossLineToEqptInDate; }
            set { _crossLineToEqptInDate = value; }
        }

        public string CrossLineToEqptInTime
        {
            get { return _crossLineToEqptInTime; }
            set { _crossLineToEqptInTime = value; }
        }

        public string CrossLineRecipeName
        {
            get { return _crossLineRecipeName; }
            set { _crossLineRecipeName = value; }
        }
        #endregion
    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DefectCode : ICloneable
    {
        private string _eqpNo;
        private string _unitNo;
        private string _chipPostion;
        private string _defectCodes;

        public string DefectCodes
        {
            get { return _defectCodes; }
            set { _defectCodes = value; }
        }
        public string EqpNo
        {
            get { return _eqpNo; }
            set { _eqpNo = value; }
        }
        public string UnitNo
        {
            get { return _unitNo; }
            set { _unitNo = value; }
        }
        public string ChipPostion
        {
            get { return _chipPostion; }
            set { _chipPostion = value; }
        }


        public object Clone()
        {
            DefectCode dc = (DefectCode)this.MemberwiseClone();
            return dc;
        }
    }
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AbnormalCode : ICloneable
    {
        private string _sequenceNo = string.Empty;
        private string _abnormalFlag = string.Empty;
        private string _postionNo = string.Empty;

       
        
        public string SequenceNo
        {
            get { return _sequenceNo; }
            set { _sequenceNo = value; }
        }
        public string AbnormalFlag
        {
            get { return _abnormalFlag; }
            set { _abnormalFlag = value; }
        }

        public string PostionNo
        {
            get { return _postionNo; }
            set { _postionNo = value; }
        }
        public AbnormalCode()
        {

        }
        public AbnormalCode(string seqNo, string abnormalFlag)
        {
            this.SequenceNo = seqNo;
            this.AbnormalFlag = abnormalFlag;
        }
        public object Clone()
        {
            AbnormalCode ac = (AbnormalCode)this.MemberwiseClone();
            return ac;
        }
    }
}
