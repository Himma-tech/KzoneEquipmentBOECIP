using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// Trx type enumeration
	/// </summary>
	public enum TrxTypeEnum : byte
	{
		/// <summary>
		/// receive from plc
		/// </summary>
		Receive,
		/// <summary>
		/// send to plc
		/// </summary>
		Send
	}
}
