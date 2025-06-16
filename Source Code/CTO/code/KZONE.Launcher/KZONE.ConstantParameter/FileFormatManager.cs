using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace KZONE.ConstantParameter
{
    public class FileFormatManager
    {
        private string _formatFilePath;

        private IDictionary<string, FileFormateSection> _formats = new Dictionary<string, FileFormateSection>();

        public string FormatFilePath
        {
            get
            {
                return this._formatFilePath;
            }
            set
            {
                this._formatFilePath = value;
            }
        }

        public void Init()
        {
            if (string.IsNullOrEmpty(this._formatFilePath))
            {
                throw new Exception("File Format Mananger format File is Empty!");
            }
            if (!File.Exists(this._formatFilePath))
            {
                throw new Exception(string.Format("Not found Parameter Config file {0}", this.FormatFilePath));
            }
            this.LoadFormatFile();
        }

        private void LoadFormatFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this._formatFilePath);
            XmlNode root = doc.DocumentElement;
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    this.RetrieveFormatSection(child);
                }
            }
        }

        private void RetrieveFormatSection(XmlNode parentNode)
        {
            string name = parentNode.Attributes["name"].Value;
            FileFormateSection section = new FileFormateSection(name);
            section.FileName = parentNode.Attributes["fileName"].Value;
            section.FilePath = parentNode.Attributes["filePath"].Value;
            section.Type = parentNode.Attributes["type"].Value;
            if (this.ExistAttribute(parentNode, "title"))
            {
                section.Title = parentNode.Attributes["title"].Value;
            }
            if (parentNode.HasChildNodes)
            {
                foreach (XmlNode child in parentNode.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element)
                    {
                        FileFormatItem item = this.RetrieveFormatItem(child);
                        section.Items.Add(item);
                    }
                }
                lock (this._formats)
                {
                    this._formats.Add(section.Name, section);
                }
            }
        }

        private FileFormatItem RetrieveFormatItem(XmlNode parentNode)
        {
            string key = parentNode.Attributes["key"].Value;
            string name = parentNode.Attributes["name"].Value;
            FileFormatItem item = new FileFormatItem(key, name);
            item.Format = parentNode.Attributes["format"].Value;
            item.Property = parentNode.Attributes["property"].Value;
            if (this.ExistAttribute(parentNode, "length"))
            {
                item.Length = int.Parse(parentNode.Attributes["length"].Value);
            }
            return item;
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

        public void CreateFormatFile(string sectionid, string subPath, object obj, bool isBL)
        {
            if (!this._formats.ContainsKey(sectionid))
            {
                throw new Exception(string.Format("Can not found setion {0}", sectionid));
            }
            if (obj == null)
            {
                throw new Exception(string.Format("Object is null.", new object[0]));
            }
            FileFormateSection section = this._formats[sectionid];
            if (obj.GetType().Name != section.Type)
            {
                throw new Exception(string.Format("Type is mismatch {0}", section.Type));
            }
            section.SaveToFile(obj, subPath, isBL);
        }

        public void DeleteFormatFile(string sectionId, string subPath, string fileName)
        {
            if (!this._formats.ContainsKey(sectionId))
            {
                throw new Exception(string.Format("Can not found setion {0}", sectionId));
            }
            FileFormateSection section = this._formats[sectionId];
            section.DeleteFile(subPath, fileName);
        }

        public void MoveFormatFile(string sectionId, string sourceSubPath, string targetPath, string fileName)
        {
            if (!this._formats.ContainsKey(sectionId))
            {
                throw new Exception(string.Format("Can not found setion {0}", sectionId));
            }
            FileFormateSection section = this._formats[sectionId];
            section.MoveFile(sourceSubPath, targetPath, fileName);
        }

        public void DeleteFileByTime(string sectionId, string subPath)
        {
            if (!this._formats.ContainsKey(sectionId))
            {
                throw new Exception(string.Format("Can not found setion {0}", sectionId));
            }
            FileFormateSection section = this._formats[sectionId];
            section.CheckCreateTimeAndDelete(subPath);
        }

        public string GetFilename(string sectionid, string subPath, object obj)
        {
            if (!this._formats.ContainsKey(sectionid))
            {
                throw new Exception(string.Format("Can not found setion {0}", sectionid));
            }
            if (obj == null)
            {
                throw new Exception(string.Format("Object is null.", new object[0]));
            }
            FileFormateSection section = this._formats[sectionid];
            if (obj.GetType().Name != section.Type)
            {
                throw new Exception(string.Format("Type is mismatch {0}", section.Type));
            }
            return section.GetFilename(obj, subPath);
        }

        public string GetFilename(string sectionid, string subPath, string GlassChipMaskCutID)
        {
            if (!this._formats.ContainsKey(sectionid))
            {
                throw new Exception(string.Format("Can not found setion {0}", sectionid));
            }
            if (string.IsNullOrEmpty(GlassChipMaskCutID))
            {
                throw new Exception(string.Format("GlassChipMaskCutID is null or empty.", new object[0]));
            }
            FileFormateSection section = this._formats[sectionid];
            return section.GetFilename(GlassChipMaskCutID, subPath, false);
        }
    }
}
