using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KZONE.UI
{
    public class Line
    {
        public string LineID { get; set; }          //MES Line ID
        public string LineID2 { get; set; } 
        public string LineName { get; set; }        //Line Name
        public string LineType { get; set; }
        public string FabType { get; set; }         //Fab Type - "ARRAY","CF","CELL","MODULE"
        public string ServerName { get; set; }      //BC Server Name
        public string HistoryType { get; set; }    //History Type -- 撈取history param用

        public string JobDataLineType { get; set; }   //Job Data Line Type
        //public string EquipmentRunMode { get; set; }   //CF / TFT ( for UPK Line 要支援 Array 和  CF 的切換顯示)
        public string DBConn { get; set; }          //DB Connection String

        public string SocketIP { get; set; }        //BCS socket connection ip
        public string SocketPort { get; set; }      //BCS socket connection port

        public string LineSpecialFun { get; set; }  //SBRM_LINE->[OPI_FUNCTION] , Line Control畫面顯示用
        public int IndexerMode { get; set; }
       

        public string LineOperMode { get; set; }
        public string ShortCutMode { get; set; }
        public string LineStatus { get; set; }
        public string MesControlMode { get; set; }  //LOCAL,OFFLINE,REMOTE
        public string MesControlMode2 { get; set; }  //for line2 use -- LOCAL,OFFLINE,REMOTE
        public string PLCStatus { get; set; }
        public int CoolRunSetCount { get; set; }
        public int CoolRunRemainCount { get; set; }
        public eRobotFetchSequenceMode RobotFetchSequenceMode { get; set; } // for indexer node use -- Robot Fetch Sequence Mode
        public eCSTOperationMode CassetteOperationMode { get; set; }

        public bool CrossLineRecipeCheck { get; set; } // 控制跨Line时是否需要check 下一条Line 机台的recipe (FBBMPH100、FBRPH100、FBGPH100、FBBPH100)
       // public csXmlLine ConnectionData { get; set; }
       
        public string ChangerPlanID { get; set; }
        public eChangerPlanStatus ChangerPlanStatus { get; set; }
        public BindingList<PlanDetail> LstChangerPlanDetail { get; set; }
        public List<CSTCtrlDefaultData> LstCstControlDefaultData { get; set; } //offline 下貨帶入的default data

        
        public List<LineIndexerMode> LineIndexerModes { get; set; }
        public List<FetchGlassProportionalName> FetchGlassProportionalNames { get; set; }
        public List<ProcessType_Array> Lst_ProcessType_Array { get; set; }

        //public List<SBRM_OPI_OBJECT_DEF> HideContrls { get; set; }

        //public DenseCtrlDefaultData DenseControlDefaultData { get; set; }
        public Node IndexerNode { get; set; }
        public Node CV06_Node { get; set; }
        public eForceVCRMode_SOR ForceVCRMode { get; set; } // CBSOR100,CBSOR200,CBSOR300 Line use -- Force VCR Mode
        public eVirualPortOpMode_SOR VirualPortOpMode { get; set; } // CBSOR100,CBSOR200 Line use -- Virual Port Enable Mode

       
        public Line()
        {
            
            ForceVCRMode = new eForceVCRMode_SOR();
            VirualPortOpMode = new eVirualPortOpMode_SOR();

            LineIndexerModes = new List<LineIndexerMode>();
            Lst_ProcessType_Array = new List<ProcessType_Array>();
            FetchGlassProportionalNames = new List<FetchGlassProportionalName>();

            LstCstControlDefaultData = new List<CSTCtrlDefaultData>();
            //DenseControlDefaultData = new DenseCtrlDefaultData();
            MesControlMode = string.Empty;
            MesControlMode2 = string.Empty;
            CassetteOperationMode = eCSTOperationMode.Unknown;

            
        }
    }

    public class FetchGlassProportionalName
    {
        private int proportionalNameNo = 0;
        private string proportionalName = string.Empty;

        public int ProportionalNameNo
        {
            get { return proportionalNameNo; }
            set { proportionalNameNo = value; }
        }
        public string ProportionalName
        {
            get { return proportionalName; }
            set { proportionalName = value; }
        }
        public string ProportionalNameDesc
        {
            get { return string.Format("{0}:{1}", proportionalNameNo.ToString(), proportionalName); }
        }
    }

    public class LineRunMode
    {
        private string runModeNo = "0";
        private string runModeName = string.Empty;

        public string RunModeNo
        {
            get { return runModeNo; }
            set { runModeNo = value; }
        }
        public string RunModeName
        {
            get { return runModeName; }
            set { runModeName = value; }
        }
        public string RunModeDesc
        {
            get
            {
                if (runModeNo == runModeName) return runModeName;
                else return string.Format("{0}:{1}", runModeNo.ToString(), runModeName);
            }
        }
    }

    public class LineIndexerMode
    {
        private int indexerModeNo = 0;
        private string indexerModeName = string.Empty;
        public int IndexerModeNo
        {
            get { return indexerModeNo; }
            set { indexerModeNo = value; }
        }
        public string IndexerModeName
        {
            get { return indexerModeName; }
            set { indexerModeName = value; }
        }
        public string IndexerModeDesc
        {
            get { return string.Format("{0}:{1}", indexerModeNo.ToString(), indexerModeName); }
        }
    }

    public class ProcessType_Array
    {
        private int processTypeNo = 0;
        private string processTypeName = string.Empty;
        public int ProcessTypeNo
        {
            get { return processTypeNo; }
            set { processTypeNo = value; }
        }
        public string ProcessTypeName
        {
            get { return processTypeName; }
            set { processTypeName = value; }
        }
        public string ProcessTypeDesc
        {
            get { return string.Format("{0}:{1}", processTypeNo.ToString(), processTypeName); }
        }
    }

    public class LoaderOperationMode_ATS
    {
        private int operationModeNo = 0;
        private string operationModeName = string.Empty;
        public int OperationModeNo
        {
            get { return operationModeNo; }
            set { operationModeNo = value; }
        }
        public string OperationModeName
        {
            get { return operationModeName; }
            set { operationModeName = value; }
        }
        public string RunModeDesc
        {
            get { return string.Format("{0}:{1}", operationModeNo.ToString(), operationModeName); }
        }
    }


    public struct PlanDetail
    {
        public string SlotNo { get; set; }
        public string ProductName { get; set; }
        public string SourceCSTID { get; set; }
        public string TargetCSTID { get; set; }
        public bool HaveBeenUse { get; set; }
    }

    public class CSTCtrlDefaultData
    {
        public string PanelName { get; set; }  //Panel name
        public string CheckBoxName { get; set; } //對應勾選的check box名稱
        public string ObjectName { get; set; }  //對應物件名稱
        public eObjectType ObjectType { get; set; } //對應物件型態 (textbox,combobox,checkbox)

        public bool CheckBoxValue { get; set; } //對應勾選的check box設定值
        public string ObjectValue { get; set; } //對應物件設定值      
        
        #region for Lot Recipe
        public string LotRecipeName { get; set; }
        public string LotRecipeID { get; set; }
        #endregion
    }

   
}
