using System.Collections.Generic;
using IPD.Common;
using Newtonsoft.Json;

namespace IPD.Model
{
    public class ControlCycle
    {
        public int controlCycle { get; set; }
    }

    public class CoupleCoe
    {
        /// <summary>
        /// </summary>
        public double Couple_Coe_1_2 { get; set; }

        /// <summary>
        /// </summary>
        public double Couple_Coe_2_3 { get; set; }

        /// <summary>
        /// </summary>
        public double Couple_Coe_3_2 { get; set; }

        /// <summary>
        /// </summary>
        public double Couple_Coe_3_4 { get; set; }

        /// <summary>
        /// </summary>
        public double Couple_Coe_4_5 { get; set; }

        /// <summary>
        /// </summary>
        public double Couple_Coe_4_6 { get; set; }

        /// <summary>
        /// </summary>
        public double Couple_Coe_5_6 { get; set; }
    }

    public class DhParameterPalletRoot
    {
        /// <summary>
        /// </summary>
        public CoupleCoe CoupleCoe { get; set; }

        public PalletDynamicLimit dynamicLimit { get; set; }

        /// <summary>
        /// </summary>
        public List<LinkItem> Link { get; set; }

        /// <summary>
        /// </summary>
        public bool upsideDown { get; set; }
    }

    public class DhParameterRoot
    {
        /// <summary>
        /// </summary>
        public CoupleCoe CoupleCoe { get; set; }

        /// <summary>
        /// </summary>
        public List<LinkItem> Link { get; set; }

        /// <summary>
        /// </summary>
        public bool upsideDown { get; set; }
    }

    public class DhParameterScaraRoot
    {
        /// <summary>
        /// </summary>
        public CoupleCoe CoupleCoe { get; set; }

        /// <summary>
        /// </summary>
        public List<LinkItem> Link { get; set; }

        public int pitch { get; set; }

        /// <summary>
        /// </summary>
        public bool upsideDown { get; set; }
    }

    public class DhParameterSprayRoot
    {
        /// <summary>
        /// </summary>
        public CoupleCoe CoupleCoe { get; set; }

        /// <summary>
        /// </summary>
        public SprayLink Link { get; set; }

        /// <summary>
        /// </summary>
        public bool upsideDown { get; set; }
    }

    public class EniName
    {
        public string ENIName { get; set; }
        public bool isHaveENI { get; set; }
    }

    public class Joint
    {
        /// <summary>
        /// </summary>
        public int AxisDirection { get; set; }

        /// <summary>
        /// </summary>
        public int AxisNum { get; set; }

        /// <summary>
        /// </summary>
        public double BackLash { get; set; }

        /// <summary>
        /// </summary>
        public double DeRatedVel { get; set; }

        /// <summary>
        /// </summary>
        public double Direction { get; set; }

        /// <summary>
        /// </summary>
        public double EncoderResolution { get; set; }

        /// <summary>
        /// </summary>
        public double MaxAcc { get; set; }

        /// <summary>
        /// </summary>
        public double MaxDecel { get; set; }

        /// <summary>
        /// </summary>
        public double MaxDeRotSpeed { get; set; }

        /// <summary>
        /// </summary>
        public double MaxRotSpeed { get; set; }

        /// <summary>
        /// </summary>
        public double NegSWLimit { get; set; }

        /// <summary>
        /// </summary>
        public double PosSWLimit { get; set; }

        /// <summary>
        /// </summary>
        public double RatedDeRotSpeed { get; set; }

        /// <summary>
        /// </summary>
        public double RatedRotSpeed { get; set; }

        /// <summary>
        /// </summary>
        public double RatedVel { get; set; }

        /// <summary>
        /// </summary>
        public double ReducRatio { get; set; }
    }

    public class JointRoot
    {
        /// <summary>
        /// </summary>
        public Joint Joint { get; set; }
    }

    public class LinkItem
    {
        /// <summary>
        /// </summary>
        public double a { get; set; }

        /// <summary>
        /// </summary>
        public double alpha { get; set; }

        /// <summary>
        /// </summary>
        public double d { get; set; }

        /// <summary>
        /// </summary>
        public double theta { get; set; }
    }

    public class PalletDynamicLimit
    {
        public double max { get; set; }
        public double min { get; set; }
    }

    public class RobotItem
    {
        /// <summary>
        /// </summary>
        public string robotType { get; set; }

        /// <summary>
        /// </summary>
        public List<int> servoMap { get; set; }

        /// <summary>
        /// </summary>
        public int syncGroupSum { get; set; }

        /// <summary>
        /// </summary>
        public List<List<int>> syncMap { get; set; }

        /// <summary>
        /// </summary>
        public int syncSum { get; set; }

        /// <summary>
        /// </summary>
        public List<int> syncType { get; set; }
    }

    public class RobotParameter : ModuleBase
    {
        private static readonly object lockObj = new object();

        private static RobotParameter instance = null;

        private ControlCycle controlCycle;
        private int controlCycleSelectedIndex;

        private DhParameterRoot dhParameter;

        private DhParameterPalletRoot dhParameterPallet;

        private DhParameterScaraRoot dhParameterScara;

        private DhParameterSprayRoot dhParameterSpray;

        private EniName eniName;

        private List<JointRoot> jointList;

        private RobotCommon robotCommon = RobotCommon.GetInstance;

        private SlaveConfigRoot slaveConfig;
        private SlaveTypeList slaveTypeList;
        private List<SlaveSelectorItem> slaveSelectorList;

