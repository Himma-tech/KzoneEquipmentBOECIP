using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;
using KZONE.Log;

namespace KZONE.EntityManager
{
    public class PortManager : EntityManager, IDataSource
	{
		private Dictionary<string, Port> _entities = new Dictionary<string, Port>();

        private List<EntityData> _tempEntityDatas = null;

        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.BIN;
        }

        protected override string GetSelectHQL()
        {
            return string.Format("from PortEntityData where SERVERNAME = '{0}'", BcServerName);
        }

        protected override Type GetTypeOfEntityData()
        {
            return typeof(PortEntityData);
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            _tempEntityDatas = EntityDatas;
            foreach (EntityData entity_data in EntityDatas)
            {
                PortEntityData port_entity_data = entity_data as PortEntityData;
                if (port_entity_data != null)
                {
                    string file_name = string.Format("{0}.bin", port_entity_data.PORTID);
                    Filenames.Add(file_name);
                }
            }
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(PortEntityFile);
        }

        protected override EntityFile NewEntityFile(string Filename)
        {
            int maxCount = 56;
            string port_id = Filename.Substring(0, Filename.Length - ".bin".Length);
            foreach (PortEntityData data in _tempEntityDatas)
            {
                if (data.PORTID == port_id)
                {
                    if (data.MAXCOUNT > 0)
                        maxCount = data.MAXCOUNT;
                    else
                        maxCount = 120;
                    break;
                }
            }
            return new PortEntityFile(maxCount);
        }

        protected override void AfterInit(List<EntityData> entityDatas, List<EntityFile> entityFiles)
        {
            foreach (EntityData entity_data in entityDatas)
            {
                PortEntityData port_entity_data = entity_data as PortEntityData;
                if (port_entity_data != null)
                {
                    foreach (EntityFile entity_file in entityFiles)
                    {
                        PortEntityFile port_entity_file = entity_file as PortEntityFile;
                        if (port_entity_file != null)
                        {
                            //port_entity_file.RedimArrayJoExistenceSlot(port_entity_data.MAXCOUNT);//  重新分配ARRAY Job Existence
                            string fextname = port_entity_file.GetFilename();
                            string fname = Path.GetFileNameWithoutExtension(fextname);
                            if (string.Compare(port_entity_data.PORTID, fname, true) == 0)
                                _entities.Add(port_entity_data.PORTID, new Port(port_entity_data, port_entity_file));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get  Port by Equipnt ID ,Port No 
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="portNo"></param>
        /// <returns></returns>
		public Port GetPort(string eqpID, int portNo)
		{
			Port ret = null;
			foreach (Port entity in _entities.Values)
			{
                if (entity.Data.NODEID == eqpID && entity.Data.PORTNO == portNo)
				{
					ret = entity;
					break;
				}
			}
			return ret;
		}

        /// <summary>
        /// Get Ports by Equipment ID 
        /// </summary>
        /// <param name="eqpID"></param>
        /// <returns></returns>
        public List<Port> GetPorts(string eqpID)
        {
            List<Port> ret = new List<Port>();
            foreach (Port entity in _entities.Values)
            {
                if (entity.Data.NODEID == eqpID)
                {
                    ret.Add(entity);
                    //break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Get  Port by  Port ID 
        /// </summary>
        /// <param name="portID"></param>
        /// <returns></returns>
		public Port GetPort(string portID)
		{
			Port ret = null;
            if (_entities.ContainsKey(portID))
                ret = _entities[portID];
			return ret;
		}

        /// <summary>
        /// get Port by Equipment No and PortNo 
        /// </summary>
        /// <param name="eqpNo"></param>
        /// <param name="portNo"></param>
        /// <returns></returns>
        public Port GetPortByEqpNoAndPortNo(string eqpNo, int portNo) {
            Port ret = null;
            foreach (Port entity in _entities.Values) {
                if (entity.Data.NODENO == eqpNo && entity.Data.PORTNO == portNo) {
                    ret = entity;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Get Ports 
        /// </summary>
        /// <returns></returns>
		public List<Port> GetPorts()
		{
			List<Port> ret = new List<Port>();
			foreach (Port entity in _entities.Values)
			{
				ret.Add(entity);
			}
			return ret;
		}

        /// <summary>
        /// 以line去取ports
        /// </summary>
        /// <param name="lineID"></param>
        /// <returns></returns>
        public List<Port> GetPortsByLine(string lineID)
        {
            List<Port> ret = new List<Port>();
            foreach (Port entity in _entities.Values)
            {
                if (entity.Data.LINEID == lineID)
                {
                    ret.Add(entity);
                    //break;
                }
            }
            return ret;
        }

        public IList<string> GetEntityNames()
        {
            IList<string> entityName = new List<string>();
            entityName.Add("PortManager");
            return entityName;
        }

        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                PortEntityData data = new PortEntityData();
                PortEntityFile file = new PortEntityFile();
                DataTableHelp.DataTableAppendColumn(data, dt);
                DataTableHelp.DataTableAppendColumn(file, dt);

                List<Port> port_entities = GetPorts();
                foreach (Port entity in port_entities)
                {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(entity.Data, dr);
                    DataTableHelp.DataRowAssignValue(entity.File, dr);
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (System.Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
           
        }
        /// <summary>
        /// Save  Port History
        /// </summary>
        /// <param name="port"></param>
        public void SavePortHistory(Port port) {
            try{
                PortHistory his = new PortHistory();
                int temp=0;
                bool done=int.TryParse(port.File.CassetteSequenceNo,out temp);
                his.CASSETTESEQNO = temp;

                his.LINEID = port.Data.LINEID;
                his.NODEID = port.Data.NODEID;
                his.PORTENABLEMODE = port.File.EnableMode.ToString();
                his.PORTID = port.Data.PORTID;
                his.PORTTYPE = port.File.PortType.ToString();
                //his.PORTSTATUS = port.File.PortStatus.ToString();
                //his.CASSETTESTATUS = port.File.CassetteStatus.ToString();
                his.PORTNO = port.Data.PORTNO;
                his.PORTTRANSFERMODE = port.File.TransferMode.ToString();
                his.UPDATETIME = DateTime.Now;
                his.CASSETTEPORTSTATUS = port.File.PortCassetteStatus.ToString();
                his.CASSETTEID = port.File.CassetteID;
                //20170222 ODF  Unloader  Criterial No  Tom 
                his.CRITERIALNUMBER = port.File.CriterialNo;
                //20170224 Add Port History  Tom 
                his.PORTGROUPNO = port.File.GroupNo;
              //  his.PORTGRADE = port.File.PortGrade;
                his.SORTGRADE = port.File.SortULD_BcSetSortGrade;
                
                ObjectManager.PortManager.InsertHistory(his);

            }catch (System.Exception ex){
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
    }
}
