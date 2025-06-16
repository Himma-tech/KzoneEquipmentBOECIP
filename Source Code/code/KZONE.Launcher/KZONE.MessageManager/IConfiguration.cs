using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.MessageManager
{
    public interface IConfiguration
    {
        string ToFormatString();

        int SaveConfigFile(string formatString);
    }
}
