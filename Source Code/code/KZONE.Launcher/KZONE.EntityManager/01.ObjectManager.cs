///All Object Manager define

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KZONE.ConstantParameter;

namespace KZONE.EntityManager
{
    public class ObjectManager
    {
        public static LineManager LineManager { get; set; }
        public static EquipmentManager EquipmentManager { get; set; }
        public static PortManager PortManager { get; set; }
        //public static CassetteManager CassetteManager { get; set; }
        public static UnitManager UnitManager { get; set; }
        public static AlarmManager AlarmManager { get; set; }
        public static JobManager JobManager { get; set; }
        public static ProcessDataManager ProcessDataManager { get; set; }
        public static DailyCheckManager DailyCheckManager { get; set; }
        public static MaterialManager MaterialManager { get; set; }
        public static QtimeManager QtimeManager { get; set; }
        public static RecipeManager RecipeManager { get; set; }
        public static SubJobDataManager SubJobDataManager { get; set; }
        public static SubBlockManager SubBlockManager {get;set; }
        //public static PalletManager PalletManager { get; set; }
        public static PlanManager PlanManager { get; set; }
        public static GroupNoManager GroupNoManager { get; set; }
        public static ParameterManager ParameterManager { get; set; }
       // public static RobotManager RobotManager { get; set; }
        public static SamplingRuleManager SamplingRuleManager { get; set; }
    }
}
