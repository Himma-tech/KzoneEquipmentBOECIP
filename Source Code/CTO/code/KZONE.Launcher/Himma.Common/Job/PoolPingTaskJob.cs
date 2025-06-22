using Himma.Common.Enums;
using Himma.Common.Log;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Job
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    /// <summary>
    /// 连接池自动ping定时任务
    /// </summary>
    internal class PoolPingTaskJob : IJob
    {
        private string _logName = "PoolPing";
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                try
                {
                    LogHelper.Debug("PoolPingTaskJob Run", _logName);
                    if (CacheManager.Current.ContainsKey(Const.CACHED_PLC_BAKING))
                    {

                        //var bakingPlcs = CacheManager.Current[Const.CACHED_PLC_BAKING] as List<BakingPLC>;
                        //bakingPlcs.ForEach(m =>
                        //{
                        //    if (m.UsedFlag == 1)
                        //    {
                        //        if (m.RunStatus == RunStatus.Running)
                        //        {
                        //            m.AutoPing();
                        //            LogHelper.Debug($"AutoPing {m.PLCName}", _logName);
                        //        }
                        //    }
                        //});
                    }

                    if (CacheManager.Current.ContainsKey(Const.CACHED_PLC_TRANS))
                    {
                        //var transPlcs = CacheManager.Current[Const.CACHED_PLC_TRANS] as List<TransPLC>;
                        //transPlcs.ForEach(m =>
                        //{
                        //    if (m.UsedFlag == 1)
                        //    {
                        //        if (m.RunStatus == RunStatus.Running)
                        //        {
                        //            m.AutoPing();
                        //            LogHelper.Debug($"AutoPing {m.PLCName}", _logName);
                        //        }
                        //    }

                        //});
                    }
                    if (CacheManager.Current.ContainsKey(Const.CACHED_PLC_TRANS))
                    {
                        //var exchangePlcs = CacheManager.Current[Const.CACHED_PLC_EXCHANGE] as List<PuckExchangePLC>;
                        //exchangePlcs.ForEach(m =>
                        //{
                        //    if (m.UsedFlag == 1)
                        //    {
                        //        if (m.RunStatus == RunStatus.Running)
                        //        {
                        //            m.AutoPing();
                        //            LogHelper.Debug($"AutoPing {m.PLCName}", _logName);
                        //        }
                        //    }
                        //});
                    }





                }
                catch (Exception ex)
                {

                    LogHelper.Error($"PoolPing Error {ex.ToString()}", _logName);
                }
            });

        }
    }
}
