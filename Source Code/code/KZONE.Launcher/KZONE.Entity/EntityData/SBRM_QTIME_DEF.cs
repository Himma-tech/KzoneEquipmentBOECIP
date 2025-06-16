using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMQTIMEDEF

	/// <summary>
	/// SBRMQTIMEDEF object for NHibernate mapped table 'SBRM_QTIME_DEF'.
	/// </summary>
	public class QtimeDefEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINEID;
		protected string _sERVERNAME;
		protected string _qTIMEID;
		protected string _sTARTNODEID;
		protected string _sTARTNODENO;
		protected string _sTARTNUNITID;
		protected string _sTARTNUNITNO;
		protected string _sTARTEVENTMSG;
		protected string _eNDNODEID;
		protected string _eNDNODENO;
		protected string _eNDNUNITID;
		protected string _eNDNUNITNO;
		protected string _eNDEVENTMSG;
		protected int _sETTIMEVALUE;
		protected string _rEMARK;
		protected string _sTARTNODERECIPEID;
		protected int _sETCFRWVALUE;
		protected string _eNABLED;

		#endregion

		#region Constructors

		public QtimeDefEntityData() { }

        public QtimeDefEntityData(string lINEID, string sERVERNAME, string qTIMEID, string sTARTNODEID, string sTARTNODENO, string sTARTNUNITID, string sTARTNUNITNO, string sTARTEVENTMSG, string eNDNODEID, string eNDNODENO, string eNDNUNITID, string eNDNUNITNO, string eNDEVENTMSG, int sETTIMEVALUE, string rEMARK, string sTARTNODERECIPEID, int sETCFRWVALUE, string eNABLED)
		{
			this._lINEID = lINEID;
			this._sERVERNAME = sERVERNAME;
			this._qTIMEID = qTIMEID;
			this._sTARTNODEID = sTARTNODEID;
			this._sTARTNODENO = sTARTNODENO;
			this._sTARTNUNITID = sTARTNUNITID;
			this._sTARTNUNITNO = sTARTNUNITNO;
			this._sTARTEVENTMSG = sTARTEVENTMSG;
			this._eNDNODEID = eNDNODEID;
			this._eNDNODENO = eNDNODENO;
			this._eNDNUNITID = eNDNUNITID;
			this._eNDNUNITNO = eNDNUNITNO;
			this._eNDEVENTMSG = eNDEVENTMSG;
			this._sETTIMEVALUE = sETTIMEVALUE;
			this._rEMARK = rEMARK;
			this._sTARTNODERECIPEID = sTARTNODERECIPEID;
			this._sETCFRWVALUE = sETCFRWVALUE;
			this._eNABLED = eNABLED;
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

		public virtual string QTIMEID
		{
			get { return _qTIMEID; }
			set
			{				
				_qTIMEID = value;
			}
		}

		public virtual string STARTNODEID
		{
			get { return _sTARTNODEID; }
			set
			{				
				_sTARTNODEID = value;
			}
		}

		public virtual string STARTNODENO
		{
			get { return _sTARTNODENO; }
			set
			{				
				_sTARTNODENO = value;
			}
		}

		public virtual string STARTNUNITID
		{
			get { return _sTARTNUNITID; }
			set
			{				
				_sTARTNUNITID = value;
			}
		}

		public virtual string STARTNUNITNO
		{
			get { return _sTARTNUNITNO; }
			set
			{				
				_sTARTNUNITNO = value;
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

		public virtual string ENDNODEID
		{
			get { return _eNDNODEID; }
			set
			{				
				_eNDNODEID = value;
			}
		}

		public virtual string ENDNODENO
		{
			get { return _eNDNODENO; }
			set
			{				
				_eNDNODENO = value;
			}
		}

		public virtual string ENDNUNITID
		{
			get { return _eNDNUNITID; }
			set
			{				
				_eNDNUNITID = value;
			}
		}

		public virtual string ENDNUNITNO
		{
			get { return _eNDNUNITNO; }
			set
			{				
				_eNDNUNITNO = value;
			}
		}

		public virtual string ENDEVENTMSG
		{
			get { return _eNDEVENTMSG; }
			set
			{				
				_eNDEVENTMSG = value;
			}
		}

		public virtual int SETTIMEVALUE
		{
			get { return _sETTIMEVALUE; }
			set { _sETTIMEVALUE = value; }
		}

		public virtual string REMARK
		{
			get { return _rEMARK; }
			set
			{				
				_rEMARK = value;
			}
		}

		public virtual string STARTNODERECIPEID
		{
			get { return _sTARTNODERECIPEID; }
			set
			{				
				_sTARTNODERECIPEID = value;
			}
		}

		public virtual int SETCFRWVALUE
		{
			get { return _sETCFRWVALUE; }
			set { _sETCFRWVALUE = value; }
		}

		public virtual string ENABLED
		{
			get { return _eNABLED; }
			set
			{				
				_eNABLED = value;
			}
		}

		

		#endregion
	}
	#endregion
}