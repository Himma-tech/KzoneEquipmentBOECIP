using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;
using System.Xml;
using KZONE.Work;
using System.Collections;

namespace KZONE.EntityManager
{
    public class CassetteManager : EntityManager, IDataSource
    {
        private Dictionary<string, Cassette> _entities = new Dictionary<string, Cassette>();

        private string _CompleteCSTPath = string.Empty;
        private string _IncompleteCSTPath = string.Empty;
        private string _LotEndExecuteCSTPath = string.Empty;

        public string CompleteCSTPath 
        {
            get { return this._CompleteCSTPath.Replace("{ServerName}", Workbench.ServerName); }
            set { this._CompleteCSTPath = value; }
        }

        public string IncompleteCSTPath
        {
            get { return this._IncompleteCSTPath.Replace("{ServerName}", Workbench.ServerName); }
            set { this._IncompleteCSTPath = value; }
        }

        public string LotEndExecuteCSTPath
        {
            get { return this._LotEndExecuteCSTPath.Replace("{ServerName}", Workbench.ServerName); }
            set { this._LotEndExecuteCSTPath = value; }
        }

        #region Implement EntityManager
        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.BIN;
        }

        protected override Type GetTypeOfEntityData()
        {
            return null;// typeof(CassetteEntityData);
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(Cassette);
        }

        protected override void AfterInit(List<EntityData> entityDatas, List<EntityFile> entityFiles)
        {           
            foreach (EntityFile file in entityFiles)
            {
                Cassette cst = file as Cassette;
                if (!_entities.ContainsKey(cst.CassetteSequenceNo))
                    _entities.Add(cst.CassetteSequenceNo, cst);
            }
        }

        protected override string GetSelectHQL() {
            return string.Empty;
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames) {
            Filenames = new List<string>();
            Filenames.Add("*.Bin");
        }

        protected override EntityFile NewEntityFile(string Filename) {
            return null;
        }

        #endregion

        public Cassette GetCassette(string lineID, string eqpNo, string portNo)
        {
            Cassette ret = null;
            foreach (Cassette entity in _entities.Values)
            {
                if (entity.LineID == lineID && entity.NodeNo == eqpNo && entity.PortNo == portNo)
                {
                    ret = entity;
                    break;
                }
            }
            return ret;
        }

        public Cassette GetCassette(string eqpID, string portNo)
        {
            Cassette ret = null;
            foreach (Cassette entity in _entities.Values)
            {
                if (entity.NodeID == eqpID && entity.PortNo == portNo)
                {
                    ret = entity;
                    break;
                }
            }
            return ret;
        } 

        public Cassette GetCassette(string cstID)
        {
            Cassette ret = null;
            //watson modify 20141120 For 避免找不到Cassette
            //ret = _entities.Values.FirstOrDefault(c => c.CassetteID.Equals(cstID));
            ret = _entities.Values.FirstOrDefault(c => c.CassetteID.Trim().Equals(cstID.Trim()));
            return ret;
        }

        public Cassette GetCassette(int cassetteSequenceNo)
        {
            Cassette ret = null;
            if (_entities.ContainsKey(cassetteSequenceNo.ToString()))
                ret = _entities[cassetteSequenceNo.ToString()];
            return ret;
        }

        public void CreateCassette(Cassette cassette)
        {
            lock (_entities)
            {
                if (_entities.ContainsKey(cassette.CassetteSequenceNo))
                    _entities.Remove(cassette.CassetteSequenceNo);

                IList<Cassette> csts = _entities.Values.Where(c =>
                    c.CassetteID.Trim().Equals(cassette.CassetteID.Trim())).ToList<Cassette>();

                foreach (Cassette cst in csts)
                {
                    DeleteCassette(cst.CassetteSequenceNo);
                }

                cassette.SetFilename(string.Format("{0}_{1}_{2}.bin", cassette.PortID, cassette.CassetteSequenceNo, cassette.CassetteID));
                _entities.Add(cassette.CassetteSequenceNo, cassette);
                EnqueueSave(cassette);
            }
        }


        public void DeleteCassette(string CassetteSequenceNo)
        {
            lock (_entities)
            {
                if (_entities.ContainsKey(CassetteSequenceNo))
                {
                    Cassette cst = _entities[CassetteSequenceNo];
                    _entities.Remove(CassetteSequenceNo);
                    cst.WriteFlag = false;
                    EnqueueSave(cst);
                }
            }
        }

      
        public List<Cassette> GetCassettes()
        {
            List<Cassette> ret = new List<Cassette>();
            foreach (Cassette entity in _entities.Values)
            {
                ret.Add(entity);
            }
            return ret;
        }

        

