using Himma.Common.Job;
using Himma.Common.Service.Interface;
using Prism.Events;
using Prism.Ioc;
using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Quartz.Logging.OperationName;

namespace Himma.Common.Service
{
    public class CommonService : ICommService
    {
        #region Properties
        private readonly IEventAggregator _eventAggregator;
        private CancellationTokenSource _cancellationToken;

        public Action<object> ActionHandler { get; set; }

        protected bool IsRunning { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public Action<string> CurrentTimeAction;
        private IMainBLL _mainBLL;
        #endregion

        /// <summary>
        /// 通用服务示例
        /// </summary>
        /// <param name="iLoggerHelper"></param>
        /// <param name="eventAggregator"></param>
        public CommonService(IEventAggregator eventAggregator, IContainerProvider container, IMainBLL mainBLL)
        {
            _mainBLL = mainBLL;
            _eventAggregator = eventAggregator;
            IsRunning = false;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            _cancellationToken = new CancellationTokenSource();
            if (IsRunning) return;
            IsRunning = true;
            TurnOnOffService(IsRunning);
            ActionHandler?.Invoke(new object());
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Stop()
        {
            TurnOnOffService(IsRunning = false);
            _cancellationToken.Dispose();

        }

        /// <summary>
        /// 启动/关闭服务
        /// </summary>
        /// <param name="isRunning"></param>
        private void TurnOnOffService(bool isRunning)
        {
            Task.Run(() =>
            {
                _mainBLL.Init();
            }, _cancellationToken.Token);
            Task.Run(async () => { await ConfigAsync(); });
        }
        public IScheduler Scheduler;

        private async Task ConfigAsync()
        {
            Quartz.Logging.LogProvider.SetCurrentLogProvider(new LogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            Scheduler = await factory.GetScheduler();


            IJobDetail job_p = JobBuilder.Create<LogClearTaskJob>()
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

            //IJobDetail job_sp = JobBuilder.Create<PoolStatusTaskJob>()
            //    .WithIdentity("job_PoolStatus", "group1")
            //    .Build();
            //// Trigger the job to run now, and then repeat every 10 seconds
            //ITrigger trigger_sp = TriggerBuilder.Create()
            //    .WithIdentity("trigger_PoolStatus", "group1")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInMinutes(2)
            //        .RepeatForever())
            //    .Build();
            //await Scheduler.ScheduleJob(job_sp, trigger_sp);

            // IJobDetail job_PoolPing = JobBuilder.Create<PoolPingTaskJob>()
            //.WithIdentity("job_PoolPing", "group1")
            //.Build();
            //// Trigger the job to run now, and then repeat every 10 seconds
            // ITrigger trigger_PoolPing = TriggerBuilder.Create()
            //     .WithIdentity("trigger_PoolPing", "group1")
            //     .StartNow()
            //     .WithSimpleSchedule(x => x
            //         .WithIntervalInMinutes(1)
            //     .RepeatForever())
            //     .Build();
            // await Scheduler.ScheduleJob(job_PoolPing, trigger_PoolPing);

            //过程监控--按客户要求对设备部分参数进行监控 _DeviceMonitorJob
            // var DeviceMonitorCorn = GlobalConfig.DeviceMonitorCorn;

            //var deviceMonitorJob = JobBuilder.Create<DeviceMonitorJob>().WithIdentity("deviceMonitorJob", "group2").Build();
            //// 定义Cron触发器
            //ITrigger trigger_deviceMonitor = TriggerBuilder.Create()
            //                                      .WithIdentity("deviceMonitorJob", "group2")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(2)
            //        .RepeatForever())
            //    .Build();
            //await Scheduler.ScheduleJob(deviceMonitorJob, trigger_deviceMonitor);


            // and start it off
            await Scheduler.Start();
        }


    }
}
