using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMNODE

	/// <summary>
	/// SBRMNODE object for NHibernate mapped table 'SBRM_NODE'.
	/// </summary>
	public class EquipmentEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINEID;
		protected string _sERVERNAME;
		protected string _nODENO;
		protected string _nODEID;
		protected string _rEPORTMODE;
		protected string _aTTRIBUTE;
		protected int _rECIPEIDX;
		protected int _rECIPELEN;
		protected string _rECIPESEQ;
		protected int _uNITCOUNT;
		protected string _nODENAME;
		protected string _rECIPEREGVALIDATIONENABLED;
		protected string _uSERUNMODE;
		protected string _uSEINDEXERMODE;
		protected string _uSEEDCREPORT;
		protected int _dCRCOUNT;
		protected int _sTOPBITCOUNT;
		protected string _oPITYPE;
		protected string _eQPPROFILE;
		protected string _rECIPEPARAVALIDATIONENABLED;
		protected string _fDCREPORT;
		protected int _fDCREPORTTIME;
		protected string _dAILYCHECKREPORT;
		protected int _dAILYCHECKREPORTTIME;

		#endregion

		#region Constructors

		public EquipmentEntityData() { }

        public EquipmentEntityData(string lINEID, string sERVERNAME, string nODENO, string nODEID, string rEPORTMODE, string aTTRIBUTE, int rECIPEIDX, int rECIPELEN, string rECIPESEQ, int uNITCOUNT, string nODENAME, string rECIPEREGVALIDATIONENABLED, string uSERUNMODE, string uSEINDEXERMODE, string uSEEDCREPORT, int dCRCOUNT, int sTOPBITCOUNT, string oPITYPE, string eQPPROFILE, string rECIPEPARAVALIDATIONENABLED, string fDCREPORT, int fDCREPORTTIME, string dAILYCHECKREPORT, int dAILYCHECKREPORTTIME)
		{
			this._lINEID = lINEID;
			this._sERVERNAME = sERVERNAME;
			this._nODENO = nODENO;
			this._nODEID = nODEID;
			this._rEPORTMODE = rEPORTMODE;
			this._aTTRIBUTE = aTTRIBUTE;
			this._rECIPEIDX = rECIPEIDX;
			this._rECIPELEN = rECIPELEN;
			this._rECIPESEQ = rECIPESEQ;
			this._uNITCOUNT = uNITCOUNT;
			this._nODENAME = nODENAME;
			this._rECIPEREGVALIDATIONENABLED = rECIPEREGVALIDATIONENABLED;
			this._uSERUNMODE = uSERUNMODE;
			this._uSEINDEXERMODE = uSEINDEXERMODE;
			this._uSEEDCREPORT = uSEEDCREPORT;
			this._dCRCOUNT = dCRCOUNT;
			this._sTOPBITCOUNT = sTOPBITCOUNT;
			this._oPITYPE = oPITYPE;
			this._eQPPROFILE = eQPPROFILE;
			this._rECIPEPARAVALIDATIONENABLED = rECIPEPARAVALIDATIONENABLED;
			this._fDCREPORT = fDCREPORT;
			this._fDCREPORTTIME = fDCREPORTTIME;
			this._dAILYCHECKREPORT = dAILYCHECKREPORT;
			this._dAILYCHECKREPORTTIME = dAILYCHECKREPORTTIME;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual string LINEID
		{
			get { return _lINEID; }
			set
			{				
				_lINEID = value;
			}
		}

		public virtual string SERVERNAME
		{
			get { return _sERVERNAME; }
			set
			{				
				_sERVERNAME = value;
			}
		}

		public virtual string NODENO
		{
			get { return _nODENO; }
			set
			{				
				_nODENO = value;
			}
		}

		public virtual string NODEID
		{
			get { return _nODEID; }
			set
			{				
				_nODEID = value;
			}
		}

		public virtual string REPORTMODE
		{
			get { return _rEPORTMODE; }
			set
			{				
				_rEPORTMODE = value;
			}
		}

		public virtual string ATTRIBUTE
		{
			get { return _aTTRIBUTE; }
			set
			{				
				_aTTRIBUTE = value;
			}
		}

		public virtual int RECIPEIDX
		{
			get { return _rECIPEIDX; }
			set { _rECIPEIDX = value; }
		}

		public virtual int RECIPELEN
		{
			get { return _rECIPELEN; }
			set { _rECIPELEN = value; }
		}

		public virtual string RECIPESEQ
		{
			get { return _rECIPESEQ; }
			set
			{				
				_rECIPESEQ = value;
			}
		}

		public virtual int UNITCOUNT
		{
			get { return _uNITCOUNT; }
			set { _uNITCOUNT = value; }
		}

		public virtual string NODENAME
		{
			get { return _nODENAME; }
			set
			{				
				_nODENAME = value;
			}
		}

		public virtual string RECIPEREGVALIDATIONENABLED
		{
			get { return _rECIPEREGVALIDATIONENABLED; }
			set
			{				
				_rECIPEREGVALIDATIONENABLED = value;
			}
		}

		public virtual string USERUNMODE
		{
			get { return _uSERUNMODE; }
			set
			{				
				_uSERUNMODE = value;
			}
		}

		public virtual string USEINDEXERMODE
		{
			get { return _uSEINDEXERMODE; }
			set
			{				
				_uSEINDEXERMODE = value;
			}
		}

		public virtual string USEEDCREPORT
		{
			get { return _uSEEDCREPORT; }
			set
			{				
				_uSEEDCREPORT = value;
			}
		}

		public virtual int DCRCOUNT
		{
			get { return _dCRCOUNT; }
			set { _dCRCOUNT = value; }
		}

		public virtual int STOPBITCOUNT
		{
			get { return _sTOPBITCOUNT; }
			set { _sTOPBITCOUNT = value; }
		}

		public virtual string OPITYPE
		{
			get { return _oPITYPE; }
			set
			{				
				_oPITYPE = value;
			}
		}

		public virtual string EQPPROFILE
		{
			get { return _eQPPROFILE; }
			set
			{				
				_eQPPROFILE = value;
			}
		}

		public virtual string RECIPEPARAVALIDATIONENABLED
		{
			get { return _rECIPEPARAVALIDATIONENABLED; }
			set
			{				
				_rECIPEPARAVALIDATIONENABLED = value;
			}
		}

		public virtual string FDCREPORT
		{
			get { return _fDCREPORT; }
			set
			{				
				_fDCREPORT = value;
			}
		}

		public virtual int FDCREPORTTIME
		{
			get { return _fDCREPORTTIME; }
			set { _fDCREPORTTIME = value; }
		}

		public virtual string DAILYCHECKREPORT
		{
			get { return _dAILYCHECKREPORT; }
			set
			{				
				_dAILYCHECKREPORT = value;
			}
		}

		public virtual int DAILYCHECKREPORTTIME
		{
			get { return _dAILYCHECKREPORTTIME; }
			set { _dAILYCHECKREPORTTIME = value; }
		}

		

		#endregion
	}
	#endregion
}