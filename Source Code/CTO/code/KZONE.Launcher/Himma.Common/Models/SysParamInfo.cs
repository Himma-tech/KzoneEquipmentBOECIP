using Himma.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Models
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    /// <summary>
    /// 系统参数类
    /// </summary>
    public class SysParamInfo : BaseModel
    {
        public string ParamName { get; set; }
        public string ParamType { get; set; }
        public string ParamValue { get; set; }
        public int ParamGroup { get; set; }
        public int ParamShowFlag { get; set; }
    }

    public class ParamGroup
    {
        public int ParamGroupID { get; set; }
        public string ParamGroupName { get; set; }
    }
}
