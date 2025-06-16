using System;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// ASCII expression class
	/// </summary>
	public static class ExpressionASCII
	{
		/// <summary>
		/// ASCII decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>	
		/// <param name="itemBOffset">item bit offset(10 base),range=0 or 8</param>
		/// <param name="itemBPoints">item bit points(10 base),range=8~R (R is multiple of 8)</param>
		/// <param name="iodata">source plc io data</param>
		/// <returns>string value</returns>
		public static string Decode(int evtStartAddress10, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
			StringBuilder sb = new StringBuilder();
			int loop = itemBPoints / 8;
			int wstart = evtStartAddress10 + itemWOffset;
			int bstart = itemBOffset / 8;
			for (int i = 0; i < loop; i++)
			{
				if (bstart > 1)
				{
					bstart = 0;
					wstart++;
				}
				sb.Append((char)(iodata[wstart] >> bstart * 8 & 255));
				bstart++;
			}
			return sb.ToString().Replace('\0', ' ');
		}

		/// <summary>
		/// ASCII encode method
		/// </summary>
		/// <param name="ascString">ascii string data</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~M</param>	
		/// <param name="itemBOffset">item bit offset(10 base),range=0 or 8</param>
		/// <param name="itemBPoints">item bit points(10 base),range=8~R (R is multiple of 8)</param>
		/// <param name="iodata">dest plc io data</param>
		public static void Encode(string ascString, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
			int loop = itemBPoints / 8;
			int wstart = itemWOffset;
			int bstart = itemBOffset / 8;
			
			if (ascString == null)
			{
				ascString = string.Empty;
			}

			if (ascString.Length < loop)
			{
				ascString = ascString.PadRight(loop, ' ');
			}
            //bruce.zhao 2018/11/30 AsciiÎª¿ÕÊ±Òì³£
            //if (ascString == null)
            //{
            //    ascString = "";
            //    ascString = ascString.PadRight(loop, ' ');
            //}

		    for (int i = 0; i < loop; i++)
			{
				if (bstart > 1)
				{
					bstart = 0;
					wstart++;
				}
				short mask = (short)(Convert.ToInt16(ascString[i]) << bstart * 8);
				int expr_63_cp_1 = wstart;
				iodata[expr_63_cp_1] |= mask;
				bstart++;
			}
		}
	}
}
