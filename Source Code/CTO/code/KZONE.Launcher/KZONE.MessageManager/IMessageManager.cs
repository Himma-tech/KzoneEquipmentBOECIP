using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KZONE.MessageManager
{
    public interface IMessageManager
    {
        string ConfigFileName
        {
            get;
            set;
        }

        IConfiguration Configuration
        {
            get;
        }

        double AvgConcurrentCount
        {
            get;
        }

        double MaxConcurrentCount
        {
            get;
        }

        void Init();

        void MessageDispatch(string name, object[] parameters);

        void Invoke(string name, object[] parameters);

        void Reload();

        bool IsEnabled(string name);

        void Enable(string name);

        void Disable(string name);

        DataTable GetMessageMap(DataTable messageTable);
    }
}
