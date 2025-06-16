using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace KZONE.ConstantParameter
{
    public class ParameterManager
    {
        private IDictionary<string, ParameterData> _parameters = new Dictionary<string, ParameterData>();

        private string _configFileName;

        public IDictionary<string, ParameterData> Parameters
        {
            get
            {
                return this._parameters;
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

        public ParameterData this[string key]
        {
            get
            {
                string uKey = key.ToUpper();
                if (this.Parameters.ContainsKey(uKey))
                {
                    return this.Parameters[uKey];
                }
                throw new Exception(string.Format("Not Found Parameter key {0}.", key));
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return this.Parameters.Keys;
            }
        }

        public IEnumerable<ParameterData> Values
        {
            get
            {
                return this.Parameters.Values;
            }
        }

        public void Init()
        {
            if (string.IsNullOrEmpty(this.ConfigFileName))
            {
                throw new Exception("Parameter Mananger Config File is Empty!");
            }
            if (!File.Exists(this.ConfigFileName))
            {
                throw new Exception(string.Format("Not found Parameter Config file {0}", this.ConfigFileName));
            }
            this.LoadConfigFile();
        }

        private void LoadConfigFile()
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.ConfigFileName);
            XmlNode root = document.DocumentElement;
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    this.RetrieveParameterMap(child);
                }
            }
        }

        private void RetrieveParameterMap(XmlNode parentNode)
        {
            string name = parentNode.Attributes["name"].Value;
            ParameterData variable = new ParameterData(name.ToUpper());
            if (this.ExistAttribute(parentNode, "value"))
            {
                variable.Value = parentNode.Attributes["value"].Value;
            }
            if (this.ExistAttribute(parentNode, "discription"))
            {
                variable.Discription = parentNode.Attributes["discription"].Value;
            }
            if (this.ExistAttribute(parentNode, "type"))
            {
                string type = parentNode.Attributes["type"].Value;
                string a;
                if ((a = type) != null)
                {
                    if (a == "bool")
                    {
                        variable.DataType = "bool";
                        goto IL_FC;
                    }
                    if (a == "integer")
                    {
                        variable.DataType = "integer";
                        goto IL_FC;
                    }
                    if (a == "string")
                    {
                        variable.DataType = "string";
                        goto IL_FC;
                    }
                }
                variable.DataType = "string";
            }
        IL_FC:
            if (variable.Name == "OPIMAXCOUNT")
            {
                int max_count = 0;
                if (!int.TryParse(variable.Value.ToString(), out max_count) || max_count < 6 || max_count > 20)
                {
                    variable.Value = "6";
                }
            }
            this._parameters.Add(variable.Name, variable);
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
            string uKey = key.ToUpper();
            return this.Parameters.ContainsKey(uKey);
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
            writer.WriteStartElement("parameters");
            foreach (ParameterData variable in this._parameters.Values)
            {
                variable.WriteTo(writer);
            }
            writer.WriteEndElement();
        }
    }
}
