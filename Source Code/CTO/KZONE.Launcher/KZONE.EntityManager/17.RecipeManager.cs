using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KZONE.Entity;
using KZONE.DB;
using KZONE.Log;
using System.Reflection;
using System.Collections;
using KZONE.Work;
using System.Text.RegularExpressions;
using KZONE.ConstantParameter;
using System.IO;

namespace KZONE.EntityManager
{
    public class RecipeManager
    {
        /// <summary>
        /// 各机台的Recipe Parameter设定档
        /// </summary>
        private IDictionary<string, IList<RecipeParameter>> _recipeParameters =
            new Dictionary<string, IList<RecipeParameter>>();

        /// <summary>
        /// 取得DB 内Recipe ID 设定档
        /// </summary>
        Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
            new Dictionary<string, Dictionary<string, RecipeEntityData>>();


        /// <summary>
        /// NLog Logger Name
        /// </summary>
        public string LoggerName { get; set; }

        private string _dataFilePath;

        private string _hisFilePath;
        /// <summary>
        /// Histrory  File Path
        /// </summary>
        public string HisFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_hisFilePath))
                {
                    _hisFilePath = "..\\Data\\RecipeParameters\\";

                }
                _hisFilePath = _hisFilePath.Replace("{ServerName}", Workbench.ServerName);
                return _hisFilePath;
            }
            set
            {
                _hisFilePath = value;
                if (_hisFilePath[_hisFilePath.Length - 1] != '\\')
                {
                    _hisFilePath = _hisFilePath + "\\";
                }
            }
        }
        public string DataFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_dataFilePath))
                {
                    _dataFilePath = "..\\Data\\RecipeParameters\\";
                }
                _dataFilePath = _dataFilePath.Replace("{ServerName}", Workbench.ServerName);
                return _dataFilePath;
            }
            set
            {
                _dataFilePath = value;
                if (_dataFilePath[_dataFilePath.Length - 1] != '\\')
                {
                    _dataFilePath = _dataFilePath + "\\";
                }
            }
        }

        /// <summary>
        /// 用來讀取DB
        /// </summary>
        public HibernateAdapter HibernateAdapter { get; set; }
        public HibernateAdapter HistoryHibernateAdapter { get; set; }

        public void Init()
        {
            _recipeParameters = Reload();
            _recipeEntitys = ReloadRecipe();//ReloadRecipeByNo();
        }

        protected IDictionary<string, IList<RecipeParameter>> Reload()
        {
            try
            {
                IDictionary<string, IList<RecipeParameter>> recipeParameters =
                    new Dictionary<string, IList<RecipeParameter>>();
                string hql =
                    string.Format("from RecipeParameterEntityData where SERVERNAME = '{0}' order by NODENO,WOFFSET ",
                        Workbench.ServerName);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<RecipeParameter> recipeList = null;
                if (list != null)
                {
                    foreach (RecipeParameterEntityData recipe in list)
                    {
                        //过滤一些特殊字符
                        //recipe.PARAMETERNAME = recipe.PARAMETERNAME.Replace(',', ';').Replace('=', '-').Replace('<', '(').Replace('>', ')').Replace('/', '-');
                        recipe.PARAMETERNAME = Regex.Replace(recipe.PARAMETERNAME, @"[/| |?|/|<|>|'|\-|,]", "_");
                        if (!recipeParameters.ContainsKey(recipe.NODENO))
                        {
                            recipeList = new List<RecipeParameter>();
                            recipeParameters.Add(recipe.NODENO, recipeList);
                        }

                        recipeParameters[recipe.NODENO].Add(new RecipeParameter(recipe));

                    }
                }

                return recipeParameters;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                    ex);
                throw;
            }
        }

        /// <summary>
        /// By Equipment No Reload Reipe Parameter
        /// </summary>
        /// <param name="nodeNo"></param>
        public void ReloadByEqpNO(string nodeNo)
        {
            try
            {
                string hql =
                    string.Format(
                        "from RecipeParameterEntityData where SERVERNAME = '{0}' and NODENO='{1}' order by WOFFSET ",
                        Workbench.ServerName, nodeNo);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<RecipeParameter> recipeList = new List<RecipeParameter>();
                if (list != null)
                {
                    foreach (RecipeParameterEntityData data in list)
                    {
                        //过滤一些特殊字符
                        //data.PARAMETERNAME = data.PARAMETERNAME.Replace(',', ';').Replace('=', '-').Replace('<', '(').Replace('>', ')').Replace('/', '-');
                        data.PARAMETERNAME = Regex.Replace(data.PARAMETERNAME, @"[/| |?|/|<|>|'|\-|,]", "_");
                        recipeList.Add(new RecipeParameter(data));
                    }

                    lock (_recipeParameters)
                    {
                        if (_recipeParameters.ContainsKey(nodeNo))
                        {
                            _recipeParameters.Remove(nodeNo);
                        }

                        _recipeParameters.Add(nodeNo, recipeList);
                    }
                }

                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                    string.Format("Reload Recipe Parameter EQUIPMENT=[{0}]", nodeNo));

            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                    ex);
            }
        }

        /// <summary>
        /// Relaod All Equipment RecipeParameter
        /// </summary>
        public void ReloadAll()
        {
            try
            {
                IDictionary<string, IList<RecipeParameter>> tempDic = Reload();

                if (tempDic != null)
                {
                    lock (_recipeParameters)
                    {
                        _recipeParameters = tempDic;
                    }

                }

                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                    string.Format("Reload Recipe Parameter all Equipment."));
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        /// <summary>
        /// By Equipment No Get RecipeParameter 
        /// </summary>
        /// <param name="equipmentNo"></param>
        /// <returns></returns>
        public IList<RecipeParameter> GetRecipeParameter(string equipmentNo)
        {
            try
            {
                if (_recipeParameters.ContainsKey(equipmentNo))
                {
                    return _recipeParameters[equipmentNo];
                }

                return null;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        public RecipeEntityData FindOne(string lineType, string lineRecipeName, string hostMode)
        {
            try
            {
                if (this.HibernateAdapter != null)
                {
                    IList list2 = HibernateAdapter.GetObject_AND(typeof(RecipeEntityData),
                        new string[] {"LINETYPE", "LINERECIPENAME", "ONLINECONTROLSTATE"},
                        new object[] {lineType, lineRecipeName, hostMode}, null, null);

                    if (list2 != null && list2.Count > 0)
                    {
                        return (RecipeEntityData) list2[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()", ex);
            }

            return null;
        }

        public RecipeEntityData FindOne(string nodeNo, string recipeID)
        {
            try
            {
                if (this.HibernateAdapter != null)
                {
                    IList list2 = HibernateAdapter.GetObject_AND(typeof(RecipeEntityData),
                        new string[] {"NODENO", "RECIPEID"},
                        new object[] { nodeNo, recipeID }, null, null);

                    if (list2 != null && list2.Count > 0)
                    {
                        return (RecipeEntityData) list2[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()", ex);
            }

            return null;
        }
       
        //20161214 add PPID Use Flag Info
        public void SaveRecipeObject(Line line, string lineRecipeName, string ppid, string ip, string recipeType,
            string recipePPIDUseFlag)
        {
            try
            {
                if (ppid.Substring(ppid.Length - 1, 1) == ";")
                {
                    ppid = ppid.Substring(0, ppid.Length - 1);
                }

                //20161214 add PPID UseFlag去掉最後的;
                if (recipePPIDUseFlag.Substring(recipePPIDUseFlag.Length - 1, 1) == ";")
                {
                    recipePPIDUseFlag = recipePPIDUseFlag.Substring(0, recipePPIDUseFlag.Length - 1);
                }

                //(string lineType, string lineRecipeName, string hostMode)
                RecipeEntityData recipe = FindOne(line.Data.LINETYPE, lineRecipeName, line.File.HostMode.ToString());
                if (recipe != null)
                {

                    HibernateAdapter.UpdateObject(recipe);
                }
                else
                {
                    recipe = new RecipeEntityData();

                    recipe.LINETYPE = line.Data.LINETYPE;
                    recipe.FABTYPE = line.Data.FABTYPE;


                    HibernateAdapter.SaveObject(recipe);


                }

            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void SaveRecipeOject(RecipeEntityData newRecipe)
        {
            try
            {
                HibernateAdapter.SaveObject(newRecipe);

            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void SaveRecipeHistory(RecipeEntityData recipe,eRecipeEvent eventName,string opID)
        {
            try
            {
                RecipeHistory hisRecipe = new RecipeHistory();
                hisRecipe.UPDATETIME = DateTime.Now;
                hisRecipe.NODENAME = recipe.NODENO;
                hisRecipe.RECIPENO = recipe.RECIPENO;
                hisRecipe.RECIPEID = recipe.RECIPEID;
                hisRecipe.CREATETIME = recipe.CREATETIME;
                hisRecipe.VERSIONNO = recipe.VERSIONNO;
                hisRecipe.EVENT = eventName.ToString();
                hisRecipe.FILENAME = recipe.FILENAME;
                hisRecipe.RECIPESTATUS = recipe.RECIPESTATUS;
                hisRecipe.OPERATORID = opID;

                HistoryHibernateAdapter.SaveObject(hisRecipe);
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void DeleteRecipeObject(string recipeID, string nodeNo)
        {
            try

            {
                RecipeEntityData recipe = FindOne(nodeNo, recipeID);
                if (recipe !=null)
                {
                    HibernateAdapter.DeleteObject(recipe);
                }
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()"
                    , ex);
            }
        }

        public void UpateRecipeObject(RecipeEntityData upateRecipe)
        {
            try
            {
                RecipeEntityData recipe = FindOne(upateRecipe.NODENO, upateRecipe.RECIPEID);
                if (recipe != null)
                {
                    HibernateAdapter.UpdateObject(upateRecipe);
                }
                else
                {
                    HibernateAdapter.SaveObject(upateRecipe);
                }
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name + "()"
                    , ex);
            }
        }


        /// <summary>
        /// 取出DB Recipe ID 的设定档存入Dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, RecipeEntityData>> ReloadRecipe()
        {
            try
            {
                Dictionary<string, RecipeEntityData> recipeEntity;
                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeEntitys = new  Dictionary<string, Dictionary<string, RecipeEntityData>>();
                string hql = string.Format("from RecipeEntityData where LINEID = '{0}'", Workbench.ServerName);
                IList lists = this.HibernateAdapter.GetObjectByQuery(hql);
                if (lists != null)
                {
                    foreach (RecipeEntityData entity in lists)
                    {
                        if (!recipeEntitys.ContainsKey(entity.LINEID))
                        {
                            recipeEntity = new Dictionary<string, RecipeEntityData>();
                            recipeEntitys.Add(entity.LINEID, recipeEntity);
                        }
                        recipeEntitys[entity.LINEID].Add(entity.RECIPENO, entity);
                    }
                }
                return recipeEntitys;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                throw;
            }
        }

        public Dictionary<string, Dictionary<string, RecipeEntityData>> ReloadRecipeByNo()
        {
            try
            {
                Dictionary<string, RecipeEntityData> recipeEntity;
                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeEntitys = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                string hql = string.Format("from RecipeEntityData where LINEID = '{0}'", Workbench.ServerName);
                IList lists = this.HibernateAdapter.GetObjectByQuery(hql);
                if (lists != null)
                {
                    foreach (RecipeEntityData entity in lists)
                    {
                        if (!recipeEntitys.ContainsKey(entity.LINEID))
                        {
                            recipeEntity = new Dictionary<string, RecipeEntityData>();
                            recipeEntitys.Add(entity.LINEID, recipeEntity);
                        }
                        recipeEntitys[entity.LINEID].Add(entity.RECIPENO, entity);
                    }
                }
                return recipeEntitys;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                throw;
            }
        }
        /// <summary>
        /// OPI Reload DB SBRM_RECIPE  使用此方法
        /// </summary>
        public void ReloadRecipeTable()
        {
            try
            {
                Dictionary<string, Dictionary<string, RecipeEntityData>> tempDic = ReloadRecipe();

                if (tempDic != null)
                {
                    lock (_recipeEntitys)
                    {
                        _recipeEntitys = tempDic;
                    }

                }
                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                           string.Format("Reload SBRM_RECIPE Success"));
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }
        /// <summary>
        ///取得指定LineType 和 LineRecipeName 的Recipe ID 设定档 For BCS 使用
        /// </summary>
        /// <param name="linetype"></param>
        /// <param name="lineRecipeName"></param>
        /// <returns></returns>
        public RecipeEntityData GetRecipeSeting(string linetype , string lineRecipeName)
        {
            RecipeEntityData ret = null;
            try
            {
                if (_recipeEntitys.ContainsKey(linetype))
                {
                    if (_recipeEntitys[linetype].ContainsKey(lineRecipeName))
                    {
                        ret = _recipeEntitys[linetype][lineRecipeName];
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
        /// write Recipe Parameter  to File
        /// </summary>
     
        public void MakeRecipeDataValuesToFile(string fileName, string value)
        {
            try
            {
                string directoryPath = this.DataFilePath;
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filePath = string.Format("{0}\\{1}.txt", directoryPath, fileName);

                File.WriteAllText(filePath, value);

               
                string hisFilePath = this.HisFilePath;
               
                string newFilePath = string.Format("{0}{1}.txt", hisFilePath, fileName);
                if (Directory.Exists(hisFilePath) == false)
                {
                    Directory.CreateDirectory(hisFilePath);
                }
                    File.Copy(filePath, newFilePath,true);

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 移动指定Recipe文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void MoveRecipeDataValuesToFile(string fileName)
        {
            try
            {
                string directoryPath = this.DataFilePath;
                string hisFilePath = this.HisFilePath;
                string filePath = string.Format("{0}{1}.txt", directoryPath, fileName);
                string newFilePath = string.Format("{0}{1}.txt", hisFilePath, fileName);
                if (File.Exists(filePath) == true)
                {
                    if (Directory.Exists(hisFilePath) == false)
                    {
                        Directory.CreateDirectory(hisFilePath);
                    }
                    if (File.Exists(newFilePath) == false)
                    {
                        Directory.Move(filePath, newFilePath);
                    }
                    else {
                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public IList<string> RecipeDataValues(bool his,string key)
        {
            List<string> paramter = null; ;
            try
            {
                if (string.IsNullOrEmpty(key) == true)
                    return null;

                if (key.Split('_').Length < 4)
                    throw new Exception(string.Format("File Name Format Error [{0}]!", key));

              //  string timeKey = key.Split('_')[3].ToString().Trim();
                string directoryPath;
                if (!his)
                {
                    directoryPath = this.DataFilePath ;
                } 
                else {
                    directoryPath = this.HisFilePath ;
                }
                if (!File.Exists(directoryPath + key+".txt"))
                {
                    throw new Exception(string.Format("Can't find Recipe Data File [{0}] in Directory!", key));
                }
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
    }
     
}


public class RecipeDataReportItem
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public RecipeDataReportItem(string n, string v)
        {
            _name = n;
            _value = v;
        }

        public RecipeDataReportItem()
        {

        }
    }

