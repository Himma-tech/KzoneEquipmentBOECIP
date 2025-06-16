using System;
using System.Collections;

namespace KZONE.Entity
{
    #region SBCSINCOMPLETECST

    /// <summary>
    /// RecipeHistory object for NHibernate mapped table 'SBCS_RECIPEHISTORY'.
    /// </summary>
    public class RecipeHistory : EntityData
	{
        public virtual long Id { get; set; }
        public virtual DateTime UPDATETIME { get; set; }
        public virtual string NODENAME { get; set; }
        public virtual string RECIPENO { get; set; }
        public virtual string RECIPEID { get; set; }
        public virtual DateTime CREATETIME { get; set; }
        public virtual string VERSIONNO { get; set; }
        public virtual string EVENT { get; set; }
        public virtual string RECIPESTATUS { get; set; }
        public virtual string OPERATORID { get; set; }
        public virtual string FILENAME { get; set; }

    }
    #endregion
}