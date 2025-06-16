using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// internal flag enumeration
	/// </summary>
	[Flags]
	public enum InternalFlagsEnum : ushort
	{
		/// <summary>
		/// none
		/// </summary>
		None = 0,
		/// <summary>
		/// is read
		/// </summary>
		IsTrxRead = 1,
		/// <summary>
		/// is write
		/// </summary>
		IsTrxWrite = 2,
		/// <summary>
		/// is write raw
		/// </summary>
		IsTrxRawWrite = 4,
		/// <summary>
		/// is trigger
		/// </summary>
		IsTrxTrigger = 8,
		/// <summary>
		/// is random write
		/// </summary>
		IsTrxRandWrite = 128,
		/// <summary>
		/// is direct read
		/// </summary>
		IsDirectRead = 16,
		/// <summary>
		/// is sync read
		/// </summary>
		IsSyncRead = 32,
		/// <summary>
		/// is sync write
		/// </summary>
		IsSyncWrite = 64
	}
}
