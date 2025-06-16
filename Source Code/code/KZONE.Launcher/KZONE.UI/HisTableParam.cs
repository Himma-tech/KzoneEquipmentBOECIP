using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Log;


namespace KZONE.UI
{
    /// <summary> 動態產生的元件類型
    /// 
    /// </summary>
    enum FieldTypes
    {
        /// <summary> 未定義
        /// 
        /// </summary>
        Unknown,
        /// <summary> 日期區間元件
        /// 
        /// </summary>
        DateTime,
        /// <summary> 下拉選單元件
        /// 
        /// </summary>
        ComboBox,
        /// <summary> 文字輸輸框元件
        /// 
        /// </summary>
        TextBox
    }

    /// <summary> FormHistoryBase 參數類別
    /// 
    /// </summary>
    public class HisTableParam
    {
        #region Fields
        private string _tableName = string.Empty;
        private List<string> _lstQueryFields = new List<string>();
        private List<string> _lstDisplayFields = new List<string>();
        private List<string> _lstFieldWidth = new List<string>();
        private string _sortFields = string.Empty;
        private string _dateTimeField = string.Empty;
        private string _cboValueSQL = string.Empty;

        private List<ColumnProp> _lstGridViewColumnInfo = new List<ColumnProp>();
        private List<ParamInfo> _lstConditionParamInfo = new List<ParamInfo>();
        private string _sqlQuery = string.Empty;
        #endregion

        #region Property
        /// <summary> 表單上DataGridView欄位資訊
        /// 
        /// </summary>
        public List<ColumnProp> GridViewColumnInfo
        {
            get { return _lstGridViewColumnInfo; }
        }

        /// <summary> 表單上查詢條件資訊
        /// 
        /// </summary>
        public List<ParamInfo> ConditionParamInfo
        {
            get { return _lstConditionParamInfo; }
        }

        /// <summary> 查詢的資料表名稱
        /// 
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }

        /// <summary> 查詢的欄位 (A1,A2,A3,...)
        /// 
        /// </summary>
        public List<string> QueryFields
        {
            get { return _lstQueryFields; }
        }

        /// <summary> 查詢的SQL Statement (SELECT * FROM XXX)
        /// 
        /// </summary>
        public string SqlQuery
        {
            get { return _sqlQuery; }
        }

        /// <summary> 查詢SQL DateTime 條件
        /// 
        /// </summary>
        public string DateTimeField
        {
            get { return _dateTimeField; }
        }

        /// <summary> 排序的SQL Statement (ORDER BY XXX)
        /// 
        /// </summary>
        public string SortExpression
        {
            get { return _sortFields; }
        }
        #endregion

        #region Constructor
        public HisTableParam(string tableName)
        {
            this._tableName = tableName;
            this.Initial();
        }
        #endregion

