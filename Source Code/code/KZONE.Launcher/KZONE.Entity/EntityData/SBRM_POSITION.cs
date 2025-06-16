using System;
using System.Collections;

namespace KZONE.Entity
{
    #region SBRM_POSITION

    /// <summary>
    /// SBRMALARM object for NHibernate mapped table 'SBRM_POSITION'.
    /// </summary>
    [Serializable]
    public class PositionEntityData : EntityData
    {
        #region Member Variables

        public virtual long ID { get; set; }
        public virtual string LineID { get; set; }
        public virtual string NodeNo { get; set; }
        public virtual string UnitType { get; set; }
        public virtual string UnitNo { get; set; }
        public virtual int PositionNo { get; set; }
        public virtual string PositionName { get; set; }


        #endregion

        #region Constructors

        public PositionEntityData() { }


        #endregion
    }
    #endregion
}