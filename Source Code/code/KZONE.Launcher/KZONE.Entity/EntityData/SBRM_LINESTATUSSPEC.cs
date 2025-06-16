using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMLINESTATUSSPEC

	/// <summary>
	/// SBRMLINESTATUSSPEC object for NHibernate mapped table 'SBRM_LINESTATUSSPEC'.
	/// </summary>
	public class LineStatusSpecEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINETYPE;
		protected string _cONDITIONSTATUS;
		protected int _cONDITIONSEQNO;
		protected string _andEQPNOLIST;
        protected string _orEQPNOLIST;
		protected DateTime _uPDATETIME;
		protected string _oPERATORID;

		#endregion

		#region Constructors

		public LineStatusSpecEntityData() { }

        public LineStatusSpecEntityData(string lINETYPE, string cONDITIONSTATUS, int cONDITIONSEQNO, string eQPNOLIST, DateTime uPDATETIME, string oPERATORID)
		{
			this._lINETYPE = lINETYPE;
			this._cONDITIONSTATUS = cONDITIONSTATUS;
			this._cONDITIONSEQNO = cONDITIONSEQNO;
			this._andEQPNOLIST = eQPNOLIST;
			this._uPDATETIME = uPDATETIME;
			this._oPERATORID = oPERATORID;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual string LINETYPE
		{
			get { return _lINETYPE; }
			set
			{				
				_lINETYPE = value;
			}
		}

		public virtual string CONDITIONSTATUS
		{
			get { return _cONDITIONSTATUS; }
			set
			{				
				_cONDITIONSTATUS = value;
			}
		}

		public virtual int CONDITIONSEQNO
		{
			get { return _cONDITIONSEQNO; }
			set { _cONDITIONSEQNO = value; }
		}

		public virtual string ANDEQPNOLIST
		{
			get { return _andEQPNOLIST; }
			set
			{				
				_andEQPNOLIST = value;
			}
		}
        public virtual string OREQPNOLIST
        {
            get { return _orEQPNOLIST; }
            set
            {
                _orEQPNOLIST = value;
            }
        }
		public virtual DateTime UPDATETIME
		{
			get { return _uPDATETIME; }
			set { _uPDATETIME = value; }
		}

		public virtual string OPERATORID
		{
			get { return _oPERATORID; }
			set
			{				
				_oPERATORID = value;
			}
		}

		

		#endregion
	}
	#endregion
}