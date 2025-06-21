using Himma.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Models
{
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
