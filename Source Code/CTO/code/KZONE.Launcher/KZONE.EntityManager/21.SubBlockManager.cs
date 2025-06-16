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
using System.Data;

namespace KZONE.EntityManager
{
    public class SubBlockManager:IDataSource
    {
        /// <summary> 目前ServerName相關的Sub Block設定.以Start EQP為Key.
        /// 
        /// </summary>
        private IDictionary<string, IList<SubBlock>> _subBlocks = new Dictionary<string, IList<SubBlock>>();

        public IDictionary<string, IList<SubBlock>> SubBlocks
        {
            get { return _subBlocks; }
            set { _subBlocks = value; }
        }

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
         //   _subBlocks = Reload();
        }

        /// <summary>
        /// Load SubBlocks
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IList<SubBlock>> Reload()
        {
            try
            {
                IDictionary<string, IList<SubBlock>> subBlocks = new Dictionary<string, IList<SubBlock>>();
                string hql = string.Format("from SubBlockEntityData where SERVERNAME = '{0}'", Workbench.ServerName);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<SubBlock> subBlockList = null;
                if (list != null)
                {
                    foreach (SubBlockEntityData subBlock in list)
                    {
                        if (!subBlocks.ContainsKey(subBlock.STARTEQP))
                        {
                            subBlockList = new List<SubBlock>();
                            subBlocks.Add(subBlock.STARTEQP, subBlockList);
                        }
                        subBlocks[subBlock.STARTEQP].Add(new SubBlock(subBlock));
                       
                    }
                }
                return subBlocks;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                throw ;
            }
        }

