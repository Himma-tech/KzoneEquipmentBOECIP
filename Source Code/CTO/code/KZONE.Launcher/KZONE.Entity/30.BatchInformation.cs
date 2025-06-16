using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KZONE.Entity
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
    public class BatchInformation:ICloneable
    {
        private int _batchNo = 0;
        private string _batchID=string.Empty;
        private string _startEquipmentNo=string.Empty;
        private string _dispatchEquipmentNo=string.Empty;
        private string _dispatchPoint=string.Empty;
        private string _samplingRate=string.Empty;
        private string _batchKey = string.Empty;
        private bool _ForceCheck = false;
        private string _jobDataItemName = string.Empty;
        private string _descript = string.Empty;

        public string Descript
        {
            get { return _descript; }
            set { _descript = value; }
        }
        public string JobDataItemName
        {
            get { return _jobDataItemName; }
            set { _jobDataItemName = value; }
        }
                    

        public string BatchKey {
            get { return _batchKey; }
            set { _batchKey = value; }
        }
        private int _priority = 0;

        public int Priority {
            get { return _priority; }
            set { _priority = value; }
        }
        private bool _sampled = false;
        /// <summary>
        /// 是否被抽样
        /// </summary>
        public bool Sampled {
            get { return _sampled; }
            set { _sampled = value; }
        }

        /// <summary>
        /// 1:2 二抽1 
        /// </summary>
        public string SamplingRate {
            get { return _samplingRate; }
            set { _samplingRate = value; }
        }


        public string DispatchEquipmentNo {
            get { return _dispatchEquipmentNo; }
            set { _dispatchEquipmentNo = value; }
        }
        

        public string DispatchPoint {
            get { return _dispatchPoint; }
            set { _dispatchPoint = value; }
        }

        public string StartEquipmentNo {
            get { return _startEquipmentNo; }
            set { _startEquipmentNo = value; }
        }
       
        public string BatchID {
            get { return _batchID; }
            set { _batchID = value; }
        }
        public int BatchNo {
            get { return _batchNo; }
            set { _batchNo = value; }
        }

        public bool ForceCheck {
            get { return _ForceCheck; }
            set { _ForceCheck = value; }
        }


        public BatchInformation() {

        }

        public  BatchInformation(BatchEntityFile batchEntityFile) {
            this.BatchNo = batchEntityFile.BatchNo;
            this._batchID = batchEntityFile.BatchId;
            this._dispatchEquipmentNo = batchEntityFile.DispatchEquipmentNo;
            this._dispatchPoint = batchEntityFile.DispatchPoint;
            this._samplingRate = batchEntityFile.SamplingRate;
            this._startEquipmentNo = batchEntityFile.StartEquipmentNo;
            this._priority = batchEntityFile.Priority;
            this._batchKey = batchEntityFile.BatchKey;
            this.JobDataItemName = batchEntityFile.JobDataItemName;
            this.Descript = batchEntityFile.Descripte;
            
        }

        public object Clone() {
            BatchInformation bi =(BatchInformation) this.MemberwiseClone();
            return bi;
        }
    }
}
