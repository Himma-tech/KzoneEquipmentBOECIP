using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;
using System.Collections;
using System.Xml.Schema;
using KZONE.Log;
using KZONE.MessageManager;
using KZONE.ConstantParameter;

namespace KZONE.EntityManager
{
    public class EquipmentManager : EntityManager, IDataSource
    {


        private Dictionary<string, Equipment> _entities = new Dictionary<string, Equipment>();
        private Dictionary<string, Dictionary<string, List<SecsVariableDataEntityData>>> _secsVariableData = new Dictionary<string, Dictionary<string, List<SecsVariableDataEntityData>>>();

        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.BIN;
        }

        protected override string GetSelectHQL()
        {
            return string.Format("from EquipmentEntityData where SERVERNAME = '{0}'", BcServerName);
        }

        protected string GetPositionSQL(string LineID)
        {
            return string.Format("from PositionEntityData where LINEID = '{0}'", LineID);
        }
        protected string GetTackSQL(string LineID)
        {
            return string.Format("from TactTimeEntityData where LINEID = '{0}'", LineID);
        }
        protected override Type GetTypeOfEntityData()
        {
            return typeof(EquipmentEntityData);
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            foreach (EntityData entity_data in EntityDatas)
            {
                EquipmentEntityData eqp_entity_data = entity_data as EquipmentEntityData;
                if (eqp_entity_data != null)
                {
                    string file_name = string.Format("{0}.bin", eqp_entity_data.NODEID);
                    Filenames.Add(file_name);
                }
            }
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(EquipmentEntityFile);
        }

        protected override EntityFile NewEntityFile(string Filename)
        {
            return new EquipmentEntityFile();
        }

        protected override void AfterInit(List<EntityData> entityDatas, List<EntityFile> entityFiles)
        {
            foreach (EntityData entity_data in entityDatas)
            {
                EquipmentEntityData eqp_entity_data = entity_data as EquipmentEntityData;
                if (eqp_entity_data != null)
                {
                    foreach (EntityFile entity_file in entityFiles)
                    {
                        EquipmentEntityFile eqp_entity_file = entity_file as EquipmentEntityFile;
                        if (eqp_entity_file != null)
                        {
                            string fextname = eqp_entity_file.GetFilename();
                            string fname = Path.GetFileNameWithoutExtension(fextname);
                            if (string.Compare(eqp_entity_data.NODEID, fname, true) == 0)
                            {
                                if (eqp_entity_data.DCRCOUNT > 0 && eqp_entity_file.DCREnableMode.Count() == 0)
                                {
                                    for (int i = 0; i < eqp_entity_data.DCRCOUNT; i++) eqp_entity_file.DCREnableMode.Add(eEnableDisable.Disable);
                                    EnqueueSave(eqp_entity_file);
                                }
                                _entities.Add(eqp_entity_data.NODEID, new Equipment(eqp_entity_data, eqp_entity_file));
                            }
                        }
                    }
                }
            }
            ReloadSECSVariableDataAll();
        }

        private Dictionary<string, List<SecsVariableDataEntityData>> LoadSECSVariableData(Equipment eqp)
        {
            string hql = string.Format("from SecsVariableDataEntityData where NODENO='{0}' and LINEID='{1}' order by TRID,ITEM_ID ", eqp.Data.NODENO, eqp.Data.LINEID);

            if (HibernateAdapter == null)
            {
                throw new Exception("Hibernate Adapter is Null. Cant get data from Data");
            }
            Dictionary<string, List<SecsVariableDataEntityData>> variables = new Dictionary<string, List<SecsVariableDataEntityData>>();
            IList list = HibernateAdapter.GetObjectByQuery(hql);

            if (list != null)
            {
                foreach (SecsVariableDataEntityData data in list)
                {
                    if (variables.ContainsKey(data.TRID))
                    {
                        variables[data.TRID].Add(data);
                    }
                    else
                    {
                        List<SecsVariableDataEntityData> variableList = new List<SecsVariableDataEntityData>();
                        variableList.Add(data);
                        variables.Add(data.TRID, variableList);
                    }
                }
            }
            return variables;

        }

