using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Xml;
using KZONE.Entity;
using KZONE.Log;
using KZONE.ConstantParameter;
using KZONE.MessageManager;
using KZONE.Work;
using System.IO;

namespace KZONE.EntityManager
{
    public class JobManager : EntityManager, IDataSource
    {
        private Dictionary<string, Job> _entities = new Dictionary<string, Job>();
        //private Dictionary<string, GroupNo> _groupNoData = new Dictionary<string, GroupNo>();
        private int _jobTimeOver = 7; //Defaule 7 

        private string _trashPath = @"D:\KZONELOG\{ServerName}\Job\";

        /// <summary>
        /// get /set Job Trash Path
        /// </summary>
        public string TrashPath
        {
            get { return _trashPath; }
            set { _trashPath = value; }
        }
        /// <summary>
        /// get /Set Job Time over Day;
        /// </summary>
        public int JobTimeOver 
        {
            get { return _jobTimeOver; }
            set { _jobTimeOver = value; }
        }

        

        private Timer _timer;//定时删除Job的Timer 默认值24小时执行一次

        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.BIN;
        }

        protected override string GetSelectHQL()
        {
            return string.Empty;
        }

        protected override Type GetTypeOfEntityData()
        {
            return null;
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            Filenames.Add("*.bin");
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(Job);
        }

        protected override EntityFile NewEntityFile(string Filename)
        {

            string[] substrs = Filename.Split('.')[0].Split('_');
            if (substrs != null && substrs.Length == 2)
            {
                int cstSeqNo = Convert.ToInt32(substrs[0]);
                int slotNo = Convert.ToInt32(substrs[1]);
                return new Job(cstSeqNo, slotNo);
            }
            return null;
        }

