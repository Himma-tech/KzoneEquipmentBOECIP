///Line Object  DB Data + BCS Data 
///BCS Data Save To File

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using KZONE.ConstantParameter;

namespace KZONE.Entity
{
	/// <summary>
	/// 對應File, 修改Property後呼叫Save(), 會序列化存檔
	/// </summary>
	[Serializable]
	public class LineEntityFile : EntityFile
    {
        #region Field
        private string _status = "DOWN";
        private eHostMode _hostMode = eHostMode.OFFLINE;
        private eHostMode _preHostMode = eHostMode.OFFLINE;
        private eHostMode _opictlmode = eHostMode.OFFLINE;
        private string _plcStatus = "Disconnected";
        private string _hsmsStatus = string.Empty;
        private string _mesConnectState = "Disconnect";//MES Connect Status 连线状态
        private string _mesEqpRunMode = eMESEqpRunMode.NOML;
        private string _indexerRunMode = string.Empty;  //20170110 Line的_indexerRunMode不用了.改用_runMode
        private string _runMode = "0";         //20170110 Line的_indexerRunMode不用了.改用_runMode
        private string _opiIndRunMode = string.Empty;     //20170110 For OPI用

        
        private int _coolRunSetCount = 1000;
        private int _coolRunRemainCount = 1000;
        private int _lastDailyCstSeqNo = 65000;

        public int LastDailyCstSeqNo
        {
            get 
            {
                if (_lastDailyCstSeqNo > 65500 | _lastDailyCstSeqNo < 65000)
                    _lastDailyCstSeqNo = 65000;
                
                return _lastDailyCstSeqNo; 
            }
            set { _lastDailyCstSeqNo = value; }
        }

        private bool _onBoradCardAlive = true;

        //20161004 add BlockControl Enable/Disable Flag
        private bool _subBlockControlEnabled = false; //default是不啟動該功能!!

        #endregion

        #region Property
        public string MesConnectState
        {
            get
            {
                if (string.IsNullOrEmpty(_mesConnectState))
                {
                    _mesConnectState = "Disconnect";
                }
                return _mesConnectState;
            }
            set { _mesConnectState = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public eHostMode OPICtlMode
        {
            get { return _opictlmode; }
            set { _opictlmode = value; }
        }

        public eHostMode HostMode
        {
            get { return _hostMode; }
            set { _hostMode = value; }
        }

        public eHostMode PreHostMode
        {
            get { return _preHostMode; }
            set { _preHostMode = value; }
        }

        public string PLCStatus
        {
            get { return _plcStatus; }
            set { _plcStatus = value; }
        }

        public string HSMSStatus
        {
            get { return _hsmsStatus; }
            set { _hsmsStatus = value; }
        }

        public bool OnBoradCardAlive
        {
            get { return _onBoradCardAlive; }
            set { _onBoradCardAlive = value; }
        }

        /// <summary>
        /// 20170110 Ray: line.File.IndexerRunMode不用了,改用line.File.RunMode
        /// </summary>
        //public string IndexerRunMode
        //{
        //    get { return _indexerRunMode; }
        //    set { _indexerRunMode = value; }
        //}

        /// <summary>
        /// 20170110 Ray: line.File.IndexerRunMode不用了,改用line.File.RunMode
        /// </summary>
        public string RunMode
        {
            get { return _runMode; }
            set { _runMode = value; }   
        }

        /// <summary>
        /// 20170110 Ray: 這是For OPI用的,BC要用line.File.RunMode
        /// </summary>
        public string OpiIndRunMode
        {
            get { return _opiIndRunMode; }
            set { _opiIndRunMode = value; }
        }

        public string MesEqpRunMode
        {
            get { return _mesEqpRunMode; }
            set { _mesEqpRunMode = value; }
        }

        public int CoolRunSetCount
        {
            get { return _coolRunSetCount; }
            set { _coolRunSetCount = value; }
        }

        public int CoolRunRemainCount
        {
            get { return _coolRunRemainCount; }
            set { _coolRunRemainCount = value; }
        }

        /// <summary> Sub Block 功能是否啟動
        /// 
        /// </summary>
        public bool SubBlockControlEnabled
        {
            get { return _subBlockControlEnabled; }
            set { _subBlockControlEnabled = value; }
        }

        //20161115 add for Line NodeIndexerRunModeList
        private SerializableDictionary<string, string> _lineIndexerRunModeList = new SerializableDictionary<string, string>();

        /// <summary> Line All Eqp report Indexer Run Mode List. (NodeNo, Index Run Mode(string))
        /// 
        /// </summary>
        public SerializableDictionary<string, string> LineIndexerRunModeList
        {
            get { return _lineIndexerRunModeList; }
            set { _lineIndexerRunModeList = value; }
        }

        //20170620 add A1PHL Parameter
        private int _parameter_A1PHL_PCD_Prefetch_TimeOut = 40000;

        /// <summary> A1PHL PCD PreFetch TimeOut(ms) default=40000
        /// 
        /// </summary>
        public int Parameter_A1PHL_PCD_Prefetch_TimeOut
        {
            get { return _parameter_A1PHL_PCD_Prefetch_TimeOut; }
            set { _parameter_A1PHL_PCD_Prefetch_TimeOut = value; }
        }

        private int _parameter_A1PHL_PCD_Prefetch_ReturnToCST_TimeOut = 60000;

        /// <summary> A1PHL PCD PreFetch Return To CST TimeOut(ms) default=60000
        /// 
        /// </summary>
        public int Parameter_A1PHL_PCD_Prefetch_ReturnToCST_TimeOut
        {
            get { return _parameter_A1PHL_PCD_Prefetch_ReturnToCST_TimeOut; }
            set { _parameter_A1PHL_PCD_Prefetch_ReturnToCST_TimeOut = value; }
        }

        #endregion 
    }

	public class Line : Entity
	{

		public LineEntityData Data { get; private set; }

		public LineEntityFile File { get; private set; }

        public Line(LineEntityData data, LineEntityFile file)
        {
            Data = data;
            File = file;
        }
	}
}
