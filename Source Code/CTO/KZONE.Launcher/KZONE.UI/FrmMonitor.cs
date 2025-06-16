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
using KZONE.ConstantParameter;
using KZONE.Service;
using KZONE.Log;
using System.Reflection;

namespace KZONE.UI
{
    public partial class FrmMonitor : Form
    {
        public FrmMonitor()
        {
            InitializeComponent();
        }
        public string LoggerName { get; set; }
        public ConstantManager ConstantManager
        {
            get;
            set;
        }

        public ParameterManager ParameterManager
        {
            get;
            set;
        }

        public EquipmentService EquipmentService
        {
            get;
            set;
        }

        public void Init()
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            try
            {
                Equipment eqp = ObjectManager.EquipmentManager.GetEQP("L3");
                if (eqp == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", "L3"));
                   
                }
                this.label1.Text = "";
                this.label2.Text = "";
                this.label3.Text = "";
                this.label4.Text = "";
                this.label5.Text = "";
                this.label1.BackColor = this.BackColor;
                this.label2.BackColor = this.BackColor;
                this.label3.BackColor = this.BackColor;
                this.label4.BackColor = this.BackColor;
                this.label5.BackColor = this.BackColor;
                switch (eqp.File.WaterLevel)
                { 
                    case -2 :
                        this.label1.Text = "";
                        this.label1.BackColor = Color.Red;
                        break;
                    case 2:
                        this.label5.Text = "";
                        this.label5.BackColor = Color.Red;
                        break;
                    case -1:
                        this.label2.Text = "";
                        this.label2.BackColor = Color.Yellow;
                        break;
                    case 1:
                        this.label4.Text = "";
                        this.label4.BackColor = Color.Yellow;
                        break;
                    case 0:
                        this.label3.Text = "";
                        this.label3.BackColor = Color.Green;
                        break;
                    default:
                        this.label3.BackColor = Color.Green;
                        break;
                
                }

                IList<KZONE.Entity.Unit> units = ObjectManager.UnitManager.GetUnits();

                if (units == null)
                {
                    throw new Exception("CAN'T FIND UNITS");
                 
                }
                foreach(KZONE.Entity.Unit unit in units)
                {
                    switch (unit.Data.UNITNO)
                    { 
                    
                        case 1:
                            this.label26.Text = unit.File.SetCVSpeed;
                            this.label27.Text = unit.File.CvSpeed;
                            break;


                        case 2:
                            this.label28.Text = unit.File.SetCVSpeed;
                            this.label29.Text = unit.File.CvSpeed;
                            this.label30.Text = unit.File.C01Flow;
                            this.label31.Text = unit.File.C02Flow;

                            break;

                        case 3:
                            this.label32.Text = unit.File.SetCVSpeed;
                            this.label34.Text = unit.File.CvSpeed;

                            break;

                        case 4:
                            this.label37.Text = unit.File.C01Flow;
                            this.label36.Text = unit.File.C02Flow;

                            break;

                        case 5:
                            this.label41.Text = unit.File.C01Flow;
                            this.label40.Text = unit.File.C02Flow;


                            break;


                        case 6 :
                            this.label46.Text = unit.File.SetCVSpeed;
                            this.label54.Text = unit.File.CvSpeed;
                            this.label45.Text = unit.File.C01Flow;
                            this.label44.Text = unit.File.C02Flow;
                            break;

                        case 7 :
                            this.label53.Text = unit.File.SetCVSpeed;
                            this.label52.Text = unit.File.CvSpeed;
                            this.label51.Text = unit.File.C01Flow;
                            this.label50.Text = unit.File.C02Flow;

                            break;

                        case 8:
                            this.label61.Text = unit.File.SetCVSpeed;
                            this.label60.Text = unit.File.CvSpeed;

                            break;


                        default:

                            break;

                    }

                }


            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, GetType().Name, MethodInfo.GetCurrentMethod().Name + "()", ex);
            }



        }




    }
}
