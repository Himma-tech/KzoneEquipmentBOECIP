using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Service.ContractService
{
    public interface IRunnablaService : IService
    {
        void ExecuteMonitor();
        void Start();
        void Stop();
    }
}
