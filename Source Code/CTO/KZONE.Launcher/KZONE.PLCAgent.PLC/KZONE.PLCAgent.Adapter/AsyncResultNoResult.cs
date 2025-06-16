using System;
using System.Threading;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// asyncresult with no result
	/// </summary>
	public class AsyncResultNoResult : IAsyncResult
	{
		private const int c_StatePending = 0;

		private const int c_StateCompletedSynchronously = 1;

		private const int c_StateCompletedAsynchronously = 2;

		private readonly AsyncCallback m_AsyncCallback;

		private readonly object m_AsyncState;

		private int m_CompletedState = 0;

		private ManualResetEvent m_AsyncWaitHandle;

		private Exception m_exception;

		/// <summary>
		/// get/set is timeout occure
		/// </summary>
		public bool IsTimeOut
		{
			get;
			set;
		}

		/// <summary>
		/// get async state
		/// </summary>
		public object AsyncState
		{
			get
			{
				return this.m_AsyncState;
			}
		}

		/// <summary>
		/// get is completed synchronously
		/// </summary>
		public bool CompletedSynchronously
		{
			get
			{
				return this.m_CompletedState == 1;
			}
		}

		/// <summary>
		/// get async waithandle
		/// </summary>
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (this.m_AsyncWaitHandle == null)
				{
					bool done = this.IsCompleted;
					ManualResetEvent mre = new ManualResetEvent(done);
					if (Interlocked.CompareExchange<ManualResetEvent>(ref this.m_AsyncWaitHandle, mre, null) != null)
					{
						mre.Close();
					}
					else if (!done && this.IsCompleted)
					{
						this.m_AsyncWaitHandle.Set();
					}
				}
				return this.m_AsyncWaitHandle;
			}
		}

		/// <summary>
		/// get is async result complete
		/// </summary>
		public bool IsCompleted
		{
			get
			{
				return this.m_CompletedState != 0;
			}
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="asyncCallback">callback function</param>
		/// <param name="state">stat object</param>
		public AsyncResultNoResult(AsyncCallback asyncCallback, object state)
		{
			this.m_AsyncCallback = asyncCallback;
			this.m_AsyncState = state;
		}

		/// <summary>
		/// set async result complete
		/// </summary>
		/// <param name="exception">error exception</param>
		/// <param name="completedSynchronously">flag</param>
		public void SetAsCompleted(Exception exception, bool completedSynchronously)
		{
			this.m_exception = exception;
			int prevState = Interlocked.Exchange(ref this.m_CompletedState, completedSynchronously ? 1 : 2);
			if (prevState == 0)
			{
				if (this.m_AsyncWaitHandle != null)
				{
					this.m_AsyncWaitHandle.Set();
				}
				if (this.m_AsyncCallback != null)
				{
					this.m_AsyncCallback(this);
				}
			}
		}

		/// <summary>
		/// invoke end
		/// </summary>
		public void EndInvoke()
		{
			if (!this.IsCompleted)
			{
				this.AsyncWaitHandle.WaitOne();
				this.AsyncWaitHandle.Close();
				this.m_AsyncWaitHandle = null;
			}
			if (this.m_exception != null)
			{
				throw this.m_exception;
			}
		}

		/// <summary>
		/// invoke end
		/// </summary>
		/// <param name="timeoutMS">timeout value(ms)</param>
		public void EndInvoke(int timeoutMS)
		{
			if (!this.IsCompleted)
			{
				if (!this.AsyncWaitHandle.WaitOne(timeoutMS))
				{
					this.IsTimeOut = true;
				}
				this.AsyncWaitHandle.Close();
				this.m_AsyncWaitHandle = null;
			}
			if (this.m_exception != null)
			{
				throw this.m_exception;
			}
		}
	}
}
