using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Himma.Common.Log;
using Himma.Common.DAL.BizDAL;
using Himma.Common.Service.Interface;
using Himma.Common.DAL.Variable;
using Himma.Common.Helper.Log;

namespace Himma.Common.Pages.ViewModels
    {
        /// <summary>
        /// Copyright (c) 2020 All Rights Reserved.
        /// 描述：主界面逻辑
        /// 创建人： Henick
        /// 创建时间：2020/4/3 8:46:16
        /// </summary>
        public class ViewMonitorViewModel : BindableBase
        {
            private readonly IEventAggregator _eventAggregator;
            private TaskFactory taskFactory = new TaskFactory();
            private BizBatteryResultDAL _bizBatteryResultDAL = new BizBatteryResultDAL();//结果数据
            private object lockLog = new object();
            #region Properties

            private int _tabSelectedIndex;

            /// <summary>
            /// 所选的tab页
            /// </summary>
            public int TabSelectedIndex
            {
                get { return _tabSelectedIndex; }
                set { SetProperty(ref _tabSelectedIndex, value); }
            }

            

            //private IPLC _plc;

            private long _okCount;

            public long OKCount
            {
                get => _okCount;
                set => SetProperty(ref _okCount, value);
            }

            private long _ngCount;

            public long NGCount
            {
                get => _ngCount;
                set => SetProperty(ref _ngCount, value);
            }

            private long _count;

            public long Count
            {
                get => _count;
                set => SetProperty(ref _count, value);
            }

          
          
            private string _mesMode;

            public string MesMode
            {
                get => _mesMode;
                set => SetProperty(ref _mesMode, value);
            }
            #endregion Properties

            /// <summary>
            /// 构造函数
            /// </summary>
            public ViewMonitorViewModel(IContainerProvider container, ICommService commService, IEventAggregator eventAggregator)
            {
                try
                {
                    MesMode = GlobalVariable.MesScan == 20 ? "屏蔽" : "在线";
                    _eventAggregator = eventAggregator;
                    LogHelper.InfoNotify += _log_InfoNotify;
                    //taskFactory.StartNew(() => { BrushYield(); }, TaskCreationOptions.LongRunning);//刷新产量
                    GlobalVariable.mesModeChanged += (o) => { MesMode = o == 20 ? "屏蔽" : "在线"; };
                    commService.Start();
                    SubscribeComponent();
                }
                catch (Exception ex)
                {
                 
                }
            }


            private void _log_InfoNotify(object sender, EventArgs e)
            {
               
            }

            /// <summary>
            /// 订阅部件状态
            /// </summary>
            private void SubscribeComponent()
            {
            }

            private void BrushYield()
            {
                while (true)
                {
                    //var yield= _bizBatteryResultDAL.CurrentDb.GetList(m => m.CreateTime < DateTime.Now && m.CreateTime > DateTime.Now.Date);
                   
                    Thread.Sleep(5000);
                }
            }

            private ObservableCollection<string> _normalLogCollection = new ObservableCollection<string>();

            /// <summary>
            /// 普通日志
            /// </summary>
            public ObservableCollection<string> NormalLogCollection
            {
                get { return _normalLogCollection; }
                set { SetProperty(ref _normalLogCollection, value); }
            }

            private ObservableCollection<string> _mesLogCollection = new ObservableCollection<string>();

            /// <summary>
            /// MES日志
            /// </summary>
            public ObservableCollection<string> MESLogCollection
            {
                get { return _mesLogCollection; }
                set { SetProperty(ref _mesLogCollection, value); }
            }

           


        }

    }
