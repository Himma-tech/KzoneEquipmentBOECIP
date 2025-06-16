using System;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// BIT expression class
	/// </summary>
	public static class ExpressionBIT
	{
		/// <summary>
		/// BIT decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemBOffset">item bit offset(10 base),range=0~N</param>
		/// <param name="itemBPoints">item bit points(10 base),range=1~M</param>
		/// <param name="iodata">source plc io data</param>
		/// <returns>bit string(從低到高)</returns>
		public static string Decode(int evtStartAddress10, int itemBOffset, int itemBPoints, short[] iodata)
		{
			StringBuilder sb = new StringBuilder();
			int wstart = (evtStartAddress10 + itemBOffset) / 16;
			int bstart = (evtStartAddress10 + itemBOffset) % 16;
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
		/// BIT encode method
		/// </summary>
		/// <param name="bitString">bit string data(從低到高)</param>
		/// <param name="itemBOffset">item bit offset(10 base),range=0~N</param>
		/// <param name="itemBPoints">item bit points(10 base),range=1~M</param>
		/// <param name="iodata">dest plc io data</param>
		public static void Encode(string bitString, int itemBOffset, int itemBPoints, short[] iodata)
		{
			int wstart = itemBOffset / 16;
			int bstart = itemBOffset % 16;
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
					int expr_4D_cp_1 = wstart;
					iodata[expr_4D_cp_1] |= mask;
					bstart++;
				}
			}
		}
	}
}
