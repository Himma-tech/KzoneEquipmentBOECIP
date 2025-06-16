using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.PLCAgent.PLC.KZONE.PLCAgent.Data
{
    public class ExpressionDOUBLE
    {
        /// <summary>
        /// DOUBLE decode method
        /// </summary>
        /// <param name="evtStartAddress10">起始地址(10进制)</param>
        /// <param name="itemWOffset">item项word偏移量</param>
        /// <param name="itemWPoints">item项占用word大小</param>
        /// <param name="itemBOffset">item项bit偏移量</param>
        /// <param name="itemBPoints">item项bit占用大小</param>
        /// <param name="iodata">PLC源数据地址</param>
        /// <returns>64位双精度浮点数</returns>
        public static double Decode(int evtStartAddress10, int itemWOffset, int itemWPoint, int itemBOfftset, int itemBPoint, short[] iodata)
        {
            byte[] buffer = new byte[8];
            int bufOffset = 0;
            int startbyte = (evtStartAddress10 + itemWOffset) * 2;
            for (int i = 0; i < itemWPoint; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Buffer.BlockCopy(iodata, startbyte + i * 2 + j, buffer, bufOffset, 1);
                    bufOffset++;
                }
            }
            double resD = BitConverter.ToDouble(buffer, 0);
            return resD;
        }
        /// <summary>
        /// DOUBLE decode method
        /// </summary>
        /// <param name="doubleString">源数据</param>
        /// <param name="itemWOffset">item项word偏移量</param>
        /// <param name="itemWPoints">item项占用word大小</param>
        /// <param name="itemBOffset">item项bit偏移量</param>
        /// <param name="itemBPoints">item项bit占用大小</param>
        /// <param name="iodata">PLC目标数据地址</param>
        public static void Encode(string doubleString, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
        {
            byte[] buffer = new byte[8];
            int bufOffset = 0;
            int startbyte = 0;
            double d = Convert.ToDouble(doubleString);
            buffer = BitConverter.GetBytes(d);
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
