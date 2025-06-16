using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// EXP expression class
	/// </summary>
	public static class ExpressionEXP
	{
		/// <summary>
		/// EXP decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=2</param>		
		/// <param name="iodata">source plc io data</param>
		/// <returns>float value</returns>
		public static float Decode_IEEE754(int evtStartAddress10, int itemWOffset, int itemWPoints, short[] iodata)
		{
			byte[] buffer = new byte[4];
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
			float[] value = new float[1];
			bufOffset = 0;
			for (int j = 0; j < 4; j++)
			{
				Buffer.BlockCopy(buffer, bufOffset, value, j, 1);
				bufOffset++;
			}
			return value[0];
		}

		/// <summary>
		/// EXP decode method
		/// </summary>		
		/// <param name="evtStartAddress10">event start address(10 base)</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=2</param>		
		/// <param name="iodata">source plc io data</param>
		/// <returns>string value</returns>
		public static string Decode(int evtStartAddress10, int itemWOffset, int itemWPoints, short[] iodata)
		{
			if (itemWPoints > 2)
			{
				throw new ArgumentOutOfRangeException("itemWPoints", string.Format(" itemWPoints NOT > 2", new object[0]));
			}
			int index = evtStartAddress10 + itemWOffset;
			return string.Format("{0}E{1}", iodata[index], iodata[index + 1]);
		}

		/// <summary>
		/// EXP encode method
		/// </summary>
		/// <param name="floatString">float string data</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=2</param>		
		/// <param name="iodata">dest plc io data</param>
		public static void Encode_IEEE754(string floatString, int itemWOffset, int itemWPoints, short[] iodata)
		{
			byte[] buffer = new byte[4];
			float[] value = new float[]
			{
				Convert.ToSingle(floatString)
			};
			int bufOffset = 0;
			for (int i = 0; i < 4; i++)
			{
				Buffer.BlockCopy(value, i, buffer, bufOffset, 1);
				bufOffset++;
			}
			bufOffset = 0;
			int startbyte = itemWOffset * 2;
			for (int j = 0; j < itemWPoints; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					Buffer.BlockCopy(buffer, bufOffset, iodata, startbyte + j * 2 + i, 1);
					bufOffset++;
				}
			}
		}

		/// <summary>
		/// EXP encode method
		/// </summary>
		/// <param name="expString">float string data</param>
		/// <param name="itemWOffset">item word offset(10 base),range=0~N</param>
		/// <param name="itemWPoints">item word points(10 base),range=2</param>		
		/// <param name="iodata">dest plc io data</param>
		public static void Encode(string expString, int itemWOffset, int itemWPoints, short[] iodata)
		{
			if (itemWPoints > 2)
			{
				throw new ArgumentOutOfRangeException("itemWPoints", string.Format(" itemWPoints NOT > 2", new object[0]));
			}
			string[] array = expString.Split(new char[]
			{
				'E',
				'e'
			});
			for (int i = 0; i < 2; i++)
			{
				iodata[itemWOffset + i] = Convert.ToInt16(array[i]);
			}
		}
	}
}
