using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// trx triggercondition enumeration
	/// </summary>
	public enum TrxTriggerConditionEnum : byte
	{
		/// <summary>
		/// none
		/// </summary>
		NONE,
		/// <summary>
		/// bit 1-&gt;0 trigger
		/// </summary>
		OFF,
		/// <summary>
		/// bit 0-&gt;1 trigger
		/// </summary>
		ON,
		/// <summary>
		/// change trigger
		/// </summary>
		CHANGE
	}
}
