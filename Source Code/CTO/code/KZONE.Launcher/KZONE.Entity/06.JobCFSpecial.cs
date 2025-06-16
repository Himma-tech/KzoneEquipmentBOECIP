using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KZONE.Entity
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
    public class JobCFSpeical:ICloneable
    {
        private string _maskInspecationResult;
        private string _turnOverCount;
        private string _ngMark;
        //private string _toPortNo; //20160923 PLC Address Map Delete
        private eBitResult _EngraveFlag;
        private eBitResult _turnFlag;
        private eMask_Status _maskStatus=eMask_Status.UNKNOW;
        private string _coaterHis = string.Empty;//For CF  CoaterHis 
        private string _alignerhis = string.Empty;//For Aligner His   
        private string _hpHis = string.Empty;//for  HPCP His
        private string _lineSpecialFlag=string.Empty; //CF  PHL Special Flag 2016-11-30
       
        [Category("CF Special")]
        public string MaskInspecationResult {
            get { return _maskInspecationResult; }
            set{_maskInspecationResult=value;}
        }
        [Category("CF Special")]
        public string TurnOverCount {
            get { return _turnOverCount; }
            set { _turnOverCount = value; }
        }
        [Category("CF Special")]
        public string NgMark {
            get { return _ngMark; }
            set { _ngMark = value; }
        }

        //20160923 PLC Address Map Delete
        /// <summary>
        /// CF  Photo  Easy Sort Mode  Use
        /// </summary>
        //[Category("CF Special")]
        //public string ToPortNo {
        //    get { return _toPortNo; }
        //    set { _toPortNo = value; }
        //}

        /// <summary>
        /// CF  Photo Line  0: Not Engrave 1.Engrave
        /// </summary>
        [Category("CF Special")]
        public eBitResult EngraveFlag {
            get { return _EngraveFlag; }
            set { _EngraveFlag = value; }
        }
        /// <summary>
        /// F1FMA 0:Not turn 1: Turn USE
        /// </summary>
        [Category("CF Special")]
        public eBitResult TurnFlag {
            get { return _turnFlag; }
            set { _turnFlag = value; }
        }
        [Category("CF Special")]
        public eMask_Status MaskStatus {
            get { return _maskStatus; }
            set { _maskStatus = value; }
        }
        [Category("CF Special")]
        public string CoaterHis {
            get { return _coaterHis; }
            set { _coaterHis = value; }
        }
        [Category("CF Specila")]
        public string AlignerHis {
            get { return _alignerhis; }
            set { _alignerhis = value; }
        }
        [Category("Cf Special")]
        public string HPHis {
            get { return _hpHis; }
            set { _hpHis = value; }
        }
        [Category("Cf Special")]
        public string LineSpecialFlag
        {
            get { return _lineSpecialFlag; }
            set { _lineSpecialFlag = value; }
        }

        //20170131 add for CF PHL Mapping時的SamplingRate 分母分子
        private string _samplingRate_Mu = string.Empty;
        private string _samplingRate_Zi = string.Empty;

        /// <summary> Mapping Download時SamplingRate的分母
        /// 
        /// </summary>
        [Category("Cf Special")]
        public string SamplingRate_Mu
        {
            get { return _samplingRate_Mu; }
            set { _samplingRate_Mu = value; }
        }

        /// <summary> Mapping Download時SamplingRate的分子
        /// 
        /// </summary>
        [Category("Cf Special")]
        public string SamplingRate_Zi
        {
            get { return _samplingRate_Zi; }
            set { _samplingRate_Zi = value; }
        }


        public JobCFSpeical()
        {
            _lineSpecialFlag = new string('0', 32);
            _ngMark = new string('0', 32);// NG Mark  做初始化 2016-12-12 tom
        }
       

        public object Clone() {
            JobCFSpeical special = (JobCFSpeical)this.MemberwiseClone();
            return special;
        }
    }
}
