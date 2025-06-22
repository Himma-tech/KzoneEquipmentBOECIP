using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    public class PLCSignalModel : EventArgs
    {
        public string name { get; set; }

        public dynamic data { get; set; }

        public int type { get; set; }
    }
}
