using Spring.Context;
using Spring.Context.Support;
using Spring.Objects;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml;
using KZONE.UI;
using KZONE.Log;
using KZONE.MessageManager;



namespace KZONE.Work
{
    public class Workbench
    {
        private static Workbench _workbench;

        private static string _version;

        private static string _serverName;

        private static string _lineType;

        private static string _closeReson;

        private MDIForm _mainForm;

        private static eWorkbenchState _state = eWorkbenchState.ERROR;

        private static ILogManager _logger = NLogManager.Logger;

        private DefaultListableObjectFactory _objectFactory;

        private IDictionary<string, IServerAgent> _agentList;

        public static WorkbenchInitialStatusEventHandler WorkbenchInitialStatus;

        public static EventHandler WorkbenchClosing;

        public static IApplicationContext _applicationContext;

        public static Workbench Instance
        {
            get
            {
                if (Workbench._workbench == null)
                {
                    Workbench._workbench = new Workbench();
                }
                return Workbench._workbench;
            }
        }

        public static string CloseReson
        {
            get
            {
                return Workbench._closeReson;
            }
            set
            {
                Workbench._closeReson = value;
            }
        }

        public static eWorkbenchState State
        {
            get
            {
                return Workbench._state;
            }
            internal set
            {
                Workbench._state = value;
            }
        }

        public DefaultListableObjectFactory ObjectFactory
        {
            get
            {
                return this._objectFactory;
            }
            set
            {
                this._objectFactory = value;
            }
        }

        public IDictionary<string, IServerAgent> AgentList
        {
            get
            {
                return this._agentList;
            }
        }

        public static string Version
        {
            get
            {
                return Workbench._version;
            }
            set
            {
                Workbench._version = value;
            }
        }

        public static string ServerName
        {
            get
            {
                return Workbench._serverName;
            }
            set
            {
                Workbench._serverName = value;
            }
        }

        public static string LineType
        {
            get
            {
                return Workbench._lineType;
            }
            set
            {
                Workbench._lineType = value;
            }
        }

        public MDIForm MainForm
        {
            get
            {
                return this._mainForm;
            }
        }

        private Workbench()
        {
        }

        public bool Init(string fileName)
        {
            Workbench.State = eWorkbenchState.INIT;
            if (Workbench.WorkbenchInitialStatus != null)
            {
                Workbench.WorkbenchInitialStatus("Workbench Initial Start.", true);
            }
            this._agentList = new Dictionary<string, IServerAgent>();
            if (this.LoadConfigFormFile(fileName))
            {
                return this.CreateMainForm();
            }
            Workbench.State = eWorkbenchState.ERROR;
            return false;
        }

        public void Run()
        {
            Workbench.State = eWorkbenchState.START;
            foreach (IServerAgent sa in this.AgentList.Values)
            {
                if (sa.IsEnabled)
                {
                    if (Workbench.WorkbenchInitialStatus != null)
                    {
                        Workbench.WorkbenchInitialStatus(string.Format("Workbench Run {0} Agent.", sa.Name), true);
                    }
                    sa.Start();
                }
            }
            Workbench.State = eWorkbenchState.RUN;
        }

        public  void Shutdown()
        {
            Workbench.State = eWorkbenchState.STOP;
            if (this.AgentList != null)
            {
                foreach (IServerAgent sa in this.AgentList.Values)
                {
                    if (sa.IsEnabled)
                    {
                        sa.Destroy();
                    }
                }
            }
            Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Shutdown KZONE System ServerName {0} Ver {1} Reason {2}.", Workbench.ServerName, Workbench.Version, Workbench.CloseReson));
            Workbench._applicationContext.Dispose();
        }

