using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// asyncresult
	/// </summary>
	/// <typeparam name="TResult">result</typeparam>
	public class AsyncResult<TResult> : AsyncResultNoResult
	{
		private TResult _result = default(TResult);

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="asyncCallback">callback function</param>
		/// <param name="state">state object</param>
		public AsyncResult(AsyncCallback asyncCallback, object state) : base(asyncCallback, state)
		{
		}

		/// <summary>
		/// set aysnc result completed
		/// </summary>
		/// <param name="result">result</param>
		/// <param name="completedSynchronously">flag</param>
		public void SetAsCompleted(TResult result, bool completedSynchronously)
		{
			this._result = result;
			base.SetAsCompleted(null, completedSynchronously);
		}

		/// <summary>
		/// invoke end
		/// </summary>
		/// <returns>result</returns>
		public new TResult EndInvoke()
		{
			base.EndInvoke();
			return this._result;
		}

		/// <summary>
		/// invoke end
		/// </summary>
		/// <param name="timeoutMS">timeout value(ms)</param>
		/// <returns>result</returns>
		public new TResult EndInvoke(int timeoutMS)
		{
			base.EndInvoke(timeoutMS);
			return this._result;
		}
	}
}
