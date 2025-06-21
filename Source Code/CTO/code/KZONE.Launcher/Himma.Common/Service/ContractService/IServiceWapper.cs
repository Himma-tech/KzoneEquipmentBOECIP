using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Service.ContractService
{
    public interface IServiceWapper
    {
        void Intial();
        void StartAllServices();
        void StopAllServices();
        void StopService(string serviceName);
        void StartService(string serviceName);
        // Service.BaseService GetService(string serviceName);

    }
}
