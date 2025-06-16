using System;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// BCD expression class
	/// </summary>
	public static class ExpressionBCD
	{
		/// <summary>
		/// BCD decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>	
		/// <param name="itemBOffset">item bit offset(10 base),range=0</param>
		/// <param name="itemBPoints">item bit points(10 base),range=16~R (R is multiple of 16)</param>
		/// <param name="iodata">source plc io data</param>
		/// <returns>string value</returns>
		public static string Decode(int evtStartAddress10, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
            return ExpressionBCD.Decode(evtStartAddress10, itemWOffset, itemWPoints, itemBOffset, itemBPoints, iodata, true);
		}

		/// <summary>
		/// BCD decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>	
		/// <param name="itemBOffset">item bit offset(10 base),range=0</param>
		/// <param name="itemBPoints">item bit points(10 base),range=16~R (R is multiple of 16)</param>
		/// <param name="iodata">source plc io data</param>
		/// <param name="flip">true=flip</param>
		/// <returns>string value</returns>
        public static string Decode(int evtStartAddress10, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata, bool flip = true)
		{
			StringBuilder sb = new StringBuilder();
			int loop = itemBPoints / 16;
			int wstart = evtStartAddress10 + itemWOffset;
			for (int i = 0; i < loop; i++)
			{
				string h = Convert.ToString(iodata[wstart] >> 12 & 15, 16).ToUpper();
				string h2 = Convert.ToString(iodata[wstart] >> 8 & 15, 16).ToUpper();
				string h3 = Convert.ToString(iodata[wstart] >> 4 & 15, 16).ToUpper();
				string h4 = Convert.ToString((int)(iodata[wstart] & 15), 16).ToUpper();
				if (flip)
				{
					sb.Append(h3);
					sb.Append(h4);
					sb.Append(h);
					sb.Append(h2);
				}
				else
				{
					sb.Append(h);
					sb.Append(h2);
					sb.Append(h3);
					sb.Append(h4);
				}
				wstart++;
			}
			return sb.ToString();
		}

		/// <summary>
		/// BCD encode method
		/// </summary>
		/// <param name="bcdString">bcd string data</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>	
		/// <param name="itemBOffset">item bit offset(10 base),range=0 </param>
		/// <param name="itemBPoints">item bit points(10 base),range=16~R (R is multiple of 16)</param>
		/// <param name="iodata">dest plc io data</param>
		public static void Encode(string bcdString, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
            ExpressionBCD.Encode(bcdString, itemWOffset, itemWPoints, itemBOffset, itemBPoints, iodata, true);
		}

		/// <summary>
		/// BCD encode method
		/// </summary>
		/// <param name="bcdString">bcd string data</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>	
		/// <param name="itemBOffset">item bit offset(10 base),range=0 </param>
		/// <param name="itemBPoints">item bit points(10 base),range=16~R (R is multiple of 16)</param>
		/// <param name="iodata">dest plc io data</param>
		/// <param name="flip">true=flip</param>
        public static void Encode(string bcdString, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata, bool flip = true)
		{
			int loop = itemBPoints / 16;
			int wstart = itemWOffset;
			if (bcdString.Length < loop * 4)
			{
				bcdString = bcdString.PadLeft(loop * 4, '0');
			}
			for (int i = 0; i < loop; i++)
			{
				string h = bcdString.Substring(i * 4, 1);
				string h2 = bcdString.Substring(i * 4 + 1, 1);
				string h3 = bcdString.Substring(i * 4 + 2, 1);
				string h4 = bcdString.Substring(i * 4 + 3, 1);
				if (flip)
				{
					short m = (short)(Convert.ToInt16(h, 16) << 4);
					int expr_8D_cp_1 = wstart;
					iodata[expr_8D_cp_1] |= m;
					short m2 = Convert.ToInt16(h2, 16);
					int expr_B0_cp_1 = wstart;
					iodata[expr_B0_cp_1] |= m2;
					short m3 = (short)(Convert.ToInt16(h3, 16) << 12);
					int expr_D6_cp_1 = wstart;
					iodata[expr_D6_cp_1] |= m3;
					short m4 = (short)(Convert.ToInt16(h4, 16) << 8);
					int expr_FB_cp_1 = wstart;
					iodata[expr_FB_cp_1] |= m4;
				}
				else
				{
					short m = (short)(Convert.ToInt16(h, 16) << 12);
					int expr_127_cp_1 = wstart;
					iodata[expr_127_cp_1] |= m;
					short m2 = (short)(Convert.ToInt16(h2, 16) << 8);
					int expr_14C_cp_1 = wstart;
					iodata[expr_14C_cp_1] |= m2;
					short m3 = (short)(Convert.ToInt16(h3, 16) << 4);
					int expr_171_cp_1 = wstart;
					iodata[expr_171_cp_1] |= m3;
					short m4 = Convert.ToInt16(h4, 16);
					int expr_194_cp_1 = wstart;
					iodata[expr_194_cp_1] |= m4;
				}
				wstart++;
			}
		}
	}
}
