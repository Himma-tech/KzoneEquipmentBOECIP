using System;
using System.Runtime.Serialization;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// driver operation exception
	/// </summary>
	[Serializable]
	public class PLCDriverException : Exception
	{
		private const string codeName = "Code";

		private int m_code = 0;

		/// <summary>
		/// error code
		/// </summary>
		public int Code
		{
			get
			{
				return this.m_code;
			}
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public PLCDriverException()
		{
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="message">exception message</param>
		public PLCDriverException(string message) : base(message)
		{
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="message">exception message</param>
		/// <param name="code">error code</param>
		public PLCDriverException(int code, string message) : base(message)
		{
			this.m_code = code;
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="message">exception message</param>
		/// <param name="innerException">inner exception object</param>
		public PLCDriverException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="message">exception message</param>
		/// <param name="code">error code</param>
		/// <param name="innerException">inner exception object</param>
		public PLCDriverException(int code, string message, Exception innerException) : base(message, innerException)
		{
			this.m_code = code;
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="info">serialization info object</param>
		/// <param name="context">streaming context object</param>
		protected PLCDriverException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.m_code = (int)info.GetValue("Code", typeof(int));
		}

		/// <summary>
		/// override base GetObjectData method
		/// </summary>
		/// <param name="info">serialization info object</param>
		/// <param name="context">stream context object</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Code", this.m_code);
		}
	}
}
