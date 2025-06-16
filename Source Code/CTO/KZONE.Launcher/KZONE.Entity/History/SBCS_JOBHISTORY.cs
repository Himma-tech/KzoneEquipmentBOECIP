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
        public virtual string CASSETTESEQNO { get; set; }
        public virtual string CASSETTESLOTNO { get; set; }
        public virtual string NODENO { get; set; }
        public virtual string UNITNO { get; set; }
        public virtual string JOBID { get; set; }


        public virtual string GroupNumber { get; set; }
        public virtual string GlassType { get; set; }
        public virtual string GlassJudge { get; set; }
        public virtual string ProcessSkipFlag { get; set; }
        public virtual string LastGlassFlag { get; set; }
        public virtual string CIMModeCreate { get; set; }
        public virtual string SamplingFlag { get; set; }
        public virtual string Reserved { get; set; }
        public virtual string InspectionJudgeResult { get; set; }
        public virtual string InspectionReservationSignal { get; set; }
        public virtual string ProcessReservationSignal { get; set; }
        public virtual string TrackingDataHistory { get; set; }
        public virtual string EquipmentSpecialFlag { get; set; }
        public virtual string GlassID { get; set; }
        public virtual string SorterGrade { get; set; }
        public virtual string GlassGrade { get; set; }
        public virtual string FromPortNo { get; set; }
        public virtual string TargetPortNo { get; set; }
        public virtual string TargetSlotNo { get; set; }
        public virtual string TargetCassetteID { get; set; }
        public virtual string Reserve { get; set; }
        public virtual string PPID { get; set; }


        #endregion


    }
    #endregion
}