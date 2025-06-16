using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBCSDISPATCHHISTORY

	/// <summary>
	/// SBCSDISPATCHHISTORY object for NHibernate mapped table 'SBCS_DISPATCHHISTORY'.
	/// </summary>
	public class DispatchHistory:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected DateTime _uPDATETIME=DateTime.Now;
		protected string _eVENTNAME;
		protected int _cASSETTESEQNO;
		protected int _cASSETTESLOTNO;
		protected string _glASSID;
		protected int _gROUPNO;
		protected string _cIMMODE;
		protected string _gLASSTYPE;
		protected string _gLASSJUDGE;
		protected string _gLASSGRADE;
		protected string _pPID;
		protected string _iNSPECTIONRESERVATIONSIGNAL;
		protected string _pROCESSRESERVATIONSIGNAL;
		protected string _iNSPJUDGEDRESULT;
		protected string _tRACKINGDATAHISTORY;
		protected string _eQUIPMENTSPECIALFLAG;
		protected string _dISPATCHNODENO;
		protected string _dISPATCHPOINT;
		protected string _dISPATCHTARGET;
		protected string _sAMPLINGRATE;
		protected string _dISPATCHRESULT;
		protected string _bATCHID;
        protected string _nGMARK;
        protected string _rEASON;
		#endregion

		#region Constructors

		public DispatchHistory() { }

        public DispatchHistory(DateTime uPDATETIME, string eVENTNAME, int cASSETTESEQNO, int cASSETTESLOTNO, string glASSID, int gROUPNO, string cIMMODE, string gLASSTYPE, string gLASSJUDGE, string gLASSGRADE, string pPID, string iNSPECTIONRESERVATIONSIGNAL, string pROCESSRESERVATIONSIGNAL, string iNSPJUDGEDRESULT, string tRACKINGDATAHISTORY, string eQUIPMENTSPECIALFLAG, string dISPATCHNODENO, string dISPATCHPOINT, string dISPATCHTARGET, string sAMPLINGRATE, string dISPATCHRESULT, string bATCHID)
		{
			this._uPDATETIME = uPDATETIME;
			this._eVENTNAME = eVENTNAME;
			this._cASSETTESEQNO = cASSETTESEQNO;
			this._cASSETTESLOTNO = cASSETTESLOTNO;
			this._glASSID = glASSID;
			this._gROUPNO = gROUPNO;
			this._cIMMODE = cIMMODE;
			this._gLASSTYPE = gLASSTYPE;
			this._gLASSJUDGE = gLASSJUDGE;
			this._gLASSGRADE = gLASSGRADE;
			this._pPID = pPID;
			this._iNSPECTIONRESERVATIONSIGNAL = iNSPECTIONRESERVATIONSIGNAL;
			this._pROCESSRESERVATIONSIGNAL = pROCESSRESERVATIONSIGNAL;
			this._iNSPJUDGEDRESULT = iNSPJUDGEDRESULT;
			this._tRACKINGDATAHISTORY = tRACKINGDATAHISTORY;
			this._eQUIPMENTSPECIALFLAG = eQUIPMENTSPECIALFLAG;
			this._dISPATCHNODENO = dISPATCHNODENO;
			this._dISPATCHPOINT = dISPATCHPOINT;
			this._dISPATCHTARGET = dISPATCHTARGET;
			this._sAMPLINGRATE = sAMPLINGRATE;
			this._dISPATCHRESULT = dISPATCHRESULT;
			this._bATCHID = bATCHID;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual DateTime UPDATETIME
		{
			get { return _uPDATETIME; }
			set { _uPDATETIME = value; }
		}

		public virtual string EVENTNAME
		{
			get { return _eVENTNAME; }
			set
			{				
				_eVENTNAME = value;
			}
		}

		public virtual int CASSETTESEQNO
		{
			get { return _cASSETTESEQNO; }
			set { _cASSETTESEQNO = value; }
		}

		public virtual int CASSETTESLOTNO
		{
			get { return _cASSETTESLOTNO; }
			set { _cASSETTESLOTNO = value; }
		}

		public virtual string GlASSID
		{
			get { return _glASSID; }
			set
			{				
				_glASSID = value;
			}
		}

		public virtual int GROUPNO
		{
			get { return _gROUPNO; }
			set { _gROUPNO = value; }
		}

		public virtual string CIMMODE
		{
			get { return _cIMMODE; }
			set
			{				
				_cIMMODE = value;
			}
		}

		public virtual string GLASSTYPE
		{
			get { return _gLASSTYPE; }
			set
			{				
				_gLASSTYPE = value;
			}
		}

		public virtual string GLASSJUDGE
		{
			get { return _gLASSJUDGE; }
			set
			{				
				_gLASSJUDGE = value;
			}
		}

		public virtual string GLASSGRADE
		{
			get { return _gLASSGRADE; }
			set
			{				
				_gLASSGRADE = value;
			}
		}

		public virtual string PPID
		{
			get { return _pPID; }
			set
			{				
				_pPID = value;
			}
		}

		public virtual string INSPECTIONRESERVATIONSIGNAL
		{
			get { return _iNSPECTIONRESERVATIONSIGNAL; }
			set
			{				
				_iNSPECTIONRESERVATIONSIGNAL = value;
			}
		}

		public virtual string PROCESSRESERVATIONSIGNAL
		{
			get { return _pROCESSRESERVATIONSIGNAL; }
			set
			{				
				_pROCESSRESERVATIONSIGNAL = value;
			}
		}

		public virtual string INSPJUDGEDRESULT
		{
			get { return _iNSPJUDGEDRESULT; }
			set
			{				
				_iNSPJUDGEDRESULT = value;
			}
		}

		public virtual string TRACKINGDATAHISTORY
		{
			get { return _tRACKINGDATAHISTORY; }
			set
			{				
				_tRACKINGDATAHISTORY = value;
			}
		}

		public virtual string EQUIPMENTSPECIALFLAG
		{
			get { return _eQUIPMENTSPECIALFLAG; }
			set
			{				
				_eQUIPMENTSPECIALFLAG = value;
			}
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

		public virtual string DISPATCHTARGET
		{
			get { return _dISPATCHTARGET; }
			set
			{				
				_dISPATCHTARGET = value;
			}
		}

		public virtual string SAMPLINGRATE
		{
			get { return _sAMPLINGRATE; }
			set
			{				
				_sAMPLINGRATE = value;
			}
		}

		public virtual string DISPATCHRESULT
		{
			get { return _dISPATCHRESULT; }
			set
			{				
				_dISPATCHRESULT = value;
			}
		}

		public virtual string BATCHID
		{
			get { return _bATCHID; }
			set
			{				
				_bATCHID = value;
			}
		}

        public virtual string NGMARK {
            get { return _nGMARK; }
            set { _nGMARK = value; }
        }

        public virtual string REASON {
            get { return _rEASON; }
            set { _rEASON = value; }
        }

		#endregion
	}
	#endregion
}