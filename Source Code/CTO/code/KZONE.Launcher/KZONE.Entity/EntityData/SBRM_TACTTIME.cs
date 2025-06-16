using System;
using System.Collections;

namespace KZONE.Entity
{
    #region SBRM_TACTTIME

    /// <summary>
    /// SBRMALARM object for NHibernate mapped table 'SBRM_TACTTIME'.
    /// </summary>
    [Serializable]
    public class TactTimeEntityData : EntityData
    {
        #region Member Variables

        public virtual long ID { get; set; }
        public virtual string LineID { get; set; }
        public virtual string NodeNo { get; set; }
        public virtual int TackNo { get; set; }
        public virtual string UnitNo { get; set; }
        public virtual int PositionNo { get; set; }
        public virtual string TackName { get; set; }


        #endregion

        #region Constructors

        public TactTimeEntityData() { }


        #endregion
    }
    #endregion
}