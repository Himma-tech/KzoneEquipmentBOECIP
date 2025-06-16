using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Timers;
using KZONE.Log;
namespace KZONE.ConstantParameter
{
    public class TimerManager
    {
        private static ILogManager logger = NLogManager.Logger;

        private IDictionary<string, UserTimer> _timers;

        public TimerManager()
        {
            this._timers = new Dictionary<string, UserTimer>();
        }

        public void Init()
        {
        }

        public bool CreateTimer(string timerId, bool aAutoReset, double aInterval, ElapsedEventHandler aCallbackHandler, object state = null)
        {
            try
            {
                bool result;
                if (string.IsNullOrEmpty(timerId))
                {
                    result = false;
                    return result;
                }
                if (this._timers.ContainsKey(timerId))
                {
                    result = false;
                    return result;
                }
                if (aInterval == 0.0)
                {
                    result = false;
                    return result;
                }
                Timer timer = new Timer(aInterval);
                timer.AutoReset = aAutoReset;
                timer.Elapsed += new ElapsedEventHandler(this.OnTimedEvent);
                lock (this._timers)
                {
                    this._timers[timerId] = new UserTimer(timerId, timer, aCallbackHandler)
                    {
                        State = state
                    };
                }
                timer.Start();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                TimerManager.logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return false;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                lock (this._timers)
                {
                    foreach (UserTimer timer in this._timers.Values)
                    {
                        if (timer != null && timer.Timer.Equals(source))
                        {
                            if (!timer.Timer.AutoReset)
                            {
                                timer.Timer.Stop();
                                timer.Timer.Dispose();
                                this._timers.Remove(timer.TimerId);
                            }
                            if (timer.CallBackHandler != null)
                            {
                                timer.CallBackHandler(timer, e);
                                break;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TimerManager.logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void TerminateTimer(string aTimerID)
        {
            try
            {
                if (!string.IsNullOrEmpty(aTimerID))
                {
                    lock (this._timers)
                    {
                        if (this._timers.ContainsKey(aTimerID))
                        {
                            UserTimer timer = this._timers[aTimerID];
                            if (timer != null)
                            {
                                timer.Timer.Stop();
                                timer.Timer.Dispose();
                                this._timers.Remove(aTimerID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TimerManager.logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public bool IsAliveTimer(string aTimerID)
        {
            try
            {
                if (string.IsNullOrEmpty(aTimerID))
                {
                    bool result = false;
                    return result;
                }
                lock (this._timers)
                {
                    bool result = this._timers.ContainsKey(aTimerID);
                    return result;
                }
            }
            catch (Exception ex)
            {
                TimerManager.logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return false;
        }

        public UserTimer GetAliveTimer(string aTimerID)
        {
            try
            {
                lock (this._timers)
                {
                    if (this._timers.ContainsKey(aTimerID))
                    {
                        return this._timers[aTimerID];
                    }
                }
            }
            catch (Exception ex)
            {
                TimerManager.logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return null;
        }
    }
}
