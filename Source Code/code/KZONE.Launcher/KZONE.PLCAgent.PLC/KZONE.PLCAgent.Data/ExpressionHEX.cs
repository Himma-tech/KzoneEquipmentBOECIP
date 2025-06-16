using System;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// HEX expression class
	/// </summary>
	public static class ExpressionHEX
	{
		/// <summary>
		/// HEX decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>
		/// <param name="itemBOffset">item bit offset(10 base),range=0 or 4 or 8 or 12</param>
		/// <param name="itemBPoints">item bit points(10 base),range=4~R (R is multiple of 4)</param>
		/// <param name="iodata">source plc io data</param>
		/// <returns>string value</returns>
		public static string Decode(int evtStartAddress10, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
			StringBuilder sb = new StringBuilder();
			StringBuilder wordhex = new StringBuilder();
			int loop = itemBPoints / 4;
			int wstart = evtStartAddress10 + itemWOffset;
			int bstart = itemBOffset / 4;
			for (int i = 0; i < loop; i++)
			{
				if (bstart > 3)
				{
					bstart = 0;
					wstart++;
					sb.Append(wordhex.ToString());
					wordhex.Clear();
				}
				string h = Convert.ToString(iodata[wstart] >> bstart * 4 & 15, 16).ToUpper();
				wordhex.Insert(0, h);
				bstart++;
			}
			sb.Append(wordhex.ToString());
			return sb.ToString();
		}

		/// <summary>
		/// HEX encode method
		/// </summary>
		/// <param name="hexString">hex string data</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>	
		/// <param name="itemBOffset">item bit offset(10 base),range=0 or 4 or 8 or 12</param>
		/// <param name="itemBPoints">item bit points(10 base),range=4~R (R is multiple of 4)</param> 
		/// <param name="iodata">dest plc io data</param>
		public static void Encode(string hexString, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
			int loop = itemBPoints / 4;
			int wstart = itemWOffset;
			int bstart = itemBOffset / 4;
			int offset = 0;
			int wc = 0;
			int wd = bstart;
			if (hexString.Length < loop)
			{
				hexString = hexString.PadLeft(loop, '0');
			}
			for (int i = 0; i < loop; i++)
			{
				if (bstart > 3)
				{
					string tmp = hexString.Substring(offset, wc);
					short mask = (short)(Convert.ToInt16(tmp, 16) << wd * 4);
					int expr_6E_cp_1 = wstart;
					iodata[expr_6E_cp_1] |= mask;
					offset += wc;
					wc = 0;
					wd = 0;
					bstart = 0;
					wstart++;
				}
				wc++;
				bstart++;
			}
			if (loop - offset > 0)
			{
				string tmp = hexString.Substring(offset, loop - offset);
				short mask = (short)(Convert.ToInt16(tmp, 16) << wd * 4);
				int expr_E3_cp_1 = wstart;
				iodata[expr_E3_cp_1] |= mask;
			}
		}
	}
}
