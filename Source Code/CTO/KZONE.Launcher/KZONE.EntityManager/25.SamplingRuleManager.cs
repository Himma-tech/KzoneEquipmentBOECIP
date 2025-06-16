using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KZONE.Entity;
using System.Collections;
using System.Reflection;
using System.Data;
using KZONE.Log;
using KZONE.Work;
using Quartz;
using System.Xml;
using System.IO;
using System.Threading;


namespace KZONE.EntityManager
{
    public class SamplingRuleManager:EntityManager,IDataSource,IDisposable
    {
        private Dictionary<string, RecipeEntityData> _recipeTable = new Dictionary<string, RecipeEntityData>();
        private Dictionary<string, SamplingEntityData> _samplingData = new Dictionary<string, SamplingEntityData>();
        private Dictionary<string,List<SamplingRateEntityData>> _samplingRate = new Dictionary<string,List<SamplingRateEntityData>>();
        private Dictionary<string, Dictionary<string,BatchEntityFile>> _batchData = new Dictionary<string, Dictionary<string,BatchEntityFile>>();
        private Dictionary<string, List<Subscriber>> _subscribers = new Dictionary<string, List<Subscriber>>();
        private IScheduler _scheduler = null;

        public override EntityManager.FILE_TYPE GetFileType() {
            return FILE_TYPE.BIN;
        }

        protected override string GetSelectHQL() {
            return string.Format("from RecipeEntityData where SERVERNAME = '{0}'", BcServerName);
        }

        protected override Type GetTypeOfEntityData() {
            return null;
        }
        protected override Type GetTypeOfEntityFile() {
            return typeof(BatchEntityFile);
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames) {
            Filenames = new List<string>();
            Filenames.Add("*.bin");
        }

       
        protected override EntityFile NewEntityFile(string Filename) {
            return null;
        }

        protected override void AfterInit(List<EntityData> entityDatas, List<EntityFile> entityFiles) {
            foreach (EntityFile file in entityFiles){
                try {
                    BatchEntityFile bef = file as BatchEntityFile;
                    if (bef != null) {
                        Dictionary<string, BatchEntityFile> temp;
                        if (_batchData.ContainsKey(bef.StartEquipmentNo)) {
                            temp = _batchData[bef.StartEquipmentNo];
                        } else {
                            temp = new Dictionary<string, BatchEntityFile>();
                            _batchData.Add(bef.StartEquipmentNo, temp);
                        }
                        if (temp.ContainsKey(bef.BatchId)) {
                            temp.Remove(bef.BatchId);
                        }
                        temp.Add(bef.BatchId, bef);
                    }
                }catch (System.Exception ex){
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

                }
            }
           // LoadRecipe();
            //LoadSamplingData();
            LoadSamplingRate();
            LoadSubscriberFromFile();

            ITrigger trigger = new TimerTrigger("DeleteBatch", "1H", false);
            IJob job = new MethodJob("DeleteBatchFile", this, "DeleteBatchFile", null);
            _scheduler = SchedulerManager.Instance.CreateDefaultScheduler(trigger, job);
            SchedulerManager.Instance.AddScheduler(_scheduler);

        }

        /// <summary>
        /// Loader Sampling Data 
        /// </summary>
        public void LoadSamplingData() {
            Dictionary<string, SamplingEntityData> samplingData = new Dictionary<string, SamplingEntityData>();
            string hql = string.Format("from SamplingEntityData where SERVERNAME = '{0}'", BcServerName);
            IList list = this.HibernateAdapter.GetObjectByQuery(hql);
           
            if (list != null) {
                foreach (SamplingEntityData sed in list) {
                    if(samplingData.ContainsKey(sed.SAMPLINGINDEX)){
                        samplingData.Remove(sed.SAMPLINGINDEX);
                    }
                    samplingData.Add(sed.SAMPLINGINDEX, sed);
                }
            }
            if (samplingData.Count > 0) {
                lock (_samplingData) {
                    _samplingData = samplingData;
                }
            }
        }

