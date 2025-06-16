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
    public class SubJobDataManager:IDataSource
    {
        /// <summary>
        /// 所有需要拆解的SubJobData
        /// </summary>
        private IDictionary<string, IList<SubJobData>> _subJobDatas = new Dictionary<string, IList<SubJobData>>();

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
            _subJobDatas = Reload();
        }

        /// <summary>
        /// Load SubJobData list
        /// </summary>
        /// <returns></returns>
        protected IDictionary<string, IList<SubJobData>> Reload()
        {
            IDictionary<string, IList<SubJobData>> subJobDatas = new Dictionary<string, IList<SubJobData>>();
              
            try
            {
                //modify by bruce 2015/7/16 修改篩選條件
                string hql = string.Format("from SubJobDataEntityData where LINETYPE = '{0}' order by subitemlength,subitemloffset", Workbench.LineType);
                IList list = this.HibernateAdapter.GetObjectByQuery(hql);
                IList<SubJobData> subJobDataList = null;
                if (list != null)
                {
                    foreach (SubJobDataEntityData subJobData in list)
                    {
                        if (!subJobDatas.ContainsKey(subJobData.ITEMNAME.Trim()))
                        {
                            subJobDataList = new List<SubJobData>();
                            subJobDatas.Add(subJobData.ITEMNAME.Trim(), subJobDataList);
                        }
                        subJobDatas[subJobData.ITEMNAME.Trim()].Add(new SubJobData(subJobData));

                    }
                }
                return subJobDatas;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                return subJobDatas;
            }
        }

        /// <summary>
        /// value为PLC Agent解出的原始字符串,decode成10进制的subitem值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public IDictionary<string, string> Decode(string value,string itemName)
        {
            IDictionary<string, string> _subItemValues = new Dictionary<string, string>();
            try
            {
                if (!_subJobDatas.Keys.Contains(itemName.Trim()))
                {
                    return _subItemValues;
                }
                IList<SubJobData> list = _subJobDatas[itemName.Trim()];
                foreach (SubJobData subJobData in list)
                {
                    string subItemValue = value.Substring(subJobData.Data.SUBITEMLOFFSET, subJobData.Data.SUBITEMLENGTH);
                    //需要反转字符串
                    char[] csubItemValue = subItemValue.ToCharArray().Reverse().ToArray();
                    subItemValue = new string(csubItemValue);
                    if (!_subItemValues.ContainsKey(subJobData.Data.SUBITEMNAME))
                    {

                        _subItemValues.Add(subJobData.Data.SUBITEMNAME, Convert.ToUInt32(subItemValue,2).ToString());
                    }
                }
                return _subItemValues;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,string.Format("ItemName={0},Value={1}.\n",itemName,value), ex);
                return _subItemValues;
            }
        }

        /// <summary>
        /// dicSubItems中value为10进制值,转成PLC Agent所需的string
        /// </summary>
        /// <param name="dicSubItems"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string Encode(IDictionary<string, string> dicSubItems, string itemName)
        {
            string strJobData = "";
            try
            {
                if (!_subJobDatas.Keys.Contains(itemName.Trim()))
                {
                    return "";
                }
                IList<SubJobData> list = _subJobDatas[itemName.Trim()];
                list=list.OrderBy(s => s.Data.SUBITEMLOFFSET).ToList<SubJobData>();
                int itemLenth = list[0].Data.ITEMLENGTH;
                strJobData = new string('0', itemLenth);
                foreach (SubJobData subJobData in list) {
                    string value = "";
                    if (dicSubItems.ContainsKey(subJobData.Data.SUBITEMNAME)) {

                        value = dicSubItems[subJobData.Data.SUBITEMNAME] == "" ? "0" : dicSubItems[subJobData.Data.SUBITEMNAME];
                        string tempValue = Convert.ToString(int.Parse(value), 2);
                        char[] charValue = tempValue.ToCharArray().Reverse().ToArray();
                        value = new string(charValue);
                        strJobData=Replace(strJobData, subJobData, value);
                    }
                    
                    //if (value.Length>subJobData.Data.SUBITEMLENGTH)
                    //{
                    //    strJobData += value.Substring(0, subJobData.Data.SUBITEMLENGTH);
                    //    continue;
                    //}
                    //strJobData += value.PadRight(subJobData.Data.SUBITEMLENGTH,'0');
                }
                //strJobData = strJobData.PadRight(itemLenth,'0');
                return strJobData;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                return strJobData;
            }
        }

        private string Replace(string source, SubJobData item, string value)
        {
            string first = source.Substring(0,item.Data.SUBITEMLOFFSET);
            string secode = source.Substring(item.Data.SUBITEMLOFFSET + item.Data.SUBITEMLENGTH);

            source=string.Format("{0}{1}{2}",first,value.PadRight(item.Data.SUBITEMLENGTH,'0'),secode);
            return source;
        }

        /// <summary>
        /// 获取subitem的集合
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public IDictionary<string,string> GetSubItem(string itemName)
        {
            try
            {
                IDictionary<string,string> _subItems = new Dictionary<string,string>();
                if (!_subJobDatas.Keys.Contains(itemName.Trim()))
                {
                    return null;
                }
                IList<SubJobData> list = _subJobDatas[itemName.Trim()];
                foreach (SubJobData subJobData in list)
                {
                    if (!_subItems.ContainsKey(subJobData.Data.SUBITEMNAME))
                    {

                        _subItems.Add(subJobData.Data.SUBITEMNAME, "");
                    }
                }
                return _subItems;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取item的长度
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public int GetItemLenth(string itemName)
        {
            try
            {
                if (!_subJobDatas.Keys.Contains(itemName.Trim()))
                {
                    return 0;
                }
                int lenth=0;
                IList<SubJobData> list = _subJobDatas[itemName.Trim()];
                foreach (SubJobData subJobData in list)
                {
                    if (subJobData.Data.ITEMLENGTH!=0)
                    {
                        lenth = subJobData.Data.ITEMLENGTH;
                        break;
                    }
                }
                return lenth;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return 0;
            }
        }

        public DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                SubJobDataEntityData data = new SubJobDataEntityData();

                DataTableHelp.DataTableAppendColumn(data, dt);

                foreach (string key in _subJobDatas.Keys)
                {
                    IList<SubJobData> specs = _subJobDatas[key];
                    foreach (SubJobData spec in specs)
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

        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("SubJobDataManager");
            return entityNames;
        }
    }
}
