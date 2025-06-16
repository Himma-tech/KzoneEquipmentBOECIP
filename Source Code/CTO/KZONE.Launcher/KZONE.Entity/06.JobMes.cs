using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using KZONE.ConstantParameter;

namespace KZONE.Entity
{
    /// <summary>
    /// MES Verify  Lot
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
    public class LOT : ICloneable
    {
        public  string  LotId {get;set;}
        public  string SpltId {get;set;}
        public  string ProductCate{get;set;}
        public string ProductId{get;set;}
        public string ProductType {get;set;}
        public string EcCode{get;set;}
        public  string NxRouteId {get;set;}
        public string NxRouteVer{get;set;}
        public  string NxProcId{get;set;}
        public string NxOpeNo{get;set;}
        public string NxOpeVer{get;set;}
        public string NxOpeDsc {get;set;}
        public string NxOpeId {get;set;}
        public string NxPepLvl {get;set;}
        public string NxCrrSetCode {get;set;}
        public string Nx2RouteId { get; set; }
        public string Nx2RouteVer {get;set;}
        public string Nx2OpeNo { get; set; }
        public string PnlCnt {get;set;}
        public string RepUnit{get;set;}
        public string MesId {get;set;}
        public string UpLoadId{get;set;}
        public string DownLoadId{get;set;}
        public string UpEqptId {get;set;}
        public string UpRecipeId {get;set;}
        public string QrsRouteId {get;set;}
        public string QrsRouteVer {get;set;}
        public string QrsOpeId {get;set;}
        public string QrsDate {get;set;}
        public string QrsTime {get;set;}
        public string RwkCnt {get;set;}
        public string MaxRwkCnt {get;set;}
        public string Ppbody {get;set;}
        public string SpcCheckFlg {get;set;}
        public string SpecCheckFlg {get;set;}
        public string LogofEqptId {get;set;}
        public string LogofPortId {get;set;}
        public string LogofRecipeId {get;set;}
        public string PfcBankId {get;set;}
        public string SgrId {get;set;}
        public string OwnerId {get;set;}
        public string PfcFlg {get;set;}
        public string MtrlProductId {get;set;}
        public string MaxShtCnt {get;set;}
        public string BendingParm {get;set;}
        public string ShopId {get;set;}
        public string UpLoadEqptId { get; set; }
        public string UpLoadOpeNo { get; set; }
        public List<GradeGroup> GardeGroups { get; set; }
        public string RmsBypass { get; set; }
        public string Pnlr { get; set; }
        public string Pnln { get; set; }
        public string Glsi { get; set; }
        public string Glsp { get; set; }
        public string Pnli { get; set; }
        public string Pnlp { get; set; }
        public string ReturnRouteID { get; set; }
        public string ReturnRouteVer { get; set; }
        public string ReturnOpeNo { get; set; }
        //20170721 bruce.zhan
        #region 新增F1FRE&&F1IRP  ProductInformationRequestReportReplyData
        public string RA { get; set; }
        public string RB { get; set; }
        public string RC { get; set; }
        public string RD { get; set; }
        public string NA { get; set; }
        public string NB { get; set; }
        public string NC { get; set; }
        public string ND { get; set; }
        public string LAYOUT { get; set; }
        #endregion


        #region CELL LOT Special
        public string CrCrrSetCode { get; set; }
        public string Prty { get; set; }
        public string LoadPrty { get; set; }
        public string ShtCnt { get; set; }
        public string EdcDate { get; set; }
        public string PackMethod { get; set; }
        public string ChkGrpFlg { get; set; }
        public string GroupID { get; set; }
        public string CutFlag { get; set; }
        public string ProdCht { get; set; }
        public List<PpidOary> PpidOarys { get; set; }

