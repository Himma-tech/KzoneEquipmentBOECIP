using System;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// BIT expression class
	/// </summary>
	public static class ExpressionBIN
	{
		/// <summary>
		/// BIN decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>		
		/// <param name="itemBOffset">item bit offset(10 base),range=0~15</param>
		/// <param name="itemBPoints">item bit points(10 base),range=1~R</param>
		/// <param name="iodata">source plc io data</param>
		/// <returns>bit string(從低到高)</returns>
		public static string Decode(int evtStartAddress10, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
			StringBuilder sb = new StringBuilder();
			int wstart = evtStartAddress10 + itemWOffset;
			int bstart = itemBOffset;
			for (int p = 0; p < itemBPoints; p++)
			{
				if (bstart > 15)
				{
					bstart = 0;
					wstart++;
				}
				int mask = 1 << bstart;
				sb.Append(((int)iodata[wstart] & mask) / mask);
				bstart++;
			}
			return sb.ToString();
		}

		/// <summary>
		///  BIN encode method
		/// </summary>
		/// <param name="bitString">bit string data(從低到高)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>		
		/// <param name="itemBOffset">item bit offset(10 base),range=0~15</param>
		/// <param name="itemBPoints">item bit points(10 base),range=1~R</param>
		/// <param name="iodata">dest plc io data</param>
		public static void Encode(string bitString, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
			int wstart = itemWOffset;
			int bstart = itemBOffset;
			
			if (bitString.Length < itemBPoints)
			{
				bitString = bitString.PadRight(itemBPoints, '0');
			}
			for (int p = 0; p < itemBPoints; p++)
			{
				if (bstart > 15)
				{
					bstart = 0;
					wstart++;
				}
				if (bitString[p] != '1')
				{
					bstart++;
				}
				else
				{
					short mask = (short)(1 << bstart);
					int expr_69_cp_1 = wstart;
					iodata[expr_69_cp_1] |= mask;
					bstart++;
				}
			}
		}
	}
}
