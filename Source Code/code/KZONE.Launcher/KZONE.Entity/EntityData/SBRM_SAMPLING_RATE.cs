using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMSAMPLINGRATE

	/// <summary>
	/// SBRMSAMPLINGRATE object for NHibernate mapped table 'SBRM_SAMPLING_RATE'.
	/// </summary>
	public class SamplingRateEntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _sERVERNAME;
		protected int _bATCHNO;
		protected string _bATCHNODENO;
		protected string _bATCHNODESEQ;
		protected int _sAMPLINGNO;
		protected int _iNSPECTIONNO;
		protected string _iNSPECTIONNODE;
		protected string _iNSPECTIONTYPE;
		protected string _dESCRIPTION;
		protected int _pRIORITY;
		protected string _dISPATCHNODENO;
		protected string _dISPATCHPOINT;
		protected string _bATCHKEY;
		protected string _lINEMODE;
		protected string _lAYOUT;
        protected string _jobDataItemName;

		#endregion

		#region Constructors

		public SamplingRateEntityData() { }

        public SamplingRateEntityData(string sERVERNAME, int bATCHNO, string bATCHNODENO, string bATCHNODESEQ, int sAMPLINGNO, int iNSPECTIONNO, string iNSPECTIONNODE, string iNSPECTIONTYPE, string dESCRIPTION, int pRIORITY, string dISPATCHNODENO, string dISPATCHPOINT, string bATCHKEY, string lINEMODE, string lAYOUT)
		{
			this._sERVERNAME = sERVERNAME;
			this._bATCHNO = bATCHNO;
			this._bATCHNODENO = bATCHNODENO;
			this._bATCHNODESEQ = bATCHNODESEQ;
			this._sAMPLINGNO = sAMPLINGNO;
			this._iNSPECTIONNO = iNSPECTIONNO;
			this._iNSPECTIONNODE = iNSPECTIONNODE;
			this._iNSPECTIONTYPE = iNSPECTIONTYPE;
			this._dESCRIPTION = dESCRIPTION;
			this._pRIORITY = pRIORITY;
			this._dISPATCHNODENO = dISPATCHNODENO;
			this._dISPATCHPOINT = dISPATCHPOINT;
			this._bATCHKEY = bATCHKEY;
			this._lINEMODE = lINEMODE;
			this._lAYOUT = lAYOUT;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual string SERVERNAME
		{
			get { return _sERVERNAME; }
			set
			{				
				_sERVERNAME = value;
			}
		}

		public virtual int BATCHNO
		{
			get { return _bATCHNO; }
			set { _bATCHNO = value; }
		}

		public virtual string BATCHNODENO
		{
			get { return _bATCHNODENO; }
			set
			{				
				_bATCHNODENO = value;
			}
		}

		public virtual string BATCHNODESEQ
		{
			get { return _bATCHNODESEQ; }
			set
			{				
				_bATCHNODESEQ = value;
			}
		}

		public virtual int SAMPLINGNO
		{
			get { return _sAMPLINGNO; }
			set { _sAMPLINGNO = value; }
		}

		public virtual int INSPECTIONNO
		{
			get { return _iNSPECTIONNO; }
			set { _iNSPECTIONNO = value; }
		}

		public virtual string INSPECTIONNODE
		{
			get { return _iNSPECTIONNODE; }
			set
			{				
				_iNSPECTIONNODE = value;
			}
		}

		public virtual string INSPECTIONTYPE
		{
			get { return _iNSPECTIONTYPE; }
			set
			{				
				_iNSPECTIONTYPE = value;
			}
		}

		public virtual string DESCRIPTION
		{
			get { return _dESCRIPTION; }
			set
			{				
				_dESCRIPTION = value;
			}
		}

		public virtual int PRIORITY
		{
			get { return _pRIORITY; }
			set { _pRIORITY = value; }
		}

		public virtual string DISPATCHNODENO
		{
			get { return _dISPATCHNODENO; }
			set
			{				
				_dISPATCHNODENO = value;
			}
		}

		public virtual string DISPATCHPOINT
		{
			get { return _dISPATCHPOINT; }
			set
			{				
				_dISPATCHPOINT = value;
			}
		}

		public virtual string BATCHKEY
		{
			get { return _bATCHKEY; }
			set
			{				
				_bATCHKEY = value;
			}
		}

		public virtual string LINEMODE
		{
			get { return _lINEMODE; }
			set
			{				
				_lINEMODE = value;
			}
		}

		public virtual string LAYOUT
		{
			get { return _lAYOUT; }
			set
			{				
				_lAYOUT = value;
			}
		}

        public virtual string JOBDATAITEMNAME
        {
            get { return _jobDataItemName; }
            set { _jobDataItemName = value; }
        }

		#endregion
	}
	#endregion
}