using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.PLCAgent.PLC.KZONE.PLCAgent.Data
{
    public class ExpressionFLOAT
    {
        /// <summary>
        /// FLOAT decode method
        /// </summary>
        /// <param name="evtStartAddress10">起始地址(10进制)</param>
        /// <param name="itemWOffset">item项word偏移量</param>
        /// <param name="itemWPoints">item项占用word大小</param>
        /// <param name="itemBOffset">item项bit偏移量</param>
        /// <param name="itemBPoints">item项bit占用大小</param>
        /// <param name="iodata">PLC源数据地址</param>
        /// <returns>32位单精度浮点数</returns>
        public static float Decode(int evtStartAddress10, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
        {
            byte[] buffer = new byte[4];//读取数据长度
            int bufOffset = 0;
            int startbyte = (evtStartAddress10 + itemWOffset) * 2;//起始地址---按字节byte计算
            for (int i = 0; i < itemWPoints; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Buffer.BlockCopy(iodata, startbyte + i * 2 + j, buffer, bufOffset, 1);
                    bufOffset++;
                }
            }
            float resF = BitConverter.ToSingle(buffer, 0);

            return resF;
        }
        /// <summary>
        /// FLOAT decode method
        /// </summary>
        /// <param name="floatString">源数据</param>
        /// <param name="itemWOffset">item项word偏移量</param>
        /// <param name="itemWPoints">item项占用word大小</param>
        /// <param name="itemBOffset">item项bit偏移量</param>
        /// <param name="itemBPoints">item项bit占用大小</param>
        /// <param name="iodata">PLC目标地址</param>
        public static void Encode(string floatString, int itemWOffset, int itemWPoints, int itemBOffset, int itemBPoints, short[] iodata)
        {
            byte[] buffer = new byte[4];//数据暂存区
            int bufOffset = 0;//暂存区偏移量
            int startbyte = 0;//PLC地址起始字节

            float f = Convert.ToSingle(floatString);
            buffer = BitConverter.GetBytes(f);//float转byte数组

            for (int i = 0; i < itemWPoints; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Buffer.BlockCopy(buffer, bufOffset, iodata, startbyte + i * 2 + j, 1);//数据写入PLC目标地址
                    bufOffset++;
                }
            }
        }
    }
}
