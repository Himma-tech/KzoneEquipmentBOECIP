using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// expression enumeration
	/// </summary>
	[Flags]
	public enum ItemExpressionEnum : ushort
	{
		/// <summary>
		/// none
		/// </summary>
		NONE = 0,
		/// <summary>
		/// "INT"= 轉碼成Uint16.
		/// </summary>
		INT = 1,
		/// <summary>
		/// "SINT"=轉碼成Int16.
		/// </summary>
		SINT = 2,
		/// <summary>
		/// "LONG"=轉碼成Uint32.
		/// </summary>
		LONG = 4,
		/// <summary>
		/// "SLONG"=轉碼成Int32.
		/// </summary>
		SLONG = 8,
		/// <summary>
		/// "EXP"=轉碼成Float.
		/// </summary>
		EXP = 16,
		/// <summary>
		/// "ASCII"= 轉碼成 ASCII.
		/// </summary>
		ASCII = 32,
		/// <summary>
		/// "HEX"=轉碼成 HEX.
		/// </summary>
		HEX = 64,
		/// <summary>
		/// "BIT"= 轉成Bit，只有 0 or 1.
		/// </summary>
		BIT = 128,
		/// <summary>
		/// "BIN" = 轉碼成Binary 0101001(用於Word的轉換)
		/// </summary>
		BIN = 256,
		/// <summary>
		/// "BCD"=轉碼成 BCD
		/// </summary>
		BCD = 512
	}
}
