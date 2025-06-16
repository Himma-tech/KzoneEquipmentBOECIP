using System;
using System.Collections;

namespace KZONE.Entity
{
    #region 

    /// <summary>
    /// TANKHISTORY object for NHibernate mapped table 'SBCS_TANKHISTORY'.
    /// </summary>
   [Serializable]
    public class TankHistory : EntityData
	{
        #region Member Variables

        public virtual long ID { get; set; }
        public virtual string NODEID { get; set; }
        public virtual string  TANKID { get; set; }
        public virtual string TANKEVENT { get; set; }
        public virtual DateTime STARTTIME { get; set; }
        public virtual DateTime ENDTIME { get; set; }
        public virtual string TOTALTIME { get; set; }
        public virtual string QUANTITY { get; set; }
        public virtual string SPEED { get; set; }
        public virtual string OPERATORID { get; set; }

        #endregion



        public TankHistory() { }

      

	}
	#endregion
}