        /// <summary>
        /// Reload SubBlocks by UI
        /// </summary>
        /// <returns></returns>
        public void ReloadByUI()
        {
            try
            {

                //20161208 modify Entity Name Error Bug
                string hql = string.Format("from SubBlockEntityData where SERVERNAME = '{0}'", Workbench.ServerName);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<SubBlock> subBlockList = null;
                if (list != null)
                {
                    lock (_subBlocks)
                    {
                        _subBlocks=new Dictionary<string, IList<SubBlock>>();
                        foreach (SubBlockEntityData subBlock in list)
                        {
                            if (!_subBlocks.ContainsKey(subBlock.STARTEQP))
                            {
                                subBlockList = new List<SubBlock>();
                                _subBlocks.Add(subBlock.STARTEQP, subBlockList);
                            }
                            _subBlocks[subBlock.STARTEQP].Add(new SubBlock(subBlock));
                        }
                    }
                }
                NLogManager.Logger.LogInfoWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            "Reload SubBlock by UI!");

            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary> 获取Start EQP為 node+unit的SubBlock. : Start EQP Format:Node= N:L2 , Unit= U:L12:1 , Port= P:L30:2
        ///
        /// </summary>
        /// <param name="nodeno"></param>
        /// <param name="unitno"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public IDictionary<string, IList<SubBlock>> GetBlock(string nodeno,int unitno,string eventName)
        {
            try
            {
                IDictionary<string, IList<SubBlock>> subBlocks = new Dictionary<string, IList<SubBlock>>();
                
                string startEq="";
                
                if (unitno == 0)
                {
                    startEq = string.Format("N:{0}", nodeno);
                }
                else
                {
                    startEq = string.Format("U:{0}:{1}", nodeno, unitno.ToString());
                }

                if (!_subBlocks.Keys.Contains(startEq))
                {
                    return null;
                }

                IList<SubBlock> list = _subBlocks[startEq];
                IList<SubBlock> subBlockList = null;

                foreach (SubBlock subBlock in list)
                {
                    if (subBlock.Data.STARTEVENTMSG==eventName)
                    {
                        if (!subBlocks.ContainsKey(startEq))
                        {
                            subBlockList = new List<SubBlock>();
                            subBlocks.Add(startEq, subBlockList);
                        }
                        subBlocks[startEq].Add(new SubBlock(subBlock.Data));
                    }
                }

                return subBlocks;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        /// <summary> 获取对应node+port的SubBlock : Start EQP Format:Node= N:L2 , Unit= U:L12:1 , Port= P:L30:2
        ///
        /// </summary>
        /// <param name="nodeno"></param>
        /// <param name="portno"></param>
        /// <returns></returns>
        public IDictionary<string, IList<SubBlock>> GetBlock(string nodeno, int portno)
        {
            try
            {
                IDictionary<string, IList<SubBlock>> subBlocks = new Dictionary<string, IList<SubBlock>>();
                string startEq = "";
                //if (!portno.Contains("P"))
                //{
                //    return subBlocks;
                //}
                //else
                //{

                //}
                startEq = string.Format("P:{0}:{1}", nodeno, portno);

                if (!_subBlocks.Keys.Contains(startEq))
                {
                    return null;
                }
                IList<SubBlock> list = _subBlocks[startEq];
                IList<SubBlock> subBlockList = null;
                foreach (SubBlock subBlock in list)
                {
                    if (!subBlocks.ContainsKey(startEq))
                    {
                        subBlockList = new List<SubBlock>();
                        subBlocks.Add(startEq, subBlockList);
                    }
                    subBlocks[startEq].Add(new SubBlock(subBlock.Data));                    
                }
                return subBlocks;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        #region Implement IDataSource

        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("SubBlockManager");
            return entityNames;
        }

        public DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                SubBlockEntityData data = new SubBlockEntityData();

                DataTableHelp.DataTableAppendColumn(data, dt);

                foreach (string key in _subBlocks.Keys)
                {
                    IList<SubBlock> specs = _subBlocks[key];
                    foreach (SubBlock spec in specs)
                    {
                        DataRow dr = dt.NewRow();
                        DataTableHelp.DataRowAssignValue(spec.Data, dr);

                        dt.Rows.Add(dr);
                    }

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

        //20161004 add for Get SubBlockList
        public IList<SubBlock> GetBlockList(string _nodeNo, string _eventName)
        {
            return GetBlockList(_nodeNo, string.Empty, _eventName, false);
        }

        /// <summary> Get BlockList by NodeNo and UnitNo or PortNo
        /// 
        /// </summary>
        /// <param name="_nodeNo"></param>
        /// <param name="_unitNo_portNo"></param>
        /// <param name="_eventName"></param>
        /// <param name="isPortflag">True:Port, False:Node or Unit</param>
        /// <returns></returns>
        public IList<SubBlock> GetBlockList(string _nodeNo, string _unitNo_portNo, string _eventName, bool isPortflag)
        {
            string _s = string.Empty;
            IList<SubBlock> _result = null;

            try
            {
                string startEq = "";

                if (_unitNo_portNo == "" || _unitNo_portNo == "0")
                {
                    startEq = string.Format("N:{0}", _nodeNo);
                }
                else
                {
                    if (isPortflag == true)
                    {
                        startEq = string.Format("P:{0}:{1}", _nodeNo, _unitNo_portNo);
                    }
                    else
                    {
                        startEq = string.Format("U:{0}:{1}", _nodeNo, _unitNo_portNo);
                    }
                }

                //20170424 modify針對設定檔內的StartEQP為多Node的處理 EX: N:L6,N:L12
                //if (!_subBlocks.Keys.Contains(startEq))
                //{
                //    return null;
                //}

                string startEQPKey = string.Empty;

                foreach (string startEQPList in _subBlocks.Keys)
                {
                    if (startEQPList.Contains(startEq) == true)
                    {
                        startEQPKey = startEQPList;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(startEQPKey) == true)
                {

                    return null;
                }

                //20170424 modify針對設定檔內的StartEQP為多Node的處理 EX: N:L6,N:L12
                //IList<SubBlock> list = _subBlocks[startEq];
                IList<SubBlock> list = _subBlocks[startEQPKey];

                List<SubBlock> subBlockList = new List<SubBlock>();

                foreach (SubBlock subBlock in list)
                {
                    if (subBlock.Data.STARTEVENTMSG == _eventName)
                    {
                        subBlockList.Add(new SubBlock(subBlock.Data));
                    }
                }
                _result = subBlockList;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return _result;
        }

    }
}
