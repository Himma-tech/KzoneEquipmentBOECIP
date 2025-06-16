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

namespace KZONE.EntityManager
{
    public class DailyCheckManager
    {
        /// <summary>
        /// 各机台的Daily Check设定档
        /// </summary>
        public IDictionary<string, IList<DailyCheckData>> _eqpDailyCheckDatas = new Dictionary<string, IList<DailyCheckData>>();
        /// <summary>
        /// 各机台的FDC 设定档
        /// </summary>
         Dictionary<string, IList<FDCDataEntityData>> _fdcEntitys = new Dictionary<string, IList<FDCDataEntityData>>();
        /// <summary>
        /// NLog Logger Name
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// 用來讀取DB
        /// </summary>
        public HibernateAdapter HibernateAdapter { get; set; }

        public void Init()
        {
            _eqpDailyCheckDatas = Reload();
            _fdcEntitys = FDCReload();
        }

        protected IDictionary<string, IList<DailyCheckData>> Reload()
        {
            try
            {
                IDictionary<string, IList<DailyCheckData>> eqpDailyCheckDatas = new Dictionary<string, IList<DailyCheckData>>();
                string hql = string.Format("from DailyCheckEntityData where LINETYPE = '{0}' order by NODENO, OBJECTKEY", Workbench.LineType);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<DailyCheckData> eqpDailyCheckDataList = null;
                if (list != null)
                {
                    foreach (DailyCheckEntityData data in list)
                    {
                        //过滤一些特殊字符
                        //data.PARAMETERNAME = data.PARAMETERNAME.Replace(',', ';').Replace('=', '-').Replace('<', '(').Replace('>', ')').Replace('/', '-');
                        data.PARAMETERNAME = Regex.Replace(data.PARAMETERNAME, @"[/| |?|/|<|>|'|\-|,]", "_");
                        if (!eqpDailyCheckDatas.ContainsKey(data.NODENO))
                        {
                            eqpDailyCheckDataList = new List<DailyCheckData>();
                            eqpDailyCheckDatas.Add(data.NODENO, eqpDailyCheckDataList);
                        }
                        eqpDailyCheckDatas[data.NODENO].Add(new DailyCheckData(data));

                    }
                }
                return eqpDailyCheckDatas;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                throw ;
            }
        }

        /// <summary>
        /// 取得Daily Check 设定档
        /// </summary>
        public IList<DailyCheckData> GetDailyCheckProfile(string eqpNo)
        {
            try
            {
                if (_eqpDailyCheckDatas.ContainsKey(eqpNo))
                {
                    lock (_eqpDailyCheckDatas)
                    {
                        return _eqpDailyCheckDatas[eqpNo];
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
                IDictionary<string, IList<DailyCheckData>> tempDic = Reload();

                if (tempDic != null)
                {
                    lock (_eqpDailyCheckDatas)
                    {
                        _eqpDailyCheckDatas = tempDic;
                    }

                }
                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                           string.Format("Reload Daily Check Data all Equipment."));
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
                string hql = string.Format("from DailyCheckEntityData where LINETYPE = '{0}' and NODENO='{1}' order by SVID ", Workbench.LineType, eqpNo);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<DailyCheckData> eqpDailyCheckDataList = new List<DailyCheckData>();
                if (list != null)
                {
                    foreach (DailyCheckEntityData data in list)
                    {
                        //过滤一些特殊字符
                        //data.PARAMETERNAME = data.PARAMETERNAME.Replace(',', ';').Replace('=', '-').Replace('<', '(').Replace('>', ')').Replace('/', '-');
                        data.PARAMETERNAME = Regex.Replace(data.PARAMETERNAME, @"[/| |?|/|<|>|'|\-|,]", "_");
                        eqpDailyCheckDataList.Add(new DailyCheckData(data));
                    }
                    lock (_eqpDailyCheckDatas)
                    {
                        if (_eqpDailyCheckDatas.ContainsKey(eqpNo))
                        {
                            _eqpDailyCheckDatas.Remove(eqpNo);
                        }
                        _eqpDailyCheckDatas.Add(eqpNo, eqpDailyCheckDataList);
                    }
                }
                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            string.Format("Reload Daily Check Data EQUIPMENT=[{0}]", eqpNo));

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public System.Data.DataTable GetDataTable(string entityName)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("DailyCheckManager");
            return entityNames;
        }

        protected Dictionary<string, IList<FDCDataEntityData>> FDCReload()
        {
            try
            {
                FDCDataEntityData recipeEntity;
                IList<FDCDataEntityData> recipeEntityList = new List<FDCDataEntityData>(); 
                Dictionary<string, IList<FDCDataEntityData>> fdcEntitys = new Dictionary<string, IList<FDCDataEntityData>>();
                string hql = string.Format("from FDCDataEntityData where LINEID = '{0}' order by NODENO, OBJECTKEY", Workbench.ServerName);
                IList lists = this.HibernateAdapter.GetObjectByQuery(hql);
                if (lists != null)
                {
                    foreach (FDCDataEntityData entity in lists)
                    {
                        entity.PARAMETERNAME = Regex.Replace(entity.PARAMETERNAME, @"[/| |?|/|<|>|'|\-|,]", "_");
                        if (!fdcEntitys.ContainsKey(entity.NODENO))
                        {
                            recipeEntity = new FDCDataEntityData();
                            fdcEntitys.Add(entity.NODENO, recipeEntityList);
                        }
                        fdcEntitys[entity.NODENO].Add(entity);
                    }
                   
                }
                return fdcEntitys;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                throw;
            }
        }

        public void FDCReloadAll()
        {
            try
            {
                Dictionary<string, IList<FDCDataEntityData>> tempDic = FDCReload();

                if (tempDic != null)
                {
                    lock (_fdcEntitys)
                    {
                        _fdcEntitys = tempDic;
                    }

                }
                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                           string.Format("Reload FDC Data all Equipment."));
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        public IList<FDCDataEntityData> GetFDCDataProfile(string eqpNo)
        {
            try
            {
                if (_fdcEntitys.ContainsKey(eqpNo))
                {
                    lock (_fdcEntitys)
                    {
                        return _fdcEntitys[eqpNo];
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

    }
}
