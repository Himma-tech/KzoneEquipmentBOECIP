using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using KZONE.Log;

namespace KZONE.UI
{
    public partial class FormVCRResult : FormBase
    {
       private HisTableParam _frmParam = null;
        string _grbDateTimeTag = string.Empty;
        public DateTime dtpStartDateValue{get;set;}
        public DateTime dtpEndDateValues { get; set; }
        public FlowLayoutPanel flpnlCondition;

        public FormVCRResult()
        {
            InitializeComponent();
            lblCaption.Text = "DCR Result";
        }

        public FormVCRResult(string grbDateTimeTag, HisTableParam frmParam, FlowLayoutPanel cl)
        {

            InitializeComponent();
            lblCaption.Text = "DCR Result";
            flpnlCondition = cl;
            _grbDateTimeTag = grbDateTimeTag;
            _frmParam = frmParam;
        }

        private void FormVCRResult_Load(object sender, EventArgs e)
        {
            try
            {
                #region 組查詢語法
                string _sqlCnt = string.Format("SELECT COUNT(*) FROM {0}", _frmParam.TableName);
                StringBuilder sbSqlCnt = new StringBuilder(_sqlCnt);
                StringBuilder sbSqlCount = new StringBuilder(_sqlCnt);
                bool bolFirstCondition = true;

                #region 組日期條件
                string resultCnt = string.Format(" WHERE {0} BETWEEN '{1}' AND '{2}'",
                    _grbDateTimeTag,
                    string.Format("{0}:00:00", dtpStartDateValue.ToString("yyyy-MM-dd HH")),
                    string.Format("{0}:59:59", dtpEndDateValues.ToString("yyyy-MM-dd HH")));
                sbSqlCnt.Append(resultCnt);
                sbSqlCount.Append(resultCnt);
                bolFirstCondition = false;
                #endregion

                foreach (iHistBase uc in flpnlCondition.Controls)
                {
                    sbSqlCnt.Append(uc.GetCondition(ref bolFirstCondition));
                }
                #endregion
                OPIInfo opiap = OPIInfo.CreateInstance();
                DBHISSETDataContext ctx = opiap.HisDBCtx;
                SetDataGridView(ctx, sbSqlCnt, sbSqlCount);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetData(DBHISSETDataContext ctx, StringBuilder sbSqlCnt, string name, string ShowName, int _resultCnt, int _okRetry, int length)
        {
            try
            {
                try
                {
                    if (sbSqlCnt.ToString().Contains(name))
                    {
                        IEnumerable<int> _queryCntOKRetry = ctx.ExecuteQuery<int>(sbSqlCnt.ToString());
                        _okRetry = _queryCntOKRetry.FirstOrDefault();
                        dgvDCRResult.Rows.Add(ShowName, _okRetry.ToString(), (Convert.ToDouble(_okRetry) / Convert.ToDouble(_resultCnt)).ToString("0.00%"));
                    }
                    else
                    {
                        if (sbSqlCnt.ToString().Length == length)
                        {
                            sbSqlCnt = sbSqlCnt.Append(string.Format(name));
                            IEnumerable<int> _queryCntOKRetry = ctx.ExecuteQuery<int>(sbSqlCnt.ToString());
                            _okRetry = _queryCntOKRetry.FirstOrDefault();
                            dgvDCRResult.Rows.Add(ShowName, _okRetry.ToString(), (Convert.ToDouble(_okRetry) / Convert.ToDouble(_resultCnt)).ToString("0.00%"));
                            sbSqlCnt.Remove(length, name.Length);
                        }
                        else
                        {
                            dgvDCRResult.Rows.Add(ShowName, "0", (Convert.ToDouble(0) / Convert.ToDouble(_resultCnt)).ToString("0.00%"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void SetDataGridView(DBHISSETDataContext ctx, StringBuilder sbSqlCnt, StringBuilder sbSqlCount)
        {
            try
            {
                OPIInfo opiap = OPIInfo.CreateInstance();
                SBRM_OPI_PARAMETER _data = (from q in opiap.DBBRMCtx.SBRM_OPI_PARAMETER
                                            where q.SUBKEY.Equals("DCRResult") && q.LINETYPE.Equals(opiap.APName)
                                            select q).FirstOrDefault();
                int sqlLength = sbSqlCnt.ToString().Length;
                dgvDCRResult.Rows.Clear();
                dgvDCRResult.ReadOnly = true;
                int _resultCnt = 0;
                int _okRetry = 0;
                IEnumerable<int> _queryCnt = ctx.ExecuteQuery<int>(sbSqlCnt.ToString());
                _resultCnt = _queryCnt.FirstOrDefault();
                if (_resultCnt.Equals(0))
                {
                    ShowMessage(this, "Query Reuslt", "", "No matching data for your query！", MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
                dgvDCRResult.Rows.Add("Count", _resultCnt.ToString(), "");
                Dictionary<int, string> _dcr = new Dictionary<int, string>();
                string[] _dcrName = _data.ITEMVALUE.ToString().Split(';');
                for (int i = 0; i < _dcrName.Length; i++)
                {
                    _dcr.Add(i, _dcrName[i]);
                }
                for (int i = 0; i < _dcr.Count; i++)
                {
                    string[] _sqlName = _dcr[i].Split(':');
                    SetData(ctx, sbSqlCnt, _sqlName[1], _sqlName[0], _resultCnt, _okRetry, sqlLength);
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
