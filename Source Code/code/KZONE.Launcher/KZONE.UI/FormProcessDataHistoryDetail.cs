using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using KZONE.EntityManager;
using KZONE.Log;
using System.Collections.Generic;
//using UniAuto.UniBCS.HKC.OpiSpec;


namespace KZONE.UI
{
    public partial class FormProcessDataHistoryDetail : FormBase
    {

        DataRow CurDataRow = null;

        bool tack = false;

        public FormProcessDataHistoryDetail(DataRow _row)
        {
            InitializeComponent();
            lblCaption.Text = "ProcessDataHistory Detail";

            CurDataRow = _row;
        }
        public FormProcessDataHistoryDetail(bool tact,DataRow _row)
        {
            InitializeComponent();
            lblCaption.Text = "Tact Time History Detail";
            tack = tact;
            CurDataRow = _row;
        }

        private void FormProcessDataHistoryDetail_Load(object sender, EventArgs e)
        {
            try
            {
                string _err = string.Empty;
                string[] splitc;

                txtJobSeqNo.Text = CurDataRow["CASSETTESLOTNO"].ToString(); ;
                txtCstSeqNo.Text = CurDataRow["CASSETTESEQNO"].ToString();
                txtJobID.Text = CurDataRow["JOBID"].ToString();
              
                IList<string> paramter = null;
                if (tack)
                {
                    txtNodeNo.Text = "L3";
                   paramter = ObjectManager.ProcessDataManager.TactDataValues(CurDataRow["FILENAMA"].ToString());
                }
                else
                {
                    txtNodeNo.Text = CurDataRow["NODEID"].ToString();

                    paramter = ObjectManager.ProcessDataManager.ProcessDataValues(CurDataRow["FILENAMA"].ToString());
                }
                if (paramter != null && paramter.Count != 0)
                {
                    DataTable dt = UniTools.InitDt(new string[] { "NAME", "VALUE" });
                    foreach (var data in paramter)
                    {
                        if (!string.IsNullOrEmpty(data) && data.Contains("="))
                        {
                            splitc = data.Trim().Split(new string[] { "=" }, StringSplitOptions.None);

                            DataRow drNew = dt.NewRow();
                            drNew["NAME"] = splitc[0].Trim();
                            drNew["VALUE"] = splitc[1].Trim();
                            dt.Rows.Add(drNew);

                        }

                    }

                    dgvData.DataSource = dt;
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
