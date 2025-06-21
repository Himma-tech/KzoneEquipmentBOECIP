using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Himma.Common.Models
{
    public static class KeyBoardControl
    {

        #region 卸载加载

        private const int CM_LOCATE_DEVNODE_NORMAL = 0x00000000;
        private const int CM_REENUMERATE_NORMAL = 0x00000000;
        private const int CR_SUCCESS = 0x00000000;

        [DllImport("CfgMgr32.dll", SetLastError = true)]
        private static extern int CM_Locate_DevNodeA(ref int pdnDevInst, string pDeviceID, int ulFlags);

        [DllImport("CfgMgr32.dll", SetLastError = true)]
        private static extern int CM_Reenumerate_DevNode(int dnDevInst, int ulFlags);


        /// <summary>
        /// 卸载设备
        /// </summary>
        /// <param name="ID">实例化路径</param>
        /// <param name="Devpath">Devcon路径</param>
        public static void UnloadDevices(List<dynamic> Drives, string Devpath)
        {
            //根据某个条件查找到对应设备
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM win32_PnPEntity ");

            //遍历所有设备信息
            foreach (ManagementObject mgt in searcher.Get())
            {
                if (Drives.Find(x => x.PARAM_VALUE == Convert.ToString(mgt["DeviceID"])) != null)
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = Devpath;
                    startInfo.Arguments = "-remove @" + Convert.ToString(mgt["DeviceID"]);
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardInput = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.CreateNoWindow = false;
                    process.StartInfo = startInfo;
                    startInfo.Verb = "RunAs";
                    process.Start();
                    Drives.Remove(Drives.FirstOrDefault(i => i.PARAM_VALUE == Convert.ToString(mgt["DeviceID"])));
                }
                if (Drives.Count == 0)
                    break;
            }
        }
        /// <summary>
        /// 卸载设备
        /// </summary>
        /// <param name="ID">实例化路径</param>
        /// <param name="Devpath">Devcon路径</param>
        public static void UnloadDevices(string ID, string Devpath)
        {
            var j = 0;
            //根据某个条件查找到对应设备
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM win32_PnPEntity ");

            //遍历所有设备信息
            foreach (ManagementObject mgt in searcher.Get())
            {
                if (Convert.ToString(mgt["DeviceID"]) == ID)
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = Devpath;
                    startInfo.Arguments = "-remove @" + Convert.ToString(mgt["DeviceID"]);
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardInput = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.CreateNoWindow = false;
                    process.StartInfo = startInfo;
                    startInfo.Verb = "RunAs";
                    process.Start();
                }
            }
        }

        /// <summary>
        /// 加载设备
        /// </summary>
        public static void LoadDevices()
        {
            int pdnDevInst = 0;
            CM_Locate_DevNodeA(ref pdnDevInst, null, CM_LOCATE_DEVNODE_NORMAL);
            CM_Reenumerate_DevNode(pdnDevInst, CM_REENUMERATE_NORMAL);
        }

        #endregion


        #region 启用/警用

        #region 定义
        [Flags]
        private enum DIGCF : uint
        {
            DEFAULT = 0x00000001,
            PRESENT = 0x00000002,
            ALLCLASSES = 0x00000004,
            PROFILE = 0x00000008,
            DEVICEINTERFACE = 0x00000010
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVINFO_DATA
        {
            public UInt32 cbSize;
            public Guid ClassGuid;
            public UInt32 DevInst;
            public IntPtr Reserved;

        }


        [StructLayout(LayoutKind.Sequential)]
        private struct SP_CLASSINSTALL_HEADER
        {
            public UInt32 cbSize;
            public DIF InstallFunction;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct SP_PROPCHANGE_PARAMS
        {
            public SP_CLASSINSTALL_HEADER ClassInstallHeader;
            public DICS StateChange;
            public DICS_FLAG Scope;
            public UInt32 HwProfile;
        }
        private enum DIF : uint
        {
            SELECTDEVICE = 0x00000001,
            INSTALLDEVICE = 0x00000002,
            ASSIGNRESOURCES = 0x00000003,
            PROPERTIES = 0x00000004,
            REMOVE = 0x00000005,
            FIRSTTIMESETUP = 0x00000006,
            FOUNDDEVICE = 0x00000007,
            SELECTCLASSDRIVERS = 0x00000008,
            VALIDATECLASSDRIVERS = 0x00000009,
            INSTALLCLASSDRIVERS = 0x0000000A,
            CALCDISKSPACE = 0x0000000B,
            DESTROYPRIVATEDATA = 0x0000000C,
            VALIDATEDRIVER = 0x0000000D,
            DETECT = 0x0000000F,
            INSTALLWIZARD = 0x00000010,
            DESTROYWIZARDDATA = 0x00000011,
            PROPERTYCHANGE = 0x00000012,
            ENABLECLASS = 0x00000013,
            DETECTVERIFY = 0x00000014,
            INSTALLDEVICEFILES = 0x00000015,
            UNREMOVE = 0x00000016,
            SELECTBESTCOMPATDRV = 0x00000017,
            ALLOW_INSTALL = 0x00000018,
            REGISTERDEVICE = 0x00000019,
            NEWDEVICEWIZARD_PRESELECT = 0x0000001A,
            NEWDEVICEWIZARD_SELECT = 0x0000001B,
            NEWDEVICEWIZARD_PREANALYZE = 0x0000001C,
            NEWDEVICEWIZARD_POSTANALYZE = 0x0000001D,
            NEWDEVICEWIZARD_FINISHINSTALL = 0x0000001E,
            UNUSED1 = 0x0000001F,
            INSTALLINTERFACES = 0x00000020,
            DETECTCANCEL = 0x00000021,
            REGISTER_COINSTALLERS = 0x00000022,
            ADDPROPERTYPAGE_ADVANCED = 0x00000023,
            ADDPROPERTYPAGE_BASIC = 0x00000024,
            RESERVED1 = 0x00000025,
            TROUBLESHOOTER = 0x00000026,
            POWERMESSAGEWAKE = 0x00000027,
            ADDREMOTEPROPERTYPAGE_ADVANCED = 0x00000028,
            UPDATEDRIVER_UI = 0x00000029,
            FINISHINSTALL_ACTION = 0x0000002A,
            RESERVED2 = 0x00000030,
        }
        private enum DICS : uint
        {
            ENABLE = 0x00000001,
            DISABLE = 0x00000002,
            PROPCHANGE = 0x00000003,
            START = 0x00000004,
            STOP = 0x00000005,
        }
        [Flags]
        private enum DICS_FLAG : uint
        {
            GLOBAL = 0x00000001,
            CONFIGSPECIFIC = 0x00000002,
            CONFIGGENERAL = 0x00000004,
        }

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr handle);
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevsW([In] ref Guid ClassGuid, [MarshalAs(UnmanagedType.LPWStr)] string Enumerator, IntPtr parent, DIGCF flags);
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, UInt32 memberIndex, [Out] out SP_DEVINFO_DATA deviceInfoData);
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiSetClassInstallParams(IntPtr deviceInfoSet, [In] ref SP_DEVINFO_DATA deviceInfoData, [In] ref SP_PROPCHANGE_PARAMS classInstallParams, UInt32 ClassInstallParamsSize);
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiChangeState(IntPtr deviceInfoSet, [In] ref SP_DEVINFO_DATA deviceInfoData);





        #endregion

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="deviceClassGuid">类ID</param>
        /// <param name="DdevInst">序列号</param>
        private static void DisableDevice(string deviceClassGuid, UInt32 DdevInst)
        {
            IntPtr info = IntPtr.Zero;
            Guid NullGuid = Guid.Empty;
            try
            {
                info = SetupDiGetClassDevsW(ref NullGuid, null, IntPtr.Zero, DIGCF.ALLCLASSES | DIGCF.PROFILE);
                SP_DEVINFO_DATA devdata = new SP_DEVINFO_DATA();
                devdata.cbSize = (UInt32)Marshal.SizeOf(devdata);
                ///遍历设备
                for (uint i = 0; SetupDiEnumDeviceInfo(info, i, out devdata); i++)
                {
                    if (devdata.ClassGuid == new Guid(deviceClassGuid) && devdata.DevInst == DdevInst)
                    {
                        SP_CLASSINSTALL_HEADER header = new SP_CLASSINSTALL_HEADER();
                        header.cbSize = (UInt32)Marshal.SizeOf(header);
                        header.InstallFunction = DIF.PROPERTYCHANGE;
                        SP_PROPCHANGE_PARAMS propchangeparams = new SP_PROPCHANGE_PARAMS
                        {
                            ClassInstallHeader = header,
                            StateChange = DICS.DISABLE,
                            Scope = DICS_FLAG.GLOBAL,
                            HwProfile = 0
                        };
                        SetupDiSetClassInstallParams(info, ref devdata, ref propchangeparams, (UInt32)Marshal.SizeOf(propchangeparams));
                        SetupDiChangeState(info, ref devdata);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ChangeMouseState failed,the reason is {0}", ex));
            }
            finally
            {
                if (info != IntPtr.Zero)
                    SetupDiDestroyDeviceInfoList(info);
            }
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="deviceClassGuid">类ID</param>
        /// <param name="DdevInst">序列号</param>
        private static void EnableDevice(string deviceClassGuid, UInt32 DdevInst)
        {

            IntPtr info = IntPtr.Zero;
            Guid NullGuid = Guid.Empty;
            try
            {
                info = SetupDiGetClassDevsW(ref NullGuid, null, IntPtr.Zero, DIGCF.ALLCLASSES);
                SP_DEVINFO_DATA devdata = new SP_DEVINFO_DATA();
                devdata.cbSize = (UInt32)Marshal.SizeOf(devdata);
                ///遍历设备
                for (uint i = 0; SetupDiEnumDeviceInfo(info, i, out devdata); i++)
                {
                    if (devdata.ClassGuid == new Guid(deviceClassGuid))
                    {
                        SP_CLASSINSTALL_HEADER header = new SP_CLASSINSTALL_HEADER();
                        header.cbSize = (UInt32)Marshal.SizeOf(header);
                        header.InstallFunction = DIF.PROPERTYCHANGE;
                        SP_PROPCHANGE_PARAMS propchangeparams = new SP_PROPCHANGE_PARAMS
                        {
                            ClassInstallHeader = header,
                            StateChange = DICS.ENABLE,
                            Scope = DICS_FLAG.GLOBAL,
                            HwProfile = 0
                        };
                        SetupDiSetClassInstallParams(info, ref devdata, ref propchangeparams, (UInt32)Marshal.SizeOf(propchangeparams));
                        SetupDiChangeState(info, ref devdata);

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ChangeMouseState failed,the reason is {0}", ex));
            }
            finally
            {
                if (info != IntPtr.Zero)
                    SetupDiDestroyDeviceInfoList(info);
            }
        }

        /// <summary>
        /// 获取该类ID所有设备信息
        /// </summary>
        /// <param name="deviceClassGuid"></param>
        /// <returns></returns>
        private static List<SP_DEVINFO_DATA> GetDevData(string deviceClassGuid)
        {

            IntPtr info = IntPtr.Zero;
            Guid NullGuid = Guid.Empty;
            List<SP_DEVINFO_DATA> list = new List<SP_DEVINFO_DATA>();
            try
            {
                info = SetupDiGetClassDevsW(ref NullGuid, null, IntPtr.Zero, DIGCF.ALLCLASSES);
                SP_DEVINFO_DATA devdata = new SP_DEVINFO_DATA();
                devdata.cbSize = (UInt32)Marshal.SizeOf(devdata);
                ///遍历设备
                for (uint i = 0; SetupDiEnumDeviceInfo(info, i, out devdata); i++)
                {
                    if (devdata.ClassGuid == new Guid(deviceClassGuid))
                    {

                        list.Add(devdata);
                    }
                }

                return list;

            }
            catch (Exception)
            {
                return null;

            }

        }


        #endregion
    }
}
