using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.UI
{
    public class StageEntity
    {
        #region DB
        public string ServerName { get; set; }
        public string LineID { get; set; }
        public string LocalNo { get; set; }    // L2,L3....
        public string NodeNo { get; set; }  //02,03...
        public string NodeID { get; set; }
        public string PortID { get; set; }
        public string StageName { get; set; }
        public int StagePosition { get; set; }
        public string RobotName { get; set; }
        //public UniAuto.UniBCS.HKC.OpiSpec.eStageType StageType { get; set; }
        public int SlotCount { get; set; }
        public int MaxSlot { get; set; }
        public bool ExistDB { get; set; } //是否存在於DB
        #endregion

        #region 
        public List<RobotSlotInfo> Lst_SlotInfo { get; set; }
        public string SecondVerify { get; set; }
        #endregion

        public StageEntity()
        {
            Lst_SlotInfo = new List<RobotSlotInfo>();
            SecondVerify = string.Empty;

            ExistDB = false;
        }

        #region

        //public void SetStageInfo(RobotStageInfoReport.STAGEc stageData)
        //{
        //    foreach (RobotStageInfoReport.SLOTc _slot in stageData.SLOTLIST)
        //    {
        //        int _slotNo = 0;
        //        int.TryParse(_slot.SLOT_NO, out _slotNo);

        //        bool _isNumber = false;
        //        RobotSlotInfo _info;

        //        if (_slotNo != 0)
        //        {
        //            _info = Lst_SlotInfo.Where(r => r.SlotNo.Equals(_slot.SLOT_NO.PadLeft(3, '0'))).FirstOrDefault();
        //            _isNumber = true;
        //        }
        //        else
        //        {
        //            _info = Lst_SlotInfo.Where(r => r.SlotNo.Equals(_slot.SLOT_NO)).FirstOrDefault();
        //            _isNumber = false;
        //        }

        //        lock (Lst_SlotInfo)
        //        {
        //            if (_info == null)
        //            {
        //                RobotSlotInfo _new = new RobotSlotInfo();
        //                _new.SlotNo = _isNumber ? _slot.SLOT_NO.PadLeft(3, '0') : _slot.SLOT_NO;
        //                _new.SlotStatus = _slot.RCS_SLOT_STATUS;
        //                _new.ProcessStatus = _slot.PROCESS_STATUS;
        //                _new.PresenceJobKeys = _slot.PRESENCE_JOB_KEY;
        //                _new.ReserveJobKeys = _slot.RESERVE_JOB_KEY;
        //                _new.PresenceRoute = _slot.PRESENCE_ROUTE;
        //                _new.PresenceLastStepID = _slot.PRESENCE_LAST_STEP_ID;
        //                _new.PresenceRealStepID = _slot.PRESENCE_REAL_STEP_ID;
        //                Lst_SlotInfo.Add(_new);
        //            }
        //            else
        //            {
        //                _info.SlotStatus = _slot.RCS_SLOT_STATUS;
        //                _info.ProcessStatus = _slot.PROCESS_STATUS;
        //                _info.PresenceJobKeys = _slot.PRESENCE_JOB_KEY;
        //                _info.ReserveJobKeys = _slot.RESERVE_JOB_KEY;
        //                _info.PresenceRoute = _slot.PRESENCE_ROUTE;
        //                _info.PresenceLastStepID = _slot.PRESENCE_LAST_STEP_ID;
        //                _info.PresenceRealStepID = _slot.PRESENCE_REAL_STEP_ID;
        //            }
        //        }
        //    }
        //}

        //public void SetStageInfo(RobotStageInfoRequestReply.STAGEc stageData)
        //{
        //    foreach (RobotStageInfoRequestReply.SLOTc _slot in stageData.SLOTLIST)
        //    {
        //        int _slotNo = 0;
        //        int.TryParse(_slot.SLOT_NO, out _slotNo);

        //        bool _isNumber = false;
        //        RobotSlotInfo _info;

        //        if (_slotNo != 0)
        //        {
        //            _info = Lst_SlotInfo.Where(r => r.SlotNo.Equals(_slot.SLOT_NO.PadLeft(3, '0'))).FirstOrDefault();
        //            _isNumber = true;
        //        }
        //        else
        //        {
        //            _info = Lst_SlotInfo.Where(r => r.SlotNo.Equals(_slot.SLOT_NO)).FirstOrDefault();
        //            _isNumber = false;
        //        }

        //        lock (Lst_SlotInfo)
        //        {
        //            if (_info == null)
        //            {
        //                RobotSlotInfo _new = new RobotSlotInfo();
        //                _new.SlotNo = _isNumber ? _slot.SLOT_NO.PadLeft(3, '0') : _slot.SLOT_NO;
        //                _new.SlotStatus = _slot.RCS_SLOT_STATUS;
        //                _new.ProcessStatus = _slot.PROCESS_STATUS;
        //                _new.PresenceJobKeys = _slot.PRESENCE_JOB_KEY;
        //                _new.ReserveJobKeys = _slot.RESERVE_JOB_KEY;
        //                _new.PresenceRoute = _slot.PRESENCE_ROUTE;
        //                _new.PresenceLastStepID = _slot.PRESENCE_LAST_STEP_ID;
        //                _new.PresenceRealStepID = _slot.PRESENCE_REAL_STEP_ID;
        //                Lst_SlotInfo.Add(_new);
        //            }
        //            else
        //            {
        //                _info.SlotStatus = _slot.RCS_SLOT_STATUS;
        //                _info.ProcessStatus = _slot.PROCESS_STATUS;
        //                _info.PresenceJobKeys = _slot.PRESENCE_JOB_KEY;
        //                _info.ReserveJobKeys = _slot.RESERVE_JOB_KEY;
        //                _info.PresenceRoute = _slot.PRESENCE_ROUTE;
        //                _info.PresenceLastStepID = _slot.PRESENCE_LAST_STEP_ID;
        //                _info.PresenceRealStepID = _slot.PRESENCE_REAL_STEP_ID;
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