        #region Private Methods
        private void Initial()
        {
            DataTable dt = this.GetFormParamFromDB();
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            _lstQueryFields = (dr["QUERYFIELDS"].ToString().Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
            _lstDisplayFields = (dr["DISPLAYFIELDS"].ToString().Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
            _lstFieldWidth = (dr["FIELDWIDTH"].ToString().Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
            _sortFields = dr["SORTFIELDS"].ToString();
            _dateTimeField = dr["TIMESTAMPFIELD"].ToString();
            _cboValueSQL = dr["FIELDCOMBOSQL"].ToString();

            #region Check 欄位是否存在entity
            List<string> _lstColName = new List<string>();

            switch (dr["TABLENAME"].ToString())
            {
                case "SBCS_ALARMHISTORY":
                    _lstColName = typeof(SBCS_ALARMHISTORY).GetProperties().Select(a => a.Name).ToList();                    
                    break;

                case "SBCS_PROCESSDATAHISTORY":
                    _lstColName = typeof(SBCS_PROCESSDATAHISTORY).GetProperties().Select(a => a.Name).ToList();
                    break;

                case "SBCS_JOBHISTORY":
                    _lstColName = typeof(SBCS_JOBHISTORY).GetProperties().Select(a => a.Name).ToList();
                    break;

                case "SBCS_NODEHISTORY":
                    _lstColName = typeof(SBCS_NODEHISTORY).GetProperties().Select(a => a.Name).ToList();
                    break;

                case "SBCS_UNITHISTORY":
                    _lstColName = typeof(SBCS_UNITHISTORY).GetProperties().Select(a => a.Name).ToList();
                    break;


                case "SBCS_CIMMESSAGEHISTORY":
                    _lstColName = typeof(SBCS_CIMMESSAGEHISTORY).GetProperties().Select(a => a.Name).ToList(); 
                    break;

                case "SBRM_RECIPE":
                    _lstColName = typeof(SBRM_RECIPE).GetProperties().Select(a => a.Name).ToList(); 
                    break;

                case "SBCS_TANKHISTORY":
                    _lstColName = typeof(SBCS_TANKHISTORY).GetProperties().Select(a => a.Name).ToList();
                    break;

                case "SBCS_RECIPEHISTORY":
                    _lstColName = typeof(SBCS_RECIPEHISTORY).GetProperties().Select(a => a.Name).ToList(); 
                    break;

                case "SBCS_TACTDATAHISTORY":
                    _lstColName = typeof(SBCS_TACTDATAHISTORY).GetProperties().Select(a => a.Name).ToList(); 
                    break;
                case "SBCS_MATERIALHISTORY":
                    _lstColName = typeof(SBCS_MATERIALHISTORY).GetProperties().Select(a => a.Name).ToList();
                    break;
            }

            List<int> _lstRemoveIndex = new List<int>();

            for (int i = _lstQueryFields.Count-1; i >= 0 ; i--)
            {
                if (_lstColName.Contains(_lstQueryFields[i]) == false) _lstRemoveIndex.Add(i);
            }

            foreach (int i in _lstRemoveIndex)
            {
                _lstQueryFields.RemoveAt(i);
                _lstDisplayFields.RemoveAt(i);
                _lstFieldWidth.RemoveAt(i);
            }
            #endregion

            this.SetHistField();
            this.SetHistParam(dt);
        }

        private DataTable GetFormParamFromDB()
        {
            OPIInfo opiap = OPIInfo.CreateInstance();

            DBSETDataContext ctx = opiap.DBBRMCtx;//.DBCtx;

            var query =
                    (from hf in ctx.SBRM_HISTABLEFIELDS
                     join hp in ctx.SBRM_HISTABLEPARAM on hf.TABLENAME equals hp.TABLENAME into subGrp
                     from hp in subGrp.DefaultIfEmpty()
                     where hf.TABLENAME == _tableName && hf.HISTORYTYPE == hp.HISTORYTYPE && hf.HISTORYTYPE == opiap.CurLine.HistoryType
                     orderby hp.FIELDSEQUENCE
                     select new
                     {
                         hf.TABLENAME,
                         hf.QUERYFIELDS,
                         hf.DISPLAYFIELDS,
                         hf.FIELDWIDTH,
                         hf.SORTFIELDS,
                         hp.FIELDKEY,
                         hp.FIELDCAPTION,
                         hp.FIELDTYPE,
                         hp.FIELDDISPLAY,
                         hp.FIELDVALUE,
                         hp.FIELDDEFAULT,
                         hp.FIELDSEQUENCE,
                         hf.TIMESTAMPFIELD,
                         hp.FIELDCOMBOSQL,
                         hp.FIELDATTRIBUTE,
                         hp.FIELDCOMBORELATION,
                         hp.ITEMCONDITION
                     });

            return DBConnect.ToDataTable(query);
        }

        private void SetHistField()
        {
            _lstGridViewColumnInfo.Clear();

            StringBuilder sbSql = new StringBuilder("SELECT * ");
            ColumnProp cp = null;
            for(int idx = 0; idx < _lstDisplayFields.Count; idx++)
            {
                cp = new ColumnProp()
                {
                    DataField = _lstQueryFields[idx],
                    DisplayField = _lstDisplayFields[idx],
                    FieldWidth = _lstFieldWidth[idx],
                    FieldFormat = _lstQueryFields[idx].EndsWith("DATETIME") ? "yyyy/MM/dd HH:mm:ss.fff" : ""    // 欄位名稱是DATETIME結尾的統一格式為「yyyy/MM/dd HH:mm:ss」
                    //FieldFormat = _lstQueryFields[idx].EndsWith("DATETIME") ? "yyyy/MM/dd tt hh:mm:ss" : ""
                };
                _lstGridViewColumnInfo.Add(cp);
            }
            sbSql.AppendFormat("FROM {0}", _tableName);

            _sqlQuery = sbSql.ToString();
        }



        private void SetHistParam(DataTable dt)
        {
            try
            {
                _lstConditionParamInfo.Clear();

                ParamInfo pi = null;
                DataRow dr = null;
                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {
                    dr = dt.Rows[idx];
                    if ("".Equals(dr["FIELDKEY"].ToString()))
                        continue;

                    if (_lstQueryFields.Contains(dr["FIELDKEY"].ToString()) == false)
                        continue ;

                    pi = new ParamInfo()
                    {
                        FieldKey = dr["FIELDKEY"].ToString(),
                        FieldCaption = dr["FIELDCAPTION"].ToString(),
                        FieldType = dr["FIELDTYPE"].ToString(),
                        FieldDisplay = dr["FIELDDISPLAY"].ToString(),
                        FieldValue = dr["FIELDVALUE"].ToString(),
                        FieldDefault = dr["FIELDDEFAULT"].ToString(),
                        FieldSequence = dr["FIELDSEQUENCE"].ToString(),
                        FieldCboValueSQL = dr["FIELDCOMBOSQL"].ToString(),
                        FiledAttribute = dr["FIELDATTRIBUTE"].ToString(),
                        RelationField = dr["FIELDCOMBORELATION"].ToString(),
                        ItemCondition = dr["ITEMCONDITION"].ToString()
                    };


                    #region 查詢comboBox item 內容
                    if (pi.FieldType == "ComboBox" && pi.FieldDisplay == string.Empty && pi.FieldCboValueSQL != string.Empty)
                    {
                        OPIInfo oPIInfo = OPIInfo.CreateInstance();
                        DBSETDataContext _ctx = oPIInfo.DBBRMCtx;  //FormMainMDI.G_OPIAp.DBCtx;
                        
                        string _data = string.Empty ;
                        if (pi.ItemCondition == "SERVERNAME")
                        {
                          //  _data = string.Format(pi.FieldCboValueSQL, oPIInfo.ServerName);
                        }
                        else if (pi.ItemCondition == "DATETIME")
                        {                            
                            //改至觸發時動態取得 FormHistory.cs -> dtpDateTime_ValueChanged
                            //string _start = string.Format("{0}:00:00", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH"));
                            //string _end = string.Format("{0}:59:59", DateTime.Now.ToString("yyyy-MM-dd HH"));
                            //_data = string.Format(pi.FieldCboValueSQL, _start, _end);
                        }
                        else
                        {
                            _data = pi.FieldCboValueSQL;
                        }

                        if (_data != string.Empty)
                        {
                            string _display = string.Empty;
                            string _value = string.Empty;

                            //判斷FIELDCOMBOSQL欄位為SQL or 物件名稱 --SQL 則執行SQL取得comboboxitem ; 物件名稱則取程式內對應物件資料(Node,Port,Unit)
                            switch (_data)
                            {
                                case "Node":

                                #region Node
                                foreach (Node _node in oPIInfo.Dic_Node.Values)
                                {
                                    _display = _display + (_display == string.Empty ? "" : ",") + string.Format("{0}-{1}-{2}", _node.NodeNo, _node.NodeID, _node.NodeName);
                                    _value = _value + (_value == string.Empty ? "" : ",") + _node.NodeID;
                                }
                                pi.FieldDisplay = _display;
                                pi.FieldValue = _value;
                                break;
                                #endregion

                                case "Port":

                                    //#region Port
                                    //foreach (Port _port in FormMainMDI.G_OPIAp.Dic_Port.Values)
                                    //{
                                    //    _display = _display + (_display == string.Empty ? "" : ",") + string.Format("{0}-{1}-{2}", _port.NodeNo, _port.PortNo, _port.PortID);
                                    //    _value = _value + (_value == string.Empty ? "" : ",") + _port.PortID;
                                    //}
                                    //pi.FieldDisplay = _display;
                                    //pi.FieldValue = _value;
                                    //break;
                                    //#endregion

                                case "Unit":

                                    #region Unit
                                foreach (Unit _unit in oPIInfo.Dic_Unit.Values)
                                    {
                                        _display = _display + (_display == string.Empty ? "" : ",") + string.Format("{0}-{1}-{2}", _unit.NodeNo, _unit.UnitNo.ToString().PadLeft(2, '0'), _unit.UnitID);
                                        _value = _value + (_value == string.Empty ? "" : ",") + _unit.UnitID;
                                    }
                                    pi.FieldDisplay = _display;
                                    pi.FieldValue = _value;
                                    break;
                                    #endregion

                                default: 

                                    #region SQL
                                    if (pi.FiledAttribute == "INT")
                                    {
                                        List<int> _lst = _ctx.ExecuteQuery<int>(_data).ToList();

                                        pi.FieldDisplay = string.Join(",", _lst.ToArray());
                                        pi.FieldValue = string.Join(",", _lst.ToArray());
                                    }
                                    else
                                    {
                                        List<string> _lst = _ctx.ExecuteQuery<string>(_data).ToList();

                                        pi.FieldDisplay = string.Join(",", _lst.ToArray());
                                        pi.FieldValue = string.Join(",", _lst.ToArray());
                                    }
                                    
                                    break;
                                    #endregion
                            }

                            //if (pi.FiledAttribute == "INT")
                            //{
                            //    List<int> _lst = _ctx.ExecuteQuery<int>(_data).ToList();

                            //    pi.FieldDisplay = string.Join(",", _lst.ToArray());
                            //    pi.FieldValue = string.Join(",", _lst.ToArray());
                            //}
                            //else
                            //{
                            //    List<string> _lst = _ctx.ExecuteQuery<string>(_data).ToList();

                            //    pi.FieldDisplay = string.Join(",", _lst.ToArray());
                            //    pi.FieldValue = string.Join(",", _lst.ToArray());
                            //}
                        }
                    }
                    #endregion

                    _lstConditionParamInfo.Add(pi);
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public DataTable ToDataTable(System.Data.Linq.DataContext ctx, object query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            IDbCommand cmd = ctx.GetCommand((IQueryable)query);
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            DataTable dt = new DataTable("dataTbl");
            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        //public DataTable ToDataTable(IEnumerable query)
        //{
        //    DataTable dt = new DataTable();
        //    foreach (object obj in query)
        //    {
        //        Type t = obj.GetType();
        //        PropertyInfo[] pis = t.GetProperties();
        //        if (dt.Columns.Count == 0)
        //        {
        //            foreach (PropertyInfo pi in pis)
        //            {
        //                dt.Columns.Add(pi.Name);
        //            }
        //        }
        //        DataRow dr = dt.NewRow();
        //        foreach (PropertyInfo pi in pis)
        //        {
        //            object value = pi.GetValue(obj, null);
        //            dr[pi.Name] = value;
        //        }
        //        dt.Rows.Add(dr);
        //    }
        //    return dt;
        //}
        #endregion
    }

    public class ColumnProp
    {
        public string DataField { get; set; }
        public string DisplayField { get; set; }
        public string FieldWidth { get; set; }
        public string FieldFormat { get; set; }
    }

    public class ParamInfo
    {
        public string FieldKey { get; set; }
        public string FieldCaption { get; set; }
        public string FieldType { get; set; }
        public string FieldDisplay { get; set; }
        public string FieldValue {get; set; }
        public string FieldDefault { get; set; }
        public string FieldSequence { get; set; }
        public string FieldDateTime { get; set; }
        public string FieldCboValueSQL { get; set; }
        public string FiledAttribute { get; set; }    
        public string ItemCondition { get; set; }
        public string RelationField { get; set; }
    }
}
