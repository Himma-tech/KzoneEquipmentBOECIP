using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace KZONE.ConstantParameter
{
    public class UserTimer
    {
        private string _timerId;

        private Timer _timer;

        private ElapsedEventHandler _callBackHandler;

        private object state;

        private DateTime _createTime = DateTime.Now;

        private DateTime _runTime;

        public DateTime CreateTime
        {
            get
            {
                return this._createTime;
            }
            set
            {
                this._createTime = value;
            }
        }

        public DateTime RunTime
        {
            get
            {
                return this._runTime;
            }
            set
            {
                this._runTime = value;
            }
        }

        public double Interval
        {
            get
            {
                if (this._timer != null)
                {
                    return this._timer.Interval;
                }
                return -1.0;
            }
        }

        public string TimerId
        {
            get
            {
                return this._timerId;
            }
            set
            {
                this._timerId = value;
            }
        }

        public Timer Timer
        {
            get
            {
                return this._timer;
            }
            set
            {
                this._timer = value;
            }
        }

        public ElapsedEventHandler CallBackHandler
        {
            get
            {
                return this._callBackHandler;
            }
            set
            {
                this._callBackHandler = value;
            }
        }

        public object State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        public UserTimer(string timerid, Timer timer, ElapsedEventHandler handler)
        {
            this._timer = timer;
            this._timerId = timerid;
            this._callBackHandler = handler;
            this._runTime = this._createTime.AddMilliseconds(timer.Interval);
        }

        public override string ToString()
        {
            return string.Format("Timer ID={0},Interval={1},StartTime={2},RunTime={3},CallBackHandler={4}", new object[]
			{
				this._timerId,
				this.Interval,
				this._createTime.ToString("HH:mm:ss.ffff"),
				this._runTime.ToString("HH:mm:ss.ffff"),
				this.CallBackHandler.Method
			});
        }
    }
}