        #region CELL Unloader LOT
        public string CrrStat { get; set; }
        public string ValidFlag { get; set; }
        public string CrrSetCode { get; set; }
        public string CrrMntRstDate { get; set;}
        public string ClupFlag { get; set; }
        public string PnlShtCnt { get; set; }
        public string ClupDate { get; set; }
        public string ClupTime {get ;  set;}
        public  string RealEmp { get; set; }
        public string RouteCate { get ; set; }
        public string RouteID { get; set; }
        public string RouteVer { get; set; }
        public string OpeNo { get; set; }
        public string OpeID {get ; set; }
        public string OpeVer { get; set; }
        public string OpeDsc {get; set; }
        public string DeptCode {get; set; }
        public string procID { get; set; }
        public string ProcDsc { get; set; }
        public string Priority { get; set; }
        public string EqptID { get; set; }
        public string EqptPortID { get; set; }
        public string EqptDsc { get; set; }
        public string PvEqptID { get; set; }
        public string PepLvl { get; set; }
        public string RecipeID { get; set; }
        public string PvOpeNo {get; set; }
        public string MprocID {get; set; }
        public string MprocFlag { get; set; }
        public string WipBankFlg { get; set; }
        public string DefWipBankID {get; set; }
        public string ShpBankFlag {get; set; }
        public string CrrgID { get; set; }
        public string CrrUseCnt { get; set; }
        public string MaxCrrUseCnt { get; set; }
        public string MaxUseOverFlag { get; set; }
        public string AreaCode { get; set; }
        public string XaxisCnt { get; set; }
        public string YaxisCnt { get; set; }
        public string GrsAryCnt { get; set; }
        public List<QtimeTable> QtimeTables { get; set; }
        public string MapFlag { get; set; }
        public string CrrOwn { get; set; }
        public string CrossFlag { get; set; }
        public string CrrRgstOwn { get; set; }
        #endregion
        #endregion

        public LOT()
        {
            LotId = string.Empty;
            SpltId = string.Empty;
            ProductCate = string.Empty;
            ProductId = string.Empty;
            ProductType = string.Empty;
            EcCode = string.Empty;
            NxRouteId = string.Empty;
            NxRouteVer = string.Empty;
            NxProcId = string.Empty;
            NxOpeNo = string.Empty;
            NxOpeVer = string.Empty;
            NxOpeDsc = string.Empty;
            NxOpeId = string.Empty;
            NxPepLvl = string.Empty;
            NxCrrSetCode = string.Empty;
            Nx2RouteVer = string.Empty;
            Nx2RouteId = string.Empty;
            Nx2OpeNo = string.Empty;
            GardeGroups = new List<GradeGroup>();
            CrCrrSetCode = string.Empty;
            Prty = string.Empty;
            LoadPrty = string.Empty;
            EdcDate = string.Empty;
            PackMethod = string.Empty;
            ChkGrpFlg = string.Empty;
            GroupID = string.Empty;
            CutFlag = string.Empty;
            ProdCht = string.Empty;
            PpidOarys = new List<PpidOary>();

            CrrStat = string.Empty;
            ValidFlag = string.Empty;
            CrrSetCode = string.Empty;
            CrrMntRstDate = string.Empty;
            ClupFlag = string.Empty;
            ClupDate = string.Empty;
            ClupTime = string.Empty;
            RealEmp = string.Empty;
            RouteCate = string.Empty;
            RouteID = string.Empty;
            RouteVer = string.Empty;
            OpeNo = string.Empty;
            OpeID = string.Empty;
            OpeVer = string.Empty;
            OpeDsc = string.Empty;
            DeptCode = string.Empty;
            procID = string.Empty;
            ProcDsc = string.Empty;
            Priority = string.Empty;
            EqptID = string.Empty;
            EqptPortID = string.Empty;
            EqptDsc = string.Empty;
            PvEqptID = string.Empty;
            PepLvl = string.Empty;
            RecipeID = string.Empty;
            PvOpeNo = string.Empty;
            MprocID = string.Empty;
            MprocFlag = string.Empty;
            WipBankFlg = string.Empty;
            DefWipBankID = string.Empty;
            ShpBankFlag = string.Empty;
            CrrgID = string.Empty;
            CrrUseCnt = string.Empty;
            MaxCrrUseCnt = string.Empty;
            MaxUseOverFlag = string.Empty;
            AreaCode = string.Empty;
            XaxisCnt = string.Empty;
            YaxisCnt = string.Empty;
            QtimeTables = new List<QtimeTable>();
            MapFlag = string.Empty;
            CrrOwn = string.Empty;
            CrossFlag = string.Empty;
            CrrRgstOwn = string.Empty;
            ReturnRouteID = string.Empty;//20170309 增加三个CST一致性条件
            ReturnRouteVer = string.Empty;
            ReturnOpeNo = string.Empty;
            OwnerId = string.Empty;//增加防呆 2016-12-7

            //20170721 BRUCE.ZHAN 
            RA = string.Empty;
            RB = string.Empty;
            RC = string.Empty;
            RD = string.Empty;
            NA = string.Empty;
            NB = string.Empty;
            NC = string.Empty;
            ND = string.Empty;
            LAYOUT = string.Empty;

        } 


