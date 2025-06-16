using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBCSJOBHISTORY

	/// <summary>
	/// SBCSJOBHISTORY object for NHibernate mapped table 'SBCS_JOBHISTORY'.
	/// </summary>
    public class JobHistory : EntityData
    {
        #region Member Variables

        public virtual long ID { get; set; }
        public virtual DateTime UPDATETIME { get; set; }
        public virtual string EVENTNAME { get; set; }
        public virtual string NODENO { get; set; }
        public virtual string UNITNO { get; set; }


        public virtual string Cassette_Sequence_No { get; set; }
        public virtual string Job_Sequence_No { get; set; }
      
        public virtual string GlassID { get; set; }

        public virtual string Lot_ID { get; set; }
        public virtual string Product_ID { get; set; }
        public virtual string Operation_ID { get; set; }
        public virtual string CST_Operation_Mode { get; set; }
        public virtual string Substrate_Type { get; set; }
        public virtual string Product_Type { get; set; }
        public virtual string Job_Type { get; set; }
        public virtual string Dummy_Type { get; set; }
        public virtual string Skip_Flag { get; set; }
        public virtual string Process_Flag { get; set; }
        public virtual string Process_Reason_Code { get; set; }
        public virtual string LOT_Code { get; set; }
        public virtual string Glass_Thickness { get; set; }
        public virtual string Glass_Degree { get; set; }
        public virtual string Inspection_Flag { get; set; }
        public virtual string Job_Judge { get; set; }
        public virtual string Job_Grade { get; set; }
        public virtual string Job_Recovery_Flag { get; set; }
        public virtual string Mode { get; set; }
        public virtual string Step_ID { get; set; }
        public virtual string VCR_Read_ID { get; set; }
        public virtual string Master_Recipe_ID { get; set; }
        public virtual string PPID { get; set; }


        #endregion


    }
    #endregion
}