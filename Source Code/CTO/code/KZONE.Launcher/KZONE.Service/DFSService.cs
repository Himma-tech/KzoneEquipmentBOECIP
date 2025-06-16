using KZONE.ConstantParameter;
using KZONE.Entity;
using KZONE.PLCAgent.PLC;
using Spring.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml;
using static KZONE.EntityManager.SaveFileThread;

namespace KZONE.Service
{
    public class DFSService
    {
        private static string _fabType = string.Empty;
        private static string _lineID = string.Empty;
        private static string _eqpID = string.Empty;
        private static string _ftpFileDirectoryPath = string.Empty;
        private static string _userName = string.Empty;
        private static string _password = string.Empty;
        private static string _stepID = string.Empty;
        private static string _localFileDirectoryPath = string.Empty;
        private static bool _enable = false;
        private static bool _InitSuccess = false;

        private static DFSService dFSService = new DFSService();


        public static string FabType
        {
            get
            {
                return _fabType;
            }
            set
            {
                _fabType = value;
            }
        }

        public static string LineID
        {
            get
            {
                return _lineID;
            }
            set
            {
                _lineID = value;
            }
        }

        public static string EQPID
        {
            get
            {
                return _eqpID;
            }
            set
            {
                _eqpID = value;
            }
        }

        public static string FtpFileDirectoryPath
        {
            get
            {
                return _ftpFileDirectoryPath;
            }
            set
            {
                _ftpFileDirectoryPath = value;
            }
        }

        public static string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public static string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public static string StepID
        {
            get
            {
                return _stepID;
            }
            set
            {
                _stepID = value;
            }
        }

        public static string LocalFileDirectoryPath
        {
            get
            {
                return _localFileDirectoryPath;
            }
            set
            {
                _localFileDirectoryPath = value;
            }
        }

