using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;
using KZONE.Log;

namespace KZONE.EntityManager
{
    public class PalletManager : EntityManager, IDataSource
    {
        private Dictionary<string, Pallet> _entities = new Dictionary<string, Pallet>();

        public override EntityManager.FILE_TYPE GetFileType()
        {
            return FILE_TYPE.BIN;
        }

        protected override string GetSelectHQL()
        {
            return string.Empty;
        }

        protected override void AfterSelectDB(List<EntityData> EntityDatas, string FilePath, out List<string> Filenames)
        {
            Filenames = new List<string>();
            Filenames.Add("*.bin");
        }


        protected override Type GetTypeOfEntityData()
        {
            return null;
        }

        protected override Type GetTypeOfEntityFile()
        {
            return typeof(PalletEntityFile);
        }

        protected override EntityFile NewEntityFile(string Filename)
        {
            return new PalletEntityFile();
        }
        protected override void AfterInit(List<EntityData> entityDatas,List<EntityFile> entityFiles)
        {
            foreach (EntityFile entity_file in entityFiles)
            {
                PalletEntityFile pallet_entity_file = entity_file as PalletEntityFile;
                if (pallet_entity_file != null)
                {
                    string fextname = pallet_entity_file.GetFilename();
                    string fname = Path.GetFileNameWithoutExtension(fextname);
                    if (string.Compare(pallet_entity_file.PalletID, fname, true) == 0)
                        _entities.Add(pallet_entity_file.PalletID, new Pallet(pallet_entity_file));
                }
            }
        }

        //public Pallet GetPallet(string palletID)
        //{
        //    Pallet ret = null;
        //    foreach (Pallet entity in _entities.Values)
        //    {
        //        if (entity.File.PalletID == palletID)
        //        {
        //            ret = entity;
        //            break;
        //        }
        //    }
        //    return ret;
        //}

        public Pallet GetPalletByID(string palletID)
        {
            Pallet ret = null;
            foreach (Pallet entity in _entities.Values)
            {
                if (entity.File.PalletID == palletID)
                {
                    ret = entity;
                    break;
                }
            }
            return ret;
        }

        public Pallet GetPalletByNo(string palletNo)
        {
            Pallet ret = null;
            foreach (Pallet entity in _entities.Values)
            {
                if (entity.File.PalletNo == palletNo)
                {
                    ret = entity;
                    break;
                }
            }
            return ret;
        }

        public IList<Pallet> GetPallets()
        {
            IList<Pallet> ret = new List<Pallet>();
            foreach (Pallet entity in _entities.Values)
            {
                ret.Add(entity);
            }
            return ret;
        }

        public void AddPallet(Pallet pallet)
        {
            try
            {
                if (_entities.ContainsKey(pallet.File.PalletNo))
                {
                    Pallet obj = _entities[pallet.File.PalletNo];
                    obj.File.WriteFlag = false;
                    EnqueueSave(obj.File);
                    _entities.Remove(pallet.File.PalletNo);
                }

                //Jun Add 20150304 如果Pallet No重覆，也只留下一個
                List<string> delList = new List<string>();
                foreach (Pallet obj in _entities.Values)
                {
                    if (obj.File.PalletID == pallet.File.PalletID)
                    {
                        obj.File.WriteFlag = false;
                        EnqueueSave(obj.File);
                        delList.Add(obj.File.PalletNo);
                        //_entities.Remove(obj.File.PalletID);
                    }
                }
                //Jun Add 20150404 Foreach裡面不能進行Dictionary的Remove，改用List接再Remove
                foreach (string palletNo in delList)
                {
                    if (_entities.ContainsKey(palletNo))
                        _entities.Remove(palletNo);
                }

                _entities.Add(pallet.File.PalletNo, pallet);
                EnqueueSave(pallet.File);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        public void DeletePallet(string palletID)
        {
            if (_entities.ContainsKey(palletID))
            {
                //Jun Modify 20150404 修正不將Pallet Object刪除，如果把Pallet Object刪除OPI會無法顯示
                Pallet pallet = _entities[palletID];
                pallet.File.PalletMode = ePalletMode.UNKNOWN;
                pallet.File.PalletDataRequest = "0";
                pallet.File.DenseBoxList = new List<string>();
                pallet.File.LineRecipeName = "";
                pallet.File.Mes_ValidatePalletReply = "";
                pallet.File.PalletID = "";
                //_entities.Remove(palletID);
                //pallet.File.WriteFlag = false;
                EnqueueSave(pallet.File);
            } 
        }

        public override void EnqueueSave(EntityFile file)
        {
            if (file is PalletEntityFile)
            {
                PalletEntityFile pallet_entity_file = file as PalletEntityFile;
                string fname = string.Format("{0}.bin", pallet_entity_file.PalletNo);
                file.SetFilename(fname);
                base.EnqueueSave(file);
            }
        }

        public IList<string> GetEntityNames()
        {
            IList<string> entityNames = new List<string>();
            entityNames.Add("PalletManager");
            return entityNames;
        }

        public System.Data.DataTable GetDataTable(string entityName)
        {
            try
            {
                DataTable dt = new DataTable();
                PalletEntityFile file = new PalletEntityFile();
                DataTableHelp.DataTableAppendColumn(file, dt);

                //IList<Pallet> pallets = GetPallets();
                foreach (Pallet pallet in _entities.Values)
                {
                    DataRow dr = dt.NewRow();
                    DataTableHelp.DataRowAssignValue(pallet.File, dr);
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
