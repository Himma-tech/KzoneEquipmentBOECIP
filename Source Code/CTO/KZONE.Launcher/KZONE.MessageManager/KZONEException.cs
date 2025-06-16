using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.MessageManager
{
    public class KZONEException : Exception
	{
        public KZONEException(string msg) : base(msg)
		{
		}
	}
}
