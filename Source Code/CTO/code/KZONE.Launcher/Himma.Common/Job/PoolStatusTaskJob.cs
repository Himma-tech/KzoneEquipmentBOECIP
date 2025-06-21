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
    /// 连接池状态定时任务
    /// </summary>
    internal class PoolStatusTaskJob : IJob
    {
        private string _logName = "PoolStatus";
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                try
                {
                    LogHelper.Debug("PoolStatusTaskJob Run", _logName);
                    if (CacheManager.Current.ContainsKey(Const.CACHED_PLC_BAKING))
                    {

                        //var bakingPlcs = CacheManager.Current[Const.CACHED_PLC_BAKING] as List<BakingPLC>;
                        //bakingPlcs.ForEach(m =>
                        //{
                        //    if (m.UsedFlag == 1)
                        //    {
                        //        if (m.RunStatus == RunStatus.Running)
                        //        {
                        //            LogHelper.Debug(m.GetPoolInfo(), _logName);
                        //            m.AutoClean();
                        //            LogHelper.Debug($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}] AutoCleanTaskJob", _logName);
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
                        //            LogHelper.Debug(m.GetPoolInfo(), _logName);
                        //            m.AutoClean();
                        //            LogHelper.Debug($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}] AutoCleanJob", _logName);
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
                        //            LogHelper.Debug(m.GetPoolInfo(), _logName);
                        //            m.AutoClean();
                        //            LogHelper.Debug($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}] AutoCleanJob", _logName);
                        //        }
                        //    }
                        //});
                    }




                    LogHelper.Debug(Common.DapperHelper.GetPoolInfo(), _logName);

                    CommonCSVHelper.ClearAndFlush();//一分钟刷新一次缓存写入
                    LogHelper.Debug("CSV ClearAndFlush Execute", _logName);

                }
                catch (Exception ex)
                {

                    LogHelper.Error($"PoolStatusTaskJob Error {ex.ToString()}", _logName);
                }
            });

        }
    }
}
