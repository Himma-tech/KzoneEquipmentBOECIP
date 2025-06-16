using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// Save Trace Style
	/// </summary>
	public enum LogStyleEnum : byte
	{
		/// <summary>
		/// No Save Log
		/// </summary>
		HEAD,
		/// <summary>
		/// Save Raw Data
		/// </summary>
		RAWDATA,
		/// <summary>
		/// Save Raw data and item data
		/// </summary>
		DETAIL
	}
}