        public static bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                _enable = value;
            }
        }

        public static bool InitSuccess
        {
            get
            {
                return _InitSuccess;
            }
            set
            {
                _InitSuccess = value;
            }
        }

        public static bool Init(string lineID)
        {
            bool res = true;
            try
            {
                Type type = typeof(DFSService);
                PropertyInfo propertyInfo = null;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(@"..\Config\DFS\DFS_Config.xml");

                foreach (XmlNode item in xmlDocument.DocumentElement.ChildNodes)
                {
                    if (item.NodeType == XmlNodeType.Element)
                    {
                        XmlElement element = (XmlElement)item;
                        var res1 = element.Attributes.GetNamedItem("LineID");
                        if (res1.Value.ToUpper().Trim() == lineID.ToUpper().Trim())
                        {
                            foreach (XmlAttribute attribute in element.Attributes)
                            {
                                propertyInfo = type.GetProperty(attribute.Name);
                                if (propertyInfo != null)
                                {
                                    if (propertyInfo.PropertyType == typeof(Boolean))
                                    {
                                        bool enable = Boolean.Parse(attribute.InnerText.Trim());
                                        propertyInfo.SetValue(DFSService.dFSService, enable, null);
                                    }
                                    else
                                    {
                                        propertyInfo.SetValue(DFSService.dFSService, attribute.InnerText.Trim(), null);
                                    }
                                }
                            }

                            InitSuccess = res;
                            return res;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                InitSuccess = res;
                return res;
            }
            catch
            {
                res = false;
                InitSuccess = res;
                return res;
            }



        }

        public static bool SaveDVFile(Trx trx, out string localFilePath)
        {
            bool flag = false;
            try
            {
                if (DFSService.Enable)
                {
                    string glassID = trx[0][0]["GlassID"].Value.Trim();
                    int strcount = glassID.Length;
                    string yyyymmdd = trx.TrackKey.Substring(0, 8);
                    string hhmmss = trx.TrackKey.Substring(8, 6);
                    //Glass ID_Step ID_EQP ID_yyyymmdd_hhmmss.gls
                    string fileName = $"{glassID}_{DFSService.StepID}_{DFSService.EQPID}_{yyyymmdd}_{hhmmss}.gls";
                    string parameterName = "GLS_INF";
                    string parameterValue = "GLS_INF_DATA";
                    for (int i = 0; i < trx[0][1].Items.Count; i++)
                    {
                        if (i == 0)
                        {
                            parameterName += ",FILE_COUNT";
                            parameterValue += $",{trx[0][1].Items.Count + 1}";
                        }

                        string[] res = trx[0][1][i].Name.ToUpper().Trim().Split('/');
                        string name = res[0];
                        double rate = 0;

                        string value = trx[0][1][i].Value.Trim();
                        if (res.Length > 1)
                        {
                            rate = Double.Parse(res[1]);

                            if (trx[0][1][i].Metadata.Expression != ItemExpressionEnum.ASCII)
                            {
                                double tmp = double.Parse(value);
                                value = (tmp / rate).ToString();
                            }

                        }

                        if (trx[0][1][i].Metadata.Expression == ItemExpressionEnum.ASCII)
                        {

                            if (value == string.Empty)
                            {
                                value = "***";
                            }

                        }

                        parameterName += $",{name.ToUpper().Trim()}";
                        parameterValue += $",{value}";

                        if (i == trx[0][1].Items.Count - 1)
                        {
                            parameterName += "\r\n";
                            parameterValue += "\r\n";
                        }

                    }

                    string data = parameterName + parameterValue;
                    string saveDirectoryPath = DFSService.LocalFileDirectoryPath + $"{DFSService.LineID}\\DFS\\{yyyymmdd}";
                    if (!Directory.Exists(saveDirectoryPath))
                    {
                        Directory.CreateDirectory(saveDirectoryPath);
                    }
                    File.WriteAllText($"{saveDirectoryPath}\\{fileName}", data);
                    flag = true;
                    localFilePath = $"{saveDirectoryPath}\\{fileName}";

                    //新增上报至Dummy文件夹
                    //文件名：Glass ID_Step ID_EQP ID_yyyymmdd_hhmmss.gls.dmy ps:空文件即可
                    //本地存档
                    string savedmyDirectoryPath = saveDirectoryPath + "\\Dummy";
                    if (!Directory.Exists(savedmyDirectoryPath))
                    {
                        Directory.CreateDirectory(savedmyDirectoryPath);
                    }
                    File.WriteAllText($"{savedmyDirectoryPath}\\{fileName}.dmy", "");
                }
                else
                {
                    localFilePath = string.Empty;
                }

                return flag;
            }
            catch
            {
                localFilePath = string.Empty;
                return flag;
            }
        }


        public static bool FtpUpload(string glassID, string localFilePath)
        {
            bool res = false;
            try
            {
                string ftpRootAddress = DFSService.FtpFileDirectoryPath + DFSService.StepID + "/";//ftp://10.45.128.20/DATA/C30001/

                int length = glassID.Length;
                string destinationPath = string.Empty;
                string destinationDirectory = string.Empty;
                string[] tmp = localFilePath.Split('\\');
                string fileName = tmp[tmp.Length - 1];
                string currentSubDirectoryPath = string.Empty;
                //3种格式的GlassID
                if (length == 11)
                {
                    //-> KAEA36001A1
                    //-> KAEA/36/0/01/
                    currentSubDirectoryPath = $"{glassID.Substring(0, 4)}/{glassID.Substring(4, 2)}/{glassID.Substring(6, 1)}/{glassID.Substring(7, 2)}/";
                    destinationDirectory = ftpRootAddress + currentSubDirectoryPath;
                    destinationPath = destinationDirectory + fileName;
                }
                else if (length == 12)
                {
                    //-> KAEA36001A1B
                    //-> KAEA/36/0/01/A1/
                    currentSubDirectoryPath = $"{glassID.Substring(0, 4)}/{glassID.Substring(4, 2)}/{glassID.Substring(6, 1)}/{glassID.Substring(7, 2)}/{glassID.Substring(9, 2)}/";
                    destinationDirectory = ftpRootAddress + currentSubDirectoryPath;
                    destinationPath = destinationDirectory + fileName;
                }
                else if (length == 15)
                {
                    //-> KAEA36001A1B001
                    //-> KAEA/36/0/01/A1/B/
                    currentSubDirectoryPath = $"{glassID.Substring(0, 4)}/{glassID.Substring(4, 2)}/{glassID.Substring(6, 1)}/{glassID.Substring(7, 2)}/{glassID.Substring(9, 2)}/{glassID.Substring(11, 1)}/";
                    destinationDirectory = ftpRootAddress + currentSubDirectoryPath;
                    destinationPath = destinationDirectory + fileName;
                }

                if (destinationPath == string.Empty)
                {
                    return res;
                }

                //上传
                //LIST
                //if (!EquipmentService.DirectoryPathList.Contains(destinationDirectory))
                //{
                //    string[] strings = currentSubDirectoryPath.Split('/');
                //    if (strings.Length > 0)
                //    {
                //        CreateServerPath(ftpRootAddress, strings);
                //        EquipmentService.ServerPathRefresh(DFSService.FtpFileDirectoryPath);
                //    }
                //}
                //return res = Ftp.Upload(DFSService.UserName, DFSService.Password, localFilePath, destinationPath, false);
                return res = Ftp.FtpUpload(ftpRootAddress, DFSService.UserName, DFSService.Password, currentSubDirectoryPath + fileName, localFilePath);
            }
            catch
            {
                return res;
            }
        }

        public static bool CreateServerPath(string ftpRootAddress, string[] paths)
        {
            bool res = false;
            try
            {
                string curPath = string.Empty;
                for (int i = 0; i < paths.Length; i++)
                {
                    if (i == 0)
                    {
                        curPath = ftpRootAddress + paths[i] + "/";
                    }
                    else
                    {
                        curPath += paths[i] + "/";
                    }

                    if (!EquipmentService.DirectoryPathList.Contains(curPath))
                    {
                    //需要新建一个路径
                    Re:
                        bool flag = Ftp.MakeDir(DFSService.UserName, DFSService.Password, curPath);
                        if (!flag)
                        {
                            Thread.Sleep(100);
                            goto Re;
                        }
                        EquipmentService.DirectoryPathList.Add(curPath);
                    }
                }
                res = true;
                return res;
            }
            catch
            {
                return res;
            }

        }
    }
}

