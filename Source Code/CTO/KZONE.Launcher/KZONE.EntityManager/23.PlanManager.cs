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
    public class PlanManager : EntityManager, IDataSource
    {
        private Dictionary<string, Plan> _disPlans = new Dictionary<string, Plan>();


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
            return typeof(LineEntityData);
        }

        protected override void AfterSelectDB(List<Entity.EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            Filenames.Add("*.xml");
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(Plan);
        }

        protected override Entity.EntityFile NewEntityFile(string Filename)
        {
            return new Plan();
        }

        protected override void AfterInit(List<Entity.EntityData> entityDatas, List<Entity.EntityFile> entityFiles)
        {
            foreach (EntityFile entity_file in entityFiles)
            {
                Plan plan_entity = entity_file as Plan;
                if (plan_entity != null)
                {
                    string planID = plan_entity.PLAN_NAME;
                    if (!_disPlans.ContainsKey(planID))
                    {
                        _disPlans.Add(planID, plan_entity);
                    }
                }
            }
        }

        /// <summary>
        /// 將Plan加入Dictionary
        /// Plan DownLoad 成功后添加
        /// </summary>
        public void AddPlan(Plan plan)
        {
            lock (_disPlans)
            {
                string planID = plan.PLAN_NAME;
                if (!_disPlans.ContainsKey(planID))
                {
                    _disPlans.Add(planID, plan);
                }
                else
                {
                    _disPlans.Remove(planID);
                    _disPlans.Add(planID, plan);
                }
                plan.SetFilename(string.Format("{0}.xml", planID));
                EnqueueSave(plan);
            }
        }

        /// <summary>
        /// 將Plan從Dictionary移除
        /// </summary>
        /// <param name="plan"></param>
        public void DeletePlan(Plan  plan)
        {
            lock (_disPlans)
            {
                string planID = plan.PLAN_NAME;
                if (_disPlans.ContainsKey(planID))
                {
                    plan.WriteFlag = false;
                    _disPlans.Remove(planID);
                    EnqueueSave(plan);
                }
            }
        }

        public void DeletePlan(string planId)
        {
            lock (_disPlans)
            {

                if (_disPlans.ContainsKey(planId))
                {
                    Plan plan = _disPlans[planId];
                    plan.WriteFlag = false;
                    _disPlans.Remove(planId);
                    EnqueueSave(plan);
                }
            }

        }

        public void DeletePlans()
        {
            lock (_disPlans)
            {
                foreach (string key in _disPlans.Keys)
                {
                    _disPlans[key].WriteFlag = false;
                    _disPlans.Remove(_disPlans[key].PLAN_NAME);
                    EnqueueSave(_disPlans[key]);
                }
            }
        }

        /// <summary>
        /// 取得line Plan 以List 方式傳回
        /// </summary>
        /// <returns>Plan List</returns>
        public List<Plan> GetPlans()
        {
            List<Plan> ret = null;
            lock (_disPlans)
            {
                ret = _disPlans.Values.ToList();
            }
            return ret;
        }

        public Plan GetPlan(string planID)
        {
            Plan plan = null;
            if (_disPlans.ContainsKey(planID))
                plan = _disPlans[planID];
            return plan;
        }
     
        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("PlanManager");
            return entityNames;
        }

        /// <summary>
        /// 在Objects 画面显示Plan
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                Plan file = new Plan();
                DataTableHelp.DataTableAppendColumn(file, dt);

                List<Plan> plans = GetPlans();
                foreach (Plan entity in plans)
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
   
    }

    //public class ShowPlan
    //{
    //    private string _plinID = string.Empty;
    //    private string _portID = string.Empty;
    //    private string _cstID = string.Empty;
    //    private string _portType = string.Empty;
    //    private List<SlotInfo> _slotInfos = new List<SlotInfo>();

    //    public List<SlotInfo> SlotInfos
    //    {
    //        get { return _slotInfos; }
    //        set { _slotInfos = value; }
    //    }
    //    public string PLAN_ID
    //    {
    //        get { return _plinID; }
    //        set { _plinID = value; }
    //    }
    //    public string PortID
    //    {
    //        get { return _portID; }
    //        set { _portID = value; }
    //    }
    //    public string CstID
    //    {
    //        get { return _cstID; }
    //        set { _cstID = value; }
    //    }
    //    public string PortType
    //    {
    //        get { return _portType; }
    //        set { _portType = value; }
    //    }
       
    //}

    //public class SlotInfo
    //{
    //    private string _pRODUCTNAME = string.Empty;
    //    private int _tARGETSLOTNO = 0;
    //    private int __sOURCESLOTNO = 0;
    //    private bool _hAVEBEENUSED = false;
    //    private string _sOURCEPORTID = string.Empty;
    //    private string _tARGETPORTID = string.Empty;

       
    //    public string PRODUCT_NAME
    //    {
    //        get { return _pRODUCTNAME; }
    //        set { _pRODUCTNAME = value; }
    //    }

    //    public int TARGETSLOTNO
    //    {
    //        get { return _tARGETSLOTNO; }
    //        set { _tARGETSLOTNO = value; }
    //    }

    //    public int SOURCESLOTNO
    //    {
    //        get { return __sOURCESLOTNO; }
    //        set { __sOURCESLOTNO = value; }
    //    }

    //    public bool HAVE_BEEN_USED
    //    {
    //        get { return _hAVEBEENUSED; }
    //        set { _hAVEBEENUSED = value; }
    //    }

    //    public string SOURCE_PORT_ID
    //    {
    //        get { return _sOURCEPORTID; }
    //        set { _sOURCEPORTID = value; }
    //    }

    //    public string TARGET_PORT_ID
    //    {
    //        get { return _tARGETPORTID; }
    //        set { _tARGETPORTID = value; }
    //    }
    //}
}
