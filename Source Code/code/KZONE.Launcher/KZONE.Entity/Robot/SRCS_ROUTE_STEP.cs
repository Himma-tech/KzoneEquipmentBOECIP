using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;

namespace KZONE.Entity
{
    public class SRCS_ROUTE_STEP : EntityData
    {
        private void SetStringProertyToStringEmpty()
        {
            PropertyInfo[] properties = typeof(SRCS_ROUTE_STEP).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(this, string.Empty, null);
            }
        }

        #region Constructors

        public SRCS_ROUTE_STEP()
        {
            SetStringProertyToStringEmpty();
        }

        #endregion

        #region Public Properties

        public virtual long OBJECTKEY { get; set; }

        public virtual string SERVERNAME { get; set; }

        public virtual string ROUTENAME { get; set; }

        public virtual int STEPID { get; set; }

        public virtual string ROBOTACTION { get; set; }

        public virtual string ROBOTUSEARM { get; set; }

        /// <summary>
        /// STAGENAME 的 List, 以;隔開
        /// </summary>
        public virtual string STAGELIST { get; set; }

        public virtual int NEXTSTEPID { get; set; }

        #endregion
    }
}
