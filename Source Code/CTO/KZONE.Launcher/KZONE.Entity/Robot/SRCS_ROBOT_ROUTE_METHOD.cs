using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;

namespace KZONE.Entity
{
    public class SRCS_ROBOT_ROUTE_METHOD : EntityData
    {
        private void SetStringProertyToStringEmpty()
        {
            PropertyInfo[] properties = typeof(SRCS_ROBOT_ROUTE_METHOD).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(this, string.Empty, null);
            }
        }

        #region Constructors

        public SRCS_ROBOT_ROUTE_METHOD()
        {
            SetStringProertyToStringEmpty();
        }

        #endregion

        #region Public Properties

        public virtual long OBJECTKEY { get; set; }

        public virtual string SERVERNAME { get; set; }

        public virtual string ROBOTNAME { get; set; }

        public virtual string ROUTENAME { get; set; }

        public virtual string RECV_METHOD { get; set; }

        public virtual string SEND_METHOD { get; set; }

        public virtual string ARM_LOAD_METHOD { get; set; }

        public virtual string ARM_UNLOAD_METHOD { get; set; }

        #endregion
    }
}
