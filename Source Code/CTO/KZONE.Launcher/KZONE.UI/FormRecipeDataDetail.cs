using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using KZONE.EntityManager;
using KZONE.Log;
using System.Collections.Generic;



namespace KZONE.UI
{
    public partial class FormRecipeDataDetail : FormBase
    {

        DataGridViewRow CurDataRow = null;
        bool His = false;

        public FormRecipeDataDetail(DataGridViewRow _row)
        {
            InitializeComponent();
            lblCaption.Text = "Recipe Data Detail";

            CurDataRow = _row;
        }
        public FormRecipeDataDetail(DataGridViewRow _row,bool his)
        {
            InitializeComponent();
            lblCaption.Text = "Recipe Data History Detail";

            CurDataRow = _row;
            His = his;
        }


        private void FormProcessDataHistoryDetail_Load(object sender, EventArgs e)
        {
            try
            {
                string _err = string.Empty;
                string[] splitc;
                txtRecipeNo.Text = CurDataRow.Cells["RECIPENO"].Value.ToString();
                txtRecipeID.Text = CurDataRow.Cells["RECIPEID"].Value.ToString();
                txtCreateTime.Text = CurDataRow.Cells["CREATETIME"].Value.ToString();
                txtRecipeStatus.Text = CurDataRow.Cells["RECIPESTATUS"].Value.ToString(); 
                txtVersionNo.Text = CurDataRow.Cells["VERSIONNO"].Value.ToString();
                txtFileName.Text = CurDataRow.Cells["FILENAME"].Value.ToString();

                IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(His, CurDataRow.Cells["FILENAME"].Value.ToString());
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