        protected override void AfterInit(List<EntityData> entityDatas, List<EntityFile> entityFiles)
        {
            foreach (EntityFile entity_file in entityFiles)
            {
                try
                {
                    Job job_entity_file = entity_file as Job;
                    if (job_entity_file != null)
                    {
                        if (!_entities.ContainsKey(job_entity_file.JobKey))
                        {
                            _entities.Add(job_entity_file.JobKey, job_entity_file);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                }

            }

            _timer = new Timer(24 * 3600 * 1000);//24*3600 * 1000
            //_timer = new Timer(60000);
            _timer.AutoReset = true;
            _timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            _timer.Start();
            TrashPath = TrashPath.Replace("{ServerName}", Workbench.ServerName);
            if (TrashPath[TrashPath.Length - 1] != '\\')
                TrashPath += "\\";
        }

        /// <summary>
        ///固定时间删除Job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {

                if (_entities != null && _entities.Count > 0)
                {
                    ParameterManager para = Workbench.Instance.GetObject("ParameterManager") as ParameterManager;

                    IList<Job> jobs = null;
                    //Product Glass Delete
                    lock (_entities) {
                        DateTime now = DateTime.Now;
                        jobs = _entities.Values.Where(job => job.LastUpdateTime.AddDays(para["ProdcutOverTimer"].GetInteger()).CompareTo(now) < 0
                                                        ).ToList();
                    }
                    if (jobs != null) {
                        this.DeleteJobs(jobs);
                    }

                    //Dummy Glass Delete 
                    //lock (_entities) {
                    //    DateTime now = DateTime.Now;
                    //    jobs = _entities.Values.Where(job => job.LastUpdateTime.AddDays(para["DummyOverTimer"].GetInteger()).CompareTo(now) < 0
                    //                                    && (job.JobType == eJobType.ITODummy ||job.JobType == eJobType.Coater1Dummy || job.JobType == eJobType.Coater2Dummy || job.JobType == eJobType.Dummy)).ToList();
                    //}
                    //if (jobs != null) {
                    //    this.DeleteJobs(jobs);
                    //}

                    ////UV Mask 保留60天以上再砍
                    //lock (_entities) {
                    //    DateTime now = DateTime.Now;
                    //    jobs = _entities.Values.Where(job => job.LastUpdateTime.AddDays(para["UVMaskOverTimer"].GetInteger()).CompareTo(now) < 0
                    //                                    && job.JobType == eJobType.UV).ToList();
                    //}
                    //if (jobs != null) {
                    //    this.DeleteJobs(jobs);
                    //}
                    ////不明来历的Glass 7 天Delete  tom 2017-5-2
                    //lock (_entities) {
                    //    DateTime now = DateTime.Now;
                    //    jobs = _entities.Values.Where(job => job.LastUpdateTime.AddDays(para["ProdcutOverTimer"].GetInteger()).CompareTo(now) < 0
                    //                                    && job.JobType == eJobType.Unknown).ToList();
                    //}

                    if (jobs != null) {
                        this.DeleteJobs(jobs);
                    }
                }

            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// 將EntityFile放入Queue, 由Thread存檔
        /// </summary>
        /// <param name="file">JobEntityFile(job.File)</param>
        public override void EnqueueSave(EntityFile file)
        {
            if (file is Job)
            {
                Job job = file as Job;

                if (job.CassetteSequenceNo == 0)
                    return;
                if (job.JobSequenceNo == 0)
                    return;

                lock (job)
                {
                    job.LastUpdateTime = DateTime.Now;
                }

                string job_no = string.Format("{0}_{1}", job.CassetteSequenceNo, job.JobSequenceNo);
                string fname = string.Format("{0}.bin", job_no);
                file.SetFilename(fname);
                base.EnqueueSave(file);
            }
        }

        /// <summary>
        /// Add job by list
        /// </summary>
        /// <param name="jobs"></param>
        public void AddJobs(IList<Job> jobs)
        {
            if (jobs == null) return;

            foreach (Job j in jobs)
            {
                try
                {
                    if (_entities.ContainsKey(j.JobKey))
                    {
                        _entities.Remove(j.JobKey);
                    }
                    _entities.Add(j.JobKey, j);
                    EnqueueSave(j);
                }
                catch (Exception ex)
                {
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                }
            }
        }
        /// <summary>
        /// 將Job加入Dictionary
        /// </summary>
        /// <param name="Job">若Job Key有重複, 會throw exception</param>
        public void AddJob(Job job)
        {
            if (job.CassetteSequenceNo == 0)
                return;
            if (job.JobSequenceNo == 0)
                return;

            lock (_entities)
            {
                if (_entities.ContainsKey(job.JobKey))
                {
                    _entities.Remove(job.JobKey);
                    DeleteJob(job);
                }
                _entities.Add(job.JobKey, job);
                EnqueueSave(job);
            }
        }
        /// <summary>
        /// 取得line Job 以List 方式傳回
        /// </summary>
        /// <returns>Job List</returns>
        public IList<Job> GetJobs()
        {
            IList<Job> ret = null;
            lock (_entities)
            {
                ret = _entities.Values.ToList();
            }
            return ret;
        }

        /// <summary>
        /// delete Job ，此法在定时执行清除过期时使用，在平时请使用MoveJob，以提高效能
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public bool DeleteJob(Job job)
        {
            try
            {
                if (_entities.ContainsKey(job.JobKey))
                {
                    lock (_entities)
                    {
                        _entities.Remove(job.JobKey);
                    }
                    lock (job)
                    {
                        job.WriteFlag = false;
                    }

                    EnqueueSave(job);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }

        /// <summary>
        /// delete job by List此法在定时执行清除过期时使用，在平时请使用MoveJob，以提高效能
        /// </summary>
        /// <param name="jobs"></param>
        public void DeleteJobs(IList<Job> jobs)
        {
            if (jobs == null) return;
            foreach (Job j in jobs)
            {
                DeleteJob(j);
            }
        }

        /// <summary>
        /// 将Job 移动到垃圾桶中等待删除
        /// </summary>
        /// <param name="job"></param>
        public void MoveJob(Job job)
        {
            try{
                if (_entities.ContainsKey(job.JobKey))
                {
                    string fileName = string.Format("{1}.{2}",job.JobKey ,GetFileExtension());
                    
                    if (!Directory.Exists(this.DataFilePath))
                    {
                        return ;
                    }
                    
                    string descFile = Path.Combine(TrashPath, DateTime.Now.ToString("yyyyMMdd"), job.JobKey.Split('_')[0]);
                    if (!Directory.Exists(descFile))
                    {
                        Directory.CreateDirectory(descFile);
                    }
                    string file = Path.Combine(DataFilePath, fileName);
                    if (File.Exists(file))
                    {
                        File.Move(file, descFile + "\\" + fileName);
                    }
                    lock (_entities)
                    {
                        _entities.Remove(job.JobKey);
                    }
                   
                }
            
            }catch (System.Exception ex){
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// 将一些Job 移动到垃圾桶中等待删除
        /// </summary>
        /// <param name="jobs"></param>
        public void MoveJobs(IList<Job> jobs)
        {
            if (jobs != null)
            {
                foreach (Job job in jobs){
                    MoveJob(job);
                }
            }
        }

        /// <summary>
        /// 取得line Job 以List 方式傳回
        /// </summary>
        /// <returns>Job List</returns>
        public IList<Job> GetJobs(string cstSeqNo)
        {
            IList<Job> ret = null;
            lock (_entities)
            {
                ret = _entities.Values.Where(w => w.CassetteSequenceNo == int.Parse(cstSeqNo)).ToList();
            }
            return ret;
        }

        /// <summary>
        /// 以Job No Work No (Cassette Sequence No + Job Sequence No)取得Job(WIP)
        /// </summary>
        /// <param name="cassetteSequenceNo">Cassette SEQ No</param>
        /// <param name="jobSequenceNo">Job SEQ No</param>
        /// <returns>Job</returns>
        public Job GetJob(string cassetteSequenceNo, string jobSequenceNo)
        {
            string job_no = string.Format("{0}_{1}", cassetteSequenceNo, jobSequenceNo);
            Job ret = null;
            lock (_entities)
            {
                if (_entities.ContainsKey(job_no))
                {
                    ret = _entities[job_no];
                }
            }
            return ret;
        }

        /// <summary>
        /// 以Job No Work No (Cassette Sequence No + Job Sequence No)取得Job(WIP)
        /// </summary>
        /// <param name="cassetteSequenceNo">Cassette SEQ No</param>
        /// <param name="jobSequenceNo">Job SEQ No</param>
        /// <returns>Job</returns>
        public Job GetJobByJobKey(string jobKey)
        {
            Job ret = null;
            lock (_entities)
            {
                if (_entities.ContainsKey(jobKey))
                {
                    ret = _entities[jobKey];
                }
            }
            return ret;
        }

        /// <summary>
        /// 以Glass or Chip or Mask or Cut ID來取得WIP(Job)
        /// </summary>
        /// <param name="GlassChipMaskBlockID">ID</param>
        /// <returns>Job</returns>
        public Job GetJob(string jobID)
        {
            try
            {
                lock (_entities)
                {
                    //20150417 cy:先以wip的create time做排序,避免取到舊資料
                    //Job ret = _entities.Values.FirstOrDefault(w => w.GlassChipMaskBlockID == jobID);
                    Job ret = _entities.Values.OrderByDescending(w => w.CreateTime).FirstOrDefault(j => j.JobId.Trim() == jobID.Trim()); //.Select(w => w.GlassChipMaskBlockID == jobID)
                    if (ret != null)
                    {
                        return ret;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                throw new Exception(string.Format("Job ID=[{0}] is already Exist", jobID));
            }
        }

        /// <summary>
        /// get BCS keep Job Count
        /// </summary>
        /// <returns></returns>
        public int GetJobCount()
        {
            try
            {
                return _entities.Count;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return 0;
            }
        }

        /// <summary>
        /// Remove job 
        /// </summary>
        /// <param name="eqpNo"></param>
        /// <param name="cstSeq"></param>
        /// <param name="slotNo"></param>
        public void RemoveJobByUI(string eqpNo = "", string cstSeq = "", string slotNo = "") {
            try {
                List<Job> jobs = GetJobs() as List<Job>;
                if (!string.IsNullOrEmpty(eqpNo) && jobs != null) {
                    jobs = jobs.Where(w => w.CurrentEQPNo == eqpNo && w.RemoveFlag == false).ToList();
                }
                if (!string.IsNullOrEmpty(cstSeq) && jobs != null) {
                    jobs = jobs.Where(w => w.CassetteSequenceNo ==int.Parse(cstSeq)).ToList();
                }
                if (!string.IsNullOrEmpty(slotNo) && jobs != null) {
                    jobs = jobs.Where(w => w.JobSequenceNo == int.Parse(slotNo)).ToList();
                }
                if (jobs != null) {
                    foreach (Job job in jobs) {
                        job.RemoveFlag = true;
                        NLogManager.Logger.LogWarnWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Remove Job wiht BCS UI CST_SEQNO=[{0}] JOB_SEQNO=[{1}] GLASS_ID=[{2}]", job.CassetteSequenceNo, job.JobSequenceNo, job.JobId));
                    }
                }
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        /// <summary>
        ///  Recovery job  
        /// </summary>
        /// <param name="eqpNo"></param>
        /// <param name="cstSeq"></param>
        /// <param name="slotNo"></param>
        public void RecoveryJobByUI(string eqpNo = "", string cstSeq = "", string slotNo = "") {

            try {
                List<Job> jobs = GetJobs() as List<Job>;
                if (!string.IsNullOrEmpty(eqpNo) && jobs != null) {
                    jobs = jobs.Where(w => w.CurrentEQPNo == eqpNo && w.RemoveFlag == false).ToList();
                }
                if (!string.IsNullOrEmpty(cstSeq) && jobs != null) {
                    jobs = jobs.Where(w => w.CassetteSequenceNo == int.Parse(cstSeq)).ToList();
                }
                if (!string.IsNullOrEmpty(slotNo) && jobs != null) {
                    jobs = jobs.Where(w => w.JobSequenceNo == int.Parse(slotNo)).ToList();
                }
                if (jobs != null) {
                    foreach (Job job in jobs) {
                        job.RemoveFlag = false;

                        NLogManager.Logger.LogWarnWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Remove Job wiht BCS UI CST_SEQNO=[{0}] JOB_SEQNO=[{1}] GLASS_ID=[{2}]", job.CassetteSequenceNo, job.JobSequenceNo, job.JobId));
                    }
                }
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite("LoggerName", this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        /// <summary>
        /// Get managerment Entity Name
        /// </summary>
        /// <returns></returns>
        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("JobManager");
            //entityNames.Add("GroupNoManager");
            return entityNames;
        }

        /// <summary>
        /// get all entity by entity name
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDataTable(string entityName)
        {
            switch (entityName)
            {
                case "JobManager":
                    return GetJobManagerDataTable();
                //case "GroupNoManager":
                //    return GetGroupNoDataTable();
                default:
                    return null;

            }

        }

        //private DataTable GetGroupNoDataTable()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        GroupNo file = new GroupNo();
        //        DataTableHelp.DataTableAppendColumn(file, dt);

        //        foreach (GroupNo product in _groupNoData.Values)
        //        {
        //            DataRow dr = dt.NewRow();

        //            DataTableHelp.DataRowAssignValue(product, dr);
        //            dt.Rows.Add(dr);
        //        }
        //        return dt;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
        //        return null;
        //    }
        //}

        private DataTable GetJobManagerDataTable() {
            try {
                DataTable dt = new DataTable();
                Job file = new Job();
                DataTableHelp.DataTableAppendColumn(file, dt);
                dt.Columns.Add("No");//增加一个序列号
                IList<Job> jobs = GetJobs();
                int i = 1;
                foreach (Job job in jobs) {
                    DataRow dr = dt.NewRow();
                    dr["No"] = i++;
                    DataTableHelp.DataRowAssignValue(job, dr);
                    dt.Rows.Add(dr);
                }
                return dt;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        public void SaveJobHistory(string readID,string eventName) {
            try {
                JobHistory his = new JobHistory();
                his.UPDATETIME = DateTime.Now;
              
                his.EVENTNAME = eventName;
              
                //his.VCRNO = "1";
                //his.VCRREADGLASSID = readID;

                this.InsertHistory(his);

            } catch (Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void SaveJobHistory(IList<Job> jobs, string eventName, string equipmentId, string equipmentNo, string unitNo, string unitId, string portNo, string slotNo) {
            try {
                IList<JobHistory> histoies = new List<JobHistory>();
                foreach (Job job in jobs) {
                    JobHistory his = new JobHistory();
                    his.UPDATETIME = DateTime.Now;
                   
                    his.EVENTNAME = eventName;
                   
                    //his.VCRNO = "1";
                    //his.VCRREADGLASSID = job.VCRJobID;
                  
                    
                    histoies.Add(his);
                }
                this.InsertHistory(histoies.ToArray<JobHistory>());

            } catch (Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void SaveJobHistory(Job job, string eventName, string equipmentId, string equipmentNo, string unitNo, string unitId, string portNo, string slotNo)
        {
            try
            {
              
                JobHistory his = new JobHistory();
                his.UPDATETIME = DateTime.Now; 
                his.EVENTNAME = eventName;
                his.CASSETTESEQNO = job.CassetteSequenceNo.ToString();
                his.CASSETTESLOTNO = job.IJobSequenceNo.ToString();
                his.NODENO = equipmentNo;
                his.UNITNO = unitNo;
                his.JOBID = job.JobId;

                his.GroupNumber = job.GroupNumber;
                his.GlassType = job.GlassType;
                his.GlassJudge = job.GlassJudge;
                his.ProcessSkipFlag = job.ProcessSkipFlag;
                his.LastGlassFlag = job.LastGlassFlag;
                his.CIMModeCreate = job.CIMModeCreate;
                his.SamplingFlag = job.SamplingFlag;
                his.Reserved = job.Reserved;
                his.InspectionJudgeResult = job.InspectionJudgeResult;
                his.InspectionReservationSignal = job.InspectionReservationSignal;
                his.ProcessReservationSignal = job.ProcessReservationSignal;
                his.TrackingDataHistory = job.TrackingDataHistory;
                his.EquipmentSpecialFlag = job.EquipmentSpecialFlag;
                his.GlassID = job.GlassID;
                his.SorterGrade = job.SorterGrade;
                his.GlassGrade = job.GlassGrade;
                his.FromPortNo = job.FromPortNo;
                his.TargetPortNo = job.TargetPortNo;
                his.TargetSlotNo = job.TargetSlotNo;
                his.TargetCassetteID = job.TargetCassetteID;
                his.Reserve = job.Reserve;
                his.PPID= job.PPID;

                this.InsertHistory(his);
            }

            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        /// <summary>
        /// Find Last Flag 
        /// </summary>
        /// <param name="reMoveCovejob"></param>
        /// <param name="jobcmd"></param>
        /// <param name="theNewLastJob"></param>
        /// <returns></returns>
        //public bool FindLastGlass(Job reMoveCovejob, eJobCommand jobcmd, out Job theNewLastJob) {
        //    try {
        //        theNewLastJob = null;
        //        Dictionary<int, Job> lastSecondSlotdic = new Dictionary<int, Job>();

        //        if (reMoveCovejob.LastGlassFlg != eBitResult.ON)
        //            return false;

        //        //Step.1: Save All Sampling Slot Glass
        //        foreach (Job job in ObjectManager.JobManager.GetJobs(reMoveCovejob.CassetteSequenceNo)) {
        //            int slotno = 0;
        //            if (job.RemoveFlag) //移走不用再加入考慮
        //                continue;
        //            if (job.SamplingFlag != eBitResult.ON)  //沒有抽的片 不需要考慮
        //                continue;
        //            if (!int.TryParse(job.SourceSlotNo, out slotno))//找不到slotno
        //                continue;
        //            if (job.JobId == reMoveCovejob.JobId)
        //                continue;

        //            if (!lastSecondSlotdic.ContainsKey(slotno))
        //                lastSecondSlotdic.Add(slotno, job);
        //        }

        //        //Step.2: Find out the New Last Glass
        //        if (jobcmd == eJobCommand.JOBREMOVE) {
        //            //Array  A1PHL 需要目前是从上往下抽片，所以需要算Last glass Flag 的方式不同 20170121 Tom
        //            if (ObjectManager.LineManager.GetLine().Data.FABTYPE == eFabType.ARRAY.ToString()) {
        //             ////   StageEntity stage = ObjectManager.RobotManager.GetStageByPortID(reMoveCovejob.SourcePortID);
        //             //   if (stage != null && stage.SLOTFETCHSEQ == "DESC") {
        //             //       theNewLastJob = (from d in lastSecondSlotdic orderby d.Key descending select d.Value).Last();
        //             //       return true;
        //             //   }
        //            }

        //            theNewLastJob = (from d in lastSecondSlotdic orderby d.Key ascending select d.Value).Last();
        //            return true;
        //        } else {
        //            int removSlotno, lastSlotno;
        //            if (int.TryParse(reMoveCovejob.SourceSlotNo, out removSlotno))//找不到slotno
        //            {
        //                theNewLastJob = lastSecondSlotdic.Values.FirstOrDefault(w => w.LastGlassFlg == eBitResult.ON); //現存在CST的最後一片
        //                if (theNewLastJob == null)
        //                    return false;
        //                if (int.TryParse(theNewLastJob.SourceSlotNo, out lastSlotno))//找不到slotno
        //                {
        //                    if (removSlotno > lastSlotno) //蓋回的是最大的
        //                    {
        //                        //Cancel Last Glass 
        //                        theNewLastJob = reMoveCovejob;
        //                        return true;
        //                    } else
        //                        return false;
        //                }
        //            }
        //        }

        //        return false;
        //    } catch (Exception ex) {
        //        Debug.Print(ex.Message);
        //        theNewLastJob = null;
        //        return false;
        //    }
        //}

        /// <summary>
        /// Save  Dispatch Hisotory 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="eventName"></param>
        /// <param name="dispatchEquipment"></param>
        /// <param name="dispatchPoint"></param>
        /// <param name="dispatchTarget"></param>
        /// <param name="dispatchResult"></param>
        /// <param name="samplingRate"></param>
        /// <param name="batchID"></param>
        //public void SaveDispatchHistory(Job job, string eventName, string dispatchEquipment, string dispatchPoint, string dispatchTarget, string dispatchResult, string samplingRate,string batchID,string reason) {
        //    try{
        //        DispatchHistory his = new DispatchHistory();
        //        his.UPDATETIME = DateTime.Now;
        //        his.CASSETTESEQNO = Convert.ToInt32(job.CassetteSequenceNo);
        //        his.CASSETTESLOTNO = Convert.ToInt32(job.JobSequenceNo);
        //        his.CIMMODE = job.CimModeCreate.ToString();
        //        his.EVENTNAME = eventName;
        //        his.GLASSGRADE = job.GlassGrade;
        //        his.GlASSID = job.JobId;
        //        his.GLASSTYPE = job.JobType.ToString();
        //        his.GLASSJUDGE = job.JobJudge;
        //        his.GROUPNO = Convert.ToInt32(job.GroupNo.Value);
        //        his.INSPECTIONRESERVATIONSIGNAL = job.InspectionReservalitionSignal;
        //        his.INSPJUDGEDRESULT = job.InspectionJudgeResult;
        //        his.TRACKINGDATAHISTORY = job.TrackingDataHistory;
        //        his.PPID = job.Ppid;
        //        his.PROCESSRESERVATIONSIGNAL = job.ProcessReservalitionSignal;
        //        his.EQUIPMENTSPECIALFLAG = job.EquipmentSpecialFlag;
        //        his.DISPATCHTARGET = dispatchTarget;
        //        his.DISPATCHRESULT = dispatchResult;
        //        his.DISPATCHPOINT = dispatchPoint;
        //        his.DISPATCHNODENO = dispatchEquipment;
        //        his.SAMPLINGRATE = samplingRate;
        //        his.BATCHID = batchID;
        //        his.NGMARK = job.CfSpecialData.NgMark;
        //        his.REASON = reason;
        //        this.InsertHistory(his);
       
        //    }catch (System.Exception ex){
        //        NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }
        //}

        /// <summary>
        /// Save DCR Read History
        /// </summary>
        /// <param name="cstSeq"></param>
        /// <param name="slotNo"></param>
        /// <param name="OldGlassID"></param>
        /// <param name="readGlassID"></param>
        /// <param name="result"></param>
        /// <param name="description"></param>
        public void SaveDCRResultHistory(string cstSeq, string slotNo, string OldGlassID, string readGlassID, string result, string description,string nodeNo,string dcrNo) {
            try{
                DCRResultHistory his = new DCRResultHistory();
                his.CASSETTESEQNO = Convert.ToInt32(cstSeq);
                his.CASSETTESLOTNO = Convert.ToInt32(slotNo);
                his.GlASSID = OldGlassID;
                his.READGLASSID = readGlassID;
                his.RESULT = result;
                his.DESCRIPTION = description;
                his.NODENO = nodeNo;
                his.DCRNO = dcrNo;
                this.InsertHistory(his);

            }catch (System.Exception ex){
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }


        //20170330 add SaveGlassQTimeHistory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="qtime">job Qtime Event</param>
        /// <param name="startQtimeUnit">Null is Not Start Unit</param>
        /// <param name="endQtimeUnit">Null is Not End Unit</param>
        /// <param name="useTime">total use Time. Before Qtime EndEvent this Value is 0, After Qtime EndEvnet this Value is EndDataTime - StartDateTime </param>
        public void SaveGlassQTimeHistory(Job job, Qtime qtime, Unit startQtimeUnit, Unit endQtimeUnit, Double useTime, string eventName)
        {
            try
            {
                GlassQTimeHistory his = new GlassQTimeHistory();
                his.CASSETTESEQNO = Convert.ToInt32(job.CassetteSequenceNo);
                his.CASSETTESLOTNO = Convert.ToInt32(job.JobSequenceNo);
                his.GlASSID = job.JobId;
                his.QTIMEID = qtime.QTimeID;
                his.STARTNODEID = qtime.StartEquipmentId;
                his.STARTNODENO = qtime.StartEquipmentNo;
                his.EVENTNAME = eventName;

                if (startQtimeUnit != null)
                {
                    his.STARTNUNITID = startQtimeUnit.Data.UNITID;
                    his.STARTNUNITNO = startQtimeUnit.Data.UNITNO.ToString();
                }
                else
                {
                    his.STARTNUNITID = "0";
                    his.STARTNUNITNO = "0";
                }

                his.STARTEVENTMSG = qtime.StartEvent;

                his.ENDNODEID = qtime.EndEquipmentId;
                his.ENDNODENO = qtime.EndEquipmentNo;

                if (endQtimeUnit != null)
                {
                    his.ENDNUNITID = endQtimeUnit.Data.UNITID;
                    his.ENDNUNITNO = endQtimeUnit.Data.UNITNO.ToString();

                }
                else
                {
                    his.ENDNUNITID = "0";
                    his.ENDNUNITNO = "0";
                }

                int qtimeSetVal = 0;
                int.TryParse(qtime.QtimeValue, out qtimeSetVal);

                his.SETTIMEVALUE = qtimeSetVal;
                his.ENDEVENTMSG = qtime.EndEvent;
                his.STARTDATETIME = qtime.StartQTime.ToString("yyyy-MM-dd HH:mm:ss:ffff");

                if (qtime.EndQTime != DateTime.MinValue)
                {
                    his.ENDDATETIME = qtime.EndQTime.ToString("yyyy-MM-dd HH:mm:ss:ffff");

                }

                his.SPENDQTIMEVALUE = (int)useTime;

                if (qtime.OverQTimeFlag == true)
                {
                    his.ISOVERQTIME = "Y";
                }
                else
                {
                    his.ISOVERQTIME = "N";
                }

                his.STARTNODERECIPEID = qtime.RecipeID;

                if (qtime.Enable == true)
                {
                    his.ENABLED = "Y";
                }
                else
                {
                    his.ENABLED = "N";
                }

                this.InsertHistory(his);

            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

    }
}
