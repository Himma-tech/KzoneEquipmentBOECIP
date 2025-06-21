using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Service.Interface
{
    public interface IMainBLL
    {
        void Init();
        void _plc_PLCNotifyEventArgs(object obj, object sender);
    }
}
