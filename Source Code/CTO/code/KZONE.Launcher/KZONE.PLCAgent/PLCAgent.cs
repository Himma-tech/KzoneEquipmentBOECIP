using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using KZONE.Log;
using KZONE.MessageManager;
using KZONE.PLCAgent.PLC;
using KZONE.Work;

namespace KZONE.PLCAgent
{
	public class PLCAgent : IServerAgent, IConfiguration
	{
		private IPLCDriver _driver;

		private Thread _ReceiveTask;

		private Thread _SendTask;

		private CancellationTokenSource _LocalSource;

		private CancellationToken _Token;

		private int _TaskWaitPeriodMS = 3;

		private ConcurrentQueue<Trx> _TrxResultQueue = new ConcurrentQueue<Trx>();

		private bool _IsDestroy;

		private string _messageToken = "PLC";

		private bool _FlipBCDValue = true;

        private IQueueManager _QueueManager = new QueueManager();

        public IQueueManager QueueManager
        {
            get { return _QueueManager; }
            set { _QueueManager = value; }
        }

		public string Name
		{
			get;
			set;
		}

		public string ConfigFileName
		{
			get;
			set;
		}

		public string FormatFileName
		{
			get;
			set;
		}

		public IMessageManager MessageManager
		{
			get;
			set;
		}

        //public QueueManager QueueManager1
        //{
        //    get;
        //    set;
        //}
        //public IQueueManager QueueManager1
        //{
        //    get;
        //    set;
        //}
		public eAgentStatus AgentStatus
		{
			get;
			set;
		}

		public string ConnectedState
		{
			get
			{
				string result;
				if (this._driver != null)
				{
					IDictionary<string, object> dict = this._driver.PLCState;
					using (IEnumerator<object> enumerator = dict.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string state = (string)enumerator.Current;
							if (state == "Disconnected".ToString())
							{
								result = "Disconnected".ToString();
								return result;
							}
						}
					}
					result = "Connected".ToString();
				}
				else
				{
					result = "Disconnected".ToString();
				}
				return result;
			}
		}

		public IDictionary<string, object> RuntimeInfo
		{
			get
			{
				IDictionary<string, object> info = new Dictionary<string, object>();
				try
				{
					if (this._driver != null)
					{
						info = this._driver.RuntimeInfo;
					}
					info.Add("QueueCount", this.QueueManager.GetCount(this.Name).ToString());
				}
				catch
				{
				}
				return info;
			}
		}

		public object Tag
		{
			get;
			set;
		}

		public bool IsEnabled
		{
			get;
			set;
		}

		public IConfiguration Configuration
		{
			get
			{
				return this;
			}
		}

