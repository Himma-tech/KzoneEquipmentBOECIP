using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    public class MaterialConsume
    {
        public string MaterialID { get; set; }
        public string Quantity { get; set; }
        public string MaterialType { get; set; }
        public string JigID { get; set; }

        public MaterialConsume() {
            MaterialID = "";
            Quantity = "";
            MaterialType = "";
            JigID = "";
        }

        public MaterialConsume(string id, string qty, string mType,string jigId) {
            MaterialID = id;
            Quantity = qty;
            MaterialType = mType;
            JigID = jigId;
        }
    }
}
