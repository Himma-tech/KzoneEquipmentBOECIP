using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMSORTGRADERULE

	/// <summary>
	/// SBRMSORTGRADERULE object for NHibernate mapped table 'SBRM_SORTGRADERULE'.
	/// </summary>
	public class SortGradeRule
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINEID;
		protected string _sORTGRADE;
		protected int _pRORITY;
		protected DateTime _uPDATETIME;
		protected string _oPERATORID;

		#endregion

		#region Constructors

		public SortGradeRule() { }

		public SortGradeRule( string lINEID, string sORTGRADE, int pRORITY, DateTime uPDATETIME, string oPERATORID )
		{
			this._lINEID = lINEID;
			this._sORTGRADE = sORTGRADE;
			this._pRORITY = pRORITY;
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

		public virtual string LINEID
		{
			get { return _lINEID; }
			set
			{				
				_lINEID = value;
			}
		}

		public virtual string SORTGRADE
		{
			get { return _sORTGRADE; }
			set
			{				
				_sORTGRADE = value;
			}
		}

		public virtual int PRORITY
		{
			get { return _pRORITY; }
			set { _pRORITY = value; }
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