		public PLCDataModel Model
		{
			get
			{
				PLCDataModel result;
				if (this._driver != null)
				{
					result = this._driver.GetModel();
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		public string MessageToken
		{
			get
			{
				return this._messageToken;
			}
			set
			{
				this._messageToken = value;
			}
		}

		public bool DebugOutEnable
		{
			get
			{
				return this._driver != null && this._driver.DebugOutEnable;
			}
			set
			{
				if (this._driver != null)
				{
					this._driver.DebugOutEnable = value;
				}
			}
		}

		public bool FlipBCDValue
		{
			get
			{
				bool flipBCDValue;
				if (this._driver != null)
				{
					flipBCDValue = this._driver.FlipBCDValue;
				}
				else
				{
					flipBCDValue = this._FlipBCDValue;
				}
				return flipBCDValue;
			}
			set
			{
				this._FlipBCDValue = value;
				if (this._driver != null)
				{
					this._driver.FlipBCDValue = this._FlipBCDValue;
				}
			}
		}

		public bool ContainsTransaction(string trxName)
		{
			return this._driver != null && this._driver.GetModel().Transaction.Get(trxName) != null;
		}

		public bool Init()
		{
			bool result;
			try
			{
				this.AgentStatus = eAgentStatus.INIT;
				this.ConfigFileName = this.ConfigFileName.Replace("{ServerName}", Workbench.ServerName);
				this.ConfigFileName = this.ConfigFileName.Replace("{LineType}", Workbench.LineType);
				this.FormatFileName = this.FormatFileName.Replace("{ServerName}", Workbench.ServerName);
				this.FormatFileName = this.FormatFileName.Replace("{LineType}", Workbench.LineType);


				//  this.QueueManager..Add("PLC", null);
				//this.ConfigFileName = @"..\Configtep\Agent\PLC\Cell\C1BPI\C1BPI_PLCCfg.xml";
				//this.FormatFileName = @"..\Configtep\Agent\PLC\Cell\C1BPI\C1BPI_PLCFmt.xml";
				if (this._driver == null)
				{
					this._driver = PLCDriverFactory.CreatePLCDriver(this.Name);
					if (this._driver == null)
					{
						this.AgentStatus = eAgentStatus.ERROR;
						string log = string.Format("Fail to create PLCDriver,Name={0}", this.Name);
						NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", log);
						throw new Exception(log);
					}
					this._driver.PLCDriverDebugOutEvent += new PLCDriverDebugOutEventHandler(this.PLCDriverDebugOutEventHandler);
					this._driver.PLCConnectedEvent += new PLCConnectedEventHandler(this.PLCConnectedEventHandler);
					this._driver.PLCDisconnectedEvent += new PLCDisconnectedEventHandler(this.PLCDisconnectedEventHandler);
					this._driver.TrxTriggeredEvent += new TrxTriggeredEventHandler(this.TrxTriggeredEventHandler);
					this._driver.ReadTrxResultEvent += new ReadTrxResultEventHandler(this.ReadTrxResultEventHandler);
					this._driver.WriteTrxResultEvent += new WriteTrxResultEventHandler(this.WriteTrxResultEventHandler);
					this._driver.FlipBCDValue = this._FlipBCDValue;
				}
				string reason;
				bool done = this._driver.Init(this.ConfigFileName, this.FormatFileName, out reason);
				if (!done)
				{
					this.AgentStatus = eAgentStatus.ERROR;
					string log = string.Format("Fail to init PLCDriver,Name={0},Err={1}", this.Name, reason);
					NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", log);
					throw new Exception(log);
				}
				this.QueueManager.CreateQueue(this.Name);
				this._LocalSource = new CancellationTokenSource();
				this._Token = this._LocalSource.Token;
				this._ReceiveTask = new Thread(new ThreadStart(this.ReceiveTaskProc));
				this._ReceiveTask.IsBackground = true;
				this._ReceiveTask.Start();
				this._SendTask = new Thread(new ThreadStart(this.SendTaskProc));
				this._SendTask.IsBackground = true;
				this._SendTask.Start();
				result = done;
			}
			catch (Exception ex)
			{
				this.AgentStatus = eAgentStatus.ERROR;
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
				result = false;
			}
			return result;
		}

		public bool Start()
		{
			bool result;
			try
			{
				this.AgentStatus = eAgentStatus.START;
				if (this._driver == null)
				{
					this.AgentStatus = eAgentStatus.ERROR;
					result = false;
				}
				else if (this._driver.IsStarted)
				{
					result = false;
				}
				else
				{
					string reason;
					bool done = this._driver.Start(out reason);
					if (!done)
					{
						this.AgentStatus = eAgentStatus.ERROR;
						NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Fail to start PLCDriver,Name={0},Err={1}", this.Name, reason));
					}
					else
					{
						this.AgentStatus = eAgentStatus.RUN;
					}
					result = done;
				}
			}
			catch (Exception ex)
			{
				this.AgentStatus = eAgentStatus.ERROR;
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
				result = false;
			}
			return result;
		}

		public bool Stop()
		{
			this.AgentStatus = eAgentStatus.STOP;
			bool result;
			if (this._driver == null)
			{
				result = true;
			}
			else if (!this._driver.IsStarted)
			{
				result = true;
			}
			else
			{
				this._driver.Stop();
				result = true;
			}
			return result;
		}

		public bool Reset()
		{
			this.Stop();
			return this.Start();
		}

		public void Destroy()
		{
			this.AgentStatus = eAgentStatus.STOP;
			this._IsDestroy = true;
			if (this._driver != null)
			{
				this._driver.Destroy();
			}
		}

		public bool IsTrxExist(string trxName)
		{
			bool result;
			try
			{
				if (this._driver != null)
				{
					Trx trx = this._driver.GetEmptyTrx(trxName);
					result = (trx != null);
					return result;
				}
			}
			catch (Exception ex)
			{
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			result = false;
			return result;
		}

		public object GetTransactionFormat(string trxName)
		{
			object result;
			try
			{
				if (this._driver == null)
				{
					result = null;
				}
				else
				{
					Trx trx = this._driver.GetEmptyTrx(trxName);
					if (trx == null)
					{
						NLogManager.Logger.LogWarnWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("TransactionFormat not found,trxname=[{0}]", trxName));
					}
					result = trx;
				}
			}
			catch (Exception ex)
			{
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
				result = null;
			}
			return result;
		}

		public string ToFormatString()
		{
			string s = string.Empty;
			if (this._driver != null)
			{
				s = this._driver.ConfigContent;
			}
			return s;
		}

		public int SaveConfigFile(string formatString)
		{
			return 0;
		}

		public Trx SyncReadTrx(string trxName, bool log = false)
		{
			Trx result;
			try
			{
				if (this._driver == null)
				{
					result = null;
				}
				else
				{
					string reason;
					Trx trx = this._driver.SyncReadTrx(trxName, out reason);
					if (trx == null)
					{
						NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Fail to do SyncReadTrx,TrxName={0},Err={1}", trxName, reason));
					}
					else if (log)
					{
						this.TrxOut(trx);
						StringBuilder sb = new StringBuilder();
						sb.Append(string.Format("SyncReadTrx ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3},HasError={4},RetCode={5},RetMsg={6},", new object[]
						{
							trx.ReqSNo,
							trx.Name,
							trx.TrackKey,
							trx.TrxFlags,
							false,
							0,
							reason
						}));
						NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", sb.ToString());
					}
					result = trx;
				}
			}
			catch (Exception ex)
			{
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
				string reason = "Fail to do SyncReadTrx,occure exception=" + ex;
				result = null;
			}
			return result;
		}

		public bool SyncWriteTrx(Trx trx, bool log = false)
		{
			bool result;
			try
			{
				if (this._driver == null)
				{
					result = false;
				}
				else
				{
					string reason;
					bool done = this._driver.SyncWriteTrx(trx, out reason);
					if (log)
					{
						this.TrxOut(trx);
						StringBuilder sb = new StringBuilder();
						sb.Append(string.Format("SyncWriteTrx ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3},HasError={4},RetCode={5},RetMsg={6},", new object[]
						{
							trx.ReqSNo,
							trx.Name,
							trx.TrackKey,
							trx.TrxFlags,
							done,
							string.Empty,
							reason
						}));
						int cc = 0;
						using (IEnumerator<object> enumerator = trx.WriteCompleteStations.Values.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								PLCOpRequestResult rr = (PLCOpRequestResult)enumerator.Current;
								cc++;
								sb.Append(string.Format("(rr{0}: station={1},retcode={2},retmsg={3},delaywrite={4},StartDT={5},spenttime={6},IsTimeOut={7},", new object[]
								{
									cc,
									rr.request.LogicalStationNo,
									rr.result.RetCode,
									rr.result.RetErrMsg,
									rr.request.HasOpDelay,
									rr.request.ReqStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"),
									rr.request.ReqCompDT.Subtract(rr.request.ReqStartDT).ToString(),
									rr.request.IsReqTimeOut
								}));
								for (int i = 0; i < rr.request.BlockOps.Count; i++)
								{
									sb.Append(string.Format("(BlockOp{0}: IsOpComp={1},OpStartDT={2},OpCompDT={3},EventKey={4},OpDelayTime={5},Device={6},Address={7},Points={8})", new object[]
									{
										i,
										rr.request.BlockOps[i].IsOpComp,
										rr.request.BlockOps[i].OpStartDT.ToString("HH:mm:ss.ffff"),
										rr.request.BlockOps[i].OpCompDT.ToString("HH:mm:ss.ffff"),
										rr.request.BlockOps[i].EventKey,
										rr.request.BlockOps[i].OpDelayTimeMS,
										rr.request.BlockOps[i].DevType,
										rr.request.BlockOps[i].DevNo,
										rr.request.BlockOps[i].Points
									}));
								}
								for (int i = 0; i < rr.request.RandOps.Count; i++)
								{
									sb.Append(string.Format("(RandOp{0}: IsOpComp={1},OpStartDT={2},OpCompDT={3},EventKey={4},OpDelayTime={5}", new object[]
									{
										i,
										rr.request.RandOps[i].IsOpComp,
										rr.request.RandOps[i].OpStartDT.ToString("HH:mm:ss.ffff"),
										rr.request.RandOps[i].OpCompDT.ToString("HH:mm:ss.ffff"),
										rr.request.RandOps[i].EventKey,
										rr.request.RandOps[i].OpDelayTimeMS
									}));
									for (int j = 0; j < rr.request.RandOps[i].Blocks.Count; j++)
									{
										sb.Append(string.Format("(block{0}: Device={1},Address={2},Points={3})", new object[]
										{
											j,
											rr.request.RandOps[i].Blocks[j].DevType,
											rr.request.RandOps[i].Blocks[j].DevNo,
											rr.request.RandOps[i].Blocks[j].Points
										}));
									}
								}
								sb.Append(")");
							}
						}
						NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", sb.ToString());
					}
					result = done;
				}
			}
			catch (Exception ex)
			{
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
				string reason = "Fail to do SyncWriteTrx,occure exception=" + ex;
				result = false;
			}
			return result;
		}

		private void TrxOut(Trx trx)
		{
			NLogManager.Logger.LogTrxWrite(this.Name, trx.ToString());
		}

		public IEnumerable<string> GetTransactionKey()
		{
			return this._driver.GetModel().Transaction.AllKeys.AsEnumerable<string>();
		}

		public IEnumerable<object> GetTransactionValue()
		{
			return this._driver.GetModel().Transaction.AllValues.AsEnumerable<object>();
		}

		private void ReceiveTaskProc()
		{
			int cc = 0;
			while (true)
			{
				try
				{
					if (this._IsDestroy)
					{
						break;
					}
					bool empty = this.DoReceive();
					if (this._TrxResultQueue.Count <= 0)
					{
						Thread.Sleep(this._TaskWaitPeriodMS);
					}
					else
					{
						cc++;
						if (cc > 100)
						{
							cc = 0;
							Thread.Sleep(this._TaskWaitPeriodMS);
						}
					}
				}
				catch
				{
				}
			}
		}

		private bool DoReceive()
		{
			Trx trx;
			bool result;
			if (!this._TrxResultQueue.TryDequeue(out trx))
			{
				result = true;
			}
			else if (trx == null)
			{
				result = false;
			}
			else
			{
				this.TrxOut(trx);
				if ((ushort)(trx.TrxFlags & InternalFlagsEnum.IsTrxTrigger) == 8)
				{
					try
					{
						string name = string.Format("{1}{0}", trx.Name.Substring(trx.Name.IndexOf('_')), this.MessageToken);
						this.MessageManager.MessageDispatch(name, new object[]
						{
							trx
						});
						NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Dispatch MsgName={0},ReqSNo={1},TrxName={2},TrackKey={3},TrxFlags={4},TriggerCondition={5},IsInitTrigger={6},QueueDepth={7}", new object[]
						{
							name,
							trx.ReqSNo,
							trx.Name,
							trx.TrackKey,
							trx.TrxFlags.ToString(),
							trx.Metadata.TriggerCondition,
							trx.IsInitTrigger,
							this._TrxResultQueue.Count
						}));
					}
					catch (Exception ex)
					{
						NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
					}
				}
				else if ((ushort)(trx.TrxFlags & InternalFlagsEnum.IsTrxRead) == 1)
				{
					try
					{
						string name = string.Format("{1}{0}", trx.Name.Substring(trx.Name.IndexOf('_')), this.MessageToken);
						this.MessageManager.MessageDispatch(name, new object[]
						{
							trx
						});
						NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Dispatch MsgName={0},ReqSNo={1},TrxName={2},TrackKey={3},TrxFlags={4},QueueDepth={5}", new object[]
						{
							name,
							trx.ReqSNo,
							trx.Name,
							trx.TrackKey,
							trx.TrxFlags.ToString(),
							this._TrxResultQueue.Count
						}));
					}
					catch (Exception ex)
					{
						NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
					}
				}
				result = false;
			}
			return result;
		}

		private void SendTaskProc()
		{
			Stopwatch sw = new Stopwatch();
			int cc = 0;
			while (true)
			{
				try
				{
					if (this._IsDestroy)
					{
						break;
					}
					bool empty = this.DoSend(sw);
					int queue_count = this.QueueManager.GetCount(this.Name);
					if (queue_count <= 0)
					{
						Thread.Sleep(this._TaskWaitPeriodMS);
					}
					else
					{
						cc++;
						if (cc > 100)
						{
							cc = 0;
							Thread.Sleep(this._TaskWaitPeriodMS);
						}
					}
				}
				catch
				{
				}
			}
		}

		private bool DoSend(Stopwatch sw)
		{
			bool result;
			if (!this._driver.IsStarted)
			{
				result = true;
			}
			else
			{
				xMessage msg = this.QueueManager.GetMessage(this.Name);
				int queue_count = this.QueueManager.GetCount(this.Name);
				if (msg == null)
				{
					result = true;
				}
				else
				{
					Trx trx = msg.Data as Trx;
					NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Proc MsgName={0},TrxName={1},TrackKey={2},Queue Count={3}", new object[]
					{
						msg.Name,
						(trx == null) ? "null" : trx.Name,
						(trx == null) ? "" : trx.TrackKey.ToString(),
						queue_count
					}));
					if (trx == null)
					{
						result = false;
					}
					else
					{
						try
						{
							string reason;
							bool done;
							if ((ushort)(trx.TrxFlags & InternalFlagsEnum.IsTrxRawWrite) == 4)
							{
								done = this._driver.WriteTrxRaw(trx, out reason);
							}
							else
							{
								done = this._driver.WriteTrx(trx, out reason);
							}
							if (!done)
							{
								NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Fail to write Trx,TrxName={0},ReqSNo={1},TrackKey={2},TrxFlags={3},Err={4}", new object[]
								{
									trx.Name,
									trx.ReqSNo,
									trx.TrxFlags.ToString(),
									trx.TrackKey,
									reason
								}));
							}
						}
						catch (Exception ex)
						{
							NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
						}
						result = false;
					}
				}
			}
			return result;
		}

		private void ReadTrxResultEventHandler(object sender, ReadTrxResultEventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("ReadTrxResultEvent ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3},HasError={4},RetCode={5},RetMsg={6},", new object[]
			{
				e.SourceTrx.ReqSNo,
				e.SourceTrx.Name,
				e.SourceTrx.TrackKey,
				e.SourceTrx.TrxFlags,
				e.HasError,
				e.RetCode,
				e.RetMsg
			}));
			if ((ushort)(e.SourceTrx.TrxFlags & InternalFlagsEnum.IsDirectRead) == 16)
			{
				int cc = 0;
				using (IEnumerator<object> enumerator = e.SourceTrx.ReadCompleteStations.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PLCOpRequestResult rr = (PLCOpRequestResult)enumerator.Current;
						cc++;
						sb.Append(string.Format("(rr{0}: station={1},retcode={2},retmsg={3},hasDelay={4},StartDT={5},spenttime={6},IsTimeOut={7},", new object[]
						{
							cc,
							rr.request.LogicalStationNo,
							rr.result.RetCode,
							rr.result.RetErrMsg,
							rr.request.HasOpDelay,
							rr.request.ReqStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"),
							rr.request.ReqCompDT.Subtract(rr.request.ReqStartDT).ToString(),
							rr.request.IsReqTimeOut
						}));
						for (int i = 0; i < rr.request.BlockOps.Count; i++)
						{
							sb.Append(string.Format("(BlockOp{0}: IsOpComp={1},OpStartDT={2},OpCompDT={3},EventKey={4},OpDelayTime={5},Device={6},Address={7},Points={8})", new object[]
							{
								i,
								rr.request.BlockOps[i].IsOpComp,
								rr.request.BlockOps[i].OpStartDT.ToString("HH:mm:ss.ffff"),
								rr.request.BlockOps[i].OpCompDT.ToString("HH:mm:ss.ffff"),
								rr.request.BlockOps[i].EventKey,
								rr.request.BlockOps[i].OpDelayTimeMS,
								rr.request.BlockOps[i].DevType,
								rr.request.BlockOps[i].DevNo,
								rr.request.BlockOps[i].Points
							}));
						}
						for (int i = 0; i < rr.request.RandOps.Count; i++)
						{
							sb.Append(string.Format("(RandOp{0}: IsOpComp={1},OpStartDT={2},OpCompDT={3},EventKey={4},OpDelayTime={5},", new object[]
							{
								i,
								rr.request.RandOps[i].IsOpComp,
								rr.request.RandOps[i].OpStartDT.ToString("HH:mm:ss.ffff"),
								rr.request.RandOps[i].OpCompDT.ToString("HH:mm:ss.ffff"),
								rr.request.RandOps[i].EventKey,
								rr.request.RandOps[i].OpDelayTimeMS
							}));
							for (int j = 0; j < rr.request.RandOps[i].Blocks.Count; j++)
							{
								sb.Append(string.Format("(block{0}: Device={1},Address={2},Points={3})", new object[]
								{
									j,
									rr.request.RandOps[i].Blocks[j].DevType,
									rr.request.RandOps[i].Blocks[j].DevNo,
									rr.request.RandOps[i].Blocks[j].Points
								}));
							}
						}
						sb.Append(")");
					}
				}
			}
			NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", sb.ToString());
			this._TrxResultQueue.Enqueue(e.SourceTrx);
		}

		private void WriteTrxResultEventHandler(object sender, WriteTrxResultEventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("WriteTrxResultEvent ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3},HasError={4},RetCode={5},RetMsg={6},", new object[]
			{
				e.SourceTrx.ReqSNo,
				e.SourceTrx.Name,
				e.SourceTrx.TrackKey,
				e.SourceTrx.TrxFlags,
				e.HasError,
				e.RetCode,
				e.RetMsg
			}));
			int cc = 0;
			using (IEnumerator<object> enumerator = e.SourceTrx.WriteCompleteStations.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PLCOpRequestResult rr = (PLCOpRequestResult)enumerator.Current;
					cc++;
					sb.Append(string.Format("(rr{0}: station={1},retcode={2},retmsg={3},hasDelay={4},StartDT={5},spenttime={6},IsTimeOut={7},", new object[]
					{
						cc,
						rr.request.LogicalStationNo,
						rr.result.RetCode,
						rr.result.RetErrMsg,
						rr.request.HasOpDelay,
						rr.request.ReqStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"),
						rr.request.ReqCompDT.Subtract(rr.request.ReqStartDT).ToString(),
						rr.request.IsReqTimeOut
					}));
					for (int i = 0; i < rr.request.BlockOps.Count; i++)
					{
						sb.Append(string.Format("(BlockOp{0}: IsOpComp={1},OpStartDT={2},OpCompDT={3},EventKey={4},OpDelayTime={5},Device={6},Address={7},Points={8})", new object[]
						{
							i,
							rr.request.BlockOps[i].IsOpComp,
							rr.request.BlockOps[i].OpStartDT.ToString("HH:mm:ss.ffff"),
							rr.request.BlockOps[i].OpCompDT.ToString("HH:mm:ss.ffff"),
							rr.request.BlockOps[i].EventKey,
							rr.request.BlockOps[i].OpDelayTimeMS,
							rr.request.BlockOps[i].DevType,
							rr.request.BlockOps[i].DevNo,
							rr.request.BlockOps[i].Points
						}));
					}
					for (int i = 0; i < rr.request.RandOps.Count; i++)
					{
						sb.Append(string.Format("(RandOp{0}: IsOpComp={1},OpStartDT={2},OpCompDT={3},EventKey={4},OpDelayTime={5},", new object[]
						{
							i,
							rr.request.RandOps[i].IsOpComp,
							rr.request.RandOps[i].OpStartDT.ToString("HH:mm:ss.ffff"),
							rr.request.RandOps[i].OpCompDT.ToString("HH:mm:ss.ffff"),
							rr.request.RandOps[i].EventKey,
							rr.request.RandOps[i].OpDelayTimeMS
						}));
						for (int j = 0; j < rr.request.RandOps[i].Blocks.Count; j++)
						{
							sb.Append(string.Format("(block{0}: Device={1},Address={2},Points={3})", new object[]
							{
								j,
								rr.request.RandOps[i].Blocks[j].DevType,
								rr.request.RandOps[i].Blocks[j].DevNo,
								rr.request.RandOps[i].Blocks[j].Points
							}));
						}
					}
					sb.Append(")");
				}
			}
			NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", sb.ToString());
			this.TrxOut(e.SourceTrx);
		}

		private void TrxTriggeredEventHandler(object sender, TrxTriggeredEventArgs e)
		{
			NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("TrxTriggeredEvent ReqSNo={0},TrxName={1},TrackKey={2},TrxFlags={3},condition={4},IsInitTrigger={5}", new object[]
			{
				e.SourceTrx.ReqSNo,
				e.SourceTrx.Name,
				e.SourceTrx.TrackKey,
				e.SourceTrx.TrxFlags,
				e.SourceTrx.Metadata.TriggerCondition,
				e.SourceTrx.IsInitTrigger
			}));
			this._TrxResultQueue.Enqueue(e.SourceTrx);
		}

		private void PLCDisconnectedEventHandler(object sender, PLCDisconnectedEventArgs e)
		{
			NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("PLCDisconnectedEvent PLC diconnected,StationNo={0}", e.SourceStationNo));
			try
			{
				string name = string.Format("{0}_PLCStatusChange", this.MessageToken);
				this.MessageManager.MessageDispatch(name, new object[]
				{
					e.SourceStationNo.ToString(),
					"Disconnected".ToString()
				});
				NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Dispatch MsgName={0},StationNo={1},Statue={2}", name, e.SourceStationNo, "Disconnected".ToString()));
			}
			catch (Exception ex)
			{
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
			}
		}

		private void PLCConnectedEventHandler(object sender, PLCConnectedEventArgs e)
		{
			NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("PLCConnectedEvent PLC connected,StationNo={0}", e.SourceStationNo));
			try
			{
				string name = string.Format("{0}_PLCStatusChange", this.MessageToken);
				this.MessageManager.MessageDispatch(name, new object[]
				{
					e.SourceStationNo.ToString(),
					"Connected".ToString()
				});
				NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Dispatch MsgName={0},StationNo={1},Statue={2}", name, e.SourceStationNo, "Connected".ToString()));
			}
			catch (Exception ex)
			{
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
			}
		}

		private void PLCDriverDebugOutEventHandler(object sender, PLCDriverDebugOutEventArgs e)
		{
			string s = string.Format("LogLevel={0},MethodName={1},StationNo={2},TrackKey={3},Code={4},Message={5}", new object[]
			{
				e.LogLevel,
				e.MethodName,
				e.SourceStationNo,
				e.TrackKey,
				e.Code,
				e.Message
			});
			if (e.LogLevel == PLCDriverDebugOutLevel.ERROR || e.LogLevel == PLCDriverDebugOutLevel.EXCEPTION)
			{
				NLogManager.Logger.LogErrorWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", s);
			}
			else if (e.LogLevel == PLCDriverDebugOutLevel.DUMP)
			{
				NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", e.Message);
			}
			else
			{
				NLogManager.Logger.LogDebugWrite(this.Name, base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", s);
			}
		}
	}
}
