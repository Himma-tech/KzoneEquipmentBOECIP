using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KZONE.ConstantParameter;
using System.Xml;

namespace KZONE.Entity
{
    [Serializable]
    public class Subscriber:IEquatable<Subscriber>
    {   
        private eSubscribeType _subscribeType=eSubscribeType.JobKey;
        private SerializableDictionary<string, string> _parameters = new SerializableDictionary<string, string>();
        private eSubscribeState _state = eSubscribeState.Create;
        private string _inspectionEquipmentNo = string.Empty;
        private int _inspectionSequence = 0;

        public int InspectionSequence
        {
            get { return _inspectionSequence; }
            set { _inspectionSequence = value; }
        }

        public string InspectionEquipmentNo
        {
            get { return _inspectionEquipmentNo; }
            set { _inspectionEquipmentNo = value; }
        }
        public eSubscribeState State
        {
            get { return _state; }
            set { _state = value; }
        }
        public eSubscribeType SubscribeType
        {
          get { return _subscribeType; }
          set { _subscribeType = value; }
        }
        public SerializableDictionary<string, string> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public bool Equals(Subscriber other)
        {
            if (other == null) return false;
            if (this.InspectionEquipmentNo != other.InspectionEquipmentNo) return false;
            if (this.InspectionSequence != other.InspectionSequence) return false;
            if (this.SubscribeType != other.SubscribeType) return false;
            if (this.Parameters.Count != other.Parameters.Count) return false;
            foreach (string key in this.Parameters.Keys)
            {
                if (!other.Parameters.ContainsKey(key))
                {
                    return false;
                }
                if(this.Parameters[key]!=other.Parameters[key]) return false;

            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            Subscriber objAsPart = obj as Subscriber;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

       
        public void WriteTo(XmlWriter writer)
        {
            writer.WriteStartElement("subscriber");
            writer.WriteAttributeString("inspectionEquipmentNo", this.InspectionEquipmentNo);
            writer.WriteAttributeString("type", this.SubscribeType.ToString());
            writer.WriteAttributeString("state", this.State.ToString());
            if (Parameters.Count > 0)
            {
                writer.WriteStartElement("parameters");
                foreach (string key in Parameters.Keys)
                {
                    writer.WriteStartElement("item");
                    writer.WriteAttributeString("key", key);
                    writer.WriteAttributeString("value", this.Parameters[key]);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public override string ToString()
        {
            StringBuilder sb =new StringBuilder();
            sb.AppendFormat("Equipment [{0}],Type [{1}],State [{2}] Parameters ", InspectionEquipmentNo, SubscribeType.ToString(), State.ToString());

            foreach (string key in Parameters.Keys)
            {
                sb.AppendFormat("key [{0}],value [{1}];", key, Parameters[key]);
            }
            return sb.ToString();
        }
       
    }
}
