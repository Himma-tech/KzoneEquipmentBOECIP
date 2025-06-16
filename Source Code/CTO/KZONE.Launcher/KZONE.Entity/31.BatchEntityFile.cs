using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    [Serializable]
    public class BatchEntityFile:EntityFile
    {
        private int _batchNo;
        private string _batchId;
        private string _startEquipmentNo;
        private string _dispatchEquipmentNo;
        private string _dispatchPoint;
        private string _samplingRate;
        private int _batchCurrent;
        private int _samplingCurrent;
        private int _batchCount;
        private int _samplingCount;
        private int _priority;
        private int _serialNo;
        private int _batchIndex = 0;
        private string _inspectionType = "1";
        private eBatchStatus _batchstatus=eBatchStatus.Start;
        private eSamplingStatus _samplingStatus=eSamplingStatus.Start;
        private DateTime _updateTime=DateTime.Now;
        private DateTime _createTime = DateTime.Now;
        private string _batchKey;
        private bool _forceEnd = false;
        private int _inspectionNo;
        private string _jobDataItemName = string.Empty;
        private string _descripte;

        

        

        public BatchEntityFile() {

        }

        public BatchEntityFile(int batchNo, string batchId, string startEqNo, string dispatchEqNo, string dispatchPoint, string samplingRate, int priority) {
            this.BatchNo = batchNo;
            _batchId = batchId;
            _startEquipmentNo = startEqNo;
            _dispatchPoint = dispatchEqNo;
            _dispatchPoint = dispatchPoint;
            _samplingRate = samplingRate;
            _priority = priority;
        }
        public BatchEntityFile(int batchNo, string startEqNo, string dispatchEqNo, string dispatchPoint, int batchCount,int samplingCount, int priority,string inspectionType,string batchKey,int inspectionNo,string itemName,string descript="") {
            this.BatchNo = batchNo;
            _inspectionNo = inspectionNo;
            _startEquipmentNo = startEqNo;
            _dispatchEquipmentNo = dispatchEqNo;
            _dispatchPoint = dispatchPoint;

            _batchCount = batchCount;
            _samplingCount = samplingCount;
            _samplingRate = string.Format("{0}:{1}", samplingCount, batchCount);
            _priority = priority;
            _inspectionType = inspectionType;
            _batchKey = batchKey;
            _jobDataItemName = itemName;
            _descripte = descript;
        }


        public int BatchNo {
            get { return _batchNo; }
            set { _batchNo = value; }
        }

        public DateTime UpdateTime {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        

        public int Priority {
            get { return _priority; }
            set { _priority = value; }
        }
        
        public DateTime CreateTime {
            get { return _createTime; }
            set { _createTime = value; }
        }
        
        public int SamplingCount {
            get { return _samplingCount; }
            set { _samplingCount = value; }
        }
        
        public int BatchCount {
            get { return _batchCount; }
            set { _batchCount = value; }
        }

        public eBatchStatus BatchStatus {
            get { return _batchstatus; }
            set { _batchstatus = value; }
        }

        public string SamplingRate {
            get { return _samplingRate; }
            set { _samplingRate = value; }
        }

        public string DispatchPoint {
            get { return _dispatchPoint; }
            set { _dispatchPoint = value; }
        }

        public string DispatchEquipmentNo {
            get { return _dispatchEquipmentNo; }
            set { _dispatchEquipmentNo = value; }
        }

        public string StartEquipmentNo {
            get { return _startEquipmentNo; }
            set { _startEquipmentNo = value; }
        }

        public string BatchId {
            get { return _batchId; }
            set { _batchId = value; }
        }

        public eSamplingStatus SamplingStatus {
            get { return _samplingStatus; }
            set { _samplingStatus = value; }
        }

        public int BatchCurrent {
            get { return _batchCurrent; }
            set { _batchCurrent = value; }
        }
        public int SamplingCurrent {
            get { return _samplingCurrent; }
            set { _samplingCurrent = value; }
        }
        public int SerialNo {
            get { return _serialNo; }
            set { _serialNo = value; }
        }

        public int BatchIndex {
            get { return _batchIndex; }
            set { _batchIndex = value; }
        }
        public string InspectionType {
            get { return _inspectionType; }
            set { _inspectionType = value; }
        }

        public string BatchKey {
            get { return _batchKey; }
            set { _batchKey = value; }
        }

        /// <summary>
        /// 是否强制结束
        /// </summary>
        public bool ForceEnd {
            get { return _forceEnd; }
            set { _forceEnd = value; }
        }

        public int InspectionNo {
            get { return _inspectionNo; }
            set { _inspectionNo = value; }
        }
        public string JobDataItemName
        {
            get { return _jobDataItemName; }
            set { _jobDataItemName = value; }
        }

        public string Descripte
        {
            get { return _descripte; }
            set { _descripte = value; }
        }
        
    }
}
