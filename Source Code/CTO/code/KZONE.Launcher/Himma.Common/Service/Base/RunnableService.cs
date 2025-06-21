using Himma.Common.Enums;
using Himma.Common.Log;
using Himma.Common.Service.ContractService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Service.Base
{
    /// <summary>
    /// 可运行，监控服务类
    /// </summary>
    public abstract class RunnableService : BaseService, IRunnablaService
    {

        public RunStatus ServiceRunStatus { get; protected set; }

        // 是否自动启用task线程
        private bool startTaskAuto = true;
        private bool isEnabledMQTask = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RunnableService()
        {
            ServiceRunStatus = RunStatus.Stopped;
        }
        public RunnableService(bool autoTask, bool cached, string serviceName, bool enabledMQ = false)
            : base(cached, serviceName)
        {
            startTaskAuto = autoTask;
            isEnabledMQTask = enabledMQ;
        }

        // 执行监控的抽象方法，由派生类来实现
        public virtual void ExecuteMonitor() { }

       

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            try
            {
                if (ServiceRunStatus != RunStatus.Running)
                {
                    ServiceRunStatus = RunStatus.Running;
                    if (!isEnabledMQTask)
                    {
                        if (startTaskAuto)
                            StartThread(ExecuteMonitor);
                        else ExecuteMonitor();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"启动失败，错误提示【{ex}】", "system");
            }

        }

        /// <summary>
        /// 终止服务
        /// </summary>
        public void Stop()
        {
            ServiceRunStatus = RunStatus.Stopped;
        }
    }
}
