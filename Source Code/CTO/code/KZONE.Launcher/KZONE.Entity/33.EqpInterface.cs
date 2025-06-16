using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace KZONE.Entity
{
    [Serializable]
    public class EqpInterface : EntityFile
    {
        public string Name { get; set; }

    }

    [Serializable]
    public class CIMInterface
    {

    }
}
