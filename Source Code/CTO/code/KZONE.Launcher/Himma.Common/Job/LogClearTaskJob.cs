using Himma.Common.Log;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Job
{
    internal class LogClearTaskJob : IJob
    {
        private string _logName = "ClearLog";
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                try
                {

                    LogHelper.Debug("LogClearTaskJob Run", _logName);
                    LogHelper.ClearLogObj();
                    LogHelper.Debug("ClearLogObj Execute", _logName);
                    if (GlobalConfig.IsUseLogBackups == 1)
                    {
                        var dt = DateTime.Now;
                        if (dt.Hour == 7 || dt.Hour == 19)//本判断只满足外部 30分钟调度器
                        {
                            if (dt.Minute <= 30)
                            {
                                await LogHelper.ClearAndBackupAsync();
                                LogHelper.Debug("ClearAndBackupAsync Execute", _logName);
                            }
                        }
                    }
                    else
                    {
                        await LogHelper.ClearAndBackupAsync();
                        LogHelper.Debug("ClearAndBackupAsync Execute", _logName);
                    }
                    GC.Collect();
                    LogHelper.Debug("GC Collect Execute", _logName);

                }
                catch (Exception ex)
                {

                    LogHelper.Error($"LogClearTaskJob Error {ex.ToString()}", _logName);
                }
            });

        }
    }
}
