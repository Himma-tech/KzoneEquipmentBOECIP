using Spring.Context;
using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using System.Xml;

namespace KZONE.MessageManager
{
    public class MessageManager : IMessageManager, IApplicationContextAware
    {
        private const int INTERVAL = 10000;

        private IApplicationContext _applicationContext;

        private string _configFileName;

        private Dictionary<string, List<Partaker>> _messageMapping;

        private MessageConfiguration _configuration;

        private bool _stopFlag;

        private double _concurrentCount;

        private double _avgconcurrentCount;

        private double _maxConcurrentCount;

        private Timer _concurrentTimer;

        private object _syncObject = new object();

        public Dictionary<string, List<Partaker>> MessageMapping
        {
            get
            {
                return this._messageMapping;
            }
            set
            {
                this._messageMapping = value;
            }
        }

        public double AvgConcurrentCount
        {
            get
            {
                return this._avgconcurrentCount;
            }
        }

        public double MaxConcurrentCount
        {
            get
            {
                return this._maxConcurrentCount;
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

        public IConfiguration Configuration
        {
            get
            {
                return this._configuration;
            }
        }

        public IApplicationContext ApplicationContext
        {
            set
            {
                this._applicationContext = value;
            }
        }

        public void Init()
        {
            this._configuration = new MessageConfiguration(this._configFileName);
            this._configuration.Init();
            XmlDocument doc = this._configuration.Document;
            IObjectFactory factory = this._applicationContext;
            Dictionary<string, List<Partaker>> mapping = new Dictionary<string, List<Partaker>>();
           
            doc.Load("..\\Config\\MessageMapping\\Common_Message.xml");
            XmlNode root = doc.DocumentElement;
           
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    this.RetrieveMessageMap(child, factory, mapping);
                }
                else if (child.NodeType == XmlNodeType.EntityReference)
                {

                    int i = 0;
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            if (i != 0)
                            {
                                this.RetrieveMessageMap(node, factory, mapping);
                                i++;
                            }
                        }
                    }
                }
            }
            this._messageMapping = mapping;
            this._concurrentTimer = new Timer(10000.0);
            this._concurrentTimer.AutoReset = true;
            this._concurrentTimer.Elapsed += new ElapsedEventHandler(this._concurrentTimer_Elapsed);
            this._concurrentTimer.Start();
        }

        private void _concurrentTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                double count = this._concurrentCount / 10000.0;
                if (count > this._maxConcurrentCount)
                {
                    this._maxConcurrentCount = count;
                }
                if (count != 0.0)
                {
                    this._avgconcurrentCount = count;
                }
                lock (this._syncObject)
                {
                    this._concurrentCount = 0.0;
                }
            }
            catch
            {
            }
        }

        private void RetrieveMessageMap(XmlNode parentNode, IObjectFactory factory, Dictionary<string, List<Partaker>> mapping)
        {
            string msg = parentNode.Attributes["name"].Value;
            if (parentNode.HasChildNodes)
            {
                List<Partaker> partakers = new List<Partaker>();
                foreach (XmlNode subNode in parentNode.ChildNodes)
                {
                    Partaker hi = new Partaker(subNode.Attributes["id"].Value, subNode.Attributes["method"].Value);
                    IService handler = factory.GetObject(hi.ObjectId) as IService;
                    hi.Service = handler;
                    partakers.Add(hi);
                }
                try
                {
                    mapping.Add(msg, partakers);
                    return;
                }
                catch (Exception)
                {
                    throw new KZONEException(string.Format("message=[{0}] is Add Error .", msg));
                }
            }
            throw new KZONEException(string.Format("Message=[{0}] is not setting handler information.", msg));
        }

        public void MessageDispatch(string name, object[] parameters)
        {
            lock (this._syncObject)
            {
                this._concurrentCount += 1.0;
            }
            if (!this._stopFlag)
            {
                if (this._messageMapping.ContainsKey(name))
                {
                    using (List<Partaker>.Enumerator enumerator = this._messageMapping[name].GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Partaker hi = enumerator.Current;
                            if (!hi.BeginInvoke(parameters))
                            {
                                throw new KZONEException(string.Format("Message {0},objectID {1},Method {2}, Queue User Work Item Error.", name, hi.ObjectId, hi.MethodName));
                            }
                        }
                        return;
                    }  
                }
                return;
            }
            goto IL_A9;
        IL_A9:
            throw new KZONEException("Message Dispatch is Stop!");
        }

        public void Invoke(string name, object[] parameters)
        {
            if (this._messageMapping.ContainsKey(name))
            {
                foreach (Partaker hi in this._messageMapping[name])
                {
                    hi.Invoke(parameters);
                }
            }
        }

        public void Reload()
        {
            MessageConfiguration config = new MessageConfiguration(this._configFileName);
            config.Init();
            XmlDocument doc = config.Document;
            IObjectFactory factory = this._applicationContext;
            Dictionary<string, List<Partaker>> mapping = new Dictionary<string, List<Partaker>>();
            XmlNode root = doc.DocumentElement;
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    this.RetrieveMessageMap(child, factory, mapping);
                }
                else if (child.NodeType == XmlNodeType.EntityReference)
                {
                    foreach (XmlNode node in child.ChildNodes)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            this.RetrieveMessageMap(node, factory, mapping);
                        }
                    }
                }
            }
            this.Stop();
            this._messageMapping = mapping;
            this._configuration = config;
        }

        public bool IsEnabled(string name)
        {
            if (this._messageMapping.ContainsKey(name))
            {
                using (List<Partaker>.Enumerator enumerator = this._messageMapping[name].GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        return false;
                    }
                    Partaker hi = enumerator.Current;
                    return hi.IsEnabled;
                }
            }
            throw new KZONEException(string.Format("Not found Message {0}.", name));
        }

        public void Enable(string name)
        {
            if (this._messageMapping.ContainsKey(name))
            {
                foreach (Partaker hi in this._messageMapping[name])
                {
                    hi.Enable();
                }
            }
        }

        public void Disable(string name)
        {
            if (this._messageMapping.ContainsKey(name))
            {
                foreach (Partaker hi in this._messageMapping[name])
                {
                    hi.Disable();
                }
            }
        }

        public void Stop()
        {
            this._stopFlag = true;
        }

        public DataTable GetMessageMap(DataTable table)
        {
            try
            {
                foreach (string key in this._messageMapping.Keys)
                {
                    foreach (Partaker hi in this._messageMapping[key])
                    {
                        DataRow row = table.NewRow();
                        row["Message"] = key;
                        row["Service"] = hi.ObjectId;
                        row["Method"] = hi.MethodName;
                        row["State"] = hi.IsEnabled;
                        row["Count"] = hi.RunCount;
                        row["Minimun"] = hi.Minimum;
                        row["Maximun"] = hi.Maximum;
                        table.Rows.Add(row);
                    }
                }
            }
            catch
            {
            }
            return table;
        }
    }
}