        #region Implement IDataSource 
        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("CassetteManager");
            return entityNames;
        }

        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                Cassette file = new Cassette();
                DataTableHelp.DataTableAppendColumn(file, dt);

                IList<Cassette> cst_entities = GetCassettes();
                foreach (Cassette cst in cst_entities)
                {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(cst, dr);
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
           
        }
        #endregion
        
        #region InCompleteCassette handler

        /// <summary> 
        /// LotEndExecute - [XmlDocument Data] ->  [LotEndExecute]
        /// 描述：Xml資料寫入作業。
        /// 1. 將傳入的 XmlDocument 資料寫入至設定的 LotEndExecuteCSTPath 路徑中。
        /// 2. Xml 檔名格式為 Pord ID + Cassette ID + Trx ID。
        /// 3. 若目標位置有相同的檔案，會將其覆蓋。
        /// </summary>
        /// <param name="strPordID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="xmlDocument">已組好的 XmlDocument</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool FileSaveToLotEndExecute(string strPordID, string strCassetteID, string strMesTrxID, XmlDocument xmlDocument, out string strDescription, string strFileType="")
        {
            try
            {
                if (string.IsNullOrEmpty(this.LotEndExecuteCSTPath))
                {
                    string strErr = string.Format("FileSaveToLotEndExecute NG - CassetteManager.LotEndExecuteCSTPath is null or empty!  ,PordID:{0}, CassetteID:{1}, MesTrxID:{2}"
                        , strPordID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }

                if (this.LotEndExecuteCSTPath[this.LotEndExecuteCSTPath.Length - 1] != '\\')
                    this.LotEndExecuteCSTPath += "\\";

                if (Directory.Exists(this.LotEndExecuteCSTPath) == false)
                    Directory.CreateDirectory(this.LotEndExecuteCSTPath);

                StringBuilder sbErr = new StringBuilder();
                if(string.IsNullOrEmpty(strPordID.Trim()))
                    sbErr.Append("[PordID is null or empty] ");
                if (string.IsNullOrEmpty(strCassetteID.Trim()))
                    sbErr.Append("[CassetteID is null or empty] ");
                if (string.IsNullOrEmpty(strMesTrxID.Trim()))
                    sbErr.Append("[MesTrxID is null or empty] ");
                if (sbErr.Length > 0)
                {
                    string strErr = string.Format("FileSaveToLotEndExecute NG - Parameters Error - {0}", sbErr.ToString());
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }

                strPordID = strPordID.Trim();
                strCassetteID = strCassetteID.Trim();
                strMesTrxID = strMesTrxID.Trim();

                string strFilePath = string.Format("{0}{1}_{2}_{3}_{4}.xml", this.LotEndExecuteCSTPath, strFileType , strPordID, strCassetteID, strMesTrxID);
                xmlDocument.Save(strFilePath);
                strDescription = "FileSaveToLotEndExecute OK";
                return true;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("FileSaveToLotEndExecute NG - Exception Message:{0}  ,PordID:{1}, CassetteID:{2}, MesTrxID:{3}"
                    , ex.Message, strPordID, strCassetteID, strMesTrxID);
                return false;
            }
        }

        /// <summary> 
        /// FileMoveToCompleteCST - [LotEndExecute] -> [CompleteCST]
        /// 描述：檔案搬移作業。
        /// 1. 從 LotEndExecute\ Folder 中，將 Xml 文件搬移至 CompleteCST\yyyyMMdd\ Folder 內。
        /// 2. 尋找目標 Xml 文件的檔名格式為 Pord ID + Cassette ID + Trx ID。
        /// 3. 若目標位置有相同的檔案，會將其覆蓋。
        /// </summary>
        /// <param name="strPordID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool FileMoveToCompleteCST(string strPordID, string strCassetteID, string strMesTrxID, out string strDescription, string strFileType = "")
        {
            try
            {
                string strDate = strMesTrxID.Substring(0, 8);

                #region [Check CassetteManager Path]
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(this.LotEndExecuteCSTPath))
                    sb.Append("[CassetteManager.LotEndExecuteCSTPath is null or empty! - core.xml] ");
                if (string.IsNullOrEmpty(this.CompleteCSTPath))
                    sb.Append("[CassetteManager.CompleteCSTPath is null or empty! - core.xml] ");
                if (sb.Length > 0)
                {
                    string strErr = "FileMoveToCompleteCST NG - " + sb.ToString();
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                if (this.LotEndExecuteCSTPath[this.LotEndExecuteCSTPath.Length - 1] != '\\')
                    this.LotEndExecuteCSTPath += "\\";
                if (this.CompleteCSTPath[this.CompleteCSTPath.Length - 1] != '\\')
                    this.CompleteCSTPath += "\\";
                #endregion

                #region [Check File Name & Format]
                StringBuilder sbErr = new StringBuilder();
                if (string.IsNullOrEmpty(strPordID.Trim()))
                    sbErr.Append("[PordID is null or empty] ");
                if (string.IsNullOrEmpty(strCassetteID.Trim()))
                    sbErr.Append("[CassetteID is null or empty] ");
                if (string.IsNullOrEmpty(strMesTrxID.Trim()))
                    sbErr.Append("[MesTrxID is null or empty] ");
                if (sbErr.Length > 0)
                {
                    string strErr = string.Format("FileMoveToCompleteCST NG - Parameters Error - {0}", sbErr.ToString());
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }

                strPordID = strPordID.Trim();
                strCassetteID = strCassetteID.Trim();
                strMesTrxID = strMesTrxID.Trim();

                string strFileName = string.Format("{0}_{1}_{2}_{3}.xml", strFileType , strPordID, strCassetteID, strMesTrxID);
                string strTargetPath = string.Format("{0}{1}\\", this.CompleteCSTPath, strDate);
                string strSourceFile = string.Format("{0}{1}", this.LotEndExecuteCSTPath, strFileName);
                string strTargetFile = string.Format("{0}{1}", strTargetPath, strFileName);
                #endregion

                #region [File Process]
                //Create Folder
                if (Directory.Exists(strTargetPath) == false)
                    Directory.CreateDirectory(strTargetPath);
                //Check File Exists
                if (!File.Exists(strSourceFile))
                {
                    string strErr = string.Format("FileMoveToCompleteCST NG - Source File: {0} , It's not exist.  PordID:{1}, CassetteID:{2}, MesTrxID:{3}", strSourceFile, strPordID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                //Move File
                File.Delete(strTargetFile);
                File.Move(strSourceFile, strTargetFile);
                #endregion

                strDescription = "FileMoveToCompleteCST OK";
                return true;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("FileMoveToCompleteCST NG - Exception Message:{0} ,PordID:{1}, CassetteID:{2}, MesTrxID:{3}", ex.Message, strPordID, strCassetteID, strMesTrxID);
                return false;
            }
        }

        /// <summary> 
        /// FileMoveToIncompleteCST - [LotEndExecute] -> [IncompleteCST]
        /// 描述：檔案搬移作業。
        /// 1. 從 LotEndExecute\ Folder 中，將 Xml 文件搬移至 IncompleteCST\yyyyMMdd\ Folder 內。
        /// 2. 尋找目標 Xml 文件的檔名格式為 Pord ID + Cassette ID + Trx ID。
        /// 3. 若目標位置有相同的檔案，會將其覆蓋。
        /// </summary>
        /// <param name="strPordID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool FileMoveToIncompleteCST(string strPordID, string strCassetteID, string strMesTrxID, out string strDescription, string strFileType = "")
        {
            try
            {
                string strDate = strMesTrxID.Substring(0, 8);

                #region [Check CassetteManager Path]
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(this.LotEndExecuteCSTPath))
                    sb.Append("[CassetteManager.LotEndExecuteCSTPath is null or empty! - core.xml] ");
                if (string.IsNullOrEmpty(this.IncompleteCSTPath))
                    sb.Append("[CassetteManager.IncompleteCSTPath is null or empty! - core.xml] ");
                if (sb.Length > 0)
                {
                    string strErr = string.Format("FileMoveToIncompleteCST NG - {0} , PordID:{1}, CassetteID:{2}, MesTrxID:{3}", sb.ToString(), strPordID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                if (this.LotEndExecuteCSTPath[this.LotEndExecuteCSTPath.Length - 1] != '\\')
                    this.LotEndExecuteCSTPath += "\\";
                if (this.IncompleteCSTPath[this.IncompleteCSTPath.Length - 1] != '\\')
                    this.IncompleteCSTPath += "\\";
                #endregion

                #region [Check File Name & Format]

                StringBuilder sbErr = new StringBuilder();
                if (string.IsNullOrEmpty(strPordID.Trim()))
                    sbErr.Append("[PordID is null or empty] ");
                if (string.IsNullOrEmpty(strCassetteID.Trim()))
                    sbErr.Append("[CassetteID is null or empty] ");
                if (string.IsNullOrEmpty(strMesTrxID.Trim()))
                    sbErr.Append("[MesTrxID is null or empty] ");
                if (sbErr.Length > 0)
                {
                    string strErr = string.Format("FileMoveToIncompleteCST NG - Parameters Error - {0}", sbErr.ToString());
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }

                strPordID = strPordID.Trim();
                strCassetteID = strCassetteID.Trim();
                strMesTrxID = strMesTrxID.Trim();

                string strFileName = string.Format("{0}_{1}_{2}_{3}.xml", strFileType , strPordID, strCassetteID, strMesTrxID);
                string strTargetPath = string.Format("{0}{1}\\", this.IncompleteCSTPath, strDate);
                string strSourceFile = string.Format("{0}{1}", this.LotEndExecuteCSTPath, strFileName);
                string strTargetFile = string.Format("{0}{1}", strTargetPath, strFileName);
                #endregion

                #region [File Process]
                //Create Folder
                if (Directory.Exists(strTargetPath) == false)
                    Directory.CreateDirectory(strTargetPath);
                //Check File Exists
                if (!File.Exists(strSourceFile))
                {
                    string strErr = string.Format("FileMoveToIncompleteCST NG - Source File: {0} , It's not exist. PordID:{1}, CassetteID:{2}, MesTrxID:{3}", strSourceFile, strPordID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                //Move File
                File.Delete(strTargetFile);
                File.Move(strSourceFile, strTargetFile);
                #endregion

                strDescription = "FileMoveToIncompleteCST - OK";
                return true;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("FileMoveToIncompleteCST NG - Exception Message:{0} ,PordID:{1}, CassetteID:{2}, MesTrxID:{3}", ex.Message, strPordID, strCassetteID, strMesTrxID);
                return false;
            }
        }

        /// <summary>
        /// IncompleteCassetteDataReplyFull - [IncompleteCST] Return XmlDocument
        /// 描述：IncompleteCST Folder 下的 Xml 資料回傳作業。
        /// </summary>
        /// <param name="strXmlFileName"> XML 文件名稱 - 格式:[Name or Name.xml]</param>
        /// <param name="strDate">XML 文件所屬的資料夾名稱 - 格式:[yyyyMMdd]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <param name="xmlDocument">接收回傳的 XmlDocument 物件</param>1
        /// <returns></returns>
        public bool IncompleteCassetteDataReplyFull(string strXmlFileName, string strDate, out string strDescription, out XmlDocument xmlDocument)
        {
            try
            {
                #region [check CassetteManager path]
                if (string.IsNullOrEmpty(this.IncompleteCSTPath))
                {
                    string strErr = string.Format("IncompleteCassetteDataReplyFull NG - CassetteManager.IncompleteCSTPath is null or empty! - core.xml , XmlFileName:{0}, Date:{1}", strXmlFileName, strDate);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                if (this.IncompleteCSTPath[this.IncompleteCSTPath.Length - 1] != '\\')
                    this.IncompleteCSTPath += "\\";
                #endregion

                #region [check parameters]
                StringBuilder sbDescription = new StringBuilder();
                if (string.IsNullOrEmpty(strXmlFileName))
                    sbDescription.Append("[strXmlFileName is null or empty.] ");
                if (string.IsNullOrEmpty(strDate))
                    sbDescription.Append("[strDate is null or empty.] ");
                if (sbDescription.Length > 0)
                {
                    string strErr = string.Format("IncompleteCassetteDataReplyFull NG - Parameters Error - {0} , XmlFileName:{1}, Date:{2}", sbDescription.ToString(), strXmlFileName, strDate);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                strXmlFileName = strXmlFileName.Trim();
                strDate = strDate.Trim();
                #endregion

                #region [file name format]
                string _strXmlFileName = strXmlFileName;
                if (strXmlFileName.Length <= 4 || strXmlFileName.Substring(strXmlFileName.Length - 4, 4).ToLower() != ".xml")
                    _strXmlFileName += ".xml";
                string strFilePath = string.Format("{0}{1}\\{2}", this.IncompleteCSTPath, strDate, _strXmlFileName);
                #endregion

                #region [check file]
                if (!File.Exists(strFilePath))
                {
                    string strErr = string.Format("IncompleteCassetteDataReplyFull NG - File is not Exist. XmlFileName:{0}, Date:{1}", strXmlFileName, strDate);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                #endregion

                //讀取 Incomplete path 中指定的檔案，並回傳 XmlDocument ]
                xmlDocument = new XmlDocument();
                xmlDocument.Load(strFilePath);
                strDescription = "IncompleteCassetteDataReplyFull OK";
                return true;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("IncompleteCassetteDataReplyFull NG - Exception Message:{0} , XmlFileName:{1}, Date:{2}", ex.Message, strXmlFileName, strDate);
                xmlDocument = null;
                return false;
            }
        }

        /// <summary>
        /// IncompleteCassetteDataReply - [IncompleteCST] Return XmlDocument
        /// 描述：IncompleteCST Folder 下的 Xml 資料回傳作業。
        /// </summary>
        /// <param name="strPortID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="strXmlFileName"> XML 文件名稱 - 格式:[Name or Name.xml]</param>
        /// <param name="strDate">XML 文件所屬的資料夾名稱 - 格式:[yyyyMMdd]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <param name="xmlDocument">OPI XML - IncompleteCassetteDataReply</param>
        /// <returns></returns>
        public bool IncompleteCassetteDataReply(string logoffType, string strPortID, string strCassetteID, string strMesTrxID, string strXmlFileName, string strDate, out string strDescription, out XmlDocument xmlDocument)
        {
            try
            {
                strDate = strMesTrxID.Substring(0, 8);

                #region [check CassetteManager path]
                if (string.IsNullOrEmpty(this.IncompleteCSTPath))
                {
                    string strErr = string.Format("IncompleteCassetteDataReply NG - IncompleteCSTPath_Error - [CassetteManager.IncompleteCSTPath is null or empty. PortID:{0}, CassetteID:{1}, MesTrxID:{2}, XmlFileName:{3}, Date:{4}]"
                        , strPortID, strCassetteID, strMesTrxID, strXmlFileName, strDate);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                if (this.IncompleteCSTPath[this.IncompleteCSTPath.Length - 1] != '\\')
                    this.IncompleteCSTPath += "\\";

                
                #endregion

                #region [check parameters]
                StringBuilder sbDescription = new StringBuilder();
                if (string.IsNullOrEmpty(logoffType))
                    sbDescription.Append("[logoffType is null or empty.] ");
                if (string.IsNullOrEmpty(strPortID))
                    sbDescription.Append("[strPordID is null or empty.] ");
                if (string.IsNullOrEmpty(strCassetteID))
                    sbDescription.Append("[strCassetteID is null or empty.] ");
                if (string.IsNullOrEmpty(strMesTrxID))
                    sbDescription.Append("[strMesTrxID is null or empty.] ");
                if (string.IsNullOrEmpty(strXmlFileName))
                    sbDescription.Append("[strFileName is null or empty.] ");
                if (string.IsNullOrEmpty(strDate))
                    sbDescription.Append("[strDate is null or empty.] ");
                if (sbDescription.Length > 0)
                {
                    string strErr = string.Format("IncompleteCassetteDataReply NG - Parameters_Error - {0} , logoffType{6}, PortID:{1}, CassetteID:{2}, MesTrxID:{3}, XmlFileName:{4}, Date:{5}"
                        , sbDescription.ToString(), strPortID, strCassetteID, strMesTrxID, strXmlFileName, strDate, logoffType);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                strPortID = strPortID.Trim();
                strCassetteID = strCassetteID.Trim();
                strMesTrxID = strMesTrxID.Trim();
                strXmlFileName = strXmlFileName.Trim();
                strDate = strDate.Trim();
                #endregion

                #region [file name format]
                string _strXmlFileName = strXmlFileName;
                if (strXmlFileName.Length <= 4 || strXmlFileName.Substring(strXmlFileName.Length - 4, 4).ToLower() != ".xml")
                    _strXmlFileName += ".xml";

                //string strFilePath = string.Format("{0}{1}\\{2}", this.IncompleteCSTPath, strDate, _strXmlFileName);
                string strFilePath = string.Format("{0}{1}\\{2}", this.IncompleteCSTPath,strDate, _strXmlFileName);
                 #endregion

                #region [check file]
                if (!File.Exists(strFilePath))
                {
                    string strErr = string.Format("IncompleteCassetteDataReply NG - File_Error - [File is not exist in =[{0}], logoffType{6}, PortID:{1}, CassetteID:{2}, MesTrxID:{3}, XmlFileName:{4}, Date:{5}]"
                        , strFilePath, strPortID, strCassetteID, strMesTrxID, strXmlFileName, strDate, logoffType);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                #endregion

                #region [讀取 Incomplete path 中指定的檔案，並結創建回傳的 XmlDocument 結構]
                //建立要回傳的 XmlDocument
                XmlDocument xmlDocumentReturn = new XmlDocument();

                xmlDocumentReturn.Load(strFilePath);
                #endregion

                //設定回傳值
                xmlDocument = xmlDocumentReturn;
                strDescription = "IncompleteCassetteDataReply OK";
                return true;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("IncompleteCassetteDataReply NG - Exception_Error - [Message:{0} ,PortID:{1}, CassetteID:{2}, MesTrxID:{3}, XmlFileName:{4}, Date:{5}]"
                    , ex.Message, strPortID, strCassetteID, strMesTrxID, strXmlFileName, strDate);
                xmlDocument = null;
                return false;
            }
        }

        /// <summary>
        /// IncompleteCassetteEditSaveReply
        /// 描述: OPI 上傳要回存至 Incomplete Xml 檔案的 XmlDocument 資料。
        /// </summary>
        /// <param name="strXmlFileName"> XML 文件名稱 - 格式:[Name or Name.xml]</param>
        /// <param name="strDate">XML 文件所屬的資料夾名稱 - 格式:[yyyyMMdd]</param>
        /// <param name="xmlDocument">OPI XML - IncompleteCassetteEditSave</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool IncompleteCassetteEditSaveReply(string strPortID, string strCassetteID, string strMesTrxID, string strXmlFileName, string strDate, XmlDocument xmlDocument, out string strDescription)
        {
            try
            {
                strDate = strMesTrxID.Substring(0, 8);

                #region [check CassetteManager path]
                if (string.IsNullOrEmpty(this.IncompleteCSTPath))
                {
                    string strErr = string.Format("IncompleteCassetteEditSaveReply NG - IncompleteCSTPath_Error - [CassetteManager.IncompleteCSTPath is null or empty. XmlFileName:{0}, Date:{1}, PortID:{2}, CassetteID:{3}, MesTrxID:{4}]"
                        , strXmlFileName, strDate, strPortID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                if (this.IncompleteCSTPath[this.IncompleteCSTPath.Length - 1] != '\\')
                    this.IncompleteCSTPath += "\\";
                #endregion

                #region [check parameters]
                StringBuilder sbDescription = new StringBuilder();
                if (string.IsNullOrEmpty(strPortID))
                    sbDescription.Append("[strPortID is null or empty.] ");
                if (string.IsNullOrEmpty(strCassetteID))
                    sbDescription.Append("[strCassetteID is null or empty.] ");
                if (string.IsNullOrEmpty(strMesTrxID))
                    sbDescription.Append("[strMesTrxID is null or empty.] ");
                if (string.IsNullOrEmpty(strXmlFileName))
                    sbDescription.Append("[strFileName is null or empty.] ");
                if (string.IsNullOrEmpty(strDate))
                    sbDescription.Append("[strDate is null or empty.] ");
                if (sbDescription.Length > 0)
                {
                    string strErr = string.Format("IncompleteCassetteEditSaveReply NG - Parameters_Error - {0} , XmlFileName:{1}, Date:{2}, PortID:{3}, CassetteID:{4}, MesTrxID:{5}"
                        , sbDescription.ToString(), strXmlFileName, strDate, strPortID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                strPortID = strPortID.Trim();
                strCassetteID = strCassetteID.Trim();
                strMesTrxID = strMesTrxID.Trim();
                strXmlFileName = strXmlFileName.Trim();
                strDate = strDate.Trim();
                #endregion

                #region [file name format]
                string _strXmlFileName = strXmlFileName;
                if (strXmlFileName.Length <= 4 || strXmlFileName.Substring(strXmlFileName.Length - 4, 4).ToLower() != ".xml")
                    _strXmlFileName += ".xml";
                string strFilePath = string.Format("{0}{1}\\{2}", this.IncompleteCSTPath, strDate, _strXmlFileName);
                #endregion

                #region [check file]
                if (!File.Exists(strFilePath))
                {
                    string strErr = string.Format("IncompleteCassetteEditSaveReply NG - File_Error - [File is not exist in =[{0}],XmlFileName:{1}, Date:{2}, PortID:{3}, CassetteID:{4}, MesTrxID:{5}]"
                        , strFilePath, strXmlFileName, strDate, strPortID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                #endregion
                // 20170308 将原来的资料加回来才能保存  Tom
                XmlDocument oldXML = new XmlDocument();
                oldXML.Load(strFilePath);
                XmlNodeList slotNodeList = oldXML.DocumentElement.GetElementsByTagName("SLOTLIST");
                if (slotNodeList != null && slotNodeList.Count>0) {
                    XmlNode tempNode = xmlDocument.ImportNode(slotNodeList[0], true);
                    xmlDocument.DocumentElement.AppendChild(tempNode);
                }

                //直接保存覆盖 
                xmlDocument.Save(strFilePath);
                strDescription = "IncompleteCassetteEditSaveReply OK";
                return true;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("IncompleteCassetteEditSaveReply NG - Exception_Error - [Message:{0} ,XmlFileName:{1}, Date:{2}, PortID:{3}, CassetteID:{4}, MesTrxID:{5}]"
                    , ex.Message, strXmlFileName, strDate, strPortID, strCassetteID, strMesTrxID);
                return false;
            }
        }

        /// <summary>
        /// IncompleteCassetteCommandReply
        /// 描述: OPI 傳送指令，做 DELETE or RESEND 。
        /// </summary>
        /// <param name="strCommand">Command [ DELETE | RESEND ]</param>
        /// <param name="strPortID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="State">SBCS_INCOMPLETECST.STATE 的狀態資料 - [ OK | NG | CLOSE ]</param>
        /// <param name="strXmlFileName"> XML 文件名稱 - 格式:[Name or Name.xml]</param>
        /// <param name="strDate">XML 文件所屬的資料夾名稱 - 格式:[yyyyMMdd]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool IncompleteCassetteCommandReply(string strCommand, string strPortID, string strCassetteID, string strMesTrxID, string strXmlFileName, string strDate, out string strDescription, out XmlDocument xmlDocument)
        {
            try
            {
                strDate = strMesTrxID.Substring(0, 8);

                #region [Check CassetteManager Path]
                StringBuilder sbMsg = new StringBuilder();
                if (string.IsNullOrEmpty(this.IncompleteCSTPath))
                    sbMsg.Append("[CassetteManager.IncompleteCSTPath is null or empty.]");
                if (string.IsNullOrEmpty(this.CompleteCSTPath))
                    sbMsg.Append("[CassetteManager.CompleteCSTPath is null or empty.]");
                if (string.IsNullOrEmpty(this.LotEndExecuteCSTPath))
                    sbMsg.Append("[CassetteManager.LotEndExecuteCSTPath is null or empty.]");
                if (sbMsg.Length > 0)
                {
                    string strErr = string.Format("IncompleteCassetteCommandReply NG - Path_Error - {0} , Command:{1}, PortID:{2}, CassetteID:{3}, MesTrxID:{4}"
                        , sbMsg.ToString(), strCommand, strPortID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                if (this.IncompleteCSTPath[this.IncompleteCSTPath.Length - 1] != '\\')
                    this.IncompleteCSTPath += "\\";
                if (this.CompleteCSTPath[this.CompleteCSTPath.Length - 1] != '\\')
                    this.CompleteCSTPath += "\\";
                if (this.LotEndExecuteCSTPath[this.LotEndExecuteCSTPath.Length - 1] != '\\')
                    this.LotEndExecuteCSTPath += "\\";
                #endregion

                #region [Check Parameters]
                if (string.IsNullOrEmpty(strCommand))
                    sbMsg.Append("[strCommand is null or empty.] ");
                if (string.IsNullOrEmpty(strPortID))
                    sbMsg.Append("[strPordID is null or empty.] ");
                if (string.IsNullOrEmpty(strCassetteID))
                    sbMsg.Append("[strCassetteID is null or empty.] ");
                if (string.IsNullOrEmpty(strMesTrxID))
                    sbMsg.Append("[strMesTrxID is null or empty.] ");
                if (string.IsNullOrEmpty(strXmlFileName))
                    sbMsg.Append("[strFileName is null or empty.] ");
                if (string.IsNullOrEmpty(strDate))
                    sbMsg.Append("[strDate is null or empty.] ");
                if (sbMsg.Length > 0)
                {
                    string strErr = string.Format("IncompleteCassetteCommandReply NG - Parameters_Error - {0} , Command:{1}, PortID:{2}, CassetteID:{3}, MesTrxID:{4}"
                        , sbMsg.ToString(), strCommand, strPortID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                strCommand = strCommand.Trim();
                strPortID = strPortID.Trim();
                strCassetteID = strCassetteID.Trim();
                strMesTrxID = strMesTrxID.Trim();
                strXmlFileName = strXmlFileName.Trim();
                strDate = strDate.Trim();
                #endregion

                #region [File Name Format]
                if (strXmlFileName.Length <= 4 || strXmlFileName.Substring(strXmlFileName.Length - 4, 4).ToLower() != ".xml")
                    strXmlFileName += ".xml";
                string strSourcePathFile = string.Format("{0}{1}\\{2}", this.IncompleteCSTPath, strDate, strXmlFileName);
                string strCompletePath = string.Format("{0}{1}\\", this.CompleteCSTPath, strDate);
                string strCompletePathFile = string.Format("{0}{1}", strCompletePath, strXmlFileName);
                string strExecutePathPathFile = string.Format("{0}{1}", this.LotEndExecuteCSTPath, strXmlFileName);
                #endregion

                #region [Check File]
                if (!File.Exists(strSourcePathFile))
                    sbMsg.Append(string.Format("[File is not exist in ({0})]", strSourcePathFile));
                if (sbMsg.Length > 0)
                {
                    string strErr = string.Format("IncompleteCassetteCommandReply NG - File_Error - =[{0}], Command:{1}, PortID:{2}, CassetteID:{3}, MesTrxID:{4}"
                        , sbMsg.ToString(), strCommand, strPortID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                #endregion

                #region [Move File  & Update DB]
                if (strCommand.ToUpper() == "DELETE")
                {
                    //Update DB
                    if (!UpdateIncompleteCassetteToDB(strPortID, strCassetteID, strMesTrxID, "CLOSE", out strDescription))
                    {
                        string strErr = string.Format("IncompleteCassetteCommandReply NG - UpdateDB_Error - [Message:{0} , Command:{1}, PortID:{2}, CassetteID:{3}, MesTrxID:{4} ] "
                            , strDescription, strCommand, strPortID, strCassetteID, strMesTrxID);
                        Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                        strDescription = strErr;
                        xmlDocument = null;
                        return false;
                    }

                    //Create Folder
                    if (Directory.Exists(strCompletePath) == false)
                        Directory.CreateDirectory(strCompletePath);

                    //Move File
                    if (File.Exists(strCompletePathFile))
                        File.Delete(strCompletePathFile);
                    File.Move(strSourcePathFile, strCompletePathFile);
                    xmlDocument = null;
                }
                else if (strCommand.ToUpper() == "RESEND")
                {
                    //Create Folder
                    if (Directory.Exists(this.LotEndExecuteCSTPath) == false)
                        Directory.CreateDirectory(this.LotEndExecuteCSTPath);

                    //Move File
                    if (File.Exists(strExecutePathPathFile))
                        File.Delete(strExecutePathPathFile);
                    File.Move(strSourcePathFile, strExecutePathPathFile);

                    //Update Xml Return Code & Return Message
                    xmlDocument = new XmlDocument();
                    xmlDocument.Load(strExecutePathPathFile);
                    //xmlDocument.SelectSingleNode(string.Format("{0}/{1}/{2}", eKeyHost.MESSAGE, eKeyHost.RETURN, eKeyHost.RETURNCODE)).InnerText = "0";
                    //xmlDocument.SelectSingleNode(string.Format("{0}/{1}/{2}", eKeyHost.MESSAGE, eKeyHost.RETURN, eKeyHost.RETURNMESSAGE)).InnerText = string.Empty;
                    //xmlDocument.Save(strExecutePathPathFile);
                }
                else
                {
                    string strErr = string.Format("IncompleteCassetteCommandReply NG - Command_Error - [Command's value is =[{0}], it's not equals (DELETE) or (RESEND).  PortID:{1}, CassetteID:{2}, MesTrxID:{3}]"
                        , strCommand, strPortID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                #endregion

                strDescription = "IncompleteCassetteCommandReply OK";
                return true;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("IncompleteCassetteCommandReply NG - Exception_Error - [Message:{0} , Command:{1}, PortID:{2}, CassetteID:{3}, MesTrxID:{4}]"
                    , ex.Message, strCommand, strPortID, strCassetteID, strMesTrxID);
                xmlDocument = null;
                return false;
            }
        }

        /// <summary>
        /// UpdateLotPorcessEndMesReplyToExecuteXml
        /// 描述: LotProcessEnd 根據 MES Reply 回覆的 XmlDocument 中的 Return Code 與 Return Message ，回存 Executing Folder 中相對應的 Xml File 中的 Return Code、Return Message
        /// </summary>
        /// <param name="strPortID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="strState">State - [ OK | NG ]</param>
        /// <param name="strReturnCode">MES Reply 回報的 Return Code</param>
        /// <param name="strReturnMessage">MES Reply 回報的 Return Message</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool UpdateLotPorcessEndMesReplyToExecuteXmlAndDB(string strPortID, string strCassetteID,string cstSeqNo ,string strMesTrxID, string strState, string strReturnCode, string strReturnMessage, out string strDescription, string strFileType = "",string transactonName="")
        {
            try
            {
                #region [Check CassetteManager Path
                if (string.IsNullOrEmpty(this.LotEndExecuteCSTPath))
                {
                    string strErr = string.Format("UpdateLotPorcessEndMesReplyToExecuteXmlAndDB NG - CassetteManager.LotEndExecuteCSTPath is null or empty! , PortID:{0}, CassetteID:{1}, MesTrxID:{2} State:{3} ReturnCode:{4} ReturnMessage:{5}",
                        strPortID, strCassetteID, strMesTrxID, strState, strReturnCode, strReturnMessage);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                if (this.LotEndExecuteCSTPath[this.LotEndExecuteCSTPath.Length - 1] != '\\')
                    this.LotEndExecuteCSTPath += "\\";
                #endregion

                #region [Check File]
                string strFilePath = string.Format("{0}{1}_{2}_{3}_{4}.xml", this.LotEndExecuteCSTPath, strFileType , strPortID, strCassetteID, strMesTrxID);
                if (!File.Exists(strFilePath))
                {
                    string strErr = string.Format("UpdateLotPorcessEndMesReplyToExecuteXmlAndDB NG - File_Error - [File is not exist in =[{0}], PortID:{1}, CassetteID:{2}, MesTrxID:{3} State:{4} ReturnCode:{5} ReturnMessage:{6}"
                        , strFilePath, strPortID, strCassetteID, strMesTrxID, strState, strReturnCode, strReturnMessage);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                #endregion

                #region [Check Parameters]
                StringBuilder sbMsg = new StringBuilder();
                if (string.IsNullOrEmpty(strPortID))
                    sbMsg.Append("[strPordID is null or empty.] ");
                if (string.IsNullOrEmpty(strCassetteID))
                    sbMsg.Append("[strCassetteID is null or empty.] ");
                if (string.IsNullOrEmpty(strMesTrxID))
                    sbMsg.Append("[strMesTrxID is null or empty.] ");
                if (string.IsNullOrEmpty(strState))
                    sbMsg.Append("[strState is null or empty.] ");
                if (sbMsg.Length > 0)
                {
                    string strErr = string.Format("IncompleteCassetteCommandReply NG - Parameters_Error - {0} , PortID:{1}, CassetteID:{2}, MesTrxID:{3}"
                        , sbMsg.ToString(), strPortID, strCassetteID, strMesTrxID);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                strPortID = strPortID.Trim();
                strCassetteID = strCassetteID.Trim();
                strMesTrxID = strMesTrxID.Trim();
                strState = strState.Trim();
                #endregion

                #region [Update Executing Xml File Data]
                //XmlDocument xDocument = new XmlDocument();
                //xDocument.Load(strFilePath);
                //xDocument.SelectSingleNode(string.Format("{0}/{1}/{2}", eKeyHost.MESSAGE, eKeyHost.RETURN, eKeyHost.RETURNCODE)).InnerText = strReturnCode;
                //xDocument.SelectSingleNode(string.Format("{0}/{1}/{2}", eKeyHost.MESSAGE, eKeyHost.RETURN, eKeyHost.RETURNMESSAGE)).InnerText = strReturnMessage;  
                //xDocument.Save(strFilePath);
                #endregion

                #region [Update DB INCOMPLETECST State]
                IncompleteCassette inComplete = new IncompleteCassette();
                IList list = HistoryHibernateAdapter.GetObjectByQuery(string.Format("from IncompleteCassette where PORTID='{0}' and CASSETTEID='{1}' and MESTRXID='{2}'", strPortID, strCassetteID, strMesTrxID));
                if (strState.ToUpper() == "OK")
                {
                    if (list != null && list.Count > 0)
                    {
                        if (!UpdateIncompleteCassetteToDB(strPortID, strCassetteID, strMesTrxID, "OK", strReturnMessage, out strDescription,transactonName))
                        {
                            string strErr = string.Format("UpdateLotPorcessEndMesReplyToExecuteXmlAndDB NG - UpdateDB_Error - [Message:{0} , PortID:{1}, CassetteID:{2}, MesTrxID:{3} State:{4} ReturnCode:{5} ReturnMessage:{6}]"
                                , strDescription, strPortID, strCassetteID, strMesTrxID, strState, strReturnCode, strReturnMessage);
                            Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                            strDescription = strErr;
                            return false;
                        }
                    }
                }
                else if (strState.ToUpper() == "NG")
                {
                    if (list != null && list.Count > 0)
                    {
                        if (!UpdateIncompleteCassetteToDB(strPortID, strCassetteID, strMesTrxID, "NG", strReturnMessage, out strDescription,transactonName))
                        {
                            string strErr = string.Format("UpdateLotPorcessEndMesReplyToExecuteXmlAndDB NG - UpdateDB_Error - [Message:{0}] , PortID:{1}, CassetteID:{2}, MesTrxID:{3} State:{4} ReturnCode:{5} ReturnMessage:{6}"
                                , strDescription, strPortID, strCassetteID, strMesTrxID, strState, strReturnCode, strReturnMessage);
                            Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                            strDescription = strErr;
                            return false;
                        }
                    }
                    else
                    {
                        int cassetteSeqNo=0;
                        int.TryParse(cstSeqNo, out cassetteSeqNo);
                        if (!SaveIncompleteCassetteToDB(strPortID, strCassetteID, strMesTrxID, cassetteSeqNo, "NG", strReturnMessage, out strDescription, strFileType))
                        {
                            string strErr = string.Format("UpdateLotPorcessEndMesReplyToExecuteXmlAndDB NG - InsertDB_Error - [Message:{0}] , PortID:{1}, CassetteID:{2}, MesTrxID:{3} State:{4} ReturnCode:{5} ReturnMessage:{6}"
                                , strDescription, strPortID, strCassetteID, strMesTrxID, strState, strReturnCode, strReturnMessage);
                            Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                            strDescription = strErr;
                            return false;
                        }
                    }
                }
                else
                {
                    string strErr = string.Format("UpdateLotPorcessEndMesReplyToExecuteXmlAndDB NG - Command_Error - [State:{0} is not [OK] or [NG] , PortID:{1}, CassetteID:{2}, MesTrxID:{3} ReturnCode:{4} ReturnMessage:{5}]"
                                ,strState, strPortID, strCassetteID, strMesTrxID, strReturnCode, strReturnMessage);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    return false;
                }
                #endregion



                strDescription = "UpdateLotPorcessEndMesReplyToExecuteXmlAndDB OK";
                return true;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("NG - Exception Message:{0} , PortID:{1}, CassetteID:{2}, MesTrxID:{3} State:{4} ReturnCode:{5} ReturnMessage:{6}"
                    , ex.Message, strPortID, strCassetteID, strMesTrxID, strState, strReturnCode, strReturnMessage);
                return false;
            }
        }

        /// <summary>
        /// IncompleteCassetteDataReply - [IncompleteCST] Return XmlDocument
        /// 描述：IncompleteCST Folder 下的 Xml 資料回傳作業。
        /// </summary>
        /// <param name="strPortID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="strXmlFileName"> XML 文件名稱 - 格式:[Name or Name.xml]</param>
        /// <param name="strDate">XML 文件所屬的資料夾名稱 - 格式:[yyyyMMdd]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <param name="xmlDocument">MES XML - BoxProcessEnd</param>
        /// <returns></returns>
        public bool IncompleteBoxDataReply(string strPortID, string strMesTrxID, string strXmlFileName, string strDate, out string strDescription, out XmlDocument xmlDocument)
        {
            try
            {
                strDate = strMesTrxID.Substring(0, 8);

                #region [check CassetteManager path]
                if (string.IsNullOrEmpty(this.IncompleteCSTPath))
                {
                    string strErr = string.Format("IncompleteBoxDataReply NG - IncompleteCSTPath_Error - [CassetteManager.IncompleteCSTPath is null or empty. PortID:{0}, MesTrxID:{1}, XmlFileName:{2}, Date:{3}]"
                        , strPortID, strMesTrxID, strXmlFileName, strDate);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                if (this.IncompleteCSTPath[this.IncompleteCSTPath.Length - 1] != '\\')
                    this.IncompleteCSTPath += "\\";

                #endregion

                #region [check parameters]
                StringBuilder sbDescription = new StringBuilder();
                if (string.IsNullOrEmpty(strPortID))
                    sbDescription.Append("[strPordID is null or empty.] ");
                if (string.IsNullOrEmpty(strMesTrxID))
                    sbDescription.Append("[strMesTrxID is null or empty.] ");
                if (string.IsNullOrEmpty(strXmlFileName))
                    sbDescription.Append("[strFileName is null or empty.] ");
                if (string.IsNullOrEmpty(strDate))
                    sbDescription.Append("[strDate is null or empty.] ");
                if (sbDescription.Length > 0)
                {
                    string strErr = string.Format("IncompleteBoxDataReply NG - Parameters_Error - {0} ,PortID:{1}, MesTrxID:{2}, XmlFileName:{3}, Date:{4}"
                        , sbDescription.ToString(), strPortID, strMesTrxID, strXmlFileName, strDate);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                strPortID = strPortID.Trim();
                strMesTrxID = strMesTrxID.Trim();
                strXmlFileName = strXmlFileName.Trim();
                strDate = strDate.Trim();
                #endregion

                #region [file name format]
                string _strXmlFileName = strXmlFileName;
                if (strXmlFileName.Length <= 4 || strXmlFileName.Substring(strXmlFileName.Length - 4, 4).ToLower() != ".xml")
                    _strXmlFileName += ".xml";
                string strFilePath = string.Format("{0}{1}\\{2}", this.IncompleteCSTPath, strDate, _strXmlFileName);
                #endregion

                #region [check file]
                if (!File.Exists(strFilePath))
                {
                    string strErr = string.Format("IncompleteBoxDataReply NG - File_Error - [File is not exist in =[{0}],PortID:{1}, MesTrxID:{2}, XmlFileName:{3}, Date:{4}]"
                        , strFilePath, strPortID, strMesTrxID, strXmlFileName, strDate);
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strErr);
                    strDescription = strErr;
                    xmlDocument = null;
                    return false;
                }
                #endregion

                xmlDocument = new XmlDocument();
                xmlDocument.Load(strFilePath);//MES XML - BoxProcessEnd
                strDescription = "IncompleteBoxDataReply OK";
                return true;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("IncompleteBoxDataReply NG - Exception_Error - [Message:{0} ,PortID:{1}, MesTrxID:{2}, XmlFileName:{3}, Date:{4}]"
                    , ex.Message, strPortID, strMesTrxID, strXmlFileName, strDate);
                xmlDocument = null;
                return false;
            }
        }


        /// <summary>
        /// SaveIncompleteCassetteToDB
        /// 描述:依據傳入的參數，於 SBCS_INCOMPLETECST.STATE 新增一筆資料
        /// </summary>
        /// <param name="strPordID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="strCassetteSeqNO"></param>
        /// <param name="State">SBCS_INCOMPLETECST.STATE 的狀態資料 - [ OK | NG | CLOSE ]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool SaveIncompleteCassetteToDB(string strPordID, string strCassetteID, string strMesTrxID, int intCassetteSeqNO, string State, string strNGReason, out string strDescription, string strFileType = "",string trxName="")
        {
            try
            {
                IncompleteCassette inComplete = new IncompleteCassette();
                inComplete.CASSETTEID = strCassetteID;
                inComplete.CASSETTESEQNO = intCassetteSeqNO;
                inComplete.FILENAME = string.Format("{0}_{1}_{2}_{3}", strFileType , strPordID, strCassetteID, strMesTrxID);
                inComplete.MESTRXID = strMesTrxID;
                inComplete.PORTID = strPordID;
                inComplete.STATE = State;
                inComplete.UPDATETIME = DateTime.Now;
                inComplete.NGREASON = strNGReason;
                inComplete.TRANSACTIONNAME = trxName;
                this.HistoryHibernateAdapter.SaveObject(inComplete);

                strDescription = "SaveIncompleteCassetteToDB OK";
                return true;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("SaveIncompleteCassetteToDB NG - Exception Message:{0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// UpdateIncompleteCassetteToDB
        /// 描述:依據 Port ID, Cassette ID, Mes Trx ID, 於 SBCS_INCOMPLETECST 找到其資料並更新其 State 欄位的狀態
        /// </summary>
        /// <param name="strPordID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="State">SBCS_INCOMPLETECST.STATE 的狀態資料 - [ OK | NG | CLOSE ]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool UpdateIncompleteCassetteToDB(string strPordID, string strCassetteID, string strMesTrxID, string State, out string strDescription)
        {
            try
            {
                IncompleteCassette inComplete = new IncompleteCassette();
                IList list = HistoryHibernateAdapter.GetObjectByQuery(string.Format("from IncompleteCassette where PORTID='{0}' and CASSETTEID='{1}' and MESTRXID='{2}'", strPordID, strCassetteID, strMesTrxID));
                if (list != null)
                {
                    inComplete = list[0] as IncompleteCassette;
                    inComplete.STATE = State;
                    inComplete.UPDATETIME = DateTime.Now;
                    HistoryHibernateAdapter.UpdateObject(inComplete);

                    strDescription = "UpdateIncompleteCassetteToDB OK";
                    return true;
                }
                else
                {
                    strDescription = "UpdateIncompleteCassetteToDB NG - It's not find the data.";
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strDescription);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("UpdateIncompleteCassetteToDB Exception Message:{0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// UpdateIncompleteCassetteToDB
        /// 描述:依據 Port ID, Cassette ID, Mes Trx ID, 於 SBCS_INCOMPLETECST 找到其資料並更新其 State 欄位的狀態
        /// </summary>
        /// <param name="strPordID">Pord ID</param>
        /// <param name="strCassetteID">Cassette ID</param>
        /// <param name="strMesTrxID">MES Trx ID - [yyyyMMddhhmmssfff]</param>
        /// <param name="State">SBCS_INCOMPLETECST.STATE 的狀態資料 - [ OK | NG | CLOSE ]</param>
        /// <param name="strDescription">執行結果描述 - 格式:[(OK or NG) + Message]</param>
        /// <returns></returns>
        public bool UpdateIncompleteCassetteToDB(string strPordID, string strCassetteID, string strMesTrxID, string State, string ReturnMessage, out string strDescription,string trxName="")
        {
            try
            {
                IncompleteCassette inComplete = new IncompleteCassette();
                IList list = HistoryHibernateAdapter.GetObjectByQuery(string.Format("from IncompleteCassette where PORTID='{0}' and CASSETTEID='{1}' and MESTRXID='{2}'", strPordID, strCassetteID, strMesTrxID));
                if (list != null)
                {
                    inComplete = list[0] as IncompleteCassette;
                    inComplete.STATE = State;
                    inComplete.UPDATETIME = DateTime.Now;
                    inComplete.NGREASON = ReturnMessage;
                    inComplete.TRANSACTIONNAME = trxName;
                    HistoryHibernateAdapter.UpdateObject(inComplete);

                    strDescription = "UpdateIncompleteCassetteToDB OK";
                    return true;
                }
                else
                {
                    strDescription = "UpdateIncompleteCassetteToDB NG - It's not find the data.";
                    Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strDescription);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                strDescription = string.Format("UpdateIncompleteCassetteToDB Exception Message:{0}", ex.Message);
                return false;
            }
        }
        #endregion

        
        public void SaveCassetteHistory(Cassette cassette, string cstPortStatus, int cstJobCount, string cstJobExistInfo, string cstType)
        {
            try
            {
                CassetteHistory his = new CassetteHistory();
                int temp = 0;

                //,[UPDATETIME]
                //,[CASSETTEID]
                //,[CASSETTESEQNO]
                //,[CASSETTESTATUS]
                //,[NODEID]
                //,[JOBCOUNT]
                //,[PORTID]
                //,[JOBEXISTENCE]
                //,[CASSETTECONTROLCOMMAND]
                //,[COMMANDRETURNCODE]
                //,[OPERATORID]
                //,[COMMPLETEDCASSETTEDATA]
                //,[LOADINGCASSETTETYPE]
                //,[QTIMEFLAG]
                //,[PARTIALFULLFLAG]
                //,[LOADTIME]
                //,[PROCESSSTATTIME]
                //,[PROCESSENDTIME]

                his.UPDATETIME = DateTime.Now;
                his.CASSETTEID = cassette.CassetteID;

                bool done = int.TryParse(cassette.CassetteSequenceNo, out temp);
                his.CASSETTESEQNO = temp;
                his.CASSETTESTATUS = cstPortStatus;
                his.NODEID = cassette.NodeID;
                his.JOBCOUNT = cstJobCount;
                his.PORTID = cassette.PortID;
                his.JOBEXISTENCE = cstJobExistInfo;
                his.CASSETTECONTROLCOMMAND = cassette.CassetteCommand.ToString();
                his.COMMANDRETURNCODE = cassette.ReasonCode;
                //his.OPERATORID = "";
                his.COMMPLETEDCASSETTEDATA = cassette.CstEndReason;
                his.LOADINGCASSETTETYPE = cstType;
                //his.QTIMEFLAG = 0;
                //his.PARTIALFULLFLAG = 0; 
                his.LOADTIME = cassette.LoadTime;
                his.PROCESSSTATTIME = cassette.StartTime;
                his.PROCESSENDTIME = cassette.EndTime;

                ObjectManager.CassetteManager.InsertHistory(his);

            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

    }

}
