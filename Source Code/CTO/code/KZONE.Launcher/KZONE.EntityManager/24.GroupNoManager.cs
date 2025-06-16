using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KZONE.Entity;
using KZONE.Log;
using System.Reflection;
using System.Collections;
using KZONE.DB;
using System.Data;
using KZONE.Work;


namespace KZONE.EntityManager
{
    
    public class GroupNoManager : EntityManager,IDataSource
    {
        private GourpNoConstant _groupNos;

        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.XML;
        }

        protected override string GetSelectHQL()
        {
            return string.Empty;
        }

        protected override Type GetTypeOfEntityData()
        {
            return null;
        }

        protected override void AfterSelectDB(List<Entity.EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            Filenames.Add("GroupNo.xml");
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(GourpNoConstant);
        }

        protected override Entity.EntityFile NewEntityFile(string Filename)
        {
            return new GourpNoConstant();
        }

        protected override void AfterInit(List<Entity.EntityData> entityDatas, List<Entity.EntityFile> entityFiles)
        {
            if (entityFiles.Count() > 0)
            {
                _groupNos = entityFiles[0] as GourpNoConstant;
            }
            else
                _groupNos = new GourpNoConstant();
        }

        public IList<GroupNo> GetGroupNos()
        {
            return _groupNos.GroupNoCollection;
        }

        public void UpdateLastUseTime(GroupNo type)
        {
            type.LastUseTime = DateTime.Now;
            EnqueueSave(_groupNos);
        }

        public void AddNewGroupNo(GroupNo type)
        {
            _groupNos.GroupNoCollection.Add(type);

            _groupNos.GroupNoCollection.Sort((a, b) =>
                {
                    if (a.Value > b.Value)
                        return 1;
                    else if (a.Value == b.Value)
                        return 0;
                    else
                        return -1;
                });
            EnqueueSave(_groupNos);
        }

        //private const int OFFLINE_DAILY_MIN_GROUPNO = 1;
        //private const int OFFLINE_DAILY_MAX_GROUPNO = 20;
        //private const int OFFLINE_DUMMY_MIN_GROUPNO = 21;
        //private const int OFFLINE_DUMMY_MAX_GROUPNO = 30;
        //private const int OFFLINE_NORMAL_MIN_GROUPNO = 31;
        //private const int OFFLINE_NORMAL_MAX_GROUPNO = 100;
        //private const int ONLINE_DAILY_MIN_GROUPNO = 101;
        //private const int ONLINE_DAILY_MAX_GROUPNO = 120;
        //private const int ONLINE_DUMMY_MIN_GROUPNO = 121;
        //private const int ONLINE_DUMMY_MAX_GROUPNO = 130;
        //private const int ONLINE_NORMAL_MIN_GROUPNO = 131;
        //private const int ONLINE_NORMAL_MAX_GROUPNO = 65535;

        //public bool GetGroupNo(eFabType fabtype, IList<Job> jobs, Job job, out string err, bool isOnlineFlag = false)
        //{
        //    err = string.Empty;
        //    try
        //    {
        //        Job tmpJob = null;

        //        #region [ 20160930 modify by Online/Offline and JobType決定GroupNo Range Range ]

        //        //20160921 add for GroupNo by Online/Offline and Job Type
        //        //Offline	1-100	  Normal	31-100
        //        //                    Dummy	    21-30
        //        //                    Daily 	1-20
        //        //Online	101-65535 Normal	131-65535
        //        //                    Dummy 	121-130
        //        //                    Daily	    101-120

        //        int curMinGroupNo = 0;
        //        int curMaxGroupNo = 0;

        //        if (isOnlineFlag == false)
        //        {

        //            #region [ Get Offline GroupNo ]

        //            switch (job.JobType)
        //            {

        //                case eJobType.DM: //Dummy	    21-30

        //                    curMaxGroupNo = GroupNo_Def.OFFLINE_DUMMY_MAX_GROUPNO;
        //                    curMinGroupNo = GroupNo_Def.OFFLINE_DUMMY_MIN_GROUPNO;

        //                    break;

        //                case eJobType.DY: //Daily 	1-20                            

        //                    curMaxGroupNo = GroupNo_Def.OFFLINE_DAILY_MAX_GROUPNO;
        //                    curMinGroupNo = GroupNo_Def.OFFLINE_DAILY_MIN_GROUPNO;

