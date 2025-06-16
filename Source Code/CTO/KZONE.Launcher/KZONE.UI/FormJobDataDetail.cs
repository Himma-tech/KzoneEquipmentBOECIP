using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using KZONE.Log;
using KZONE.Entity;
using KZONE.EntityManager;

namespace KZONE.UI
{
    public partial class FormJobDataDetail : FormBase
    {
        string GlassID = string.Empty;
        string CSTSeqNo = string.Empty;
        string CstSlotNo = string.Empty;
        private GlassData  data = null;
      //  private bool  formFlag = true;

        public FormJobDataDetail(string glassID, string cstSeqNo, string cstSlotNo)
        {
            InitializeComponent();

            GlassID = glassID;
            CSTSeqNo = cstSeqNo;
            CstSlotNo = cstSlotNo;
        }

        public FormJobDataDetail(Interface interFace, string key,bool flag)
        {
          //  formFlag = false;

            if (flag == true)
            {
                data = interFace.UpstreamJobData[key];

            }
            else {
                data = interFace.DownstreamJobData[key];

            }
        
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

        private void FormJobDataDetail_Load(object sender, EventArgs e)
        {
            try
            {
                    Job job = ObjectManager.JobManager.GetJob(GlassID);
                    if (job != null)
                    {
                    this.txtCassetteSeqeenceNo.Text = job.CassetteSequenceNo.ToString();
                    this.txtJobSlotNo.Text = job.JobSequenceNo.ToString();
                    this.txtGlassID.Text = job.JobId;

                    this.txtGroupNumber.Text = job.GroupNumber;
                    this.txtGlassType.Text = job.GlassType;
                    this.txtGlassJudge.Text = job.GlassJudge;
                    this.txtProcessSkipFlag.Text = job.ProcessSkipFlag;
                    this.txtLastGlassFlag.Text = job.LastGlassFlag;
                    this.txtCIMModeCreate.Text = job.CIMModeCreate;
                    this.txtSamplingFlag.Text = job.SamplingFlag;
                    this.txtReserved.Text = job.Reserved;
                    this.txtInspectionJudgeResult.Text = job.InspectionJudgeResult;
                    this.txtInspectionReservationSignal.Text = job.InspectionReservationSignal;
                    this.txtProcessReservationSignal.Text = job.ProcessReservationSignal;
                    this.txtTrackingDataHistory.Text = job.TrackingDataHistory;
                    this.txtEquipmentSpecialFlag.Text = job.EquipmentSpecialFlag;
                    this.txtSorterGrade.Text = job.SorterGrade;
                    this.txtGlassGrade.Text = job.GlassGrade;
                    this.txtFromPortNo.Text = job.FromPortNo;
                    this.txtTargetPortNo.Text = job.TargetPortNo;
                    this.txtTargetSlotNo.Text = job.TargetSlotNo;
                    this.txtTargetCassetteID.Text = job.TargetCassetteID;
                    this.txtReserve.Text = job.Reserve;
                    this.txtPPID.Text = job.PPID;

               
 
                    }
          
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);                
            }
        }
    }
}
