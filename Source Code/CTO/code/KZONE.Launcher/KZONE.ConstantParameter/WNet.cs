using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace KZONE.ConstantParameter
{
    public class WNet
    {
        [StructLayout(LayoutKind.Sequential)]
        private class NetResource
        {
            public int dwScope;

            public int dwType;

            public int dwDisplayType;

            public int dwUsage;

            public string lpLocalName;

            public string lpRemoteName;

            public string lpComment;

            public string lpProvider;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Ansi)]
        private static extern uint WNetAddConnection2(WNet.NetResource lpNetResource, string lpPassword, string lpUsername, uint dwFlags);

        [DllImport("Mpr.dll", CharSet = CharSet.Ansi)]
        private static extern uint WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);

        public static uint WNetAddConnection(string username, string password, string remoteName, string localName)
        {
            WNet.NetResource netResource = new WNet.NetResource();
            netResource.dwScope = 2;
            netResource.dwType = 1;
            netResource.dwDisplayType = 3;
            netResource.dwUsage = 1;
            netResource.lpLocalName = localName;
            netResource.lpRemoteName = remoteName.TrimEnd(new char[]
			{
				'\\'
			});
            if (Directory.Exists(localName))
            {
                return 0u;
            }
            return WNet.WNetAddConnection2(netResource, password, username, 0u);
        }

        public static uint WNetCancelConnection(string name, bool force)
        {
            return WNet.WNetCancelConnection2(name, 0u, force);
        }
    }
}
