using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.ConstantParameter
{
    public static class StringExtend
    {
        public static ulong And(this string input, string y, int baseFormat = 2, int len = 32)
        {
            string stri = input.PadRight(len, '0').Substring(0, len);
            string stry = y.PadRight(len, '0').Substring(0, len);
            ulong i = Convert.ToUInt64(stri, baseFormat);
            ulong _y = Convert.ToUInt64(stry, baseFormat);
            return i & _y;
        }

        public static string Or(this string input, string y, int baseFormat = 2, int len = 32)
        {
            input.PadRight(len, '0').Substring(0, len);
            y.PadRight(len, '0').Substring(0, len);
            ulong i = Convert.ToUInt64(input, baseFormat);
            ulong _y = Convert.ToUInt64(y, baseFormat);
            long result = (long)(i | _y);
            return Convert.ToString(result, baseFormat).PadRight(len, '0').Substring(0, len);
        }

        public static ulong Opp(this string input, int baseFormat = 2)
        {
            ulong i = Convert.ToUInt64(input, baseFormat);
            return ~i;
        }

        public static ulong Xor(this string input, string y, int baseFormat = 2)
        {
            ulong i = Convert.ToUInt64(input, baseFormat);
            ulong _y = Convert.ToUInt64(y, baseFormat);
            return i ^ _y;
        }
    }
}
