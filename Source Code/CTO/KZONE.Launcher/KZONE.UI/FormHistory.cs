using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using KZONE.Entity;
using KZONE.EntityManager;
using KZONE.Log;

namespace KZONE.UI
{
    public partial class FormHistory : FormBase
    {
        #region Fields
        public const int DEFAULT_COLUMN_WIDTH = 100;
        private HisTableParam _frmParam;
        #endregion
        bool RadioFlag = false;

        public FormHistory()
        {
            this.InitializeComponent();
        }


        public void Init()
        {
            try
            {

                HisTableParam histParam = new HisTableParam("SBCS_JOBHISTORY");

                this._frmParam = histParam;

                this.GenerateConditionUserCtl();
                this.GenerateDataGridViewCol();
                if (_frmParam.TableName == "SBCS_DCRRESULTHISTORY")
                    btnVCRResule.Visible = true;
                else btnVCRResule.Visible = false;

                if (_frmParam.TableName == "SBCS_JOBHISTORY")
                {
                    // dgvData.CellDoubleClick -= new DataGridViewCellEventHandler(dgvData_CellDoubleClick_JobDataHis);
                    //  dgvData.CellDoubleClick += new DataGridViewCellEventHandler(dgvData_CellDoubleClick_JobDataHis);
                }


                // FormHistory  frmJobHistory = new FormHistory(new HisTableParam("SBCS_JOBHISTORY"));

            }
            catch (Exception exception)
            {
                NLogManager.Logger.LogErrorWrite("", "FormHistory", "Init", "FormHistory Setup Fail", exception);
            }
        }

        public FormHistory(HisTableParam histParam)
        {
            InitializeComponent();

            this._frmParam = histParam;

            this.GenerateConditionUserCtl();
            this.GenerateDataGridViewCol();
            if (_frmParam.TableName == "SBCS_DCRRESULTHISTORY")
                btnVCRResule.Visible = true;
            else btnVCRResule.Visible = false;

            if (_frmParam.TableName == "SBCS_JOBHISTORY")
            {
                // dgvData.CellDoubleClick -= new DataGridViewCellEventHandler(dgvData_CellDoubleClick_JobDataHis);
                // dgvData.CellDoubleClick += new DataGridViewCellEventHandler(dgvData_CellDoubleClick_JobDataHis);
            }
            if (_frmParam.TableName == "SBRM_RECIPE")
            {
                //this.Controls.Remove(grbLatest);
                //this.Controls.Remove(grbDateTime);
                //grbLatest.Visible = false;
                //grbDateTime.Visible = false;
            }
        }

