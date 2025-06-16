using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc adapter debug out level
	/// </summary>
	public enum PLCAdapterDebugOutLevel
	{
		/// <summary>
		/// trace level
		/// </summary>
		TRACE,
		/// <summary>
		/// debug level
		/// </summary>
		DEBUG,
		/// <summary>
		/// error level
		/// </summary>
		ERROR,
		/// <summary>
		/// exception level
		/// </summary>
		EXCEPTION,
		/// <summary>
		/// dump level
		/// </summary>
		DUMP
	}
}
