using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// INT expression class
	/// </summary>
	public static class ExpressionINT
	{
		/// <summary>
		/// INT decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~2</param>		
		/// <param name="itemBOffset">item bit offset(10 base),range=0~15</param>
		/// <param name="itemBPoints">item bit points(10 base),range=1~16</param>
		/// <param name="iodata">source plc io data</param>
		/// <returns>ushort value</returns>
		public static ushort Decode(int evtStartAddress10, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
			byte[] buffer = new byte[8];
			int bufOffset = 0;
			int startbyte = (evtStartAddress10 + itemWOffset) * 2;
			for (int i = 0; i < itemWPoints; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Buffer.BlockCopy(iodata, startbyte + i * 2 + j, buffer, bufOffset, 1);
					bufOffset++;
				}
			}
			ulong[] value = new ulong[1];
			Buffer.BlockCopy(buffer, 0, value, 0, 8);
			return (ushort)(value[0] >> itemBOffset & (ulong)(Math.Pow(2.0, (double)itemBPoints) - 1.0));
		}

		/// <summary>
		/// INT encode method,may throw exception
		/// </summary>
		/// <param name="intString">uint string data</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=1~2</param>		
		/// <param name="itemBOffset">item bit offset(10 base),range=0~15</param>
		/// <param name="itemBPoints">item bit points(10 base),range=1~16</param>
		/// <param name="iodata">dest plc io data</param>
		public static void Encode(string intString, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
		{
			byte[] buffer = new byte[8];
			int bufOffset = 0;
			int startbyte = itemWOffset * 2;
			for (int i = 0; i < itemWPoints; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Buffer.BlockCopy(iodata, startbyte + i * 2 + j, buffer, bufOffset, 1);
					bufOffset++;
				}
			}
			ulong[] value = new ulong[1];
			Buffer.BlockCopy(buffer, 0, value, 0, 8);

            //if (string.IsNullOrWhiteSpace(intString))
            //{
            //    intString = "0";
            //}
			ulong uvalue = (ulong)Convert.ToUInt16(intString);
			ulong mask = (ulong)(Math.Pow(2.0, (double)itemBPoints) - 1.0);
			if (0uL > uvalue || mask < uvalue)
			{
				throw new ArgumentOutOfRangeException("intString", string.Format("NOT 0 <= {0} <= 2^{1}-1", uvalue, itemBPoints));
			}
			value[0] |= (uvalue & mask) << itemBOffset;
			Buffer.BlockCopy(value, 0, buffer, 0, 8);
			bufOffset = 0;
			startbyte = itemWOffset * 2;
			for (int i = 0; i < itemWPoints; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Buffer.BlockCopy(buffer, bufOffset, iodata, startbyte + i * 2 + j, 1);
					bufOffset++;
				}
			}
		}
	}
}
