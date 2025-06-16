using System;
using System.Collections.Generic;
using System.Reflection;
using KZONE.Entity;
using KZONE.Log;
using KZONE.DB;
using System.Collections;
using KZONE.Work;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using Quartz;
using OfficeOpenXml;
using System.Linq;

namespace KZONE.EntityManager
{
    public class ProcessDataManager:IDataSource
    {
        /// <summary>
        /// 各机台的EDC设定档
        /// </summary>
        private IDictionary<string, IList<ProcessData>> _processDatas = new Dictionary<string, IList<ProcessData>>();
        
        /// <summary>
        /// NLog Logger Name
        /// </summary>
        public string LoggerName { get; set; }

        private string _historyPath;
        
        /// <summary>
        /// Histrory  File Path
        /// </summary>
        public string HistoryPath
        {
            get
            {
                if (string.IsNullOrEmpty(_historyPath))
                {
                    _historyPath = "..\\Data\\ProcessData\\";
                }

                return _historyPath;               
            }
          set
            {
                _historyPath = value;
                if (_historyPath[_historyPath.Length - 1] != '\\')
                {
                    _historyPath = _historyPath + "\\";
                }
            }
        }
        public string TactHistoryPath { get; set; }
        
        /// <summary>
        /// 用來讀取DB
        /// </summary>
        public HibernateAdapter HibernateAdapter { get; set; }

        /// <summary>
        /// 用來讀取DB
        /// </summary>
        public HibernateAdapter HistoryHibernateAdapter { get; set; }

        private IScheduler _scheduler = null;
        public void Init()
        {
            _processDatas = Reload();
            HistoryPath = HistoryPath.Replace("{ServerName}", Workbench.ServerName);
            TactHistoryPath = TactHistoryPath.Replace("{ServerName}", Workbench.ServerName);
            ITrigger trigger = new TimerTrigger("DeleteProcessFile", "1H", false);
            IJob job = new MethodJob("DeleteProcessFile", this, "DeleteProcessDataFiles", null);
            _scheduler = SchedulerManager.Instance.CreateDefaultScheduler(trigger, job);
            SchedulerManager.Instance.AddScheduler(_scheduler);
        }

