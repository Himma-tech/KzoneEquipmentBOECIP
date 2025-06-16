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
    public class MaterialManager : EntityManager, IDataSource
    {
        private Dictionary<string, MaterialEntity> _entities = new Dictionary<string, MaterialEntity>(); //NodeNo+UnitNo+MaterialPort+MaterialID

        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.XML;
        }
        protected override string GetSelectHQL()
        {
            return string.Empty;
        }
        protected override Type GetTypeOfEntityData()
        {
            return null;
        }
        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            Filenames.Add("*.xml");
        }
        protected override Type GetTypeOfEntityFile()
        {
            return typeof(MaterialEntity);
        }
        protected override EntityFile NewEntityFile(string Filename)
        {
            return new MaterialEntity();
        }
        protected override void AfterInit(List<EntityData> entityDatas, List<EntityFile> entityFiles)
        {
            foreach (EntityFile entity_file in entityFiles)
            {
                MaterialEntity material_entity = entity_file as MaterialEntity;
                if (material_entity != null)
                {
                    string material_key = string.Format("{0}_{1}_{2}_{3}", material_entity.NodeNo, material_entity.UnitNo, material_entity.MaterialId, material_entity.UpdateTime.ToString("yyyyMMddHHmmss"));
                    if (!_entities.ContainsKey(material_key))
                    {
                        _entities.Add(material_key, material_entity);
                    }

                }
            }
        }

        /// <summary>
        /// 將Material加入Dictionary
        /// Mount Material
        /// </summary>
        /// <param name="Job">若Unit No有重複, 會覆蓋</param>
        public void AddMaterial(MaterialEntity material)
        {
            lock (_entities)
            {
                //添加時判斷，刪除31天前的記錄
                for (int i = 0; i < _entities.Count; i++)
                {
                    string key = _entities.ElementAt(i).Key;
                    if (_entities[key].UpdateTime.AddDays(31) < DateTime.Now)
                    {
                        DeleteMaterial(_entities[key]);
                    }
                }

                string materialKey = string.Format("{0}_{1}_{2}_{3}", material.NodeNo, material.UnitNo, material.MaterialId, material.UpdateTime.ToString("yyyyMMddHHmmss"));
                if (!_entities.ContainsKey(materialKey))
                {
                    _entities.Add(materialKey, material);
                }
                else
                {
                    _entities.Remove(materialKey);
                    _entities.Add(materialKey, material);
                }
                material.SetFilename(string.Format("{0}.xml", materialKey));
                EnqueueSave(material);
            }
        }

        /// <summary>
        /// 將Material從Dictionary移除
        /// </summary>
        /// <param name="material"></param>
        public void DeleteMaterial(MaterialEntity material)
        {
            lock (_entities)
            {
                string materialKey = string.Format("{0}_{1}_{2}_{3}", material.NodeNo, material.UnitNo, material.MaterialId, material.UpdateTime.ToString("yyyyMMddHHmmss"));
                if (_entities.ContainsKey(materialKey))
                {
                    material.WriteFlag = false;
                    _entities.Remove(materialKey);
                    EnqueueSave(material);
                }
            }
        }

        /// <summary>
        /// 取得line Material 以List 方式傳回
        /// </summary>
        /// <returns>Job List</returns>
        public List<MaterialEntity> GetMaterials()
        {
            List<MaterialEntity> ret = null;
            lock (_entities)
            {
                ret = _entities.Values.ToList();
            }
            return ret;
        }




        public MaterialEntity GetMaterialByID(string eqpNo, string name)
        {
            try
            {
                var res = _entities.OrderByDescending(x => x.Value.UpdateTime).ToList();
                foreach (KeyValuePair<string, MaterialEntity> kvp in res)
                {
                    if (kvp.Value.NodeNo.ToUpper() == eqpNo.ToUpper() && kvp.Value.MaterialId == name)
                        return kvp.Value;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }


        public MaterialEntity GetMaterialByKey(string key)
        {
            try
            {
                if (this._entities.ContainsKey(key))
                {
                    return this._entities[key];
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return null;
            }
        }

        /// <summary>
        ///將Material從Dictionary去掉
        /// UnMount Material
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public bool UnMountMaterial(MaterialEntity material)
        {
            try
            {
                string materialKey = string.Format("{0}_{1}_{2}", material.NodeNo, material.UnitNo, material.MaterialId);
                if (_entities.ContainsKey(materialKey))
                {
                    _entities.Remove(materialKey);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }


        public bool UnMountMaterial(string eqpNo, string unitNo, string materialID)
        {
            try
            {
                string materialKey = string.Format("{0}_{1}_{2}", eqpNo, unitNo, materialID);
                if (_entities.ContainsKey(materialKey))
                {
                    _entities.Remove(materialKey);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }


        /// <summary>
        /// MaterialHistory 
        /// </summary>
        /// <param name="equipmentID"></param>
        /// <param name="unitNo"></param>
        /// <param name="currentMaterial"></param>
        /// <param name="oldMaterialId"></param>
        public void SaveMaterialHistory(string equipmentID, string unitNo, MaterialEntity currentMaterial, string oldMaterialId, string fileName = "", string count = "0")
        {
            try
            {
                MaterialHistory materialHistory = new MaterialHistory()
                {
                    UPDATETIME = DateTime.Now,
                    NODEID = equipmentID,
                    UNITNO = unitNo,
                    MATERIALID = currentMaterial.MaterialId,
                    MATERIALCOUNT = count,
                    MATERIALSTATUS = currentMaterial.MaterialStatus.ToString(),
                    MATERIALTYPE = currentMaterial.MaterialType.ToString(),
                    EVENT = currentMaterial.Event.ToString(),
                    OPERATORID = currentMaterial.OperatorID,
                    FILENAME = fileName,
                    OLDMATERIALID = oldMaterialId
                };
                this.HistoryHibernateAdapter.SaveObject(materialHistory);
            }
            catch (System.Exception ex)
            {
                Log.NLogManager.Logger.LogErrorWrite(this.LoggerName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("MaterialManager");

            return entityNames;
        }




        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                MaterialEntity file = new MaterialEntity();
                DataTableHelp.DataTableAppendColumn(file, dt);

                List<MaterialEntity> materials = GetMaterials();
                foreach (MaterialEntity entity in materials)
                {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(entity, dr);
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
    }
}