        public bool LoadConfigFormFile(string fileName)
        {
            XmlDocument document = new XmlDocument();
            try
            {
                if (!File.Exists(fileName))
                {
                    bool result = false;
                    return result;
                }
                document.Load(fileName.Trim());
                if (document == null)
                {
                    bool result = false;
                    return result;
                }
                string xpath = "//framework/object-definition";
                int count = document.SelectSingleNode(xpath).ChildNodes.Count;
                if (count == 0)
                {
                    Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Not fount object definition in document");
                    bool result = false;
                    return result;
                }
                IList<string> list = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    if (document.SelectSingleNode(xpath).ChildNodes[i].NodeType == XmlNodeType.Element)
                    {
                        string innerText = document.SelectSingleNode(xpath).ChildNodes[i].InnerText;
                        if (innerText != null && innerText != "")
                        {
                            list.Add(innerText);
                        }
                    }
                }
                if (Workbench.WorkbenchInitialStatus != null)
                {
                    Workbench.WorkbenchInitialStatus("Workbench Load Starup.xml finish.\nWorkbench Initial Spring framework.", true);
                }
                Workbench.Version = document.SelectNodes("//version")[0].InnerText;
                Workbench.ServerName = document.SelectNodes("//servername")[0].InnerText;
                Workbench.LineType = document.SelectNodes("//linetype")[0].InnerText;
                string[] array = new string[list.Count];
                list.CopyTo(array, 0);
                Workbench._applicationContext = new XmlApplicationContext(array);
                IObjectFactory objectFactory = Workbench._applicationContext;
                IConfigurableApplicationContext context = (IConfigurableApplicationContext)Workbench._applicationContext;
                this.ObjectFactory = new DefaultListableObjectFactory(context.ObjectFactory);
                this.ObjectDefinitionMapping(context.ObjectFactory);
                xpath = "//framework/agentlist";
                count = document.SelectSingleNode(xpath).ChildNodes.Count;
                if (count == 0)
                {
                    Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Not fount proxy in document");
                    bool result = false;
                    return result;
                }
                for (int j = 0; j < count; j++)
                {
                    if (document.SelectSingleNode(xpath).ChildNodes[j].NodeType == XmlNodeType.Element)
                    {
                        string innerText2 = document.SelectSingleNode(xpath).ChildNodes[j].InnerText;
                        if (innerText2 != null && innerText2 != "" && document.SelectSingleNode(xpath).ChildNodes[j].Attributes["enabled"].Value.ToUpper() == "TRUE")
                        {
                            IServerAgent serverAgent = objectFactory.GetObject(innerText2) as IServerAgent;
                            serverAgent.IsEnabled = true;
                            this.AgentList.Add(innerText2, serverAgent);
                        }
                    }
                }
                Workbench._logger.LogInfoWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Server Loading is Success");
            }
            catch (Exception exception)
            {
                Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Server Loading is Fail - " + exception.ToString());
                throw exception;
            }
            return true;
        }

        public bool CreateMainForm()
        {
            try
            {
                if (Workbench.WorkbenchInitialStatus != null)
                {
                    Workbench.WorkbenchInitialStatus("Workbench Load KZONE Main form.", true);
                }
                MDIForm form = this.GetObjectOfType(typeof(MDIForm)) as MDIForm;
                if (form == null)
                {
                    Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Create Main Form Error.");
                    bool result = false;
                    return result;
                }
                this._mainForm = form;
                this._mainForm.Initial(Workbench.ServerName, Workbench.Version);
                if (Workbench.WorkbenchInitialStatus != null)
                {
                    Workbench.WorkbenchInitialStatus(string.Format("{0} Ver {1}", Workbench.ServerName, Workbench.Version), false);
                }
            }
            catch (Exception ex)
            {
                Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Build Main Form is Fail - " + ex.Message + " " + ex.InnerException.Message);
                bool result = false;
                return result;
            }
            return true;
        }

