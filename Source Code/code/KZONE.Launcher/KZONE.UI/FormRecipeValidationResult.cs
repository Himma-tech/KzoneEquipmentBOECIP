using KZONE.Entity;
using KZONE.Log;
using KZONE.Work;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using KZONE.EntityManager;

namespace KZONE.UI
{
    public partial class FormRecipeValidationResult : Form
    {
        AbstractMethod ab = new AbstractMethod();
        private OPIInfo oPIInfo { get; set; }

        public FormRecipeValidationResult(OPIInfo opiInfo)
        {
            InitializeComponent();
            this.TopMost = true;
            this.Visible = true;
            oPIInfo = opiInfo;
            ab = AbstractMethod.CreateInstance();
            GetRecipeValidationResultHistories();
        }

        public void GetRecipeValidationResultHistories()
        {
            try
            {
                var Q_BCMessage = (from msg in oPIInfo.HisDBCtx.SBCS_RECIPEVALIDATIONRESULTHISTORY
                                   orderby msg.RECEIVETIME descending
                                   select msg).Take(200).ToList();

                if (Q_BCMessage.Count == 0 || Q_BCMessage == null)
                {
                    dataGridView1.DataSource = null;
                    return;
                }
                else
                {
                    dataGridView1.Rows.Clear();
                    int i = 1;
                    foreach (SBCS_RECIPEVALIDATIONRESULTHISTORY mesg in Q_BCMessage)
                    {
                        dataGridView1.Rows.Add(i, mesg.RECEIVETIME, mesg.MASTERRECIPEID, mesg.LOCALRECIPEID, mesg.RMS_RESULT, mesg.RMS_RESULTTEXT);
                        i++;
                    }
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public void UpData(Equipment eq)
        {
            try
            {
                dataGridView1.Rows.Clear();
                //int i = 1;
                //foreach (RECIPEVALIDATIONRESULTHISTORY mesg in eq.File.SetRMSMesg)
                //{
                //    dataGridView1.Rows.Add(i, mesg.RECEIVETIME, mesg.MASTERRECIPEID, mesg.LOCALRECIPEID, mesg.RMS_RESULT, mesg.RMS_RESULTTEXT);

                //    i++;
                //}
                GetRecipeValidationResultHistories();
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);

            }
        }

        private void FormRecipeValidationResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDIForm.FrmRecipeValidationMessage = null;
            Equipment eqp = EntityManager.ObjectManager.EquipmentManager.GetEQP("L3");
            eqp.File.RecipeValidationMessageDisplay = false;
            eqp.File.RecipeValidationMessageUpdate = false;
            EntityManager.ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            this.Dispose();
        }
    }
}
