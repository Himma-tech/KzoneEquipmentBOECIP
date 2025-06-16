using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;
using KZONE.Log;
using System.Collections;

namespace KZONE.EntityManager
{
    public class AlarmManager : EntityManager
    {
        /// <summary>
        /// 各機台的Alarm設定檔
        /// </summary>
        public Dictionary<string, Dictionary<string, AlarmEntityData>> eqpAlarms = new Dictionary<string, Dictionary<string, AlarmEntityData>>();
        /// <summary>
        /// 各機台目前正在發生的Alarm記錄
        /// </summary>
        public Dictionary<string, AlarmEntityFile> _occurAlarms = new Dictionary<string, AlarmEntityFile>();

        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.BIN;
        }

        protected override string GetSelectHQL()
        {
            return string.Format("from AlarmEntityData where SERVERNAME = '{0}'", BcServerName);
        }

        protected override Type GetTypeOfEntityData()
        {
            return typeof(AlarmEntityData);
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(AlarmEntityFile);
        }

        protected override EntityFile NewEntityFile(string Filename)
        {
            return null;
        }

        protected override void AfterInit(List<EntityData> entity_Data, List<EntityFile> entity_File)
        {
            // Reload DB
            try
            {

                Dictionary<string, AlarmEntityData> _alarmNode;

                foreach (EntityData ent in entity_Data)
                {
                    AlarmEntityData alarm = ent as AlarmEntityData;
                    if (eqpAlarms.ContainsKey(alarm.NODENO))
                    {
                        _alarmNode = eqpAlarms[alarm.NODENO];
                    }
                    else
                    {
                        _alarmNode = new Dictionary<string, AlarmEntityData>();
                        eqpAlarms.Add(alarm.NODENO, _alarmNode);
                    }

                    if (!_alarmNode.ContainsKey( alarm.ALARMID))
                    {
                        _alarmNode.Add(alarm.ALARMID, alarm);
                    }
                }

                // Reload History File
                foreach (EntityFile file in entity_File)
                {
                    try
                    {
                        AlarmEntityFile ef = file as AlarmEntityFile;
                        if (_occurAlarms.ContainsKey(ef.EQPNo))
                        {
                            _occurAlarms.Remove(ef.EQPNo);
                        }
                        _occurAlarms.Add(ef.EQPNo, ef);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Print(ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            Filenames.Add("*.bin");
        }

        /// <summary>
        /// 取得Alarm 設定檔
        /// </summary>
        public AlarmEntityData GetAlarmProfile(string eqpNo, string alarmID)
        {
            AlarmEntityData ret = null;
            try
            {
                if (eqpAlarms.ContainsKey(eqpNo))
                {
                    if (eqpAlarms[eqpNo].ContainsKey(alarmID))
                    {
                        ret = eqpAlarms[eqpNo][alarmID];
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        /// <summary>
        /// get  Occur Alarm
        /// </summary>
        /// <param name="eqpNo"></param>
        /// <param name="alarmCode"></param>
        /// <returns></returns>
        public HappeningAlarm GetOccurAlarm(string eqpNo, string alarmCode)
        {
            HappeningAlarm ret = null;
            try
            {
                if (_occurAlarms.ContainsKey(eqpNo))
                {
                    AlarmEntityFile nodeAlarm = _occurAlarms[eqpNo];
                    if (nodeAlarm.HappingAlarms.ContainsKey(alarmCode))
                    {
                        ret = nodeAlarm.HappingAlarms[alarmCode];
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        public AlarmEntityFile GetEQPAlarm(string eqpNo)
        {
            AlarmEntityFile ret = null;
            try
            {
                if (_occurAlarms.ContainsKey(eqpNo))
                {
                    ret = _occurAlarms[eqpNo];
                }
                return ret;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }
        
        /// <summary>
        /// 取得目前正在發生的Alarm或Warning
        /// </summary>
        public HappeningAlarm GetOccurAlarm(string eqpNo, string unitNo, string alarmCode)
        {
            HappeningAlarm ret = null;
            try
            {
                if (_occurAlarms.ContainsKey(eqpNo))
                {
                    AlarmEntityFile eqpAlarm = _occurAlarms[eqpNo];
                    if (eqpAlarm.HappingAlarms.ContainsKey(alarmCode))
                    {
                        HappeningAlarm obj = eqpAlarm.HappingAlarms[alarmCode];
                        if (obj.Alarm.UNITNO.Equals(unitNo))
                        {
                            ret = obj;
                        }
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        /// <summary>
        /// 新增目前正在發生的Alarm
        /// </summary>
        public bool AddOccurAlarm(AlarmEntityData _entity)
        {
            try
            {
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(_entity.NODENO);
                if (eqp == null)
                {
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Can't find the Equipment No [{0}]", _entity.NODENO));
                    return false;
                }

                AlarmEntityFile his;
                if (_occurAlarms.ContainsKey(_entity.NODENO))
                {
                    his = _occurAlarms[_entity.NODENO];
                }
                else
                {
                    his = new AlarmEntityFile(eqp.Data.NODENO, eqp.Data.NODEID);
                    _occurAlarms.Add(his.EQPNo, his);
                }

                if (his.HappingAlarms.ContainsKey(_entity.ALARMID))
                {
                    his.HappingAlarms.Remove(_entity.ALARMID);
                }
                his.HappingAlarms.Add(_entity.ALARMID, new HappeningAlarm(_entity, DateTime.Now));
                eqp = null;
                EnqueueSave(his);
                return true;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }

        /// <summary>
        /// 刪除目前正在發生的Alarm
        /// </summary>
        public bool DeleteOccurAlarm(AlarmEntityData _entity)
        {
            try
            {
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(_entity.NODENO);
                if (eqp == null)
                {
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Can't find the Equipment No [{0}]", _entity.NODENO));
                    return false;
                }
                eqp = null;
                // 找不到就跳出
                if (!_occurAlarms.ContainsKey(_entity.NODENO)) return true;

                AlarmEntityFile his = _occurAlarms[_entity.NODENO];

                if (his.HappingAlarms.ContainsKey(_entity.ALARMID))
                {
                    his.HappingAlarms.Remove(_entity.ALARMID);
                }
                EnqueueSave(his);
                return true;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }

        /// <summary>
        /// 清除所有機台正在發生的Alarm
        /// </summary>
        public void ClearOccurAlarm()
        {
            try
            {
                foreach (AlarmEntityFile his in _occurAlarms.Values)
                {
                    his.HappingAlarms.Clear();
                    EnqueueSave(his);
                }
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// 清除指定機台正在發生的Alarm
        /// </summary>
        public void ClearOccurAlarm(string eqpNo)
        {
            try
            {
                if (_occurAlarms.ContainsKey(eqpNo))
                {
                    AlarmEntityFile his = _occurAlarms[eqpNo];

                    his.HappingAlarms.Clear();
                    EnqueueSave(his);
                }
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// 檢查指定機台是否有Alarm 發生
        /// </summary>
        public bool HasAlarm(string eqpNo, string alarmID)
        {
            try
            {
                if (!_occurAlarms.ContainsKey(eqpNo)) return false;
                if (!_occurAlarms[eqpNo].HappingAlarms.ContainsKey(alarmID)) return false;

                return true;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }

        public System.Data.DataTable GetDataTable()
        {
            try
            {
                DataTable dt = new DataTable();
                AlarmEntityData data = new AlarmEntityData();
                DataTableHelp.DataTableAppendColumn(data, dt);

                Dictionary<string, Dictionary<string, AlarmEntityData>>.Enumerator enumerator = eqpAlarms.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Dictionary<string, AlarmEntityData> dic = enumerator.Current.Value;
                    Dictionary<string, AlarmEntityData>.Enumerator en = dic.GetEnumerator();
                    while (en.MoveNext())
                    {
                        DataRow dr = dt.NewRow();
                        DataTableHelp.DataRowAssignValue(en.Current.Value, dr);
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
           
        }

        /// <summary>
        /// Reload All  Alarm 
        /// 20150321 Tom
        /// </summary>
        public void ReloadAll()
        {
            try
            {
                IList alarmEntitys= HibernateAdapter.GetObjectByQuery(GetSelectHQL());

                Dictionary<string, AlarmEntityData> _alarmNode;
                Dictionary<string, Dictionary<string, AlarmEntityData>> entitys = new Dictionary<string, Dictionary<string, AlarmEntityData>>();

                if (alarmEntitys != null)
                {
                    foreach (AlarmEntityData entity in alarmEntitys)
                    {
                        if (!entitys.ContainsKey(entity.NODENO))
                        {
                            _alarmNode = new Dictionary<string, AlarmEntityData>();
                            entitys.Add(entity.NODENO, _alarmNode);
                        }
                        entitys[entity.NODENO].Add(entity.UNITNO + "_" + entity.ALARMID, entity);
                    }
                }
                if (entitys.Count > 0)
                {
                    lock (eqpAlarms)
                        eqpAlarms = entitys;
                    Log.NLogManager.Logger.LogInfoWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Reload Alarm Success.");
                }

            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        /// <summary>
        /// 根据Equipment No Reload Alarm Profile
        /// </summary>
        /// <param name="eqpNo"></param>
        public void Reload(string eqpNo) {
            try {
                IList alarmEntitys = HibernateAdapter.GetObjectByQuery(string.Format("from AlarmEntityData where SERVERNAME = '{0}' and NODENO= '{1}'", BcServerName,eqpNo));

                Dictionary<string, AlarmEntityData> alarmNode=new Dictionary<string, AlarmEntityData>();
                
                if (alarmEntitys != null) {
                    foreach (AlarmEntityData entity in alarmEntitys) {
                        alarmNode.Add( entity.ALARMID, entity);
                    }

                }
                if (alarmNode.Count > 0) {
                    lock (eqpAlarms) {
                        if (eqpAlarms.ContainsKey(eqpNo)) {
                            eqpAlarms.Remove(eqpNo);
                        }
                        eqpAlarms.Add(eqpNo, alarmNode);
                    }
                    Log.NLogManager.Logger.LogInfoWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Reload Alarm Success.");
                }

            } catch (System.Exception ex) {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        public void SaveAlarmHistory(AlarmHistory history)
        {
            this.InsertHistory(history);
 
        }

        public void SaveAlarmHistory(List<AlarmHistory> historys)
        {
            this.InsertHistory(historys.ToArray());
        }
    }
}
