using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Service.Interface
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Henick
    /// 创建时间：2020/4/24 13:57:04
    /// </summary>
    public interface ICommService
    {
        Action<object> ActionHandler { get; set; }

        /// <summary>
        /// 启动服务
        /// </summary>
        void Start();

        /// <summary>
        /// 关闭服务
        /// </summary>
        void Stop();
    }
}
