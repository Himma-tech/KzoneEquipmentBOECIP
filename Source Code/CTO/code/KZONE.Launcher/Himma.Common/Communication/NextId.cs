using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Communication
{
    internal class NextId
    {
        private static int _id = 0;
        internal static int Get()
        {
            return Interlocked.Increment(ref _id);
        }
    }
}