        /// <summary>
        /// Load  Sampling Rate
        /// </summary>
        public void LoadSamplingRate() {
            Dictionary<string, List<SamplingRateEntityData>> samplingRate = new Dictionary<string, List<SamplingRateEntityData>>();
            string hql = string.Format("from SamplingRateEntityData where SERVERNAME='{0}'", BcServerName);
            IList list = this.HibernateAdapter.GetObjectByQuery(hql);
            if (list != null) {
                foreach (SamplingRateEntityData data in list) {
                    List<SamplingRateEntityData> samplingRates;
                    if (samplingRate.ContainsKey(data.BATCHNODENO)) {
                        samplingRates = samplingRate[data.BATCHNODENO];
                    } else {
                        samplingRates = new List<SamplingRateEntityData>();
                        samplingRate.Add(data.BATCHNODENO, samplingRates);
                    }
                    samplingRates.Add(data);
                }

                if (samplingRate.Count > 0) {
                    lock (_samplingRate) {
                        _samplingRate = samplingRate;
                    }
                }
            }
        }

        /// <summary>
        /// Load Recipe Table
        /// </summary>
        //public void LoadRecipe() {
        //    Dictionary<string, RecipeEntityData> recipeEntityDatas = new Dictionary<string, RecipeEntityData>();
        //    string hql = string.Format("from RecipeEntityData where LINETYPE='{0}'", Workbench.LineType);
        //    IList list = this.HibernateAdapter.GetObjectByQuery(hql);

        //    if (list != null) {
        //        foreach (RecipeEntityData sed in list) {
        //            // Montor 和Onlne  下Dispatch Rule 不一样 2017 -05-14
        //            string key = sed.ONLINECONTROLSTATE + "_" + sed.LINERECIPENAME;
        //            if (recipeEntityDatas.ContainsKey(key)) {
        //                recipeEntityDatas.Remove(key);
        //            }
        //            recipeEntityDatas.Add(key, sed);
        //        }
        //    }
        //    if (recipeEntityDatas.Count > 0) {
        //        lock (_recipeTable) {
        //            _recipeTable = recipeEntityDatas;
        //        }
        //    }
        //    LoadSamplingData();
        //}

        /// <summary>
        /// Get  Sampling Data
        /// </summary>
        /// <param name="lineRecipeName"></param>
        /// <returns></returns>
        public SamplingEntityData GetSamplingEntityData(string lineRecipeName) {
            if (_samplingData.ContainsKey(lineRecipeName)) {
                return _samplingData[lineRecipeName];
            }
            return null;
        }

        /// <summary>
        /// get Sampling  By BatchNode No
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public List<SamplingRateEntityData> GetSamplingRate(string batchEQNo) {
            if (_samplingRate.ContainsKey(batchEQNo)) {
                return _samplingRate[batchEQNo];
            }
            return null;
        }

        public bool HasSamplingRate(string eqpNo) {
            return _samplingRate.ContainsKey(eqpNo);
        }