        public object Clone()
        {
            LOT lot = (LOT)this.MemberwiseClone();
            lot.GardeGroups = new List<GradeGroup>();
            lot.GardeGroups.AddRange(this.GardeGroups);
            lot.PpidOarys = new List<PpidOary>();
            lot.PpidOarys.AddRange(this.PpidOarys);
            lot.QtimeTables = new List<QtimeTable>();
            lot.QtimeTables.AddRange(this.QtimeTables);
            return lot;
        }
    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Product:ICloneable
    {
        public string SlotPpid {get;set;}
        public string Nx2ShtRecipeId {get;set;}
        public string TitleFlag {get;set;}
        public string ProductId {get;set;}
        public string SgrId {get;set;}
        public string VryOpeProcFlg {get;set;}
        public string RwkCnt    {get;set;}
        public string MtrlGrade { get; set; }
        public string StbShop { get; set; }
        public string PnlShtCnt { get; set; }
        public string SubShtCnt { get; set; }
        public List<SubProduct> SubProducts { get; set; }
        public SerializableDictionary<string, string> AbnormalCodes { get; set; }
        public string SortGrade { get; set; }
        public string NGFlag {get;set;}

        #region CELL Product Special
        public string OpenCellLabel { get; set; }
        public string PiOverRwkFlag { get; set; }
        public string ProcPanel { get; set; }
        public string DestShop { get; set; }
        public string ShtRecipeID { get; set; }
        /// <summary>
        ///MES Download Judge(string. EX:G,N...). 0：Unused
        ///1：G, OK
        ///2：N, NG
        ///3：U, Gray
        ///4：V, Gray
        ///5：W, Gray
        ///6：P, Repair
        ///7：I, Ink Repair
        ///8：S, Scrap
        ///9：R, Absolute X
        ///10：A, Gray
        ///11~15 TBD
        /// </summary>
        public string ShtJudge { get; set; }
        public string PnlJudge { get; set; }
       
        //20161108 add ShtGrade
        public string ShtGrade { get; set; }

        public string PnlScpFlg { get; set; }

        #region CELL Unloader Product
        public string ShtExistFlag { get; set; }
        public string ShtStat { get; set; }
        public string ReprocFlag { get; set; }
        public string ShtNoteFlag { get; set; }
        public string XyDim { get; set; }
        public string PnlCnt { get; set; }
        public string RouteCate { get; set; }
        public string RouteID { get; set; }
        public string RouteVer { get; set; }
        public string OpeNo { get; set; }
        public string OpeID { get; set; }
        public string OpeVer { get; set; }
        public string OwnerID { get; set; }
        public string AbnCnt { get; set; }
        public SerializableDictionary<string, string> AbnormalFlags { get; set; }
        #endregion 

        #endregion
        public string PnlGrade { get; set; }
        public string NgMark { get; set; }
        public string GradeGroup { get; set; }
        public string InsCnt { get; set; }
        public string CoaterHis { get; set; }
        public string AlignerHis { get; set; }
        public string HpHis { get; set; }
        public string ProcessFlag { get; set; }
        public string InlineRwkCnt { get; set; }
        public string RgbRwkCnt { get; set; }
        public string ItoRwkCnt { get; set; }
        public string PsRwkCnt { get; set; }
        public string ChangerFlag { get; set; }
        public string OxInforamtion { get; set; }
        public string MesGlassID { get; set; }

        public Product() {
            SlotPpid = string.Empty;
            Nx2ShtRecipeId = string.Empty;
            TitleFlag = string.Empty;
            ProductId = string.Empty;
            SgrId = string.Empty;
            VryOpeProcFlg = string.Empty;
            RwkCnt = string.Empty;
            MtrlGrade = string.Empty;
            StbShop = string.Empty;
            PnlShtCnt = string.Empty;
            SubShtCnt = string.Empty;
            SortGrade = string.Empty;
            NGFlag = string.Empty;
            OpenCellLabel = string.Empty;
            PiOverRwkFlag = string.Empty;
            ProcPanel = string.Empty;
            DestShop = string.Empty;
            ShtRecipeID = string.Empty;
            ShtJudge = string.Empty;
            PnlJudge = string.Empty;
            ShtExistFlag = string.Empty;
            ShtStat = string.Empty;
            ReprocFlag = string.Empty;
            ShtNoteFlag = string.Empty;
            XyDim = string.Empty;
            PnlCnt = string.Empty;
            RouteCate = string.Empty;
            RouteID = string.Empty;
            RouteVer = string.Empty;
            OpeNo = string.Empty;
            OpeID = string.Empty;
            OpeVer = string.Empty;
            OwnerID = string.Empty;
            AbnCnt = string.Empty;
            //20161108 add ShtGrade
            ShtGrade = string.Empty;
            PnlGrade = string.Empty;
            NgMark = string.Empty;
            GradeGroup = string.Empty;
            InsCnt = string.Empty;
            CoaterHis = string.Empty;
            AlignerHis = string.Empty;
            HpHis = string.Empty;
            ProcessFlag = string.Empty;
            InlineRwkCnt = string.Empty;
            RgbRwkCnt = string.Empty;
            ItoRwkCnt = string.Empty;
            PsRwkCnt = string.Empty;
            ChangerFlag = string.Empty; // CF  Changer Mode  2017-02-21 tom 
            OxInforamtion = string.Empty;
            SubProducts = new List<SubProduct>();
            AbnormalCodes = new SerializableDictionary<string, string>();
            AbnormalFlags = new SerializableDictionary<string, string>();
            MesGlassID = string.Empty;
        }


        public object Clone() {
            Product p = (Product)this.MemberwiseClone();
            p.SubProducts = new List<SubProduct>();
            p.SubProducts.AddRange(this.SubProducts);
            p.AbnormalCodes = new SerializableDictionary<string, string>();
            foreach (string key in this.AbnormalCodes.Keys) {
                p.AbnormalCodes.Add(key, this.AbnormalCodes[key]);
            }
            p.AbnormalFlags = new SerializableDictionary<string, string>();
            foreach (string key in this.AbnormalFlags.Keys) {
                p.AbnormalFlags.Add(key, this.AbnormalFlags[key]);
            }
            return p;
        }
    }
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SubProduct
    {
        public string XyDim { get; set; }
        public string SubShtId { get; set; }
        public string CutProductID { get; set; }
        public string CutEcCode { get; set; }
        public bool IsCorss { get; set; }
        //20170612 add by MES Spec 1.49
        public string Abnormal_Flg_1 { get; set; }
    }
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GradeGroup 
    {
        //public string Gade { get; set; } 20160830 根据1.17 Spec 修改
        //public string TargetPortId {get;set;}
        public string SubEqptId { get; set; }
        public string SmpType { get; set; }
        public string SmpRate { get; set; }
        #region Cell ORRY4 
        public string AcProdID { get; set; }
        public string AcEqptID { get; set; }
        public string AcPpid { get; set; }
        public List<AcRecipeTable> RecipeTables { get; set; }
        public string AcDirection { get; set; }
        public string AcCrrSetCode { get; set; }

        #endregion 

      public GradeGroup()
        {
            RecipeTables = new List<AcRecipeTable>();
        }

      public object Clone()
        {
            GradeGroup gradeGroup = (GradeGroup)this.MemberwiseClone();
            gradeGroup.RecipeTables = new List<AcRecipeTable>();
            gradeGroup.RecipeTables.AddRange(this.RecipeTables);
            return gradeGroup;
        }
    }
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PpidOary 
    {
        public string Ppid { get; set; }
        public List<AcRecipeTable> PpidRecipeTables { get; set;}

        public PpidOary()
        {
            PpidRecipeTables = new List<AcRecipeTable>();

        }
        public object Clone()
        {
            PpidOary ppidOary = (PpidOary)this.MemberwiseClone();
            ppidOary.PpidRecipeTables = new List<AcRecipeTable>();
            ppidOary.PpidRecipeTables.AddRange(this.PpidRecipeTables);
            return ppidOary;
        }

    }
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcRecipeTable
    {
        public string SubEqptID { get; set; }
        public string SubRecipeID { get; set; }
        public string RunFlag { get; set; }
    }
     [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class QtimeTable
    {
        public string QrsOpeID { get; set; }
        public string QrsTyp { get; set; }
        public string QrsDate { get; set; }
        public string QrsTime { get; set; }
        public string QrkDate  { get; set; }
        public string QrkTime  { get; set; }
    }
}

