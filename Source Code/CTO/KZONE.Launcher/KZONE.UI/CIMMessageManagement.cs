using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KZONE.Entity;
using KZONE.EntityManager;
using KZONE.Log;
using System.Reflection;
using KZONE.Work;

namespace KZONE.UI
{
    public partial class CIMMessageManagement : Form
    {
        public CIMMessageManagement(OPIInfo opiInfo )
        {
            InitializeComponent();     
            oPIInfo = opiInfo;
            this.TopMost = true;
            this.Visible = true;
            ab = AbstractMethod.CreateInstance();
        }


        AbstractMethod ab = new AbstractMethod();
        private OPIInfo oPIInfo { get; set; }

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");
        //        if (eq.File.SetCimMesg.Count > 0)
        //        {
        //            dataGridView1.Rows.Clear();
        //            int i = 1;
        //            foreach (CIMMESSAGEHISTORY mesg in eq.File.SetCimMesg)
        //            {
        //                dataGridView1.Rows.Add(i,mesg.MESSAGEID,mesg.UPDATETIME,mesg.MESSAGETEXT,"Comfirm");
                        
        //                i++;
        //            }

        //            this.TopMost = true;
        //            this.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
               
        //    }
        //}

        public void GetCimmessagehistorys()
        {
            try
            {
                var Q_BCMessage = (from msg  in oPIInfo.HisDBCtx.SBCS_CIMMESSAGEHISTORY
                                   orderby msg.UPDATETIME descending
                                   select msg).Take(20).ToList();

                if (Q_BCMessage.Count == 0 || Q_BCMessage == null)
                {
                    dataGridView2.DataSource = null;
                    return;
                }
                else
                {  dataGridView2.Rows.Clear();
                    int i = 1;
                    foreach (SBCS_CIMMESSAGEHISTORY mesg in Q_BCMessage)
                    {
                        dataGridView2.Rows.Add(i, mesg.UPDATETIME, mesg.MESSAGEID, mesg.MESSAGETEXT);
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
                int i = 1;
                foreach (CIMMESSAGEHISTORY mesg in eq.File.SetCimMesg)
                {
                    dataGridView1.Rows.Add(i, mesg.MESSAGEID, mesg.UPDATETIME, mesg.MESSAGETEXT, "Comfirm");

                    i++;
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null && e.ColumnIndex>0)
                {
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "ColConfirm")
                    {
                        string id = dataGridView1.CurrentRow.Cells[ColID.Name].Value.ToString();
                        ObjectManager.EquipmentManager.UpdateCIMMessage(id, "clear");

                        
                        ab.Invoke(eServiceName.EquipmentService,"CPCCIMMessageConfirmReport", new object[] { id,eBitResult.ON });
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);

            }
        }

        private void CIMMessageManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDIForm.FrmCIMMesg = null;
           // this.Close();
            this.Dispose();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TabControl _tab = (TabControl)sender;
                
                if (_tab.SelectedIndex == 1)
                {
                     GetCimmessagehistorys(); 

                    if (this.dataGridView2.Rows.Count > 0)
                    {
                        this.dataGridView2.ClearSelection();

                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
              
            }
        }
    }
}
