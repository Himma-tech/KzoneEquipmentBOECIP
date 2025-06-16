using KZONE.PLCAgent.PLC.KZONE.PLCAgent.Data;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace KZONE.PLCAgent.PLC
{
    internal class PLCDriverRuntime : IPLCDriver, IDisposable
    {
        private IDictionary<string, int> _DataDictionary1;

        private IDictionary<string, int> _DataDictionary2;

        private IDictionary<string, int> _DataDictionary3;

        private PLCDataModel _Model;

        private Dictionary<string, PLCScanBuffer> _IODatas = new Dictionary<string, PLCScanBuffer>();

        private Dictionary<int, PLCAdapter> _Adapters = new Dictionary<int, PLCAdapter>();

        private PLCAdapter _DefaultAdapter;

        protected CancellationTokenSource _LocalSource = new CancellationTokenSource();

        protected CancellationToken _Token;

        private Thread _TriggerTask;

        private Thread _ScanTask;

        private Thread _WriteTask;

        private int _ScanTaskWaitFrequency = 20;

        private int _ScanTaskWaitPeriodMS = 10;

        private int _WriteTaskWaitPeriodMS = 5;

        private int _TriggerTaskWaitFrequency = 10;

        private int _TriggerTaskWaitPeriodMS = 10;

        private int _TriggerMaxWorkIterateTimes = 10;

        private int _PLCAdapterMaxThreadSleepTimeMS = 100;

        private int _PLCAdapterMaxIterateTimes = 10;

        private bool _DebugOutEnable = false;

        private bool _FlipBCDValue = true;

        private ConcurrentQueue<Trx> _TrxReadQueue = new ConcurrentQueue<Trx>();

        private ConcurrentQueue<Trx> _TrxWriteQueue = new ConcurrentQueue<Trx>();

        private Timer _Timer;

        private int _TotalRead;

        private int _TotalWrite;

        private int _TotalScan;

        private int _TotalScanRead;

        private string _TriggerTaskState = string.Empty;

        private string _ScanTaskState = string.Empty;

        private string _WriteTaskState = string.Empty;

        private int _MaxScanPerSec;

        private double _AvgScanPerSec;

        private int _ScanPerSec;

        private TimeSpan _MaxScanSpentTime;

        private TimeSpan _ScanSpentTime;

        private DateTime _ScanStartDT;

        private int _ScanReadCount;

        private int _MaxTrxWritePerSec;

        private double _AvgTrxWritePerSec;

        private int _TrxWritePerSec;

        private int _MaxTrxReadPerSec;

        private double _AvgTrxReadPerSec;

        private int _TrxReadPerSec;

        private TimeSpan _MaxTrxTriggerSpentTime;

        private TimeSpan _TrxTriggerSpentTime;

        private DateTime _TrxTriggerStartDT;

        private int _MaxTrxTriggerCount;

        private double _AvgTrxTriggerCount;

        private int _TrxTriggerCount;

        private Dictionary<int, bool> _ConnectState = new Dictionary<int, bool>();

        private int _recvreqsno;

        private int _sendreqsno = new Random((int)DateTime.Now.Ticks).Next();

        private object _SyncLock = new object();

        private bool _IsDestroy;

        public event PLCDriverDebugOutEventHandler PLCDriverDebugOutEvent;

        public event PLCConnectedEventHandler PLCConnectedEvent;

        public event PLCDisconnectedEventHandler PLCDisconnectedEvent;

        public event TrxTriggeredEventHandler TrxTriggeredEvent;

        public event ReadTrxResultEventHandler ReadTrxResultEvent;

        public event WriteTrxResultEventHandler WriteTrxResultEvent;

        private int TrxReadQueueDepth
        {
            get
            {
                return this._TrxReadQueue.Count;
            }
        }

        private int TrxWriteQueueDepth
        {
            get
            {
                return this._TrxWriteQueue.Count;
            }
        }

        private int NewRecvReqSno
        {
            get
            {
                return Interlocked.Increment(ref this._recvreqsno);
            }
        }

        private int NewSendReqSno
        {
            get
            {
                return Interlocked.Increment(ref this._sendreqsno);
            }
        }

        public string DriverId
        {
            get;
            internal set;
        }

        public string ConfigFileUrl
        {
            get;
            internal set;
        }

        public string FormatFileUrl
        {
            get;
            internal set;
        }

        public bool IsInited
        {
            get;
            internal set;
        }

        public bool IsStarted
        {
            get;
            internal set;
        }

        public bool IsDisposed
        {
            get;
            internal set;
        }

        public bool EnableScanBufferDump
        {
            get;
            set;
        }

        public int ScanTaskWaitFrequency
        {
            get
            {
                return this._ScanTaskWaitFrequency;
            }
            set
            {
                this._ScanTaskWaitFrequency = value;
                if (this._ScanTaskWaitFrequency <= 0)
                {
                    this._ScanTaskWaitFrequency = 1;
                }
            }
        }

        public int ScanTaskWaitPeriodMS
        {
            get
            {
                return this._ScanTaskWaitPeriodMS;
            }
            set
            {
                this._ScanTaskWaitPeriodMS = value;
                if (this._ScanTaskWaitPeriodMS <= 0)
                {
                    this._ScanTaskWaitPeriodMS = 50;
                }
            }
        }

        public int WriteTaskWaitPeriodMS
        {
            get
            {
                return this._WriteTaskWaitPeriodMS;
            }
            set
            {
                this._WriteTaskWaitPeriodMS = value;
                if (this._WriteTaskWaitPeriodMS <= 0)
                {
                    this._WriteTaskWaitPeriodMS = 5;
                }
            }
        }

        public int TriggerTaskWaitFrequency
        {
            get
            {
                return this._TriggerTaskWaitFrequency;
            }
            set
            {
                this._TriggerTaskWaitFrequency = value;
                if (this._TriggerTaskWaitFrequency <= 0)
                {
                    this._TriggerTaskWaitFrequency = 1;
                }
            }
        }

        public int TriggerTaskWaitPeriodMS
        {
            get
            {
                return this._TriggerTaskWaitPeriodMS;
            }
            set
            {
                this._TriggerTaskWaitPeriodMS = value;
                if (this._TriggerTaskWaitPeriodMS <= 0)
                {
                    this._TriggerTaskWaitPeriodMS = 30;
                }
            }
        }

        public int TriggerMaxWorkIterateTimes
        {
            get
            {
                return this._TriggerMaxWorkIterateTimes;
            }
            set
            {
                this._TriggerMaxWorkIterateTimes = value;
                if (this._TriggerMaxWorkIterateTimes <= 0)
                {
                    this._TriggerMaxWorkIterateTimes = 10;
                }
            }
        }

        public int PLCAdapterMaxThreadSleepTimeMS
        {
            get
            {
                return this._PLCAdapterMaxThreadSleepTimeMS;
            }
            set
            {
                this._PLCAdapterMaxThreadSleepTimeMS = value;
                if (this._PLCAdapterMaxThreadSleepTimeMS <= 0)
                {
                    this._PLCAdapterMaxThreadSleepTimeMS = 1;
                }
                foreach (PLCAdapter adapter in this._Adapters.Values)
                {
                    adapter.MaxThreadSleepTimeMS = this._PLCAdapterMaxThreadSleepTimeMS;
                }
            }
        }

        public int PLCAdapterMaxIterateTimes
        {
            get
            {
                return this._PLCAdapterMaxIterateTimes;
            }
            set
            {
                this._PLCAdapterMaxIterateTimes = value;
                if (this._PLCAdapterMaxIterateTimes <= 0)
                {
                    this._PLCAdapterMaxIterateTimes = 10;
                }
                foreach (PLCAdapter adapter in this._Adapters.Values)
                {
                    adapter.MaxWorkIterateTimes = this._PLCAdapterMaxIterateTimes;
                }
            }
        }

        public IDictionary<string, object> RuntimeInfo
        {
            get
            {
                Dictionary<string, object> infos = new Dictionary<string, object>();
                try
                {
                    infos.Add("Version", this.FormatVersion.Trim());
                    infos.Add("Driver_TriggerTaskState", (this._TriggerTaskState == null) ? "" : this._TriggerTaskState);
                    infos.Add("Driver_ScanTaskState", (this._ScanTaskState == null) ? "" : this._ScanTaskState);
                    infos.Add("Driver_WriteTaskState", (this._WriteTaskState == null) ? "" : this._WriteTaskState);
                    infos.Add("Driver_MaxScanSpentTime", this._MaxScanSpentTime.ToString());
                    infos.Add("Driver_ScanSpentTime", this._ScanSpentTime.ToString());
                    infos.Add("Driver_ScanStartDT", this._ScanStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                    infos.Add("Driver_ScanReadCount", this._ScanReadCount.ToString());
                    infos.Add("Driver_MaxTrxWritePerSec", this._MaxTrxWritePerSec.ToString());
                    infos.Add("Driver_AvgTrxWritePerSec", this._AvgTrxWritePerSec.ToString("F1"));
                    infos.Add("Driver_TrxWritePerSec", this._TrxWritePerSec.ToString());
                    infos.Add("Driver_MaxTrxReadPerSec", this._MaxTrxReadPerSec.ToString());
                    infos.Add("Driver_AvgTrxReadPerSec", this._AvgTrxReadPerSec.ToString("F1"));
                    infos.Add("Driver_TrxReadPerSec", this._TrxReadPerSec.ToString());
                    infos.Add("Driver_MaxTrxTriggerSpentTime", this._MaxTrxTriggerSpentTime.ToString());
                    infos.Add("Driver_TrxTriggerSpentTime", this._TrxTriggerSpentTime.ToString());
                    infos.Add("Driver_TrxTriggerStartDT", this._TrxTriggerStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                    infos.Add("Driver_MaxTrxTriggerCount", this._MaxTrxTriggerCount.ToString());
                    infos.Add("Driver_AvgTrxTriggerCount", this._AvgTrxTriggerCount.ToString("F1"));
                    infos.Add("Driver_TrxTriggerCount", this._TrxTriggerCount.ToString());
                    infos.Add("Driver_TrxReadQueueDepth", this.TrxReadQueueDepth.ToString());
                    infos.Add("Driver_TrxWriteQueueDepth", this.TrxWriteQueueDepth.ToString());
                    infos.Add("Driver_IsInited", this.IsInited.ToString());
                    infos.Add("Driver_IsStarted", this.IsStarted.ToString());
                    object[] allValues = this._Model.Scan.AllValues;
                    for (int i = 0; i < allValues.Length; i++)
                    {
                        WatchData watch = (WatchData)allValues[i];
                        infos.Add(string.Format("WatchData_{0}_{1}_{2}_ScanIntervalMS", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.ScanIntervalMS.ToString());
                        infos.Add(string.Format("WatchData_{0}_{1}_{2}_MaxScanPerSec", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.MaxScanPerSec.ToString());
                        infos.Add(string.Format("WatchData_{0}_{1}_{2}_AvgScanPerSec", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.AvgScanPerSec.ToString("F1"));
                        infos.Add(string.Format("WatchData_{0}_{1}_{2}_ScanPerSec", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.ScanPerSec.ToString());
                        infos.Add(string.Format("WatchData_{0}_{1}_{2}_LastScanDT", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.LastScanDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                        infos.Add(string.Format("WatchData_{0}_{1}_{2}_LastScanSpentTime", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.LastScanSpentTime.ToString());
                        infos.Add(string.Format("WatchData_{0}_{1}_{2}_LastScanError", watch.Name, watch.LogicalStationNo, watch.DeviceCode), (watch.LastScanError == null) ? "" : watch.LastScanError);
                        infos.Add(string.Format("WatchData_{0}_{1}_{2}_LastScanErrorDT", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.LastScanErrorDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                    }
                    foreach (PLCAdapter v in this._Adapters.Values)
                    {
                        infos.Add(string.Format("Adapter_{0}_IsInited", v.LogicalStationNo), v.IsInited.ToString());
                        infos.Add(string.Format("Adapter_{0}_IsOpened", v.LogicalStationNo), v.IsOpened.ToString());
                        infos.Add(string.Format("Adapter_{0}_IsConnected", v.LogicalStationNo), v.IsConnected.ToString());
                        infos.Add(string.Format("Adapter_{0}_IsTriggerEnable", v.LogicalStationNo), v.IsTriggerEnable.ToString());
                        infos.Add(string.Format("Adapter_{0}_IsEnableW2ZR", v.LogicalStationNo), v.IsEnableW2ZR.ToString());
                        infos.Add(string.Format("Adapter_{0}_WorkerState", v.LogicalStationNo), (v.WorkerState == null) ? "" : v.WorkerState);
                        infos.Add(string.Format("Adapter_{0}_PLCLastErrorCode", v.LogicalStationNo), v.PLCLastErrorCode.ToString());
                        infos.Add(string.Format("Adapter_{0}_PLCLastErrorDesc", v.LogicalStationNo), (v.PLCLastErrorDesc == null) ? "" : v.PLCLastErrorDesc);
                        infos.Add(string.Format("Adapter_{0}_PLCLastErrorDT", v.LogicalStationNo), v.PLCLastErrorDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                        infos.Add(string.Format("Adapter_{0}_WriteReqQueueDepth", v.LogicalStationNo), v.WriteReqQueueDepth.ToString());
                        infos.Add(string.Format("Adapter_{0}_ReadReqQueueDepth", v.LogicalStationNo), v.ReadReqQueueDepth.ToString());
                        infos.Add(string.Format("Adapter_{0}_ReadDelayCount", v.LogicalStationNo), v.ReadDelayCount.ToString());
                        infos.Add(string.Format("Adapter_{0}_WriteDelayCount", v.LogicalStationNo), v.WriteDelayCount.ToString());
                    }
                    foreach (PLCScanBuffer buf in this._IODatas.Values)
                    {
                        foreach (WatchData watch in buf.Source)
                        {
                            infos.Add(string.Format("IO_{0}_{1}_{2}_Address", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.Address.ToString());
                            infos.Add(string.Format("IO_{0}_{1}_{2}_Points", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.Points.ToString());
                            infos.Add(string.Format("IO_{0}_{1}_{2}_IsDataInited", watch.Name, watch.LogicalStationNo, watch.DeviceCode), watch.IsDataInited.ToString());
                            infos.Add(string.Format("IO_{0}_{1}_{2}_LastRefreshDT", watch.Name, watch.LogicalStationNo, watch.DeviceCode), buf.LastRefreshDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                            infos.Add(string.Format("IO_{0}_{1}_{2}_LastRefreshError", watch.Name, watch.LogicalStationNo, watch.DeviceCode), (buf.LastRefreshError == null) ? "" : buf.LastRefreshError);
                            infos.Add(string.Format("IO_{0}_{1}_{2}_LastRefreshErrorDT", watch.Name, watch.LogicalStationNo, watch.DeviceCode), buf.LastRefreshErrorDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                        }
                    }
                }
                catch
                {
                }
                return infos;
            }
        }

        public IDictionary<string, object> PLCState
        {
            get
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                try
                {
                    foreach (PLCAdapter v in this._Adapters.Values)
                    {
                        dict.Add(v.LogicalStationNo.ToString(), v.IsConnected ? "Connected" : "Disconnected");
                    }
                }
                catch
                {
                }
                return dict;
            }
        }

        public string FormatVersion
        {
            get;
            internal set;
        }

        public string ConfigContent
        {
            get;
            set;
        }

        public Dictionary<string, PLCScanBuffer> IODatas
        {
            get
            {
                return this._IODatas;
            }
        }

        public Dictionary<int, PLCAdapter> Adapters
        {
            get
            {
                return this._Adapters;
            }
        }

        public bool DebugOutEnable
        {
            get
            {
                return this._DebugOutEnable;
            }
            set
            {
                this._DebugOutEnable = value;
            }
        }

        public bool FlipBCDValue
        {
            get
            {
                return this._FlipBCDValue;
            }
            set
            {
                this._FlipBCDValue = value;
            }
        }

        public bool Init(string configFileUrl, string formatFileUrl, out string reason)
        {
            this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "Init", 0, string.Format("do plcdriver init,config={0},format={1}", configFileUrl, formatFileUrl), string.Empty, 0);
            bool result;
            if (this.IsInited)
            {
                reason = "driver already inited err";
                result = false;
            }
            else
            {
                if (string.IsNullOrEmpty(configFileUrl))
                {
                    throw new PLCDriverException(20001, "configFileUrl argument is null or empty");
                }
                if (string.IsNullOrEmpty(formatFileUrl))
                {
                    throw new PLCDriverException(20001, "formatFileUrl argument is null or empty");
                }
                this.ConfigFileUrl = configFileUrl;
                this.FormatFileUrl = this.FormatFileUrl;
                if (!this.CreateAdapters(configFileUrl, out reason))
                {
                    result = false;
                }
                else if (!this.CreateModel(formatFileUrl, out reason))
                {
                    result = false;
                }
                else
                {
                    this.IsInited = true;
                    result = true;
                }
            }
            return result;
        }

        public bool Init(string configFileUrl, string formatFileUrl, out string reason, List<Transform> transforms)
        {
            bool done = this.Init(configFileUrl, formatFileUrl, out reason);
            bool result;
            if (!done)
            {
                result = done;
            }
            else if (transforms == null)
            {
                result = true;
            }
            else
            {
                foreach (Transform xfr in transforms)
                {
                    if (!this._Adapters.ContainsKey(xfr.TargetLogicalStationNo))
                    {
                        reason = string.Format("transform node={0},logicstno={1},invalid logicstno value", xfr.TargetNodeNo, xfr.TargetLogicalStationNo);
                        result = false;
                        return result;
                    }
                    PLCAdapter adapter = this._Adapters[xfr.TargetLogicalStationNo];
                    if (!adapter.Devices.ContainsKey(xfr.FromDeviceCode))
                    {
                        reason = string.Format("transform node={0},logicstno={1},invalid FromDeviceCode value={2}", xfr.TargetNodeNo, xfr.TargetLogicalStationNo, xfr.FromDeviceCode);
                        result = false;
                        return result;
                    }
                    xfr.FromDevice = adapter.Devices[xfr.FromDeviceCode];
                    if (!adapter.Devices.ContainsKey(xfr.ToDeviceCode))
                    {
                        reason = string.Format("transform node={0},logicstno={1},fromDevideCode={2},invalid ToDeviceCode value={3}", new object[]
                        {
                            xfr.TargetNodeNo,
                            xfr.TargetLogicalStationNo,
                            xfr.FromDeviceCode,
                            xfr.ToDeviceCode
                        });
                        result = false;
                        return result;
                    }
                    xfr.ToDevice = adapter.Devices[xfr.ToDeviceCode];
                    if (xfr.FromDevice.isbit != xfr.ToDevice.isbit)
                    {
                        reason = string.Format("transform node={0},logicstno={1},fromDevideCode={2} is not same devicetype as ToDeviceCod={3}", new object[]
                        {
                            xfr.TargetNodeNo,
                            xfr.TargetLogicalStationNo,
                            xfr.FromDeviceCode,
                            xfr.ToDeviceCode
                        });
                        result = false;
                        return result;
                    }
                    try
                    {
                        xfr.FromNodeStartAddr10 = Convert.ToInt32(xfr.FromNodeStartAddress, xfr.FromDevice.nbase);
                    }
                    catch
                    {
                        reason = string.Format("transform node={0},logicstno={1},fromDevideCode={2},fail to convert fromNodeStartAddress value={3}", new object[]
                        {
                            xfr.TargetNodeNo,
                            xfr.TargetLogicalStationNo,
                            xfr.FromDeviceCode,
                            xfr.FromNodeStartAddress
                        });
                        result = false;
                        return result;
                    }
                    try
                    {
                        xfr.ToNodeStartAddr10 = Convert.ToInt32(xfr.ToNodeStartAddress, xfr.ToDevice.nbase);
                    }
                    catch
                    {
                        reason = string.Format("transform node={0},logicstno={1},fromDevideCode={2},fail to convert ToNodeStartAddress value={3}", new object[]
                        {
                            xfr.TargetNodeNo,
                            xfr.TargetLogicalStationNo,
                            xfr.FromDeviceCode,
                            xfr.ToNodeStartAddress
                        });
                        result = false;
                        return result;
                    }
                    string key = xfr.TargetLogicalStationNo + "_" + xfr.ToDeviceCode;
                    if (!this._IODatas.ContainsKey(key))
                    {
                        reason = string.Format("transform node={0},logicstno={1},toDeviceCode={2},plc scan buffer not found", xfr.TargetNodeNo, xfr.TargetLogicalStationNo, xfr.ToDeviceCode);
                        result = false;
                        return result;
                    }
                }
                foreach (Trx trx in from Trx t in this._Model.Transaction.AllValues
                                    where t.Metadata.TrxType == TrxTypeEnum.Receive
                                    select t)
                {
                    object[] allValues = trx.EventGroups.AllValues;
                    for (int i = 0; i < allValues.Length; i++)
                    {
                        EventGroup eg = (EventGroup)allValues[i];
                        object[] allValues2 = eg.Events.AllValues;
                        for (int j = 0; j < allValues2.Length; j++)
                        {
                            Event evt = (Event)allValues2[j];
                            foreach (Transform xfr in transforms)
                            {
                                if (trx.Metadata.NodeNo == xfr.TargetNodeNo && evt.Metadata.LogicalStationNo == xfr.TargetLogicalStationNo && evt.Metadata.DeviceCode == xfr.FromDeviceCode)
                                {
                                    int offset = evt.Metadata.OriginalStartAddress10 - xfr.FromNodeStartAddr10;
                                    if (offset < 0)
                                    {
                                        reason = string.Format("transform node={0},logicstno={1},fromDevideCode={2},Event={3},Address {4} is lower than fromNodeStartAddress {5}", new object[]
                                        {
                                            xfr.TargetNodeNo,
                                            xfr.TargetLogicalStationNo,
                                            xfr.FromDeviceCode,
                                            evt.Name,
                                            evt.Metadata.OriginalAddress,
                                            xfr.FromNodeStartAddress
                                        });
                                        result = false;
                                        return result;
                                    }
                                    evt.Metadata.IsApplyTransform = true;
                                    evt.Metadata.DeviceCode = xfr.ToDeviceCode;
                                    evt.Metadata.IsBitDeviceType = xfr.ToDevice.isbit;
                                    int newAddress10 = xfr.ToNodeStartAddr10 + offset;
                                    if (xfr.ToDevice.nbase == 16)
                                    {
                                        evt.Metadata.Address = newAddress10.ToString("X7");
                                        evt.Metadata.IsAddressHex = true;
                                    }
                                    else
                                    {
                                        evt.Metadata.Address = newAddress10.ToString();
                                        evt.Metadata.IsAddressHex = false;
                                    }
                                    evt.Metadata.StartAddress10 = newAddress10;
                                    break;
                                }
                            }
                        }
                    }
                }
                result = true;
            }
            return result;
        }

        public bool Start(out string reason)
        {
            this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "Start", 0, "do plcdriver start", string.Empty, 0);
            bool result2;
            if (!this.IsInited)
            {
                reason = "driver not inited yet err";
                result2 = false;
            }
            else if (this.IsStarted)
            {
                reason = "dirver already started err";
                result2 = false;
            }
            else
            {
                foreach (PLCAdapter v2 in this._Adapters.Values)
                {
                    PLCOpResult result = v2.Open();
                    if (result.RetCode != 0)
                    {
                        throw new PLCDriverException(result.RetCode, result.RetErrMsg);
                    }
                }
                reason = string.Empty;
                this.IsStarted = true;
                DateTime now = DateTime.Now;
                while (DateTime.Now.Subtract(now).TotalSeconds < 5.0)
                {
                    Thread.Sleep(300);
                    bool all = true;
                    foreach (PLCAdapter v2 in this._Adapters.Values)
                    {
                        if (!v2.IsConnected)
                        {
                            all = false;
                            break;
                        }
                    }
                    if (all)
                    {
                        break;
                    }
                }
                object[] allValues = this._Model.Scan.AllValues;
                for (int i = 0; i < allValues.Length; i++)
                {
                    WatchData w = (WatchData)allValues[i];
                    w.LastPutDataDT = DateTime.Now;
                }
                if (this._ScanTask == null)
                {
                    this._Token = this._LocalSource.Token;
                    this._TriggerTask = new Thread(new ThreadStart(this.TriggerProc));
                    this._TriggerTask.IsBackground = true;
                    this._ScanTask = new Thread(new ThreadStart(this.ScanProc));
                    this._ScanTask.IsBackground = true;
                    this._WriteTask = new Thread(new ThreadStart(this.WriteProc));
                    this._WriteTask.IsBackground = true;
                    this._TriggerTask.Start();
                    this._ScanTask.Start();
                    this._WriteTask.Start();
                }
                if (this._Timer == null)
                {
                    this._Timer = new Timer(delegate (object v)
                    {
                        object[] allValues2 = this._Model.Scan.AllValues;
                        for (int j = 0; j < allValues2.Length; j++)
                        {
                            WatchData watch = (WatchData)allValues2[j];
                            watch.ScanPerSec = watch.TotalScan;
                            if (watch.ScanPerSec > watch.MaxScanPerSec)
                            {
                                watch.MaxScanPerSec = watch.ScanPerSec;
                            }
                            watch.AvgScanPerSec = (watch.AvgScanPerSec + (double)watch.TotalScan) / 2.0;
                            watch.TotalScan = 0;
                        }
                        this._TrxReadPerSec = this._TotalRead;
                        if (this._TrxReadPerSec > this._MaxTrxReadPerSec)
                        {
                            this._MaxTrxReadPerSec = this._TrxReadPerSec;
                        }
                        this._AvgTrxReadPerSec = (this._AvgTrxReadPerSec + (double)this._TotalRead) / 2.0;
                        this._TotalRead = 0;
                        this._ScanPerSec = this._TotalScan;
                        if (this._ScanPerSec > this._MaxScanPerSec)
                        {
                            this._MaxScanPerSec = this._ScanPerSec;
                        }
                        this._AvgScanPerSec = (this._AvgScanPerSec + (double)this._TotalScan) / 2.0;
                        this._TotalScan = 0;
                        this._ScanReadCount = this._TotalScanRead;
                        this._TotalScanRead = 0;
                        this._TrxWritePerSec = this._TotalWrite;
                        if (this._TrxWritePerSec > this._MaxTrxWritePerSec)
                        {
                            this._MaxTrxWritePerSec = this._TrxWritePerSec;
                        }
                        this._AvgTrxWritePerSec = (this._AvgTrxWritePerSec + (double)this._TotalWrite) / 2.0;
                        this._TotalWrite = 0;
                    }, null, 0, 1000);
                }
                result2 = true;
            }
            return result2;
        }

        public void Stop()
        {
            this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "Stop", 0, "do plcdriver stop", string.Empty, 0);
            this.IsStarted = false;
            foreach (PLCAdapter v in this._Adapters.Values)
            {
                PLCOpResult result = v.Close();
            }
        }

        public void Destroy()
        {
            this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "Destroy", 0, "do plcdriver Destroy", string.Empty, 0);
            this.Dispose();
        }

        public Trx GetEmptyTrx(string trxName)
        {
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            if (!this.IsInited)
            {
                throw new PLCDriverException(20203, "plcdriver not init yet");
            }
            Trx trx = this._Model.Transaction.Get(trxName);
            Trx result;
            if (trx != null)
            {
                result = (Trx)trx.Clone();
            }
            else
            {
                result = null;
            }
            return result;
        }

        public bool ReadTrx(string trxName, out string reason)
        {
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            if (string.IsNullOrEmpty(trxName))
            {
                throw new PLCDriverException(20001, "trxName argument is null or empty");
            }
            bool result;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result = false;
            }
            else
            {
                Trx trx = this._Model.Transaction.Get(trxName);
                if (trx == null)
                {
                    reason = "trx not found,name=" + trxName;
                    result = false;
                }
                else
                {
                    Trx newTrx = (Trx)trx.Clone();
                    newTrx.TrxFlags = InternalFlagsEnum.IsTrxRead;
                    newTrx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newTrx.ReqSNo = this.NewRecvReqSno;
                    this._TrxReadQueue.Enqueue(newTrx);
                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "ReadTrx", 0, string.Format("trx enqueue OK,ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3}", new object[]
                    {
                        newTrx.ReqSNo,
                        newTrx.Name,
                        newTrx.TrackKey,
                        newTrx.TrxFlags
                    }), newTrx.TrackKey, 0);
                    reason = string.Empty;
                    result = true;
                }
            }
            return result;
        }

        public bool ReadTrx(Trx trx, out string reason)
        {
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            bool result;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result = false;
            }
            else
            {
                trx.TrxFlags = InternalFlagsEnum.IsTrxRead;
                trx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                trx.ReqSNo = this.NewRecvReqSno;
                this._TrxReadQueue.Enqueue(trx);
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "ReadTrx", 0, string.Format("trx enqueue OK,ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3}", new object[]
                {
                    trx.ReqSNo,
                    trx.Name,
                    trx.TrackKey,
                    trx.TrxFlags
                }), trx.TrackKey, 0);
                reason = string.Empty;
                result = true;
            }
            return result;
        }

        public bool DirectReadTrx(Trx trx, out string reason)
        {
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            bool result;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result = false;
            }
            else
            {
                trx.TrxFlags = InternalFlagsEnum.IsTrxRead;
                trx.TrxFlags |= InternalFlagsEnum.IsDirectRead;
                trx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                trx.ReqSNo = this.NewRecvReqSno;
                this._TrxReadQueue.Enqueue(trx);
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "DirectReadTrx", 0, string.Format("trx enqueue OK,ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3}", new object[]
                {
                    trx.ReqSNo,
                    trx.Name,
                    trx.TrackKey,
                    trx.TrxFlags
                }), trx.TrackKey, 0);
                reason = string.Empty;
                result = true;
            }
            return result;
        }

        public Trx SyncReadTrx(string trxName, out string reason)
        {
            reason = string.Empty;
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            if (string.IsNullOrEmpty(trxName))
            {
                throw new PLCDriverException(20001, "trxName argument is null or empty");
            }
            Trx result2;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result2 = null;
            }
            else
            {
                Trx trx = this._Model.Transaction.Get(trxName);
                if (trx == null)
                {
                    reason = "trx not found,name=" + trxName;
                    result2 = null;
                }
                else
                {
                    Trx newTrx = (Trx)trx.Clone();
                    newTrx.TrxFlags = InternalFlagsEnum.IsTrxRead;
                    Trx expr_AC = newTrx;
                    expr_AC.TrxFlags |= InternalFlagsEnum.IsSyncRead;
                    newTrx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newTrx.ReqSNo = this.NewRecvReqSno;
                    PLCOpResult result = this.DecodeTrx(newTrx, false);
                    if (result.RetCode != 0)
                    {
                        reason = result.RetCode + "," + result.RetErrMsg;
                        result2 = null;
                    }
                    else if (result.RetCode == 0)
                    {
                        this._TotalRead++;
                        result2 = newTrx;
                    }
                    else
                    {
                        result2 = null;
                    }
                }
            }
            return result2;
        }

        public bool SyncReadTrx(Trx trx, out string reason)
        {
            reason = string.Empty;
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            bool result2;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result2 = false;
            }
            else
            {
                trx.TrxFlags = InternalFlagsEnum.IsTrxRead;
                trx.TrxFlags |= InternalFlagsEnum.IsSyncRead;
                trx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                trx.ReqSNo = this.NewRecvReqSno;
                PLCOpResult result = this.DecodeTrx(trx, false);
                if (result.RetCode != 0)
                {
                    reason = result.RetCode + "," + result.RetErrMsg;
                    result2 = false;
                }
                else
                {
                    this._TotalRead++;
                    result2 = true;
                }
            }
            return result2;
        }

        public bool SyncWriteTrx(Trx trx, out string reason)
        {
            reason = string.Empty;
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            if (trx == null)
            {
                throw new PLCDriverException(20001, "trxName argument is null or empty");
            }
            bool result2;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result2 = false;
            }
            else
            {
                trx.TrxFlags = InternalFlagsEnum.IsTrxWrite;
                trx.TrxFlags |= InternalFlagsEnum.IsSyncWrite;
                trx.ReqSNo = this.NewSendReqSno;
                trx.WriteRequestStations = new ConcurrentDictionary<int, object>();
                trx.WriteCompleteStations = new ConcurrentDictionary<int, object>();
                trx.WriteCompleteSync = new object();
                PLCOpResult result = this.EncodeTrx(trx);
                if (result.RetCode != 0)
                {
                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "SyncWriteTrx", result.RetCode, string.Format("fail to encode,syncwritetrx={0},reqsno={1},err={2}", trx.Name, trx.ReqSNo, result.RetErrMsg), trx.TrackKey, 0);
                    result2 = false;
                }
                else
                {
                    Dictionary<int, PLCOpRequest> requests = new Dictionary<int, PLCOpRequest>();
                    object[] allValues = trx.EventGroups.AllValues;
                    for (int i = 0; i < allValues.Length; i++)
                    {
                        EventGroup evtgrp = (EventGroup)allValues[i];
                        if (!evtgrp.IsDisable)
                        {
                            if (evtgrp.IsMergeEvent)
                            {
                                if (evtgrp.Events.Count != 0)
                                {
                                    Event evt0 = evtgrp.Events[0];
                                    int startAddress10 = evt0.Metadata.StartAddress10;
                                    int totalPoints = 0;
                                    object[] allValues2 = evtgrp.Events.AllValues;
                                    for (int j = 0; j < allValues2.Length; j++)
                                    {
                                        Event evt = (Event)allValues2[j];
                                        if (evt.Metadata.IsBitDeviceType)
                                        {
                                            reason = string.Format("fail to merge,syncwritetrx={0},reqsno={1},eventgroup={2},event={3},err=can't merge bit device event", new object[]
                                            {
                                                trx.Name,
                                                trx.ReqSNo,
                                                evtgrp.Name,
                                                evt.Name
                                            });
                                            result2 = false;
                                            return result2;
                                        }
                                        if (evt.Metadata.DeviceCode != evt0.Metadata.DeviceCode)
                                        {
                                            reason = string.Format("fail to merge,syncwritetrx={0},reqsno={1},eventgroup={2},event={3},err=can't merge different device code", new object[]
                                            {
                                                trx.Name,
                                                trx.ReqSNo,
                                                evtgrp.Name,
                                                evt.Name
                                            });
                                            result2 = false;
                                            return result2;
                                        }
                                        if (evt.Metadata.LogicalStationNo != evt0.Metadata.LogicalStationNo)
                                        {
                                            reason = string.Format("fail to merge event,syncwritetrx={0},reqsno={1},eventgroup={2},event={3},err=can't merge different logicalstno", new object[]
                                            {
                                                trx.Name,
                                                trx.ReqSNo,
                                                evtgrp.Name,
                                                evt.Name
                                            });
                                            result2 = false;
                                            return result2;
                                        }
                                        if (evt.Metadata.StartAddress10 != startAddress10)
                                        {
                                            reason = string.Format("fail to merge,syncwritetrx={0},reqsno={1},eventgroup={2},event={3},err=can't merge discontinous device address", new object[]
                                            {
                                                trx.Name,
                                                trx.ReqSNo,
                                                evtgrp.Name,
                                                evt.Name
                                            });
                                            result2 = false;
                                            return result2;
                                        }
                                        startAddress10 += evt.Metadata.Points;
                                        totalPoints += evt.Metadata.Points;
                                    }
                                    evtgrp.MergeBuffer = new short[totalPoints];
                                    int offset = 0;
                                    allValues2 = evtgrp.Events.AllValues;
                                    for (int j = 0; j < allValues2.Length; j++)
                                    {
                                        Event evt = (Event)allValues2[j];
                                        if (evt.IsDisable)
                                        {
                                            offset += evt.Metadata.Points;
                                        }
                                        else if (evt.RawData == null)
                                        {
                                            offset += evt.Metadata.Points;
                                        }
                                        else
                                        {
                                            int points;
                                            if (evt.Metadata.Points > evt.RawData.Length)
                                            {
                                                points = evt.RawData.Length;
                                            }
                                            else
                                            {
                                                points = evt.Metadata.Points;
                                            }
                                            Buffer.BlockCopy(evt.RawData, 0, evtgrp.MergeBuffer, offset * 2, points * 2);
                                            offset += evt.Metadata.Points;
                                        }
                                    }
                                    PLCOpRequest req = null;
                                    if (requests.ContainsKey(evt0.Metadata.LogicalStationNo))
                                    {
                                        req = requests[evt0.Metadata.LogicalStationNo];
                                    }
                                    if (req == null)
                                    {
                                        req = new PLCOpRequest();
                                        req.ReqSNo = trx.ReqSNo;
                                        req.LogicalStationNo = evt0.Metadata.LogicalStationNo;
                                        req.Tag = trx;
                                        requests.Add(evt0.Metadata.LogicalStationNo, req);
                                    }
                                    PLCBlockOp op = new PLCBlockOp();
                                    op.OpDelayTimeMS = evt0.OpDelayTimeMS;
                                    if (op.OpDelayTimeMS > 0)
                                    {
                                        req.HasOpDelay = true;
                                    }
                                    op.EventKey = evtgrp.Name;
                                    op.DevType = evt0.Metadata.DeviceCode;
                                    op.DevNo = evt0.Metadata.Address;
                                    op.Points = totalPoints;
                                    op.Buf = evtgrp.MergeBuffer;
                                    req.BlockOps.Add(op);
                                }
                            }
                            else
                            {
                                object[] allValues2 = evtgrp.Events.AllValues;
                                for (int j = 0; j < allValues2.Length; j++)
                                {
                                    Event evt = (Event)allValues2[j];
                                    if (!evt.IsDisable)
                                    {
                                        if (evt.RawData != null)
                                        {
                                            PLCOpRequest req = null;
                                            if (requests.ContainsKey(evt.Metadata.LogicalStationNo))
                                            {
                                                req = requests[evt.Metadata.LogicalStationNo];
                                            }
                                            if (req == null)
                                            {
                                                req = new PLCOpRequest();
                                                req.ReqSNo = trx.ReqSNo;
                                                req.LogicalStationNo = evt.Metadata.LogicalStationNo;
                                                req.Tag = trx;
                                                requests.Add(evt.Metadata.LogicalStationNo, req);
                                            }
                                            if (evt.Metadata.IsBitDeviceType)
                                            {
                                                PLCRandOp op2 = new PLCRandOp();
                                                op2.OpDelayTimeMS = evt.OpDelayTimeMS;
                                                if (op2.OpDelayTimeMS > 0)
                                                {
                                                    req.HasOpDelay = true;
                                                }
                                                op2.EventKey = evt.Name;
                                                op2.Blocks = new List<PLCRandOp.RandBlock>();
                                                PLCRandOp.RandBlock b = new PLCRandOp.RandBlock();
                                                b.DevType = evt.Metadata.DeviceCode;
                                                b.DevNo = evt.Metadata.Address;
                                                b.Points = evt.Metadata.Points;
                                                b.Buf = evt.RawData;
                                                op2.Blocks.Add(b);
                                                req.RandOps.Add(op2);
                                            }
                                            else
                                            {
                                                PLCBlockOp op = new PLCBlockOp();
                                                op.OpDelayTimeMS = evt.OpDelayTimeMS;
                                                if (op.OpDelayTimeMS > 0)
                                                {
                                                    req.HasOpDelay = true;
                                                }
                                                op.EventKey = evt.Name;
                                                op.DevType = evt.Metadata.DeviceCode;
                                                op.DevNo = evt.Metadata.Address;
                                                op.Points = evt.Metadata.Points;
                                                op.Buf = evt.RawData;
                                                req.BlockOps.Add(op);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (PLCOpRequest v in requests.Values)
                    {
                        PLCAdapter adapter = this._Adapters[v.LogicalStationNo];
                        result = adapter.Write(v, 10000);
                        PLCOpRequestResult rr = new PLCOpRequestResult();
                        rr.request = v;
                        rr.result = result;
                        trx.WriteCompleteStations.TryAdd(v.LogicalStationNo, rr);
                        if (result.RetCode != 0)
                        {
                            reason = result.RetCode + "," + result.RetErrMsg;
                            result2 = false;
                            return result2;
                        }
                    }
                    this._TotalWrite++;
                    result2 = true;
                }
            }
            return result2;
        }

        public bool WriteTrx(Trx trx, out string reason)
        {
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            if (trx == null)
            {
                throw new PLCDriverException(20001, "trxName argument is null or empty");
            }
            bool result;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result = false;
            }
            else
            {
                trx.TrxFlags = InternalFlagsEnum.IsTrxWrite;
                trx.ReqSNo = this.NewSendReqSno;
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "WriteTrx", 0, string.Format("try to enqueue trx,ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3}", new object[]
                {
                    trx.ReqSNo,
                    trx.Name,
                    trx.TrackKey,
                    trx.TrxFlags
                }), trx.TrackKey, 0);
                this._TrxWriteQueue.Enqueue(trx);
                reason = string.Empty;
                result = true;
            }
            return result;
        }

        public bool RandomWriteTrx(Trx trx, out string reason)
        {
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            if (trx == null)
            {
                throw new PLCDriverException(20001, "trxName argument is null or empty");
            }
            bool result;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result = false;
            }
            else
            {
                int? logicstno = null;
                object[] allValues = trx.EventGroups.AllValues;
                for (int i = 0; i < allValues.Length; i++)
                {
                    EventGroup evtgrp = (EventGroup)allValues[i];
                    object[] allValues2 = evtgrp.Events.AllValues;
                    for (int j = 0; j < allValues2.Length; j++)
                    {
                        Event evt = (Event)allValues2[j];
                        if (!logicstno.HasValue)
                        {
                            logicstno = new int?(evt.Metadata.LogicalStationNo);
                        }
                        else if (logicstno != evt.Metadata.LogicalStationNo)
                        {
                            reason = "events not same logicstno";
                            result = false;
                            return result;
                        }
                    }
                }
                trx.TrxFlags = InternalFlagsEnum.IsTrxRandWrite;
                trx.ReqSNo = this.NewSendReqSno;
                this._TrxWriteQueue.Enqueue(trx);
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "RandomWriteTrx", 0, string.Format("trx enqueue OK,ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3}", new object[]
                {
                    trx.ReqSNo,
                    trx.Name,
                    trx.TrackKey,
                    trx.TrxFlags
                }), trx.TrackKey, 0);
                reason = string.Empty;
                result = true;
            }
            return result;
        }

        public bool WriteTrxRaw(Trx trx, out string reason)
        {
            if (this.IsDisposed)
            {
                throw new PLCDriverException(20201, "plcdriver object is disposed");
            }
            if (trx == null)
            {
                throw new PLCDriverException(20001, "trxName argument is null or empty");
            }
            bool result;
            if (!this.IsStarted)
            {
                reason = "driver not started err";
                result = false;
            }
            else
            {
                trx.TrxFlags = InternalFlagsEnum.IsTrxRawWrite;
                trx.ReqSNo = this.NewSendReqSno;
                this._TrxWriteQueue.Enqueue(trx);
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "WriteTrxRaw", 0, string.Format("trx enqueue OK,ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3}", new object[]
                {
                    trx.ReqSNo,
                    trx.Name,
                    trx.TrackKey,
                    trx.TrxFlags
                }), trx.TrackKey, 0);
                reason = string.Empty;
                result = true;
            }
            return result;
        }

        public PLCDataModel GetModel()
        {
            return this._Model;
        }

        /// <summary>
        /// dispose plcdriver
        /// </summary>
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                this.IsDisposed = true;
                this._IsDestroy = true;
                foreach (PLCAdapter v in this._Adapters.Values)
                {
                    v.Dispose();
                }
            }
        }

        private bool CreateAdapters(string configFileUrl, out string reason)
        {
            XmlTextReader reader = new XmlTextReader(configFileUrl);
            XElement root = XElement.Load(reader);
            PLCAdapter adapter = null;
            this.ConfigContent = root.ToString();
            bool result2;
            foreach (XElement x in root.Elements("plc"))
            {
                adapter = new PLCAdapter();
                adapter.PLCAdapterDebugOutEvent += new PLCAdapterDebugOutEventHandler(this.adapter_PLCAdapterDebugOutEvent);
                XElement x2 = x.Element("adapter");
                if (x2 == null)
                {
                    throw new PLCDriverException(20101, "config missing required adapter setting");
                }
                XElement elm;
                string tmp = ((elm = x2.Element("logicalstno")) != null) ? elm.Value : string.Empty;
                int logicalstno;
                if (!int.TryParse(tmp, out logicalstno))
                {
                    reason = "invalid logicalstno=" + tmp;
                    result2 = false;
                    return result2;
                }
                if (logicalstno <= 0 && logicalstno > 1023)
                {
                    reason = string.Format("logicalstno ={0} is out of range(1-1023)", tmp);
                    result2 = false;
                    return result2;
                }
                tmp = (((elm = x2.Element("istrigger")) != null) ? elm.Value : string.Empty);
                bool istrigger = true;
                if (tmp == "N")
                {
                    istrigger = false;
                }
                tmp = (((elm = x2.Element("isw2zr")) != null) ? elm.Value : string.Empty);
                bool isw2zr = false;
                if (tmp == "Y")
                {
                    isw2zr = true;
                }
                tmp = (((elm = x2.Element("isdefault")) != null) ? elm.Value : string.Empty);
                bool isdefault = true;
                if (tmp == "N")
                {
                    isdefault = false;
                }
                adapter.IsDefaultAdapter = isdefault;
                string plctype = ((elm = x2.Element("plctype")) != null) ? elm.Value : string.Empty;
                string linktype = ((elm = x2.Element("linktype")) != null) ? elm.Value : string.Empty;
                Hashtable paramTable = new Hashtable();
                foreach (XElement x3 in x.Elements("parameters"))
                {
                    foreach (XElement x4 in x3.Elements("parameter"))
                    {
                        string name = ((elm = x4.Element("name")) != null) ? elm.Value : string.Empty;
                        string value = ((elm = x4.Element("value")) != null) ? elm.Value : string.Empty;
                        paramTable.Add(name, value);
                    }
                }
                if (isdefault)
                {
                    if (this._DefaultAdapter != null)
                    {
                        reason = "exists duplicate default adapter";
                        result2 = false;
                        return result2;
                    }
                    this._DefaultAdapter = adapter;
                }
                if (this._Adapters.ContainsKey(logicalstno))
                {
                    reason = "exists duplicate logicalstno=" + logicalstno;
                    result2 = false;
                    return result2;
                }
                this._Adapters.Add(logicalstno, adapter);
                this._ConnectState.Add(logicalstno, false);
                PLCOpResult result = adapter.Init(plctype, linktype, paramTable, logicalstno, istrigger, isw2zr, 10000);
                if (result.RetCode != 0)
                {
                    throw new PLCDriverException(result.RetCode, result.RetErrMsg);
                }
            }
            if (this._Adapters.Values.Count == 0)
            {
                reason = "no adapter found";
                result2 = false;
            }
            else
            {
                if (this._DefaultAdapter == null)
                {
                    this._DefaultAdapter = this._Adapters.Values.First<PLCAdapter>();
                }
                reader.Close();
                reason = string.Empty;
                result2 = true;
            }
            return result2;
        }

        private bool CreateModel(string formatFileUrl, out string reason)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            this._Model = new PLCDataModel();
            XmlTextReader reader = new XmlTextReader(formatFileUrl);
            XDocument doc = XDocument.Load(reader);
            this.FormatVersion = string.Empty;
            for (XNode node = doc.FirstNode; node != null; node = node.NextNode)
            {
                if (node.NodeType == XmlNodeType.Comment)
                {
                    string s = node.ToString();
                    if (s.Contains("Version"))
                    {
                        string[] result = s.Split(new string[]
                        {
                            "--"
                        }, StringSplitOptions.None);
                        string[] array = result;
                        int i = 0;
                        while (i < array.Length)
                        {
                            string v = array[i];
                            if (v.Contains("Version"))
                            {
                                int idx = v.IndexOf(':');
                                if (idx < 0 || idx == v.Length)
                                {
                                    break;
                                }
                                this.FormatVersion = v.Substring(v.IndexOf(':') + 1);
                                break;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        break;
                    }
                }
            }
            XElement root = doc.Element("plcdriver");
            bool result2;
            if (root == null)
            {
                reason = string.Format("fail to create model from format xml={0},missing plcdriver config settings", formatFileUrl);
                result2 = false;
            }
            else
            {
                foreach (XElement x in root.Elements("datagathering"))
                {
                    foreach (XElement x2 in x.Elements("scan"))
                    {
                        foreach (XElement x3 in x2.Elements("watchdata"))
                        {
                            WatchData watch = this.CreateWatchData(x3, out reason);
                            if (watch == null)
                            {
                                result2 = false;
                                return result2;
                            }
                            this._Model.Scan.Add(watch.Name, watch);
                        }
                    }
                }
                foreach (XElement x in root.Elements("itemgroupcollection"))
                {
                    foreach (XElement x2 in x.Elements("itemgroup"))
                    {
                        ItemGroup itemgrp = this.CreateItemGroup(x2, out reason);
                        if (itemgrp == null)
                        {
                            result2 = false;
                            return result2;
                        }
                        this._Model.ItemGroupCollection.Add(itemgrp.Name, itemgrp);
                        foreach (XElement x3 in x2.Elements("item"))
                        {
                            Item item = this.CreateItem(x3, "itemgroupcollection itemgroup", out reason);
                            if (item == null)
                            {
                                result2 = false;
                                return result2;
                            }
                            itemgrp.Items.Add(item.Name, item);
                        }
                    }
                }
                foreach (XElement x in root.Elements("eventmap"))
                {
                    foreach (XElement x2 in x.Elements("event"))
                    {
                        Event evt = this.CreateEvent(x2, out reason);
                        if (evt == null)
                        {
                            result2 = false;
                            return result2;
                        }
                        this._Model.EventMap.Add(evt.Name, evt);
                        foreach (XElement x3 in x2.Elements("item"))
                        {
                            Item item = this.CreateItem(evt, x3, "eventmap event", out reason);
                            if (item == null)
                            {
                                result2 = false;
                                return result2;
                            }
                            if (evt.Metadata.IsBitDeviceType)
                            {
                                if (item.Metadata.BitOffset + item.Metadata.BitPoints > evt.Metadata.Points)
                                {
                                    reason = string.Format("eventmap event={0} item={1} out of range,boffset={2},bpoints={3} is beyond event points={4}", new object[]
                                    {
                                        evt.Name,
                                        item.Name,
                                        item.Metadata.BitOffset,
                                        item.Metadata.BitPoints,
                                        evt.Metadata.Points
                                    });
                                    result2 = false;
                                    return result2;
                                }
                            }
                            else if (item.Metadata.WordOffset + item.Metadata.WordPoints > evt.Metadata.Points)
                            {
                                reason = string.Format("eventmap event={0} item={1} out of range,woffset={2},wpoints={3} is beyond event points={4}", new object[]
                                {
                                    evt.Name,
                                    item.Name,
                                    item.Metadata.WordOffset,
                                    item.Metadata.WordPoints,
                                    evt.Metadata.Points
                                });
                                result2 = false;
                                return result2;
                            }
                            evt.Items.Add(item.Name, item);
                        }
                        foreach (XElement x3 in x2.Elements("itemgroup"))
                        {
                            XAttribute att = x3.Attribute("name");
                            if (att == null)
                            {
                                reason = string.Format("eventmap event={0} exists tag=itemgroup att=name ,att is null or empty", evt.Name);
                                result2 = false;
                                return result2;
                            }
                            ItemGroup itemgrp = this._Model.ItemGroupCollection.Get(att.Value);
                            if (itemgrp != null)
                            {
                                object[] allValues = itemgrp.Items.AllValues;
                                for (int i = 0; i < allValues.Length; i++)
                                {
                                    Item v2 = (Item)allValues[i];
                                    if (evt.Metadata.IsBitDeviceType)
                                    {
                                        if (v2.Metadata.BitOffset + v2.Metadata.BitPoints > evt.Metadata.Points)
                                        {
                                            reason = string.Format("eventmap event={0} itemgrp={1} item={2} out of range,boffset={3},bpoints={4} is beyond event points={5}", new object[]
                                            {
                                                evt.Name,
                                                itemgrp.Name,
                                                v2.Name,
                                                v2.Metadata.BitOffset,
                                                v2.Metadata.BitPoints,
                                                evt.Metadata.Points
                                            });
                                            result2 = false;
                                            return result2;
                                        }
                                    }
                                    else if (v2.Metadata.WordOffset + v2.Metadata.WordPoints > evt.Metadata.Points)
                                    {
                                        reason = string.Format("eventmap event={0} itemgrp={1} item={2} out of range,woffset={3},wpoints={4} is beyond event points={5}", new object[]
                                        {
                                            evt.Name,
                                            itemgrp.Name,
                                            v2.Name,
                                            v2.Metadata.WordOffset,
                                            v2.Metadata.WordPoints,
                                            evt.Metadata.Points
                                        });
                                        result2 = false;
                                        return result2;
                                    }
                                    evt.Items.Add(v2.Name, v2);
                                }
                            }
                        }
                    }
                }
                foreach (XElement x in root.Elements("transaction"))
                {
                    foreach (XElement x2 in x.Elements("receive"))
                    {
                        foreach (XElement x3 in x2.Elements("trx"))
                        {
                            Trx trx = this.CreateTrx(x3, "receive", out reason);
                            if (trx == null)
                            {
                                result2 = false;
                                return result2;
                            }
                            trx.Metadata.TrxType = TrxTypeEnum.Receive;
                            this._Model.Transaction.Add(trx.Name, trx);
                            foreach (XElement x4 in x3.Elements("eventgroup"))
                            {
                                EventGroup evtgrp = this.CreateEventGroup(x4, trx.Name, out reason);
                                if (evtgrp == null)
                                {
                                    result2 = false;
                                    return result2;
                                }
                                trx.EventGroups.Add(evtgrp.Name, evtgrp);
                                foreach (XElement x5 in x4.Elements("event"))
                                {
                                    XAttribute att = x5.Attribute("name");
                                    if (att == null)
                                    {
                                        reason = string.Format("transaction receive trx={0} exists tag=event att=name ,att is null or empty", trx.Name);
                                        result2 = false;
                                        return result2;
                                    }
                                    Event evt = this._Model.EventMap.Get(att.Value);
                                    if (evt == null)
                                    {
                                        reason = string.Format("transaction receive trx={0} event={1},related event can't not found in eventmap", trx.Name, att.Value);
                                        result2 = false;
                                        return result2;
                                    }
                                    att = x5.Attribute("trigger");
                                    if (att != null)
                                    {
                                        if (att.Value.ToUpper() == "TRUE")
                                        {
                                            evtgrp.TriggerEventNames.Add(evt.Name, evt.Name);
                                        }
                                    }
                                    evtgrp.Events.Add(evt.Name, evt);
                                }
                            }
                        }
                    }
                    foreach (XElement x2 in x.Elements("send"))
                    {
                        foreach (XElement x3 in x2.Elements("trx"))
                        {
                            Trx trx = this.CreateTrx(x3, "send", out reason);
                            if (trx == null)
                            {
                                result2 = false;
                                return result2;
                            }
                            trx.Metadata.TrxType = TrxTypeEnum.Send;
                            this._Model.Transaction.Add(trx.Name, trx);
                            foreach (XElement x4 in x3.Elements("eventgroup"))
                            {
                                EventGroup evtgrp = this.CreateEventGroup(x4, trx.Name, out reason);
                                if (evtgrp == null)
                                {
                                    result2 = false;
                                    return result2;
                                }
                                trx.EventGroups.Add(evtgrp.Name, evtgrp);
                                foreach (XElement x5 in x4.Elements("event"))
                                {
                                    XAttribute att = x5.Attribute("name");
                                    if (att == null)
                                    {
                                        reason = string.Format("transaction send trx={0} exists tag=event att=name ,att is null or empty", trx.Name);
                                        result2 = false;
                                        return result2;
                                    }
                                    Event evt = this._Model.EventMap.Get(att.Value);
                                    if (evt == null)
                                    {
                                        reason = string.Format("transaction send trx={0} event={1},related event can't not found in eventmap", trx.Name, att.Value);
                                        result2 = false;
                                        return result2;
                                    }
                                    att = x5.Attribute("trigger");
                                    if (att != null)
                                    {
                                        if (att.Value.ToUpper() == "TRUE")
                                        {
                                            evtgrp.TriggerEventNames.Add(evt.Name, evt.Name);
                                        }
                                    }
                                    evtgrp.Events.Add(evt.Name, evt);
                                }
                            }
                        }
                    }
                }
                foreach (XElement x in root.Elements("pair"))
                {
                    foreach (XElement x2 in x.Elements("trxcatalog"))
                    {
                        TrxCatalog catalog = this.CreateTrxCatalog(x2, out reason);
                        if (catalog == null)
                        {
                            result2 = false;
                            return result2;
                        }
                        this._Model.Pair.Add(catalog.FromTrxCatalog, catalog);
                        foreach (XElement x3 in x2.Elements("mapping"))
                        {
                            Mapping mapping = this.CreateMapping(x3, out reason);
                            if (mapping == null)
                            {
                                result2 = false;
                                return result2;
                            }
                            catalog.Mappings.Add(mapping.FromItem, mapping);
                        }
                    }
                }
                foreach (XElement x in root.Elements("itemdefinecollection"))
                {
                    foreach (XElement x2 in x.Elements("itemdefine"))
                    {
                        ItemDefine define = this.CreateItemDefine(x2, out reason);
                        if (define == null)
                        {
                            result2 = false;
                            return result2;
                        }
                        this._Model.ItemDefineCollection.Add(define.Name, define);
                        foreach (XElement x3 in x2.Elements("id"))
                        {
                            Id id = this.CreateId(x3, out reason);
                            if (id == null)
                            {
                                result2 = false;
                                return result2;
                            }
                            define.IDs.Add(id.Name, id);
                        }
                    }
                }
                reader.Close();
                sw.Stop();
                reason = string.Empty;
                result2 = true;
            }
            return result2;
        }

        private ItemGroup CreateItemGroup(XElement elm, out string reason)
        {
            ItemGroup itemgrp = new ItemGroup();
            foreach (XAttribute att in elm.Attributes())
            {
                string localName = att.Name.LocalName;
                if (localName != null)
                {
                    if (localName == "name")
                    {
                        itemgrp.Name = att.Value;
                    }
                }
            }
            ItemGroup result;
            if (string.IsNullOrEmpty(itemgrp.Name))
            {
                reason = string.Format("itemgroupcollection exists tag=itemgroup att=name,att is null or empty", new object[0]);
                result = null;
            }
            else
            {
                reason = string.Empty;
                result = itemgrp;
            }
            return result;
        }

        //private Item CreateItem(XElement elm, string scope, out string reason)
        //{
        //    Item item = new Item();
        //    reason = string.Empty;
        //    return item; 

        //}

        //private Item CreateItem(Event evt, XElement elm, string scope, out string reason)
        //{
        //    Item item = new Item();
        //    reason = string.Empty;
        //    return item; 
        //}

        private Item CreateItem(XElement elm, string scope, out string reason)
        {
            Item item = new Item();
            foreach (XAttribute att in elm.Attributes())
            {
                string text = att.Name.LocalName;
                if (text == null)
                {
                    goto IL_317;
                }
                if (_DataDictionary1 == null)
                {
                    _DataDictionary1 = new Dictionary<string, int>(6)
                    {
                        {
                            "name",
                            0
                        },
                        {
                            "woffset",
                            1
                        },
                        {
                            "boffset",
                            2
                        },
                        {
                            "wpoints",
                            3
                        },
                        {
                            "bpoints",
                            4
                        },
                        {
                            "expression",
                            5
                        }
                    };
                }
                int num;
                if (!_DataDictionary1.TryGetValue(text, out num))
                {
                    goto IL_317;
                }
                switch (num)
                {
                    case 0:
                        item.Name = att.Value;
                        break;
                    case 1:
                        {
                            int tmp = 0;
                            int.TryParse(att.Value, out tmp);
                            item.Metadata.WordOffset = tmp;
                            break;
                        }
                    case 2:
                        {
                            int tmp = 0;
                            int.TryParse(att.Value, out tmp);
                            item.Metadata.BitOffset = tmp;
                            break;
                        }
                    case 3:
                        {
                            int tmp = 0;
                            int.TryParse(att.Value, out tmp);
                            item.Metadata.WordPoints = tmp;
                            break;
                        }
                    case 4:
                        {
                            int tmp = 0;
                            int.TryParse(att.Value, out tmp);
                            item.Metadata.BitPoints = tmp;
                            break;
                        }
                    case 5:
                        text = att.Value.ToUpper();
                        if (text == null)
                        {
                            goto IL_306;
                        }
                        if (_DataDictionary2 == null)
                        {
                            _DataDictionary2 = new Dictionary<string, int>(10)
                        {
                            {
                                "HEX",
                                0
                            },
                            {
                                "ASCII",
                                1
                            },
                            {
                                "INT",
                                2
                            },
                            {
                                "SINT",
                                3
                            },
                            {
                                "LONG",
                                4
                            },
                            {
                                "SLONG",
                                5
                            },
                            {
                                "EXP",
                                6
                            },
                            {
                                "BIT",
                                7
                            },
                            {
                                "BIN",
                                8
                            },
                            {
                                "BCD",
                                9
                            },
                             {
                                "FLOAT",
                                10
                            },
                            {
                                "DOUBLE",
                                11
                            }
                        };
                        }
                        if (!_DataDictionary2.TryGetValue(text, out num))
                        {
                            goto IL_306;
                        }
                        switch (num)
                        {
                            case 0:
                                item.Metadata.Expression = ItemExpressionEnum.HEX;
                                break;
                            case 1:
                                item.Metadata.Expression = ItemExpressionEnum.ASCII;
                                break;
                            case 2:
                                item.Metadata.Expression = ItemExpressionEnum.INT;
                                break;
                            case 3:
                                item.Metadata.Expression = ItemExpressionEnum.SINT;
                                break;
                            case 4:
                                item.Metadata.Expression = ItemExpressionEnum.LONG;
                                break;
                            case 5:
                                item.Metadata.Expression = ItemExpressionEnum.SLONG;
                                break;
                            case 6:
                                item.Metadata.Expression = ItemExpressionEnum.EXP;
                                break;
                            case 7:
                                item.Metadata.Expression = ItemExpressionEnum.BIT;
                                break;
                            case 8:
                                item.Metadata.Expression = ItemExpressionEnum.BIN;
                                break;
                            case 9:
                                item.Metadata.Expression = ItemExpressionEnum.BCD;
                                break;
                            case 10:
                                item.Metadata.Expression = ItemExpressionEnum.FLOAT;
                                break;
                            case 11:
                                item.Metadata.Expression = ItemExpressionEnum.DOUBLE;
                                break;
                            default:
                                goto IL_306;
                        }
                        break;
                    IL_306:
                        item.Metadata.Expression = ItemExpressionEnum.NONE;
                        break;
                    default:
                        goto IL_317;
                }
                continue;
            IL_317:
                item.UserAttributes.Add(att.Name.LocalName, att.Value);
            }
            Item result;
            if (string.IsNullOrEmpty(item.Name))
            {
                reason = string.Format("{0} exists tag=item att=name,att is null or empty", scope);
                result = null;
            }
            else
            {
                ItemExpressionEnum expression = item.Metadata.Expression;
                if (expression <= ItemExpressionEnum.EXP)
                {
                    switch (expression)
                    {
                        case ItemExpressionEnum.INT:
                        case ItemExpressionEnum.SINT:
                            //  bruce.zhan 20171225
                            //if (item.Metadata.BitPoints < 1 || item.Metadata.BitPoints > 16)
                            //{
                            //    reason = string.Format("{0} item={1} att=bpoints,att value={2} err(1~16)", scope, item.Name, item.Metadata.BitPoints);
                            //    result = null;
                            //    return result;
                            //}
                            if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
                            {
                                reason = string.Format("{0} item={1} att=boffset,att value={2} err(0~15)", scope, item.Name, item.Metadata.BitOffset);
                                result = null;
                                return result;
                            }
                            goto IL_7B3;
                        case ItemExpressionEnum.INT | ItemExpressionEnum.SINT:
                            goto IL_7B3;
                        case ItemExpressionEnum.LONG:
                            break;
                        default:
                            if (expression != ItemExpressionEnum.SLONG)
                            {
                                if (expression != ItemExpressionEnum.EXP)
                                {
                                    goto IL_7B3;
                                }
                                if (item.Metadata.WordPoints != 2)
                                {
                                    reason = string.Format("{0} item={1} att=wpoints,att value={2} err(2)", scope, item.Name, item.Metadata.WordPoints);
                                    result = null;
                                    return result;
                                }
                                goto IL_7B3;
                            }
                            break;
                    }
                    if (item.Metadata.BitPoints < 1 || item.Metadata.BitPoints > 32)
                    {
                        reason = string.Format("{0} item={1} att=bpoints,att value={2} out of range(1~32)", scope, item.Name, item.Metadata.BitPoints);
                        result = null;
                        return result;
                    }
                    if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
                    {
                        reason = string.Format("{0} item={1} att=boffset,att value={2} out of range(0~15)", scope, item.Name, item.Metadata.BitOffset);
                        result = null;
                        return result;
                    }
                }
                else if (expression <= ItemExpressionEnum.HEX)
                {
                    if (expression != ItemExpressionEnum.ASCII)
                    {
                        if (expression == ItemExpressionEnum.HEX)
                        {
                            if (item.Metadata.BitPoints < 4 || item.Metadata.BitPoints % 4 != 0)
                            {
                                reason = string.Format("{0} item={1} att=bpoints,att value={2} err(4~4^M)", scope, item.Name, item.Metadata.BitPoints);
                                result = null;
                                return result;
                            }
                            if (item.Metadata.BitOffset != 0 && item.Metadata.BitOffset != 4 && item.Metadata.BitOffset != 8 && item.Metadata.BitOffset != 12)
                            {
                                reason = string.Format("{0} item={1} att=boffset,att value={2} err(0 or 4 or 8 or 12)", scope, item.Name, item.Metadata.BitOffset);
                                result = null;
                                return result;
                            }
                        }
                    }
                    else
                    {
                        if (item.Metadata.BitPoints < 8 || item.Metadata.BitPoints % 8 != 0)
                        {
                            reason = string.Format("{0} item={1} att=bpoints,att value={2} err(8~8^M)", scope, item.Name, item.Metadata.BitPoints);
                            result = null;
                            return result;
                        }
                        if (item.Metadata.BitOffset != 0 && item.Metadata.BitOffset != 8)
                        {
                            reason = string.Format("{0} item={1} att=boffset,att value={2} err(0 or 8)", scope, item.Name, item.Metadata.BitOffset);
                            result = null;
                            return result;
                        }
                    }
                }
                else if (expression != ItemExpressionEnum.BIN)
                {
                    if (expression == ItemExpressionEnum.BCD)
                    {
                        if (item.Metadata.BitPoints < 16 || item.Metadata.BitPoints % 16 != 0)
                        {
                            reason = string.Format("{0} item={1} att=bpoints,att value={2} err(16~16^M)", scope, item.Name, item.Metadata.BitPoints);
                            result = null;
                            return result;
                        }
                    }
                }
                else if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
                {
                    reason = string.Format("{0} item={1} att=boffset,att value={2} err(0~15)", scope, item.Name, item.Metadata.BitOffset);
                    result = null;
                    return result;
                }
            IL_7B3:
                if (item.Metadata.Expression != ItemExpressionEnum.BIT && item.Metadata.Expression != ItemExpressionEnum.EXP && item.Metadata.Expression != ItemExpressionEnum.NONE)
                {
                    if ((item.Metadata.BitOffset + item.Metadata.BitPoints) % 16 > 0)
                    {
                        if (item.Metadata.WordPoints != (item.Metadata.BitOffset + item.Metadata.BitPoints) / 16 + 1)
                        {
                            reason = string.Format("{0} item={1} wpoints check error,wpoints={2} != (boffset={3}+bpoints={4})/16+1", new object[]
                            {
                                scope,
                                item.Name,
                                item.Metadata.WordPoints,
                                item.Metadata.BitOffset,
                                item.Metadata.BitPoints
                            });
                            result = null;
                            return result;
                        }
                    }
                    else if (item.Metadata.WordPoints != (item.Metadata.BitOffset + item.Metadata.BitPoints) / 16)
                    {
                        reason = string.Format("{0} item={1} wpoints check error,wpoints={2} != (boffset={3}+bpoints={4})/16", new object[]
                        {
                            scope,
                            item.Name,
                            item.Metadata.WordPoints,
                            item.Metadata.BitOffset,
                            item.Metadata.BitPoints
                        });
                        result = null;
                        return result;
                    }
                }
                reason = string.Empty;
                result = item;
            }
            return result;
        }

        private Item CreateItem(Event evt, XElement elm, string scope, out string reason)
        {
            Item item = new Item();
            foreach (XAttribute att in elm.Attributes())
            {
                string text = att.Name.LocalName;
                if (text == null)
                {
                    goto IL_2DA;
                }
                if (!(text == "name"))
                {
                    if (!(text == "offset"))
                    {
                        if (!(text == "points"))
                        {
                            if (!(text == "expression"))
                            {
                                goto IL_2DA;
                            }
                            text = att.Value.ToUpper();
                            if (text == null)
                            {
                                goto IL_2C9;
                            }
                            if (_DataDictionary2 == null)
                            {
                                _DataDictionary2 = new Dictionary<string, int>(10)
                                {
                                    {
                                        "HEX",
                                        0
                                    },
                                    {
                                        "ASCII",
                                        1
                                    },
                                    {
                                        "INT",
                                        2
                                    },
                                    {
                                        "SINT",
                                        3
                                    },
                                    {
                                        "LONG",
                                        4
                                    },
                                    {
                                        "SLONG",
                                        5
                                    },
                                    {
                                        "EXP",
                                        6
                                    },
                                    {
                                        "BIT",
                                        7
                                    },
                                    {
                                        "BIN",
                                        8
                                    },
                                    {
                                        "BCD",
                                        9
                                    },
                                    {
                                        "FLOAT",
                                        10
                                    },
                                    {
                                        "DOUBLE",
                                        11
                                    }
                                };
                            }
                            int num;
                            if (!_DataDictionary2.TryGetValue(text, out num))
                            {
                                goto IL_2C9;
                            }
                            switch (num)
                            {
                                case 0:
                                    item.Metadata.Expression = ItemExpressionEnum.HEX;
                                    break;
                                case 1:
                                    item.Metadata.Expression = ItemExpressionEnum.ASCII;
                                    break;
                                case 2:
                                    item.Metadata.Expression = ItemExpressionEnum.INT;
                                    break;
                                case 3:
                                    item.Metadata.Expression = ItemExpressionEnum.SINT;
                                    break;
                                case 4:
                                    item.Metadata.Expression = ItemExpressionEnum.LONG;
                                    break;
                                case 5:
                                    item.Metadata.Expression = ItemExpressionEnum.SLONG;
                                    break;
                                case 6:
                                    item.Metadata.Expression = ItemExpressionEnum.EXP;
                                    break;
                                case 7:
                                    item.Metadata.Expression = ItemExpressionEnum.BIT;
                                    break;
                                case 8:
                                    item.Metadata.Expression = ItemExpressionEnum.BIN;
                                    break;
                                case 9:
                                    item.Metadata.Expression = ItemExpressionEnum.BCD;
                                    break;
                                case 10:
                                    item.Metadata.Expression = ItemExpressionEnum.FLOAT;
                                    break;
                                case 11:
                                    item.Metadata.Expression = ItemExpressionEnum.DOUBLE;
                                    break;
                                default:
                                    goto IL_2C9;
                            }
                            continue;
                        IL_2C9:
                            item.Metadata.Expression = ItemExpressionEnum.NONE;
                        }
                        else
                        {
                            int tmp = 0;
                            int.TryParse(att.Value, out tmp);
                            if (evt.Metadata.IsBitDeviceType)
                            {
                                item.Metadata.BitPoints = tmp;
                            }
                            else
                            {
                                item.Metadata.WordPoints = tmp;
                                item.Metadata.BitPoints = tmp * 16;
                            }
                        }
                    }
                    else
                    {
                        int tmp = 0;
                        int.TryParse(att.Value, out tmp);
                        if (evt.Metadata.IsBitDeviceType)
                        {
                            item.Metadata.BitOffset = tmp;
                        }
                        else
                        {
                            item.Metadata.WordOffset = tmp;
                        }
                    }
                }
                else
                {
                    item.Name = att.Value;
                }
                continue;
            IL_2DA:
                item.UserAttributes.Add(att.Name.LocalName, att.Value);
            }
            Item result;
            if (string.IsNullOrEmpty(item.Name))
            {
                reason = string.Format("{0} exists tag=item att=name,att is null or empty", scope);
                result = null;
            }
            else
            {
                ItemExpressionEnum expression = item.Metadata.Expression;
                if (expression <= ItemExpressionEnum.EXP)
                {
                    switch (expression)
                    {
                        case ItemExpressionEnum.INT:
                        case ItemExpressionEnum.SINT:
                            //bruce.zhan 20171225
                            //if (item.Metadata.BitPoints < 1 || item.Metadata.BitPoints > 16)
                            //{
                            //    reason = string.Format("{0} item={1} att=bpoints,att value={2} err(1~16)", scope, item.Name, item.Metadata.BitPoints);
                            //    result = null;
                            //    return result;
                            //}
                            if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
                            {
                                reason = string.Format("{0} item={1} att=boffset,att value={2} err(0~15)", scope, item.Name, item.Metadata.BitOffset);
                                result = null;
                                return result;
                            }
                            goto IL_782;
                        case ItemExpressionEnum.INT | ItemExpressionEnum.SINT:
                            goto IL_782;
                        case ItemExpressionEnum.LONG:
                            break;
                        default:
                            if (expression != ItemExpressionEnum.SLONG)
                            {
                                if (expression != ItemExpressionEnum.EXP)
                                {
                                    goto IL_782;
                                }
                                if (item.Metadata.WordPoints != 2)
                                {
                                    reason = string.Format("{0} item={1} att=wpoints,att value={2} err(2)", scope, item.Name, item.Metadata.WordPoints);
                                    result = null;
                                    return result;
                                }
                                goto IL_782;
                            }
                            break;
                    }
                    if (item.Metadata.BitPoints < 1 || item.Metadata.BitPoints > 32)
                    {
                        reason = string.Format("{0} item={1} att=bpoints,att value={2} out of range(1~32)", scope, item.Name, item.Metadata.BitPoints);
                        result = null;
                        return result;
                    }
                    if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
                    {
                        reason = string.Format("{0} item={1} att=boffset,att value={2} out of range(0~15)", scope, item.Name, item.Metadata.BitOffset);
                        result = null;
                        return result;
                    }
                }
                else if (expression <= ItemExpressionEnum.HEX)
                {
                    if (expression != ItemExpressionEnum.ASCII)
                    {
                        if (expression == ItemExpressionEnum.HEX)
                        {
                            if (item.Metadata.BitPoints < 4 || item.Metadata.BitPoints % 4 != 0)
                            {
                                reason = string.Format("{0} item={1} att=bpoints,att value={2} err(4~4^M)", scope, item.Name, item.Metadata.BitPoints);
                                result = null;
                                return result;
                            }
                            if (item.Metadata.BitOffset != 0 && item.Metadata.BitOffset != 4 && item.Metadata.BitOffset != 8 && item.Metadata.BitOffset != 12)
                            {
                                reason = string.Format("{0} item={1} att=boffset,att value={2} err(0 or 4 or 8 or 12)", scope, item.Name, item.Metadata.BitOffset);
                                result = null;
                                return result;
                            }
                        }
                    }
                    else
                    {
                        if (item.Metadata.BitPoints < 8 || item.Metadata.BitPoints % 8 != 0)
                        {
                            reason = string.Format("{0} item={1} att=bpoints,att value={2} err(8~8^M)", scope, item.Name, item.Metadata.BitPoints);
                            result = null;
                            return result;
                        }
                        if (item.Metadata.BitOffset != 0 && item.Metadata.BitOffset != 8)
                        {
                            reason = string.Format("{0} item={1} att=boffset,att value={2} err(0 or 8)", scope, item.Name, item.Metadata.BitOffset);
                            result = null;
                            return result;
                        }
                    }
                }
                else if (expression != ItemExpressionEnum.BIN)
                {
                    if (expression == ItemExpressionEnum.BCD)
                    {
                        if (item.Metadata.BitPoints < 16 || item.Metadata.BitPoints % 16 != 0)
                        {
                            reason = string.Format("{0} item={1} att=bpoints,att value={2} err(16~16^M)", scope, item.Name, item.Metadata.BitPoints);
                            result = null;
                            return result;
                        }
                    }
                }
                else if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
                {
                    reason = string.Format("{0} item={1} att=boffset,att value={2} err(0~15)", scope, item.Name, item.Metadata.BitOffset);
                    result = null;
                    return result;
                }
            IL_782:
                if (item.Metadata.Expression != ItemExpressionEnum.BIT && item.Metadata.Expression != ItemExpressionEnum.EXP && item.Metadata.Expression != ItemExpressionEnum.NONE)
                {
                    if ((item.Metadata.BitOffset + item.Metadata.BitPoints) % 16 > 0)
                    {
                        if (item.Metadata.WordPoints != (item.Metadata.BitOffset + item.Metadata.BitPoints) / 16 + 1)
                        {
                            reason = string.Format("{0} item={1} wpoints check error,wpoints={2} != (boffset={3}+bpoints={4})/16+1", new object[]
                            {
                                scope,
                                item.Name,
                                item.Metadata.WordPoints,
                                item.Metadata.BitOffset,
                                item.Metadata.BitPoints
                            });
                            result = null;
                            return result;
                        }
                    }
                    else if (item.Metadata.WordPoints != (item.Metadata.BitOffset + item.Metadata.BitPoints) / 16)
                    {
                        reason = string.Format("{0} item={1} wpoints check error,wpoints={2} != (boffset={3}+bpoints={4})/16", new object[]
                        {
                            scope,
                            item.Name,
                            item.Metadata.WordPoints,
                            item.Metadata.BitOffset,
                            item.Metadata.BitPoints
                        });
                        result = null;
                        return result;
                    }
                }
                reason = string.Empty;
                result = item;
            }
            return result;
        }


        #region  
        //private Item CreateItem(XElement elm, string scope, out string reason)
        //{
        //    Item item = new Item();
        //    foreach (XAttribute att in elm.Attributes())
        //    {
        //        string text = att.Name.LocalName;
        //        if (text == null)
        //        {
        //            goto IL_317;
        //        }
        //        if (<PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c6-1 == null)
        //        {
        //            <PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c6-1 = new Dictionary<string, int>(6)
        //            {
        //                {
        //                    "name",
        //                    0
        //                },
        //                {
        //                    "woffset",
        //                    1
        //                },
        //                {
        //                    "boffset",
        //                    2
        //                },
        //                {
        //                    "wpoints",
        //                    3
        //                },
        //                {
        //                    "bpoints",
        //                    4
        //                },
        //                {
        //                    "expression",
        //                    5
        //                }
        //            };
        //        }
        //        int num;
        //        if (!<PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c6-1.TryGetValue(text, out num))
        //        {
        //            goto IL_317;
        //        }
        //        switch (num)
        //        {
        //        case 0:
        //            item.Name = att.Value;
        //            break;
        //        case 1:
        //        {
        //            int tmp = 0;
        //            int.TryParse(att.Value, out tmp);
        //            item.Metadata.WordOffset = tmp;
        //            break;
        //        }
        //        case 2:
        //        {
        //            int tmp = 0;
        //            int.TryParse(att.Value, out tmp);
        //            item.Metadata.BitOffset = tmp;
        //            break;
        //        }
        //        case 3:
        //        {
        //            int tmp = 0;
        //            int.TryParse(att.Value, out tmp);
        //            item.Metadata.WordPoints = tmp;
        //            break;
        //        }
        //        case 4:
        //        {
        //            int tmp = 0;
        //            int.TryParse(att.Value, out tmp);
        //            item.Metadata.BitPoints = tmp;
        //            break;
        //        }
        //        case 5:
        //            text = att.Value.ToUpper();
        //            if (text == null)
        //            {
        //                goto IL_306;
        //            }
        //            if (<PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c6-2 == null)
        //            {
        //                <PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c6-2 = new Dictionary<string, int>(10)
        //                {
        //                    {
        //                        "HEX",
        //                        0
        //                    },
        //                    {
        //                        "ASCII",
        //                        1
        //                    },
        //                    {
        //                        "INT",
        //                        2
        //                    },
        //                    {
        //                        "SINT",
        //                        3
        //                    },
        //                    {
        //                        "LONG",
        //                        4
        //                    },
        //                    {
        //                        "SLONG",
        //                        5
        //                    },
        //                    {
        //                        "EXP",
        //                        6
        //                    },
        //                    {
        //                        "BIT",
        //                        7
        //                    },
        //                    {
        //                        "BIN",
        //                        8
        //                    },
        //                    {
        //                        "BCD",
        //                        9
        //                    }
        //                };
        //            }
        //            if (!<PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c6-2.TryGetValue(text, out num))
        //            {
        //                goto IL_306;
        //            }
        //            switch (num)
        //            {
        //            case 0:
        //                item.Metadata.Expression = ItemExpressionEnum.HEX;
        //                break;
        //            case 1:
        //                item.Metadata.Expression = ItemExpressionEnum.ASCII;
        //                break;
        //            case 2:
        //                item.Metadata.Expression = ItemExpressionEnum.INT;
        //                break;
        //            case 3:
        //                item.Metadata.Expression = ItemExpressionEnum.SINT;
        //                break;
        //            case 4:
        //                item.Metadata.Expression = ItemExpressionEnum.LONG;
        //                break;
        //            case 5:
        //                item.Metadata.Expression = ItemExpressionEnum.SLONG;
        //                break;
        //            case 6:
        //                item.Metadata.Expression = ItemExpressionEnum.EXP;
        //                break;
        //            case 7:
        //                item.Metadata.Expression = ItemExpressionEnum.BIT;
        //                break;
        //            case 8:
        //                item.Metadata.Expression = ItemExpressionEnum.BIN;
        //                break;
        //            case 9:
        //                item.Metadata.Expression = ItemExpressionEnum.BCD;
        //                break;
        //            default:
        //                goto IL_306;
        //            }
        //            break;
        //            IL_306:
        //            item.Metadata.Expression = ItemExpressionEnum.NONE;
        //            break;
        //        default:
        //            goto IL_317;
        //        }
        //        continue;
        //        IL_317:
        //        item.UserAttributes.Add(att.Name.LocalName, att.Value);
        //    }
        //    Item result;
        //    if (string.IsNullOrEmpty(item.Name))
        //    {
        //        reason = string.Format("{0} exists tag=item att=name,att is null or empty", scope);
        //        result = null;
        //    }
        //    else
        //    {
        //        ItemExpressionEnum expression = item.Metadata.Expression;
        //        if (expression <= ItemExpressionEnum.EXP)
        //        {
        //            switch (expression)
        //            {
        //            case ItemExpressionEnum.INT:
        //            case ItemExpressionEnum.SINT:
        //                if (item.Metadata.BitPoints < 1 || item.Metadata.BitPoints > 16)
        //                {
        //                    reason = string.Format("{0} item={1} att=bpoints,att value={2} err(1~16)", scope, item.Name, item.Metadata.BitPoints);
        //                    result = null;
        //                    return result;
        //                }
        //                if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
        //                {
        //                    reason = string.Format("{0} item={1} att=boffset,att value={2} err(0~15)", scope, item.Name, item.Metadata.BitOffset);
        //                    result = null;
        //                    return result;
        //                }
        //                goto IL_7B3;
        //            case ItemExpressionEnum.INT | ItemExpressionEnum.SINT:
        //                goto IL_7B3;
        //            case ItemExpressionEnum.LONG:
        //                break;
        //            default:
        //                if (expression != ItemExpressionEnum.SLONG)
        //                {
        //                    if (expression != ItemExpressionEnum.EXP)
        //                    {
        //                        goto IL_7B3;
        //                    }
        //                    if (item.Metadata.WordPoints != 2)
        //                    {
        //                        reason = string.Format("{0} item={1} att=wpoints,att value={2} err(2)", scope, item.Name, item.Metadata.WordPoints);
        //                        result = null;
        //                        return result;
        //                    }
        //                    goto IL_7B3;
        //                }
        //                break;
        //            }
        //            if (item.Metadata.BitPoints < 1 || item.Metadata.BitPoints > 32)
        //            {
        //                reason = string.Format("{0} item={1} att=bpoints,att value={2} out of range(1~32)", scope, item.Name, item.Metadata.BitPoints);
        //                result = null;
        //                return result;
        //            }
        //            if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
        //            {
        //                reason = string.Format("{0} item={1} att=boffset,att value={2} out of range(0~15)", scope, item.Name, item.Metadata.BitOffset);
        //                result = null;
        //                return result;
        //            }
        //        }
        //        else if (expression <= ItemExpressionEnum.HEX)
        //        {
        //            if (expression != ItemExpressionEnum.ASCII)
        //            {
        //                if (expression == ItemExpressionEnum.HEX)
        //                {
        //                    if (item.Metadata.BitPoints < 4 || item.Metadata.BitPoints % 4 != 0)
        //                    {
        //                        reason = string.Format("{0} item={1} att=bpoints,att value={2} err(4~4^M)", scope, item.Name, item.Metadata.BitPoints);
        //                        result = null;
        //                        return result;
        //                    }
        //                    if (item.Metadata.BitOffset != 0 && item.Metadata.BitOffset != 4 && item.Metadata.BitOffset != 8 && item.Metadata.BitOffset != 12)
        //                    {
        //                        reason = string.Format("{0} item={1} att=boffset,att value={2} err(0 or 4 or 8 or 12)", scope, item.Name, item.Metadata.BitOffset);
        //                        result = null;
        //                        return result;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (item.Metadata.BitPoints < 8 || item.Metadata.BitPoints % 8 != 0)
        //                {
        //                    reason = string.Format("{0} item={1} att=bpoints,att value={2} err(8~8^M)", scope, item.Name, item.Metadata.BitPoints);
        //                    result = null;
        //                    return result;
        //                }
        //                if (item.Metadata.BitOffset != 0 && item.Metadata.BitOffset != 8)
        //                {
        //                    reason = string.Format("{0} item={1} att=boffset,att value={2} err(0 or 8)", scope, item.Name, item.Metadata.BitOffset);
        //                    result = null;
        //                    return result;
        //                }
        //            }
        //        }
        //        else if (expression != ItemExpressionEnum.BIN)
        //        {
        //            if (expression == ItemExpressionEnum.BCD)
        //            {
        //                if (item.Metadata.BitPoints < 16 || item.Metadata.BitPoints % 16 != 0)
        //                {
        //                    reason = string.Format("{0} item={1} att=bpoints,att value={2} err(16~16^M)", scope, item.Name, item.Metadata.BitPoints);
        //                    result = null;
        //                    return result;
        //                }
        //            }
        //        }
        //        else if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
        //        {
        //            reason = string.Format("{0} item={1} att=boffset,att value={2} err(0~15)", scope, item.Name, item.Metadata.BitOffset);
        //            result = null;
        //            return result;
        //        }
        //        IL_7B3:
        //        if (item.Metadata.Expression != ItemExpressionEnum.BIT && item.Metadata.Expression != ItemExpressionEnum.EXP && item.Metadata.Expression != ItemExpressionEnum.NONE)
        //        {
        //            if ((item.Metadata.BitOffset + item.Metadata.BitPoints) % 16 > 0)
        //            {
        //                if (item.Metadata.WordPoints != (item.Metadata.BitOffset + item.Metadata.BitPoints) / 16 + 1)
        //                {
        //                    reason = string.Format("{0} item={1} wpoints check error,wpoints={2} != (boffset={3}+bpoints={4})/16+1", new object[]
        //                    {
        //                        scope,
        //                        item.Name,
        //                        item.Metadata.WordPoints,
        //                        item.Metadata.BitOffset,
        //                        item.Metadata.BitPoints
        //                    });
        //                    result = null;
        //                    return result;
        //                }
        //            }
        //            else if (item.Metadata.WordPoints != (item.Metadata.BitOffset + item.Metadata.BitPoints) / 16)
        //            {
        //                reason = string.Format("{0} item={1} wpoints check error,wpoints={2} != (boffset={3}+bpoints={4})/16", new object[]
        //                {
        //                    scope,
        //                    item.Name,
        //                    item.Metadata.WordPoints,
        //                    item.Metadata.BitOffset,
        //                    item.Metadata.BitPoints
        //                });
        //                result = null;
        //                return result;
        //            }
        //        }
        //        reason = string.Empty;
        //        result = item;
        //    }
        //    return result;
        //}

        //private Item CreateItem(Event evt, XElement elm, string scope, out string reason)
        //{
        //    Item item = new Item();
        //    foreach (XAttribute att in elm.Attributes())
        //    {
        //        string text = att.Name.LocalName;
        //        if (text == null)
        //        {
        //            goto IL_2DA;
        //        }
        //        if (!(text == "name"))
        //        {
        //            if (!(text == "offset"))
        //            {
        //                if (!(text == "points"))
        //                {
        //                    if (!(text == "expression"))
        //                    {
        //                        goto IL_2DA;
        //                    }
        //                    text = att.Value.ToUpper();
        //                    if (text == null)
        //                    {
        //                        goto IL_2C9;
        //                    }
        //                    if (<PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c7-1 == null)
        //                    {
        //                        <PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c7-1 = new Dictionary<string, int>(10)
        //                        {
        //                            {
        //                                "HEX",
        //                                0
        //                            },
        //                            {
        //                                "ASCII",
        //                                1
        //                            },
        //                            {
        //                                "INT",
        //                                2
        //                            },
        //                            {
        //                                "SINT",
        //                                3
        //                            },
        //                            {
        //                                "LONG",
        //                                4
        //                            },
        //                            {
        //                                "SLONG",
        //                                5
        //                            },
        //                            {
        //                                "EXP",
        //                                6
        //                            },
        //                            {
        //                                "BIT",
        //                                7
        //                            },
        //                            {
        //                                "BIN",
        //                                8
        //                            },
        //                            {
        //                                "BCD",
        //                                9
        //                            }
        //                        };
        //                    }
        //                    int num;
        //                    if (!<PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000c7-1.TryGetValue(text, out num))
        //                    {
        //                        goto IL_2C9;
        //                    }
        //                    switch (num)
        //                    {
        //                    case 0:
        //                        item.Metadata.Expression = ItemExpressionEnum.HEX;
        //                        break;
        //                    case 1:
        //                        item.Metadata.Expression = ItemExpressionEnum.ASCII;
        //                        break;
        //                    case 2:
        //                        item.Metadata.Expression = ItemExpressionEnum.INT;
        //                        break;
        //                    case 3:
        //                        item.Metadata.Expression = ItemExpressionEnum.SINT;
        //                        break;
        //                    case 4:
        //                        item.Metadata.Expression = ItemExpressionEnum.LONG;
        //                        break;
        //                    case 5:
        //                        item.Metadata.Expression = ItemExpressionEnum.SLONG;
        //                        break;
        //                    case 6:
        //                        item.Metadata.Expression = ItemExpressionEnum.EXP;
        //                        break;
        //                    case 7:
        //                        item.Metadata.Expression = ItemExpressionEnum.BIT;
        //                        break;
        //                    case 8:
        //                        item.Metadata.Expression = ItemExpressionEnum.BIN;
        //                        break;
        //                    case 9:
        //                        item.Metadata.Expression = ItemExpressionEnum.BCD;
        //                        break;
        //                    default:
        //                        goto IL_2C9;
        //                    }
        //                    continue;
        //                    IL_2C9:
        //                    item.Metadata.Expression = ItemExpressionEnum.NONE;
        //                }
        //                else
        //                {
        //                    int tmp = 0;
        //                    int.TryParse(att.Value, out tmp);
        //                    if (evt.Metadata.IsBitDeviceType)
        //                    {
        //                        item.Metadata.BitPoints = tmp;
        //                    }
        //                    else
        //                    {
        //                        item.Metadata.WordPoints = tmp;
        //                        item.Metadata.BitPoints = tmp * 16;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                int tmp = 0;
        //                int.TryParse(att.Value, out tmp);
        //                if (evt.Metadata.IsBitDeviceType)
        //                {
        //                    item.Metadata.BitOffset = tmp;
        //                }
        //                else
        //                {
        //                    item.Metadata.WordOffset = tmp;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            item.Name = att.Value;
        //        }
        //        continue;
        //        IL_2DA:
        //        item.UserAttributes.Add(att.Name.LocalName, att.Value);
        //    }
        //    Item result;
        //    if (string.IsNullOrEmpty(item.Name))
        //    {
        //        reason = string.Format("{0} exists tag=item att=name,att is null or empty", scope);
        //        result = null;
        //    }
        //    else
        //    {
        //        ItemExpressionEnum expression = item.Metadata.Expression;
        //        if (expression <= ItemExpressionEnum.EXP)
        //        {
        //            switch (expression)
        //            {
        //            case ItemExpressionEnum.INT:
        //            case ItemExpressionEnum.SINT:
        //                if (item.Metadata.BitPoints < 1 || item.Metadata.BitPoints > 16)
        //                {
        //                    reason = string.Format("{0} item={1} att=bpoints,att value={2} err(1~16)", scope, item.Name, item.Metadata.BitPoints);
        //                    result = null;
        //                    return result;
        //                }
        //                if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
        //                {
        //                    reason = string.Format("{0} item={1} att=boffset,att value={2} err(0~15)", scope, item.Name, item.Metadata.BitOffset);
        //                    result = null;
        //                    return result;
        //                }
        //                goto IL_782;
        //            case ItemExpressionEnum.INT | ItemExpressionEnum.SINT:
        //                goto IL_782;
        //            case ItemExpressionEnum.LONG:
        //                break;
        //            default:
        //                if (expression != ItemExpressionEnum.SLONG)
        //                {
        //                    if (expression != ItemExpressionEnum.EXP)
        //                    {
        //                        goto IL_782;
        //                    }
        //                    if (item.Metadata.WordPoints != 2)
        //                    {
        //                        reason = string.Format("{0} item={1} att=wpoints,att value={2} err(2)", scope, item.Name, item.Metadata.WordPoints);
        //                        result = null;
        //                        return result;
        //                    }
        //                    goto IL_782;
        //                }
        //                break;
        //            }
        //            if (item.Metadata.BitPoints < 1 || item.Metadata.BitPoints > 32)
        //            {
        //                reason = string.Format("{0} item={1} att=bpoints,att value={2} out of range(1~32)", scope, item.Name, item.Metadata.BitPoints);
        //                result = null;
        //                return result;
        //            }
        //            if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
        //            {
        //                reason = string.Format("{0} item={1} att=boffset,att value={2} out of range(0~15)", scope, item.Name, item.Metadata.BitOffset);
        //                result = null;
        //                return result;
        //            }
        //        }
        //        else if (expression <= ItemExpressionEnum.HEX)
        //        {
        //            if (expression != ItemExpressionEnum.ASCII)
        //            {
        //                if (expression == ItemExpressionEnum.HEX)
        //                {
        //                    if (item.Metadata.BitPoints < 4 || item.Metadata.BitPoints % 4 != 0)
        //                    {
        //                        reason = string.Format("{0} item={1} att=bpoints,att value={2} err(4~4^M)", scope, item.Name, item.Metadata.BitPoints);
        //                        result = null;
        //                        return result;
        //                    }
        //                    if (item.Metadata.BitOffset != 0 && item.Metadata.BitOffset != 4 && item.Metadata.BitOffset != 8 && item.Metadata.BitOffset != 12)
        //                    {
        //                        reason = string.Format("{0} item={1} att=boffset,att value={2} err(0 or 4 or 8 or 12)", scope, item.Name, item.Metadata.BitOffset);
        //                        result = null;
        //                        return result;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (item.Metadata.BitPoints < 8 || item.Metadata.BitPoints % 8 != 0)
        //                {
        //                    reason = string.Format("{0} item={1} att=bpoints,att value={2} err(8~8^M)", scope, item.Name, item.Metadata.BitPoints);
        //                    result = null;
        //                    return result;
        //                }
        //                if (item.Metadata.BitOffset != 0 && item.Metadata.BitOffset != 8)
        //                {
        //                    reason = string.Format("{0} item={1} att=boffset,att value={2} err(0 or 8)", scope, item.Name, item.Metadata.BitOffset);
        //                    result = null;
        //                    return result;
        //                }
        //            }
        //        }
        //        else if (expression != ItemExpressionEnum.BIN)
        //        {
        //            if (expression == ItemExpressionEnum.BCD)
        //            {
        //                if (item.Metadata.BitPoints < 16 || item.Metadata.BitPoints % 16 != 0)
        //                {
        //                    reason = string.Format("{0} item={1} att=bpoints,att value={2} err(16~16^M)", scope, item.Name, item.Metadata.BitPoints);
        //                    result = null;
        //                    return result;
        //                }
        //            }
        //        }
        //        else if (item.Metadata.BitOffset < 0 || item.Metadata.BitOffset > 15)
        //        {
        //            reason = string.Format("{0} item={1} att=boffset,att value={2} err(0~15)", scope, item.Name, item.Metadata.BitOffset);
        //            result = null;
        //            return result;
        //        }
        //        IL_782:
        //        if (item.Metadata.Expression != ItemExpressionEnum.BIT && item.Metadata.Expression != ItemExpressionEnum.EXP && item.Metadata.Expression != ItemExpressionEnum.NONE)
        //        {
        //            if ((item.Metadata.BitOffset + item.Metadata.BitPoints) % 16 > 0)
        //            {
        //                if (item.Metadata.WordPoints != (item.Metadata.BitOffset + item.Metadata.BitPoints) / 16 + 1)
        //                {
        //                    reason = string.Format("{0} item={1} wpoints check error,wpoints={2} != (boffset={3}+bpoints={4})/16+1", new object[]
        //                    {
        //                        scope,
        //                        item.Name,
        //                        item.Metadata.WordPoints,
        //                        item.Metadata.BitOffset,
        //                        item.Metadata.BitPoints
        //                    });
        //                    result = null;
        //                    return result;
        //                }
        //            }
        //            else if (item.Metadata.WordPoints != (item.Metadata.BitOffset + item.Metadata.BitPoints) / 16)
        //            {
        //                reason = string.Format("{0} item={1} wpoints check error,wpoints={2} != (boffset={3}+bpoints={4})/16", new object[]
        //                {
        //                    scope,
        //                    item.Name,
        //                    item.Metadata.WordPoints,
        //                    item.Metadata.BitOffset,
        //                    item.Metadata.BitPoints
        //                });
        //                result = null;
        //                return result;
        //            }
        //        }
        //        reason = string.Empty;
        //        result = item;
        //    }
        //    return result;
        //}

        #endregion


        private Event CreateEvent(XElement elm, out string reason)
        {
            Event evt = new Event();
            foreach (XAttribute att in elm.Attributes())
            {
                string localName = att.Name.LocalName;
                if (localName != null)
                {
                    if (!(localName == "name"))
                    {
                        if (!(localName == "logicalstno"))
                        {
                            if (!(localName == "devicecode"))
                            {
                                if (!(localName == "address"))
                                {
                                    if (!(localName == "points"))
                                    {
                                        if (localName == "skipdecode")
                                        {
                                            bool bol = false;
                                            bool.TryParse(att.Value, out bol);
                                            evt.Metadata.SkipDecode = bol;
                                        }
                                    }
                                    else
                                    {
                                        int tmp = 0;
                                        int.TryParse(att.Value, out tmp);
                                        evt.Metadata.Points = tmp;
                                    }
                                }
                                else
                                {
                                    evt.Metadata.Address = att.Value;
                                    evt.Metadata.OriginalAddress = evt.Metadata.Address;
                                }
                            }
                            else
                            {
                                evt.Metadata.DeviceCode = att.Value;
                                evt.Metadata.OriginalDeviceCode = evt.Metadata.DeviceCode;
                            }
                        }
                        else
                        {
                            int tmp = 0;
                            int.TryParse(att.Value, out tmp);
                            evt.Metadata.LogicalStationNo = tmp;
                        }
                    }
                    else
                    {
                        evt.Name = att.Value;
                    }
                }
            }
            Event result;
            if (string.IsNullOrEmpty(evt.Name))
            {
                reason = string.Format("eventmap exists tag=event att=name,att is null or empty", new object[0]);
                result = null;
            }
            else
            {
                if (evt.Metadata.LogicalStationNo != 0)
                {
                    if (!this._Adapters.ContainsKey(evt.Metadata.LogicalStationNo))
                    {
                        reason = string.Format("eventmap event={0} att=logicstno,att value={1} can't map to adapter", evt.Name, evt.Metadata.LogicalStationNo);
                        result = null;
                        return result;
                    }
                }
                else
                {
                    evt.Metadata.LogicalStationNo = this._DefaultAdapter.LogicalStationNo;
                    //brue.zhao 修改为默认的LogicalStationNo 20180113
                    //string[] LogicalStationNo = evt.Metadata.Name.Split('_');
                    //evt.Metadata.LogicalStationNo = int.Parse(LogicalStationNo[0].Substring(1));
                }
                PLCAdapter adapter = this._Adapters[evt.Metadata.LogicalStationNo];
                if (!adapter.Devices.ContainsKey(evt.Metadata.DeviceCode))
                {
                    reason = string.Format("eventmap event={0} att=devicecode,invalid att value={1}", evt.Name, evt.Metadata.DeviceCode);
                    result = null;
                }
                else
                {
                    DeviceSymbol device = adapter.Devices[evt.Metadata.DeviceCode];
                    int dno = 0;
                    try
                    {
                        dno = Convert.ToInt32(evt.Metadata.Address, device.nbase);
                        if (device.nbase == 16)
                        {
                            evt.Metadata.Address = dno.ToString("X7");
                            evt.Metadata.OriginalAddress = evt.Metadata.Address;
                            evt.Metadata.IsAddressHex = true;
                            evt.Metadata.IsOriginalAddressHex = evt.Metadata.IsAddressHex;
                        }
                        evt.Metadata.StartAddress10 = dno;
                        evt.Metadata.OriginalStartAddress10 = evt.Metadata.StartAddress10;
                        evt.Metadata.IsBitDeviceType = device.isbit;
                    }
                    catch
                    {
                        reason = string.Format("eventmap event={0} att=address,invalid att value={1}", evt.Name, evt.Metadata.Address);
                        result = null;
                        return result;
                    }
                    if (adapter.IsEnableW2ZR && evt.Metadata.DeviceCode == "W")
                    {
                        evt.Metadata.DeviceCode = "ZR";
                        evt.Metadata.Address = dno.ToString().PadLeft(7, '0');
                        evt.Metadata.IsAddressHex = false;
                    }
                    reason = string.Empty;
                    result = evt;
                }
            }
            return result;
        }

        private WatchData CreateWatchData(XElement elm, out string reason)
        {
            WatchData watch = new WatchData();
            foreach (XAttribute att in elm.Attributes())
            {
                string localName = att.Name.LocalName;
                if (localName != null)
                {
                    if (!(localName == "name"))
                    {
                        if (!(localName == "logicalstno"))
                        {
                            if (!(localName == "devicecode"))
                            {
                                if (!(localName == "address"))
                                {
                                    if (!(localName == "points"))
                                    {
                                        if (localName == "interval")
                                        {
                                            int tmp = 0;
                                            int.TryParse(att.Value, out tmp);
                                            watch.ScanIntervalMS = tmp;
                                        }
                                    }
                                    else
                                    {
                                        int tmp = 0;
                                        int.TryParse(att.Value, out tmp);
                                        watch.Points = tmp;
                                    }
                                }
                                else
                                {
                                    watch.Address = att.Value;
                                }
                            }
                            else
                            {
                                watch.DeviceCode = att.Value;
                            }
                        }
                        else
                        {
                            int tmp = 0;
                            int.TryParse(att.Value, out tmp);
                            watch.LogicalStationNo = tmp;
                        }
                    }
                    else
                    {
                        watch.Name = att.Value;
                    }
                }
            }
            WatchData result;
            if (string.IsNullOrEmpty(watch.Name))
            {
                reason = string.Format("datagathering scan exists tag=watchdata att=name,att is null or empty", new object[0]);
                result = null;
            }
            else if (this._Model.Scan.AllKeys.Contains(watch.Name))
            {
                reason = string.Format("datagathering scan watch={0},name duplicate", watch.Name);
                result = null;
            }
            else
            {
                if (watch.LogicalStationNo != 0)
                {
                    if (!this._Adapters.ContainsKey(watch.LogicalStationNo))
                    {
                        reason = string.Format("datagathering scan watch={0} att=logicstno,att value={1} can't map to adapter", watch.Name, watch.LogicalStationNo);
                        result = null;
                        return result;
                    }
                }
                else
                {
                    watch.LogicalStationNo = this._DefaultAdapter.LogicalStationNo;
                }
                if (watch.ScanIntervalMS == 0)
                {
                    watch.ScanIntervalMS = 100;
                }
                PLCAdapter adapter = this._Adapters[watch.LogicalStationNo];
                if (!adapter.Devices.ContainsKey(watch.DeviceCode))
                {
                    reason = string.Format("datagathering scan watch={0} att=devicecode,invalid att value={1}", watch.Name, watch.DeviceCode);
                    result = null;
                }
                else
                {
                    DeviceSymbol device = adapter.Devices[watch.DeviceCode];
                    int dno = 0;
                    try
                    {
                        dno = Convert.ToInt32(watch.Address, device.nbase);
                        if (device.nbase == 16)
                        {
                            watch.Address = dno.ToString("X7");
                            watch.IsAddressHex = true;
                        }
                        watch.StartAddress10 = dno;
                        watch.IsBitDeviceType = device.isbit;
                    }
                    catch
                    {
                        reason = string.Format("datagathering scan watch={0} att=address,invalid att value={1}", watch.Name, watch.Address);
                        result = null;
                        return result;
                    }
                    if (adapter.IsEnableW2ZR && watch.DeviceCode == "W")
                    {
                        watch.DeviceCode = "ZR";
                        watch.Address = dno.ToString().PadLeft(7, '0');
                        watch.IsAddressHex = false;
                    }
                    if (watch.IsBitDeviceType)
                    {
                        if (dno % 16 != 0)
                        {
                            reason = string.Format("datagathering scan watch={0} att=address,invalid att value={1}(not multiple of 16)", watch.Name, watch.Address);
                            result = null;
                            return result;
                        }
                        if (watch.Points % 16 != 0)
                        {
                            reason = string.Format("datagathering scan watch={0} att=points,invalid att value={1}(not multiple of 16)", watch.Name, watch.Points);
                            result = null;
                            return result;
                        }
                        int len = (watch.Points - 1) / 16 + 1;
                        watch.NewSnapShot = new short[len];
                        watch.TempSnapShot = new short[len];
                    }
                    else
                    {
                        int len = watch.Points;
                        watch.NewSnapShot = new short[len];
                        watch.TempSnapShot = new short[len];
                    }
                    string key = watch.LogicalStationNo + "_" + watch.DeviceCode;
                    PLCScanBuffer scanbuf;
                    if (this._IODatas.ContainsKey(key))
                    {
                        scanbuf = this._IODatas[key];
                    }
                    else
                    {
                        scanbuf = new PLCScanBuffer();
                        scanbuf.LogicalStationNo = watch.LogicalStationNo;
                        scanbuf.DeviceCode = watch.DeviceCode;
                        scanbuf.StartAddress10 = 0;
                        scanbuf.IsBitDeviceType = watch.IsBitDeviceType;
                        scanbuf.Source = new List<WatchData>();
                        if (scanbuf.IsBitDeviceType)
                        {
                            scanbuf.Points = PLCScanBuffer.MAX_BIT_POINTS;
                            scanbuf.NewSnapShot = new short[PLCScanBuffer.MAX_BIT_POINTS / 16];
                            scanbuf.OldSnapShot = new short[PLCScanBuffer.MAX_BIT_POINTS / 16];
                            scanbuf.ScanBuffer = new short[PLCScanBuffer.MAX_BIT_POINTS / 16];
                        }
                        else
                        {
                            scanbuf.Points = PLCScanBuffer.MAX_WORD_POINTS;
                            scanbuf.NewSnapShot = new short[PLCScanBuffer.MAX_WORD_POINTS];
                            scanbuf.OldSnapShot = new short[PLCScanBuffer.MAX_WORD_POINTS];
                            scanbuf.ScanBuffer = new short[PLCScanBuffer.MAX_WORD_POINTS];
                        }
                        this._IODatas.Add(key, scanbuf);
                    }
                    if (watch.IsBitDeviceType)
                    {
                        if (watch.StartAddress10 + watch.Points > PLCScanBuffer.MAX_BIT_POINTS)
                        {
                            reason = string.Format("datagathering scan watchdata={0} device={1} address={2} point={3} is beyond MAX_BIT_POINTS={4}", new object[]
                            {
                                watch.Name,
                                watch.DeviceCode,
                                watch.Address,
                                watch.Points,
                                PLCScanBuffer.MAX_BIT_POINTS
                            });
                            result = null;
                            return result;
                        }
                    }
                    else if (watch.StartAddress10 + watch.Points > PLCScanBuffer.MAX_WORD_POINTS)
                    {
                        reason = string.Format("datagathering scan watchdata={0} device={1} address={2} point={3} is beyond MAX_WORD_POINTS={4}", new object[]
                        {
                            watch.Name,
                            watch.DeviceCode,
                            watch.Address,
                            watch.Points,
                            PLCScanBuffer.MAX_WORD_POINTS
                        });
                        result = null;
                        return result;
                    }
                    watch.ScanBuffer = scanbuf.ScanBuffer;
                    scanbuf.Source.Add(watch);
                    scanbuf.LastRefreshDT = DateTime.Now;
                    reason = string.Empty;
                    result = watch;
                }
            }
            return result;
        }

        private Trx CreateTrx(XElement elm, string scope, out string reason)
        {
            Trx trx = new Trx();
            foreach (XAttribute att in elm.Attributes())
            {
                string text = att.Name.LocalName;
                if (text == null)
                {
                    goto IL_118;
                }
                if (!(text == "name"))
                {
                    if (!(text == "triggercondition"))
                    {
                        goto IL_118;
                    }
                    text = att.Value.ToUpper();
                    if (text == null)
                    {
                        goto IL_107;
                    }
                    if (!(text == "ON"))
                    {
                        if (!(text == "OFF"))
                        {
                            if (!(text == "CHANGE"))
                            {
                                goto IL_107;
                            }
                            trx.Metadata.TriggerCondition = TrxTriggerConditionEnum.CHANGE;
                        }
                        else
                        {
                            trx.Metadata.TriggerCondition = TrxTriggerConditionEnum.OFF;
                        }
                    }
                    else
                    {
                        trx.Metadata.TriggerCondition = TrxTriggerConditionEnum.ON;
                    }
                    continue;
                IL_107:
                    trx.Metadata.TriggerCondition = TrxTriggerConditionEnum.NONE;
                }
                else
                {
                    trx.Name = att.Value;
                    int index = trx.Name.IndexOf('_');
                    if (index >= 0)
                    {
                        trx.Metadata.NodeNo = trx.Name.Substring(0, index);
                    }
                }
                continue;
            IL_118:
                trx.UserAttributes.Add(att.Name.LocalName, att.Value);
            }
            Trx result;
            if (string.IsNullOrEmpty(trx.Name))
            {
                reason = string.Format("transaction {0} exists tag=trx att=name,att is null or empty", scope);
                result = null;
            }
            else
            {
                reason = string.Empty;
                result = trx;
            }
            return result;
        }

        private EventGroup CreateEventGroup(XElement elm, string scope, out string reason)
        {
            EventGroup evtgrp = new EventGroup();
            EventGroup result;
            foreach (XAttribute att in elm.Attributes())
            {
                string text = att.Name.LocalName;
                if (text != null)
                {
                    if (!(text == "name"))
                    {
                        if (text == "dir")
                        {
                            text = att.Value.ToUpper();
                            if (text != null)
                            {
                                if (!(text == "E2B"))
                                {
                                    if (!(text == "B2E"))
                                    {
                                        goto IL_C6;
                                    }
                                    evtgrp.Metadata.Dir = EventGroupDirEnum.B2E;
                                }
                                else
                                {
                                    evtgrp.Metadata.Dir = EventGroupDirEnum.E2B;
                                }
                                continue;
                            }
                        IL_C6:
                            reason = string.Format("transaction {0} eventgroup={1} att=dir,invalid att value={2}", scope, evtgrp.Name, att.Value);
                            result = null;
                            return result;
                        }
                        if (text == "logstyle")
                        {
                            text = att.Value.ToUpper();
                            if (text != null)
                            {
                                if (!(text == "HEAD"))
                                {
                                    if (!(text == "RAWDATA"))
                                    {
                                        if (!(text == "DETAIL"))
                                        {
                                            goto IL_143;
                                        }
                                        evtgrp.LogStyle = LogStyleEnum.DETAIL;
                                    }
                                    else
                                    {
                                        evtgrp.LogStyle = LogStyleEnum.RAWDATA;
                                    }
                                }
                                else
                                {
                                    evtgrp.LogStyle = LogStyleEnum.HEAD;
                                }
                                continue;
                            }
                        IL_143:
                            reason = string.Format("Log Style {0} eventgroup={1} att=dir,invalid att value={2}", scope, evtgrp.Name, att.Value);
                            result = null;
                            return result;
                        }
                    }
                    else
                    {
                        evtgrp.Name = att.Value;
                    }
                }
            }
            if (string.IsNullOrEmpty(evtgrp.Name))
            {
                reason = string.Format("transaction {0} exists tag=eventgroup att=name,att is null or empty", scope);
                result = null;
            }
            else
            {
                reason = string.Empty;
                result = evtgrp;
            }
            return result;
        }


        private TrxCatalog CreateTrxCatalog(XElement elm, out string reason)
        {
            TrxCatalog catalog = new TrxCatalog();
            TrxCatalog result;
            foreach (XAttribute att in elm.Attributes())
            {
                string text = att.Name.LocalName;
                if (text != null)
                {
                    if (!(text == "frtrxcatalog"))
                    {
                        if (!(text == "totrxcatalog"))
                        {
                            if (text == "primary")
                            {
                                text = att.Value.ToUpper();
                                if (text != null)
                                {
                                    if (!(text == "BCS"))
                                    {
                                        if (!(text == "EQP"))
                                        {
                                            goto IL_FE;
                                        }
                                        catalog.Primary = TrxCatalogPrimaryEnum.EQP;
                                    }
                                    else
                                    {
                                        catalog.Primary = TrxCatalogPrimaryEnum.BCS;
                                    }
                                    continue;
                                }
                            IL_FE:
                                reason = string.Format("pair trxcatalog={0} att=primary,invalid att value={1}", catalog.FromTrxCatalog, att.Value);
                                result = null;
                                return result;
                            }
                            if (text == "action")
                            {
                                text = att.Value.ToUpper();
                                if (text != null)
                                {
                                    if (_DataDictionary3 == null)
                                    {
                                        _DataDictionary3 = new Dictionary<string, int>(9)
                                        {
                                            {
                                                "PBW/SBW",
                                                0
                                            },
                                            {
                                                "PBW/SB",
                                                1
                                            },
                                            {
                                                "PBW",
                                                2
                                            },
                                            {
                                                "PB",
                                                3
                                            },
                                            {
                                                "PW",
                                                4
                                            },
                                            {
                                                "PB/SBW",
                                                5
                                            },
                                            {
                                                "PB/SB",
                                                6
                                            },
                                            {
                                                "PWI/SWI",
                                                7
                                            },
                                            {
                                                "PWI",
                                                8
                                            }
                                        };
                                    }
                                    int num;
                                    if (_DataDictionary3.TryGetValue(text, out num))
                                    {
                                        switch (num)
                                        {
                                            case 0:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PBW_SBW;
                                                break;
                                            case 1:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PBW_SB;
                                                break;
                                            case 2:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PBW;
                                                break;
                                            case 3:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PB;
                                                break;
                                            case 4:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PW;
                                                break;
                                            case 5:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PB_SBW;
                                                break;
                                            case 6:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PB_SB;
                                                break;
                                            case 7:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PWI_SWI;
                                                break;
                                            case 8:
                                                catalog.TrxCatalogAction = TrxCatalogActionEnum.PWI;
                                                break;
                                            default:
                                                goto IL_256;
                                        }
                                        continue;
                                    }
                                }
                            IL_256:
                                reason = string.Format("pair trxcatalog={0} att=action,invalid att value={1}", catalog.FromTrxCatalog, att.Value);
                                result = null;
                                return result;
                            }
                            if (!(text == "menucatalog"))
                            {
                                if (text == "chart")
                                {
                                    catalog.Chart = att.Value;
                                }
                            }
                            else
                            {
                                catalog.MenuCatalog = att.Value;
                            }
                        }
                        else
                        {
                            catalog.ToTrxCatalog = att.Value;
                        }
                    }
                    else
                    {
                        catalog.FromTrxCatalog = att.Value;
                    }
                }
            }
            reason = string.Empty;
            result = catalog;
            return result;
        }



        #region
        //private TrxCatalog CreateTrxCatalog(XElement elm, out string reason)
        //{
        //    TrxCatalog catalog = new TrxCatalog();
        //    reason = string.Empty;
        //    return catalog;

        //}
        //private TrxCatalog CreateTrxCatalog(XElement elm, out string reason)
        //{
        //    TrxCatalog catalog = new TrxCatalog();
        //    TrxCatalog result;
        //    foreach (XAttribute att in elm.Attributes())
        //    {
        //        string text = att.Name.LocalName;
        //        if (text != null)
        //        {
        //            if (!(text == "frtrxcatalog"))
        //            {
        //                if (!(text == "totrxcatalog"))
        //                {
        //                    if (text == "primary")
        //                    {
        //                        text = att.Value.ToUpper();
        //                        if (text != null)
        //                        {
        //                            if (!(text == "BCS"))
        //                            {
        //                                if (!(text == "EQP"))
        //                                {
        //                                    goto IL_FE;
        //                                }
        //                                catalog.Primary = TrxCatalogPrimaryEnum.EQP;
        //                            }
        //                            else
        //                            {
        //                                catalog.Primary = TrxCatalogPrimaryEnum.BCS;
        //                            }
        //                            continue;
        //                        }
        //                        IL_FE:
        //                        reason = string.Format("pair trxcatalog={0} att=primary,invalid att value={1}", catalog.FromTrxCatalog, att.Value);
        //                        result = null;
        //                        return result;
        //                    }
        //                    if (text == "action")
        //                    {
        //                        text = att.Value.ToUpper();
        //                        if (text != null)
        //                        {
        //                            if (<PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000cc-1 == null)
        //                            {
        //                                <PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000cc-1 = new Dictionary<string, int>(9)
        //                                {
        //                                    {
        //                                        "PBW/SBW",
        //                                        0
        //                                    },
        //                                    {
        //                                        "PBW/SB",
        //                                        1
        //                                    },
        //                                    {
        //                                        "PBW",
        //                                        2
        //                                    },
        //                                    {
        //                                        "PB",
        //                                        3
        //                                    },
        //                                    {
        //                                        "PW",
        //                                        4
        //                                    },
        //                                    {
        //                                        "PB/SBW",
        //                                        5
        //                                    },
        //                                    {
        //                                        "PB/SB",
        //                                        6
        //                                    },
        //                                    {
        //                                        "PWI/SWI",
        //                                        7
        //                                    },
        //                                    {
        //                                        "PWI",
        //                                        8
        //                                    }
        //                                };
        //                            }
        //                            int num;
        //                            if (<PrivateImplementationDetails>{4BD0E755-1504-44A3-B588-5E3A4FEEEC79}.$$method0x60000cc-1.TryGetValue(text, out num))
        //                            {
        //                                switch (num)
        //                                {
        //                                case 0:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PBW_SBW;
        //                                    break;
        //                                case 1:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PBW_SB;
        //                                    break;
        //                                case 2:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PBW;
        //                                    break;
        //                                case 3:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PB;
        //                                    break;
        //                                case 4:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PW;
        //                                    break;
        //                                case 5:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PB_SBW;
        //                                    break;
        //                                case 6:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PB_SB;
        //                                    break;
        //                                case 7:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PWI_SWI;
        //                                    break;
        //                                case 8:
        //                                    catalog.TrxCatalogAction = TrxCatalogActionEnum.PWI;
        //                                    break;
        //                                default:
        //                                    goto IL_256;
        //                                }
        //                                continue;
        //                            }
        //                        }
        //                        IL_256:
        //                        reason = string.Format("pair trxcatalog={0} att=action,invalid att value={1}", catalog.FromTrxCatalog, att.Value);
        //                        result = null;
        //                        return result;
        //                    }
        //                    if (!(text == "menucatalog"))
        //                    {
        //                        if (text == "chart")
        //                        {
        //                            catalog.Chart = att.Value;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        catalog.MenuCatalog = att.Value;
        //                    }
        //                }
        //                else
        //                {
        //                    catalog.ToTrxCatalog = att.Value;
        //                }
        //            }
        //            else
        //            {
        //                catalog.FromTrxCatalog = att.Value;
        //            }
        //        }
        //    }
        //    reason = string.Empty;
        //    result = catalog;
        //    return result;
        //}

        #endregion



        private Mapping CreateMapping(XElement elm, out string reason)
        {
            Mapping mapping = new Mapping();
            foreach (XAttribute att in elm.Attributes())
            {
                string localName = att.Name.LocalName;
                if (localName != null)
                {
                    if (!(localName == "fritem"))
                    {
                        if (localName == "toitem")
                        {
                            mapping.ToItem = att.Value;
                        }
                    }
                    else
                    {
                        mapping.FromItem = att.Value;
                    }
                }
            }
            reason = string.Empty;
            return mapping;
        }

        private ItemDefine CreateItemDefine(XElement elm, out string reason)
        {
            ItemDefine define = new ItemDefine();
            foreach (XAttribute att in elm.Attributes())
            {
                string localName = att.Name.LocalName;
                if (localName != null)
                {
                    if (localName == "name")
                    {
                        define.Name = att.Value;
                    }
                }
            }
            ItemDefine result;
            if (string.IsNullOrEmpty(define.Name))
            {
                reason = string.Format("itemdeinecollection exists tag=itemdefine att=name,att is null or empty", new object[0]);
                result = null;
            }
            else
            {
                reason = string.Empty;
                result = define;
            }
            return result;
        }

        private Id CreateId(XElement elm, out string reason)
        {
            Id id = new Id();
            foreach (XAttribute att in elm.Attributes())
            {
                string localName = att.Name.LocalName;
                if (localName != null)
                {
                    if (!(localName == "name"))
                    {
                        if (localName == "value")
                        {
                            id.Value = att.Value;
                        }
                    }
                    else
                    {
                        id.Name = att.Value;
                    }
                }
            }
            Id result;
            if (string.IsNullOrEmpty(id.Name))
            {
                reason = string.Format("itemdeinecollection itemdefine exists tag=id att=name,att is null or empty", new object[0]);
                result = null;
            }
            else
            {
                reason = string.Empty;
                result = id;
            }
            return result;
        }

        private void FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel level, string method, int code, string msg, string trackkey, int station)
        {
            try
            {
                if (!this.DebugOutEnable)
                {
                    if (level == PLCDriverDebugOutLevel.TRACE || level == PLCDriverDebugOutLevel.DEBUG)
                    {
                        return;
                    }
                }
                PLCDriverDebugOutEventHandler tmp = Interlocked.CompareExchange<PLCDriverDebugOutEventHandler>(ref this.PLCDriverDebugOutEvent, null, null);
                if (tmp != null)
                {
                    tmp(this, new PLCDriverDebugOutEventArgs(level, method, code, msg, trackkey, station));
                }
            }
            catch
            {
            }
        }

        private void FirePLCConnectedEvent(int station, bool isdefaultAdapter)
        {
            try
            {
                PLCConnectedEventHandler tmp = Interlocked.CompareExchange<PLCConnectedEventHandler>(ref this.PLCConnectedEvent, null, null);
                if (tmp != null)
                {
                    tmp(this, new PLCConnectedEventArgs(station, isdefaultAdapter));
                }
            }
            catch
            {
            }
        }

        private void FirePLCDisconnectedEvent(int station, bool isdefaultAdapter)
        {
            try
            {
                PLCDisconnectedEventHandler tmp = Interlocked.CompareExchange<PLCDisconnectedEventHandler>(ref this.PLCDisconnectedEvent, null, null);
                if (tmp != null)
                {
                    tmp(this, new PLCDisconnectedEventArgs(station, isdefaultAdapter));
                }
            }
            catch
            {
            }
        }

        private void FireTrxTriggeredEvent(Trx trx)
        {
            try
            {
                TrxTriggeredEventHandler tmp = Interlocked.CompareExchange<TrxTriggeredEventHandler>(ref this.TrxTriggeredEvent, null, null);
                if (tmp != null)
                {
                    tmp(this, new TrxTriggeredEventArgs(trx));
                }
            }
            catch
            {
            }
        }

        private void FireReadTrxResultEvent(Trx trx, int station, bool hasErr, int code, string msg)
        {
            try
            {
                ReadTrxResultEventHandler tmp = Interlocked.CompareExchange<ReadTrxResultEventHandler>(ref this.ReadTrxResultEvent, null, null);
                if (tmp != null)
                {
                    tmp(this, new ReadTrxResultEventArgs(trx, station, hasErr, code, msg));
                }
            }
            catch
            {
            }
        }

        private void FireWriteTrxResultEvent(Trx trx, int station, bool hasErr, int code, string msg)
        {
            try
            {
                WriteTrxResultEventHandler tmp = Interlocked.CompareExchange<WriteTrxResultEventHandler>(ref this.WriteTrxResultEvent, null, null);
                if (tmp != null)
                {
                    tmp(this, new WriteTrxResultEventArgs(trx, station, hasErr, code, msg));
                }
            }
            catch (Exception ex)
            {
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.EXCEPTION, "FireWriteTrxResultEvent", 20904, ex.ToString(), string.Empty, 0);
            }
        }

        private void adapter_PLCAdapterDebugOutEvent(object sender, PLCAdapterDebugOutEventArgs e)
        {
            if (this.DebugOutEnable)
            {
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "PLCAdapter." + e.MethodName, e.Code, e.Message, e.TrackKey, e.SourceStationNo);
            }
        }

        private void TriggerProc()
        {
            int cc = 0;
            while (true)
            {
                try
                {
                    while (true)
                    {
                    IL_0A:
                        this._TriggerTaskState = string.Concat(new object[]
                        {
                            this._TriggerTask.ManagedThreadId,
                            ",",
                            this._TriggerTask.ThreadState.ToString(),
                            ",",
                            DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff")
                        });
                        cc++;
                        if (cc > this._TriggerTaskWaitFrequency)
                        {
                            cc = 0;
                            Thread.Sleep(this._TriggerTaskWaitPeriodMS);
                        }
                        if (this._IsDestroy)
                        {
                            break;
                        }
                        lock (this._SyncLock)
                        {
                            foreach (PLCScanBuffer v in this._IODatas.Values)
                            {
                                v.Refresh();
                            }
                        }
                        for (int i = 0; i < this._TriggerMaxWorkIterateTimes; i++)
                        {
                            if (!this.IsStarted)
                            {
                                break;
                            }
                            bool empty = this.DoTrxRead();
                            if (empty)
                            {
                                break;
                            }
                            if (this._IsDestroy)
                            {
                                goto Block_7;
                            }
                        }
                        if (!this.IsStarted)
                        {
                            goto Block_8;
                        }
                        foreach (PLCScanBuffer buf in this._IODatas.Values)
                        {
                            foreach (WatchData watch in buf.Source)
                            {
                                if (!watch.IsDataInited)
                                {
                                    goto IL_0A;
                                }
                            }
                        }
                        goto IL_212;
                    }
                Block_7:
                    break;
                Block_8:
                    continue;
                IL_212:
                    DateTime now = DateTime.Now;
                    bool enable = false;
                    foreach (PLCAdapter v2 in this._Adapters.Values)
                    {
                        if (v2.IsTriggerEnable)
                        {
                            enable = true;
                            break;
                        }
                    }
                    if (enable)
                    {
                        if (this.EnableScanBufferDump)
                        {
                            foreach (PLCScanBuffer v in this._IODatas.Values)
                            {
                                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DUMP, "TriggerProc", 0, v.NewSnapShotHexDump(), string.Empty, 0);
                            }
                        }
                        this.DoTrxTrigger();
                    }
                    this._TrxTriggerStartDT = now;
                    this._TrxTriggerSpentTime = DateTime.Now.Subtract(this._TrxTriggerStartDT);
                    if (this._TrxTriggerSpentTime > this._MaxTrxTriggerSpentTime)
                    {
                        this._MaxTrxTriggerSpentTime = this._TrxTriggerSpentTime;
                    }
                    if (this._TrxTriggerCount > this._MaxTrxTriggerCount)
                    {
                        this._MaxTrxTriggerCount = this._TrxTriggerCount;
                    }
                    this._AvgTrxTriggerCount = (this._AvgTrxTriggerCount + (double)this._TrxTriggerCount) / 2.0;
                    if (this._IsDestroy)
                    {
                        break;
                    }
                }
                catch
                {
                }
            }
        }

        private bool DoTrxRead()
        {
            Trx trx;
            bool exists = this._TrxReadQueue.TryDequeue(out trx);
            bool result2;
            if (!exists || trx == null)
            {
                result2 = true;
            }
            else if ((ushort)(trx.TrxFlags & InternalFlagsEnum.IsDirectRead) == 16)
            {
                this.DoDirectRead(trx);
                result2 = false;
            }
            else
            {
                PLCOpResult result = this.DecodeTrx(trx, true);
                if (result.RetCode != 0)
                {
                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxRead", result.RetCode, string.Format("fail to decode,readtrx={0},err={1}", trx.Name, result.RetErrMsg), trx.TrackKey, 0);
                }
                trx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                this.FireReadTrxResultEvent(trx, 0, result.RetCode != 0, result.RetCode, result.RetErrMsg);
                this._TotalRead++;
                result2 = false;
            }
            return result2;
        }

        private void DoDirectRead(Trx trx)
        {
            PLCOpResult result = new PLCOpResult();
            trx.ReadRequestStations = new ConcurrentDictionary<int, object>();
            trx.ReadCompleteStations = new ConcurrentDictionary<int, object>();
            trx.ReadCompleteSync = new object();
            Dictionary<int, PLCOpRequest> requests = new Dictionary<int, PLCOpRequest>();
            object[] allValues = trx.EventGroups.AllValues;
            for (int i = 0; i < allValues.Length; i++)
            {
                EventGroup evtgrp = (EventGroup)allValues[i];
                if (!evtgrp.IsDisable)
                {
                    if (evtgrp.IsMergeEvent)
                    {
                        if (evtgrp.Events.Count != 0)
                        {
                            Event evt0 = evtgrp.Events[0];
                            int startAddress10 = evt0.Metadata.StartAddress10;
                            int totalPoints = 0;
                            string reason = string.Empty;
                            object[] allValues2 = evtgrp.Events.AllValues;
                            for (int j = 0; j < allValues2.Length; j++)
                            {
                                Event evt = (Event)allValues2[j];
                                if (evt.Metadata.IsBitDeviceType)
                                {
                                    reason = string.Format("fail to merge event,directread trx={0},reqsno={1},eventgroup={2},event={3},err=can't merge bit device event", new object[]
                                    {
                                        trx.Name,
                                        trx.ReqSNo,
                                        evtgrp.Name,
                                        evt.Name
                                    });
                                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoDirectRead", 20202, reason, trx.TrackKey, 0);
                                    this.FireReadTrxResultEvent(trx, 0, true, 20202, reason);
                                    return;
                                }
                                if (evt.Metadata.DeviceCode != evt0.Metadata.DeviceCode)
                                {
                                    reason = string.Format("fail to merge event,directread trx={0},reqsno={1},eventgroup={2},event={3},err=can't merge different device code", new object[]
                                    {
                                        trx.Name,
                                        trx.ReqSNo,
                                        evtgrp.Name,
                                        evt.Name
                                    });
                                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoDirectRead", 20202, reason, trx.TrackKey, 0);
                                    this.FireReadTrxResultEvent(trx, 0, true, 20202, reason);
                                    return;
                                }
                                if (evt.Metadata.LogicalStationNo != evt0.Metadata.LogicalStationNo)
                                {
                                    reason = string.Format("fail to merge event,directread trx={0},reqsno={1},eventgroup={2},event={3},err=can't merge different logicalstno", new object[]
                                    {
                                        trx.Name,
                                        trx.ReqSNo,
                                        evtgrp.Name,
                                        evt.Name
                                    });
                                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoDirectRead", 20202, reason, trx.TrackKey, 0);
                                    this.FireReadTrxResultEvent(trx, 0, true, 20202, reason);
                                    return;
                                }
                                if (evt.Metadata.StartAddress10 != startAddress10)
                                {
                                    reason = string.Format("fail to merge event,directread trx={0},reqsno={1},eventgroup={2},event={3},err=can't merge discontinous device address", new object[]
                                    {
                                        trx.Name,
                                        trx.ReqSNo,
                                        evtgrp.Name,
                                        evt.Name
                                    });
                                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoDirectRead", 20202, reason, trx.TrackKey, 0);
                                    this.FireReadTrxResultEvent(trx, 0, true, 20202, reason);
                                    return;
                                }
                                startAddress10 += evt.Metadata.Points;
                                totalPoints += evt.Metadata.Points;
                                evt.RawData = new short[evt.Metadata.Points];
                            }
                            evtgrp.MergeBuffer = new short[totalPoints];
                            PLCOpRequest req = null;
                            if (requests.ContainsKey(evt0.Metadata.LogicalStationNo))
                            {
                                req = requests[evt0.Metadata.LogicalStationNo];
                            }
                            if (req == null)
                            {
                                req = new PLCOpRequest();
                                req.ReqSNo = trx.ReqSNo;
                                req.LogicalStationNo = evt0.Metadata.LogicalStationNo;
                                req.Tag = trx;
                                requests.Add(evt0.Metadata.LogicalStationNo, req);
                            }
                            PLCBlockOp op = new PLCBlockOp();
                            op.OpDelayTimeMS = 0;
                            op.EventKey = evtgrp.Name;
                            op.DevType = evt0.Metadata.DeviceCode;
                            op.DevNo = evt0.Metadata.Address;
                            op.Points = totalPoints;
                            op.Buf = evtgrp.MergeBuffer;
                            req.BlockOps.Add(op);
                        }
                    }
                    else
                    {
                        object[] allValues2 = evtgrp.Events.AllValues;
                        for (int j = 0; j < allValues2.Length; j++)
                        {
                            Event evt = (Event)allValues2[j];
                            if (!evt.IsDisable)
                            {
                                if (evt.Metadata.IsBitDeviceType)
                                {
                                    evt.RawData = new short[(evt.Metadata.Points - 1) / 16 + 1];
                                }
                                else
                                {
                                    evt.RawData = new short[evt.Metadata.Points];
                                }
                                PLCOpRequest req = null;
                                if (requests.ContainsKey(evt.Metadata.LogicalStationNo))
                                {
                                    req = requests[evt.Metadata.LogicalStationNo];
                                }
                                if (req == null)
                                {
                                    req = new PLCOpRequest();
                                    req.ReqSNo = trx.ReqSNo;
                                    req.LogicalStationNo = evt.Metadata.LogicalStationNo;
                                    req.Tag = trx;
                                    requests.Add(evt.Metadata.LogicalStationNo, req);
                                }
                                if (evt.Metadata.IsBitDeviceType)
                                {
                                    PLCRandOp op2 = new PLCRandOp();
                                    op2.OpDelayTimeMS = 0;
                                    op2.EventKey = evt.Name;
                                    op2.Blocks = new List<PLCRandOp.RandBlock>();
                                    PLCRandOp.RandBlock b = new PLCRandOp.RandBlock();
                                    b.DevType = evt.Metadata.DeviceCode;
                                    b.DevNo = evt.Metadata.Address;
                                    b.Points = evt.Metadata.Points;
                                    b.Buf = evt.RawData;
                                    op2.Blocks.Add(b);
                                    req.RandOps.Add(op2);
                                }
                                else
                                {
                                    PLCBlockOp op = new PLCBlockOp();
                                    op.OpDelayTimeMS = 0;
                                    op.EventKey = evt.Name;
                                    op.DevType = evt.Metadata.DeviceCode;
                                    op.DevNo = evt.Metadata.Address;
                                    op.Points = evt.Metadata.Points;
                                    op.Buf = evt.RawData;
                                    req.BlockOps.Add(op);
                                }
                            }
                        }
                    }
                }
            }
            foreach (PLCOpRequest v in requests.Values)
            {
                PLCAdapter adapter = this._Adapters[v.LogicalStationNo];
                adapter.BeginRead(v, new AsyncCallback(this.DirectReadCompelete));
                trx.ReadRequestStations.TryAdd(v.LogicalStationNo, v);
            }
        }

        private void DoTrxTrigger()
        {
            int tc = 0;
            int total = 0;
            int loop = 0;
            IEnumerable<Trx> queryReceiveTrx = from Trx v in this._Model.Transaction.AllValues
                                               where v.Metadata.TrxType == TrxTypeEnum.Receive
                                               select v;
            foreach (Trx trx in queryReceiveTrx)
            {
                if (!this.IsStarted)
                {
                    break;
                }
                bool triggertrx = false;
                bool inittriggertrx = false;
                object[] allValues = trx.EventGroups.AllValues;
                for (int i = 0; i < allValues.Length; i++)
                {
                    EventGroup evtgrp = (EventGroup)allValues[i];
                    object[] allValues2 = evtgrp.TriggerEventNames.AllValues;
                    for (int j = 0; j < allValues2.Length; j++)
                    {
                        string evtname = (string)allValues2[j];

                        Event evt = evtgrp.Events.Get(evtname);
                        if (evt != null)
                        {
                            PLCAdapter adapter = this._Adapters[evt.Metadata.LogicalStationNo];

                            // PLCAdapter adapter = this._Adapters[int.Parse(evt.Name.Substring(1,1))];
                            if (adapter.IsTriggerEnable)
                            {
                                if (trx.Metadata.TriggerCondition != TrxTriggerConditionEnum.NONE)
                                {
                                    string key = evt.Metadata.LogicalStationNo + "_" + evt.Metadata.DeviceCode;
                                    // string key = evt.Name.Substring(1, 1) + "_" + evt.Metadata.DeviceCode;
                                    if (this._IODatas.ContainsKey(key))
                                    {
                                        short[] newData = this._IODatas[key].NewSnapShot;
                                        short[] oldData = this._IODatas[key].OldSnapShot;
                                        object[] allValues3 = evt.Items.AllValues;
                                        for (int k = 0; k < allValues3.Length; k++)
                                        {
                                            Item item = (Item)allValues3[k];
                                            string newValue;
                                            string oldValue;
                                            try
                                            {
                                                if (evtname.Contains("EQDPositionGlassChangeReport#01"))
                                                {


                                                }

                                                newValue = this.GetItemValue(evt, item, newData);
                                                oldValue = this.GetItemValue(evt, item, oldData);
                                            }
                                            catch (PLCDriverException ex)
                                            {
                                                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.EXCEPTION, "DoTrxTrigger", 20403, ex.Message, string.Empty, 0);
                                                goto IL_39F;
                                            }
                                            switch (trx.Metadata.TriggerCondition)
                                            {
                                                case TrxTriggerConditionEnum.OFF:
                                                    if (Interlocked.Exchange(ref trx.Metadata.InitTriggerEnd, 1) == 0)
                                                    {
                                                        if (newValue == "0")
                                                        {
                                                            inittriggertrx = true;
                                                            triggertrx = true;
                                                            goto IL_39F;
                                                        }
                                                    }
                                                    else if (oldValue == "1" && newValue == "0")
                                                    {
                                                        triggertrx = true;
                                                        goto IL_39F;
                                                    }
                                                    break;
                                                case TrxTriggerConditionEnum.ON:
                                                    if (Interlocked.Exchange(ref trx.Metadata.InitTriggerEnd, 1) == 0)
                                                    {
                                                        if (newValue == "1")
                                                        {
                                                            inittriggertrx = true;
                                                            triggertrx = true;
                                                            goto IL_39F;
                                                        }
                                                    }
                                                    else if (oldValue == "0" && newValue == "1")
                                                    {
                                                        triggertrx = true;
                                                        goto IL_39F;
                                                    }
                                                    break;
                                                case TrxTriggerConditionEnum.CHANGE:
                                                    if (Interlocked.Exchange(ref trx.Metadata.InitTriggerEnd, 1) == 0)
                                                    {
                                                        inittriggertrx = true;
                                                        triggertrx = true;
                                                        goto IL_39F;
                                                    }
                                                    if (oldValue != newValue)
                                                    {
                                                        triggertrx = true;
                                                        goto IL_39F;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            IL_39F:
                if (!triggertrx)
                {
                    total++;
                    if (total > this._TriggerMaxWorkIterateTimes * 100)
                    {
                        total = 0;
                        Thread.Sleep(this._TriggerTaskWaitPeriodMS);
                        if (this._IsDestroy)
                        {
                            this._TrxTriggerCount = tc;
                            return;
                        }
                    }
                }
                else
                {
                    loop++;
                    tc++;
                    Trx newTrx = (Trx)trx.Clone();
                    newTrx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newTrx.ReqSNo = this.NewRecvReqSno;
                    Trx expr_434 = newTrx;
                    expr_434.TrxFlags |= InternalFlagsEnum.IsTrxTrigger;
                    PLCOpResult result = this.DecodeTrx(newTrx, true);
                    if (result.RetCode == 0)
                    {
                        if (inittriggertrx)
                        {
                            newTrx.IsInitTrigger = true;
                        }
                        this.FireTrxTriggeredEvent(newTrx);
                    }
                    else
                    {
                        this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxTrigger", result.RetCode, result.RetErrMsg, string.Empty, 0);
                    }
                    if (loop > this._TriggerMaxWorkIterateTimes * 10)
                    {
                        loop = 0;
                        Thread.Sleep(this._TriggerTaskWaitPeriodMS);
                        if (this._IsDestroy)
                        {
                            this._TrxTriggerCount = tc;
                            return;
                        }
                    }
                }
            }
            this._TrxTriggerCount = tc;
        }

        private PLCOpResult DecodeTrx(Trx trx, bool snapshot = true)
        {
            PLCOpResult result = new PLCOpResult();
            PLCOpResult result2;
            try
            {
                object[] allValues = trx.EventGroups.AllValues;
                for (int i = 0; i < allValues.Length; i++)
                {
                    EventGroup evtgrp = (EventGroup)allValues[i];
                    if (!evtgrp.IsDisable)
                    {
                        object[] allValues2 = evtgrp.Events.AllValues;
                        for (int j = 0; j < allValues2.Length; j++)
                        {
                            Event evt = (Event)allValues2[j];
                            if (!evt.IsDisable)
                            {
                                string key = evt.Metadata.LogicalStationNo + "_" + evt.Metadata.DeviceCode;
                                if (!this._IODatas.ContainsKey(key))
                                {
                                    result.Set(20301, string.Format("trx={0},evtgrp={1},evt={2},fail to get iodata by key={3}", new object[]
                                    {
                                        trx.Name,
                                        evtgrp.Name,
                                        evt.Name,
                                        key
                                    }));
                                    result2 = result;
                                    return result2;
                                }
                                short[] newData;
                                if (snapshot)
                                {
                                    newData = this._IODatas[key].NewSnapShot;
                                }
                                else
                                {
                                    newData = this._IODatas[key].ScanBuffer;
                                }
                                if (!this._IODatas[key].IsBitDeviceType)
                                {
                                    evt.RawData = new short[evt.Metadata.Points];
                                    Buffer.BlockCopy(newData, evt.Metadata.StartAddress10 * 2, evt.RawData, 0, evt.Metadata.Points * 2);
                                }
                                if (!evt.Metadata.SkipDecode)
                                {
                                    object[] allValues3 = evt.Items.AllValues;
                                    for (int k = 0; k < allValues3.Length; k++)
                                    {
                                        Item item = (Item)allValues3[k];
                                        try
                                        {
                                            item.Value = this.GetItemValue(evt, item, newData);
                                        }
                                        catch (PLCDriverException ex)
                                        {
                                            result.Set(ex.Code, string.Format("trx={0},evtgrp={1},evt={2},fail to get item value,{3}", new object[]
                                            {
                                                trx.Name,
                                                evtgrp.Name,
                                                evt.Name,
                                                ex.Message
                                            }));
                                            result2 = result;
                                            return result2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                result.Set(20302, ex2.ToString());
            }
            result2 = result;
            return result2;
        }

        private PLCOpResult DecodeDirectReadTrx(Trx trx)
        {
            PLCOpResult result = new PLCOpResult();
            PLCOpResult result2;
            try
            {
                object[] allValues = trx.EventGroups.AllValues;
                for (int i = 0; i < allValues.Length; i++)
                {
                    EventGroup evtgrp = (EventGroup)allValues[i];
                    if (!evtgrp.IsDisable)
                    {
                        int offset = 0;
                        object[] allValues2;
                        if (evtgrp.IsMergeEvent)
                        {
                            allValues2 = evtgrp.Events.AllValues;
                            for (int j = 0; j < allValues2.Length; j++)
                            {
                                Event evt = (Event)allValues2[j];
                                Buffer.BlockCopy(evtgrp.MergeBuffer, offset * 2, evt.RawData, 0, evt.Metadata.Points * 2);
                                offset += evt.Metadata.Points;
                            }
                        }
                        allValues2 = evtgrp.Events.AllValues;
                        for (int j = 0; j < allValues2.Length; j++)
                        {
                            Event evt = (Event)allValues2[j];
                            if (!evt.IsDisable)
                            {
                                if (!evt.Metadata.SkipDecode)
                                {
                                    object[] allValues3 = evt.Items.AllValues;
                                    for (int k = 0; k < allValues3.Length; k++)
                                    {
                                        Item item = (Item)allValues3[k];
                                        try
                                        {
                                            item.Value = this.GetItemValue0(evt, item, evt.RawData);
                                        }
                                        catch (PLCDriverException ex)
                                        {
                                            result.Set(ex.Code, string.Format("trx={0},evtgrp={1},evt={2},fail to get item value,{3}", new object[]
                                            {
                                                trx.Name,
                                                evtgrp.Name,
                                                evt.Name,
                                                ex.Message
                                            }));
                                            result2 = result;
                                            return result2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                result.Set(20303, ex2.ToString());
            }
            result2 = result;
            return result2;
        }

        private string GetItemValue(Event evt, Item item, short[] data)
        {
            string value = string.Empty;
            try
            {
                ItemExpressionEnum expression = item.Metadata.Expression;
                if (expression <= ItemExpressionEnum.ASCII)
                {
                    if (expression <= ItemExpressionEnum.SLONG)
                    {
                        switch (expression)
                        {
                            case ItemExpressionEnum.INT:
                                value = ExpressionINT.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString();
                                break;
                            case ItemExpressionEnum.SINT:
                                value = ExpressionSINT.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString();
                                break;
                            case ItemExpressionEnum.INT | ItemExpressionEnum.SINT:
                                break;
                            case ItemExpressionEnum.LONG:
                                value = ExpressionLONG.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString();
                                break;
                            default:
                                if (expression == ItemExpressionEnum.SLONG)
                                {
                                    value = ExpressionSLONG.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString();
                                }
                                break;
                        }
                    }
                    else if (expression != ItemExpressionEnum.EXP)
                    {
                        if (expression == ItemExpressionEnum.ASCII)
                        {
                            value = ExpressionASCII.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                        }
                    }
                    else
                    {
                        value = ExpressionEXP.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, data).ToString();
                    }
                }
                else if (expression <= ItemExpressionEnum.BIT)
                {
                    if (expression != ItemExpressionEnum.HEX)
                    {
                        if (expression == ItemExpressionEnum.BIT)
                        {
                            value = ExpressionBIT.Decode(evt.Metadata.StartAddress10, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                        }
                    }
                    else
                    {
                        value = ExpressionHEX.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                    }
                }
                else if (expression != ItemExpressionEnum.BIN)
                {
                    if (expression == ItemExpressionEnum.BCD)
                    {
                        value = ExpressionBCD.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data, this._FlipBCDValue);
                    }
                    if (expression == ItemExpressionEnum.FLOAT)
                    {
                        value = ExpressionFLOAT.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString("R");
                    }
                    if (expression == ItemExpressionEnum.DOUBLE)
                    {
                        value = ExpressionDOUBLE.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString("R");
                    }
                }
                else
                {
                    value = ExpressionBIN.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                }
            }
            catch (Exception ex)
            {
                throw new PLCDriverException(20401, string.Format("startadd={0},woffset={1},wpoints={2},boffset={3},bpoints={4},datalen={5},err={6}", new object[]
                {
                    evt.Metadata.StartAddress10,
                    item.Metadata.WordOffset,
                    item.Metadata.WordPoints,
                    item.Metadata.BitOffset,
                    item.Metadata.BitPoints,
                    (data == null) ? 0 : data.Length,
                    ex.ToString()
                }));
            }
            return value;
        }

        private string GetItemValue0(Event evt, Item item, short[] data)
        {
            string value = string.Empty;
            try
            {
                ItemExpressionEnum expression = item.Metadata.Expression;
                if (expression <= ItemExpressionEnum.ASCII)
                {
                    if (expression <= ItemExpressionEnum.SLONG)
                    {
                        switch (expression)
                        {
                            case ItemExpressionEnum.INT:
                                value = ExpressionINT.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString();
                                break;
                            case ItemExpressionEnum.SINT:
                                value = ExpressionSINT.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString();
                                break;
                            case ItemExpressionEnum.INT | ItemExpressionEnum.SINT:
                                break;
                            case ItemExpressionEnum.LONG:
                                value = ExpressionLONG.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString();
                                break;
                            default:
                                if (expression == ItemExpressionEnum.SLONG)
                                {
                                    value = ExpressionSLONG.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString();
                                }
                                break;
                        }
                    }
                    else if (expression != ItemExpressionEnum.EXP)
                    {
                        if (expression == ItemExpressionEnum.ASCII)
                        {
                            value = ExpressionASCII.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                        }
                    }
                    else
                    {
                        value = ExpressionEXP.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, data).ToString();
                    }
                }
                else if (expression <= ItemExpressionEnum.BIT)
                {
                    if (expression != ItemExpressionEnum.HEX)
                    {
                        if (expression == ItemExpressionEnum.BIT)
                        {
                            value = ExpressionBIT.Decode(0, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                        }
                    }
                    else
                    {
                        value = ExpressionHEX.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                    }
                }
                else if (expression != ItemExpressionEnum.BIN)
                {
                    if (expression == ItemExpressionEnum.BCD)
                    {
                        value = ExpressionBCD.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data, this._FlipBCDValue);
                    }
                    if (expression == ItemExpressionEnum.FLOAT)
                    {
                        value = ExpressionFLOAT.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString("R");
                    }
                    if (expression == ItemExpressionEnum.DOUBLE)
                    {
                        value = ExpressionDOUBLE.Decode(evt.Metadata.StartAddress10, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data).ToString("R");
                    }
                }
                else
                {
                    value = ExpressionBIN.Decode(0, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                }
            }
            catch (Exception ex)
            {
                throw new PLCDriverException(20402, string.Format("startadd={0},woffset={1},wpoints={2},boffset={3},bpoints={4},datalen={5},err={6}", new object[]
                {
                    0,
                    item.Metadata.WordOffset,
                    item.Metadata.WordPoints,
                    item.Metadata.BitOffset,
                    item.Metadata.BitPoints,
                    (data == null) ? 0 : data.Length,
                    ex.ToString()
                }));
            }
            return value;
        }

        private void DirectReadCompelete(IAsyncResult ar)
        {
            try
            {
                PLCOpRequest req = (PLCOpRequest)ar.AsyncState;
                if (req != null)
                {
                    Trx trx = req.Tag as Trx;
                    if (trx != null)
                    {
                        PLCAdapter adapter = this._Adapters[req.LogicalStationNo];
                        PLCOpResult result = adapter.EndRead(ar);
                        bool allcomp = false;
                        lock (trx.ReadCompleteSync)
                        {
                            object tmp;
                            trx.ReadRequestStations.TryRemove(req.LogicalStationNo, out tmp);
                            PLCOpRequestResult rr = new PLCOpRequestResult();
                            rr.request = req;
                            rr.result = result;
                            trx.ReadCompleteStations.TryAdd(req.LogicalStationNo, rr);
                            if (trx.ReadRequestStations.Count == 0)
                            {
                                allcomp = true;
                            }
                        }
                        if (allcomp)
                        {
                            foreach (PLCOpRequestResult v in trx.ReadCompleteStations.Values.Cast<PLCOpRequestResult>())
                            {
                                if (v.result.RetCode != 0)
                                {
                                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DirectReadCompelete", result.RetCode, string.Format("fail to do directread,trx={0},reqsno={1},err={2}", trx.Name, trx.ReqSNo, result.RetErrMsg), trx.TrackKey, v.request.LogicalStationNo);
                                    this.FireReadTrxResultEvent(trx, v.request.LogicalStationNo, true, v.result.RetCode, v.result.RetErrMsg);
                                    this._TotalRead++;
                                    return;
                                }
                            }
                            result = this.DecodeDirectReadTrx(trx);
                            if (result.RetCode != 0)
                            {
                                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DirectReadCompelete", result.RetCode, string.Format("fail to decode directread,trx={0},err={1}", trx.Name, result.RetErrMsg), trx.TrackKey, 0);
                            }
                            trx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            this.FireReadTrxResultEvent(trx, 0, result.RetCode != 0, result.RetCode, result.RetErrMsg);
                            this._TotalRead++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.EXCEPTION, "DirectReadCompelete", 20801, ex.ToString(), string.Empty, 0);
            }
        }

        private void ScanProc()
        {
            int cc = 0;
            while (true)
            {
                try
                {
                    this._ScanTaskState = string.Concat(new object[]
                    {
                        this._ScanTask.ManagedThreadId,
                        ",",
                        this._ScanTask.ThreadState.ToString(),
                        ",",
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff")
                    });
                    foreach (PLCAdapter adapter in this._Adapters.Values)
                    {
                        //PLCAdapter adapter;
                        if (adapter.IsConnected && !this._ConnectState[adapter.LogicalStationNo])
                        {
                            this._ConnectState[adapter.LogicalStationNo] = adapter.IsConnected;
                            this.FirePLCConnectedEvent(adapter.LogicalStationNo, adapter.IsDefaultAdapter);
                        }
                        else if (!adapter.IsConnected && this._ConnectState[adapter.LogicalStationNo])
                        {
                            this._ConnectState[adapter.LogicalStationNo] = adapter.IsConnected;
                            this.FirePLCDisconnectedEvent(adapter.LogicalStationNo, adapter.IsDefaultAdapter);
                        }
                    }
                    cc++;
                    if (cc > this._ScanTaskWaitFrequency)
                    {
                        cc = 0;
                        Thread.Sleep(this._ScanTaskWaitPeriodMS);
                    }
                    if (this._IsDestroy)
                    {
                        break;
                    }
                    DateTime now = DateTime.Now;
                    int rcnt = 0;
                    object[] allValues = this._Model.Scan.AllValues;
                    for (int i = 0; i < allValues.Length; i++)
                    {
                        WatchData watch = (WatchData)allValues[i];
                        if (!this.IsStarted)
                        {
                            break;
                        }
                        PLCAdapter adapter = this._Adapters[watch.LogicalStationNo];
                        watch.LastScanDT = now;
                        if (watch.Points > 0)
                        {
                            PLCOpRequest req = new PLCOpRequest();
                            req.ReqSNo = this.NewRecvReqSno;
                            PLCBlockOp op = new PLCBlockOp();
                            op.DevType = watch.DeviceCode;
                            op.DevNo = watch.Address;
                            op.Points = watch.Points;
                            op.Buf = new short[watch.NewSnapShot.Length];
                            op.Offset = 0;
                            req.BlockOps.Add(op);
                            PLCOpResult result = adapter.Read(req, 5000);
                            rcnt++;
                            if (result.RetCode != 0)
                            {
                                watch.LastScanErrorDT = now;
                                watch.LastScanError = result.RetCode + "," + result.RetErrMsg;
                                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "ScanProc", result.RetCode, string.Format("fail to scan,watchdata={0},err={1}", watch.Name, result.RetErrMsg), string.Empty, watch.LogicalStationNo);
                            }
                            else
                            {
                                watch.LastScanSpentTime = req.ReqCompDT.Subtract(req.ReqStartDT);
                                foreach (PLCBlockOp v in req.BlockOps)
                                {
                                    string reason;
                                    bool done = watch.PutData(v.Buf, out reason);
                                    double interval = DateTime.Now.Subtract(watch.LastPutDataDT).TotalMilliseconds;
                                    watch.LastPutDataDT = DateTime.Now;
                                    if (!done)
                                    {
                                        watch.LastScanErrorDT = now;
                                        watch.LastScanError = reason;
                                        this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "ScanProc", 20601, string.Format("fail to putdata,watchdata={0},err={1}", watch.Name, reason), string.Empty, watch.LogicalStationNo);
                                    }
                                }
                                if (this._IsDestroy)
                                {
                                    return;
                                }
                                watch.TotalScan++;
                            }
                        }
                    }
                    lock (this._SyncLock)
                    {
                        allValues = this._Model.Scan.AllValues;
                        for (int i = 0; i < allValues.Length; i++)
                        {
                            WatchData watch = (WatchData)allValues[i];
                            string err;
                            if (!watch.CopyTemp2New(out err))
                            {
                                watch.LastScanErrorDT = now;
                                watch.LastScanError = err;
                                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "ScanProc", 20602, string.Format("fail to do CopyTemp2New,watchdata={0},err={1}", watch.Name, err), string.Empty, watch.LogicalStationNo);
                            }
                        }
                    }
                    if (rcnt != 0)
                    {
                        this._ScanStartDT = now;
                        this._ScanSpentTime = DateTime.Now.Subtract(this._ScanStartDT);
                        if (this._ScanSpentTime > this._MaxScanSpentTime)
                        {
                            this._MaxScanSpentTime = this._ScanSpentTime;
                        }
                        this._TotalScan++;
                        this._TotalScanRead += rcnt;
                    }
                }
                catch (Exception ex)
                {
                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.EXCEPTION, "ScanProc", 20902, ex.ToString(), string.Empty, 0);
                }
            }
        }

        private void WriteProc()
        {
            while (true)
            {
                try
                {
                    this._WriteTaskState = string.Concat(new object[]
                    {
                        this._WriteTask.ManagedThreadId,
                        ",",
                        this._WriteTask.ThreadState.ToString(),
                        ",",
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff")
                    });
                    if (this._IsDestroy)
                    {
                        break;
                    }
                    if (!this.IsStarted)
                    {
                        Thread.Sleep(this._WriteTaskWaitPeriodMS);
                    }
                    else
                    {
                        bool empty = this.DoTrxWrite();
                        if (empty)
                        {
                            Thread.Sleep(this._WriteTaskWaitPeriodMS);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.EXCEPTION, "WriteProc", 20903, ex.ToString(), string.Empty, 0);
                }
            }
        }

        private bool DoTrxWrite()
        {
            PLCOpResult result = new PLCOpResult();
            Trx trx;
            bool exists = this._TrxWriteQueue.TryDequeue(out trx);
            bool result2;
            if (!exists || trx == null)
            {
                result2 = true;
            }
            else
            {
                trx.WriteRequestStations = new ConcurrentDictionary<int, object>();
                trx.WriteCompleteStations = new ConcurrentDictionary<int, object>();
                trx.WriteCompleteSync = new object();
                if ((ushort)(trx.TrxFlags & InternalFlagsEnum.IsTrxRawWrite) != 4)
                {
                    result = this.EncodeTrx(trx);
                    if (result.RetCode != 0)
                    {
                        this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxWrite", result.RetCode, string.Format("fail to encode,writetrx={0},reqsno={1},err={2}", trx.Name, trx.ReqSNo, result.RetErrMsg), trx.TrackKey, 0);
                        this.FireWriteTrxResultEvent(trx, 0, true, result.RetCode, result.RetErrMsg);
                        result2 = false;
                        return result2;
                    }
                }
                Dictionary<int, PLCOpRequest> requests = new Dictionary<int, PLCOpRequest>();
                object[] allValues = trx.EventGroups.AllValues;
                for (int i = 0; i < allValues.Length; i++)
                {
                    EventGroup evtgrp = (EventGroup)allValues[i];
                    if (!evtgrp.IsDisable)
                    {
                        if (evtgrp.IsMergeEvent)
                        {
                            if (evtgrp.Events.Count != 0)
                            {
                                Event evt0 = evtgrp.Events[0];
                                int startAddress10 = evt0.Metadata.StartAddress10;
                                int totalPoints = 0;
                                string reason = string.Empty;
                                object[] allValues2 = evtgrp.Events.AllValues;
                                for (int j = 0; j < allValues2.Length; j++)
                                {
                                    Event evt = (Event)allValues2[j];
                                    if (evt.Metadata.IsBitDeviceType)
                                    {
                                        reason = string.Format("fail to merge event,writetrx={0},reqsno={1},eventgroup={2},event={3},err=can't merge bit device event", new object[]
                                        {
                                            trx.Name,
                                            trx.ReqSNo,
                                            evtgrp.Name,
                                            evt.Name
                                        });
                                        this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxWrite", 20202, reason, trx.TrackKey, 0);
                                        this.FireWriteTrxResultEvent(trx, 0, true, 20202, reason);
                                        result2 = false;
                                        return result2;
                                    }
                                    if (evt.Metadata.DeviceCode != evt0.Metadata.DeviceCode)
                                    {
                                        reason = string.Format("fail to merge event,writetrx={0},reqsno={1},eventgroup={2},event={3},err=can't merge different device code", new object[]
                                        {
                                            trx.Name,
                                            trx.ReqSNo,
                                            evtgrp.Name,
                                            evt.Name
                                        });
                                        this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxWrite", 20202, reason, trx.TrackKey, 0);
                                        this.FireWriteTrxResultEvent(trx, 0, true, 20202, reason);
                                        result2 = false;
                                        return result2;
                                    }
                                    if (evt.Metadata.LogicalStationNo != evt0.Metadata.LogicalStationNo)
                                    {
                                        reason = string.Format("fail to merge event,writetrx={0},reqsno={1},eventgroup={2},event={3},err=can't merge different logicalstno", new object[]
                                        {
                                            trx.Name,
                                            trx.ReqSNo,
                                            evtgrp.Name,
                                            evt.Name
                                        });
                                        this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxWrite", 20202, reason, trx.TrackKey, 0);
                                        this.FireWriteTrxResultEvent(trx, 0, true, 20202, reason);
                                        result2 = false;
                                        return result2;
                                    }
                                    if (evt.Metadata.StartAddress10 != startAddress10)
                                    {
                                        reason = string.Format("fail to merge event,writetrx={0},reqsno={1},eventgroup={2},event={3},err=can't merge discontinous device address", new object[]
                                        {
                                            trx.Name,
                                            trx.ReqSNo,
                                            evtgrp.Name,
                                            evt.Name
                                        });
                                        this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxWrite", 20202, reason, trx.TrackKey, 0);
                                        this.FireWriteTrxResultEvent(trx, 0, true, 20202, reason);
                                        result2 = false;
                                        return result2;
                                    }
                                    startAddress10 += evt.Metadata.Points;
                                    totalPoints += evt.Metadata.Points;
                                }
                                evtgrp.MergeBuffer = new short[totalPoints];
                                int offset = 0;
                                allValues2 = evtgrp.Events.AllValues;
                                for (int j = 0; j < allValues2.Length; j++)
                                {
                                    Event evt = (Event)allValues2[j];
                                    if (evt.IsDisable)
                                    {
                                        offset += evt.Metadata.Points;
                                    }
                                    else if (evt.RawData == null)
                                    {
                                        offset += evt.Metadata.Points;
                                    }
                                    else
                                    {
                                        int points;
                                        if (evt.Metadata.Points > evt.RawData.Length)
                                        {
                                            points = evt.RawData.Length;
                                        }
                                        else
                                        {
                                            points = evt.Metadata.Points;
                                        }
                                        Buffer.BlockCopy(evt.RawData, 0, evtgrp.MergeBuffer, offset * 2, points * 2);
                                        offset += evt.Metadata.Points;
                                    }
                                }
                                PLCOpRequest req = null;
                                if (requests.ContainsKey(evt0.Metadata.LogicalStationNo))
                                {
                                    req = requests[evt0.Metadata.LogicalStationNo];
                                }
                                if (req == null)
                                {
                                    req = new PLCOpRequest();
                                    req.ReqSNo = trx.ReqSNo;
                                    req.LogicalStationNo = evt0.Metadata.LogicalStationNo;
                                    req.Tag = trx;
                                    requests.Add(evt0.Metadata.LogicalStationNo, req);
                                }
                                PLCBlockOp op = new PLCBlockOp();
                                op.OpDelayTimeMS = evt0.OpDelayTimeMS;
                                if (op.OpDelayTimeMS > 0)
                                {
                                    req.HasOpDelay = true;
                                }
                                op.EventKey = evtgrp.Name;
                                op.DevType = evt0.Metadata.DeviceCode;
                                op.DevNo = evt0.Metadata.Address;
                                op.Points = totalPoints;
                                op.Buf = evtgrp.MergeBuffer;
                                req.BlockOps.Add(op);
                            }
                        }
                        else
                        {
                            object[] allValues2 = evtgrp.Events.AllValues;
                            for (int j = 0; j < allValues2.Length; j++)
                            {
                                Event evt = (Event)allValues2[j];
                                if (!evt.IsDisable)
                                {
                                    if (evt.RawData != null)
                                    {
                                        PLCOpRequest req = null;
                                        if (requests.ContainsKey(evt.Metadata.LogicalStationNo))
                                        {
                                            req = requests[evt.Metadata.LogicalStationNo];
                                        }
                                        if (req == null)
                                        {
                                            req = new PLCOpRequest();
                                            req.ReqSNo = trx.ReqSNo;
                                            req.LogicalStationNo = evt.Metadata.LogicalStationNo;
                                            req.Tag = trx;
                                            requests.Add(evt.Metadata.LogicalStationNo, req);
                                        }
                                        if ((ushort)(trx.TrxFlags & InternalFlagsEnum.IsTrxRandWrite) == 128)
                                        {
                                            PLCRandOp op2;
                                            if (req.RandOps.Count > 0)
                                            {
                                                op2 = req.RandOps[0];
                                            }
                                            else
                                            {
                                                op2 = new PLCRandOp();
                                                op2.OpDelayTimeMS = 0;
                                                op2.EventKey = trx.Name;
                                                op2.Blocks = new List<PLCRandOp.RandBlock>();
                                                req.RandOps.Add(op2);
                                            }
                                            PLCRandOp.RandBlock b = new PLCRandOp.RandBlock();
                                            b.DevType = evt.Metadata.DeviceCode;
                                            b.DevNo = evt.Metadata.Address;
                                            b.Points = evt.Metadata.Points;
                                            b.Buf = evt.RawData;
                                            op2.Blocks.Add(b);
                                        }
                                        else if (evt.Metadata.IsBitDeviceType)
                                        {
                                            PLCRandOp op2 = new PLCRandOp();
                                            op2.OpDelayTimeMS = evt.OpDelayTimeMS;
                                            if (op2.OpDelayTimeMS > 0)
                                            {
                                                req.HasOpDelay = true;
                                            }
                                            op2.EventKey = evt.Name;
                                            op2.Blocks = new List<PLCRandOp.RandBlock>();
                                            PLCRandOp.RandBlock b = new PLCRandOp.RandBlock();
                                            b.DevType = evt.Metadata.DeviceCode;
                                            b.DevNo = evt.Metadata.Address;
                                            b.Points = evt.Metadata.Points;
                                            b.Buf = evt.RawData;
                                            op2.Blocks.Add(b);
                                            req.RandOps.Add(op2);
                                        }
                                        else
                                        {
                                            PLCBlockOp op = new PLCBlockOp();
                                            op.OpDelayTimeMS = evt.OpDelayTimeMS;
                                            if (op.OpDelayTimeMS > 0)
                                            {
                                                req.HasOpDelay = true;
                                            }
                                            op.EventKey = evt.Name;
                                            op.DevType = evt.Metadata.DeviceCode;
                                            op.DevNo = evt.Metadata.Address;
                                            op.Points = evt.Metadata.Points;
                                            op.Buf = evt.RawData;
                                            req.BlockOps.Add(op);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (requests.Count<KeyValuePair<int, PLCOpRequest>>() == 0)
                {
                    result.Set(21001, string.Format("empty writereq,trx={0}", trx.Name));
                    this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxWrite", result.RetCode, string.Format("empty writereq,writetrx={0},reqsno={1},err={2}", trx.Name, trx.ReqSNo, result.RetErrMsg), trx.TrackKey, 0);
                    this.FireWriteTrxResultEvent(trx, 0, true, result.RetCode, result.RetErrMsg);
                    result2 = false;
                }
                else
                {
                    int delay = (from r in requests.Values
                                 where r.HasOpDelay
                                 select r).Count<PLCOpRequest>();
                    int nodelay = (from r in requests.Values
                                   where !r.HasOpDelay
                                   select r).Count<PLCOpRequest>();
                    if (delay > 0 && nodelay > 0)
                    {
                        foreach (PLCOpRequest v in from r in requests.Values
                                                   where !r.HasOpDelay
                                                   select r)
                        {
                            PLCAdapter adapter = this._Adapters[v.LogicalStationNo];
                            result = adapter.Write(v, 10000);
                            PLCOpRequestResult rr = new PLCOpRequestResult();
                            rr.request = v;
                            rr.result = result;
                            trx.WriteCompleteStations.TryAdd(v.LogicalStationNo, rr);
                            if (result.RetCode != 0)
                            {
                                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "DoTrxWrite", result.RetCode, string.Format("fail to write,trx={0},reqsno={1},err={2}", trx.Name, trx.ReqSNo, result.RetErrMsg), trx.TrackKey, 0);
                                this.FireWriteTrxResultEvent(trx, 0, true, result.RetCode, result.RetErrMsg);
                                result2 = false;
                                return result2;
                            }
                            this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "DoTrxWrite", 0, string.Format("beginWrite(sync),ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3}", new object[]
                            {
                                trx.ReqSNo,
                                trx.Name,
                                trx.TrackKey,
                                trx.TrxFlags
                            }), trx.TrackKey, v.LogicalStationNo);
                        }
                        foreach (PLCOpRequest v in from r in requests.Values
                                                   where r.HasOpDelay
                                                   select r)
                        {
                            trx.WriteRequestStations.TryAdd(v.LogicalStationNo, v);
                        }
                        foreach (PLCOpRequest v in from r in requests.Values
                                                   where r.HasOpDelay
                                                   select r)
                        {
                            PLCAdapter adapter = this._Adapters[v.LogicalStationNo];
                            IAsyncResult ar = adapter.BeginWrite(v, new AsyncCallback(this.WriteCompelete));
                            this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "DoTrxWrite", 0, string.Format("beginWrite(async),ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3},IsCompleted={4}", new object[]
                            {
                                trx.ReqSNo,
                                trx.Name,
                                trx.TrackKey,
                                trx.TrxFlags,
                                ar.IsCompleted
                            }), trx.TrackKey, v.LogicalStationNo);
                        }
                    }
                    else
                    {
                        foreach (PLCOpRequest v in requests.Values)
                        {
                            trx.WriteRequestStations.TryAdd(v.LogicalStationNo, v);
                        }
                        foreach (PLCOpRequest v in requests.Values)
                        {
                            PLCAdapter adapter = this._Adapters[v.LogicalStationNo];
                            IAsyncResult ar = adapter.BeginWrite(v, new AsyncCallback(this.WriteCompelete));
                            this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "DoTrxWrite", 0, string.Format("beginWrite,ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3},IsCompleted={4}", new object[]
                            {
                                trx.ReqSNo,
                                trx.Name,
                                trx.TrackKey,
                                trx.TrxFlags,
                                ar.IsCompleted
                            }), trx.TrackKey, v.LogicalStationNo);
                        }
                    }
                    result2 = false;
                }
            }
            return result2;
        }

        private PLCOpResult EncodeTrx(Trx trx)
        {
            PLCOpResult result = new PLCOpResult();
            PLCOpResult result2;
            try
            {
                object[] allValues = trx.EventGroups.AllValues;
                for (int i = 0; i < allValues.Length; i++)
                {
                    EventGroup evtgrp = (EventGroup)allValues[i];
                    if (!evtgrp.IsDisable)
                    {
                        object[] allValues2 = evtgrp.Events.AllValues;
                        for (int j = 0; j < allValues2.Length; j++)
                        {
                            Event evt = (Event)allValues2[j];
                            if (!evt.IsDisable)
                            {
                                if (evt.Metadata.SkipDecode || evt.WriteRaw)
                                {
                                    if (evt.RawData == null)
                                    {
                                        result.Set(20702, string.Format("trx={0},evtgrp={1},evt={2},skip encode but rawdata is null", trx.Name, evtgrp.Name, evt.Name));
                                        result2 = result;
                                        return result2;
                                    }
                                    int len;
                                    if (evt.Metadata.IsBitDeviceType)
                                    {
                                        len = (evt.Metadata.Points - 1) / 16 + 1;
                                    }
                                    else
                                    {
                                        len = evt.Metadata.Points;
                                    }
                                    if (evt.RawData.Length != len)
                                    {
                                        result.Set(20702, string.Format("trx={0},evtgrp={1},evt={2},skip encode but rawdata length={3} <> {4}", new object[]
                                        {
                                            trx.Name,
                                            evtgrp.Name,
                                            evt.Name,
                                            evt.RawData.Length,
                                            len
                                        }));
                                        result2 = result;
                                        return result2;
                                    }
                                }
                                else
                                {
                                    if (evt.Metadata.IsBitDeviceType)
                                    {
                                        evt.RawData = new short[(evt.Metadata.Points - 1) / 16 + 1];
                                    }
                                    else
                                    {
                                        evt.RawData = new short[evt.Metadata.Points];
                                    }
                                    object[] allValues3 = evt.Items.AllValues;
                                    for (int k = 0; k < allValues3.Length; k++)
                                    {
                                        Item item = (Item)allValues3[k];
                                        try
                                        {
                                            this.SetItemValue(evt, item, evt.RawData);
                                        }
                                        catch (PLCDriverException ex)
                                        {
                                            result.Set(ex.Code, string.Format("trx={0},evtgrp={1},evt={2},fail to set item value,{3}", new object[]
                                            {
                                                trx.Name,
                                                evtgrp.Name,
                                                evt.Name,
                                                ex.Message
                                            }));
                                            result2 = result;
                                            return result2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                result.Set(20701, ex2.ToString());
            }
            result2 = result;
            return result2;
        }

        private void SetItemValue(Event evt, Item item, short[] data)
        {
            try
            {
                ItemExpressionEnum expression = item.Metadata.Expression;
                if (expression <= ItemExpressionEnum.ASCII)
                {
                    if (expression <= ItemExpressionEnum.SLONG)
                    {
                        switch (expression)
                        {
                            case ItemExpressionEnum.INT:
                                ExpressionINT.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                                break;
                            case ItemExpressionEnum.SINT:
                                ExpressionSINT.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                                break;
                            case ItemExpressionEnum.INT | ItemExpressionEnum.SINT:
                                break;
                            case ItemExpressionEnum.LONG:
                                ExpressionLONG.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                                break;
                            default:
                                if (expression == ItemExpressionEnum.SLONG)
                                {
                                    ExpressionSLONG.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                                }
                                break;
                        }
                    }
                    else if (expression != ItemExpressionEnum.EXP)
                    {
                        if (expression == ItemExpressionEnum.ASCII)
                        {
                            ExpressionASCII.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                        }
                    }
                    else
                    {
                        ExpressionEXP.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, data);
                    }
                }
                else if (expression <= ItemExpressionEnum.BIT)
                {
                    if (expression != ItemExpressionEnum.HEX)
                    {
                        if (expression == ItemExpressionEnum.BIT)
                        {
                            ExpressionBIT.Encode(item.Value, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                        }
                    }
                    else
                    {
                        ExpressionHEX.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                    }
                }
                else if (expression != ItemExpressionEnum.BIN)
                {
                    if (expression == ItemExpressionEnum.BCD)
                    {
                        ExpressionBCD.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data, this._FlipBCDValue);
                    }
                    if (expression == ItemExpressionEnum.FLOAT)
                    {
                        ExpressionFLOAT.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                    }
                    if (expression == ItemExpressionEnum.DOUBLE)
                    {
                        ExpressionDOUBLE.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                    }
                }
                else
                {
                    ExpressionBIN.Encode(item.Value, item.Metadata.WordOffset, item.Metadata.WordPoints, item.Metadata.BitOffset, item.Metadata.BitPoints, data);
                }
            }
            catch (Exception ex)
            {
                throw new PLCDriverException(20501, string.Format("startadd={0},woffset={1},wpoints={2},boffset={3},bpoints={4},itemval={5},err={6}", new object[]
                {
                    evt.Metadata.StartAddress10,
                    item.Metadata.WordOffset,
                    item.Metadata.WordPoints,
                    item.Metadata.BitOffset,
                    item.Metadata.BitPoints,
                    item.Value,
                    ex.ToString()
                }));
            }
        }

        private void WriteCompelete(IAsyncResult ar)
        {
            PLCOpRequest req = null;
            Trx trx = null;
            PLCOpResult result = null;
            try
            {
                req = (PLCOpRequest)ar.AsyncState;
                trx = (req.Tag as Trx);
                PLCAdapter adapter = this._Adapters[req.LogicalStationNo];
                result = adapter.EndWrite(ar);
                bool allcomp = false;
                lock (trx.WriteCompleteSync)
                {
                    object tmp;
                    trx.WriteRequestStations.TryRemove(req.LogicalStationNo, out tmp);
                    PLCOpRequestResult rr = new PLCOpRequestResult();
                    rr.request = req;
                    rr.result = result;
                    trx.WriteCompleteStations.TryAdd(req.LogicalStationNo, rr);
                    if (trx.WriteRequestStations.Count == 0)
                    {
                        allcomp = true;
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("allcomp={0},station={1},retcode={2},retmsg={3},hasDelay={4},StartDT={5},spenttime={6},IsTimeOut={7},", new object[]
                {
                    allcomp,
                    req.LogicalStationNo,
                    result.RetCode,
                    result.RetErrMsg,
                    req.HasOpDelay,
                    req.ReqStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"),
                    req.ReqCompDT.Subtract(req.ReqStartDT).ToString(),
                    req.IsReqTimeOut
                }));
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.DEBUG, "WriteComplete", result.RetCode, string.Format("writereq complete,ReqSNo={0},TrxName={1},TrackKey={2},{3}", new object[]
                {
                    trx.ReqSNo,
                    trx.Name,
                    trx.TrackKey,
                    sb.ToString()
                }), trx.TrackKey, req.LogicalStationNo);
                if (allcomp)
                {
                    foreach (PLCOpRequestResult v in trx.WriteCompleteStations.Values.Cast<PLCOpRequestResult>())
                    {
                        if (v.result.RetCode != 0)
                        {
                            this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.ERROR, "WriteComplete", v.result.RetCode, string.Format("fail to do writetrx,trx={0},reqsno={1},err={2}", trx.Name, trx.ReqSNo, v.result.RetErrMsg), trx.TrackKey, v.request.LogicalStationNo);
                            this.FireWriteTrxResultEvent(trx, v.request.LogicalStationNo, true, v.result.RetCode, v.result.RetErrMsg);
                            this._TotalWrite++;
                            return;
                        }
                    }
                    this.FireWriteTrxResultEvent(trx, 0, false, 0, string.Empty);
                    this._TotalWrite++;
                }
            }
            catch (Exception ex)
            {
                this.FirePLCDriverDebugOutEvent(PLCDriverDebugOutLevel.EXCEPTION, "WriteCompelete", 20901, ex.ToString(), string.Empty, 0);
            }
        }
    }
}
