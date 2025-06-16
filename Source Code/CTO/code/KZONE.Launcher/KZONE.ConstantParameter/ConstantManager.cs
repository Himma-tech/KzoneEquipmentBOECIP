using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace KZONE.ConstantParameter
{
    public class ConstantManager
    {
        private Dictionary<string, ConstantData> _constantList = new Dictionary<string, ConstantData>();

        private string _configFileName;

        public Dictionary<string, ConstantData> ConstantList
        {
            get
            {
                return this._constantList;
            }
        }

        public string ConfigFileName
        {
            get
            {
                return this._configFileName;
            }
            set
            {
                this._configFileName = value;
            }
        }

        public ConstantData this[string key]
        {
            get
            {
                if (this._constantList.ContainsKey(key))
                {
                    return this._constantList[key];
                }
                return null;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return this._constantList.Keys;
            }
        }

        public IEnumerable<ConstantData> Values
        {
            get
            {
                return this._constantList.Values;
            }
        }

        public void Init()
        {
            if (string.IsNullOrEmpty(this.ConfigFileName))
            {
                throw new Exception("Constant Mananger Config File is Empty!");
            }
            if (!File.Exists(this.ConfigFileName))
            {
                throw new Exception(string.Format("Not found Variable Config file {0}", this.ConfigFileName));
            }
            this.LoadConfig();
        }

        private void LoadConfig()
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.ConfigFileName);
            XmlNode root = document.DocumentElement;
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    this.RetrieveVariableMap(child);
                }
            }
        }

        private void RetrieveVariableMap(XmlNode parentNode)
        {
            string name = parentNode.Attributes["name"].Value;
            ConstantData constant = new ConstantData(name);
            if (this.ExistAttribute(parentNode, "default"))
            {
                ConstantItem item = new ConstantItem();
                item.Value = parentNode.Attributes["default"].Value;
                constant.DefaultValue = item;
                if (this.ExistAttribute(parentNode, "discription"))
                {
                    item.Discription = parentNode.Attributes["discription"].Value;
                }
            }
            if (parentNode.HasChildNodes)
            {
                foreach (XmlNode node in parentNode.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        foreach (XmlNode chilNode in node.ChildNodes)
                        {
                            if (chilNode.NodeType == XmlNodeType.Element)
                            {
                                string key = chilNode.Attributes["key"].Value;
                                ConstantItem item = new ConstantItem();
                                item.Value = chilNode.Attributes["value"].Value;
                                if (this.ExistAttribute(chilNode, "discription"))
                                {
                                    item.Discription = chilNode.Attributes["discription"].Value;
                                }
                                constant.Add(key, item);
                            }
                        }
                    }
                }
            }
            this._constantList.Add(constant.Name, constant);
        }

        private bool ExistAttribute(XmlNode node, string attributeName)
        {
            foreach (XmlAttribute artribute in node.Attributes)
            {
                if (artribute.Name == attributeName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(string key)
        {
            return this._constantList.ContainsKey(key);
        }

        public XmlDocument WriteToXML()
        {
            XmlDocument result;
            try
            {
                FileAttributes attributes = File.GetAttributes(this.ConfigFileName);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(this.ConfigFileName, FileAttributes.Normal);
                }
                StringBuilder output = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment
                };
                XmlWriter xmlWriter = XmlWriter.Create(output, settings);
                this.WriteTo(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
                XmlDocument document = new XmlDocument();
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "utf-8", null);
                document.LoadXml(output.ToString());
                document.InsertBefore(declaration, document.DocumentElement);
                document.Save(this.ConfigFileName);
                result = document;
            }
            catch
            {
                result = null;
            }
            return result;
        }

        private void WriteTo(XmlWriter writer)
        {
            writer.WriteStartElement("ROOT");
            foreach (ConstantData constant in this.ConstantList.Values)
            {
                constant.WriteTo(writer);
            }
            writer.WriteEndElement();
        }
    }
}