        protected IDictionary<string, IList<ProcessData>> Reload()
        {
            try
            {
                IDictionary<string, IList<ProcessData>> processDatas = new Dictionary<string, IList<ProcessData>>();
                string hql = string.Format("from ProcessDataEntityData where LINEID = '{0}' order by NODENO, OBJECTKEY", Workbench.ServerName);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<ProcessData> processDataList = null;
                if (list != null)
                {
                    foreach (ProcessDataEntityData data in list)
                    {
                        //data.PARAMETERNAME = data.PARAMETERNAME.Replace(',', ';').Replace('=', '-').Replace('<','(').Replace('>',')').Replace('/','-');//,及=會影響ProcessDataService.HandleProcessData()組字串
                        data.PARAMETERNAME = Regex.Replace(data.PARAMETERNAME, @"[/| |?|/|<|>|'|\-|,]", "_");
                        if (!processDatas.ContainsKey(data.NODENO))
                        {
                            processDataList = new List<ProcessData>();
                            processDatas.Add(data.NODENO, processDataList);
                        }
                        processDatas[data.NODENO].Add(new ProcessData(data));
                    }
                }
                return processDatas;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                throw;
            }
        }

       
        /// <summary>
        /// 取得EDC 设定档
        /// </summary>
        public IList<ProcessData> GetProcessData(string eqpNo)
        {
            try
            {
                if (_processDatas.ContainsKey(eqpNo))
                {
                    lock (_processDatas)
                    {
                        return _processDatas[eqpNo];
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }
  
        /// <summary>
        /// Reload DB Profile by All
        /// </summary>
        /// <returns></returns>
        public void ReloadAll()
        {
            try
            {
                IDictionary<string, IList<ProcessData>> tempDic = Reload();

                if (tempDic != null)
                {
                    lock (_processDatas)
                    {
                        _processDatas = tempDic;
                    }

                }
                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                           string.Format("Reload Process Data all Equipment."));

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        /// <summary>
        /// Reload DB Profile by Node
        /// </summary>
        /// <returns></returns>
        public void ReloadByNo(string eqpNo)
        {
            try
            {
                string hql = string.Format("from ProcessDataEntityData where LINEID = '{0}' and NODENO='{1}' order by SVID ", Workbench.ServerName, eqpNo);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<ProcessData> processDataList = new List<ProcessData>();
                if (list != null)
                {
                    foreach (ProcessDataEntityData data in list)
                    {
                        data.PARAMETERNAME = Regex.Replace(data.PARAMETERNAME, @"[/| |?|/|<|>|'|\-|,]", "_");
                        processDataList.Add(new ProcessData(data));
                    }
                    lock (_processDatas)
                    {
                        if (_processDatas.ContainsKey(eqpNo))
                        {
                            _processDatas.Remove(eqpNo);
                        }
                        _processDatas.Add(eqpNo, processDataList);
                    }
                }
                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            string.Format("Reload Process Data EQUIPMENT=[{0}]", eqpNo));

            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Save Process Data History to Database
        /// </summary>
        /// <param name="history"></param>
        public void SaveProcessDataHistory(ProcessDataHistory history)
        {
            try
            {
                HistoryHibernateAdapter.SaveObject(history);
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// Save Process Data History to Database
        /// </summary>
        /// <param name="history"></param>
        public void SaveTactDataHistory(TactDataHistory history)
        {
            try
            {
                HistoryHibernateAdapter.SaveObject(history);
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// write Process Data  to File
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="cstSeqNo"></param>
        /// <param name="jobSeqNo"></param>
        /// <param name="timeKey">yyyyMMddHHmmssfff</param>
        /// <param name="value"></param>
        public void MakeProcessDataValuesToFile(string eqpID, string cstSeqNo, string jobSeqNo, string timeKey, string value)
        {
            try
            {
                string directoryPath = this.HistoryPath + timeKey.Substring(0, 10);
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                //CASSETTESEQNO_JOBSEQNO_NODEID_TRXID.txt
                string fileName = string.Format("{0}\\{1}.txt", directoryPath, string.Format("{0}_{1}_{2}_{3}", cstSeqNo,jobSeqNo, eqpID, timeKey));

                File.WriteAllText(fileName, value);

                DeleteProcessDataFiles();
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void MakeProcessDataValuesToEXCEL(string glassID, string cstSeqNo, string jobSeqNo, string timeKey, string value)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string directoryPath = this.HistoryPath + timeKey.Substring(0, 6);
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                //CASSETTESEQNO_JOBSEQNO_NODEID_TRXID.txt
               // string fileName = string.Format("{0}\\{1}.txt", directoryPath, timeKey.Substring(0, 10));

                FileInfo newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 8)}.xlsx");
                if (!newFile.Exists)
                {
                 
                    newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 8)}.xlsx");
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet;
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                         worksheet = package.Workbook.Worksheets.Add($"{timeKey.Substring(0, 8)}");
                        worksheet.Cells.Style.ShrinkToFit = true;
                    }
                    else {
                         worksheet = package.Workbook.Worksheets[0];
                    }
                    string[] st = value.Split(',');

                    if (worksheet.Dimension == null)
                    {
                       

                        worksheet.Cells[1, 1].Value = "DateTime";
                        worksheet.Cells[1, 2].Value = "CstSeqNo";
                        worksheet.Cells[1, 3].Value = "JobSeqNo";
                        worksheet.Cells[1, 4].Value = "GlassID";
                        worksheet.Cells[2, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[2, 2].Value = cstSeqNo;
                        worksheet.Cells[2, 3].Value = jobSeqNo;
                        worksheet.Cells[2, 4].Value = glassID;
                        worksheet.Column(1).AutoFit();
                        worksheet.Column(2).AutoFit();
                        worksheet.Column(3).AutoFit();
                        worksheet.Column(4).AutoFit();
                        int i = 5;

                        foreach (string s in st)
                        {
                            if (s.Contains("="))
                            {
                                string[] nv = s.Split('=');
                                worksheet.Cells[1, i].Value = nv[0];
                                worksheet.Cells[2, i].Value = nv[1];

                                worksheet.Column(i).AutoFit();
                            }
                            i++;
                        }
                    }
                    else {

                        worksheet.Cells[worksheet.Dimension.End.Row+1, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[worksheet.Dimension.Rows , 2].Value = cstSeqNo;
                        worksheet.Cells[worksheet.Dimension.Rows, 3].Value = jobSeqNo;
                        worksheet.Cells[worksheet.Dimension.Rows, 4].Value = glassID;

                        int i = 5;

                        foreach (string s in st)
                        {
                            if (s.Contains("="))
                            {
                                string[] nv = s.Split('=');

                                //worksheet.Cells[worksheet.Dimension.Rows, i].Value = nv[0];
                                worksheet.Cells[worksheet.Dimension.Rows, i].Value = nv[1];
                            }
                            i++;
                        }
                    }

                  

                    package.Save();
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        public void MakeSVDataValuesToEXCEL( string timeKey, string value)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string directoryPath = this.HistoryPath.Replace("ProcessData", "SVData") + timeKey.Substring(0, 6);
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                //CASSETTESEQNO_JOBSEQNO_NODEID_TRXID.txt
                // string fileName = string.Format("{0}\\{1}.txt", directoryPath, timeKey.Substring(0, 10));

                FileInfo newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 8)}.xlsx");
                if (!newFile.Exists)
                {

                    newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 8)}.xlsx");
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet;
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        worksheet = package.Workbook.Worksheets.Add($"{timeKey.Substring(0, 8)}");
                        worksheet.Cells.Style.ShrinkToFit = true;
                    }
                    else
                    {
                        worksheet = package.Workbook.Worksheets[0];
                    }
                    string[] st = value.Split(',');

                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "DateTime";
                        worksheet.Cells[2, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Column(1).AutoFit();
                        int i = 2;

                        foreach (string s in st)
                        {
                            if (s.Contains("="))
                            {
                                string[] nv = s.Split('=');
                                worksheet.Cells[1, i].Value = nv[0];
                                worksheet.Cells[2, i].Value = nv[1];

                                worksheet.Column(i).AutoFit();
                            }
                            i++;
                        }
                    }
                    else
                    {

                        worksheet.Cells[worksheet.Dimension.End.Row + 1, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                       

                        int i = 2;

                        foreach (string s in st)
                        {
                            if (s.Contains("="))
                            {
                                string[] nv = s.Split('=');

                                //worksheet.Cells[worksheet.Dimension.Rows, i].Value = nv[0];
                                worksheet.Cells[worksheet.Dimension.Rows, i].Value = nv[1];
                            }
                            i++;
                        }
                    }



                    package.Save();
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CreateExcel(string path, string name)
        {
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(new FileInfo($"{path}\\{name}.xlsx")))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{name}");//创建worksheet
                    package.Save();//保存excel
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }




        public void MakeTactDataValuesToFile( string cstSeqNo, string jobSeqNo, string timeKey, string value)
        {
            try
            {
                string directoryPath = this.TactHistoryPath + timeKey.Substring(0, 10);
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

               
                string fileName = string.Format("{0}\\{1}.txt", directoryPath, string.Format("{0}_{1}_{2}", cstSeqNo, jobSeqNo, timeKey));

                File.WriteAllText(fileName, value);

                DeleteTactDataFiles();
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// Get Process Datav Values,
        /// 传入FileName =CSTSeq_JobSeq_EqpID_yyyyMMddHHmmssfff;
        /// Return IList<string> Ex "Recipe ID=14";
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<string> TactDataValues(string key)
        {
            List<string> paramter = null; ;
            try
            {
                if (string.IsNullOrEmpty(key) == true)
                    return null;

                if (key.Split('_').Length < 3)
                    throw new Exception(string.Format("File Name Format Error [{0}]!", key));

                string timeKey = key.Split('_')[2].ToString().Trim();

                string directoryPath = this.TactHistoryPath + timeKey.Substring(0, 10);

                if (!File.Exists(directoryPath + "\\" + string.Format("{0}.txt", key)))
                    throw new Exception(string.Format("Can't find Tact Data File [{0}] in Directory!", key));

                string[] filePathArray = Directory.GetFiles(directoryPath, string.Format("{0}.txt", key), SearchOption.TopDirectoryOnly);

                if (filePathArray != null && filePathArray.Length > 0)
                {
                    string value = File.ReadAllText(filePathArray[0]);
                    string[] valueArray = value.Split(',');
                    paramter = new List<string>();
                    paramter.AddRange(valueArray);
                }
                return paramter;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }

        }
        /// <summary>
        /// Delete ProcessData Files 目前保留14天
        /// </summary>
        public void DeleteTactDataFiles()
        {
            try
            {
                if (Directory.Exists(TactHistoryPath) == false)
                {
                    return;
                }
                string[] directoryList = Directory.GetDirectories(TactHistoryPath);

                foreach (string directoryPath in directoryList)
                {
                    DateTime checkTime = Directory.GetCreationTime(directoryPath).AddDays(14);

                    if (checkTime < DateTime.Now)
                    {
                        if (Directory.Exists(directoryPath))
                            Directory.Delete(directoryPath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }




        /// <summary>
        /// Write Lot Process Data to File
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="cstSeqNo"></param>
        /// <param name="timeKey"></param>
        /// <param name="value"></param>
        /// <param name="fileName"></param>
        public void MakeLotProcessDataValuesToFile(string eqpID, string lotID, string timeKey, string value, out string fileName)
        {
            fileName = string.Empty;
            try
            {
                string directoryPath = this.HistoryPath + timeKey.Substring(0, 8);
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                //CASSETTESEQNO_NODEID_TRXID.txt
                fileName = string.Format("{0}\\{1}_{2}_{3}.txt", directoryPath, lotID, eqpID, timeKey);

                File.WriteAllText(fileName, value);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// Get Process Datav Values,
        /// 传入FileName =CSTSeq_JobSeq_EqpID_yyyyMMddHHmmssfff;
        /// Return IList<string> Ex "Recipe ID=14";
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<string> ProcessDataValues(string key)
        {
            List<string> paramter = null; ;
            try
            {
                if (string.IsNullOrEmpty(key) == true)
                    return null ;

                if (key.Split('_').Length < 4)
                    throw new Exception(string.Format("File Name Format Error [{0}]!", key));

                string timeKey = key.Split('_')[3].ToString().Trim();//因命名方式，将"3"改为"4"，例：12_13_3AES09_STR_20220503101452479，适用于21038~21043，2022.05.03 changed by MIXI

                string directoryPath = this.HistoryPath + timeKey.Substring(0, 10);

                if (!File.Exists(directoryPath + "\\" + string.Format("{0}.txt", key)))
                    throw new Exception(string.Format("Can't find Process Data File [{0}] in Directory!", key));

                string[] filePathArray = Directory.GetFiles(directoryPath, string.Format("{0}.txt", key), SearchOption.TopDirectoryOnly);

                if (filePathArray != null && filePathArray.Length > 0)
                {
                    string value = File.ReadAllText(filePathArray[0]);
                    string[] valueArray = value.Split(',');
                    paramter = new List<string>();
                    paramter.AddRange(valueArray);
                }
                return paramter;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
            
        }
        /// <summary>
        /// Delete ProcessData Files 目前保留100天
        /// </summary>
        public  void DeleteProcessDataFiles()
        {
            try
            {
                if(Directory.Exists(_historyPath)==false){
                    return;
                }
                string[] directoryList = Directory.GetDirectories(_historyPath);

                foreach (string directoryPath in directoryList)
                {
                    DateTime checkTime = Directory.GetCreationTime(directoryPath).AddDays(100);

                    if (checkTime < DateTime.Now)
                    {
                        if (Directory.Exists(directoryPath))
                            Directory.Delete(directoryPath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #region Implement IDataSource
        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();

                ProcessDataEntityData file = new ProcessDataEntityData();
                DataTableHelp.DataTableAppendColumn(file, dt);

               
                foreach (IList<ProcessData> list in _processDatas.Values)
                {
                    foreach (ProcessData pd in list)
                    {
                        DataRow dr = dt.NewRow();
                        DataTableHelp.DataRowAssignValue(pd.Data, dr);
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("ProcessDataManager");
            return entityNames;
        }
        #endregion
    }
}