        /// <summary>
        /// Load SECS Variable Data 
        /// </summary>
        public void ReloadSECSVariableDataAll()
        {
            foreach (Equipment eqp in _entities.Values)
            {
                eReportMode reportMode;
                Enum.TryParse<eReportMode>(eqp.Data.REPORTMODE, out reportMode);
                if (reportMode == eReportMode.HSMS || reportMode == eReportMode.HSMS_NIKON)
                {
                    Dictionary<string, List<SecsVariableDataEntityData>> variableDatas = LoadSECSVariableData(eqp);
                    lock (_secsVariableData)
                    {
                        if (_secsVariableData.ContainsKey(eqp.Data.NODENO))
                        {
                            _secsVariableData[eqp.Data.NODENO] = variableDatas;
                        }
                        else
                        {
                            _secsVariableData.Add(eqp.Data.NODENO, variableDatas);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// 
        /// </summary>
        /// <param name="eqpNo"></param>
        public void ReloadSECSVariableDataByEqpNo(string eqpNo)
        {
            Equipment eqp = GetEquipmentByNo(eqpNo);
            if (eqp == null)
                throw new Exception(string.Format("Canot found Equipment No ({0})", eqpNo));
            eReportMode reportMode;
            Enum.TryParse<eReportMode>(eqp.Data.REPORTMODE, out reportMode);
            if (reportMode == eReportMode.HSMS || reportMode == eReportMode.HSMS_NIKON || reportMode == eReportMode.HSMS_PLC) //20150131 cy add HSMS_PLC
            {
                Dictionary<string, List<SecsVariableDataEntityData>> variables = LoadSECSVariableData(eqp);
                lock (_secsVariableData)
                {
                    if (_secsVariableData.ContainsKey(eqpNo))
                    {
                        _secsVariableData[eqpNo] = variables;
                    }
                    else
                    {
                        _secsVariableData.Add(eqpNo, variables);
                    }
                }
            }
            else
            {
                throw new Exception(string.Format("Equipment No ({0}) reportMode is {1} not Supper this function. ", eqpNo, reportMode));
            }
        }

        /// <summary>
        /// Reload Recipe Reister validation Enable
        /// </summary>
        public void ReloadRecipeCheckFlagAll()
        {
            IList entiyDatas = HibernateAdapter.GetObjectByQuery(GetSelectHQL());

            if (entiyDatas != null)
            {
                foreach (EquipmentEntityData eqp in entiyDatas)
                {
                    Equipment eq = GetEquipmentByNo(eqp.NODENO);
                    if ((eq != null) && (eq.Data.RECIPEREGVALIDATIONENABLED != eqp.RECIPEREGVALIDATIONENABLED))
                    {
                        lock (eq)
                        {
                            eq.Data.RECIPEREGVALIDATIONENABLED = eqp.RECIPEREGVALIDATIONENABLED;
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Reload Recipe Parameter validation Enable
        /// </summary>
        public void ReloadRecipeParamCheckFlagAll()
        {
            IList entiyDatas = HibernateAdapter.GetObjectByQuery(GetSelectHQL());

            if (entiyDatas != null)
            {
                foreach (EquipmentEntityData eqp in entiyDatas)
                {
                    Equipment eq = GetEquipmentByNo(eqp.NODENO);
                    if ((eq != null) && (eq.Data.RECIPEPARAVALIDATIONENABLED != eqp.RECIPEPARAVALIDATIONENABLED))
                    {
                        lock (eq)
                        {
                            eq.Data.RECIPEPARAVALIDATIONENABLED = eqp.RECIPEPARAVALIDATIONENABLED;
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Reload Proflie Version
        /// </summary>
        public void ReloadProfileVersion()
        {
            IList entiyDatas = HibernateAdapter.GetObjectByQuery(GetSelectHQL());

            if (entiyDatas != null)
            {
                foreach (EquipmentEntityData eqp in entiyDatas)
                {
                    Equipment eq = GetEquipmentByNo(eqp.NODENO);
                    if ((eq != null) && (eq.Data.EQPPROFILE != eqp.EQPPROFILE))
                    {
                        lock (eq)
                        {
                            eq.Data.EQPPROFILE = eqp.EQPPROFILE;
                        }

                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eqp"></param>
        /// <param name="trid"></param>
        /// <returns></returns>
        public List<SecsVariableDataEntityData> GetVariableData(string eqp, string trid)
        {

            if (_secsVariableData.ContainsKey(eqp))
            {
                if (_secsVariableData[eqp].ContainsKey(trid))
                {
                    return _secsVariableData[eqp][trid];
                }
            }
            return null;
        }

        /// <summary>
        /// UPK Line By Equipment No Get Other EQP ID
        /// </summary>
        /// <param name="EQPNo"></param>
        /// <returns></returns>
        public string GetEQPID(string eqpNo)
        {
            string EQPID = string.Empty;

            Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
            if (eqp == null)
                return EQPID;

            Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
            if (line == null)
            {
                EQPID = eqp.Data.NODEID;
                return EQPID;
            }
            return EQPID;
        }

        /// <summary>
        /// By Equipment No Select Node no
        /// </summary>
        /// <param name="eqpNo"></param>
        /// <returns></returns>
        public Equipment GetEquipmentByNo(string eqpNo)
        {
            Equipment ret = null;
            foreach (Equipment entity in _entities.Values)
            {
                if (entity.Data.NODENO == eqpNo)
                {
                    ret = entity;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// by Equipment ID Select Equipment object
        /// </summary>
        /// <param name="eqpID"></param>
        /// <returns></returns>
        public Equipment GetEquipmentByID(string eqpID)
        {
            string _eqpID = string.Empty;
            Equipment ret = null;

            if (_entities.ContainsKey(eqpID))
            {
                ret = _entities[eqpID];
            }
            return ret;
        }

        public Dictionary<int, PositionEntityData> GetPositionData(string lineID)
        {
            Dictionary<int, PositionEntityData> position = new Dictionary<int, PositionEntityData>();
            string slecet = GetPositionSQL(lineID);
            foreach (EntityData data in FindByQuery(slecet))
            {
                PositionEntityData positionData = data as PositionEntityData;
                if (positionData != null)
                {
                    position.Add(positionData.PositionNo, positionData);
                }
            }

            return position;
        }

        public Dictionary<int, TactTimeEntityData> GetTackTimeData(string lineID)
        {
            Dictionary<int, TactTimeEntityData> position = new Dictionary<int, TactTimeEntityData>();
            string slecet = GetTackSQL(lineID);
            foreach (EntityData data in FindByQuery(slecet))
            {
                TactTimeEntityData positionData = data as TactTimeEntityData;
                if (positionData != null)
                {
                    position.Add(positionData.TackNo, positionData);
                }
            }

            return position;
        }



        public List<Equipment> GetEQPs()
        {
            List<Equipment> ret = new List<Equipment>();
            foreach (Equipment entity in _entities.Values)
            {
                ret.Add(entity);
            }
            return ret;
        }

        public List<Equipment> GetEQPsByLine(string lineID)
        {
            List<Equipment> ret = new List<Equipment>();
            foreach (Equipment entity in _entities.Values)
            {
                if (entity.Data.LINEID.Trim() == lineID.Trim())
                    ret.Add(entity);
            }
            return ret;
        }

        /// <summary>
        /// Save Equipment History  
        /// </summary>
        /// <param name="eqp"></param>
        public void SaveEquipmentHistory(Equipment eqp)
        {
            try
            {
                // Save DB
                EquipmentHistory his = new EquipmentHistory();

                his.LINEID = eqp.Data.LINEID;
                his.NODEID = eqp.Data.NODEID;
                his.NODENO = eqp.Data.NODENO;
                his.CIMMODE = eqp.File.CIMMode == eBitResult.ON ? "CIM ON" : "CIM OFF";
                his.VCRENBLE = eqp.File.DCREnableMode[0].ToString();
                his.AUTOMANUEL = eqp.File.EquipmentOperationMode.ToString();
                his.RECIPEAUTOCHANGE = eqp.File.AutoRecipeChangeMode.ToString();
                his.RECIPECHECK = eqp.File.RecipeCheckMode.ToString();

                his.CURRENTRECIPEID = eqp.File.CurrentRecipeID;
                his.CURRENTSTATUS = eqp.File.Status.ToString();
                his.TFTJOBCOUNT = eqp.File.TotalTFTGlassCount;
                his.HFJOBCOUNT = eqp.File.TotalHFGlassCount;
                his.DUMMYJOBCOUNT = eqp.File.DummyGlassCount;
                his.UVMASKCOUNT = eqp.File.UVMaskCount;
                his.MQCJOBCOUNT = eqp.File.MQCGlassCount;
                his.PRODUCTTYPE = eqp.File.ProductType;
                his.UPINLINEMODE = eqp.File.UpInlineMode.ToString();
                his.DOWNINLINEMODE = eqp.File.DownInlineMode.ToString();
                his.PRODUCTIDCHECKMODE = eqp.File.ProductIDCheckMode.ToString();
                his.PRODUCTTYPECHECKMODE = eqp.File.ProductTypeCheckMode.ToString();
                his.GROUPINDEXCHECKMODE = eqp.File.GroupIndexCheckMode.ToString();
                his.DUPLICATECHECKMODE = eqp.File.JobDuplicateCheckMode.ToString();


                his.ADDLIQUID = eqp.File.AddWater == eBitResult.ON ? "ON" : "OFF";
                his.MINLIQUID = eqp.File.DisWater == eBitResult.ON ? "ON" : "OFF";
                his.VCRID = eqp.File.VcrReadID;
                his.GLASSCHECKMODE = eqp.File.GlassCheckMode.ToString();

                ObjectManager.EquipmentManager.InsertHistory(his);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                EquipmentEntityData data = new EquipmentEntityData();
                EquipmentEntityFile file = new EquipmentEntityFile();
                DataTableHelp.DataTableAppendColumn(data, dt);
                DataTableHelp.DataTableAppendColumn(file, dt);

                List<Equipment> eqp_entities = GetEQPs();
                foreach (Equipment entity in eqp_entities)
                {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(entity.Data, dr);
                    DataTableHelp.DataRowAssignValue(entity.File, dr);
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

        public IList<string> GetEntityNames()
        {
            IList<string> entityName = new List<string>();
            entityName.Add("EquipmentManager");
            return entityName;
        }

        /// <summary>
        /// By Equipment No Select Node no
        /// </summary>
        /// <param name="eqpNo"></param>
        /// <returns></returns>
        public Equipment GetEQP(string eqpNo)
        {
            Equipment ret = null;
            foreach (Equipment entity in _entities.Values)
            {
                if (entity.Data.NODENO == eqpNo || entity.Data.ATTRIBUTE == eqpNo)
                {
                    ret = entity;
                    break;
                }
            }
            return ret;
        }


        /// <summary>
        /// AssemblyHistory 
        /// </summary>
        /// <param name="equipmentID"></param>
        /// <param name="unitNo"></param>
        /// <param name="currentMaterial"></param>
        /// <param name="oldMaterialId"></param>
        public void SaveTankHistory(TankHistory tankHistory)
        {
            try
            {

                this.HistoryHibernateAdapter.SaveObject(tankHistory);
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void SaveCIMMessageHistory(CIMMESSAGEHISTORY history)
        {
            this.InsertHistory(history);

            GetSetMessageHistory();



        }

        public void SaveRMSMessageHistory(RecipeValidationResultHistory history)
        {
            this.InsertHistory(history);


        }

        public void GetSetMessageHistory()
        {
            try
            {
                Equipment eq = GetEQP("L3");
                eq.File.SetCimMesg = new List<CIMMESSAGEHISTORY>();

                IList allmessage = this.HistoryHibernateAdapter.GetObjectByQuery(
                    String.Format("from CIMMESSAGEHISTORY where MESSAGESTATUS ='set' order by UPDATETIME desc"));

                //IList top20message = this.HistoryHibernateAdapter.GetObjectByQuery(
                //    String.Format("top 20 * from CIMMESSAGEHISTORY where MESSAGESTATUS ='set' order by UPDATETIME desc"));


                if (allmessage != null)
                {
                    foreach (var mesg in allmessage)
                    {
                        if (eq.File.SetCimMesg.Count <= 4)
                        {
                            CIMMESSAGEHISTORY cimMessage = (CIMMESSAGEHISTORY)mesg;

                            eq.File.SetCimMesg.Add(cimMessage);
                        }
                        else
                        {
                            CIMMESSAGEHISTORY cimMessage = (CIMMESSAGEHISTORY)mesg;

                            UpdateCIMMessage(cimMessage.MESSAGEID, "clear");
                        }
                    }
                }


            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void UpdateCIMMessage(string cimMessageID, string cimMessageStatus)
        {
            try
            {
                IList message = this.HistoryHibernateAdapter.GetObjectByQuery(
                    String.Format("from CIMMESSAGEHISTORY where MESSAGEID ='{0}' AND MESSAGESTATUS ='set'", cimMessageID));

                if (message != null)
                {
                    foreach (var mes in message)
                    {
                        CIMMESSAGEHISTORY cimMessage = (CIMMESSAGEHISTORY)mes;

                        cimMessage.MESSAGESTATUS = cimMessageStatus;

                        this.HistoryHibernateAdapter.UpdateObject(cimMessage);
                    }

                }

                GetSetMessageHistory();

                //if (message!=null && message[0] is CIMMESSAGEHISTORY)
                //{
                //    CIMMESSAGEHISTORY cimMessage = (CIMMESSAGEHISTORY) message[0];

                //    cimMessage.MESSAGESTATUS = cimMessageStatus;

                //    this.HistoryHibernateAdapter.UpdateObject(cimMessage);
                //}
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


    }
}
