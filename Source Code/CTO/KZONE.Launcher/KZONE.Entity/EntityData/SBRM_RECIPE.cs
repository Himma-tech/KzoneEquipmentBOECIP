using System;
using System.Collections;
using System.Collections.Generic;

namespace KZONE.Entity
{
	#region SBRMRECIPE

	/// <summary>
	/// SBRMRECIPE object for NHibernate mapped table 'SBRM_RECIPE'.
	/// </summary>
	public class RecipeEntityData:EntityData
	{
		#region Member Variables
		

		public virtual long Id { get; set; }
		public virtual string FABTYPE { get; set; }
        public virtual string LINETYPE { get; set; }
        public virtual string LINEID { get; set; }
        public virtual string NODENO { get; set; }
        public virtual string RECIPENO { get; set; }
        public virtual string RECIPEID { get; set; }
        public virtual DateTime CREATETIME { get; set; }
        public virtual string VERSIONNO { get; set; }
        public virtual string RECIPESTATUS { get; set; }
        public virtual string OPERATORID { get; set; }
        public virtual string FILENAME { get; set; }
      

        #endregion



        public RecipeEntityData() { }


	}
	#endregion
}