        private void FormHistory_Load(object sender, EventArgs e)
        {
            try
            {
                rdoHour.Checked = true;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                //ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                OPIInfo opiap = OPIInfo.CreateInstance();
                #region 組查詢語法
                string _sql = string.Format("SELECT TOP {0} OBJECTKEY, {1} FROM {2}", opiap.QueryMaxCount, String.Join(",", this._frmParam.QueryFields.ToArray()), this._frmParam.TableName);
                string _sqlCnt = string.Format("SELECT COUNT(*) FROM {0}", this._frmParam.TableName);
                StringBuilder sbSql = new StringBuilder(_sql);
                StringBuilder sbSqlCnt = new StringBuilder(_sqlCnt);
                bool bolFirstCondition = true;

                if (_frmParam.TableName == "SBRM_RECIPE")
                {
                    grbDateTime.Tag = "CREATETIME";
                }
                if (_frmParam.TableName == "SBCS_TANKHISTORY")
                {
                    grbDateTime.Tag = "STARTTIME";
                }


                #region 組日期條件
                string result = string.Format(" WHERE ({0} BETWEEN '{1}' AND '{2}')",
                        grbDateTime.Tag.ToString(),
                        string.Format("{0}:00:00", dtpStartDate.Value.ToString("yyyy-MM-dd HH")),
                        string.Format("{0}:59:59", dtpEndDate.Value.ToString("yyyy-MM-dd HH")));

                string resultCnt = string.Format(" WHERE ({0} BETWEEN '{1}' AND '{2}')",
                    grbDateTime.Tag.ToString(),
                    string.Format("{0}:00:00", dtpStartDate.Value.ToString("yyyy-MM-dd HH")),
                    string.Format("{0}:59:59", dtpEndDate.Value.ToString("yyyy-MM-dd HH")));

                sbSql.Append(result);
                sbSqlCnt.Append(resultCnt);

                bolFirstCondition = false;
                #endregion

                foreach (iHistBase uc in flpnlCondition.Controls)
                {
                    sbSql.Append(uc.GetCondition(ref bolFirstCondition));
                    sbSqlCnt.Append(uc.GetCondition(ref bolFirstCondition));
                }
                //2022.05.25 新增 By MiXi
                if (_frmParam.TableName == "SBRM_RECIPE")
                {
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                    if (eq.Data.LINEID == "KWF22003L" || eq.Data.LINEID == "KWF22004L" || eq.Data.LINEID == "KWF22005R")
                    {
                        sbSql.Append(string.Format(" AND LINEID='{0}'", eq.Data.LINEID));
                    }
                }

                sbSql.Append(string.Format(" ORDER BY {0}", _frmParam.SortExpression));

                #endregion

                //DBSETDataContext ctx = FormMainMDI.G_OPIAp.DBCtx; //modify by sy.wu 2016/06/27
                // UniBCS_HisDataContext ctx = FormMainMDI.G_OPIAp.HisDBCtx;

                // OPIInfo opiap = new OPIInfo();
                System.Data.Linq.DataContext ctx = null;

                if (this._frmParam.TableName == "SBRM_RECIPE")
                {
                    opiap.DBBRMCtx.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, opiap.DBBRMCtx.SBRM_RECIPE);
                    ctx = opiap.DBBRMCtx;
                }
                else
                {
                    ctx = opiap.HisDBCtx;
                }



                #region 檢查搜尋筆數不得大於最大值
                //IEnumerable<int> _queryCnt = ctx.ExecuteQuery<int>(sbSqlCnt.ToString());

                //int _resultCnt = _queryCnt.FirstOrDefault();

                //if (_resultCnt > FormMainMDI.G_OPIAp.QueryMaxCount)
                //{
                //    ShowMessage(this, "Query Reuslt", "", string.Format("Result count [{0}] > QueryMaxCount [{1}], Please reset condition ！", _resultCnt.ToString(), FormMainMDI.G_OPIAp.QueryMaxCount.ToString()), MessageBoxIcon.Information);
                //    return;
                //}
                #endregion

                string projectName = Assembly.GetExecutingAssembly().GetName().Name;
                var query = ctx.ExecuteQuery(Type.GetType(string.Format("{0}.{1}", projectName, _frmParam.TableName)), sbSql.ToString(), new object[0]);


                DataTable dt = DBConnect.ToDataTable(query);

                int rowCount = dt.Rows.Count;

                // 轉成只留下欲查詢欄位的DataTable
                DataTable dtDisplay = null;
                if (dt.Rows.Count == 0)
                {
                    dgvData.DataSource = null;

                    this.GenerateDataGridViewCol();
                }
                else
                {
                    dtDisplay = dt.DefaultView.ToTable(false, _frmParam.QueryFields.ToArray());
                    dgvData.PageSize = opiap.QueryPageSixeCount;
                    dgvData.SetPagedDataSource(dtDisplay, bnavRecord);
                }

                //ShowRate(dt);
                gbHistory.Text = string.Format("Total Count: {0}", rowCount.ToString());

                if (dt.Rows.Count == 0)
                    ShowMessage(this, "Query Reuslt", "", "No matching data for your query！", MessageBoxIcon.Information);


                //OPI的Job History的Grid畫面能用顏色顯色不同的Evnet訊息內容.
                if (_frmParam.TableName == "SBCS_JOBHISTORY")
                {
                    foreach (DataGridViewRow _row in dgvData.Rows)
                    {
                        _row.DefaultCellStyle.BackColor = FindColorCode(_row.Cells["EVENTNAME"].Value.ToString());
                    }
                }


                if (_frmParam.TableName == "SBCS_RECIPEHISTORY")
                {
                    foreach (DataGridViewRow _row in dgvData.Rows)
                    {
                        _row.DefaultCellStyle.BackColor = RecipeColorCode(_row.Cells["EVENT"].Value.ToString());
                    }
                }

                if (this._frmParam.TableName == "SBRM_RECIPE")
                {
                    dgvData.ReadOnly = false;
                    foreach (DataGridViewRow _row in dgvData.Rows)
                    {
                        _row.ReadOnly = true;
                    }
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        //動態產生查詢條件所需的控制項
        private void GenerateConditionUserCtl()
        {
            try
            {
                List<ParamInfo> lstParamInfo = _frmParam.ConditionParamInfo;
                if (lstParamInfo.Count == 0)
                    return;

                FieldTypes enumFieldType;
                foreach (ParamInfo pi in lstParamInfo)
                {
                    enumFieldType = (Enum.TryParse<FieldTypes>(pi.FieldType, out enumFieldType) == true) ? enumFieldType : FieldTypes.Unknown;
                    switch (enumFieldType)
                    {
                        case FieldTypes.ComboBox:
                            ucComBox_His objCombox = new ucComBox_His(pi);
                            objCombox.Caption = pi.FieldCaption;

                            if (pi.RelationField.ToUpper() == "Y")
                            {
                                if (pi.FieldKey == "NODEID")
                                {
                                    objCombox.cmbItem.SelectedValueChanged += new EventHandler(CboNode_SelectedValueChanged);
                                    objCombox.cmbItem.Tag = objCombox;

                                    objCombox.chkUse.CheckedChanged += new EventHandler(chkNodeUse_CheckedChanged);
                                    objCombox.chkUse.Tag = objCombox;
                                }
                            }

                            flpnlCondition.Controls.Add(objCombox);
                            break;

                        case FieldTypes.TextBox:
                            ucTextBox_His objTextBox = new ucTextBox_His(pi);
                            objTextBox.Caption = pi.FieldCaption;
                            flpnlCondition.Controls.Add(objTextBox);
                            break;

                        case FieldTypes.Unknown:

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        //動態產生DataGridView的欄位
        private void GenerateDataGridViewCol()
        {
            try
            {
                List<ColumnProp> lstColumnInfo = _frmParam.GridViewColumnInfo;
                grbDateTime.Tag = _frmParam.DateTimeField;

                if (lstColumnInfo.Count == 0)
                    return;

                dgvData.Columns.Clear();
                int width = 0;
                DataGridViewColumn col = null;
                foreach (ColumnProp cp in lstColumnInfo)
                {
                    col = new DataGridViewTextBoxColumn();
                    col.DataPropertyName = cp.DataField;
                    col.HeaderText = cp.DisplayField;
                    col.Name = cp.DataField;

                    if ("F".Equals(cp.FieldWidth))
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    else
                    {
                        col.Width = (int.TryParse(cp.FieldWidth, out width) == true) ? width : DEFAULT_COLUMN_WIDTH;
                    }

                    if (!"".Equals(cp.FieldFormat))
                    {
                        col.DefaultCellStyle.Format = cp.FieldFormat;
                    }

                    dgvData.Columns.Add(col);

                }

                //if (_frmParam.TableName == "SBCS_JOBHISTORY")
                //{

                //    dgvData.CellDoubleClick -= new DataGridViewCellEventHandler(dgvData_CellDoubleClick_JobDataHis);
                //    dgvData.CellDoubleClick += new DataGridViewCellEventHandler(dgvData_CellDoubleClick_JobDataHis);
                //}

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void chkNodeUse_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox _chk = (CheckBox)sender;

                if (_chk.Checked == false) return;

                ucComBox_His objCombox = (ucComBox_His)_chk.Tag;

                if (objCombox.cmbItem.SelectedValue == null) return;

                string _nodeID = objCombox.cmbItem.SelectedValue.ToString();

                ReloadComboBoxItem_NodeID(_nodeID);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void CboNode_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox _cbo = (ComboBox)sender;

                if (_cbo.Tag == null) return;

                if (_cbo.SelectedValue == null) return;

                ucComBox_His objCombox = (ucComBox_His)_cbo.Tag;

                if (objCombox.Checked == false) return;

                string _nodeID = _cbo.SelectedValue.ToString().Trim();

                ReloadComboBoxItem_NodeID(_nodeID);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void btnVCRResule_Click(object sender, EventArgs e)
        {
            try
            {
                FormVCRResult _vcrResult = new FormVCRResult(grbDateTime.Tag.ToString(), _frmParam, flpnlCondition);
                _vcrResult.dtpStartDateValue = dtpStartDate.Value;
                _vcrResult.dtpEndDateValues = dtpEndDate.Value;
                _vcrResult.ShowDialog(this);
                _vcrResult.Dispose();
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtExport = dgvData.Source;

                // DataGridView 沒資料不執行匯出動作
                if (dtExport.Rows.Count == 0)
                {
                    ShowMessage(this, lblCaption.Text, "", "No Data to Export!", MessageBoxIcon.Information);
                    return;
                }

                string message = string.Empty;
                if (UniTools.ExportToExcel(this.lblCaption.Text, dgvData, true, out message))
                {
                    ShowMessage(this, lblCaption.Text, "", "Export Success！", MessageBoxIcon.Information);
                }
                else
                {
                    if (!"".Equals(message))
                    {
                        ShowMessage(this, lblCaption.Text, "", message, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void rdoLatest_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton _rdo = (RadioButton)sender;

                if (_rdo.Checked == false) return;

                //DateTime _
                switch (_rdo.Tag.ToString())
                {
                    case "H":
                        RadioFlag = false;
                        dtpStartDate.Value = DateTime.Now.AddHours(-1);
                        RadioFlag = true;
                        dtpEndDate.Value = DateTime.Now;
                        break;

                    case "D":
                        RadioFlag = false;
                        dtpStartDate.Value = DateTime.Now.AddDays(-1);
                        RadioFlag = true;
                        dtpEndDate.Value = DateTime.Now;
                        break;

                    case "W":
                        RadioFlag = false;
                        dtpStartDate.Value = DateTime.Now.AddDays(-7);
                        RadioFlag = true;
                        dtpEndDate.Value = DateTime.Now;
                        break;

                    case "M":
                        RadioFlag = false;
                        dtpStartDate.Value = DateTime.Now.AddMonths(-1);
                        RadioFlag = true;
                        dtpEndDate.Value = DateTime.Now;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
            finally
            {
                RadioFlag = true;
            }
        }

        private void dtpDateTime_ValueChanged(object sender, EventArgs e)
        {
            if (RadioFlag)
                ReloadComboBoxItem_DateTime();
        }

        private void ReloadComboBoxItem_NodeID(string NodeId)
        {
            try
            {
                string _display = string.Empty;
                string _value = string.Empty;

                foreach (ucComBox_His _cbo in flpnlCondition.Controls.OfType<ucComBox_His>())
                {
                    if (_cbo.Param.ItemCondition != "NODEID") continue;

                    OPIInfo opiap = OPIInfo.CreateInstance();
                    switch (_cbo.Name)
                    {
                        case "PORTID":
                            foreach (Port _port in opiap.Dic_Port.Values.Where(r => r.NodeID.Equals(NodeId)))
                            {
                                _display = _display + (_display == string.Empty ? "" : ",") + string.Format("{0}-{1}-{2}", _port.NodeNo, _port.PortNo, _port.PortID);
                                _value = _value + (_value == string.Empty ? "" : ",") + _port.PortID;
                            }
                            break;

                        case "UNITID":
                            foreach (Unit _unit in opiap.Dic_Unit.Values.Where(r => r.NodeID.Equals(NodeId)))
                            {
                                _display = _display + (_display == string.Empty ? "" : ",") + string.Format("{0}-{1}-{2}", _unit.NodeNo, _unit.UnitNo, _unit.UnitID);
                                _value = _value + (_value == string.Empty ? "" : ",") + _unit.UnitID;
                            }
                            break;

                        default:
                            break;
                    }

                    _cbo.Param.FieldDisplay = _display;
                    _cbo.Param.FieldValue = _value;

                    _cbo.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        //2015-05-09 REX修改
        private Color FindColorCode(string value)
        {
            try
            {
                int index = 0;
                string[] keyWord = { "SendOut ", "Receive", "Store", "FetchOut", "Remove", "Create", "Delete", "Recovery", "Hold", "OXUpdate", "Delete_CST_Complete", "EQP_NEW", "Edit", "CUT_CREATE_CHIP", "Assembly", "VCR_Report", "VCR_Mismatch", "VCR_Mismatch_Copy" };
                for (; index < keyWord.Length; index++)
                {
                    if (value.IndexOf(keyWord[index]) != -1)
                    {
                        break;
                    }
                }
                switch (index)
                {
                    //DeepSkyBlue
                    case 0:
                        return Color.FromArgb(0, 191, 255);
                    //PaleGreen1
                    case 1:
                        return Color.FromArgb(154, 255, 154);
                    //PaleGoldenrod
                    case 2:
                        return Color.FromArgb(238, 232, 170);
                    //RosyBrown1
                    case 3:
                        return Color.FromArgb(255, 193, 193);
                    //Thistle
                    case 4:
                        return Color.FromArgb(216, 191, 216);
                    //Salmon1
                    case 5:
                        return Color.FromArgb(255, 140, 105);
                    //AntiqueWhite1
                    case 6:
                        return Color.FromArgb(255, 239, 219);
                    //Azure2
                    case 7:
                        return Color.FromArgb(224, 238, 238);
                    //Plum1
                    case 8:
                        return Color.FromArgb(255, 187, 255);
                    //grey71
                    case 9:
                        return Color.FromArgb(181, 181, 181);
                    //LightGreen
                    case 10:
                        return Color.FromArgb(144, 238, 144);
                    //LightCyan3
                    case 11:
                        return Color.FromArgb(180, 205, 205);
                    //Chartreuse1
                    case 12:
                        return Color.FromArgb(127, 255, 0);
                    //DarkSeaGreen3
                    case 13:
                        return Color.FromArgb(155, 205, 155);
                    //CornflowerBlue
                    case 14:
                        return Color.FromArgb(100, 149, 237);
                    //Yellow1
                    case 15:
                        return Color.FromArgb(255, 255, 0);
                    //LtGoldenrodYello
                    case 16:
                        return Color.FromArgb(250, 250, 210);
                    default:
                        return Color.White;
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
                return Color.White;
            }
        }

        private Color RecipeColorCode(string value)
        {

            //Unused = 0,
            //Create = 1,
            //Update = 2,
            //Delete = 3,
            //Request = 4,
            //Regist = 5
            try
            {
                int index = 0;
                string[] keyWord = { "Unused", "Create", "Update", "Delete", "Request", "Regist" };
                for (; index < keyWord.Length; index++)
                {
                    if (value.IndexOf(keyWord[index]) != -1)
                    {
                        break;
                    }
                }
                switch (index)
                {
                    //DeepSkyBlue
                    case 0:
                        return Color.White;
                    //PaleGreen1
                    case 1:
                        return Color.FromArgb(85, 170, 119);
                    //PaleGoldenrod
                    case 2:
                        return Color.FromArgb(57, 152, 216);
                    //RosyBrown1
                    case 3:
                        return Color.FromArgb(255, 0, 0);
                    //Thistle
                    case 4:
                        return Color.FromArgb(239, 33, 239);
                    //Salmon1
                    case 5:
                        return Color.FromArgb(200, 73, 98);

                    default:
                        return Color.White;
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
                return Color.White;
            }
        }

        public void ReloadComboBoxItem_DateTime()
        {
            try
            {
                string _chooseItem = string.Empty;

                foreach (ucComBox_His _cbo in flpnlCondition.Controls.OfType<ucComBox_His>())
                {
                    if (_cbo.Param.ItemCondition != "DATETIME") continue;

                    if (dtpStartDate.Value > dtpEndDate.Value) continue;

                    string _start = string.Format("{0}:00:00", dtpStartDate.Value.ToString("yyyy-MM-dd HH"));
                    string _end = string.Format("{0}:59:59", dtpEndDate.Value.ToString("yyyy-MM-dd HH"));

                    string _sql = string.Format(_cbo.Param.FieldCboValueSQL.ToString(), _start, _end);

                    //DBSETDataContext _ctx = FormMainMDI.G_OPIAp.DBCtx; //modify by sy.wu 2016/06/27
                    //  UniBCS_HisDataContext _ctx = FormMainMDI.G_OPIAp.HisDBCtx;
                    OPIInfo opiap = OPIInfo.CreateInstance();

                    if (this._frmParam.TableName == "SBRM_RECIPE")
                    {
                        DBSETDataContext _ctx = opiap.DBBRMCtx;

                        if (_cbo.Param.FiledAttribute == "INT")
                        {
                            List<int> _lst = _ctx.ExecuteQuery<int>(_sql).ToList();

                            _cbo.Param.FieldDisplay = string.Join(",", _lst.ToArray());
                            _cbo.Param.FieldValue = string.Join(",", _lst.ToArray());
                        }
                        else
                        {
                            List<string> _lst = _ctx.ExecuteQuery<string>(_sql).ToList();

                            _cbo.Param.FieldDisplay = string.Join(",", _lst.ToArray());
                            _cbo.Param.FieldValue = string.Join(",", _lst.ToArray());
                        }

                    }
                    else
                    {
                        DBHISSETDataContext _ctx = opiap.HisDBCtx;

                        if (_cbo.Param.FiledAttribute == "INT")
                        {
                            List<int> _lst = _ctx.ExecuteQuery<int>(_sql).ToList();

                            _cbo.Param.FieldDisplay = string.Join(",", _lst.ToArray());
                            _cbo.Param.FieldValue = string.Join(",", _lst.ToArray());
                        }
                        else
                        {
                            List<string> _lst = _ctx.ExecuteQuery<string>(_sql).ToList();

                            _cbo.Param.FieldDisplay = string.Join(",", _lst.ToArray());
                            _cbo.Param.FieldValue = string.Join(",", _lst.ToArray());
                        }
                    }

                    _chooseItem = _cbo.cmbItem.Text;

                    _cbo.BindDataSource();

                    _cbo.cmbItem.Text = _chooseItem;
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellDoubleClick_JobDataHis(object sender, DataGridViewCellEventArgs e)
        {

        }

        //private void dgvData_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (this._frmParam.TableName == "SBRM_RECIPE")
        //    {
        //        if (e.Button == MouseButtons.Right)
        //        {
        //            if (e.RowIndex >= 0)
        //            {
        //                //若行已是选中状态就不再进行设置
        //                if (dgvData.Rows[e.RowIndex].Selected == false)
        //                {
        //                    dgvData.ClearSelection();
        //                    dgvData.Rows[e.RowIndex].Selected = true;
        //                }

        //                //只选中一行时设置活动单元格
        //                if (dgvData.SelectedRows.Count == 1)
        //                {
        //                    dgvData.CurrentCell = dgvData.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //                }

        //                //dgvData.BackgroundColor = Color.White;
        //                //dgvData.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

        //                //弹出操作菜单
        //                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
        //            }
        //        }
        //    }
        //}

        private void Save_Click(object sender, EventArgs e)
        {

        }

        private void Modify_Click(object sender, EventArgs e)
        {
            //dgvData.Rows[dgvData.CurrentCell.RowIndex].ReadOnly = false;
            //dgvData.BeginEdit(true);   
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            //dgvData.Source.Rows.RemoveAt(dgvData.CurrentCell.RowIndex);
            //dgvData.DataSource = dgvData.Source;
            //dgvData.Show();
        }

        private void New_Click(object sender, EventArgs e)
        {
            //DataGridViewRow row = new DataGridViewRow();

            // row.Cells[0].Value = row.Cells[1].Value = DateTime.Now;

            //((DataTable)dgvData.Source).Rows.Add(DateTime.Now, DateTime.Now,"","","","","","","","","","","");
            //dgvData.DataSource = dgvData.Source;
            //dgvData.Show();
            //dgvData.Rows[dgvData.RowCount-1].ReadOnly = false;
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (_frmParam.TableName == "SBCS_JOBHISTORY")
            {
                foreach (DataGridViewRow _row in dgvData.Rows)
                {
                    _row.DefaultCellStyle.BackColor = FindColorCode(_row.Cells["EVENTNAME"].Value.ToString());
                }
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (_frmParam.TableName == "SBCS_JOBHISTORY")
            {
                foreach (DataGridViewRow _row in dgvData.Rows)
                {
                    _row.DefaultCellStyle.BackColor = FindColorCode(_row.Cells["EVENTNAME"].Value.ToString());
                }
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_frmParam.TableName == "SBRM_RECIPE")
                {
                    if (e.RowIndex < 0) return;

                    if (dgvData.CurrentRow != null)
                    {

                        FormRecipeDataDetail _frm = new FormRecipeDataDetail(dgvData.CurrentRow) { TopMost = true };
                        _frm.ShowDialog();

                        //if (_frm != null) 
                        _frm.Dispose();
                    }
                }
                if (_frmParam.TableName == "SBCS_RECIPEHISTORY")
                {
                    if (e.RowIndex < 0) return;

                    if (dgvData.CurrentRow != null)
                    {

                        FormRecipeDataDetail _frm = new FormRecipeDataDetail(dgvData.CurrentRow, true) { TopMost = true };
                        _frm.ShowDialog();

                        //if (_frm != null) 
                        _frm.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }

        }
    }
}
