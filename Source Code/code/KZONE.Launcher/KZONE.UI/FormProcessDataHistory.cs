using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
//using System.Data;
using System.Data.SqlClient;
using KZONE.Log;
using KZONE.EntityManager;
using NPOI.HSSF.Record.Formula.Functions;
using Spring.Validation;
using System.IO;
using KZONE.Entity;
using NPOI.Util;
using static NPOI.HSSF.Util.HSSFColor;
using OfficeOpenXml;
using System.Threading;
using System.Threading.Tasks;
using NPOI.HSSF.Record.Formula;

namespace KZONE.UI
{
    public partial class FormProcessDataHistory : FormBase
    {
        #region Fields
        public const int DEFAULT_COLUMN_WIDTH = 100;
        private HisTableParam _frmParam = null;
        private List<string> VisibleFields = new List<string> { "FILENAMA", "TRXID" };
        private DataTable dt;
        private FileInfo fi;
        #endregion

        string MsgCaption = "Process Data History";

        public FormProcessDataHistory(HisTableParam histParam)
        {
            InitializeComponent();
            if (histParam.TableName == "SBCS_TACTDATAHISTORY")
            {
                this.lblCaption.Text = "Tact Time History";
            }
            else
            {
                this.lblCaption.Text = "Process Data History";
            }
            this._frmParam = histParam;

            this.GenerateConditionUserCtl();
            this.GenerateDataGridViewCol();
            dt = new DataTable();
            fi = new FileInfo("C:\\Users\\MiXi\\Desktop\\");
        }

