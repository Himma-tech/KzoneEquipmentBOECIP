using Himma.Common.Enums;
using Himma.Common.Service;
using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Quartz.Logging.OperationName;
using System.Windows;
using System.Configuration;
using Himma.Common.Log;
using Himma.Common.Service.ContractService;
using Himma.Common.Service.Base;

namespace Himma.Common.Service
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    /// <summary>
    /// 业务入口类
    /// </summary>
    public class ServiceWrapper : IServiceWapper
    {
        #region Field
        private static List<BaseService> _services;
        private const string _logName = "System";
        #endregion

        #region constructors
        public ServiceWrapper()
        {
            _services = new List<BaseService>();
            this.ResetServices();
            this.ConfigServices();
        }
        #endregion

        public void Intial()
        {
            this.CachedBaseData();
        }

        /// <summary>
        /// 重置服务列表
        /// </summary>
        private void ResetServices()
        {
            _services = new List<BaseService>();
        }

        /// <summary>
        /// 开启服务的具体执行方法
        /// </summary>
        private void StartRunnableServices()
        {
            _services.ForEach(m =>
            {
                if (m is RunnableService)
                    (m as RunnableService).Start();
            });
        }

        /// <summary>
        /// 配置服务列表
        /// </summary>
        protected virtual void ConfigServices()
        {
            _services.Add(new SysParamService());
           
            
            Task.Run(async () => { await ConfigAsync(); });
        }
        public IScheduler Scheduler;
        private async Task ConfigAsync()
        {
            Quartz.Logging.LogProvider.SetCurrentLogProvider(new Job.LogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            Scheduler = await factory.GetScheduler();

            // and start it off
            await Scheduler.Start();

            IJobDetail job_p = JobBuilder.Create<Job.LogClearTaskJob>()
            .WithIdentity("job_LogClear", "group1")
            .Build();
            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger_p = TriggerBuilder.Create()
                .WithIdentity("trigger_LogClear", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(29) //原来是10 改为29
                .RepeatForever())
                .Build();
            await Scheduler.ScheduleJob(job_p, trigger_p);

            IJobDetail job_sp = JobBuilder.Create<Job.PoolStatusTaskJob>()
                .WithIdentity("job_PoolStatus", "group1")
                .Build();
            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger_sp = TriggerBuilder.Create()
                .WithIdentity("trigger_PoolStatus", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(2)
                    .RepeatForever())
                .Build();
            await Scheduler.ScheduleJob(job_sp, trigger_sp);


            IJobDetail job_PoolPing = JobBuilder.Create<Job.PoolPingTaskJob>()
           .WithIdentity("job_PoolPing", "group1")
           .Build();
            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger_PoolPing = TriggerBuilder.Create()
                .WithIdentity("trigger_PoolPing", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                .RepeatForever())
                .Build();
            await Scheduler.ScheduleJob(job_PoolPing, trigger_PoolPing);
        }
        /// <summary>
        /// 缓存基础数据
        /// </summary>
        private void CachedBaseData()
        {
            _services.ForEach(m =>
            {
                if (m.EnabledCached && !(m is SysParamService)) m.Cache();
            });
        }

        /// <summary>
        /// 启动所有服务
        /// </summary>
        public void StartAllServices()
        {
            LogHelper.Info("启动所有服务...", _logName);
            this.StartRunnableServices();
        }

        /// <summary>
        /// 停止所有服务
        /// </summary>
        public void StopAllServices()
        {
            LogHelper.Info("停止所有服务...", _logName);
            _services.ForEach(m =>
            {
                if (m is RunnableService)
                    (m as RunnableService).Stop();
            });
        }

        /// <summary>
        /// 关闭某一个服务
        /// </summary>
        /// <param name="serviceName"></param>
        public void StopService(string serviceName)
        {
            var svs = _services.FirstOrDefault(m => m.ServiceName == serviceName);
            if (svs == null) throw new NotSupportedException("sevice not found~");

            var rsvs = svs as RunnableService;
            if (rsvs == null) throw new NotSupportedException("not a runnable service~");

            if (rsvs.ServiceRunStatus == RunStatus.Running)
                rsvs.Stop();
        }

        /// <summary>
        /// 开启某个服务
        /// </summary>
        /// <param name="serviceName"></param>
        public void StartService(string serviceName)
        {
            var svs = _services.FirstOrDefault(m => m.ServiceName == serviceName);
            if (svs == null) throw new NotSupportedException("sevice not found~");

            var rsvs = svs as RunnableService;
            if (rsvs == null) throw new NotSupportedException("not a runnable service~");

            if (rsvs.ServiceRunStatus == RunStatus.Stopped)
                rsvs.Start();
        }

        /// <summary>
        /// 根据服务名称，获取服务实例
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static BaseService GetService(string serviceName)
        {
            return _services.FirstOrDefault(m => m.ServiceName == serviceName);
        }
    }
}
