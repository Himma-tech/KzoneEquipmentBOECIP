using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;
using KZONE.Log;
using System.Collections;

namespace KZONE.EntityManager
{
    public class UnitManager : EntityManager, IDataSource
	{
		private Dictionary<string, Unit> _entities = new Dictionary<string, Unit>();

        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.BIN;
        }

        protected override string GetSelectHQL()
        {
            return string.Format("from UnitEntityData where SERVERNAME = '{0}'", BcServerName);
        }

        protected override Type GetTypeOfEntityData()
        {
            return typeof(UnitEntityData);
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            foreach (EntityData entity_data in EntityDatas)
            {
                UnitEntityData unit_entity_data = entity_data as UnitEntityData;
                if (unit_entity_data != null)
                {
                    string file_name = string.Format("{0}.bin", unit_entity_data.UNITID);
                    Filenames.Add(file_name);
                }
            }
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(UnitEntityFile);
        }

        protected override EntityFile NewEntityFile(string Filename)
        {
            return new UnitEntityFile();
        }

        protected override void AfterInit(List<EntityData> entityDatas, List<EntityFile> entityFiles)
        {
            foreach (EntityData entity_data in entityDatas)
            {
                UnitEntityData unit_entity_data = entity_data as UnitEntityData;
                if (unit_entity_data != null)
                {
                    foreach (EntityFile entity_file in entityFiles)
                    {
                        UnitEntityFile unit_entity_file = entity_file as UnitEntityFile;
                        if (unit_entity_file != null)
                        {
                            string fextname = unit_entity_file.GetFilename();
                            string fname = Path.GetFileNameWithoutExtension(fextname);
                            if (string.Compare(unit_entity_data.UNITID, fname, true) == 0)
                                _entities.Add(unit_entity_data.UNITID, new Unit(unit_entity_data, unit_entity_file));
                        }
                    }
                }
            }
        }

        public Unit GetUnit(string eqpNo,int unitNo)
		{
			Unit ret = null;
			foreach (Unit entity in _entities.Values)
			{
                if (entity.Data.NODENO == eqpNo && entity.Data.UNITNO == unitNo)
				{
					ret = entity;
					break;
				}
			}
			return ret;
		}

        public Unit GetUnit(string unitID)
		{
            string _unitID = string.Empty;
			Unit ret = null;

            if (_entities.ContainsKey(unitID))
                ret = _entities[unitID];
			return ret;
		}

		public IList<Unit> GetUnits()
		{
			IList<Unit> ret = new List<Unit>();
			foreach (Unit entity in _entities.Values)
			{
				ret.Add(entity);
			}
			return ret;
		}

        public IList<Unit> GetUnitsByEQPNo(string eqpNo)
        {
            IList<Unit> ret = new List<Unit>();
            foreach (Unit entity in _entities.Values)
            {
                if (entity.Data.NODENO == eqpNo)
                {
                    ret.Add(entity);
                }
            }
            return ret;
        }

        public IList<Unit> GetUnitsByEQPName(string eqpName)
        {
            IList<Unit> ret = new List<Unit>();
            foreach (Unit entity in _entities.Values)
            {
                if (entity.Data.NODEID == eqpName)
                {
                    ret.Add(entity);
                }
            }
            return ret;
        }

        /// <summary>
        /// Save Unit History 20150427 tom
        /// </summary>
        /// <param name="unit"></param>
        public void SaveUnitHistory(Unit unit)
        {
            try
            {
                // Save DB
                UnitHistory his = new UnitHistory()
                {
                    NODEID = unit.Data.NODEID,
                    NODENO = unit.Data.NODENO,
                    UNITNO = unit.Data.UNITNO,
                    UNITID = unit.Data.UNITID,
                    UNITSTATUS = unit.File.Status.ToString(),
                    UNITTYPE = unit.Data.UNITTYPE,
                    UNITRECIPEID = unit.File.UnitRecipeID
                };
                ObjectManager.UnitManager.InsertHistory(his);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite(LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
       
        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                UnitEntityData data = new UnitEntityData();
                UnitEntityFile file = new UnitEntityFile();
                DataTableHelp.DataTableAppendColumn(data, dt);
                DataTableHelp.DataTableAppendColumn(file, dt);

                IList<Unit> unit_entities = GetUnits();
                foreach (Unit entity in unit_entities)
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

        public IList<string> GetEntityNames()
        {
            IList<string> entityName = new List<string>();
            entityName.Add("UnitManager");
            return entityName;
        }
	}
}
