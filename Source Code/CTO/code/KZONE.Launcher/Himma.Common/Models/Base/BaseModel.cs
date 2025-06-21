using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Models.Base
{
    /// <summary>
    /// 业务实体基类
    /// </summary>
    public class BaseModel
    {
        public string Id { get; set; }

        public string ControlLeft { get; set; }
        public string ControlTop { get; set; }
    }
}
