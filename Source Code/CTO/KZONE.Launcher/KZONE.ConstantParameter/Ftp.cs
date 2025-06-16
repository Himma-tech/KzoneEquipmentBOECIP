using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;

namespace KZONE.ConstantParameter
{
    public static class Ftp
    {
        public static bool Upload(string userName, string password, string fileName, string uploadfileName, bool deletelocalfile = false)
        {
            bool result;
            try
            {
                FileInfo fileInf = new FileInfo(fileName);
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(uploadfileName));
                reqFTP.Credentials = new NetworkCredential(userName, password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = "STOR";
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                FileStream fs = null;
                Stream strm = null;
                try
                {
                    fs = fileInf.OpenRead();
                    strm = reqFTP.GetRequestStream();
                    for (int contentLen = fs.Read(buff, 0, buffLength); contentLen != 0; contentLen = fs.Read(buff, 0, buffLength))
                    {
                        strm.Write(buff, 0, contentLen);
                    }
                    if (deletelocalfile)
                    {
                        fileInf.Delete();
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                    if (strm != null)
                    {
                        strm.Close();
                    }
                }
            }
            catch (Exception ex2)
            {
                throw ex2;
            }
            return result;
        }

        public static bool Download(string userName, string password, string serverFileName, string localPath, string targetFileNama, bool isOverride = true)
        {
            string targetFileName = Path.Combine(localPath, targetFileNama);
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            else if (File.Exists(targetFileName))
            {
                if (!isOverride)
                {
                    throw new Exception(string.Format("{0} is exists don't download from server.", targetFileNama));
                }
                File.Delete(targetFileName);
            }
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(serverFileName));
            ftpWebRequest.Method = "RETR";
            ftpWebRequest.Timeout = 60000;
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
            ftpWebRequest.Credentials = new NetworkCredential(userName, password);
            FtpWebResponse response = null;
            FileStream outputStream = null;
            Stream ftpStream = null;
            bool result;
            try
            {
                response = (ftpWebRequest.GetResponse() as FtpWebResponse);
                outputStream = new FileStream(targetFileName, FileMode.Create);
                ftpStream = response.GetResponseStream();
                long arg_A9_0 = response.ContentLength;
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                for (int readCount = ftpStream.Read(buffer, 0, bufferSize); readCount > 0; readCount = ftpStream.Read(buffer, 0, bufferSize))
                {
                    outputStream.Write(buffer, 0, readCount);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        public static bool Delete(string userName, string password, string serverFileName)
        {
            FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(serverFileName));
            reqFTP.Credentials = new NetworkCredential(userName, password);
            reqFTP.KeepAlive = false;
            reqFTP.Method = "DELE";
            string arg_35_0 = string.Empty;
            FtpWebResponse response = null;
            Stream datastream = null;
            bool result;
            try
            {
                response = (FtpWebResponse)reqFTP.GetResponse();
                long arg_4C_0 = response.ContentLength;
                datastream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(datastream))
                {
                    sr.ReadToEnd();
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (datastream != null)
                {
                    datastream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        public static bool MakeDir(string userName, string password, string targetPath)
        {
            FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(targetPath));
            reqFTP.Credentials = new NetworkCredential(userName, password);
            reqFTP.KeepAlive = false;
            reqFTP.Method = "MKD";
            reqFTP.UseBinary = true;
            FtpWebResponse response = null;
            bool result;
            try
            {
                response = (FtpWebResponse)reqFTP.GetResponse();
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        public static bool RemoveDir(string userName, string password, string targetPath)
        {
            FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(targetPath));
            reqFTP.Credentials = new NetworkCredential(userName, password);
            reqFTP.KeepAlive = false;
            reqFTP.Method = "RMD";
            reqFTP.UseBinary = true;
            FtpWebResponse response = null;
            bool result;
            try
            {
                response = (FtpWebResponse)reqFTP.GetResponse();
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        public static List<string> GetFileNameList(string userName, string password, string serverPath)
        {
            List<string> fileNames = new List<string>();
            List<string> result;
            try
            {
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(serverPath));
                ftpWebRequest.Method = "NLST";
                ftpWebRequest.Timeout = 60000;
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse response = ftpWebRequest.GetResponse() as FtpWebResponse;
                try
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        for (string strLine = sr.ReadLine(); strLine != null; strLine = sr.ReadLine())
                        {
                            fileNames.Add(strLine);
                        }
                    }
                    result = fileNames;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
            }
            catch (Exception ex2)
            {
                throw ex2;
            }
            return result;
        }
    }
}
