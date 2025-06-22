using Himma.Common.Service.Interface;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using System.Threading;
using System.Diagnostics;
using MessagePipe;
using StackExchange.Redis;
using static Quartz.Logging.OperationName;
using Himma.Common.Log;

namespace Himma.Common.Business
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    public class MainBLL : IMainBLL
    {
        /// <summary>
        /// 异步事件订阅
        /// </summary>
        //IAsyncSubscriber<HimmaCommon.MessageType, PLCVariableModel> _asyncSubscriber = GlobalMessagePipe.GetAsyncSubscriber<Com.Common.MessageType, PLCVariableModel>();

        //private Dictionary<int, Action<object>> funcDic;
        private readonly IContainerProvider _icontainerProvider;
        private readonly IContainerRegistry _containerRegistry;
        //private readonly IBusiness _business;//业务
        public readonly TaskFactory _taskFactory = new TaskFactory();
        //BasParaVariableDAL _basParaVariableDal = new BasParaVariableDAL();

        //入站信号量
        private static Semaphore _SemapIn = new Semaphore(4, 6);
        //出站信号量
        private static Semaphore _SemapOut = new Semaphore(4, 6);

        public MainBLL(IContainerProvider containerProvider, IContainerRegistry containerRegistry)
        {
            try
            {
                _icontainerProvider = containerProvider;
                _containerRegistry = containerRegistry;
                //_business = containerProvider.Resolve<IBusiness>(GlobalVariable.ProcessNo);
            }
            catch (Exception ex)
            {
                //_business = _business ?? containerProvider.Resolve<IBusiness>("BaseBll");
            }

        }
        /// <summary>
        /// 业务初始化
        /// </summary>
        public void Init()
        {
            try
            {
                //_business.InitComponent();
                PLCResolve();
                //_business.Init();
            }
            catch (Exception ex)
            {
                LogHelper.Error("程序初始化失败：" + ex);
            }
        }
        /// <summary>
        /// 初始化多PLC实例
        /// </summary>
        public void PLCResolve()
        {
            try
            {
                //foreach (var item in GlobalVariable.PLCParams)
                //{
                //    var _plc = _icontainerProvider.Resolve<IHimmaPLC>();
                //    _plc.Name = item.Name;
                //    _plc.ProcessNo = item.ProcessNo;
                //    _plc.StationCode = item.StationCode;
                //    _plc.ProcessCode = item.ProcessCode;
                //    _plc.EqptNo = item.EqptNo;
                //    //_plc.PLCNotifyEventArgs += _plc_PLCNotifyEventArgs;
                //    _plc.PLCInit(item.PLCIPAddress, item.PLCPort, int.Parse(GlobalVariable.PLCCycleTime), item.Name);
                //    _containerRegistry.RegisterInstance(_plc, item.PLCIPAddress); //将PlC放入缓存对象
                //}
                ////GlobalMessagePipe.GetSubscriber<string>().Subscribe((string str) =>
                ////{
                ////    Console.WriteLine(str);
                ////});
                //_asyncSubscriber.Subscribe(MessageType.GlobalPLC,
                //    async (_, x) => _plc_PLCNotifyEventArgs(_, x));
            }
            catch (Exception ex)
            {
                LogHelper.Error($"PLC实例化失败" + ex);
            }
        }

        /// <summary>
        /// 订阅处理方法
        /// </summary>
        /// <param name="obj"></param>
        //public void _plc_PLCNotifyEventArgs(PLCVariableModel plcVariableModel, object sender)
        //{
        //    var _st = Stopwatch.StartNew();
        //    var plc = _icontainerProvider.Resolve<IHimmaPLC>(plcVariableModel.PLCIPAddress);

        //    //LogHelper.Debug($"接收PLC:{plc.Name}的{plcVariableModel.VariableName}[{plcVariableModel.VariableHandle}]值:{plcVariableModel.VariableValue};");

        //    if (plcVariableModel != null)
        //    {
        //        var taskid = plcVariableModel.TasKId;
        //        try
        //        {


        //            var value = int.Parse(plcVariableModel.BizType);
        //            HandleBizType(value, plcVariableModel, plc);
        //            _st.Stop();
        //            if (_st.Elapsed.TotalMilliseconds > GlobalConfig.BatteryOtherTimeOut)
        //            { LogHelper.Info($"TaskID:{taskid};{plcVariableModel.VariableName}[{plcVariableModel.VariableDescrition}]业务处理spends:{_st.Elapsed.TotalMilliseconds}ms;", "MainTimeOut"); }
        //            _st.Restart();
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.Error($"TaskID:{taskid} PLC信号触发处理异常：{plcVariableModel.VariableName}||{plcVariableModel.VariableHandle}||{ex + ex.StackTrace}");
        //        }
        //        finally
        //        {
        //            _st.Stop();
        //            //LogHelper.Debug($"TaskID:{taskid};结束{plc.Name}的{plcVariableModel.VariableDescrition};{plcVariableModel.VariableName}[{plcVariableModel.VariableHandle}]值:{plcVariableModel.VariableValue}的处理;");
        //        }
        //    }
        //}
        /// <summary>
        /// 按触发信号处理不同的业务类型
        /// </summary>
        /// <param name="bizType"></param>
        /// <param name="plcVariableModel"></param>
        /// <param name="plc"></param>
        //private void HandleBizType(int bizType, PLCVariableModel plcVariableModel, IHimmaPLC plc)
        //{
        //    switch (bizType)
        //    {
        //        case 1:
        //            _business.BatteryForIn(plcVariableModel, plc);
        //            break;
        //        case 8:
        //            _business.CupChangeRequest(plcVariableModel, plc);
        //            break;
        //        case 3:
        //            _business.BatteryForOut(plcVariableModel, plc);
        //            break;
        //        case 2:
        //            _business.FeedingForIn(plcVariableModel, plc);
        //            break;
        //        case 4:
        //            _business.FeedingInRequest(plcVariableModel, plc);
        //            break;
        //        case 7:
        //            _business.ScannerRequest(plcVariableModel, plc);
        //            break;
        //        case 10:
        //            _business.CollectRealTimeData(plcVariableModel, plc);
        //            break;
        //        case 11:
        //            _business.FeedingOutRequest(plcVariableModel, plc);
        //            break;
        //        case 12:
        //            _business.TrayloadBindRequest(plcVariableModel, plc);
        //            break;
        //        case 13:
        //            _business.HipotRealTimeRead(plcVariableModel, plc);
        //            break;
        //        case 14:
        //            _business.HipotHandleRead(plcVariableModel, plc);
        //            break;
        //        case 15:
        //            _business.HipotHandleWrite(plcVariableModel, plc);
        //            break;
        //        case 16:
        //            _business.WindNGForOut(plcVariableModel, plc);
        //            break;
        //        case 17:
        //            _business.WindFlawCoordinate(plcVariableModel, plc);
        //            break;
        //        case 5:
        //            _business.PEXuploadRequest(plcVariableModel, plc);
        //            break;
        //    }
        //}
        public void _plc_PLCNotifyEventArgs(object obj, object sender)
        {
            throw new NotImplementedException();
        }
    }


}
