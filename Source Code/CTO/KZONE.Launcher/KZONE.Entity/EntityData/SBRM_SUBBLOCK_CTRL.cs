using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMSUBBLOCKCTRL

	/// <summary>
	/// SBRMSUBBLOCKCTRL object for NHibernate mapped table 'SBRM_SUBBLOCK_CTRL'.
	/// </summary>
	public class SubBlockEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINEID;
		protected string _sERVERNAME;
		protected string _sUBBLOCKID;
		protected string _sTARTEQP;
		protected string _cONTROLEQP;
		protected string _sTARTEVENTMSG;
		protected string _sTOPBITCOMMANDNO;
		protected string _nEXTSUBBLOCKEQP;
		protected string _nEXTSUBBLOCKEQPLIST;
		protected string _eNABLED;
        protected string _sTOPBITCOMMANDREPLYNO;
		protected string _rEMARK;

		#endregion

		#region Constructors

		public SubBlockEntityData() { }

        public SubBlockEntityData(string lINEID, string sERVERNAME, string sUBBLOCKID, string sTARTEQP, string cONTROLEQP, string sTARTEVENTMSG, string sTOPBITCOMMANDNO, string nEXTSUBBLOCKEQP, string nEXTSUBBLOCKEQPLIST, string eNABLED, string sTOPBITCOMMANDREPLYNO, string rEMARK)
		{
			this._lINEID = lINEID;
			this._sERVERNAME = sERVERNAME;
			this._sUBBLOCKID = sUBBLOCKID;
			this._sTARTEQP = sTARTEQP;
			this._cONTROLEQP = cONTROLEQP;
			this._sTARTEVENTMSG = sTARTEVENTMSG;
			this._sTOPBITCOMMANDNO = sTOPBITCOMMANDNO;
			this._nEXTSUBBLOCKEQP = nEXTSUBBLOCKEQP;
			this._nEXTSUBBLOCKEQPLIST = nEXTSUBBLOCKEQPLIST;
			this._eNABLED = eNABLED;
			this._sTOPBITCOMMANDREPLYNO = sTOPBITCOMMANDREPLYNO;
			this._rEMARK = rEMARK;
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

		public virtual string SUBBLOCKID
		{
			get { return _sUBBLOCKID; }
			set
			{				
				_sUBBLOCKID = value;
			}
		}

		public virtual string STARTEQP
		{
			get { return _sTARTEQP; }
			set
			{				
				_sTARTEQP = value;
			}
		}

		public virtual string CONTROLEQP
		{
			get { return _cONTROLEQP; }
			set
			{				
				_cONTROLEQP = value;
			}
		}

		public virtual string STARTEVENTMSG
		{
			get { return _sTARTEVENTMSG; }
			set
			{				
				_sTARTEVENTMSG = value;
			}
		}

		public virtual string STOPBITCOMMANDNO
		{
			get { return _sTOPBITCOMMANDNO; }
			set
			{				
				_sTOPBITCOMMANDNO = value;
			}
		}

		public virtual string NEXTSUBBLOCKEQP
		{
			get { return _nEXTSUBBLOCKEQP; }
			set
			{				
				_nEXTSUBBLOCKEQP = value;
			}
		}

		public virtual string NEXTSUBBLOCKEQPLIST
		{
			get { return _nEXTSUBBLOCKEQPLIST; }
			set
			{				
				_nEXTSUBBLOCKEQPLIST = value;
			}
		}

		public virtual string ENABLED
		{
			get { return _eNABLED; }
			set
			{				
				_eNABLED = value;
			}
		}

        public virtual string STOPBITCOMMANDREPLYNO
		{
            get { return _sTOPBITCOMMANDREPLYNO; }
			set
			{
                _sTOPBITCOMMANDREPLYNO = value;
			}
		}

		public virtual string REMARK
		{
			get { return _rEMARK; }
			set
			{				
				_rEMARK = value;
			}
		}

		

		#endregion
	}
	#endregion
}