        public static RobotParameter GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new RobotParameter();
                        }
                    }
                }
                return instance;
            }
        }

        public ControlCycle ControlCycle { get => controlCycle; set => UpdateProperty(ref controlCycle, value); }
        public int ControlCycleSelectedIndex { get => controlCycleSelectedIndex; set => UpdateProperty(ref controlCycleSelectedIndex, value); }
        public DhParameterRoot DhParameter { get => dhParameter; set => UpdateProperty(ref dhParameter, value); }
        public DhParameterPalletRoot DhParameterPallet { get => dhParameterPallet; set => UpdateProperty(ref dhParameterPallet, value); }
        public DhParameterScaraRoot DhParameterScara { get => dhParameterScara; set => UpdateProperty(ref dhParameterScara, value); }
        public DhParameterSprayRoot DhParameterSpray { get => dhParameterSpray; set => UpdateProperty(ref dhParameterSpray, value); }
        public EniName EniName { get => eniName; set => UpdateProperty(ref eniName, value); }
        public List<JointRoot> JointList { get => jointList; set => UpdateProperty(ref jointList, value); }
        public SlaveConfigRoot SlaveConfig { get => slaveConfig; set => UpdateProperty(ref slaveConfig, value); }
        public SlaveTypeList SlaveTypeList { get => slaveTypeList; set => UpdateProperty(ref slaveTypeList, value); }
        public List<SlaveSelectorItem> SlaveSelectorList { get => slaveSelectorList; set => UpdateProperty(ref slaveSelectorList, value); }

        public void ControlCycleChange(string data)
        {
            ControlCycle v = JsonConvert.DeserializeObject<ControlCycle>(data);
            ControlCycle = v;
            switch (v.controlCycle)
            {
                case 1:
                    ControlCycleSelectedIndex = 0;
                    break;

                case 2:
                    ControlCycleSelectedIndex = 1;
                    break;

                case 4:
                    ControlCycleSelectedIndex = 2;
                    break;

                case 8:
                    ControlCycleSelectedIndex = 3;
                    break;

                default:
                    break;
            }
        }

        public void DhParameterChange(string data)
        {
            int num = robotCommon.RobotType.Num;
            if (num == 15)
            {
                DhParameterSprayRoot v = JsonConvert.DeserializeObject<DhParameterSprayRoot>(data);
                DhParameterSpray = v;
            }
            else if (num == 2 || num == 9 || num == 13)
            {
                DhParameterScaraRoot v = JsonConvert.DeserializeObject<DhParameterScaraRoot>(data);
                DhParameterScara = v;
            }
            else
            {
                DhParameterRoot v = JsonConvert.DeserializeObject<DhParameterRoot>(data);
                DhParameter = v;
            }
        }

        public void EniNameChange(string data)
        {
            EniName v = JsonConvert.DeserializeObject<EniName>(data);
            EniName = v;
        }

        public void JointParameterChange(string data)
        {
            JointRoot v = JsonConvert.DeserializeObject<JointRoot>(data);

            if (JointList == null)
            {
                List<JointRoot> list = new List<JointRoot> { v };
                JointList = list;
            }
            else if (JointList.Count < robotCommon.RobotType.AxisSum)
            {
                JointList.Add(v);
            }
            else
            {
                JointList[v.Joint.AxisNum - 1] = v;
            }
        }

        public void SlaveConfigChange(string data)
        {
            SlaveConfigRoot v = JsonConvert.DeserializeObject<SlaveConfigRoot>(data);
            SlaveConfig = v;
        }

        public void SlaveTypeListChange(string data)
        {
            SlaveTypeList v = JsonConvert.DeserializeObject<SlaveTypeList>(data);
            List<SlaveSelectorItem> l = new List<SlaveSelectorItem>();
            SlaveSelectorItem u = new SlaveSelectorItem();
            u.Num = 0;
            u.Type = "虚拟伺服";
            u.TypeEnglish = "Virtual Servo";
            l.Add(u);
            if (data == "{}" || v.slaveType == null)
            {
                v.slaveType = new List<string> { "虚拟伺服" };
                SlaveTypeList = v;
                SlaveSelectorList = l;
                return;
            }
            for (int i = 0; i < v.slaveType.Count; i++)
            {
                SlaveSelectorItem s = new SlaveSelectorItem();
                s.Type = v.slaveType[i];
                s.TypeEnglish = v.slaveTypeEnglish[i];
                s.Num = i + 1;
                l.Add(s);
            }
            SlaveSelectorList = l;
            return;
        }
    }

    public class SlaveConfigRoot
    {
        /// <summary>
        /// </summary>
        public List<RobotItem> robot { get; set; }

        /// <summary>
        /// </summary>
        public int servoSum { get; set; }

        /// <summary>
        /// </summary>
        public int sum { get; set; }
    }

    public class SlaveSelectorItem
    {
        public string Type { get; set; }
        public string TypeEnglish { get; set; }
        public int Num { get; set; }
    }

    public class SlaveTypeList
    {
        public List<int> IONum { get; set; }
        public List<int> servoNum { get; set; }
        public List<string> slaveType { get; set; }
        public List<string> slaveTypeEnglish { get; set; }
    }

    public class SprayLink
    {
        /// <summary>
        /// </summary>
        public double L1 { get; set; }

        /// <summary>
        /// </summary>
        public double L2 { get; set; }

        /// <summary>
        /// </summary>
        public double L3 { get; set; }

        /// <summary>
        /// </summary>
        public double L4 { get; set; }

        /// <summary>
        /// </summary>
        public double L5 { get; set; }

        /// <summary>
        /// </summary>
        public double L6 { get; set; }

        /// <summary>
        /// </summary>
        public double L7 { get; set; }

        /// <summary>
        /// </summary>
        public int L8 { get; set; }
    }
}