using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;

namespace KZONE.Entity
{
    public class SRCS_ROUTE_STEP_JUMP : EntityData
    {
        private void SetStringProertyToStringEmpty()
        {
            PropertyInfo[] properties = typeof(SRCS_ROUTE_STEP_JUMP).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(this, string.Empty, null);
            }
        }

        #region Constructors

        public SRCS_ROUTE_STEP_JUMP()
        {
            SetStringProertyToStringEmpty();
        }

        #endregion

        #region Public Properties

        public virtual long OBJECTKEY { get; set; }

        public virtual string SERVERNAME { get; set; }

        public virtual string ROUTENAME { get; set; }

        public virtual int FROMSTEPID { get; set; }

        public virtual int GOTOSTEPID { get; set; }

        public virtual string JUMP_METHOD { get; set; }

        #endregion
    }
}
