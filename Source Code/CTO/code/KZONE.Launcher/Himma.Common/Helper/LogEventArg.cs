using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Helper.Log
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    /// <summary>
    /// 日志事件类
    /// </summary>
    public class LogEventArg : EventArgs
    {
        public string Message { get; set; }
        public string LogType { get; set; }
    }
}