        public string GetAssemblyVersion(string name)
        {
            try
            {
                Assembly assembly = this.ObjectFactory.GetObjectDefinition(name).ObjectType.Assembly;
                if (assembly != null)
                {
                    Version version = assembly.GetName().Version;
                    return string.Format("{0}.{1}.{2}.{3}", new object[]
					{
						version.Major,
						version.Minor,
						version.Build,
						version.Revision
					});
                }
            }
            catch (Exception exception)
            {
                Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "failed to Get Assembly Version [" + name + "]", exception);
            }
            return "";
        }

        public NameValueCollection GetObjectProperties(string name, IServerAgent serverAgent)
        {
            NameValueCollection values = null;
            try
            {
                if (serverAgent == null)
                {
                    return values;
                }
                ObjectWrapper wrapper = new ObjectWrapper(serverAgent);
                PropertyDescriptorCollection propertyDescriptors = wrapper.PropertyDescriptors;
                if (propertyDescriptors.Count > 0)
                {
                    values = new NameValueCollection();
                    for (int i = 0; i < propertyDescriptors.Count; i++)
                    {
                        string propertyName = propertyDescriptors[i].Name;
                        try
                        {
                            string str2 = Convert.ToString(wrapper.GetPropertyValue(propertyName));
                            if (str2 != null)
                            {
                                values[propertyName] = str2;
                            }
                            else
                            {
                                Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "property [ " + propertyName + " ] value is null");
                                values[propertyName] = "null";
                            }
                        }
                        catch (Exception exception2)
                        {
                            Exception exception = exception2;
                            Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Concat(new object[]
							{
								"failed to set obj property [",
								propertyName,
								"]",
								exception
							}));
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                Exception exception = exception3;
                Workbench._logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Concat(new object[]
				{
					"failed to get obj property [",
					name,
					"]",
					exception
				}));
            }
            return values;
        }

        public string GetObjectTypeName(string objName)
        {
            IObjectDefinition objectDefinition = this.ObjectFactory.GetObjectDefinition(objName);
            if (objectDefinition != null)
            {
                return objectDefinition.ObjectTypeName;
            }
            return "";
        }

        private void ObjectDefinitionMapping(IConfigurableListableObjectFactory cof)
        {
            try
            {
                string[] objectDefinitionNames = cof.GetObjectDefinitionNames();
                for (int i = 0; i < objectDefinitionNames.Length; i++)
                {
                    try
                    {
                        if (this.ObjectFactory.GetObjectDefinition(objectDefinitionNames[i]) == null)
                        {
                            this.ObjectFactory.RegisterObjectDefinition(objectDefinitionNames[i], cof.GetObjectDefinition(objectDefinitionNames[i].ToString(), true));
                            this.ObjectFactory.RegisterSingleton(objectDefinitionNames[i], cof.GetObject(objectDefinitionNames[i]));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public object GetObjectOfType(Type type)
        {
            try
            {
                IEnumerator enumerator = Workbench.Instance.ObjectFactory.GetObjectsOfType(type).Values.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    return enumerator.Current;
                }
            }
            catch (Exception exception)
            {
                NLogManager.Logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", exception);
            }
            return null;
        }

        public object GetObject(string objName)
        {
            IObjectFactory objectFactory = Workbench._applicationContext;
            return objectFactory.GetObject(objName);
        }

        public object Invoke(string className, string methodName, object[] @params)
        {
            try
            {
                object obj = this.GetObject(className);
                if (obj != null)
                {
                    Type[] typeList = this.GetParameterType(@params);
                    MethodInfo mi = obj.GetType().GetMethod(methodName, typeList);
                    object result;
                    if (mi == null)
                    {
                        result = null;
                        return result;
                    }
                    result = mi.Invoke(obj, @params);
                    return result;
                }
            }
            catch
            {
            }
            return null;
        }

        private Type[] GetParameterType(object[] param)
        {
            Type[] typeList = new Type[param.Length];
            for (int i = 0; i < param.Length; i++)
            {
                typeList[i] = param[i].GetType();
            }
            return typeList;
        }
    }
}
