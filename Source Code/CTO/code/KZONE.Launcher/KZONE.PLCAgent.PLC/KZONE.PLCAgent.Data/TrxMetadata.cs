using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// trx metadata class
	/// </summary>
	[Serializable]
	public class TrxMetadata
	{
		/// <summary>
		/// get is init trigger end (0=not end,1=end)
		/// </summary>
		internal int InitTriggerEnd;

		/// <summary>
		/// get Trx的名稱
		/// </summary>
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// get trx type (Receive/Send)
		/// </summary>
		public TrxTypeEnum TrxType
		{
			get;
			internal set;
		}

		/// <summary>
		/// get trx triggercondition (ON/OFF/CHANGE)
		/// </summary>
		public TrxTriggerConditionEnum TriggerCondition
		{
			get;
			internal set;
		}

		/// <summary>
		/// get node no
		/// </summary>
		public string NodeNo
		{
			get;
			internal set;
		}
	}
}
