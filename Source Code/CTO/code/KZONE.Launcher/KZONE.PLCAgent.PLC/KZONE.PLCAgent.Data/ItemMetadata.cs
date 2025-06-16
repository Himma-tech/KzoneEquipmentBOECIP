using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// item metadata class
	/// </summary>
	[Serializable]
	public class ItemMetadata
	{
		/// <summary>
		/// get Item的名稱
		/// </summary>
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// get Device Code以Word為單位的Offset位置，Item在Group的Word的Offset位置，從0~n .
		/// </summary>
		public int WordOffset
		{
			get;
			internal set;
		}

		/// <summary>
		/// get Bit的Offset位置.
		/// <br>Case1:如果DeviceCode的單位為Word,是指從第幾個Word開始(w)的第幾個Bit(b),須配合woffset使用,從0~n.</br>
		/// <br>Case2:Item在Group的Bit的Offset位置，從第幾個Bit開始(b), 從0~n.</br>
		/// </summary>
		public int BitOffset
		{
			get;
			internal set;
		}

		/// <summary>
		/// get DeviceCode單位為是Word的使用長度.
		/// <br>Case1: Item在Group的佔用點位數,0~n</br>
		/// <br>If Event.DeviceCode=W or ZR device是以Word為單位.</br>
		/// </summary>									
		public int WordPoints
		{
			get;
			internal set;
		}

		/// <summary>
		/// get total bit points
		///             <br>Case1:Item在Group的佔用Word長度(w)，佔用的幾個bit的長度(b)</br>
		///             <br>Case2:Item在Group的佔用bit的長度.</br>
		/// </summary>	
		public int BitPoints
		{
			get;
			internal set;
		}

		/// <summary>
		/// get item expression
		/// <br>HEX =Hex and BCD混用.</br>
		/// <br>ASCII 轉碼成 ASCII.</br>
		/// <br>INT 轉碼成Uint16.</br>
		/// <br>SINT 轉碼成Int16.</br>
		/// <br>LONG 轉碼成Uint32.</br>
		/// <br>SLONG 轉碼成Int32.</br>
		/// <br>EXP 轉碼成Float.</br>
		/// <br>BIT 轉成Bit，只有 0 or 1.</br>
		/// <br>BIN 轉碼成Binary 0101001(用於Word的轉換)</br>
		/// </summary>
		public ItemExpressionEnum Expression
		{
			get;
			internal set;
		}
	}
}
