using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Models.Base
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
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
