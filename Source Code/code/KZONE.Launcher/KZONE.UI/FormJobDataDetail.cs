using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using KZONE.Log;
using KZONE.Entity;
using KZONE.EntityManager;
using System.Linq;
using System.ComponentModel;

namespace KZONE.UI
{
    public partial class FormJobDataDetail : FormBase
    {
        string GlassID = string.Empty;
        string CSTSeqNo = string.Empty;
        string CstSlotNo = string.Empty;
        private GlassData data = null;
        //  private bool  formFlag = true;

        public FormJobDataDetail(string glassID, string cstSeqNo, string cstSlotNo)
        {
            InitializeComponent();

            GlassID = glassID;
            CSTSeqNo = cstSeqNo;
            CstSlotNo = cstSlotNo;
        }

        public FormJobDataDetail(Interface interFace, string key, bool flag)
        {
            //  formFlag = false;

            if (flag == true)
            {
                data = interFace.UpstreamJobData[key];

            }
            else
            {
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
                    //Common
                    this.txtCassetteSeqenceNo.Text = job.Cassette_Sequence_No.ToString();
                    this.txtJobSlotNo.Text = job.Job_Sequence_No.ToString();
                    this.txtLotID.Text = job.Lot_ID;
                    this.txtProductID.Text = job.Product_ID;
                    this.txtOperationID.Text = job.Operation_ID;
                    this.txtGlassID.Text = job.GlassID_or_PanelID;
                    this.txtCSTOperationMode.Text = job.CST_Operation_Mode;
                    this.txtSubstrateType.Text = job.Substrate_Type;
                    this.txtProductType.Text = job.Product_Type;
                    this.txtJobType.Text = job.Job_Type;
                    this.txtDummyType.Text = job.Dummy_Type;
                    this.txtSkipFlag.Text = job.Skip_Flag;
                    this.txtProcessFlag.Text = job.Process_Flag;
                    this.txtProcessReasonCode.Text = job.Process_Reason_Code;
                    this.txtLOTCode.Text = job.LOT_Code.ToString();
                    this.txtGlassThickness.Text = job.Glass_Thickness;
                    this.txtGlassDegree.Text = job.Glass_Degree;
                    this.txtInspectionFlag.Text = job.Inspection_Flag;
                    this.txtJobJudge.Text = job.Job_Judge;
                    this.txtJobGrade.Text = job.Job_Grade;
                    this.txtJobRecoveryFlag.Text = job.Job_Recovery_Flag;
                    this.txtMode.Text = job.Mode;
                    this.txtStepID.Text = job.Step_ID;
                    this.txtVCRReadID.Text = "";//job.VCR_Read_ID;
                    this.txtMasterRecipeID.Text = job.Master_Recipe_ID;
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                    string lineID = eq.Data.LINEID;
                    KZONE.Entity.Line line = ObjectManager.LineManager.GetLine(lineID);
                    if (line.Data.FABTYPE.ToUpper().Trim() == "ARRAY")//Array Special
                    {
                        this.txtPPID.Text = job.PPID;

                        Type type = typeof(Job);
                        List<PropertyInfo> listPropertyInfos = new List<PropertyInfo>();
                        var properties = type.GetProperties();
                        foreach (PropertyInfo item in properties)
                        {
                            string strAttribute = item.GetCustomAttributesData().FirstOrDefault()?.ConstructorArguments.FirstOrDefault().Value.ToString();
                            if (!string.IsNullOrEmpty(strAttribute) && strAttribute.Contains("Array"))
                            {
                                if (item.Name == "PPID")
                                {
                                    continue;
                                }
                                listPropertyInfos.Add(item);
                            }
                        }

                        for (int i = 0; i < listPropertyInfos.Count; i++)
                        {
                            string name = listPropertyInfos[i].Name;
                            bool flag1 = true;
                            bool flag2 = true;
                            foreach (Control controls in this.Controls[0].Controls[1].Controls[0].Controls[1].Controls[0].Controls)
                            {
                                if ((flag1 || flag2) == false)
                                {
                                    break;
                                }
                                if (controls.Controls.ContainsKey($"lblSpecialData{i + 1}"))
                                {
                                    controls.Controls[$"lblSpecialData{i + 1}"].Text = name;
                                    flag1 = false;
                                }

                                if (controls.Controls.ContainsKey($"txtSpecialData{i + 1}"))
                                {
                                    controls.Controls[$"txtSpecialData{i + 1}"].Text = listPropertyInfos[i].GetValue(job, null)?.ToString();
                                    flag2 = false;
                                }

                            }
                        }

                    }
                    else if (line.Data.FABTYPE.ToUpper().Trim() == "CF")//CF Special
                    {
                        this.txtPPID.Text = job.PPID;
                        //string eqType = line.Data.ATTRIBUTE.Trim();
                        //if (eqType == "CLN")
                        //{

                        //}
                        //else if (eqType == "DEV")
                        //{

                        //}
                        Type type = typeof(Job);
                        List<PropertyInfo> listPropertyInfos = new List<PropertyInfo>();
                        var properties = type.GetProperties();
                        foreach (PropertyInfo item in properties)
                        {
                            string strAttribute = item.GetCustomAttributesData().FirstOrDefault()?.ConstructorArguments.FirstOrDefault().Value.ToString();
                            if (!string.IsNullOrEmpty(strAttribute) && (strAttribute.Contains("CF")))
                            {
                                if (item.Name == "PPID")
                                {
                                    continue;
                                }
                                listPropertyInfos.Add(item);
                            }
                        }

                        for (int i = 0; i < listPropertyInfos.Count; i++)
                        {
                            string name = listPropertyInfos[i].Name;
                            bool flag1 = true;
                            bool flag2 = true;
                            foreach (Control controls in this.Controls[0].Controls[1].Controls[0].Controls[1].Controls[0].Controls)
                            {
                                if ((flag1 || flag2) == false)
                                {
                                    break;
                                }
                                if (controls.Controls.ContainsKey($"lblSpecialData{i + 1}"))
                                {
                                    controls.Controls[$"lblSpecialData{i + 1}"].Text = name;
                                    flag1 = false;
                                }

                                if (controls.Controls.ContainsKey($"txtSpecialData{i + 1}"))
                                {
                                    controls.Controls[$"txtSpecialData{i + 1}"].Text = listPropertyInfos[i].GetValue(job, null)?.ToString();
                                    flag2 = false;
                                }

                            }
                        }
                    }
                    else if (line.Data.FABTYPE.ToUpper().Trim() == "CELL")//Cell Special
                    {
                        this.txtPPID.Text = job.PPID03;
                        Type type = typeof(Job);
                        List<PropertyInfo> listPropertyInfos = new List<PropertyInfo>();
                        var properties = type.GetProperties();
                        foreach (PropertyInfo item in properties)
                        {
                            string strAttribute = item.GetCustomAttributesData().FirstOrDefault()?.ConstructorArguments.FirstOrDefault().Value.ToString();
                            if (!string.IsNullOrEmpty(strAttribute) && strAttribute.Contains("Cell"))
                            {
                                if (item.Name.Contains("PPID"))
                                {
                                    continue;
                                }
                                listPropertyInfos.Add(item);
                            }
                        }

                        for (int i = 0; i < listPropertyInfos.Count; i++)
                        {
                            string name = listPropertyInfos[i].Name;
                            bool flag1 = true;
                            bool flag2 = true;
                            foreach (Control controls in this.Controls[0].Controls[1].Controls[0].Controls[1].Controls[0].Controls)
                            {
                                if ((flag1 || flag2) == false)
                                {
                                    break;
                                }
                                if (controls.Controls.ContainsKey($"lblSpecialData{i + 1}"))
                                {
                                    controls.Controls[$"lblSpecialData{i + 1}"].Text = name;
                                    flag1 = false;
                                }

                                if (controls.Controls.ContainsKey($"txtSpecialData{i + 1}"))
                                {
                                    controls.Controls[$"txtSpecialData{i + 1}"].Text = listPropertyInfos[i].GetValue(job, null)?.ToString();
                                    flag2 = false;
                                }

                            }
                        }
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
