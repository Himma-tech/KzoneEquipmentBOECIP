using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using KZONE.Log;
using System.Reflection;

namespace KZONE.MessageManager
{
    public class MessageConfiguration : IConfiguration
    {
        private string _fileName;

        private XmlDocument _document;

        public XmlDocument Document
        {
            get
            {
                return this._document;
            }
        }

        public MessageConfiguration(string configFile)
        {
            this._fileName = configFile;
        }

        public void Init()
        {
            this._document = new XmlDocument();
            if (!File.Exists(this._fileName))
            {
                throw new KZONEException("Service ConfigFile [" + this._fileName + "] Not Found");
            }
            this._document.Load(this._fileName);
        }

        public string ToFormatString()
        {
            if (this._document != null)
            {
                return this._document.OuterXml;
            }
            return "";
        }

        public int SaveConfigFile(string formatString)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(formatString);
                this._document = doc;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return 1;
            }
            return 0;
        }
    }
}
