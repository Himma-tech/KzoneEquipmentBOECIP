using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Himma.Common.Communication
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    internal class NextId
    {
        private static int _id = 0;
        internal static int Get()
        {
            return Interlocked.Increment(ref _id);
        }
    }
}