        //                    break;

        //                default: //Normal(Others)	31-100

        //                    curMaxGroupNo = GroupNo_Def.OFFLINE_NORMAL_MAX_GROUPNO;
        //                    curMinGroupNo = GroupNo_Def.OFFLINE_NORMAL_MIN_GROUPNO;

        //                    break;
        //            }

        //            #endregion

        //        }
        //        else
        //        {

        //            #region [ Get Online GroupNo ]

        //            switch (job.JobType)
        //            {

        //                case eJobType.DM: //Dummy	    121-130

        //                    curMaxGroupNo = GroupNo_Def.ONLINE_DUMMY_MAX_GROUPNO;
        //                    curMinGroupNo = GroupNo_Def.ONLINE_DUMMY_MIN_GROUPNO;

        //                    break;

        //                case eJobType.DY: //Daily 	101-120                            

        //                    curMaxGroupNo = GroupNo_Def.OFFLINE_DAILY_MAX_GROUPNO;
        //                    curMinGroupNo = GroupNo_Def.OFFLINE_DAILY_MIN_GROUPNO;

        //                    break;

        //                default: //Normal(Others)	131-65535

        //                    curMaxGroupNo = GroupNo_Def.ONLINE_NORMAL_MAX_GROUPNO;
        //                    curMinGroupNo = GroupNo_Def.ONLINE_NORMAL_MIN_GROUPNO;

        //                    break;
        //            }


        //            #endregion

        //        }

        //        #endregion

        //        // 1. 先從此次CST資料的去找.如果是第一片則表示Jobs為空 20160930 add 判斷GroupNo Range
        //        tmpJob = jobs.FirstOrDefault(j => ( j.GroupNo.Equals(job.GroupNo) && (j.GroupNo.Value >= curMinGroupNo & j.GroupNo.Value <= curMaxGroupNo)));
        //        if (tmpJob != null)
        //        {
        //            job.GroupNo.Value = tmpJob.GroupNo.Value;
        //            return true;
        //        }

        //        List<GroupNo> AllGroupNos = GetGroupNos() as List<GroupNo>;

        //        // 2. 從GroupNoManager去找整線的.看看目前Inline有包含哪些GroupNo設定 20160930 add 判斷GroupNo Range
        //        GroupNo checkGroupNo = AllGroupNos.FirstOrDefault(t => (t.Equals(job.GroupNo) && (t.Value >= curMinGroupNo & t.Value <= curMaxGroupNo)));

        //        if (checkGroupNo != null)
        //        {
        //            job.GroupNo =(GroupNo) checkGroupNo.Clone();
        //            UpdateLastUseTime(checkGroupNo);
        //            return true;
        //        }

        //        // 3. 從全部的JobData去找, 防止有人刪掉GroupNo的資料 Inline GroupNo被刪除則要新增=>20170301 mark. 不再從WIP中搜尋.
        //        IList<Job> allJobs = ObjectManager.JobManager.GetJobs();
        //        //// 20160930 add 判斷GroupNo Range
        //        //tmpJob = allJobs.FirstOrDefault(j => (j.GroupNo.Equals(job.GroupNo) && (j.GroupNo.Value >= curMinGroupNo & j.GroupNo.Value <= curMaxGroupNo)));

        //        //if (tmpJob != null)
        //        //{
        //        //    job.GroupNo.Value = tmpJob.GroupNo.Value;
        //        //    AddNewGroupNo(job.GroupNo);
        //        //    return true;
        //        //}

        //        //4. 之前都找不到時, 重新產生新的GroupNo. GroupNo:1~65535
        //        #region [ by Shop計算GroupNo ]               

        //        #region [ Get Can Use GroupNo ]

        //        //Get All Offline Daily GroupNo info
        //        List<int> tpList = AllGroupNos.Select(t => t.Value).Where(t => (t >= curMinGroupNo && t <= curMaxGroupNo)).ToList<int>();

        //        if (tpList.Count() >= (curMaxGroupNo - curMinGroupNo + 1))
        //        {
        //            // 已全部使用, 要從前面號碼開始找目前沒被JOBWIP使用的GroupNo
        //            List<GroupNo> tpList2 = AllGroupNos.Where(t => (t.Value >= curMinGroupNo && t.Value <= curMaxGroupNo) && !allJobs.Any(j => j.GroupNo.Value.Equals(t.Value))).ToList<GroupNo>();