        private void FormProcessDataHistory_Load(object sender, EventArgs e)
        {
            try
            {
                rdoHour.Checked = true;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        #region Events
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                OPIInfo opiap = OPIInfo.CreateInstance();
                #region 組查詢語法
                string _sql = string.Format("SELECT TOP {0} OBJECTKEY, {1} FROM {2}", opiap.QueryMaxCount, String.Join(",", _frmParam.QueryFields.ToArray()), _frmParam.TableName);
                string _sqlCnt = string.Format("SELECT COUNT(*) FROM {0}", _frmParam.TableName);
                StringBuilder sbSql = new StringBuilder(_sql);
                StringBuilder sbSqlCnt = new StringBuilder(_sqlCnt);
                bool bolFirstCondition = true;

                #region 組日期條件
                string result = string.Format(" WHERE {0} BETWEEN '{1}' AND '{2}'",
                    grbDateTime.Tag.ToString(),
                    string.Format("{0}:00:00", dtpStartDate.Value.ToString("yyyy-MM-dd HH")),
                    string.Format("{0}:59:59", dtpEndDate.Value.ToString("yyyy-MM-dd HH")));

                string resultCnt = string.Format(" WHERE {0} BETWEEN '{1}' AND '{2}'",
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
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                sbSql.Append(string.Format($" AND NODEID='{eq.Data.LINEID}'"));
                sbSql.Append(string.Format(" ORDER BY {0}", _frmParam.SortExpression));
                #endregion

                //DBSETDataContext ctx = FormMainMDI.G_OPIAp.DBCtx; //modify by sy.wu 2016/06/27
                //OPIInfo opiap = new OPIInfo();
                DBHISSETDataContext ctx = opiap.HisDBCtx;

                #region 檢查搜尋筆數不得大於最大值

                //IEnumerable<int> _queryCnt = ctx.ExecuteQuery<int>(sbSqlCnt.ToString());
                //int _resultCnt = _queryCnt.FirstOrDefault();
                //if (_resultCnt > FormMainMDI.G_OPIAp.QueryMaxCount)
                //{
                //    ShowMessage(this, "Query Reuslt", "", string.Format("Result count [{0}] > QueryMaxCount [{1}], Please reset condition ！", _resultCnt.ToString(), FormMainMDI.G_OPIAp.QueryMaxCount.ToString()), MessageBoxIcon.Information);
                //    return ;
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
                    dgvData.PageSize = opiap.QueryPageSixeCount;
                    this.GenerateDataGridViewCol();
                }
                else
                {
                    dtDisplay = dt.DefaultView.ToTable(false, _frmParam.QueryFields.ToArray());
                    dgvData.PageSize = opiap.QueryMaxCount;
                    dgvData.SetPagedDataSource(dtDisplay, bnavRecord);

                }
                gbHistory.Text = string.Format("Total Count: {0}", rowCount.ToString());

                if (dt.Rows.Count == 0)
                    ShowMessage(this, "Query Reuslt", "", "No matching data for your query！", MessageBoxIcon.Information);
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
                    ShowMessage(this, MsgCaption, "", "No Data to Export!", MessageBoxIcon.Information);
                    return;
                }

                string message = string.Empty;
                if (UniTools.ExportToExcel(this.lblCaption.Text, dgvData, true, out message))
                {
                    ShowMessage(this, MsgCaption, "", "Export Success！", MessageBoxIcon.Information);
                }
                else
                {
                    if (!"".Equals(message))
                    {
                        ShowMessage(this, MsgCaption, "", message, MessageBoxIcon.Error);
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

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var senderGrid = (DataGridView)sender;
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    if (dgvData.Rows[e.RowIndex].DataBoundItem != null)
                    {
                        DataRow dr = ((DataRowView)dgvData.Rows[e.RowIndex].DataBoundItem).Row;
                        FormProcessDataHistoryDetail _frm;
                        if (_frmParam.TableName == "SBCS_TACTDATAHISTORY")
                        {
                            _frm = new FormProcessDataHistoryDetail(true, dr) { TopMost = true };
                        }
                        else
                        {
                            _frm = new FormProcessDataHistoryDetail(dr) { TopMost = true };
                        }
                        _frm.ShowDialog();

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

        private void lblCaption_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string fieldWidth = string.Empty;

                // 取得各欄位寬度並組成逗點隔開的字串
                foreach (DataGridViewColumn col in dgvData.Columns)
                {
                    if (col is DataGridViewButtonColumn)
                        continue;
                    fieldWidth += string.Format(",{0}", col.Width);
                }

                OPIInfo opiap = OPIInfo.CreateInstance();
                DBSETDataContext ctx = opiap.DBCtx;
                SBRM_HISTABLEFIELDS histField =
                    (from c in ctx.SBRM_HISTABLEFIELDS
                     where c.TABLENAME == _frmParam.TableName
                     select c).Single();
                histField.FIELDWIDTH = fieldWidth.Substring(1);

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Private Methods
        // 動態產生查詢條件所需的控制項
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
                            flpnlCondition.Controls.Add(objCombox);
                            break;
                        case FieldTypes.TextBox:
                            ucTextBox_His objTextBox = new ucTextBox_His(pi);
                            objTextBox.Caption = pi.FieldCaption;
                            flpnlCondition.Controls.Add(objTextBox);
                            break;
                        case FieldTypes.Unknown:
                        default:
                            // TODO:
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

                //20150318 cy:將Detail欄位往前提到第一欄
                col = new DataGridViewButtonColumn();
                col.HeaderText = "Detail";
                ((DataGridViewButtonColumn)col).Text = "Detail";
                ((DataGridViewButtonColumn)col).UseColumnTextForButtonValue = true;
                col.Width = 100;
                col.ReadOnly = true;
                dgvData.Columns.Add(col);

                foreach (ColumnProp cp in lstColumnInfo)
                {
                    col = new DataGridViewTextBoxColumn();
                    col.DataPropertyName = cp.DataField;
                    col.HeaderText = cp.DisplayField;

                    if ("F".Equals(cp.FieldWidth))
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    else
                    {
                        col.Width = (int.TryParse(cp.FieldWidth, out width) == true) ? width : DEFAULT_COLUMN_WIDTH;
                    }
                    col.ReadOnly = true;
                    if (VisibleFields.Contains(cp.DataField)) col.Visible = false;

                    dgvData.Columns.Add(col);
                }

                col = new DataGridViewCheckBoxColumn();
                col.HeaderText = "Export Parameters";
                col.Name = "colExport";
                ((DataGridViewCheckBoxColumn)col).ToolTipText = "Export Parameters";
                col.Width = 150;
                col.ValueType = typeof(bool);
                dgvData.Columns.Add(col);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }


        #endregion



        private void rdoLatest_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton _rdo = (RadioButton)sender;
                //DateTime _
                switch (_rdo.Tag.ToString())
                {
                    case "H":
                        dtpStartDate.Value = DateTime.Now.AddHours(-1);
                        dtpEndDate.Value = DateTime.Now;
                        break;

                    case "D":
                        dtpStartDate.Value = DateTime.Now.AddDays(-1);
                        dtpEndDate.Value = DateTime.Now;
                        break;

                    case "W":
                        dtpStartDate.Value = DateTime.Now.AddDays(-7);
                        dtpEndDate.Value = DateTime.Now;
                        break;

                    case "M":
                        dtpStartDate.Value = DateTime.Now.AddMonths(-1);
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
        }

        private void dtpDateTime_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ucComBox_His _cbo in flpnlCondition.Controls.OfType<ucComBox_His>())
                {
                    if (_cbo.Param.ItemCondition != "DATETIME") continue;

                    if (dtpStartDate.Value > dtpEndDate.Value) continue;

                    string _start = string.Format("{0}:00:00", dtpStartDate.Value.ToString("yyyy-MM-dd HH"));
                    string _end = string.Format("{0}:59:59", dtpEndDate.Value.ToString("yyyy-MM-dd HH"));

                    string _sql = string.Format(_cbo.Param.FieldCboValueSQL.ToString(), _start, _end);

                    //DBSETDataContext _ctx = FormMainMDI.G_OPIAp.DBCtx; //modify by sy.wu 2016/06/27
                    //UniBCS_HisDataContext _ctx = FormMainMDI.G_OPIAp.HisDBCtx;
                    OPIInfo opiap = OPIInfo.CreateInstance();
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

                    string _value = string.Empty;
                    _cbo.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void btnExportParameters_Click(object sender, EventArgs e)
        {

            try
            {
                if (dgvData.Rows.Count <= 0)
                {
                    MessageBox.Show("无Process Data记录，请确认后再次导出!");
                    return;

                }
                dt.Rows.Clear();
                dt.Columns.Clear();
                bool exportFail = false;
                string errorMessage = string.Empty;
                List<string> MessageTips = new List<string>();
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    //判断表中有无数据，是否有勾选
                    bool res = dgvData.Rows[i].Cells["colExport"].Value == null ? false : (bool)dgvData.Rows[i].Cells["colExport"].Value;
                    if (res == true)
                    {
                        string fileName = dgvData.Rows[i].Cells[11].EditedFormattedValue.ToString();
                        string cstNo = dgvData.Rows[i].Cells[7].EditedFormattedValue.ToString();
                        string slotNo = dgvData.Rows[i].Cells[8].EditedFormattedValue.ToString();
                        string glassID = dgvData.Rows[i].Cells[9].EditedFormattedValue.ToString();
                        string time = dgvData.Rows[i].Cells[2].EditedFormattedValue.ToString();
                        string localID = dgvData.Rows[i].Cells[3].EditedFormattedValue.ToString();
                        IList<string> paramter = ObjectManager.ProcessDataManager.ProcessDataValues(fileName);
                        if (paramter == null || paramter.Count == 0)
                        {
                            exportFail = true;
                            errorMessage = string.Empty;
                            if (paramter == null)
                            {
                                string directoryName = fileName.Split('_')[3].Substring(0, 10);
                                errorMessage = $"不存在CstNo={cstNo},SlotNo={slotNo},GlassID={glassID}这张玻璃的Process Data数据!\r\n因为不存在文件{fileName}.txt\n请至以下确认-> D:\\KZONELOG\\{localID}\\ProcessData\\{directoryName}\\";
                                MessageTips.Add(errorMessage);
                            }
                            else if (paramter.Count == 0)
                            {
                                errorMessage = $"CstNo={cstNo},SlotNo={slotNo},GlassID={glassID}这张玻璃的Process Data数据为空!";
                                MessageTips.Add(errorMessage);
                            }
                            continue;
                        }
                        paramter.RemoveAt(paramter.Count - 1);
                        if (dt.Rows.Count == 0)
                        {
                            //添加列名
                            paramter.Insert(0, "GlassID");
                            paramter.Insert(0, "SlotNo");
                            paramter.Insert(0, "CassetteNo");
                            paramter.Insert(0, "CreateTime");
                            for (int j = 0; j < paramter.Count; j++)
                            {
                                string colName = paramter[j].Split('=')[0];
                                //if (!dt.Columns.Contains(colName))
                                //{
                                dt.Columns.Add(colName);
                                //}

                            }

                            //添加第一行数据
                            List<string> values = new List<string>();
                            values.Add(time);
                            values.Add(cstNo);
                            values.Add(slotNo);
                            values.Add(glassID);
                            paramter.RemoveAt(0);
                            paramter.RemoveAt(0);
                            paramter.RemoveAt(0);
                            paramter.RemoveAt(0);
                            for (int j = 0; j < paramter.Count; j++)
                            {
                                if (paramter[j].Split('=').Length > 1)
                                    values.Add(paramter[j].Split('=')[1]);
                            }
                            dt.Rows.Add(values.ToArray());
                        }
                        else
                        {
                            List<string> values = new List<string>();
                            values.Add(time);
                            values.Add(cstNo);
                            values.Add(slotNo);
                            values.Add(glassID);
                            //paramter.RemoveAt(0);
                            //paramter.RemoveAt(0);
                            //paramter.RemoveAt(0);
                            //paramter.RemoveAt(0);
                            for (int j = 0; j < paramter.Count; j++)
                            {
                                if (paramter[j].Split('=').Length > 1)
                                    values.Add(paramter[j].Split('=')[1]);
                            }
                            dt.Rows.Add(values.ToArray());
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {

                    //要导出的csv文件的存放位置
                    string fileName = "";
                    string saveFileName = "";
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.FileName = fileName;
                    saveDialog.DefaultExt = "csv";
                    saveDialog.Filter = ".xls|*.xls|.xlsx|*.xlsx|csv文件|*.csv";
                    saveDialog.ShowDialog();
                    saveFileName = saveDialog.FileName;
                    if (saveFileName.IndexOf(":") < 0) return; //被点了取消

                    saveDialog.Title = "Save a File";

                    fi = new FileInfo(saveFileName);
                    if (fi.Directory != null && !fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }
                    else if (fi.Directory.Exists)
                    {
                        File.Delete(saveFileName);
                    }
                    //使用文件流
                    //FileStream fs = new FileStream(saveFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                    //StringBuilder data = new StringBuilder();

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    if (i == 0)
                    //    {
                    //        //添加列名               
                    //        for (int j = 0; j < dt.Columns.Count; j++)
                    //        {
                    //            data.Append(dt.Columns[j].ToString());
                    //            data.Append(",");
                    //        }
                    //        sw.WriteLine(data);
                    //    }
                    //    //添加数据

                    //    data = new StringBuilder();
                    //    for (int j = 0; j < dt.Columns.Count; j++)
                    //    {
                    //        data.Append(dt.Rows[i][j].ToString());
                    //        data.Append(",");
                    //    }
                    //    sw.WriteLine(data);

                    //}



                    //sw.Close();
                    //fs.Close();


                    //使用Epplus             
                    Thread _sendThread = new Thread(new ThreadStart(ExportByEpplus)) { IsBackground = true };
                    _sendThread.Start();




                    string errMsg = string.Empty;
                    if (exportFail)
                    {
                        errMsg += "\r\n";
                        errMsg += "部分数据存在异常:\r\n";
                        for (int i = 0; i < MessageTips.Count; i++)
                        {
                            errMsg += MessageTips[i] + "\r\n";
                        }
                    }

                    MessageBox.Show($"导出成功!{errMsg}");

                }
                else
                {
                    if (!exportFail)
                    {
                        MessageBox.Show("未勾选需要导出的数据，请检查后再次操作!");
                    }
                    else
                    {
                        string errMsg = string.Empty;
                        errMsg += "\r\n";
                        errMsg += "部分数据存在异常:\r\n";
                        for (int i = 0; i < MessageTips.Count; i++)
                        {
                            errMsg += MessageTips[i] + "\r\n";
                        }
                        MessageBox.Show(errMsg);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ExportByEpplus()
        {
            using (ExcelPackage package = new ExcelPackage(fi))
            {
                ExcelWorksheet worksheet;
                if (package.Workbook.Worksheets.Count == 0)
                {
                    worksheet = package.Workbook.Worksheets.Add($"{DateTime.Now.ToString().Substring(0, 8)}");
                    worksheet.Cells.Style.ShrinkToFit = true;
                }
                else
                {
                    worksheet = package.Workbook.Worksheets[0];
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        //添加列名               
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            worksheet.Cells[1, j + 1].Value = dt.Columns[j];
                        }
                    }

                    //添加数据
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (i == 0)
                        {
                            worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j].ToString();
                        }
                        else
                        {
                            if (j == 0)
                            {
                                worksheet.Cells[worksheet.Dimension.End.Row + 1, j + 1].Value = dt.Rows[i][j].ToString();
                            }
                            else
                            {
                                worksheet.Cells[worksheet.Dimension.Rows, j + 1].Value = dt.Rows[i][j].ToString();
                            }
                        }
                        worksheet.Column(j + 1).AutoFit();
                    }

                }

                package.Save();
            }
        }
        private void cmsSelectCancel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                dgvData.Rows[i].Cells["colExport"].Value = false;
            }
        }

        private void cmsSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                dgvData.Rows[i].Cells["colExport"].Value = true;
            }
        }
    }
}
