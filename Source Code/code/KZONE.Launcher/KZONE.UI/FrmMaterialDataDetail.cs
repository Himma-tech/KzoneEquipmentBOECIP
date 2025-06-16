using KZONE.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KZONE.UI
{
    public partial class FrmMaterialDataDetail : Form
    {
        DataGridViewRow CurDataRow = null;

        public FrmMaterialDataDetail(DataGridViewRow _row)
        {
            InitializeComponent();
            CurDataRow = _row;
        }

        private void FrmMaterialDataDetail_Load(object sender, EventArgs e)
        {
            try
            {
                string _err = string.Empty;
                txtLineID.Text = CurDataRow.Cells["NODEID"].Value.ToString().Trim();
                txtMaterialID.Text = CurDataRow.Cells["MATERIALID"].Value.ToString().Trim();
                txtFileName.Text = CurDataRow.Cells["FILENAME"].Value.ToString().Trim();

                string path = $"..\\Data\\{txtLineID.Text}\\MaterialManager";

                if (Directory.Exists(path))
                {
                    string filePath = path + $"\\{txtFileName.Text}.xml";
                    if (File.Exists(filePath))
                    {
                        DataTable dt = UniTools.InitDt(new string[] { "NAME", "VALUE" });
                        XDocument _doc = XDocument.Load(filePath);
                        foreach (var item in _doc.Root.Elements())
                        {
                            string name = item.Name.ToString().Trim();
                            string value = string.Empty;
                            if (name.Contains("Time"))
                            {
                                if (item.Elements().Count() > 0)
                                {
                                    foreach (var subItem in item.Elements())
                                    {
                                        if (subItem.Name.ToString().ToUpper().Trim() == "YEAR")
                                        {
                                            value += subItem.Value.Trim().PadLeft(4, '0') + "-";
                                        }
                                        else if (subItem.Name.ToString().ToUpper().Trim() == "MONTH")
                                        {
                                            value += subItem.Value.Trim().PadLeft(2, '0') + "-";
                                        }
                                        else if (subItem.Name.ToString().ToUpper().Trim() == "DAY")
                                        {
                                            value += subItem.Value.Trim().PadLeft(2, '0') + " ";
                                        }
                                        else if (subItem.Name.ToString().ToUpper().Trim() == "HOUR")
                                        {
                                            value += subItem.Value.Trim().PadLeft(2, '0') + ":";
                                        }
                                        else if (subItem.Name.ToString().ToUpper().Trim() == "MINUTE")
                                        {
                                            value += subItem.Value.Trim().PadLeft(2, '0') + ":";
                                        }
                                        else if (subItem.Name.ToString().ToUpper().Trim() == "SECOND")
                                        {
                                            value += subItem.Value.Trim().PadLeft(2, '0');
                                        }
                                    }
                                }
                                else
                                {
                                    value = item.Value.ToString().Trim();
                                }
                            }
                            else
                            {
                                value = item.Value.ToString().Trim();
                            }

                            DataRow drNew = dt.NewRow();
                            drNew["NAME"] = name;
                            drNew["VALUE"] = value;
                            dt.Rows.Add(drNew);
                        }
                        dgvData.DataSource = dt;
                    }
                    else
                    {
                        NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, $"File path {filePath} is not exist!");
                    }
                }
                else
                {
                    NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, $"Directory path {path} is not exist!");
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);

            }
        }

        public void ShowMessage(Form parent, string msgCaption, Exception ex, MessageBoxIcon showIcon)
        {
            OPIInfo.Q_OPIMessage.Enqueue(new OPIMessage(msgCaption, ex));
        }
    }
}
