using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;
using KZONE.Log;
using System.Collections;
using KZONE.DB;
using KZONE.Work;


namespace KZONE.EntityManager
{
    public class QtimeManager: IDataSource
    {
        //Qtime 的設定檔 Qtime Spec
        private Dictionary<string, QtimeDefEntityData> _entities = new Dictionary<string, QtimeDefEntityData>();

        protected string GetSelectHQL()
        {
            return string.Format("from QtimeDefEntityData where SERVERNAME = '{0}'", Workbench.ServerName);
        }

        /// <summary>
        /// NLog Logger Name
        /// </summary>
        public string LoggerName { get; set; }

        public HibernateAdapter HibernateAdapter { get; set; }
        /// <summary>
        /// 直接从DB中取得资料
        /// </summary>
        /// <param name="namelist">Entity 的属性名称</param>
        /// <param name="valuelist">Entity 的属性的值</param>
        /// <returns></returns>
        public IList<QtimeDefEntityData> Find(string[] namelist, object[] valuelist)
        {
            try
            {
                IList<QtimeDefEntityData> list = new List<QtimeDefEntityData>();
                if (this.HibernateAdapter != null)
                {
                    IList list2 = HibernateAdapter.GetObject_AND(typeof(QtimeDefEntityData), namelist, valuelist, null, null);

                    if (list2 == null)
                    {
                        return list;
                    }
                    foreach (QtimeDefEntityData local in list2)
                    {
                        list.Add(local);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return null;
        }

        public void Init() {
            _entities = Reload();

        }

        /// <summary>
        /// Reload  Entity Data from DB 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, QtimeDefEntityData> Reload() {
            try{
                string hql = string.Format("from QtimeDefEntityData where SERVERNAME = '{0}' order by QTIMEID ", Workbench.ServerName);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                Dictionary<string, QtimeDefEntityData> entities = new Dictionary<string, QtimeDefEntityData>();
                if (list != null) {
                    foreach (QtimeDefEntityData qtime in list) {
                        if (entities.ContainsKey(qtime.QTIMEID)) {
                            entities.Remove(qtime.QTIMEID);
                        }
                        entities.Add(qtime.QTIMEID, qtime);
                    }
                }
                return entities;
            }catch (System.Exception ex){
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                throw ;
            }
        }

        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("QtimeManager");
            return entityNames;
        }

        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                QtimeDefEntityData data = new QtimeDefEntityData();
                //QtimeEntityFile file = new QtimeEntityFile();
                DataTableHelp.DataTableAppendColumn(data, dt);
                //DataTableHelp.DataTableAppendColumn(file, dt);

                foreach (QtimeDefEntityData qtime in _entities.Values)
                {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(qtime, dr);
                    //DataTableHelp.DataRowAssignValue(qtime.File, dr);
                    dt.Rows.Add(dr);
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
        /// 查詢Job的Qtime的結果，Q time過了就是NG
        /// </summary>
        /// <param name="cstseqno">Job.cstseqNo</param>
        /// <param name="jobseqno">Job.jobseqNo</param>
        /// <param name="Eqp">Equipment Object</param>
        /// <returns>2 結果:OK,NG</returns>
        //public string GetQtimeisOver(string cstseqno, string jobseqno,Equipment eqp)
        //{
        //    try
        //    {
        //        Job job = ObjectManager.JobManager.GetJob(cstseqno, jobseqno);
        //        //找不到Glass 直接回复OK
        //        if (job == null) 
        //        {
        //            string err = string.Format("[EQUIPMENT={0}] CST_SEQ_NO=[{1}] SLOT_SEQ_NO=[{2}]  WIP IS NULL!!", eqp.Data.NODENO, cstseqno, jobseqno);
        //            Log.NLogManager.Logger.LogWarnWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", err);
        //            return "OK";
        //        }

        //        if (job.QtimeList == null)
        //        {
        //            string err = string.Format("[EQUIPMENT={0}] CST_SEQ_NO=[{1}] SLOT_SEQ_NO=[{2}]  QTime IS NULL!!Qtime Is Not Start!", eqp.Data.NODENO, cstseqno, jobseqno);
        //            Log.NLogManager.Logger.LogWarnWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", err);
        //            return "OK";
        //        }

        //        foreach (Qtime qtime in job.QtimeList)
        //        {
        //            if (qtime.EndEquipmentNo == eqp.Data.NODENO)
        //            {
        //                if (!qtime.Enable) 
        //                {
        //                    string err = string.Format("[EQUIPMENT={0}] CST_SEQ_NO=[{1}] SLOT_SEQ_NO=[{2}] GLASSID=[{3}] IS DISABLE", 
        //                                            eqp.Data.NODENO, cstseqno, jobseqno, job.JobId);
        //                    Log.NLogManager.Logger.LogWarnWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", err);
        //                    continue;
        //                }

        //                if (qtime.QtimeValue == "0") 
        //                {
        //                    string err = string.Format("[EQUIPMENT={0}] CST_SEQ_NO=[{1}] SLOT_SEQ_NO=[{2}] GLASSID=[{3}] QTime Value = '0' ,QTime IS DISABLE!!",
        //                                eqp.Data.NODENO, cstseqno, jobseqno, job.JobId);
        //                    Log.NLogManager.Logger.LogWarnWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", err);
        //                    continue;
        //                }

        //                if (!qtime.StartQTimeFlag)
        //                {
        //                    string err = string.Format("[EQUIPMENT={0}] CST_SEQ_NO=[{1}] SLOT_SEQ_NO=[{2}] GLASSID=[{3}] START_NODE_ID=[{4}] START_EVENT=[{5}] IS NOT TIRGGER!!", 
        //                            eqp.Data.NODENO, cstseqno, jobseqno, job.JobId, eqp.Data.NODEID, qtime.StartEvent);
        //                    Log.NLogManager.Logger.LogWarnWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", err);
        //                    continue;
        //                }
                       
        //                if (JudgeQTimeOver(job, qtime.QtimeValue, qtime))
        //                {
        //                    qtime.OverQTimeFlag = true;
        //                    string err = string.Format("[EQUIPMENT={0}] CST_SEQ_NO=[{1}] SLOT_SEQ_NO=[{2}] GLASSID=[{3}] NG_QTIME_VLAUE=[{4}] START_DATETIME=[{5}] IS TIME UP!!", 
        //                            eqp.Data.NODENO, cstseqno, jobseqno, job.JobId, qtime.QtimeValue, qtime.StartQTime);
        //                    Log.NLogManager.Logger.LogWarnWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", err);
        //                }
        //                else
        //                {
        //                    string info = string.Format("[EQUIPMENT={0}] CST_SEQ_NO=[{1}] SLOT_SEQ_NO=[{2}] GLASSID=[{3}] NG_QTIME_VLAUE=[{4}] START_DATETIME=[{5}] IS NOT OVER NG_QTIME", 
        //                        eqp.Data.NODENO, cstseqno, jobseqno, job.JobId, qtime.QtimeValue, qtime.StartQTime);
        //                    Log.NLogManager.Logger.LogInfoWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", info);
        //                }

        //                if (qtime.OverQTimeFlag)
        //                    return "NG";
        //                else
        //                {
        //                    return "OK";
        //                }
        //            }
        //            else
        //                continue;
        //        }
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
        //        return "OK";
        //    }
        //}

        /// <summary>
        /// Watson Add 20150317 For Qtime Judge 以Qtime 或CF Rework Qtime
        /// </summary>
        /// <param name="cstseqno"></param>
        /// <param name="jobseqno"></param>
        /// <param name="qTimeVlaue"></param>
        /// <returns></returns>
        private bool JudgeQTimeOver(Job job, string qTimeVlaue, Qtime qtime)
        {
            //20170411 add for 如果QTime End Time尚未更新(Recive Event尚未上報) 此時已目前時間來做為End Time
            //TimeSpan qtimesec = (qtime.EndQTime - qtime.StartQTime);
            TimeSpan qtimesec;
            string tmpLog = string.Empty;

            if (qtime.EndQTime == DateTime.MinValue)
            {
                qtimesec = (DateTime.Now - qtime.StartQTime);

                tmpLog = string.Format("[EQUIPMENT={0}] CST_SEQ_NO=[{1}] JOB_SEQ_NO=[{2}] GLASSID=[{3}] QTimeID=[{4}] EndTime not Exist, Use current DataTime Judge QTimeOver Result!", 
                                       qtime.EndEquipmentNo, job.CassetteSequenceNo, job.JobSequenceNo, job.JobId,
                                       qtime.QTimeID);
                Log.NLogManager.Logger.LogWarnWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", tmpLog);

            }
            else
            {

                qtimesec = (qtime.EndQTime - qtime.StartQTime);
            }


            int setvalue = 0;
            if (!int.TryParse(qTimeVlaue, out setvalue))
            {
                string err = string.Format("CST_SEQ_NO=[{0}] SLOT_SEQ_NO=[{1}] GLASSID =[{2}] QTIME VALUE=[{3}] FORMAT ERROR!", 
                    job.CassetteSequenceNo, job.JobSequenceNo, job.JobId, qTimeVlaue);
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", err);
                return false;
            }

            if (setvalue == 0)
            {
                string err = string.Format("CST_SEQ_NO=[{0}] SLOT_SEQ_NO=[{1}] GLASSID=[{2}] QTIMEVLAUE=[{3}] TOTAL_SECOND=[{4}] STARTIME=[{5}] IS NOT START!",
                   job.CassetteSequenceNo, job.JobSequenceNo, job.JobId, qTimeVlaue, ((int)qtimesec.TotalSeconds).ToString(), qtime.StartQTime);
                Log.NLogManager.Logger.LogWarnWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", err);
                return false;
            }

            if ((int)qtimesec.TotalSeconds > setvalue)
                return true;

            return false;
        }

        //DB Q time Setting Move to Job Qtime 
        //public bool ValidateDBQTime(Job job)
        //{
        //    try
        //    {
        //        if (job == null)
        //            return false;

        //        job.QtimeList = new List<Qtime>();

        //        foreach (QtimeDefEntityData entitydata in _entities.Values)
        //        {
        //            Qtime qtime = new Qtime();
        //            qtime.LineName = entitydata.SERVERNAME;
        //            qtime.QTimeID = entitydata.QTIMEID;
        //            qtime.EndEvent = entitydata.ENDEVENTMSG;
        //            qtime.EndEquipmentId = entitydata.ENDNODEID;
        //            qtime.EndEquipmentNo = entitydata.ENDNODENO;
        //            qtime.EndUnits = entitydata.ENDNUNITID;
        //            qtime.RecipeID = entitydata.STARTNODERECIPEID;
        //            qtime.QtimeValue = (entitydata.SETTIMEVALUE).ToString();
        //            qtime.StartEvent = entitydata.STARTEVENTMSG;
        //            qtime.StartEquipmentId = entitydata.STARTNODEID;
        //            qtime.StartEquipmentNo = entitydata.STARTNODENO;
        //            qtime.StartUnits = entitydata.STARTNUNITID;
        //            qtime.RecipeID = entitydata.STARTNODERECIPEID;
        //            qtime.OverQTimeFlag = false;
        //            qtime.Enable = entitydata.ENABLED == "Y" ? true : false;
        //            job.QtimeList.Add(qtime);
        //        }
                
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
        //        return false;
        //    }
        //}

        //public void ReloadQTime()
        //{
        //    try
        //    {
        //        IList entiyDatas = HibernateAdapter.GetObjectByQuery(GetSelectHQL());

        //        Dictionary<string, QtimeDefEntityData> tempObject = new Dictionary<string, QtimeDefEntityData>();

        //        if (entiyDatas != null)    //add by sy.wu
        //        {
        //            foreach (QtimeDefEntityData obj in entiyDatas)
        //            {
        //                if (tempObject.ContainsKey(obj.QTIMEID))
        //                    tempObject.Remove(obj.QTIMEID);
        //                tempObject.Add(obj.QTIMEID, obj);
        //            }
        //            lock (_entities)
        //                _entities = tempObject;

        //            ResetJobQtime();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
        //        return;
        //    }
        //}

        //private void ResetJobQtime()
        //{
        //    try
        //    {
        //        foreach (Job job in ObjectManager.JobManager.GetJobs())
        //        {
        //            if (job.QtimeList != null)
        //            {
        //                #region Updata Job Qtime
        //                for (int i = job.QtimeList.Count - 1; i >= 0; i--)
        //                {
        //                    Qtime qtimec = job.QtimeList[i];
        //                    if (_entities.ContainsKey(qtimec.QTimeID))
        //                    {
        //                        //qtimec.CFRWQTIME = (_entities[qtimec.QTimeID].CFRWQTIME).ToString();
        //                        qtimec.EndEvent = _entities[qtimec.QTimeID].ENDEVENTMSG;
        //                        qtimec.EndEquipmentId = _entities[qtimec.QTimeID].ENDNODEID;
        //                        //20170331 modify 也要更新Qtime EQPNo
        //                        qtimec.EndEquipmentNo = _entities[qtimec.QTimeID].ENDNODENO;

        //                        qtimec.EndUnits = _entities[qtimec.QTimeID].ENDNUNITID;
        //                        qtimec.QtimeValue = (_entities[qtimec.QTimeID].SETTIMEVALUE).ToString();
        //                        qtimec.RecipeID = _entities[qtimec.QTimeID].STARTNODERECIPEID;
        //                        qtimec.StartEvent = _entities[qtimec.QTimeID].STARTEVENTMSG;
        //                        qtimec.StartEquipmentId = _entities[qtimec.QTimeID].STARTNODEID;
        //                        //20170331 modify 也要更新Qtime EQPNo
        //                        qtimec.StartEquipmentNo = _entities[qtimec.QTimeID].STARTNODENO;

        //                        qtimec.StartUnits = _entities[qtimec.QTimeID].STARTNUNITID;
        //                        qtimec.Enable = _entities[qtimec.QTimeID].ENABLED == "Y" ? true : false;
        //                        //不得更新
        //                        //qtimec.STARTQTIMEFLAG
        //                        //qtimec.CFRWQTIMEFLAG
        //                        //qtimec.OVERQTIMEFLAG

        //                        string str = string.Format("QTIME RELOAD  CST_SEQ_NO=[{0}], JOB_SEQ_NO=[{1}] GLASSID[{2}]  QTIMEID =[{3}] QTIME VALUE=[{4}] QTIME ENABLE=[{5}].", job.CassetteSequenceNo,
        //                            job.JobSequenceNo, job.JobId, qtimec.QTimeID, qtimec.QtimeValue, qtimec.Enable);
        //                        Log.NLogManager.Logger.LogInfoWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", str);
        //                    }
        //                    else
        //                    {
        //                        job.QtimeList.RemoveAt(i);

        //                        string str = string.Format("QTIME DELETE  CST_SEQ_NO=[{0}], JOB_SEQ_NO=[{1}] GLASSID[{2}]  QTIMEID =[{3}] QTIME VALUE=[{4}] QTIME ENABLE=[{5}].", job.CassetteSequenceNo,
        //                             job.JobSequenceNo, job.JobId, qtimec.QTimeID, qtimec.QtimeValue, qtimec.Enable);
        //                        Log.NLogManager.Logger.LogInfoWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", str);
        //                    }
        //                }
        //                #endregion
        //                #region DB Add
        //                foreach (string dbQtimeID in _entities.Keys)
        //                {
        //                    bool bolAlread = false;
        //                    foreach (Qtime qtimec in job.QtimeList)
        //                    {
        //                        if (qtimec.QTimeID == dbQtimeID)
        //                        {
        //                            bolAlread = true;
        //                            break;
        //                        }
        //                    }
        //                    if (bolAlread)
        //                        continue;
        //                    //Add 
        //                    Qtime nqtimec = new Qtime();
        //                    if (_entities[dbQtimeID].LINEID != Workbench.ServerName)
        //                        continue;
        //                    nqtimec.LineName = _entities[dbQtimeID].LINEID;
        //                    nqtimec.QTimeID = _entities[dbQtimeID].QTIMEID;
        //                    //nqtimec.CFRWQTIME = (_entities[dbQtimeID].CFRWQTIME).ToString();
        //                    nqtimec.EndEvent = _entities[dbQtimeID].ENDEVENTMSG;
        //                    nqtimec.EndEquipmentId = _entities[dbQtimeID].ENDNODEID;
        //                    nqtimec.EndUnits = _entities[dbQtimeID].ENDNUNITID;
        //                    nqtimec.QtimeValue = (_entities[dbQtimeID].SETTIMEVALUE).ToString();
        //                    nqtimec.RecipeID = _entities[dbQtimeID].STARTNODERECIPEID;
        //                    nqtimec.StartEvent = _entities[dbQtimeID].STARTEVENTMSG;
        //                    nqtimec.StartEquipmentId = _entities[dbQtimeID].STARTNODEID;
        //                    nqtimec.StartUnits = _entities[dbQtimeID].STARTNUNITID;
        //                    nqtimec.OverQTimeFlag = false;
        //                    nqtimec.Enable = _entities[dbQtimeID].ENABLED == "Y" ? true : false;
        //                    job.QtimeList.Add(nqtimec);

        //                    string str = string.Format("QTIME ADDING  CST_SEQ_NO=[{0}], JOB_SEQ_NO=[{1}] GLASSID[{2}]  QTIMEID =[{3}] QTIME VALUE=[{4}] QTIME ENABLE=[{4}].", job.CassetteSequenceNo,
        //                        job.JobSequenceNo, job.JobId, nqtimec.QTimeID, nqtimec.QtimeValue, nqtimec.Enable);
        //                    Log.NLogManager.Logger.LogInfoWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", str);
        //                }
        //                #endregion

        //                //EnqueueSave(job);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }
        //}
    }
}
