using System.Collections.Generic;
using System.Xml;
using IPD.Common;
using IPD.Tcp;
using Newtonsoft.Json;

namespace IPD.Model
{
    public class CurrentRobotObject
    {
        public int mode { get; set; }
        public int robot { get; set; }
    }

    public class RobotSumObject
    {
        public int sum { get; set; }
    }

    public class RobotTypeObject
    {
        public int type { get; set; }
    }

    public class RobotTypes
    {
        public int AxisSum
        {
            get; set;
        }

        public string En { get; set; }
        public string Name { get; set; }
        public int Num { get; set; }
    }

    public class ToolNum
    {
        public int robot { get; set; }
        public int curToolNum { get; set; }
    }

    public class PosRoot
    {
        /// <summary>
        /// </summary>
        public int configuration { get; set; }

        /// <summary>
        /// </summary>
        public int coord { get; set; }

        /// <summary>
        /// </summary>
        public int deg { get; set; }

        /// <summary>
        /// </summary>
        public List<double> pos { get; set; }

        /// <summary>
        /// </summary>
        public List<double> posDeg { get; set; }

        /// <summary>
        /// </summary>
        public int robot { get; set; }
    }

    public class RobotCommon : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static RobotCommon instance = null;

        private int currentRobot = 1;
        private int multiRobotMode;
        private int robotSum = 1;
        private RobotTypes robotType = new RobotTypes();

        //private int coord;
        //
        //private bool deadman;
        //private int mode;
        //private double[] motorTorque;
        //private double[] motorVel;
        private PosRoot pos;

        //private int servoStatus;
        //private double speed;
        //private int teachType;

        //private int user;
        public PosRoot Pos { get => pos; set => UpdateProperty(ref pos, value); }

        public void ChangePos(string data)
        {
            PosRoot v = JsonConvert.DeserializeObject<PosRoot>(data);
            Pos = v;
        }

        public static RobotCommon GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new RobotCommon();
                        }
                    }
                }
                return instance;
            }
        }

        public int CurrentRobot
        {
            get => currentRobot; set
            {
                UpdateProperty(ref currentRobot, value);
                Client client = Client.GetInstance();
                client.SendMessage(0x2e02, "{}");
                client.SendMessage(0x2e15, "{}");
            }
        }

        private ToolNum curToolNum;
        public ToolNum CurToolNum { get => curToolNum; set => UpdateProperty(ref curToolNum, value); }
        public int MultiRobotMode { get => multiRobotMode; set => UpdateProperty(ref multiRobotMode, value); }
        public int RobotSum { get => robotSum; set => UpdateProperty(ref robotSum, value); }
        public RobotTypes RobotType { get => robotType; set => UpdateProperty(ref robotType, value); }

        public void CurrentRobotChange(string data)
        {
            CurrentRobotObject cr = JsonConvert.DeserializeObject<CurrentRobotObject>(data);
            CurrentRobot = cr.robot;
            MultiRobotMode = cr.mode;
        }

        public void ChangeToolNum(string data)
        {
            ToolNum v = JsonConvert.DeserializeObject<ToolNum>(data);
            CurToolNum = v;
        }

        public void RobotSumChange(string data)
        {
            RobotSumObject rs = JsonConvert.DeserializeObject<RobotSumObject>(data);
            RobotSum = rs.sum;
        }

        public void RobotTypeChange(string data)
        {
            RobotTypeObject rt = JsonConvert.DeserializeObject<RobotTypeObject>(data);
            int typeNum = rt.type;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Resource.RobotType);
            XmlNode rootNode = xml.SelectSingleNode("a");
            XmlNodeList rootChildList = rootNode.ChildNodes;
            RobotTypes robotTypes = new RobotTypes();
            for (int i = 0; i < rootChildList.Count; i++)
            {
                XmlNode cnode = rootChildList.Item(i);
                XmlNodeList cnodeChild = cnode.ChildNodes;
                if (int.Parse(cnodeChild.Item(3).InnerText) == typeNum)
                {
                    robotTypes.AxisSum = int.Parse(cnodeChild.Item(0).InnerText);
                    robotTypes.En = cnodeChild.Item(2).InnerText;
                    robotTypes.Name = cnodeChild.Item(1).InnerText;
                    robotTypes.Num = int.Parse(cnodeChild.Item(3).InnerText);
                    RobotType = robotTypes;
                    break;
                }
            }
        }
    }
}