        /// <summary>
        /// Check  是否 Inspection  Equipment
        /// </summary>
        /// <param name="nodeNo">Inspection  Equipment  No</param>
        /// <returns></returns>
        public bool IsInsptectionEquipment(string nodeNo) {
            foreach (List<SamplingRateEntityData> list in _samplingRate.Values) {
                foreach (SamplingRateEntityData d in list) {
                    if (d.INSPECTIONNODE == nodeNo)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get Sampling Rate By Inspection
        /// </summary>
        /// <param name="nodeNo"></param>
        /// <returns></returns>
        public List<SamplingRateEntityData> GetSamplingRateByInspection(string nodeNo) {

            List < SamplingRateEntityData > list= new List<SamplingRateEntityData>();
            foreach (List<SamplingRateEntityData> lst in _samplingRate.Values) {
                foreach (SamplingRateEntityData d in lst) {
                    if (d.INSPECTIONNODE == nodeNo)
                        list.Add(d);
                }
            }
            if (list.Count == 0)
                return null;
            return list;
        }

        /// <summary>
        /// Get Batch by Equipment No  and Batch ID 
        /// </summary>
        /// <param name="eqpNo">Batch Start Equipment No</param>
        /// <param name="batchId">Batch ID </param>
        /// <returns></returns>
        public BatchEntityFile GetBatch(string eqpNo, string batchId) {
            if (_batchData.ContainsKey(eqpNo)) {
                if (_batchData[eqpNo].ContainsKey(batchId)) {
                    return _batchData[eqpNo][batchId];
                }
            }
            return null;
        }
        
        /// <summary>
        /// Get Batch By Equipment No 
        /// </summary>
        /// <param name="eqpNo">Batch Start Equipment No</param>
        /// <returns></returns>
        public Dictionary<string,BatchEntityFile> GetBatch(string eqpNo) {
            if (_batchData.ContainsKey(eqpNo)) {
                return _batchData[eqpNo];
            }
            return null;
        }

        /// <summary>
        /// Get Batch By BatchID
        /// </summary>
        /// <param name="batchID"></param>
        /// <returns></returns>
        public BatchEntityFile GetBatchByID(string batchID) {
            foreach (string key in _batchData.Keys) {
                if (_batchData[key].ContainsKey(batchID)) {
                    return _batchData[key][batchID];
                }
            }
            return null;
        }

        /// <summary>
        /// Delete Batch By  EqpNo,Batch ID 
        /// </summary>
        /// <param name="eqpNo"></param>
        /// <param name="batchId"></param>
        public void DeleteBatch(string eqpNo, string batchId) {

            if (_batchData.ContainsKey(eqpNo)) {

                if (_batchData[eqpNo].ContainsKey(batchId)) {
                    BatchEntityFile batch = _batchData[eqpNo][batchId];
                    lock (_batchData[eqpNo]) {
                        _batchData[eqpNo].Remove(batchId);
                    }
                    lock (batch) {
                        batch.WriteFlag = false;
                    }
                    this.EnqueueSave(batch);
                }

            }
            
        }

        /// <summary>
        /// delete batch By eqp No
        /// </summary>
        /// <param name="eqpNo"></param>
        public void DeleteBatch(string eqpNo) {
            if (_batchData.ContainsKey(eqpNo)) {

                Dictionary<string, BatchEntityFile> dic = _batchData[eqpNo];
                lock (_batchData) {
                    _batchData.Remove(eqpNo);
                }
                foreach (string key in dic.Keys) {
                    lock (dic[key]) {
                        dic[key].WriteFlag = false;
                    }
                    EnqueueSave(dic[key]);
                }

            }
        }

        public void DeleteBatchByID(string batchID) {
            BatchEntityFile batch = GetBatchByID(batchID);
           
            if (batch != null) {
                DeleteBatch(batch.StartEquipmentNo, batchID);
            }
        }

        public void DeleteBatch(BatchEntityFile batch) {
            DeleteBatch(batch.StartEquipmentNo, batch.BatchId);
        }

        public void DeleteBatchFile() {
            try {
                if (_batchData != null && _batchData.Count > 0) {
                    DateTime now = DateTime.Now;
                    List<BatchEntityFile> deleteBatch = new List<BatchEntityFile>();
                    lock (_batchData) {
                        foreach (string key in _batchData.Keys) {
                            foreach (BatchEntityFile batch in _batchData[key].Values) {
                                if ((batch.BatchStatus == eBatchStatus.End && batch.SamplingStatus == eSamplingStatus.End) ||
                                    batch.UpdateTime.AddDays(14).CompareTo(now) < 0) {
                                    deleteBatch.Add(batch);
                                }
                            }
                        }
                    }
                    foreach(BatchEntityFile b in deleteBatch){
                        DeleteBatch(b);
                    }
                }

            }catch (System.Exception ex){
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        public void AddBatch(BatchEntityFile batch) {
            if (_batchData.ContainsKey(batch.StartEquipmentNo)) {
                lock (_batchData[batch.StartEquipmentNo]) {
                    if (_batchData[batch.StartEquipmentNo].ContainsKey(batch.BatchId)) {
                        _batchData[batch.StartEquipmentNo].Remove(batch.BatchId);
                    }
                    _batchData[batch.StartEquipmentNo].Add(batch.BatchId, batch);
                }
            } else {
                Dictionary<string, BatchEntityFile> dic = new Dictionary<string, BatchEntityFile>();
                dic.Add(batch.BatchId, batch);
                lock (_batchData) {
                    _batchData.Add(batch.StartEquipmentNo,dic);
                }
            }
            EnqueueSave(batch);
        }

        /// <summary>
        /// get Recipe Necipe需要使用ONline Control Mode +“_"+recipeName
        /// </summary>
        /// <param name="recipeName"></param>
        /// <returns></returns>
        public RecipeEntityData GetRecipe(string recipeName) {
            if (_recipeTable.ContainsKey(recipeName)) {
                return _recipeTable[recipeName];
            }
            return null;
        }

        /// <summary>
        /// 是否有这样的Recipe Name 需要使用ONline Control Mode +“_"+recipeName
        /// </summary>
        /// <param name="recipeName"></param>
        /// <returns></returns>
        public bool HasRecipe(string recipeName) {
            return _recipeTable.ContainsKey(recipeName);
        }

        /// <summary>
        /// 將EntityFile放入Queue, 由Thread存檔
        /// </summary>
        /// <param name="file">JobEntityFile(job.File)</param>
        public override void EnqueueSave(EntityFile file) {
            if (file is BatchEntityFile) {
                BatchEntityFile batch = file as BatchEntityFile;

                lock (batch) {
                    batch.UpdateTime = DateTime.Now;
                }

                string key = string.Format("{0}_{1}",batch.StartEquipmentNo, batch.BatchId);
                string fname = string.Format("{0}.bin", key);
                file.SetFilename(fname);
                base.EnqueueSave(file);
            }
        }

        public System.Data.DataTable GetDataTable(string entityName)
        {
            switch (entityName)
            {
                case "BatchManager":
                   return  GetDataTableForBatch();
                case "SubscriberManager":
                    return GetDataTableForSubscriber();
                case "RecipeManager":
                    return GetDataTableForRecipe();
                case "SamplingManager":
                    return GetDataTableForSampling();
                default :
                    return null;
            }
        }

        /// <summary>
        /// Add Recipe Table for Monitor
        /// </summary>
        /// <returns></returns>
        private System.Data.DataTable GetDataTableForBatch()
        {
            try
            {
                DataTable dt = new DataTable();
                BatchEntityFile file = new BatchEntityFile();
                DataTableHelp.DataTableAppendColumn(file, dt);
                foreach (Dictionary<string, BatchEntityFile> list in _batchData.Values)
                {
                    foreach (BatchEntityFile b in list.Values)
                    {
                        DataRow dr = dt.NewRow();
                        DataTableHelp.DataRowAssignValue(b, dr);
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            } catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        private System.Data.DataTable GetDataTableForSubscriber()
        {
            try
            {
                DataTable dt = new DataTable();
                Subscriber file = new Subscriber();
                DataTableHelp.DataTableAppendColumn(file, dt);

                foreach (List<Subscriber> list in _subscribers.Values)
                {
                    foreach (Subscriber b in list)
                    {
                        DataRow dr = dt.NewRow();
                        DataTableHelp.DataRowAssignValue(b, dr);
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            } catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        private System.Data.DataTable GetDataTableForRecipe() {
            try {
                DataTable dt = new DataTable();
                RecipeEntityData entity = new RecipeEntityData();
                DataTableHelp.DataTableAppendColumn(entity, dt);

                foreach (RecipeEntityData d in _recipeTable.Values) {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(d, dr);
                    dt.Rows.Add(dr);

                }
                return dt;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }
        private System.Data.DataTable GetDataTableForSampling() {
            try {
                DataTable dt = new DataTable();
                SamplingEntityData entity = new SamplingEntityData();
                DataTableHelp.DataTableAppendColumn(entity, dt);

                foreach (SamplingEntityData d in _samplingData.Values) {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(d, dr);
                    dt.Rows.Add(dr);

                }
                return dt;
            } catch (System.Exception ex) {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        public IList<string> GetEntityNames() {
            IList<string> entityNames = new List<string>();
            entityNames.Add("BatchManager");
            entityNames.Add("SubscriberManager");
            entityNames.Add("RecipeManager");
            entityNames.Add("SamplingManager");
            return entityNames;
        }

        /// <summary>
        /// Update  Sampling Entity Data
        /// </summary>
        /// <param name="data"></param>
        public void SaveOrUpdate(Line line, string lineRecipeName, string zi, string mu, string ip) {
            SamplingEntityData sd = GetSamplingEntityData(lineRecipeName+"_"+line.File.HostMode.ToString());
            if (sd != null) {
                sd.SAMPLINGFENZI = zi;
                sd.SAMPLINGFENMU = mu;
                sd.TRXIP = ip;
                sd.TRXDATE = DateTime.Now;
                HibernateAdapter.UpdateObject(sd);
            } else {
                sd = new SamplingEntityData();
                sd.SAMPLINGFENZI = zi;
                sd.SAMPLINGFENMU = mu;
                sd.TRXIP = ip;
                sd.TRXUSERID = "BCS";
                sd.SERVERNAME = Workbench.ServerName;
                sd.SAMPLINGINDEX = lineRecipeName + "_" + line.File.HostMode.ToString();
                HibernateAdapter.SaveObject(sd);
                
                LoadSamplingData();
            }
        }

        /// <summary>
        /// Add Subcriber  by inspection Equipment No 
        /// </summary>
        /// <param name="subcriber"></param>
        public void AddSubscriber(Subscriber subcriber)
        {
            lock (_subscribers)
            {
                if (!_subscribers.ContainsKey(subcriber.InspectionEquipmentNo))
                {
                    List<Subscriber> list = new List<Subscriber>();
                    _subscribers.Add(subcriber.InspectionEquipmentNo, list);

                }
                _subscribers[subcriber.InspectionEquipmentNo].Add(subcriber);
            }
        }

        public void AddSubscriber(List<Subscriber> subcribers)
        {
            foreach (Subscriber s in subcribers)
            {
                if (_subscribers.ContainsKey(s.InspectionEquipmentNo))// 删除当前的全部的同类型的Subscriber
                {
                    List<Subscriber> old = _subscribers[s.InspectionEquipmentNo].Where(so => so.SubscribeType == s.SubscribeType).ToList();
                    lock (_subscribers)
                    {
                        foreach(Subscriber o in old)
                            _subscribers[o.InspectionEquipmentNo].Remove(o);
                    }
                }
               
            }
            foreach (Subscriber s in subcribers)
            {
                AddSubscriber(s);
            }


            WriteSubscriberToFile();
        }

        /// <summary>
        /// Remove Subscriber
        /// </summary>
        /// <param name="subcriber"></param>
        public void RemoveSubscriber(Subscriber subcriber)
        {
            _subscribers[subcriber.InspectionEquipmentNo].Remove(subcriber);
            WriteSubscriberToFile();
        }


        public List<Subscriber> GetSubscriberByInspectionEqNo(string inspectionEqNo)
        {
            if (_subscribers.ContainsKey(inspectionEqNo))
            {
                return _subscribers[inspectionEqNo];
            }
            return null;
        }
        
        /// <summary>
        /// Load Subscriber From File 
        /// </summary>
        public void LoadSubscriberFromFile()
        {
            string fileName = this.DataFilePath + "subscriber.xml";
            if (!File.Exists(fileName))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode root = doc.DocumentElement;
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    RetrieveSubscriber(child);
                }
            }

        }

        private void RetrieveSubscriber(XmlNode parentNode)
        {
            Subscriber subscriber = new Subscriber();
            subscriber.InspectionEquipmentNo = parentNode.Attributes["inspectionEquipmentNo"].Value;
            subscriber.SubscribeType = (eSubscribeType)Enum.Parse(typeof(eSubscribeType), parentNode.Attributes["type"].Value);
            subscriber.State = (eSubscribeState)Enum.Parse(typeof(eSubscribeState), parentNode.Attributes["state"].Value);
            if (parentNode.HasChildNodes)
            {
                foreach (XmlNode node in parentNode.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (node.NodeType == XmlNodeType.Element)
                            {
                                string key = childNode.Attributes["key"].Value;
                                string value = childNode.Attributes["value"].Value;
                                subscriber.Parameters.Add(key, value);
                            }
                        }
                    }
                }
                AddSubscriber(subscriber);
            }
        }

        /// <summary>
        /// Write Subscriber To File
        /// </summary>
        /// <returns></returns>
        public XmlDocument WriteSubscriberToFile()
        {
            try
            {
                string fileName = this.DataFilePath + "subscriber.xml";
                string tempFileName = this.DataFilePath + "subscriberTemp.xml";
                FileAttributes attributes = File.GetAttributes(fileName);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                }

                StringBuilder output = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment
                };
                XmlWriter xmlWriter = XmlWriter.Create(output, settings);
                this.WriteTo(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
                XmlDocument document = new XmlDocument();
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "utf-8", null);
                document.LoadXml(output.ToString());
                document.InsertBefore(declaration, document.DocumentElement);
                document.Save(tempFileName);
                if (File.Exists(fileName))
                    File.Delete(fileName);
                while (File.Exists(fileName))
                    Thread.Sleep(1);
                File.Move(tempFileName, fileName);
                return document;
            } catch
            {
                return null;
            }
        }

        private void WriteTo(XmlWriter writer)
        {
            writer.WriteStartElement("subcribers");
            foreach (string key in this._subscribers.Keys)
            {
                foreach (Subscriber s in _subscribers[key])
                {
                    s.WriteTo(writer);
                }
            }
            writer.WriteEndElement();
        }

        public void Dispose()
        {
            if (_scheduler != null)
            {
                SchedulerManager.Instance.RemoveScheduler(_scheduler);
            }
        }
    }
}
