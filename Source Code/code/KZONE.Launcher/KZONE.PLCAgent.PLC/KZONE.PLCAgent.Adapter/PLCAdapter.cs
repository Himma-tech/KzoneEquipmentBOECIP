using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// adapter to access plc
	/// *NOTE:adapter must dispose if no use
	/// </summary>
	public class PLCAdapter : IDisposable
	{
		private bool _IsInitializing;

		private bool _IsInited;

		private bool _IsOpened;

		private bool _IsDisposed;

		private Thread _Worker;

		private int _MaxThreadSleepTimeMS = 100;

		private int _MaxWorkIterateTimes = 10;

		private bool _CancelRunRequest;

		private object _ThisLock = new object();

		private AbstractAdapter _Adapter;

		private int _LogicalStationNo;

		private bool _IsEnableTrigger;

		private bool _IsEnableW2ZR;

		private string _PLCType;

		private string _LinkType;

		private Hashtable _ParamTable;

		private Dictionary<string, DeviceSymbol> _DeviceSymbolTable = new Dictionary<string, DeviceSymbol>();

		private ConcurrentQueue<AsyncResult<PLCOpResult>> _ReadReqQueue = new ConcurrentQueue<AsyncResult<PLCOpResult>>();

		private ConcurrentQueue<AsyncResult<PLCOpResult>> _WriteReqQueue = new ConcurrentQueue<AsyncResult<PLCOpResult>>();

		private ConcurrentDictionary<int, AsyncResult<PLCOpResult>> _ReadDelayDict = new ConcurrentDictionary<int, AsyncResult<PLCOpResult>>();

		private ConcurrentDictionary<int, AsyncResult<PLCOpResult>> _WriteDelayDict = new ConcurrentDictionary<int, AsyncResult<PLCOpResult>>();

		private AutoResetEvent _AutoResetReadWriteEvent = new AutoResetEvent(false);

		/// <summary>
		/// Event Notify when plc connected
		/// </summary>
		public event EventHandler ConnectedEvent;

		/// <summary>
		/// Event Notify when plc Disconnected
		/// </summary>
		public event EventHandler DisconnectedEvent;

		/// <summary>
		/// plc adapter log event
		/// </summary>
		public event PLCAdapterDebugOutEventHandler PLCAdapterDebugOutEvent;

		/// <summary>
		/// get plc logical station no (1-1023)
		/// </summary>
		public int LogicalStationNo
		{
			get
			{
				return this._LogicalStationNo;
			}
		}

		/// <summary>
		/// get is enable trigger
		/// </summary>
		public bool IsTriggerEnable
		{
			get
			{
				return this._IsEnableTrigger;
			}
		}

		/// <summary>
		/// get is enable W convert to ZR
		/// </summary>
		public bool IsEnableW2ZR
		{
			get
			{
				return this._IsEnableW2ZR;
			}
		}

		/// <summary>
		/// get/set is default adapter
		/// </summary>
		public bool IsDefaultAdapter
		{
			get;
			set;
		}

		/// <summary>
		/// get/set worker thread state (thread id+stat+timestamp)
		/// </summary>
		public string WorkerState
		{
			get;
			internal set;
		}

		/// <summary>
		/// get is adapter inited 
		/// </summary>
		public bool IsInited
		{
			get
			{
				return this._IsInited;
			}
		}

		/// <summary>
		/// get is adapter opened
		/// </summary>
		public bool IsOpened
		{
			get
			{
				return this._IsOpened;
			}
		}

		/// <summary>
		/// get is plc connected
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return this._Adapter != null && this._Adapter.IsConnected;
			}
		}

		/// <summary>
		/// get plc type
		/// </summary>
		public string PLCType
		{
			get
			{
				return this._PLCType;
			}
		}

		/// <summary>
		/// get plc link type
		/// </summary>
		public string LinkType
		{
			get
			{
				return this._LinkType;
			}
		}

		/// <summary>
		/// get plc available device list
		/// </summary>
		public Dictionary<string, DeviceSymbol> Devices
		{
			get
			{
				return this._DeviceSymbolTable;
			}
		}

		/// <summary>
		/// get/set thread worker max sleep time(ms) ,default=5
		/// </summary>
		public int MaxThreadSleepTimeMS
		{
			get
			{
				return this._MaxThreadSleepTimeMS;
			}
			set
			{
				this._MaxThreadSleepTimeMS = value;
				if (this._MaxThreadSleepTimeMS <= 0)
				{
					this._MaxThreadSleepTimeMS = 5;
				}
			}
		}

		/// <summary>
		/// get/set max iterate work times,default=5
		/// </summary>
		public int MaxWorkIterateTimes
		{
			get
			{
				return this._MaxWorkIterateTimes;
			}
			set
			{
				this._MaxWorkIterateTimes = value;
				if (this._MaxWorkIterateTimes <= 0)
				{
					this._MaxWorkIterateTimes = 5;
				}
			}
		}

		/// <summary>
		/// get last plc operation error code
		/// </summary>		
		public int PLCLastErrorCode
		{
			get;
			set;
		}

		/// <summary>
		/// get last plc operation error desc
		/// </summary>
		public string PLCLastErrorDesc
		{
			get;
			set;
		}

		/// <summary>
		/// get last plc operation error datetime
		/// </summary>
		public DateTime PLCLastErrorDT
		{
			get;
			set;
		}

		/// <summary>
		/// plc parameter list
		/// </summary>		
		public Hashtable PLCParameterTable
		{
			get
			{
				Hashtable result;
				if (this._Adapter != null)
				{
					result = (Hashtable)this._Adapter.ParameterTable.Clone();
				}
				else
				{
					result = new Hashtable();
				}
				return result;
			}
		}

		/// <summary>
		/// get read request queue depth
		/// </summary>
		public int ReadReqQueueDepth
		{
			get
			{
				return this._ReadReqQueue.Count;
			}
		}

		/// <summary>
		/// get write request queue depth
		/// </summary>
		public int WriteReqQueueDepth
		{
			get
			{
				return this._WriteReqQueue.Count;
			}
		}

		/// <summary>
		/// get read dealy count
		/// </summary>
		public int ReadDelayCount
		{
			get
			{
				return this._ReadDelayDict.Count;
			}
		}

		/// <summary>
		/// get write delay count
		/// </summary>
		public int WriteDelayCount
		{
			get
			{
				return this._WriteDelayDict.Count;
			}
		}

		/// <summary>
		/// begin adapter asyncronous init operation
		/// </summary>
		/// <param name="plctype">plc type(MITSUBISHI)</param>
		/// <param name="linktype">plc link type(MX3,MDFUNC)</param>
		/// <param name="paramTable">plc parameters</param>
		/// <param name="logicalStno">plc logical station no</param>
		/// <param name="isTrigger">is enable trigger</param>
		/// <param name="isW2ZR">is enable W convert to ZR</param>
		/// <param name="callback">init comp callback</param>
		/// <param name="state">user object</param>		
		public IAsyncResult BeginInit(string plctype, string linktype, Hashtable paramTable, int logicalStno, bool isTrigger, bool isW2ZR, AsyncCallback callback, object state)
		{
			PLCOpResult result = new PLCOpResult();
			AsyncResult<PLCOpResult> ar = new AsyncResult<PLCOpResult>(callback, state);
			IAsyncResult result2;
			try
			{
				lock (this._ThisLock)
				{
					if (this._IsInited)
					{
						result.Set(10001, "adapter already init");
						ar.SetAsCompleted(result, false);
						result2 = ar;
						return result2;
					}
					if (this._IsInitializing)
					{
						result.Set(10002, "adapter is current initializing");
						ar.SetAsCompleted(result, false);
						result2 = ar;
						return result2;
					}
					this._IsInitializing = true;
				}
				this._PLCType = plctype;
				this._LinkType = linktype;
				this._ParamTable = paramTable;
				this._LogicalStationNo = logicalStno;
				this._IsEnableTrigger = isTrigger;
				this._IsEnableW2ZR = isW2ZR;
				this._Worker = new Thread(new ParameterizedThreadStart(this.ThreadProc));
				this._Worker.Priority = ThreadPriority.Highest;
				this._Worker.Name = string.Concat(new object[]
				{
					"PLCAdapter_",
					plctype,
					"_",
					linktype,
					"_",
					this._LogicalStationNo
				});
				this._Worker.SetApartmentState(ApartmentState.STA);
				this._Worker.Start(ar);
			}
			catch (Exception ex)
			{
				this._IsInitializing = false;
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "BeginInit", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10004, ex.ToString());
				ar.SetAsCompleted(result, false);
			}
			result2 = ar;
			return result2;
		}

		/// <summary>
		/// end adapter asyncronous init operation
		/// </summary>
		/// <param name="asyncResult">asyncResult</param>
		public PLCOpResult EndInit(IAsyncResult asyncResult)
		{
			PLCOpResult result = new PLCOpResult();
			try
			{
				AsyncResult<PLCOpResult> ar = (AsyncResult<PLCOpResult>)asyncResult;
				result = ar.EndInvoke();
			}
			catch (Exception ex)
			{
				result.Set(10051, ex.ToString());
			}
			return result;
		}

		/// <summary>
		/// end adapter asyncronous init operation,wait until comp or timeout
		/// </summary>
		/// <param name="asyncResult">asyncResult</param>
		/// <param name="timeoutMS">timeout time(ms)</param>
		public PLCOpResult EndInit(IAsyncResult asyncResult, int timeoutMS)
		{
			PLCOpResult result = new PLCOpResult();
			try
			{
				AsyncResult<PLCOpResult> ar = (AsyncResult<PLCOpResult>)asyncResult;
				result = ar.EndInvoke(timeoutMS);
				if (result == null)
				{
					result = new PLCOpResult();
				}
				if (ar.IsTimeOut)
				{
					result.Set(10008, "Init timeout");
				}
			}
			catch (Exception ex)
			{
				result.Set(10051, ex.ToString());
			}
			return result;
		}

		/// <summary>
		/// adapter syncronous init operation
		/// </summary>
		/// <param name="plctype">plc type(MITSUBISHI)</param>
		/// <param name="linktype">plc link type(MX3,MDFUNC)</param>
		/// <param name="paramTable">plc parameters</param>
		/// <param name="logicalStno">plc logical station no</param>
		/// <param name="isTrigger">is plc trigger</param>
		/// <param name="isW2ZR">is device W convert to ZR</param>
		/// <param name="timeoutMS">timeout time(ms)</param>
		/// <returns>result</returns>
		public PLCOpResult Init(string plctype, string linktype, Hashtable paramTable, int logicalStno, bool isTrigger, bool isW2ZR, int timeoutMS)
		{
			return this.EndInit(this.BeginInit(plctype, linktype, paramTable, logicalStno, isTrigger, isW2ZR, null, null), timeoutMS);
		}

		/// <summary>
		/// adapter open operation to connect plc
		/// </summary>
		public PLCOpResult Open()
		{
			PLCOpResult result = new PLCOpResult();
			PLCOpResult result2;
			if (!this._IsInited)
			{
				result.Set(10101, "adapter not init yet");
				result2 = result;
			}
			else
			{
				this._IsOpened = true;
				result2 = result;
			}
			return result2;
		}

		/// <summary>
		/// adapter close operation to disconnect plc
		/// </summary>		
		public PLCOpResult Close()
		{
			PLCOpResult result = new PLCOpResult();
			PLCOpResult result2;
			if (!this._IsInited)
			{
				result.Set(10201, "adapter not init yet");
				result2 = result;
			}
			else
			{
				this._IsOpened = false;
				result2 = result;
			}
			return result2;
		}

		/// <summary>
		/// begin adapter asyncronous read operation		
		/// </summary>
		/// <param name="request">read request object</param>
		/// <param name="callback">read complete callback</param>				
		public IAsyncResult BeginRead(PLCOpRequest request, AsyncCallback callback)
		{
			PLCOpResult result = new PLCOpResult();
			AsyncResult<PLCOpResult> ar = new AsyncResult<PLCOpResult>(callback, request);
			IAsyncResult result2;
			try
			{
				if (!this._IsInited)
				{
					result.Set(10301, "adapter not init yet");
					ar.SetAsCompleted(result, false);
					result2 = ar;
					return result2;
				}
				if (!this._IsOpened)
				{
					result.Set(10302, "adapter not open yet");
					ar.SetAsCompleted(result, false);
					result2 = ar;
					return result2;
				}
				if (!this._Adapter.IsConnected)
				{
					result.Set(10303, string.Format("PLC not connected,LastPLCError {0},{1},{2}", this.PLCLastErrorDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"), this.PLCLastErrorCode, this.PLCLastErrorDesc));
					ar.SetAsCompleted(result, false);
					result2 = ar;
					return result2;
				}
				if (0 == request.BlockOps.Count + request.RandOps.Count)
				{
					result.Set(10310, "no operation found");
					ar.SetAsCompleted(result, false);
					result2 = ar;
					return result2;
				}
				foreach (PLCBlockOp block in request.BlockOps)
				{
					DeviceSymbol device = this._Adapter.GetDeviceSymbol(block.DevType);
					if (device == null)
					{
						result.Set(10306, string.Format("unknown device type {0}", block.DevType));
						ar.SetAsCompleted(result, false);
						result2 = ar;
						return result2;
					}
					int dno = 0;
					try
					{
						dno = Convert.ToInt32(block.DevNo, device.nbase);
						if (device.nbase == 16)
						{
							block.DevNo = dno.ToString("X7");
						}
					}
					catch
					{
						result.Set(10308, string.Format("start device no error {0}", block.DevNo));
						ar.SetAsCompleted(result, false);
						result2 = ar;
						return result2;
					}
					if (device.isbit)
					{
						if (dno % 16 != 0)
						{
							result.Set(10409, string.Format("bit device block read devno {0} not multiple of 16", block.DevNo));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
						if (block.Points % 16 != 0)
						{
							result.Set(10410, string.Format("bit device block read points {0} not multiple of 16", block.Points));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
						if (block.Buf.Length - block.Offset < (block.Points - 1) / 16 + 1)
						{
							result.Set(10307, string.Format("buf size error, length {0} - offset {1} < points {2}", block.Buf.Length, block.Offset, (block.Points - 1) / 16 + 1));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
					}
					else if (block.Buf.Length - block.Offset < block.Points)
					{
						result.Set(10307, string.Format("buf size error, length {0} - offset {1} < points {2}", block.Buf.Length, block.Offset, block.Points));
						ar.SetAsCompleted(result, false);
						result2 = ar;
						return result2;
					}
				}
				foreach (PLCRandOp op in request.RandOps)
				{
					int bc = 0;
					foreach (PLCRandOp.RandBlock block2 in op.Blocks)
					{
						bc++;
						DeviceSymbol device = this._Adapter.GetDeviceSymbol(block2.DevType);
						if (device == null)
						{
							result.Set(10306, string.Format("unknown device type {0},block {1}", block2.DevType, bc));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
						if (device.isbit)
						{
							if (block2.Buf.Length != (block2.Points - 1) / 16 + 1)
							{
								result.Set(10307, string.Format("buf size error {0}<>{1},block {2}", block2.Buf.Length, (block2.Points - 1) / 16 + 1, bc));
								ar.SetAsCompleted(result, false);
								result2 = ar;
								return result2;
							}
						}
						else if (block2.Buf.Length != block2.Points)
						{
							result.Set(10307, string.Format("buf size error {0}<>{1},block {2}", block2.Buf.Length, block2.Points, bc));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
						try
						{
							int dno = Convert.ToInt32(block2.DevNo, device.nbase);
							if (device.nbase == 16)
							{
								block2.DevNo = dno.ToString("X7");
							}
						}
						catch
						{
							result.Set(10308, string.Format("start device no error {0},block {1}", block2.DevNo, bc));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
					}
				}
				request.ReqOpType = "READ";
				request.LogicalStationNo = this.LogicalStationNo;
				request.ReqStartDT = DateTime.Now;
				this._ReadReqQueue.Enqueue(ar);
				this._AutoResetReadWriteEvent.Set();
			}
			catch (Exception ex)
			{
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "BeginRead", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10305, ex.ToString());
				ar.SetAsCompleted(result, false);
			}
			result2 = ar;
			return result2;
		}

		/// <summary>
		/// end adapter asyncronous block read operation
		/// </summary>
		/// <param name="asyncResult">ayncresult</param>
		public PLCOpResult EndRead(IAsyncResult asyncResult)
		{
			PLCOpResult result = new PLCOpResult();
			try
			{
				AsyncResult<PLCOpResult> ar = (AsyncResult<PLCOpResult>)asyncResult;
				result = ar.EndInvoke();
			}
			catch (Exception ex)
			{
				result.Set(10351, ex.ToString());
			}
			return result;
		}

		/// <summary>
		/// end adapter asyncronous block read operation,wait until comp or timeout
		/// </summary>
		/// <param name="timeoutMS">request timout time(ms)</param>
		/// <param name="asyncResult">asyncResult</param>
		public PLCOpResult EndRead(IAsyncResult asyncResult, int timeoutMS)
		{
			PLCOpResult result = new PLCOpResult();
			try
			{
				AsyncResult<PLCOpResult> ar = (AsyncResult<PLCOpResult>)asyncResult;
				result = ar.EndInvoke(timeoutMS);
				if (result == null)
				{
					result = new PLCOpResult();
				}
				if (ar.IsTimeOut)
				{
					result.Set(10309, "read operation timeout");
					PLCOpRequest req = (PLCOpRequest)ar.AsyncState;
					req.ReqCompDT = DateTime.Now;
					req.IsReqTimeOut = true;
				}
			}
			catch (Exception ex)
			{
				result.Set(10351, ex.ToString());
			}
			return result;
		}

		/// <summary>
		/// adapter syncronous read operation	
		/// </summary>
		/// <param name="request">read request object</param>
		/// <param name="timeoutMS">request timout time(ms)</param>
		/// <returns>asyncResult</returns>
		public PLCOpResult Read(PLCOpRequest request, int timeoutMS)
		{
			return this.EndRead(this.BeginRead(request, null), timeoutMS);
		}

		/// <summary>
		/// begin adapter asyncronous write operation		
		/// </summary>
		/// <param name="request">write request object</param>
		/// <param name="callback">write complete callback</param>		
		public IAsyncResult BeginWrite(PLCOpRequest request, AsyncCallback callback)
		{
			PLCOpResult result = new PLCOpResult();
			AsyncResult<PLCOpResult> ar = new AsyncResult<PLCOpResult>(callback, request);
			IAsyncResult result2;
			try
			{
				if (!this._IsInited)
				{
					result.Set(10401, "adapter not init yet");
					ar.SetAsCompleted(result, false);
					result2 = ar;
					return result2;
				}
				if (!this._IsOpened)
				{
					result.Set(10402, "adapter not open yet");
					ar.SetAsCompleted(result, false);
					result2 = ar;
					return result2;
				}
				if (!this._Adapter.IsConnected)
				{
					result.Set(10403, string.Format("PLC not connected,LastPLCError {0},{1},{2}", this.PLCLastErrorDT.ToString("yyyy/MM/dd HH:mm:ss.ffff"), this.PLCLastErrorCode, this.PLCLastErrorDesc));
					ar.SetAsCompleted(result, false);
					result2 = ar;
					return result2;
				}
				if (0 == request.BlockOps.Count + request.RandOps.Count)
				{
					result.Set(10412, "no operation found");
					ar.SetAsCompleted(result, false);
					result2 = ar;
					return result2;
				}
				foreach (PLCBlockOp block in request.BlockOps)
				{
					DeviceSymbol device = this._Adapter.GetDeviceSymbol(block.DevType);
					if (device == null)
					{
						result.Set(10406, string.Format("unknown device type {0}", block.DevType));
						ar.SetAsCompleted(result, false);
						result2 = ar;
						return result2;
					}
					int dno = 0;
					try
					{
						dno = Convert.ToInt32(block.DevNo, device.nbase);
						if (device.nbase == 16)
						{
							block.DevNo = dno.ToString("X7");
						}
					}
					catch
					{
						result.Set(10408, string.Format("start device no error {0}", block.DevNo));
						ar.SetAsCompleted(result, false);
						result2 = ar;
						return result2;
					}
					if (device.isbit)
					{
						if (dno % 16 != 0)
						{
							result.Set(10410, string.Format("bit device block write devno {0} not multiple of 16", block.DevNo));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
						if (block.Points % 16 != 0)
						{
							result.Set(10410, string.Format("bit device block write points {0} not multiple of 16", block.Points));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
						if (block.Buf.Length - block.Offset < (block.Points - 1) / 16 + 1)
						{
							result.Set(10407, string.Format("buf size error, length {0} - offset {1} < points {2}", block.Buf.Length, block.Offset, (block.Points - 1) / 16 + 1));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
					}
					else if (block.Buf.Length - block.Offset < block.Points)
					{
						result.Set(10407, string.Format("buf size error, length {0} - offset {1} < points {2}", block.Buf.Length, block.Offset, block.Points));
						ar.SetAsCompleted(result, false);
						result2 = ar;
						return result2;
					}
				}
				foreach (PLCRandOp op in request.RandOps)
				{
					int bc = 0;
					foreach (PLCRandOp.RandBlock block2 in op.Blocks)
					{
						bc++;
						DeviceSymbol device = this._Adapter.GetDeviceSymbol(block2.DevType);
						if (device == null)
						{
							result.Set(10406, string.Format("unknown device type {0},block {1}", block2.DevType, bc));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
						if (device.isbit)
						{
							if (block2.Buf.Length != (block2.Points - 1) / 16 + 1)
							{
								result.Set(10407, string.Format("buf size error {0}<>{1},block {2}", block2.Buf.Length, (block2.Points - 1) / 16 + 1, bc));
								ar.SetAsCompleted(result, false);
								result2 = ar;
								return result2;
							}
						}
						else if (block2.Buf.Length != block2.Points)
						{
							result.Set(10407, string.Format("buf size error {0}<>{1},block {2}", block2.Buf.Length, block2.Points, bc));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
						try
						{
							int dno = Convert.ToInt32(block2.DevNo, device.nbase);
							if (device.nbase == 16)
							{
								block2.DevNo = dno.ToString("X7");
							}
						}
						catch
						{
							result.Set(10408, string.Format("start device no error {0},block {1}", block2.DevNo, bc));
							ar.SetAsCompleted(result, false);
							result2 = ar;
							return result2;
						}
					}
				}
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "BeginWrite", 0, string.Format("try to enqueue writereq,reqsno={0}", request.ReqSNo), string.Empty, this._LogicalStationNo);
				request.ReqOpType = "WRITE";
				request.LogicalStationNo = this.LogicalStationNo;
				request.ReqStartDT = DateTime.Now;
				this._WriteReqQueue.Enqueue(ar);
				this._AutoResetReadWriteEvent.Set();
			}
			catch (Exception ex)
			{
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "BeginWrite", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10405, ex.ToString());
				ar.SetAsCompleted(result, false);
			}
			result2 = ar;
			return result2;
		}

		/// <summary>
		/// end adapter asyncronous block write operation
		/// </summary>
		/// <param name="asyncResult">asyncresult</param>
		public PLCOpResult EndWrite(IAsyncResult asyncResult)
		{
			PLCOpResult result = new PLCOpResult();
			try
			{
				AsyncResult<PLCOpResult> ar = (AsyncResult<PLCOpResult>)asyncResult;
				result = ar.EndInvoke();
			}
			catch (Exception ex)
			{
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "EndWrite", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10452, ex.ToString());
			}
			return result;
		}

		/// <summary>
		/// end adapter asyncronous block write operation,wait until comp or timeout
		/// </summary>
		/// <param name="timeoutMS">request timout time(ms)</param>
		/// <param name="asyncResult">asyncResult</param>
		public PLCOpResult EndWrite(IAsyncResult asyncResult, int timeoutMS)
		{
			PLCOpResult result = new PLCOpResult();
			try
			{
				AsyncResult<PLCOpResult> ar = (AsyncResult<PLCOpResult>)asyncResult;
				result = ar.EndInvoke(timeoutMS);
				if (result == null)
				{
					result = new PLCOpResult();
				}
				if (ar.IsTimeOut)
				{
					result.Set(10411, "write operation timeout");
					PLCOpRequest req = (PLCOpRequest)ar.AsyncState;
					req.ReqCompDT = DateTime.Now;
					req.IsReqTimeOut = true;
				}
			}
			catch (Exception ex)
			{
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "EndWriteT", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10452, ex.ToString());
			}
			return result;
		}

		/// <summary>
		/// adapter syncronous write operation	
		/// </summary>
		/// <param name="request">write request object</param>
		/// <param name="timeoutMS">request timout time(ms)</param>
		/// <returns>asyncResult</returns>
		public PLCOpResult Write(PLCOpRequest request, int timeoutMS)
		{
			return this.EndWrite(this.BeginWrite(request, null), timeoutMS);
		}

		/// <summary>
		/// dispose adapter
		/// </summary>
		public void Dispose()
		{
			if (!this._IsDisposed)
			{
				this._IsDisposed = true;
				this._CancelRunRequest = true;
			}
		}

		private void FireConnectedEvent()
		{
			try
			{
				EventHandler tmp = Interlocked.CompareExchange<EventHandler>(ref this.ConnectedEvent, null, null);
				if (tmp != null)
				{
					tmp(this, new EventArgs());
				}
			}
			catch
			{
			}
		}

		private void FireDisConnectedEvent()
		{
			try
			{
				EventHandler tmp = Interlocked.CompareExchange<EventHandler>(ref this.DisconnectedEvent, null, null);
				if (tmp != null)
				{
					tmp(this, new EventArgs());
				}
			}
			catch
			{
			}
		}

		private void FirePLCAdapterDebugOutEvent(string level, string method, int code, string msg, string trackkey, int station)
		{
			try
			{
				PLCAdapterDebugOutEventHandler tmp = Interlocked.CompareExchange<PLCAdapterDebugOutEventHandler>(ref this.PLCAdapterDebugOutEvent, null, null);
				if (tmp != null)
				{
					tmp(this, new PLCAdapterDebugOutEventArgs(level, method, code, msg, trackkey, station));
				}
			}
			catch
			{
			}
		}

		private void ThreadProc(object state)
		{
			PLCOpResult result = new PLCOpResult();
			AsyncResult<PLCOpResult> ar = state as AsyncResult<PLCOpResult>;
			try
			{
				string reason;
				this._Adapter = AdapterFactory.CreateAdapter(this._PLCType, this._LinkType, out reason);
				if (this._Adapter == null)
				{
					result.Set(10006, "create Adapter fail," + reason);
					ar.SetAsCompleted(result, false);
					return;
				}
				int ret = this._Adapter.Init(this._ParamTable);
				if (ret != 0)
				{
					this.PLCLastErrorDT = DateTime.Now;
					this.PLCLastErrorCode = ret;
					this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
					result.Set(10007, string.Format("init operation fail,err={0},desc={1}", ret.ToString("X"), this.PLCLastErrorDesc));
					ar.SetAsCompleted(result, false);
					return;
				}
				foreach (KeyValuePair<string, DeviceSymbol> v in this._Adapter.DeviceSymbolTable)
				{
					this._DeviceSymbolTable.Add(v.Key, (DeviceSymbol)v.Value.Clone());
				}
				this._IsInited = true;
				ar.SetAsCompleted(result, false);
			}
			catch (Exception ex)
			{
				result.Set(10005, ex.ToString());
				ar.SetAsCompleted(result, false);
				return;
			}
			finally
			{
				this._IsInitializing = false;
			}
			while (true)
			{
				try
				{
					this.WorkerState = string.Concat(new string[]
					{
						Thread.CurrentThread.ManagedThreadId.ToString(),
						",",
						Thread.CurrentThread.ThreadState.ToString(),
						",",
						DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff")
					});
					if (this._CancelRunRequest)
					{
						if (this._IsOpened)
						{
							this.Close();
						}
						if (this._Adapter.IsConnected)
						{
							this._Adapter.Disconnect();
							this._Adapter.IsConnected = false;
						}
						break;
					}
					for (int i = 0; i < this._MaxWorkIterateTimes; i++)
					{
						this.DoWork();
					}
					this._AutoResetReadWriteEvent.WaitOne(this._MaxThreadSleepTimeMS);
				}
				catch (Exception ex)
				{
					this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "ThreadProc", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
					if (this._IsOpened)
					{
						this._Adapter.IsConnected = false;
					}
				}
			}
		}

		private void DoWork()
		{
			if (!this._IsDisposed)
			{
				if (!this._CancelRunRequest)
				{
					if (!this._IsOpened)
					{
						if (this._Adapter.IsConnected)
						{
							this._Adapter.Disconnect();
							this._Adapter.IsConnected = false;
							this.FireDisConnectedEvent();
						}
					}
					else if (!this._Adapter.IsConnected)
					{
						if (this._Adapter.DetachEnable)
						{
							this._Adapter.Disconnect();
						}
						int ret = this._Adapter.Connect();
						if (ret != 0)
						{
							this.PLCLastErrorDT = DateTime.Now;
							this.PLCLastErrorCode = ret;
							this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
						}
						else
						{
							this._Adapter.IsConnected = true;
							this.FireConnectedEvent();
						}
						if (this._CancelRunRequest)
						{
							return;
						}
					}
					this.HandleReadReq();
					for (int i = 0; i < this._MaxWorkIterateTimes * 2; i++)
					{
						this.HandleWriteReq();
						if (this._CancelRunRequest)
						{
							return;
						}
					}
					this.HandleReadDelay();
					this.HandleWriteDelay();
				}
			}
		}

		private void HandleReadReq()
		{
			PLCOpResult result = new PLCOpResult();
			AsyncResult<PLCOpResult> ar = null;
			PLCOpRequest req = null;
			try
			{
				while (true)
				{
					bool exists = this._ReadReqQueue.TryDequeue(out ar);
					if (!exists || ar == null)
					{
						break;
					}
					req = (PLCOpRequest)ar.AsyncState;
					if (!ar.IsTimeOut)
					{
						goto IL_98;
					}
					if (req != null)
					{
						this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleReadReq", 0, string.Format("reqsno={0} readreq is timeout", req.ReqSNo), string.Empty, this._LogicalStationNo);
					}
				}
				return;
				IL_98:
				if (req == null)
				{
					result.Set(10705, "null request");
					ar.SetAsCompleted(result, false);
				}
				else
				{
					for (int cc = 0; cc < req.BlockOps.Count; cc++)
					{
						PLCBlockOp op = req.BlockOps[cc];
						if (op.OpDelayTimeMS > 0)
						{
							req.HasOpDelay = true;
						}
						else
						{
							op.OpStartDT = DateTime.Now;
							DeviceSymbol device = this._Adapter.GetDeviceSymbol(op.DevType);
							int ret;
							if (device.isbit)
							{
								ret = this._Adapter.BlockRead(op.DevType, op.DevNo, (op.Points - 1) / 16 + 1, op.Buf, op.Offset);
							}
							else
							{
								ret = this._Adapter.BlockRead(op.DevType, op.DevNo, op.Points, op.Buf, op.Offset);
							}
							if (ret != 0)
							{
								this.PLCLastErrorDT = DateTime.Now;
								this.PLCLastErrorCode = ret;
								this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
								if (this._Adapter.DetachEnable)
								{
									this._Adapter.Disconnect();
									this._Adapter.IsConnected = false;
									this.FireDisConnectedEvent();
								}
								result.Set(10750, string.Format("reqsno={0} eventkey={1} block read err={2},desc={3}", new object[]
								{
									req.ReqSNo,
									op.EventKey,
									ret.ToString("X"),
									this.PLCLastErrorDesc
								}));
							}
							op.IsOpComp = true;
							op.OpCompDT = DateTime.Now;
							req.BlockOps[cc] = op;
							if (ret != 0)
							{
								req.ReqCompDT = DateTime.Now;
								ar.SetAsCompleted(result, false);
								return;
							}
						}
					}
					for (int cc = 0; cc < req.RandOps.Count; cc++)
					{
						PLCRandOp op2 = req.RandOps[cc];
						if (op2.OpDelayTimeMS > 0)
						{
							req.HasOpDelay = true;
						}
						else
						{
							op2.OpStartDT = DateTime.Now;
							int ret = this._Adapter.RandomRead(op2.Blocks);
							if (ret != 0)
							{
								this.PLCLastErrorDT = DateTime.Now;
								this.PLCLastErrorCode = ret;
								this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
								if (this._Adapter.DetachEnable)
								{
									this._Adapter.Disconnect();
									this._Adapter.IsConnected = false;
									this.FireDisConnectedEvent();
								}
								result.Set(10751, string.Format("reqsno={0} eventkey={1} random read err={2},desc={3}", new object[]
								{
									req.ReqSNo,
									op2.EventKey,
									ret.ToString("X"),
									this.PLCLastErrorDesc
								}));
							}
							op2.IsOpComp = true;
							op2.OpCompDT = DateTime.Now;
							req.RandOps[cc] = op2;
							if (ret != 0)
							{
								req.ReqCompDT = DateTime.Now;
								ar.SetAsCompleted(result, false);
								return;
							}
						}
					}
					if (req.HasOpDelay)
					{
						req.DelayStartTime = DateTime.Now;
						if (!this._ReadDelayDict.TryAdd(req.ReqSNo, ar))
						{
							result.Set(10710, "readdelay add fail");
							ar.SetAsCompleted(result, false);
						}
					}
					else
					{
						req.ReqCompDT = DateTime.Now;
						ar.SetAsCompleted(result, false);
					}
				}
			}
			catch (Exception ex)
			{
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "HandleReadReq", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10701, ex.ToString());
				if (req != null)
				{
					req.ReqCompDT = DateTime.Now;
				}
				if (ar != null)
				{
					ar.SetAsCompleted(result, false);
				}
			}
		}

		private void HandleWriteReq()
		{
			PLCOpResult result = new PLCOpResult();
			AsyncResult<PLCOpResult> ar = null;
			PLCOpRequest req = null;
			try
			{
				while (this._WriteReqQueue.TryDequeue(out ar))
				{
					req = (PLCOpRequest)ar.AsyncState;
					this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleWriteReq", 0, string.Format("writereq dequeue,reqsno={0}", req.ReqSNo), string.Empty, this._LogicalStationNo);
					if (ar.IsTimeOut)
					{
						if (req != null)
						{
							this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleWriteReq", 0, string.Format("reqsno={0} writereq is timeout", req.ReqSNo), string.Empty, this._LogicalStationNo);
						}
					}
					else
					{
						if (req == null)
						{
							result.Set(10707, "null request");
							ar.SetAsCompleted(result, false);
							break;
						}
						for (int cc = 0; cc < req.BlockOps.Count; cc++)
						{
							PLCBlockOp op = req.BlockOps[cc];
							if (op.OpDelayTimeMS > 0)
							{
								req.HasOpDelay = true;
							}
							else
							{
								op.OpStartDT = DateTime.Now;
								DeviceSymbol device = this._Adapter.GetDeviceSymbol(op.DevType);
								int ret;
								if (device.isbit)
								{
									ret = this._Adapter.BlockWrite(op.DevType, op.DevNo, (op.Points - 1) / 16 + 1, op.Buf, op.Offset);
								}
								else
								{
									ret = this._Adapter.BlockWrite(op.DevType, op.DevNo, op.Points, op.Buf, op.Offset);
								}
								this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleWriteReq", ret, string.Format("block write reqsno={0} eventkey={1} ret={2} SDT={3}", new object[]
								{
									req.ReqSNo,
									op.EventKey,
									ret,
									op.OpStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff")
								}), string.Empty, this._LogicalStationNo);
								if (ret != 0)
								{
									this.PLCLastErrorDT = DateTime.Now;
									this.PLCLastErrorCode = ret;
									this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
									if (this._Adapter.DetachEnable)
									{
										this._Adapter.Disconnect();
										this._Adapter.IsConnected = false;
										this.FireDisConnectedEvent();
									}
									result.Set(10752, string.Format("reqsno={0} eventkey={1} block write err={2},desc={3}", new object[]
									{
										req.ReqSNo,
										op.EventKey,
										ret.ToString("X"),
										this.PLCLastErrorDesc
									}));
								}
								op.IsOpComp = true;
								op.OpCompDT = DateTime.Now;
								req.BlockOps[cc] = op;
								if (ret != 0)
								{
									req.ReqCompDT = DateTime.Now;
									ar.SetAsCompleted(result, false);
									return;
								}
							}
						}
						for (int cc = 0; cc < req.RandOps.Count; cc++)
						{
							PLCRandOp op2 = req.RandOps[cc];
							if (op2.OpDelayTimeMS > 0)
							{
								req.HasOpDelay = true;
							}
							else
							{
								op2.OpStartDT = DateTime.Now;
								int ret = this._Adapter.RandomWrite(op2.Blocks);
								this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleWriteReq", ret, string.Format("random write reqsno={0} eventkey={1} ret={2} SDT={3}", new object[]
								{
									req.ReqSNo,
									op2.EventKey,
									ret,
									op2.OpStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff")
								}), string.Empty, this._LogicalStationNo);
								if (ret != 0)
								{
									this.PLCLastErrorDT = DateTime.Now;
									this.PLCLastErrorCode = ret;
									this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
									if (this._Adapter.DetachEnable)
									{
										this._Adapter.Disconnect();
										this._Adapter.IsConnected = false;
										this.FireDisConnectedEvent();
									}
									result.Set(10753, string.Format("reqsno={0} eventkey={1} random write err={2},desc={3}", new object[]
									{
										req.ReqSNo,
										op2.EventKey,
										ret.ToString("X"),
										this.PLCLastErrorDesc
									}));
								}
								op2.IsOpComp = true;
								op2.OpCompDT = DateTime.Now;
								req.RandOps[cc] = op2;
								if (ret != 0)
								{
									req.ReqCompDT = DateTime.Now;
									ar.SetAsCompleted(result, false);
									return;
								}
							}
						}
						if (req.HasOpDelay)
						{
							req.DelayStartTime = DateTime.Now;
							bool done = this._WriteDelayDict.TryAdd(req.ReqSNo, ar);
							this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleWriteReq", 0, string.Format("writedelay add,reqsno={0}", req.ReqSNo), string.Empty, this._LogicalStationNo);
							if (!done)
							{
								result.Set(10709, "writedelay add fail");
								ar.SetAsCompleted(result, false);
							}
						}
						else
						{
							req.ReqCompDT = DateTime.Now;
							ar.SetAsCompleted(result, false);
						}
						break;
					}
				}
			}
			catch (Exception ex)
			{
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "HandleWriteReq", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10702, ex.ToString());
				if (req != null)
				{
					req.ReqCompDT = DateTime.Now;
				}
				if (ar != null)
				{
					ar.SetAsCompleted(result, false);
				}
			}
		}

		private void HandleReadDelay()
		{
			PLCOpResult result = new PLCOpResult();
			AsyncResult<PLCOpResult> ar = null;
			PLCOpRequest req = null;
			try
			{
				if (!this._ReadDelayDict.IsEmpty)
				{
					List<AsyncResult<PLCOpResult>> arlist = this._ReadDelayDict.Values.ToList<AsyncResult<PLCOpResult>>();
					foreach (AsyncResult<PLCOpResult> v in arlist)
					{
						ar = v;
						req = (PLCOpRequest)ar.AsyncState;
						if (ar.IsTimeOut)
						{
							if (req != null)
							{
								AsyncResult<PLCOpResult> tmp;
								this._ReadDelayDict.TryRemove(req.ReqSNo, out tmp);
								this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleReadDelay", 0, string.Format("reqsno={0} readdelay is timeout", req.ReqSNo), string.Empty, this._LogicalStationNo);
							}
						}
						else
						{
							if (req == null)
							{
								result.Set(10706, "null request");
								ar.SetAsCompleted(result, false);
								return;
							}
							for (int cc = 0; cc < req.BlockOps.Count; cc++)
							{
								PLCBlockOp op = req.BlockOps[cc];
								if (!op.IsOpComp)
								{
									if (Math.Abs(DateTime.Now.Subtract(req.DelayStartTime).TotalMilliseconds) >= (double)op.OpDelayTimeMS)
									{
										op.OpStartDT = DateTime.Now;
										DeviceSymbol device = this._Adapter.GetDeviceSymbol(op.DevType);
										int ret;
										if (device.isbit)
										{
											ret = this._Adapter.BlockRead(op.DevType, op.DevNo, (op.Points - 1) / 16 + 1, op.Buf, op.Offset);
										}
										else
										{
											ret = this._Adapter.BlockRead(op.DevType, op.DevNo, op.Points, op.Buf, op.Offset);
										}
										if (ret != 0)
										{
											this.PLCLastErrorDT = DateTime.Now;
											this.PLCLastErrorCode = ret;
											this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
											if (this._Adapter.DetachEnable)
											{
												this._Adapter.Disconnect();
												this._Adapter.IsConnected = false;
												this.FireDisConnectedEvent();
											}
											result.Set(10750, string.Format("reqsno={0} eventkey={1} block read err={2},desc={3}", new object[]
											{
												req.ReqSNo,
												op.EventKey,
												ret.ToString("X"),
												this.PLCLastErrorDesc
											}));
										}
										op.IsOpComp = true;
										op.OpCompDT = DateTime.Now;
										req.BlockOps[cc] = op;
										if (ret != 0)
										{
											req.ReqCompDT = DateTime.Now;
											ar.SetAsCompleted(result, false);
											return;
										}
									}
								}
							}
							for (int cc = 0; cc < req.RandOps.Count; cc++)
							{
								PLCRandOp op2 = req.RandOps[cc];
								if (!op2.IsOpComp)
								{
									if (Math.Abs(DateTime.Now.Subtract(req.DelayStartTime).TotalMilliseconds) >= (double)op2.OpDelayTimeMS)
									{
										op2.OpStartDT = DateTime.Now;
										int ret = this._Adapter.RandomRead(op2.Blocks);
										if (ret != 0)
										{
											this.PLCLastErrorDT = DateTime.Now;
											this.PLCLastErrorCode = ret;
											this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
											if (this._Adapter.DetachEnable)
											{
												this._Adapter.Disconnect();
												this._Adapter.IsConnected = false;
												this.FireDisConnectedEvent();
											}
											result.Set(10751, string.Format("reqsno={0} eventkey={1} random read err={2},desc={3}", new object[]
											{
												req.ReqSNo,
												op2.EventKey,
												ret.ToString("X"),
												this.PLCLastErrorDesc
											}));
										}
										op2.IsOpComp = true;
										op2.OpCompDT = DateTime.Now;
										req.RandOps[cc] = op2;
										if (ret != 0)
										{
											req.ReqCompDT = DateTime.Now;
											ar.SetAsCompleted(result, false);
											return;
										}
									}
								}
							}
							bool allcomp = true;
							foreach (PLCRandOp op2 in req.RandOps)
							{
								//PLCRandOp op2;
								if (!op2.IsOpComp)
								{
									allcomp = false;
									break;
								}
							}
							foreach (PLCBlockOp op in req.BlockOps)
							{
								//PLCBlockOp op;
								if (!op.IsOpComp)
								{
									allcomp = false;
								}
							}
							if (allcomp)
							{
								AsyncResult<PLCOpResult> tmp;
								this._ReadDelayDict.TryRemove(req.ReqSNo, out tmp);
								req.ReqCompDT = DateTime.Now;
								ar.SetAsCompleted(result, false);
							}
						}
					}
					arlist.Clear();
				}
			}
			catch (Exception ex)
			{
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "HandleReadDelay", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10703, ex.ToString());
				if (req != null)
				{
					req.ReqCompDT = DateTime.Now;
					AsyncResult<PLCOpResult> tmp;
					this._ReadDelayDict.TryRemove(req.ReqSNo, out tmp);
				}
				if (ar != null)
				{
					ar.SetAsCompleted(result, false);
				}
			}
		}

		private void HandleWriteDelay()
		{
			PLCOpResult result = new PLCOpResult();
			AsyncResult<PLCOpResult> ar = null;
			PLCOpRequest req = null;
			try
			{
				if (!this._WriteDelayDict.IsEmpty)
				{
					List<AsyncResult<PLCOpResult>> arlist = this._WriteDelayDict.Values.ToList<AsyncResult<PLCOpResult>>();
					foreach (AsyncResult<PLCOpResult> v in arlist)
					{
						ar = v;
						req = (PLCOpRequest)ar.AsyncState;
						if (ar.IsTimeOut)
						{
							if (req != null)
							{
								AsyncResult<PLCOpResult> tmp;
								this._WriteDelayDict.TryRemove(req.ReqSNo, out tmp);
								this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleWriteDelay", 0, string.Format("reqsno={0} writedelay is timeout", req.ReqSNo), string.Empty, this._LogicalStationNo);
							}
						}
						else
						{
							if (req == null)
							{
								result.Set(10708, "null request");
								ar.SetAsCompleted(result, false);
								return;
							}
							for (int cc = 0; cc < req.BlockOps.Count; cc++)
							{
								PLCBlockOp op = req.BlockOps[cc];
								if (!op.IsOpComp)
								{
									if (Math.Abs(DateTime.Now.Subtract(req.DelayStartTime).TotalMilliseconds) >= (double)op.OpDelayTimeMS)
									{
										op.OpStartDT = DateTime.Now;
										DeviceSymbol device = this._Adapter.GetDeviceSymbol(op.DevType);
										int ret;
										if (device.isbit)
										{
											ret = this._Adapter.BlockWrite(op.DevType, op.DevNo, (op.Points - 1) / 16 + 1, op.Buf, op.Offset);
										}
										else
										{
											ret = this._Adapter.BlockWrite(op.DevType, op.DevNo, op.Points, op.Buf, op.Offset);
										}
										this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleWriteDelay", ret, string.Format("block write reqsno={0} eventkey={1} ret={2} SDT={3}", new object[]
										{
											req.ReqSNo,
											op.EventKey,
											ret,
											op.OpStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff")
										}), string.Empty, this._LogicalStationNo);
										if (ret != 0)
										{
											this.PLCLastErrorDT = DateTime.Now;
											this.PLCLastErrorCode = ret;
											this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
											if (this._Adapter.DetachEnable)
											{
												this._Adapter.Disconnect();
												this._Adapter.IsConnected = false;
												this.FireDisConnectedEvent();
											}
											result.Set(10752, string.Format("reqsno={0} eventkey={1} block write err={2},desc={3}", new object[]
											{
												req.ReqSNo,
												op.EventKey,
												ret.ToString("X"),
												this.PLCLastErrorDesc
											}));
										}
										op.IsOpComp = true;
										op.OpCompDT = DateTime.Now;
										req.BlockOps[cc] = op;
										if (ret != 0)
										{
											req.ReqCompDT = DateTime.Now;
											ar.SetAsCompleted(result, false);
											return;
										}
									}
								}
							}
							for (int cc = 0; cc < req.RandOps.Count; cc++)
							{
								PLCRandOp op2 = req.RandOps[cc];
								if (!op2.IsOpComp)
								{
									if (Math.Abs(DateTime.Now.Subtract(req.DelayStartTime).TotalMilliseconds) >= (double)op2.OpDelayTimeMS)
									{
										op2.OpStartDT = DateTime.Now;
										int ret = this._Adapter.RandomWrite(op2.Blocks);
										this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.DEBUG.ToString(), "HandleWriteDelay", ret, string.Format("random write reqsno={0} eventkey={1} ret={2} SDT={3}", new object[]
										{
											req.ReqSNo,
											op2.EventKey,
											ret,
											op2.OpStartDT.ToString("yyyy/MM/dd HH:mm:ss.ffff")
										}), string.Empty, this._LogicalStationNo);
										if (ret != 0)
										{
											this.PLCLastErrorDT = DateTime.Now;
											this.PLCLastErrorCode = ret;
											this.PLCLastErrorDesc = this._Adapter.GetErrorDesc(ret);
											if (this._Adapter.DetachEnable)
											{
												this._Adapter.Disconnect();
												this._Adapter.IsConnected = false;
												this.FireDisConnectedEvent();
											}
											result.Set(10753, string.Format("reqsno={0} eventkey={1} random write err={2},desc={3}", new object[]
											{
												req.ReqSNo,
												op2.EventKey,
												ret.ToString("X"),
												this.PLCLastErrorDesc
											}));
										}
										op2.IsOpComp = true;
										op2.OpCompDT = DateTime.Now;
										req.RandOps[cc] = op2;
										if (ret != 0)
										{
											req.ReqCompDT = DateTime.Now;
											ar.SetAsCompleted(result, false);
											return;
										}
									}
								}
							}
							bool allcomp = true;
							foreach (PLCRandOp op2 in req.RandOps)
							{
								//PLCRandOp op2;
								if (!op2.IsOpComp)
								{
									allcomp = false;
									break;
								}
							}
							foreach (PLCBlockOp op in req.BlockOps)
							{
								//PLCBlockOp op;
								if (!op.IsOpComp)
								{
									allcomp = false;
								}
							}
							if (allcomp)
							{
								AsyncResult<PLCOpResult> tmp;
								bool done = this._WriteDelayDict.TryRemove(req.ReqSNo, out tmp);
								req.ReqCompDT = DateTime.Now;
								ar.SetAsCompleted(result, false);
							}
						}
					}
					arlist.Clear();
				}
			}
			catch (Exception ex)
			{
				this.FirePLCAdapterDebugOutEvent(PLCAdapterDebugOutLevel.EXCEPTION.ToString(), "HandleWriteDelay", -1, ex.ToString(), string.Empty, this._LogicalStationNo);
				result.Set(10704, ex.ToString());
				if (req != null)
				{
					req.ReqCompDT = DateTime.Now;
					AsyncResult<PLCOpResult> tmp;
					this._WriteDelayDict.TryRemove(req.ReqSNo, out tmp);
				}
				if (ar != null)
				{
					ar.SetAsCompleted(result, false);
				}
			}
		}
	}
}
