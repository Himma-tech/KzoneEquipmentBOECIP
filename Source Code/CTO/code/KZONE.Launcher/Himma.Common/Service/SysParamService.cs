using Himma.Common.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Himma.Common.Log;
using Himma.Common.Service.Base;
using Himma.Common.Service.ContractService;
using Himma.Common.BaseData.Func;
using Himma.Common.BaseData.Func;
using Himma.Common.Models;
using System.Windows.Forms;

namespace Himma.Common.Service
{
    public class SysParamService : RunnableService, IService
    {
        /// <summary>
        /// 基础信息操作
        /// </summary>
        //private readonly SysParamDataAccess _sysParamDataAccess = new SysParamDataAccess();  
        //private readonly MasterDataAccess _masterDataAccess = new MasterDataAccess();
        //ConnectionMultiplexer redis;

        /// <summary>
        /// 日志名称
        /// </summary>
        private const string _logName = "SystemParam";

        // 需要做缓存
        public SysParamService() : base(true, true, Const.SYSPARAM_SERVICE) { }

        //protected List<SysParamInfo> GetAll()
        //{
        //    return _sysParamDataAccess.QueryAll();
        //}

        //protected async Task<List<SysParamInfo>> GetAllAsync()
        //{
        //    return await _sysParamDataAccess.QueryAllAsync();
        //}

        public override void Cache()
        {
            //初始化 配置线程池最大处理数
            //ThreadPool.SetMinThreads(GlobalConfig.RunThreadNum, GlobalConfig.RunThreadNum);

            LogHelper.Debug($"开始缓存系统参数配置[SystemParam]...", _logName);
            //base.Cache(Const.CACHED_SYSPARAM, GetAll());

            LogHelper.Debug($"初始化，开始执行WaterCheckMonitor", _logName);
            if (GlobalConfig.WarterCheck_ProcessIsOn == "1")
            {
                WaterCheckMonitor();
            }
        }

        public override async void ExecuteMonitor()
        {
            while (ServiceRunStatus == RunStatus.Running)
            {
                try
                {

                    //List<SysParamInfo> sysParams = await GetAllAsync();
                    //var sysParamInfos = CacheManager.Get(Const.CACHED_SYSPARAM) as List<SysParamInfo>;
                    //foreach (var v in sysParamInfos)
                    //{
                    //    var currVal = sysParams.FirstOrDefault(t => t.Id == v.Id)?.ParamValue;
                    //    if (v.ParamValue != currVal)
                    //    {
                    //        v.ParamValue = currVal;
                    //        LogHelper.Debug($"参数编码[{v.Id}] 值更新为[{v.ParamValue}]", _logName);
                    //    }
                    //    ;
                    //}
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"SysParamService执行异常，{ex}", _logName);
                }
                finally
                {
                    await Task.Delay(GlobalConfig.SysParamInterval);
                }
            }
        }

        private void WaterCheckMonitor()
        {
            CommonFunc.StartThread(() =>
            {
                while (true)
                {
                    StartMonitor();
                    Thread.Sleep(GlobalConfig.MornitorProcessInterval);
                }
            });
        }

        private void StartMonitor()
        {
            try
            {
                Process p = new Process();
                string currentProcessPath = Directory.GetCurrentDirectory(); //获取当前程序进程路径
                string processName = GlobalConfig.ProcessName_WaterCheck;//监控的进程名
                if (string.IsNullOrEmpty(processName))
                {
                    LogHelper.Error("监控的进程名为空，请检查配置信息！", "hcs");
                    return;
                }
                else
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    if (processes.Length < 1)
                    {
                        string needStartProcessPath = GlobalConfig.ProcessDirectory_WaterCheck;//需要打开的进程路径
                        string needStartBatName = GlobalConfig.BatName_WaterCheck;
                        Directory.SetCurrentDirectory(needStartProcessPath);
                        p.StartInfo.FileName = needStartBatName;
                        p.Start();
                       
                    }
                    else
                    {
                        if (!currentProcessPath.Contains("PTF UI"))
                        {
                            Directory.SetCurrentDirectory(GlobalConfig.ProcessDirectory_HCS);//若外部进程已经打开，需要还原本程序路径
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"监控进程方法存在未知异常！异常信息：{ex}", "hcs");
                return;
            }
        }

        public void Update(string pNo, string pVal)
        {
            //_sysParamDataAccess.SaveValue(new SysParamInfo
            //{
            //    Id = pNo,
            //    ParamValue = pVal
            //});
            //Cache(Const.CACHED_SYSPARAM, GetAll());
        }
    }
}
