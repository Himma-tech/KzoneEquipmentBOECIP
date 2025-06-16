using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMUNIT

	/// <summary>
	/// SBRMUNIT object for NHibernate mapped table 'SBRM_UNIT'.
	/// </summary>
	public class UnitEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINEID;
		protected string _nODEID;
		protected int _uNITNO;
		protected string _uNITID;
		protected string _nODENO;
		protected string _uNITTYPE;
		protected string _sERVERNAME;
		protected string _aTTRIBUTE;
		protected string _uSERUNMODE;
        protected string _sUBUNIT;

		#endregion

		#region Constructors

		public UnitEntityData() { }

        public UnitEntityData(string lINEID, string nODEID, int uNITNO, string uNITID, string nODENO, string uNITTYPE, string sERVERNAME, string aTTRIBUTE, string uSERUNMODE,string sUBUNIT)
		{
			this._lINEID = lINEID;
			this._nODEID = nODEID;
			this._uNITNO = uNITNO;
			this._uNITID = uNITID;
			this._nODENO = nODENO;
			this._uNITTYPE = uNITTYPE;
			this._sERVERNAME = sERVERNAME;
			this._aTTRIBUTE = aTTRIBUTE;
			this._uSERUNMODE = uSERUNMODE;
            this._sUBUNIT = sUBUNIT;
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

		public virtual string NODEID
		{
			get { return _nODEID; }
			set
			{				
				_nODEID = value;
			}
		}

		public virtual int UNITNO
		{
			get { return _uNITNO; }
			set
			{				
				_uNITNO = value;
			}
		}

		public virtual string UNITID
		{
			get { return _uNITID; }
			set
			{				
				_uNITID = value;
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

		public virtual string UNITTYPE
		{
			get { return _uNITTYPE; }
			set
			{				
				_uNITTYPE = value;
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

		public virtual string ATTRIBUTE
		{
			get { return _aTTRIBUTE; }
			set
			{				
				_aTTRIBUTE = value;
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
        public virtual string SUBUNIT
        {
            get { return _sUBUNIT; }
            set { _sUBUNIT = value; }
        }
		

		#endregion
	}
	#endregion
}