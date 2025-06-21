using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Business.Base
{
    /// <summary>
    /// 业务接口
    /// </summary>
    public interface IBusiness
    {
        /// <summary>
        /// 部件初始化
        /// </summary>
        void InitComponent();

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();


        /// <summary>
        /// 1电芯入站
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
       // void BatteryForIn(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 2辅料入站
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void FeedingForIn(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 3电芯出站
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void BatteryForOut(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 4辅料上料
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void FeedingInRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 5托杯交换机出站触发
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void PEXuploadRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 6设备报修
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void RepairRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 7扫码枪
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void ScannerRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 8托杯交换
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void CupChangeRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 9喷码信息上传
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void PrintRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC);

        /// <summary>
        /// 10数据实时采集
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void CollectRealTimeData(PLCVariableModel plcVariableModel, IHimmaPLC PLC);

        ///// <summary>
        ///// 下料数据保存
        ///// </summary>
        ///// <param name="plcVariableModel"></param>
        ///// <param name="PLC"></param>
        ///// <returns></returns>
        //dynamic SaveBatteryOutData(PLCVariableModel plcVariableModel, IHimmaPLC PLC,string BatteryGuid);


        /// <summary>
        /// 11辅料下料
        /// </summary>
        /// <param name="plcVariableModel"></param>
        /// <param name="PLC"></param>
        /// <returns></returns>
        //void FeedingOutRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 12装盘机绑盘触发
        /// </summary>
        /// <param name="plcVariableModel"></param>
        /// <param name="PLC"></param>
        //void TrayloadBindRequest(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 13Hipot实时读取块触发
        /// </summary>
        /// <param name="plcVariableModel"></param>
        /// <param name="PLC"></param>
        //void HipotRealTimeRead(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 14Hipot操作读取块触发
        /// </summary>
        /// <param name="plcVariableModel"></param>
        /// <param name="PLC"></param>
        //void HipotHandleRead(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 15Hipot操作写入块触发
        /// </summary>
        /// <param name="plcVariableModel"></param>
        /// <param name="PLC"></param>
        //void HipotHandleWrite(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 16卷绕NG保存触发
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void WindNGForOut(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        /// <summary>
        /// 17卷绕蓝标触发
        /// </summary>
        /// <param name="pLCVariableModel"></param>
        /// <param name="PLC"></param>
        //void WindFlawCoordinate(PLCVariableModel plcVariableModel, IHimmaPLC PLC);
        string CreatBatteryGuid(int intLength);
    }
}