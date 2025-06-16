using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace KZONE.ConstantParameter
{
    internal class FileFormateSection
    {
        private string _name;

        private string _type;

        private string _fileName;

        private string _filePath;

        private string _title;

        private IList<FileFormatItem> items = new List<FileFormatItem>();

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        public string FilePath
        {
            get
            {
                return this._filePath;
            }
            set
            {
                if (value[value.Length - 1] != '\\')
                {
                    value += "\\";
                }
                this._filePath = value;
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
            }
        }

        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        internal IList<FileFormatItem> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        public FileFormatItem this[int index]
        {
            get
            {
                if (index > 0 && index < this.Items.Count)
                {
                    return this.Items[index];
                }
                return null;
            }
        }

        public FileFormateSection(string name, string type, string fileName, string filePath, string title)
        {
            this._name = name;
            this._type = type;
            this._fileName = fileName;
            this._filePath = filePath;
            this._title = title;
        }

        public FileFormateSection(string name, string type, string fileName, string filePath)
        {
            this._name = name;
            this._type = type;
            this._fileName = fileName;
            this._filePath = filePath;
        }

        public FileFormateSection(string name)
        {
            this._name = name;
        }

        public void SaveToFile(object obj, string subPath, bool isBL)
        {
            List<FileFormatItem> itemsClone = new List<FileFormatItem>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.items.Count; i++)
            {
                FileFormatItem item = this.items[i].Clone() as FileFormatItem;
                itemsClone.Add(item);
                if (!isBL)
                {
                    sb.Append(item.ToFormatString(obj));
                }
                else
                {
                    sb.AppendFormat("{0}\r\n", item.ToFormatString(obj));
                }
            }
            Regex regex = new Regex("\\{\\w*\\s+\\w*}|\\{\\w*}");
            MatchCollection matches = regex.Matches(this.FileName);
            string fileName = this.FileName;
            for (int ctr = 0; ctr <= matches.Count - 1; ctr++)
            {
                string key = matches[ctr].Value.Substring(1, matches[ctr].Value.Length - 1 - 1);
                for (int j = 0; j < itemsClone.Count; j++)
                {
                    if (key == itemsClone[j].Name)
                    {
                        fileName = fileName.Replace(matches[ctr].Value, itemsClone[j].Value.Trim());
                        break;
                    }
                }
            }
            string filePath = Path.Combine(this.FilePath, subPath);
            this.Save(filePath, fileName, sb.ToString());
        }

        private void Save(string path, string fileName, string context)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, fileName);
            File.WriteAllText(filePath, context);
        }

        public void DeleteFile(string subPath, string fileName)
        {
            string path = Path.Combine(this.FilePath, subPath);
            if (!Directory.Exists(path))
            {
                return;
            }
            string filePath = Path.Combine(path, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            string[] files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                Directory.Delete(path, true);
            }
        }

        public void MoveFile(string sourceSubPath, string targetPath, string fileName)
        {
            string sourceFile = Path.Combine(this.FilePath, sourceSubPath);
            if (!Directory.Exists(sourceFile))
            {
                return;
            }
            string descFile = Path.Combine(targetPath, fileName);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string file = Path.Combine(sourceFile, fileName);
            if (File.Exists(file))
            {
                File.Move(file, descFile);
            }
            string[] files = Directory.GetFiles(sourceFile);
            if (files.Length == 0)
            {
                Directory.Delete(sourceFile, true);
            }
        }

        public void CheckCreateTimeAndDelete(string subPath)
        {
            string path = Path.Combine(this.FilePath, subPath);
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i];
                string filePath = Path.Combine(path, fileName);
                if (DateTime.Now.Subtract(File.GetCreationTime(filePath)).Days > 10)
                {
                    this.DeleteFile(subPath, filePath);
                }
            }
        }

        public string GetFilename(object obj, string subPath)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.items.Count; i++)
            {
                sb.AppendFormat("{0}\r\n", this.items[i].ToFormatString(obj));
            }
            Regex regex = new Regex("\\{\\w*\\s+\\w*}|\\{\\w*}");
            MatchCollection matches = regex.Matches(this.FileName);
            string fileName = this.FileName;
            for (int ctr = 0; ctr <= matches.Count - 1; ctr++)
            {
                string key = matches[ctr].Value.Substring(1, matches[ctr].Value.Length - 1 - 1);
                for (int j = 0; j < this.items.Count; j++)
                {
                    if (key == this.items[j].Name)
                    {
                        fileName = fileName.Replace(matches[ctr].Value, this.items[j].Value.Trim());
                        break;
                    }
                }
            }
            string filePath = Path.Combine(this.FilePath, subPath);
            return Path.Combine(filePath, fileName);
        }

        public string GetFilename(string GlassChipMaskCutID, string subPath, bool isBL)
        {
            Regex regex = new Regex("\\{\\w*\\s+\\w*}|\\{\\w*}");
            MatchCollection matches = regex.Matches(this.FileName);
            string fileName = this.FileName;
            int ctr = 0;
            if (ctr <= matches.Count - 1)
            {
                matches[ctr].Value.Substring(1, matches[ctr].Value.Length - 1 - 1);
                fileName = fileName.Replace(matches[ctr].Value, GlassChipMaskCutID);
            }
            string filePath = Path.Combine(this.FilePath, subPath);
            return Path.Combine(filePath, fileName);
        }
    }
}
