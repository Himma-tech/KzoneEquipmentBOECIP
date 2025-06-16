using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    public class eALARM_STATE
    {
        public const string CLEAR = "CLEAR";

        public const string SET = "SET";
    }

  
    /// <summary>
    /// 正在發生的Alarm資料
    /// </summary>
    [Serializable]
    public class HappeningAlarm
    {
        private AlarmEntityData _alarm = null;
        private DateTime _occurDateTime = DateTime.Now;

        public HappeningAlarm(AlarmEntityData alarm, DateTime occurTime)
        {
            _alarm = alarm;
            _occurDateTime = occurTime;
        }

        public AlarmEntityData Alarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }
        public DateTime OccurDateTime
        {
            get { return _occurDateTime; }
            set { _occurDateTime = value; }
        }
    }
    /// <summary>
    /// 機台正在發生的Alarm記錄
    /// </summary>
    [Serializable]
    public class AlarmEntityFile : EntityFile
    {
        private string _eqpNo = string.Empty;
        private string _eqpID = string.Empty;

        public string EQPNo
        {
            get { return _eqpNo; }
            set { _eqpNo = value; }
        }

        public string EQPID
        {
            get { return _eqpID; }
            set 
            {
                _eqpID = value;
                SetFilename(_eqpID + ".bin");
            }
        }

        public AlarmEntityFile() { }

        public AlarmEntityFile(string eqpNo, string eqpID)
        {
            EQPNo = eqpNo;
            EQPID = eqpID;
        }
        public Dictionary<string, HappeningAlarm> HappingAlarms = new Dictionary<string, HappeningAlarm>();
    }
}
