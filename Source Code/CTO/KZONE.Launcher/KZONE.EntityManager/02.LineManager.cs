/************************************************************
 * 1.0 修改SkipAgent DB中会直接Key OEE，MES，EDA等，程序中需要补充"Agent"  20150126  Tom
 * 
 * 
 * 
 * 
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.DB;
using KZONE.Entity;
using KZONE.Log;
using System.Collections;

namespace KZONE.EntityManager
{
    public class LineManager : EntityManager, IDataSource
    {
        private Dictionary<string, Line> _entities = new Dictionary<string, Line>();

        private Dictionary<string, List<LineStatusSpecEntityData>> _LineStatusSpecs = new Dictionary<string, List<LineStatusSpecEntityData>>();

        private Dictionary<string, List<SkipReportEntityData>> _skipReports = new Dictionary<string, List<SkipReportEntityData>>();

        private Dictionary<string, Dictionary<string, RunModeCheckRuleEntityData>> _runModeCheckRules = new Dictionary<string, Dictionary<string, RunModeCheckRuleEntityData>>();

        private Dictionary<string, Dictionary<string, SortGradeRule>> _sortGradeRules = new Dictionary<string, Dictionary<string, SortGradeRule>>();

        public override EntityManager.FILE_TYPE GetFileType() {
            return FILE_TYPE.BIN;
        }

        protected override string GetSelectHQL() {
            return string.Format("from LineEntityData where SERVERNAME = '{0}'", BcServerName);
        }

        protected override Type GetTypeOfEntityData() {
            return typeof(LineEntityData);
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames) {
            Filenames = new List<string>();
            foreach (EntityData entity_data in EntityDatas) {
                LineEntityData line_entity_data = entity_data as LineEntityData;
                if (line_entity_data != null) {
                    string file_name = string.Format("{0}.bin", line_entity_data.LINEID);
                    Filenames.Add(file_name);
                }
            }
        }

        protected override Type GetTypeOfEntityFile() {
            return typeof(LineEntityFile);
        }

        protected override EntityFile NewEntityFile(string Filename) {
            return new LineEntityFile();
        }

        protected override void AfterInit(List<EntityData> entityDatas, List<EntityFile> entityFiles) {
            foreach (EntityData entity_data in entityDatas) {
                LineEntityData line_entity_data = entity_data as LineEntityData;
                if (line_entity_data != null) {
                    foreach (EntityFile entity_file in entityFiles) {
                        LineEntityFile line_entity_file = entity_file as LineEntityFile;
                        if (line_entity_file != null) {
                            string fextname = line_entity_file.GetFilename();
                            string fname = Path.GetFileNameWithoutExtension(fextname);
                            if (string.Compare(line_entity_data.LINEID, fname, true) == 0)
                                _entities.Add(line_entity_data.LINEID, new Line(line_entity_data, line_entity_file));
                        }
                    }
                }
            }
         //   ReloadLineStatusSpec();
          //  ReloadRunModeCheckRule();
           // ReloadSortGradeRule();
            //ReloadSkipReport();
        }

        /// <summary>
        /// Reload CheckCrossRecipe Enable/Disable
        /// </summary>
        public void ReloadCheckCrossRecipe() {
            string hql = string.Format("from LineEntityData where SERVERNAME = '{0}'", BcServerName);
            IList entiyDatas = HibernateAdapter.GetObjectByQuery(hql);

            if (entiyDatas != null) {
                foreach (LineEntityData line in entiyDatas) {
                    Line Line = GetLine(line.LINEID);
                    if ((Line != null) && (Line.Data.CHECKCROSSRECIPE != line.CHECKCROSSRECIPE)) {
                        lock (Line) {
                            Line.Data.CHECKCROSSRECIPE = line.CHECKCROSSRECIPE;
                        }

                    }

                }
            }
        }

        public void ReloadLineStatusSpec() {
            Dictionary<string, List<LineStatusSpecEntityData>> dic = LoadLineStatusSpec();
            if (dic != null && dic.Count > 0) {
                lock (_LineStatusSpecs) {
                    _LineStatusSpecs = dic;
                }
            }
        }

        /// <summary>
        /// Reload Skip Report
        /// </summary>
        public void ReloadSkipReport() {
            Dictionary<string, List<SkipReportEntityData>> dic = LoadSkipReport();
            if (dic != null && dic.Count > 0) {
                lock (_skipReports) {
                    _skipReports = dic;
                }
            }
        }

        /// <summary>
        /// Reload Run Mode  Check Rule
        /// </summary>
        public void ReloadRunModeCheckRule()
        {
            Dictionary<string, Dictionary<string, RunModeCheckRuleEntityData>> dic = LoadRunModeCheckRule();

            if (dic != null && dic.Count > 0)
            {
                lock (_runModeCheckRules)
                {
                    _runModeCheckRules = dic;
                }
            }
        }

        /// <summary>
        /// Roload Sort Grade Rule 
        /// </summary>
        public void ReloadSortGradeRule()
        {
            Dictionary<string, Dictionary<string, SortGradeRule>> dic = LoadSortGradeRule();
            if (dic != null)
            {
                lock (_sortGradeRules)
                {
                    _sortGradeRules = dic;
                }
            }
        }

        /// <summary>
        /// Load Line Status Spec
        /// </summary>
        protected Dictionary<string, List<LineStatusSpecEntityData>> LoadLineStatusSpec() {
            Dictionary<string, List<LineStatusSpecEntityData>> lineStatusSpecs = new Dictionary<string, List<LineStatusSpecEntityData>>();
            foreach (Line line in _entities.Values) {
                string hql = string.Format("from LineStatusSpecEntityData where LINETYPE='{0}'  ORDER BY  CONDITIONSEQNO", line.Data.LINETYPE);
                IList list = HibernateAdapter.GetObjectByQuery(hql);
                List<LineStatusSpecEntityData> lineStatusSpecsList = null;
                if (list != null) {
                    lineStatusSpecsList = new List<LineStatusSpecEntityData>();

                    foreach (LineStatusSpecEntityData spec in list) {
                        lineStatusSpecsList.Add(spec);
                    }
                }
                lock (lineStatusSpecs) {
                    if (lineStatusSpecs.ContainsKey(line.Data.LINEID)) {
                        lineStatusSpecs[line.Data.LINEID] = lineStatusSpecsList;
                    } else {
                        lineStatusSpecs.Add(line.Data.LINEID, lineStatusSpecsList);
                    }
                }

            }
            return lineStatusSpecs;
        }

        /// <summary>
        /// Load Skip Report Flag
        /// </summary>
        protected Dictionary<string, List<SkipReportEntityData>> LoadSkipReport() {
            Dictionary<string, List<SkipReportEntityData>> skipReports = new Dictionary<string, List<SkipReportEntityData>>();
            foreach (Line line in _entities.Values) {
                string hql = string.Format("from SkipReportEntityData where LINEID='{0}'  ORDER BY  NODENO", line.Data.LINEID);
                IList list = HibernateAdapter.GetObjectByQuery(hql);
                List<SkipReportEntityData> lineSkipReports = new List<SkipReportEntityData>();
                if (list != null) {
                    foreach (SkipReportEntityData spec in list) {
                        lineSkipReports.Add(spec);
                    }
                }
                lock (skipReports) {
                    if (skipReports.ContainsKey(line.Data.LINEID)) {
                        skipReports[line.Data.LINEID] = lineSkipReports;
                    } else {
                        skipReports.Add(line.Data.LINEID, lineSkipReports);
                    }
                }
            }
            return skipReports;
        }

        protected Dictionary<string, Dictionary<string,RunModeCheckRuleEntityData>> LoadRunModeCheckRule()
        {
            Dictionary<string, Dictionary<string, RunModeCheckRuleEntityData>> runModeCheckRules = new Dictionary<string, Dictionary<string, RunModeCheckRuleEntityData>>();
            foreach (Line line in _entities.Values)
            {
                string hql = string.Format("from RunModeCheckRuleEntityData where LINEID='{0}'", line.Data.LINEID);
                IList list = HibernateAdapter.GetObjectByQuery(hql);
                Dictionary<string, RunModeCheckRuleEntityData> checkRules = new Dictionary<string, RunModeCheckRuleEntityData>();
                if (list != null)
                {
                    foreach (RunModeCheckRuleEntityData entity in list)
                    {
                        if (checkRules.ContainsKey(entity.RUNMODE))
                        {
                            checkRules.Remove(entity.RUNMODE);
                        }
                        checkRules.Add(entity.RUNMODE, entity);
                    }

                    if (runModeCheckRules.ContainsKey(line.Data.LINEID))
                    {
                        runModeCheckRules.Remove(line.Data.LINEID);
                    }
                    runModeCheckRules.Add(line.Data.LINEID, checkRules);
                    
                }

            }
            return runModeCheckRules;


        }

        protected Dictionary<string, Dictionary<string, SortGradeRule>> LoadSortGradeRule()
        {
            Dictionary<string, Dictionary<string, SortGradeRule>> sortGradeRules = new Dictionary<string, Dictionary<string, SortGradeRule>>();
            foreach (Line line in _entities.Values)
            {
                string hql = string.Format("from SortGradeRule where LINEID='{0}' ORDER BY PRORITY", line.Data.LINEID);
                IList list = HibernateAdapter.GetObjectByQuery(hql);
                Dictionary<string, SortGradeRule> rules = new Dictionary<string, SortGradeRule>();
                if (list != null)
                {
                    foreach (SortGradeRule rule in list)
                    {
                        if (rules.ContainsKey(rule.SORTGRADE))
                        {
                            rules.Remove(rule.SORTGRADE);
                        }
                        rules.Add(rule.SORTGRADE, rule);
                    }
                    if (sortGradeRules.ContainsKey(line.Data.LINEID))
                    {
                        sortGradeRules.Remove(line.Data.LINEID);
                    }
                    sortGradeRules.Add(line.Data.LINEID, rules);
                }
                
            }
            return sortGradeRules;

        }
        /// <summary>
        /// get Line object
        /// </summary>
        /// <param name="lineID"></param>
        /// <returns></returns>
        public Line GetLine()
        {
            Line ret = null;

            if (_entities.Count > 0)
                ret = _entities.First().Value;
            return ret;
        }

        /// <summary>
        /// get Line object
        /// </summary>
        /// <param name="lineID"></param>
        /// <returns></returns>
        public Line GetLine(string lineID) {
            string _lineID = string.Empty;
            Line ret = null;

            if (_entities.ContainsKey(lineID.Trim()))
                ret = _entities[lineID.Trim()];
            return ret;
        }

        /// <summary>
        /// get  All  Line Ojbects
        /// </summary>
        /// <returns></returns>
        public List<Line> GetLines() {
            List<Line> ret = new List<Line>();
            foreach (Line entity in _entities.Values) {
                ret.Add(entity);
            }
            return ret;
        }

        #region  Implement IDataSource
        public System.Data.DataTable GetDataTable(string entityName) {
            switch (entityName) {
                case "LineManager":
                    return GetLineDataTable();
                case "LineStatusSpec":
                    return GetLineStatusSpecDataTable();
                case "SkipReport":
                    return GetSkipReportDataTable();
                default:
                    return null;

            }

        }

        private DataTable GetLineDataTable() {
            try {
                DataTable dt = new DataTable();
                LineEntityData data = new LineEntityData();
                LineEntityFile file = new LineEntityFile();
                DataTableHelp.DataTableAppendColumn(data, dt);
                DataTableHelp.DataTableAppendColumn(file, dt);

                List<Line> line_entities = GetLines();
                foreach (Line entity in line_entities) {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(entity.Data, dr);
                    DataTableHelp.DataRowAssignValue(entity.File, dr);
                    dt.Rows.Add(dr);
                }
                return dt;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        private DataTable GetLineStatusSpecDataTable() {
            try {
                DataTable dt = new DataTable();
                LineStatusSpecEntityData data = new LineStatusSpecEntityData();

                DataTableHelp.DataTableAppendColumn(data, dt);

                foreach (string key in _LineStatusSpecs.Keys) {
                    List<LineStatusSpecEntityData> specs = _LineStatusSpecs[key];
                    foreach (LineStatusSpecEntityData spec in specs) {
                        DataRow dr = dt.NewRow();
                        DataTableHelp.DataRowAssignValue(spec, dr);

                        dt.Rows.Add(dr);
                    }

                }
                return dt;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        private DataTable GetSkipReportDataTable() {
            try {
                DataTable dt = new DataTable();
                SkipReportEntityData data = new SkipReportEntityData();

                DataTableHelp.DataTableAppendColumn(data, dt);

                foreach (string key in _skipReports.Keys) {
                    List<SkipReportEntityData> specs = _skipReports[key];
                    foreach (SkipReportEntityData spec in specs) {
                        DataRow dr = dt.NewRow();
                        DataTableHelp.DataRowAssignValue(spec, dr);

                        dt.Rows.Add(dr);
                    }

                }
                return dt;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        public IList<string> GetEntityNames() {
            IList<string> entitys = new List<string>();
            entitys.Add("LineManager");
            entitys.Add("LineStatusSpec");
            entitys.Add("SkipReport");
            return entitys;

        }
        #endregion

        #region Line State
        /// <summary>
        /// Get Line State
        /// </summary>
        /// <param name="lineId">line ID</param>
        /// <returns>Line State</returns>
        public string GetLineState(string lineId) {
            try {
                List<Equipment> equipmentList = ObjectManager.EquipmentManager.GetEQPsByLine(lineId);
                if (equipmentList == null || equipmentList.Count == 0) {
                    NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Can't found Equipment list");
                    return "";
                }
                foreach (Equipment equipment in equipmentList) {
                    if (equipment.File.CIMMode == eBitResult.OFF) {
                        if (equipment.Data.ATTRIBUTE.Equals("LD") || equipment.Data.ATTRIBUTE.Equals("UD") || equipment.Data.ATTRIBUTE.Equals("LU")) {
                            NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("Loader or Unloader Equipment[{0}] CIM mode is OFF.", equipment.Data.NODENO));

                            return eLINE_STATUS.DOWN;
                        }
                    }
                }


                for (int i = 1; i <= 3; i++)
                {
                    string conditionState = this.GetLineStatusToString(i);

                    if (!_LineStatusSpecs.ContainsKey(lineId))
                    {
                        NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                                                       string.Format("Can't found Line Status Spec list.[{0}]", conditionState));

                        return "";
                    }
                    List<LineStatusSpecEntityData> lineConditionList = _LineStatusSpecs[lineId].Where(n => n.CONDITIONSTATUS.ToUpper().Equals(conditionState)).ToList<LineStatusSpecEntityData>();

                    if (lineConditionList == null || lineConditionList.Count == 0)
                    {
                        NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                                                        string.Format("Can't found Line Status Spec list.[{0}]", conditionState));
                    }
                    else
                    {

                        foreach (LineStatusSpecEntityData lineStatusSpec in lineConditionList)
                        {
                            if (this.CheckEqpStatus_OR(equipmentList, lineStatusSpec.OREQPNOLIST, conditionState))
                            {
                                return conditionState;
                            }
                            else
                            {
                                bool result = this.CheckEqpStatus_AND(equipmentList, lineStatusSpec.ANDEQPNOLIST, conditionState);

                                if (result)
                                {
                                    return conditionState;
                                }
                            }
                        }
                    }
                }
                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", "Can't found matching 'EQP status' in LINESTATUSSPEC,Keep original Status.");

            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod() + "()", ex);
                return "";
            }
            return "";
        }

        private string GetLineStatusToString(int index) {
            string condition = "";

            switch (index) {
                case 1:
                    condition = eLINE_STATUS.DOWN;
                    break;
                case 2:
                    condition = eLINE_STATUS.RUN;
                    break;
                case 3:
                    condition = eLINE_STATUS.IDLE;
                    break;
            }

            return condition;
        }

        private bool CheckEqpStatus_AND(IList<Equipment> equipmentList, string ConditionEqpList, string conditionState)
        {
            if (string.IsNullOrEmpty(ConditionEqpList))
            {
                return false;
            }
            
            string[] conditionEqpList = ConditionEqpList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (conditionEqpList == null || conditionEqpList.Length == 0) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Can't found Line Status Spec Condition Eqp list.[{0}]", conditionState));

                return false;
            }

            try {
                bool allEQPCIMModeOff = true;

                foreach (string conditionLocalNo in conditionEqpList) {
                    foreach (Equipment equipment in equipmentList) {
                        if (equipment.Data.NODENO.ToUpper().Equals(conditionLocalNo.ToUpper())) {
                            if (equipment.File.CIMMode == eBitResult.OFF) {
                                if (conditionEqpList.Length == 1) {
                                    return false;
                                } else {
                                    continue;
                                }
                            }

                            allEQPCIMModeOff = false;

                            eEQPStatus eqpStatus = equipment.File.Status;

                            string lineStatus = ConvertEQPStatus(eqpStatus);
                            if (lineStatus.ToUpper() == conditionState.ToUpper()) {
                                continue;
                            } else {
                                return false;
                            }
                        }
                    }
                }

                if (allEQPCIMModeOff) {
                    NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                                        string.Format("All EQP CIM Mode is OFF.[{0}]", conditionState));
                    return false;
                }

                return true;
            } catch (Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }

            return false;
        }

        private bool CheckEqpStatus_OR(IList<Equipment> equipmentList, string coditionEqpList, string conditionState)
        {
            if (string.IsNullOrEmpty(coditionEqpList)) {
                return false;
            }

            string[] conditionEqpList = coditionEqpList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (conditionEqpList == null || conditionEqpList.Length == 0)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Can't found Line Status Spec Condition Eqp list.[{0}]", conditionState));

                return false;
            }
            try
            {
                bool allEQPCIMModeOff = true;

                foreach (string conditionLocalNo in conditionEqpList)
                {
                    foreach (Equipment equipment in equipmentList)
                    {
                        if (equipment.Data.NODENO.ToUpper().Equals(conditionLocalNo.ToUpper()))
                        {
                            if (equipment.File.CIMMode == eBitResult.OFF)
                            {
                                if (conditionEqpList.Length == 1)
                                {
                                    return false;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            allEQPCIMModeOff = false;

                            eEQPStatus eqpStatus = equipment.File.Status;

                            string lineStatus = ConvertEQPStatus(eqpStatus);
                            if (lineStatus.ToUpper() == conditionState.ToUpper())
                            {
                                return true; //有相等的就直接退出
                            }
                            else
                            {
                                continue;// 不相等 继续下一个
                            }
                        }
                    }
                }

                if (allEQPCIMModeOff)
                {
                    NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                                        string.Format("All EQP CIM Mode is OFF.[{0}]", conditionState));
                    return false;
                }

                return false;
            } catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }

            return false;
        }

        private string ConvertEQPStatus(eEQPStatus eqpStatus) {
            string returnValue = "";

            switch (eqpStatus) {
                case eEQPStatus.Idle:
                    returnValue = eLINE_STATUS.IDLE;
                    break;

                case eEQPStatus.Run:
                    returnValue = eLINE_STATUS.RUN;
                    break;

                case eEQPStatus.Initial:
                case eEQPStatus.Down:
                case eEQPStatus.Stop:
                case eEQPStatus.Unused:
                    returnValue = eLINE_STATUS.DOWN;
                    break;

                default: break;
            }

            return returnValue;
        }

        #endregion

        /// <summary>
        /// Check Need To Report MES or OEE 
        /// False Need to Report
        /// True don’t Report 
        /// </summary>
        /// <param name="agentName">OEE,MES,EDA</param>
        /// <returns></returns>
        public bool CheckSkipReportByID(string lineName, string machineName, string unitID, string reportName, string agentName) {
            try {
                if (_skipReports.ContainsKey(lineName)) {
                    List<SkipReportEntityData> _skiplist = _skipReports[lineName];

                    foreach (SkipReportEntityData sr in _skiplist) {
                        if (sr.NODEID == machineName && sr.UNITID == unitID && sr.SKIPREPORTTRX == reportName.ToUpper() && sr.SKIPAGENT + "Agent" == agentName) {
                            return true;
                        }
                    }

                }
                return false;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return false;
        }


        /// <summary>
        /// Check Need To Report MES or OEE 
        /// False Need to Report
        /// True don’t Report 
        /// </summary>
        /// <param name="agentName">OEE,MES,EDA</param>
        /// <returns></returns>
        public bool CheckSkipReportByNo(string lineName, string equipmentNo, string unitNo, string reportName, string agentName) {
            try {
                if (_skipReports.ContainsKey(lineName)) {
                    List<SkipReportEntityData> _skiplist = _skipReports[lineName];

                    foreach (SkipReportEntityData sr in _skiplist) {
                        if (sr.NODENO == equipmentNo && sr.UNITNO == unitNo && sr.SKIPREPORTTRX == reportName.ToUpper() && sr.SKIPAGENT + "Agent" == agentName) {
                            return true;
                        }
                    }

                }
                return false;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return false;
        }

        /// <summary>
        ///Check Alarm Code Need To Report MES or OEE 
        /// False Need to Report
        /// True don’t Report
        /// </summary>
        /// <param name="lineName"></param>
        /// <param name="machineName"></param>
        /// <param name="unitID"></param>
        /// <param name="alarmCode"></param>
        /// <param name="agentName"></param>
        /// <returns></returns>
        public bool CheckSkipReportAlarmByID(string lineName, string machineName, string unitID, string alarmCode, string agentName) {
            try {
                if (_skipReports.ContainsKey(lineName)) {
                    List<SkipReportEntityData> _skiplist = _skipReports[lineName];

                    foreach (SkipReportEntityData sr in _skiplist) {
                        if (sr.NODEID == machineName && sr.UNITID == unitID && sr.SKIPCONDITION == alarmCode && sr.SKIPAGENT + "Agent" == agentName) {
                            return true;
                        }
                    }

                }
                return false;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return false;
        }

        /// <summary>
        /// Check Alarm Code Need To Report MES or OEE 
        /// False Need to Report
        /// True don’t Report
        /// </summary>
        /// <param name="lineName"></param>
        /// <param name="equipmentNo"></param>
        /// <param name="unitNo"></param>
        /// <param name="alarmCode"></param>
        /// <param name="agentName"></param>
        /// <returns></returns>
        public bool CheckSkipReportAlarmByNo(string lineName, string equipmentNo, string unitNo, string alarmCode, string agentName) {
            try {
                if (_skipReports.ContainsKey(lineName)) {
                    List<SkipReportEntityData> _skiplist = _skipReports[lineName];

                    foreach (SkipReportEntityData sr in _skiplist) {
                        if (sr.NODENO == equipmentNo && sr.UNITNO == unitNo && sr.SKIPCONDITION == alarmCode && sr.SKIPAGENT + "Agent" == agentName) {
                            return true;
                        }
                    }

                }
                return false;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return false;
        }

        public void SaveLineHistory(Line line) {

        }

        public void SaveTerminalMessageHistory( string trxId, string lineId, string caption,string msg) {
            try {
                TerminalMessageHistory tmh = new TerminalMessageHistory();
                tmh.CAPTION = caption;
                tmh.LINEID = lineId;
                tmh.TERMINALTEXT = msg;
                tmh.TRANSACTIONID = trxId;
                tmh.UPDATETIME = DateTime.Now;
                HistoryHibernateAdapter.SaveObject(tmh);
            } catch (Exception ex) {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// Get Run Mode Check Rule Entity 
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="runMode"></param>
        /// <returns></returns>
        public RunModeCheckRuleEntityData GetRunModeCheckRule(string lineId, string runMode)
        {
            if (_runModeCheckRules.ContainsKey(lineId))
            {
                if (_runModeCheckRules[lineId].ContainsKey(runMode))
                {
                    return _runModeCheckRules[lineId][runMode];
                }
            }
            return null;
        }

        public Dictionary<string, SortGradeRule> GetSortGradeRule(string lineId)
        {
            if (_sortGradeRules.ContainsKey(lineId))
            {
                return _sortGradeRules[lineId];
            }
            return new Dictionary<string, SortGradeRule>();
        }

        public SortGradeRule GetSortGradeRule(string lineid, string sortGrade)
        {
            if (_sortGradeRules.ContainsKey(lineid))
            {
                if (_sortGradeRules[lineid].ContainsKey(sortGrade))
                {
                    return _sortGradeRules[lineid][sortGrade];
                }
            }
            return null;
        }
    }
}
