using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace KZONE.Entity
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
    public class Qtime:ICloneable
    {
        public string QTimeID {get;set;}

        public string LineName { get; set; }

        public string StartEquipmentNo{ get; set; }

        public string StartEquipmentId { get; set; }

        public string StartUnits { get; set; }

        public string StartEvent { get; set; }

        public string RecipeID { get; set; }

        public string EndEquipmentNo { get; set; }

        public string EndEquipmentId { get; set; }

        public string EndUnits { get; set; }

        public string EndEvent { get; set; }

        public string QtimeValue { get; set; } 

        public bool OverQTimeFlag { get; set; }
        public DateTime StartQTime { get; set; }
        public DateTime EndQTime { get; set; }
        public bool StartQTimeFlag { get; set; }
      
        public bool Enable { get; set; } //QTIME啟用
        public Qtime()
        {
            QTimeID = string.Empty;
            LineName = string.Empty;
            StartEquipmentNo = string.Empty;
            StartEvent = string.Empty;
            StartUnits = string.Empty;
            RecipeID = string.Empty;
            EndEquipmentNo = string.Empty;
            EndUnits = string.Empty;
            EndEvent = string.Empty;
            QtimeValue = "0";   //DB設定的SETTIMEVALUE
            OverQTimeFlag = false;
            StartQTimeFlag = false;
            Enable = false;
        }

        public object Clone()
        {
            Qtime qt = (Qtime)this.MemberwiseClone();
            return qt;
        }
    }
   
}