        //            if (tpList2.Count() > 0)
        //            {
        //                ReplaceNewGroupNo(job, tpList2);
        //                return true;
        //            }
        //            else
        //            {
        //                err = string.Format("Each GroupNo for [{0}]-[{1}] has been used.", curMinGroupNo.ToString(), curMaxGroupNo.ToString());
        //                return false;
        //            }
        //        }

        //        //取得區間的Group Exist 資訊顯示
        //        StringBuilder groupNoExistInfo = new StringBuilder(new string('0', (curMaxGroupNo - curMinGroupNo + 1)));

        //        for (int i = 0; i < tpList.Count(); i++)
        //        {
        //            //Exist 是以Min為1 , Lenth = Max-Min + 1
        //            groupNoExistInfo.Replace('0', '1', tpList[i] - curMinGroupNo , 1);
        //        }

        //        //Index 所以 lenth=Max-Min+1 , Index= Max - 1 (EX 1~20)
        //        int lastNo = groupNoExistInfo.ToString().LastIndexOf('1');

        //        if (lastNo.Equals(-1))
        //        {
        //            //找不到表示就從Min開始編
        //            job.GroupNo.Value = curMinGroupNo;
        //            ObjectManager.GroupNoManager.AddNewGroupNo((GroupNo)job.GroupNo.Clone());
        //            return true;
        //        }
        //        else if (lastNo == (curMaxGroupNo - curMinGroupNo))
        //        {
        //            //第MAX的位置為1則回False 目前沒有可以用的GroupNo
        //            //err = "Each GroupNo for Offline Normal has been used.";
        //            err = string.Format("Each GroupNo for [{0}]-[{1}] has been used.", curMinGroupNo.ToString(), curMaxGroupNo.ToString());
        //            return false;

        //        }
        //        else
        //        {
        //            //找到 index Min~ Max-2之間的第一個0
        //            int no = groupNoExistInfo.ToString().IndexOf('0', lastNo);

        //            if (no == -1)
        //            {
        //                //都找不到表示都滿了
        //                //err = "Each GroupNo for Offline Normal has been used.";
        //                err = string.Format("Each GroupNo for [{0}]-[{1}] has been used.", curMinGroupNo.ToString(), curMaxGroupNo.ToString());
        //                return false;

        //            }
        //            else
        //            {
        //                //找到就更新GroupNo
        //                job.GroupNo.Value = no + curMinGroupNo;
        //                ObjectManager.GroupNoManager.AddNewGroupNo((GroupNo)job.GroupNo.Clone());
        //                return true;
        //            }
        //        }

        //        #endregion
                
        //        #endregion =======

        //    }
        //    catch (Exception ex)
        //    {
        //        err = ex.ToString();
        //        return false;
        //    }

        //}

        public GroupNo GetGroupNo(int groupNo) {
            List<GroupNo> AllGroupNos = GetGroupNos() as List<GroupNo>;

            // 2. 從GroupNoManager去找整線的.看看目前Inline有包含哪些GroupNo設定
            GroupNo checkGroupNo = AllGroupNos.FirstOrDefault(t => t.Value==groupNo);
            return checkGroupNo;
        }

        //public void ReplaceNewGroupNo(Job job, List<GroupNo> noInUseGroupNos)
        //{
        //    List<GroupNo> lst = noInUseGroupNos.OrderBy(p => p.LastUseTime).ToList<GroupNo>();

        //    job.GroupNo.Value = lst[0].Value;
        //    lst[0] =(GroupNo) job.GroupNo.Clone();
        //    lst[0].LastUseTime = DateTime.Now;
        //    EnqueueSave(_groupNos);
        //}

        #region Implement IDataSource
        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("GroupNoManager");
            return entityNames;
        }

        public DataTable GetDataTable(string entityName)
        {
            try
            {

                DataTable dt = new DataTable();

                GroupNo file = new GroupNo();

                DataTableHelp.DataTableAppendColumn(file, dt);


                foreach (GroupNo entity in _groupNos.GroupNoCollection)
                {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(entity, dr);
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
        
        #endregion
    }
}
