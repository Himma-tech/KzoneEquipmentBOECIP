using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common
{
    public class PLCSignalModel : EventArgs
    {
        public string name { get; set; }

        public dynamic data { get; set; }

        public int type { get; set; }
    }
}
