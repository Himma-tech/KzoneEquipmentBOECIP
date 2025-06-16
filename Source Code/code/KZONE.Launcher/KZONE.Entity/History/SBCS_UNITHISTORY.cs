using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBCSUNITHISTORY

	/// <summary>
	/// SBCSUNITHISTORY object for NHibernate mapped table 'SBCS_UNITHISTORY'.
	/// </summary>
    public class UnitHistory : EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected DateTime _uPDATETIME=DateTime.Now;
		protected string _nODEID;
		protected string _nODENO;
		protected int _uNITNO;
		protected string _uNITID;
		protected string _uNITSTATUS;
		protected string _uNITTYPE;
        protected string _unitRecipeID;

		#endregion

		#region Constructors

		public UnitHistory() { }

        public UnitHistory(DateTime uPDATETIME, string nODEID, string nODENO, int uNITNO, string uNITID, string uNITSTATUS, string uNITTYPE,string uNITRECIPEID)
		{
			this._uPDATETIME = uPDATETIME;
			this._nODEID = nODEID;
			this._nODENO = nODENO;
			this._uNITNO = uNITNO;
			this._uNITID = uNITID;
			this._uNITSTATUS = uNITSTATUS;
			this._uNITTYPE = uNITTYPE;
            this._unitRecipeID = uNITRECIPEID;
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

		public virtual string NODEID
		{
			get { return _nODEID; }
			set
			{				
				_nODEID = value;
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

		public virtual string UNITSTATUS
		{
			get { return _uNITSTATUS; }
			set
			{				
				_uNITSTATUS = value;
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

        public virtual string UNITRECIPEID {
            get { return _unitRecipeID; }
            set { _unitRecipeID = value; }
        }
		

		#endregion
	}
	#endregion
}